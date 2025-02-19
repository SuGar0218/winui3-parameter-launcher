using ChromiumBasedAppLauncherCommon.Helpers.ForSQL;

namespace ChromiumBasedAppLauncherCommon.Dao.Config;

public class ParameterTable : ITableConfig<ParameterTable.MyColumns, ParameterTable.MyFields>
{
    public static string Name => "parameter";

    public static MyColumns Columns => columns;
    public static MyFields Fields => fields;

    public static string SqlCommandToCreateIfNotExists => @$"
        CREATE TABLE IF NOT EXISTS {Name}
        (
            {Fields.Id} INTEGER PRIMARY KEY AUTOINCREMENT,
            {Fields.AppId} INTEGER NOT NULL,
            {Fields.Parameter} TEXT NOT NULL,
            {Fields.Enabled} BOOL NOT NULL,
            FOREIGN KEY ({Fields.AppId}) REFERENCES {AppTable.Name}({AppTable.Fields.Id})
        );
    ";

    public class MyColumns : AbstractColumnConfig
    {
        public MyColumns(string table) : base(table) { }

        public string Id => ColumnName(Fields.Id);
        public string AppId => ColumnName(Fields.AppId);
        public string Parameter => ColumnName(Fields.Parameter);
        public string Enabled => ColumnName(Fields.Enabled);
    }

    public class MyFields : IFieldConfig
    {
        public string Id => "id";
        public string AppId => "app_id";
        public string Parameter => "parameter";
        public string Enabled => "enabled";
    }

    private static readonly MyColumns columns = new(Name);
    private static readonly MyFields fields = new();
}
