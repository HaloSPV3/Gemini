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
using System.Runtime.CompilerServices;
using HXE;
using HXE.SPV3;
using SPV3.Annotations;
using static System.Environment;
using static System.IO.Directory;
using static System.IO.File;
using static System.IO.Path;
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
    private string _status = "Awaiting user input ...";

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
    public void Initialise()
    {
      /**
       * Gracefully create directories and configuration data.
       */

      CreateDirectory(HXE.Paths.Directory);
      CreateDirectory(Paths.Directory);

      var configuration = (Configuration) HXE.Paths.Configuration;

      if (!configuration.Exists())
        configuration.Save();

      /**
       * Test if the working directory is read-only. If a simple file cannot be written or deleted, then any loading or
       * updating routines will likely fail. As such, we will prevent updating or loading in such circumstances.
       */

      try
      {
        var test = Combine(CurrentDirectory, "io.bin");

        WriteAllBytes(test, new byte[8]);
        File.Delete(test);

        CanLoad = true;
      }
      catch (Exception)
      {
        Status = "You are in a read-only folder. If you are running from the ISO file, please install SPV3 to a "    +
                 "normal folder outside of Program Files. Otherwise, try running as admin!\n\nYour current folder: " +
                 CurrentDirectory;

        CanLoad = false;
      }
    }

    /// <summary>
    ///   Invokes SPV3 through the HXE loader.
    /// </summary>
    public void Start(string args = null)
    {
      var configuration = (Configuration) HXE.Paths.Configuration;
      var openSauce     = (OpenSauce) HXE.Paths.Custom.OpenSauce(Paths.Directory);
      var chimera       = (Chimera) HXE.Paths.Custom.Chimera(Paths.Directory);

      if (configuration.Exists())
        configuration.Load();

      if (openSauce.Exists())
        openSauce.Load();
      else
        openSauce.Camera.CalculateFOV(); /* imposes initial fov  */

      if (chimera.Exists())
      {
        chimera.Load();
        chimera.Interpolation        = 9;    /* enhancements */
        chimera.AnisotropicFiltering = true; /* enhancements */
        chimera.UncapCinematic       = true; /* enhancements */
        chimera.BlockLOD             = true; /* enhancements */
      }

      configuration.Kernel.EnableSpv3KernelMode = true; /* hxe spv3 compatibility */
      openSauce.HUD.ScaleHUD                    = true; /* fixes menu stretching  */
      openSauce.HUD.ShowHUD                     = true; /* fixes menu stretching  */

      configuration.Save();
      openSauce.Save();

      Status = "Installing SPV3 ...";

      switch (Cli.Start(args))
      {
        case Exit.Code.Success:
          Status = "SPV3 loading routine has gracefully succeeded.";
          break;
        case Exit.Code.Exception:
          Status = "Exception has occurred. Review log file.";
          break;
        case Exit.Code.InvalidInstall:
          Status = "Could not detect a legal HCE installation.";
          break;
      }
    }

    public void StartWindow()
    {
      Start("-window");
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}