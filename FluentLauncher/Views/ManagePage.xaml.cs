using CommunityToolkit.WinUI.Controls;
using FluentLauncher.Contracts.Services;
using FluentLauncher.ViewModels;
using Microsoft.UI.Xaml.Navigation;

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


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadGamesAsync();
        }

        private void SettingsCard_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = App.GetService<INavigationService>();
            string pageKey = typeof(ManageDetailViewModel).FullName!;

            // 获取点击的卡片类型
            string cardType = ((sender as SettingsCard)?.Header as string) ?? "Unknown";
            navigationService.NavigateTo(pageKey, cardType);
        }
    }
}