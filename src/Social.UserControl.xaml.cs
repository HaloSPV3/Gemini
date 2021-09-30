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
using System.Windows.Controls;
using System.Windows.Input;

namespace SPV3
{
  public partial class Social_UserControl : UserControl
  {
    public Social_UserControl()
    {
      InitializeComponent();
    }

    private ProcessStartInfo _getStartInfo(string uri)
    {
      return new ProcessStartInfo(uri) { UseShellExecute = true };
    }

    private void Reddit(object sender, MouseButtonEventArgs e)
    {
      Process.Start(_getStartInfo("https://www.reddit.com/r/halospv3"));
    }

    private void Twitter(object sender, MouseButtonEventArgs e)
    {
      Process.Start(_getStartInfo("https://twitter.com/halo_spv3"));
    }

    private void Discord(object sender, MouseButtonEventArgs e)
    {
      Process.Start(_getStartInfo("https://discord.gg/q4f7nTt"));
    }

    private void Wikia(object sender, MouseButtonEventArgs e)
    {
      Process.Start(_getStartInfo("https://halo-spv3.fandom.com"));
    }
  }
}
