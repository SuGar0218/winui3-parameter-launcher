using ChromiumBasedAppLauncherCommon.Dao;
using ChromiumBasedAppLauncherCommon.Entities;

using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

namespace ChromiumBasedAppLauncherGUI.ViewModels;

public partial class ParameterConfigViewModel : ObservableObject
{
    public ParameterConfigViewModel(AppListItem appListItem)
    {
        AppListItem = appListItem;
        parameterDao = new ParameterDao(App.Current.SqliteConnection);
        foreach (ParameterConfig config in parameterDao.ListParameterConfigs(appListItem.Id))
        {
            ParameterConfigs.Add(new ParameterListItem(ParameterConfigs.Count + 1, config));
        }
    }

    public AppListItem AppListItem { get; init; }

    public ObservableCollection<ParameterListItem> ParameterConfigs { get; } = [];

    private readonly ParameterDao parameterDao;

    //private HashSet<ParameterConfig> changedConfigs = [];
    //private HashSet<ParameterConfig> addedConfigs = [];
    //private HashSet<ParameterConfig> removedConfigs = [];

    public void AddParameterConfig()
    {
        ParameterConfig newParameterConfig = new(AppListItem.App, string.Empty, true);
        ParameterConfigs.Add(new ParameterListItem(ParameterConfigs.Count + 1, newParameterConfig));
        //addedConfigs.Add(newParameterConfig);
    }

    public void RemoveParameterConfig(ParameterListItem item)
    {
        ParameterConfig removingConfig = item.Config;
        ParameterConfigs.Remove(item);

        //if (!addedConfigs.Remove(removingConfig))
        //{
        //    removedConfigs.Add(removingConfig);
        //}
    }

    //public void ChangeParameterConfig(ParameterListItem item)
    //{
    //    changedConfigs.Add(item.Config);
    //}

    public void Persist()
    {
        //foreach (ParameterConfig config in changedConfigs)
        //{
        //    if (!string.IsNullOrWhiteSpace(config.Parameter))
        //        parameterDao.Update(config);
        //}
        //foreach (ParameterConfig config in addedConfigs)
        //{
        //    if (!string.IsNullOrWhiteSpace(config.Parameter))
        //        config.Id = parameterDao.Add(config);
        //}
        //foreach (ParameterConfig config in removedConfigs)
        //{
        //    parameterDao.Remove(config.Id);
        //}

        parameterDao.RemoveAll(AppListItem.Id);
        foreach (ParameterListItem item in ParameterConfigs)
        {
            if (!string.IsNullOrWhiteSpace(item.Parameter))
            {
                parameterDao.Add(item.Config);
            }
        }
    }
}
