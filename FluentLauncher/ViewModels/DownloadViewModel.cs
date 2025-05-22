using CommunityToolkit.Mvvm.ComponentModel;
using FluentLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentLauncher.ViewModels
{
    public partial class DownloadViewModel : ObservableObject
    {
        public List<DownloadData> DownloadList;
        public DownloadViewModel()
        {
            DownloadList = new List<DownloadData>
            {
                new DownloadData
                {
                    gameVersion = "1.20.1",
                    gamedescribe = "最新正式版本 - 完整洞穴与山崖更新",
                    modLoaders = new List<ModLoaders> { ModLoaders.Forge, ModLoaders.Fabric, ModLoaders.Quilt, ModLoaders.NeoForge }
                },
                new DownloadData
                {
                    gameVersion = "1.19.4",
                    gamedescribe = "稳定版本 - 骆驼和竹筏更新",
                    modLoaders = new List<ModLoaders> { ModLoaders.Forge, ModLoaders.Fabric }
                },
                new DownloadData
                {
                    gameVersion = "1.18.2",
                    gamedescribe = "经典版本 - 洞穴与山崖第一部分",
                    modLoaders = new List<ModLoaders> { ModLoaders.Forge, ModLoaders.Fabric }
                },
                new DownloadData
                {
                    gameVersion = "1.16.5",
                    gamedescribe = "经典版本 - 下界更新",
                    modLoaders = new List<ModLoaders> { ModLoaders.Forge, ModLoaders.Fabric }
                },
                new DownloadData
                {
                    gameVersion = "1.12.2",
                    gamedescribe = "怀旧版本 - 经典模组版本",
                    modLoaders = new List<ModLoaders> { ModLoaders.Forge }
                }
            };
        }
    }
}
