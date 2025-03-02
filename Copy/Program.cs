using System.Diagnostics;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Copy from to");
            return;
        }
        string src = args[0];
        string dst = args[1];
        File.Copy(src, dst, overwrite: true);
    }
}
