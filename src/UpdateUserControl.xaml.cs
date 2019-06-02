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

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.MessageBox;
using static System.Windows.MessageBoxButton;

namespace SPV3
{
  public partial class UpdateUserControl : UserControl
  {
    private readonly Update _update;

    public UpdateUserControl()
    {
      InitializeComponent();
      _update = (Update) DataContext;
      UpdateLoader();
    }

    private void UpdateAssets(object sender, RoutedEventArgs e)
    {
      _update.Assets.Commit();
    }

    private async void UpdateLoader()
    {
      await Task.Run(() => { _update.Initialise(); });

      if (!_update.Loader.Available) return;

      var update = Show($"SPV3 Loader update is available: build-{_update.Loader.Version:D4}\n\n" +
                        "Would you like to download the update?", "Update", YesNo);

      if (update == MessageBoxResult.Yes)
        _update.Loader.Commit();
    }
  }
}