using FluentLauncher.Activation;
using FluentLauncher.Contracts.Services;
using FluentLauncher.Views;
using System.IO;

namespace FluentLauncher.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private UIElement? _shell = null;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        await InitializeAsync();

        if (App.MainWindow.Content == null)
        {
            // 检查配置文件是否存在，判断是否首次安装
            var configFilePath = Path.Combine(AppContext.BaseDirectory, "launcher-config.json");
            if (!File.Exists(configFilePath))
            {
                _shell = App.GetService<InitialSetupPage>();
            }
            else
            {
                _shell = App.GetService<ShellPage>();
            }
            App.MainWindow.Content = _shell ?? new Frame();
            App.MainWindow.ExtendsContentIntoTitleBar = true;
        }

        await HandleActivationAsync(activationArgs);
      
        App.MainWindow.Activate();

        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await Task.CompletedTask;
    }
}
