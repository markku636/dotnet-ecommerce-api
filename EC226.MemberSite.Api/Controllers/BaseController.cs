using EC226.Utility.JWT;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EC226.MemberSite.Api.Controllers
{
   //[EnableCors("CorsPolicy")]
   public class BaseController: ControllerBase
   {
      protected int GetMemberId()
      {
         int memberId = 0;
         try
         {
            var memberPayloadObject = HttpContext.Items["member"];
            if (memberPayloadObject != null)
            {

               var memberPayload = (MemberPayLoadData)memberPayloadObject;
               return memberPayload.MemberId;
            }
         }
         catch (Exception ex)
         {

         }

         return memberId;
      }

   }
}
