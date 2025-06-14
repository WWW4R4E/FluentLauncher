using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentLauncher.Controls
{
    public sealed partial class LoginContentDialog : UserControl
    {
        public LoginContentDialog()
        {
            InitializeComponent();
            button.Content = "离线登录";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Password.Visibility = Password.Visibility.Equals(Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            button.Content = Password.Visibility.Equals(Visibility.Visible) ? "离线登录" : "在线登录";

        }
    }
}
