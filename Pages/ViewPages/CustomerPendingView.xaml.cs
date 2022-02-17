using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerPendingView : Page
    {
        bool isHovering_AddBtn;
        BitmapSource addBtnNormal;
        BitmapSource addBtnHover;

        private ObservableCollection<InvoiceClass> PendingInvoices;

        public CustomerPendingView()
        {
            this.InitializeComponent(); addBtnNormal = App.addBtnNormal;
            addBtnHover = App.addBtnHover;
            AddIcon.Source = addBtnNormal;
            PendingInvoices = new ObservableCollection<InvoiceClass>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ObservableCollection<InvoiceClass> PendingInv = (ObservableCollection<InvoiceClass>)e.Parameter;
            if (PendingInv.Count == 0)
            {
                NoInvoiceText.Visibility = Visibility.Visible;
                InvoiceList.Visibility = Visibility.Collapsed;
            }
            else
            {
                foreach (InvoiceClass invoice in PendingInv)
                {
                    PendingInvoices.Add(invoice);
                }
                NoInvoiceText.Visibility = Visibility.Collapsed;
                InvoiceList.Visibility = Visibility.Visible;
            }

        }

        private void AddInvoice_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddInvoice_OnHover(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
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

        private async void PendingInvoiceList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ContentDialogResult dialog = await PendingInvoiceClickPanel.ShowAsync();
            ObservableCollection<Customer> newCustomerList = new();
            if (dialog == ContentDialogResult.Primary)
            {
                Debug.WriteLine("PendingInvoiceList_ItemClick click");
                if (IsPendingPaid.IsChecked == true)
                {
                    Debug.WriteLine("PendingInvoiceList_ItemClick ISCHECKED");
                    InvoiceClass _theINv = (InvoiceClass)e.ClickedItem;
                    List<InvoiceClass> _customerInvoices = CustomerViewPage.SelectedCustomer.Invoices;

                    foreach (InvoiceClass invoice in _customerInvoices)
                    {
                        if (invoice == _theINv)
                        {
                            invoice.Completed = true;
                        }
                    }
                    foreach (Customer item in App.CUSTOMERS)
                    {
                        if (item.Name == CustomerViewPage.SelectedCustomer.Name)
                        {
                            newCustomerList.Add(CustomerViewPage.SelectedCustomer);
                        }
                        else
                        {
                            newCustomerList.Add(item);
                        }                        
                    }

                    App.CUSTOMERS = newCustomerList;
                    CustomerViewPage.SortInvoicesToLists(_customerInvoices);

                    MainPage.MAIN.MainContentFrame.NavigateToType(typeof(CustomerViewPage), CustomerViewPage.SelectedCustomer, App.AnimatePage("left"));
                    SaveManager.SaveCustomerEdits();

                    App.companyActive.PendingInvoices--;
                    App.companyActive.CompleteInvoices++;
                    SaveManager.SaveCompanyEdits();
                }
                
            }
        }
    }
}
