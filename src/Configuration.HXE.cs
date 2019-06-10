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
using HXE.SPV3;

namespace SPV3
{
  public partial class Configuration
  {
    public partial class ConfigurationHXE
    {
      private HXE.Configuration       Configuration { get; }      = (HXE.Configuration) global::HXE.Paths.Configuration;
      public  ConfigurationHXEKernel  Kernel        { get; set; } = new ConfigurationHXEKernel();
      public  ConfigurationHXEShaders Shaders       { get; set; } = new ConfigurationHXEShaders();

      public void Save()
      {
        /* core */
        {
          Configuration.Kernel.SkipVerifyMainAssets = Kernel.SkipVerifyMainAssets;
          Configuration.Kernel.SkipInvokeCoreTweaks = Kernel.SkipInvokeCoreTweaks;
          Configuration.Kernel.SkipResumeCheckpoint = Kernel.SkipResumeCheckpoint;
          Configuration.Kernel.SkipSetShadersConfig = Kernel.SkipSetShadersConfig;
          Configuration.Kernel.SkipInvokeExecutable = Kernel.SkipInvokeExecutable;
          Configuration.Kernel.SkipPatchLargeAAware = Kernel.SkipPatchLargeAAware;
          Configuration.Kernel.EnableSpv3KernelMode = Kernel.EnableSpv3KernelMode;
          Configuration.Kernel.EnableSpv3LegacyMode = Kernel.EnableSpv3LegacyMode;
        }

        /* shaders */
        {
          Configuration.PostProcessing.DynamicLensFlares = Shaders.DynamicLensFlares;
          Configuration.PostProcessing.Volumetrics       = Shaders.Volumetrics;
          Configuration.PostProcessing.LensDirt          = Shaders.LensDirt;
          Configuration.PostProcessing.HudVisor          = Shaders.HudVisor;
          Configuration.PostProcessing.FilmGrain         = Shaders.FilmGrain;

          switch (Shaders.Mxao)
          {
            case 0:
              Configuration.PostProcessing.Mxao = PostProcessing.MxaoOptions.Off;
              break;
            case 1:
              Configuration.PostProcessing.Mxao = PostProcessing.MxaoOptions.Low;
              break;
            case 2:
              Configuration.PostProcessing.Mxao = PostProcessing.MxaoOptions.High;
              break;
          }

          switch (Shaders.MotionBlur)
          {
            case 0:
              Configuration.PostProcessing.MotionBlur = PostProcessing.MotionBlurOptions.Off;
              break;
            case 1:
              Configuration.PostProcessing.MotionBlur = PostProcessing.MotionBlurOptions.BuiltIn;
              break;
            case 2:
              Configuration.PostProcessing.MotionBlur = PostProcessing.MotionBlurOptions.PombLow;
              break;
            case 3:
              Configuration.PostProcessing.MotionBlur = PostProcessing.MotionBlurOptions.PombHigh;
              break;
          }

          switch (Shaders.Dof)
          {
            case 0:
              Configuration.PostProcessing.Dof = PostProcessing.DofOptions.Off;
              break;
            case 1:
              Configuration.PostProcessing.Dof = PostProcessing.DofOptions.Low;
              break;
            case 2:
              Configuration.PostProcessing.Dof = PostProcessing.DofOptions.High;
              break;
          }
        }

        Configuration.Save();
      }

      public void Load()
      {
        if (!Configuration.Exists()) return;

        Configuration.Load();

        /* core */
        {
          Kernel.SkipVerifyMainAssets = Configuration.Kernel.SkipVerifyMainAssets;
          Kernel.SkipInvokeCoreTweaks = Configuration.Kernel.SkipInvokeCoreTweaks;
          Kernel.SkipResumeCheckpoint = Configuration.Kernel.SkipResumeCheckpoint;
          Kernel.SkipSetShadersConfig = Configuration.Kernel.SkipSetShadersConfig;
          Kernel.SkipInvokeExecutable = Configuration.Kernel.SkipInvokeExecutable;
          Kernel.SkipPatchLargeAAware = Configuration.Kernel.SkipPatchLargeAAware;
          Kernel.EnableSpv3KernelMode = Configuration.Kernel.EnableSpv3KernelMode;
          Kernel.EnableSpv3LegacyMode = Configuration.Kernel.EnableSpv3LegacyMode;
        }

        /* shaders */
        {
          Shaders.DynamicLensFlares = Configuration.PostProcessing.DynamicLensFlares;
          Shaders.Volumetrics       = Configuration.PostProcessing.Volumetrics;
          Shaders.LensDirt          = Configuration.PostProcessing.LensDirt;
          Shaders.HudVisor          = Configuration.PostProcessing.HudVisor;
          Shaders.FilmGrain         = Configuration.PostProcessing.FilmGrain;

          switch (Configuration.PostProcessing.Mxao)
          {
            case PostProcessing.MxaoOptions.Off:
              Shaders.Mxao = 0;
              break;
            case PostProcessing.MxaoOptions.Low:
              Shaders.Mxao = 1;
              break;
            case PostProcessing.MxaoOptions.High:
              Shaders.Mxao = 2;
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }

          switch (Configuration.PostProcessing.MotionBlur)
          {
            case PostProcessing.MotionBlurOptions.Off:
              Shaders.MotionBlur = 0;
              break;
            case PostProcessing.MotionBlurOptions.BuiltIn:
              Shaders.MotionBlur = 1;
              break;
            case PostProcessing.MotionBlurOptions.PombLow:
              Shaders.MotionBlur = 2;
              break;
            case PostProcessing.MotionBlurOptions.PombHigh:
              Shaders.MotionBlur = 3;
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }

          switch (Configuration.PostProcessing.Dof)
          {
            case PostProcessing.DofOptions.Off:
              Shaders.Dof = 0;
              break;
            case PostProcessing.DofOptions.Low:
              Shaders.Dof = 1;
              break;
            case PostProcessing.DofOptions.High:
              Shaders.Dof = 2;
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
    }
  }
}