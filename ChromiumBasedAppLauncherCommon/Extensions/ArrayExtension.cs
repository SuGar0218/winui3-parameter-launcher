using System.Text;

namespace ChromiumBasedAppLauncherCommon.Extensions;

public static class ArrayExtension
{
    public static string ToString<T>(this T[] array, char delim) where T : notnull
    {
        StringBuilder stringBuilder = new(array[0].ToString());
        for (int i = 1; i < array.Length; i++)
        {
            stringBuilder.Append(delim).Append(array[i].ToString());
        }
        return stringBuilder.ToString();
    }

    public static string ToString<T>(this T[] array, char delim, char beforeEach, char afterEach) where T : notnull
    {
        StringBuilder stringBuilder = new StringBuilder().Append(beforeEach).Append(array[0].ToString()).Append(afterEach);
        for (int i = 1; i < array.Length; i++)
        {
            stringBuilder.Append(delim).Append(beforeEach).Append(array[i].ToString()).Append(afterEach);
        }
        return stringBuilder.ToString();
    }
}
