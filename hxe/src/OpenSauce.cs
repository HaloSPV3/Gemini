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
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using static System.Math;
using static System.Windows.SystemParameters;
using static HXE.OpenSauce.OpenSauceHUD;
using static HXE.OpenSauce.OpenSauceObjects.ObjectsWeapon.PositionWeapon;

namespace HXE
{
  /// <summary>
  ///   Object representing an OpenSauce user configuration.
  /// </summary>
  public class OpenSauce : File
  {
    public OpenSauceCacheFiles CacheFiles { get; set; } = new OpenSauceCacheFiles();
    public OpenSauceRasterizer Rasterizer { get; set; } = new OpenSauceRasterizer();
    public OpenSauceCamera     Camera     { get; set; } = new OpenSauceCamera();
    public OpenSauceNetworking Networking { get; set; } = new OpenSauceNetworking();
    public OpenSauceHUD        HUD        { get; set; } = new OpenSauceHUD();
    public OpenSauceObjects    Objects    { get; set; } = new OpenSauceObjects();

    /// <summary>
    ///   Saves object state to the inbound file.
    /// </summary>
    public void Save()
    {
      using (var writer = new StringWriter())
      {
        var serialiser = new XmlSerializer(typeof(OpenSauce));
        serialiser.Serialize(writer, this);
        WriteAllText(writer.ToString());
      }
    }

    /// <summary>
    ///   Loads object state from the inbound file.
    /// </summary>
    public void Load()
    {
      using (var reader = new StringReader(ReadAllText()))
      {
        var serialiser = new XmlSerializer(typeof(OpenSauce));
        var serialised = (OpenSauce) serialiser.Deserialize(reader);

        CacheFiles = serialised.CacheFiles;
        Rasterizer = serialised.Rasterizer;
        Camera     = serialised.Camera;
        Networking = serialised.Networking;
        HUD        = serialised.HUD;
        Objects    = serialised.Objects;
      }
    }

    /// <summary>
    ///   Applies DOOM style weapons alignment and forces the HUD to be enabled.
    /// </summary>
    public void ApplyDOOM()
    {
      Objects.Weapon.ApplyDOOM();
      HUD.ShowHUD = true;
    }

    /// <summary>
    ///   Applies DOOM style weapons alignment and patches invisible HUD.
    /// </summary>
    public void ApplyBlind()
    {
      Objects.Weapon.ApplyBlind();
      HUD.ShowHUD = false;
      HUD.HUDScale = new HUDHUDScale
      {
        X = 1,
        Y = 1
      };
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="openSauce">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(OpenSauce openSauce)
    {
      return openSauce.Path;
    }

    /// <summary>
    ///   Represents the inbound string as an object.
    /// </summary>
    /// <param name="name">
    ///   String to represent as object.
    /// </param>
    /// <returns>
    ///   Object representation of the inbound string.
    /// </returns>
    public static explicit operator OpenSauce(string name)
    {
      return new OpenSauce
      {
        Path = name
      };
    }

    public class OpenSauceCacheFiles
    {
      public bool CheckYeloFilesFirst { get; set; } = true;
    }

    public class OpenSauceRasterizer
    {
      public RasterizerGBuffer          GBuffer          { get; set; } = new RasterizerGBuffer();
      public RasterizerUpgrades         Upgrades         { get; set; } = new RasterizerUpgrades();
      public RasterizerShaderExtensions ShaderExtensions { get; set; } = new RasterizerShaderExtensions();
      public RasterizerPostProcessing   PostProcessing   { get; set; } = new RasterizerPostProcessing();

      public class RasterizerGBuffer
      {
        public bool Enabled { get; set; } = true;
      }

      public class RasterizerUpgrades
      {
        public bool MaximumRenderedTriangles { get; set; } = true;
      }

      public class RasterizerShaderExtensions
      {
        public bool                        Enabled     { get; set; } = true;
        public ShaderExtensionsObject      Object      { get; set; } = new ShaderExtensionsObject();
        public ShaderExtensionsEnvironment Environment { get; set; } = new ShaderExtensionsEnvironment();
        public ShaderExtensionsEffect      Effect      { get; set; } = new ShaderExtensionsEffect();

        public class ShaderExtensionsObject
        {
          public bool NormalMaps       { get; set; } = true;
          public bool DetailNormalMaps { get; set; } = true;
          public bool SpecularMaps     { get; set; } = true;
          public bool SpecularLighting { get; set; } = true;
        }

        public class ShaderExtensionsEnvironment
        {
          public bool DiffuseDirectionalLightmaps  { get; set; } = true;
          public bool SpecularDirectionalLightmaps { get; set; } = true;
        }

        public class ShaderExtensionsEffect
        {
          public bool DepthFade { get; set; } = true;
        }
      }

      public class RasterizerPostProcessing
      {
        public PostProcessingMotionBlur      MotionBlur      { get; set; } = new PostProcessingMotionBlur();
        public PostProcessingBloom           Bloom           { get; set; } = new PostProcessingBloom();
        public PostProcessingAntiAliasing    AntiAliasing    { get; set; } = new PostProcessingAntiAliasing();
        public PostProcessingExternalEffects ExternalEffects { get; set; } = new PostProcessingExternalEffects();
        public PostProcessingMapEffects      MapEffects      { get; set; } = new PostProcessingMapEffects();

        public class PostProcessingMotionBlur
        {
          public bool   Enabled    { get; set; } = true;
          public double BlurAmount { get; set; } = 1.00;
        }

        public class PostProcessingBloom
        {
          public bool Enabled { get; set; } = true;
        }

        public class PostProcessingAntiAliasing
        {
          public bool Enabled { get; set; } = false;
        }

        public class PostProcessingExternalEffects
        {
          public bool Enabled { get; set; } = true;
        }

        public class PostProcessingMapEffects
        {
          public bool Enabled { get; set; } = true;
        }
      }
    }

    public class OpenSauceCamera
    {
      public double FieldOfView                 { get; set; } = 70.00;
      public bool   IgnoreFOVChangeInCinematics { get; set; } = true;
      public bool   IgnoreFOVChangeInMainMenu   { get; set; } = true;

      public double CalculateFOV()
      {
        return CalculateFOV(PrimaryScreenWidth, PrimaryScreenHeight);
      }

      public double CalculateFOV(double width, double height)
      {
        double Degrees(double value)
        {
          return value * (180 / PI);
        }

        /**
         * 2 * arctan(((A)/(B)) * tan(C))
         * Formula by Mortis
         *
         * A = New Width / New Height
         * B = Old Width / Old Height (or ratio) (HCE = 4:3)
         *
         * This gets me nostalgic!
         */

        var w = width;
        var h = height;

        var a = Degrees(w  / h);
        var b = Degrees(4  / 3);
        var c = Degrees(70 / 2);

        var x = Atan2(a / b, Tan(c));

        var dirtyResultFix = 9;
        var y              = Degrees(2 * x) - dirtyResultFix;

        FieldOfView = Truncate(y * 100) / 100;

        return FieldOfView;
      }
    }

    public class OpenSauceNetworking
    {
      public NetworkingGameSpy      GameSpy      { get; set; } = new NetworkingGameSpy();
      public NetworkingMapDownload  MapDownload  { get; set; } = new NetworkingMapDownload();
      public NetworkingVersionCheck VersionCheck { get; set; } = new NetworkingVersionCheck();

      public class NetworkingGameSpy
      {
        public bool NoUpdateCheck { get; set; } = true;
      }

      public class NetworkingMapDownload
      {
        public bool Enabled { get; set; } = true;
      }

      public class NetworkingVersionCheck
      {
        public VersionCheckDate       Date       { get; set; } = new VersionCheckDate();
        public VersionCheckServerList ServerList { get; set; } = new VersionCheckServerList();

        public class VersionCheckDate
        {
          public int Day   { get; set; } = (int) DateTime.Now.DayOfWeek;
          public int Month { get; set; } = DateTime.Now.Month;
          public int Year  { get; set; } = DateTime.Now.Year;
        }

        public class VersionCheckServerList
        {
          public int    Version { get; set; } = 1;
          public string Server  { get; set; } = "http://os.halomods.com/Halo1/CE/Halo1_CE_Version.xml";
        }
      }
    }

    public class OpenSauceObjects
    {
      public bool          VehicleRemapperEnabled { get; set; } = true;
      public ObjectsWeapon Weapon                 { get; set; } = new ObjectsWeapon();

      public class ObjectsWeapon
      {
        public List<PositionWeapon> Positions { get; set; } = new List<PositionWeapon>();

        /// <summary>
        ///   Applies DOOM style weapons alignment.
        /// </summary>
        public void ApplyDOOM()
        {
          /**
           * Coordinates are courtesy of Masterz.
           */

          Positions = new List<PositionWeapon>
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
            new PositionWeapon /* M7/Caseless SMG */
            {
              Name = "Picked up a M7/Caseless SMG",
              Position = new WeaponPosition
              {
                I = 0, /* TODO! */
                J = 0, /* TODO! */
                K = 0  /* TODO! */
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

        /// <summary>
        ///   Applies Blind style weapons alignment.
        /// </summary>
        public void ApplyBlind()
        {
          Positions = new List<PositionWeapon>
          {
            new PositionWeapon /* Default */
            {
              Name = null,
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* M7/Caseless SMG */
            {
              Name = "Picked up a M7/Caseless SMG",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* M310 Grenade Launcher */
            {
              Name = "Picked up a M310 Grenade Launcher",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* M247H Turret */
            {
              Name = "Picked up a M247H Turret",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* SOCOM M6C Pistol */
            {
              Name = "Picked up a SOCOM M6C Pistol",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* AMPED M393 Designated Marksman Rifle */
            {
              Name = "Picked up an AMPED M393 Designated Marksman Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* BR54-HB Battle Rifle */
            {
              Name = "Picked up a BR54-HB Battle Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Spiker */
            {
              Name = "Picked up a Spiker",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Focus Rifle */
            {
              Name = "Picked up a Focus Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Particle Carbine */
            {
              Name = "Picked up a Particle Carbine",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* MA4E Assault Rifle */
            {
              Name = "Picked up an MA4E Assault Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* UNRECOGNIZED ORGANIC PENETRATION INCINERATION BEAM */
            {
              Name = "UNRECOGNIZED ORGANIC PENETRATION INCINERATION BEAM",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* UNRECOGNIZED EXTENDED RANGE INCINERATION BEAM */
            {
              Name = "UNRECOGNIZED EXTENDED RANGE INCINERATION BEAM",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Advanced Plasma Rifle */
            {
              Name = "Picked up an advanced Plasma Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* AMPED M7/Caseless SMG */
            {
              Name = "Picked up an AMPED M7/Caseless SMG",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Brute Shot */
            {
              Name = "Picked up a Brute Shot",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Hunter Fuel Rod Cannon */
            {
              Name = "Picked up a Hunter Fuel Rod Cannon",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Hunter Fuel Rod Beam */
            {
              Name = "Picked up a Hunter Fuel Rod Beam",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Hunter's Shade Cannon */
            {
              Name = "Picked up a Hunter's Shade Cannon",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Jackal Shield */
            {
              Name = "Picked up a Jackal Shield",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Needler */
            {
              Name = "Picked up a Needler",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Piercer */
            {
              Name = "Picked up a Piercer",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Plasma Pistol */
            {
              Name = "Picked up a Plasma Pistol",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Void's Tear */
            {
              Name = "Picked up a Void's Tear",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Brute Plasma Pistol */
            {
              Name = "Picked up a Brute Plasma Pistol",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Shredder */
            {
              Name = "Picked up a Shredder",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Plasma Rifle */
            {
              Name = "Picked up a Plasma Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* Brute Plasma Rifle */
            {
              Name = "Picked up a Brute Plasma Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* UNRECOGNIZED INCINERATION BEAM */
            {
              Name = "UNRECOGNIZED INCINERATION BEAM",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* MA5E Assault Rifle */
            {
              Name = "Picked up an MA5E Assault Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* MA5E Assault Rifle + M45 Grenade Launcher */
            {
              Name = "Picked up a MA5E Assault Rifle + M45 Grenade Launcher",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* M91 Shotgun */
            {
              Name = "Picked up a M91 Shotgun",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* M7057 Defoliant Flamethrower */
            {
              Name = "Picked up a M7057 Defoliant Flamethrower",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* M6G Pistol */
            {
              Name = "Picked up a M6G Pistol",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* M393 Designated Marksman Rifle */
            {
              Name = "Picked up a M393 Designated Marksman Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* BR54-HBS Battle Rifle */
            {
              Name = "Picked up a BR54-HBS Battle Rifle",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* BR54-HB Battle Rifle + M45 Grenade Launcher */
            {
              Name = "Picked up a BR54-HB Battle Rifle + M45 Grenade Launcher",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            },
            new PositionWeapon /* MA4E Assault Rifle + M45 Grenade Launcher */
            {
              Name = "Picked up a MA4E Assault Rifle + M45 Grenade Launcher",
              Position = new WeaponPosition
              {
                I = 10,
                J = 10,
                K = 10
              }
            }
          };
        }

        [XmlType("Weapon")]
        public class PositionWeapon
        {
          public string         Name     { get; set; } = string.Empty;
          public WeaponPosition Position { get; set; } = new WeaponPosition();

          public class WeaponPosition
          {
            public double I { get; set; }
            public double J { get; set; }
            public double K { get; set; }
          }
        }
      }
    }

    public class OpenSauceHUD
    {
      public bool        ShowHUD  { get; set; }
      public bool        ScaleHUD { get; set; }
      public HUDHUDScale HUDScale { get; set; }

      public class HUDHUDScale
      {
        public double X { get; set; }
        public double Y { get; set; }
      }
    }
  }
}