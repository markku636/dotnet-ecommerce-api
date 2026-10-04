

using System.Net;
using EC226.Loging;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EC226.Filter
{
    /// <summary>
    /// 介面全域性異常錯誤日誌
    /// </summary>
    public class GlobalExceptionsFilterForClent : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {

            NLogUtil.WriteAll("Global Exception", NLog.LogLevel.Error, context.Exception);
                     
            //處理各種異常       
            var result = new Result("系統返回異常，請聯繫管理員進行處理！");
        
            context.ExceptionHandled = true;
         
            context.Result = new ObjectResult(result);
        }

    }

}
