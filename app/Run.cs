using System.Diagnostics;
using static Helper;

public class Run
{
    public void startApp()
    {
        Console.WriteLine("Running app");
        bool restart = false;

        var console = new Process();
        console.StartInfo = new ProcessStartInfo("/home/wehrum/madpack/TheMadPack/LaunchServer.sh")
        {
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
        };

        console.OutputDataReceived += new DataReceivedEventHandler(async (sender, e) =>
        {
            if (e.Data != null)
            {
                string[] result = e.Data.ToLower().Split(' ');
                if (result.Length > 4)
                {
                    try
                    {
                        if (result[3] == "completed" && result[4] == "server-stop")
                        {
                            startApp();
                        }
                        switch (result[4])
                        {
                            case "!tp":
                                if (result.Length == 6)
                                {
                                    string firstPlayer = result[3].Replace(">", "").Replace("<", "");
                                    string secondPlayer = result[5];
                                    Command($"tp {firstPlayer} {secondPlayer}", console);
                                    Say($"Telporting {firstPlayer} to {secondPlayer}", console);
                                }
                                else
                                {
                                    
                                }

                                break;
                            case "!difficulty":
                                if (result.Length == 6)
                                {
                                    for (int i = 0; i < Constants.Gamemodes.Length; i++)
                                    {
                                        if (Constants.Gamemodes[i] == result[5])
                                        {
                                            Command($"difficulty {result[5]}", console);
                                            Say($"Switching difficulty to {result[5]}", console);
                                            break;
                                        }
                                        else if (i == Constants.Gamemodes.Length - 1)
                                        {
                                            Say($"Switching difficulty to {result[5]}", console);
                                        }
                                    }
                                }
                                else
                                {
                                    Say($"To use: !difficulty (difficulty) ex: !difficulty hard", console);
                                }
                                break;
                            case "!restart":
                                restart = true;
                                Say($"This will RESTART the server, if you're sure type !confirm", console);
                                break;
                            case "!confirm":
                                if (restart)
                                {
                                    for (int i = 10; i > 0; i--)
                                    {
                                        Say($"Restarting the server in {i}!", console);
                                        Thread.Sleep(1000);
                                    }
                                    console.StandardInput.WriteLine($"stop");
                                }
                                else
                                {
                                    Say("Please type !restart first to avoid accidental restarts", console);
                                }
                                break;
                            case "!":
                            case "!?":
                            case "!help":
                                Say($"Available commands:");
                                Thread.Sleep(1000);
                                Say($"- !difficulty");
                                Thread.Sleep(1000);
                                Say($"- !restart");
                                Thread.Sleep(1000);
                                Say($"- !tp");
                                break;
                        }
                    }
                    catch (Exception err)
                    {
                        Say($"Serious error occured, let Connor know || Stack: {err.Message}", console);
                    }

                }

            }

            Console.WriteLine(e.Data);
        });
        console.Start();
        console.BeginOutputReadLine();


        //Prevent closing
        Console.Read();
    }
}
