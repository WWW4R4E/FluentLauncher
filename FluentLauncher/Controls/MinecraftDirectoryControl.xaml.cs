

// Minecraft目录选择控件代码
using Windows.Storage;
using Windows.Storage.Pickers;

namespace FluentLauncher.Controls
{
    public sealed partial class MinecraftDirectoryControl : UserControl
    {
        public string SelectedMinecraftDir { get; private set; }

        public MinecraftDirectoryControl()
        {
            this.InitializeComponent();
        }

        private void DefaultButton_OnClick(object sender, RoutedEventArgs e)
        {
            // 获取用户目录下的 AppData\Roaming\.minecraft 路径
            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string minecraftPath = Path.Combine(roamingPath, ".minecraft");

            // 设置文本框的文本
            MinecraftPathText.Text = minecraftPath;
            SelectedMinecraftDir = MinecraftPathText.Text;
        }

        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {

            FolderPicker openPicker = new();
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await openPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                MinecraftPathText.Text = folder.Path;
                SelectedMinecraftDir = MinecraftPathText.Text;
            }
        }
    }
}