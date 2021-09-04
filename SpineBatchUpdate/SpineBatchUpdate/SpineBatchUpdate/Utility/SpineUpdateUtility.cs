using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpineBatchUpdate.Utility
{
    public static class SpineUpdateUtility
    {
        static List<string> log = new();

        public static List<string> UpdateSpineFiles(List<string> spineFiles) {
            log.Clear();
            foreach (string spineFile in spineFiles)
            {
                log.Add(spineFile);
            }
            return log;
        }

    }
}
