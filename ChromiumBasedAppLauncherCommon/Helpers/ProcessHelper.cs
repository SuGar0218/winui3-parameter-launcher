using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace ChromiumBasedAppLauncherCommon.Helpers;

public class ProcessHelper
{
    public static Process? StartSilentAsAdmin(string exePath, string arguments = "")
    {
        Process process = new();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = Path.GetFullPath(exePath);
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.Verb = "runas";
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UserName = null;
        try
        {
            if (!process.Start())
                return null;
        }
        catch // 找不到文件、UAC被取消
        {
            return null;
        }
        return process;
    }

    public static Process? StartSilent(string exePath, string arguments = "")
    {
        Process process = new();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = Path.GetFullPath(exePath);
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UserName = null;
        try
        {
            if (!process.Start())
                return null;
        }
        catch
        {
            return null;
        }
        return process;
    }
    public static Process? StartSilentAsAdminAndWait(string exePath, string arguments = "")
    {
        Process process = new();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = Path.GetFullPath(exePath);
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.Verb = "runas";
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UserName = null;
        try
        {
            if (!process.Start())
                return null;
        }
        catch // 找不到文件、UAC被取消
        {
            return null;
        }
        process.WaitForExit();
        return process;
    }

    public static Process? StartSilentAndWait(string exePath, string arguments = "")
    {
        Process process = new();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = Path.GetFullPath(exePath);
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UserName = null;
        try
        {
            if (!process.Start())
                return null;
        }
        catch (Win32Exception)
        {
            return null;
        }
        process.WaitForExit();
        return process;
    }
}
