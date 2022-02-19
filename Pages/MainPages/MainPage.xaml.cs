using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Animation;
using SimpleJSON;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Grid Popup_Panel { get; set; }
        public static StackPanel Popup_Content { get; set; }
        private CreateProduct _newProduct;
        private ApplicationView appView;
        public Frame MainContentFrame { get { return ContentFrame; } }
        public static MainPage MAIN;
        
        public NavigationView MainPageNav { get { return MainPageNavigation; } }
        public string currentActivePage;


        public MainPage()
        {
            this.InitializeComponent();
            //Window.Current.SizeChanged += Window_SizeChanged;
            //ContentFrame.Navigate(typeof(AddCompany));
            MAIN = this;
            LoadFirstPage();
            
        }
       /* protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.companyActive = (Company)e.Parameter;

            LoadFirstPage();
        }*/

        private void LoadFirstPage()
        {
            foreach (NavigationViewItemBase item in MainPageNavigation.MenuItems)
            {
                if (item is NavigationViewItem && item.Content.ToString() == "Stats")
                {
                    MainPageNavigation.SelectedItem = item;
                    ContentFrame.Navigate(typeof(ViewStats));
                    currentActivePage = "Stats";
                }
            }
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            appView = ApplicationView.GetForCurrentView();
            appView.TryEnterFullScreenMode();
        }

        public void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            };            
            navOptions.IsNavigationStackEnabled = false;

            NavigationViewItemBase item = args.InvokedItemContainer;

            switch (item.Name)
            {
                case "StatsContent":                    
                    ContentFrame.NavigateToType(typeof(ViewStats), null, navOptions);
                    currentActivePage = "Stats";
                    break;

                case "ViewCustomersContent":
                    ContentFrame.NavigateToType(typeof(ViewCustomers), null, navOptions);
                    currentActivePage = "Customers";
                    break;

                case "ViewInvoicesContent":
                    ContentFrame.NavigateToType(typeof(ViewInvoices), null, navOptions);
                    currentActivePage = "Invoices";
                    break;

                case "AddCustomersContent":
                    ContentFrame.NavigateToType(typeof(CreateCustomer), null, navOptions);
                    currentActivePage = "Create Customer";
                    break;

                case "AddInvoicesContent":
                    ContentFrame.NavigateToType(typeof(CreateInvoice), null, navOptions);
                    currentActivePage = "Create Invoice";
                    break;
                
                case "ViewProductContent":
                    ContentFrame.NavigateToType(typeof(ViewProducts), null, navOptions);
                    currentActivePage = "Products";
                    break;
            }
            
        }
        public void NavigateToPage(string pageViewName, Customer customer)
        {
            Debug.WriteLine("The PageView_ItemInvoled was clicked");
            FrameNavigationOptions navOptions = new FrameNavigationOptions();

            navOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            };            
            navOptions.IsNavigationStackEnabled = false;

            switch (pageViewName)
            {
                case "Stats":
                    foreach (NavigationViewItemBase item in MainPageNavigation.MenuItems)
                    {
                        if (item is NavigationViewItem && item.Content.ToString() == "Stats")
                        {
                            MainPageNavigation.SelectedItem = item;
                            ContentFrame.NavigateToType(typeof(ViewStats), null, navOptions);
                            currentActivePage = "Stats";
                        }
                    }

                    break;
                case "Customers":
                    foreach (NavigationViewItemBase item in MainPageNavigation.MenuItems)
                    {
                        if (item is NavigationViewItem && item.Content.ToString() == "Customers")
                        {
                            MainPageNavigation.SelectedItem = item;
                            ContentFrame.NavigateToType(typeof(CustomerViewPage), null, navOptions);
                            currentActivePage = "Customers";
                        }
                    }
                    
                    break;

                case "Create Customer":
                    foreach (NavigationViewItemBase item in MainPageNavigation.MenuItems)
                    {
                        if (item is NavigationViewItem && item.Content.ToString() == "Create Customer")
                        {
                            MainPageNavigation.SelectedItem = item;
                            ContentFrame.NavigateToType(typeof(CreateCustomer), null, navOptions);
                            currentActivePage = "Create Customer";
                        }
                    }                    
                    break;

                case "Invoices":
                    foreach (NavigationViewItemBase item in MainPageNavigation.MenuItems)
                    {
                        if (item is NavigationViewItem && item.Content.ToString() == "Invoices")
                        {
                            MainPageNavigation.SelectedItem = item;
                            ContentFrame.NavigateToType(typeof(ViewInvoices), null, navOptions);
                            currentActivePage = "Invoices";
                        }
                    }
                    
                    break;

                case "Create Invoice":
                    foreach (NavigationViewItemBase item in MainPageNavigation.MenuItems)
                    {
                        if (item is NavigationViewItem && item.Content.ToString() == "Create Invoice")
                        {
                            MainPageNavigation.SelectedItem = item;
                            ContentFrame.NavigateToType(typeof(CreateInvoice), customer, navOptions);
                            currentActivePage = "Create Invoice";
                        }
                    }
                    break;
                case "Products":
                    foreach (NavigationViewItemBase item in MainPageNavigation.MenuItems)
                    {
                        if (item is NavigationViewItem && item.Content.ToString() == "Products")
                        {
                            MainPageNavigation.SelectedItem = item;
                            ContentFrame.NavigateToType(typeof(ViewProducts), customer, navOptions);
                            currentActivePage = "Products";
                        }
                    }
                    break;
                case "Create Product":
                    foreach (NavigationViewItemBase item in MainPageNavigation.MenuItems)
                    {
                        if (item is NavigationViewItem && item.Content.ToString() == "Create Product")
                        {
                            MainPageNavigation.SelectedItem = item;
                            ShowAddProducts(null, null);
                        }
                    }
                    break;
            }
        }

        private void ShowAddProducts(object sender, TappedRoutedEventArgs e)
        {
            Color redColor = Color.FromArgb(170, 0, 0, 0);
            PopUpPanel.Background = new SolidColorBrush(redColor);
            _newProduct = new CreateProduct();           
            PopUpContent.Children.Add(_newProduct);
            PopUpContent.Padding = new Thickness(10);
            
            PopUpPanel.Visibility = Visibility.Visible;
            Popup_Panel = PopUpPanel;
            Popup_Content = PopUpContent; 
        }

        
    }
}
