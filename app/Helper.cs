using System.Diagnostics;

public static class Helper
{
     public static void Say(string message, Process process)
    {
        process.StandardInput.WriteLine($"say {message}");
    }

    public static void Command(string message, Process process)
    {
        process.StandardInput.WriteLine(message);
    }
}
