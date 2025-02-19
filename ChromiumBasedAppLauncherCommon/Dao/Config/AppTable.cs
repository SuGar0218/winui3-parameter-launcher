namespace ChromiumBasedAppLauncherCommon.Dao.Config;

public class AppTable : ITableConfig<AppTable.MyColumns, AppTable.MyFields>
{
    public static string Name => "app";

    public static MyColumns Columns => columns;
    public static MyFields Fields => fields;

    public static string SqlCommandToCreateIfNotExists => @$"
        CREATE TABLE IF NOT EXISTS {Name}
        (
            {Fields.Id} INTEGER PRIMARY KEY AUTOINCREMENT,
            {Fields.Name} TEXT UNIQUE NOT NULL,
            {Fields.Path} TEXT UNIQUE NOT NULL,
            {Fields.Enabled} BOOL NOT NULL
        );
    ";

    public class MyColumns : AbstractColumnConfig
    {
        public MyColumns(string table) : base(table) { }

        public string Id => ColumnName(Fields.Id);
        public string Name => ColumnName(Fields.Name);
        public string Path => ColumnName(Fields.Path);
        public string Enabled => ColumnName(Fields.Enabled);
    }

    public class MyFields : IFieldConfig
    {
        public string Id => "app_id";
        public string Name => "name";
        public string Path => "path";
        public string Enabled => "enabled";
    }

    private static readonly MyColumns columns = new(Name);
    private static readonly MyFields fields = new();
}
