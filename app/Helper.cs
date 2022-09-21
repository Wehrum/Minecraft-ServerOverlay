using System.Diagnostics;

public static class Helper
{
     public static void Say(Process process, string message)
    {
        process.StandardInput.WriteLine($"say {message}");
    }

    public static void Command(Process process, string message)
    {
        process.StandardInput.WriteLine(message);
    }
}
