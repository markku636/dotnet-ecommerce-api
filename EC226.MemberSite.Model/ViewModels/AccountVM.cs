using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels
{
    public class AccountVM
    {

        public int Id { get; set; }
        public string UserAccount { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Sort { get; set; }
        public bool IsDelete { get; set; }
        
        public string RoleIds { get; set; }
    }
}
