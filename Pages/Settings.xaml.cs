using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {

           
        }

        private void CloseSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CloseSettingsBtn_Click WAS CALLED");
            MainPage.Popup_Panel.Visibility = Visibility.Collapsed;
            MainPage.Popup_Content.Children.Clear();
            Debug.WriteLine(MainPage.MAIN.currentActivePage);
            App.ChangePageTo(MainPage.MAIN.currentActivePage, null);
        }

       
    }
}
