using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SpineBatchUpdate.Utility;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.ObjectModel;
namespace SpineBatchUpdate
{
    public sealed partial class MainPage : Page
    {
        static SpineTreeView dataSource;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            titleText.Text = "Clicked";
        }

        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            folderPath.Text = "";
            FolderPicker fp = new FolderPicker();
            fp.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            fp.FileTypeFilter.Add(".spine");

            WinRT.Interop.InitializeWithWindow.Initialize(fp, MainWindow.m_hwnd);
            StorageFolder folder = await fp.PickSingleFolderAsync();
            if (folder != null)
            {
                folderPath.Text = SpineTreeGen.GenerateTreeFromPath(folder.Path).ToString();
                dataSource = new SpineTreeView(SpineTreeGen.GenerateTreeFromPath(folder.Path));
                RefreshTreeView();
            }
        }

        private void RefreshTreeView() {
            TreeViewNode rootNode = GenerateNode(dataSource.RootFolder);
            foreach (SpineItemView item in dataSource.RootFolder.Children)
            {
                TreeViewNode node = GenerateNode(item);
                if (item.IsFolder && item.Children.Count > 0) AddChildrenToTreeNode(node);
                rootNode.Children.Add(node);
            }
            treeView.RootNodes.Clear();
            treeView.RootNodes.Add(rootNode);
        }

        private TreeViewNode GenerateNode(SpineItemView item) {
            TreeViewNode node = new TreeViewNode();
            node.Content = item;
            node.IsExpanded = true;
            return node;
        }

        private void AddChildrenToTreeNode(TreeViewNode node) {
            foreach (SpineItemView item in ((SpineItemView)node.Content).Children)
            {
                TreeViewNode childNode = GenerateNode(item);
                if (item.IsFolder && item.Children.Count > 0) AddChildrenToTreeNode(childNode);
                node.Children.Add(childNode);
            }
        }
    }
}
