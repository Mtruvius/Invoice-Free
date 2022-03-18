using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using System.IO;
using sharpPDF;
using Microsoft.UI.Text;
using PInvoke;
using System.Runtime.InteropServices;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice_Free
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PDFexport : Page
    {
        /// <summary>
        /// The MAPISendMail function sends a message.
        ///
        /// This function differs from the MAPISendDocuments function in that it allows greater
        /// flexibility in message generation.
        /// </summary>


        public InvoiceClass SelectedInvoice { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string CustomerTax { get; set; }
        public int TotalProductsInvoices { get; set; }
        private float TaxTotal { get; set; }
        private string InvoiceFooterMessage { get; set; }

        public ObservableCollection<InvoiceProduct> _products;

        public PDFexport()
        {
            this.InitializeComponent();
            _products = new ObservableCollection<InvoiceProduct>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SelectedInvoice = (InvoiceClass)e.Parameter;
            CustomerName = SelectedInvoice.CustomerName;

            foreach (Customer customer in App.CUSTOMERS)
            {
                if (customer.Name == CustomerName)
                {
                    Email = customer.Email;
                    if (customer.Tax != null)
                    {
                        CustomerTax = customer.Tax;
                    }
                    InvoiceFooterMessage = App.companyActive.InvoiceFooterMsg;
                }
            }
            foreach (InvoiceProduct product in SelectedInvoice.InvoicedProducts)
            {
                _products.Add(product);
                TotalProductsInvoices += product.Quantity;
            }

            if (!App.companyActive.AddTax)
            {
                subtotalTitle.Visibility = Visibility.Collapsed;
                TaxTitle.Visibility = Visibility.Collapsed;
                subtotalValue.Visibility = Visibility.Collapsed;
                TaxValue.Visibility = Visibility.Collapsed;
            }
            else
            {
                TaxTitle.Text = "Tax "+ App.companyActive.TaxRate.ToString() + "%:";               

                float num1 = SelectedInvoice.InvoiceTotal;
                float num2 = SelectedInvoice.ExcludingTaxTotal;
                float TaxTotal = MathF.Abs(num1 - num2);

                Debug.WriteLine("num1: " + num1);
                Debug.WriteLine("num2: " + num2);
                Debug.WriteLine("TaxTotal: " + TaxTotal);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.ChangePageTo("Main", null, null);
            MainPage.Instance.NavigateToPage("Invoices", null);
        }
        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {

            await CreateTempImage();


            FileSavePicker savePicker = new FileSavePicker();

            App.InitializeWithWindow(savePicker);
            savePicker.SuggestedFileName = "INV #" + SelectedInvoice.Number;
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("PDF (*.pdf)", new List<string>(new string[] { ".pdf" }));
            savePicker.DefaultFileExtension = ".pdf";
            
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                pdfDocument doc = new pdfDocument("INV #" + SelectedInvoice.Number, App.companyActive.CompanyName);
                pdfPage pg = doc.addPage(sharpPDF.Enumerators.predefinedPageSize.csA4Page);


                System.Drawing.Image img = System.Drawing.Image.FromFile(Directory.GetCurrentDirectory() + "\\Tmp\\TempImage.png");
                
                doc.addImageReference(img, "TempImage");
                var imgref = doc.getImageReference("TempImage");
                pg.addImage(imgref, 0, 0, pg.height, pg.width);
                doc.createPDF(file.Path);

                MikesContentDialog dialog = new();
                dialog.Title = "Save Successfull";
                dialog.TitleHorizontalAlignment = HorizontalAlignment.Center;
                dialog.TitleFontWeight = FontWeights.Bold;
                
                StackPanel sp = new();
                StackPanel TextSp = new() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center};
                TextBlock str1 = new() {Text = "Invoice number " , HorizontalAlignment = HorizontalAlignment.Center };
                TextBlock str2 = new()
                {
                    Text = "#" + SelectedInvoice.Number,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                TextSp.Children.Add(str1);
                TextSp.Children.Add(str2);

                TextBlock str3 = new() {Text = "has been successfully saved at location: " ,HorizontalAlignment = HorizontalAlignment.Center };
                TextBlock str4 = new() {Text = file.Path, FontStyle = Windows.UI.Text.FontStyle.Italic , HorizontalAlignment = HorizontalAlignment.Center };
                
                sp.Children.Add(TextSp);
                sp.Children.Add(str3);
                sp.Children.Add(str4);
                
                dialog.Content = sp;
                dialog.DialogContentHorizontalAlignment = HorizontalAlignment.Center;
                dialog.DialogContentMaxWidth = 720;
                dialog.ButtonsAlignment = HorizontalAlignment.Right;
                //dialog.buttons
                dialog.PrimaryButtonText = "ok";
                dialog.PrimaryButtonClick += DialogBtnClick;
                dialog.ButtonsFontSize = 14;

                dialog.XamlRoot = App.m_window.Content.XamlRoot;
                await dialog.ShowAsync();
               
            }
            else
            {
                MikesContentDialog dialog = new();
                dialog.Title = "Operation Cancelled";
                dialog.TitleHorizontalAlignment = HorizontalAlignment.Center;
                dialog.TitleFontWeight = FontWeights.Bold;
                dialog.Content = "The Save operation has been cancelled";
                dialog.DialogContentHorizontalAlignment = HorizontalAlignment.Center;
                dialog.DialogContentMaxWidth = 720;
                dialog.ButtonsAlignment = HorizontalAlignment.Right;
                //dialog.buttons
                dialog.PrimaryButtonText = "ok";
                dialog.PrimaryButtonClick += DialogBtnClick;
                dialog.ButtonsFontSize = 14;

                dialog.XamlRoot = App.m_window.Content.XamlRoot;
                await dialog.ShowAsync();


            }

        }

        
       

        private  void DialogBtnClick(ContentDialog dialog, RoutedEventArgs args)
        {
            dialog.Hide();
            
        }

        private async Task CreateTempImage()
        {
           
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(thePage, 2480, 3508);

           
            IBuffer pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

            if (!File.Exists("Tmp/TempImage.png"))
            {
                File.Create("Tmp/TempImage.png");
            }
            StorageFile file = await StorageFile.GetFileFromPathAsync(Directory.GetCurrentDirectory() + "\\Tmp\\TempImage.png");
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)2480, (uint)3508, 300, 300, pixelBuffer.ToArray());

                    await encoder.FlushAsync();
                    stream.Dispose();
                    
                }
            }           
        }

        
    }
}
