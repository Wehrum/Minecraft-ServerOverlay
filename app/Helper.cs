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
        File.WriteAllText($"{AppContext.BaseDirectory}homeconfig.json", JsonSerializer.Serialize(data, new JsonSerializerOptions {
             WriteIndented = true
         }));
    }

    public static void HomeConfigChecker()
    {
        if (!System.IO.File.Exists($"{AppContext.BaseDirectory}homeconfig.json"))
        {
            Console.WriteLine("Didn't find homeconfig.json, creating file..");
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
            Console.WriteLine("homeconfig.json found!");
        }
    }

    public static void SystemMessage(string message)
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        var time = DateTime.Now;
        Console.Write(time.ToString("[HH:mm:ss] "));
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[Server-Overlay]: ");
        Console.ResetColor();
        Console.Write($"{message} \r\n");
    }
}
