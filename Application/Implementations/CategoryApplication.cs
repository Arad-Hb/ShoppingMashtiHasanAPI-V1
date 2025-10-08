using ApplicationServiceContract.Services;
using Catalogue.Domain.Category;
using Catalogue.Domain.Models;
using Catalogue.Domain.Product;
using Catalogue.DomainServiceContract.Services;
using Framework.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations
{
    public class CategoryApplication : ICategoryApplicationServiceContract
    {
        private readonly ICategoryRepository repo;
        public CategoryApplication(ICategoryRepository repo)
        {
            this.repo = repo;
        }
        //public async Task<OperationResult> AddNewCategory(CategoryAddModel category)
        //{
        //    CategoryUpdateModel parent = null;
        //    OperationResult op = new OperationResult("AddNewCategory");
        //    int depth = 0;
        //    if (category.ParentID==null) 
        //    {
        //        depth = 1;
        //    }
        //    if (category.ParentID!=null)
        //    {
        //         parent = await repo.GetCategory(category.ParentID.Value);
        //        if (parent.Depth==3)
        //        {
        //            return new OperationResult("AddNewCategory").ToFail("this tree can hav maximum 3 depth",System.Net.HttpStatusCode.Forbidden);
        //        }
        //        depth = parent.Depth + 1;
                
        //    }
        //    if (await repo.ExistCategoryName(category.CategoryName)) 
        //    {
        //        return op.ToFail("CatgoryName Already Exist",System.Net.HttpStatusCode.Conflict);
        //    }

        //   // category.Depth = depth;
        //    var AddResult = await repo.AddNewCategory(category);
        //    string l;
        //    if (category.ParentID == null)
        //    {
        //        l = AddResult.RecordID.ToString() + ",";
        //    }
        //    else
        //    {
        //        l = parent.Lineage + AddResult.RecordID.ToString() + ",";
        //    }
        //    var r = await repo.SetLineAge(AddResult.RecordID.Value, l,depth);
        //    return AddResult;


        //}

        public async Task<OperationResult> AssignFeatureToCategory(int CategoryID, int FeatureID)
        {
            OperationResult op = new OperationResult("AssignFeatureToCategory");
            var exist = await repo.ExistCategoryFeature(CategoryID, FeatureID);
            if (exist)
            {
                return op.ToFail("Feature Already assigned to category");
            }
            var cat = await repo.GetCategory(CategoryID);
            if (cat.Depth !=3) {
                return op.ToFail("Feature can not assign level1 or level2  category");
            }
            op = await(repo.AssignFeatureToCategory(CategoryID,FeatureID));
            return op;
        }

        public async Task<OperationResult> DeleteCategory(int CategoryID)
        {
            OperationResult op = new OperationResult("DeleteCategory");

            if (await repo.HasChildCategory(CategoryID))
            {
                return op.ToFail("This category has sub category");
            }
            if (await repo.HasProduct(CategoryID))
            {
                return op.ToFail("This category has sub Product");

            }
            
             return await repo.DeleteCategory(CategoryID);
        }

        public async Task<CategoryUpdateModel> GetCategory(int CategoryID)
        {
            //TODO IF Current Use Has Permission
            //TODO Log who seen Data
            return await repo.GetCategory(CategoryID);
        }

        public async Task<OperationResult> RemoveFeatureFromCategory(int CategoryFeatureID)
        {
            return await repo.RemoveFeatureFromCategory(CategoryFeatureID);
        }

        public async Task<CategoryListComplexResult> Search(CategorySearchModel sm)
        {
            return await repo.Search(sm);
        }

        //public async Task<OperationResult> UpdateNewCategory(CategoryUpdateModel category)
        //{
        //    OperationResult op = new OperationResult("UpdateNewCategory");
        //    if (await repo.AsignedCurrentCategoryNameToAnotherCategory(category.CategoryID, category.CategoryName))
        //    {
        //        return op.ToFail("CategoryName Exist");
        //    }

        //    CategoryUpdateModel NewParentNode = null;
        //    int NewParentDepth=0;
        //    var MaxChildrenRelativeDepth = 0;//omgh e farzandan az node e jari
        //    var OldNode = await repo.GetCategory(category.CategoryID);//version Ghadimi node ghbal az taghirat
        //    string oldParentLineage = "";
        //    if (OldNode.ParentID != category.ParentID)// agar parent avaz shode bood
        //    {
        //        var oldParent = await repo.GetCategory(OldNode.ParentID ?? 0);
        //        if (oldParent != null)
        //        {
        //            oldParentLineage = oldParent.Lineage;
        //        }
        //        else //its extra
        //        {
        //            oldParentLineage = "";
        //        }

        //        NewParentNode = await repo.GetCategory(category.ParentID ?? 0);
        //        string newParentLineage = string.Empty;
        //        if (NewParentNode == null || (category.ParentID ?? 0) == 0)
        //        {
        //            NewParentDepth = 1;
        //            newParentLineage = "";
        //        }
        //        else
        //        {
        //            newParentLineage = NewParentNode.Lineage;
        //            NewParentDepth = NewParentNode.Depth;
        //        }

        //        MaxChildrenRelativeDepth
        //            = await repo.GetChildNodeLevels(category.CategoryID);//max omgh e nesbi farzandan nesbat be valed ghadim
        //        if (MaxChildrenRelativeDepth + NewParentDepth > 3)
        //        {
        //            return op.ToFail("عمق نمی تواند بیش از 3 باشد", category.CategoryID);
        //        }

        //        var nodeChildrenList = await repo.GetChildren(category.CategoryID); //setting children node lineages based on new parent lineage

        //        foreach (var nodeChild in nodeChildrenList)
        //        {
        //            string newLineage = string.Empty;
        //            if (string.IsNullOrEmpty(oldParentLineage))
        //            {
        //                newLineage = newParentLineage + category.CategoryID + ",";
        //            }
        //            else
        //            {
        //                newLineage = nodeChild.Lineage.Replace(oldParentLineage, newParentLineage);
        //            }
        //            int newDepth = newLineage.Count(c => c == ',');
        //            await repo.AssignLineAgeAndDepthToCategory(nodeChild.CategoryID, newLineage, newDepth);
        //        }
        //    }
           
        //    return await repo.UpdateNewCategory(category);
        //}



        //my application methods:

        public async Task<OperationResult> AddNewCategory(CategoryAddModel category)
        {
            OperationResult op = new OperationResult("AddNewCategory");

            //check valid categoryName
            if (await repo.ExistCategoryName(category.CategoryName))
            {
                return op.ToFail("CatgoryName Already Exist", System.Net.HttpStatusCode.Conflict);
            }

            //check valid depth
            if (category.ParentID != null)
            {
                CategoryUpdateModel parent = await repo.GetCategory(category.ParentID.Value);
                if (parent.Depth == 3)
                {
                    return new OperationResult("AddNewCategory").ToFail("this tree can hav maximum 3 depth", System.Net.HttpStatusCode.Forbidden);
                }
            }

            //add new category
            var AddResult = await repo.AddNewCategory(category);

            return AddResult;
        }

        public async Task<OperationResult> UpdateNewCategory(CategoryUpdateModel category)
        {
            OperationResult op = new OperationResult("UpdateNewCategory");

            var OldNode = await repo.GetCategory(category.CategoryID);
            List<int> childrenIDs = await repo.GetChildren(category.CategoryID);

            CategoryUpdateModel NewParentNode = null;

            //check if category name already exists
            if (await repo.AsignedCurrentCategoryNameToAnotherCategory(category.CategoryID, category.CategoryName))
            {
                return op.ToFail("CategoryName Exist",category.CategoryID);
            }


            //check if ParentId has been changed
            if (OldNode.ParentID != category.ParentID) 
            {
                NewParentNode = await repo.GetCategory(category.ParentID ?? 0);
                int MaxChildrenRelativeDepth = await repo.GetChildNodeLevels(category.CategoryID);

                //check if depth was more than 3
                if (MaxChildrenRelativeDepth + NewParentNode.Depth > 3)
                {
                    return op.ToFail("invalid Depth", category.CategoryID);
                }

                //check if new parentNode.Id exists in childrens arrey
                if (childrenIDs.Count>0 && childrenIDs.Contains(NewParentNode.CategoryID))
                {
                    return op.ToFail("invalid parent id", category.CategoryID);
                }

                //check if ParentId is null and the category must be root
                //if (category.ParentID == null)
                //{
                //    finalLineage = await repo.GetLineageString(category.CategoryID);
                //    finalDepth = finalLineage.Count(c => c == ',');
                //}

            }
            
            var updateNewNode = await repo.UpdateNewCategory(category);

            //set new lineage and depth for childrens
            if (updateNewNode.Success)
            {
                foreach (var childID in childrenIDs)
                {
                    await repo.AssignLineAgeAndDepthToCategory(childID);
                }
            }

            return updateNewNode;
   
        }
    }
}
