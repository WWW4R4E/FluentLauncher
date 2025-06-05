using FluentLauncher.ViewModels;
using Microsoft.UI.Xaml.Navigation;

namespace FluentLauncher.Views
{
    public sealed partial class DownloadPage : Page
    {
       public DownloadViewModel ViewModel { get; }
        public DownloadPage()
        {
            ViewModel = App.GetService<DownloadViewModel>();
            InitializeComponent();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ViewModel.DownloadList.Count <= 10)
            {
                ViewModel.LoadVersionsAsync();
            }
      
        }
    }
}
