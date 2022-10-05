using System.Text.Json;
using System.Text.Json.Serialization;

var myapp = new Run();

Thread Th1 = new Thread(new ThreadStart(myapp.ServerOverlay));
Thread Th2 = new Thread(new ThreadStart(myapp.ConsoleReader));

Th1.Start();
Th2.Start();