using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels
{
    public class ElementUICascaderVM
    {
        public int Value { get; set; }

        public string Label { get; set; }

        public int? ParentId { get; set; }

        public List<ElementUICascaderVM> Children { get; set; }

    }
}
