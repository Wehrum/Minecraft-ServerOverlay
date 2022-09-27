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
            return true;
        }
        else
        {
            Say(console, "To use: !tp (name) ex: !tp AConnor");
        }
        return false;
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
        Say(console, $"{Constants.Color.Red}This will RESTART the server, if you're sure type !confirm");
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
                Say(console, $"{Constants.Color.Red}Restarting the server in {i}!");
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

    public static void SetHomeLogic(Process console, string[] coordinates, string player, string homeName)
    {
        var data = ReadHomeConfig();
        if (homeName == string.Empty)
        {
            Command(console, $"tell {player} Give your home a name, dingus");
        }
        for (int i = 0; i < data.Players.Count; i++)
        {
            if (data.Players[i].Username == player)
            {
                if (data.Players[i].IsInWorld) //If the player is in the overworld
                {
                    if (data.Players[i].UserHomes != null) //If the player has homes to check
                    {
                        foreach (var item in data.Players[i].UserHomes) //If a player has a home with the same name, stop them from adding a new one
                        {
                            if (item.HomeName == homeName)
                            {
                                Command(console, $"tell {player} {Constants.Color.Red}Error: You already have a home set with that name. To list homes type !homes");
                                return;
                            }
                        }
                        data.Players[i].UserHomes.Add( //If there is a list, adds a new home to the list.
                            new Home{
                                HomeName = homeName,
                                Coordinates = coordinates
                            }
                        );
                    }
                    else //If there is no list, make a new list and add the players home
                    {
                        data.Players[i].UserHomes = new List<Home>(){
                            new Home {
                                HomeName = homeName,
                                Coordinates = coordinates
                            }};
                        break;
                    }
                }
                else //If the player is not in the overworld, don't let them set a home.
                {
                    Command(console, $"tell {player} {Constants.Color.Red}Error: !sethome functionality is only available in the overworld");
                    return;
                }
            }
        }
        double.TryParse(coordinates[0], out double x);
        double.TryParse(coordinates[1], out double y);
        double.TryParse(coordinates[2], out double z);
        Command(console, $"tell {player} successfully created home: '{homeName}' at X: {Math.Round(x)} Y: {Math.Round(y)} Z: {Math.Round(z)}");

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
                    if (data.Players[i].IsInWorld)
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
                        Command(console, $"tell {player} {Constants.Color.Red}Error: couldn't find home {result[5]}, use !homes to see your homes");
                        return;
                    }
                    else
                    {
                        Command(console, $"tell {player} {Constants.Color.Red}Error: !home functionality is only available in the overworld");
                        return;
                    }
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
                    if (data.Players[i].UserHomes != null)
                    {
                        if (data.Players[i].UserHomes.Count == 0)
                        {
                           Say(console, $"You don't have any homes, try making one with !sethome");   
                           return;
                        }
                        string homes = "Homes: ";
                        foreach (var item in data.Players[i].UserHomes)
                        {
                            homes += $"{item.HomeName}, ";
                        }
                        Command(console, $"tell {player} {homes.TrimEnd().TrimEnd(',')}");
                        return;
                    }
                    else
                    {
                        Say(console, $"You don't have any homes, try making one with !sethome");
                    }
                }
            }

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
                            data.Players[i].UserHomes.Remove(item);
                            WriteHomeConfig(data);

                            return;
                        }
                    }
                    Command(console, $"tell {player} {Constants.Color.Red}Error: couldn't find home: '{result[5]}', use !homes to see your homes");
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

    public static void UpdateDimension(Process console, string[] result)
    {
        if (result[5] == "joining" && result[6] == "dimension")
        {
            var data = ReadHomeConfig();
            string player = result[4];

            for (int i = 0; i < data.Players.Count; i++)
            {
                if (data.Players[i].Username == player)
                {
                    if (result[7] == "0")
                    {
                        data.Players[i].IsInWorld = true;
                        WriteHomeConfig(data);
                        return;
                    }
                    else
                    {
                        data.Players[i].IsInWorld = false;
                        WriteHomeConfig(data);
                        return;
                    }
                }
            }
            if (result[7] == "0") //If player is not found, we want to make a new one and keep track of this value
            {
                data.Players.Add(
                    new Player
                    {
                        Username = player,
                        IsInWorld = true
                    }
                );
            }
            else
            {
                data.Players.Add(
                    new Player
                    {
                        Username = player,
                        IsInWorld = false
                    }
                );
            }
            WriteHomeConfig(data);
        }
    }

    public static void Welcome(Process console, string[] result)
    {
        string player = result[10];
        string uuid = result[13];
        foreach (var item in Helper.WelcomeMessage(player))
        {
            Command(console, $"tell {player} {item}");
        }
        Command(console, $"tell {player} {Constants.Color.Red}Authenticating {Constants.Color.Red}with {Constants.Color.Red}UUID: {Constants.Color.Red}{uuid}");
    }
}
