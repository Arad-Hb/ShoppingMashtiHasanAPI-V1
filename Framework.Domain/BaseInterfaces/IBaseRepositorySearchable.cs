using Framework.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.BaseInterfaces
{
    public interface IBaseRepositorySearchable<TAddModel,TUpdateModel,TListItem,TSearchModel,TKey>
    {
        Task<OperationResult> AddNew(TAddModel model);
        Task<OperationResult> UpdateNew(TUpdateModel model);
        Task<OperationResult> Remove(TKey ID);
        Task<TUpdateModel> Get(TKey ID);
        Task<GenericComplexResult<TSearchModel,TListItem>> Search(TSearchModel model);
    }
}
