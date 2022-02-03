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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ApplicationView appView;
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Window_SizeChanged;          
            
        }

        private void AddNewCompany(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            appView = ApplicationView.GetForCurrentView();
            appView.TryEnterFullScreenMode();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
