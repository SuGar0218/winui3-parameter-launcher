using Microsoft.Data.Sqlite;

namespace ChromiumBasedAppLauncherCommon.Helpers.ForSQL.ForSqlite;

/// <summary>
/// 提供 ForSqliteCommandReading 方法针对 Sqlite 的无类型参数特化版本
/// </summary>
public static class SqlBuilderSqliteExtension
{
    public static SqliteReadingSqlExecutor ForSqliteCommandReading(this SqlBuilder sqlBuilder, SqliteCommand command)
        => sqlBuilder.ForCommandReading<SqliteReadingSqlExecutor, SqliteCommand, SqliteDataReader>(command);

    public static SqliteWritingSqlExecutor ForSqliteCommandWriting(this SqlBuilder sqlBuilder, SqliteCommand command)
        => sqlBuilder.ForCommandWriting<SqliteWritingSqlExecutor, SqliteCommand, SqliteDataReader>(command);
}
