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
using System.Windows;
using System.Windows.Forms;
using static System.Diagnostics.Process;
using static System.Environment;
using static System.IO.File;
using static System.IO.Path;
using Exit = HXE.Exit;
using File = HXE.File;

namespace SPV3
{
  public partial class CompilerWindow : Window
  {
    public CompilerWindow()
    {
      InitializeComponent();
      Target.Text = Combine(GetFolderPath(SpecialFolder.Personal), "SPV3.Compile");
    }

    private void BrowseTarget(object sender, RoutedEventArgs e)
    {
      using (var dialog = new FolderBrowserDialog())
      {
        dialog.ShowDialog();
        Target.Text = dialog.SelectedPath;
      }
    }

    private void Compile(object sender, RoutedEventArgs e)
    {
      Status.Content = "Installing SPV3 ...";

      var process = Start(new ProcessStartInfo
      {
        FileName  = Combine(CurrentDirectory, "hxe.exe"),
        Arguments = $"/install \"{Target.Text}\""
      });

      if (process == null)
        throw new NullReferenceException("Could not construct CLI process.");

      process.WaitForExit();
      var exit = (Exit.Code) process.ExitCode;

      switch (exit)
      {
        case Exit.Code.Success:
          var target = Combine(Target.Text, "data");

          Copy(Combine(target, "hxe.exe"), Combine(Target.Text, "hxe.exe"));

          var cli = (File) GetCurrentProcess().MainModule.FileName;
          cli.CopyTo(Target.Text);

          var mkisofs = Combine(CurrentDirectory, "mkisofs.exe");

          if (Exists(mkisofs))
          {
            var baseDir = GetDirectoryName(Target.Text) ??
                          throw new DirectoryNotFoundException("Base directory not found.");

            var isoFile = Combine(baseDir, "SPV3.iso");

            var command = $"-udf -V \"SPV3\" -o \"{isoFile}\" \"{Target.Text}\"";

            Start(mkisofs, command);
          }

          Status.Content = "SPV3 compilation routine has gracefully succeeded.";
          break;
        case Exit.Code.Exception:
          Status.Content = "Exception has occurred. Review log file.";
          break;
        case Exit.Code.InvalidInstall:
          Status.Content = "Could not detect a legal HCE installation.";
          break;
      }
    }
  }
}