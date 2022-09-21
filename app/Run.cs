using System.Diagnostics;
using static Helper;

public class Run
{
    public void startApp()
    {
        Console.WriteLine("Running app");
        bool restart = false;

        var console = new Process();
        console.StartInfo = new ProcessStartInfo("/home/wehrum/madpack/TheMadPack/LaunchServer.sh")
        {
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
        };

        console.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (e.Data != null)
            {
                string[] result = e.Data.ToLower().Split(' ');
                if (result.Length > 4)
                {
                    try
                    {
                        if (result[3] == "completed" && result[4] == "server-stop")
                        {
                            startApp();
                        }
                        switch (result[4])
                        {
                            case "!tp":
                                Commands.Teleport(console, result);
                                break;
                            case "!difficulty":
                                Commands.Difficulty(console, result);
                                break;
                            case "!restart":
                                restart = true;
                                Say(console, "This will RESTART the server, if you're sure type !confirm");
                                break;
                            case "!confirm":
                                Commands.Confirm(restart, console);
                                break;
                            case "!":
                            case "!?":
                            case "!help":
                                Say(console, "Available commands:");
                                Thread.Sleep(1000);
                                Say(console, "- !difficulty");
                                Thread.Sleep(1000);
                                Say(console, "- !restart");
                                Thread.Sleep(1000);
                                Say(console, "- !tp");
                                break;
                        }
                    }
                    catch (Exception err)
                    {
                        Say(console, $"Serious error occured, let Connor know || Stack: {err.Message}");
                    }

                }

            }

            Console.WriteLine(e.Data);
        });
        console.Start();
        console.BeginOutputReadLine();


        //Prevent closing
        string result = Console.ReadLine() ?? String.Empty;
        Command(console, result);
        console.BeginOutputReadLine();
    }
}
