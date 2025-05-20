using FluentLauncher.Controls;
using FluentLauncher.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using Windows.UI;

namespace FluentLauncher.Views
{
    public sealed partial class InitialSetupPage : Page
    {
        private int _currentIndex = 0;
        private readonly List<UserControl> _pages =
        [
            new LanguageSelectionControl(),
            new MinecraftDirectoryControl(),
            new AccountControl(),
            new JDKPathControl()
        ];

        public InitialSetupPage()
        {
            InitializeComponent();
            ShowCurrentPage();
            UpdateButtonStates();
            UpdatepTabStatu();
        }

        private void ShowCurrentPage()
        {
            if (_currentIndex >= 0 && _currentIndex < _pages.Count)
            {
                ContentHost.Content = _pages[_currentIndex];
            }
        }
        private void UpdatepTabStatu()
        {
            // 重置所有文本颜色为灰色
            ((TextBlock)((Button)TabLanguage).Content).Foreground = new SolidColorBrush(Color.FromArgb(255, 153, 153, 153));
            ((TextBlock)((Button)TabMinecraft).Content).Foreground = new SolidColorBrush(Color.FromArgb(255, 153, 153, 153));
            ((TextBlock)((Button)TabAccount).Content).Foreground = new SolidColorBrush(Color.FromArgb(255, 153, 153, 153));
            ((TextBlock)((Button)TabJDK).Content).Foreground = new SolidColorBrush(Color.FromArgb(255, 153, 153, 153));
            switch (_currentIndex)
            {
                case 0:
                    ((TextBlock)TabLanguage.Content).Foreground = new SolidColorBrush(Colors.White);
                    break;
                case 1:
                    ((TextBlock)TabMinecraft.Content).Foreground = new SolidColorBrush(Colors.White);
                    break;
                case 2:
                    ((TextBlock)TabAccount.Content).Foreground = new SolidColorBrush(Colors.White);
                    break;
                case 3:
                    ((TextBlock)TabJDK.Content).Foreground = new SolidColorBrush(Colors.White);
                    break;
            }
        }

        private void OnTabClick(object sender, RoutedEventArgs e)
        {
           

            if (sender is Button button && int.TryParse(button.Tag?.ToString(), out int index))
            {
                // 设置当前选中项为白色
                _currentIndex = index;
                UpdatepTabStatu();
                ShowCurrentPage();
                UpdateButtonStates();
            }
        }


        private void OnPreviousClick(object sender, RoutedEventArgs e)
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                UpdatepTabStatu();
                ShowCurrentPage();
                UpdateButtonStates();
            }
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < _pages.Count - 1)
            {
                _currentIndex++;
                ShowCurrentPage();
                UpdatepTabStatu();
                UpdateButtonStates();
            }
        }

        private void UpdateButtonStates()
        {
            PreviousButton.IsEnabled = _currentIndex > 0;
            NextButton.IsEnabled = _currentIndex < _pages.Count - 1;
        }
    }
}