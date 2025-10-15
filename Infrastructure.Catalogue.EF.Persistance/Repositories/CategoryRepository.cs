using Catalogue.Domain.Category;
using Catalogue.Domain.Models;
using Catalogue.Domain.Product;
using Catalogue.DomainServiceContract.Services;
using Framework.Domain.BaseModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Catalogue.EF.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogueContext db;

        public CategoryRepository(CatalogueContext db)
        {

            this.db = db;

        }

        public async Task<OperationResult> AssignFeatureToCategory(int CategoryID, int FeatureID)
        {
            OperationResult op = new OperationResult("Assign_Feature_To_Category");
            try
            {
                CategoryFeature cf = new CategoryFeature { CategoryID = CategoryID, FeatureID = FeatureID };
                db.CategoryFeatures.Add(cf);
                await db.SaveChangesAsync();
                return op.ToSuccess("Add Successfully",cf.CategoryFeatureID);
            }
            catch 
            {
                return op.ToFail("AssignFeatureToCategory Failed");
                
            }

        }

        public async Task<OperationResult> DeleteCategory(int CategoryID)
        {
            OperationResult op = new OperationResult("Remove Category");

            try
            {
                var cat = await db.Categories.FirstOrDefaultAsync(X => X.CategoryID == CategoryID);
                if (cat == null)
                {
                    return op.ToFail("Invalid CategoryID", CategoryID);
                }
                db.Categories.Remove(cat);
                await db.SaveChangesAsync();
                return op.ToSuccess("Category Removed", CategoryID);
            }
            catch (Exception ex)
            {
                return op.ToFail("Category Remove Failed " + ex.Message, CategoryID);
                
            }
        }

        public async Task<bool> ExistCategoryName(string categoryName)
        {
            return await db.Categories.AnyAsync(x=> x.CategoryName == categoryName);
        }

        public  async Task<bool> ExistSlug(string slug)
        {
                        return await db.Categories.AnyAsync(x=> x.Slug == slug);

        }

        public async Task<CategoryUpdateModel> GetCategory(int CategoryID)
        {
            var c = await db.Categories.FirstOrDefaultAsync(x=>x.CategoryID== CategoryID);
            if (c!=null)
            {
                CategoryUpdateModel cat = new CategoryUpdateModel
                {
                    CategoryID = CategoryID,
                    CategoryDescription = c.CategoryDescription,
                    CategoryName = c.CategoryName,
                    Lineage = c.Lineage,
                    ParentID = c.ParentID,
                    Depth=c.Depth
                    
                };
                return cat;
            }
            else
            {
                return new CategoryUpdateModel();
            }
           
        }
        
        public async Task<int> GetChildeCount(int CategoryID)
        {
            return await  db.Categories.CountAsync(x=>x.ParentID== CategoryID);
        }

        public async Task<int> GetProductCount(int CategoryID)
        {
            return await db.Products.CountAsync(x=>x.CategoryID == CategoryID);
        }

        public async Task<OperationResult> RemoveFeatureFromCategory(int CategoryFeatureID)
        {
            OperationResult op = new OperationResult("RemoveFeatureFromCategory");
            try
            {
                CategoryFeature cf = await db.CategoryFeatures.FirstOrDefaultAsync(x=>x.CategoryFeatureID== CategoryFeatureID);
                db.CategoryFeatures.Remove(cf);
                await db.SaveChangesAsync();
                return op.ToSuccess("RemoveFeatureFromCategory Successfully", cf.CategoryFeatureID);
            }
            catch 
            {
                return op.ToFail("RemoveFeatureFromCategory Failed");

            }
        }

        public async Task<CategoryListComplexResult> Search(CategorySearchModel sm)
        {
            var r = new CategoryListComplexResult();
            
            
            var q = from c in db.Categories select c;

            if (!string.IsNullOrEmpty(sm.txt))
            {
                q = q.Where(x => x.CategoryName.Contains(sm.txt) || x.CategoryDescription.Contains(sm.txt));
            }
            if (sm.ParentID !=null)
            {
                q = q.Where(x => x.ParentID == sm.ParentID);

            }
            var q2 = from cat in q select new CategoryListItem 
            {
                CategoryID =   cat.CategoryID,
                CategoryName = cat.CategoryName,
                ProductCount = cat.Products.Count()
            };
            sm.RecordCount = await q2.CountAsync();
           
            q2 = q2.OrderByDescending(x => x.CategoryID).Skip(sm.PageIndex * sm.PageSize).Take(sm.PageSize);
            r.Categories = await q2.ToListAsync();
            r.SearchModel = sm;
            return r;




        }

        public async Task<bool> ExistCategoryFeature(int CategoryID, int FeatureID)
        {
            return await db.CategoryFeatures.AnyAsync(x => x.CategoryID == CategoryID && x.FeatureID == FeatureID);
        }

        public async Task<bool> HasChildCategory(int CategoryID)
        {
            return await db.Categories.AnyAsync(x=>x.ParentID == CategoryID);
        }

        public async Task<bool> HasProduct(int CategoryID)
        {
            return await db.Products.AnyAsync(x=>x.CategoryID== CategoryID);
        }

        public async Task<bool> AsignedCurrentSlugToAnotherCategory(int CategoryID, string Slug)
        {
            return await db.Categories.AnyAsync(c=>c.CategoryID!= CategoryID && c.Slug== Slug);
        }

        public async Task<bool> AsignedCurrentCategoryNameToAnotherCategory(int CategoryID, string CategoryName)
        {
            return await db.Categories.AnyAsync(c => c.CategoryID != CategoryID && c.CategoryName == CategoryName);
        }

        public async Task<Category> GetParent(int CategoryID)
        {
            return await db.Categories.FirstOrDefaultAsync(x => x.ParentID == CategoryID);
        }

        public async Task<bool> SetLineAge(int CategoryID, string Lineage,int Depth)
        {
            try
            {
                var c = await db.Categories.FirstOrDefaultAsync(x => x.CategoryID == CategoryID);
                c.Lineage = Lineage;
                c.Depth = Depth;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        public List<CategoryWithLevels> GetAllChildWithRelativeLevels(int CategoryID)
        {
            string str = @$";WITH CatTree AS (
    -- ریشه
    SELECT 
        c.CategoryID,
        c.ParentID,
        c.CategoryName,
        c.Lineage,
        1 AS DepthRelative
    FROM dbo.Categories AS c
    WHERE c.CategoryID = {CategoryID.ToString()}

    UNION ALL

    -- فرزندان
    SELECT 
        ch.CategoryID,
        ch.ParentID,
        ch.CategoryName,
        ch.Lineage,
        p.DepthRelative + 1
    FROM dbo.Categories AS ch
    JOIN CatTree AS p
        ON ch.ParentID = p.CategoryID
)
SELECT 
    CategoryID,
    ParentID,
    CategoryName,
    Lineage,
    DepthRelative
FROM CatTree
-- اگر فقط بچه‌ها را می‌خواهی (بدون خود ریشه):
ORDER BY DepthRelative, CategoryName
OPTION (MAXRECURSION 4);";

            var records =  db.CategoryWithLevels
      .FromSqlRaw(str)
      .AsNoTracking()
      .ToList();
            return records;
        }



        //my repository methods:

        public Category ToDBModel(CategoryAddModel category)
        {
            return new Category
            {
                CategoryDescription = category.CategoryDescription,
                CategoryName = category.CategoryName,
                Lineage = string.Empty,
                Depth = category.Depth,
                ParentID = category.ParentID??null
            };
        }
        public async Task<OperationResult> AddNewCategory(CategoryAddModel category)
        {
            OperationResult op = new OperationResult("Add_New_Category");
            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var cat = ToDBModel(category);
                db.Categories.Add(cat);
                await db.SaveChangesAsync();

                cat.Lineage = CalculateLineage(cat);
                db.Categories.Update(cat);
                await db.SaveChangesAsync();

                await transaction.CommitAsync();
                return op.ToSuccess("Category Added Successfully", cat.CategoryID);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return op.ToFail("Category Added Failed " + ex.Message);
            }
        }

        public async Task<OperationResult> UpdateNewCategory(CategoryUpdateModel category)
        {
            OperationResult op = new OperationResult("UpdateNewCategory");
            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                Category c = await db.Categories.FirstOrDefaultAsync(x => x.CategoryID == category.CategoryID);
                c.CategoryDescription = category.CategoryDescription;
                c.CategoryName = category.CategoryName;
                c.ParentID = category.ParentID;
                c.Depth = category.Depth;

                await db.SaveChangesAsync();

                c.Lineage = CalculateLineage(c);
                

                db.Categories.Update(c);
                await db.SaveChangesAsync();

                await transaction.CommitAsync();
                return op.ToSuccess("Update Successfully", c.CategoryID);
            }
            catch
            {
                await transaction.RollbackAsync();
                return op.ToFail("Update Failed", category.CategoryID);
            }
        }

        public async Task<List<int>> GetChildren(int CategoryID)
        {
            var catIds = new List<int>();
            var node = await db.Categories.FirstOrDefaultAsync(x => x.CategoryID == CategoryID);
            catIds = await db.Categories.Where(x => x.Lineage.StartsWith(node.Lineage)).Select(x=>x.CategoryID).ToListAsync();
            return catIds;
        }


        public string CalculateLineage(Category cat)
        {
            string lineage = string.Empty;

            if (cat.ParentID == null)
            {
                lineage = cat.CategoryID.ToString() + ",";
            }
            else
            {
                lineage = (cat.Parent.Lineage + (cat.CategoryID.ToString())) + ",";
            }
            return lineage;
        }
        public async Task<OperationResult> AssignLineAgeAndDepthToCategory(int CategoryID)
        {
            OperationResult op=new OperationResult("AssignLineAgeAndDepthToCategory");
            try
            {
                var cat = await db.Categories.FirstOrDefaultAsync(x => x.CategoryID == CategoryID);
                cat.Lineage = this.CalculateLineage(cat);
                cat.Depth = cat.Lineage.Count(c => c == ',');
                await db.SaveChangesAsync();
                return op.ToSuccess("assigning lineages and depth was successfull",CategoryID);
            }
            catch(Exception ex) 
            {
                return op.ToFail("assigning lineages and depth went wrong"+ex.Message, CategoryID);
            }
        }
        public async Task<string> GetLineageString(int categoryId)
        {
            string lineage = string.Empty;
            var category = await db.Categories.FirstOrDefaultAsync(x => x.CategoryID == categoryId);

            if (category.ParentID == null)
            {
                lineage = categoryId.ToString() + ",";
            }
            else
            {
                lineage = (category.Parent.Lineage + categoryId.ToString()) + ",";
            }
            return lineage;
        }
        public async Task<int> GetChildNodeLevels(int CategoryID)
        {
            var node = await db.Categories.FirstOrDefaultAsync(x => x.CategoryID == CategoryID);
            int maxdepth = await db.Categories.Where(x => x.Lineage.StartsWith(node.Lineage)).MaxAsync(x => x.Depth);
            var MaxChildDepth = maxdepth - (node.Depth);
            return MaxChildDepth;
        }


public async Task<bool> UpdateDescendantsDepthAndLineageAsync(int categoryId)
    {
            try
            {
                var sql = @"
        ;WITH DescendantsCTE AS (
            -- Anchor
            SELECT
                c.CategoryID,
                c.ParentID,
                c.Depth,
                c.Lineage
            FROM Categories AS c
            WHERE c.CategoryID = @RootId
        
            UNION ALL
        
            -- Recursive
            SELECT
                child.CategoryID,
                child.ParentID,
                p.Depth + 1 AS Depth,
                CASE
                    WHEN p.Lineage IS NULL OR p.Lineage = '' THEN CAST(child.CategoryID AS VARCHAR(255))
                    ELSE p.Lineage + '/' + CAST(child.CategoryID AS VARCHAR(255))
                END AS Lineage
            FROM Categories AS child
            INNER JOIN DescendantsCTE AS p
                ON child.ParentID = p.CategoryID
        )
        UPDATE c
        SET c.Depth = d.Depth,
            c.Lineage = d.Lineage
        FROM Categories AS c
        INNER JOIN DescendantsCTE AS d ON c.CategoryID = d.CategoryID;
        ";

                await using var conn = db.Database.GetDbConnection();
                await using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@RootId", categoryId));

                if (conn.State != System.Data.ConnectionState.Open)
                    await conn.OpenAsync();

                var affected = await cmd.ExecuteNonQueryAsync();
                return affected > 0;

            }
            catch (Exception ex) 
            {
            throw new Exception(ex.Message);
            }
       
    }
}
}
