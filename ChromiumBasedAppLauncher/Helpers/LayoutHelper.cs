using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ChromiumBasedAppLauncherGUI.Helpers;

public static class LayoutHelper
{
    public static void VerticalAlignContent(StackPanel stackPanel, VerticalAlignment alignment)
    {
        foreach (UIElement element in stackPanel.Children)
        {
            ((FrameworkElement) element).VerticalAlignment = alignment;
        }
    }

    public static void HorizontalAlignContent(StackPanel stackPanel, HorizontalAlignment alignment)
    {
        foreach (UIElement element in stackPanel.Children)
        {
            ((FrameworkElement) element).HorizontalAlignment = alignment;
        }
    }

    public static void StackPanelCenterVerticalAlignContentOnLoaded(object stackPanel, RoutedEventArgs e)
    {
        VerticalAlignContent((StackPanel) stackPanel, VerticalAlignment.Center);
    }

    public static void StackPanelCenterHorizontalAlignContentOnLoaded(object stackPanel, RoutedEventArgs e)
    {
        HorizontalAlignContent((StackPanel) stackPanel, HorizontalAlignment.Center);
    }
}
