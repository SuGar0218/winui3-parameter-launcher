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
        Process process = new();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        process.StandardInput.WriteLine($"copy \"{src}\" \"{dst}\" &exit");
        process.WaitForExit();
    }
}
