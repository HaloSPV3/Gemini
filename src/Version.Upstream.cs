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
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using SPV3.Annotations;
using static HXE.Net.DefaultHttpClient;

namespace SPV3
{
  public partial class Version
  {
    public class VersionUpstream : INotifyPropertyChanged
    {
      private const string Latest = "https://github.com/HaloSPV3/HCE/raw/meta/latest.xml";
      private string _address;
      private string _content;
      private System.Version _version;

      private Visibility _visibility = Visibility.Collapsed;

      public Visibility Visibility
      {
        get => _visibility;
        set
        {
          if (value == _visibility) return;
          _visibility = value;
          OnPropertyChanged();
        }
      }

      public string Content
      {
        get => _content;
        set
        {
          if (value == _content) return;
          _content = value;
          OnPropertyChanged();
        }
      }

      public string Address
      {
        get => _address;
        set
        {
          if (value == _address) return;
          _address = value;
          OnPropertyChanged();
        }
      }

      // TODO: rename to NewVersion
      public System.Version Version
      {
        get => _version;
        set
        {
          if (value == _version) return;
          _version = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public async void Initialise()
      {
        try
        {
          var oldTimeout = Client.Timeout;
          Client.Timeout = TimeSpan.FromSeconds(10);
          using (var rm = await Client.GetAsync(Latest))
          using (var rs = await rm.Content.ReadAsStreamAsync())
          using (var sr = new StreamReader(rs
                                           ?? throw new Exception("Could not get response stream.")))
          {
            var serverVersion = System.Version.Parse(sr.ReadLine()?.TrimEnd()
                                          ?? throw new Exception("Could not infer server-side version."));
            var clientVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            Version = serverVersion;
            Content    = $"Update loader to {serverVersion}";
            Visibility = serverVersion > clientVersion ? Visibility.Visible : Visibility.Collapsed;
            Address    = sr.ReadLine()?.TrimEnd() ?? throw new Exception("Could not infer update ZIP.");
          }
          Client.Timeout = oldTimeout;
        }
        catch (Exception)
        {
          Visibility = Visibility.Collapsed;
        }
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
