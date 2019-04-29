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
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;
using static System.IO.Path;

namespace SPV3
{
  public partial class UpdateUserControl : UserControl
  {
    private const string Header  = "HEADER.txt";
    private const string Address = "https://dist.n2.network/spv3/";

    private int _version;

    public UpdateUserControl()
    {
      InitializeComponent();
      InitialiseUpdate();
      DownloadButton.Visibility = Visibility.Collapsed;
    }

    private async void InitialiseUpdate()
    {
      await Task.Run(() =>
      {
        try
        {
          var request = (HttpWebRequest) WebRequest.Create(Address + Header);
          using (var response = (HttpWebResponse) request.GetResponse())
          using (var stream = response.GetResponseStream())
          using (var reader = new StreamReader(stream ?? throw new Exception("Could not get response stream.")))
          {
            var serverVersion = int.Parse(reader.ReadLine()?.TrimEnd()
                                          ?? throw new Exception("Could not infer server-side version."));

            var clientVersion = Assembly.GetEntryAssembly().GetName().Version.Major;

            if (serverVersion <= clientVersion) return;

            _version = serverVersion;
          }
        }
        catch (Exception e)
        {
          var log = Combine(GetFolderPath(ApplicationData), "SPV3", "exception.log");

          WriteAllText(log, e.ToString());
        }
      });

      if (_version <= 0) return;

      DownloadButton.Content    = $"Update available (build-{_version:D4}). Click to download!";
      DownloadButton.Visibility = Visibility.Visible;
    }

    private void Download(object sender, RoutedEventArgs e)
    {
      Process.Start($"https://dist.n2.network/spv3/{_version:D4}/bin.zip");
    }
  }
}