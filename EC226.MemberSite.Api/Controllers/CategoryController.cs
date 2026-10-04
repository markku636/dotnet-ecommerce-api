using System.Collections.Generic;
using System.Threading.Tasks;
using EC226.Pagination;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.Api.Controllers
{
   [Route("category")]
   public class CategoryController: BaseController
   {
      private readonly ICategoryService _categoryService;

      public CategoryController(ICategoryService categoryService)
      {
         _categoryService = categoryService;
      }

      [Route("all")]
      [HttpGet]
      public async Task<Result<List<CategoryVM>>> GetCategoryAsync()
      {
         return await _categoryService.GetCategoryResultByNameAsync("");
      }
   }
}
