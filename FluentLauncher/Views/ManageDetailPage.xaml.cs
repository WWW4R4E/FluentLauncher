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
    }
}