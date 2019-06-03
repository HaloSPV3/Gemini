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
using System.IO;
using System.Linq;
using HXE.Exceptions;
using HXE.HCE;
using HXE.SPV3;
using static System.Environment;
using static System.IO.Path;
using static System.Windows.Forms.Screen;
using static HXE.Console;
using static HXE.HCE.Profile.ProfileAudio;
using static HXE.Paths;
using static HXE.HCE.Profile.ProfileVideo;

namespace HXE
{
  /// <summary>
  ///   Class used for bootstrapping the HCE loading procedure.
  /// </summary>
  public static class Kernel
  {
    /**
     * Inic.txt can be located across multiple locations on the filesystem; however, HCE only deals with the one in
     * the working directory -- hence the name!
     */
    private static readonly Initiation RootInitc = (Initiation) Combine(CurrentDirectory, Files.Initiation);

    /// <summary>
    ///   Invokes the HCE loading procedure.
    /// </summary>
    public static void Bootstrap(Executable executable)
    {
      var configuration = (Configuration) Files.Configuration;

      if (!configuration.Exists())
        configuration.Save(); /* gracefully create new configuration */

      configuration.Load();

      if (!configuration.Kernel.SkipVerifyMainAssets)
        VerifyMainAssets();
      else
        Info("Skipped Kernel.VerifyMainAssets");

      if (configuration.Kernel.EnableSpv3KernelMode)
      {
        if (!configuration.Kernel.SkipInvokeCoreTweaks)
          InvokeCoreTweaks(executable);
        else
          Info("Skipped Kernel.InvokeCoreTweaks");

        if (!configuration.Kernel.SkipResumeCheckpoint)
          ResumeCheckpoint(executable);
        else
          Info("Skipped Kernel.ResumeCheckpoint");

        /**
         * The main difference between SPV3.1 and SPV3.2 is what the f1 variable in the initiation file is used for. In
         * 3.1, it's used for unlocking the maps. In SPV3.2, it's used for encoding the post-processing settings.
         */

        if (configuration.Kernel.EnableSpv3LegacyMode)
        {
          SetSpv31InitMode(configuration);
          Info("Enabled SPV3.1 initiation mode");
        }
        else
        {
          if (!configuration.Kernel.SkipSetShadersConfig)
            SetShadersConfig(configuration);
          else
            Info("Skipped Kernel.SkipSetShadersConfig");
        }
      }
      else
      {
        Info("Skipped Kernel.EnableSpv3KernelMode");
      }

      if (!configuration.Kernel.SkipPatchLargeAAware)
        PatchLargeAAware(executable);
      else
        Info("Skipped Kernel.SkipPatchLargeAAware");

      if (!configuration.Kernel.SkipInvokeExecutable)
        InvokeExecutable(executable);
      else
        Info("Skipped Kernel.InvokeExecutable");
    }

    /// <summary>
    ///   Invokes the HCE data verification routines.
    /// </summary>
    private static void VerifyMainAssets()
    {
      /**
       * It is preferable to blacklist the type of files we would like to skip. The focus would be to skip any files
       * which are expected to be changed.
       *
       * For example, if HCE were to be distributed with a configuration file, then changing its contents would
       * result in an asset verification error. Additionally, changing HXE executable by updating it could result in
       * the same error.
       */

      var blacklist = new List<string>
      {
        ".exe",
        ".map",
        ".txt",
        ".cfg",
        ".ini",
        ".xml",
        ".m"
      };

      /**
       * Through the use of the manifest that was copied to the installation directory, the loader can infer the list of
       * HCE files on the filesystem that it must verify. Verification is done through a simple size comparison between
       * the size of the file on the filesystem and the one declared in the manifest.
       *
       * This routine relies on combining...
       *
       * - the path of the working directory; with
       * - the entry' declared relative path; with
       * - the actual name of the respective file
       *
       * ... to determine the absolute path of the file on the filesystem:
       * 
       * X:\Installations\HCE\gallery\Content\editbox1024.PNG
       * |----------+---------|-------+-------|-------+-------|
       *            |                 |               |
       *            |                 |               + - File on the filesystem
       *            |                 + ----------------- Package relative path
       *            + ----------------------------------- Working directory
       */

      var manifest = (Manifest) Combine(CurrentDirectory, Files.Manifest);

      /**
       * This shouldn't be an issue in conventional HCE installations; however, for existing/current SPV3 installations
       * OR installations that weren't conducted by the installer, the manifest will likely not be present. As such, we
       * have no choice but to skip the verification mechanism.
       */

      if (!manifest.Exists())
      {
        Info("Could not find manifest binary - skipping asset verification");
        return;
      }

      manifest.Load();

      Info("Deserialised manifest binary to its corresponding object");

      foreach (var package in manifest.Packages)
      {
        var file = Combine(CurrentDirectory, package.Entry.Path, package.Entry.Name);
        var size = package.Entry.Size;

        if (!System.IO.File.Exists(file))
          throw new FileNotFoundException("File does not currently exist - " + package.Entry.Name);

        Info("Inferred file on the filesystem - " + package.Entry.Name);

        var lowercaseName = package.Entry.Name.ToLower();

        if (blacklist.Any(lowercaseName.Contains)) /* skip verification if current file isn't in the whitelist */
          continue;

        Info("File is not whitelisted - " + package.Entry.Name);

        if (size != new FileInfo(file).Length)
          throw new AssetException("Asset size mismatch - " + package.Entry.Name);

        Info("File matches its manifest metadata - " + package.Entry.Name);
      }

      Done("Asset verification routine has been successfully completed");
    }

    /// <summary>
    ///   Invokes core improvements to the auto-detected profile, such as auto max resolution and gamma fixes. This is
    ///   NOT done when a profile does not exist/cannot be found!
    /// </summary>
    /// <param name="executable"></param>
    private static void InvokeCoreTweaks(Executable executable)
    {
      try
      {
        if (executable.Video.Width == 0 && executable.Video.Height == 0)
        {
          executable.Video.Width  = (ushort) PrimaryScreen.Bounds.Width;
          executable.Video.Height = (ushort) PrimaryScreen.Bounds.Height;

          Info("Dimensions not specified - applied native resolution");
          Info("Executable -vidmode width  - " + executable.Video.Width);
          Info("Executable -vidmode height - " + executable.Video.Height);
        }

        var lastprof = (LastProfile) Combine(executable.Profile.Path, Files.LastProfile);

        if (!lastprof.Exists())
          return;

        lastprof.Load();

        Info("Deserialised found lastprof file");

        var profile = (Profile) Combine(
          executable.Profile.Path,
          Directories.Profiles,
          lastprof.Profile,
          Files.Profile
        );

        if (!profile.Exists())
          return;

        profile.Load();

        Info("Deserialised inferred HCE profile");

        foreach (var availableProfile in Profile.List(executable.Profile.Path))
          Debug(availableProfile.Details.Name.Equals(profile.Details.Name)
            ? $"Available profile: {availableProfile.Details.Name} << SELECTED PROFILE"
            : $"Available profile: {availableProfile.Details.Name}");

        profile.Video.Resolution.Width  = executable.Video.Width;
        profile.Video.Resolution.Height = executable.Video.Height;
        profile.Video.FrameRate         = VideoFrameRate.VsyncOff; /* ensure no FPS locking */
        profile.Video.Particles         = VideoParticles.High;
        profile.Video.Quality           = VideoQuality.High;
        profile.Video.Effects.Specular  = true;
        profile.Video.Effects.Shadows   = true;
        profile.Video.Effects.Decals    = true;

        Info("Applied profile video patches");

        profile.Audio.Quality = AudioQuality.High;
        profile.Audio.Variety = AudioVariety.High;

        Info("Applied profile audio patches");

        profile.Save();
        profile.Load();

        Info("Saved profile data to the filesystem");

        Debug("Patched video resolution width  - " + profile.Video.Resolution.Width);
        Debug("Patched video resolution height - " + profile.Video.Resolution.Height);
        Debug("Patched video frame rate        - " + profile.Video.FrameRate);
        Debug("Patched video quality           - " + profile.Video.Particles);
        Debug("Patched video texture           - " + profile.Video.Quality);
        Debug("Patched video effect - specular - " + profile.Video.Effects.Specular);
        Debug("Patched video effect - shadows  - " + profile.Video.Effects.Shadows);
        Debug("Patched video effect - decals   - " + profile.Video.Effects.Decals);
        Debug("Patched audio quality           - " + profile.Audio.Quality);
        Debug("Patched audio variety           - " + profile.Audio.Variety);
      }
      catch (Exception e)
      {
        Error(e.Message + " -- CORE TWEAKS WILL NOT BE APPLIED");
      }
    }

    /// <summary>
    ///   Invokes the profile & campaign auto-detection mechanism.
    /// </summary>
    private static void ResumeCheckpoint(Executable executable)
    {
      try
      {
        var lastprof = (LastProfile) Combine(executable.Profile.Path, Files.LastProfile);

        if (!lastprof.Exists())
          return;

        lastprof.Load();

        Info("Deserialised found lastprof file");

        var playerDat = (Progress) Combine(
          executable.Profile.Path,
          Directories.Profiles,
          lastprof.Profile,
          Files.Progress
        );

        if (!playerDat.Exists())
          return;

        playerDat.Load();

        Info("Deserialised inferred progress binary");

        RootInitc.Mission    = playerDat.Mission;
        RootInitc.Difficulty = playerDat.Difficulty;

        Info("Updated the initiation file with campaign progress");

        RootInitc.Save();

        Info("Saved campaign progress to the initiation file");

        Debug("Resumed campaign mission    - " + playerDat.Mission);
        Debug("Resumed campaign difficulty - " + playerDat.Difficulty);
      }
      catch (UnauthorizedAccessException e)
      {
        Error(e.Message + " -- CAMPAIGN WILL NOT RESUME");
      }
    }

    /// <summary>
    ///   Sets the initc.txt to SPV3.1 mode, i.e. unlocking maps instead of encoding post-processing settings.
    /// </summary>
    private static void SetSpv31InitMode(Configuration configuration)
    {
      try
      {
        RootInitc.Unlock = configuration.Kernel.EnableSpv3LegacyMode;

        Info("Updated the initiation file with SPV3.1 maps unlocking code");

        RootInitc.Save();

        Info("Saved unlocking code to the initiation file");
      }
      catch (UnauthorizedAccessException e)
      {
        Error(e.Message + " -- PLEASE RUN AS ADMINISTRATOR");
      }
    }

    /// <summary>
    ///   Applies the post-processing settings.
    /// </summary>
    private static void SetShadersConfig(Configuration configuration)
    {
      try
      {
        RootInitc.PostProcessing = configuration.PostProcessing;

        Info("Updated the initiation file with the post-processing settings");

        RootInitc.Save();

        Info("Saved post-processing settings to the initiation file");

        Debug("Applied PP settings for MXAO        - " + RootInitc.PostProcessing.Mxao);
        Debug("Applied PP settings for DOF         - " + RootInitc.PostProcessing.Dof);
        Debug("Applied PP settings for Motion Blur - " + RootInitc.PostProcessing.MotionBlur);
        Debug("Applied PP settings for Lens Flares - " + RootInitc.PostProcessing.DynamicLensFlares);
        Debug("Applied PP settings for Volumetrics - " + RootInitc.PostProcessing.Volumetrics);
        Debug("Applied PP settings for Lens Dirt   - " + RootInitc.PostProcessing.LensDirt);
      }
      catch (UnauthorizedAccessException e)
      {
        Error(e.Message + " -- POST PROCESSING WILL NOT BE APPLIED");
      }
    }

    /// <summary>
    ///   Patches HCE executable for Large Address Aware.
    /// </summary>
    private static void PatchLargeAAware(Executable executable)
    {
      const byte value  = 0x2F;  /* LAA flag   */
      const long offset = 0x136; /* LAA offset */

      try
      {
        using (var fs = new FileStream(executable.Path, FileMode.Open, FileAccess.ReadWrite))
        using (var ms = new MemoryStream(0x24B000))
        using (var bw = new BinaryWriter(ms))
        using (var br = new BinaryReader(ms))
        {
          ms.Position = 0;
          fs.Position = 0;
          fs.CopyTo(ms);

          ms.Position = offset;

          if (br.ReadByte() != value)
          {
            ms.Position -= 1; /* restore position */
            bw.Write(value);  /* patch LAA flag   */

            fs.Position = 0;
            ms.Position = 0;
            ms.CopyTo(fs);

            Info("Applied LAA patch to the HCE executable");
          }
          else
          {
            Info("HCE executable already patched with LAA");
          }
        }
      }
      catch (Exception e)
      {
        Error(e.Message + " -- LAA PATCH WILL NOT BE APPLIED");
      }
    }

    /// <summary>
    ///   Invokes the HCE executable.
    /// </summary>
    private static void InvokeExecutable(Executable executable)
    {
      executable.Start();
    }
  }
}