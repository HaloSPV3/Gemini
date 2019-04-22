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
using System.Text;

namespace SPV3.CLI
{
  /// <inheritdoc />
  /// <summary>
  ///   Representation of the HCE executable on the filesystem.
  /// </summary>
  public class Executable : File
  {
    public VideoOptions   Video   { get; set; } = new VideoOptions();
    public DebugOptions   Debug   { get; set; } = new DebugOptions();
    public ProfileOptions Profile { get; set; } = new ProfileOptions();

    /// <summary>
    ///   Invokes the HCE executable with the arguments that represent this object's properties' states.
    /// </summary>
    public void Start()
    {
      /**
       * Converts the properties to arguments which the HCE executable can be invoked with.
       */
      string GetArguments()
      {
        var args = new StringBuilder();

        /**
         * Arguments for debugging purposes. 
         */
        if (Debug.Console) args.Append("-console ");
        if (Debug.Developer) args.Append("-devmode -screenshot ");
        if (Debug.Screenshot) args.Append("-screenshot ");

        /**
         * Arguments for video overrides.
         */
        if (Video.Window) args.Append("-window ");

        if (Video.Width > 0 && Video.Height > 0 && Video.Refresh > 0)
          args.Append($"-vidmode {Video.Width},{Video.Height},{Video.Refresh} ");

        if (Video.Adapter > 1)
          args.Append($"-adapter {Video.Adapter}");

        if (!string.IsNullOrWhiteSpace(Profile.Path))
          args.Append($"-path {System.IO.Path.GetFullPath(Profile.Path)}");

        return args.ToString();
      }

      Process.Start(Path, GetArguments());
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="executable">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(Executable executable)
    {
      return executable.Path;
    }

    /// <summary>
    ///   Represents the inbound string as an object.
    /// </summary>
    /// <param name="executable">
    ///   String to represent as object.
    /// </param>
    /// <returns>
    ///   Object representation of the inbound string.
    /// </returns>
    public static explicit operator Executable(string executable)
    {
      return new Executable
      {
        Path = executable
      };
    }

    public class DebugOptions
    {
      public bool Console    { get; set; }
      public bool Developer  { get; set; }
      public bool Screenshot { get; set; }
    }

    public class VideoOptions
    {
      public bool Window  { get; set; }
      public int  Width   { get; set; }
      public int  Height  { get; set; }
      public int  Refresh { get; set; }
      public int  Adapter { get; set; }
    }

    public class ProfileOptions
    {
      public string Path { get; set; } = Paths.Directories.HCE;
    }
  }
}