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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HXE;
using SPV3.Annotations;
using static HXE.OpenSauce.OpenSauceObjects.ObjectsWeapon;
using static HXE.OpenSauce.OpenSauceObjects.ObjectsWeapon.PositionWeapon;

namespace SPV3
{
  public partial class Configuration
  {
    public class ConfigurationOpenSauce : INotifyPropertyChanged
    {
      private bool   _bloom;
      private bool   _detailedMaps;
      private double _fieldOfView;
      private bool   _normalMaps;
      private bool   _specularLighting;
      private bool   _specularMaps;

      public OpenSauce Configuration { get; } = (OpenSauce) global::HXE.Paths.Custom.OpenSauce(Paths.Directory);

      public bool NormalMaps
      {
        get => _normalMaps;
        set
        {
          if (value == _normalMaps) return;
          _normalMaps = value;
          OnPropertyChanged();
        }
      }

      public bool DetailedMaps
      {
        get => _detailedMaps;
        set
        {
          if (value == _detailedMaps) return;
          _detailedMaps = value;
          OnPropertyChanged();
        }
      }

      public bool SpecularMaps
      {
        get => _specularMaps;
        set
        {
          if (value == _specularMaps) return;
          _specularMaps = value;
          OnPropertyChanged();
        }
      }

      public bool SpecularLighting
      {
        get => _specularLighting;
        set
        {
          if (value == _specularLighting) return;
          _specularLighting = value;
          OnPropertyChanged();
        }
      }

      public bool Bloom
      {
        get => _bloom;
        set
        {
          if (value == _bloom) return;
          _bloom = value;
          OnPropertyChanged();
        }
      }

      public double FieldOfView
      {
        get => _fieldOfView;
        set
        {
          if (value.Equals(_fieldOfView)) return;
          _fieldOfView = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Save()
      {
        Configuration.Rasterizer.ShaderExtensions.Object.NormalMaps       = NormalMaps;
        Configuration.Rasterizer.ShaderExtensions.Object.DetailNormalMaps = DetailedMaps;
        Configuration.Rasterizer.ShaderExtensions.Object.SpecularMaps     = SpecularMaps;
        Configuration.Rasterizer.ShaderExtensions.Object.SpecularLighting = SpecularLighting;
        Configuration.Rasterizer.PostProcessing.Bloom.Enabled             = Bloom;
        Configuration.Camera.FieldOfView                                  = FieldOfView;
        Configuration.Save();
      }

      public void Load()
      {
        if (!Configuration.Exists()) return;

        Configuration.Load();
        NormalMaps       = Configuration.Rasterizer.ShaderExtensions.Object.NormalMaps;
        DetailedMaps     = Configuration.Rasterizer.ShaderExtensions.Object.DetailNormalMaps;
        SpecularMaps     = Configuration.Rasterizer.ShaderExtensions.Object.SpecularMaps;
        SpecularLighting = Configuration.Rasterizer.ShaderExtensions.Object.SpecularLighting;
        Bloom            = Configuration.Rasterizer.PostProcessing.Bloom.Enabled;
        FieldOfView      = Configuration.Camera.FieldOfView;
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      public void CalculateFOV()
      {
        Configuration.Camera.CalculateFOV();
        FieldOfView = Configuration.Camera.FieldOfView;
      }

      /// <summary>
      ///   Applies DOOM style weapons alignment.
      /// </summary>
      public void ApplyDOOM()
      {
        /**
         * Coordinates are courtesy of Masterz.
         */

        Configuration.Objects.Weapon.Positions = new List<PositionWeapon>
        {
          new PositionWeapon /* Default */
          {
            Name = null,
            Position = new WeaponPosition
            {
              I = 0,
              J = 0,
              K = 0
            }
          },
          new PositionWeapon /* M310 Grenade Launcher */
          {
            Name = "Picked up a M310 Grenade Launcher",
            Position = new WeaponPosition
            {
              I = 0.01538467,
              J = -0.04615384,
              K = -0.01538461
            }
          },
          new PositionWeapon /* M247H Turret */
          {
            Name = "Picked up a M247H Turret",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.02564102,
              K = 0
            }
          },
          new PositionWeapon /* SOCOM M6C Pistol */
          {
            Name = "Picked up a SOCOM M6C Pistol",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.05641025,
              K = -0.01538461
            }
          },
          new PositionWeapon /* AMPED M393 Designated Marksman Rifle */
          {
            Name = "Picked up an AMPED M393 Designated Marksman Rifle",
            Position = new WeaponPosition
            {
              I = 0.005128264,
              J = -0.02564102,
              K = -0.01538461
            }
          },
          new PositionWeapon /* BR54-HB Battle Rifle */
          {
            Name = "Picked up a BR54-HB Battle Rifle",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.02564102,
              K = -0.01538461
            }
          },
          new PositionWeapon /* Spiker */
          {
            Name = "Picked up a Spiker",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.05641025,
              K = 0
            }
          },
          new PositionWeapon /* Focus Rifle */
          {
            Name = "Picked up a Focus Rifle",
            Position = new WeaponPosition
            {
              I = 0.01538467,
              J = -0.07692307,
              K = -0.02564102
            }
          },
          new PositionWeapon /* Particle Carbine */
          {
            Name = "Picked up a Particle Carbine",
            Position = new WeaponPosition
            {
              I = -0.03589743,
              J = -0.04615384,
              K = -0.01538461
            }
          },
          new PositionWeapon /* MA4E Assault Rifle */
          {
            Name = "Picked up an MA4E Assault Rifle",
            Position = new WeaponPosition
            {
              I = -0.02564102,
              J = -0.05641025,
              K = -0.005128205
            }
          },
          new PositionWeapon /* UNRECOGNIZED ORGANIC PENETRATION INCINERATION BEAM */
          {
            Name = "UNRECOGNIZED ORGANIC PENETRATION INCINERATION BEAM",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.06666666,
              K = -0.01538461
            }
          },
          new PositionWeapon /* UNRECOGNIZED EXTENDED RANGE INCINERATION BEAM */
          {
            Name = "UNRECOGNIZED EXTENDED RANGE INCINERATION BEAM",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.06666666,
              K = -0.01538461
            }
          },
          new PositionWeapon /* Advanced Plasma Rifle */
          {
            Name = "Picked up an advanced Plasma Rifle",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.04615384,
              K = -0.01538461
            }
          },
          new PositionWeapon /* AMPED M7/Caseless SMG */
          {
            Name = "Picked up an AMPED M7/Caseless SMG",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.03589743,
              K = 0
            }
          },
          new PositionWeapon /* Brute Shot */
          {
            Name = "Picked up a Brute Shot",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.05641025,
              K = 0.02564108
            }
          },
          new PositionWeapon /* Hunter Fuel Rod Cannon */
          {
            Name = "Picked up a Hunter Fuel Rod Cannon",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.1179487,
              K = -0.03589743
            }
          },
          new PositionWeapon /* Hunter Fuel Rod Beam */
          {
            Name = "Picked up a Hunter Fuel Rod Beam",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.1179487,
              K = 0
            }
          },
          new PositionWeapon /* Hunter's Shade Cannon */
          {
            Name = "Picked up a Hunter's Shade Cannon",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.1179487,
              K = -0.03589743
            }
          },
          new PositionWeapon /* Jackal Shield */
          {
            Name = "Picked up a Jackal Shield",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.05641025,
              K = -0.05641025
            }
          },
          new PositionWeapon /* Needler */
          {
            Name = "Picked up a Needler",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.06666666,
              K = -0.04615384
            }
          },
          new PositionWeapon /* Piercer */
          {
            Name = "Picked up a Piercer",
            Position = new WeaponPosition
            {
              I = -0.02564102,
              J = -0.04615384,
              K = -0.02564102
            }
          },
          new PositionWeapon /* Plasma Pistol */
          {
            Name = "Picked up a Plasma Pistol",
            Position = new WeaponPosition
            {
              I = -0.07692307,
              J = -0.05641025,
              K = -0.005128205
            }
          },
          new PositionWeapon /* Void's Tear */
          {
            Name = "Picked up a Void's Tear",
            Position = new WeaponPosition
            {
              I = -0.07692307,
              J = -0.05641025,
              K = -0.005128205
            }
          },
          new PositionWeapon /* Brute Plasma Pistol */
          {
            Name = "Picked up a Brute Plasma Pistol",
            Position = new WeaponPosition
            {
              I = -0.05641025,
              J = -0.05641025,
              K = -0.005128205
            }
          },
          new PositionWeapon /* Shredder */
          {
            Name = "Picked up a Shredder",
            Position = new WeaponPosition
            {
              I = -0.04615384,
              J = -0.07692307,
              K = -0.03589743
            }
          },
          new PositionWeapon /* Plasma Rifle */
          {
            Name = "Picked up a Plasma Rifle",
            Position = new WeaponPosition
            {
              I = 0.03589749,
              J = -0.04615384,
              K = -0.02564102
            }
          },
          new PositionWeapon /* Brute Plasma Rifle */
          {
            Name = "Picked up a Brute Plasma Rifle",
            Position = new WeaponPosition
            {
              I = 0.02564108,
              J = -0.04615384,
              K = -0.01538461
            }
          },
          new PositionWeapon /* UNRECOGNIZED INCINERATION BEAM */
          {
            Name = "UNRECOGNIZED INCINERATION BEAM",
            Position = new WeaponPosition
            {
              I = -0.01538461,
              J = -0.05641025,
              K = -0.01538461
            }
          },
          new PositionWeapon /* MA5E Assault Rifle */
          {
            Name = "Picked up an MA5E Assault Rifle",
            Position = new WeaponPosition
            {
              I = -0.03589743,
              J = -0.05641025,
              K = -0.005128205
            }
          },
          new PositionWeapon /* MA5E Assault Rifle + M45 Grenade Launcher */
          {
            Name = "Picked up a MA5E Assault Rifle + M45 Grenade Launcher",
            Position = new WeaponPosition
            {
              I = -0.02564102,
              J = -0.04615384,
              K = -0.005128205
            }
          },
          new PositionWeapon /* M91 Shotgun */
          {
            Name = "Picked up a M91 Shotgun",
            Position = new WeaponPosition
            {
              I = -0.005128205,
              J = -0.03589743,
              K = -0.005128205
            }
          },
          new PositionWeapon /* M7057 Defoliant Flamethrower */
          {
            Name = "Picked up a M7057 Defoliant Flamethrower",
            Position = new WeaponPosition
            {
              I = 0.02564108,
              J = -0.04615384,
              K = -0.005128205
            }
          },
          new PositionWeapon /* M6G Pistol */
          {
            Name = "Picked up a M6G Pistol",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.005128205,
              K = 0
            }
          },
          new PositionWeapon /* M393 Designated Marksman Rifle */
          {
            Name = "Picked up a M393 Designated Marksman Rifle",
            Position = new WeaponPosition
            {
              I = 0,
              J = -0.02564102,
              K = -0.01538461
            }
          },
          new PositionWeapon /* BR54-HBS Battle Rifle */
          {
            Name = "Picked up a BR54-HBS Battle Rifle",
            Position = new WeaponPosition
            {
              I = 0.01538467,
              J = -0.02564102,
              K = -0.01538461
            }
          },
          new PositionWeapon /* BR54-HB Battle Rifle + M45 Grenade Launcher */
          {
            Name = "Picked up a BR54-HB Battle Rifle + M45 Grenade Launcher",
            Position = new WeaponPosition
            {
              I = 0.005128264,
              J = -0.02564102,
              K = -0.02564102
            }
          },
          new PositionWeapon /* MA4E Assault Rifle + M45 Grenade Launcher */
          {
            Name = "Picked up a MA4E Assault Rifle + M45 Grenade Launcher",
            Position = new WeaponPosition
            {
              I = -0.02564102,
              J = -0.04615384,
              K = -0.005128205
            }
          }
        };
      }
    }
  }
}