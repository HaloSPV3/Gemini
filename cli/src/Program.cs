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
using System.Linq;
using System.Threading.Tasks;
using static System.Console;
using static System.Environment.SpecialFolder;
using static System.Reflection.Assembly;
using static SPV3.CLI.Console;
using static SPV3.CLI.Exit.Code;
using static SPV3.CLI.Names.Files;

namespace SPV3.CLI
{
  /// <summary>
  ///   SPV3.CLI Program.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    ///   SPV3.CLI entry.
    /// </summary>
    /// <param name="args">
    ///   Arguments for the CLI.
    /// </param>
    public static void Main(string[] args)
    {
      InitiateData(); /* startup data initiation       */
      OutputBanner(); /* startup ascii output          */
      HandleUpdate(); /* startup update verification   */
      HandleInvoke(); /* startup command invocations   */

      /**
       * Displays the ASCII art, key information, and build version.
       */

      void OutputBanner()
      {
        var v = GetEntryAssembly().GetName().Version.Major.ToString("D3");

        ForegroundColor = ConsoleColor.Green;
        WriteLine(@"   _____ ____ _    _______  ________    ____");
        WriteLine(@"  / ___// __ \ |  / /__  / / ____/ /   /  _/");
        WriteLine(@"  \__ \/ /_/ / | / / /_ < / /   / /    / /  ");
        WriteLine(@" ___/ / ____/| |/ /___/ // /___/ /____/ /   ");
        WriteLine(@"/____/_/     |___//____(_)____/_____/___/   ");
        WriteLine(@"============================================");
        WriteLine(@"The SPV3 CLI ~ Automatic loader for HCE/SPV3");
        WriteLine(@"--------------------------------------------");
        WriteLine(@"source  ::  https://cgit.n2.network/spv3.cli");
        WriteLine(@"binary  ::  https://dist.n2.network/spv3.cli");
        WriteLine(@"--------------------------------------------");
        WriteLine($"Executable has been compiled from build: {v}");
        WriteLine(@"--------------------------------------------");
        ForegroundColor = ConsoleColor.White;
      }

      /**
       * Conduct data initiation on each start-up.
       */

      void InitiateData()
      {
        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(ApplicationData), Names.Directories.Data));
      }

      /**
       * The CLI provides both an interactive and automatic update mechanism. Using --auto-update, the loader will
       * automatically update itself when necessary. Without the argument, the user will be prompted to choose whether
       * to update the loader now or not.
       *
       * Once that's out of the way, the --auto-update will be removed from the array of arguments, as it's no longer
       * needed in subsequent invocations throughout the CLI.
       */

      void HandleUpdate()
      {
        try
        {
          if (!Update.Verify()) return;

          Warn(@"Loader update is available to download!");

          using (var reader = new StringReader(Update.Logs()))
          {
            string line;
            while ((line = reader.ReadLine()) != null)
              Logs("> " + line);
          }

          if (args.Contains("--auto-update"))
          {
            Warn(@"Will automatically conduct auto-update!");
            Update.Commit();
          }
          else
          {
            Warn(@"Would you like to conduct update? [y/n]");
            if (ReadLine() == "y")
              Update.Commit();
          }
        }
        catch (Exception e)
        {
          Info(e.Message);
        }

        args = args.Where(val => val != "--auto-update").ToArray();
      }

      /**
       * Commands are invoked either explicitly or implicitly. The only implicitly invoked command is the loading one,
       * and it's done when there are no commands being passed to the CLI.
       */

      void HandleInvoke()
      {
        /**
         * Implicit Loading command.
         */
        if (args.Length == 0)
        {
          Info("Implicitly invoked 'load' command.");
          Run(Kernel.Bootstrap);

          return;
        }

        var command = args[0];

        /**
         * Updating command.
         */

        switch (command)
        {
          case "update" when args.Length >= 2:
          {
            Info("Explicitly invoked 'update' command.");

            switch (args[1])
            {
              case "install":
                Info("Explicitly invoked 'install' argument.");
                Run(Update.Install);
                Warn("Update has been successfully installed!");

                return;
              case "finish":
                Info("Explicitly invoked 'finish' argument.");
                Run(Update.Finish);
                Warn("Update has been successfully finished!");

                return;
            }

            return;
          }

          /**
           * Compilation command.
           */

          case "compile" when args.Length >= 2:
          {
            Info("Explicitly invoked 'compile' command.");

            var source = args.Length == 2 ? Environment.CurrentDirectory : args[2]; /* implicitly use working dir */
            var target = args[1];

            Run(() => { Compiler.Compile(source, target); });
            return;
          }

          /**
           * Installation command.
           */

          case "install" when args.Length >= 1:
          {
            Info("Explicitly invoked 'install' command.");

            var source = args.Length == 2 ? Environment.CurrentDirectory : args[2]; /* allow non-working dir paths */
            var target = args.Length == 1 ? Path.Combine(Environment.GetFolderPath(ApplicationData), "SPV3") : args[1];

            Run(() => { Installer.Install(source, target); });
            return;
          }

          /**
           * Placeholder command.
           */

          case "placeholder" when args.Length > 1:
          {
            Info("Explicitly invoked 'placeholder' command.");

            switch (args[1])
            {
              case "commit" when args.Length >= 4:
              {
                Info("Explicitly invoked 'commit' argument.");

                var bitmap = args[2];
                var target = args[3];
                var filter = args.Length == 4 ? "*.bitmap" : args[4];

                Run(() => { Placeholder.Commit(bitmap, target, filter); });
                return;
              }
              case "revert" when args.Length >= 2:
              {
                Info("Explicitly invoked 'revert' argument.");

                var records = args[2];

                Run(() => { Placeholder.Revert(records); });
                return;
              }
              default:
                Error("Invalid placeholder args.");
                Exit.WithCode(InvalidArgument);
                return;
            }
          }

          /**
           * Dump command.
           */

          case "dump" when args.Length > 1:
          {
            Info("Explicitly invoked 'dump' command.");

            switch (args[1])
            {
              case "opensauce":
                Info("Explicitly invoked 'opensauce' argument.");

                var openSaucePath = Names.Files.OpenSauce;

                Run(() => { new OpenSauce {Path = openSaucePath}.Save(); });
                return;
              default:
                Error("Invalid dump args.");
                Exit.WithCode(InvalidArgument);
                return;
            }
          }

          default:
            Error("Invalid command.");
            Exit.WithCode(InvalidCommand);
            return;
        }
      }
    }

    private static void Run(Action action)
    {
      try
      {
        Task.Run(action).GetAwaiter().GetResult();
      }
      catch (Exception e)
      {
        Error(e.Message);
        System.Console.Error.WriteLine(e.StackTrace);
        Exit.WithCode(Exit.Code.Exception);
      }
    }
  }
}