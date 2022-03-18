using HoveyTech.SearchableComboBox;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core.Preview;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;

namespace Invoice_Free
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static Window m_window;

        public static string DataFolder { get; set; }
        public static string AssetsFolder { get; set; }
        public static string PathToCompanies { get; set; }
        public static string CompanyLogoPath { get; set; }
        public static string PathToCustomers { get; set; }
        public static string PathToProducts { get; set; }

        public delegate void OnImageSetected(BitmapImage Image, StorageFile file);
        public static event OnImageSetected ImageSelected;
        public static Company companyActive;

        public static ObservableCollection<Customer> CUSTOMERS { get; set; }
        public static ObservableCollection<Product> PRODUCTS { get; set; }
        public static ObservableCollection<InvoiceClass> ALL_INVOICES { get; set; }
        public static ObservableCollection<string> PRODUCTCATAGORIESLIST { get; set; }

        public static BitmapSource addBtnNormal;
        public static BitmapSource addBtnHover;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of Main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            AssetsFolder = Directory.GetCurrentDirectory() + "\\Assets\\";
            DataFolder = Directory.GetCurrentDirectory() + "\\Data\\";
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }
            Debug.WriteLine("PATH: " + Directory.GetCurrentDirectory());
            //this.Suspending += OnSuspending;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

           
            PathToCompanies = DataFolder + "Companies\\";

            CUSTOMERS = new ObservableCollection<Customer>();
            PRODUCTS = new ObservableCollection<Product>();
            ALL_INVOICES = new ObservableCollection<InvoiceClass>();
            PRODUCTCATAGORIESLIST = new ObservableCollection<string>();
            
            m_window = new MainWindow();
            m_window.Title = "Invoice Free";
            m_window.Activate();
            m_window.ExtendsContentIntoTitleBar = true;
           
            m_window.Maximize();

           
        }


        #region AddedMethods

       
        

      /*  public async void App_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            var deferral = e.GetDeferral();
            var dialog = new MessageDialog("Are you sure you want to exit?", "Exit");
            var confirmCommand = new UICommand("Yes");
            var cancelCommand = new UICommand("No");
            dialog.Commands.Add(confirmCommand);

            dialog.Commands.Add(cancelCommand);

            if (await dialog.ShowAsync() == cancelCommand)
            {
                //cancel close by handling the event
                e.Handled = true;
            }

            deferral.Complete();
        }*/

        public static async void App_ExitRequested()
        {
            MikesContentDialog dialog = new MikesContentDialog();
            dialog.DialogMinHeight = 100;
            dialog.Title = "Exit";
            dialog.TitleFontSize = 25;
            dialog.TitleFontWeight = FontWeights.Bold;

            StackPanel sp = new StackPanel();
            TextBlock tb = new()
            {
                Text = "Are you sure you want to exit?",
                MinHeight = 80
        };
            sp.Children.Add(tb);
            dialog.Content = sp;


            dialog.DialogContentMaxWidth = 600;            
            
            dialog.PrimaryButtonText = "Yes";
            dialog.ButtonsFontSize = 16;
            dialog.CloseButtonText = "No";

            dialog.PrimaryButtonClick += MikesContentDialogPrimary_click;
            dialog.CloseButtonClick += MikesContentDialogClose_click;
            dialog.XamlRoot = m_window.Content.XamlRoot;
            await dialog.ShowAsync();
        }

        private static void MikesContentDialogClose_click(ContentDialog dialog, RoutedEventArgs args)
        {
            dialog.Hide();
        }

        private static void MikesContentDialogPrimary_click(ContentDialog dialog, RoutedEventArgs args)
        {
            Current.Exit();
        }

        public static void InitializeWithWindow(object obj)
        {
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(obj, windowHandle);
        }

        public static bool TryGoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                return true;
            }
            return false;
        }

        public static void MaintainMaimized(object sender, WindowSizeChangedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
        }

        public static async void Minimize_Checked(object sender, RoutedEventArgs e)
        {
            IList<AppDiagnosticInfo> infos = await AppDiagnosticInfo.RequestInfoForAppAsync();
            IList<AppResourceGroupInfo> resourceInfos = infos[0].GetResourceGroups();
            await resourceInfos[0].StartSuspendAsync();
        }

        public static async void CheckTitleTxtColor(UISettings sender, object args, TextBlock TitleTxt, Button closeBtn)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                SolidColorBrush myBrush = new SolidColorBrush();
                var c = sender.GetColorValue(UIColorType.Accent);
                bool isDark = (5 * c.G + 2 * c.R + c.B) <= 8 * 128;
                //myBrush = new SolidColorBrush();
                if (!isDark)
                {
                    myBrush.Color = Colors.Black;

                }
                else
                {
                    myBrush.Color = Colors.White;
                }

                TitleTxt.Foreground = myBrush;
                closeBtn.Foreground = myBrush;

            }
            ).AsTask();


            //TestText.RequestedTheme = IsAccentColorDark() ? ElementTheme.Dark : ElementTheme.Light;
        }

        public static async void SelectImage()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();

                    await bitmapImage.SetSourceAsync(fileStream);

                    ImageSelected(bitmapImage, file);

                }
                // Application now has read/write access to the picked file
                Debug.WriteLine("Picked photo: " + file.Name);
                //OutputTextBlock.Text = "Picked photo: " + file.Name;
            }
            else
            {
                Debug.WriteLine("Operation cancelled.");
                //OutputTextBlock.Text = "Operation cancelled.";
            }
        }

        public static void ChangePageTo(string page,object parameter, FrameNavigationOptions NavOptions)
        {
            Frame rootFrame = MainWindow.m_Frame;
            switch (page)
            {
                case "Intro":
                    rootFrame.NavigateToType(typeof(IntroPage),null, NavOptions);
                    break;
                case "AddCompany":
                    rootFrame.NavigateToType(typeof(CreateCompany), null, NavOptions);
                    break;
                case "Main":
                    rootFrame.NavigateToType(typeof(MainPage), null, NavOptions);
                    break;
                case "AddCustomer":
                    rootFrame.NavigateToType(typeof(CreateCustomer), null, NavOptions);
                    break;
                case "SplashScreen":
                    rootFrame.NavigateToType(typeof(SplashScreen), null, NavOptions);
                    break;
                case "PdfExport":
                    rootFrame.NavigateToType(typeof(PDFexport), parameter, NavOptions);
                    break;
                default:
                    break;
            }
        }

        public static FrameNavigationOptions AnimatePage(string direction)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            if (direction == "left")
            {
                navOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo()
                {
                    Effect = SlideNavigationTransitionEffect.FromLeft
                };
            }
            else if (direction == "right")
            {
                navOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo()
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                };
            }
            else if (direction == "start")
            {
                navOptions.TransitionInfoOverride = new EntranceNavigationTransitionInfo();
            }
            else
            {
                navOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo()
                {
                    Effect = SlideNavigationTransitionEffect.FromBottom
                };
            }
            navOptions.IsNavigationStackEnabled = false;
            return navOptions;
        }

        public static string ValidateTextBox(TextBox input, MenuFlyout flyout, MenuFlyoutItem flyoutItem)
        {
            if (string.IsNullOrEmpty(input.Text))
            {
                input.BorderBrush = new SolidColorBrush(Colors.Red);
                flyoutItem.Text = "This field cannot be empty!";
                flyout.ShowAt(input);
                return null;
            }
            else
            {
                Brush borderColor = (Brush)Application.Current.Resources["TextBoxDisabledBorderThemeBrush"];
                input.BorderBrush = borderColor;
                return input.Text;
            }

        }

        public static float ValidateNumericBox(TextBox input, MenuFlyout flyout, MenuFlyoutItem flyoutItem)
        {
            if (string.IsNullOrEmpty(input.Text))
            {
                input.BorderBrush = new SolidColorBrush(Colors.Red);
                flyoutItem.Text = "This field cannot be empty!";
                flyout.ShowAt(input);
                return -1;
            }
            else
            {
                try
                {
                    float number = float.Parse(input.Text);
                    Brush borderColor = (Brush)Application.Current.Resources["TextBoxDisabledBorderThemeBrush"];
                    input.BorderBrush = borderColor;
                    return number;
                }
                catch (Exception)
                {
                    input.BorderBrush = new SolidColorBrush(Colors.Red);
                    flyoutItem.Text = "A Numeric value is required for this field!";
                    flyout.ShowAt(input);
                    return -1;
                }
            }


        }

        public static object ValidateSelectedItem(SearchableComboBox box, MenuFlyout flyout, MenuFlyoutItem flyoutItem, string errorMessage)
        {
            if (box.SelectedItem == null)
            {
                box.BorderBrush = new SolidColorBrush(Colors.Red);
                flyoutItem.Text = errorMessage;
                flyout.ShowAt(box);
                return null;
            }
            else
            {
                Brush borderColor = (Brush)Application.Current.Resources["TextBoxDisabledBorderThemeBrush"];
                box.BorderBrush = borderColor;
                return box.SelectedItem;
            }

        }
        public static object ValidateSelectedItem(ComboBox box, MenuFlyout flyout, MenuFlyoutItem flyoutItem, string errorMessage)
        {
            if (box.SelectedItem == null)
            {
                box.BorderBrush = new SolidColorBrush(Colors.Red);
                flyoutItem.Text = errorMessage;
                flyout.ShowAt(box);
                return null;
            }
            else
            {
                Brush borderColor = (Brush)Application.Current.Resources["TextBoxDisabledBorderThemeBrush"];
                box.BorderBrush = borderColor;
                return box.SelectedItem;
            }

        }

        public static string ValidateEmail(TextBox input, MenuFlyout flyout, MenuFlyoutItem flyoutItem)
        {
            bool containsTLD = false;

            if (string.IsNullOrEmpty(input.Text))
            {
                input.BorderBrush = new SolidColorBrush(Colors.Red);
                flyoutItem.Text = "This field cannot be empty!";
                flyout.ShowAt(input);
                return null;
            }
            else
            {
                try
                {
                    string[] theString = input.Text.Split('.');
                    string TLD = theString[1];

                    foreach (string item in TLDs.DoMains)
                    {
                        if (item.ToLower() == TLD.ToLower())
                        {
                            containsTLD = true;
                        }
                    }
                }
                catch (Exception)
                {

                }

                if (!input.Text.Contains('@') || !containsTLD)
                {
                    input.BorderBrush = new SolidColorBrush(Colors.Red);
                    flyoutItem.Text = "A valid email address is required!";
                    flyout.ShowAt(input);
                    return null;
                }
                else
                {
                    Brush borderColor = (Brush)Application.Current.Resources["TextBoxDisabledBorderThemeBrush"];
                    input.BorderBrush = borderColor;
                    return input.Text;
                }
            }

        }


        #endregion

    }
}
