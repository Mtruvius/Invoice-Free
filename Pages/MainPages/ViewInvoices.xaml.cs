using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewInvoices : Page, INotifyPropertyChanged
    {
        private ObservableCollection<InvoiceClass> InvoicesList;
        private ObservableCollection<SearchOptions> InvoiceSearchOptions;
        private ObservableCollection<SearchOptions> InvoiceFilterOptions;

        public int InvoiceListCount { get; private set; }
        public List<InvoiceClass> FilteredInvoicesList { get; private set; }

        bool isHovering_AddBtn;
        BitmapSource addBtnNormal;
        BitmapSource addBtnHover;
        private bool IsSearching;
        private int OptionChangeCount = 0;
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewInvoices()
        {
            this.InitializeComponent();

            CreateInvoiceList();
            CreateSearchOptions();
            addBtnNormal = App.addBtnNormal;
            addBtnHover = App.addBtnHover;
            AddIcon.Source = addBtnNormal;
            FilteredInvoicesList = new List<InvoiceClass>();
        }

        private void CreateSearchOptions()
        {
            InvoiceSearchOptions = new ObservableCollection<SearchOptions>();            
            string[] SearchOptions = new string[]
            {
               "CustomerName", "Date","Number"
            }; 
            foreach (string option in SearchOptions)
            {
                SearchOptions theoption = new()
                {
                    option = option
                };
                InvoiceSearchOptions.Add(theoption);
            }
            invoiceSearchOption.SelectedItem = InvoiceSearchOptions[0];
            invoiceSearchOption.ItemsSource = InvoiceSearchOptions;
            invoiceSearchOption.UpdateLayout();

            InvoiceFilterOptions = new ObservableCollection<SearchOptions>();
            string[] FilterOptions = new string[]
            {
               "All", "Complete","Pending"
            };
            foreach (string filter in FilterOptions)
            {
                SearchOptions thefilter = new()
                {
                    option = filter
                };
                InvoiceFilterOptions.Add(thefilter);
            }
            invoiceFilterOption.SelectedItem = InvoiceFilterOptions[0];
            invoiceFilterOption.ItemsSource = InvoiceFilterOptions;
            invoiceFilterOption.UpdateLayout();
        }

        private void CreateInvoiceList()
        {
            InvoicesList = new ObservableCollection<InvoiceClass>();
            foreach (InvoiceClass invoice in App.ALL_INVOICES)
            {
                InvoicesList.Add(invoice);
            }
            

            if (InvoicesList.Count < 1)
            {
                NoInvoicesText.Visibility = Visibility.Visible;
                InvoicesPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                InvoicesPanel.ItemsSource = InvoicesList;
            }

            InvoicesPanel.UpdateLayout();
        }

        private void CreateInvoice_OnHover(object sender, PointerRoutedEventArgs e)
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

        private void CreateInvoice_OnClick(object sender, PointerRoutedEventArgs e)
        {
            MainPage.MAIN.NavigateToPage("Create Invoice", null);
        }

        private void TextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            HandleProductFilterRequest(sender.Text);
        }

        private void HandleProductFilterRequest(string filteredInvoices)
        {
            Debug.WriteLine("App.ALL_INVOICES: " + App.ALL_INVOICES.ToString());
            IsSearching = true;
            string selectItem;
            string filterSelection;
            if (invoiceSearchOption.SelectedItem == InvoiceSearchOptions[0])
            {
                selectItem = "CustomerName";
            }
            else if (invoiceSearchOption.SelectedItem == InvoiceSearchOptions[1])
            {
                selectItem = "Date";
            }
            else
            {
                selectItem = "Number";
            }

            if (invoiceFilterOption.SelectedItem == InvoiceFilterOptions[0])
            {
                filterSelection = "All";
            }
            else if (invoiceFilterOption.SelectedItem == InvoiceFilterOptions[1])
            {
                filterSelection = "Completed";
            }
            else
            {
                filterSelection = "Pending";
            }
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                ExecuteProductFiltering(filteredInvoices, selectItem, filterSelection);
            });
        }
        private void ExecuteProductFiltering(string filteredInvoices, string SelectedSearchOption, string SelectedFilterOption)
        {
            
            filteredInvoices = filteredInvoices?.Trim().ToLower();
            if (SelectedFilterOption == "All")
            {
                switch (SelectedSearchOption)
                {
                    case "CustomerName":
                        FilteredInvoicesList = App.ALL_INVOICES.
                            Where(x => string.IsNullOrEmpty(
                                filteredInvoices) || x.CustomerName.ToLower().Contains(filteredInvoices)
                                ).Take(10).ToList();
                        break;
                    case "Date":
                        FilteredInvoicesList = App.ALL_INVOICES.
                            Where(x => string.IsNullOrEmpty(
                                filteredInvoices) || x.Date.ToLower().Contains(filteredInvoices)
                                ).Take(10).ToList();
                        break;
                    case "Number":
                        FilteredInvoicesList = App.ALL_INVOICES.
                            Where(x => string.IsNullOrEmpty(
                                filteredInvoices) || x.Number.ToString().ToLower().Contains(filteredInvoices)
                                ).Take(10).ToList();
                        break;

                }
            }
            else if (SelectedFilterOption == "Completed")
            {
                switch (SelectedSearchOption)
                {
                    case "CustomerName":
                        FilteredInvoicesList = App.ALL_INVOICES.
                            Where(x => string.IsNullOrEmpty(
                                filteredInvoices) || (x.CustomerName.ToLower().Contains(filteredInvoices) && x.Completed)).Take(10).ToList();
                        break;
                    case "Date":
                        FilteredInvoicesList = App.ALL_INVOICES.
                            Where(x => string.IsNullOrEmpty(
                                filteredInvoices) || (x.Date.ToLower().Contains(filteredInvoices) && x.Completed)).Take(10).ToList();
                        break;
                    case "Number":
                        FilteredInvoicesList = App.ALL_INVOICES.
                            Where(x => string.IsNullOrEmpty(
                                filteredInvoices) || (x.Number.ToString().ToLower().Contains(filteredInvoices) && x.Completed)).Take(10).ToList();
                        break;

                }
            }
            else
            {
                switch (SelectedSearchOption)
                {
                    case "CustomerName":
                        FilteredInvoicesList = App.ALL_INVOICES.
                            Where(x => string.IsNullOrEmpty(
                                filteredInvoices) || (x.CustomerName.ToLower().Contains(filteredInvoices) && !x.Completed)).Take(10).ToList();
                        break;
                    case "Date":
                        FilteredInvoicesList = App.ALL_INVOICES.
                            Where(x => string.IsNullOrEmpty(
                                filteredInvoices) || (x.Date.ToLower().Contains(filteredInvoices) && !x.Completed)).Take(10).ToList();
                        break;
                    case "Number":
                        FilteredInvoicesList = App.ALL_INVOICES.
                            Where(x => string.IsNullOrEmpty(
                                filteredInvoices) || (x.Number.ToString().ToLower().Contains(filteredInvoices) && !x.Completed)).Take(10).ToList();
                        break;
                }
            }
            OnInvoiceListSearch(filteredInvoices);
        }

        private async void OnInvoiceListSearch([CallerMemberName] string propName = "")
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                InvoicesList.Clear();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

                foreach (InvoiceClass invoice in FilteredInvoicesList)
                {
                    InvoicesList.Add(invoice);
                }
                if (InvoicesList.Count < 1)
                {
                    NoInvoicesText.Visibility = Visibility.Visible;
                }
                else
                {
                    NoInvoicesText.Visibility = Visibility.Collapsed;
                }
                IsSearching = false;
            });

        }

        private void OptionChange(object sender, SelectionChangedEventArgs e)
        {
            if (OptionChangeCount > 1)
            {
                HandleProductFilterRequest(searchbox.Text);
            }
            else
            {
                OptionChangeCount++;
            }
            
        }
    }

   
}
