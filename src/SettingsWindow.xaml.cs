using System.Windows;

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
      _settings.Level = 0;
    }

    private void SetPreset1(object sender, RoutedEventArgs e)
    {
      _settings.Level = 1;
    }

    private void SetPreset2(object sender, RoutedEventArgs e)
    {
      _settings.Level = 2;
    }

    private void SetPreset3(object sender, RoutedEventArgs e)
    {
      _settings.Level = 3;
    }

    private void SetPreset4(object sender, RoutedEventArgs e)
    {
      _settings.Level = 4;
    }

    private void SetPreset5(object sender, RoutedEventArgs e)
    {
      _settings.Level = 5;
    }

    private void SetPreset6(object sender, RoutedEventArgs e)
    {
      _settings.Level = 6;
    }

    private void SetPreset7(object sender, RoutedEventArgs e)
    {
      _settings.Level = 7;
    }

    private void SetPreset8(object sender, RoutedEventArgs e)
    {
      _settings.Level = 8;
    }
  }
}