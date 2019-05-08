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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using SPV3.Annotations;

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
      Loader.Load();
      Assets.Load();
    }

    /// <summary>
    ///   Update module for the SPV3 loader.
    /// </summary>
    public class LoaderUpdate : INotifyPropertyChanged
    {
      private const string Header  = "HEADER.txt";
      private const string Address = "https://dist.n2.network/spv3/";

      private bool _available;

      private int _version;

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
        var request = (HttpWebRequest) WebRequest.Create(Address + Header);
        using (var response = (HttpWebResponse) request.GetResponse())
        using (var stream = response.GetResponseStream())
        using (var reader = new StreamReader(stream ?? throw new Exception("Could not get response stream.")))
        {
          var serverVersion = int.Parse(reader.ReadLine()?.TrimEnd()
                                        ?? throw new Exception("Could not infer server-side version."));

          var clientVersion = Assembly.GetEntryAssembly().GetName().Version.Major;

          Version   = serverVersion;
          Available = serverVersion > clientVersion;
        }
      }

      public void Commit()
      {
        Process.Start(Address + $"/{Version:D4}.iso");
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
      private const string Address = "https://raw.githubusercontent.com/yumiris/SPV3/meta/update.xml";

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

      public string      Name        { get; set; } = string.Empty;                              /* name of the update */
      public string      Description { get; set; } = string.Empty;                              /* update description */
      public long        Time        { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); /* manifest timestamp */
      public List<Entry> Entries     { get; set; } = new List<Entry>();                         /* files for updating */

      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      ///   Updates object state using server-side metadata.
      /// </summary>
      public void Load()
      {
        using (var response = (HttpWebResponse) WebRequest.Create(Address).GetResponse())
        using (var stream = response.GetResponseStream())
        {
          var update = (AssetsUpdate) new XmlSerializer(typeof(AssetsUpdate))
            .Deserialize(stream ?? throw new Exception("Could not get response stream."));

          Name        = update.Name;
          Description = update.Description;
          Time        = update.Time;
          Entries     = update.Entries;
        }
      }

      /// <summary>
      ///   Conducts update on the filesystem using the current object state.
      /// </summary>
      public void Commit()
      {
        Status = "Updating SPV3 main assets ...";

        foreach (var entry in Entries)
        {
          Status = "Updating SPV3 main asset - " + entry.Name;

          var target = Path.Combine(Environment.CurrentDirectory, entry.Path ?? string.Empty, entry.Name);
          var backup = target + ".bak";

          /**
           * If the file exists AND is the same length as the value declared in the current entry, then it's safe to
           * assume that the end-user has the updated file.
           */

          if (File.Exists(target))
          {
            if (new FileInfo(target).Length != entry.Size)
            {
              if (!File.Exists(backup))
              {
                Status = "Backing up current asset - " + entry.Name;
                File.Move(target, backup);
              }
            }
            else
            {
              continue;
            }
          }

          /**
           * We will gracefully create the relevant directories and download the file. On connection/download failure,
           * the backed up file will be restored.
           */

          try
          {
            using (var client = new WebClient())
            {
              Status = "Preparing to update asset - " + entry.Name;

              var temp = Path.Combine(Environment.CurrentDirectory,
                Guid.NewGuid().ToString());                             /* temporary directory */
              var pack = Path.Combine(temp, Guid.NewGuid().ToString()); /* compressed package  */
              var file = Path.Combine(temp, entry.Name);                /* package entry file  */

              Directory.CreateDirectory(temp);

              Status = "Downloading asset - " + entry.Name;
              client.DownloadFile(entry.URL, pack);

              Status = "Extracting asset - " + entry.Name;
              ZipFile.ExtractToDirectory(pack, temp);

              Status = "Installing asset - " + entry.Name;
              if (entry.Path != null)
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, entry.Path));

              File.Move(file, target);
              Directory.Delete(temp, true);
            }

            if (File.Exists(backup))
              File.Delete(backup);
          }
          catch (Exception)
          {
            if (!File.Exists(backup)) throw;

            if (File.Exists(target)) File.Delete(target);
            File.Move(backup, target);

            throw;
          }
        }

        Status = "Your SPV3 assets are up to date!";
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      /// <summary>
      ///   Update entry object.
      /// </summary>
      public class Entry
      {
        public string URL  { get; set; } = string.Empty; /* http path for downloading the binary to the file system  */
        public string Name { get; set; } = string.Empty; /* expected name for the binary once it has been downloaded */
        public string Path { get; set; } = string.Empty; /* path relative to the working folder for storing the file */
        public string Hash { get; set; } = string.Empty; /* md5 hash of the file for potential checksum verification */
        public long   Size { get; set; } = 0;            /* byte size of the binary on for length-based verification */
      }
    }
  }
}