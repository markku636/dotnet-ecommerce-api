using System.Threading.Tasks;
using EC226.MemberSite.Model.ViewModels;
using EC226.Pagination;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.MemberSite.IServices
{
   public interface IArticleService
   {
      /// <summary>
      /// 取得分頁結果（非同步）
      /// </summary>
      /// <param name="model">分頁參數</param>
      /// <returns>分頁結果</returns>
      Task<Result<PageModel<ArticleListVM>>> PagedListResultAsync(PageModel model);

      /// <summary>
      /// 取得分頁（非同步）
      /// </summary>
      /// <param name="model">分頁參數</param>
      /// <returns>分頁資料</returns>
      Task<PageModel<ArticleListVM>> PagedListAsync(PageModel model);

      /// <summary>
      /// 根據 ID 取得詳細內容（非同步）
      /// </summary>
      /// <param name="id">文章 ID</param>
      /// <returns>文章詳細資訊</returns>
      Task<Result<ArticleVM>> GetByIdAsync(int id);

      /// <summary>
      /// 建立文章（非同步）
      /// </summary>
      /// <param name="vm">文章模型</param>
      /// <returns>操作結果</returns>
      Task<Result> CreateAsync(ArticleVM vm);

      /// <summary>
      /// 更新文章（非同步）
      /// </summary>
      /// <param name="vm">文章模型</param>
      /// <returns>操作結果</returns>
      Task<Result> UpdateAsync(ArticleVM vm);

      /// <summary>
      /// 刪除文章（非同步）
      /// </summary>
      /// <param name="id">文章 ID</param>
      /// <returns>操作結果</returns>
      Task<Result> DeleteAsync(int id);
   }
}
