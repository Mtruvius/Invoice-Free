using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Input;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerInvoicesView : Page
    {
        bool isHovering_AddBtn;
        BitmapSource addBtnNormal;
        BitmapSource addBtnHover;
        private ObservableCollection<InvoiceClass> PaidInvoices;

        public CustomerInvoicesView()
        {
            InitializeComponent();
            addBtnNormal = App.addBtnNormal;
            addBtnHover = App.addBtnHover;
            AddIcon.Source = addBtnNormal;
            PaidInvoices = new ObservableCollection<InvoiceClass>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ObservableCollection<InvoiceClass> completedInvoices = (ObservableCollection<InvoiceClass>)e.Parameter;
            if (completedInvoices.Count == 0)
            {
                NoInvoiceText.Visibility = Visibility.Visible;
                InvoiceList.Visibility = Visibility.Collapsed;
            }
            else
            {
                foreach (InvoiceClass invoice in completedInvoices)
                {
                    PaidInvoices.Add(invoice);
                }
                NoInvoiceText.Visibility = Visibility.Collapsed;
                InvoiceList.Visibility = Visibility.Visible;
            }
            InvoiceList.UpdateLayout();
        }

        private void CreateInvoice_OnClick(object sender, RoutedEventArgs e)
        {
            MainPage.Instance.NavigateToPage("Create Invoice", CustomerViewPage.SelectedCustomer);
        }

        private void AddInvoice_OnHover(object sender, PointerRoutedEventArgs e)
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

        private void InvoiceList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
