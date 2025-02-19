using ChromiumBasedAppLauncherCommon.Entities;

namespace ChromiumBasedAppLauncherGUI.ViewModels;

public partial class ParameterListItem// : ObservableObject
{
    public ParameterListItem(ParameterConfig config) : this(0, config) { }

    public ParameterListItem(int num, ParameterConfig config)
    {
        Num = num;
        Config = config;
    }

    /// <summary>
    /// 此项在列表视图中的编号
    /// </summary>
    public int Num { get; init; }

    public ParameterConfig Config { get; init; }

    public string Parameter
    {
        get => Config.Parameter;
        set => Config.Parameter = value;
    }

    public bool Enabled
    {
        get => Config.Enabled;
        set => Config.Enabled = value;
    }
}
