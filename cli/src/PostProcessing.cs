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

namespace SPV3.CLI
{
  /// <inheritdoc />
  /// <summary>
  ///   Object representing Post-Processing effects.
  /// </summary>
  public class PostProcessing : File
  {
    /// <summary>
    ///   Depth of Field values.
    /// </summary>
    public enum DofOptions
    {
      Off  = 0x0,
      Low  = 0x1,
      High = 0x2
    }

    /// <summary>
    ///   Motion Blur values.
    /// </summary>
    public enum MotionBlurOptions
    {
      Off      = 0x0,
      BuiltIn  = 0x1,
      PombLow  = 0x2,
      PombHigh = 0x3
    }

    /// <summary>
    ///   MXAO values.
    /// </summary>
    public enum MxaoOptions
    {
      Off  = 0x0,
      Low  = 0x0,
      High = 0x0
    }

    /// <summary>
    ///   Binary file length.
    /// </summary>
    private const int Length = 0x10;

    public bool Internal          { get; set; }
    public bool External          { get; set; }
    public bool GBuffer           { get; set; }
    public bool DepthFade         { get; set; }
    public bool Bloom             { get; set; }
    public bool LensDirt          { get; set; }
    public bool DynamicLensFlares { get; set; }
    public bool Volumetrics       { get; set; }
    public bool AntiAliasing      { get; set; }
    public bool HudVisor          { get; set; }

    public MotionBlurOptions MotionBlur { get; set; } = MotionBlurOptions.PombHigh;
    public MxaoOptions       Mxao       { get; set; } = MxaoOptions.High;
    public DofOptions        Dof        { get; set; } = DofOptions.High;

    public ExperimentalPostProcessing Experimental { get; set; } = new ExperimentalPostProcessing();

    /// <summary>
    ///   Saves object state to the inbound file.
    /// </summary>
    public void Save()
    {
      using (var fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
      using (var ms = new MemoryStream(Length))
      using (var bw = new BinaryWriter(ms))
      {
        bw.BaseStream.Seek(0x00, SeekOrigin.Begin);

        bw.Write(Internal);                                  /* 0x00 */
        bw.Write(External);                                  /* 0x01 */
        bw.Write(GBuffer);                                   /* 0x02 */
        bw.Write(DepthFade);                                 /* 0x03 */
        bw.Write(Bloom);                                     /* 0x04 */
        bw.Write(DynamicLensFlares);                         /* 0x05 */
        bw.Write(Volumetrics);                               /* 0x06 */
        bw.Write(AntiAliasing);                              /* 0x07 */
        bw.Write(HudVisor);                                  /* 0x08 */
        bw.Write((byte) MotionBlur);                         /* 0x09 */
        bw.Write((byte) Mxao);                               /* 0x0A */
        bw.Write((byte) Dof);                                /* 0x0B */
        bw.Write((byte) Experimental.ThreeDimensional);      /* 0x0C */
        bw.Write((byte) Experimental.ColorBlindMode);        /* 0x0D */
        bw.Write(new byte[Length - bw.BaseStream.Position]); /* pad */

        ms.WriteTo(fs);
      }
    }

    /// <summary>
    ///   Loads object state from the inbound file.
    /// </summary>
    public void Load()
    {
      using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
      using (var ms = new MemoryStream(Length))
      using (var br = new BinaryReader(ms))
      {
        fs.CopyTo(ms);
        br.BaseStream.Seek(0x00, SeekOrigin.Begin);

        Internal                      = br.ReadBoolean();                                                   /* 0x00 */
        External                      = br.ReadBoolean();                                                   /* 0x01 */
        GBuffer                       = br.ReadBoolean();                                                   /* 0x02 */
        DepthFade                     = br.ReadBoolean();                                                   /* 0x03 */
        Bloom                         = br.ReadBoolean();                                                   /* 0x04 */
        DynamicLensFlares             = br.ReadBoolean();                                                   /* 0x05 */
        Volumetrics                   = br.ReadBoolean();                                                   /* 0x06 */
        AntiAliasing                  = br.ReadBoolean();                                                   /* 0x07 */
        HudVisor                      = br.ReadBoolean();                                                   /* 0x08 */
        MotionBlur                    = (MotionBlurOptions) br.ReadByte();                                  /* 0x09 */
        Mxao                          = (MxaoOptions) br.ReadByte();                                        /* 0x0A */
        Dof                           = (DofOptions) br.ReadByte();                                         /* 0x0B */
        Experimental.ThreeDimensional = (ExperimentalPostProcessing.ThreeDimensionalOptions) br.ReadByte(); /* 0x0C */
        Experimental.ColorBlindMode   = (ExperimentalPostProcessing.ColorBlindModeOptions) br.ReadByte();   /* 0x0D */
      }
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="postProcessing">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(PostProcessing postProcessing)
    {
      return postProcessing.Path;
    }

    /// <summary>
    ///   Represents the inbound string as an object.
    /// </summary>
    /// <param name="path">
    ///   String to represent as object.
    /// </param>
    /// <returns>
    ///   Object representation of the inbound string.
    /// </returns>
    public static explicit operator PostProcessing(string path)
    {
      return new PostProcessing
      {
        Path = path
      };
    }

    /// <summary>
    ///   Experimental overrides for SPV3.
    /// </summary>
    public class ExperimentalPostProcessing
    {
      public enum ColorBlindModeOptions
      {
        Off          = 0x0,
        Protanopia   = 0x1,
        Deuteranopes = 0x2,
        Tritanopes   = 0x3
      }

      public enum ThreeDimensionalOptions
      {
        Off          = 0x0,
        Anaglyphic   = 0x1,
        Interleaving = 0x2,
        SideBySide   = 0x3
      }

      public ThreeDimensionalOptions ThreeDimensional { get; set; } = ThreeDimensionalOptions.Off;
      public ColorBlindModeOptions   ColorBlindMode   { get; set; } = ColorBlindModeOptions.Off;
    }
  }
}