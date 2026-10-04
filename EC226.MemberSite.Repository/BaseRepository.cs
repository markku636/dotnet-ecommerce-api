using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.Model.ViewModels.Basics;

using SqlSugar;

namespace EC226.MemberSite.Repository
{
   public abstract class BaseRepository<T>: IBaseRepository<T> where T : class, new()
   {
      //private readonly IUnitOfWork _unitOfWork;
      private readonly SqlSugarScope _dbBase;

      protected BaseRepository(IUnitOfWork unitOfWork)
      {
         //_unitOfWork = unitOfWork;
         _dbBase = unitOfWork.GetDbClient();
      }

      private ISqlSugarClient DbBaseClient => _dbBase;

      protected ISqlSugarClient DbClient => DbBaseClient;

      /// <summary>
      ///     根據主值查詢單條數據
      /// </summary>
      /// <param name="pkValue">主鍵值</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>泛型實體</returns>
      public T QueryById(object pkValue, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().With(SqlWith.NoLock).InSingle(pkValue)
             : DbBaseClient.Queryable<T>().InSingle(pkValue);
      }

      /// <summary>
      ///     根據主值查詢單條數據
      /// </summary>
      /// <param name="objId">id（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns>數據實體</returns>
      public async Task<T> QueryByIdAsync(object objId, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().In(objId).With(SqlWith.NoLock).SingleAsync()
             : await DbBaseClient.Queryable<T>().In(objId).SingleAsync();
      }

      /// <summary>
      ///     根據主值列表查詢單條數據
      /// </summary>
      /// <param name="lstIds">id列表（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <returns>數據實體列表</returns>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      public List<T> QueryByIDs(object[] lstIds, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToList()
             : DbBaseClient.Queryable<T>().In(lstIds).ToList();
      }

      /// <summary>
      ///     根據主值列表查詢單條數據
      /// </summary>
      /// <param name="lstIds">id列表（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <returns>數據實體列表</returns>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      public async Task<List<T>> QueryByIDsAsync(object[] lstIds, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToListAsync()
             : await DbBaseClient.Queryable<T>().In(lstIds).ToListAsync();
      }

      /// <summary>
      ///     根據主值列表查詢單條數據
      /// </summary>
      /// <param name="lstIds">id列表（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <returns>數據實體列表</returns>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      public List<T> QueryByIDs(int[] lstIds, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToList()
             : DbBaseClient.Queryable<T>().In(lstIds).ToList();
      }

      /// <summary>
      ///     根據主值列表查詢單條數據
      /// </summary>
      /// <param name="lstIds">id列表（必須指定主鍵特性 [SugarColumn(IsPrimaryKey=true)]），如果是聯合主鍵，請使用Where條件</param>
      /// <returns>數據實體列表</returns>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      public async Task<List<T>> QueryByIDsAsync(int[] lstIds, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToListAsync()
             : await DbBaseClient.Queryable<T>().In(lstIds).ToListAsync();
      }

      /// <summary>
      ///     查詢表單所有數據(無分頁,請慎用)
      /// </summary>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public List<T> Query(bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().With(SqlWith.NoLock).ToList()
             : DbBaseClient.Queryable<T>().ToList();
      }

      /// <summary>
      /// 拿回 Queryable
      /// </summary>
      /// <returns></returns>
      public ISugarQueryable<T> Queryable()
      {
         return DbBaseClient.Queryable<T>();
      }

      /// <summary>
      ///     查詢表單所有數據(無分頁,請慎用)
      /// </summary>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<List<T>> QueryAsync(bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().With(SqlWith.NoLock).ToListAsync()
             : await DbBaseClient.Queryable<T>().ToListAsync();
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
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).With(SqlWith.NoLock).ToList()
             : DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList();
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="strWhere">條件</param>
      /// <param name="orderBy">排序欄位，如name asc,age desc</param>
      /// <returns>泛型實體集合</returns>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      public async Task<List<T>> QueryListByClauseAsync(string strWhere, string orderBy = "",
          bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).With(SqlWith.NoLock).ToListAsync()
             : await DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="orderBy">排序欄位，如name asc,age desc</param>
      /// <returns>泛型實體集合</returns>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      public List<T> QueryListByClause(Expression<Func<T, bool>> predicate, string orderBy = "",
          bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToList()
             : DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(predicate != null, predicate).ToList();
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="orderBy">排序欄位，如name asc,age desc</param>
      /// <returns>泛型實體集合</returns>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, string orderBy = "",
          bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToListAsync()
             : await DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(predicate != null, predicate).ToListAsync();
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
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToList()
             : DbBaseClient.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .WhereIF(predicate != null, predicate).ToList();
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
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToListAsync()
             : await DbBaseClient.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .WhereIF(predicate != null, predicate).ToListAsync();
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
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .WhereIF(predicate != null, predicate).Take(take).With(SqlWith.NoLock).ToList()
             : DbBaseClient.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .WhereIF(predicate != null, predicate).Take(take).ToList();
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
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .WhereIF(predicate != null, predicate).Take(take).With(SqlWith.NoLock).ToListAsync()
             : await DbBaseClient.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                 .WhereIF(predicate != null, predicate).Take(take).ToListAsync();
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
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                 .Where(predicate).Take(take).With(SqlWith.NoLock).ToList()
             : DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                 .Where(predicate).Take(take).ToList();
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
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                 .Where(predicate).Take(take).With(SqlWith.NoLock).ToListAsync()
             : await DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                 .Where(predicate).Take(take).ToListAsync();
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public T QueryByClause(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().With(SqlWith.NoLock).First(predicate)
             : DbBaseClient.Queryable<T>().First(predicate);
      }

      /// <summary>
      ///     根據條件查詢數據
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().With(SqlWith.NoLock).FirstAsync(predicate)
             : await DbBaseClient.Queryable<T>().FirstAsync(predicate);
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
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().OrderBy(orderByPredicate, orderByType).With(SqlWith.NoLock).First(predicate)
             : DbBaseClient.Queryable<T>().OrderBy(orderByPredicate, orderByType).First(predicate);
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
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().OrderBy(orderByPredicate, orderByType).With(SqlWith.NoLock)
                 .FirstAsync(predicate)
             : await DbBaseClient.Queryable<T>().OrderBy(orderByPredicate, orderByType).FirstAsync(predicate);
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體數據</param>
      /// <returns></returns>
      public int Insert(T entity)
      {
         return DbBaseClient.Insertable(entity).ExecuteReturnIdentity();
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體數據</param>
      /// <returns></returns>
      public async Task<int> InsertAsync(T entity)
      {
         return await DbBaseClient.Insertable(entity).ExecuteReturnIdentityAsync();
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體數據</param>
      /// <param name="insertColumns">插入的列</param>
      /// <returns></returns>
      public int Insert(T entity, Expression<Func<T, object>> insertColumns = null)
      {
         var insert = DbBaseClient.Insertable(entity);
         if (insertColumns == null)
            return insert.ExecuteReturnIdentity();
         return insert.InsertColumns(insertColumns).ExecuteReturnIdentity();
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體數據</param>
      /// <param name="insertColumns">插入的列</param>
      /// <returns></returns>
      public async Task<int> InsertAsync(T entity, Expression<Func<T, object>> insertColumns = null)
      {
         var insert = DbBaseClient.Insertable(entity);
         if (insertColumns == null)
            return await insert.ExecuteReturnIdentityAsync();
         return await insert.InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <param name="insertColumns">需插入的欄位</param>
      /// <returns></returns>
      public bool InsertGuid(T entity, Expression<Func<T, object>> insertColumns = null)
      {
         var insert = DbBaseClient.Insertable(entity);
         if (insertColumns == null)
            return insert.ExecuteCommand() > 0;
         return insert.InsertColumns(insertColumns).ExecuteCommand() > 0;
      }

      /// <summary>
      ///     寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <param name="insertColumns">需插入的欄位</param>
      /// <returns></returns>
      public async Task<bool> InsertGuidAsync(T entity, Expression<Func<T, object>> insertColumns = null)
      {
         var insert = DbBaseClient.Insertable(entity);
         if (insertColumns == null)
            return await insert.ExecuteCommandAsync() > 0;
         return await insert.InsertColumns(insertColumns).ExecuteCommandAsync() > 0;
      }

      /// <summary>
      ///     批量寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public int Insert(List<T> entity)
      {
         return DbBaseClient.Insertable(entity.ToArray()).ExecuteReturnIdentity();
      }

      /// <summary>
      ///     批量寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public async Task<int> InsertAsync(List<T> entity)
      {
         return await DbBaseClient.Insertable(entity.ToArray()).ExecuteCommandAsync();
      }

      /// <summary>
      ///     批量寫入實體數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public async Task<int> InsertCommandAsync(List<T> entity)
      {
         return await DbBaseClient.Insertable(entity.ToArray()).ExecuteCommandAsync();
      }

      /// <summary>
      ///     批量更新實體數據
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
      public bool Update(List<T> entity)
      {
         return DbBaseClient.Updateable(entity).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     批量更新實體數據
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(List<T> entity)
      {
         return await DbBaseClient.Updateable(entity).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     更新實體數據
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
      public bool Update(T entity)
      {
         return DbBaseClient.Updateable(entity).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     更新實體數據
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(T entity)
      {
         return await DbBaseClient.Updateable(entity).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     根據手寫條件更新
      /// </summary>
      /// <param name="entity"></param>
      /// <param name="strWhere"></param>
      /// <returns></returns>
      public bool Update(T entity, string strWhere)
      {
         return DbBaseClient.Updateable(entity).Where(strWhere).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     根據手寫條件更新
      /// </summary>
      /// <param name="entity"></param>
      /// <param name="strWhere"></param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(T entity, string strWhere)
      {
         return await DbBaseClient.Updateable(entity).Where(strWhere).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     根據手寫sql語句更新數據
      /// </summary>
      /// <param name="strSql"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      public bool Update(string strSql, SugarParameter[] parameters = null)
      {
         return DbBaseClient.Ado.ExecuteCommand(strSql, parameters) > 0;
      }

      /// <summary>
      ///     根據手寫sql語句更新數據
      /// </summary>
      /// <param name="strSql"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null)
      {
         return await DbBaseClient.Ado.ExecuteCommandAsync(strSql, parameters) > 0;
      }

      /// <summary>
      ///     更新某個欄位
      /// </summary>
      /// <param name="columns">lamdba表達式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
      /// <param name="where">lamdba判斷</param>
      /// <returns></returns>
      public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where)
      {
         var i = DbBaseClient.Updateable<T>().SetColumns(columns).Where(where).ExecuteCommand();
         return i > 0;
      }

      /// <summary>
      ///     更新某個欄位
      /// </summary>
      /// <param name="columns">lamdba表達式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
      /// <param name="where">lamdba判斷</param>
      /// <returns></returns>
      public async Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where)
      {
         return await DbBaseClient.Updateable<T>().SetColumns(columns).Where(where).ExecuteCommandHasChangeAsync();
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
         var up = DbBaseClient.Updateable(entity);
         if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
         if (lstColumns != null && lstColumns.Count > 0) up = up.UpdateColumns(lstColumns.ToArray());
         if (!string.IsNullOrEmpty(strWhere)) up = up.Where(strWhere);
         return await up.ExecuteCommandHasChangeAsync();
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
         var up = DbBaseClient.Updateable(entity);
         if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
         if (lstColumns != null && lstColumns.Count > 0) up = up.UpdateColumns(lstColumns.ToArray());
         if (!string.IsNullOrEmpty(strWhere)) up = up.Where(strWhere);
         return up.ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public bool Delete(T entity)
      {
         return DbBaseClient.Deleteable(entity).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public async Task<bool> DeleteAsync(T entity)
      {
         return await DbBaseClient.Deleteable(entity).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public bool Delete(IEnumerable<T> entity)
      {
         return DbBaseClient.Deleteable<T>(entity).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="entity">實體類</param>
      /// <returns></returns>
      public async Task<bool> DeleteAsync(IEnumerable<T> entity)
      {
         return await DbBaseClient.Deleteable<T>(entity).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="where">過濾條件</param>
      /// <returns></returns>
      public bool Delete(Expression<Func<T, bool>> where)
      {
         return DbBaseClient.Deleteable(where).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除數據
      /// </summary>
      /// <param name="where">過濾條件</param>
      /// <returns></returns>
      public async Task<bool> DeleteAsync(Expression<Func<T, bool>> where)
      {
         return await DbBaseClient.Deleteable(where).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     刪除指定ID的數據
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public bool DeleteById(object id)
      {
         return DbBaseClient.Deleteable<T>(id).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除指定ID的數據
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdAsync(object id)
      {
         return await DbBaseClient.Deleteable<T>(id).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(int[] ids)
      {
         return DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(int[] ids)
      {
         return await DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(long[] ids)
      {
         return DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(long[] ids)
      {
         return await DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
      }


      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(Guid[] ids)
      {
         return DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(Guid[] ids)
      {
         return await DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
      }


      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(string[] ids)
      {
         return DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(string[] ids)
      {
         return await DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
      }


      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(List<int> ids)
      {
         return DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(List<int> ids)
      {
         return await DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(List<string> ids)
      {
         return DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(List<string> ids)
      {
         return await DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(List<Guid> ids)
      {
         return DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(List<Guid> ids)
      {
         return await DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public bool DeleteByIds(List<long> ids)
      {
         return DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChange();
      }

      /// <summary>
      ///     刪除指定ID集合的數據(批量刪除)
      /// </summary>
      /// <param name="ids"></param>
      /// <returns></returns>
      public async Task<bool> DeleteByIdsAsync(List<long> ids)
      {
         return await DbBaseClient.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
      }


      /// <summary>
      ///     判斷數據是否存在
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public bool Exists(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().Where(predicate).With(SqlWith.NoLock).Any()
             : DbBaseClient.Queryable<T>().Where(predicate).Any();
      }

      /// <summary>
      ///     判斷數據是否存在
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().Where(predicate).With(SqlWith.NoLock).AnyAsync()
             : await DbBaseClient.Queryable<T>().Where(predicate).AnyAsync();
      }

      /// <summary>
      ///     取得數據總數
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public int GetCount(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().With(SqlWith.NoLock).Count(predicate)
             : DbBaseClient.Queryable<T>().Count(predicate);
      }

      /// <summary>
      ///     取得數據總數
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().With(SqlWith.NoLock).CountAsync(predicate)
             : await DbBaseClient.Queryable<T>().CountAsync(predicate);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="field">欄位</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public int GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field, bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().Where(predicate).With(SqlWith.NoLock).Sum(field)
             : DbBaseClient.Queryable<T>().Where(predicate).Sum(field);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="field">欄位</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<int> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field,
          bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().Where(predicate).With(SqlWith.NoLock).SumAsync(field)
             : await DbBaseClient.Queryable<T>().Where(predicate).SumAsync(field);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="field">欄位</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public decimal GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
          bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().Where(predicate).With(SqlWith.NoLock).Sum(field)
             : DbBaseClient.Queryable<T>().Where(predicate).Sum(field);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="field">欄位</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<decimal> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
          bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().Where(predicate).With(SqlWith.NoLock).SumAsync(field)
             : await DbBaseClient.Queryable<T>().Where(predicate).SumAsync(field);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="field">欄位</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public float GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field,
          bool blUseNoLock = false)
      {
         return blUseNoLock
             ? DbBaseClient.Queryable<T>().Where(predicate).With(SqlWith.NoLock).Sum(field)
             : DbBaseClient.Queryable<T>().Where(predicate).Sum(field);
      }

      /// <summary>
      ///     取得數據某個欄位的合計
      /// </summary>
      /// <param name="predicate">條件表達式樹</param>
      /// <param name="field">欄位</param>
      /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
      /// <returns></returns>
      public async Task<float> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field,
          bool blUseNoLock = false)
      {
         return blUseNoLock
             ? await DbBaseClient.Queryable<T>().Where(predicate).With(SqlWith.NoLock).SumAsync(field)
             : await DbBaseClient.Queryable<T>().Where(predicate).SumAsync(field);
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
         var totalCount = 0;
         var page = blUseNoLock
             ? DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(predicate != null, predicate).With(SqlWith.NoLock)
                 .ToPageList(pageIndex, pageSize, ref totalCount)
             : DbClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(predicate != null, predicate).ToPageList(pageIndex, pageSize, ref totalCount);

         var list = new PageList<T>(page, pageIndex, pageSize, totalCount);
         return list;
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
         RefAsync<int> totalCount = 0;
         var page = blUseNoLock
             ? await DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(predicate != null, predicate).With(SqlWith.NoLock)
                 .ToPageListAsync(pageIndex, pageSize, totalCount)
             : await DbBaseClient.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                 .WhereIF(predicate != null, predicate).ToPageListAsync(pageIndex, pageSize, totalCount);
         var list = new PageList<T>(page, pageIndex, pageSize, totalCount);
         return list;
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
         var totalCount = 0;
         var page = blUseNoLock
             ? DbBaseClient.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                 .WhereIF(predicate != null, predicate).With(SqlWith.NoLock)
                 .ToPageList(pageIndex, pageSize, ref totalCount)
             : DbBaseClient.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                 .WhereIF(predicate != null, predicate).ToPageList(pageIndex, pageSize, ref totalCount);
         var list = new PageList<T>(page, pageIndex, pageSize, totalCount);
         return list;
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
         RefAsync<int> totalCount = 0;
         var page = blUseNoLock
             ? await DbBaseClient.Queryable<T>().WhereIF(predicate != null, predicate).OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                 .With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount)
             : await DbBaseClient.Queryable<T>().WhereIF(predicate != null, predicate).OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                 .ToPageListAsync(pageIndex, pageSize, totalCount);
         var list = new PageList<T>(page, pageIndex, pageSize, totalCount);
         return list;
      }

      /// <summary>
      ///     查詢-2表查詢
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
          Expression<Func<T1, T2, bool>> whereLambda = null,
          bool blUseNoLock = false) where T1 : class, new()
      {
         if (whereLambda == null)
            return blUseNoLock
                ? DbBaseClient.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock).ToList()
                : DbBaseClient.Queryable(joinExpression).Select(selectExpression).ToList();
         return blUseNoLock
             ? DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).With(SqlWith.NoLock)
                 .ToList()
             : DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToList();
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
          Expression<Func<T1, T2, bool>> whereLambda = null,
          bool blUseNoLock = false) where T1 : class, new()
      {
         if (whereLambda == null)
            return blUseNoLock
                ? await DbBaseClient.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock)
                    .ToListAsync()
                : await DbBaseClient.Queryable(joinExpression).Select(selectExpression).ToListAsync();
         return blUseNoLock
             ? await DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression)
                 .With(SqlWith.NoLock).ToListAsync()
             : await DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToListAsync();
      }

      /// <summary>
      ///     查詢-二表查詢
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
          Expression<Func<T1, T2, bool>> whereLambda = null,
          bool blUseNoLock = false) where T1 : class, new()
      {
         if (whereLambda == null)
            return blUseNoLock
                ? DbBaseClient.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock).First()
                : DbBaseClient.Queryable(joinExpression).Select(selectExpression).First();
         return blUseNoLock
             ? DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).With(SqlWith.NoLock)
                 .First()
             : DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).First();
      }

      /// <summary>
      ///     查詢-二表查詢
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
          Expression<Func<T1, T2, bool>> whereLambda = null,
          bool blUseNoLock = false) where T1 : class, new()
      {
         if (whereLambda == null)
            return blUseNoLock
                ? await DbBaseClient.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock)
                    .FirstAsync()
                : await DbBaseClient.Queryable(joinExpression).Select(selectExpression).FirstAsync();
         return blUseNoLock
             ? await DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression)
                 .With(SqlWith.NoLock).FirstAsync()
             : await DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).FirstAsync();
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
          Expression<Func<T1, T2, T3, bool>> whereLambda = null,
          bool blUseNoLock = false) where T1 : class, new()
      {
         if (whereLambda == null)
            return blUseNoLock
                ? DbBaseClient.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock)
                    .ToList()
                : DbBaseClient.Queryable(joinExpression).Select(selectExpression).ToList();
         return blUseNoLock
             ? DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).With(SqlWith.NoLock)
                 .ToList()
             : DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression)
                 .ToList();
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
          Expression<Func<T1, T2, T3, bool>> whereLambda = null,
          bool blUseNoLock = false) where T1 : class, new()
      {
         if (whereLambda == null)
            return blUseNoLock
                ? await DbBaseClient.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock)
                    .ToListAsync()
                : await DbBaseClient.Queryable(joinExpression).Select(selectExpression).ToListAsync();
         return blUseNoLock
             ? await DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression)
                 .With(SqlWith.NoLock)
                 .ToListAsync()
             : await DbBaseClient.Queryable(joinExpression).Where(whereLambda).Select(selectExpression)
                 .ToListAsync();
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
         var list = DbBaseClient.Ado.SqlQuery<T>(sql, parameters);
         return list;
      }

      /// <summary>
      ///     執行sql語句並返回List<T>
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="sql"></param>
      /// <returns></returns>
      public async Task<List<T>> SqlQueryable(string sql)
      {
         var list = await DbBaseClient.SqlQueryable<T>(sql).ToListAsync();
         return list;
      }
   }
}
