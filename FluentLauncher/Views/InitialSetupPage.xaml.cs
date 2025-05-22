using FluentLauncher.Controls;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics;
using Windows.Security.Credentials;
using Windows.UI;

namespace FluentLauncher.Views
{
    public sealed partial class InitialSetupPage : Page
    {
        private enum SetupPage
        {
            Language,
            JDK,
            Minecraft,
            Account,
            Finish
        }

        private readonly List<UserControl> _pages = new()
        {
            new LanguageSelectionControl(),
            new JDKPathControl(),
            new MinecraftDirectoryControl(),
            new AccountControl(),
            new FinishSetupControl()
        };

        private SetupPage _currentPage = SetupPage.Language;
        private bool _isTransitioning = true; 

        public InitialSetupPage()
        {
            InitializeComponent();
            ShowPage(SetupPage.Language, true);
        }

        private void ShowPage(SetupPage page, bool isNext = true)
        {

            try
            {
                _isTransitioning = false;
                UpdateButtonStates();
   
                Storyboard slideOutStoryboard = new Storyboard();
                DoubleAnimation slideOut = new DoubleAnimation
                {
                    From = 0,
                    To = isNext ? -200 : 200, 
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(slideOut, ContentHost);
                Storyboard.SetTargetProperty(slideOut, "(UIElement.RenderTransform).(TranslateTransform.X)");
                slideOutStoryboard.Children.Add(slideOut);

                DoubleAnimation fadeOut = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(fadeOut, ContentHost);
                Storyboard.SetTargetProperty(fadeOut, "Opacity");
                slideOutStoryboard.Children.Add(fadeOut);

                slideOutStoryboard.Completed += (s, e) =>
                {
                    Debug.WriteLine($"SlideOut completed, switching to page {page}");
                    _currentPage = page;
                    ContentHost.Content = _pages[(int)page];
                    UpdateTabStates(page);
                    UpdateButtonStates();

                    Storyboard slideInStoryboard = new Storyboard();
                    DoubleAnimation slideIn = new DoubleAnimation
                    {
                        From = isNext ? 200 : -200, 
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
                    };
                    Storyboard.SetTarget(slideIn, ContentHost);
                    Storyboard.SetTargetProperty(slideIn, "(UIElement.RenderTransform).(TranslateTransform.X)");
                    slideInStoryboard.Children.Add(slideIn);

                    DoubleAnimation fadeIn = new DoubleAnimation
                    {
                        From = 0.0,
                        To = 1.0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
                    };
                    Storyboard.SetTarget(fadeIn, ContentHost);
                    Storyboard.SetTargetProperty(fadeIn, "Opacity");
                    slideInStoryboard.Children.Add(fadeIn);

                    // 滑入动画完成后解除锁定
                    slideInStoryboard.Completed += (s2, e2) =>
                    {
                        _isTransitioning = true;
                        UpdateButtonStates();
                    };

                    slideInStoryboard.Begin();
                };

                slideOutStoryboard.Begin();
            
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ShowPage Error: {ex.Message}");
            }
        }

        private void UpdateTabStates(SetupPage currentPage)
        {
            var buttons = new[]
            {
                new { Button = TabLanguage, Page = SetupPage.Language },
                new { Button = TabJDK, Page = SetupPage.JDK },
                new { Button = TabMinecraft, Page = SetupPage.Minecraft },
                new { Button = TabAccount, Page = SetupPage.Account },
                new { Button = TabFinish, Page = SetupPage.Finish }
            };

            foreach (var item in buttons)
            {
                if (item.Button.Content is TextBlock textBlock)
                {
                    textBlock.Foreground = item.Page == currentPage
                        ? new SolidColorBrush(Colors.White)
                        : new SolidColorBrush(Color.FromArgb(255, 153, 153, 153));
                }
            }
        }

        private void OnTabClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && Enum.TryParse(button.Tag?.ToString(), out SetupPage page))
            {
                bool isNext = (int)page > (int)_currentPage;
                ShowPage(page, isNext);
            }
        }

        private void OnPreviousClick(object sender, RoutedEventArgs e)
        {
            if ((int)_currentPage > 0)
            {
                ShowPage((SetupPage)((int)_currentPage - 1), false);
            }
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            if ((int)_currentPage < _pages.Count - 1)
            {
                ShowPage((SetupPage)((int)_currentPage + 1), true);
            }
        }

        private void UpdateButtonStates()
        {
            PreviousButton.IsEnabled = (int)_currentPage > 0 && _isTransitioning;
            NextButton.IsEnabled = (int)_currentPage < _pages.Count - 1 && _isTransitioning;
        }
    }
}