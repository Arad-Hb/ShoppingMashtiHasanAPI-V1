using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Product
{
    public class ProductUpdateNewProduct
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public int BasePrice { get; set; }
        public string Desceription { get; set; }
        public string Introduction { get; set; }
        public string DefaultImage { get; set; }
        public int SupplierID { get; set; }
       
        
    }
}
