using ChromiumBasedAppLauncherCommon.Helpers;

using ChromiumBasedAppLauncherGUI.Extensions;
using ChromiumBasedAppLauncherGUI.Helpers;
using ChromiumBasedAppLauncherGUI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using System.Threading.Tasks;

namespace ChromiumBasedAppLauncherGUI.Views;

/// <summary>
/// 导航参数为选中的 ConfiguredApp
/// </summary>
public sealed partial class ParameterConfigPage : Page
{
    public ParameterConfigPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 导航到此页时，需要 ConfiguredApp 类型的参数，以记录此页正在配置哪个程序的启动参数。
    /// </summary>
    /// <param name="e">e.Parameter 为导航参数</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        viewModel = new ParameterConfigViewModel((AppListItem) e.Parameter);
        base.OnNavigatedTo(e);
    }

    /// <summary>
    /// 离开页面时自动保存
    /// </summary>
    /// <param name="e"></param>
    //protected override void OnNavigatedFrom(NavigationEventArgs e)
    //{
    //    viewModel.Persist();
    //    base.OnNavigatedFrom(e);
    //}

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        viewModel.Persist();
    }

    private ParameterConfigViewModel viewModel = null!;  // 通过 Frame.Navigate 导航过来就不是 null

    private void AppBarButton_Add_Click(object sender, RoutedEventArgs e)
    {
        viewModel.AddParameterConfig();
    }

    private void ParameterTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void AppBarButton_Save_Click(object sender, RoutedEventArgs e)
    {
        viewModel.Persist();
    }

    private void RemovingListView_ItemClick(object sender, ItemClickEventArgs e)
    {
    }

    private void HyperlinkButton_RemoveConfig_Click(object sender, RoutedEventArgs e)
    {
        viewModel.RemoveParameterConfig((ParameterListItem) ((FrameworkElement) sender).DataContext);
    }

    private void ParameterListItemChanged(object sender, TextChangedEventArgs e)
    {
        //viewModel.ChangeParameterConfig((ParameterListItem) ((FrameworkElement) sender).DataContext);
    }

    private void CheckBox_Parameter_CheckedChanged(object sender, RoutedEventArgs e)
    {
        //viewModel.ChangeParameterConfig((ParameterListItem) ((FrameworkElement) sender).DataContext);
    }

    private void HyperlinkButton_Copy_Click(object sender, RoutedEventArgs e)
    {
        ClipboardHelper.Copy(DataContextHelper.GetDataContext<ParameterListItem>(sender).Parameter);
    }

    private void AppBarButton_Launch_Click(object sender, RoutedEventArgs e)
    {
        AppBarButton button = (AppBarButton) sender;
        button.IsEnabled = false;
        Task.Run(() =>
        {
            if (ProcessHelper.StartSilentAndWait(viewModel.AppListItem.Path) is null)
            {
                DispatcherQueue.TryEnqueue(async () => await this.MessageBox("请考虑尝试以管理员身份启动", "启动失败"));
            }
        });
        button.IsEnabled = true;
    }

    private void AppBarButton_LaunchAsAdmin_Click(object sender, RoutedEventArgs e)
    {
        AppBarButton button = (AppBarButton) sender;
        button.IsEnabled = false;
        Task.Run(() =>
        {
            if (ProcessHelper.StartSilentAsAdminAndWait(viewModel.AppListItem.Path) is null)
            {
                DispatcherQueue.TryEnqueue(async () => await this.MessageBox("", "启动失败"));
            }
        });
        button.IsEnabled = true;
    }
}
