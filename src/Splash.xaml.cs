using System.Windows;

namespace SPV3
{
  /// <summary>
  /// Interaction logic for Splash.xaml
  /// </summary>
  public partial class Splash : Window
  {
    public Splash()
    {
      WindowStartupLocation = WindowStartupLocation.CenterScreen;
      InitializeComponent();
    }

    public void QueueSplashClose()
    {
      System.Threading.Thread.Sleep(2000);
      Close();
    }
  }
}
