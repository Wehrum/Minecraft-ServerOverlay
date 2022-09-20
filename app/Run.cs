using System.Diagnostics;

public class Run
{
    public void startApp()
    {
        Console.WriteLine("Running app");
        bool restart = false;

        var process = new Process();
        process.StartInfo = new ProcessStartInfo("/home/wehrum/madpack/TheMadPack/LaunchServer.sh")
        {
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
        };

        process.OutputDataReceived += new DataReceivedEventHandler(async (sender, e) =>
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
                                    process.StandardInput.WriteLine($"tp {firstPlayer} {secondPlayer}");
                                    process.StandardInput.WriteLine($"say Telporting {firstPlayer} to {secondPlayer} if it's not working check spelling");
                                }
                                else
                                {
                                    process.StandardInput.WriteLine($"say To use: !tp (player) ex: !tp AConnor");
                                }

                                break;
                            case "!difficulty":
                                if (result.Length == 6)
                                {
                                    for (int i = 0; i < Constants.Gamemodes.Length; i++)
                                    {
                                        if (Constants.Gamemodes[i] == result[5])
                                        {
                                            process.StandardInput.WriteLine($"difficulty {result[5]}");
                                            process.StandardInput.WriteLine($"say Switching difficulty to {result[5]}");
                                            break;
                                        }
                                        else if (i == Constants.Gamemodes.Length - 1)
                                        {
                                            process.StandardInput.WriteLine($"say Switching difficulty to {result[5]}");
                                        }
                                    }
                                }
                                else
                                {
                                    process.StandardInput.WriteLine($"say To use: !difficulty (difficulty) ex: !difficulty hard");
                                }
                                break;
                            case "!restart":
                                restart = true;
                                process.StandardInput.WriteLine($"say This will RESTART the server, if you're sure type !confirm");
                                break;
                            case "!confirm":
                                if (restart)
                                {
                                    for (int i = 10; i > 0; i--)
                                    {
                                        process.StandardInput.WriteLine($"say Restarting the server in {i}!");
                                        Thread.Sleep(1000);
                                    }
                                    process.StandardInput.WriteLine($"stop");
                                }
                                else
                                {
                                    process.StandardInput.WriteLine($"say Please type !restart first to avoid accidental restarts");
                                }
                                break;
                            case "!":
                            case "!?":
                            case "!help":
                                process.StandardInput.WriteLine($"say Available commands:");
                                Thread.Sleep(1000);
                                process.StandardInput.WriteLine($"say - !difficulty");
                                Thread.Sleep(1000);
                                process.StandardInput.WriteLine($"say - !restart");
                                Thread.Sleep(1000);
                                process.StandardInput.WriteLine($"say - !tp");
                                break;
                        }
                    }
                    catch (Exception err)
                    {
                        process.StandardInput.WriteLine($"say Serious error occured, let Connor know || Stack: {err.Message}");
                    }

                }

            }

            Console.WriteLine(e.Data);
        });
        process.Start();
        process.BeginOutputReadLine();


        //Prevent closing
        Console.Read();
    }
}
