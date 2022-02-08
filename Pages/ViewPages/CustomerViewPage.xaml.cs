using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerViewPage : Page
    {
        public string CustomerName { get; private set; }
        public string CustomerEmail { get; private set; }
        public string CustomerContact { get; private set; }
        public string CustomerAddress { get; private set; }
        public string CustomerContactPerson { get; private set; }

        public CustomerViewPage()
        {
            this.InitializeComponent();
            CustomerNavigation.BackRequested += BackToCustomers;
        }

        private void BackToCustomers(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromLeft
            };
            navOptions.IsNavigationStackEnabled = false;

            ViewCustomers viewCust = new();
            CustomerViewFrame.NavigateToType(typeof(ViewCustomers), null, navOptions);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Customer _selectedCustomer = (Customer)e.Parameter;

            LoadCustomer(_selectedCustomer);
        }

        private void LoadCustomer(Customer selectedCustomer)
        {
            foreach (NavigationViewItemBase item in CustomerNavigation.MenuItems)
            {
                if (item is NavigationViewItem && item.Content.ToString() == "Complete")
                {
                    CustomerNavigation.SelectedItem = item;
                    InvoicesContentFrame.Navigate(typeof(CustomerInvoicesView));
                }
            }
            CustomerName = selectedCustomer.CustomerName;
            CustomerEmail = selectedCustomer.Email;
            CustomerContact = selectedCustomer.Contact;
            CustomerAddress = selectedCustomer.Address;
            CustomerContactPerson = selectedCustomer.ContactPerson;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            };
            navOptions.IsNavigationStackEnabled = false;

            NavigationViewItemBase item = args.InvokedItemContainer;

            switch (item.Name)
            {
                case "Complete":
                    InvoicesContentFrame.NavigateToType(typeof(ShowStats), null, navOptions);
                    break;

                case "Pending":
                    InvoicesContentFrame.NavigateToType(typeof(ViewCustomers), null, navOptions);
                    break;
                case "Edit":
                    EditCustomer();
                    break;
                case "Delete":
                    DeleteCustomer();
                    break;
                default:
                    InvoicesContentFrame.NavigateToType(typeof(ShowStats), null, navOptions);
                    break;
            }
        }

        private void EditCustomer()
        {
            Debug.WriteLine("The customer is in edit mode");
        }
        private void DeleteCustomer()
        {
            Debug.WriteLine("The customer is being deleted");
        }
    }
}
