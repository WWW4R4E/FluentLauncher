using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentLauncher.Core.Commands;
using FluentLauncher.Models;
using System.Text.Json;
using FluentLauncher.Core.Models;
using FluentLauncher.Core.Utils;
using CommunityToolkit.Mvvm.Input;

namespace FluentLauncher.ViewModels
{
    public partial class DownloadViewModel : ObservableObject
    {
        [ObservableProperty] private List<DownloadData> _downloadList = new();
        [ObservableProperty] private List<DownloadData> _filteredList;
        [ObservableProperty] private string _selectedType;

        private readonly Dictionary<string, string> _typeMapping = new()
            { { "正式版", "release" }, { "快照版", "snapshot" }, { "远古版β", "old_beta" }, { "远古版α", "old_alpha" } };

        public List<string> Types { get; } = new() { "正式版", "快照版", "远古版β", "远古版α" };

        public DownloadViewModel()
        {
        }

        partial void OnSelectedTypeChanged(string oldValue, string newValue)
        {
            UpdateFilteredList();
        }

        private void UpdateFilteredList()
        {
            FilteredList =
                DownloadList.Where(d => d.type == _typeMapping[SelectedType]).ToList();
            Debug.Write(FilteredList.Count);
        }

        [RelayCommand]
        public async Task LoadVersionsAsync()
        {
            try
            {
                var versionJson = await GetVersionMainfestAsync();
                DownloadList = versionJson;
                SelectedType = "正式版";
            }
            catch (Exception ex)
            {
                // 添加错误处理逻辑
                Console.WriteLine($"加载版本失败: {ex.Message}");
            }
        }

        internal async Task<List<DownloadData>> GetVersionMainfestAsync()
        {
            var versionpath = Path.Combine(Properties.Settings.Default.Applocal, "version.json");
            if (File.Exists(versionpath))
            {
                string json = await File.ReadAllTextAsync(versionpath);
                VersionsJson? versionJson =
                    JsonSerializer.Deserialize(json, AppJsonSerializerContext.Default.VersionsJson);
                return versionJson!.Versions
                    .Select(v => new DownloadData
                    {
                        gameVersion = v.Id,
                        type = v.Type,
                        gamedescribe = v.Type,
                        modLoaders = new List<ModLoaders>()
                    }).ToList();
            }

            string VersionManifestUrl = "https://piston-meta.mojang.com/mc/game/version_manifest.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(VersionManifestUrl);
            File.WriteAllText("version_manifest.json", response);
            var manifest = await InstallCommand.GetVersionManifestAsync();
            await JsonConfigUtils.SaveToFileAsync(manifest.Value, versionpath);
            return manifest.Value.Versions
                .Select(v => new DownloadData
                {
                    gameVersion = v.Id,
                    gamedescribe = v.Type,
                    modLoaders = new List<ModLoaders>()
                }).ToList();
        }
    }
}