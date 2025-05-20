using CommunityToolkit.Mvvm.ComponentModel;
using FluentLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentLauncher.ViewModels
{
    public partial class ManageDetailViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private List<SavedGame> savedGames;
        public ManageDetailViewModel()
        {
            savedGames = new List<SavedGame>()
            {
                new SavedGame 
                { 
                    FiledName = "我的生存世界", 
                    FildePathName = "survival_world_001",
                    FiledDescription = "创建于2025年的生存世界"
                },
                new SavedGame 
                { 
                    FiledName = "创造模式", 
                    FildePathName = "creative_world_001",
                    FiledDescription = "用于建筑测试的创造世界"
                },
                new SavedGame 
                { 
                    FiledName = "红石测试", 
                    FildePathName = "redstone_test",
                    FiledDescription = "红石电路实验场"
                }
            };
        }
    }
}
