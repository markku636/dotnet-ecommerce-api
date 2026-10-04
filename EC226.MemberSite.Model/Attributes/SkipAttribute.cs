using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.Attributes
{
    public class SkipLogAttribute : System.Attribute
    {
        public SkipLogAttribute()
        {
        }
    }

    public class SkipLogParamsAttribute : System.Attribute
    {
        public string[] param;

        public SkipLogParamsAttribute(params string[] objs)
        {
            this.param = objs;
        }
    }
}
