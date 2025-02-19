using ChromiumBasedAppLauncherCommon;
using ChromiumBasedAppLauncherCommon.Helpers;

using ChromiumBasedAppLauncherGUI.Extensions;
using ChromiumBasedAppLauncherGUI.Helpers;
using ChromiumBasedAppLauncherGUI.Helpers.ForFilePicker;
using ChromiumBasedAppLauncherGUI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;

namespace ChromiumBasedAppLauncherGUI.Views;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        App.Current.MainWindow.SetTitleBar(TitleBarArea);
        titleBarPassthroughHelper = new TitleBarPassthroughHelper(App.Current.MainWindow);
        titleBarPassthroughHelper.Passthrough(TitleBarPaneToggleButton);

        TabViewContentFrame.Navigate(typeof(EmptyPage));
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        viewModel.Persist();
    }

    private void TitleBarPaneToggleButton_Click(object sender, RoutedEventArgs e)
    {
        Navigation.IsPaneOpen = !Navigation.IsPaneOpen;
    }

    private readonly MainViewModel viewModel = new();

    private TitleBarPassthroughHelper titleBarPassthroughHelper = null!;

    private async void MenuFlyoutItem_PickFile_ClickAsync(object sender, RoutedEventArgs e)
    {
        StorageFile file = await FileOpenPickerHelper
            .OpenFileForWindow(App.Current.MainWindow)
            .SetViewMode(PickerViewMode.List)
            .AddFileTypeFilter("exe")
            .PickSingleAsync();

        viewModel.AddAppListItem(file.Name, file.Path);
    }

    private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is not null)
        {
            viewModel.OpenAppListItem((AppListItem) args.SelectedItem);
        }
    }

    private void TabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            TabViewContentFrame.Navigate(typeof(ParameterConfigPage), e.AddedItems[0]);
        }
        else
        {
            TabViewContentFrame.Navigate(typeof(EmptyPage));
        }
    }

    private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        viewModel.CloseAppListItem((AppListItem) args.Item);
    }

    private void CloseAllTabsButton_Click(object sender, RoutedEventArgs e)
    {
        viewModel.CloseAllAppListItems();
    }

    private async void AppListMenuFlyout_RemoveItem_Click(object sender, RoutedEventArgs e)
    {
        AppListItem item = DataContextHelper.GetDataContext<AppListItem>(sender);
        if (! await viewModel.RemoveAppListItemAsync(item))
        {
            await this.MessageBox("您取消了操作。删除程序配置需要恢复注册表项，需要管理员权限。请允许。", "请不要取消管理员授权");
            return;
        }
    }

    private async void MenuFlyout_InputPath_Click(object sender, RoutedEventArgs e)
    {
        AskForInputPage page = new();
        if (await this.MessageBox(page, "程序所在位置", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            return;

        string path = Path.GetFullPath(page.Input);
        if (!File.Exists(path))
        {
            await this.MessageBox(path, "文件不存在");
            return;
        }
        if (!page.Input.EndsWith(".exe"))
        {
            await this.MessageBox(path, "这不是可执行文件（exe）");
            return;
        }
        viewModel.AddAppListItem(Path.GetFileName(path), path);
    }

    private void AppListMenuFlyoutItem_CopyPath_Click(object sender, RoutedEventArgs e)
    {
        ClipboardHelper.Copy(DataContextHelper.GetDataContext<AppListItem>(sender).Path);
    }

    private void AppListMenuFlyoutItem_OpenPath_Click(object sender, RoutedEventArgs e)
    {
        Process.Start("explorer", "/select," + DataContextHelper.GetDataContext<AppListItem>(sender).Path);
    }

    private bool toggleCanceled;

    /// <summary>
    /// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options
    /// </summary>
    private async void AppListItemToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (toggleCanceled)
        {
            toggleCanceled = false;
            return;
        }

        ToggleSwitch toggleSwitch = (ToggleSwitch) sender;
        AppListItem item = DataContextHelper.GetDataContext<AppListItem>(sender);

        if (item is null)
            return;

        // 此时更改还没有同步到绑定的数据
        if (! await MainViewModel.EnableAppConfig(item.Id, toggleSwitch.IsOn))
        {
            toggleCanceled = true;
            toggleSwitch.IsOn = !toggleSwitch.IsOn;
        }
        else
        {
            toggleCanceled = false;
        }
    }

    private async void AppBarButton_Help_Click(object sender, RoutedEventArgs e)
    {
        await this.MessageBox(new HelpPage(), "帮助");
    }
}
