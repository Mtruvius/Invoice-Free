using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddProducts : Page, INotifyPropertyChanged
    {
        private DispatcherQueue dispatcherQueue;
        public List<Product> FilteredProductList { get; private set; }
        private ObservableCollection<Product> ProductsList;
        private string _filterProductList;
        private Product selectedProduct;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsSearching { get; private set; }
        public bool ErrorOccured { get; private set; }

        public delegate void OnSelectProductComplete(InvoiceProduct product);
        public static event OnSelectProductComplete ProductSelected;

        public AddProducts()
        {
            this.InitializeComponent();
            ProductsList = new ObservableCollection<Product>();
            dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            if (App.PRODUCTS.Count < 1)
            {
                MainPage.Instance.NavigateToPage("Create Product", null);
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

            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ProductsDisplayList.ItemsSource = ProductsList;
            if (e == null)
            {
                ProductsDisplayList.SelectedItem = e;
            }

        }
        

        public string FilterProducts
        {

            get => _filterProductList;
            set
            {
                _filterProductList = value;
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
        private void OnProductListSearch([CallerMemberName] string propName = "")
        {
            dispatcherQueue.TryEnqueue(() => {
                ProductsList.Clear();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

                foreach (Product product in FilteredProductList)
                {
                    ProductsList.Add(product);
                }
                IsSearching = false;
            });

        }

        private void AddingProduct_complete(object sender, RoutedEventArgs e)
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
                    TotalPrice = total,
                    Tax = GetExcludingTaxValue(total)
                };

                ProductSelected(prod);               
                ErrorOccured = false;
                AddingProduct_cancel(null, null);
            }
        }
        private void AddingProduct_cancel(object sender, RoutedEventArgs e)
        {
            MainPage.Popup_Panel.Visibility = Visibility.Collapsed;
            MainPage.Popup_Content.Children.Clear();
        }

        private float GetExcludingTaxValue(float total)
        {
            float percentageRate = total * ((float)App.companyActive.VatRate / 100);
            float excludingTaxValue = total - percentageRate;
            return excludingTaxValue;
        }
    }
}
