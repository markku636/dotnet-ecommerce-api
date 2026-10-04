using System.Collections.Generic;
using EC226.Pagination;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.Api.Controllers
{
    [Route("health")]
    public class HealthController : BaseController
    {
        private IMemberService _memberService;
        public HealthController()
        {

        }

        [HttpGet]
        [Route("check")]
        public IActionResult CheckHealth()
        {
            return Ok();
        }
    }
}
