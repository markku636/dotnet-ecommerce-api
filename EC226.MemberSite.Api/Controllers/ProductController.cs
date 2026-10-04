using System.Collections.Generic;
using System.Threading.Tasks;
using EC226.Pagination;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.Api.Controllers
{
   [Route("product")]
   public class ProductController: BaseController
   {
      private readonly IProductManagementService _productManagementService;

      public ProductController(IProductManagementService productManagementService)
      {
         _productManagementService = productManagementService;
      }

      [Route("list")]
      [HttpGet]
      public async Task<Result<PageModel<ProductVM>>> GetListAsync([FromQuery] PageModel model)
      {
         return await _productManagementService.GetWebPortalProductListResultAsync(model);
      }

      [Route("detail")]
      [HttpGet]
      public async Task<Result<ProductDetailVM>> GetDetailAsync(int spuId)
      {
         return await _productManagementService.GetWebPortalProductDetailResultAsync(spuId);
      }
   }
}
