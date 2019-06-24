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

namespace HXE.SPV3
{
  /// <summary>
  ///   Object representation of the SPV3.2 campaign attributes.
  /// </summary>
  public static class Campaign
  {
    /// <summary>
    ///   Available SPV3.2 difficulties.
    /// </summary>
    public enum Difficulty
    {
      Normal,    // normal
      Heroic,    // hard
      Legendary, // impossible
      Noble      // easy
    }

    /// <summary>
    ///   Available SPV3.2 missions.
    /// </summary>
    public enum Mission
    {
      Spv3A10        = 1,  /* must match data\levels\ui\scripts\script.hsc */
      Spv3A30        = 2,  /* must match data\levels\ui\scripts\script.hsc */
      Spv3A50        = 3,  /* must match data\levels\ui\scripts\script.hsc */
      Spv3B30        = 4,  /* must match data\levels\ui\scripts\script.hsc */
      Spv3B30Evolved = 5,  /* must match data\levels\ui\scripts\script.hsc */
      Spv3B40        = 6,  /* must match data\levels\ui\scripts\script.hsc */
      Spv3C10        = 7,  /* must match data\levels\ui\scripts\script.hsc */
      Spv3C20        = 8,  /* must match data\levels\ui\scripts\script.hsc */
      Spv3C40        = 9,  /* must match data\levels\ui\scripts\script.hsc */
      Spv3D20        = 10, /* must match data\levels\ui\scripts\script.hsc */
      Spv3D25        = 11, /* must match data\levels\ui\scripts\script.hsc */
      Spv3D30        = 12, /* must match data\levels\ui\scripts\script.hsc */
      Spv3D30Evolved = 13, /* must match data\levels\ui\scripts\script.hsc */
      Spv3D40        = 14, /* must match data\levels\ui\scripts\script.hsc */
      LumoriaA       = 15, /* must match data\levels\ui\scripts\script.hsc */
      LumoriaB       = 16, /* must match data\levels\ui\scripts\script.hsc */
      LumoriaCd      = 17  /* must match data\levels\ui\scripts\script.hsc */
    }
  }
}