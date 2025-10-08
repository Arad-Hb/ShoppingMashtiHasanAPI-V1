using Catalogue.Domain.Category;
using Catalogue.Domain.Models;
using Catalogue.Domain.Product;
using Framework.Domain.BaseModel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.DomainServiceContract.Services
{
    public interface ICategoryRepository
    {
        Task<OperationResult> AddNewCategory(CategoryAddModel category);
        Task<bool> ExistCategoryName(string categoryName);
        Task<bool> SetLineAge(int CategoryID,string Lineage,int depth);
        Task<bool> ExistSlug(string slug);
        Task<bool> ExistCategoryFeature(int CategoryID, int FeatureID);
        Task<bool> HasChildCategory(int CategoryID);
        Task<bool> HasProduct(int CategoryID);
        Task<OperationResult> UpdateNewCategory(CategoryUpdateModel category);
        Task<OperationResult> DeleteCategory(int CategoryID);
        Task<CategoryListComplexResult> Search(CategorySearchModel sm);
        Task<CategoryUpdateModel> GetCategory(int CategoryID);
        Task<OperationResult> AssignFeatureToCategory(int CategoryID, int FeatureID);
        Task<OperationResult> RemoveFeatureFromCategory(int CategoryFeatureID);
        Task<int> GetProductCount(int CategoryID);
        Task<int> GetChildeCount(int CategoryID);
        Task<bool> AsignedCurrentSlugToAnotherCategory(int CategoryID,string Slug);
        Task<bool> AsignedCurrentCategoryNameToAnotherCategory(int CategoryID,string CategoryName);
        //Task AssignLineAgeAndDepthToCategory(int CategoryID,string Lineage,int depth);
        Task<Category> GetParent(int CategoryID);
        Task<int> GetChildNodeLevels(int CategoryID);
        Task<List<int>> GetChildren(int CategoryID);




        //MyGroupCollectionAttribute interfaces:

        Task<string> GetLineageString(int categoryId);
        Task<OperationResult> AssignLineAgeAndDepthToCategory(int CategoryID);
        Task<string> CalculateLineage(Category cat);


    }
}
