using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.BaseModel
{
    public class GenericComplexResult<TSearchModel,TListModel>
    {
        public TSearchModel SearchModel { get; set; }
        public List<TListModel> List { get; set; }
    }
}
