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
        var jsonData = System.IO.File.ReadAllText($"{AppContext.BaseDirectory}data/homeconfig.json");
        Data data = JsonSerializer.Deserialize<Data>(jsonData) ?? throw new NullReferenceException();

        return data;
    }

    public static ServerOverlay ReadServerConfig()
    {
        var jsonData = System.IO.File.ReadAllText($"{AppContext.BaseDirectory}data/serveroverlay.json");
        ServerOverlay data = JsonSerializer.Deserialize<ServerOverlay>(jsonData) ?? throw new NullReferenceException();

        return data;
    }

    public static void WriteConfig(object obj, string config)
    {
        if (!System.IO.Directory.Exists($"{AppContext.BaseDirectory}data/"))
        {
            System.IO.Directory.CreateDirectory($"{AppContext.BaseDirectory}data/");
        }

        File.WriteAllText($"{AppContext.BaseDirectory}data/{config}",
        JsonSerializer.Serialize(obj, new JsonSerializerOptions
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
            SystemMessage($"{config} found!", ConsoleColor.Green);
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
        SystemMessage("Consult the documentation at my GitHub for further information: https://github.com/Wehrum/Minecraft-ServerOverlay", ConsoleColor.Yellow);
        var ServerOverlay = new ServerOverlay
        {
            File = @"Enter start.bat filename located in your Minecraft Server folder Ex: myserver.bat"
        };
        WriteConfig(ServerOverlay, "serveroverlay.json");
        SystemMessage($"Successfully created serveroverlay.json at: {AppContext.BaseDirectory}data\\serveroverlay.json", ConsoleColor.Green);
        SystemMessage("Please open and edit this file to continue", ConsoleColor.Yellow);
        SystemMessage("Press any key to exit..", ConsoleColor.Yellow);
        Console.ReadKey();
        Environment.Exit(1);
    }
}