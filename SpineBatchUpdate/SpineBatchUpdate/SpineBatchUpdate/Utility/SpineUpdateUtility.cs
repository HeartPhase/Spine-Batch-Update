using System;
using System.Collections.Generic;
using System.Linq;
using ABI.Microsoft.UI.Xaml.Controls;

namespace SpineBatchUpdate.Utility
{
    public static class SpineUpdateUtility
    {
        private static List<string> commands = new();

        public static string GetLastOutputPath(string spineFile, string exec)
        {
            return "this\\is\\a\\test\\path";
        }

        public static void ClearUpdateConfig()
        {
            commands.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spineFile"></param>
        /// <param name="root"></param>
        /// <param name="export">输出路径，如果之前获取过该文件的outputPath就会使用，否则使用选取的output，再否则输出到.spine文件的同一目录下</param>
        /// <param name="config">如果留空则根据下一个参数确定输出Skel还是Json，否则使用这个文件中的配置</param>
        /// <param name="exportAsSkel">是否输出为二进制Skel格式，会被config覆盖</param>
        public static void AddUpdateConfig(string spineFile, string root, string export, bool isStandardOutput, string config, bool? exportAsSkel)
        {

            string relativePath = spineFile[root.Length..];
            string fileName = relativePath.Split("\\").Last();
            relativePath = relativePath[..relativePath.IndexOf(fileName, StringComparison.Ordinal)];
            string commandImportArgs = " -i \"" + spineFile + "\"";
            string commandOutputArgs = string.Empty;
            string commandExportArgs = string.Empty;
            if (!string.IsNullOrEmpty(export))
            {
                if (isStandardOutput)
                {
                    commandOutputArgs = " -o \"" + export + relativePath[..^1] + "\"";
                }
                else
                {
                    commandOutputArgs = " -o \"" + export + "\"";
                }
            }

            if (!string.IsNullOrEmpty(config))
            {
                commandExportArgs = " -e \"" + config + "\"";
            }
            else
            {
                commandExportArgs = exportAsSkel != null && (bool)exportAsSkel ? " -e binary" : " -e json";
            }

            commands.Add(commandImportArgs + commandOutputArgs + commandExportArgs);
        }

        public static void ExecuteUpdate(string exec, string import)
        {
            string commandHead = (string.IsNullOrEmpty(exec)) ? "Spine" : "\"" + exec + "\"";
            foreach (var command in commands)
            {
                commandHead += command;
            }
            CommandLineUtility.RunCommand(new List<string>() {commandHead, "exit"}, import);
        }

        public static void UpdateSpineFiles(List<string> spineFiles, string root, string export, string config, string exec)
        {
            string command = "\"" + exec + "\"";
            foreach (string spineFile in spineFiles)
            {
                string relativePath = spineFile[root.Length..];
                string fileName = relativePath.Split("\\").Last();
                relativePath = relativePath[..relativePath.IndexOf(fileName)];
                string currentFileArgs = " -i \""
                    + spineFile
                    + "\" -o \""
                    + export + relativePath[..(relativePath.Length - 1)]
                    + "\" -e \""
                    + config + "\"";
                command += currentFileArgs;

                //string command = "\"" + exec + "\" -i \""
                //    + spineFile
                //    + "\" -o \""
                //    + export + relativePath[..(relativePath.Length - 1)]
                //    + "\" -e \""
                //    + config + "\"";

                //string command = "xcopy \""
                //    + spineFile
                //    + "\" \""
                //    + export + relativePath + "\"";
                //commands.Add(command);
            }
            commands.Add(command);
            commands.Add("exit");
            CommandLineUtility.RunCommand(commands, export);
            // TODO: we only run spine exec once, not for every export task.
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
