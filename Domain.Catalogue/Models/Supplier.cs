using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Picture { get; set; }
        public ICollection<Product> Products { get; set; }
        public Supplier()
        {
            Products = new List<Product>();
        }
    }
}
