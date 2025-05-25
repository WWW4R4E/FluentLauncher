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

            // Fix for CS0029: Explicitly check if e.Parameter is a non-empty string
            if (e.Parameter is string parameter && !string.IsNullOrEmpty(parameter))
            {
                var L = new List<DataTemplate>()
                {
                    Resources["GameSaveListViewTemplate"] as DataTemplate,
                    Resources["ModListViewTemplate"] as DataTemplate,
                    Resources["GameArgumentListViewTemplate"] as DataTemplate
                };
                ViewModel.Resources = L;
                DispatcherQueue.TryEnqueue(() => { ViewModel.LoadData(parameter); });
            }
        }
    }
}