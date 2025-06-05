    using CommunityToolkit.Mvvm.ComponentModel;
using FluentLauncher.Core.Models;
using FluentLauncher.Core.Utils;
using FluentLauncher.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FluentLauncher.ViewModels;

public partial class ManageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<GameConfig> _versions;
    public ManageViewModel()
    {
    }

    public async void LoadGamesAsync()
    {
        try
        {
            var json = await JsonConfigUtils.LoadFromFile();
            Versions = new ObservableCollection<GameConfig>(json.GameConfig);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"加载游戏配置失败: {ex.Message}");
        }
    }
}