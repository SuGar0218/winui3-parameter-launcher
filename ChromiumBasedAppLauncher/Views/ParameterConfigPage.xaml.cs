using ChromiumBasedAppLauncherGUI.Helpers;
using ChromiumBasedAppLauncherGUI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace ChromiumBasedAppLauncherGUI.Views;

/// <summary>
/// ��������Ϊѡ�е� ConfiguredApp
/// </summary>
public sealed partial class ParameterConfigPage : Page
{
    public ParameterConfigPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// ��������ҳʱ����Ҫ ConfiguredApp ���͵Ĳ������Լ�¼��ҳ���������ĸ����������������
    /// </summary>
    /// <param name="e">e.Parameter Ϊ��������</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        viewModel = new ParameterConfigViewModel((AppListItem) e.Parameter);
        base.OnNavigatedTo(e);
    }

    /// <summary>
    /// �뿪ҳ��ʱ�Զ�����
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

    private ParameterConfigViewModel viewModel = null!;  // ͨ�� Frame.Navigate ���������Ͳ��� null

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
}
