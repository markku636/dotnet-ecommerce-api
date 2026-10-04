using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels.Basics;
using EC226.MemberSite.Model.ViewModels.UI;
using SqlSugar;


namespace EC226.MemberSite.IRepository
{
	/// <summary>
    /// ArticleCategoryRelation 工廠介面
    /// </summary>
    public interface IArticleCategoryRelationRepository : IBaseRepository<ArticleCategoryRelation>
    {
        #region 重寫增刪改查操作===========================================================

        /// <summary>
        /// 重寫非同步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<Result> InsertAsync(ArticleCategoryRelation entity);


        /// <summary>
        /// 重寫非同步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<Result> UpdateAsync(ArticleCategoryRelation entity);


        /// <summary>
        /// 重寫非同步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<Result> UpdateAsync(List<ArticleCategoryRelation> entity);


        /// <summary>
        /// 重寫刪除指定ID的數據
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new Task<Result> DeleteByIdAsync(object id);


        /// <summary>
        /// 重寫刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        new Task<Result> DeleteByIdsAsync(int[] ids);

        #endregion

        #region 取得快取的所有數據==========================================================

        /// <summary>
        /// 取得快取的所有數據
        /// </summary>
        /// <returns></returns>
        Task<List<ArticleCategoryRelation>> GetCaChe();

        /// <summary>
        ///     更新cache
        /// </summary>
        Task<List<ArticleCategoryRelation>> UpdateCaChe();

        #endregion


        /// <summary>
        ///     重寫根據條件查詢分頁數據
        /// </summary>
        /// <param name="predicate">判斷集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">目前頁面索引</param>
        /// <param name="pageSize">分佈大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        new Task<PageList<ArticleCategoryRelation>> QueryPageAsync(
            Expression<Func<ArticleCategoryRelation, bool>> predicate,
            Expression<Func<ArticleCategoryRelation, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

    }
}
