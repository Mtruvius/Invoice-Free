using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class CreateProduct : UserControl
    {
        bool isHovering_AddBtn;
        BitmapSource addBtnNormal;
        BitmapSource addBtnHover;
        public CreateProduct()
        {
            this.InitializeComponent();
            GetAssets();
        }

        private async void GetAssets()
        {
            var assetFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var addFile = await assetFolder.GetFileAsync("Icons\\add.png");
            var addHoverFile = await assetFolder.GetFileAsync("Icons\\add_hover.png");

            addBtnNormal = new BitmapImage(new Uri(addFile.Path));
            addBtnHover = new BitmapImage(new Uri(addHoverFile.Path));

            AddIcon.Source = addBtnNormal;
        }

        private void AddBtn_PointerHover(object sender, PointerRoutedEventArgs e)
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

        private void AddBtn_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Add button was pressed");
            
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
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
            ComboBoxItem selectedItem = (ComboBoxItem)catagorySelection.SelectedItem;
            newProduct.Add("Catagory", selectedItem.Content.ToString());
            newProduct.Add("Name", productName.Text);
            newProduct.Add("Description", description.Text);
            newProduct.Add("Cost", costPrice.Text);
            newProduct.Add("Price", sellingPrice.Text);
            newProduct.Add("IsTaxable", isTaxable.IsChecked);


            productsDataArray.Add(newProduct);

            Debug.WriteLine("THE TEXT: "+ selectedItem.Content.ToString());

            File.WriteAllText(productsPATH, productsDataArray.ToString());

            MainPage.MAIN.MainContentFrame.NavigateToType(typeof(ViewProducts), null, App.AnimatePage("right"));

             MainPage.Popup_Content.Children.Clear();
             MainPage.Popup_Panel.Visibility = Visibility.Collapsed;

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Popup_Panel.Visibility = Visibility.Collapsed;
            MainPage.Popup_Content.Children.Clear();
        }
    }
}
