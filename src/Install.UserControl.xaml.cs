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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace SPV3
{
  public partial class Install_UserControl : UserControl
  {
    private readonly Install _install;

    public Install_UserControl()
    {
      InitializeComponent();
      _install = (Install) DataContext;
      _install.Initialise();
    }

    public event EventHandler Home;

    private async void Install(object sender, RoutedEventArgs e)
    {
      InstallButton.Content = "Installing...";
      await Task.Run(() => _install.Commit());
      InstallButton.Content = "Install";
    }

    private void Back(object sender, RoutedEventArgs e)
    {
      Home?.Invoke(sender, e);
    }

    private void Browse(object sender, RoutedEventArgs e)
    {
      using (var dialog = new FolderBrowserDialog())
      {
        if (dialog.ShowDialog() == DialogResult.OK)
          _install.Target = dialog.SelectedPath;
      }
    }

    private void BrowseSteam(object sender, RoutedEventArgs e)
    {
      using (var dialog = new OpenFileDialog())
      {
        if (dialog.ShowDialog() == DialogResult.OK)
          _install.SteamEXEPath = dialog.FileName;
      }
    }

    private void InstallHce(object sender, RoutedEventArgs e)
    {
      _install.InstallHce();
    }

    private void InvokeSpv3(object sender, RoutedEventArgs e)
    {
      _install.InvokeSpv3();
    }

    private void VerifyHce(object sender, RoutedEventArgs e)
    {
      _install.Initialise();
    }

    private void Quit(object sender, RoutedEventArgs e)
    {
      Environment.Exit(0);
    }
  }
}