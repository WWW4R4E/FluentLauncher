using FluentLauncher.Helpers;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;

namespace FluentLauncher;

public sealed partial class MainWindow
{
    public string Title { get; }

    public MainWindow()
    {
        InitializeComponent();
        AppWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Title = "AppDisplayName".GetLocalized();
        Content = null;
        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.PreferredMinimumWidth = 950;
            presenter.PreferredMinimumHeight = 700;
        }
        //SetWindowMinSizeWithDpiScaling(950, 700);
    }
    //private void SetWindowMinSizeWithDpiScaling(double minWidthDip, double minHeightDip)
    //{
    //    // 获取当前窗口的 AppWindow
    //    var appWindow = AppWindow;

    //    // 获取当前显示器的 DPI 缩放比例
    //    double dpiScale = GetDpiScale();

    //    // 将逻辑像素 (DIP) 转换为物理像素
    //    int minWidthPhysical = (int)(minWidthDip * dpiScale);
    //    int minHeightPhysical = (int)(minHeightDip * dpiScale);

    //    // 设置最小尺寸
    //    if (appWindow.Presenter is OverlappedPresenter presenter)
    //    {
    //        // 使用 SetMinSize（推荐，Windows App SDK 1.6+）
    //        presenter.PreferredMinimumWidth =minWidthPhysical;
    //        presenter.PreferredMinimumHeight = minHeightPhysical;
    //    }
    //}


    //private double GetDpiScale()
    //{
    //    // 获取当前窗口的句柄
    //    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

    //    // 使用 DisplayInformation 获取 DPI 缩放比例
    //    var displayInfo = DisplayInformation.GetForCurrentView();
    //    return displayInfo.RawPixelsPerViewPixel; 
    //}

}
