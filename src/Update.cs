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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using SPV3.Annotations;
using static System.Environment;
using static System.IO.Path;

namespace SPV3
{
  /// <summary>
  ///   Wrapper for loader & assets update modules.
  /// </summary>
  public class Update
  {
    public LoaderUpdate Loader { get; set; } = new LoaderUpdate();
    public AssetsUpdate Assets { get; set; } = new AssetsUpdate();

    public void Initialise()
    {
      bool canUpdate;

      try
      {
        var test = Combine(CurrentDirectory, "io.bin");

        File.WriteAllBytes(test, new byte[8]);
        File.Delete(test);

        canUpdate = true;
      }
      catch (Exception)
      {
        canUpdate = false;
      }

      if (!canUpdate) return;

      Loader.Load();
    }

    /// <summary>
    ///   Update module for the SPV3 loader.
    /// </summary>
    public class LoaderUpdate : INotifyPropertyChanged
    {
      private const string Header = "https://dist.n2.network/spv3/HEADER.txt";

      private string _address;
      private bool   _available;
      private int    _version;

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

      public int Version
      {
        get => _version;
        set
        {
          if (value == _version) return;
          _version = value;
          OnPropertyChanged();
        }
      }

      public bool Available
      {
        get => _available;
        set
        {
          if (value == _available) return;
          _available = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Load()
      {
        var request = (HttpWebRequest) WebRequest.Create(Header);
        using (var response = (HttpWebResponse) request.GetResponse())
        using (var stream = response.GetResponseStream())
        using (var reader = new StreamReader(stream ?? throw new Exception("Could not get response stream.")))
        {
          var serverVersion = int.Parse(reader.ReadLine()?.TrimEnd()
                                        ?? throw new Exception("Could not infer server-side version."));

          var clientVersion = Assembly.GetEntryAssembly().GetName().Version.Major;

          Version   = serverVersion;
          Available = serverVersion > clientVersion;
          Address   = reader.ReadLine()?.TrimEnd() ?? throw new Exception("Could not infer download address.");
        }
      }

      public void Commit()
      {
        MessageBox.Show("Please replace this loader with the contents from the ZIP you will download!");
        Process.Start(Address);
        Exit(0);
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    /// <summary>
    ///   Update module for HCE/SPV3 assets.
    /// </summary>
    public class AssetsUpdate : INotifyPropertyChanged
    {
      private const string Address = "https://raw.githubusercontent.com/yumiris/SPV3/meta/update.hxe";

      private string _status = "Awaiting user input...";

      [XmlIgnore]
      public string Status
      {
        get => _status;
        set
        {
          if (value == _status) return;
          _status = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      ///   Conducts update on the filesystem using the current object state.
      /// </summary>
      public void Commit()
      {
        Status = "Updating SPV3 main assets ...";

        switch (Cli.Start($"-update \"{Address}\""))
        {
          case HXE.Exit.Code.Success:
            Status = "SPV3 update routine has gracefully succeeded.";
            break;
          case HXE.Exit.Code.Exception:
            Status = "Exception has occurred. Review log file.";
            break;
          case HXE.Exit.Code.InvalidInstall:
            Status = "Could not detect a legal HCE installation.";
            break;
        }

        Status = "Your SPV3 assets are up to date!";
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}