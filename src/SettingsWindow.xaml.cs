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
  }
}