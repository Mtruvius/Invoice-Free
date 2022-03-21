using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml;
using Windows.Storage;
using System.IO;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IntroPage : Page
    {
        
        private ObservableCollection<CompanyListViewItem> _companies;
        

        public IntroPage()
        {
            InitializeComponent();
            SetContent();

            //Window.Current.SizeChanged += App.MaintainMaimized;
            
           
        }

       

        
        private async void SetContent()
        {
            Debug.WriteLine(App.PathToCompanies);
             StorageFolder companiesFolder = await StorageFolder.GetFolderFromPathAsync(App.PathToCompanies);
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
                LastInvoiceNo = companyInfo[0]["LastInvoiceNo"],
                PriorRevenue = GetRevenueIntArray(companyInfo[0]["PriorRevenue"]),
                PreviousRevenue = GetRevenueIntArray(companyInfo[0]["PreviousRevenue"]),
                Revenue = GetRevenueIntArray(companyInfo[0]["Revenue"]),
                CurrentYear = companyInfo[0]["CurrentYear"],
                CompleteInvoices = companyInfo[0]["CompleteInvoices"],
                PendingInvoices = companyInfo[0]["PendingInvoices"],
                TotalQuotes = companyInfo[0]["TotalQuotes"],
                TotalCustomers = companyInfo[0]["TotalCustomers"],
                AddVat = companyInfo[0]["AddVat"],
                VatRate = companyInfo[0]["VatRate"],
            };
            JSONNode CatagoriesList = companyInfo[0]["ProductCatagoriesList"];
            App.companyActive = company;
            CreateCustomersList();
            CreateProductsList();            
            CreateProductCatagoryList(CatagoriesList);

            this.Frame.Navigate(typeof(MainPage));           


        }

        private void CreateProductCatagoryList(JSONNode CatagoriesList)
        {
            foreach (JSONNode catagory in CatagoriesList)
            {
                App.PRODUCTCATAGORIESLIST.Add(catagory.Value.ToString());
            }
           
        }

        private double[] GetRevenueIntArray(JSONNode revenueArray)
        {
            Debug.WriteLine(revenueArray[0]);
            double[] revenue = new double[12];
            for (int i = 0; i < revenueArray.Count; i++)
            {
                revenue[i] = revenueArray[i];
            }
            return revenue;
        }

        

        private void CreateProductsList()
        {
            string PATH = App.PathToCompanies + App.companyActive.CompanyName + "\\products.json";
            string ProductsJsonFile;
            if (!File.Exists(PATH))
            {
                File.Create(PATH);
                ProductsJsonFile = "";
            }
            else
            {
               ProductsJsonFile = File.ReadAllText(PATH);               
            }
            App.PathToProducts = PATH;
            
            JSONNode products = JSONNode.Parse(ProductsJsonFile);
            foreach (JSONNode product in products)
            {
                Product pro = new()
                {
                    Catagory = product["Catagory"],
                    Name = product["Name"],
                    Description = product["Description"],
                    Cost = product["Cost"],
                    Price = product["Price"]
                };
                App.PRODUCTS.Add(pro);
            }

            Debug.WriteLine("Products COUNT: " + App.PRODUCTS.Count);
        
        }

        private void CreateCustomersList()
        {
            string PATH = App.PathToCompanies + App.companyActive.CompanyName + "\\customers.json";
            string CustomerJsonFile;
            if (!File.Exists(PATH))
            {
                File.Create(PATH);
                CustomerJsonFile = "";
            }
            else
            {
                CustomerJsonFile = File.ReadAllText(PATH);               
            }
            App.PathToCustomers = PATH;
            JSONNode customersData = JSONNode.Parse(CustomerJsonFile);
            foreach (JSONNode customer in customersData)
            {

                JSONNode customerInvoices = customer["Invoices"];
                Debug.WriteLine("customerInvoices: " + customerInvoices.Count);
                List<InvoiceClass> invoicesList = new List<InvoiceClass>();                
                List<InvoiceProduct> invoicedProductList = new List<InvoiceProduct>();

                foreach (JSONNode invoice in customerInvoices)
                {
                    foreach (JSONNode product in invoice["InvoicedProducts"])
                    {
                        InvoiceProduct productToAdd = new()
                        {
                            Name = product["Name"],
                            Description = product["Description"],
                            Quantity = product["Quantity"],
                            TotalPrice = product["TotalPrice"]
                        };
                        invoicedProductList.Add(productToAdd);
                    }
                    InvoiceClass inv = new()
                    {
                        CustomerName = invoice["CustomerName"],
                        Number = invoice["Number"],
                        Date = invoice["Date"],
                        Completed = invoice["Completed"],
                        InvoiceTotal = invoice["InvoiceTotal"],
                        InvoicedProducts = invoicedProductList,
                    };
                    App.ALL_INVOICES.Add(inv);
                    invoicesList.Add(inv);
                }
                Debug.WriteLine("INTRO_CreateCustomersList: " +invoicesList.Count);
                Customer Obj = new()
                {
                    Name = customer["Name"],
                    Email = customer["Email"],
                    Contact = customer["Contact"],
                    Address = customer["Address"],
                    VatOrTax = customer["VatOrTax"],
                    ContactPerson = customer["ContactPerson"],
                    Invoices = invoicesList,
                    InvoiceCount = customer["InvoiceCount"]
                };
                App.CUSTOMERS.Add(Obj);
            }
            Debug.WriteLine("CUSTOMERS count: " + App.CUSTOMERS.Count);
        }

        private void AddNewCompany_Btn_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = MainWindow.m_Frame;

            frame.NavigateToType(typeof(CreateCompany),null,App.AnimatePage("bottom"));
        }
    }


    internal class CompanyListViewItem
    {
        public ImageSource CompanyLogo;
        public string CompanyName;
    }

}
