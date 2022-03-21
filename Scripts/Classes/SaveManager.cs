using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice_Free
{
    public static class SaveManager
    {
        public static void SaveInvoiceToCustomer(Customer selectedCustomer, InvoiceClass invoice)
        {
            Debug.WriteLine(invoice.InvoiceTotal);
            JSONArray completeCustomerList = new JSONArray();

            foreach (Customer customer in App.CUSTOMERS)
            {
                if (customer == selectedCustomer)
                {
                    Debug.WriteLine("customer is equal");
                    customer.InvoiceCount++;
                    customer.Invoices.Add(invoice);
                }
                JSONObject customerObj = new JSONObject();
                customerObj.Add("Name", customer.Name);
                customerObj.Add("Email", customer.Email);
                customerObj.Add("Address", customer.Address);
                customerObj.Add("Contact", customer.Contact);
                customerObj.Add("ContactPerson", customer.ContactPerson);
                customerObj.Add("Tax", customer.Tax);
                customerObj.Add("InvoiceCount", customer.InvoiceCount);

                JSONArray customerInvoiceList = new JSONArray();
                foreach (InvoiceClass inv in customer.Invoices)
                {
                    JSONObject theInv = new JSONObject();
                    theInv.Add("CustomerName", inv.CustomerName);
                    theInv.Add("Date", inv.Date);
                    theInv.Add("Number", inv.Number);
                    theInv.Add("InvoiceTotal", inv.InvoiceTotal);
                    theInv.Add("Completed", inv.Completed);
                    JSONArray invoiceProductList = new JSONArray();
                    foreach (InvoiceProduct product in inv.InvoicedProducts)
                    {
                        JSONObject theProd = new JSONObject();
                        theProd.Add("Name", product.Name);
                        theProd.Add("Description", product.Description);
                        theProd.Add("Quantity", product.Quantity);
                        theProd.Add("TotalPrice", product.TotalPrice);

                        invoiceProductList.Add(theProd);
                    }
                    theInv.Add("InvoicedProducts", invoiceProductList);
                    customerInvoiceList.Add(theInv);
                }
                customerObj.Add("Invoices", customerInvoiceList);
                completeCustomerList.Add(customerObj);
            }
            App.ALL_INVOICES.Add(invoice);

            Debug.WriteLine("customers: " + completeCustomerList.ToString());
            Debug.WriteLine("App.PathToCustomers: " + App.PathToCustomers);

            File.WriteAllText(App.PathToCustomers, completeCustomerList.ToString());
        }

        public static void SaveCustomerEdits()
        {
            Debug.WriteLine("SaveCustomerEdits WAS CALLED");
            Debug.WriteLine("CUSTOMERS COUNT " + App.CUSTOMERS.Count);
            JSONArray CustomerList = new JSONArray();
            
            foreach (Customer cust in App.CUSTOMERS)
            {
                JSONArray CustomerInvoiceList = new JSONArray();
                JSONArray CustomerInvoiceProductList = new JSONArray();

                JSONObject customer = new();
                customer.Add("Name", cust.Name);
                customer.Add("Email", cust.Email);
                customer.Add("Address", cust.Address);
                customer.Add("Contact", cust.Contact);
                customer.Add("ContactPerson", cust.ContactPerson);
                customer.Add("Tax", cust.Tax);
                customer.Add("InvoiceCount", cust.InvoiceCount);

                foreach (InvoiceClass Inv in cust.Invoices)
                {
                    JSONObject invoices = new();
                    invoices.Add("Date", Inv.Date);
                    invoices.Add("Number", Inv.Number);
                    invoices.Add("Completed", Inv.Completed);
                    invoices.Add("InvoiceTotal", Inv.InvoiceTotal);

                    foreach (InvoiceProduct prod in Inv.InvoicedProducts)
                    {
                        JSONObject products = new();
                        products.Add("Name", prod.Name);
                        products.Add("Description", prod.Description);
                        products.Add("Quantity", prod.Quantity);
                        products.Add("TotalPrice", prod.TotalPrice);
                        CustomerInvoiceProductList.Add(products);
                    }

                    invoices.Add("InvoicedProducts", CustomerInvoiceProductList);
                    CustomerInvoiceList.Add(invoices);
                }

                customer.Add("Invoices", CustomerInvoiceList);
                CustomerList.Add(customer);
            }

            File.WriteAllText(App.PathToCustomers, CustomerList.ToString());
        }

        public static void SaveCompanyEdits()
        {
            Debug.WriteLine("SaveCompanyEdits LastInvoiceNo" + App.companyActive.LastInvoiceNo);
            Company company = App.companyActive;
            Debug.WriteLine("company LastInvoiceNo" + company.LastInvoiceNo);
            Debug.WriteLine("TEST1: " + App.companyActive.InvoiceFooterMsg);
            Company EditedCompany = new()
            {
                CompanyName = company.CompanyName,
                Email = company.Email,
                CompanyLogo = company.CompanyLogo,
                Contact = company.Contact,
                Address = company.Address,
                RegNo = company.RegNo,
                Tax = company.Tax,
                ContactPerson = company.ContactPerson,
                LastInvoiceNo = company.LastInvoiceNo,
                PreviousRevenue = company.PriorRevenue,
                PriorRevenue = company.PreviousRevenue,
                Revenue = company.Revenue,
                CurrentYear = company.CurrentYear,
                CompleteInvoices = company.CompleteInvoices,
                PendingInvoices = company.PendingInvoices,
                TotalQuotes = company.TotalQuotes,
                TotalCustomers = company.TotalCustomers,
                AddTax = company.AddTax,
                TaxRate = company.TaxRate,
                InvoiceFooterMsg = company.InvoiceFooterMsg
            };
            Debug.WriteLine("TEST2: " + EditedCompany.InvoiceFooterMsg);
            App.companyActive = EditedCompany;
            Debug.WriteLine("EditedCompany LastInvoiceNo" + EditedCompany.LastInvoiceNo);

            JSONArray newCompanyDetail = new JSONArray();
            JSONArray ProductCatagoriesList = new JSONArray();
            JSONObject newCompanyOBJ = new JSONObject();
            newCompanyOBJ.Add("CompanyName", EditedCompany.CompanyName);
            newCompanyOBJ.Add("Email", EditedCompany.Email);
            newCompanyOBJ.Add("Address", EditedCompany.Address);
            newCompanyOBJ.Add("Contact", EditedCompany.Contact);
            newCompanyOBJ.Add("Tax", EditedCompany.Tax);
            newCompanyOBJ.Add("RegNo", EditedCompany.RegNo);
            newCompanyOBJ.Add("ContactPerson", EditedCompany.ContactPerson);
            newCompanyOBJ.Add("LastInvoiceNo", EditedCompany.LastInvoiceNo);
            newCompanyOBJ.Add("PriorRevenue", GetRevenueArray(EditedCompany.PriorRevenue));
            newCompanyOBJ.Add("PreviousRevenue", GetRevenueArray(EditedCompany.PreviousRevenue));
            newCompanyOBJ.Add("Revenue", GetRevenueArray(EditedCompany.Revenue));
            newCompanyOBJ.Add("CurrentYear", EditedCompany.CurrentYear);
            newCompanyOBJ.Add("CompleteInvoices", EditedCompany.CompleteInvoices);
            newCompanyOBJ.Add("PendingInvoices", EditedCompany.PendingInvoices);
            newCompanyOBJ.Add("TotalQuotes", EditedCompany.TotalQuotes);
            newCompanyOBJ.Add("TotalCustomers", EditedCompany.TotalCustomers);            
            foreach (string catagory in App.PRODUCTCATAGORIESLIST)
            {
                ProductCatagoriesList.Add(catagory);
            }
            newCompanyOBJ.Add("ProductCatagoriesList", ProductCatagoriesList);
            newCompanyOBJ.Add("AddTax", EditedCompany.AddTax);
            newCompanyOBJ.Add("TaxRate", EditedCompany.TaxRate);

            newCompanyOBJ.Add("InvoiceFooterMsg", EditedCompany.InvoiceFooterMsg);
            newCompanyDetail.Add(newCompanyOBJ);

            Debug.WriteLine("TEST3: " + EditedCompany.InvoiceFooterMsg);

            File.WriteAllText(App.PathToCompanies + EditedCompany.CompanyName + "\\" + EditedCompany.CompanyName + ".json", newCompanyDetail.ToString());
        }

        private static JSONNode GetRevenueArray(double[] revenue)
        {
            JSONArray intArray = new JSONArray();
            for (int i = 0; i < revenue.Length; i++)
            {
                intArray.Add(revenue[i]);
            }
            return intArray;
        }
    }
}
