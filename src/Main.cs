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
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HXE;
using SPV3.Annotations;
using static System.DateTimeOffset;
using static System.Environment;
using static System.IO.Compression.ZipFile;
using Exit = HXE.Exit;
using File = System.IO.File;

namespace SPV3
{
  /// <summary>
  ///   Main loader code.
  /// </summary>
  public class Main : INotifyPropertyChanged
  {
    private bool   _canLoad;
    private string _status;

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

    public bool CanLoad
    {
      get => _canLoad;
      set
      {
        if (value == _canLoad) return;
        _canLoad = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    ///   Initialises the loader
    /// </summary>
    public async void Initialise()
    {
      /**
       * Gracefully create directories and configuration data.
       */

      Directory.CreateDirectory(Paths.Directories.Data);
      Directory.CreateDirectory(HXE.Paths.Directories.HXE);

      var configuration = (Configuration) HXE.Paths.Files.Configuration;

      if (!configuration.Exists())
        configuration.Save();

      /**
       * Test if the working directory is read-only. If a simple file cannot be written or deleted, then any loading or
       * updating routines will likely fail. As such, we will prevent updating or loading in such circumstances.
       */

      try
      {
        var test = Path.Combine(CurrentDirectory, "io.bin");

        File.WriteAllBytes(test, new byte[8]);
        File.Delete(test);

        CanLoad = true;
      }
      catch (Exception)
      {
        Status = "You are in a read-only folder. If you are running from the ISO file, please install SPV3 to a "    +
                 "normal folder outside of Program Files. Otherwise, try running as admin!\n\nYour current folder: " +
                 CurrentDirectory;

        CanLoad = false;
        return;
      }

      try
      {
        var update = new Update();

        Status = "Checking for latest updates...";
        await Task.Run(() => update.Load());

        CanLoad = false;
        Status  = "Applying any necessary updates. Please wait...";

        await Task.Run(() => update.Commit());

        CanLoad = true;
        Status  = "Your main SPV3 data is up to date!";
      }
      catch (Exception e)
      {
        CanLoad = true;
        Status  = "Update error -- " + e.Message + " -- any old files have been restored!";
      }
    }

    /// <summary>
    ///   Invokes SPV3 through the HXE loader.
    /// </summary>
    public void Start()
    {
      var configuration = (Configuration) HXE.Paths.Files.Configuration;

      if (configuration.Exists())
      {
        configuration.Load();
        configuration.Kernel.EnableSpv3KernelMode = true;
        configuration.Save();
      }

      switch (Cli.Start())
      {
        case Exit.Code.Success:
          Status = "SPV3 loading routine has gracefully succeeded.";
          break;
        case Exit.Code.Exception:
          Status = "Exception has occurred. Review log file.";
          break;
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    ///   Update module for HCE/SPV3 assets.
    /// </summary>
    public class Update
    {
      private const string Address = "https://raw.githubusercontent.com/yumiris/SPV3/meta/update.xml";

      public string      Name        { get; set; } = string.Empty;               /* name of the update */
      public string      Description { get; set; } = string.Empty;               /* update description */
      public long        Time        { get; set; } = UtcNow.ToUnixTimeSeconds(); /* manifest timestamp */
      public List<Entry> Entries     { get; set; } = new List<Entry>();          /* files for updating */

      /// <summary>
      ///   Updates object state using server-side metadata.
      /// </summary>
      public void Load()
      {
        using (var response = (HttpWebResponse) WebRequest.Create(Address).GetResponse())
        using (var stream = response.GetResponseStream())
        {
          var update = (Update) new XmlSerializer(typeof(Update))
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
        foreach (var entry in Entries)
        {
          var file   = Path.Combine(CurrentDirectory, entry.Path ?? string.Empty, entry.Name);
          var backup = file + ".bak";

          /**
           * If the file exists AND is the same length as the value declared in the current entry, then it's safe to
           * assume that the end-user has the updated file.
           */

          if (File.Exists(file))
          {
            if (new FileInfo(file).Length != entry.Size)
            {
              if (!File.Exists(backup)) File.Move(file, backup);
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
            if (entry.Path != null)
              Directory.CreateDirectory(Path.Combine(CurrentDirectory, entry.Path));

            using (var client = new WebClient())
            {
              var data = Path.Combine(CurrentDirectory, Guid.NewGuid().ToString());
              var temp = Path.Combine(CurrentDirectory, Guid.NewGuid().ToString());

              client.DownloadFile(entry.URL, data);

              ExtractToDirectory(data, temp);

              File.Move(Path.Combine(temp, entry.Name), file);
              File.Delete(data);
              File.Delete(temp);
            }

            if (File.Exists(backup))
              File.Delete(backup);
          }
          catch (Exception)
          {
            if (!File.Exists(backup)) throw;

            if (File.Exists(file)) File.Delete(file);
            File.Move(backup, file);

            throw;
          }
        }
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