using Framework.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Product
{
    public class CategorySearchModel:PageModel
    {
        public CategorySearchModel()
        {
            this.txt = string.Empty;
        }
        public string txt { get; set; }
        public int? ParentID { get; set; }
    }
}
