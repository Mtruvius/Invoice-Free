using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
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
    public sealed partial class CreateInvoice : Page, INotifyPropertyChanged
    {
        public static CreateInvoice Instance;
        private bool IsSearching;
        private bool ErrorOccured;
        private float InvoiceTotal;
        private double ScrollerHeight;
        private BitmapSource addBtnNormal;
        private BitmapSource addBtnHover;

        private ObservableCollection<Customer> CustomersList;
        private ObservableCollection<InvoiceProduct> SelectedProducts;
        private ObservableCollection<Product> ProductsList;
        public List<Product> FilteredProductList { get; private set; }
        public List<Customer> FilteredCustomersList { get; private set; }

        public Product selectedProduct;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public CreateInvoice()
        {
            Instance = this;
            this.InitializeComponent();
            SolidColorBrush brush = Application.Current.Resources["SystemAccentColor"] as SolidColorBrush;
            addBtnNormal = App.addBtnNormal;
            addBtnHover = App.addBtnHover;
            //AddIcon.Source = addBtnNormal;
            SelectedProducts = new ObservableCollection<InvoiceProduct>();
            ProductsList = new ObservableCollection<Product>();
            CustomersList = new ObservableCollection<Customer>();           
            ScrollerHeight = Window.Current.Content.ActualSize.Y / 2;
            ScrollerView.MaxHeight = ScrollerHeight;
            Debug.WriteLine(ScrollerHeight);
            ScrollerView.UpdateLayout();
            InvoiceAddProduct_dialog.Closing += CloseInvoiceAddProduct_dialog;
            GetInvoiceNumber();
            CreateCustomerOptionList();
        }

        private void CloseInvoiceAddProduct_dialog(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (ErrorOccured)
            {
                args.Cancel = true;
            }
        }

        private string _filterProductList;
        public string FilterProducts
        {
            
            get => _filterProductList;
            set
            {
                _filterProductList = value;
                //RaisePropertyChanged(() => FilterText);
                Debug.WriteLine("VALUE: " + value);
                HandleProductFilterRequest(_filterProductList);
            }
        }
        private void HandleProductFilterRequest(string filteredProducts)
        {
            IsSearching = true;

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                ExecuteProductFiltering(filteredProducts);
            });
        }
        private void ExecuteProductFiltering(string filteredProducts)
        {
            filteredProducts = filteredProducts?.Trim().ToLower();

                FilteredProductList = App.PRODUCTS.Where(x => string.IsNullOrEmpty(filteredProducts)
                                                     || x.Name.ToLower().Contains(filteredProducts))
                    .Take(10)
                    .ToList();

            OnProductListSearch(filteredProducts);
        }
        private async void OnProductListSearch([CallerMemberName] string propName = "")
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                ProductsList.Clear();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

                foreach (Product product in FilteredProductList)
                {
                    ProductsList.Add(product);
                }
                IsSearching = false;
            });

        }

        private string _filterCustomersList;
        

        public string FilterCustomers
        {

            get => _filterCustomersList;
            set
            {
                _filterCustomersList = value;
                //RaisePropertyChanged(() => FilterText);
                Debug.WriteLine("VALUE: " + value);
                HandleCustomerFilterRequest(_filterCustomersList);
            }
        }
        private void HandleCustomerFilterRequest(string filteredCustomers)
        {
            IsSearching = true;

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                ExecuteCustomersFiltering(filteredCustomers);
            });
        }
        private void ExecuteCustomersFiltering(string filteredCustomers)
        {
            filteredCustomers = filteredCustomers?.Trim().ToLower();

            FilteredCustomersList = App.CUSTOMERS.Where(x => string.IsNullOrEmpty(filteredCustomers)
                                                 || x.Name.ToLower().Contains(filteredCustomers))
                .Take(10)
                .ToList();

            OnCustomerListSearch(filteredCustomers);
        }
        private async void OnCustomerListSearch([CallerMemberName] string propName = "")
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                CustomersList.Clear();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

                foreach (Customer customer in FilteredCustomersList)
                {
                    CustomersList.Add(customer);
                }
                IsSearching = false;
            });

        }

        private void GetInvoiceNumber()
        {
            string invoiceNo = (App.companyActive.LastInvoiceNo + 1).ToString();
            InvoiceNo.Text = invoiceNo;
            addInvoicePanel.UpdateLayout();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if (e.Parameter != null)
            {
                Customer theCustomer = (Customer)e.Parameter;
                CustomersSelectBox.SelectedItem = theCustomer;
            }
            
        }
        
        private void CreateCustomerOptionList()
        {
            foreach (Customer customer in App.CUSTOMERS)
            {
                CustomersList.Add(customer);
            }
        }
        
        public async void ShowProducts_OnClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("App.PRODUCTS.Count: " + App.PRODUCTS.Count);
            if (App.PRODUCTS.Count < 1)
            {
                MainPage.MAIN.NavigateToPage("Create Product", null);
            }
            else
            {
                if (ProductsList.Count < 1)
                {
                    foreach (Product product in App.PRODUCTS)
                    {
                        ProductsList.Add(product);
                    }
                }
                
                
                ProductsDisplayList.ItemsSource = ProductsList;
                if (e == null)
                {
                    ProductsDisplayList.SelectedItem = sender;
                }

               await InvoiceAddProduct_dialog.ShowAsync();
               
            }
        }
       
        private void AddProductToInvoice(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrEmpty(QuantitySelected.Text))
            {
                QuantitySelected.BorderBrush = new SolidColorBrush(Colors.Red);
                ErrorFlyout.Text = "Please add product quantity";
                TextBlockFlyout.ShowAt(QuantitySelected);
                ErrorOccured = true;
                return;
            }
            else
            {
                int _qty;
                try
                {
                    _qty = int.Parse(QuantitySelected.Text);
                    Brush borderColor = (Brush)Application.Current.Resources["TextBoxDisabledBorderThemeBrush"];
                    QuantitySelected.BorderBrush = borderColor;                    
                }
                catch (Exception)
                {
                    QuantitySelected.BorderBrush = new SolidColorBrush(Colors.Red);
                    ErrorFlyout.Text = "A numeric value is required!";
                    TextBlockFlyout.ShowAt(QuantitySelected);
                    ErrorOccured = true;
                    return;
                }

                selectedProduct = (Product)App.ValidateSelectedItem(ProductsDisplayList, TextBlockFlyout, ErrorFlyout, "Please select a product to continue.");
                if (selectedProduct == null)
                {
                    ErrorOccured = true;
                    return;                    
                }
                float price = selectedProduct.Price;
                float total = _qty * price;
                InvoiceProduct prod = new()
                {
                    Name = selectedProduct.Name,
                    Description = selectedProduct.Description,
                    Quantity = _qty,
                    TotalPrice = total
                };
                SelectedProducts.Add(prod);

                InvoiceTotal += total;
                TotalAmount.Text = InvoiceTotal.ToString("C");
                addInvoicePanel.UpdateLayout();
                ErrorOccured = false;
                
            }
            
        }        

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (!InvoiceDate.Date.HasValue)
            {
                InvoiceDate.BorderBrush = new SolidColorBrush(Colors.Red);
                ErrorFlyout.Text = "A Date must be selected!";
                TextBlockFlyout.ShowAt(InvoiceDate);
                return;
            }
            else
            {
                Brush borderColor = (Brush)Application.Current.Resources["TextBoxDisabledBorderThemeBrush"];
                InvoiceDate.BorderBrush = borderColor;               
            }
            DateTimeOffset? dateTime = InvoiceDate.Date;
            string _invoiceDate = string.Format("{0}/{1}/{2}", dateTime.Value.Day, dateTime.Value.Month, dateTime.Value.Year);

            if (SelectedProducts.Count < 1)
            {
                InvoiceDate.BorderBrush = new SolidColorBrush(Colors.Red);
                ErrorFlyout.Text = "At least one product must be added to the invoice!";
                TextBlockFlyout.ShowAt(addProduct_Btn);
                return;
            }
            else
            {
                Brush borderColor = (Brush)Application.Current.Resources["TextBoxDisabledBorderThemeBrush"];
                InvoiceProductsList.BorderBrush = borderColor;
            }
            List<InvoiceProduct> theProducts = new List<InvoiceProduct>();
            foreach (InvoiceProduct item in SelectedProducts)
            {
                theProducts.Add(item);
            }
            
            Customer _selectedCustomer = (Customer)App.ValidateSelectedItem(CustomersSelectBox, TextBlockFlyout, ErrorFlyout, "Please select a customer.");
            if (_selectedCustomer == null)
            {
                return;
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
            
            SaveManager.SaveInvoiceToCustomer(_selectedCustomer, _invoice);

            AddToCompany(_invoice);
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
            App.companyActive.LastInvoiceNo = int.Parse(InvoiceNo.Text);
            Debug.WriteLine("AddToCompany LastInvoiceNo" + App.companyActive.LastInvoiceNo);
            SaveManager.SaveCompanyEdits();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
           // Debug.WriteLine("FILTER TEXT: " + FilterText);
        }
    }
}
