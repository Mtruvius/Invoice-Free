using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice_Free
{
    public class Customer
    {       
        public string Name;
        public string Contact;
        public string Email;
        public string Address;
        public string VatOrTax;
        public string ContactPerson;
        public int InvoiceCount;
        public List<InvoiceClass> Invoices;
    }
}
