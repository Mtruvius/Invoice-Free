using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class CreateInvoice : Page
    {
        public CreateInvoice()
        {
            this.InitializeComponent();
            CreateSearchOptions();
        }

       // private ObservableCollection<Customer> _customers;
        private ObservableCollection<SearchOptions> InvoiceSearchOptions;

        

        private void CreateSearchOptions()
        {
            InvoiceSearchOptions = new ObservableCollection<SearchOptions>();
            GetInvoiceNumber();
            CreateCustomerOptionList();
            
            //customerSearchOption.SelectedItem = CustomerSearchOptions[0];
            invoiceSearchOption.ItemsSource = InvoiceSearchOptions;
            invoiceSearchOption.UpdateLayout();
        }

        private void GetInvoiceNumber()
        {
            string invoiceNo = (int.Parse(App.companyActive.LastInvoiceNo) + 1).ToString();
            TextBox_invoiceNo.Text = invoiceNo;
            addInvoicePanel.UpdateLayout();
        }

        private void CreateCustomerOptionList()
        {
            string customersFile = File.ReadAllText(App.PathToCompanies + App.companyActive.CompanyName + "\\customers.json");
            JSONNode customerData = JSONNode.Parse(customersFile);

            foreach (JSONNode customer in customerData)
            {
                Debug.WriteLine(customer[0]);
                SearchOptions option = new()
                {
                    option = customer[0]
                };
                InvoiceSearchOptions.Add(option);
            }
        }

        private async void ShowProducts_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string productsFile = File.ReadAllText(App.PathToCompanies + App.companyActive.CompanyName + "\\products.json");
                ObservableCollection<ComboBoxItem> productsSearchOptions = new();

                JSONNode products = JSONNode.Parse(productsFile);
               
                ComboBoxItem product = new()
                {
                    Content = "Products Go HERE!!!"
                };
                productsSearchOptions.Add(product);
                ComboBox Box = new ComboBox();
                Box.ItemsSource = productsSearchOptions;
                Box.Name = "ProductSelectionbox";
                
                Button AddNew = new()
                {
                    Content = "Add new product"
                };

                StackPanel addProductsPanel = new();
                addProductsPanel.Children.Add(Box);
                addProductsPanel.Children.Add(AddNew);
                
                ContentDialog dialog = new()
                {
                    Content = addProductsPanel,
                    PrimaryButtonText = "Add",
                    CloseButtonText = "Cancel",
                };
                dialog.PrimaryButtonClick += new TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs>((sender, e) => AddProductToInvoice(sender, e, Box));

                await dialog.ShowAsync();
            }
            catch (Exception error)
            {
                Debug.WriteLine("ERROR: " + error);
                File.WriteAllText(App.PathToCompanies + App.companyActive.CompanyName + "\\products.json", "[]");

                TextBlock str = new()
                {
                    Text = "No Product, please add a product"
                };
                Button AddNew = new()
                {
                    Content = "Add new product"
                };
                StackPanel panel = new();
                panel.Children.Add(str);
                panel.Children.Add(AddNew);
                ContentDialog dialog = new()
                {
                    Content = panel,
                    CloseButtonText = "Cancel"
                };

                await dialog.ShowAsync();
            }
            
        }

        private void AddProductToInvoice(ContentDialog sender, ContentDialogButtonClickEventArgs args, ComboBox box)
        {
            ComboBoxItem selectedProduct = (ComboBoxItem)box.SelectedItem;
            Debug.WriteLine(selectedProduct.Content);
            
        }

        
    }
}
