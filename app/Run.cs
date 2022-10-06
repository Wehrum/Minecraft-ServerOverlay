using System.Diagnostics;
using static Helper;

public class Run
{
    public Process console;
    public void ServerOverlay()
    {
        SystemMessage("Starting Server-Overlay");
        Helper.HomeConfigChecker(); //Runs a check to see if homeconfig.json exists, if not, create and populate it.
        bool restart = false;
        string homeName = "";
        bool setHomeWasCalled = false;
        bool tpWasCalled = false;

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
                                if (Commands.Teleport(console, result))
                                {
                                    tpWasCalled = true;
                                }
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
                            case "teleported": //When !sethome is called, console executed a teleport, 
                                               //check for this message to grab coords
                                var cords = result[6].Split(",");
                                if (setHomeWasCalled)
                                {
                                    string player = result[4];
                                    Commands.SetHomeLogic(console, cords, player, homeName);
                                    homeName = "";
                                    setHomeWasCalled = false;
                                }
                                if (tpWasCalled)
                                {
                                    Say(console, $"{String.Join(" ", result, 3, result.Count()-3)}");
                                    tpWasCalled = false;
                                }
                                break;
                                case "that": //when !teleport is called, console will try to teleport
                                             //we look for the first word of error message, "that"
                                             //to validate if the command was successful
                                 if (tpWasCalled && result[4] == "player")
                                 {
                                     Say(console, "Error: That player could not be found");
                                     tpWasCalled = false;
                                 }
                                break;
                        }
                    }
                    catch (Exception err)
                    {
                        SystemMessage(console, $"Serious error occured, let Connor know: {err}");
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
                SystemMessage("Exiting program due to console exit.");
                Environment.Exit(0);
            }
            Command(console, result);
            ConsoleReader();
        }
        catch (Exception e)
        {
            SystemMessage(e.Message);
        }

    }
}