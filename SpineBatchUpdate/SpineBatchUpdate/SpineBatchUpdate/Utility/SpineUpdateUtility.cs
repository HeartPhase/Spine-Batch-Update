using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpineBatchUpdate.Utility
{
    public static class SpineUpdateUtility
    {
        public static EventHandler LogUpdated = delegate { };

        public static async void UpdateSpineFiles(List<string> spineFiles, string root, string export, string config) {
            CommandLineUtility clu = new CommandLineUtility();
            foreach (string spineFile in spineFiles)
            {
                string relativePath = spineFile[root.Length..];
                string fileName = relativePath.Split("\\").Last();
                relativePath = relativePath[..relativePath.IndexOf(fileName)];
                string command = "Spine -i "
                    + spineFile
                    + " -o "
                    + export + relativePath
                    + " -e "
                    + config;
                string log = await clu.RunCommand(command);
                LogUpdated.Invoke(null, new LogUpdatedEventArgs(log));
            }
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
