using FluentLauncher.ViewModels;


namespace FluentLauncher.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel{get;}

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
        Loaded += MainPageLoaded;
    }

    internal void MainPageLoaded(object sender, RoutedEventArgs e)
    {
        GameSelectButton.Translation += new System.Numerics.Vector3(0, 0, 16);
        UserSelectButton.Translation += new System.Numerics.Vector3(0, 0, 16);
    }
}
