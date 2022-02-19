﻿using SimpleJSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateCustomer : Page
    {
        public Customer CustomerToAdd;
        public string ErrorText;


        public CreateCustomer()
        {
            this.InitializeComponent();
            CustomerToAdd = new Customer();
            DataContext = this;
        }

        private void AddToCustomerClass(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            switch (textBox.Name)
            {
                case "customerName":
                    CustomerToAdd.Name = App.ValidateTextBox(textBox, TextBlockFlyout, ErrorFlyout);
                    break;
                case "Email":
                    CustomerToAdd.Email = App.ValidateEmail(textBox, TextBlockFlyout, ErrorFlyout);
                    break;
                case "Contact":
                    CustomerToAdd.Contact = textBox.Text;
                    break;
                case "companyAddress": 
                    CustomerToAdd.Address = textBox.Text;
                    break;
                case "VatTax":
                    CustomerToAdd.VatOrTax = textBox.Text;
                    break;
                case "ContactPerson":
                    CustomerToAdd.ContactPerson = textBox.Text;
                    break;
            }
        }        

        private void AddCustomerButton_OnClick(object sender, RoutedEventArgs e)
        {
            string PathToCustomersJson = App.PathToCompanies + App.companyActive.CompanyName + "\\customers.json";

            if (string.IsNullOrEmpty(CustomerToAdd.Name) || string.IsNullOrEmpty(CustomerToAdd.Email))
            {
                btnErrorFlyout.Text = "Please fill in the missing fields to continue!";
                ButtonFlyout.ShowAt(AddCustomerBtn);
            }
            else
            {
                if (File.Exists(PathToCustomersJson))
                {
                    string customersFile = File.ReadAllText(PathToCustomersJson);
                    JSONNode customersList = JSONNode.Parse(customersFile);
                    AddToCustomers(PathToCustomersJson, customersList.AsArray);
                }
                else
                {
                    JSONArray Customers = new JSONArray();
                    AddToCustomers(PathToCustomersJson, Customers);
                }
            }

        }

        private void AddToCustomers(string PathToCustomersJson, JSONArray customers)
        {
            if (string.IsNullOrEmpty(customers))
            {
                customers = new JSONArray();
            }
            JSONObject newCustomer = new JSONObject();

            newCustomer.Add("Name", CustomerToAdd.Name);
            newCustomer.Add("Email", CustomerToAdd.Email);            
            newCustomer.Add("Contact", CheckStringNotNull(CustomerToAdd.Contact));
            newCustomer.Add("Address", CheckStringNotNull(CustomerToAdd.Address));
            newCustomer.Add("VatOrTax", CheckStringNotNull(CustomerToAdd.VatOrTax));
            newCustomer.Add("ContactPerson", CheckStringNotNull(CustomerToAdd.ContactPerson));
            newCustomer.Add("InvoiceCount", "0");
            JSONArray customerInvoiceArray = new JSONArray();
            newCustomer.Add("Invoices", customerInvoiceArray.ToString());           

            customers.Add(newCustomer);

            File.WriteAllText(PathToCustomersJson, customers.ToString());

            Customer theCustomer = new()
            {
                Name = newCustomer["Name"],
                Email = newCustomer["Email"],
                Contact = newCustomer["Contact"],
                Address = newCustomer["Address"],
                VatOrTax = newCustomer["VatOrTax"],
                ContactPerson = newCustomer["ContactPerson"],
                InvoiceCount = newCustomer["InvoiceCount"],
                Invoices = new List<InvoiceClass>()
            };
            App.CUSTOMERS.Add(theCustomer);
            App.companyActive.TotalCustomers++;
            SaveManager.SaveCompanyEdits();
            AddCustomerFrame.NavigateToType(typeof(CustomerViewPage), theCustomer, App.AnimatePage("right"));
        }

        private string CheckStringNotNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                return str;
            }
        }
    }
}