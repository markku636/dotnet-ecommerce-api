using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace EC226.Auth.HttpContextUser
{
    public interface IHttpContextUser
    {
        string Name { get; }
        int ID { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
        List<string> GetClaimValueByType(string ClaimType);

        string GetToken();
        List<string> GetUserInfoFromToken(string ClaimType);
    }
}
