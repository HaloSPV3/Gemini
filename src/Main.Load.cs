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
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using HXE;
using HXE.HCE;
using SPV3.Annotations;
using static HXE.SPV3.PostProcessing;
using static HXE.SPV3.PostProcessing.MotionBlurOptions;
using File = System.IO.File;

namespace SPV3
{
  public partial class Main
  {
    public class MainLoad : INotifyPropertyChanged
    {
      private Visibility _visibility = Visibility.Collapsed;

      public Visibility Visibility
      {
        get => _visibility;
        set
        {
          if (value == _visibility) return;
          _visibility = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Invoke()
      {
        var spv3      = new Configuration.ConfigurationLoader();                 /* for configuration          */
        var hxe       = (HXE.Configuration) HXE.Paths.Configuration;             /* for compatibility          */
        var openSauce = (OpenSauce) HXE.Paths.Custom.OpenSauce(Paths.Directory); /* for menu fixes, gfx, modes */

        if (spv3.Exists())
          spv3.Load();

        if (hxe.Exists())
          hxe.Load();

        if (openSauce.Exists())
          openSauce.Load();
        else
          openSauce.Camera.CalculateFOV(); /* apply native field of view */

        if (spv3.DOOM && !spv3.Blind)
          if (File.Exists(Paths.DOOM))
          {
            openSauce.Objects.Weapon.Load(Paths.DOOM);
            openSauce.HUD.ShowHUD = true;
          }

        if (spv3.Blind && !spv3.DOOM)
        {
          if (File.Exists(Paths.Blind))
          {
            openSauce.Objects.Weapon.Load(Paths.Blind);
            openSauce.HUD.ShowHUD = false;
          }
        }
        else
        {
          openSauce.HUD.ShowHUD  = true; /* forcefully enable hud   */
          openSauce.HUD.ScaleHUD = true; /* fixes user interface    */
        }


        /* This is used for maintaining compatibility between the OpenSauce & SPV3 Post-Processing systems! */
        openSauce.Rasterizer.PostProcessing.MotionBlur.Enabled = hxe.PostProcessing.MotionBlur == BuiltIn;
        openSauce.Camera.IgnoreFOVChangeInCinematics           = true;       /* fixes user interface    */
        openSauce.Camera.IgnoreFOVChangeInMainMenu             = true;       /* fixes user interface    */
        openSauce.Rasterizer.ShaderExtensions.Effect.DepthFade = true;       /* shader optimisations    */
        openSauce.Rasterizer.GBuffer.Enabled                   = !spv3.Bare; /* low-end graphics mode   */

        hxe.Kernel.EnableSpv3KernelMode = true; /* hxe spv3 compatibility */
        hxe.Kernel.SkipVerifyMainAssets = true; /* skips verifying assets */

        spv3.Save();      /* saves to %APPDATA%\SPV3 */
        openSauce.Save(); /* saves to %APPDATA%\SPV3 */
        hxe.Save();       /* saves to %APPDATA%\HXE  */

        Kernel.Bootstrap(new Executable
        {
          Path = Path.Combine(Environment.CurrentDirectory, HXE.Paths.HCE.Executable),
          Profile = new Executable.ProfileOptions
          {
            Path = Paths.Directory
          },
          Video = new Executable.VideoOptions
          {
            Width   = spv3.Width,
            Height  = spv3.Height,
            Window  = spv3.Window,
            NoGamma = !spv3.Gamma /* flip boolean */
          }
        });
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}