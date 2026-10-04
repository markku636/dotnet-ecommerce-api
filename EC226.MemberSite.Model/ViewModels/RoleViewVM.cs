using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels
{
    public class RoleViewVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SpecialToken { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public int Status { get; set; }
        public bool IsDelete { get; set; }
        public int Sort { get; set; }
    }
}
