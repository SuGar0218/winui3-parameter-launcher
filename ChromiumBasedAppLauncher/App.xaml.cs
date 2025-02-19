using ChromiumBasedAppLauncherCommon;
using ChromiumBasedAppLauncherCommon.Dao.Config;

using Microsoft.Data.Sqlite;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChromiumBasedAppLauncherGUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
        MainWindow = null!;
    }

    public static new App Current => (App) Application.Current;

    public MainWindow MainWindow { get; private set; }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();
        //MainWindow.Activate();

        SqliteConnection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = GlobalProperties.SqliteDataSource }.ToString());
        using (SqliteConnection)
        {
            SqliteConnection.Open();
            using SqliteCommand command = SqliteConnection.CreateCommand();

            command.CommandText = AppTable.SqlCommandToCreateIfNotExists;
            command.ExecuteNonQuery();

            command.CommandText = ParameterTable.SqlCommandToCreateIfNotExists;
            command.ExecuteNonQuery();
        }
    }

    public SqliteConnection SqliteConnection { get; private set; } = null!;
}
