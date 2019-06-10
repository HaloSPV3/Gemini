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
using System.Threading;
using System.Threading.Tasks;
using HXE;
using SPV3.Annotations;
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;
using static System.IO.Path;
using static System.Reflection.Assembly;

namespace SPV3
{
  public class Compile : INotifyPropertyChanged
  {
    private bool   _canCompile = true;
    private string _status     = "Awaiting user input...";
    private string _target     = Combine(GetFolderPath(Personal), "My Games", "Halo SPV3 Compiled");

    public bool CanCompile
    {
      get => _canCompile;
      set
      {
        if (value == _canCompile) return;
        _canCompile = value;
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
      }
    }

    public void Commit()
    {
      try
      {
        CanCompile = false;

        /**
         * Back up the file which permits us to compile.
         *
         * After all, we wouldn't want our end-users compiling. :-)
         */

        var sourceCompileFile = Combine(CurrentDirectory,               Paths.Compile);
        var backupCompileFile = Combine(GetFolderPath(ApplicationData), Paths.Compile);

        if (Exists(backupCompileFile))
          Delete(backupCompileFile);

        if (Exists(sourceCompileFile))
          Move(sourceCompileFile, backupCompileFile);

        /**
         * And now... we lift off!
         */

        var task = new Task(() => { Compiler.Compile(CurrentDirectory, Combine(_target, "data")); });

        task.Start();

        var dots = 0;
        var body = "Compiling SPV3. Please wait until this is finished!";

        while (!task.IsCompleted)
        {
          Status = $"{body} {new string('.', dots)}";
          Thread.Sleep(1000);

          switch (dots)
          {
            case 0:
              dots = 1;
              break;
            case 1:
              dots = 2;
              break;
            case 2:
              dots = 3;
              break;
            case 3:
              dots = 0;
              break;
          }
        }

        /**
         * Copy data... 
         */

        Copy(GetExecutingAssembly().Location, Combine(Target, "spv3.exe"));
        Copy(Combine(CurrentDirectory,                        "hxe.exe"), Combine(Target, "hxe.exe"));

        /**
         * Restore the file which permits us to compile!
         */

        if (Exists(backupCompileFile))
          Move(backupCompileFile, sourceCompileFile);

        Status     = "Compilation has successfully finished!";
        CanCompile = true;
      }
      catch (Exception e)
      {
        Status     = e.Message;
        CanCompile = true;
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}