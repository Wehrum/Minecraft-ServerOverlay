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

    public static void WriteHomeConfig(Data data)
    {
        File.WriteAllText($"{AppContext.BaseDirectory}homeconfig.json", JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }

    public static void HomeConfigChecker()
    {
        if (!System.IO.File.Exists($"{AppContext.BaseDirectory}homeconfig.json"))
        {
            SystemMessage("Didn't find homeconfig.json, creating file..");
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
            WriteHomeConfig(obj);
        }
        else
        {
            SystemMessage("homeconfig.json found!");
        }
    }

    public static void SystemMessage(Process console, string message)
    {
        if(!console.HasExited)
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
}
