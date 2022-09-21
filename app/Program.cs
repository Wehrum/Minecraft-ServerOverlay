var myapp = new Run();
//myapp.startApp();
Thread Th1 = new Thread(new ThreadStart(myapp.startApp));
Thread Th2 = new Thread(new ThreadStart(myapp.Test));
Th1.Start();
Th2.Start();