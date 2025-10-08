using Catalogue.Domain.Models;
using Catalogue.Domain.Product;
using Catalogue.DomainServiceContract.Services;
using Framework.Domain.BaseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Catalogue.EF.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogueContext db;
        public ProductRepository(CatalogueContext db)
        {
            this.db = db;
        }
        public async Task<OperationResult> AddNew(ProductAddModel model)
        {
            OperationResult op = new OperationResult("AddNewProduct");
            try
            {
                Product product = new Product
                {
                    BasePrice = model.BasePrice,
                    CategoryID = model.CategoryID,
                    ProductName = model.ProductName,
                    DefaultImage = model.DefaultImage,
                    Desceription = model.Desceription,
                    Introduction = model.Introduction,
                    SupplierID = model.SupplierID,

                };
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return op.ToSuccess("Added Successfully", product.ProductID);
            }
            catch (Exception ex)
            {

                return op.ToFail("Added Failed " + ex.Message);
            }
            

        }

        public async Task<bool> AssignAllCategoryFeaturesToProduct(int productID, List<int> categoryFeatureIDs)
        {
            try
            {
                if (categoryFeatureIDs != null && categoryFeatureIDs.Any())
                {
                    foreach (var featureID in categoryFeatureIDs)
                    {
                        db.ProductFeatures.Add(new ProductFeature
                        {
                            EffectonBasePrice = 0
                            ,
                            FeatureID = featureID,
                            IsDefault = false
                            ,
                            ProductID = productID
                            ,
                            Value = string.Empty
                        });
                    }
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch 
            {

                return false;
            }
        }

        public async Task<OperationResult> AssignFeatureToProduct(ProductFeature pf)
        {
            OperationResult op =  new OperationResult("AssignFeatureToProduct");
            try
            {
                db.ProductFeatures.Add(pf);
                await db.SaveChangesAsync();
                return op.ToSuccess("Feature Assigned Successfully",pf.ProductFeatureID);
            }
            catch (Exception ex)
            {

                return op.ToFail(ex.Message);
            }
        }

        public async Task<ProductUpdateNewProduct> Get(int ID)
        {
            var p = await db.Products.FirstOrDefaultAsync(x => x.ProductID == ID);
            ProductUpdateNewProduct r = new ProductUpdateNewProduct 
            { 
                BasePrice = p.BasePrice,
                ProductID = p.ProductID,
                CategoryID = p.CategoryID,
                DefaultImage = p.DefaultImage,
                Desceription = p.Desceription,
                Introduction= p.Introduction,
                ProductName = p.ProductName,
                SupplierID = p.SupplierID

            
            };
            return r;
        }

        public async Task< List<int>> GetProductCategoryFeatures(int productID)
        {
           var cat = await db.Products.FirstOrDefaultAsync(x=>x.ProductID==productID);

            List<int> result =
                await  db.CategoryFeatures.Where(x => x.CategoryID == cat.CategoryID).Select(x => x.FeatureID).ToListAsync();
            return result;
        }

        public async Task<bool> IFProductNameExistForAnotherProductID(int ProductID, string ProductName)
        {
            return await db.Products.AnyAsync(x => x.ProductName == ProductName && x.ProductID != ProductID);
        }

        public async Task<bool> IFSlugExistForAnotherProductID(int ProductID, string Slug)
        {
            return await db.Products.AnyAsync(x => x.Slug == Slug && x.ProductID != ProductID);
        }

        public async Task<bool> ProductNameAlreadyExist(string ProductName)
        {
            return await db.Products.AnyAsync(x => x.ProductName== ProductName);
        }

        public async Task<OperationResult> Remove(int ID)
        {
            OperationResult op = new OperationResult("Remove");

            await using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                var pfs = db.ProductFeatures.Where(x => x.ProductID == ID);
                db.ProductFeatures.RemoveRange(pfs);
                await db.SaveChangesAsync();

                var p = await db.Products.FirstOrDefaultAsync(x => x.ProductID == ID);
                if (p != null)
                {
                    db.Products.Remove(p);
                    await db.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return op.ToSuccess("Removed", ID);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return op.ToFail(ex.Message);
            }


        }

        public async Task<GenericComplexResult<ProductSearchModel, ProductListItem>> Search(ProductSearchModel sm)
        {
            GenericComplexResult<ProductSearchModel, ProductListItem> result = new GenericComplexResult<ProductSearchModel, ProductListItem>();
            var q = from item in db.Products select item;
            if (!string.IsNullOrEmpty(sm.ProductName))
            {
                q = q.Where(x => x.ProductName.Contains(sm.ProductName));
            }
            if (sm.FromUnitPrice!=null)
            {
                q = q.Where(x => x.BasePrice >= sm.FromUnitPrice);
            }
            if (sm.ToUnitPrice!= null)
            {
                q = q.Where(x => x.BasePrice <= sm.ToUnitPrice);
            }
            bool categorySearched = false;
            if (sm.Level3CategoryID !=null)
            {
                q = q.Where(x => x.CategoryID == sm.Level3CategoryID);
                categorySearched = true;
            }
            if (!categorySearched && sm.Level2CategoryID!=null)
            {
                var c = await db.Categories.FirstOrDefaultAsync(x => x.CategoryID == sm.Level2CategoryID);
                var level3IDS = await db.Categories.Where(x=>x.Lineage.StartsWith(c.Lineage)).Select(x=>x.CategoryID).ToListAsync();
                q = q.Where(prod => level3IDS.Contains(prod.CategoryID));
                categorySearched = true;

            }
            if (! categorySearched && sm.Level1CategoryID!=null)
            {
                var c = await db.Categories.FirstOrDefaultAsync(x => x.CategoryID == sm.Level1CategoryID);
                var nextLevelIDS = await db.Categories.Where(x => x.Lineage.StartsWith(c.Lineage)).Select(x => x.CategoryID).ToListAsync();
                q = q.Where(prod => nextLevelIDS.Contains(prod.CategoryID));
            }
            var q2 = from prod in q
                     select new ProductListItem
                     {
                         CategoryName = prod.Category.CategoryName
                ,
                         ProductID = prod.ProductID
                ,
                         ProductName = prod.ProductName
                ,
                         SupplierName = prod.Supplier.SupplierName
                ,
                         UnitPrice = prod.BasePrice
                     };
            sm.RecordCount = await q.CountAsync();
            q2 = q2.OrderByDescending(x=>x.ProductID).Skip(sm.PageIndex * sm.PageSize).Take(sm.PageSize);
            result.List = q2.ToList();
            result.SearchModel = sm;
            return result;

            

        }

        public async Task< OperationResult> SetSeoModel(ProductSeoModel productSeoModel)
        {
            OperationResult op = new OperationResult("SetSeoModel");
            try
            {
                var prod = await db.Products.FirstOrDefaultAsync(x => x.ProductID == productSeoModel.ProductID);
                prod.Slug = productSeoModel.Slug;
                prod.BodyExtraData = productSeoModel.BodyExtraData;
                prod.HeadExtraData = productSeoModel.HeadExtraData;
                prod.MetaDescription = productSeoModel.MetaDescription;
                prod.MetaTag = productSeoModel.MetaTag;
                prod.PageTitle = productSeoModel.PageTitle;
                await db.SaveChangesAsync();
                return op.ToSuccess("");
            }
            catch 
            {
                return op.ToFail("");

            }

        }

        public async Task<OperationResult> UpdateNew(ProductUpdateNewProduct model)
        {
            OperationResult op = new OperationResult("UpdateNew");
            try
            {
                var prod = await db.Products.FirstOrDefaultAsync(x => x.ProductID == model.ProductID);
                prod.ProductName = model.ProductName;
                prod.BasePrice = model.BasePrice;
                prod.CategoryID = model.CategoryID;
                prod.DefaultImage = model.DefaultImage;
                prod.Desceription = model.Desceription;
                prod.SupplierID = model.SupplierID;
                prod.Introduction = model.Introduction;
                await db.SaveChangesAsync();
                return op.ToSuccess("Product Updated Successfully", prod.ProductID);
            }
            catch (Exception ex)
            {

                return op.ToFail("Product Updated Failed"+ex.Message);
            }
            
        }
    }
}
