using System.Diagnostics;
using static Helper;

public class Commands
{
    public static bool Teleport(Process console, string[] result)
    {
        if (result.Length == 6)
        {
            string firstPlayer = result[3].Replace(">", "").Replace("<", "");
            string secondPlayer = result[5];
            Command(console, $"tp {firstPlayer} {secondPlayer}");
            //Say(console, $"Telporting {firstPlayer} to {secondPlayer}");
            return true;
            //TODO: Add validation to check if player is valid.
        }
        else
        {
            Say(console, "To use: !tp (name) ex: !tp AConnor");
        }
        return false;
    }

    public static void Teleport_Logic(Process console, string[] result)
    {

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
                    Say(console, $"Switching difficulty to {result[5]}");
                    return;
                }
            }
            Say(console, $"Error: Unknown difficulty");
        }
        else
        {
            Say(console, $"To use: !difficulty (difficulty) ex: !difficulty hard");
        }
    }

    public static bool Restart(Process console)
    {
        Say(console, "This will RESTART the server, if you're sure type !confirm");
        return true;
    }

    public static bool Confirm(bool restart, Process console)
    {
        bool resetCalled = false;
        if (restart)
        {
            Console.WriteLine("Restarting the server, please wait.");
            for (int i = 10; i > 0; i--)
            {
                Say(console, $"Restarting the server in {i}!");
                Thread.Sleep(1000);
            }
            console.StandardInput.WriteLine($"stop");
            Thread.Sleep(1000);
            resetCalled = true;
            return resetCalled;
        }
        else
        {
            Say(console, "Please type !restart first to avoid accidental restarts");
            return resetCalled;
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

    public static void SetHomeLogic(Process console, string[] coordinates, string userName, string homeName)
    {
        var data = ReadHomeConfig();
        if (homeName == string.Empty)
        {
            Command(console, $"tell {userName} Give your home a name, dingus");
        }
        for (int i = 0; i < data.Players.Count; i++)
        {
            if (data.Players[i].Username == userName)
            {
                foreach (var item in data.Players[i].UserHomes)
                {
                    if (item.HomeName == homeName)
                    {
                        Command(console, $"tell {userName} Error: You already have a home set with that name. To list homes type !homes");
                        return;
                    }
                }
                data.Players[i].UserHomes.Add(
                    new Home
                    {
                        HomeName = homeName,
                        Coordinates = coordinates
                    }
                );
                break;

            }
            else if (i == data.Players.Count - 1)
            {
                var initialHome = new List<Home>();
                initialHome.Add(new Home
                {
                    HomeName = homeName,
                    Coordinates = coordinates
                });

                data.Players.Add(
                    new Player
                    {
                        Username = userName,
                        UserHomes = initialHome
                    }
                );
                break;
            }
        }
        double.TryParse(coordinates[0], out double x);
        double.TryParse(coordinates[1], out double y);
        double.TryParse(coordinates[2], out double z);
        Command(console, $"tell {userName} successfully created home: '{homeName}' at X: {Math.Round(x)} Y: {Math.Round(y)} Z: {Math.Round(z)}");

        WriteHomeConfig(data);
    }

    public static void Home(Process console, string[] result)
    {
        if (result.Length == 6)
        {
            var data = ReadHomeConfig();

            string player = result[3].Replace(">", "").Replace("<", "");

            for (int i = 0; i < data.Players.Count; i++)
            {
                if (data.Players[i].Username == player)
                {
                    foreach (var item in data.Players[i].UserHomes)
                    {
                        if (item.HomeName == result[5])
                        {
                            Command(console, $"tell {player} Teleporting to home: '{result[5]}'");
                            Command(console, $"tp {player} {item.Coordinates[0]} {item.Coordinates[1]} {item.Coordinates[2]}");
                            return;
                        }
                    }
                    Command(console, $"tell {player} Error: couldn't find home {result[5]}, use !homes to see your homes");
                    return;
                }
            }
            Say(console, $"You don't have any homes, try making one with !sethome");
            return;
        }
        else
        {
            Say(console, $"To use: !home (home) ex: !home myhome");
        }
    }

    public static void ListHomes(Process console, string[] result)
    {
        if (result.Length == 5)
        {
            var data = ReadHomeConfig();

            string player = result[3].Replace(">", "").Replace("<", "");

            for (int i = 0; i < data.Players.Count; i++)
            {
                if (data.Players[i].Username == player)
                {
                    string homes = "Homes: ";
                    foreach (var item in data.Players[i].UserHomes)
                    {
                        homes += $"{item.HomeName}, ";
                    }
                    Command(console, $"tell {player} {homes.TrimEnd().TrimEnd(',')}");
                    return;
                }
            }
            Say(console, $"You don't have any homes, try making one with !sethome");
        }
        else
        {
            Say(console, $"To use: !homes OR !listhomes ex: !homes");
        }


    }

    public static void DeleteHome(Process console, string[] result)
    {
        if (result.Length == 6)
        {
            var data = ReadHomeConfig();

            string player = result[3].Replace(">", "").Replace("<", "");

            for (int i = 0; i < data.Players.Count; i++)
            {
                if (data.Players[i].Username == player)
                {
                    foreach (var item in data.Players[i].UserHomes)
                    {
                        if (item.HomeName == result[5])
                        {
                            Command(console, $"tell {player} Deleting home: '{result[5]}'");

                            if (data.Players[i].UserHomes.Count > 1)
                            {
                                data.Players[i].UserHomes.Remove(item);
                                WriteHomeConfig(data);
                            }
                            else
                            {
                                data.Players.Remove(data.Players[i]);
                                WriteHomeConfig(data);
                            }
                            return;
                        }
                    }
                    Command(console, $"tell {player} Error: couldn't find home: '{result[5]}', use !homes to see your homes");
                    return;
                }
            }
            Say(console, $"You don't have any homes, try making one with !sethome");
        }
        else
        {
            Say(console, $"To use: !delhome (home) ex: !delhome myplace");
        }

    }

    public static void Help(Process console, string[] result)
    {
        string player = result[3].Replace(">", "").Replace("<", "");
        Command(console, $"tell {player} /// Commands ///");
        foreach (var item in Constants.Commands)
        {
            Command(console, $"tell {player} - {item}");
        }
    }
}
