﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using SimpleJSON;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddCompany : Page
    {
        private string _CompanyName;        
        private Image _CompanyLogo;
        private StorageFile _chosenImage;
        private int _detailsTracker = 0;

        public AddCompany()
        {
            this.InitializeComponent();
            App.ImageSelected += ImageSelected;
            PanelTitleChange("name");
        }

        private void ImageSelected(BitmapImage image, StorageFile file)
        {  
            Company_logo.Source = image;
            _chosenImage = file;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.TryGoBack();
        }

        private void CompanyNameChosen(object sender, RoutedEventArgs e)
        {
            _CompanyName = CompanyName.Text;
            DetailsDisplayHandler(_detailsTracker);
        }

        private void CompanyLogoChosen(object sender, RoutedEventArgs e)
        {
            _CompanyLogo = new Image();
            _CompanyLogo.Source = Company_logo.Source;
            _CompanyLogo.Name = _CompanyName;
            DetailsDisplayHandler(_detailsTracker);
        }
        private async void CompanyDetailsChosen(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = await App.PublisherFolder.CreateFolderAsync("Companies");
            StorageFolder CompanyFolder = await folder.CreateFolderAsync(_CompanyName);
            Debug.WriteLine(folder.Path);

            JSONArray CompanyDetails = new JSONArray();

            JSONObject CompanyObj = new JSONObject();
            CompanyObj.Add("Name", _CompanyName);
            CompanyObj.Add("Contact", Contact.Text);
            CompanyObj.Add("Email", Email.Text);
            CompanyObj.Add("Address", companyAddress.Text);
            CompanyObj.Add("VatOrTax", VatTax.Text);
            CompanyObj.Add("RegNo", CompanyReg.Text);
            CompanyObj.Add("Person", ContactPerson.Text);

            CompanyDetails.Add(_CompanyName, CompanyObj);

            await _chosenImage.CopyAsync(await StorageFolder.GetFolderFromPathAsync(CompanyFolder.Path), "logo.jpg");

            StorageFile JsonFile = await CompanyFolder.CreateFileAsync(_CompanyName + ".json", CreationCollisionOption.ReplaceExisting);
            File.WriteAllText(JsonFile.Path, CompanyDetails.ToString());

            App.ChangePageTo("Main");
        }

        private void DetailsDisplayHandler(int v)
        {
            switch (v)
            {
                case 0:
                    COMP_name_panel.Visibility = Visibility.Collapsed;
                    COMP_Image_panel.Visibility = Visibility.Visible;
                    PanelTitleChange("logo");

                    _detailsTracker++;
                    break;
                case 1:
                    COMP_Image_panel.Visibility = Visibility.Collapsed;
                    COMP_Details_panel.Visibility = Visibility.Visible;
                    PanelTitleChange("details");
                    _detailsTracker++;
                    break;                
            }
            ContentHost.UpdateLayout();
        }
        private void PanelTitleChange(string name)
        {
            switch (name)
            {
                case "name":
                    panelTitle.Text = "COMPANY NAME";
                    break;
                case "logo":
                    panelTitle.Text = "COMPANY LOGO";
                    break;
                case "details":
                    panelTitle.Text = "COMPANY DETAILS";
                    break;
                default:
                    break;
            }
        }
        private void BackDisplayHandler(object sender, RoutedEventArgs e)
        {
            switch (_detailsTracker)
            {
                case 0:
                    COMP_name_panel.Visibility = Visibility.Visible;
                    
                    break;
                case 1:
                    COMP_name_panel.Visibility = Visibility.Visible;
                    COMP_Image_panel.Visibility = Visibility.Collapsed;
                    PanelTitleChange("name");
                    _detailsTracker--;
                    break;
                case 2:
                    COMP_Image_panel.Visibility = Visibility.Visible;
                    COMP_Details_panel.Visibility = Visibility.Collapsed;                    
                    _detailsTracker--;
                    PanelTitleChange("logo");
                    break;
            }
        }

        private void EnterPressed(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CompanyNameChosen(sender, e);
            }

        }        
    }
}