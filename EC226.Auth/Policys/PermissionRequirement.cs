using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace EC226.Auth.Policys
{
    /// <summary>
    /// 必要參數類
    /// 繼承 IAuthorizationRequirement，用於設計自定義許可權處理器PermissionHandler
    /// 因為AuthorizationHandler 中的泛型參數 TRequirement 必須繼承 IAuthorizationRequirement
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 使用者許可權集合，一個訂單包含了很多詳情，
        /// 同理，一個網站的認證發行中，也有很多許可權詳情(這裡是Role和URL的關係)
        /// </summary>
        public List<PermissionItem> Permissions { get; set; }
        /// <summary>
        /// 無許可權action
        /// </summary>
        public string DeniedAction { get; set; }

        /// <summary>
        /// 認證授權型別
        /// </summary>
        public string ClaimType { internal get; set; }
        /// <summary>
        /// 請求路徑
        /// </summary>
        public string LoginPath { get; set; } = "/Api/Login";
        /// <summary>
        /// 發行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 訂閱人
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 過期時間
        /// </summary>
        public TimeSpan Expiration { get; set; }
        /// <summary>
        /// 簽名驗證
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }


        /// <summary>
        /// 構造
        /// </summary>
        /// <param name="deniedAction">拒約請求的url</param>
        /// <param name="permissions">許可權集合</param>
        /// <param name="claimType">聲明型別</param>
        /// <param name="issuer">發行人</param>
        /// <param name="audience">訂閱人</param>
        /// <param name="signingCredentials">簽名驗證實體</param>
        /// <param name="expiration">過期時間</param>
        public PermissionRequirement(string deniedAction, List<PermissionItem> permissions, string claimType, string issuer, string audience, SigningCredentials signingCredentials, TimeSpan expiration)
        {
            ClaimType = claimType;
            DeniedAction = deniedAction;
            Permissions = permissions;
            Issuer = issuer;
            Audience = audience;
            Expiration = expiration;
            SigningCredentials = signingCredentials;
        }
    }
}
