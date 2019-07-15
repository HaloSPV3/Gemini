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
using System.Windows;

namespace HXE
{
  /// <summary>
  ///   Interaction logic for Settings.xaml
  /// </summary>
  public partial class Settings
  {
    private readonly Kernel.Configuration _configuration = new Kernel.Configuration(Paths.Configuration);

    public Settings()
    {
      InitializeComponent();

      Console.Info("Loading kernel settings");

      _configuration.Load();

      switch (_configuration.Mode)
      {
        case Kernel.Configuration.ConfigurationMode.HCE:
          Mode.SelectedIndex = 0;
          break;
        case Kernel.Configuration.ConfigurationMode.SPV31:
          Mode.SelectedIndex = 1;
          break;
        case Kernel.Configuration.ConfigurationMode.SPV32:
          Mode.SelectedIndex = 2;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      MainReset.IsChecked          = _configuration.Main.Reset;
      MainPatch.IsChecked          = _configuration.Main.Patch;
      MainStart.IsChecked          = _configuration.Main.Start;
      MainResume.IsChecked         = _configuration.Main.Resume;
      TweaksSpeed.Text             = _configuration.Tweaks.Speed.ToString();
      TweaksCinematic.IsChecked    = _configuration.Tweaks.Cinematic;
      TweaksSensor.IsChecked       = _configuration.Tweaks.Sensor;
      TweaksMagnetism.IsChecked    = _configuration.Tweaks.Magnetism;
      TweaksAutoAim.IsChecked      = _configuration.Tweaks.AutoAim;
      TweaksAcceleration.IsChecked = _configuration.Tweaks.Acceleration;
      TweaksUnload.IsChecked       = _configuration.Tweaks.Unload;
      VideoResolution.IsChecked    = _configuration.Video.Resolution;
      VideoUncap.IsChecked         = _configuration.Video.Uncap;
      VideoQuality.IsChecked       = _configuration.Video.Quality;
      VideoGamma.Text              = _configuration.Video.Gamma.ToString();
      AudioQuality.IsChecked       = _configuration.Audio.Quality;
      AudioEnhancements.IsChecked  = _configuration.Audio.Enhancements;
      InputOverride.IsChecked      = _configuration.Input.Override;

      PrintConfiguration();
    }

    private void Save(object sender, RoutedEventArgs e)
    {
      Console.Info("Saving kernel settings");

      switch (Mode.SelectedIndex)
      {
        case 0:
          _configuration.Mode = Kernel.Configuration.ConfigurationMode.HCE;
          break;
        case 1:
          _configuration.Mode = Kernel.Configuration.ConfigurationMode.SPV31;
          break;
        case 2:
          _configuration.Mode = Kernel.Configuration.ConfigurationMode.SPV32;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      _configuration.Main.Reset          = MainReset.IsChecked          == true;
      _configuration.Main.Patch          = MainPatch.IsChecked          == true;
      _configuration.Main.Start          = MainStart.IsChecked          == true;
      _configuration.Main.Resume         = MainResume.IsChecked         == true;
      _configuration.Tweaks.Cinematic    = TweaksCinematic.IsChecked    == true;
      _configuration.Tweaks.Sensor       = TweaksSensor.IsChecked       == true;
      _configuration.Tweaks.Magnetism    = TweaksMagnetism.IsChecked    == true;
      _configuration.Tweaks.AutoAim      = TweaksAutoAim.IsChecked      == true;
      _configuration.Tweaks.Acceleration = TweaksAcceleration.IsChecked == true;
      _configuration.Tweaks.Unload       = TweaksUnload.IsChecked       == true;
      _configuration.Video.Resolution    = VideoResolution.IsChecked    == true;
      _configuration.Video.Uncap         = VideoUncap.IsChecked         == true;
      _configuration.Video.Quality       = VideoQuality.IsChecked       == true;
      _configuration.Audio.Quality       = AudioQuality.IsChecked       == true;
      _configuration.Audio.Enhancements  = AudioEnhancements.IsChecked  == true;
      _configuration.Input.Override      = InputOverride.IsChecked      == true;

      try
      {
        _configuration.Tweaks.Speed = byte.Parse(TweaksSpeed.Text);
      }
      catch (Exception)
      {
        _configuration.Tweaks.Speed = 1;
      }

      try
      {
        _configuration.Video.Gamma = byte.Parse(VideoGamma.Text);
      }
      catch (Exception)
      {
        _configuration.Video.Gamma = 0;
      }

      _configuration.Save();
      _configuration.Load();

      PrintConfiguration();

      Exit.WithCode(Exit.Code.Success);
    }

    private void PrintConfiguration()
    {
      Console.Debug("Mode                - " + _configuration.Mode);
      Console.Debug("Main.Reset          - " + _configuration.Main.Reset);
      Console.Debug("Main.Patch          - " + _configuration.Main.Patch);
      Console.Debug("Main.Start          - " + _configuration.Main.Start);
      Console.Debug("Main.Resume         - " + _configuration.Main.Resume);
      Console.Debug("Tweaks.Speed        - " + _configuration.Tweaks.Speed);
      Console.Debug("Tweaks.Cinematic    - " + _configuration.Tweaks.Cinematic);
      Console.Debug("Tweaks.Sensor       - " + _configuration.Tweaks.Sensor);
      Console.Debug("Tweaks.Magnetism    - " + _configuration.Tweaks.Magnetism);
      Console.Debug("Tweaks.AutoAim      - " + _configuration.Tweaks.AutoAim);
      Console.Debug("Tweaks.Acceleration - " + _configuration.Tweaks.Acceleration);
      Console.Debug("Tweaks.Unload       - " + _configuration.Tweaks.Unload);
      Console.Debug("Video.Resolution    - " + _configuration.Video.Resolution);
      Console.Debug("Video.Uncap         - " + _configuration.Video.Uncap);
      Console.Debug("Video.Quality       - " + _configuration.Video.Quality);
      Console.Debug("Video.Gamma         - " + _configuration.Video.Gamma);
      Console.Debug("Audio.Quality       - " + _configuration.Audio.Quality);
      Console.Debug("Audio.Enhancements  - " + _configuration.Audio.Enhancements);
      Console.Debug("Input.Override      - " + _configuration.Input.Override);
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
      Exit.WithCode(Exit.Code.Success);
    }
  }
}