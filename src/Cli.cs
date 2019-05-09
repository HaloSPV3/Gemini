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
using System.Diagnostics;
using System.IO;
using HXE;
using HXE.HCE;
using static System.Windows.Forms.Screen;

namespace SPV3
{
  /// <summary>
  ///   Helper class for invoking the CLI executable.
  /// </summary>
  public static class Cli
  {
    private const string Executable = "hxe.exe";

    public static Exit.Code Start(string args)
    {
      var width  = (ushort) PrimaryScreen.Bounds.Width;
      var height = (ushort) PrimaryScreen.Bounds.Height;

      var lastProfile = (LastProfile) Paths.Files.LastProfile;

      if (lastProfile.Exists())
      {
        lastProfile.Load();

        var profile = (Profile) Path.Combine(Paths.Directories.Profiles, lastProfile.Profile);

        if (profile.Exists())
        {
          profile.Load();

          width  = profile.Video.Resolution.Width;
          height = profile.Video.Resolution.Height;
        }
      }

      args = args + $" -path \"{Paths.Directories.Data}\" -console -devmode -screenshot -vidmode {width},{height}";
      var process = Process.Start(Executable, args);

      if (process == null)
        throw new NullReferenceException("Could not construct CLI process.");

      process.WaitForExit();
      return (Exit.Code) process.ExitCode;
    }
  }
}