using ChromiumBasedAppLauncherCommon.Dao.Config;
using ChromiumBasedAppLauncherCommon.Entities;
using ChromiumBasedAppLauncherCommon.Helpers.ForSQL;
using ChromiumBasedAppLauncherCommon.Helpers.ForSQL.ForSqlite;

using Microsoft.Data.Sqlite;

namespace ChromiumBasedAppLauncherCommon.Dao;

public class ParameterDao
{
    public ParameterDao(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public List<ParameterConfig> ListParameterConfigs(int appId)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            return SqlBuilder
                .Select([
                    ParameterTable.Columns.Id,
                    ParameterTable.Columns.AppId,
                    ParameterTable.Columns.Parameter,
                    ParameterTable.Columns.Enabled,
                    AppTable.Columns.Name,
                    AppTable.Columns.Path,
                    AppTable.Columns.Enabled])
                .From([ParameterTable.Name, AppTable.Name])
                .Where($"{ParameterTable.Columns.AppId}=$id")
                .And($"{AppTable.Columns.Id}=$id")
                .ForSqliteCommandReading(command)
                .AddParameterWithValue("$id", appId)
                .ExecuteDataReading<ParameterConfig>((reader) => new(
                    reader.GetInt32(0),
                    new ConfiguredApp(reader.GetInt32(1), reader.GetString(4), reader.GetString(5), reader.GetBoolean(6)),
                    reader.GetString(2),
                    reader.GetBoolean(3)));
        }
    }

    public List<string> ListEnabledParameters(int appId)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            return SqlBuilder
                .Select(ParameterTable.Columns.Parameter).From(ParameterTable.Name)
                .Where($"{ParameterTable.Columns.AppId} = $appid")
                .And($"{ParameterTable.Columns.Enabled} = true")
                .ForSqliteCommandReading(command)
                .AddParameterWithValue("$appid", appId)
                .ExecuteColumnReading<string>();
        }
    }

    /// <summary>
    /// 为指定程序添加启动参数，返回插入的参数数据库ID
    /// </summary>
    /// <param name="config"></param>
    /// <returns>插入的参数数据库ID</returns>
    public int Add(ParameterConfig config)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            int inserted = SqliteSqlBuilder
                .InsertOrIgnoreInto(AppTable.Name, [AppTable.Fields.Name, AppTable.Fields.Path]).Values("($name, $path)")
                .ForSqliteCommandWriting(command)
                .AddParameterWithValue("$name", config.App.Name)
                .AddParameterWithValue("$path", config.App.Path)
                .ExecuteNonQuery();

            int appId;
            if (inserted > 0)
            {
                appId = SqlBuilder
                    .Select(SqliteSqlBuilder.LastInsertRowId())
                    .ForSqliteCommandReading(command)
                    .ExecuteScalarReading<int>();
            }
            else
            {
                appId = SqlBuilder
                    .Select(AppTable.Columns.Id).From(AppTable.Name).Where($"{AppTable.Columns.Name}=$name")
                    .ForSqliteCommandReading(command)
                    .AddParameterWithValue("$name", config.App.Name)
                    .ExecuteScalarReading<int>();
            }

            SqlBuilder
                .InsertInto(ParameterTable.Name, [ParameterTable.Fields.AppId, ParameterTable.Fields.Parameter, ParameterTable.Fields.Enabled])
                .Values("($appid, $param, $enabled)")
                .ForSqliteCommandWriting(command)
                .AddParameterWithValue("$appid", appId)
                .AddParameterWithValue("$param", config.Parameter)
                .AddParameterWithValue("$enabled", config.Enabled)
                .ExecuteNonQuery();

            return SqlBuilder
                .Select(SqliteSqlBuilder.LastInsertRowId()).From(ParameterTable.Name)
                .ForSqliteCommandReading(command)
                .ExecuteScalarReading<int>();
        }
    }

    public int Update(ParameterConfig config)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            return SqlBuilder
                .Update(ParameterTable.Name)
                .Set([$"{ParameterTable.Fields.Parameter}=$param", $"{ParameterTable.Fields.Enabled}=$enabled"])
                .Where($"{ParameterTable.Columns.Id}=$appId")
                .ForSqliteCommandWriting(command)
                .AddParameterWithValue("$param", config.Enabled)
                .AddParameterWithValue("$enabled", config.Enabled)
                .AddParameterWithValue("$appId", config.Id)
                .ExecuteNonQuery();
        }
    }

    public int Remove(int id)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            return SqlBuilder
                .DeleteFrom(ParameterTable.Name).Where($"{ParameterTable.Columns.Id}=$appId")
                .ForSqliteCommandWriting(command)
                .AddParameterWithValue("$appId", id)
                .ExecuteNonQuery();
        }
    }

    public void RemoveAll(int appId)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            SqlBuilder.DeleteFrom(ParameterTable.Name).Where($"{ParameterTable.Columns.AppId}=$appid")
                .ForSqliteCommandWriting(command)
                .AddParameterWithValue("$appid", appId)
                .ExecuteNonQuery();
        }
    }

    private readonly SqliteConnection connection;
}
