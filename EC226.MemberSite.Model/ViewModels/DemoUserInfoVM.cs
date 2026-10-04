using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class DemoUserInfoVM
    {
        public DemoUser User { get; set; } = new DemoUser();
    }

    public class DemoUsersViewModel
    {
        public List<DemoUser> items { get; set; } = new List<DemoUser>() { };
    }


    public class DemoUser
    {
        public string avatar { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string introduction { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string username { get; set; }
        public string[] roles { get; set; }
    }
    
}
