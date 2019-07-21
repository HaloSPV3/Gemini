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

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using SPV3.Annotations;

namespace SPV3
{
  public partial class Configuration
  {
    public class ConfigurationLoader : INotifyPropertyChanged
    {
      private const int Length = 256;

      private byte   _adapter;
      private bool   _borderless = true;
      private bool   _cinematic  = true;
      private bool   _doom;
      private bool   _eax;
      private byte   _framerate = 60;
      private byte   _gamma     = 150;
      private ushort _height    = (ushort) Screen.PrimaryScreen.Bounds.Height;
      private bool   _photo;
      private byte   _preference = 1;
      private bool   _preset     = true;
      private bool   _shaders    = true;
      private ushort _width      = (ushort) Screen.PrimaryScreen.Bounds.Width;
      private bool   _window;

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

      public byte Preference
      {
        get => _preference;
        set
        {
          if (value == _preference) return;
          _preference = value;
          OnPropertyChanged();
        }
      }

      public bool EAX
      {
        get => _eax;
        set
        {
          if (value == _eax) return;
          _eax = value;
          OnPropertyChanged();
        }
      }

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

      public bool Cinematic
      {
        get => _cinematic;
        set
        {
          if (value == _cinematic) return;
          _cinematic = value;
          OnPropertyChanged();
        }
      }

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

      public List<string> Adapters => Screen.AllScreens
        .Select
        (
          screen => screen.DeviceName
            .Substring(4)
            .Replace("DISPLAY", "Display ")
        ).ToList();

      public event PropertyChangedEventHandler PropertyChanged;

      public void Save()
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
            bw.Write(Preference);
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
            bw.Write(EAX);
            bw.Write(Preset);
            bw.Write(Cinematic);
          }

          ms.Position = 0;
          ms.CopyTo(fs);
        }
      }

      public void Load()
      {
        if (!Exists())
          return;

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
            Shaders    = br.ReadBoolean();
            Window     = br.ReadBoolean();
            Width      = br.ReadUInt16();
            Height     = br.ReadUInt16();
            Framerate  = br.ReadByte();
            Preference = br.ReadByte();
            Gamma      = br.ReadByte();
            Adapter    = br.ReadByte();
          }

          /* modes */
          {
            DOOM       = br.ReadBoolean();
            Photo      = br.ReadBoolean();
            Borderless = br.ReadBoolean();
          }

          /* tweaks */
          {
            EAX       = br.ReadBoolean();
            Preset    = br.ReadBoolean();
            Cinematic = br.ReadBoolean();
          }
        }
      }

      public bool Exists()
      {
        return File.Exists(Paths.Configuration);
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}