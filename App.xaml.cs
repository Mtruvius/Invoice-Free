using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Diagnostics;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media.Animation;
using SimpleJSON;
using System.Collections.ObjectModel;

namespace Invoice_Free
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public  static StorageFolder PublisherFolder { get; private set; }
        public static string PathToCompanies { get; private set; }
        public static string PathToCustomers { get; set; }
        public static string PathToProducts { get; set; }

        public delegate void OnImageSetected(BitmapImage Image, StorageFile file);
        public static event OnImageSetected ImageSelected;
        public static Company companyActive;

        public static ObservableCollection<Customer> CUSTOMERS { get; set; }
        public static ObservableCollection<Product> PRODUCTS { get; set; }

        public static BitmapSource addBtnNormal;
        public static BitmapSource addBtnHover;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            PublisherFolder = ApplicationData.Current.GetPublisherCacheFolder("InvoiceFree");
            PathToCompanies = PublisherFolder.Path + "\\Companies\\";
            CUSTOMERS = new ObservableCollection<Customer>();
            PRODUCTS = new ObservableCollection<Product>();
            GetAssets();
        }


        private async void GetAssets()
        {
            var assetFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var addFile = await assetFolder.GetFileAsync("Icons\\add.png");
            var addHoverFile = await assetFolder.GetFileAsync("Icons\\add_hover.png");

            addBtnNormal = new BitmapImage(new Uri(addFile.Path));
            addBtnHover = new BitmapImage(new Uri(addHoverFile.Path));            
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += App_CloseRequested;
            
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter

                    //rootFrame.Navigate(typeof(CreateProduct));
                    GetFirstPage();
                        
                }
                // Ensure the current window is active


                Window.Current.Activate();
            }

        }

        private async void GetFirstPage()
        {
            Debug.WriteLine(PublisherFolder.Path);
            try
            {
                StorageFolder f = await PublisherFolder.GetFolderAsync("Companies");
                ChangePageTo("Intro");
            }
            catch (Exception)
            {
                ChangePageTo("AddCompany");
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
           
        }


        #region MyMethods

        public async void App_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
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
        }
        
        public static async void App_ExitRequested()
        {
            var dialog = new MessageDialog("Are you sure you want to exit?", "Exit");
            var confirmCommand = new UICommand("Yes");
            var cancelCommand = new UICommand("No");
            dialog.Commands.Add(confirmCommand);

            dialog.Commands.Add(cancelCommand);

            if (await dialog.ShowAsync() == cancelCommand)
            {

            }
            else
            {
                Current.Exit();
            }
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
           // ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
        }

        public static async void Minimize_Checked(object sender, RoutedEventArgs e)
        {
            IList<AppDiagnosticInfo> infos = await AppDiagnosticInfo.RequestInfoForAppAsync();
            IList<AppResourceGroupInfo> resourceInfos = infos[0].GetResourceGroups();
            await resourceInfos[0].StartSuspendAsync();
        }

        public static async void CheckTitleTxtColor(UISettings sender, object args, TextBlock TitleTxt, Button closeBtn)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
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

        public static void ChangePageTo(string page)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            switch (page)
            {
                case "Intro":
                    rootFrame.Navigate(typeof(IntroPage));
                    break;
                case "AddCompany":                    
                    rootFrame.Navigate(typeof(CreateCompany));
                    break;
                case "Main":
                    rootFrame.Navigate(typeof(MainPage));
                    break;
                case "AddCustomer":
                    rootFrame.Navigate(typeof(IntroPage));
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
        #endregion
    }
}
