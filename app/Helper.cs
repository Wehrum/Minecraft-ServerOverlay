using System.Diagnostics;
using System.Text.Json;
using static Constants;

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
    
    public static string[] WelcomeMessage (string Username)
    {
        string[] WelcomeMessage =
        {
            @$"{Color.Yellow}\/\/\/\/\/\/\/\/\/\/",
            @$"{Color.Aqua}Welcome {Color.Green}{Username}, {Color.Aqua}to",
            @$"{Color.Aqua}Connor's {Color.Aqua}SevTech {Color.Aqua}Server",
            @$"{Color.Aqua}Running {Color.Aqua}ServerOverlay {Color.Aqua}0.1",
            @$"{Color.Aqua}Type {Color.Red}!help {ColorSetter("for a list of commands", Color.Aqua)}",
            @$"{Color.Yellow}/\/\/\/\/\/\/\/\/\/\"
        };
        return WelcomeMessage;
    }

    public static string ColorSetter(string sentence, string color)
    {
        string[] split = sentence.Split(' ');
        string result = "";
        foreach (var item in split)
        {
            result += $"{item.Insert(0, color)} ";
        }
        return result.TrimEnd();
    }
}
