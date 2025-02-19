namespace ChromiumBasedAppLauncherCommon.Dao.Config;

public abstract class AbstractColumnConfig : IColumnConfig
{
    public AbstractColumnConfig(string table)
    {
        this.table = table;
    }

    protected readonly string table;

    protected string ColumnName(string column) => $"{table}.{column}";
}
