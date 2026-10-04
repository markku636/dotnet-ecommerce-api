using EC226.MemberSite.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.MemberSite.IServices
{
   public interface ICartService
   {
      /// <summary>
      /// Checks the available stock for the given cart items.
      /// </summary>
      /// <param name="cartItems">List of cart items to check.</param>
      /// <returns>A task that represents the asynchronous operation. The task result contains a list of check cart results.</returns>
      Task<Result<List<CheckCartResultVM>>> CheckAvaibleStockAsync(List<CheckCartItem> cartItems);

      /// <summary>
      /// Processes the checkout request.
      /// </summary>
      /// <param name="checkoutRequest">The checkout request details.</param>
      /// <param name="memberId">The member ID initiating the checkout.</param>
      /// <returns>A task that represents the asynchronous operation. The task result contains the payment data.</returns>
      Task<Result<PaymentData>> CheckOutAsync(CheckoutRequest checkoutRequest, int memberId);
   }
}
