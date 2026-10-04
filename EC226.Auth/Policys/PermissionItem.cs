
using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.Auth.Policys
{
    /// <summary>
    /// 使用者或角色或其他憑據實體
    /// </summary>
    public class PermissionItem
    {
        /// <summary>
        /// 使用者或角色或其他憑據名稱
        /// </summary>
        public virtual string Role { get; set; }
        /// <summary>
        /// 請求Url
        /// </summary>
        public virtual string Url { get; set; }
        /// <summary>
        /// 許可權標識
        /// </summary>
        public virtual string Authority { get; set; }
        /// <summary>
        /// 路由標識Url
        /// </summary>
        public virtual string RouteUrl { get; set; }
    }
}
