using ChromiumBasedAppLauncherCommon;
using ChromiumBasedAppLauncherCommon.Dao;
using ChromiumBasedAppLauncherCommon.Dao.Config;
using ChromiumBasedAppLauncherCommon.Entities;
using ChromiumBasedAppLauncherCommon.Helpers;
using ChromiumBasedAppLauncherCommon.Helpers.ForSQL;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ChromiumBasedAppLauncherGUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        appDao = new AppDao(App.Current.SqliteConnection);
        parameterDao = new ParameterDao(App.Current.SqliteConnection);
        foreach (ConfiguredApp app in appDao.ListAll())
        {
            ConfiguredApps.Add(new AppListItem(app));
        }
    }

    [ObservableProperty]
    public partial AppListItem? SelectedItem { get; set; }

    public ObservableCollection<AppListItem> ConfiguredApps { get; } = [];
    public ObservableCollection<AppListItem> OpenedConfiguredApps { get; } = [];

    private readonly HashSet<int> openedAppIdSet = new();

    private readonly AppDao appDao;
    private readonly ParameterDao parameterDao;

    public void AddAppListItem(string name, string path)
    {
        if (appDao.Contains(name))
            return;

        int appId = appDao.Add(name, path, false);
        ConfiguredApps.Add(new AppListItem(new ConfiguredApp(appId, name, path, false)));
    }

    /// <summary>
    /// 先删除复制的文件，取消映像劫持，再删除配置项。
    /// </summary>
    public async Task<bool> RemoveAppListItemAsync(AppListItem app)
    {
        if (await DeleteAppConfig(app.Id))
        {
            CloseAppListItem(app);
            ConfiguredApps.Remove(app);
            appDao.Remove(app.Id);
            return true;
        }
        return false;
    }

    public void OpenAppListItem(AppListItem app)
    {
        if (openedAppIdSet.Contains(app.Id))
            return;

        OpenedConfiguredApps.Add(app);
        openedAppIdSet.Add(app.Id);
    }

    public void CloseAppListItem(AppListItem app)
    {
        OpenedConfiguredApps.Remove(app);
        openedAppIdSet.Remove(app.Id);
        //SelectedItem = null;
    }

    public void CloseAllAppListItems()
    {
        OpenedConfiguredApps.Clear();
        openedAppIdSet.Clear();
        SelectedItem = null;
    }

    public List<string> ListParametersFor(int appId)
    {
        List<ParameterConfig> configs = parameterDao.ListParameterConfigs(appId);
        List<string> parameters = new(configs.Count);
        foreach (ParameterConfig config in configs)
        {
            parameters.Add(config.Parameter);
        }
        return parameters;
    }

    public void Persist()
    {
        foreach (AppListItem item in ConfiguredApps)
        {
            appDao.Update(item.App);
        }
    }

    public static async Task<bool> EnableAppConfig(int appId, bool enabled)
    {
        var result = await Task.Run(() =>
        {
            if (enabled)
            {
                return ProcessHelper.StartSilentAsAdmin(GlobalProperties.ConfigurerPath, $"enable {appId}");
            }
            else
            {
                return ProcessHelper.StartSilentAsAdmin(GlobalProperties.ConfigurerPath, $"disable {appId}");
            }
        });
        return result is not null;
    }

    public static async Task<bool> DeleteAppConfig(int appId)
    {
        var result = await Task.Run(() =>
        {
            return ProcessHelper.StartSilentAsAdmin(GlobalProperties.ConfigurerPath, $"delete {appId}");
        });
        return result is not null;
    }
}
