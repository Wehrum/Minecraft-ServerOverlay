using System.Text.Json;
using System.Text.Json.Serialization;

string[] bla = {"50", "20", "10"};

PlayerModel myplayer = new PlayerModel()
{
    Username = "testusername",
    UserHomes = new HomeModel[]
    {
        new HomeModel {
         Home = "Home1",
         Cordinates = bla   
        }
    }
};

// string json = JsonSerializer.Serialize(myplayer);
// var yo = $"@{AppContext.BaseDirectory}homeconfig.json";
// File.AppendAllText($"{AppContext.BaseDirectory}/homeconfig.json", json);

var jsonData = System.IO.File.ReadAllText($"{AppContext.BaseDirectory}/homeconfig.json");

var employeeList = JsonSerializer.Deserialize<List<EmployeeDetail>>(jsonData) 
                      ?? new List<EmployeeDetail>();

// var myapp = new Run();

// Thread Th1 = new Thread(new ThreadStart(myapp.ServerOverlay));
// Thread Th2 = new Thread(new ThreadStart(myapp.ConsoleReader));
// Th1.Start();
// Th2.Start();