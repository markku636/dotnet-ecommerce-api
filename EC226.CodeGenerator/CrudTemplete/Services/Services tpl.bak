using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EC226.Configuration;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels.Basics;
using EC226.MemberSite.Model.ViewModels.UI;
using SqlSugar;


namespace EC226.MemberSite.Services
{
    /// <summary>
    /// {{ModelDescription}} 介面實現
    /// </summary>
    public class {{ModelClassName}}Services : BaseServices<{{ModelClassName}}>, I{{ModelClassName}}Services
    {
        private readonly I{{ModelClassName}}Repository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public {{ModelClassName}}Services(IUnitOfWork unitOfWork, I{{ModelClassName}}Repository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        #region 實現重寫增刪改查操作==========================================================

        /// <summary>
        /// 重寫非同步插入方法
        /// </summary>
        /// <param name="entity">實體數據</param>
        /// <returns></returns>
        public new async Task<Result> InsertAsync({{ModelClassName}} entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        /// 重寫非同步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<Result> UpdateAsync({{ModelClassName}} entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重寫非同步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<Result> UpdateAsync(List<{{ModelClassName}}> entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重寫刪除指定ID的數據
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<Result> DeleteByIdAsync(object id)
        {
            return await _dal.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 重寫刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<Result> DeleteByIdsAsync(int[] ids)
        {
            return await _dal.DeleteByIdsAsync(ids);
        }

        #endregion

        #region 取得快取的所有數據==========================================================

        /// <summary>
        /// 取得快取的所有數據
        /// </summary>
        /// <returns></returns>
        public async Task<List<{{ModelClassName}}>> GetCaChe()
        {
            return await _dal.GetCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<{{ModelClassName}}>> UpdateCaChe()
        {
            return await _dal.UpdateCaChe();
        }

        #endregion

       #region 重寫根據條件查詢分頁數據
        /// <summary>
        ///     重寫根據條件查詢分頁數據
        /// </summary>
        /// <param name="predicate">判斷集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">目前頁面索引</param>
        /// <param name="pageSize">分佈大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="isSuccessUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<PageList<{{ModelClassName}}>> QueryPageAsync(Expression<Func<{{ModelClassName}}, bool>> predicate,
            Expression<Func<{{ModelClassName}}, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool isSuccessUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, isSuccessUseNoLock);
        }
        #endregion

    }
}
