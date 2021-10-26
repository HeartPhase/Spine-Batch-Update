using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpineBatchUpdate.Utility
{
    public static class LogFormatter
    {
        public static List<string> SpineUpdateLogFormatter(string logFile) {
            List<string> formatted = new();
            string currentCmd = string.Empty;
            string currentError = string.Empty;

            Regex rxError = new Regex(@"^ERROR: *", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rxCmd = new Regex(@"^[a-z]:\\*>*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (File.Exists(logFile))
            {
                string[] logs = File.ReadAllText(logFile).Split(Environment.NewLine);
                foreach (string log in logs)
                {
                    if (rxCmd.IsMatch(log)) currentCmd = log;
                    if (rxError.IsMatch(log)) { 
                    
                    }
                }
            }

            return formatted;
        }
    }
}
