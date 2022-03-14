using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SplashScreen : Page
    {
        public SplashScreen()
        {
            this.InitializeComponent();

            GetFirstPage();
        }

        

        private async void GetFirstPage()
        {
            if (await StartDelay(2500))
            {
                Debug.WriteLine("Delay Complete");
               FirstPageSelection();
            }
        }
        private void FirstPageSelection()
        {
            //MainWindow.m_Frame.Navigate( typeof(Settings));
            if (!Directory.Exists(App.PathToCompanies))
            {
                App.ChangePageTo("AddCompany",null, App.AnimatePage("start"));
            }
            else
            {
                App.ChangePageTo("Intro",null, App.AnimatePage("start"));
            }
        }

        private static async Task<bool> StartDelay(int delay)
        {
            await Task.Delay(delay);
            return true;
        }
    }
}
