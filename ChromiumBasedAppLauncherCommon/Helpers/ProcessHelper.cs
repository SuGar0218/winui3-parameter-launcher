using System.ComponentModel;
using System.Diagnostics;

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
        try
        {
            return process.Start() ? process : null;
        }
        catch (Win32Exception) // 找不到文件、UAC被取消
        {
            return null;
        }
    }

    public static Process? StartSilent(string exePath, string arguments = "")
    {
        Process process = new();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = Path.GetFullPath(exePath);
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.Arguments = arguments;
        try
        {
            return process.Start() ? process : null;
        }
        catch (Win32Exception)
        {
            return null;
        }
    }
}
