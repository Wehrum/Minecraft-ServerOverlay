using System.Text.Json;
using System.Text.Json.Serialization;

Helper.FileChecker(); //Runs a check to see if homeconfig.json exists, if not, create and populate it.

var myapp = new Run();

Thread Th1 = new Thread(new ThreadStart(myapp.ServerOverlay));
Thread Th2 = new Thread(new ThreadStart(myapp.ConsoleReader));
Th1.Start();
Th2.Start();