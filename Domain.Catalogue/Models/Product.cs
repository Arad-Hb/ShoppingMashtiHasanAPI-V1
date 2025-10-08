using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public int BasePrice { get; set; }
        public string Desceription { get; set; }
        public string    Introduction { get; set; }
        public string DefaultImage { get; set; }
        public string Slug { get; set; }
        public string PageTitle { get; set; }
        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }
        public string HeadExtraData { get; set; }
        public string BodyExtraData { get; set; }
        public Category Category { get; set; }
        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; }
        public Product()
        {
            ProductFeatures = new List<ProductFeature>();
        }
    }
}
