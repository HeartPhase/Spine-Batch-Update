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
        static FolderPicker fp;
        static FileOpenPicker op;

        public MainPage()
        {
            this.InitializeComponent();

            fp = new FolderPicker();
            fp.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            fp.FileTypeFilter.Add(".spine");

            op = new FileOpenPicker();
            op.FileTypeFilter.Add(".json");
            op.FileTypeFilter.Add(".exe");
            op.FileTypeFilter.Add(".com");
            WinRT.Interop.InitializeWithWindow.Initialize(fp, MainWindow.m_hwnd);
            WinRT.Interop.InitializeWithWindow.Initialize(op, MainWindow.m_hwnd);
        }

        #region UI Events

        private async void ChooseFolder_Click_Import(object sender, RoutedEventArgs e) {
            StorageFolder folder = await fp.PickSingleFolderAsync();
            if (folder != null) folderPath_Import.Text = folder.Path;
            //CommandLineUtility.LogUpdated -= LogUpdatedHandler;
        }

        private async void ChooseFolder_Click_Export(object sender, RoutedEventArgs e) {
            StorageFolder folder = await fp.PickSingleFolderAsync();
            if (folder != null) folderPath_Export.Text = folder.Path;
        }

        private async void ChooseFile_Click_JSON(object sender, RoutedEventArgs e) {
            //StorageFile file = await op.PickSingleFileAsync();
            StorageFile file = await op.PickSingleFileAsync();
            if (file != null) filePath_JSON.Text = file.Path;
        }

        private async void ChooseFile_Click_Executable(object sender, RoutedEventArgs e) {
            StorageFile file = await op.PickSingleFileAsync();
            if (file != null) filePath_Executable.Text = file.Path;
        }

        private void ProcessPath_Click_Import(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(folderPath_Import.Text)) {
                dataSource = new SpineTreeView(SpineTreeGen.GenerateTreeFromPath(folderPath_Import.Text));
            }
            RefreshTreeView();
        }

        private void ProcessAction_Click(object sender, RoutedEventArgs e) {
            logs.Text = "";
            List<string> spineFilePaths = new();
            foreach (TreeViewNode node in treeView.SelectedNodes)
            {
                SpineItemView item = (SpineItemView)node.Content;
                if (!item.IsFolder) spineFilePaths.Add(item.SpineTreeItem.ItemPath);
            }
            //using var watcher = new FileSystemWatcher(folderPath_Export.Text);
            //watcher.Filter = "*.log";
            //watcher.Changed += LogUpdatedHandler;
            //watcher.EnableRaisingEvents = true;
            //CommandLineUtility.LogUpdated += LogUpdatedHandler;
            SpineUpdateUtility.UpdateSpineFiles(spineFilePaths, folderPath_Import.Text, folderPath_Export.Text, filePath_JSON.Text, filePath_Executable.Text);
        }

        private void ExportLogs_Click(object sender, RoutedEventArgs e) {
            string logFile = folderPath_Export.Text + "\\temp.log";
            string logErrorFile = folderPath_Export.Text + "\\temp_error.log";
            if (File.Exists(logFile))
            {

                logs.Text = File.ReadAllText(logFile) + "\n Error Message \n" + File.ReadAllText(logErrorFile);
            }
        }

        #endregion

        #region Non-UI

        private void LogUpdatedHandler(object sender, FileSystemEventArgs e) {
            //LogUpdatedEventArgs _e = (LogUpdatedEventArgs)e;
            //var ignored = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //{
            //    logs.Text += (_e.newLog + "\n");
            //});
            logs.Text = File.ReadAllText(folderPath_Export.Text + "\\temp.log");
        }

        private void RefreshTreeView() {
            TreeViewNode rootNode = GenerateNode(dataSource.RootFolder);
            foreach (SpineItemView item in dataSource.RootFolder.Children)
            {
                TreeViewNode node = GenerateNode(item);
                if (item.IsFolder && item.Children.Count > 0) AddChildrenToTreeNode(item, node);
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

        private void AddChildrenToTreeNode(SpineItemView item, TreeViewNode node) {
            foreach (SpineItemView child in item.Children)
            {
                TreeViewNode childNode = GenerateNode(child);
                if (child.IsFolder && child.Children.Count > 0) AddChildrenToTreeNode(child, childNode);
                node.Children.Add(childNode);
            }
        }

        #endregion
    }

    public class TreeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderTemp { get; set; }

        public DataTemplate FileTemp { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            TreeViewNode node = (TreeViewNode)item;
            SpineItemView spineItemView = (SpineItemView)node.Content;

            return spineItemView.IsFolder ? FolderTemp : FileTemp;
        }
    }
}
