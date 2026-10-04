using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EC226.Configuration;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.Entities.Expression;
using EC226.MemberSite.Model.FromBody;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Filter;
using EC226.Loging;
using EC226.Utility.Helper;
using EC226.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;
using EC226.MemberSite.Model.ViewModels.Basics;

namespace EC226.MemberSite.Api
{
    /// <summary>
    /// {{ModelDescription}}
    ///</summary>
    [Description("{{ModelDescription}}")]
    [Route("api/[controller]/[action]")]
    [ApiController]    
    // [Authorize]
    public class {{ModelClassName}}Controller : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly I{{ModelClassName}}Services _{{ModelClassName}}Services;

        /// <summary>
        /// 建構函式
        ///</summary>
        public {{ModelClassName}}Controller(IWebHostEnvironment webHostEnvironment
            ,I{{ModelClassName}}Services {{ModelClassName}}Services
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _{{ModelClassName}}Services = {{ModelClassName}}Services;
        }                                     
    }
}
