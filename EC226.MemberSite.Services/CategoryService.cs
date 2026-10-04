
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EC226.Loging;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.Enums;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Pagination;
using EC226.Payment;
using EC226.Utility.Helper;
using EC226.Utility.Library.Extensions;
using Microsoft.EntityFrameworkCore;


namespace EC226.MemberSite.Services
{
   public class CartService:  ICartService
   {
      private readonly IComponentContext _context;

      private readonly IUnitOfWork _unitOfWork;
      public CartService(IUnitOfWork unitOfWork, IComponentContext context)
      {
         _unitOfWork = unitOfWork;
         this._context = context;
      }

      public async Task<Result<PaymentData>> CheckOutAsync(CheckoutRequest checkoutRequest, int memberId)
      {
         var result = new Result<PaymentData>();
         try
         {
            Guard.Against<Exception>(checkoutRequest == null, "參數不得為空!");

            var payment = await this._unitOfWork.GetDbClient().Queryable<EC226.MemberSite.Model.Entities.Payment>()
                .Where(x => x.Enable && x.Id == checkoutRequest.PaymentId)
                .Includes(x => x.PaymentOptions)
                .FirstAsync();

            var paymentOption = payment?.PaymentOptions.FirstOrDefault(x => x.Enable && x.Id == checkoutRequest.PaymentOptionId);

            Guard.Against<Exception>(payment == null || paymentOption == null, "找不到付款方式!");

            var paymentService = _context.ResolveNamed<IThirdPaymentService>(payment.Code);

            Guard.Against<Exception>(paymentService == null, "找不到付款 Service!");

            var orderNo = SequenceService.Current.GetNo(SequenceType.Order);

            var order = new Order
            {
               OrderDetails = new List<OrderDetail>(),
               RecipientName = checkoutRequest.RecipientName,
               Phone = checkoutRequest.Phone,
               Country = checkoutRequest.Country,
               ZipCode = checkoutRequest.ZipCode,
               Address = checkoutRequest.Address,
               Date = DateTime.Now,
               OrderNo = orderNo,
               Status = (int)OrderStatus.待付款,
               MemberId = memberId,
               PaymentId = checkoutRequest.PaymentId,
               PaymentOptionId = checkoutRequest.PaymentOptionId
            };

            var spuIds = checkoutRequest.cartItems.Select(x => x.StandardProductUnitId).ToList();

            var spuProducts = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .Where(x => spuIds.Contains(x.Id))
                .ToListAsync();

            var disableProductIds = spuProducts.Where(x => !x.Enable).Select(x => x.Id).ToList();

            var skuStockDetails = await this._unitOfWork.GetDbClient().Queryable<StockKeepingUnitStock>()
                .Where(x => spuIds.Contains(x.StandardProductUnitId) && !disableProductIds.Contains(x.StandardProductUnitId))
                .ToListAsync();

            var relations = await this._unitOfWork.GetDbClient().Queryable<SpecificationOptionRelation>()
                .Where(x => spuIds.Contains(x.StandardProductUnitId))
                .Includes(x=>x.SpecificationOption)
                .Includes(x => x.Specification)              
                .ToListAsync();

            var specListList = relations.Select(x => x.Specification);
            var specOptionList = relations.Select(x => x.SpecificationOption);

            var orderDetails = new List<OrderDetail>();
            var paymentItems = new List<Payment.PaymentItem>();

            foreach (var item in checkoutRequest.cartItems)
            {
               var stockDetail = skuStockDetails.SingleOrDefault(x => x.StandardProductUnitId == item.StandardProductUnitId && x.SKUs == item.Skus);
               var spuProduct = spuProducts.SingleOrDefault(x => x.Id == item.StandardProductUnitId);

               var skuItems = GetSkuSpecItemsBySkus(item.Skus);
               var skuName = skuItems.Aggregate(string.Empty, (current, skuItem) =>
               {
                  var spec = specListList.FirstOrDefault(x => x.Id == skuItem.SpecificationId);
                  var specOption = specOptionList.FirstOrDefault(x => x.Id == skuItem.SpecificationOptionId);

                  return current + $"{spec?.Name}:{specOption?.Name},";
               }).TrimEnd(',');

               if (stockDetail != null && spuProduct != null && !string.IsNullOrWhiteSpace(skuName))
               {
                  var orderDetail = new OrderDetail
                  {
                     SpuName = spuProduct.Name,
                     SpecOptionName = skuName,
                     StandardProductUnitId = item.StandardProductUnitId,
                     SKUs = item.Skus,
                     Quantity = item.Quantity,
                     Price = stockDetail.Price,
                     SubTotal = stockDetail.Price * item.Quantity
                  };
                  orderDetails.Add(orderDetail);

                  var paymentItem = new Payment.PaymentItem
                  {
                     Name = $"{orderDetail.SpuName}({orderDetail.SpecOptionName})",
                     Quantity = orderDetail.Quantity,
                     Price = orderDetail.Price
                  };
                  paymentItems.Add(paymentItem);
               }
               else
               {
                  throw new Exception("庫存不足");
               }
            }

            order.OrderDetails = orderDetails;
            order.FinalAmount = order.OrderDetails.Sum(x => x.SubTotal);

            this._unitOfWork.CommitTran();

            foreach (var item in order.OrderDetails)
            {
               var stock = await this._unitOfWork.GetDbClient().Queryable<StockKeepingUnitStock>()
                   .Where(x => x.StandardProductUnitId == item.StandardProductUnitId && x.SKUs == item.SKUs)
                   .SingleAsync();

               Guard.Against<Exception>(stock.Stock < item.Quantity, "庫存不足!");

               stock.Stock -= item.Quantity;

               await this._unitOfWork.GetDbClient().Updateable<StockKeepingUnitStock>(stock).ExecuteCommandAsync();
               
            }

            await this._unitOfWork.GetDbClient().Insertable<Order>(order).ExecuteCommandAsync();

            this._unitOfWork.CommitTran();

            var fullName = string.Join(",", order.OrderDetails.Select(x => x.SpuName + "(" + x.SpecOptionName + ")"));

            var paymentParameter = new PaymentParameter
            {
               IsNeedSubmitButton = true,
               OrderNo = orderNo,
               Description = fullName,
               Date = order.Date,
               PaymentMethod = PaymentMethod.Credit,
               Items = paymentItems,
               Amount = order.FinalAmount,
               ReceiverName = checkoutRequest.RecipientName,
               ReceiverCellPhone = checkoutRequest.Phone,
               PaymentOptionCode = paymentOption.Code,
               PaymentOptionSubCode = paymentOption.SubCode
            };

            var createPayFormResult = paymentService.CreatePayForm(paymentParameter);

            if (createPayFormResult)
            {
               result.Data = createPayFormResult.Data;
               result.Success();
            }
         }
         catch (Exception ex)
         {
            this._unitOfWork.CommitTran();
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }
         return result;
      }

      public async Task<Result<List<CheckCartResultVM>>> CheckAvaibleStockAsync(List<CheckCartItem> cartItems)
      {
         var result = new Result<List<CheckCartResultVM>>();
         try
         {
            var spuIds = cartItems.Select(x => x.StandardProductUnitId).ToList();

            var disableProducts = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .Where(x => spuIds.Contains(x.Id) && !x.Enable)
                .ToListAsync();

            var skuStockDetails = await this._unitOfWork.GetDbClient().Queryable<StockKeepingUnitStock>()
                .Where(x => spuIds.Contains(x.StandardProductUnitId))
                .ToListAsync();

            foreach (var item in cartItems)
            {
               var checkResult = new CheckCartResultVM
               {
                  StandardProductUnitId = item.StandardProductUnitId,
                  Skus = item.Skus
               };

               var exitistStock = skuStockDetails.FirstOrDefault(x => x.SKUs == item.Skus);
               var exitistDisableProduct = disableProducts.FirstOrDefault(x => x.Id == item.StandardProductUnitId);

               if (exitistStock != null)
               {
                  checkResult.AvableStock = exitistStock.Stock;
                  checkResult.IsEnoughStock = exitistStock.Stock > 0 && exitistStock.Stock >= item.Quantity;
               }

               checkResult.IsEnable = exitistDisableProduct == null;

               result.Data.Add(checkResult);
            }

            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }
         return result;
      }

      private List<SkuSpecItem> GetSkuSpecItemsBySkus(string strSKUs)
      {
         return strSKUs.Split(',')
             .Select(item =>
             {
                var sku = item.Split(':');
                return new SkuSpecItem
                {
                   SpecificationId = Convert.ToInt32(sku[0]),
                   SpecificationOptionId = Convert.ToInt32(sku[1])
                };
             }).ToList();
      }
   }
}
