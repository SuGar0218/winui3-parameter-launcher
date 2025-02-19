namespace ChromiumBasedAppLauncherCommon.Extensions;

public static class JavaLikeExtension
{
    public static void AddAll<T>(this ICollection<T> collection, ICollection<T> items)
    {
        foreach (T item in items)
        {
            collection.Add(item);
        }
    }
}
