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

using System.ComponentModel;
using System.IO;
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

      private bool   _bare;
      private bool   _photo;
      private bool   _doom;
      private bool   _gamma;
      private ushort _height = (ushort) Screen.PrimaryScreen.Bounds.Height;
      private ushort _width  = (ushort) Screen.PrimaryScreen.Bounds.Width;
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

      public bool Gamma
      {
        get => _gamma;
        set
        {
          if (value == _gamma) return;
          _gamma = value;
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

      public bool Bare
      {
        get => _bare;
        set
        {
          if (value == _bare) return;
          _bare = value;
          OnPropertyChanged();
        }
      }

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
            bw.Write(Width);
            bw.Write(Height);
            bw.Write(Window);
            bw.Write(Gamma);
            bw.Write(DOOM);
            bw.Write(Photo);
            bw.Write(Bare);
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
            Width  = br.ReadUInt16();
            Height = br.ReadUInt16();
            Window = br.ReadBoolean();
            Gamma  = br.ReadBoolean();
            DOOM   = br.ReadBoolean();
            Photo  = br.ReadBoolean();
            Bare   = br.ReadBoolean();
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