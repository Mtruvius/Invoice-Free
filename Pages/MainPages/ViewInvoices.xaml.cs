using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewInvoices : Page
    {
        private ObservableCollection<InvoiceClass> Invoices;
        private ObservableCollection<SearchOptions> InvoiceSearchOptions;

        public int InvoiceListCount { get; private set; }

        public ViewInvoices()
        {
            this.InitializeComponent();

            CreateInvoiceList();
            CreateSearchOptions();

        }

        private void CreateSearchOptions()
        {
            InvoiceSearchOptions = new ObservableCollection<SearchOptions>();
            string[] optionList = new string[]
            {
                "Name","Email","Address"
            };

            foreach (string item in optionList)
            {
                SearchOptions option = new()
                {
                    option = item
                };
                InvoiceSearchOptions.Add(option);
            }
            invoiceSearchOption.SelectedItem = InvoiceSearchOptions[0];
            invoiceSearchOption.ItemsSource = InvoiceSearchOptions;
            invoiceSearchOption.UpdateLayout();
        }

        private void CreateInvoiceList()
        {
            Invoices = App.ALL_INVOICES;
            if (Invoices.Count < 1)
            {
                NoInvoicesText.Visibility = Visibility.Visible;
                InvoicesPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                InvoicesPanel.ItemsSource = Invoices;
            }

            InvoicesPanel.UpdateLayout();
        }

        
    }

   
}
