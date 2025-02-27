using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromiumBasedAppLauncherCommon;

/// <summary>
/// 相对路径都是相对于GUI程序所在位置
/// </summary>
public class GlobalProperties
{
    /// <summary>
    /// 存储应用程序启动参数配置的数据库
    /// </summary>
    public const string SqliteDataSource = @"./configs.sqlite";

    /// <summary>
    /// 映像劫持配置器
    /// <br/>
    /// 参数：启用(enable)或禁用(disable)、已配置的程序数据库ID
    /// <br/>
    /// 例如：enable chrome.exe
    /// <br/>
    /// 例如：disable Code.exe
    /// </summary>
    public const string ConfigurerPath = @$"./ChromiumBasedAppLauncherConfigurer.exe";

    /// <summary>
    /// 映像劫持后重定向到的启动器
    /// <br/>
    /// 参数：启动器本体所在位置、程序ID
    /// </summary>
    public const string CoreLauncherPath = @"./ChromiumBasedAppLauncherCore.exe";

    /// <summary>
    /// 注册表中 Image File Execution Options 的位置，可以设置映像劫持。
    /// </summary>
    public const string ImageFileExecutionOptionsPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options";

    /// <summary>
    /// 规定复制文件名的命名方式。
    /// <br/>
    /// 由于映像劫持后，原exe文件不能再用了，所以需要一个不同名称的副本。
    /// </summary>
    public static string CopiedFileName(string fileName)
    {
        return new StringBuilder().Append('_').Append(fileName).ToString();
    }
}
