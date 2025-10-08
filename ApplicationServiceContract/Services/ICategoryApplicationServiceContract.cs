using Catalogue.Domain.Category;
using Catalogue.Domain.Product;
using Framework.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServiceContract.Services
{
    public interface ICategoryApplicationServiceContract
    {
        Task<OperationResult> AddNewCategory(CategoryAddModel category);
        Task<OperationResult> UpdateNewCategory(CategoryUpdateModel category);
        Task<OperationResult> DeleteCategory(int CategoryID);
        Task<CategoryListComplexResult> Search(CategorySearchModel sm);
        Task<CategoryUpdateModel> GetCategory(int CategoryID);
        Task<OperationResult> AssignFeatureToCategory(int CategoryID, int FeatureID);
        Task<OperationResult> RemoveFeatureFromCategory(int CategoryFeatureID);
        
        
    }
}
