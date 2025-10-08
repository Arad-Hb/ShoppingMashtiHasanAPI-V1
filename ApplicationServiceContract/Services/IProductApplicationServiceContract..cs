using Catalogue.Domain.Models;
using Catalogue.Domain.Product;
using Framework.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServiceContract.Services
{
    public interface IProductApplicationServiceContract
    {
         Task<OperationResult> AddNew(ProductAddModel model);


        //public Task<bool> AssignAllCategoryFeaturesToProduct(int productID, List<int> categoryFeatureIDs);
        

         Task<OperationResult> AssignFeatureToProduct(ProductFeature pf);
         Task<ProductUpdateNewProduct> Get(int ID);
         Task<OperationResult> Remove(int ID);
         Task<GenericComplexResult<ProductSearchModel, ProductListItem>> Search(ProductSearchModel sm);
         Task<OperationResult> SetSeoModel(ProductSeoModel productSeoModel);
         Task<OperationResult> UpdateNew(ProductUpdateNewProduct model);
       
    }
}
