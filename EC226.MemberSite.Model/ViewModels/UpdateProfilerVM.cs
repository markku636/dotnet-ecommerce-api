using System;
using System.Collections.Generic;
using System.Text;


namespace EC226.MemberSite.Model.ViewModels
{

    public class UpdateProfilerVM
    {
        public string DisplayName { get; set; }
        public string Phone { get; set; }     
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }        
        public string Confirm { get; set; }
    }

}
