using ChromiumBasedAppLauncherCommon;
using ChromiumBasedAppLauncherCommon.Dao;
using ChromiumBasedAppLauncherCommon.Entities;
using ChromiumBasedAppLauncherCommon.Helpers;

using Microsoft.Data.Sqlite;

using System.Diagnostics;
using System.Text;

namespace ChromiumBasedAppLauncherCore;

/// <summary>
/// 生成的 exe 与 GUI 的 exe 放在同一文件夹
/// </summary>
internal class Program
{
    /// <summary>
    /// 传入参数：[0]此程序本体所在路径、[1]程序ID
    /// <br/>
    /// 后面如果有，就是映像 Debugger 劫持传过来的参数（[3]原程序路径、[4+]参数）。
    /// <br/>
    /// 映像劫持执行时，会把原本调用的命令接在最后。
    /// </summary>
    /// <param name="args">不包含程序所在位置</param>
    static void Main(string[] args)
    {
        //Console.WriteLine(args.ToString('\n'));
        //Console.ReadLine();

        //Task.Run(() =>
        //{
        //    StringBuilder logStringBuilder = new StringBuilder().AppendLine(DateTime.Now.ToString());
        //    for (int i = 0; i < args.Length; i++)
        //    {
        //        logStringBuilder.AppendLine(args[i]);
        //    }
        //    File.AppendAllText(Path.Combine(args[0], "log.txt"), logStringBuilder.AppendLine().ToString());
        //});

        SqliteConnection connection = new(new SqliteConnectionStringBuilder { DataSource = Path.GetFullPath(Path.Combine(args[0], GlobalProperties.SqliteDataSource)) }.ToString());
        AppDao appDao = new(connection);
        ParameterDao parameterDao = new(connection);

        int appId = int.Parse(args[1]);
        ConfiguredApp? app = appDao.QueryById(appId);
        if (app is null)
        {
            return;
        }
        string appPath = app.Path;
        string copyPath = Path.GetDirectoryName(appPath) + Path.DirectorySeparatorChar + GlobalProperties.CopiedFileName(Path.GetFileName(appPath));
        StringBuilder argsStringBuilder = new();
        List<string> parameters = parameterDao.ListEnabledParameters(appId);

        foreach (string parameter in parameters)
        {
            argsStringBuilder.Append(' ').Append(parameter);
        }

        for (int i = 3; i < args.Length; i++)
        {
            argsStringBuilder.Append(' ').Append(args[i]);
        }
        
        _ = ProcessHelper.StartSilent(copyPath, argsStringBuilder.ToString()) ?? ProcessHelper.StartSilentAsAdmin(copyPath, argsStringBuilder.ToString());
    }
}
