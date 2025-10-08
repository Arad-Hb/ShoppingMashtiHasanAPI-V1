using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Category
{
    public class CategoryUpdateModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int? ParentID { get; set; }
        public string Lineage { get; set; }
        //public string Slug { get; set; }
        //public string PageTitle { get; set; }
        //public string MetaTag { get; set; }
        //public string MetaDescription { get; set; }
        //public string HeadExtraData { get; set; }
        //public string BodyExtraData { get; set; }
        public int Depth { get; set; }
    }
}
