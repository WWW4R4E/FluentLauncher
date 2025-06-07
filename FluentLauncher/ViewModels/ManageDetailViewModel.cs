using FluentLauncher.Core.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentLauncher.Core.Utils;
using Tomlyn;

namespace FluentLauncher.ViewModels
{
    public partial class ManageDetailViewModel : ObservableObject
    {
        [ObservableProperty] private string? _selectedCardType;

        [ObservableProperty] private DataTemplate? _selectedTemplate;

        [ObservableProperty] private ObservableCollection<object>? _selectedItems;

        [ObservableProperty] private List<DataTemplate> _resources;
        [ObservableProperty] private string? _gamepath;

        // 按需从 Resources 获取模板
        public async Task LoadData(string cardType, string path)
        {
            SelectedCardType = cardType;
            Gamepath = path;

            switch (cardType)
            {
                case "存档管理":
                    SelectedTemplate = Resources[0];
                    SelectedItems = new ObservableCollection<object>(GetSavedGames(path));
                    break;

                case "模组管理":
                    SelectedTemplate = Resources[1];
                    SelectedItems = new ObservableCollection<object>(GetMods(path));
                    break;

                case "启动参数":
                    SelectedTemplate = Resources[2];
                    SelectedItems = new ObservableCollection<object>(await GetGameArgumentsAsync(path));

                    break;

                default:
                    SelectedTemplate = null;
                    SelectedItems = null;
                    break;
            }
        }


        private ObservableCollection<GameSave> GetSavedGames(string path)
        {
            var savePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(path))!, "saves");

            var saves = new ObservableCollection<GameSave>();

            if (Directory.Exists(savePath))
            {
                foreach (var dir in Directory.GetDirectories(savePath))
                {
                    var saveName = new DirectoryInfo(dir).Name;
                    saves.Add(new GameSave { SaveName = saveName, SavePath = dir });
                }
            }

            return saves;
        }

        private ObservableCollection<Mod> GetMods(string path)
        {
            var mods= new ObservableCollection<Mod>();
            var modpath = Path.Combine(Path.GetDirectoryName(path)!, "mods");
            if (Directory.Exists(modpath))
            {
                foreach (var file in Directory.GetFiles(modpath, "*.jar"))
                {
                    try
                    {
                        mods.Add(ModManageCommand.GetModInfoFromJar(file));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        continue;
                    }
                }
                return mods;
            }

            return mods;
        }


        private async Task<ObservableCollection<GameArgument>> GetGameArgumentsAsync(string path)
        {
            var gameArguments = new ObservableCollection<GameArgument>();
            try
            {
                var configModel = await JsonConfigUtils.LoadFromFile();
                var gameArgument = configModel.GameConfig
                    .FirstOrDefault(x => x.GamePath == path)?.GameArguments;

                if (gameArgument != null)
                {
                    gameArguments.Add(gameArgument);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"加载游戏参数失败: {ex.Message}");
            }

            return gameArguments;
        }
    }
}