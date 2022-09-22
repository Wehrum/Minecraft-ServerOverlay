using System.Diagnostics;
using static Helper;

public class Run
{
    public Process console;
    public void ServerOverlay()
    {
        Console.WriteLine("Running app");
        bool restart = false;
        string homeName = "";
        bool setHomeWasCalled = false;

        console = new Process();

// console.StartInfo = new ProcessStartInfo("") // <------ Linux
//         {
//             FileName = "bash", 
               //Arguments = "/home/connorwehrum/project/testserver/LaunchServer.sh", 
//             RedirectStandardOutput = true,
//             RedirectStandardInput = true,
//             UseShellExecute = false,
//             WorkingDirectory = "C:\\Users\\connorwehrum\\Downloads\\minecraft-server\\"
//         };

        console.StartInfo = new ProcessStartInfo("C:\\Users\\connorwehrum\\Downloads\\minecraft-server\\start.bat") // <---- Windows
        {
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
            WorkingDirectory = "C:\\Users\\connorwehrum\\Downloads\\minecraft-server\\"
        };

        console.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (e.Data != null)
            {
                string[] result = e.Data.ToLower().Split(' ');
                if (result.Length > 4 && result[3] != "[server]")
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
                                restart = Commands.Restart(console);
                                break;
                            case "!confirm":
                                if (Commands.Confirm(restart, console))
                                {
                                    ServerOverlay();
                                }
                                break;
                            case "!sethome":
                                if (Commands.SetHome(console, result))
                                {
                                    homeName = result[5];
                                    setHomeWasCalled = true;
                                }
                                break;
                            case "!homes":
                            case "!listhome":
                            case "!listhomes":
                                Commands.ListHomes(console, result);
                                break;
                            case "!home":
                                Commands.Home(console, result);
                                break;
                            case "!delhome":
                            case "!removehome":
                            case "!deletehome":
                                Commands.DeleteHome(console, result);
                                break;
                            case "!":
                            case "!?":
                            case "!help":
                                Commands.Help(console, result);
                                break;
                        }
                        switch (result[3])
                        {
                            //TODO: Add check to make sure this is only activated 
                            //when coming from overlay and not the console 
                            case "teleported": //When !sethome is called, console executed a teleport, 
                                               //check for this message to grab coords
                                var cords = result[6].Split(",");
                                if (cords.Length == 3 && setHomeWasCalled)
                                {
                                    string player = result[4];
                                    Commands.SetHomeLogic(console, cords, player, homeName);
                                    homeName = "";
                                    setHomeWasCalled = false;
                                }
                                break;
                        }
                    }
                    catch (Exception err)
                    {
                        Say(console, $"Serious error occured, let Connor know || Stack: {err.Message}");
                        Console.WriteLine(err);
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
        try
        {
            var result = Console.ReadLine() ?? string.Empty;
            if (console.HasExited)
            {
                Console.WriteLine("Exiting program due to console exit.");
            }
            Command(console, result);
            ConsoleReader();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}