# winui3-parameter-launcher
Launch EXE with preferred parameters anywhere.

带参数启动是如何实现的？
![IFEOTip](https://github.com/user-attachments/assets/06223350-1557-4e9d-a702-1207889436e6)

ChromiumBasedAppLauncher: WinUI3 主程序。
ChromiumBasedAppLauncherCommon: 公用组件，例如数据库访问，工具类。

ChromiumBasedAppLauncherConfigurer: 用于在程序所在路径复制程序副本、在注册表设置映像劫持，使得启动原程序时，通过核心启动器中转，以附加我们自己设置的启动参数。
ChromiumBasedAppLauncherCore: 核心启动器，启动程序时，除了调用者的参数外，还会携带用户自己定的启动参数。

WinUI3 主程序会调用 ChromiumBasedAppLauncherConfigurer 和 ChromiumBasedAppLauncherCore，这两个项目生成的程序最终应放在 WinUI3 主程序所在位置。
