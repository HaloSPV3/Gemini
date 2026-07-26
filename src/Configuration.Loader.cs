/**
 * Copyright (c) 2019 Emilian Roman
 * Copyright (c) 2021 Noah Sherwin
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

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SPV3.Annotations;
using static System.Windows.Forms.Screen;

namespace SPV3
{
  public partial class Configuration
  {
    /// <summary>See <see cref="HXE.Kernel.Configuration" />, <see cref="Kernel" /> </summary>
    public class ConfigurationLoader : INotifyPropertyChanged
    {
      private const int Length = 256;

      private byte _adapter;
      private bool _borderless;
      private bool _cinemaBars;
      private byte _displayMode;
      private bool _doom;
      private bool _eax;
      private bool _elevated;
      private byte _framerate = 60;
      private bool _gammaOn;
      private byte _gamma = 150;
      private ushort _height = (ushort)PrimaryScreen.Bounds.Height;
      private bool _photo;
      private bool _preset = true;
      private bool _resolutionEnabled;
      private bool _shaders = true;
      private bool _vSync;
      private ushort _width = (ushort)PrimaryScreen.Bounds.Width;
      private bool _window;

      /// <summary> display - fullscreen/window/borderless </summary>
      /// <value> An untyped, signed-byte enum. See <see cref="DisplayModes"/>br/>
      /// 0 == Fullscreen  <br/>
      /// 1 == Window  <br/>
      /// 2 == Borderless  <br/></value>
      public byte DisplayMode
      {
        get => _displayMode;
        set
        {
          if (value == _displayMode) return;
          _displayMode = value;
          OnPropertyChanged();
          UpdateDisplayParams();
        }
      }

      public enum DisplayModes
      {
        Fullscreen = 0,
        Window = 1,
        Borderless = 2
      }

      /// <summary> runs spv3/hce as a windowed application </summary>
      public bool Window
      {
        get => _window;
        set
        {
          if (value == _window) return;
          _window = value;
          OnPropertyChanged();
        }
      }

      /// <summary> width spv3/hce will be displayed at </summary>
      public ushort Width
      {
        get => _width;
        set
        {
          if (value == _width) return;
          _width = value;
          OnPropertyChanged();
        }
      }

      /// <summary> height spv3/hce will be displayed at </summary>
      public ushort Height
      {
        get => _height;
        set
        {
          if (value == _height) return;
          _height = value;
          OnPropertyChanged();
        }
      }

      /// <summary> toggle spv3 doom mode </summary>
      public bool DOOM
      {
        get => _doom;
        set
        {
          if (value == _doom) return;
          _doom = value;
          OnPropertyChanged();
        }
      }

      /// <summary> enables spv3 photo/blind mode </summary>
      public bool Photo
      {
        get => _photo;
        set
        {
          if (value == _photo) return;
          _photo = value;
          OnPropertyChanged();
        }
      }

      /// <summary> use the built-in spv3 controller preset </summary>
      public bool Preset
      {
        get => _preset;
        set
        {
          if (value == _preset) return;
          _preset = value;
          OnPropertyChanged();
        }
      }

      /// <summary> toggle spv3 post-processing effects </summary>
      public bool Shaders
      {
        get => _shaders;
        set
        {
          if (value == _shaders) return;
          _shaders = value;
          OnPropertyChanged();
        }
      }

      /// <summary> framerate to run spv3 at (in v-sync mode) </summary>
      public byte Framerate
      {
        get => _framerate;
        set
        {
          if (value == _framerate) return;
          _framerate = value;
          OnPropertyChanged();
        }
      }

      /// <summary> V-sync preference (locked vs unlocked) </summary>
      // TODO: rename to VSync
      public bool Vsync
      {
        get => _vSync;
        set
        {
          if (value == _vSync) return;
          _vSync = value;
          OnPropertyChanged();
          if (value == true)
            ResetDisplayMode();
        }
      }

      /// <summary> toggle hardware acceleration and environmental sound (emulated via OpenAL-soft)</summary>
      public bool EAX //DevSkim: ignore DS187371
      {
        get => _eax;
        set
        {
          if (value == _eax) return;
          _eax = value;
          OnPropertyChanged();
        }
      }

      /// <summary> when false, runs spv3/hce with -nogamma </summary>
      public bool GammaOn
      {
        get => _gammaOn;
        set
        {
          if (value == _gammaOn) return;
          _gammaOn = value;
          OnPropertyChanged();
        }
      }

      /// <summary> gamma level to run spv3 at (in v-sync mode) </summary>
      public byte Gamma
      {
        get => _gamma;
        set
        {
          if (value == _gamma) return;
          _gamma = value;
          OnPropertyChanged();
        }
      }

      /// <summary> physical monitor to run hce/spv3 on </summary>
      public byte Adapter
      {
        get => _adapter;
        set
        {
          if (value == _adapter) return;
          _adapter = value;
          OnPropertyChanged();
        }
      }

      /// <summary> toggle spv3 cinematic black bars </summary>
      /// <seealso cref="HXE.Kernel.Configuration.ConfigurationTweaks.CinemaBars"/>
      public bool CinemaBars
      {
        get => _cinemaBars;
        set
        {
          if (value == _cinemaBars) return;
          _cinemaBars = value;
          OnPropertyChanged();
        }
      }

      /// <summary> run hce/spv3 without window borders </summary>
      public bool Borderless
      {
        get => _borderless;
        set
        {
          if (value == _borderless) return;
          _borderless = value;
          OnPropertyChanged();
        }
      }

      /// <summary> ability to provide custom resolution </summary>
      public bool ResolutionEnabled
      {
        get => _resolutionEnabled;
        set
        {
          if (value == _resolutionEnabled) return;
          _resolutionEnabled = value;
          OnPropertyChanged();
          if (value == true)
            ResetDisplayMode();
        }
      }

      /// <summary> runs spv3/hce in elevated (admin) mode </summary>
      public bool Elevated
      {
        get => _elevated;
        set
        {
          if (value == _elevated) return;
          _elevated = value;
          OnPropertyChanged();
          if (value == true) DisplayMode = 0;
          /// DisplayMode.Get{} calls WindowBorderlessUpdate()
        }
      }

      public static List<string> Adapters => AllScreens
        .Select
        (
          screen => screen.DeviceName
            .Substring(4)
            .Replace("DISPLAY", "Display ")
        ).ToList();

      public event PropertyChangedEventHandler? PropertyChanged;

      public void ResetDisplayMode()
      {
        if (DisplayMode == (byte)DisplayModes.Borderless)
          DisplayMode = (byte)DisplayModes.Fullscreen;
      }

      public void UpdateDisplayParams()
      {
        switch (_displayMode)
        {
          case 0:
            Borderless = false;
            /*Elevated          = Elevated;          */
            /*ResolutionEnabled = ResolutionEnabled; */
            /*Vsync             = Vsync;             */
            Window = false;
            break;

          case 1:
            Borderless = false;
            /*Elevated          = Elevated;          */
            /*ResolutionEnabled = ResolutionEnabled  */
            Vsync = true;
            Window = true;
            break;

          case 2:
            Borderless = true;
            Elevated = false;
            ResolutionEnabled = false;
            Vsync = true;
            Window = true;
            break;

          default:
            break;
        }
      }

      public void Save()
      {
        try
        {
          using (var fs = new FileStream(Paths.Configuration, FileMode.Create, FileAccess.Write))
          using (var ms = new MemoryStream(Length))
          using (var bw = new BinaryWriter(ms))
          {
            /* padding */
            {
              bw.Write(new byte[Length]);
              ms.Position = 0;
            }

            /* signature */
            {
              bw.Write(Encoding.Unicode.GetBytes("~yumiris"));
            }

            /* padding */
            {
              bw.Write(new byte[16 - ms.Position]);
            }

            /* video */
            {
              bw.Write(Shaders);
              bw.Write(Window);
              bw.Write(Width);
              bw.Write(Height);
              bw.Write(Framerate);
              bw.Write(Vsync);
              bw.Write(GammaOn);
              bw.Write(Gamma);
              bw.Write(Adapter);
            }

            /* modes */
            {
              bw.Write(DOOM);
              bw.Write(Photo);
              bw.Write(Borderless);
            }

            /* tweaks */
            {
              bw.Write(EAX); //DevSkim: ignore DS187371
              bw.Write(Preset);
              bw.Write(CinemaBars);
              bw.Write(Elevated);
            }

            /* display mode */
            {
              bw.Write(DisplayMode);
              bw.Write(ResolutionEnabled);
            }

            ms.Position = 0;
            ms.CopyTo(fs);
          }
        }
        catch (System.Exception e)
        {
          var log = (HXE.File)Paths.Exception;
          log.AppendAllText("The Loader could not save settings.\n Error: " + e + "\n");
        }
      }

      public ConfigurationLoader Load()
      {
        if (!Exists())
          return this;

        try
        {
          using (var fs = new FileStream(Paths.Configuration, FileMode.Open, FileAccess.Read))
          using (var ms = new MemoryStream(Length))
          using (var br = new BinaryReader(ms))
          {
            fs.CopyTo(ms);
            ms.Position = 0;

            /* padding */
            {
              ms.Position += 16 - ms.Position;
            }

            /* video */
            {
              Shaders = br.ReadBoolean();
              Window = br.ReadBoolean();
              Width = br.ReadUInt16();
              Height = br.ReadUInt16();
              Framerate = br.ReadByte();
              Vsync = br.ReadBoolean();
              GammaOn = br.ReadBoolean();
              Gamma = br.ReadByte();
              Adapter = br.ReadByte();
            }

            /* modes */
            {
              DOOM = br.ReadBoolean();
              Photo = br.ReadBoolean();
              Borderless = br.ReadBoolean();
            }

            /* tweaks */
            {
              EAX = br.ReadBoolean(); //DevSkim: ignore DS187371
              Preset = br.ReadBoolean();
              CinemaBars = br.ReadBoolean();
              Elevated = br.ReadBoolean();
            }

            /* display mode */
            {
              DisplayMode = br.ReadByte();
              ResolutionEnabled = br.ReadBoolean();
            }
            return this;
          }
        }
        catch (System.Exception e)
        {
          var log = (HXE.File)Paths.Exception;
          log.AppendAllText("Failed to load Loader settings.\n Error: " + e + "\n");
          return this;
        }
      }

      public static bool Exists()
      {
        return File.Exists(Paths.Configuration);
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
