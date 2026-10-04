using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels
{
    public class UploadVM
    {
        public string commandQuery { get; set; }
        public IFormFile file { get; set; }
    }
}
