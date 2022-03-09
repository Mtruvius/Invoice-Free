

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SimpleJSON;
using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage;

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateCompany : Page
    {
        private string _CompanyName;
        private Image _CompanyLogo;
        private StorageFile _chosenImage;
        private int _detailsTracker = 0;

        public CreateCompany()
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
            if (App.ValidateTextBox(CompanyName, TextBlockFlyout, ErrorFlyout) != null)
            {
                _CompanyName = CompanyName.Text;
                DetailsDisplayHandler(_detailsTracker);
            }            
        }
        private void SelectingImage()
        {
            App.SelectImage();
            ImageBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
            //x:Bind local:App.SelectImage
        }

        private void CompanyLogoChosen(object sender, RoutedEventArgs e)
        {
            BitmapImage img = (BitmapImage)Company_logo.Source;           
            Debug.WriteLine(img.UriSource);
             if (_chosenImage == null)
            {
                ImageBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                ErrorFlyout.Text = "A company logo must be selected!";
                TextBlockFlyout.ShowAt(Company_logo);
            }
            else
            {
                ImageBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
                _CompanyLogo = new Image();
                _CompanyLogo.Source = Company_logo.Source;
                _CompanyLogo.Name = _CompanyName;
                DetailsDisplayHandler(_detailsTracker);
            }
            
        }
        private async void CompanyDetailsChosen(object sender, RoutedEventArgs e)
        {
            DirectoryInfo folder;
            if (!Directory.Exists(App.PathToCompanies))
            {
                folder = Directory.CreateDirectory(App.PathToCompanies);
            }
            else
            {
                folder = new DirectoryInfo(App.PathToCompanies);
            }
            
            DirectoryInfo CompanyFolder = folder.CreateSubdirectory(_CompanyName);
           JSONArray productCatagoriesList = new();
            productCatagoriesList.Add("Default");
            
            JSONArray CompanyDetails = new JSONArray();

            JSONObject CompanyObj = new JSONObject();
            CompanyObj.Add("Name", _CompanyName);
            CompanyObj.Add("Contact", Contact.Text);
            CompanyObj.Add("Email", Email.Text);
            CompanyObj.Add("Address", companyAddress.Text);
            CompanyObj.Add("VatOrTax", VatTax.Text);
            CompanyObj.Add("RegNo", CompanyReg.Text);
            CompanyObj.Add("ContactPerson", ContactPerson.Text);
            CompanyObj.Add("LastInvNo", 0);
            CompanyObj.Add("PriorRevenue", GetNewRevenueArray());
            CompanyObj.Add("PreviousRevenue", GetNewRevenueArray());
            CompanyObj.Add("Revenue", GetNewRevenueArray());
            CompanyObj.Add("CurrentYear", DateTime.Now.Year);
            CompanyObj.Add("CompleteInvoices", 0);
            CompanyObj.Add("PendingInvoices", 0);
            CompanyObj.Add("TotalQuotes", 0);
            CompanyObj.Add("TotalCustomers", 0);
            CompanyObj.Add("ProductCatagoriesList", productCatagoriesList);
            CompanyObj.Add("AddVat", false);
            CompanyObj.Add("VatRate", 0); 

          
            
            CompanyDetails.Add("Details", CompanyObj);

            await _chosenImage.CopyAsync(await StorageFolder.GetFolderFromPathAsync(CompanyFolder.ToString()), "logo.jpg");
            App.CompanyLogoPath = CompanyFolder.ToString() +"logo.jpg";
            string JsonFilePath = CompanyFolder.ToString() +"\\"+ _CompanyName + ".json";
            File.WriteAllText(JsonFilePath, CompanyDetails.ToString());
            
            
            BitmapSource img = new BitmapImage(new Uri(App.CompanyLogoPath));

            Image image = new Image();
            image.Source = img;

            Company company = new Company()
            {
                CompanyName = _CompanyName,
                CompanyLogo = image.Source,
                Contact = Contact.Text,
                Email = Email.Text,
                Address = companyAddress.Text,
                RegNo = CompanyReg.Text,
                VatOrTax = VatTax.Text,
                ContactPerson = ContactPerson.Text,
                LastInvoiceNo = 0,
                PriorRevenue = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                PreviousRevenue = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                Revenue = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                CurrentYear = DateTime.Now.Year,
                CompleteInvoices = 0,
                PendingInvoices = 0,
                TotalQuotes = 0,
                TotalCustomers = 0,
                AddVat = false,
                VatRate = 0
            };

            App.companyActive = company;
            this.Frame.Navigate(typeof(MainPage), company);
        }

        private JSONNode GetNewRevenueArray()
        {
            JSONArray intArray = new JSONArray();
            for (int i = 0; i < 12; i++)
            {
                intArray.Add(0);
            }
            return intArray;
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
                    if (!Directory.Exists(App.PathToCompanies))
                    {
                        COMP_name_panel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        App.ChangePageTo("Intro", App.AnimatePage("start"));
                    }
                    

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

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            App.ValidateEmail(textBox, TextBlockFlyout, ErrorFlyout);
        }
    }
}
