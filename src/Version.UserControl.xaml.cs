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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SPV3
{
  public partial class Version_UserControl : UserControl
  {
    private readonly Version _version;

    public Version_UserControl()
    {
      InitializeComponent();
      _version = (Version) DataContext;
      _version.Initialise();
    }

    public event EventHandler Update;

    private void Assembly(object sender, MouseButtonEventArgs e)
    {
      Process.Start(_version.Assembly.Address);
    }

    private async void Upstream(object sender, MouseButtonEventArgs e)
    {
      Update?.Invoke(sender, e);

      await Task.Run(() =>
      {
        Thread.Sleep(2500);
        Process.Start(_version.Upstream.Address);
      });
    }
  }
}