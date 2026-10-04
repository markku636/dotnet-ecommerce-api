
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace EC226.Filter
{
    public class ApiExplorerIgnores : IActionModelConvention
    {
        /// <summary>
        /// 自帶的Controller與swagger3.0衝突，在此排除掃瞄
        /// </summary>
        /// <param name="action"></param>
        public void Apply(ActionModel action)
        {
            //衝突的Ocelot.Raft.RaftController
            if (action.Controller.ControllerName == ("WxOfficialOAuth") || action.Controller.ControllerName == ("WxOpenOAuth"))
                action.ApiExplorer.IsVisible = false;
            //Ocelot.Cache.OutputCacheController
            if (action.Controller.ControllerName == ("AliPay"))
                action.ApiExplorer.IsVisible = false;

            if (action.Controller.ControllerName == ("WeChatPay"))
                action.ApiExplorer.IsVisible = false;
        }
    }
}
