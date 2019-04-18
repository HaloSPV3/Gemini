/**
 * Copyright (c) 2019 Emilian Roman
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SPV3.CLI.Exceptions;
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;
using static SPV3.CLI.Console;
using static SPV3.CLI.Names;

namespace SPV3.CLI
{
  /// <summary>
  ///   Class used for bootstrapping the SPV3 loading procedure.
  /// </summary>
  public static class Kernel
  {
    /**
     * Inic.txt can be located across multiple locations on the filesystem; however, SPV3 only deals with the one in
     * the working directory -- hence the name!
     */
    private static readonly Initiation RootInitc = (Initiation) Path.Combine(CurrentDirectory, Files.Initiation);

    /// <summary>
    ///   Invokes the SPV3 loading procedure.
    /// </summary>
    public static void Bootstrap()
    {
      var configuration = (Configuration) Files.Kernel;

      if (!configuration.Exists())
        configuration.Save(); /* gracefully create new configuration */

      configuration.Load();

      if (!configuration.SkipHeuristicInstall)
        HeuristicInstall();
      else
        Info("Skipping Kernel.HeuristicInstall");

      if (!configuration.SkipVerifyMainAssets)
        VerifyMainAssets();
      else
        Info("Skipping Kernel.VerifyMainAssets");

      if (!configuration.SkipInvokeCoreTweaks)
        InvokeCoreTweaks();
      else
        Info("Skipping Kernel.InvokeCoreTweaks");

      if (!configuration.SkipResumeCheckpoint)
        ResumeCheckpoint();
      else
        Info("Skipping Kernel.ResumeCheckpoint");

      if (!configuration.SkipSetShadersConfig)
        SetShadersConfig();

      if (!configuration.SkipInvokeExecutable)
        InvokeExecutable();
      else
        Info("Skipping Kernel.InvokeExecutable");
    }

    /// <summary>
    ///   Heuristically conducts pre-loading installation, if necessary.
    /// </summary>
    private static void HeuristicInstall()
    {
      /**
       * If the HCE executable does not exist in the working directory, but the manifest and an initial package exists,
       * then we can conclude that this is an installation scenario. We can bootstrap the installer to install SPV3 to
       * the default path.
       */

      if (Exists("haloce.exe") || !Exists("0x00.bin") || !Exists("0x01.bin")) return;
      Info("Found manifest & package, but not the HCE executable. Assuming installation environment.");

      var destination = Path.Combine(GetFolderPath(Personal), Directories.Games, "Halo SPV3");
      Installer.Install(CurrentDirectory, destination);

      var cli = new ProcessStartInfo
      {
        FileName         = Path.Combine(destination, "SPV3.CLI.exe"),
        WorkingDirectory = destination
      };

      Process.Start(cli);
    }

    /// <summary>
    ///   Invokes the SPV3.2 data verification routines.
    /// </summary>
    private static void VerifyMainAssets()
    {
      /**
       * It is preferable to whitelist the type of files we would like to verify. The focus would be to skip any files
       * which are expected to be changed.
       *
       * For example, if SPV3.2 were to be distributed with a configuration file, then changing its contents would
       * result in an asset verification error. Additionally, changing the CLI executable by updating it could result in
       * the same error.
       */

      var whitelist = new List<string>
      {
        ".map",      /* map resources */
        "haloce.exe" /* game executable */
      };

      /**
       * Through the use of the manifest that was copied to the installation directory, the loader can infer the list of
       * SPV3 files on the filesystem that it must verify. Verification is done through a simple size comparison between
       * the size of the file on the filesystem and the one declared in the manifest.
       *
       * This routine relies on combining...
       *
       * - the path of the working directory; with
       * - the package's declared relative path; with
       * - the filename of the respective file
       *
       * ... to determine the absolute path of the file on the filesystem:
       * 
       * X:\Installations\SPV3\gallery\Content\editbox1024.PNG
       * |----------+---------|-------+-------|-------+-------|
       *            |                 |               |
       *            |                 |               + - File on the filesystem
       *            |                 + ----------------- Package relative path
       *            + ----------------------------------- Working directory
       */

      var manifest = (Manifest) Path.Combine(CurrentDirectory, Files.Manifest);

      /**
       * This shouldn't be an issue in conventional SPV3 installations; however, for existing/current SPV3 installations
       * OR installations that weren't conducted by the installer, the manifest will likely not be present. As such, we
       * have no choice but to skip the verification mechanism.
       */
      if (!manifest.Exists()) return;

      manifest.Load();

      Info("Found manifest file - proceeding with data verification ...");

      foreach (var package in manifest.Packages)
      foreach (var entry in package.Entries)
      {
        if (!whitelist.Any(entry.Name.Contains)) /* skip verification if current file isn't in the whitelist */
          continue;

        var absolutePath = Path.Combine(CurrentDirectory, package.Path, entry.Name);
        var expectedSize = entry.Size;
        var actualSize   = new FileInfo(absolutePath).Length;

        if (expectedSize == actualSize) continue;

        Info($"Size mismatch {entry.Name} (expect: {expectedSize}, actual: {actualSize}).");
        throw new AssetException($"Size mismatch {entry.Name} (expect: {expectedSize}, actual: {actualSize}).");
      }
    }

    /// <summary>
    ///   Invokes core improvements to the auto-detected profile, such as auto max resolution and gamma fixes. This is
    ///   NOT done when a profile does not exist/cannot be found!
    /// </summary>
    private static void InvokeCoreTweaks()
    {
      var lastprof = (LastProfile) Files.LastProfile;

      if (!lastprof.Exists()) return;

      lastprof.Load();

      Info("Found lastprof file - proceeding with profile detection ...");

      var profblam = (Profile) Path.Combine(Directories.Profiles, lastprof.Profile, Files.Profile);

      if (!profblam.Exists()) return;

      profblam.Load();

      Info("Found blam.sav file - proceeding with core patches ...");

      profblam.Video.Resolution.Width  = (ushort) Screen.PrimaryScreen.Bounds.Width;
      profblam.Video.Resolution.Height = (ushort) Screen.PrimaryScreen.Bounds.Height;
      profblam.Video.FrameRate         = Profile.ProfileVideo.VideoFrameRate.VsyncOff; /* ensure no FPS locking */
      profblam.Video.Particles         = Profile.ProfileVideo.VideoParticles.High;
      profblam.Video.Quality           = Profile.ProfileVideo.VideoQuality.High;

      profblam.Save();

      Info("Patched video resolution width  - " + profblam.Video.Resolution.Width);
      Info("Patched video resolution height - " + profblam.Video.Resolution.Height);
      Info("Patched video frame rate        - " + profblam.Video.FrameRate);
      Info("Patched video quality           - " + profblam.Video.Particles);
      Info("Patched video texture           - " + profblam.Video.Quality);
    }

    /// <summary>
    ///   Invokes the profile & campaign auto-detection mechanism.
    /// </summary>
    private static void ResumeCheckpoint()
    {
      var lastprof = (LastProfile) Files.LastProfile;

      if (!lastprof.Exists()) return;

      lastprof.Load();

      Info("Found lastprof file - proceeding with checkpoint detection ...");

      var playerDat = (Progress) Path.Combine(
        Directories.Profiles,
        lastprof.Profile,
        Files.Progress);

      if (!playerDat.Exists()) return;

      Info("Found checkpoint file - proceeding with resuming campaign ...");

      playerDat.Load();

      try
      {
        RootInitc.Mission    = playerDat.Mission;
        RootInitc.Difficulty = playerDat.Difficulty;
        RootInitc.Save();

        Info("Resumed campaign MISSION    - " + playerDat.Mission);
        Info("Resumed campaign DIFFICULTY - " + playerDat.Difficulty);
      }
      catch (UnauthorizedAccessException e)
      {
        Error(e.Message + " -- CAMPAIGN WILL NOT RESUME!");
      }
    }

    /// <summary>
    ///   Applies the post-processing settings.
    /// </summary>
    private static void SetShadersConfig()
    {
      try
      {
        var pp = (PostProcessing) Files.PostProcessing;

        if (!pp.Exists())
          pp.Save();

        pp.Load();

        RootInitc.PostProcessing = pp;
        RootInitc.Save();

        Info("Applied PP settings for MXAO        - " + RootInitc.PostProcessing.Mxao);
        Info("Applied PP settings for DOF         - " + RootInitc.PostProcessing.Dof);
        Info("Applied PP settings for Motion Blur - " + RootInitc.PostProcessing.MotionBlur);
        Info("Applied PP settings for Lens Flares - " + RootInitc.PostProcessing.DynamicLensFlares);
        Info("Applied PP settings for Volumetrics - " + RootInitc.PostProcessing.Volumetrics);
        Info("Applied PP settings for Lens Dirt   - " + RootInitc.PostProcessing.LensDirt);
      }
      catch (UnauthorizedAccessException e)
      {
        Error(e.Message + " -- POST PROCESSING WILL NOT BE APPLIED!");
      }
    }

    /// <summary>
    ///   Invokes the HCE executable.
    /// </summary>
    private static void InvokeExecutable()
    {
      /**
       * Gets the path of the HCE executable on the filesystem, which conventionally should be the working directory of
       * the loader, given that the loader is bundled with the rest of the SPV3.2 data.
       */

      string GetPath()
      {
        return Path.Combine(CurrentDirectory, Files.Executable);
      }

      var executable = (Executable) GetPath();

      if (!executable.Exists()) throw new FileNotFoundException("Could not find HCE executable.");

      Info("Found HCE executable in the working directory - proceeding to execute it ...");

      executable.Debug.Console    = true;
      executable.Debug.Developer  = true;
      executable.Debug.Screenshot = true;

      Info("Debug.Console    = true");
      Info("Debug.Developer  = true");
      Info("Debug.Screenshot = true");

      Info("Using the aforementioned start-up parameters when initiating HCE process.");

      executable.Start();
      Info("And... we're done!");
    }

    /// <inheritdoc />
    /// <summary>
    ///   File-driven kernel configuration object.
    /// </summary>
    public class Configuration : File
    {
      /// <summary>
      ///   Binary file length.
      /// </summary>
      private const int Length = 0x100;

      public bool SkipHeuristicInstall { get; set; }
      public bool SkipVerifyMainAssets { get; set; }
      public bool SkipInvokeCoreTweaks { get; set; }
      public bool SkipResumeCheckpoint { get; set; }
      public bool SkipSetShadersConfig { get; set; }
      public bool SkipInvokeExecutable { get; set; }

      /// <summary>
      ///   Loads object state from the inbound file.
      /// </summary>
      public void Load()
      {
        using (var fs = new FileStream(Path, FileMode.Open))
        using (var ms = new MemoryStream(0x10))
        using (var br = new BinaryReader(ms))
        {
          fs.CopyTo(ms);
          br.BaseStream.Seek(0x00, SeekOrigin.Begin);

          SkipHeuristicInstall = br.ReadBoolean(); /* 0x00 */
          SkipVerifyMainAssets = br.ReadBoolean(); /* 0x01 */
          SkipInvokeCoreTweaks = br.ReadBoolean(); /* 0x02 */
          SkipResumeCheckpoint = br.ReadBoolean(); /* 0x03 */
          SkipSetShadersConfig = br.ReadBoolean(); /* 0x04 */
          SkipInvokeExecutable = br.ReadBoolean(); /* 0x05 */
        }
      }

      /// <summary>
      ///   Saves object state to the inbound file.
      /// </summary>
      public void Save()
      {
        using (var fs = new FileStream(Path, FileMode.Create))
        using (var ms = new MemoryStream(16))
        using (var bw = new BinaryWriter(ms))
        {
          bw.BaseStream.Seek(0x00, SeekOrigin.Begin);

          bw.Write(SkipHeuristicInstall);                      /* 0x00 */
          bw.Write(SkipVerifyMainAssets);                      /* 0x01 */
          bw.Write(SkipInvokeCoreTweaks);                      /* 0x02 */
          bw.Write(SkipResumeCheckpoint);                      /* 0x03 */
          bw.Write(SkipSetShadersConfig);                      /* 0x04 */
          bw.Write(SkipInvokeExecutable);                      /* 0x05 */
          bw.Write(new byte[Length - bw.BaseStream.Position]); /* pad  */

          ms.WriteTo(fs);
        }
      }

      /// <summary>
      ///   Represents the inbound object as a string.
      /// </summary>
      /// <param name="configuration">
      ///   Object to represent as string.
      /// </param>
      /// <returns>
      ///   String representation of the inbound object.
      /// </returns>
      public static implicit operator string(Configuration configuration)
      {
        return configuration.Path;
      }

      /// <summary>
      ///   Represents the inbound string as an object.
      /// </summary>
      /// <param name="path">
      ///   String to represent as object.
      /// </param>
      /// <returns>
      ///   Object representation of the inbound string.
      /// </returns>
      public static explicit operator Configuration(string path)
      {
        return new Configuration
        {
          Path = path
        };
      }
    }
  }
}