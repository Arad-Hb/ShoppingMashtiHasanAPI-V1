using Framework.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Product
{
    public class ProductSearchModel:PageModel
    {
        public int? FromUnitPrice { get; set; }
        public int? ToUnitPrice { get; set; }
        public string ProductName { get; set; }
        public int? Level1CategoryID { get; set; }
        public int? Level2CategoryID { get; set; }
        public int? Level3CategoryID { get; set; }

    }
}
