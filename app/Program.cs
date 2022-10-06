using static Helper;

Console.Title = "Server-Overlay 0.1";

SystemMessage("Initializing..", ConsoleColor.Yellow);

if(!ConfigCheck("serveroverlay.json"))
{
    SystemMessage("Performing first time setup.", ConsoleColor.Yellow);
}
Console.ReadKey();
var myapp = new Run();

Thread Th1 = new Thread(new ThreadStart(myapp.ServerOverlay));
Thread Th2 = new Thread(new ThreadStart(myapp.ConsoleReader));

Th1.Start();
Th2.Start();