
using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.Core.Attribute
{
    /// <summary>
    /// 這個Attribute就是使用時候的驗證，把它新增到需要執行事務的方法中，即可完成事務的操作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UseTranAttribute : System.Attribute
    {

    }
}
