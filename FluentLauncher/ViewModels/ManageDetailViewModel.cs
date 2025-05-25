using FluentLauncher.Core.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FluentLauncher.ViewModels
{
    public partial class ManageDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _selectedCardType;

        [ObservableProperty]
        private DataTemplate? _selectedTemplate;

        [ObservableProperty]
        private ObservableCollection<object>? _selectedItems;

        [ObservableProperty] private List<DataTemplate> _resources;

        // 按需从 Resources 获取模板
        public void LoadData(string cardType)
        {
            SelectedCardType = cardType;

            switch (cardType)
            {
                case "存档管理":
                    SelectedTemplate = Resources[0];
                    SelectedItems = new ObservableCollection<object>(GetSavedGames());
                    break;

                case "模组管理":
                    SelectedTemplate = Resources[1];
                    SelectedItems = new ObservableCollection<object>(GetMods());
                    break;

                case "启动参数":
                    SelectedTemplate = Resources[2];
                    SelectedItems = new ObservableCollection<object>(GetGameArguments());
                    break;

                default:
                    SelectedTemplate = null;
                    SelectedItems = null;
                    break;
            }
        }


        private ObservableCollection<GameSave> GetSavedGames()
        {
            // 示例数据
            return new ObservableCollection<GameSave>
            {
                new GameSave { SaveName = "存档1", SavePath = "C:\\Saves\\Save1", SaveTime = Convert.ToDateTime("2023-10-01") },
                new GameSave { SaveName = "存档2", SavePath = "C:\\Saves\\Save2", SaveTime = Convert.ToDateTime("2023-10-02") }
            };
        }

        private ObservableCollection<Mod> GetMods()
        {
            // 示例数据
            return new ObservableCollection<Mod>
            {
                new() { ModName = "模组1", ModPath = "C:\\Mods\\Mod1", ModVersion = "1.0", ModType = "Core" },
                new() { ModName = "模组2", ModPath = "C:\\Mods\\Mod2", ModVersion = "2.0", ModType = "Plugin" }
            };
        }

        private ObservableCollection<GameArgument> GetGameArguments()
        {
            // 示例数据
            return new ObservableCollection<GameArgument>
            {
                new() { FullScrean = true, javaPath = "C:\\Java\\jdk-17", memoryArgs = "-Xmx4G", GameSize= new Tuple<uint, uint>(900,300) },
                new(){ FullScrean = false, javaPath = "C:\\Java\\jdk-11", memoryArgs = "-Xmx2G", GameSize = new Tuple<uint, uint>(900,300) }
            };
        }
    }
}