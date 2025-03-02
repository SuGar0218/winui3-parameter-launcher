using Microsoft.UI;
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

using WinUIEx;

namespace MessageWindow;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow(string title, string message)
    {
        this.title = title;
        this.message = message;
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.CompactOverlay);
    }

    private readonly string title;
    private readonly string message;

    private void Root_Loaded(object sender, RoutedEventArgs e)
    {
        StackPanel rootGrid = (StackPanel) sender;
        double scale = rootGrid.XamlRoot.RasterizationScale;
        double width = rootGrid.ActualWidth;
        double height = rootGrid.ActualHeight;
        AppWindow.Resize(new Windows.Graphics.SizeInt32((int) (width * scale), (int) (height * scale)));
        this.CenterOnScreen();
        Activate();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
