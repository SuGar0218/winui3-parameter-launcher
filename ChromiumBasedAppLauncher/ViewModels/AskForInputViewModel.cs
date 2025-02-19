using CommunityToolkit.Mvvm.ComponentModel;

namespace ChromiumBasedAppLauncherGUI.ViewModels;

public partial class AskForInputViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Text { get; set; }
}
