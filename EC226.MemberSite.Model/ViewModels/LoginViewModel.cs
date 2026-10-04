using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{

    public class LoginViewModel
    {
        [BindProperty]

        public string Username { get; set; }
        [BindProperty]

        public string Password { get; set; }

    }
}
