namespace ChromiumBasedAppLauncherGUI.Entities;

public class ConfiguredApp
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }

    public ConfiguredApp(int id, string name, string path)
    {
        this.Id = id;
        Name = name;
        Path = path;
    }

    public ConfiguredApp(string name, string path) : this(0, name, path) { }
}
