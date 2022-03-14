using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using PInvoke;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice_Free
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static Frame m_Frame;
       
        public MainWindow()
        {

            App.m_window = this;
           
            this.InitializeComponent();
            LoadIcon("Assets/logo.ico");
            m_Frame = MainFrame;
            DoSetup();
        }
         private void LoadIcon(string iconName)
        {
            //Get the Window's HWND
            var hwnd = this.As<IWindowNative>().WindowHandle;
            IntPtr hIcon = PInvoke.User32.LoadImage(IntPtr.Zero, iconName,
                      PInvoke.User32.ImageType.IMAGE_ICON, 16, 16, PInvoke.User32.LoadImageFlags.LR_LOADFROMFILE);

            PInvoke.User32.SendMessage(hwnd, PInvoke.User32.WindowMessage.WM_SETICON, (IntPtr)0, hIcon);
        }
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        internal interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }

        private void DoSetup()
        {
            App.AssetsFolder = Directory.GetCurrentDirectory() + "\\Assets\\";
            App.DataFolder = Directory.GetCurrentDirectory() + "\\Data\\";
            if (!Directory.Exists(App.DataFolder))
            {
                Directory.CreateDirectory(App.DataFolder);
            }
            Debug.WriteLine("PATH: " + Directory.GetCurrentDirectory());
            //this.Suspending += OnSuspending;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            App.PathToCompanies = App.DataFolder + "Companies\\";

            App.CUSTOMERS = new ObservableCollection<Customer>();
            App.PRODUCTS = new ObservableCollection<Product>();
            App.ALL_INVOICES = new ObservableCollection<InvoiceClass>();
            App.PRODUCTCATAGORIESLIST = new ObservableCollection<string>();
            GetAssets();

           
        }

        private void GetAssets()
        {
            App.addBtnNormal = new BitmapImage(new Uri(App.AssetsFolder + "Icons\\add.png"));
            App.addBtnHover = new BitmapImage(new Uri(App.AssetsFolder + "Icons\\add_hover.png"));

            GetSplashScreen();
           
        }

        private void GetSplashScreen()
        {
            App.ChangePageTo("SplashScreen",null, null);
        }

        
    }
}
