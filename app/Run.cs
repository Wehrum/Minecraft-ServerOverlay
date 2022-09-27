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
        bool tpWasCalled = false;

        console = new Process();

        console.StartInfo = new ProcessStartInfo("") // <------ Linux
        {
            FileName = "bash",
            Arguments = "/home/connorwehrum/project/SevTechAges/SevTechAges/LaunchServer.sh",
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
        };

        // console.StartInfo = new ProcessStartInfo("C:\\Users\\connorwehrum\\Downloads\\minecraft-server\\start.bat") // <---- Windows
        // {
        //     RedirectStandardOutput = true,
        //     RedirectStandardInput = true,
        //     UseShellExecute = false,
        //     WorkingDirectory = "C:\\Users\\connorwehrum\\Downloads\\minecraft-server\\"
        // };

        console.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (e.Data != null)
            {
                string[] result = e.Data.ToLower().Split(' ');
                if (result.Length > 5)
                {
                    if (e.Data.Contains("[Actually Additions]: Sending Player Data to"))
                    {
                        Commands.Welcome(console, result);
                    }
                    if (result[3] == "[journeymap]:" && result.Length == 8)
                    {
                        Commands.UpdateDimension(console, result);
                    }
                    var foos = new List<string>(result);
                    foos.RemoveAt(3);
                    if (result[4] == "teleported" && setHomeWasCalled)
                    {
                        foos.RemoveAt(5);
                    }
                    result = foos.ToArray();
                }
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
                                if (setHomeWasCalled)
                                {
                                    string[] cords = { result[5].Replace(",", ""), result[6].Replace(",", ""), result[7].Replace(",", "") };
                                    string player = result[4];
                                    Commands.SetHomeLogic(console, cords, player, homeName);
                                    homeName = "";
                                    setHomeWasCalled = false;
                                    tpWasCalled = false;
                                }
                                else if (tpWasCalled)
                                {
                                    Say(console, $"{String.Join(" ", result, 3, result.Count() - 3)}");
                                    tpWasCalled = false;
                                }
                                break;
                            case "entity": //when !teleport is called, console will try to teleport
                                         //we look for the first word of error message, "that"
                                         //to validate if the command was successful
                                if (tpWasCalled)
                                {
                                    Say(console, $"{Constants.Color.Red}Error: That player could not be found");
                                    tpWasCalled = false;
                                }
                                break;
                        }
                    }
                    catch (Exception err)
                    {
                        Say(console, $"{Constants.Color.Red}Serious error occured, let Connor know || Stack: {err.Message}");
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