
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewProducts : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Product> ProductsList;
        private ObservableCollection<ProductOptions> ProductSearchOptions;

        public int InvoiceListCount { get; private set; }
        public List<Product> FilteredProductList { get; private set; }

        bool isHovering_AddBtn;
        BitmapSource addBtnNormal;
        BitmapSource addBtnHover;
        private bool IsSearching;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewProducts()
        {
            this.InitializeComponent();

            CreateproductsList();
            CreateSearchOptions();
            addBtnNormal = App.addBtnNormal;
            addBtnHover = App.addBtnHover;
            AddIcon.Source = addBtnNormal;
        }

        private void CreateSearchOptions()
        {
            ProductSearchOptions = new ObservableCollection<ProductOptions>();
            string[] optionList = new string[]
            {
                "Name","Catagory","Price"
            };

            foreach (string item in optionList)
            {
                ProductOptions option = new()
                {
                    option = item
                };
                ProductSearchOptions.Add(option);
            }
            productSearchOption.SelectedItem = ProductSearchOptions[0];
            productSearchOption.ItemsSource = ProductSearchOptions;
            productSearchOption.UpdateLayout();
        }

        private void CreateproductsList()
        {
            ProductsList = new ObservableCollection<Product>();
            foreach (Product product in App.PRODUCTS)
            {
                ProductsList.Add(product);
            }
            if (ProductsList.Count < 1)
            {
                NoproductText.Visibility = Visibility.Visible;
                productsPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                productsPanel.ItemsSource = ProductsList;
            }

            productsPanel.UpdateLayout();
        }

        public async void Selectproduct_OnClick(object sender, ItemClickEventArgs e)
        {
            Product selectedProduct = (Product)e.ClickedItem;
            TextBlock info = new();
            info.Inlines.Add(new Run{ FontWeight = FontWeights.Bold, Text = "Catagory: " });
            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = selectedProduct.Catagory });
            
            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = "\n" });
            
            info.Inlines.Add(new Run{ FontWeight = FontWeights.Bold, Text = "Product Name: " });
            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = selectedProduct.Name });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = "\n" });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Bold, Text = "Product Description: " });
            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = selectedProduct.Description });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = "\n" });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Bold, Text = "Product Cost: " });
            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = selectedProduct.Cost.ToString("C") });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = "\n" });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Bold, Text = "Product Price: " });
            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = selectedProduct.Price.ToString("C") });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = "\n" }); 

            ContentDialog dialog = new()
            {
                Content = info,
                CloseButtonText = "Close"
            };
            
            await dialog.ShowAsync();
        }

        private void CreateProduct_OnHover(object sender, PointerRoutedEventArgs e)
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

        private void CreateProduct_OnClick(object sender, PointerRoutedEventArgs e)
        {
            MainPage.MAIN.NavigateToPage("Create Product", null);
        }

        private void TextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            HandleProductFilterRequest(sender.Text);
        }

        private void HandleProductFilterRequest(string filteredProducts)
        {
            IsSearching = true;
            string selectItem;
            if (productSearchOption.SelectedItem == ProductSearchOptions[0])
            {
                selectItem = "Name";
            }
            else if (productSearchOption.SelectedItem == ProductSearchOptions[1])
            {
                selectItem = "Catagory";
            }
            else
            {
                selectItem = "Price";
            }
            
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                ExecuteProductFiltering(filteredProducts , selectItem);
            });
        }
        private void ExecuteProductFiltering(string filteredProducts, string SelectedSearchOption)
        {
            filteredProducts = filteredProducts?.Trim().ToLower();            
            Debug.WriteLine("SelectedSearchOption: " + SelectedSearchOption);
            switch (SelectedSearchOption)
            {
                case "Name":
                    FilteredProductList = App.PRODUCTS.
                        Where(x => string.IsNullOrEmpty(
                            filteredProducts) || x.Name.ToLower().Contains(filteredProducts)
                            ).Take(10).ToList();
                    break;
                case "Catagory":
                    FilteredProductList = App.PRODUCTS.
                        Where(x => string.IsNullOrEmpty(
                            filteredProducts) || x.Catagory.ToLower().Contains(filteredProducts)
                            ).Take(10).ToList();
                    break;
                case "Price":
                    FilteredProductList = App.PRODUCTS.
                        Where(x => string.IsNullOrEmpty(
                            filteredProducts) || x.Price.ToString().ToLower().Contains(filteredProducts)
                            ).Take(10).ToList();
                    break;
               
            }

            

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
                if (ProductsList.Count < 1)
                {
                    NoproductText.Visibility = Visibility.Visible;
                }
                else
                {
                    NoproductText.Visibility = Visibility.Collapsed;
                }
                IsSearching = false;
            });

        }

    }

    class ProductOptions
    {
        public string option;
    }
}
 