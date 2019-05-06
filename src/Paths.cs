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

using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.Path;

namespace SPV3
{
  /// <summary>
  ///   Paths for the files and directories used by the SPV3 loader.
  /// </summary>
  public static class Paths
  {
    public static class Directories
    {
      public static readonly string Data      = Combine(GetFolderPath(ApplicationData), "SPV3");
      public static readonly string OpenSauce = Combine(Data,                           "OpenSauce");
    }

    public static class Files
    {
      public static readonly string OpenSauce = Combine(Directories.OpenSauce, "OS_Settings.User.xml");
    }
  }
}