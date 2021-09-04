using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpineBatchUpdate.Utility
{
    public static class SpineTreeGen
    {
        static List<string> log = new();

        public static SpineItem GenerateTreeFromPath(string path) {
            DirectoryInfo root = GetFolderByPath(path);
            return SearchFolder(root);
        }

        static SpineItem BuildSpineItem(DirectoryInfo dirInfo) {
            SpineItem item = new SpineItem(dirInfo.Name);
            item.IsFolder = true;
            return item;
        }

        static SpineItem BuildSpineItem(FileInfo fileInfo) {
            SpineItem item = new SpineItem(fileInfo.Name);
            item.IsFolder = false;
            item.ItemPath = fileInfo.FullName;
            return item;
        }

        static DirectoryInfo GetFolderByPath(string path) {
            DirectoryInfo di = null;
            try
            {
                di = new DirectoryInfo(path);
            }
            catch (Exception e)
            {
                log.Add(e.Message);
            }
            return di;
        }

        static SpineItem SearchFolder(DirectoryInfo root) {
            FileInfo[] files = null;
            DirectoryInfo[] directories = null;

            SpineItem item = BuildSpineItem(root);
            try
            {
                files = root.GetFiles("*.spine");
            }
            catch (Exception e)
            {
                log.Add(e.Message);
            }

            if (files != null) {
                foreach (FileInfo fileInfo in files)
                {
                    item.Children.Add(BuildSpineItem(fileInfo));
                }
            }

            directories = root.GetDirectories();

            if (directories != null) {
                foreach (DirectoryInfo dirInfo in directories)
                {
                    SpineItem spineFolder = SearchFolder(dirInfo);
                    if (spineFolder.Children.Count > 0)
                    item.Children.Add(spineFolder);
                }
            }
            return item;
        }
    }
}
