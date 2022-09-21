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
        if (!System.IO.File.Exists($"{AppContext.BaseDirectory}/homeconfig.json"))
        {
            var obj = new DataModel
            {
                Players = new List<PlayerModel>
                {
                    new PlayerModel
                    {
                        UserHomes = new List<HomeModel>
                        {
                            new HomeModel{
                                Cordinates = new string[] {""}
                            }
                        }
                    }
                }
                
            };
            File.AppendAllText($"{AppContext.BaseDirectory}/homeconfig.json", JsonSerializer.Serialize(obj));
        }
    }
}
