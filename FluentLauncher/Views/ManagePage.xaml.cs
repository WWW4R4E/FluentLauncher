using FluentLauncher.Contracts.Services;
using FluentLauncher.Services;
using FluentLauncher.ViewModels;

namespace FluentLauncher.Views
{

    public sealed partial class ManagePage : Page
    {

        public ManageViewModel ViewModel;
        public ManagePage()
        {
            ViewModel = App.GetService<ManageViewModel>();

            InitializeComponent();
        }

        private void SettingsCard_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = App.GetService<INavigationService>();
            string pageKey = typeof(ManageDetailViewModel).FullName;
            navigationService.NavigateTo(pageKey);

        }
    }
}