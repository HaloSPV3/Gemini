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
using System.Text;

namespace HXE.SPV3
{
  /// <inheritdoc />
  /// <summary>
  ///   Object representation of the OpenSauce initc.txt file on the filesystem.
  /// </summary>
  public class Initiation : File
  {
    public bool                CinematicBars   { get; set; } = true;
    public bool                PlayerAutoaim   { get; set; } = true;
    public bool                PlayerMagnetism { get; set; } = true;
    public Campaign.Mission    Mission         { get; set; } = Campaign.Mission.Spv3A10;
    public Campaign.Difficulty Difficulty      { get; set; } = Campaign.Difficulty.Normal;
    public bool                Unlock          { get; set; }

    public PostProcessing PostProcessing { get; set; } =
      new PostProcessing();

    /// <summary>
    ///   Saves object state to the inbound file.
    /// </summary>
    public void Save()
    {
      /**
       * Converts the Campaign.Difficulty value to a game_difficulty_set parameter, as specified in the loader.txt
       * documentation.
       */
      string GetDifficulty()
      {
        switch (Difficulty)
        {
          case Campaign.Difficulty.Normal:
            return "normal";
          case Campaign.Difficulty.Heroic:
            return "hard";
          case Campaign.Difficulty.Legendary:
            return "impossible";
          case Campaign.Difficulty.Noble:
            return "easy";
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      var difficulty = GetDifficulty();
      var mission    = (int) Mission;
      var autoaim    = PlayerAutoaim ? 1 : 0;
      var magnetism  = PlayerMagnetism ? 1 : 0;
      var cinematic  = CinematicBars ? 1 : 0;

      var output = new StringBuilder();
      output.AppendLine($"set f3 {mission}"); /*  */
      output.AppendLine($"set loud_dialog_hack {cinematic}");
      output.AppendLine($"player_autoaim {autoaim}");
      output.AppendLine($"player_magnetism {magnetism}");
      output.AppendLine($"game_difficulty_set {difficulty}");

      /**
       * Encodes post-processing settings to the initc file. Refer to doc/shaders.txt for further information.
       */

      switch (PostProcessing.MotionBlur)
      {
        case PostProcessing.MotionBlurOptions.Off:
          break;
        case PostProcessing.MotionBlurOptions.BuiltIn:
          output.AppendLine("set multiplayer_hit_sound_volume 1.1");
          break;
        case PostProcessing.MotionBlurOptions.PombLow:
          output.AppendLine("set multiplayer_hit_sound_volume 1.2");
          break;
        case PostProcessing.MotionBlurOptions.PombHigh:
          output.AppendLine("set multiplayer_hit_sound_volume 1.3");
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      switch (PostProcessing.Mxao)
      {
        case PostProcessing.MxaoOptions.Off:
          break;
        case PostProcessing.MxaoOptions.Low:
          output.AppendLine("set cl_remote_player_action_queue_limit 3");
          break;
        case PostProcessing.MxaoOptions.High:
          output.AppendLine("set cl_remote_player_action_queue_limit 4");
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      switch (PostProcessing.Dof)
      {
        case PostProcessing.DofOptions.Off:
          break;
        case PostProcessing.DofOptions.Low:
          output.AppendLine("set cl_remote_player_action_queue_tick_limit 7");
          break;
        case PostProcessing.DofOptions.High:
          output.AppendLine("set cl_remote_player_action_queue_tick_limit 8");
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      if (PostProcessing.Volumetrics)
        output.AppendLine("set rasterizer_soft_filter true");

      if (PostProcessing.DynamicLensFlares)
        output.AppendLine("set display_precache_progress false");

      if (!PostProcessing.LensDirt)
        output.AppendLine("set use_super_remote_players_action_update true");

      if (!PostProcessing.FilmGrain)
        output.AppendLine("set use_new_vehicle_update_scheme true");

      if (!PostProcessing.HudVisor)
        output.AppendLine("set multiplayer_draw_teammates_names true");

      Console.Info("Saving initiation data to the initc.txt file");
      WriteAllText(output.ToString());
      Console.Info("Successfully applied initc.txt configurations");
      Console.Debug("Initiation data: \n\n" + ReadAllText());
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="initiation">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(Initiation initiation)
    {
      return initiation.Path;
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
    public static explicit operator Initiation(string name)
    {
      return new Initiation
      {
        Path = name
      };
    }
  }
}