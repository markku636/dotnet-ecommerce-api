using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EC226.Caching.Manual;
using EC226.Core.Config;
using EC226.Loging;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.Const;
using EC226.MemberSite.Model.ViewModels.Basics;
using SqlSugar;

namespace EC226.MemberSite.Services
{
   public class BaseServices<T>: IBaseServices<T> where T : class, new()
   {
      public async Task<T> GetDataFromCacheAsync<T>(string key, Func<Task<T>> loadDataFuncAsync, int cacheMinutesTime = 300, bool forceUpdate = false)
      {
         T result = default;

         try
         {
            if (forceUpdate)
            {
               await ManualDataCache.Instance.RemoveAsync(key);
            }

            if (await ManualDataCache.Instance.ExistsAsync(key))
            {
               return await ManualDataCache.Instance.GetAsync<T>(key);
            }

            result = await loadDataFuncAsync();

            if (result != null)
            {
               await ManualDataCache.Instance.SetAsync(key, result, cacheMinutesTime);
            }
         }
         catch (Exception ex)
         {
            // Log error asynchronously if your logging framework supports it, otherwise consider logging synchronously
            NLogUtil.WriteSEQLog($"[Cache]{ex.Message}", NLog.LogLevel.Error, ex);
            result = await loadDataFuncAsync(); // Safely retry loading data asynchronously
         }

         return result;
      }

      //public IBaseRepository<TEntity> baseDal = new BaseRepository<TEntity>();
      public IBaseRepository<T> BaseDal; //通過在子類的建構函式中注入，這裡是基類，不用建構函式

      /// <summary>
      ///     根據主值查詢單條數據
      /// </summary>
      /// <param name="pkValue">主鍵值</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>泛型實體</returns>
      public T QueryById(object pkValue, bool blUseNoLock = false)
      {
         return BaseDal.QueryById(pkValue, blUseNoLock);
      }

      /// <summary>
      ///     根據主值查詢單條數據
      /// </summary>
      /// <param name="objId">id（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>數據實體</returns>
      public async Task<T> QueryByIdAsync(object objId, bool blUseNoLock = false)
      {
         return await BaseDal.QueryByIdAsync(objId, blUseNoLock);
      }

      /// <summary>
      ///     根據主值列表查詢單條數據
      /// </summary>
      /// <param name="lstIds">id列表（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>數據實體列表</returns>
      public List<T> QueryByIDs(object[] lstIds, bool blUseNoLock = false)
      {
         return BaseDal.QueryByIDs(lstIds, blUseNoLock);
      }

      /// <summary>
      ///     根據主值列表查詢單條數據
      /// </summary>
      /// <param name="lstIds">id列表（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>數據實體列表</returns>
      public async Task<List<T>> QueryByIDsAsync(object[] lstIds, bool blUseNoLock = false)
      {
         return await BaseDal.QueryByIDsAsync(lstIds, blUseNoLock);
      }

      /// <summary>
      ///     根據主值列表查詢單條數據
      /// </summary>
      /// <param name="lstIds">id列表（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>數據實體列表</returns>
      public List<T> QueryByIDs(int[] lstIds, bool blUseNoLock = false)
      {
         return BaseDal.QueryByIDs(lstIds, blUseNoLock);
      }

      /// <summary>
      ///     根據主值列表查詢單條數據
      /// </summary>
      /// <param name="lstIds">id列表（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>數據實體列表</returns>
      public async Task<List<T>> QueryByIDsAsync(int[] lstIds, bool blUseNoLock = false)
      {
         return await BaseDal.QueryByIDsAsync(lstIds, blUseNoLock);
      }

      /// <summary>
      ///     查詢表單所有數據(無分頁,請慎用)
      /// </summary>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public List<T> Query(bool blUseNoLock = false)
      {
         var list = BaseDal.Query(blUseNoLock);
         return list;
      }

      /// <summary>
      ///     查詢表單所有數據(無分頁,請慎用)
      /// </summary>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<List<T>> QueryAsync(bool blUseNoLock = false)
      {
         return await BaseDal.QueryAsync(blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="strWhere">條件</param>
      /// <param name="orderBy">排序欄位，如name asc,age desc</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>泛型實體集合</returns>
      public List<T> QueryListByClause(string strWhere, string orderBy = "", bool blUseNoLock = false)
      {
         return BaseDal.QueryListByClause(strWhere, orderBy, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="strWhere">條件</param>
      /// <param name="orderBy">排序欄位，如name asc,age desc</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>泛型實體集合</returns>
      public async Task<List<T>> QueryListByClauseAsync(string strWhere, string orderBy = "",
          bool blUseNoLock = false)
      {
         return await BaseDal.QueryListByClauseAsync(strWhere, orderBy, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="orderBy">排序欄位，如name asc,age desc</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>泛型實體集合</returns>
      public List<T> QueryListByClause(Expression<Func<T, bool>> predicate, string orderBy = "",
          bool blUseNoLock = false)
      {
         return BaseDal.QueryListByClause(predicate, orderBy, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="orderBy">排序欄位，如name asc,age desc</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>泛型實體集合</returns>
      public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, string orderBy = "",
          bool blUseNoLock = false)
      {
         return await BaseDal.QueryListByClauseAsync(predicate, orderBy, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="orderByPredicate">排序欄位</param>
      /// <param name="orderByType">排序順序</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>泛型實體集合</returns>
      public List<T> QueryListByClause(Expression<Func<T, bool>> predicate,
          Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
      {
         return BaseDal.QueryListByClause(predicate, orderByPredicate, orderByType, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="orderByPredicate">排序欄位</param>
      /// <param name="orderByType">排序順序</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>泛型實體集合</returns>
      public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate,
          Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
      {
         return await BaseDal.QueryListByClauseAsync(predicate, orderByPredicate, orderByType, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢一定數量數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="take">取得數量</param>
      /// <param name="orderByPredicate">排序欄位</param>
      /// <param name="orderByType">排序順序</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take,
          Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
      {
         return BaseDal.QueryListByClause(predicate, take, orderByPredicate, orderByType, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢一定數量數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="take">取得數量</param>
      /// <param name="orderByPredicate">排序欄位</param>
      /// <param name="orderByType">排序順序</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take,
          Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
      {
         return await BaseDal.QueryListByClauseAsync(predicate, take, orderByPredicate, orderByType, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢一定數量數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="take">取得數量</param>
      /// <param name="strOrderByFileds">排序欄位，如name asc,age desc</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take, string strOrderByFileds = "",
          bool blUseNoLock = false)
      {
         return BaseDal.QueryListByClause(predicate, take, strOrderByFileds, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢一定數量數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="take">取得數量</param>
      /// <param name="strOrderByFileds">排序欄位，如name asc,age desc</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take,
          string strOrderByFileds = "", bool blUseNoLock = false)
      {
         return await BaseDal.QueryListByClauseAsync(predicate, take, strOrderByFileds, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public T QueryByClause(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return BaseDal.QueryByClause(predicate, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return await BaseDal.QueryByClauseAsync(predicate, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="orderByPredicate">排序欄位</param>
      /// <param name="orderByType">排序順序</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public T QueryByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate,
          OrderByType orderByType, bool blUseNoLock = false)
      {
         var entity = BaseDal.QueryByClause(predicate, orderByPredicate, orderByType, blUseNoLock);
         return entity;
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="orderByPredicate">排序欄位</param>
      /// <param name="orderByType">排序順序</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate,
          Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
      {
         return await BaseDal.QueryByClauseAsync(predicate, orderByPredicate, orderByType, blUseNoLock);
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體數據</param>
      /// <returns></returns>
      public int Insert(T entity)
      {
         return BaseDal.Insert(entity);
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體數據</param>
      /// <returns></returns>
      public async Task<int> InsertAsync(T entity)
      {
         return await BaseDal.InsertAsync(entity);
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體數據</param>
      /// <param name="insertColumns">插入的列</param>
      /// <returns></returns>
      public int Insert(T entity, Expression<Func<T, object>> insertColumns = null)
      {
         return BaseDal.Insert(entity, insertColumns);
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體數據</param>
      /// <param name="insertColumns">插入的列</param>
      /// <returns></returns>
      public async Task<int> InsertAsync(T entity, Expression<Func<T, object>> insertColumns = null)
      {
         return await BaseDal.InsertAsync(entity, insertColumns);
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <param name="insertColumns">需插入的欄位</param>
      /// <returns></returns>
      public bool InsertGuid(T entity, Expression<Func<T, object>> insertColumns = null)
      {
         return BaseDal.InsertGuid(entity, insertColumns);
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <param name="insertColumns">需插入的欄位</param>
      /// <returns></returns>
      public async Task<bool> InsertGuidAsync(T entity, Expression<Func<T, object>> insertColumns = null)
      {
         return await InsertGuidAsync(entity, insertColumns);
      }

      /// <summary>
      ///     批量寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public int Insert(List<T> entity)
      {
         return BaseDal.Insert(entity);
      }

      /// <summary>
      ///     批量寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public async Task<int> InsertAsync(List<T> entity)
      {
         return await BaseDal.InsertAsync(entity);
      }

      /// <summary>
      ///     批量寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public async Task<int> InsertCommandAsync(List<T> entity)
      {
         return await BaseDal.InsertCommandAsync(entity);
      }

      /// <summary>
      ///     批量更新實體數據
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
      public bool Update(List<T> entity)
      {
         return BaseDal.Update(entity);
      }

      /// <summary>
      ///     批量更新實體數據
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(List<T> entity)
      {
         return await BaseDal.UpdateAsync(entity);
      }

      /// <summary>
      ///     更新實體數據
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
      public bool Update(T entity)
      {
         return BaseDal.Update(entity);
      }

      /// <summary>
      ///     更新實體數據
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(T entity)
      {
         return await BaseDal.UpdateAsync(entity);
      }

      /// <summary>
      ///     根據手寫條件更新
      /// </summary>
      /// <param name="entity"></param>
      /// <param name="strWhere"></param>
      /// <returns></returns>
      public bool Update(T entity, string strWhere)
      {
         return BaseDal.Update(entity, strWhere);
      }

      /// <summary>
      ///     根據手寫條件更新
      /// </summary>
      /// <param name="entity"></param>
      /// <param name="strWhere"></param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(T entity, string strWhere)
      {
         return await BaseDal.UpdateAsync(entity, strWhere);
      }

      /// <summary>
      ///     根據手寫sql語句更新數據
      /// </summary>
      /// <param name="strSql"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      public bool Update(string strSql, SugarParameter[] parameters = null)
      {
         return BaseDal.Update(strSql, parameters);
      }

      /// <summary>
      ///     根據手寫sql語句更新數據
      /// </summary>
      /// <param name="strSql"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null)
      {
         return await BaseDal.UpdateAsync(strSql, parameters);
      }

      /// <summary>
      ///     更新某個欄位
      /// </summary>
      /// <param name="columns">lamdba表達式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
      /// <param name="where">lamdba判斷</param>
      /// <returns></returns>
      public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where)
      {
         return BaseDal.Update(columns, where);
      }

      /// <summary>
      ///     更新某個欄位
      /// </summary>
      /// <param name="columns">lamdba表達式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
      /// <param name="where">lamdba判斷</param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where)
      {
         return await BaseDal.UpdateAsync(columns, where);
      }

      /// <summary>
      ///     根據條件更新
      /// </summary>
      /// <param name="entity"></param>
      /// <param name="lstColumns"></param>
      /// <param name="lstIgnoreColumns"></param>
      /// <param name="strWhere"></param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(T entity, List<string> lstColumns = null,
          List<string> lstIgnoreColumns = null, string strWhere = "")
      {
         return await BaseDal.UpdateAsync(entity, lstColumns, lstIgnoreColumns, strWhere);
      }

      /// <summary>
      ///     根據條件更新
      /// </summary>
      /// <param name="entity"></param>
      /// <param name="lstColumns"></param>
      /// <param name="lstIgnoreColumns"></param>
      /// <param name="strWhere"></param>
      /// <returns></returns>
      public bool Update(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null,
          string strWhere = "")
      {
         return BaseDal.Update(entity, lstColumns, lstIgnoreColumns, strWhere);
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public bool Delete(T entity)
      {
         return BaseDal.Delete(entity);
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public async Task<bool> DeleteAsync(T entity)
      {
         return await BaseDal.DeleteAsync(entity);
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public bool Delete(IEnumerable<T> entity)
      {
         return BaseDal.Delete(entity);
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public async Task<bool> DeleteAsync(IEnumerable<T> entity)
      {
         return await BaseDal.DeleteAsync(entity);
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="where">過濾條件</param>
      /// <returns></returns>
      public bool Delete(Expression<Func<T, bool>> where)
      {
         return BaseDal.Delete(where);
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="where">過濾條件</param>
      /// <returns></returns>
      public async Task<bool> DeleteAsync(Expression<Func<T, bool>> where)
      {
         return await BaseDal.DeleteAsync(where);
      }

      /// <summary>
      ///     刪除指定ID的數據
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public bool DeleteById(object id)
      {
         return BaseDal.DeleteById(id);
      }

      /// <summary>
      ///     刪除指定ID的數據
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdAsync(object id)
      {
         return await BaseDal.DeleteByIdAsync(id);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(int[] ids)
      {
         return BaseDal.DeleteByIds(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(int[] ids)
      {
         return await BaseDal.DeleteByIdsAsync(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(long[] ids)
      {
         return BaseDal.DeleteByIds(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(long[] ids)
      {
         return await BaseDal.DeleteByIdsAsync(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(Guid[] ids)
      {
         return BaseDal.DeleteByIds(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(Guid[] ids)
      {
         return await BaseDal.DeleteByIdsAsync(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(string[] ids)
      {
         return BaseDal.DeleteByIds(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(string[] ids)
      {
         return await BaseDal.DeleteByIdsAsync(ids);
      }


      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(List<int> ids)
      {
         return BaseDal.DeleteByIds(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(List<int> ids)
      {
         return await BaseDal.DeleteByIdsAsync(ids);
      }


      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(List<string> ids)
      {
         return BaseDal.DeleteByIds(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(List<string> ids)
      {
         return await BaseDal.DeleteByIdsAsync(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(List<Guid> ids)
      {
         return BaseDal.DeleteByIds(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(List<Guid> ids)
      {
         return await BaseDal.DeleteByIdsAsync(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(List<long> ids)
      {
         return BaseDal.DeleteByIds(ids);
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(List<long> ids)
      {
         return await BaseDal.DeleteByIdsAsync(ids);
      }

      /// <summary>
      ///     判斷數據是否存在
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public bool Exists(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return BaseDal.Exists(predicate, blUseNoLock);
      }

      /// <summary>
      ///     判斷數據是否存在
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return await BaseDal.ExistsAsync(predicate, blUseNoLock);
      }

      /// <summary>
      ///     取得數據總數
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public int GetCount(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return BaseDal.GetCount(predicate, blUseNoLock);
      }

      /// <summary>
      ///     取得數據總數
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return await BaseDal.GetCountAsync(predicate, blUseNoLock);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <param name="field">欄位</param>
      /// <returns></returns>
      public int GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field, bool blUseNoLock = false)
      {
         return BaseDal.GetSum(predicate, field, blUseNoLock);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <param name="field">欄位</param>
      /// <returns></returns>
      public async Task<int> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field,
          bool blUseNoLock = false)
      {
         return await BaseDal.GetSumAsync(predicate, field, blUseNoLock);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <param name="field">欄位</param>
      /// <returns></returns>
      public decimal GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
          bool blUseNoLock = false)
      {
         return BaseDal.GetSum(predicate, field, blUseNoLock);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <param name="field">欄位</param>
      /// <returns></returns>
      public async Task<decimal> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
          bool blUseNoLock = false)
      {
         return await BaseDal.GetSumAsync(predicate, field, blUseNoLock);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <param name="field">欄位</param>
      /// <returns></returns>
      public float GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field,
          bool blUseNoLock = false)
      {
         return BaseDal.GetSum(predicate, field, blUseNoLock);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <param name="field">欄位</param>
      /// <returns></returns>
      public async Task<float> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field,
          bool blUseNoLock = false)
      {
         return await BaseDal.GetSumAsync(predicate, field, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢分頁數據
      /// </summary>
      /// <param name="predicate"></param>
      /// <param name="orderBy"></param>
      /// <param name="pageIndex">目前頁面索引</param>
      /// <param name="pageSize">分佈大小</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public IPageList<T> QueryPage(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1,
          int pageSize = 20, bool blUseNoLock = false)
      {
         return BaseDal.QueryPage(predicate, orderBy, pageIndex, pageSize, blUseNoLock);
      }

      /// <summary>
      ///     根據條件查詢分頁數據
      /// </summary>
      /// <param name="predicate"></param>
      /// <param name="orderBy"></param>
      /// <param name="pageIndex">目前頁面索引</param>
      /// <param name="pageSize">分佈大小</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>> predicate, string orderBy = "",
          int pageIndex = 1, int pageSize = 20, bool blUseNoLock = false)
      {
         return await BaseDal.QueryPageAsync(predicate, orderBy, pageIndex, pageSize, blUseNoLock);
      }

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
      public IPageList<T> QueryPage(Expression<Func<T, bool>> predicate,
          Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
          int pageSize = 20, bool blUseNoLock = false)
      {
         return BaseDal.QueryPage(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
      }

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
      public async Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>> predicate,
          Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
          int pageSize = 20, bool blUseNoLock = false)
      {
         return await BaseDal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize,
             blUseNoLock);
      }

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
      public List<TResult> QueryMuch<T1, T2, TResult>(
          Expression<Func<T1, T2, object[]>> joinExpression,
          Expression<Func<T1, T2, TResult>> selectExpression,
          Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
      {
         return BaseDal.QueryMuch(joinExpression, selectExpression, whereLambda, blUseNoLock);
      }

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
      public async Task<List<TResult>> QueryMuchAsync<T1, T2, TResult>(
          Expression<Func<T1, T2, object[]>> joinExpression,
          Expression<Func<T1, T2, TResult>> selectExpression,
          Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
      {
         return await BaseDal.QueryMuchAsync(joinExpression, selectExpression, whereLambda, blUseNoLock);
      }

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
      public TResult QueryMuchFirst<T1, T2, TResult>(
          Expression<Func<T1, T2, object[]>> joinExpression,
          Expression<Func<T1, T2, TResult>> selectExpression,
          Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
      {
         return BaseDal.QueryMuchFirst(joinExpression, selectExpression, whereLambda, blUseNoLock);
      }

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
      public async Task<TResult> QueryMuchFirstAsync<T1, T2, TResult>(
          Expression<Func<T1, T2, object[]>> joinExpression,
          Expression<Func<T1, T2, TResult>> selectExpression,
          Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
      {
         return await BaseDal.QueryMuchFirstAsync(joinExpression, selectExpression, whereLambda, blUseNoLock);
      }

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
      public List<TResult> QueryMuch<T1, T2, T3, TResult>(
          Expression<Func<T1, T2, T3, object[]>> joinExpression,
          Expression<Func<T1, T2, T3, TResult>> selectExpression,
          Expression<Func<T1, T2, T3, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
      {
         return BaseDal.QueryMuch(joinExpression, selectExpression, whereLambda, blUseNoLock);
      }

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
      public async Task<List<TResult>> QueryMuchAsync<T1, T2, T3, TResult>(
          Expression<Func<T1, T2, T3, object[]>> joinExpression,
          Expression<Func<T1, T2, T3, TResult>> selectExpression,
          Expression<Func<T1, T2, T3, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new()
      {
         return await BaseDal.QueryMuchAsync(joinExpression, selectExpression, whereLambda, blUseNoLock);
      }

      /// <summary>
      ///     執行sql語句並返回List<T>
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="sql"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      public List<T> SqlQuery(string sql, List<SugarParameter> parameters)
      {
         return BaseDal.SqlQuery(sql, parameters);
      }

      /// <summary>
      ///     執行sql語句並返回List<T>
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="sql"></param>
      /// <returns></returns>
      public async Task<List<T>> SqlQueryable(string sql)
      {
         return await BaseDal.SqlQueryable(sql);
      }
   }
}
