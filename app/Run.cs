using System.Diagnostics;
using static Helper;

public class Run
{
    public Process console;
    
    //Thread tid2 = new Thread(new ThreadStart(ServerOverlay));
    public void ServerOverlay()
    {
        Console.WriteLine("Running app");
        bool restart = false;
        string homeName = "";

        console = new Process();
        console.StartInfo = new ProcessStartInfo()
        {
            FileName = "bash",
            Arguments = "/home/connorwehrum/project/testserver/LaunchServer.sh",
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
                                Commands.Confirm(console, restart);
                                break;
                                case "!sethome":
                                if(Commands.SetHome(console, result));
                                {
                                    homeName = result[5];
                                }
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
                        switch(result[3])
                        {
                            //TODO: Add check to make sure this is only activated 
                            //when coming from overlay and not the console 
                            case "teleported": //When !sethome is called, console executed a teleport, check for this message to grab coords
                            var cords = result[6].Split(",");
                            if (cords.Length == 3)
                            {
                                string player = result[4];
                                Commands.SetHomeLogic(cords, player, homeName);
                            }
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
        Console.Read();
    }

    public void ConsoleReader()
    {
        try {
            var result = Console.ReadLine() ?? string.Empty;
        Command(console, result);
        ConsoleReader();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }
}
