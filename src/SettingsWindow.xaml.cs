using System.Diagnostics;
using System.Windows;
using HXE.SPV3;

namespace SPV3
{
  public partial class SettingsWindow : Window
  {
    private readonly Settings _settings;

    public SettingsWindow()
    {
      InitializeComponent();
      _settings = (Settings) DataContext;
      _settings.Load();

      RefreshPresetButtons();
    }

    private void Save(object sender, RoutedEventArgs e)
    {
      _settings.Save();
      Close();
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void SetPreset0(object sender, RoutedEventArgs e)
    {
      SetPresetLevel(0);
    }

    private void SetPreset1(object sender, RoutedEventArgs e)
    {
      SetPresetLevel(1);
    }

    private void SetPreset2(object sender, RoutedEventArgs e)
    {
      SetPresetLevel(2);
    }

    private void SetPreset3(object sender, RoutedEventArgs e)
    {
      SetPresetLevel(3);
    }

    private void SetPreset4(object sender, RoutedEventArgs e)
    {
      SetPresetLevel(4);
    }

    private void SetPreset5(object sender, RoutedEventArgs e)
    {
      SetPresetLevel(5);
    }

    private void SetPreset6(object sender, RoutedEventArgs e)
    {
      SetPresetLevel(6);
    }

    private void SetPreset7(object sender, RoutedEventArgs e)
    {
      SetPresetLevel(7);
    }

    private void SetPreset8(object sender, RoutedEventArgs e)
    {
      SetPresetLevel(8);
    }

    private void SetPresetLevel(int level)
    {
      _settings.Main.Level = level;
      RefreshPresetButtons();
    }

    private void RefreshPresetButtons()
    {
      Preset0Button.IsEnabled = true;
      Preset1Button.IsEnabled = true;
      Preset2Button.IsEnabled = true;
      Preset3Button.IsEnabled = true;
      Preset4Button.IsEnabled = true;
      Preset5Button.IsEnabled = true;
      Preset6Button.IsEnabled = true;
      Preset7Button.IsEnabled = true;
      Preset8Button.IsEnabled = true;

      var mxao = _settings.Main.Mxao;
      var dof  = _settings.Main.Dof;
      var mb   = _settings.Main.MotionBlur;
      var lf   = _settings.Main.DynamicLensFlares;
      var vol  = _settings.Main.Volumetrics;
      var ld   = _settings.Main.LensDirt;

      if (mxao  == PostProcessing.MxaoOptions.Off && dof == PostProcessing.DofOptions.Off &&
          mb    == PostProcessing.MotionBlurOptions.Off
          && lf == false && vol == false && ld)
        Preset0Button.IsEnabled = false;

      if (mxao  == PostProcessing.MxaoOptions.Off && dof == PostProcessing.DofOptions.Off &&
          mb    == PostProcessing.MotionBlurOptions.Off
          && lf == false && vol && ld)
        Preset1Button.IsEnabled = false;

      if (mxao  == PostProcessing.MxaoOptions.Off && dof == PostProcessing.DofOptions.Low &&
          mb    == PostProcessing.MotionBlurOptions.BuiltIn
          && lf == false && vol && ld)
        Preset2Button.IsEnabled = false;

      if (mxao  == PostProcessing.MxaoOptions.Low && dof == PostProcessing.DofOptions.Low &&
          mb    == PostProcessing.MotionBlurOptions.BuiltIn
          && lf == false && vol && ld)
        Preset3Button.IsEnabled = false;

      if (mxao == PostProcessing.MxaoOptions.Low && dof == PostProcessing.DofOptions.Low &&
          mb   == PostProcessing.MotionBlurOptions.PombLow
          && lf && vol && ld)
        Preset4Button.IsEnabled = false;

      if (mxao == PostProcessing.MxaoOptions.Low && dof == PostProcessing.DofOptions.High &&
          mb   == PostProcessing.MotionBlurOptions.PombLow
          && lf && vol && ld)
        Preset5Button.IsEnabled = false;

      if (mxao == PostProcessing.MxaoOptions.High && dof == PostProcessing.DofOptions.High &&
          mb   == PostProcessing.MotionBlurOptions.PombLow
          && lf && vol && ld)
        Preset6Button.IsEnabled = false;

      if (mxao == PostProcessing.MxaoOptions.High && dof == PostProcessing.DofOptions.High &&
          mb   == PostProcessing.MotionBlurOptions.PombHigh
          && lf && vol && ld)
        Preset7Button.IsEnabled = false;

      if (mxao  == PostProcessing.MxaoOptions.Off && dof == PostProcessing.DofOptions.Off &&
          mb    == PostProcessing.MotionBlurOptions.Off
          && lf == false && vol == false && ld == false)
        Preset8Button.IsEnabled = false;
    }

    private void GetAmaiSosu(object sender, RoutedEventArgs e)
    {
      Process.Start("https://github.com/yumiris/AmaiSosu");
    }

    private void CalculateFOV(object sender, RoutedEventArgs e)
    {
      _settings.OpenSauce.CalculateFOV();
    }
  }
}