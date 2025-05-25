using CommunityToolkit.Mvvm.ComponentModel;
using FluentLauncher.Activation;
using FluentLauncher.Contracts.Services;
using FluentLauncher.Notifications;
using FluentLauncher.Services;
using FluentLauncher.ViewModels;
using FluentLauncher.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
// using FluentLauncher.Core.Contracts.Services;
// using FluentLauncher.Core.Services;

namespace FluentLauncher;

public partial class App : Application
{
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static Window MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddSingleton<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddSingleton<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services

            // Views and ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainPage>();
            services.AddSingleton<ShellPage>();
            services.AddSingleton<ShellViewModel>();
            services.AddSingleton<ManagePage>();
            services.AddSingleton<ManageViewModel>();
            services.AddSingleton<ManageDetailPage>();
            services.AddSingleton<ManageDetailViewModel>();
            services.AddSingleton<InitialSetupViewModel>();
            services.AddSingleton<InitialSetupPage>();
            services.AddSingleton<DownloadPage>();
            services.AddSingleton<DownloadViewModel>();
            // Configuration
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // 记录异常信息
        Debug.WriteLine($"Unhandled Exception: {e.Exception.Message}");
        Debug.WriteLine($"Exception Stack Trace: {e.Exception.StackTrace}");

        // 显示错误消息（可选）
        // MessageBox.Show($"发生错误: {e.Exception.Message}");

        // 防止应用直接退出（根据需要）
        e.Handled = true;
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        //App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
