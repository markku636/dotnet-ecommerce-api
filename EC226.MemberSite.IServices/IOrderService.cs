using System.Collections.Generic;
using System.Threading.Tasks;
using EC226.Pagination;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.MemberSite.IServices
{
   public interface IOrderService
   {
      /// <summary>
      /// 获取会员的订单列表
      /// </summary>
      /// <param name="memberId">会员ID</param>
      /// <returns>包含订单列表的结果</returns>
      Task<Result<List<OrderVM>>> GetWebPortalOrderByMemberIdAsync(int memberId);

      /// <summary>
      /// 根据订单号和会员ID获取订单详情
      /// </summary>
      /// <param name="orderNo">订单号</param>
      /// <param name="memberId">会员ID</param>
      /// <returns>包含订单详情的结果</returns>
      Task<Result<OrderVM>> GetWebPortalOrderDetailByOrderNoAsync(string orderNo, int memberId);

      /// <summary>
      /// 分页获取订单列表
      /// </summary>
      /// <param name="model">分页模型</param>
      /// <returns>包含分页结果的订单列表</returns>
      Task<Result<PageModel<OrderVM>>> PagedListResultAsync(PageModel model);

      /// <summary>
      /// 根据订单号获取订单详情
      /// </summary>
      /// <param name="orderNo">订单号</param>
      /// <returns>包含订单详情的结果</returns>
      Task<Result<OrderVM>> GetOrderDetailByOrderNoAsync(string orderNo);
   }
}
