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
using System.Threading.Tasks;

namespace SPV3
{
  public partial class Version
  {
    public VersionAssembly Assembly { get; set; } = new VersionAssembly();
    public VersionUpstream Upstream { get; set; } = new VersionUpstream();

    public void Initialise()
    {
      Task.Run(() => { Assembly.Initialise(); });

      if (Context.Infer() == Context.Type.Load)
        Task.Run(() => { Upstream.Initialise(); });
    }

    public void Changelog()
    {
      var startInfo = new ProcessStartInfo()
      {
          FileName = @"https://github.com/HaloSPV3/SPV3-Loader/releases/" +
                    $"v{Assembly.Version.ToString(3)}",
          UseShellExecute = true
      };
      Process.Start(startInfo);
    }
  }
}
