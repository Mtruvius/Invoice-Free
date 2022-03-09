﻿using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Windows.UI.Popups;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerViewPage : Page, INotifyPropertyChanged
    {
        
        public static Customer SelectedCustomer;
        public string CustomerName { get; private set; }
        public string CustomerEmail { get; private set; }
        public string CustomerContact { get; private set; }
        public string CustomerAddress { get; private set; }
        public string CustomerContactPerson { get; private set; }

        public static ObservableCollection<InvoiceClass> CompletedInvoices;
        public static ObservableCollection<InvoiceClass> PendingInvoices;

        public CustomerViewPage()
        {
            this.InitializeComponent();
            CustomerNavigation.BackRequested += BackToCustomers;
            CompletedInvoices = new ObservableCollection<InvoiceClass>();
            PendingInvoices = new ObservableCollection<InvoiceClass>();

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
            SelectedCustomer = (Customer)e.Parameter;
            Debug.WriteLine("SelectedCustomer param: " +SelectedCustomer.Name);
            SortInvoicesToLists(SelectedCustomer.Invoices);
            LoadCustomer(SelectedCustomer);
        }

        private void LoadCustomer(Customer selectedCustomer)
        {
            foreach (NavigationViewItemBase item in CustomerNavigation.MenuItems)
            {
                if (item is NavigationViewItem && item.Content.ToString() == "Complete")
                {
                    CustomerNavigation.SelectedItem = item;
                    InvoicesContentFrame.NavigateToType(typeof(CustomerInvoicesView), CompletedInvoices, null);
                }
            }
            CustomerName = selectedCustomer.Name;
            CustomerEmail = selectedCustomer.Email;
            CustomerContact = selectedCustomer.Contact;
            CustomerAddress = selectedCustomer.Address;
            CustomerContactPerson = selectedCustomer.ContactPerson;

            Debug.WriteLine("PendingInvoices: " + PendingInvoices.Count);
            Debug.WriteLine("CompleteInvoices: " + CompletedInvoices.Count);
            OnPropertyChanged(string.Empty);
        }

        public static void SortInvoicesToLists(List<InvoiceClass> Invoices)
        {
            Debug.WriteLine("Invoices: " + Invoices.Count);
            foreach (InvoiceClass invoice in Invoices)
            {
                if (invoice.Completed == true)
                {
                    CompletedInvoices.Add(invoice);
                }
                else
                {
                    PendingInvoices.Add(invoice);
                }
            }           
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
                    InvoicesContentFrame.NavigateToType(typeof(CustomerInvoicesView), CompletedInvoices, navOptions);
                    break;

                case "Pending":
                    InvoicesContentFrame.NavigateToType(typeof(CustomerPendingView), PendingInvoices, navOptions);
                    break;
                case "Edit":
                    EditingCustomer_buttonClick();
                    break;
                case "Delete":
                    DeleteCustomer();
                    break;
                default:
                    InvoicesContentFrame.NavigateToType(typeof(ViewStats), null, navOptions);
                    break;
            }
        }

        private void EditingCustomer_buttonClick()
        {
            Debug.WriteLine("The customer " + SelectedCustomer.Name + " is in edit mode");
            MainPage.Instance.ShowModal(new EditCustomer(), null);
        }


        private async void DeleteCustomer()
        {
            MikesContentDialog dialog = new();
            dialog.DialogContentMaxWidth = 600;
            dialog.Title = "Delete Customer";
            dialog.TitleFontWeight = FontWeights.Bold;
            dialog.TitleHorizontalAlignment = HorizontalAlignment.Center;

            dialog.ContentHeaderText = "Are you sure you want to delete " + SelectedCustomer.Name + "?";
            dialog.ContentHeaderHorizontalAlignment = HorizontalAlignment.Center;

           StackPanel contentPanel = new() { Orientation = Orientation.Horizontal };

            TextBlock str1 = new(){Text = "All data for"};
            TextBlock str2 = new()
            {
                Text = SelectedCustomer.Name,
                FontWeight = FontWeights.Bold,
                FontStyle = Windows.UI.Text.FontStyle.Italic,
                Margin = new Thickness(5, 0, 5, 0)
            };
            TextBlock str3 = new(){Text = "will be deleted"};

            contentPanel.Children.Add(str1);
            contentPanel.Children.Add(str2);
            contentPanel.Children.Add(str3);
            dialog.Content = contentPanel;
            dialog.DialogContentHorizontalAlignment = HorizontalAlignment.Center;
            
            

            dialog.FooterHeaderText = "Do you wish to continue?";
            
            dialog.PrimaryButtonText = "Yes";
            dialog.PrimaryButtonClick += DeleteCustomerConfirmation_accept;
            dialog.CloseButtonText = "No";
            dialog.CloseButtonClick += DeleteCustomerConfirmation_decline;

            dialog.XamlRoot = this.Content.XamlRoot;
            await dialog.ShowAsync();
        }

        private void DeleteCustomerConfirmation_decline(ContentDialog dialog, RoutedEventArgs args)
        {
            dialog.Hide();
        }

        private void DeleteCustomerConfirmation_accept(ContentDialog dialog, RoutedEventArgs args)
        {
            string CustomerJsonFile = File.ReadAllText(App.PathToCompanies + App.companyActive.CompanyName + "\\customers.json");
            JSONNode customersData = JSONNode.Parse(CustomerJsonFile);

            foreach (JSONNode item in customersData)
            {
                if (item["Name"] == SelectedCustomer.Name)
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

            }
            dialog.Hide();
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
