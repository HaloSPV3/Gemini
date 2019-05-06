using System.Windows;

namespace HXE
{
  /// <summary>
  /// Interaction logic for Settings.xaml
  /// </summary>
  public partial class Settings
  {
    private readonly Configuration _configuration = (Configuration) Paths.Files.Configuration;

    public Settings()
    {
      InitializeComponent();

      if (!_configuration.Exists())
        _configuration.Save();

      Console.Info("Loading kernel settings ...");

      _configuration.Load();

      EnableSpv3KernelMode.IsChecked = _configuration.Kernel.EnableSpv3KernelMode;
      EnableSpv3LegacyMode.IsChecked = _configuration.Kernel.EnableSpv3LegacyMode;
      SkipVerifyMainAssets.IsChecked = _configuration.Kernel.SkipVerifyMainAssets;
      SkipInvokeCoreTweaks.IsChecked = _configuration.Kernel.SkipInvokeCoreTweaks;
      SkipResumeCheckpoint.IsChecked = _configuration.Kernel.SkipResumeCheckpoint;
      SkipSetShadersConfig.IsChecked = _configuration.Kernel.SkipSetShadersConfig;
      SkipInvokeExecutable.IsChecked = _configuration.Kernel.SkipInvokeExecutable;
      SkipPatchLargeAAware.IsChecked = _configuration.Kernel.SkipPatchLargeAAware;

      PrintConfiguration();
    }

    private void Save(object sender, RoutedEventArgs e)
    {
      Console.Info("Saving kernel settings ...");
      
      _configuration.Kernel.EnableSpv3KernelMode = EnableSpv3KernelMode.IsChecked == true;
      _configuration.Kernel.EnableSpv3LegacyMode = EnableSpv3LegacyMode.IsChecked == true;
      _configuration.Kernel.SkipVerifyMainAssets = SkipVerifyMainAssets.IsChecked == true;
      _configuration.Kernel.SkipInvokeCoreTweaks = SkipInvokeCoreTweaks.IsChecked == true;
      _configuration.Kernel.SkipResumeCheckpoint = SkipResumeCheckpoint.IsChecked == true;
      _configuration.Kernel.SkipSetShadersConfig = SkipSetShadersConfig.IsChecked == true;
      _configuration.Kernel.SkipInvokeExecutable = SkipInvokeExecutable.IsChecked == true;
      _configuration.Kernel.SkipPatchLargeAAware = SkipPatchLargeAAware.IsChecked == true;

      _configuration.Save();

      PrintConfiguration();

      Exit.WithCode(Exit.Code.Success);
    }

    private void PrintConfiguration()
    {
      Console.Debug("Kernel.EnableSpv3KernelMode - " + _configuration.Kernel.EnableSpv3KernelMode);
      Console.Debug("Kernel.EnableSpv3LegacyMode - " + _configuration.Kernel.EnableSpv3LegacyMode);
      Console.Debug("Kernel.SkipVerifyMainAssets - " + _configuration.Kernel.SkipVerifyMainAssets);
      Console.Debug("Kernel.SkipInvokeCoreTweaks - " + _configuration.Kernel.SkipInvokeCoreTweaks);
      Console.Debug("Kernel.SkipResumeCheckpoint - " + _configuration.Kernel.SkipResumeCheckpoint);
      Console.Debug("Kernel.SkipSetShadersConfig - " + _configuration.Kernel.SkipSetShadersConfig);
      Console.Debug("Kernel.SkipInvokeExecutable - " + _configuration.Kernel.SkipInvokeExecutable);
      Console.Debug("Kernel.SkipPatchLargeAAware - " + _configuration.Kernel.SkipPatchLargeAAware);
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
      Exit.WithCode(Exit.Code.Success);
    }
  }
}