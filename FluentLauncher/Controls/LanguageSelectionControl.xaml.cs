using Windows.Globalization;
using FluentLauncher.Views;

namespace FluentLauncher.Controls
{
    public sealed partial class LanguageSelectionControl : UserControl
    {
        public LanguageSelectionControl()
        {
            this.InitializeComponent();
            InitializeLanguageComboBox();
        }

        private void InitializeLanguageComboBox()
        {
            foreach (ComboBoxItem item in LanguageComboBox.Items)
            {
                if (item.Tag.ToString() == "zh-CN")
                {
                    LanguageComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = LanguageComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                var language = selectedItem.Tag.ToString();
                try
                {
                    ApplicationLanguages.PrimaryLanguageOverride = language;
                    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
                    {
                        App.MainWindow.Content = null;
                        App.MainWindow.Content = App.GetService<InitialSetupPage>();
                    });
                }
                catch (InvalidOperationException)
                {
                    // 如果设置语言失败，至少UI已经更新了选择的语言
                    // 可以考虑在这里添加错误提示
                }
            }
        }
    }
}
