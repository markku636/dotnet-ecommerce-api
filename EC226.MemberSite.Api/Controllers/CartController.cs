using System.Collections.Generic;
using System.Threading.Tasks;
using EC226.Pagination;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.Api.Controllers
{
   [Route("cart")]
   public class CartController: BaseController
   {
      private readonly ICartService _cartService;

      public CartController(ICartService iCartService)
      {
         this._cartService = iCartService;
      }

      [Route("check")]
      [HttpPost]
      public async Task<Result<List<CheckCartResultVM>>> CheckAvaibleStockAsync([FromBody] List<CheckCartItem> cartItems)
      {
         return await _cartService.CheckAvaibleStockAsync(cartItems);
      }

      [HttpPost]
      [Route("checkout")]
      public async Task<Result> CheckOutAsync([FromBody] CheckoutRequest checkoutRequest)
      {
         return await _cartService.CheckOutAsync(checkoutRequest, this.GetMemberId());
      }
   }
}
