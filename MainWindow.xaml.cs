using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
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
        

        public Grid MainTitleBar { get; private set; }
        public MainWindow()
        {

            App.m_window = this;
            //uiSettings = new UISettings();
           // uiSettings.ColorValuesChanged += CheckTitleTxtColor;
            this.InitializeComponent();            
            m_Frame = MainFrame;
            DoSetup();
        }
       
     
        private void CheckTitleTxtColor(UISettings sender, object args)
        {
            //App.CheckTitleTxtColor(sender, args, TitleText, ClsBtn);

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
