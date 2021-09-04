using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SpineBatchUpdate.Utility
{
    public class CommandLineUtility
    {
        public Process initCMD()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            return cmd;
        }

        public async Task<string> RunCommand(string rawCommand) {
            string log = rawCommand;
            Process cmd = initCMD();
            await Task.Delay(200);
            return log;
        }
    }
}
