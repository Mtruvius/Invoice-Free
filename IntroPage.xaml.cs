using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IntroPage : Page
    {        

        
        private UISettings uiSettings;

        public IntroPage()
        {
            InitializeComponent();
            SetContent();
            Debug.WriteLine(App.PublisherFolder.Path);
            Window.Current.SizeChanged += App.MaintainMaimized;
            uiSettings = new UISettings();
            uiSettings.ColorValuesChanged += CheckTitleTxtColor;
        }

        private void CheckTitleTxtColor(UISettings sender, object args)
        {
            App.CheckTitleTxtColor(sender, args, TitleText, ClsBtn);
        }

        
        private async void SetContent()
        {
            StorageFolder companiesFolder = await App.PublisherFolder.GetFolderAsync("Companies");
            IReadOnlyList<StorageFolder> companies = (IReadOnlyList<StorageFolder>)await companiesFolder.GetItemsAsync();

            Debug.WriteLine(companies[0].Name);

            ListView _companiesList = InstatiationPanel;
            _companiesList.Margin = new Thickness(50, 0, 50, 0);

            foreach (var company in companies)
            {
                IAsyncOperation<StorageFile> ImgFile = company.GetFileAsync("logo.jpg");
                ListViewItem item = new ListViewItem();
                item.ApplyTemplate();
                //item.ContentTemplate.SetValue()
                
                item.Content = company.DisplayName;
                item.PointerReleased += new PointerEventHandler(SelectCompany);
                item.Background = new SolidColorBrush(Colors.DimGray);
                item.HorizontalContentAlignment = HorizontalAlignment.Center;
                _companiesList.Items.Add(item);
                _companiesList.Margin = new Thickness(0, 20, 0, 0);
            }           

            IntroTitle.Text = "Select a company to continue.";
            IntroTitle.TextAlignment = TextAlignment.Center;
            //InstatiationPanel.Children.Add(_companiesList);
            
            
        }


        private void SelectCompany(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Company was selected");
            
        }

        

        

    }

    
}
