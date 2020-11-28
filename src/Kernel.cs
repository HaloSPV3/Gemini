using static System.IO.File;

namespace SPV3
{
  /**
   * Object representing the HXE Kernel.
   */
  public static class Kernel
  {
    public static HXE.Kernel.Configuration          hxe  = new HXE.Kernel.Configuration(Paths.Kernel);
    public static Configuration.ConfigurationLoader spv3 = new Configuration.ConfigurationLoader();

    /**
     * Available HXE kernel modes.
     */
    public enum Mode
    {
      HCE,   /* vanilla hce mode -- won't ever be used by SPV3        */
      SPV32, /* <=SPV3.2 "Legacy" mode, without post-processing       */
      SPV33  /* >=SPV3.3 mode, with post-processing and other goodies */
    }

    /**
     * Infer the mode that the HXE kernel will use in the current environment.
     */
    public static Mode Infer()
    {
      return Exists(HXE.Paths.Version)
        ? Mode.SPV33
        : Mode.SPV32;
    }

    public static void Load()
    {
      hxe.Load();
      Save();
    }

    /// <summary>
    /// Apply SPV3 Loader overrides to the Kernel state 
    /// and save the Kernel state to the file system.
    /// </summary>
    public static void Save()
    {
      SPV3Overrides();
      hxe.Save();
    }

    public static void SPV3Overrides()
    {
      /**
      * SPV3 Loader Overrides
      * Copied from SPV3.Main.Load.Invoke()
      */
      hxe.Mode = HXE.Kernel.Configuration.ConfigurationMode.SPV33;
      hxe.Tweaks.Sensor     = true;          /* forcefully enable motion sensor   */
      hxe.Main.Reset        = true;          /* improve loading stability         */
      hxe.Main.Patch        = true;          /* improve loading stability         */
      hxe.Main.Resume       = true;          /* improve loading stability         */
      hxe.Main.Start        = true;          /* improve loading stability         */
      hxe.Main.Elevated     = spv3.Elevated; /* prevent certain crashes           */
      hxe.Video.Resolution  = true;          /* permit custom resolution override */
      hxe.Video.Quality     = false;         /* permit in-game quality settings   */
      hxe.Video.Uncap       = spv3.Vsync == false;
      hxe.Video.GammaEnabled= spv3.GammaEnabled;
      hxe.Video.Gamma       = spv3.Gamma;
      hxe.Video.Bless       = spv3.Borderless && spv3.Window && spv3.Vsync == false && spv3.Elevated == false;
      hxe.Audio.Enhancements= spv3.EAX;
      hxe.Input.Override    = spv3.Preset;
      hxe.Tweaks.CinemaBars = spv3.CinemaBars;
      hxe.Tweaks.Unload     = !spv3.Shaders;

      if (spv3.Photo && !spv3.DOOM && Exists(Paths.Photo))
        hxe.Tweaks.Sensor = false;
      /// Should this handle OpenSauce and Chimera overrides, too?
    }
  }
}