
using SimpleJSON;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewCustomers : Page
    {
        public static Frame CustomerViewMainFrame;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<SearchOptions> CustomerSearchOptions;

        public int InvoiceListCount { get; private set; }
        
        public ViewCustomers()
        {
            this.InitializeComponent();

            CreateCustomersList();
            CreateSearchOptions();
           
        }

        private void CreateSearchOptions()
        {
            CustomerSearchOptions = new ObservableCollection<SearchOptions>();
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
                CustomerSearchOptions.Add(option);
            }
            customerSearchOption.SelectedItem = CustomerSearchOptions[0];
            customerSearchOption.ItemsSource = CustomerSearchOptions;
            customerSearchOption.UpdateLayout();
        }

        private void CreateCustomersList()
        {
            _customers = App.CUSTOMERS;
            if (_customers.Count < 1)
            {
                NoCustomerText.Visibility = Visibility.Visible;
                CustomersPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                CustomersPanel.ItemsSource = _customers;
            }

            CustomersPanel.UpdateLayout();
        }

        public void SelectCustomer_OnClick(object sender, ItemClickEventArgs e)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            };            
            navOptions.IsNavigationStackEnabled = false;

            CustomerContentFrame.NavigateToType(typeof(CustomerViewPage), (Customer)e.ClickedItem, navOptions);
        }
    }

    class SearchOptions
    {
        public string option;
    }
}
