using System.Diagnostics;
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

    public static void SetHome(Process console, string[] result)
    {
        if (result.Length == 6)
        {
             string player = result[3].Replace(">", "").Replace("<", "");
            Command(console, $"tp {player} ~ ~ ~");
        }
        else
        {
            Say(console, "To use: !sethome (home) ex: !sethome myplace");
        }
    }

    public static void Home(string[] result, Process console)
    {

    }
}
