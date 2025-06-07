using FluentLauncher.Core.Models;
using FluentLauncher.Models;
using FluentLauncher.ViewModels;
using Microsoft.UI.Xaml.Navigation;


namespace FluentLauncher.Views
{
    public sealed partial class ManageDetailPage : Page
    {
        public ManageDetailViewModel ViewModel { get; }

        public ManageDetailPage()
        {
            ViewModel = App.GetService<ManageDetailViewModel>();
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is NavigationParameter parameter &&
                !string.IsNullOrEmpty(parameter.CardType) &&
                !string.IsNullOrEmpty(parameter.Path))
            {
                ViewModel.Resources = new List<DataTemplate>()
                {
                    Resources["GameSaveListViewTemplate"] as DataTemplate,
                    Resources["ModListViewTemplate"] as DataTemplate,
                    Resources["GameArgumentListViewTemplate"] as DataTemplate
                };
                DispatcherQueue.TryEnqueue(() => { ViewModel.LoadData(parameter.CardType, parameter.Path); });
            }
        }

        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                var mod = toggleSwitch.DataContext as Mod;
                if (mod != null)
                {
                    if (mod.IsEnabled)
                    {
                        mod.IsEnabled = false;
                        File.Move(mod.ModPath, mod.ModPath.Replace(".jar", ".jar.disable"));
                        mod.ModPath = mod.ModPath.Replace(".jar", ".jar.disable");
                    }
                    else
                    {
                        mod.IsEnabled = true;
                        File.Move(mod.ModPath, mod.ModPath.Replace(".jar.disable", ".jar"));
                        mod.ModPath = mod.ModPath.Replace(".jar.disable", ".jar");
                    }
                }
            }
        }
    }
}