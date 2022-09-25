using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProcessDemo;

public class Program
{
    public static void Main(string[] args)
    {
        bool shouldRun = true;
        List<string> commandHistory = new();
        while (shouldRun)
        {
            Console.Write("PS> ");
            string? line = Console.ReadLine();
            if (line is null)
                continue;
            commandHistory.Add(line);
            if (line == "exit")
            {
                shouldRun = false;
            }

            if (line == "history")
            {
                foreach (string command in commandHistory)
                {
                    Console.WriteLine(command);
                }
                continue;
            }
            try
            {
                string[] argv = line.Split(' ');
                string fileName = argv[0];
                var runInBackground = (line[^1] == '&') ? true : false;
                string arguments = line.Remove(0, fileName.Length).Trim('&');
                using (var childProcess = new Process())
                {
                    childProcess.StartInfo.FileName = fileName;
                    childProcess.StartInfo.Arguments = arguments;
                    childProcess.StartInfo.UseShellExecute = false;
                    bool started = childProcess.Start();
                    if (!runInBackground)
                    {
                        childProcess.WaitForExit();
                    }
                }

                using (var p = new Process())
                {
                    p.StartInfo.FileName = "ps";
                    p.StartInfo.Arguments = "-al";
                    p.Start();
                    p.WaitForExit();
                }
            }
            catch
            {
                Console.WriteLine("Error occurred.");
            }
        }
    } 
}