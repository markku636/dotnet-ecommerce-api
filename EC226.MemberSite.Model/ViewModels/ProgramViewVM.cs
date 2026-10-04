using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels
{
    public class ProgramViewVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string HttpMethod { get; set; }
        public string ControllerFullName { get; set; }
        public string IdentityKey { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReturnType { get; set; }
        public Nullable<int> ParameterCount { get; set; }
        public string Describe { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int SiteType { get; set; }
        public int Sort { get; set; }
    }
}
