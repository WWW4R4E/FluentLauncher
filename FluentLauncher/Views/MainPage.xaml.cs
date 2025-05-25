using FluentLauncher.Core.Commands;
using FluentLauncher.ViewModels;
using Microsoft.UI.Xaml.Navigation;


namespace FluentLauncher.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
        Loaded += MainPageLoaded;
    }
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ScanCommand scan = new();
        await scan.ScanVersionsAsync();
        await ViewModel.LoadGameListAsync();
    }


    internal void MainPageLoaded(object sender, RoutedEventArgs e)
    {
        GameSelectButton.Translation += new System.Numerics.Vector3(0, 0, 16);
        UserSelectButton.Translation += new System.Numerics.Vector3(0, 0, 16);
        if (Properties.Settings.Default?.LastSelectGame != null)
        {
            ViewModel.SelectedGame = ViewModel.GameList.FirstOrDefault(x => x.GamePath == Properties.Settings.Default.LastSelectGame);
        }
        ViewModel.InitializeViewModel();
    }
}
