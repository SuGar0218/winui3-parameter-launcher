using ChromiumBasedAppLauncherCommon.Extensions;

using System.Data.Common;
using System.Text;

namespace ChromiumBasedAppLauncherCommon.Helpers.ForSQL;

public class SqlBuilder
{
    public SqlBuilder(string sql)
    {
        stringBuilder = new StringBuilder(sql);
    }

    public static SqlBuilder Select(string columns) => new($"SELECT {columns}");
    public static SqlBuilder Select(string[] columns) => new($"SELECT {columns.ToString(',')}");
    public static SqlBuilder Select1 => Select("1");

    public static SqlBuilder InsertInto(string table) => new($"INSERT INTO {table}");
    public static SqlBuilder InsertInto(string table, string fields) => new($"INSERT INTO {table} ({fields})");
    public static SqlBuilder InsertInto(string table, string[] fields) => new($"INSERT INTO {table} ({fields.ToString(',')})");

    public static SqlBuilder Update(string table) => new($"UPDATE {table}");

    public static SqlBuilder DeleteFrom(string table) => new($"DELETE FROM {table}");

    public static SqlBuilder Not(string condition) => new(new StringBuilder().Append(' ').Append($"NOT {condition}"));

    public static SqlBuilder Exists(string sql) => new(new StringBuilder().Append(' ').Append($"EXISTS ({sql})"));
    public static SqlBuilder NotExists(string sql) => Not(Exists(sql));

    public SqlBuilder From(string tables)
    {
        stringBuilder.Append(' ').Append($"FROM {tables}");
        return this;
    }

    public SqlBuilder From(string[] tables)
    {
        stringBuilder.Append(' ').Append($"FROM {tables.ToString(',')}");
        return this;
    }

    public SqlBuilder Where(string condition)
    {
        stringBuilder.Append(' ').Append($"WHERE {condition}");
        return this;
    }

    public SqlBuilder Join(string table)
    {
        stringBuilder.Append(' ').Append($"JOIN {table}");
        return this;
    }

    public SqlBuilder On(string condition)
    {
        stringBuilder.Append(' ').Append($"ON {condition}");
        return this;
    }

    public SqlBuilder And(string condition)
    {
        stringBuilder.Append(' ').Append($"AND {condition}");
        return this;
    }

    public SqlBuilder Or(string condition)
    {
        stringBuilder.Append(' ').Append($"OR {condition}");
        return this;
    }

    public SqlBuilder Limit(int limit)
    {
        stringBuilder.Append(' ').Append($"LIMIT {limit}");
        return this;
    }

    public SqlBuilder Limit(int start, int limit)
    {
        stringBuilder.Append(' ').Append($"LIMIT {start},{limit}");
        return this;
    }

    public SqlBuilder OrderBy(string columns, OrderBy order)
    {
        stringBuilder.Append(' ').Append($"ORDER BY {columns} {order}");
        return this;
    }

    public SqlBuilder OrderBy(string[] columns, OrderBy order)
    {
        stringBuilder.Append(' ').Append($"ORDER BY {columns.ToString(',')} {order}");
        return this;
    }

    public SqlBuilder GroupBy(string columns)
    {
        stringBuilder.Append(' ').Append($"GROUP BY {columns}");
        return this;
    }

    public SqlBuilder GroupBy(string[] columns)
    {
        stringBuilder.Append(' ').Append($"GROUP BY {columns.ToString(',')}");
        return this;
    }

    public SqlBuilder Values(string values)
    {
        stringBuilder.Append(' ').Append($"VALUES {values}");
        return this;
    }

    public SqlBuilder Values(string[] values)
    {
        stringBuilder.Append(' ').Append($"VALUES {values.ToString(',')}");
        return this;
    }

    public SqlBuilder Set(string changes)
    {
        stringBuilder.Append(' ').Append($"SET {changes}");
        return this;
    }

    public SqlBuilder Set(string[] changes)
    {
        stringBuilder.Append(' ').Append($"SET {changes.ToString(',')}");
        return this;
    }

    public virtual TReadingSqlExecutor ForCommandReading<TReadingSqlExecutor, TDbCommand, TDbDataReader>(TDbCommand command)
        where TReadingSqlExecutor : AbstractReadingSqlExecutor<TDbCommand, TDbDataReader, TReadingSqlExecutor>, new()
        where TDbCommand : DbCommand
        where TDbDataReader : DbDataReader
    {
        TReadingSqlExecutor executor = new();
        executor.Command = command;
        executor.CommandText = stringBuilder.ToString();
        return executor;
    }

    public virtual TWritingSqlExecutor ForCommandWriting<TWritingSqlExecutor, TDbCommand, TDbDataReader>(TDbCommand command)
        where TWritingSqlExecutor : AbstractWritingSqlExecutor<TDbCommand, TDbDataReader, TWritingSqlExecutor>, new()
        where TDbCommand : DbCommand
        where TDbDataReader : DbDataReader
    {
        TWritingSqlExecutor executor = new();
        executor.Command = command;
        executor.CommandText = stringBuilder.ToString();
        return executor;
    }

    public override string ToString() => stringBuilder.ToString();

    public static implicit operator string(SqlBuilder self) => self.ToString();

    internal SqlBuilder(StringBuilder stringBuilder)
    {
        this.stringBuilder = stringBuilder;
    }

    private readonly StringBuilder stringBuilder;
}
