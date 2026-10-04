
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EC226.MemberSite.Model.ViewModels.Basics;
using SqlSugar;

namespace EC226.MemberSite.IServices
{
    /// <summary>
    ///     服務倉儲通用介面類
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseServices<T> where T : class
    {
        /// <summary>
        ///     根據主值查詢單條數據
        /// </summary>
        /// <param name="pkValue">主鍵值</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型實體</returns>
        T QueryById(object pkValue, bool blUseNoLock = false);

        /// <summary>
        ///     根據主值查詢單條數據
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<T> QueryByIdAsync(object objId, bool blUseNoLock = false);

        /// <summary>
        ///     根據主值列表查詢單條數據
        /// </summary>
        /// <param name="lstIds"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> QueryByIDs(object[] lstIds, bool blUseNoLock = false);

        /// <summary>
        ///     根據主值列表查詢單條數據
        /// </summary>
        /// <param name="lstIds"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryByIDsAsync(object[] lstIds, bool blUseNoLock = false);

        /// <summary>
        ///     根據主值列表查詢單條數據
        /// </summary>
        /// <param name="lstIds"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> QueryByIDs(int[] lstIds, bool blUseNoLock = false);

        /// <summary>
        ///     根據主值列表查詢單條數據
        /// </summary>
        /// <param name="lstIds"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryByIDsAsync(int[] lstIds, bool blUseNoLock = false);

        /// <summary>
        ///     查詢所有數據(無分頁,請慎用)
        /// </summary>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> Query(bool blUseNoLock = false);

        /// <summary>
        ///     查詢所有數據(無分頁,請慎用)
        /// </summary>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryAsync(bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="strWhere">條件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型實體集合</returns>
        List<T> QueryListByClause(string strWhere, string orderBy = "", bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="strWhere">條件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型實體集合</returns>
        Task<List<T>> QueryListByClauseAsync(string strWhere, string orderBy = "", bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="orderBy">排序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型實體集合</returns>
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, string orderBy = "", bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="orderBy">排序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型實體集合</returns>
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, string orderBy = "",
            bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="orderByPredicate">排序欄位</param>
        /// <param name="orderByType">排序順序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型實體集合</returns>
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate,
            OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="orderByPredicate">排序欄位</param>
        /// <param name="orderByType">排序順序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型實體集合</returns>
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢一定數量數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="take">取得數量</param>
        /// <param name="orderByPredicate">排序欄位</param>
        /// <param name="orderByType">排序順序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢一定數量數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="take">取得數量</param>
        /// <param name="orderByPredicate">排序欄位</param>
        /// <param name="orderByType">排序順序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢一定數量數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="take">取得數量</param>
        /// <param name="strOrderByFileds">排序欄位，如name asc,age desc</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take, string strOrderByFileds = "",
            bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢一定數量數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="take">取得數量</param>
        /// <param name="strOrderByFileds">排序欄位，如name asc,age desc</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take,
            string strOrderByFileds = "", bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        T QueryByClause(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="orderByPredicate">排序欄位</param>
        /// <param name="orderByType">排序順序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        T QueryByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate,
            OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢數據
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="orderByPredicate">排序欄位</param>
        /// <param name="orderByType">排序順序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate,
            OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     寫入實體數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        int Insert(T entity);

        /// <summary>
        ///     寫入實體數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        Task<int> InsertAsync(T entity);

        /// <summary>
        ///     寫入實體數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        int Insert(T entity, Expression<Func<T, object>> insertColumns = null);

        /// <summary>
        ///     寫入實體數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        Task<int> InsertAsync(T entity, Expression<Func<T, object>> insertColumns = null);

        /// <summary>
        ///     寫入實體數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        bool InsertGuid(T entity, Expression<Func<T, object>> insertColumns = null);

        /// <summary>
        ///     寫入實體數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        Task<bool> InsertGuidAsync(T entity, Expression<Func<T, object>> insertColumns = null);

        /// <summary>
        ///     批量寫入實體數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        int Insert(List<T> entity);

        /// <summary>
        ///     批量寫入實體數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        Task<int> InsertAsync(List<T> entity);

        /// <summary>
        ///     批量寫入實體數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        Task<int> InsertCommandAsync(List<T> entity);

        /// <summary>
        ///     批量更新實體數據
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(List<T> entity);

        /// <summary>
        ///     批量更新實體數據
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<T> entity);

        /// <summary>
        ///     更新實體數據
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        ///     更新實體數據
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        ///     根據手寫條件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        bool Update(T entity, string strWhere);

        /// <summary>
        ///     根據手寫條件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity, string strWhere);

        /// <summary>
        ///     根據手寫sql語句更新數據
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool Update(string strSql, SugarParameter[] parameters = null);

        /// <summary>
        ///     根據手寫sql語句更新數據
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null);

        /// <summary>
        ///     更新某個欄位
        /// </summary>
        /// <param name="columns">lamdba表達式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
        /// <param name="where">lamdba判斷</param>
        /// <returns></returns>
        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where);

        /// <summary>
        ///     更新某個欄位
        /// </summary>
        /// <param name="columns">lamdba表達式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
        /// <param name="where">lamdba判斷</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where);

        /// <summary>
        ///     根據條件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns"></param>
        /// <param name="lstIgnoreColumns"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null,
            string strWhere = "");

        /// <summary>
        ///     根據條件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns"></param>
        /// <param name="lstIgnoreColumns"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        bool Update(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null,
            string strWhere = "");

        /// <summary>
        ///     刪除數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        bool Delete(T entity);

        /// <summary>
        ///     刪除數據
        /// </summary>
        /// <param name="entity">實體類</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        ///     刪除數據
        /// </summary>
        /// <param name="entity">實體類集合</param>
        /// <returns></returns>
        bool Delete(IEnumerable<T> entity);

        /// <summary>
        ///     刪除數據
        /// </summary>
        /// <param name="entity">實體類集合</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IEnumerable<T> entity);

        /// <summary>
        ///     刪除數據
        /// </summary>
        /// <param name="where">過濾條件</param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> where);

        /// <summary>
        ///     刪除數據
        /// </summary>
        /// <param name="where">過濾條件</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Expression<Func<T, bool>> where);

        /// <summary>
        ///     刪除指定ID的數據
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteById(object id);

        /// <summary>
        ///     刪除指定ID的數據
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(object id);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(int[] ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(int[] ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(long[] ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(long[] ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(Guid[] ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(Guid[] ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(string[] ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(string[] ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(List<int> ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(List<int> ids);


        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(List<string> ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(List<string> ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(List<Guid> ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(List<Guid> ids);


        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(List<long> ids);

        /// <summary>
        ///     刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(List<long> ids);

        /// <summary>
        ///     判斷數據是否存在
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        bool Exists(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     判斷數據是否存在
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     取得數據總數
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        int GetCount(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     取得數據總數
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     取得數據某個欄位的合計
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="field">欄位</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        int GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field, bool blUseNoLock = false);

        /// <summary>
        ///     取得數據某個欄位的合計
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="field">欄位</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<int> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field,
            bool blUseNoLock = false);

        /// <summary>
        ///     取得數據某個欄位的合計
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="field">欄位</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        decimal GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
            bool blUseNoLock = false);

        /// <summary>
        ///     取得數據某個欄位的合計
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="field">欄位</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<decimal> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
            bool blUseNoLock = false);

        /// <summary>
        ///     取得數據某個欄位的合計
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="field">欄位</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        float GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field, bool blUseNoLock = false);

        /// <summary>
        ///     取得數據某個欄位的合計
        /// </summary>
        /// <param name="predicate">條件表達式樹</param>
        /// <param name="field">欄位</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<float> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field,
            bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢分頁數據
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">目前頁面索引</param>
        /// <param name="pageSize">分佈大小</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        IPageList<T> QueryPage(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢分頁數據
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">目前頁面索引</param>
        /// <param name="pageSize">分佈大小</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢分頁數據
        /// </summary>
        /// <param name="predicate">判斷集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">目前頁面索引</param>
        /// <param name="pageSize">分佈大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        IPageList<T> QueryPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByExpression,
            OrderByType orderByType, int pageIndex = 1, int pageSize = 20, bool blUseNoLock = false);

        /// <summary>
        ///     根據條件查詢分頁數據
        /// </summary>
        /// <param name="predicate">判斷集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">目前頁面索引</param>
        /// <param name="pageSize">分佈大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        /// <summary>
        ///     查詢-多表查詢
        /// </summary>
        /// <typeparam name="T1">實體1</typeparam>
        /// <typeparam name="T2">實體2</typeparam>
        /// <typeparam name="TResult">返回對像</typeparam>
        /// <param name="joinExpression">關聯表達式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表達式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查詢表達式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        List<TResult> QueryMuch<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查詢-多表查詢
        /// </summary>
        /// <typeparam name="T1">實體1</typeparam>
        /// <typeparam name="T2">實體2</typeparam>
        /// <typeparam name="TResult">返回對像</typeparam>
        /// <param name="joinExpression">關聯表達式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表達式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查詢表達式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        Task<List<TResult>> QueryMuchAsync<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查詢-多表查詢
        /// </summary>
        /// <typeparam name="T1">實體1</typeparam>
        /// <typeparam name="T2">實體2</typeparam>
        /// <typeparam name="TResult">返回對像</typeparam>
        /// <param name="joinExpression">關聯表達式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表達式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查詢表達式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        TResult QueryMuchFirst<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查詢-多表查詢
        /// </summary>
        /// <typeparam name="T1">實體1</typeparam>
        /// <typeparam name="T2">實體2</typeparam>
        /// <typeparam name="TResult">返回對像</typeparam>
        /// <param name="joinExpression">關聯表達式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表達式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查詢表達式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        Task<TResult> QueryMuchFirstAsync<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查詢-三表查詢
        /// </summary>
        /// <typeparam name="T">實體1</typeparam>
        /// <typeparam name="T2">實體2</typeparam>
        /// <typeparam name="T3">實體3</typeparam>
        /// <typeparam name="TResult">返回對像</typeparam>
        /// <param name="joinExpression">關聯表達式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表達式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查詢表達式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        List<TResult> QueryMuch<T1, T2, T3, TResult>(
            Expression<Func<T1, T2, T3, object[]>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expression<Func<T1, T2, T3, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查詢-三表查詢
        /// </summary>
        /// <typeparam name="T">實體1</typeparam>
        /// <typeparam name="T2">實體2</typeparam>
        /// <typeparam name="T3">實體3</typeparam>
        /// <typeparam name="TResult">返回對像</typeparam>
        /// <param name="joinExpression">關聯表達式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表達式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查詢表達式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        Task<List<TResult>> QueryMuchAsync<T1, T2, T3, TResult>(
            Expression<Func<T1, T2, T3, object[]>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expression<Func<T1, T2, T3, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     執行sql語句並返回List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<T> SqlQuery(string sql, List<SugarParameter> parameters);

        /// <summary>
        ///     執行sql語句並返回List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<T>> SqlQueryable(string sql);
    }
}
