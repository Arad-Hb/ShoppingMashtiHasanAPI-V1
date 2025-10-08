using ApplicationServiceContract.Services;
using Catalogue.Domain.Category;
using Catalogue.Domain.Product;
using Framework.Domain.BaseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingMashtiHasan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryManagementController : ControllerBase
    {
        private readonly ICategoryApplicationServiceContract catApp;
        public CategoryManagementController(ICategoryApplicationServiceContract catApp)
        {
            this.catApp = catApp;
        }
        [HttpPost("AddNewCategory")]

        public async Task<IActionResult> AddNewCategory(CategoryAddModel cat)
        {
            var op = await catApp.AddNewCategory(cat);
            if (op.Success)
            {
                return StatusCode(StatusCodes.Status201Created, op);
            }
            else
            {
                return BadRequest(op);
            }
        }
        [HttpPost("AssignFeatureToCategory")]
        public async Task<IActionResult> AssignFeatureToCategory(int CategoryID, int FeatureID)
        {
            var op = await catApp.AssignFeatureToCategory(CategoryID, FeatureID);
            if (op.Success)
            {
                return StatusCode(StatusCodes.Status201Created, op);
            }
            else
            {
                return BadRequest(op);
            }
        }
        [HttpDelete("RemoveCategory")]
        //[HttpDelete()]
        public async Task<IActionResult> DeleteCategory(int CategoryID)
        {
            var op = await catApp.DeleteCategory(CategoryID);
            if (op.Success)
            {
                return Ok(op);
            }
            else
            {
                return BadRequest(op);
            }
        }


        [HttpGet("GetCategory/{CategoryID}")]
        public async Task<IActionResult> GetCategory(int CategoryID)
        {
            var cat = await catApp.GetCategory(CategoryID);
            return Ok(cat);
        }

        [HttpGet("SearchCategory")]
        public async Task<IActionResult> SearchCategory([FromQuery] CategorySearchModel sm)
        {
            var cat = await catApp.Search(sm);
            return Ok(cat);
        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateModel cat)
        {
            try
            {
                var op = await catApp.UpdateNewCategory(cat);
                if (!op.Success)
                {
                    return BadRequest(op);
                }
                else
                {
                    return Ok(op);
                }
            }
            catch
            {
                //TODO Log Error                
                var op = new OperationResult("UpdateCategory").ToFail("خطایی روی داد با پشتیبانی تماس بگیرید", cat.CategoryID);
                return BadRequest(op);
            }
        }
    }
}