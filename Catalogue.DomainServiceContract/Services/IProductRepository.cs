using Catalogue.Domain.Models;
using Catalogue.Domain.Product;
using Framework.Domain.BaseInterfaces;
using Framework.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.DomainServiceContract.Services
{
    public interface IProductRepository:IBaseRepositorySearchable<ProductAddModel,ProductUpdateNewProduct,ProductListItem,ProductSearchModel,int>
    {
       Task<bool> ProductNameAlreadyExist(string ProductName);
        Task<OperationResult> AssignFeatureToProduct(ProductFeature pf);
        Task<OperationResult> SetSeoModel(ProductSeoModel productSeoModel);
       Task<List<int>> GetProductCategoryFeatures(int productID);
        Task<bool> AssignAllCategoryFeaturesToProduct(int productID,List<int> categoryFeatureIDs);
        Task<bool> IFSlugExistForAnotherProductID(int ProductID,string Slug);
        Task<bool> IFProductNameExistForAnotherProductID(int ProductID, string ProductName);

    }
}
