namespace ChromiumBasedAppLauncherCommon.Dao.Config;

public interface ITableConfig<TColumnConfig, TFieldConfig>
    where TColumnConfig : IColumnConfig
    where TFieldConfig : IFieldConfig
{
    public static string Name { get; }

    /// <summary>
    /// 表名.字段名
    /// </summary>
    public static TColumnConfig Columns { get; }

    /// <summary>
    /// 字段名称，不含表名
    /// </summary>
    public static TFieldConfig Fields { get; }

    public static string SqlCommandToCreateIfNotExists { get; }
}
