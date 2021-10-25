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

using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SPV3
{
  public partial class Information_UserControl : UserControl
  {
    public Information_UserControl()
    {
      InitializeComponent();

      ReadmeButton.Visibility    = File.Exists(Paths.Readme) ? Visibility.Visible : Visibility.Collapsed;
      ChangelogButton.Visibility = File.Exists(Paths.Changelog) ? Visibility.Visible : Visibility.Collapsed;
      CreditsButton.Visibility   = File.Exists(Paths.Credits) ? Visibility.Visible : Visibility.Collapsed;
      OptiGuideButton.Visibility = File.Exists(Paths.OptimizeGuide) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void Readme(object sender, MouseButtonEventArgs e)
    {
      Process.Start(Paths.Readme);
    }

    private void Changelog(object sender, MouseButtonEventArgs e)
    {
      Process.Start(Paths.Changelog);
    }

    private void Credits(object sender, MouseButtonEventArgs e)
    {
      Process.Start(Paths.Credits);
    }

    private void OptimizationGuide(object sender, MouseButtonEventArgs e)
    {
      Process.Start(Paths.OptimizeGuide);
    }
  }
}
