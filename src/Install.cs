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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using HXE;
using SPV3.Annotations;
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;

namespace SPV3
{
  public class Install : INotifyPropertyChanged
  {
    private readonly string _source = Path.Combine(CurrentDirectory, "data");
    private          bool   _canInstall;
    private          string _status = "Awaiting user input...";
    private          string _target = Path.Combine(GetFolderPath(Personal), "My Games", "Halo SPV3");

    public bool CanInstall
    {
      get => _canInstall;
      set
      {
        if (value == _canInstall) return;
        _canInstall = value;
        OnPropertyChanged();
      }
    }

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

    public string Target
    {
      get => _target;
      set
      {
        if (value == _target) return;
        _target = value;
        OnPropertyChanged();

        /**
         * Check validity of the specified target value.
         */

        try
        {
          var exists = Directory.Exists(Target);

          if (!Directory.Exists(Target))
            Directory.CreateDirectory(Target);

          var test = Path.Combine(Target, "io.bin");
          WriteAllBytes(test, new byte[8]);
          Delete(test);

          if (!exists)
            Directory.Delete(Target);

          Status     = "Waiting for user to install SPV3.";
          CanInstall = true;
        }
        catch (Exception e)
        {
          Status     = "Installation not possible at selected path: " + e.Message.ToLower();
          CanInstall = false;
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Initialise()
    {
      /**
       * Determine if the current environment fulfils the installation requirements.
       */

      var manifest = (Manifest) Path.Combine(_source, HXE.Paths.Manifest);

      if (manifest.Exists())
      {
        Status     = "Waiting for user to install SPV3.";
        CanInstall = true;
        return;
      }

      Status     = "Could not find manifest in the data directory.";
      CanInstall = false;
    }

    public async void Commit()
    {
      try
      {
        CanInstall = false;

        var progress = new Progress<Status>();
        progress.ProgressChanged +=
          (o, s) => Status =
            $"Installing SPV3. Please wait until this is finished! - {(decimal) s.Current / s.Total:P}";

        await Task.Run(() => { Installer.Install(_source, _target, progress); });

        Status     = "Installation has successfully finished!";
        CanInstall = true;

        MessageBox.Show(
          "Installation has been successful! " +
          "Please install OpenSauce to the SPV3 folder using AmaiSosu. Click OK to continue ...");

        new AmaiSosu {Path = Path.Combine(Target, Paths.AmaiSosu)}.Execute();
      }
      catch (Exception e)
      {
        Status     = e.Message;
        CanInstall = true;
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}