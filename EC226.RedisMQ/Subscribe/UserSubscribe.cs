
using System;
using System.Threading.Tasks;
using EC226.Configuration;

using EC226.Loging;
using InitQ.Abstractions;
using InitQ.Attributes;
using Newtonsoft.Json;
using SqlSugar;

namespace EC226.RedisMQ.Subscribe
{
    /// <summary>
    /// 使用者相關佇列操作
    /// </summary>
    public class UserSubscribe : IRedisSubscribe
    {

        //private readonly IEC226UserServices _userServices;
        //private readonly IEC226OrderServices _orderServices;
        //private readonly IEC226BillRefundServices _billRefundServices;
        //private readonly IEC226UserGradeServices _userGradeServices;


        //public UserSubscribe(IEC226UserServices userServices, IEC226OrderServices orderServices, IEC226BillRefundServices billRefundServices, IEC226UserGradeServices userGradeServices)
        public UserSubscribe()
        {
            //_userServices = userServices;
            //_orderServices = orderServices;
            //_billRefundServices = billRefundServices;
            //_userGradeServices = userGradeServices;
        }

        /// <summary>
        /// 訂單完成-使用者升級處理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Subscribe(RedisMessageQueueKey.UserUpGrade)]
        private async Task UserUpGradeHandler(string msg)
        {
            try
            {
                //var orderModel = JsonConvert.DeserializeObject<EC226Order>(msg);

                //if (orderModel == null)
                //{
                //    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "訂單完成-使用者升級處理", "訂單數據取得失敗");
                //    return;
                //}

                //var userInfo = await _userServices.QueryPageAsync(p => p.id == orderModel.userId);
                //if (userInfo == null)
                //{
                //    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "訂單完成-使用者升級處理", "用戶數據取得失敗");
                //    return;
                //}

                ////訂單支付的金額
                //var payedMoney = await _orderServices.GetSumAsync(
                //    p => p.payStatus != (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT && p.userId == orderModel.userId,
                //    p => p.orderAmount);

                ////訂單退款金額
                //var refundMoney = await _billRefundServices.GetSumAsync(
                //    p => p.type == (int)GlobalEnumVars.BillRefundType.Order && p.userId == orderModel.userId &&
                //         p.status != (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUND, p => p.money);

                //var money = payedMoney - refundMoney;

                ////取所有使用者等級資訊

                //var userGradeModel = await _userGradeServices.QueryListByClauseAsync(p => p.id > 0, p => p.id, OrderByType.Asc);

                ////var id = 0;

                //foreach (var item in userGradeModel)
                //{

                //}

                //NLogUtil.WriteAll($"訂單完成-使用者升級處理-{msg}", NLog.LogLevel.Info, LogType.RedisMessageQueue);
            }
            catch (Exception ex)
            {
                //NLogUtil.WriteAll($"訂單完成-使用者升級處理-{msg}", NLog.LogLevel.Error, LogType.RedisMessageQueue, ex);
                throw;
            }
            await Task.CompletedTask;
        }


    }
}
