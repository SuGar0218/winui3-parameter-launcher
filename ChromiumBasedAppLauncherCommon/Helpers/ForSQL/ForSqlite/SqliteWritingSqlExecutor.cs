using Microsoft.Data.Sqlite;

namespace ChromiumBasedAppLauncherCommon.Helpers.ForSQL.ForSqlite;

public class SqliteWritingSqlExecutor : AbstractWritingSqlExecutor<SqliteCommand, SqliteDataReader, SqliteWritingSqlExecutor>
{
    public SqliteWritingSqlExecutor() : base() { }

    public SqliteWritingSqlExecutor(SqliteCommand command) : base(command) { }

    public override SqliteWritingSqlExecutor AddParameterWithValue(string parameter, object? value)
    {
        Command.Parameters.AddWithValue(parameter, value);
        return this;
    }
}
