using ChromiumBasedAppLauncherCommon.Extensions;

namespace ChromiumBasedAppLauncherCommon.Helpers.ForSQL.ForSqlite;

public class SqliteSqlBuilder : SqlBuilder
{
    public SqliteSqlBuilder(string sql) : base(sql) { }

    public static SqliteSqlBuilder InsertOrIgnoreInto(string table, string fields) => new($"INSERT OR IGNORE INTO {table} ({fields})");
    public static SqliteSqlBuilder InsertOrIgnoreInto(string table, string[] fields) => new($"INSERT OR IGNORE INTO {table} ({fields.ToString(',')})");

    public static string LastInsertRowId() => "last_insert_rowid()";
}
