using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class CreateSpecOptionVM
    {
        public int SpuId { get; set; }
        public int SpecId { get; set; }

        public int SkuId { get; set; }

        public List<string> Options { get; set; }

        public bool IsCreate { get; set; }

    }
}
