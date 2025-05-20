using FluentLauncher.ViewModels;
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


namespace FluentLauncher.Views
{
    public sealed partial class ManageDetailPage : Page
    {
        public ManageDetailViewModel ViewModel {get;}
        public ManageDetailPage()
        {
            ViewModel = App.GetService<ManageDetailViewModel>();
            InitializeComponent();

        }
    }
}
