using SimpleJSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
    public sealed partial class CustomerViewPage : Page, INotifyPropertyChanged
    {
        private Customer _selectedCustomer;

        

        public string CustomerName { get; private set; }
        public string CustomerEmail { get; private set; }
        public string CustomerContact { get; private set; }
        public string CustomerAddress { get; private set; }
        public string CustomerContactPerson { get; private set; }
        public List<InvoiceClass> CustomerInvoices { get; private set; }

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
            _selectedCustomer = (Customer)e.Parameter;

            LoadCustomer(_selectedCustomer);
        }

        private void LoadCustomer(Customer selectedCustomer)
        {
            foreach (NavigationViewItemBase item in CustomerNavigation.MenuItems)
            {
                if (item is NavigationViewItem && item.Content.ToString() == "Complete")
                {
                    CustomerNavigation.SelectedItem = item;
                    InvoicesContentFrame.NavigateToType(typeof(CustomerInvoicesView), _selectedCustomer, null);
                }
            }
            CustomerName = selectedCustomer.CustomerName;
            CustomerEmail = selectedCustomer.Email;
            CustomerContact = selectedCustomer.Contact;
            CustomerAddress = selectedCustomer.Address;
            CustomerContactPerson = selectedCustomer.ContactPerson;
            CustomerInvoices = selectedCustomer.CustomerInvoices;
            OnPropertyChanged(string.Empty);
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
                    InvoicesContentFrame.NavigateToType(typeof(CustomerInvoicesView), _selectedCustomer, navOptions);
                    break;

                case "Pending":
                    InvoicesContentFrame.NavigateToType(typeof(CustomerPendingView), _selectedCustomer, navOptions);
                    break;
                case "Edit":
                    EditCustomer();
                    break;
                case "Delete":
                    DeleteCustomer();
                    break;
                default:
                    InvoicesContentFrame.NavigateToType(typeof(ViewStats), null, navOptions);
                    break;
            }
        }

        private async void EditCustomer()
        {
            Debug.WriteLine("The customer " + _selectedCustomer.CustomerName + " is in edit mode");

            await EditingPanel.ShowAsync();
        }

        private void CustomerEditComplete(ContentDialog dialog, ContentDialogButtonClickEventArgs args)
        {
            if (!string.IsNullOrEmpty(EmailInput.Text))
            {
                _selectedCustomer.Email = EmailInput.Text;
                EmailInput.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(ContactPersonInput.Text))
            {
                _selectedCustomer.ContactPerson = ContactPersonInput.Text;
                ContactPersonInput.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(Number.Text))
            {
                _selectedCustomer.Contact  = Number.Text;
                Number.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(Address.Text))
            {
                _selectedCustomer.Address = Address.Text;
                Address.Text = string.Empty;
            }
            LoadCustomer(_selectedCustomer);            
        }

        private async void DeleteCustomer()
        {
            string CustomerJsonFile = File.ReadAllText(App.PathToCompanies + App.companyActive.CompanyName + "\\customers.json");
            JSONNode customersData = JSONNode.Parse(CustomerJsonFile);

            foreach (JSONNode item in customersData)
            {                
                if (item["Name"] == _selectedCustomer.CustomerName)
                {
                    var dialog = new MessageDialog("Are you sure you want to delete customer?", "Delete " + _selectedCustomer.CustomerName + "?");
                    var confirmCommand = new UICommand("Yes");
                    var cancelCommand = new UICommand("No");
                    dialog.Commands.Add(confirmCommand);

                    dialog.Commands.Add(cancelCommand);

                    if (await dialog.ShowAsync() == cancelCommand)
                    {

                    }
                    else
                    {
                        customersData.Remove(item);
                        //App.AnimatePage("left");
                        FrameNavigationOptions navOptions = new FrameNavigationOptions();
                        navOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo()
                        {
                            Effect = SlideNavigationTransitionEffect.FromLeft
                        };
                        navOptions.IsNavigationStackEnabled = false;

                        File.WriteAllText(App.PathToCompanies + App.companyActive.CompanyName + "\\customers.json", customersData);
                        CustomerViewFrame.NavigateToType(typeof(ViewCustomers), null, navOptions);
                        CustomerViewFrame.UpdateLayout();
                    }
                    break;
                }
               
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.WriteLine("OnPropertyChanged");
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
