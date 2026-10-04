using System.Collections.Generic;
using System.Threading.Tasks;
using EC226.Pagination;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;

namespace EC226.MemberSite.Api.Controllers
{
   [Route("article")]
   public class ArticleController: BaseController
   {
      private readonly IArticleService _articleService;

      public ArticleController(IArticleService articleService)
      {
         _articleService = articleService;
      }

      [Route("list")]
      [HttpGet]
      public async Task<Result<PageModel<ArticleListVM>>> GetListAsync(PageModel model)
      {
         // 調用非同步方法
         return await _articleService.PagedListResultAsync(model);
      }

      [Route("detail")]
      [HttpGet]
      public async Task<Result<ArticleVM>> GetDetailAsync(int id)
      {
         // 調用非同步方法
         return await _articleService.GetByIdAsync(id);
      }
   }
}
