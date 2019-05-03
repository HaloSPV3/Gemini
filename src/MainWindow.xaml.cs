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

using System.Windows;
using HXE;

namespace SPV3
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Load(object sender, RoutedEventArgs e)
    {
      var configuration = (Configuration) Paths.Files.Configuration;

      if (configuration.Exists())
      {
        configuration.Load();
        configuration.Kernel.EnableSpv3KernelMode = true;
        configuration.Save();
      }
      
      switch (Cli.Start())
      {
        case Exit.Code.Success:
          Status.Content = "SPV3 loading routine has gracefully succeeded.";
          break;
        case Exit.Code.Exception:
          Status.Content = "Exception has occurred. Review log file.";
          break;
      }
    }

    private void Settings(object sender, RoutedEventArgs e)
    {
      new SettingsWindow().Show();
    }

    private void Installer(object sender, RoutedEventArgs e)
    {
      new InstallerWindow().Show();
    }

    private void Compiler(object sender, RoutedEventArgs e)
    {
      new CompilerWindow().Show();
    }
  }
}