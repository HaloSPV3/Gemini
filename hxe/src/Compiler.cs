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
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using HXE.Properties;
using static System.Diagnostics.Process;
using static System.IO.Compression.CompressionLevel;
using static System.IO.Compression.ZipArchiveMode;
using static System.IO.Compression.ZipFile;
using static HXE.Console;

namespace HXE
{
  /// <summary>
  ///   Conducts core and data compilation to manifest and compressed packages.
  /// </summary>
  public static class Compiler
  {
    /// <summary>
    ///   Compiles the data in the source directory to packages and a manifest in the target directory.
    /// </summary>
    /// <param name="source">
    ///   Source directory, expected to contain HCE data such as the executables and maps.
    /// </param>
    /// <param name="target">
    ///   Target directory, which is expected to be distributed to the end-user.
    /// </param>
    public static void Compile(string source, string target)
    {
      /**
       * Normalisation of the paths will preserve our sanity later on! ;-)
       */

      source = Path.GetFullPath(source); /* normalise source path */
      target = Path.GetFullPath(target); /* normalise target path */

      Info("Normalised inbound source and target paths");

      if (!Directory.Exists(source))
        throw new DirectoryNotFoundException("Source directory does not exist.");

      Info("Source directory exists");

      if (!Directory.Exists(target))
        Directory.CreateDirectory(target);

      Info("Gracefully created target directory");

      /**
       * We declare the manifest file to be in the target directory, because the target directory is expected to be the
       * distributed with the installer.
       * 
       * We recursively retrieve a list of files in the source directory, for the purpose of creating packages and
       * manifest entries for each of them.
       * 
       * Given that the manifest is represented by 0x00, the subsequent packages should be represented by a >=1 ID. 
       */

      var manifest = (Manifest) Path.Combine(target, Paths.Files.Manifest);
      var files    = new DirectoryInfo(source).GetFiles("*", SearchOption.AllDirectories);
      var i        = 0x01; /* 0x00 = manifest */

      Info("Retrieved list of files - creating packages");

      /**
       * For any file found in the source, we create a DEFLATE package for it . We also declare an entry for it in the
       * manifest. This permits permits both decompression and verification.
       * 
       * The manifest information seeks to be compatible and portable, by recording only relative paths to the source
       * directory.
       *
       * Each package represents a file in the source directory; hence, we must declare an entry for the respective
       * directory's path relative to the source.
       *
       * This effectively permits two scenarios:
       *
       * -   the INSTALLER can lay out the directories when installing the packages, by combining the particular
       *     target path with the relative path declared for the package.
       * -   the LOADER can infer the absolute path of a file and then match its size against the one declared in the
       *     manifest, as part of its asset verification routine.
       */

      foreach (var file in files)
      {
        var packageName = $"0x{i:X8}.bin";
        var packagePath = Path.Combine(target, packageName);
        var fileName    = file.Name;

        using (var deflate = Open(packagePath, Create))
        {
          var task = new Task(() => { deflate.CreateEntryFromFile(file.FullName, fileName, Optimal); });

          task.Start();
          Info("Started package inflation - " + packageName + " - " + fileName);

          while (!task.IsCompleted)
          {
            Wait(Resources.Progress);
            Thread.Sleep(1000);
          }

          Info("Successfully finished package deflation");
        }

        /**
         * We record the package's name on the filesystem to the manifest. This permits the INSTALLER to seek the
         * package within the source directory, and then extract its data to the target directory.
         *
         * The packages are conventionally identified by the hexadecimal equivalent of the inbound integer, with ".bin"
         * as the prefix, just like the manifest file.
         */

        manifest.Packages.Add(new Manifest.Package
        {
          Name = packageName,
          Size = new FileInfo(packagePath).Length,
          Entry = new Manifest.Package.PackageEntry
          {
            Name = fileName,
            Size = file.Length,

            /**
             * We MUST declare the RELATIVE path as the value, by inferring the relative path from the absolute path
             * which belongs to the source directory.
             *
             * X:\Development\HCE                  - source absolute path
             * X:\Development\HCE\content\Gallery  - current directory path
             *                   |---------------| - relative path to source
             * 
             * With the above diagram in mind, we can essentially remove the source path portion (and trailing slash)
             * from the full absolute path, and thus declare the relative path as the package's path.
             *
             * NOTE: The aforementioned procedure is not carried out if this iteration's directory is the source
             * directory itself.
             */

            Path = file.DirectoryName.Equals(source)
              ? string.Empty
              : file.DirectoryName.Substring(source.Length + 1)
          }
        });

        Info("Successfully added package entry to the manifest");

        i++;
      }

      if (!manifest.Exists())
        manifest.Delete();

      manifest.Save();

      Info("Serialised manifest to the filesystem - " + manifest.Path);

      /**
       * For subsequent installation convenience, we will make a copy of the current CLI to the target directory.
       */

      var cli = (File) GetCurrentProcess().MainModule?.FileName;
      cli.CopyTo(target);

      Info("Copied current HXE assembly to the target directory");
      Done("Compilation routine has been successfully completed");
    }
  }
}