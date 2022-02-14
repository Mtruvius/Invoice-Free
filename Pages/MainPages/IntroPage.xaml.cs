using Microsoft.Toolkit.Uwp.UI.Helpers;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IntroPage : Page
    {
        
        private ObservableCollection<CompanyListViewItem> _companies;
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
             IReadOnlyList<StorageFolder> companies = await companiesFolder.GetFoldersAsync();
             _companies = new ObservableCollection<CompanyListViewItem>();
             foreach (var company in companies)
             {
                 StorageFile ImgFile = await company.GetFileAsync("logo.jpg");
                 BitmapSource img = new BitmapImage(new Uri(ImgFile.Path));

                 Image image = new Image();
                 image.Source = img;

                 CompanyListViewItem Obj = new CompanyListViewItem()
                 {
                     CompanyLogo = image.Source,
                     CompanyName = company.DisplayName
                 };

                 _companies.Add(Obj);
             }

             IntroTitle.Text = "Select a company to continue.";
             IntroTitle.TextAlignment = TextAlignment.Center;
             InstatiationPanel.ItemsSource = _companies;

             InstatiationPanel.UpdateLayout();          
        }


        private void SelectCompany_OnClick(object sender, ItemClickEventArgs e)
        {
            CompanyListViewItem clickedItem = (CompanyListViewItem)e.ClickedItem;
            string _PathToCompanyFolder = App.PathToCompanies + "\\" + clickedItem.CompanyName;            
            string CompanyData = File.ReadAllText(_PathToCompanyFolder + "\\" + clickedItem.CompanyName + ".json");

            JSONNode companyInfo = JSONNode.Parse(CompanyData);

            Company company = new Company()
            {
                CompanyName = clickedItem.CompanyName,
                CompanyLogo = clickedItem.CompanyLogo,
                Contact = companyInfo[0]["Contact"],
                Email = companyInfo[0]["Email"],
                Address = companyInfo[0]["Address"],
                RegNo = companyInfo[0]["RegNo"],
                VatOrTax = companyInfo[0]["VatOrTax"],
                ContactPerson = companyInfo[0]["ContactPerson"],
                LastInvoiceNo = companyInfo[0]["LastInvNo"]
            };
            App.companyActive = company;
            CreateCustomersList();
            CreateProductsList();
            this.Frame.Navigate(typeof(MainPage));           

            Debug.WriteLine(clickedItem.CompanyName);

        }

        private void CreateProductsList()
        {
            App.PathToProducts = App.PathToCompanies + App.companyActive.CompanyName + "\\products.json";
            string ProductsJsonFile;
            ProductsJsonFile = File.ReadAllText(App.PathToProducts);
            JSONNode products = JSONNode.Parse(ProductsJsonFile);
            foreach (JSONNode product in products)
            {
                Product pro = new()
                {
                    Catagory = product["Catagory"],
                    Name = product["Name"],
                    Description = product["Description"],
                    Cost = product["Cost"],
                    Price = product["Price"],
                    IsTaxable = product["IsTaxable"]
                    
                };
                App.PRODUCTS.Add(pro);
            }

            Debug.WriteLine("Products COUNT: " + App.PRODUCTS.Count);
        
        }

        private void CreateCustomersList()
        {
            App.PathToCustomers = App.PathToCompanies + App.companyActive.CompanyName + "\\customers.json";
            string CustomerJsonFile;
            CustomerJsonFile = File.ReadAllText(App.PathToCustomers);
            JSONNode customersData = JSONNode.Parse(CustomerJsonFile);
            foreach (JSONNode customer in customersData)
            {
                JSONNode customerInvoices = JSONNode.Parse(customer["Invoices"]);
                List<InvoiceClass> invoicesList = new List<InvoiceClass>();
                int InvoiceListCount = invoicesList.Count;
                foreach (JSONNode invoice in customerInvoices)
                {
                    InvoiceClass inv = new()
                    {
                        Number = invoice["Number"],
                    };
                    invoicesList.Add(inv);
                }

                Customer Obj = new()
                {
                    CustomerName = customer["Name"],
                    Email = customer["Email"],
                    Contact = customer["Contact"],
                    Address = customer["Address"],
                    VatOrTax = customer["VatOrTax"],
                    ContactPerson = customer["ContactPerson"],
                    CustomerInvoices = invoicesList,
                    InvoiceCount = InvoiceListCount
                };
                App.CUSTOMERS.Add(Obj);
            }

            Debug.WriteLine("CUSTOMER COUNT: " + App.CUSTOMERS.Count);
        }
    }


    internal class CompanyListViewItem
    {
        public ImageSource CompanyLogo;
        public string CompanyName;
    }

}
