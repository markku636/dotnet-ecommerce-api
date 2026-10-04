using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EC226.Caching.Manual;
using EC226.Configuration;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels.Basics;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.Model.ViewModels.UI;
using SqlSugar;
using EC226.Loging;

namespace EC226.MemberSite.Repository
{
    /// <summary>
    /// Program 介面實現
    /// </summary>
    public class ProgramRepository : BaseRepository<Program>, IProgramRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProgramRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region 實現重寫增刪改查操作==========================================================

        /// <summary>
        /// 重寫非同步插入方法
        /// </summary>
        /// <param name="entity">實體數據</param>
        /// <returns></returns>
        public new async Task<Result> InsertAsync(Program entity)
        {
             var result = new Result();
             try
             {
              var id = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync();
    
                 var isSuccess = id > 0;

                 if (isSuccess)
                 {
                    result.ID = id;
                    result.Success(GlobalConstVars.CreateSuccess);
                 }
                 else
                 {
                    result.Fail(GlobalConstVars.CreateFailure);
                 }
              }
              catch (Exception ex)
              {
                 NLogUtil.WriteSEQLog(result.Message, NLog.LogLevel.Error, ex);
              }

              return result;
        }

        /// <summary>
        /// 重寫非同步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<Result> UpdateAsync(Program entity)
        {
            var result = new Result();

            try 
            {
                var oldModel = await DbClient.Queryable<Program>().In(entity.Id).SingleAsync();

                if (oldModel == null) 
                {
                    result.Fail("不存在此資訊");
                    return result;
                }

                //事物處理過程開始
                
                    oldModel.Id = entity.Id;
                
                    oldModel.Name = entity.Name;
                
                    oldModel.Module = entity.Module;
                
                    oldModel.HttpMethod = entity.HttpMethod;
                
                    oldModel.ControllerFullName = entity.ControllerFullName;
                
                    oldModel.IdentityKey = entity.IdentityKey;
                
                    oldModel.Controller = entity.Controller;
                
                    oldModel.Action = entity.Action;
                
                    oldModel.ReturnType = entity.ReturnType;
                
                    oldModel.ParameterCount = entity.ParameterCount;
                
                    oldModel.Describe = entity.Describe;
                
                    oldModel.CreateTime = entity.CreateTime;
                
                    oldModel.SiteType = entity.SiteType;
                
                    oldModel.Sort = entity.Sort;
                
                //事物處理過程結束                       

                var isSuccess = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();

                if (isSuccess) 
                {
                    result.Success(GlobalConstVars.EditSuccess);                    
                }
                else 
                {
                    result.Fail(GlobalConstVars.EditFailure);
                }
            }
            catch (Exception ex) 
            {                
                NLogUtil.WriteSEQLog(result.Message, NLog.LogLevel.Error, ex);
            }

            return result;
        }

        /// <summary>
        /// 重寫非同步更新方法
        /// </summary>
        /// <param name="entity"></
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<Result> UpdateAsync(List<Program> entity)
        {
            var result = new Result();

            try
            {
                var isSuccess = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();

                if (isSuccess) 
                {
                    result.Success(GlobalConstVars.EditSuccess);                    
                }
                else 
                {
                    result.Fail(GlobalConstVars.EditFailure);
                }
            }
            catch (Exception ex) 
            {                
                NLogUtil.WriteSEQLog(result.Message, NLog.LogLevel.Error, ex);
            }

            return result;
        }

        /// <summary>
        /// 重刪除指定ID的數據
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<Result> DeleteByIdAsync(object id)
        {
            var result = new Result();

            try
            {
                var isSuccess = await DbClient.Deleteable<Program>(id).ExecuteCommandHasChangeAsync();
            
                if (isSuccess) 
                {
                    result.Success(GlobalConstVars.DeleteSuccess);                    
                }
                else 
                {
                    result.Fail(GlobalConstVars.DeleteFailure);
                }
            }
            catch (Exception ex) 
            {                
                NLogUtil.WriteSEQLog(result.Message, NLog.LogLevel.Error, ex);
            }

            return result;
        }

        /// <summary>
        /// 重寫刪除指定ID集合的數據(批量刪除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<Result> DeleteByIdsAsync(int[] ids)
        {
            var result = new Result();

            try 
            {
                var isSuccess = await DbClient.Deleteable<Program>().In(ids).ExecuteCommandHasChangeAsync();
            
                if (isSuccess) 
                {
                    result.Success(GlobalConstVars.DeleteSuccess);                    
                }
                else 
                {
                    result.Fail(GlobalConstVars.DeleteFailure);
                }
            }
            catch (Exception ex) 
            {                
                NLogUtil.WriteSEQLog(result.Message, NLog.LogLevel.Error, ex);
            }

            return result;
        }

        #endregion

        #region 取得快取的所有數據==========================================================

        /// <summary>
        /// 取得快取的所有數據
        /// </summary>
        /// <returns></returns>
        public async Task<List<Program>> GetCaChe()
        {
            var cache = await ManualDataCache.Instance.GetAsync<List<Program>>($"CacheProgram");
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        /// 更新cache
        /// </summary>
        public async Task<List<Program>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<Program>().With(SqlWith.NoLock).ToListAsync();
            await ManualDataCache.Instance.SetAsync($"CacheProgram", list);
            return list;
        }

        #endregion


        #region 重寫根據條件查詢分頁數據
        /// <summary>
        /// 重寫根據條件查詢分頁數據
        /// </summary>
        /// <param name="predicate">判斷集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">目前頁面索引</param>
        /// <param name="pageSize">分佈大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<PageList<Program>> QueryPageAsync(Expression<Func<Program, bool>> predicate,
            Expression<Func<Program, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<Program> page;

            if (blUseNoLock)
            {
                page = await DbClient.Queryable<Program>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new Program
                {
                      Id = p.Id,
                Name = p.Name,
                Module = p.Module,
                HttpMethod = p.HttpMethod,
                ControllerFullName = p.ControllerFullName,
                IdentityKey = p.IdentityKey,
                Controller = p.Controller,
                Action = p.Action,
                ReturnType = p.ReturnType,
                ParameterCount = p.ParameterCount,
                Describe = p.Describe,
                CreateTime = p.CreateTime,
                SiteType = p.SiteType,
                Sort = p.Sort,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<Program>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new Program
                {
                      Id = p.Id,
                Name = p.Name,
                Module = p.Module,
                HttpMethod = p.HttpMethod,
                ControllerFullName = p.ControllerFullName,
                IdentityKey = p.IdentityKey,
                Controller = p.Controller,
                Action = p.Action,
                ReturnType = p.ReturnType,
                ParameterCount = p.ParameterCount,
                Describe = p.Describe,
                CreateTime = p.CreateTime,
                SiteType = p.SiteType,
                Sort = p.Sort,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }

            var list = new PageList<Program>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
      
