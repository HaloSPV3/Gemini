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

namespace SPV3
{
  /// <summary>
  ///   Main settings object.
  /// </summary>
  public class Settings : INotifyPropertyChanged
  {
    private readonly Configuration _configuration =
      (Configuration) Paths.Files.Configuration;

    private ColorBlindModeOptions   _colorBlind;
    private DofOptions              _dof;
    private bool                    _dynamicLensFlares;
    private bool                    _enableSpv3LegacyMode;
    private bool                    _hudVisor;
    private bool                    _lensDirt;
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