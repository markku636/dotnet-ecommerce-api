using System;
using System.Collections.Generic;
using System.Text;
using EC226.MemberSite.Model.Entities;


namespace EC226.MemberSite.Model.ViewModels
{

    public class RegisterMemberVM
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }



    public class SocialNetworkLogin
    {
        public string ProviderId { get; set; }

        public  string FirebaseMemberId { get; set; }
        public string ProviderMemberId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }

        public string Phone { get; set; }
    }

    public class MemberVM : Member
    {
    }


    public class MemberInfoVM
    {
        public string DisplayName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Boolean HasChangePassword { get; set; }
    }



    public class LoginTokenVM
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int Expired { get; set; } // 幾秒後失效
    }


    public class MemberAuthenticationProviderVM : MemberAuthenticationProvider
    {

    }

}
