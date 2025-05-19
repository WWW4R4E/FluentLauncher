### `NavigationView` 部分的运作机制

#### 1. 接口定义
- **`INavigationService`**：定义了导航服务的基本功能，包括导航事件、返回功能、框架属性以及导航方法。
  - `Navigated` 事件：当导航发生时触发。
  - `CanGoBack` 属性：指示是否可以返回上一页。
  - `Frame` 属性：获取或设置用于导航的框架。
  - `NavigateTo` 方法：导航到指定页面。
  - `GoBack` 方法：返回上一页。

- **`INavigationViewService`**：定义了与 `NavigationView` 相关的服务功能，包括菜单项、设置项、初始化、注销事件和获取选中项。
  - `MenuItems` 属性：获取 `NavigationView` 的菜单项。
  - `SettingsItem` 属性：获取 `NavigationView` 的设置项。
  - `Initialize` 方法：初始化 `NavigationView` 并注册事件。
  - `UnregisterEvents` 方法：注销 `NavigationView` 的事件。
  - `GetSelectedItem` 方法：根据页面类型获取选中的菜单项。

#### 2. 实现类
- **`NavigationService`**：实现了 `INavigationService` 接口，负责具体的导航逻辑。
  - 构造函数接收 `IPageService` 实例，用于获取页面类型。
  - `Frame` 属性：如果框架未初始化，则从 `App.MainWindow` 获取框架并注册导航事件。
  - `CanGoBack` 属性：检查框架是否可以返回上一页。
  - `GoBack` 方法：如果可以返回，则调用框架的 `GoBack` 方法，并通知导航前的视图模型。
  - `NavigateTo` 方法：根据页面键获取页面类型，然后调用框架的 `Navigate` 方法进行导航，并通知导航前的视图模型。
  - `OnNavigated` 方法：处理导航事件，清除导航栈并通知导航后的视图模型。

- **`NavigationViewService`**：实现了 `INavigationViewService` 接口，负责 `NavigationView` 的初始化和事件处理。
  - 构造函数接收 `INavigationService` 和 `IPageService` 实例。
  - `Initialize` 方法：初始化 `NavigationView` 并注册返回和项调用事件。
  - `UnregisterEvents` 方法：注销 `NavigationView` 的事件。
  - `GetSelectedItem` 方法：根据页面类型获取选中的菜单项。
  - `OnBackRequested` 方法：处理返回请求，调用 `NavigationService` 的 `GoBack` 方法。
  - `OnItemInvoked` 方法：处理菜单项调用事件，如果是设置项则导航到设置页面，否则根据页面键调用 `NavigationService` 的 `NavigateTo` 方法。

#### 3. 运作流程
1. **初始化**：在应用启动时，`NavigationViewService` 初始化 `NavigationView` 并注册事件。
2. **导航**：当用户点击 `NavigationView` 的菜单项时，`OnItemInvoked` 方法被调用，根据页面键调用 `NavigationService` 的 `NavigateTo` 方法进行导航。
3. **返回**：当用户点击返回按钮时，`OnBackRequested` 方法被调用，调用 `NavigationService` 的 `GoBack` 方法返回上一页。
4. **事件处理**：在导航过程中，`NavigationService` 处理导航事件，通知视图模型导航状态的变化。

通过以上机制，`NavigationView` 实现了应用内的页面导航功能。


### 点击 `NavigationViewItem` 时确定导航页面的机制

#### 1. `NavigationHelper` 附加属性
在 `ShellPage.xaml` 中，`NavigationViewItem` 使用了 `helpers:NavigationHelper.NavigateTo` 附加属性，该属性的值为视图模型的全名称，例如 `FluentLauncher.ViewModels.ManagerViewModel`。`NavigationHelper` 类定义了这个附加属性的获取和设置方法：
```csharp
public class NavigationHelper
{
    public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

    public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);

    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));
}
```

#### 2. `PageService` 页面映射
`PageService` 类实现了 `IPageService` 接口，负责管理视图模型类型和页面类型的映射关系。在构造函数中，通过 `Configure` 方法将视图模型和页面进行关联：
```csharp
public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<MainViewModel, MainPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
```

#### 3. `NavigationViewService` 事件处理
当用户点击 `NavigationViewItem` 时，`NavigationViewService` 的 `OnItemInvoked` 方法会被调用：
```csharp
private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
{
    if (args.IsSettingsInvoked)
    {
        // Navigate to the settings page.
    }
    else
    {
        var selectedItem = args.InvokedItemContainer as NavigationViewItem;

        if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
        {
            _navigationService.NavigateTo(pageKey);
        }
    }
}
```
在这个方法中，会从 `NavigationViewItem` 的附加属性 `NavigateTo` 中获取视图模型的全名称（即 `pageKey`），然后调用 `NavigationService` 的 `NavigateTo` 方法进行导航。

#### 4. `NavigationService` 导航
`NavigationService` 的 `NavigateTo` 方法会根据 `pageKey` 调用 `PageService` 的 `GetPageType` 方法获取对应的页面类型，然后使用框架的 `Navigate` 方法进行导航：
```csharp
public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
{
    var pageType = _pageService.GetPageType(pageKey);

    if (_frame != null && (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed))))
    {
        _frame.Tag = clearNavigation;
        var vmBeforeNavigation = _frame.GetPageViewModel();
        var navigated = _frame.Navigate(pageType, parameter);
        if (navigated)
        {
            _lastParameterUsed = parameter;
            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }
        }

        return navigated;
    }

    return false;
}
```

综上所述，当点击 `NavigationViewItem` 时，系统会从 `NavigationViewItem` 的附加属性中获取视图模型的全名称，通过 `PageService` 找到对应的页面类型，最后使用 `NavigationService` 进行导航。