using static System.IO.File;

namespace SPV3
{
  /**
   * Object representing the HXE Kernel.
   */
  public static class Kernel
  {
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
  }
}