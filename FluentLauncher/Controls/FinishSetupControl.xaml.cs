using FluentLauncher.Core.Commands;
using FluentLauncher.Views;

namespace FluentLauncher.Controls
{
    public sealed partial class FinishSetupControl : UserControl
    {
        public FinishSetupControl()
        {
            InitializeComponent();
        }
        public void OnFinishButtonClick(object sender, RoutedEventArgs e)
        {
            var page = App.GetService<InitialSetupPage>();
            var (jdkPath, mcDir, username, accesstoken, uuid) = App.GetService<InitialSetupPage>().GetSetupData();
           InitCommand.InitializeAsync(jdkPath, mcDir, username, accesstoken, uuid).GetAwaiter().GetResult();
            App.MainWindow.Content = App.GetService<ShellPage>();


        }
    }
}
