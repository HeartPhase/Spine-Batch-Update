using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SpineBatchUpdate.Utility
{
    public class CommandLineUtility
    {

        public static EventHandler LogUpdated = delegate { };
        static Process cmd;
        static string logFile;
        static string logErrorFile;

        public static void initCMD()
        {
            cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
        }

        public static void RunCommand(List<string> rawCommands, string export)
        {
            initCMD();
            logFile = export + "\\" + "temp" + ".log";
            logErrorFile = export + "\\" + "temp_error" + ".log";
            //await Task.Delay(200);
            cmd.OutputDataReceived += OutputHandler;
            cmd.ErrorDataReceived += ErrorHandler;
            cmd.BeginOutputReadLine();
            cmd.BeginErrorReadLine();
            foreach (string command in rawCommands)
            {
                cmd.StandardInput.WriteLine(command);
                Debug.WriteLine(command);
            }
            cmd.WaitForExit();
        }

        public static void RunCommand(string command, string export)
        {
            initCMD();
            logFile = export + "\\temp.log";
            logErrorFile = export + "\\temp_error.log";
            cmd.OutputDataReceived += OutputHandler;
            cmd.ErrorDataReceived += ErrorHandler;
            cmd.BeginOutputReadLine();
            cmd.BeginErrorReadLine();
            cmd.StandardInput.WriteLine(command);
            Debug.WriteLine(command);
            cmd.WaitForExit();
        }

        static void OutputHandler(object sender, DataReceivedEventArgs e)
        {
            WriteData(logFile, e.Data);
            //LogUpdated.Invoke(null, new LogUpdatedEventArgs(e.Data)); 
            //logs += e.Data;
        }

        static void ErrorHandler(object sender, DataReceivedEventArgs e)
        {
            WriteData(logErrorFile, e.Data);
            //WriteData(e.Data);
        }

        static void WriteData(string target, string? data)
        {
            if (!File.Exists(target))
            {
                using (StreamWriter file = File.CreateText(target))
                {
                    file.WriteLine(data);
                }
            }
            else
            {
                using (StreamWriter file = new StreamWriter(target, append: true))
                {
                    file.WriteLine(data);
                }
            }
        }

    }
}
