using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpineBatchUpdate.Utility
{
    public static class SpineUpdateUtility
    {

        public static void UpdateSpineFiles(List<string> spineFiles, string root, string export, string config, string exec) {
            List<string> commands = new();
            foreach (string spineFile in spineFiles)
            {
                string relativePath = spineFile[root.Length..];
                string fileName = relativePath.Split("\\").Last();
                relativePath = relativePath[..relativePath.IndexOf(fileName)];
                string command = "\""+ exec +"\" -i \""
                    + spineFile
                    + "\" -o \""
                    + export + relativePath[..(relativePath.Length - 1)]
                    + "\" -e \""
                    + config + "\"";
                //string command = "xcopy \""
                //    + spineFile
                //    + "\" \""
                //    + export + relativePath + "\"";
                commands.Add(command);
            }
            commands.Add("exit");
            CommandLineUtility.RunCommand(commands, export);
        }
    }

    public class LogUpdatedEventArgs : EventArgs
    {
        public string newLog;

        public LogUpdatedEventArgs(string _newLog)
        {
            newLog = _newLog;
        }
    }
}
