
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC226.Loging;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Pagination;
using EC226.Utility.Library.Extensions;
using Microsoft.EntityFrameworkCore;


namespace EC226.MemberSite.Services
{
   public class ArticleService: BaseServices<Article>, IArticleService
   {

      private readonly IArticleRepository _dal;

      private readonly IUnitOfWork _unitOfWork;
      public ArticleService(IUnitOfWork unitOfWork, IArticleRepository dal)
      {

         this._dal = dal;
         base.BaseDal = dal;
         _unitOfWork = unitOfWork;
      }


      public async Task<Result<PageModel<ArticleListVM>>> PagedListResultAsync(PageModel model)
      {
         Result<PageModel<ArticleListVM>> result = new Result<PageModel<ArticleListVM>>();

         try
         {
            // 調用非同步的 PagedListAsync 方法
            result.Data = await PagedListAsync(model);

            result.Success();
         }
         catch (Exception ex)
         {
            // 記錄異常日誌
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }


      public async Task<PageModel<ArticleListVM>> PagedListAsync(PageModel model)
      {
         List<ArticleListVM> articleViewVms = new List<ArticleListVM>();

         // 使用非同步的查詢方法
         var query = this._dal.Queryable() // 要改別的方法擴充page 
             .Where(model.QueryModel)
             .Where(x => x.Enable && !x.IsDelete)
             .OrderByDescending(z => z.CreateTime)
             .OrderBy(model.Order);

         // 使用非同步方法計算總數
         var count = await query.CountAsync();

         // 使用非同步方法進行分頁查詢
         var articles = await query.Skip((model.PageIndex - 1) * model.PageSize)
                                   .Take(model.PageSize)
                                   .ToListAsync();

         // 映射數據
         AutoMapperInstance.ConvertModel(articles, articleViewVms);

         return model.Record<ArticleListVM>(articleViewVms, count)
             .SetPrimaryKey(x => x.Id)
             .ColumFor(x => x.Id)
             .ColumFor(x => x.Title)
             .ColumFor(x => x.CreateTime);
      }

      public async Task<Result> CreateAsync(ArticleVM vm)
      {
         Result result = new Result();

         try
         {
            // 建立新資料實例
            Article newData = new Article
            {
               Title = vm.Title,
               Content = vm.Content,
               CreateTime = DateTime.UtcNow,
               IsDelete = vm.IsDelete,
               CoverUrl = vm.CoverUrl,
               Author = vm.Author,
               Enable = vm.Enable,
               Hit = vm.Hit,
               IsHot = vm.IsHot
            };

            result = await this._dal.InsertAsync(newData);

         }
         catch (Exception ex)
         {
            // 記錄異常日誌
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }


      public async Task<Result<ArticleVM>> GetByIdAsync(int id)
      {
         Result<ArticleVM> result = new Result<ArticleVM>();

         try
         {
            // 非同步查詢文章資料
            var article = await this._dal.Queryable().FirstAsync(x => x.Id == id && x.Enable && !x.IsDelete);
            //.FirstOrDefaultAsync(x => x.Id == id && x.Enable && !x.IsDelete);

            if (article == null)
            {
               // 如果找不到文章，返回失敗結果
               result.Fail("Article not found.");
               return result;
            }

            // 更新點擊次數
            article.Hit += 1;
            await this._dal.UpdateAsync(article); // 假設存在非同步更新方法

            // 映射數據到 ViewModel
            result.Data = new ArticleVM();
            AutoMapperInstance.ConvertModel(article, result.Data);

            result.Success();
         }
         catch (Exception ex)
         {
            // 記錄異常日誌
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result> UpdateAsync(ArticleVM vm)
      {
         var result = new Result();

         try
         {
            // 使用非同步方式查詢文章
            var article = await this._dal.Queryable().FirstAsync(x => !x.IsDelete && x.Id == vm.Id);

            if (article != null)
            {
               // 更新文章屬性
               article.Title = vm.Title;
               article.Content = vm.Content;
               article.ModifyTime = vm.ModifyTime;
               article.IsDelete = vm.IsDelete;
               article.CoverUrl = vm.CoverUrl;
               article.Enable = vm.Enable;
               article.Hit = vm.Hit;
               article.IsHot = vm.IsHot;
               article.Author = vm.Author;

               // 非同步更新數據庫
               await this.UpdateAsync(article);

               // 設定操作成功結果
               result.Success();
            }

         }
         catch (Exception ex)
         {
            // 記錄錯誤日誌
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      /// <summary>
      /// 刪除帳號
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public async Task<Result> DeleteAsync(int id)
      {
         var result = new Result();

         try
         {
            // 非同步獲取文章數據
            var irArticle = await this._dal.Queryable().FirstAsync(x => x.Id == id && !x.IsDelete);


            if (irArticle != null)
            {
               // 更新文章為已刪除狀態
               irArticle.IsDelete = true;
               irArticle.ModifyTime = DateTime.UtcNow;

               // 非同步保存更改
               await this._dal.UpdateAsync(irArticle);

               result.Success();
            }

         }
         catch (Exception ex)
         {
            // 記錄錯誤日誌
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);

         }

         return result;
      }
   }
}
