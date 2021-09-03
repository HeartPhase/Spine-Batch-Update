using Microsoft.UI.Xaml;
using SpineBatchUpdate.Utility;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Data;

namespace SpineBatchUpdate
{
    public sealed partial class MainWindow : Window
    {
        public static IntPtr m_hwnd;

        public MainWindow()
        {
            this.InitializeComponent();
            m_hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            mainFrame.Navigate(typeof(MainPage));
        }
    }
}
