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
           
            FadeOut();
        }

        

        private async void FadeOut()
        {
            if (await StartDelay(2500))
            {
                Debug.WriteLine("FadeOut WAS CALLED");
               GetFirstPage();
            }
        }
        private void GetFirstPage()
        {
            if (!Directory.Exists(App.PathToCompanies))
            {
                App.ChangePageTo("AddCompany", App.AnimatePage("start"));
            }
            else
            {
                App.ChangePageTo("Intro", App.AnimatePage("start"));
            }
        }
        public static float lerpFuntion(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private static async Task<bool> StartDelay(int delay)
        {
            float timeElapsed = 0;
            float lerpDuration = delay;

            while (timeElapsed < lerpDuration)
            {

                timeElapsed += DateTime.Now.Second;
                await Task.Delay(1);
            }
            await Task.Delay(1);
            return true;
        }
    }
}
