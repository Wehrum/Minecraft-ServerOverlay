using System.Diagnostics;
using System.Text.Json;
using static Helper;

public class Commands
{
    public static void Teleport(Process console, string[] result)
    {
        if (result.Length == 6)
        {
            string firstPlayer = result[3].Replace(">", "").Replace("<", "");
            string secondPlayer = result[5];
            Command(console, $"tp {firstPlayer} {secondPlayer}");
            Say(console, $"Telporting {firstPlayer} to {secondPlayer}");
        }
    }

    public static void Difficulty(Process console, string[] result)
    {
        if (result.Length == 6)
        {
            for (int i = 0; i < Constants.Gamemodes.Length; i++)
            {
                if (Constants.Gamemodes[i] == result[5])
                {
                    Command(console, $"difficulty {result[5]}");
                    Say(console, "Switching difficulty to {result[5]}");
                    break;
                }
                else if (i == Constants.Gamemodes.Length - 1)
                {
                    Say(console, $"Switching difficulty to {result[5]}");
                }
            }
        }
        else
        {
            Say(console, $"To use: !difficulty (difficulty) ex: !difficulty hard");
        }
    }

    public static void Confirm(Process console, bool restart)
    {
        if (restart)
        {
            for (int i = 10; i > 0; i--)
            {
                Say(console, $"Restarting the server in {i}!");
                Thread.Sleep(1000);
            }
            console.StandardInput.WriteLine($"stop");
            Thread.Sleep(1000);
            Run startServer = new Run();
            startServer.ServerOverlay();
        }
        else
        {
            Say(console, "Please type !restart first to avoid accidental restarts");
        }
    }

    public static bool SetHome(Process console, string[] result)
    {
        if (result.Length == 6)
        {
            string player = result[3].Replace(">", "").Replace("<", "");
            Command(console, $"tp {player} ~ ~ ~");
            return true;
        }
        else
        {
            Say(console, "To use: !sethome (home) ex: !sethome myplace");
            return false;
        }
    }

    public static void SetHomeLogic (string[] coordinates, string userName, string homeName)
    {
        var jsonData = System.IO.File.ReadAllText($"{AppContext.BaseDirectory}/homeconfig.json");
        Data data = JsonSerializer.Deserialize<Data>(jsonData);

        for (int i = 0; i < data.Players.Count; i++)
        {
            if (data.Players[i].Username == userName)
            {
                data.Players[i].UserHomes.Add(
                    new Home
                    {
                        HomeName = homeName,
                        Coordinates = coordinates
                    }
                );
            }
            else 
            {
                data.Players.Add(
                    new Player
                    {
                        Username = userName,
                        UserHomes = new List<Home>()
                    }
                );
                
                data.Players[i].UserHomes.Add(
                    new Home
                    {
                        HomeName = homeName,
                        Coordinates = coordinates
                    }
                );
            }
        }

        File.WriteAllText($"{AppContext.BaseDirectory}/homeconfig.json", JsonSerializer.Serialize(data, new JsonSerializerOptions {
             WriteIndented = true
         }));
    }

    public static void Home(string[] result, Process console)
    {

    }
}
