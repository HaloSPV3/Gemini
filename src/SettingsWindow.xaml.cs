using System.Windows;
using SPV3.CLI;
using static SPV3.CLI.PostProcessing;

namespace SPV3.GUI
{
  public partial class SettingsWindow : Window
  {
    private readonly PostProcessing _postProcessing = (PostProcessing) Paths.Files.PostProcessing;

    public SettingsWindow()
    {
      InitializeComponent();
      Load();
    }

    private void Save(object sender, RoutedEventArgs e)
    {
      var index = int.Parse(PostProcessing.Text);

      switch (index)
      {
        case 0:
          _postProcessing.Mxao              = MxaoOptions.Off;
          _postProcessing.Dof               = DofOptions.Off;
          _postProcessing.MotionBlur        = MotionBlurOptions.Off;
          _postProcessing.DynamicLensFlares = false;
          _postProcessing.Volumetrics       = false;
          _postProcessing.LensDirt          = true;
          break;
        case 1:
          _postProcessing.Mxao              = MxaoOptions.Off;
          _postProcessing.Dof               = DofOptions.Off;
          _postProcessing.MotionBlur        = MotionBlurOptions.Off;
          _postProcessing.DynamicLensFlares = false;
          _postProcessing.Volumetrics       = true;
          _postProcessing.LensDirt          = true;
          break;
        case 2:
          _postProcessing.Mxao              = MxaoOptions.Off;
          _postProcessing.Dof               = DofOptions.Low;
          _postProcessing.MotionBlur        = MotionBlurOptions.BuiltIn;
          _postProcessing.DynamicLensFlares = false;
          _postProcessing.Volumetrics       = true;
          _postProcessing.LensDirt          = true;
          break;
        case 3:
          _postProcessing.Mxao              = MxaoOptions.Low;
          _postProcessing.Dof               = DofOptions.Low;
          _postProcessing.MotionBlur        = MotionBlurOptions.BuiltIn;
          _postProcessing.DynamicLensFlares = false;
          _postProcessing.Volumetrics       = true;
          _postProcessing.LensDirt          = true;
          break;
        case 4:
          _postProcessing.Mxao              = MxaoOptions.Low;
          _postProcessing.Dof               = DofOptions.Low;
          _postProcessing.MotionBlur        = MotionBlurOptions.PombLow;
          _postProcessing.DynamicLensFlares = true;
          _postProcessing.Volumetrics       = true;
          _postProcessing.LensDirt          = true;
          break;
        case 5:
          _postProcessing.Mxao              = MxaoOptions.Low;
          _postProcessing.Dof               = DofOptions.High;
          _postProcessing.MotionBlur        = MotionBlurOptions.PombLow;
          _postProcessing.DynamicLensFlares = true;
          _postProcessing.Volumetrics       = true;
          _postProcessing.LensDirt          = true;
          break;
        case 6:
          _postProcessing.Mxao              = MxaoOptions.High;
          _postProcessing.Dof               = DofOptions.High;
          _postProcessing.MotionBlur        = MotionBlurOptions.PombLow;
          _postProcessing.DynamicLensFlares = true;
          _postProcessing.Volumetrics       = true;
          _postProcessing.LensDirt          = true;
          break;
        case 7:
          _postProcessing.Mxao              = MxaoOptions.High;
          _postProcessing.Dof               = DofOptions.High;
          _postProcessing.MotionBlur        = MotionBlurOptions.PombHigh;
          _postProcessing.DynamicLensFlares = true;
          _postProcessing.Volumetrics       = true;
          _postProcessing.LensDirt          = true;
          break;
        default:
          _postProcessing.Mxao              = MxaoOptions.Off;
          _postProcessing.Dof               = DofOptions.Off;
          _postProcessing.MotionBlur        = MotionBlurOptions.Off;
          _postProcessing.DynamicLensFlares = false;
          _postProcessing.Volumetrics       = false;
          _postProcessing.LensDirt          = true;
          break;
      }

      _postProcessing.Save();
      
      Close();
    }

    private void Load()
    {
      /**
       * Encodes the post-processing to an integer. This is the exact same logic used in the SPV3.CLI Initiation module!
       */

      int GetPostProcessing()
      {
        var mxao = _postProcessing.Mxao;
        var dof  = _postProcessing.Dof;
        var mb   = _postProcessing.MotionBlur;
        var lf   = _postProcessing.DynamicLensFlares;
        var vol  = _postProcessing.Volumetrics;
        var ld   = _postProcessing.LensDirt;

        if (mxao == MxaoOptions.Off && dof == DofOptions.Off && mb == MotionBlurOptions.Off &&
            lf   == false           && vol == false          && ld)
          return 0;

        if (mxao == MxaoOptions.Off && dof == DofOptions.Off && mb == MotionBlurOptions.Off &&
            lf   == false           && vol                   && ld)
          return 1;

        if (mxao == MxaoOptions.Off && dof == DofOptions.Low && mb == MotionBlurOptions.BuiltIn &&
            lf   == false           && vol                   && ld)
          return 2;

        if (mxao == MxaoOptions.Low && dof == DofOptions.Low && mb == MotionBlurOptions.BuiltIn &&
            lf   == false           && vol                   && ld)
          return 3;

        if (mxao == MxaoOptions.Low && dof == DofOptions.Low && mb == MotionBlurOptions.PombLow &&
            lf                      && vol                   && ld)
          return 4;

        if (mxao == MxaoOptions.Low && dof == DofOptions.High && mb == MotionBlurOptions.PombLow &&
            lf                      && vol                    && ld)
          return 5;

        if (mxao == MxaoOptions.High && dof == DofOptions.High && mb == MotionBlurOptions.PombLow &&
            lf                       && vol                    && ld)
          return 6;

        if (mxao == MxaoOptions.High && dof == DofOptions.High && mb == MotionBlurOptions.PombHigh &&
            lf                       && vol                    && ld)
          return 7;

        return 0;
      }

      if (!_postProcessing.Exists()) return;

      _postProcessing.Load();

      PostProcessingSlider.Value = GetPostProcessing();
    }
  }
}