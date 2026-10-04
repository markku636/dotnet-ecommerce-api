using EC226.Pagination;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EC226.MemberSite.IServices
{
   public interface ICategoryService
   {
      Task<Result<List<CategoryVM>>> GetCategoryResultByNameAsync(string searchCategoryName);

      Task<Result<List<CategoryVM>>> GetCategoryResultByParentIdAsync(int? parentId = null);

      Task<Result<PageModel<CategoryVM>>> PagedListResultAsync(PageModel model);

      Task<Result> CreateAsync(CategoryVM vm);

      Task<Result> UpdateAsync(CategoryVM vm);

      Task<Result<CategoryVM>> GetDetailByIdAsync(int id);

      Task<Result<List<ElementUICascaderVM>>> GetElementUICascaderResultAsync();
   }
}
