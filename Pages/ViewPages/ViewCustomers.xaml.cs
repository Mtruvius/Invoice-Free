
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewCustomers : Page, INotifyPropertyChanged
    {
        bool isHovering_AddBtn;
        BitmapSource addBtnNormal;
        BitmapSource addBtnHover;
        public static Frame CustomerViewMainFrame;
        private ObservableCollection<Customer> CustomersList;
        private ObservableCollection<SearchOptions> CustomerSearchOptions;
        private bool IsSearching;

        public event PropertyChangedEventHandler PropertyChanged;

        public int InvoiceListCount { get; private set; }
        public List<Customer> FilteredCustomerList { get; private set; }

        public ViewCustomers()
        {
            this.InitializeComponent();

            CreateCustomersList();
            CreateSearchOptions();
            addBtnNormal = App.addBtnNormal;
            addBtnHover = App.addBtnHover;
            AddIcon.Source = addBtnNormal;
        }

        private void CreateSearchOptions()
        {
            CustomerSearchOptions = new ObservableCollection<SearchOptions>();
            string[] optionList = new string[]
            {
                "Name","Email", "Contact Person"
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
            CustomersList = new ObservableCollection<Customer>();
            foreach (Customer customer in App.CUSTOMERS)
            {
                CustomersList.Add(customer);
            }
            if (CustomersList.Count < 1)
            {
                NoCustomerText.Visibility = Visibility.Visible;
                CustomersPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                CustomersPanel.ItemsSource = CustomersList;
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

           // CustomerContentFrame.NavigateToType(typeof(CustomerViewPage), (Customer)e.ClickedItem, navOptions);
        }
       
        private void CreateCustomer_OnHover(object sender, PointerRoutedEventArgs e)
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

        private void CreateCustomer_OnClick(object sender, PointerRoutedEventArgs e)
        {
            MainPage.MAIN.NavigateToPage("Create Customer", null);
        }

        private void TextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            HandleProductFilterRequest(sender.Text);
        }

        private void HandleProductFilterRequest(string filteredProducts)
        {
            IsSearching = true;
            string selectItem;
            if (customerSearchOption.SelectedItem == CustomerSearchOptions[0])
            {
                selectItem = "Name";
            }
            else if (customerSearchOption.SelectedItem == CustomerSearchOptions[1])
            {
                selectItem = "Email";
            }
            else
            {
                selectItem = "Contact Person";
            }

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                ExecuteProductFiltering(filteredProducts, selectItem);
            });
        }
        private void ExecuteProductFiltering(string filteredProducts, string SelectedSearchOption)
        {
            filteredProducts = filteredProducts?.Trim().ToLower();
            Debug.WriteLine("SelectedSearchOption: " + SelectedSearchOption);
            switch (SelectedSearchOption)
            {
                case "Name":
                    FilteredCustomerList = App.CUSTOMERS.
                        Where(x => string.IsNullOrEmpty(
                            filteredProducts) || x.Name.ToLower().Contains(filteredProducts)
                            ).Take(10).ToList();
                    break;
                case "Email":
                    FilteredCustomerList = App.CUSTOMERS.
                        Where(x => string.IsNullOrEmpty(
                            filteredProducts) || x.Email.ToLower().Contains(filteredProducts)
                            ).Take(10).ToList();
                    break;
                case "Contact Person":
                    FilteredCustomerList = App.CUSTOMERS.
                        Where(x => string.IsNullOrEmpty(
                            filteredProducts) || x.ContactPerson.ToString().ToLower().Contains(filteredProducts)
                            ).Take(10).ToList();
                    break;

            }



            OnProductListSearch(filteredProducts);
        }
        private async void OnProductListSearch([CallerMemberName] string propName = "")
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                CustomersList.Clear();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

                foreach (Customer customer in FilteredCustomerList)
                {
                    CustomersList.Add(customer);
                }
                if (CustomersList.Count < 1)
                {
                    NoCustomerText.Visibility = Visibility.Visible;
                }
                else
                {
                    NoCustomerText.Visibility = Visibility.Collapsed;
                }
                IsSearching = false;
            });

        }

    }

    class SearchOptions
    {
        public string option;
    }
}
