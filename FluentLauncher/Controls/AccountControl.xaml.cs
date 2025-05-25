

namespace FluentLauncher.Controls
{
    public partial class AccountControl : UserControl
    {
        public string UUid { get; private set; }
        public string UserName { get; private set; }
        public string? AccessToken { get; private set; }
        public AccountControl()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "登录微软正版账户";
            dialog.PrimaryButtonText = "登录";
            dialog.SecondaryButtonText = "取消";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new LoginContentDialog();

            var result = await dialog.ShowAsync();

        }
    }
}
