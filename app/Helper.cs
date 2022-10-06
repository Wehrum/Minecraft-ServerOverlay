using System.Diagnostics;
using System.Text.Json;

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

    public static Data ReadHomeConfig()
    {
        var jsonData = System.IO.File.ReadAllText($"{AppContext.BaseDirectory}homeconfig.json");
        Data data = JsonSerializer.Deserialize<Data>(jsonData) ?? throw new NullReferenceException();

        return data;
    }

    public static void WriteConfig(Data data, string config)
    {
        File.WriteAllText($"{AppContext.BaseDirectory}data/{config}",
        JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }

    public static bool ConfigCheck(string config)
    {
        if (!System.IO.File.Exists($"{AppContext.BaseDirectory}data/{config}"))
        {
            switch (config.ToLower())
            {
                case "homeconfig.json":
                    SystemMessage($"Can't find {config}!", ConsoleColor.Red);
                    SystemMessage($"Generating {config}", ConsoleColor.Yellow);
                    var obj = new Data
                    {
                        Players = new List<Player>
                        {
                            new Player
                            {
                                UserHomes = new List<Home>
                                {
                                    new Home{
                                        Coordinates = new string[] {""}
                                    }
                                }
                            }
                        }
                    };
                    WriteConfig(obj, "homeconfig.json");
                    break;
                case "serveroverlay.json":
                    return false;
                default:
                    SystemMessage("Error: Config checker failed, please report this!");
                    return false;
            }
        }
        else
        {
            SystemMessage($"{config} found!");
            return true;
        }
        return true;
    }

    public static void SystemMessage(Process console, string message)
    {
        if (!console.HasExited)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1); // Unavoidable issue that makes a new line when switching from
            Console.Write(new string(' ', Console.BufferWidth)); // output to console writeline. this removes that line for better
            Console.SetCursorPosition(0, Console.CursorTop - 1); // readability. This makes it more flush with Minecraft console.   
        }
        var time = DateTime.Now;
        Console.Write(time.ToString("[HH:mm:ss] "));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[Server-Overlay]: ");
        Console.ResetColor();
        Console.Write($"{message} \r\n");
    }

    public static void SystemMessage(string message)
    {
        var time = DateTime.Now;
        Console.Write(time.ToString("[HH:mm:ss] "));
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[Server-Overlay]: ");
        Console.ResetColor();
        Console.Write($"{message} \r\n");
    }

    public static void SystemMessage(string message, ConsoleColor color)
    {
        var time = DateTime.Now;
        Console.Write(time.ToString("[HH:mm:ss] "));
        Console.ForegroundColor = color;
        Console.Write("[Server-Overlay]: ");
        Console.ResetColor();
        Console.Write($"{message} \r\n");
    }

    public static void FirstTimeSetup()
    {
        SystemMessage("Performing first time setup.", ConsoleColor.Yellow);
        SystemMessage("Welcome to ServerOverlay! A custom console overlay for your Minecraft server.", ConsoleColor.Yellow);
        SystemMessage($"After setup, locate your config file at: {AppContext.BaseDirectory}data\\serveroverlay.json", ConsoleColor.Yellow);
        SystemMessage("Consult the documentation at my GitHub for further information: https://github.com/Wehrum/Minecraft-ServerOverlay", ConsoleColor.Yellow);
        SystemMessage("Would you like to generate and edit serveroverlay.json manually? (Not recommeneded unless you know what you're doing)", ConsoleColor.Green);
        Console.Write("[Y/N]: ");
        ConsoleKeyInfo info = Console.ReadKey(false);
        if (info.Key == ConsoleKey.N)
        {
            Console.Clear();
            SystemMessage("Point me to your start.bat file for the server", ConsoleColor.Green);
            SystemMessage("EX: C:\\Users\\bob\\Downloads\\server\\start.bat", ConsoleColor.Green);
            Console.Write("Path to file: ");
            var result = Console.ReadLine();
            while (!System.IO.File.Exists(result))
            {
                SystemMessage($"I can't find {result}, double check the path!", ConsoleColor.Red);
                Console.Write("Path to file: ");
                result = Console.ReadLine();
            }
            if (!result.EndsWith(".bat"))
            {
                SystemMessage($"The file should end in .bat this is usually found in your Minecraft server folder", ConsoleColor.Red);
                SystemMessage($"Please doublecheck you're pointing to the right file", ConsoleColor.Red);
                SystemMessage($"If you're still confused, please consult the GitHub documenation at:", ConsoleColor.Red);
                SystemMessage($"https://github.com/Wehrum/Minecraft-ServerOverlay", ConsoleColor.Red);
                while (!result.EndsWith(".bat"))
                {
                   Console.Write("Path to file: ");
                   result = Console.ReadLine();
                }
            }
            SystemMessage("", ConsoleColor.Yellow);
            SystemMessage("", ConsoleColor.Yellow);
            SystemMessage("", ConsoleColor.Yellow);
        }
        else
        {
            //Generate file code
        }
    }
}
