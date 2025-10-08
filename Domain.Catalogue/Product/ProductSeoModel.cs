using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Product
{
    public class ProductSeoModel
    {
        public int ProductID { get; set; }
        public string Slug { get; set; }
        public string PageTitle { get; set; }
        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }
        public string HeadExtraData { get; set; }
        public string BodyExtraData { get; set; }
    }
}
