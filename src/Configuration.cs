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

using System.Collections.Generic;
using static System.IO.File;
using static HXE.OpenSauce.OpenSauceObjects.ObjectsWeapon;
using static SPV3.Paths;

namespace SPV3
{
  public partial class Configuration
  {
    public ConfigurationLoader    Loader    { get; set; } = new ConfigurationLoader();
    public ConfigurationHXE       HXE       { get; set; } = new ConfigurationHXE();
    public ConfigurationOpenSauce OpenSauce { get; set; } = new ConfigurationOpenSauce();
    public ConfigurationChimera   Chimera   { get; set; } = new ConfigurationChimera();

    public void Load()
    {
      HXE.Load();
      Loader.Load();
      OpenSauce.Load();
      Chimera.Load();
    }

    public void Save()
    {
      /* keeps compatibility with the hackish post-processing configuration system */
      OpenSauce.Configuration.Rasterizer.PostProcessing.MotionBlur.Enabled = HXE.Shaders.MotionBlur == 1;

      if (Loader.DOOM && !Loader.Blind)
        if (Exists(DOOM))
          OpenSauce.Configuration.Objects.Weapon.Load(DOOM);
        else
          OpenSauce.Configuration.Objects.Weapon.Positions = new List<PositionWeapon>();

      if (Loader.Blind && !Loader.DOOM)
        if (Exists(Blind))
        {
          OpenSauce.Configuration.Objects.Weapon.Load(Blind);
          OpenSauce.Configuration.HUD.ShowHUD = false;
        }
        else
        {
          OpenSauce.Configuration.Objects.Weapon.Positions = new List<PositionWeapon>();
          OpenSauce.Configuration.HUD.ShowHUD              = true;
        }

      if (!Loader.DOOM && !Loader.Blind)
        OpenSauce.Configuration.Objects.Weapon.Positions = new List<PositionWeapon>();

      HXE.Save();
      Loader.Save();
      OpenSauce.Save();
      Chimera.Save();
    }

    public void CalculateFOV()
    {
      OpenSauce.FieldOfView = OpenSauce.Configuration.Camera.CalculateFOV(Loader.Width, Loader.Height);
    }
  }
}