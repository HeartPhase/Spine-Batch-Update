using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SpineBatchUpdate
{
    public class SpineItemView : INotifyPropertyChanged
    {
        readonly ObservableCollection<SpineItemView> children;
        readonly SpineItemView parent;
        readonly SpineItem spineTreeItem;

        bool isExpanded;
        bool isSelected;
        bool isFolder;

        public SpineItemView(SpineItem item) : this(item, null) { }

        public SpineItemView(SpineItem item, SpineItemView _parent)
        {
            parent = _parent;
            spineTreeItem = item;
            isFolder = spineTreeItem.IsFolder;

            children = new ObservableCollection<SpineItemView>(
                (from child in spineTreeItem.Children
                 select new SpineItemView(child, this)).ToList<SpineItemView>()
                );
        }

        public string IconPath {
            get => isFolder ? "Assets/folder.png" : "Assets/file.png";
        }

        public SpineItemView Parent
        {
            get => parent;
        }

        public ObservableCollection<SpineItemView> Children
        {
            get => children;
        }

        public SpineItem SpineTreeItem {
            get => spineTreeItem;
        }

        public string DisplayName { get => spineTreeItem.DisplayName; }

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                if (isExpanded && parent != null) parent.isExpanded = true;
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public bool IsFolder { get => isFolder; set => isFolder = value; }

        public bool NameContainsText(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(this.DisplayName)) return false;
            return this.DisplayName.IndexOf(text, StringComparison
                .InvariantCultureIgnoreCase) > -1;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
