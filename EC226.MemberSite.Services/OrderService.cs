
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC226.Loging;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Pagination;
using EC226.Utility.Helper;
using EC226.Utility.Library.Extensions;
using Microsoft.EntityFrameworkCore;


namespace EC226.MemberSite.Services
{
   public class OrderService: BaseServices<Order>, IOrderService
   {

      private readonly IOrderRepository _dal;

      private readonly IUnitOfWork _unitOfWork;
      public OrderService(IUnitOfWork unitOfWork, IOrderRepository dal)
      {
         this._dal = dal;
         base.BaseDal = dal;
         _unitOfWork = unitOfWork;
      }

      public async Task<Result<PageModel<OrderVM>>> PagedListResultAsync(PageModel model)
      {
         var result = new Result<PageModel<OrderVM>>();

         try
         {
            var orderVMs = new List<OrderVM>();

            var query = this._unitOfWork.GetDbClient().Queryable<Order>().OrderByDescending(x => x.Date).Includes(x => x.OrderDetails)
                .WhereAndOrder(model)
                .OrderBy(model.Order);

            var orders = await query.Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            AutoMapperInstance.ConvertModel(orders, orderVMs);

            result.Data = model.Record(orderVMs, count)
                .SetPrimaryKey(x => x.Id)
                .ColumFor(x => x.OrderNo, "", 100)
                .ColumFor(x => x.RecipientName, "", 150)
                .ColumFor(x => x.Date)
                .ColumFor(x => x.FinalAmount, "", 120)
                .ColumFor(x => x.Status, "", 100);
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<List<OrderVM>>> GetWebPortalOrderByMemberIdAsync(int memberId)
      {
         var result = new Result<List<OrderVM>>();

         try
         {
            var orderVMs = new List<OrderVM>();

            var orders = await this._unitOfWork.GetDbClient().Queryable<Order>()
                .Where(x => x.MemberId == memberId)
                .OrderByDescending(x => x.Date)
                .Includes(x => x.OrderDetails)
                .ToListAsync();

            AutoMapperInstance.ConvertModel(orders, orderVMs);

            result.Data = orderVMs;
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<OrderVM>> GetOrderDetailByOrderNoAsync(string orderNo)
      {
         var result = new Result<OrderVM>();

         try
         {
            Guard.Against<Exception>(string.IsNullOrWhiteSpace(orderNo), "查詢參數錯誤");

            var orderVMs = new OrderVM();

            var orders = await this._unitOfWork.GetDbClient().Queryable<Order>()
                .Where(x => x.OrderNo == orderNo)
                .Includes(x => x.OrderDetails)
                .FirstAsync();

            var spuIds = orders.OrderDetails.Select(x => x.StandardProductUnitId).ToList();

            var spuImages = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .Where(x => spuIds.Contains(x.Id)).ToListAsync();

            var spuImageDic = spuImages.ToDictionary(x => x.Id, y => y.CoverUrl);

            var payment = await this._unitOfWork.GetDbClient().Queryable<EC226.MemberSite.Model.Entities.Payment>()
                .Where(x => x.Id == orders.PaymentId && x.PaymentOptions.Any(y => y.Id == orders.PaymentOptionId)).Includes(x => x.PaymentOptions)
                .FirstAsync();

            AutoMapperInstance.ConvertModel(orders, orderVMs);

            if (payment != null)
            {
               orderVMs.PaymentMethod = $@"{payment.DisplayName}-{payment.PaymentOptions.FirstOrDefault(y => y.Id == orders.PaymentOptionId)?.DisplayName}";
            }

            foreach (var item in orderVMs.OrderDetails)
            {
               if (spuImageDic.ContainsKey(item.StandardProductUnitId))
               {
                  item.CoverUrl = spuImageDic[item.StandardProductUnitId];
               }
            }



            result.Data = orderVMs;
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<OrderVM>> GetWebPortalOrderDetailByOrderNoAsync(string orderNo, int memberId)
      {
         var result = new Result<OrderVM>();

         try
         {
            Guard.Against<Exception>(string.IsNullOrWhiteSpace(orderNo) || memberId == 0, "查詢參數錯誤");

            var orderVMs = new OrderVM();

            var orders = await this._unitOfWork.GetDbClient().Queryable<Order>()
                .Where(x => x.MemberId == memberId && x.OrderNo == orderNo).Includes(x => x.OrderDetails)
                .FirstAsync();

            var spuIds = orders.OrderDetails.Select(x => x.StandardProductUnitId).ToList();

            var spuImages = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .Where(x => spuIds.Contains(x.Id)).ToListAsync();

            var spuImageDic = spuImages.ToDictionary(x => x.Id, y => y.CoverUrl);

            var payment = await this._unitOfWork.GetDbClient().Queryable<EC226.MemberSite.Model.Entities.Payment>()
                .Where(x => x.Id == orders.PaymentId && x.PaymentOptions.Any(y => y.Id == orders.PaymentOptionId))
                .Includes(x => x.PaymentOptions)
                .FirstAsync();

            AutoMapperInstance.ConvertModel(orders, orderVMs);

            if (payment != null)
            {
               orderVMs.PaymentMethod = $@"{payment.DisplayName}-{payment.PaymentOptions.FirstOrDefault(y => y.Id == orders.PaymentOptionId)?.DisplayName}";
            }

            foreach (var item in orderVMs.OrderDetails)
            {
               if (spuImageDic.ContainsKey(item.StandardProductUnitId))
               {
                  item.CoverUrl = spuImageDic[item.StandardProductUnitId];
               }
            }

            result.Data = orderVMs;
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }
   }
}
