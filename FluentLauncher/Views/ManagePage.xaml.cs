using System.Collections.ObjectModel;

namespace FluentLauncher.Views
{
    public sealed partial class ManagePage : Page
    {
        // 添加一个 ObservableCollection 属性来存储数据
        public ObservableCollection<VersionListDate> Versions { get; set; }

        public ManagePage()
        {
            InitializeComponent();

            // 初始化 Versions 并添加示例数据
            Versions = new ObservableCollection<VersionListDate>
            {
                new VersionListDate
                {
                    GameName = "Minecraft",
                    GameVersion = "1.19.2",
                    IsAddMod = true,
                    ModLoaders = ModLoaders.Forge
                },
                new VersionListDate
                {
                    GameName = "Minecraft",
                    GameVersion = "1.18.2",
                    IsAddMod = false,
                    ModLoaders = ModLoaders.Fabric
                },
                new VersionListDate
                {
                    GameName = "Minecraft",
                    GameVersion = "1.17.1",
                    IsAddMod = true,
                    ModLoaders = ModLoaders.Quilt
                }
            };

            // 将数据绑定到 ListView
            DataContext = this;
        }
    }

    public class VersionListDate
    {
        internal string GameName { get; set; }
        internal string GameVersion { get; set; }
        internal bool IsAddMod { get; set; }
        internal ModLoaders ModLoaders { get; set; }
    }

    public enum ModLoaders
    {
        Forge,
        Fabric,
        Quilt,
        NeoForge
    }
}