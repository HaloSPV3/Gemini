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

using System.Windows.Forms;
using static System.IO.Directory;
using static System.IO.File;
using static System.IO.SearchOption;
using static HXE.Console;

namespace HXE
{
  /// <summary>
  ///   Object representing a Placeholder bitmap on the filesystem.
  /// </summary>
  public static class Placeholder
  {
    /// <summary>
    ///   Extension used for backed up bitmaps.
    /// </summary>
    private const string Extension = ".original";

    /// <summary>
    ///   Replaces the filtered bitmaps in the target directory with the placeholder bitmap.
    /// </summary>
    /// <param name="placeholder">
    ///   Placeholder bitmap to replace normal bitmaps with.
    /// </param>
    /// <param name="directory">
    ///   Target directory containing the files which should be replaced with the placeholder.
    /// </param>
    /// <param name="filter">
    ///   Optional filter for the bitmaps which should be handled in the target directory.
    /// </param>
    public static void Commit(string placeholder, string directory, string filter = null)
    {
      filter = filter ?? string.Empty;

      Info("Retrieving list of all files matching the inbound criteria");

      Debug("Placeholder - " + placeholder);
      Debug("Directory   - " + directory);
      Debug("Filter      - " + filter);

      var files = GetFiles(directory, $"*{filter}*.bitmap", AllDirectories);

      Info("Proceeding to replace original bitmaps with placeholders");

      foreach (var source in files)
      {
        var target = source + Extension;

        if (System.IO.File.Exists(target))
        {
          Info("Skipping file as backup already exists - " + source);
          continue;
        }

        Info("Backing up bitmap and replacing it with target - " + source);

        System.IO.File.Move(source, target);
        Copy(placeholder, source);
      }
    }

    /// <summary>
    ///   Restores bitmaps based on the provided records file.
    /// </summary>
    /// <param name="directory">
    ///   Records text file created by the commit method.
    /// </param>
    public static void Revert(string directory)
    {
      Info("Retrieving list of bitmaps from provided directory - " + directory);
      var files = GetFiles(directory, "*.bitmap" + Extension, AllDirectories);

      Info("Proceeding to restore original bitmaps from their backups");

      foreach (var source in files)
      {
        var target = source.Remove(source.Length - Extension.Length, Extension.Length);

        if (System.IO.File.Exists(target))
        {
          Info("Removing what's assumed to be placeholder file - " + target);
          System.IO.File.Delete(target);
        }

        Info("Restoring bitmap to original path - " + target);
        System.IO.File.Move(source, target);
      }
    }
  }
}