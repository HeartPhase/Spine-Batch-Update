using System.Collections.Generic;

namespace SpineBatchUpdate
{
    public interface ISpineTreeItem
    {
        bool IsChecked { get; set; }
        bool IsSelected { get; set; }
        string DisplayName { get; }
    }

    public class SpineItem : ISpineTreeItem
    {
        public List<SpineItem> Children { get; }
        public bool IsChecked { get; set; }
        public bool IsSelected { get; set; }
        public string DisplayName { get; }
        public bool IsFolder { get; set; }

        public SpineItem(string displayName)
        {
            DisplayName = displayName;
            Children = new();
        }

        public override string ToString()
        {
            string childrenStr = string.Empty;
            foreach (SpineItem item in Children)
            {
                childrenStr += item.ToString();
            }
            return DisplayName + "\n" + childrenStr;
        }
    }
}
