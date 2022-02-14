
using SimpleJSON;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Windows.UI.Xaml.Documents;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewProducts : Page
    {
        private ObservableCollection<Product> _product;
        private ObservableCollection<ProductOptions> ProductSearchOptions;

        public int InvoiceListCount { get; private set; }
        
        public ViewProducts()
        {
            this.InitializeComponent();

            CreateproductsList();
            CreateSearchOptions();
        }

        private void CreateSearchOptions()
        {
            ProductSearchOptions = new ObservableCollection<ProductOptions>();
            string[] optionList = new string[]
            {
                "Name","Email","Address"
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
            _product = App.PRODUCTS;
            if (_product.Count < 1)
            {
                NoproductText.Visibility = Visibility.Visible;
                productsPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                productsPanel.ItemsSource = _product;
            }

            productsPanel.UpdateLayout();
        }

        public void Selectproduct_OnClick(object sender, ItemClickEventArgs e)
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
            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = selectedProduct.Cost });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = "\n" });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Bold, Text = "Product Price: " });
            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = selectedProduct.Price });

            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = "\n" });            
            
            info.Inlines.Add(new Run { FontWeight = FontWeights.Bold, Text = "Taxable: " });
            info.Inlines.Add(new Run { FontWeight = FontWeights.Normal, Text = selectedProduct.IsTaxable });

            ContentDialog dialog = new()
            {
                Content = info,
                CloseButtonText = "Close"
            };
            
            var popup = dialog.ShowAsync();
        }
    }

    class ProductOptions
    {
        public string option;
    }
}
 