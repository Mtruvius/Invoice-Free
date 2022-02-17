using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class CreateInvoice : Page
    {
        bool isHovering_AddBtn;
        float InvoiceTotal;
        double ScrollerHeight;

        BitmapSource addBtnNormal;
        BitmapSource addBtnHover;

        Customer _selectedCustomer;
        Product _selectedProduct;
        
        
        

        private ObservableCollection<Customer> CustomersOptions;
        private ObservableCollection<InvoiceProduct> SelectedProducts;
        private ObservableCollection<Product> _Products;

        public CreateInvoice()
        {
            this.InitializeComponent();
            CreateSearchOptions();
            SolidColorBrush brush = Application.Current.Resources["SystemAccentColor"] as SolidColorBrush;
            addBtnNormal = App.addBtnNormal;
            addBtnHover = App.addBtnHover;
            AddIcon.Source = addBtnNormal;
            SelectedProducts = new ObservableCollection<InvoiceProduct>();
           
            ScrollerHeight = Window.Current.Content.ActualSize.Y / 2;
            ScrollerView.MaxHeight = ScrollerHeight;
            Debug.WriteLine(ScrollerHeight);
            ScrollerView.UpdateLayout();
        }

       // private ObservableCollection<Customer> _customers;
        

        

        private void CreateSearchOptions()
        {
            CustomersOptions = new ObservableCollection<Customer>();
            GetInvoiceNumber();
            CreateCustomerOptionList();

            //customerSearchOption.SelectedItem = CustomerSearchOptions[0];
            CustomersSelectBox.ItemsSource = CustomersOptions;
            CustomersSelectBox.UpdateLayout();
        }

        private void GetInvoiceNumber()
        {
            string invoiceNo = (App.companyActive.LastInvoiceNo + 1).ToString();
            InvoiceNo.Text = invoiceNo;
            addInvoicePanel.UpdateLayout();
        }

        private void CreateCustomerOptionList()
        {
            foreach (Customer customer in App.CUSTOMERS)
            {
                CustomersOptions.Add(customer);
            }
        }
        
        private void CustomersOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Customer has been selected");            
            _selectedCustomer = (Customer)e.AddedItems[0];
        }
        
        private async void ShowProducts_OnClick(object sender, RoutedEventArgs e)
        {
            _Products = App.PRODUCTS;
            ProductsDisplayList.ItemsSource = _Products;
            await InvoiceAddProduct_dialog.ShowAsync();
            
        }
        
        private void AddNewProduct_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewProduct_OnHover(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (isHovering_AddBtn)
            {
                isHovering_AddBtn = false;
                AddIcon.Source = addBtnNormal;
            }
            else
            {
                isHovering_AddBtn = true;
                AddIcon.Source = addBtnHover;
            }
        }

        private void AddProductToInvoice(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            int _qty = int.Parse(QuantitySelected.Text);
            int price = int.Parse(_selectedProduct.Price);
            int total = _qty * price;
            InvoiceProduct prod = new()
            {
                Name = _selectedProduct.Name,
                Description = _selectedProduct.Description,
                Quantity = _qty,
                TotalPrice = total                
            };
            SelectedProducts.Add(prod);

            InvoiceTotal += total;
            TotalAmount.Text = InvoiceTotal.ToString("C");
            addInvoicePanel.UpdateLayout();
        }

        private void ProductsDisplayList_ItemSelected(object sender, ItemClickEventArgs e)
        {
            _selectedProduct = (Product)e.ClickedItem;
            Debug.WriteLine("THIS WAS CALLED!!!");   
             
           
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DateTimeOffset? dateTime = InvoiceDate.Date;
            string _invoiceDate = string.Format("{0}/{1}/{2}", dateTime.Value.Day, dateTime.Value.Month, dateTime.Value.Year);
            List<InvoiceProduct> theProducts = new List<InvoiceProduct>();
            foreach (InvoiceProduct item in SelectedProducts)
            {
                theProducts.Add(item);
            }
            InvoiceClass _invoice = new()
            {
                CustomerName = _selectedCustomer.Name,
                Date = _invoiceDate,
                Number = InvoiceNo.Text,
                InvoicedProducts = theProducts,
                InvoiceTotal = float.Parse(TotalAmount.Text, NumberStyles.Currency),
                Completed = (bool)IsPaid.IsChecked
            };
            AddToCompany(_invoice);
            SaveManager.SaveInvoice(_selectedCustomer, _invoice);
            MainPage.MAIN.MainContentFrame.Navigate(typeof(ViewInvoices));
        }

        private void AddToCompany(InvoiceClass Invoice)
        {
            float invoiceTotal = Invoice.InvoiceTotal;
            Debug.WriteLine((DateTime.Now.Month - 1));
            for (int i = 0; i < App.companyActive.Revenue.Length; i++)
            {
                if (i == (DateTime.Now.Month - 1) && App.companyActive.CurrentYear == DateTime.Now.Year)
                {
                    App.companyActive.Revenue[i] += invoiceTotal;
                }
                else if (i == (DateTime.Now.Month - 1) && App.companyActive.CurrentYear != DateTime.Now.Year)
                {
                    App.companyActive.PriorRevenue = App.companyActive.PreviousRevenue;
                    App.companyActive.PreviousRevenue = App.companyActive.Revenue;
                    App.companyActive.Revenue = new float[12];
                    App.companyActive.Revenue[i] = invoiceTotal;
                    App.companyActive.CurrentYear = DateTime.Now.Year;
                }
                else
                {

                }
            }
            if (Invoice.Completed)
            {
                App.companyActive.CompleteInvoices++;
            }
            else
            {
                App.companyActive.PendingInvoices++;
            }
            App.companyActive.LastInvoiceNo++;
            SaveManager.SaveCompanyEdits();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }

    
}
