using ChromiumBasedAppLauncherCommon;
using ChromiumBasedAppLauncherCommon.Dao;
using ChromiumBasedAppLauncherCommon.Entities;

using Microsoft.Data.Sqlite;
using Microsoft.Win32;

using System.Text;

namespace ChromiumBasedAppLauncherConfigurer;

internal class Program
{
    /// <summary>
    /// 由于需要操作注册表和在程序安装位置复制文件，需要以管理员身份运行，启动时会显示用户账户控制（UAC），详见 app.manifest
    /// 参数：启用(enable)或禁用(disable)、已配置的程序数据库ID
    /// <br/>
    /// 例如：enable 1
    /// <br/>
    /// 例如：disable 1
    /// </summary>
    /// <param name="args">args不含有此程序所在路径</param>
    static void Main(string[] args)
    {
        int appId = int.Parse(args[1]);
        switch (args[0])
        {
            case "enable":
                Enable(appId);
                break;

            case "disable":
                Disable(appId);
                break;

            case "delete":
                Delete(appId);
                break;

            default:
                return;
        }

        //StringBuilder logStringBuilder = new();

    }

    /// <summary>
    /// 1. 复制exe文件
    /// <br/>
    /// 2. 设置映像劫持，把原程序重定向到启动器
    /// </summary>
    static void Enable(int appId)
    {
        ConfiguredApp? app = appDao.QueryById(appId);
        if (app is null)
            return;

        string originPath = app.Path;
        string copiedPath = Path.Combine(
            Path.GetDirectoryName(app.Path)!,
            Path.GetFileName(GlobalProperties.CopiedFileName(Path.GetFileName(app.Path))));
        //File.Copy(originPath, copiedPath, overwrite: true);
        if (File.Exists(copiedPath))
        {
            File.Delete(copiedPath);
        }
        File.CreateSymbolicLink(copiedPath, originPath);

        //List<string> parameters = parameterDao.ListEnabledParameters(appId);
        //StringBuilder argsStringBuilder = new();
        //foreach (string parameter in parameters)
        //{
        //    argsStringBuilder.Append(' ').Append(parameter);
        //}

        using RegistryKey registryKey = ImageFileExecutionOptions.OpenSubKey(app.Name, writable: true) ?? ImageFileExecutionOptions.CreateSubKey(app.Name, writable: true);
        string debugger = new StringBuilder()
            .Append('\"')
            .Append(Path.GetFullPath(GlobalProperties.CoreLauncherPath))
            .Append('\"')
            .Append(' ')
            .Append('\"')
            .Append(Path.GetFullPath(Path.GetDirectoryName(GlobalProperties.CoreLauncherPath)!))
            .Append('\"')
            .Append(' ')
            .Append(appId)
            .ToString();
        registryKey.SetValue("Debugger", debugger);
    }

    static void Disable(int appId)
    {
        ConfiguredApp? app = appDao.QueryById(appId);
        if (app is null)
            return;

        using RegistryKey registryKey = ImageFileExecutionOptions.OpenSubKey(app.Name, writable: true) ?? ImageFileExecutionOptions.CreateSubKey(app.Name, writable: true);
        registryKey.SetValue("Debugger", "");
    }

    /// <summary>
    /// 1. 删除复制的exe文件
    /// <br/>
    /// 2. 取消映像劫持，删除 Debugger 键值
    /// </summary>
    static bool Delete(int appId)
    {
        ConfiguredApp? app = appDao.QueryById(appId);
        if (app is null)
            return true;

        string originPath = app.Path;
        string copiedPath = Path.GetDirectoryName(app.Path) + Path.DirectorySeparatorChar + GlobalProperties.CopiedFileName(Path.GetFileName(app.Path));

        if (File.Exists(copiedPath))
        {
            try
            {
                File.Delete(copiedPath);
            }
            catch (UnauthorizedAccessException)  // 程序正在运行会无法删除
            {
                return false;
            }
        }

        using RegistryKey? registryKey = ImageFileExecutionOptions.OpenSubKey(app.Name, writable: true);
        registryKey?.DeleteValue("Debugger");
        return true;
    }

    /// <summary>                                  
    /// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Option
    /// </summary>
    static readonly RegistryKey ImageFileExecutionOptions = Registry.LocalMachine
        .OpenSubKey("SOFTWARE", writable: true)!
        .OpenSubKey("Microsoft", writable: true)!
        .OpenSubKey("Windows NT", writable: true)!
        .OpenSubKey("CurrentVersion", writable: true)!
        .OpenSubKey("Image File Execution Options", writable: true)!;

    static readonly SqliteConnection connection = new(new SqliteConnectionStringBuilder { DataSource = GlobalProperties.SqliteDataSource }.ToString());
    static readonly AppDao appDao = new(connection);
    static readonly ParameterDao parameterDao = new(connection);
}
