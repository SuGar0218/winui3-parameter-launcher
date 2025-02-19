using ChromiumBasedAppLauncherGUI.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Windows.UI;

namespace ChromiumBasedAppLauncherGUI;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        AppWindow.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(32, 128, 128, 128);
        AppWindow.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(16, 128, 128, 128);
    }

    private void Frame_Loaded(object sender, RoutedEventArgs e)
    {
        ((Frame) sender).Navigate(typeof(MainPage));

        double scale = Content.XamlRoot.RasterizationScale;
        AppWindow.Resize(new Windows.Graphics.SizeInt32((int) (1042 * scale), (int) (768 * scale)));
        Activate();
    }
}
