using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.Attributes
{
    /// <summary>
    /// 狀態Attrubute
    /// </summary>
    [AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class StateAttribute : Attribute
    {
        /// <summary>
        /// 允許移轉的狀態值
        /// </summary>
        public List<int> AllowStateTransfertTypes = new List<int>();

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="values">允許的狀態值</param>
        public StateAttribute(params int[] values)
        {
            if (values[0] == null)
            {
                return;
            }

            AllowStateTransfertTypes = values.ToList();
        }
    }
}
