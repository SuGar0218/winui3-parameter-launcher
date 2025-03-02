using ChromiumBasedAppLauncherCommon;
using ChromiumBasedAppLauncherCommon.Dao;
using ChromiumBasedAppLauncherCommon.Entities;
using ChromiumBasedAppLauncherCommon.Helpers;

using Microsoft.Data.Sqlite;
using Microsoft.Win32;

using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace ChromiumBasedAppLauncherCore;

/// <summary>
/// 生成的 exe 与 GUI 的 exe 放在同一文件夹
/// </summary>
internal partial class Program
{
    /// <summary>
    /// 传入参数：[0]此程序本体所在文件夹、[1]程序ID
    /// <br/>
    /// 后面如果有，就是映像 Debugger 劫持传过来的参数（[2]原程序路径、[3+]参数）。
    /// <br/>
    /// 映像劫持执行时，会把原本调用的命令接在最后。
    /// </summary>
    /// <param name="args">不包含程序所在位置</param>
    static void Main(string[] args)
    {
        SqliteConnection connection = new(new SqliteConnectionStringBuilder
        {
            DataSource = Path.GetFullPath(Path.Combine(args[0], GlobalProperties.SqliteDataSource))
        }
        .ToString());
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
            argsStringBuilder.Append(' ');
            if (parameter.Contains(' '))
            {
                argsStringBuilder.Append('\"').Append(parameter).Append('\"');
            }
            else
            {
                argsStringBuilder.Append(parameter);
            }
        }

        for (int i = 3; i < args.Length; i++)
        {
            argsStringBuilder.Append(' ');
            if (args[i].Contains(' '))
            {
                argsStringBuilder.Append('\"').Append(args[i]).Append('\"');
            }
            else
            {
                argsStringBuilder.Append(args[i]);
            }
        }

        // 尝试启动
        // 如果启动失败，先尝试重新复制文件，再尝试以管理员身份启动。
        string launchArgs = argsStringBuilder.ToString();
        Process? process = ProcessHelper.StartSilent(copyPath, launchArgs);
        bool failed;
        if (process is null)
        {
            failed = true;
        }
        else
        {
            process.WaitForExit();
            failed = (process.ExitCode != 0);
        }
        if (failed)
        {
            //_ = MessageBox(
            //    0,
            //    "此程序似乎已更新，这需要重新复制一次程序可执行文件。如果稍后询问你授权，请允许。如果稍后仍无法启动，请联系开发者。",
            //    "尝试修复此程序的启动",
            //    0);
            ProcessHelper.StartSilentAndWait(
                Path.Combine(args[0], @"MessageWindow\MessageWindow.exe"),
                " 此程序似乎已更新，这需要重新复制一次程序可执行文件。如果稍后询问你授权，请允许。如果稍后仍无法启动，请联系开发者。" +
                " 尝试修复此程序的启动");
            string copyExePath = Path.Combine(args[0], "Copy.exe");
            _ = ProcessHelper.StartSilentAsAdminAndWait(copyExePath, $"\"{appPath}\" \"{copyPath}\"");
            _ = ProcessHelper.StartSilent(copyPath, launchArgs) ??
                ProcessHelper.StartSilentAsAdmin(copyPath, launchArgs);
        }

        // 记录运行日志，以备不时之需。
        //StringBuilder logStringBuilder = new StringBuilder().AppendLine(DateTime.Now.ToString());
        //if (failed)
        //{
        //    logStringBuilder.AppendLine($"重新复制文件").AppendLine(copyPath);
        //}
        //logStringBuilder.AppendLine("启动器接收的参数");
        //for (int i = 0; i < args.Length; i++)
        //{
        //    logStringBuilder.AppendLine(args[i]);
        //}
        //logStringBuilder.AppendLine("启动应用的参数");
        //logStringBuilder.AppendLine(argsStringBuilder.ToString());
        //string logPath = Path.Combine(args[0], "log.txt");
        //File.AppendAllText(logPath, logStringBuilder.AppendLine().ToString());
    }

    //[LibraryImport("user32.dll", StringMarshalling = StringMarshalling.Utf16)]
    //public static partial int MessageBox(IntPtr hWnd, string text, string caption, uint type);
}
