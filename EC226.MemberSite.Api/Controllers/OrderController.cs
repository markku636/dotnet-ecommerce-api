using System.Collections.Generic;
using System.Threading.Tasks;
using EC226.Pagination;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.Api.Controllers
{
   [Route("order")]
   public class OrderController: BaseController
   {
      private readonly IOrderService _orderService;

      public OrderController(IOrderService orderService)
      {
         this._orderService = orderService;
      }

      [Route("list")]
      [HttpGet]
      public async Task<Result<List<OrderVM>>> GetOrderAsync()
      {
         return await _orderService.GetWebPortalOrderByMemberIdAsync(GetMemberId());
      }

      [Route("detail")]
      [HttpGet]
      public async Task<Result<OrderVM>> GetOrderDetailAsync(string orderNo)
      {
         return await _orderService.GetWebPortalOrderDetailByOrderNoAsync(orderNo, GetMemberId());
      }
   }
}
