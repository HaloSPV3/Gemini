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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using HXE;
using SPV3.Annotations;
using static HXE.SPV3.PostProcessing;
using static HXE.SPV3.PostProcessing.ExperimentalPostProcessing;
using File = System.IO.File;

namespace SPV3
{
  /// <summary>
  ///   Main settings object.
  /// </summary>
  public class Settings
  {
    public MainSettings      Main      { get; set; } = new MainSettings();
    public OpenSauceSettings OpenSauce { get; set; } = new OpenSauceSettings();

    public void Save()
    {
      Main.Save();
      OpenSauce.Save();
    }

    public void Load()
    {
      Main.Load();
      OpenSauce.Load();
    }
  }

  public class MainSettings : INotifyPropertyChanged
  {
    private readonly Configuration _configuration =
      (Configuration) HXE.Paths.Files.Configuration;

    private ColorBlindModeOptions   _colorBlind;
    private DofOptions              _dof;
    private bool                    _dynamicLensFlares;
    private bool                    _enableSpv3LegacyMode;
    private bool                    _hudVisor;
    private bool                    _lensDirt;
    private int                     _level;
    private string                  _levelDescription;
    private MotionBlurOptions       _motionBlur;
    private MxaoOptions             _mxao;
    private bool                    _skipInvokeCoreTweaks;
    private bool                    _skipInvokeExecutable;
    private bool                    _skipPatchLargeAAware;
    private bool                    _skipResumeCheckpoint;
    private bool                    _skipSetShadersConfig;
    private bool                    _skipVerifyMainAssets;
    private ThreeDimensionalOptions _threeDimensional;
    private bool                    _volumetrics;

    public int Level
    {
      get => _level;
      set
      {
        if (value == _level) return;
        _level = value;

        switch (_level)
        {
          case 0: /* bare */
            LevelDescription  = "Bare";
            Mxao              = MxaoOptions.Off;
            Dof               = DofOptions.Off;
            MotionBlur        = MotionBlurOptions.Off;
            DynamicLensFlares = false;
            Volumetrics       = false;
            LensDirt          = true;
            break;
          case 1: /* essentials */
            LevelDescription  = "Essentials";
            Mxao              = MxaoOptions.Off;
            Dof               = DofOptions.Off;
            MotionBlur        = MotionBlurOptions.Off;
            DynamicLensFlares = false;
            Volumetrics       = true;
            LensDirt          = true;
            break;
          case 2: /* minimal */
            LevelDescription  = "Minimal";
            Mxao              = MxaoOptions.Off;
            Dof               = DofOptions.Low;
            MotionBlur        = MotionBlurOptions.BuiltIn;
            DynamicLensFlares = false;
            Volumetrics       = true;
            LensDirt          = true;
            break;
          case 3: /* low */
            LevelDescription  = "Low";
            Mxao              = MxaoOptions.Low;
            Dof               = DofOptions.Low;
            MotionBlur        = MotionBlurOptions.BuiltIn;
            DynamicLensFlares = false;
            Volumetrics       = true;
            LensDirt          = true;
            break;
          case 4: /* medium */
            LevelDescription  = "Medium";
            Mxao              = MxaoOptions.Low;
            Dof               = DofOptions.Low;
            MotionBlur        = MotionBlurOptions.BuiltIn;
            DynamicLensFlares = true;
            Volumetrics       = true;
            LensDirt          = true;
            break;
          case 5: /* high */
            LevelDescription  = "High";
            Mxao              = MxaoOptions.Low;
            Dof               = DofOptions.High;
            MotionBlur        = MotionBlurOptions.BuiltIn;
            DynamicLensFlares = true;
            Volumetrics       = true;
            LensDirt          = true;
            break;
          case 6: /* ultra */
            LevelDescription  = "Ultra";
            Mxao              = MxaoOptions.High;
            Dof               = DofOptions.High;
            MotionBlur        = MotionBlurOptions.BuiltIn;
            DynamicLensFlares = true;
            Volumetrics       = true;
            LensDirt          = true;
            break;
          case 7: /* full */
            LevelDescription  = "Full";
            Mxao              = MxaoOptions.High;
            Dof               = DofOptions.High;
            MotionBlur        = MotionBlurOptions.PombHigh;
            DynamicLensFlares = true;
            Volumetrics       = true;
            LensDirt          = true;
            break;
          case 8: /* off */
            LevelDescription  = "Off";
            Mxao              = MxaoOptions.Off;
            Dof               = DofOptions.Off;
            MotionBlur        = MotionBlurOptions.Off;
            DynamicLensFlares = false;
            Volumetrics       = false;
            LensDirt          = false;
            break;
        }

        OnPropertyChanged();
      }
    }

    public string LevelDescription
    {
      get => _levelDescription;
      set
      {
        if (value == _levelDescription) return;
        _levelDescription = value;
        OnPropertyChanged();
      }
    }

    public bool EnableSpv3LegacyMode
    {
      get => _enableSpv3LegacyMode;
      set
      {
        if (value == _enableSpv3LegacyMode) return;
        _enableSpv3LegacyMode = value;
        OnPropertyChanged();
      }
    }

    public bool SkipVerifyMainAssets
    {
      get => _skipVerifyMainAssets;
      set
      {
        if (value == _skipVerifyMainAssets) return;
        _skipVerifyMainAssets = value;
        OnPropertyChanged();
      }
    }

    public bool SkipInvokeCoreTweaks
    {
      get => _skipInvokeCoreTweaks;
      set
      {
        if (value == _skipInvokeCoreTweaks) return;
        _skipInvokeCoreTweaks = value;
        OnPropertyChanged();
      }
    }

    public bool SkipResumeCheckpoint
    {
      get => _skipResumeCheckpoint;
      set
      {
        if (value == _skipResumeCheckpoint) return;
        _skipResumeCheckpoint = value;
        OnPropertyChanged();
      }
    }

    public bool SkipSetShadersConfig
    {
      get => _skipSetShadersConfig;
      set
      {
        if (value == _skipSetShadersConfig) return;
        _skipSetShadersConfig = value;
        OnPropertyChanged();
      }
    }

    public bool SkipInvokeExecutable
    {
      get => _skipInvokeExecutable;
      set
      {
        if (value == _skipInvokeExecutable) return;
        _skipInvokeExecutable = value;
        OnPropertyChanged();
      }
    }

    public bool SkipPatchLargeAAware
    {
      get => _skipPatchLargeAAware;
      set
      {
        if (value == _skipPatchLargeAAware) return;
        _skipPatchLargeAAware = value;
        OnPropertyChanged();
      }
    }

    public bool LensDirt
    {
      get => _lensDirt;
      set
      {
        if (value == _lensDirt) return;
        _lensDirt = value;
        OnPropertyChanged();
      }
    }

    public bool DynamicLensFlares
    {
      get => _dynamicLensFlares;
      set
      {
        if (value == _dynamicLensFlares) return;
        _dynamicLensFlares = value;
        OnPropertyChanged();
      }
    }

    public bool Volumetrics
    {
      get => _volumetrics;
      set
      {
        if (value == _volumetrics) return;
        _volumetrics = value;
        OnPropertyChanged();
      }
    }

    public bool HudVisor
    {
      get => _hudVisor;
      set
      {
        if (value == _hudVisor) return;
        _hudVisor = value;
        OnPropertyChanged();
      }
    }

    public DofOptions Dof
    {
      get => _dof;
      set
      {
        if (value == _dof) return;
        _dof = value;
        OnPropertyChanged();
      }
    }

    public MxaoOptions Mxao
    {
      get => _mxao;
      set
      {
        if (value == _mxao) return;
        _mxao = value;
        OnPropertyChanged();
      }
    }

    public MotionBlurOptions MotionBlur
    {
      get => _motionBlur;
      set
      {
        if (value == _motionBlur) return;
        _motionBlur = value;
        OnPropertyChanged();
      }
    }

    public ThreeDimensionalOptions ThreeDimensional
    {
      get => _threeDimensional;
      set
      {
        if (value == _threeDimensional) return;
        _threeDimensional = value;
        OnPropertyChanged();
      }
    }

    public ColorBlindModeOptions ColorBlind
    {
      get => _colorBlind;
      set
      {
        if (value == _colorBlind) return;
        _colorBlind = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Save()
    {
      _configuration.Kernel.EnableSpv3LegacyMode = EnableSpv3LegacyMode;
      _configuration.Kernel.SkipVerifyMainAssets = SkipVerifyMainAssets;
      _configuration.Kernel.SkipInvokeCoreTweaks = SkipInvokeCoreTweaks;
      _configuration.Kernel.SkipResumeCheckpoint = SkipResumeCheckpoint;
      _configuration.Kernel.SkipSetShadersConfig = SkipSetShadersConfig;
      _configuration.Kernel.SkipInvokeExecutable = SkipInvokeExecutable;
      _configuration.Kernel.SkipPatchLargeAAware = SkipPatchLargeAAware;

      _configuration.PostProcessing.LensDirt                      = LensDirt;
      _configuration.PostProcessing.DynamicLensFlares             = DynamicLensFlares;
      _configuration.PostProcessing.Volumetrics                   = Volumetrics;
      _configuration.PostProcessing.HudVisor                      = HudVisor;
      _configuration.PostProcessing.Dof                           = Dof;
      _configuration.PostProcessing.Mxao                          = Mxao;
      _configuration.PostProcessing.MotionBlur                    = MotionBlur;
      _configuration.PostProcessing.Experimental.ThreeDimensional = ThreeDimensional;
      _configuration.PostProcessing.Experimental.ColorBlindMode   = ColorBlind;

      _configuration.Save();
    }

    public void Load()
    {
      _configuration.Load();

      EnableSpv3LegacyMode = _configuration.Kernel.EnableSpv3LegacyMode;
      SkipVerifyMainAssets = _configuration.Kernel.SkipVerifyMainAssets;
      SkipInvokeCoreTweaks = _configuration.Kernel.SkipInvokeCoreTweaks;
      SkipResumeCheckpoint = _configuration.Kernel.SkipResumeCheckpoint;
      SkipSetShadersConfig = _configuration.Kernel.SkipSetShadersConfig;
      SkipInvokeExecutable = _configuration.Kernel.SkipInvokeExecutable;
      SkipPatchLargeAAware = _configuration.Kernel.SkipPatchLargeAAware;

      LensDirt          = _configuration.PostProcessing.LensDirt;
      DynamicLensFlares = _configuration.PostProcessing.DynamicLensFlares;
      Volumetrics       = _configuration.PostProcessing.Volumetrics;
      HudVisor          = _configuration.PostProcessing.HudVisor;
      Dof               = _configuration.PostProcessing.Dof;
      Mxao              = _configuration.PostProcessing.Mxao;
      MotionBlur        = _configuration.PostProcessing.MotionBlur;
      ThreeDimensional  = _configuration.PostProcessing.Experimental.ThreeDimensional;
      ColorBlind        = _configuration.PostProcessing.Experimental.ColorBlindMode;
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  public class OpenSauceSettings : INotifyPropertyChanged
  {
    private OpenSauce _openSauce = (OpenSauce) HXE.Paths.Files.OpenSauce;

    private bool   _antiAliasing;
    private bool   _bloom;
    private bool   _checkForGameUpdates;
    private bool   _depthFade;
    private bool   _detailedMaps;
    private bool   _diffuseDirectionalLightmaps;
    private bool   _externalEffects;
    public  double _fieldOfViewLevel;
    private bool   _gBuffer;
    private bool   _ignoreFovChangesInCinematics;
    private bool   _ignoreFovChangesInMainMenu;
    private bool   _increaseMaxRenderedTriangles;
    private bool   _lookForYeloMapsFirst;
    private bool   _mapDownloading;
    private bool   _mapEffects;
    private bool   _motionBlur;
    public  double _motionBlurLevel;
    private bool   _normalMaps;
    private bool   _specularDirectionalLightmaps;
    private bool   _specularLighting;
    private bool   _specularMaps;

    public void Load()
    {
      if (!_openSauce.Exists())
        _openSauce.Save();

      _openSauce.Load();

      LookForYeloMapsFirst         = _openSauce.CacheFiles.CheckYeloFilesFirst;
      FieldOfViewLevel             = _openSauce.Camera.FieldOfView;
      IgnoreFovChangesInCinematics = _openSauce.Camera.IgnoreFOVChangeInCinematics;
      IgnoreFovChangesInMainMenu   = _openSauce.Camera.IgnoreFOVChangeInMainMenu;
      MapDownloading               = _openSauce.Networking.MapDownload.Enabled;
      GBuffer                      = _openSauce.Rasterizer.GBuffer.Enabled;
      AntiAliasing                 = _openSauce.Rasterizer.PostProcessing.AntiAliasing.Enabled;
      Bloom                        = _openSauce.Rasterizer.PostProcessing.Bloom.Enabled;
      ExternalEffects              = _openSauce.Rasterizer.PostProcessing.ExternalEffects.Enabled;
      MapEffects                   = _openSauce.Rasterizer.PostProcessing.MapEffects.Enabled;
      MotionBlurLevel              = _openSauce.Rasterizer.PostProcessing.MotionBlur.BlurAmount;
      MotionBlur                   = _openSauce.Rasterizer.PostProcessing.MotionBlur.Enabled;
      DepthFade                    = _openSauce.Rasterizer.ShaderExtensions.Effect.DepthFade;
      DiffuseDirectionalLightmaps  = _openSauce.Rasterizer.ShaderExtensions.Environment.DiffuseDirectionalLightmaps;
      SpecularDirectionalLightmaps = _openSauce.Rasterizer.ShaderExtensions.Environment.SpecularDirectionalLightmaps;
      DetailedMaps                 = _openSauce.Rasterizer.ShaderExtensions.Object.DetailNormalMaps;
      NormalMaps                   = _openSauce.Rasterizer.ShaderExtensions.Object.NormalMaps;
      SpecularLighting             = _openSauce.Rasterizer.ShaderExtensions.Object.SpecularLighting;
      SpecularMaps                 = _openSauce.Rasterizer.ShaderExtensions.Object.SpecularMaps;
      IncreaseMaxRenderedTriangles = _openSauce.Rasterizer.Upgrades.MaximumRenderedTriangles;
    }

    public void Save()
    {
      _openSauce.CacheFiles.CheckYeloFilesFirst                                       = LookForYeloMapsFirst;
      _openSauce.Camera.FieldOfView                                                   = FieldOfViewLevel;
      _openSauce.Camera.IgnoreFOVChangeInCinematics                                   = IgnoreFovChangesInCinematics;
      _openSauce.Camera.IgnoreFOVChangeInMainMenu                                     = IgnoreFovChangesInMainMenu;
      _openSauce.Networking.MapDownload.Enabled                                       = MapDownloading;
      _openSauce.Rasterizer.GBuffer.Enabled                                           = GBuffer;
      _openSauce.Rasterizer.PostProcessing.AntiAliasing.Enabled                       = AntiAliasing;
      _openSauce.Rasterizer.PostProcessing.Bloom.Enabled                              = Bloom;
      _openSauce.Rasterizer.PostProcessing.ExternalEffects.Enabled                    = ExternalEffects;
      _openSauce.Rasterizer.PostProcessing.MapEffects.Enabled                         = MapEffects;
      _openSauce.Rasterizer.PostProcessing.MotionBlur.BlurAmount                      = MotionBlurLevel;
      _openSauce.Rasterizer.PostProcessing.MotionBlur.Enabled                         = MotionBlur;
      _openSauce.Rasterizer.ShaderExtensions.Effect.DepthFade                         = DepthFade;
      _openSauce.Rasterizer.ShaderExtensions.Environment.DiffuseDirectionalLightmaps  = DiffuseDirectionalLightmaps;
      _openSauce.Rasterizer.ShaderExtensions.Environment.SpecularDirectionalLightmaps = SpecularDirectionalLightmaps;
      _openSauce.Rasterizer.ShaderExtensions.Object.DetailNormalMaps                  = DetailedMaps;
      _openSauce.Rasterizer.ShaderExtensions.Object.NormalMaps                        = NormalMaps;
      _openSauce.Rasterizer.ShaderExtensions.Object.SpecularLighting                  = SpecularLighting;
      _openSauce.Rasterizer.ShaderExtensions.Object.SpecularMaps                      = SpecularMaps;
      _openSauce.Rasterizer.Upgrades.MaximumRenderedTriangles                         = IncreaseMaxRenderedTriangles;

      var backup = _openSauce.Path + Guid.NewGuid();

      if (_openSauce.Exists())
        File.Copy(_openSauce.Path, backup);

      _openSauce.Save();

      File.Delete(backup);
    }

    public bool GBuffer
    {
      get => _gBuffer;
      set
      {
        if (value == _gBuffer) return;
        _gBuffer = value;
        OnPropertyChanged();
      }
    }

    public bool IncreaseMaxRenderedTriangles
    {
      get => _increaseMaxRenderedTriangles;
      set
      {
        if (value == _increaseMaxRenderedTriangles) return;
        _increaseMaxRenderedTriangles = value;
        OnPropertyChanged();
      }
    }

    public bool IgnoreFovChangesInCinematics
    {
      get => _ignoreFovChangesInCinematics;
      set
      {
        if (value == _ignoreFovChangesInCinematics) return;
        _ignoreFovChangesInCinematics = value;
        OnPropertyChanged();
      }
    }

    public bool IgnoreFovChangesInMainMenu
    {
      get => _ignoreFovChangesInMainMenu;
      set
      {
        if (value == _ignoreFovChangesInMainMenu) return;
        _ignoreFovChangesInMainMenu = value;
        OnPropertyChanged();
      }
    }

    public bool NormalMaps
    {
      get => _normalMaps;
      set
      {
        if (value == _normalMaps) return;
        _normalMaps = value;
        OnPropertyChanged();
      }
    }

    public bool DetailedMaps
    {
      get => _detailedMaps;
      set
      {
        if (value == _detailedMaps) return;
        _detailedMaps = value;
        OnPropertyChanged();
      }
    }

    public bool SpecularMaps
    {
      get => _specularMaps;
      set
      {
        if (value == _specularMaps) return;
        _specularMaps = value;
        OnPropertyChanged();
      }
    }

    public bool SpecularLighting
    {
      get => _specularLighting;
      set
      {
        if (value == _specularLighting) return;
        _specularLighting = value;
        OnPropertyChanged();
      }
    }

    public bool DiffuseDirectionalLightmaps
    {
      get => _diffuseDirectionalLightmaps;
      set
      {
        if (value == _diffuseDirectionalLightmaps) return;
        _diffuseDirectionalLightmaps = value;
        OnPropertyChanged();
      }
    }

    public bool SpecularDirectionalLightmaps
    {
      get => _specularDirectionalLightmaps;
      set
      {
        if (value == _specularDirectionalLightmaps) return;
        _specularDirectionalLightmaps = value;
        OnPropertyChanged();
      }
    }

    public bool DepthFade
    {
      get => _depthFade;
      set
      {
        if (value == _depthFade) return;
        _depthFade = value;
        OnPropertyChanged();
      }
    }

    public bool MotionBlur
    {
      get => _motionBlur;
      set
      {
        if (value == _motionBlur) return;
        _motionBlur = value;
        OnPropertyChanged();
      }
    }

    public bool Bloom
    {
      get => _bloom;
      set
      {
        if (value == _bloom) return;
        _bloom = value;
        OnPropertyChanged();
      }
    }

    public bool AntiAliasing
    {
      get => _antiAliasing;
      set
      {
        if (value == _antiAliasing) return;
        _antiAliasing = value;
        OnPropertyChanged();
      }
    }

    public bool ExternalEffects
    {
      get => _externalEffects;
      set
      {
        if (value == _externalEffects) return;
        _externalEffects = value;
        OnPropertyChanged();
      }
    }

    public bool MapEffects
    {
      get => _mapEffects;
      set
      {
        if (value == _mapEffects) return;
        _mapEffects = value;
        OnPropertyChanged();
      }
    }

    public bool CheckForGameUpdates
    {
      get => _checkForGameUpdates;
      set
      {
        if (value == _checkForGameUpdates) return;
        _checkForGameUpdates = value;
        OnPropertyChanged();
      }
    }

    public bool MapDownloading
    {
      get => _mapDownloading;
      set
      {
        if (value == _mapDownloading) return;
        _mapDownloading = value;
        OnPropertyChanged();
      }
    }

    public bool LookForYeloMapsFirst
    {
      get => _lookForYeloMapsFirst;
      set
      {
        if (value == _lookForYeloMapsFirst) return;
        _lookForYeloMapsFirst = value;
        OnPropertyChanged();
      }
    }

    public double MotionBlurLevel
    {
      get => _motionBlurLevel;
      set
      {
        if (value == _motionBlurLevel) return;
        _motionBlurLevel = value;
        OnPropertyChanged();
      }
    }

    public double FieldOfViewLevel
    {
      get => _fieldOfViewLevel;
      set
      {
        if (value == _fieldOfViewLevel) return;
        _fieldOfViewLevel = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  /// <summary>
  ///   Radio button-enum converter.
  /// </summary>
  public class Converter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null || parameter == null)
        return false;

      var checkValue  = value.ToString();
      var targetValue = parameter.ToString();
      return checkValue.Equals(targetValue,
        StringComparison.InvariantCultureIgnoreCase);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null || parameter == null)
        return null;

      var useValue    = (bool) value;
      var targetValue = parameter.ToString();
      return useValue ? Enum.Parse(targetType, targetValue) : null;
    }
  }
}