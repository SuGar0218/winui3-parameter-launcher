using System.Data;
using System.Data.Common;

namespace ChromiumBasedAppLauncherCommon.Helpers.ForSQL;

public abstract class AbstractReadingSqlExecutor<TDbCommand, TDbDataReader, TSelf> : AbstractSqlExecutor<TDbCommand, TDbDataReader, TSelf>
    where TDbCommand : DbCommand
    where TDbDataReader : DbDataReader
{
    public AbstractReadingSqlExecutor() : base() { }

    protected AbstractReadingSqlExecutor(TDbCommand command) : base(command) { }

    public List<TData> ExecuteDataReading<TData>(Func<TDbDataReader, TData> ReadDataFromReader)
    {
        List<TData> data = [];
        using (TDbDataReader reader = (TDbDataReader) Command.ExecuteReader())
        {
            while (reader.Read())
            {
                data.Add(ReadDataFromReader(reader));
            }
        }
        return data;
    }

    public List<TData> ExecuteDataReading<TData>(Func<TDbDataReader, TData> ReadDataFromReader, CommandBehavior behavior)
    {
        List<TData> data = [];
        using (TDbDataReader reader = (TDbDataReader) Command.ExecuteReader(behavior))
        {
            while (reader.Read())
            {
                data.Add(ReadDataFromReader(reader));
            }
        }
        return data;
    }

    public List<TData> ExecuteColumnReading<TData>(int column = 0)
    {
        List<TData> data = [];
        using (TDbDataReader reader = (TDbDataReader) Command.ExecuteReader())
        {
            while (reader.Read())
            {
                data.Add(reader.GetFieldValue<TData>(column));
            }
        }
        return data;
    }

    public List<TData> ExecuteColumnReading<TData>(int column, CommandBehavior behavior)
    {
        List<TData> data = [];
        using (TDbDataReader reader = (TDbDataReader) Command.ExecuteReader(behavior))
        {
            while (reader.Read())
            {
                data.Add(reader.GetFieldValue<TData>(column));
            }
        }
        return data;
    }

    public TData? ExecuteScalarReading<TData>()
    {
        using TDbDataReader reader = (TDbDataReader) Command.ExecuteReader();
        return reader.Read() ? reader.GetFieldValue<TData>(0) : default;
    }

    public object? ExecuteScalarReading() => Command.ExecuteScalar();
}
