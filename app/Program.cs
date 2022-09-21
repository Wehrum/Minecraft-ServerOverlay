using System.Text.Json;
//using Newtonsoft.Json;
using System.Text.Json.Serialization;

string[] bla = {"50", "20", "10"};

// PlayerModel myplayer = new PlayerModel()
// {
//     Username = "testusername",
//     UserHomes = new HomeModel[]
//     {
//         new HomeModel {
//          Home = "Home1",
//          Cordinates = bla   
//         },
//         new HomeModel {
//             Home = "Home3",
//             Cordinates = bla
//         }
//     }
// };

// string json = JsonSerializer.Serialize(myplayer);
// var yo = $"@{AppContext.BaseDirectory}homeconfig.json";
// File.AppendAllText($"{AppContext.BaseDirectory}/homeconfig.json", json);

Helper.FileChecker();

var jsonData = System.IO.File.ReadAllText($"{AppContext.BaseDirectory}/homeconfig.json");

//var data = JsonSerializer.Deserialize<List<<DataModel>>(jsonData);

DataModel data = JsonSerializer.Deserialize<DataModel>(jsonData);
Console.ReadKey();

// var myapp = new Run();

// Thread Th1 = new Thread(new ThreadStart(myapp.ServerOverlay));
// Thread Th2 = new Thread(new ThreadStart(myapp.ConsoleReader));
// Th1.Start();
// Th2.Start();