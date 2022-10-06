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
            SystemMessage($"Can't find {config}!");
            SystemMessage($"Generating {config}");
            switch (config.ToLower())
            {
                case "homeconfig.json":
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
        Console.ForegroundColor = ConsoleColor.Red;
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
}
