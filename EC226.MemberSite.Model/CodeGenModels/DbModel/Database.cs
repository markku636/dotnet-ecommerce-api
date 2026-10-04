using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace EC226.MemberSite.Model.CodeGenModels 
{
    public class Database
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Desc { get; set; }
        public string Connection { get; set; }
        public DbType DbType { get; set; }
        public DateTime ChangeTime { get; set; }
        public bool IsDeleted { get;  set; }
    }
}
