using System.Threading.Tasks;
using System.Collections.Generic;
using EC226.Pagination;
using EC226.MemberSite.Api.Attributes;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EC226.MemberSite.Api.Controllers
{
   [Route("member")]
   public class MemberController: BaseController
   {
      private readonly IMemberService _memberService;

      public MemberController(IMemberService memberService)
      {
         _memberService = memberService;
      }

      [Route("register")]
      [HttpPost]
      [SkipLogParams("*.Password")]
      public async Task<Result<LoginTokenVM>> Register([FromBody] RegisterMemberVM register)
      {
         return await _memberService.RegisterAsync(register);
      }

      [Route("login")]
      [HttpPost]
      [SkipLogParams("*.Password")]
      public async Task<Result<LoginTokenVM>> Login([FromBody] LoginVM login)
      {
         return await _memberService.LoginAsync(login);
      }

      [Route("slogin")]
      [HttpPost]
      [SkipLogParams("*.Password")]
      public async Task<Result<LoginTokenVM>> RegisterAndLogin([FromBody] SocialNetworkLogin sLogin)
      {
         return await _memberService.SocialNetworkLoginAsync(sLogin);
      }

      [Route("memberInfo")]
      [HttpGet]
      public async Task<Result<MemberInfoVM>> GetMemberInfo()
      {
         return await _memberService.GetMemberInfoAsync(this.GetMemberId());
      }

      [Route("getProfile")]
      [HttpGet]
      public async Task<Result<MemberProfileVM>> GetMemberProfile()
      {
         return await _memberService.GetMemberProfileAsync(this.GetMemberId());
      }

      [Route("updateProfile")]
      [HttpPost]
      public async Task<Result> UpdateMemberInfo([FromBody] UpdateProfilerVM updateProfilerVM)
      {
         return await _memberService.UpdateMemberInfoAsync(this.GetMemberId(), updateProfilerVM);
      }

      [Route("updateDefaultAddress")]
      [HttpPost]
      public async Task<Result> UpdateDefaultAddress([FromBody] RecipienAddressVM addressVM)
      {
         return await _memberService.UpdateDefaultAddressAsync(this.GetMemberId(), addressVM);
      }
   }
}
