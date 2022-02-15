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
        BitmapSource addBtnNormal;
        BitmapSource addBtnHover;
        Product _selectedProduct;
        float InvoiceTotal;
        double ScrollerHeight;

        private ObservableCollection<SearchOptions> InvoiceSearchOptions;
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
            TotalAmount.Text = InvoiceTotal.ToString();
            addInvoicePanel.UpdateLayout();
        }

        private void ProductsDisplayList_ItemSelected(object sender, ItemClickEventArgs e)
        {
            _selectedProduct = (Product)e.ClickedItem;
            Debug.WriteLine("THIS WAS CALLED!!!");   
            Debug.WriteLine("_selectedProduct: " + _selectedProduct.Name);   
           
        }
    }

    public class InvoiceProduct
    {
        public string Name;
        public string Description;
        public int Quantity;
        public int TotalPrice;
    }
}
