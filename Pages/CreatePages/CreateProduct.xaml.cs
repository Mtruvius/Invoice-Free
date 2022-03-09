using SimpleJSON;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateProduct : Page
    {
        private bool isHovering_AddBtn;
        private BitmapSource addBtnNormal;
        private BitmapSource addBtnHover;
        private ObservableCollection<string> ProductCatagories;
        private bool ErrorOccured;

        public CreateProduct()
        {
            this.InitializeComponent();
            addBtnNormal = App.addBtnNormal;
            addBtnHover = App.addBtnHover;
            AddIcon.Source = addBtnNormal;
            ProductCatagories = new ObservableCollection<string>();
            CreateCatagory_Dialog.Closing += CheckForErrorBeforClosing;

            GetProductCatagories();
        }

        private void GetProductCatagories()
        {
            foreach (string catagory in App.PRODUCTCATAGORIESLIST)
            {
                ProductCatagories.Add(catagory);
            }
            
        }

        private void CreateProductBtn_Click(object sender, RoutedEventArgs e)
        {
            
            string productsPATH = App.PathToCompanies + App.companyActive.CompanyName + "\\products.json";
            string productsData;

            if (!File.Exists(productsPATH))
            {
                productsData = "";
            }
            else
            {
                productsData = File.ReadAllText(productsPATH);
            }
              
            JSONArray productsDataArray = new JSONArray();
            JSONNode productList = JSONNode.Parse(productsData);

            foreach (JSONNode product in productList)
            {
                productsDataArray.Add(product);
            }
            
            JSONObject newProduct = new JSONObject();
            string selectedCatagory = (string)catagorySelection.SelectedItem;

            if (App.ValidateSelectedItem(catagorySelection, TextBlockFlyout, ErrorFlyout, "A catagory must be selected") == null)
            {
                return;
            }
            newProduct.Add("Catagory", selectedCatagory);
            
            if (App.ValidateTextBox(productName, TextBlockFlyout, ErrorFlyout) == null)
            {
                return;
            }
            newProduct.Add("Name", productName.Text);  
            
            newProduct.Add("Description", description.Text);


            float costPricing = App.ValidateNumericBox(costPriceTxt, TextBlockFlyout, ErrorFlyout);
            if (costPricing == -1)
            {
                return;
            }
            else
            {
                newProduct.Add("Cost", costPricing);
            }

            float sellPricing = App.ValidateNumericBox(sellingPrice, TextBlockFlyout, ErrorFlyout);
            if (sellPricing == -1)
            {
                return;
            }
            else
            {
                newProduct.Add("Price", sellPricing);
            }
           

            productsDataArray.Add(newProduct);
            File.WriteAllText(productsPATH, productsDataArray.ToString());

            Product theProduct = new()
            {
                Name = newProduct["Name"],
                Description = newProduct["Description"],
                Catagory = newProduct["Catagory"],
                Cost = newProduct["Cost"],
                Price = newProduct["Price"],
            };
            App.PRODUCTS.Add(theProduct);

            if (MainPage.Instance.currentActivePage == "Create Invoice")
            {
                MainPage.Popup_Content.Children.Clear();
                MainPage.Popup_Panel.Visibility = Visibility.Collapsed;                
                CreateInvoice.Instance.selectedProduct = theProduct;
                CreateInvoice.Instance.ShowProducts_OnClick(theProduct, null);
            }
            else
            {
                MainPage.Instance.MainContentFrame.NavigateToType(typeof(ViewProducts), null, App.AnimatePage("right"));
                MainPage.Popup_Content.Children.Clear();
                MainPage.Popup_Panel.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelProductBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Popup_Panel.Visibility = Visibility.Collapsed;
            MainPage.Popup_Content.Children.Clear();
            Debug.WriteLine(MainPage.Instance.currentActivePage);
            MainPage.Instance.NavigateToPage(MainPage.Instance.currentActivePage, null);
        }

        private void CreateCatagoryBtn_PointerHover(object sender, PointerRoutedEventArgs e)
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

            Debug.WriteLine(e.Pointer);
        }

        private async void CreateCatagoryBtn_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            await CreateCatagory_Dialog.ShowAsync();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrEmpty(CatagoryText.Text))
            {
                CatagoryText.BorderBrush = new SolidColorBrush(Colors.Red);
                ErrorFlyout.Text = "This value cannot be empty!";
                TextBlockFlyout.ShowAt(CatagoryText);
                ErrorOccured = true;
                
            }
            else
            {
                Brush borderColor = (Brush)Application.Current.Resources["TextBoxDisabledBorderThemeBrush"];
                CatagoryText.BorderBrush = borderColor;
                ErrorOccured = false;
                App.PRODUCTCATAGORIESLIST.Add(CatagoryText.Text);
                ProductCatagories.Add(CatagoryText.Text);
                catagorySelection.SelectedItem = CatagoryText.Text;
                SaveManager.SaveCompanyEdits();
            }

        }
        private void CheckForErrorBeforClosing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (ErrorOccured)
            {
                args.Cancel = true;
            }
        }
    }
}
