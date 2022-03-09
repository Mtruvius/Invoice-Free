using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditCustomer : Page
    {
        public struct EditedValues
        {
            public string CustomerEmail;
            public string CustomerContact;
            public string CustomerAddress;
            public string CustomerContactPerson;
        }
        

        //public string CustomerName { get; private set; }
        public string CustomerEmail { get; private set; }
        public string CustomerContact { get; private set; }
        public string CustomerAddress { get; private set; }
        public string CustomerContactPerson { get; private set; }

        private Customer _activeCustomer;
        private List<string[]> CustomerChangesList;

        public EditCustomer()
        {
            this.InitializeComponent();
            EditingPanelTitle.Text = "EDITING " + CustomerViewPage.SelectedCustomer.Name.ToUpper();          
            CustomerEmail = CustomerViewPage.SelectedCustomer.Email;
            CustomerContact = CustomerViewPage.SelectedCustomer.Contact;
            CustomerAddress = CustomerViewPage.SelectedCustomer.Address;
            CustomerContactPerson = CustomerViewPage.SelectedCustomer.ContactPerson;
            _activeCustomer = CustomerViewPage.SelectedCustomer;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine("EditingCustomer_cancel: " + _activeCustomer);            
        }

        private async void EditingCustomer_complete(object sender, RoutedEventArgs e)
        {
            
            CustomerChangesList = new List<string[]>();
            foreach (var item in CustomerInput_stackPanel.Children.OfType<Grid>())
            {
                if (item.Children[1].GetValue(TagProperty) != null && item.Children[1].GetValue(TagProperty).ToString() == "CustomerInput")
                {
                    TextBox input = item.Children[1] as TextBox;
                    TextBox_ChangedCheck(input);
                }
            }
            if (CustomerChangesList.Count > 0)
            {
                string changes = "";
                foreach (var str in CustomerChangesList)
                {
                    changes += "- " + str[0] + " to " + str[1] + "\n";
                }

                MikesContentDialog dialog = new();
                dialog.DialogContentMaxWidth = 720;
                dialog.Title = "Customer Changes";
                dialog.TitleFontWeight = FontWeights.Bold;
                dialog.TitleHorizontalAlignment = HorizontalAlignment.Center;
                dialog.ContentHeaderText = "The following changes were made...";

                TextBlock content = new()
                {
                    Text = changes,
                    FontStyle = Windows.UI.Text.FontStyle.Italic
                };
                dialog.Content = content;

                dialog.FooterHeaderText = "Would you like to save these changes?";
                dialog.PrimaryButtonText = "Save";
                dialog.CloseButtonText = "Cancel";
                dialog.PrimaryButtonClick += Dialog_PrimaryClick;
                dialog.CloseButtonClick += Dialog_CloseClick;

                dialog.XamlRoot = this.Content.XamlRoot;
                await dialog.ShowAsync();
            }
            else
            {
                EditingCustomer_cancel(null, null);
            }
        }

        private void TextBox_ChangedCheck(TextBox input)
        {
            switch (input.Name)
            {
                case "EmailInput":
                    if (!string.IsNullOrEmpty(input.Text) && input.Text != _activeCustomer.Email)
                    {
                        string email = App.ValidateEmail(input, TextBlockFlyout, ErrorFlyout);
                        AddToChangeMadeList("Email: " + email, input.Text);
                    }
                    break;
                case "ContactPersonInput":
                    if (!string.IsNullOrEmpty(input.Text) && input.Text != _activeCustomer.ContactPerson)
                    {
                        AddToChangeMadeList("Contact Person: " + _activeCustomer.ContactPerson, input.Text);
                    }
                    break;
                case "CompanyContact_TextBox":
                    if (!string.IsNullOrEmpty(input.Text) && input.Text != _activeCustomer.Contact)
                    {
                        AddToChangeMadeList("Contact: " + _activeCustomer.Contact, input.Text);
                    }
                    break;
                case "CompanyAddress_TextBox":
                    if (!string.IsNullOrEmpty(input.Text) && input.Text != _activeCustomer.Address)
                    {
                        AddToChangeMadeList("Address: " + _activeCustomer.Address, input.Text);
                    }
                    break;
                
            }
        }
        

        private void Dialog_CloseClick(ContentDialog dialog, RoutedEventArgs args)
        {
            
            dialog.Hide();
        }

        private void Dialog_PrimaryClick(ContentDialog dialog, RoutedEventArgs args)
        {
            dialog.Hide();
            if (!string.IsNullOrEmpty(EmailInput.Text))
            {
                Debug.WriteLine("EmailInput" + EmailInput.Text);
                _activeCustomer.Email = EmailInput.Text;
                EmailInput.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(ContactPersonInput.Text))
            {
                Debug.WriteLine("ContactPersonInput" + ContactPersonInput.Text);
                _activeCustomer.ContactPerson = ContactPersonInput.Text;
                ContactPersonInput.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(NumberInput.Text))
            {
                Debug.WriteLine("Contact" + NumberInput.Text);
                _activeCustomer.Contact = NumberInput.Text;
                Number.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(AddressInput.Text))
            {
                Debug.WriteLine("Address" + AddressInput.Text);
                _activeCustomer.Address = AddressInput.Text;
                Address.Text = string.Empty;
            }

            SaveManager.SaveCustomerEdits();
            EditingCustomer_cancel(null, null);
        }

        private void EditingCustomer_cancel(object sender, RoutedEventArgs e)
        {
            MainPage.Popup_Panel.Visibility = Visibility.Collapsed;
            MainPage.Popup_Content.Children.Clear();

            ViewCustomers.CustomerContent_Frame.NavigateToType(typeof(CustomerViewPage), _activeCustomer, App.AnimatePage("right"));
           
        }

        

        private void AddToChangeMadeList(string a, string b)
        {
            string[] changeMade = new string[]
                {
                    a,b
                };
            CustomerChangesList.Add(changeMade);
        }
    }
}
