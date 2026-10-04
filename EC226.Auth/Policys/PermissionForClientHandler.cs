
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EC226.Utility.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EC226.Auth.Policys
{
    /// <summary>
    /// 許可權授權處理器
    /// </summary>
    public class PermissionForClientHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 驗證方案提供對像
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// 建構函式注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="navigationRepository"></param>
        /// <param name="accessor"></param>
        public PermissionForClientHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            Schemes = schemes;
        }

        // 重寫非同步處理程式
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            var httpContext = _accessor.HttpContext;

            if (!requirement.Permissions.Any())
            {
                //var data = await _navigationRepository.QueryAsync();
                //var list = (from item in data
                //            where item.isLock == false
                //            orderby item.id
                //            select new PermissionItem
                //            {
                //                Url = item.linkUrl,
                //                Role = item.identificationCode,
                //            }).ToList();
                //requirement.Permissions = list;
            }

            //請求Url
            if (httpContext != null)
            {
                var questUrl = httpContext.Request.Path.Value.ToLower();
                //判斷請求是否停止
                var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
                {
                    if (await handlers.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
                    {
                        context.Fail();
                        return;
                    }
                }
                //判斷請求是否擁有憑據，即有沒有登錄
                var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    //result?.Principal不為空即登錄成功
                    if (result?.Principal != null)
                    {
                        // 將最新的角色和介面列表更新

                        // 這裡暫時把程式碼移動到了Login取得token的api里,這樣就不用每次都請求資料庫,造成壓力.
                        // 但是這樣有個問題,就是如果修改了某一個角色的菜單許可權,不會立刻更新,
                        // 需要讓使用者退出重新登錄,如果你想實時更新,請把下邊的註釋打開即可.

                        //var data = await _roleModulePermissionServices.RoleModuleMaps();
                        //var list = (from item in data
                        //            where item.IsDeleted == false
                        //            orderby item.Id
                        //            select new PermissionItem
                        //            {
                        //                Url = item.Module?.LinkUrl,
                        //                Role = item.Role?.Name,
                        //            }).ToList();
                        //requirement.Permissions = list;

                        httpContext.User = result.Principal;

                        //許可權中是否存在請求的url
                        //if (requirement.Permissions.GroupBy(g => g.Url).Where(w => w.Key?.ToLower() == questUrl).Count() > 0)
                        //if (isMatchUrl)
                        if (true)
                        {
                            // 取得目前使用者的角色資訊
                            var currentUserRoles = (from item in httpContext.User.Claims
                                                    where item.Type == requirement.ClaimType
                                                    select item.Value).ToList();

                            var isMatchRole = false;
                            var permisssionRoles = requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role));
                            foreach (var item in permisssionRoles)
                            {
                                try
                                {
                                    if (Regex.Match(questUrl, item.Url?.ObjectToString().ToLower())?.Value == questUrl)
                                    {
                                        isMatchRole = true;
                                        break;
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }

                            //驗證許可權
                            //if (currentUserRoles.Count <= 0 || requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role) && w.Url.ToLower() == questUrl).Count() <= 0)
                            if (currentUserRoles.Count <= 0 || !isMatchRole)
                            {
                                context.Fail();
                                return;
                            }
                        }

                        //判斷過期時間（這裡僅僅是最壞驗證原則，你可以不要這個if else的判斷，因為我們使用的官方驗證，Token過期後上邊的result?.Principal 就為 null 了，進不到這裡了，因此這裡其實可以不用驗證過期時間，只是做最後嚴謹判斷）
                        if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                            return;
                        }
                        return;
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                }
                else
                {
                    context.Fail();
                    return;
                }
                //判斷沒有登錄時，是否訪問登錄的url,並且是Post請求，並且是form表單提交型別，否則為失敗
                //if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasJsonContentType()))
                //{
                //    context.Fail();
                //    return;
                //}
            }

            context.Succeed(requirement);
        }
    }
}
