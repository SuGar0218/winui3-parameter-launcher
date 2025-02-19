using ChromiumBasedAppLauncherCommon.Dao.Config;
using ChromiumBasedAppLauncherCommon.Entities;
using ChromiumBasedAppLauncherCommon.Helpers.ForSQL;
using ChromiumBasedAppLauncherCommon.Helpers.ForSQL.ForSqlite;

using Microsoft.Data.Sqlite;

namespace ChromiumBasedAppLauncherCommon.Dao;

public class AppDao
{
    public AppDao(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public List<ConfiguredApp> ListAll()
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            return SqlBuilder
                .Select([AppTable.Columns.Id, AppTable.Columns.Name, AppTable.Columns.Path, AppTable.Columns.Enabled]).From(AppTable.Name)
                .ForSqliteCommandReading(command)
                .ExecuteDataReading<ConfiguredApp>((reader) => new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3)));
        }
    }

    public List<ConfiguredApp> ListAll(int start, int limit)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            return SqlBuilder
                .Select([AppTable.Columns.Id, AppTable.Columns.Name, AppTable.Columns.Path, AppTable.Columns.Enabled]).From(AppTable.Name).Limit(start, limit)
                .ForSqliteCommandReading(command)
                .ExecuteDataReading<ConfiguredApp>((reader) => new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3)));
        }
    }

    public ConfiguredApp? QueryById(int id)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            List<ConfiguredApp> apps = SqlBuilder
                .Select([AppTable.Columns.Id, AppTable.Columns.Name, AppTable.Columns.Path, AppTable.Columns.Enabled])
                .From(AppTable.Name)
                .Where($"{AppTable.Columns.Id} = $id")
                .Limit(1)
                .ForSqliteCommandReading(command)
                .AddParameterWithValue("$id", id)
                .ExecuteDataReading<ConfiguredApp>((reader) => new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3)));
            return apps.Count > 0 ? apps[0] : null;
        }
    }

    /// <summary>
    /// 添加配置的程序信息
    /// </summary>
    /// <returns>若成功添加，返回添加的ID，否则返回0</returns>
    public int Add(string name, string path, bool enabled)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            int inserted = SqlBuilder
                .InsertInto(AppTable.Name, [AppTable.Fields.Name, AppTable.Fields.Path, AppTable.Fields.Enabled]).Values("($name, $path, $enabled)")
                .ForSqliteCommandWriting(command)
                .AddParameterWithValue("$name", name)
                .AddParameterWithValue("$path", path)
                .AddParameterWithValue("$enabled", enabled)
                .ExecuteNonQuery();
            return inserted > 0 ? SqlBuilder
                .Select(SqliteSqlBuilder.LastInsertRowId()).From(AppTable.Name)
                .ForSqliteCommandReading(command)
                .ExecuteScalarReading<int>() : 0;
        }
    }

    public int Remove(int id)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            SqlBuilder
                .DeleteFrom(ParameterTable.Name).Where($"{ParameterTable.Columns.AppId} = $appid")
                .ForSqliteCommandWriting(command)
                .AddParameterWithValue("$appid", id)
                .ExecuteNonQuery();
            return SqlBuilder
                .DeleteFrom(AppTable.Name).Where($"{AppTable.Columns.Id}=$id")
                .ForSqliteCommandWriting(command)
                .AddParameterWithValue("$id", id)
                .ExecuteNonQuery();
        }
    }

    /// <summary>
    /// 更新App配置项是否启用
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public int Update(ConfiguredApp app)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();
            return SqlBuilder
                .Update(AppTable.Name)
                .Set($"{AppTable.Fields.Enabled} = $enabled")
                .Where($"{AppTable.Fields.Id} = $id")
                .ForSqliteCommandWriting(command)
                .AddParameterWithValue("$enabled", app.Enabled)
                .AddParameterWithValue("$id", app.Id)
                .ExecuteNonQuery();
        }
    }

    /// <summary>
    /// 只核对名称是否已添加
    /// </summary>
    public bool Contains(string name)
    {
        using (connection)
        {
            connection.Open();
            using SqliteCommand command = connection.CreateCommand();

            return SqlBuilder
                .Select(SqlBuilder.Exists(SqlBuilder.Select1.From(AppTable.Name).Where($"{AppTable.Columns.Name}=$name")))
                .ForSqliteCommandReading(command)
                .AddParameterWithValue("$name", name)
                .ExecuteScalarReading<bool>();
        }
    }

    private readonly SqliteConnection connection;
}
