using Microsoft.Data.Sqlite;

namespace ChromiumBasedAppLauncherCommon.Helpers.ForSQL.ForSqlite;

public class SqliteReadingSqlExecutor : AbstractReadingSqlExecutor<SqliteCommand, SqliteDataReader, SqliteReadingSqlExecutor>
{
    public SqliteReadingSqlExecutor() : base() { }

    public SqliteReadingSqlExecutor(SqliteCommand command) : base(command) { }

    public override SqliteReadingSqlExecutor AddParameterWithValue(string parameter, object? value)
    {
        Command.Parameters.AddWithValue(parameter, value);
        return this;
    }
}
