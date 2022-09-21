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

    public static void FileChecker()
    {
        if (!System.IO.File.Exists($"{AppContext.BaseDirectory}homeconfig.json"))
        {
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
            File.WriteAllText($"{AppContext.BaseDirectory}homeconfig.json", JsonSerializer.Serialize(obj, new JsonSerializerOptions {
             WriteIndented = true
         }));
        }
    }
}
