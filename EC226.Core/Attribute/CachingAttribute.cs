
using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.Core.Attribute
{
    /// <summary>
    /// 這個Attribute就是使用時候的驗證，把它新增到要快取數據的方法中，即可完成快取的操作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAttribute : System.Attribute
    {
        /// <summary>
        /// 快取絕對過期時間（分鐘）
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 30;

    }
}
