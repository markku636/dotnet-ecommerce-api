
using System;
using System.Collections.Generic;
using System.Linq;
using EC226.MemberSite.Model.ViewModels.Basics;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EC226.Filter
{
   /// <summary>
   /// 請求驗證錯誤處理
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
   public class RequiredErrorForClent: ResultFilterAttribute
   {
      public override void OnResultExecuting(ResultExecutingContext actionContext)
      {
         //base.OnResultExecuting(actionContext);
         var modelState = actionContext.ModelState;
         List<ErrorView> errors = new List<ErrorView>();
         if (!modelState.IsValid)
         {
            var baseResult = new Result()
            {
               Code = ApiReturnCode.Invalid_Parameter
            };
            foreach (var key in modelState.Keys)
            {
               var state = modelState[key];
               if (state.Errors.Any())
               {
                  ErrorView errorView = new ErrorView();
                  errorView.ID = key;

                  var errorObj = new Dictionary<string, string>
                  {
                     { ErrorObjectKey.Message.ToString(), state.Errors.First().ErrorMessage               },
                  };

                  errorView.ErrorObj = errorObj;

                  errors.Add(errorView);
                  //baseResult.msg += errorView.ErrorName + "-" + errorView.Error;
               }
            }
            baseResult.Errors = errors;
            actionContext.Result = new ContentResult
            {
               Content = JsonConvert.SerializeObject(baseResult),
               ContentType = "application/json"
            };
         }
      }
   }
}
