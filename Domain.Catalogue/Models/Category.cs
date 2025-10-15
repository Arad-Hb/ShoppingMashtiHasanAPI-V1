using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int Depth { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int? ParentID { get; set; }
        public string Lineage { get; set; }
        public string Slug { get; set; }
        public string PageTitle { get; set; }
        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }
        public string HeadExtraData { get; set; }
        public string BodyExtraData { get; set; }

        public List<Category> Children { get; set; }
        public Category Parent { get; set; }
        public List<Product> Products { get; set; }
        public List<CategoryFeature> CategoryFeatures { get; set; }

        public Category()
        {
            CategoryFeatures = new List<CategoryFeature>();
            Children = new List<Category>();
            Products = new List<Product>();
        }
    }
}
