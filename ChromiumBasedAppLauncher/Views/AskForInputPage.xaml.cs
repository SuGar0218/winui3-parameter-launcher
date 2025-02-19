using ChromiumBasedAppLauncherGUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace ChromiumBasedAppLauncherGUI.Views;

public sealed partial class AskForInputPage : Page
{
    public AskForInputPage()
    {
        InitializeComponent();
    }

    public string Input => viewModel.Text;

    private readonly AskForInputViewModel viewModel = new();
}
