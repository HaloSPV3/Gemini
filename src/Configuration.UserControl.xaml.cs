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
using System.Windows;
using System.Windows.Controls;

namespace SPV3
{
  public partial class Configuration_UserControl : UserControl
  {
    private readonly Configuration _configuration;

    public Configuration_UserControl()
    {
      InitializeComponent();
      _configuration = (Configuration) DataContext;
      _configuration.Load();
    }

    public event EventHandler Home;

    private void Save(object sender, RoutedEventArgs e)
    {
      _configuration.Save();
      Home?.Invoke(sender, e);
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
      Home?.Invoke(sender, e);
    }

    private void CalculateFOV(object sender, RoutedEventArgs e)
    {
      _configuration.OpenSauce.CalculateFOV();
    }

    private void ApplyDOOM(object sender, RoutedEventArgs e)
    {
      _configuration.OpenSauce.ApplyDOOM();
      ApplyDOOMButton.Content   = "Done!";
      ApplyDOOMButton.IsEnabled = false;
    }
  }
}