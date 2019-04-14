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

using System.IO;
using System.Xml.Serialization;

namespace SPV3.CLI
{
  /// <inheritdoc />
  /// <summary>
  ///   Object representing the configuration which overrides any default values. The practical purpose of this object
  ///   is for debugging SPV3 by quickly overriding values within the serialised XML file.
  /// </summary>
  public class Override : File
  {
    public GeneralOverrides   General   { get; set; } = new GeneralOverrides();
    public ChimeraOverrides   Chimera   { get; set; } = new ChimeraOverrides();
    public OpenSauceOverrides OpenSauce { get; set; } = new OpenSauceOverrides();

    /// <summary>
    ///   Saves object state to the inbound file.
    /// </summary>
    public void Save()
    {
      using (var writer = new StringWriter())
      {
        var serialiser = new XmlSerializer(typeof(Override));
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
        var serialiser   = new XmlSerializer(typeof(Override));
        var deserialised = (Override) serialiser.Deserialize(reader);

        General = new GeneralOverrides
        {
          Window = deserialised.General.Window
        };

        Chimera = new ChimeraOverrides
        {
          Fps60Gameplay           = deserialised.Chimera.Fps60Gameplay,
          Fps60Cinematics         = deserialised.Chimera.Fps60Cinematics,
          DisableModelDownscaling = deserialised.Chimera.DisableModelDownscaling,
          AnisotropicFiltering    = deserialised.Chimera.AnisotropicFiltering
        };

        OpenSauce = new OpenSauceOverrides
        {
          Fov                 = deserialised.OpenSauce.Fov,
          IgnoreCinematicsFov = deserialised.OpenSauce.IgnoreCinematicsFov,
          Hud = new OpenSauceOverrides.HudOverrides
          {
            ShowHud   = deserialised.OpenSauce.Hud.ShowHud,
            ScaleHud  = deserialised.OpenSauce.Hud.ScaleHud,
            HideVisor = deserialised.OpenSauce.Hud.HideVisor
          },
          PostProcessing = new PostProcessing
          {
            Internal = deserialised.OpenSauce.PostProcessing.Internal,
            External = deserialised.OpenSauce.PostProcessing.External,

            GBuffer           = deserialised.OpenSauce.PostProcessing.GBuffer,
            DepthFade         = deserialised.OpenSauce.PostProcessing.DepthFade,
            Bloom             = deserialised.OpenSauce.PostProcessing.Bloom,
            LensDirt          = deserialised.OpenSauce.PostProcessing.LensDirt,
            DynamicLensFlares = deserialised.OpenSauce.PostProcessing.DynamicLensFlares,
            AntiAliasing      = deserialised.OpenSauce.PostProcessing.AntiAliasing,
            MotionBlur        = deserialised.OpenSauce.PostProcessing.MotionBlur,
            Mxao              = deserialised.OpenSauce.PostProcessing.Mxao,
            Volumetrics       = deserialised.OpenSauce.PostProcessing.Volumetrics,
            Dof               = deserialised.OpenSauce.PostProcessing.Dof,
            HudVisor          = deserialised.OpenSauce.PostProcessing.HudVisor,

            Experimental = new PostProcessing.ExperimentalPostProcessing
            {
              ThreeDimensional = deserialised.OpenSauce.PostProcessing.Experimental.ThreeDimensional,
              ColorBlindMode   = deserialised.OpenSauce.PostProcessing.Experimental.ColorBlindMode
            }
          }
        };
      }
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="override">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(Override @override)
    {
      return @override.Path;
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
    public static explicit operator Override(string name)
    {
      return new Override
      {
        Path = name
      };
    }

    /// <summary>
    ///   Overrides for the loader configuration.
    /// </summary>
    public class GeneralOverrides
    {
      public bool Window { get; set; } // On false, loader launches SPV3 in full-screen @ auto-detected resolution.
    }

    /// <summary>
    ///   Overrides for the Chimera configuration.
    /// </summary>
    public class ChimeraOverrides
    {
      public bool Fps60Gameplay           { get; set; }
      public bool Fps60Cinematics         { get; set; }
      public bool DisableModelDownscaling { get; set; }
      public bool AnisotropicFiltering    { get; set; }
    }

    /// <summary>
    ///   Overrides for the OpenSauce configuration.
    /// </summary>
    public class OpenSauceOverrides
    {
      public long Fov                 { get; set; } // Auto-calculate if value is set to 0.
      public bool IgnoreCinematicsFov { get; set; }

      public HudOverrides   Hud            { get; set; } = new HudOverrides();
      public PostProcessing PostProcessing { get; set; } = new PostProcessing();

      /// <summary>
      ///   Overrides for the OpenSauce HUD configuration.
      /// </summary>
      public class HudOverrides
      {
        public bool ShowHud   { get; set; }
        public bool ScaleHud  { get; set; }
        public bool HideVisor { get; set; }
      }
    }
  }
}