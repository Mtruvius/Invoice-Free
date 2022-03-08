using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice_Free
{
    public class InvoiceClass
    {
        public string CustomerName;
        public string Date;        
        public string Number;        
        public List<InvoiceProduct> InvoicedProducts;
        public float ExcludingTaxTotal;
        public float InvoiceTotal;
        public bool Completed;
    }
}
