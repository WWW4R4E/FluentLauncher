using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentLauncher.Models;

namespace FluentLauncher.ViewModels;

public partial class ManageViewModel : ObservableRecipient
{
    [ObservableProperty] 
    private ObservableCollection<GameData> _versions;
    public ManageViewModel()
    {
        Versions = new ObservableCollection<GameData>
        {
            new ()
            {
                GameName = "Minecraft",
                GameVersion = "1.19.2",
                IsAddMod = true,
                ModLoaders = ModLoaders.Forge
            },
            new ()
            {
                GameName = "Minecraft",
                GameVersion = "1.18.2",
                IsAddMod = false,
                ModLoaders = ModLoaders.Fabric
            },
            new ()
            {
                GameName = "Minecraft",
                GameVersion = "1.17.1",
                IsAddMod = true,
                ModLoaders = ModLoaders.Quilt
            }
        };
    }
}