using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Diagnostics;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerInvoicesView : Page
    {
        public CustomerInvoicesView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Customer customer = (Customer)e.Parameter;
            Debug.WriteLine(customer.CustomerName);
            if (customer.InvoiceCount ==0)
            {
                NoInvoiceText.Visibility = Visibility.Visible;
                InvoiceList.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoInvoiceText.Visibility = Visibility.Collapsed;
                InvoiceList.Visibility = Visibility.Visible;
            }

        }

        private void AddInvoice_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
