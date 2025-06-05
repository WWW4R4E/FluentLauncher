using CommunityToolkit.WinUI.Controls;
using FluentLauncher.Contracts.Services;
using FluentLauncher.Core.Models;
using FluentLauncher.Models;
using FluentLauncher.ViewModels;
using Microsoft.UI.Xaml.Media;
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

            if (sender is SettingsCard settingsCard)
            {
                var parent = VisualTreeHelper.GetParent(settingsCard);
                if (parent is FrameworkElement frameworkElement &&
                    frameworkElement.DataContext is GameConfig gameConfig)
                {
                    // 获取到当前 ListView 的数据项
                    string gamePath = gameConfig.GamePath;
                    string cardType = settingsCard.Header as string;

                    // 导航并传递参数
                    navigationService.NavigateTo(pageKey, new NavigationParameter{ CardType = cardType, Path = gamePath });
                }
            }
        }
    }
}