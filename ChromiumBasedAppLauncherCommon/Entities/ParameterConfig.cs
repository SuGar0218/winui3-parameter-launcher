namespace ChromiumBasedAppLauncherCommon.Entities;

public class ParameterConfig
{
    public int Id { get; set; }
    public ConfiguredApp App { get; set; }
    public string Parameter { get; set; }
    public bool Enabled { get; set; }

    public ParameterConfig(int id, ConfiguredApp app, string parameter, bool enabled)
    {
        Id = id;
        App = app;
        Parameter = parameter;
        Enabled = enabled;
    }

    public ParameterConfig(ConfiguredApp app, string parameter, bool enabled) : this(0, app, parameter, enabled) { }
}
