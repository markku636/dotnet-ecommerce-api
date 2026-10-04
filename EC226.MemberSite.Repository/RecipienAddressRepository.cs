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
    /// RecipienAddress 介面實現
    /// </summary>
    public class RecipienAddressRepository : BaseRepository<RecipienAddress>, IRecipienAddressRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecipienAddressRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region 實現重寫增刪改查操作==========================================================

        /// <summary>
        /// 重寫非同步插入方法
        /// </summary>
        /// <param name="entity">實體數據</param>
        /// <returns></returns>
        public new async Task<Result> InsertAsync(RecipienAddress entity)
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
        public new async Task<Result> UpdateAsync(RecipienAddress entity)
        {
            var result = new Result();

            try 
            {
                var oldModel = await DbClient.Queryable<RecipienAddress>().In(entity.Id).SingleAsync();

                if (oldModel == null) 
                {
                    result.Fail("不存在此資訊");
                    return result;
                }

                //事物處理過程開始
                
                    oldModel.Id = entity.Id;
                
                    oldModel.MemberId = entity.MemberId;
                
                    oldModel.RecipientName = entity.RecipientName;
                
                    oldModel.Phone = entity.Phone;
                
                    oldModel.Country = entity.Country;
                
                    oldModel.ZipCode = entity.ZipCode;
                
                    oldModel.Address = entity.Address;
                
                    oldModel.IsDefault = entity.IsDefault;
                
                    oldModel.CreateTime = entity.CreateTime;
                
                    oldModel.ModifyTime = entity.ModifyTime;
                
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
        public new async Task<Result> UpdateAsync(List<RecipienAddress> entity)
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
                var isSuccess = await DbClient.Deleteable<RecipienAddress>(id).ExecuteCommandHasChangeAsync();
            
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
                var isSuccess = await DbClient.Deleteable<RecipienAddress>().In(ids).ExecuteCommandHasChangeAsync();
            
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
        public async Task<List<RecipienAddress>> GetCaChe()
        {
            var cache = await ManualDataCache.Instance.GetAsync<List<RecipienAddress>>($"CacheRecipienAddress");
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        /// 更新cache
        /// </summary>
        public async Task<List<RecipienAddress>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<RecipienAddress>().With(SqlWith.NoLock).ToListAsync();
            await ManualDataCache.Instance.SetAsync($"CacheRecipienAddress", list);
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
        public new async Task<PageList<RecipienAddress>> QueryPageAsync(Expression<Func<RecipienAddress, bool>> predicate,
            Expression<Func<RecipienAddress, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<RecipienAddress> page;

            if (blUseNoLock)
            {
                page = await DbClient.Queryable<RecipienAddress>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new RecipienAddress
                {
                      Id = p.Id,
                MemberId = p.MemberId,
                RecipientName = p.RecipientName,
                Phone = p.Phone,
                Country = p.Country,
                ZipCode = p.ZipCode,
                Address = p.Address,
                IsDefault = p.IsDefault,
                CreateTime = p.CreateTime,
                ModifyTime = p.ModifyTime,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<RecipienAddress>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new RecipienAddress
                {
                      Id = p.Id,
                MemberId = p.MemberId,
                RecipientName = p.RecipientName,
                Phone = p.Phone,
                Country = p.Country,
                ZipCode = p.ZipCode,
                Address = p.Address,
                IsDefault = p.IsDefault,
                CreateTime = p.CreateTime,
                ModifyTime = p.ModifyTime,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }

            var list = new PageList<RecipienAddress>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
      
