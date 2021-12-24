using Microsoft.UI.Xaml;
using SpineBatchUpdate.Utility;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpineBatchUpdate
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static IntPtr m_hwnd;
        public MainWindow()
        {
            this.InitializeComponent();

            m_hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        }

        public void SetContent()
        {
            mainFrame.Navigate(typeof(MainPage));
        }
    }
}
