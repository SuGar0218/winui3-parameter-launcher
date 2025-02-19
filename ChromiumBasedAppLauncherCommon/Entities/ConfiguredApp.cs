namespace ChromiumBasedAppLauncherCommon.Entities;

public class ConfiguredApp
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public bool Enabled { get; set; }

    public ConfiguredApp(int id, string name, string path, bool enabled)
    {
        Id = id;
        Name = name;
        Path = path;
        Enabled = enabled;
    }

    public ConfiguredApp(string name, string path) : this(0, name, path, false) { }
}
