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

using System.IO;
using System.Text;
using HXE.SPV3;
using static System.IO.FileAccess;
using static System.IO.FileMode;
using static HXE.SPV3.PostProcessing;
using static HXE.SPV3.PostProcessing.ExperimentalPostProcessing;

namespace HXE
{
  /// <summary>
  ///   Represents the loader configuration.
  /// </summary>
  public class Configuration : File
  {
    private const int Length = 256;

    public KernelConfiguration Kernel         { get; set; } = new KernelConfiguration();
    public PostProcessing      PostProcessing { get; set; } = new PostProcessing();

    public void Save()
    {
      using (var fs = new FileStream(Paths.Configuration, OpenOrCreate, Write))
      using (var ms = new MemoryStream(Length))
      using (var bw = new BinaryWriter(ms))
      {
        ms.Position = 0;

        /* signature */
        {
          bw.Write(Encoding.Unicode.GetBytes("~yumiris"));
        }

        /* padding */
        {
          bw.Write(new byte[16 - ms.Position]);
        }

        /* kernel */
        {
          bw.Write(Kernel.SkipVerifyMainAssets);
          bw.Write(Kernel.SkipInvokeCoreTweaks);
          bw.Write(Kernel.SkipResumeCheckpoint);
          bw.Write(Kernel.SkipSetInitcContents);
          bw.Write(Kernel.SkipInvokeExecutable);
          bw.Write(Kernel.SkipPatchLargeAAware);
          bw.Write(Kernel.EnableSpv3KernelMode);
          bw.Write(Kernel.EnableSpv3LegacyMode);
          bw.Write(Kernel.SkipEnableCinematics);
          bw.Write(Kernel.SkipRastMotionSensor);
        }

        /* padding */
        {
          bw.Write(new byte[64 - ms.Position]);
        }

        /* post-processing */
        {
          bw.Write(PostProcessing.Internal);
          bw.Write(PostProcessing.External);
          bw.Write(PostProcessing.GBuffer);
          bw.Write(PostProcessing.DepthFade);
          bw.Write(PostProcessing.Bloom);
          bw.Write(PostProcessing.LensDirt);
          bw.Write(PostProcessing.DynamicLensFlares);
          bw.Write(PostProcessing.Volumetrics);
          bw.Write(PostProcessing.AntiAliasing);
          bw.Write(PostProcessing.HudVisor);
          bw.Write(PostProcessing.FilmGrain);

          bw.Write((byte) PostProcessing.MotionBlur);
          bw.Write((byte) PostProcessing.Mxao);
          bw.Write((byte) PostProcessing.Dof);
          bw.Write((byte) PostProcessing.Experimental.ThreeDimensional);
          bw.Write((byte) PostProcessing.Experimental.ColorBlindMode);
        }

        /* padding */
        {
          bw.Write(new byte[Length - ms.Position]);
        }

        ms.Position = 0;
        ms.CopyTo(fs);
      }
    }

    public void Load()
    {
      using (var fs = new FileStream(Paths.Configuration, Open, Read))
      using (var ms = new MemoryStream(Length))
      using (var br = new BinaryReader(ms))
      {
        fs.CopyTo(ms);
        ms.Position = 0;

        /* padding */
        {
          ms.Position += 16 - ms.Position;
        }

        /* kernel */
        {
          Kernel.SkipVerifyMainAssets = br.ReadBoolean();
          Kernel.SkipInvokeCoreTweaks = br.ReadBoolean();
          Kernel.SkipResumeCheckpoint = br.ReadBoolean();
          Kernel.SkipSetInitcContents = br.ReadBoolean();
          Kernel.SkipInvokeExecutable = br.ReadBoolean();
          Kernel.SkipPatchLargeAAware = br.ReadBoolean();
          Kernel.EnableSpv3KernelMode = br.ReadBoolean();
          Kernel.EnableSpv3LegacyMode = br.ReadBoolean();
          Kernel.SkipEnableCinematics = br.ReadBoolean();
          Kernel.SkipRastMotionSensor = br.ReadBoolean();
        }

        /* padding */
        {
          ms.Position += 64 - ms.Position;
        }

        /* post-processing */
        {
          PostProcessing.Internal          = br.ReadBoolean();
          PostProcessing.External          = br.ReadBoolean();
          PostProcessing.GBuffer           = br.ReadBoolean();
          PostProcessing.DepthFade         = br.ReadBoolean();
          PostProcessing.Bloom             = br.ReadBoolean();
          PostProcessing.LensDirt          = br.ReadBoolean();
          PostProcessing.DynamicLensFlares = br.ReadBoolean();
          PostProcessing.Volumetrics       = br.ReadBoolean();
          PostProcessing.AntiAliasing      = br.ReadBoolean();
          PostProcessing.HudVisor          = br.ReadBoolean();
          PostProcessing.FilmGrain         = br.ReadBoolean();

          PostProcessing.MotionBlur                    = (MotionBlurOptions) br.ReadByte();
          PostProcessing.Mxao                          = (MxaoOptions) br.ReadByte();
          PostProcessing.Dof                           = (DofOptions) br.ReadByte();
          PostProcessing.Experimental.ThreeDimensional = (ThreeDimensionalOptions) br.ReadByte();
          PostProcessing.Experimental.ColorBlindMode   = (ColorBlindModeOptions) br.ReadByte();
        }
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

    /// <summary>
    ///   File-driven kernel configuration object.
    /// </summary>
    public class KernelConfiguration
    {
      public bool SkipVerifyMainAssets { get; set; }
      public bool SkipInvokeCoreTweaks { get; set; }
      public bool SkipResumeCheckpoint { get; set; }
      public bool SkipSetInitcContents { get; set; }
      public bool SkipInvokeExecutable { get; set; }
      public bool SkipPatchLargeAAware { get; set; }
      public bool EnableSpv3KernelMode { get; set; }
      public bool EnableSpv3LegacyMode { get; set; }
      public bool SkipEnableCinematics { get; set; }
      public bool SkipRastMotionSensor { get; set; }
    }
  }
}