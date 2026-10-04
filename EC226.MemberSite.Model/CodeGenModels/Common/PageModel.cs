using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.CodeGenModels
{
    public class PageViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageTotal { get; set; }
    }
}
