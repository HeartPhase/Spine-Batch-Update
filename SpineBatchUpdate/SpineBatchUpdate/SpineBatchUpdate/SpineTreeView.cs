using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpineBatchUpdate
{
    public class SpineTreeView
    {

        readonly ObservableCollection<SpineItemView> rootList;
        readonly SpineItemView rootFolder;
        readonly ICommand searchCommand;

        IEnumerator<SpineItemView> matchingItems;
        string searchText = string.Empty;

        public SpineTreeView(SpineItem _rootFolder)
        {
            rootFolder = new SpineItemView(_rootFolder);
            rootList = new ObservableCollection<SpineItemView>(
                    new SpineItemView[] { rootFolder }
                );
            searchCommand = new SearchSpineCommand(this);
        }

        public SpineItemView RootFolder { get => rootFolder; }

        public ObservableCollection<SpineItemView> RootList
        {
            get => rootList;
        }

        public ICommand SearchCommand => searchCommand;

        public string SearchText
        {
            get => searchText;
            set
            {
                if (value == searchText) return;
                searchText = value;
                matchingItems = null;
            }
        }

        void PerformSearch()
        {
            if (matchingItems == null || !matchingItems.MoveNext()) this.VerifyMatchingItems();

            var item = matchingItems.Current;
            if (item == null) return;
            if (item.Parent != null) item.Parent.IsExpanded = true;

            item.IsSelected = true;
        }

        void VerifyMatchingItems()
        {
            var matches = this.FindMatches(searchText, rootFolder);
            matchingItems = matches.GetEnumerator();

            if (!matchingItems.MoveNext())
            {
                //MessageBox.Show(
                //        "No Matching Spines",
                //        MessageBoxButton.OK
                //   );
            }
        }

        IEnumerable<SpineItemView> FindMatches(string searchText, SpineItemView item)
        {
            if (item.NameContainsText(searchText)) yield return item;

            foreach (SpineItemView child in item.Children)
            {
                foreach (SpineItemView match in this.FindMatches(searchText, child))
                {
                    yield return match;
                }
            }
        }

        class SearchSpineCommand : ICommand
        {
            readonly SpineTreeView spineTreeView;

            public SearchSpineCommand(SpineTreeView _spineTreeView)
            {
                spineTreeView = _spineTreeView;
            }

            public bool CanExecute(object args)
            {
                return true;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object args)
            {
                spineTreeView.PerformSearch();
            }
        }
    }
}
