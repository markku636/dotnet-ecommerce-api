using EC226.Core.Attribute;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.Api.Controllers
{
   public class TestController: Controller
   {

      /// <summary>
      /// 測試Ping
      /// </summary>
      [HttpGet("/api/[controller]/Ping")]
      [AllowAnonymous]
      [Description("Ping")]      
      public IActionResult Ping()
      {
         return Ok("Pone");
      }
   }
}
