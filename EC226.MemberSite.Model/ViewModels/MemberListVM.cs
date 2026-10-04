using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels
{
    public class MemberListVM
    {
        public int Id { get; set; }
        public string UserAccount { get; set; }
        public string Email { get; set; }
        [DisplayName("UserNameTest")]
        public string UserName { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Sort { get; set; }
        public string ParentId { get; set; }
    }
}
