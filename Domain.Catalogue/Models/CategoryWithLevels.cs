using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Models
{
    public class CategoryWithLevels
    {
        public int CategoryID { get; set; }
        public int? ParentID { get; set; }
        public string CategoryName { get; set; }
        public int DepthRelative { get; set; }
        public string Lineage { get; set; }
    }
}
