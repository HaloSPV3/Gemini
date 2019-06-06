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
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using HXE.HCE;
using static System.Console;
using static System.Environment;
using static System.IO.File;
using static HXE.Console;
using static HXE.Exit;
using static HXE.Properties.Resources;

namespace HXE
{
  /// <summary>
  ///   HXE Program.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    ///   HXE entry.
    /// </summary>
    /// <param name="args">
    ///   Arguments for HXE.
    /// </param>
    [STAThread]
    public static void Main(string[] args)
    {
      DisplayBanner();     /* impress our users */
      InvokeProgram(args); /* burn baby burn */
    }

    /// <summary>
    ///   Console API to the HXE kernel, installer and compiler.
    /// </summary>
    /// <param name="args">
    ///   --config            Opens configuration GUI
    ///   --load              Initiates HCE/SPV3
    ///   --install=VALUE     Installs HCE/SPV3   to destination
    ///   --compile=VALUE     Compiles HCE/SPV3   to destination
    ///   --console           Loads HCE           with console    mode
    ///   --devmode           Loads HCE           with developer  mode
    ///   --screenshot        Loads HCE           with screenshot ability
    ///   --window            Loads HCE in window mode
    ///   --nogamma           Loads HCE           without gamma overriding
    ///   --adapter=VALUE     Loads HCE           on monitor    X
    ///   --path=VALUE        Loads HCE           with custom   profile path
    ///   --vidmode=VALUE     Loads HCE           with video    mode
    /// </param>
    private static void InvokeProgram(string[] args)
    {
      Directory.CreateDirectory(Paths.Directory);

      var hce = new Executable();

      /**
       * Implicit verification for legal HCE installations.
       */

      try
      {
        hce = Executable.Detect();
      }
      catch (Exception e)
      {
        Error(e.Message + " -- Legal copy of HCE needs to be installed for loading!");
      }

      var options = new OptionSet()
        .Add("config", "Opens configuration GUI",
          s =>
          {
            new Application().Run(new Settings());
            Exit(0);
          }
        )
        .Add("load", "Initiates HCE/SPV3",
          s => Run(() => { Kernel.Bootstrap(hce); }))
        .Add("install=", "Installs HCE/SPV3 to destination",
          s => Run(() => { Installer.Install(CurrentDirectory, s); }))
        .Add("compile=", "Compiles HCE/SPV3 to destination",
          s => Run(() => { Compiler.Compile(CurrentDirectory, s); }))
        .Add("update=", "Updates directory using manifest",
          s => Run(() =>
          {
            var update = new Update();
            update.Import(s);
            update.Commit();
          }))
        .Add("console", "Loads HCE with console mode",
          s => hce.Debug.Console = true)
        .Add("devmode", "Loads HCE with developer mode",
          s => hce.Debug.Developer = true)
        .Add("screenshot", "Loads HCE with screenshot ability",
          s => hce.Debug.Screenshot = true)
        .Add("window", "Loads HCE in window mode",
          s => hce.Video.Window = true)
        .Add("nogamma", "Loads HCE without gamma overriding",
          s => hce.Video.NoGamma = true)
        .Add("adapter=", "Loads HCE on monitor X",
          s => hce.Video.Adapter = ushort.Parse(s))
        .Add("path=", "Loads HCE with custom profile path",
          s => hce.Profile.Path = s)
        .Add("vidmode=", "Loads HCE with video mode",
          s =>
          {
            var a = s.Split(',');

            if (a.Length < 2) return;

            hce.Video.Width  = ushort.Parse(a[0]);
            hce.Video.Height = ushort.Parse(a[1]);

            if (a.Length > 2) /* optional refresh rate */
              hce.Video.Refresh = ushort.Parse(a[2]);
          });

      var input = options.Parse(args);

      /**
       * Implicitly invoke the HXE kernel when no install/compile/load/update command is passed.
       */

      if (!input.Contains("load")    &&
          !input.Contains("install") &&
          !input.Contains("compile") &&
          !input.Contains("update"))
        Run(() => { Kernel.Bootstrap(hce); });

      /**
       * This method is used for running code asynchronously and catching exceptions at the highest level.
       */

      void Run(Action action)
      {
        try
        {
          Task.Run(action).GetAwaiter().GetResult();
          WithCode(Code.Success);
        }
        catch (Exception e)
        {
          Error(e.Message);
          System.Console.Error.WriteLine("\n\n" + e.StackTrace);
          WriteAllText(Paths.Exception, e.ToString());
          WithCode(Code.Exception);
        }
      }
    }

    /// <summary>
    ///   Renders a dynamic banner which is both pleasing to the eye and informative.
    /// </summary>
    private static void DisplayBanner()
    {
      var bn = Assembly.GetEntryAssembly()?.GetName().Version.Major.ToString("D4");

      var bannerLineDecorations = new string('-', BannerBuildSource.Length + 1);

      ForegroundColor = ConsoleColor.Green; /* the colour of the one */
      WriteLine(Banner);                    /* ascii art and usage */
      WriteLine(BannerBuildNumber, bn);     /* reference build */
      WriteLine(bannerLineDecorations);     /* separator */
      WriteLine(BannerBuildSource, bn);     /* reference link */
      WriteLine(bannerLineDecorations);     /* separator */
      ForegroundColor = ConsoleColor.White; /* end banner */
    }
  }
}