
using SqlSugar;

namespace EC226.MemberSite.IRepository.UnitOfWork
{
    public interface IUnitOfWork
    {
        SqlSugarScope GetDbClient();
        void BeginTran();
        void CommitTran();
        void RollbackTran();
    }
}
