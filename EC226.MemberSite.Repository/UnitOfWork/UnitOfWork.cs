
using System;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.Loging;
using NLog;
using SqlSugar;
using SqlSugar.IOC;

namespace EC226.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        //public UnitOfWork(ISqlSugarClient sqlSugarClient)
        //{
        //    _sqlSugarClient = sqlSugarClient;
        //}


        public UnitOfWork()
        {
            _sqlSugarClient = DbScoped.SugarScope;
        }

        /// <summary>
        ///     取得DB，保證唯一性
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetDbClient()
        {
            // 必須要as，後邊會用到切換資料庫操作
            return _sqlSugarClient as SqlSugarScope;
        }

        public void BeginTran()
        {
            GetDbClient().BeginTran();
        }

        public void CommitTran()
        {
            try
            {
                GetDbClient().CommitTran(); //
            }
            catch (Exception ex)
            {
                GetDbClient().RollbackTran();

                NLogUtil.WriteSEQLog($"資料庫交易異常", LogLevel.Error, ex);
                throw;
            }
        }

        public void RollbackTran()
        {
            GetDbClient().RollbackTran();
        }
    }
}
