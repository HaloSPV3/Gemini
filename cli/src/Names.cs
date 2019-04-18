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

using System.IO;
using static System.Environment;
using static System.Environment.SpecialFolder;

namespace SPV3.CLI
{
  /// <summary>
  ///   Lists all of the files & directories on the filesystem that SPV3 deals with.
  /// </summary>
  public static class Names
  {
    /// <summary>
    ///   Files on the filesystem that SPV3 reads/writes.
    /// </summary>
    public static class Files
    {
      public const string Executable  = "haloce.exe";
      public const string Initiation  = "initc.txt";
      public const string Progress    = "savegame.bin";
      public const string Profile     = "blam.sav";
      public const string InstallPath = "install.txt";
      public const string Manifest    = "0x00.bin";

      public static readonly string LastProfile = Path.Combine(GetFolderPath(Personal),
        Directories.Games,
        Directories.Halo,
        "lastprof.txt"
      );

      public static readonly string OpenSauce = Path.Combine(GetFolderPath(Personal),
        Directories.Games,
        Directories.Halo,
        Directories.OpenSauce,
        "OS_Settings.User.xml"
      );

      public static readonly string PostProcessing = Path.Combine(GetFolderPath(ApplicationData),
        Directories.Data,
        "postprocessing.bin"
      );

      public static readonly string UpdateVersion = Path.Combine(GetFolderPath(ApplicationData),
        Directories.Data,
        "updateversion.bin");

      public static readonly string Kernel = Path.Combine(GetFolderPath(ApplicationData),
        Directories.Data,
        "kernel.bin");
    }

    /// <summary>
    ///   Directories on the filesystem that SPV3 accesses.
    /// </summary>
    public static class Directories
    {
      public const string Games     = "My Games";
      public const string Halo      = "Halo CE";
      public const string Data      = "SPV3";
      public const string OpenSauce = "OpenSauce";

      public static readonly string Profiles = Path.Combine(GetFolderPath(Personal),
        Games,
        Halo,
        "savegames"
      );
    }
  }
}