using ApplicationServiceContract.Services;
using Catalogue.Domain.Models;
using Catalogue.Domain.Product;
using Catalogue.DomainServiceContract.Services;
using Framework.Domain.BaseModel;
using Sale.DomainServiceContract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations
{
    public class ProductApplication : IProductApplicationServiceContract
    {
        private readonly IProductRepository Repo;
        private readonly IOrderRepository OrderRepo;
        public ProductApplication(IProductRepository Repo, IOrderRepository OrderRepo)
        {
            this.Repo = Repo;
            this.OrderRepo = OrderRepo;
        }
        public async Task<OperationResult> AddNew(ProductAddModel model)
        {
            if (await Repo.ProductNameAlreadyExist(model.ProductName))
            {
                return new OperationResult("AddNewProduct").ToFail("ProductName Already Exist");
            }
            // Get Feature
            var op = await Repo.AddNew(model);
            if (op.Success)
            {
                var features = await Repo.GetProductCategoryFeatures(op.RecordID.Value);
                bool result = await Repo.AssignAllCategoryFeaturesToProduct(op.RecordID.Value, features);
                if (result)
                {
                    return op.ToSuccess("Product Added And Features assigned from category");
                }
                
            }
            return op.ToFail("Add New Product Failed");

            //Assign Feature
        }

        public async Task<OperationResult> AssignFeatureToProduct(ProductFeature pf)
        {
           return await Repo.AssignFeatureToProduct(pf);
        }

        public async Task<ProductUpdateNewProduct> Get(int ID)
        {
            return await Repo.Get(ID);
        }

        public async Task<OperationResult> Remove(int ID)
        {
            if (OrderRepo.HasOrder(ID))
            {
                return new OperationResult("RemoveProduct").ToFail("ProductHasOrder");
                
            }
            return await Repo.Remove(ID);
        }

        public async Task<GenericComplexResult<ProductSearchModel, ProductListItem>> Search(ProductSearchModel sm)
        {
            return await Repo.Search(sm);
        }

        public async Task<OperationResult> SetSeoModel(ProductSeoModel productSeoModel)
        {
            if ( await Repo.IFSlugExistForAnotherProductID(productSeoModel.ProductID,productSeoModel.Slug))
            {
                return new OperationResult("SetSeoModel").ToFail("Slug Already Exist", productSeoModel.ProductID);
            }
            return await Repo.SetSeoModel(productSeoModel);
        }

        public async Task<OperationResult> UpdateNew(ProductUpdateNewProduct model)
        {
            if (await Repo.IFProductNameExistForAnotherProductID(model.ProductID,model.ProductName))
            {
                return new OperationResult("UpdateNewProduct").ToFail("ProductName AlreadyExist", model.ProductID);
            }
            return await Repo.UpdateNew(model);
        }
    }
}
