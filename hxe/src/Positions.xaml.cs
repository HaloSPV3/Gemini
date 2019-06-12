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
using System.Windows.Forms;
using static HXE.Console;
using MessageBox = System.Windows.MessageBox;

namespace HXE
{
  /// <summary>
  ///   Interaction logic for Positions.xaml
  /// </summary>
  public partial class Positions
  {
    private string _source;
    private string _target;

    public Positions()
    {
      InitializeComponent();
    }

    private void Save(object sender, RoutedEventArgs e)
    {
      Info("Saving weapon positions ...");

      var openSauce = (OpenSauce) _source;

      if (!openSauce.Exists())
      {
        MessageBox.Show("Source file does not exist.");
        return;
      }

      openSauce.Load();
      openSauce.Objects.Weapon.Save(_target);
      openSauce.Objects.Weapon.Load(_target);

      foreach (var position in openSauce.Objects.Weapon.Positions)
        Debug($"Weapon: {position.Name} | I/J/K: {position.Position.I}/{position.Position.J}/{position.Position.K}");

      Exit.WithCode(Exit.Code.Success);
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
      Exit.WithCode(Exit.Code.Success);
    }

    private void BrowseSource(object sender, RoutedEventArgs e)
    {
      using (var dialog = new OpenFileDialog())
      {
        dialog.DefaultExt = ".xml";
        dialog.Filter     = "XML files (*.xml)|*.xml";

        if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

        _source            = dialog.FileName;
        SourceTextBox.Text = _source;
      }
    }

    private void BrowseTarget(object sender, RoutedEventArgs e)
    {
      var dialog = new SaveFileDialog
      {
        DefaultExt = ".bin",
        Filter     = "BIN files (*.bin)|*.bin"
      };

      if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

      _target            = dialog.FileName;
      TargetTextBox.Text = _target;
    }
  }
}