using ChromiumBasedAppLauncherCommon.Entities;

using ChromiumBasedAppLauncherGUI.Helpers;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Media.Imaging;

namespace ChromiumBasedAppLauncherGUI.ViewModels;

public class AppListItem
{
    public AppListItem(ConfiguredApp app)
    {
        App = app;
        Icon = IconHelper.ExtractAssociatedIconBitmapImage(app.Path);
    }

    public ConfiguredApp App { get; init; }

    public int Id
    {
        get => App.Id;
        set => App.Id = value;
    }

    public string Name
    {
        get => App.Name;
        set => App.Name = value;
    }

    public string Path
    {
        get => App.Path;
        set => App.Path = value;
    }

    public bool Enabled
    {
        get => App.Enabled;
        set => App.Enabled = value;
    }

    // Object -> DependencyObject -> ImageSource -> BitmapSource -> BitmapImage
    public BitmapImage Icon { get; set; }
}
