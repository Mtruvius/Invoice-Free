using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Invoice_Free
{
    public class Company
    {
        public string CompanyName;
        public ImageSource CompanyLogo;        
        public string Contact;
        public string Email;
        public string Address;
        public string VatOrTax;
        public string RegNo;
        public string ContactPerson;
        public int LastInvoiceNo;
        public float[] PriorRevenue;
        public float[] PreviousRevenue;
        public float[] Revenue;
        public int CurrentYear;
        public int CompleteInvoices;
        public int PendingInvoices;
        public int TotalQuotes;
        public int TotalCustomers;
        
    }
}
