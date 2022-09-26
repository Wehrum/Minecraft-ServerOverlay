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
    
    public static string[] WelcomeMessage (string Username)
    {
        string[] WelcomeMessage =
        {
            @$"{Constants.Color.Yellow}\/\/\/\/\/\/\/\/\/\/",
            @$"{Constants.Color.Aqua}Welcome {Constants.Color.Green}{Username}, {Constants.Color.Aqua}to",
            @$"{Constants.Color.Aqua}Connor's {Constants.Color.Aqua}SevTech {Constants.Color.Aqua}Server",
            @$"{Constants.Color.Aqua}Running {Constants.Color.Aqua}ServerOverlay {Constants.Color.Aqua}0.1",
            @$"{Constants.Color.Aqua}Type {Constants.Color.Red}!help {Constants.Color.Aqua}for {Constants.Color.Aqua}a {Constants.Color.Aqua}list {Constants.Color.Aqua}of {Constants.Color.Aqua}commands",
            @$"{Constants.Color.Yellow}/\/\/\/\/\/\/\/\/\/\"
        };
        return WelcomeMessage;
    }
}
