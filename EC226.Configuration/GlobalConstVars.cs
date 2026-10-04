using System;

namespace EC226.Configuration
{
    public class GlobalConstVars
    {
        /// <summary>
        /// 數據刪除成功
        /// </summary>
        public const string DeleteSuccess = "數據刪除成功";
        /// <summary>
        /// 數據刪除失敗
        /// </summary>
        public const string DeleteFailure = "數據刪除失敗";
        /// <summary>
        /// 系統禁止刪除此數據
        /// </summary>
        public const string DeleteProhibitDelete = "系統禁止刪除此數據";
        /// <summary>
        /// 此數據含有子類資訊，禁止刪除
        /// </summary>
        public const string DeleteIsHaveChildren = "此數據含有子類資訊，禁止刪除";
        /// <summary>
        /// 數據處理異常
        /// </summary>
        public const string DataHandleEx = "數據接口出現異常";
        /// <summary>
        /// 數據新增成功
        /// </summary>
        public const string CreateSuccess = "數據新增成功";
        /// <summary>
        /// 數據新增失敗
        /// </summary>
        public const string CreateFailure = "數據新增失敗";
        /// <summary>
        /// 數據移動成功
        /// </summary>
        public const string MoveSuccess = "數據移動成功";
        /// <summary>
        /// 數據移動失敗
        /// </summary>
        public const string MoveFailure = "數據移動失敗";
        /// <summary>
        /// 系統禁止新增數據
        /// </summary>
        public const string CreateProhibitCreate = "系統禁止新增數據";
        /// <summary>
        /// 數據編輯成功
        /// </summary>
        public const string EditSuccess = "數據編輯成功";
        /// <summary>
        /// 數據編輯失敗
        /// </summary>
        public const string EditFailure = "數據編輯失敗";
        /// <summary>
        /// 系統禁止編輯此數據
        /// </summary>
        public const string EditProhibitEdit = "系統禁止編輯此數據";
        /// <summary>
        /// 數據已存在
        /// </summary>
        public const string DataIsHave = "數據已存在";
        /// <summary>
        /// 數據不存在
        /// </summary>
        public const string DataisNo = "數據不存在";
        /// <summary>
        /// 請提交必要的參數
        /// </summary>
        public const string DataParameterError = "請提交必要的參數";
        /// <summary>
        /// 數據插入成功
        /// </summary>
        public const string InsertSuccess = "數據插入成功！";
        /// <summary>
        /// 數據插入失敗
        /// </summary>
        public const string InsertFailure = "數據插入失敗！";
        /// <summary>
        /// Excel資料庫失敗
        /// </summary>
        public const string ExcelExportFailure = "Excel資料庫失敗";
        /// <summary>
        /// Excel資料庫成功
        /// </summary>
        public const string ExcelExportSuccess = "Excel資料庫成功";
        /// <summary>
        /// 取得數據成功
        /// </summary>
        public const string GetDataSuccess = "取得數據成功！";
        /// <summary>
        /// 取得數據異常
        /// </summary>
        public const string GetDataException = "取得數據異常！";
        /// <summary>
        /// 取得數據失敗
        /// </summary>
        public const string GetDataFailure = "取得數據失敗！";
        /// <summary>
        /// 設定數據成功
        /// </summary>
        public const string SetDataSuccess = "設定數據成功！";
        /// <summary>
        /// 設定數據異常
        /// </summary>
        public const string SetDataException = "設定數據異常！";
        /// <summary>
        /// 設定數據失敗
        /// </summary>
        public const string SetDataFailure = "設定數據失敗！";            
      
        /// <summary>
        /// 使用者AccessToken有效期
        /// </summary>
        public const string CookieOAuthAccessTokenEndTime = "CookieOAuthAccessTokenEndTime";

      public static string GetCurrentDirectory()
      {
#if DEBUG
         //return AppContext.BaseDirectory;
         return System.Environment.CurrentDirectory;
#endif
         return System.Environment.CurrentDirectory;
      }


      #region  CMS 配置參數
      /// <summary>
      /// 版本號
      /// </summary>
      public const string Version = "1.5.6";
      /// <summary>
      /// 介面域名目錄
      /// </summary>
      /// <param name="configuration"></param>
      public const string RootUrl = "/api/";
      #endregion

   }

}

/// <summary>
/// RedisMqKey佇列
/// </summary>
public static class RedisMessageQueueKey
    {
        /// <summary>
        /// 微信支付成功后推送到介面進行數據處理
        /// </summary>
        public const string WeChatPayNotice = "WeChatPayNoticeQueue";
        /// <summary>
        /// 微信模板訊息
        /// </summary>
        public const string SendWxTemplateMessage = "SendWxTemplateMessage";

        /// <summary>
        /// 訂單完結後走代理或分銷商提成處理
        /// </summary>
        public const string OrderAgentOrDistribution = "OrderAgentOrDistributionQueue";
        /// <summary>
        /// 訂單完成時，結算該訂單
        /// </summary>
        public const string OrderFinishCommand = "OrderFinishCommandQueue";
        /// <summary>
        /// 訂單完成時，門店訂單自動發貨
        /// </summary>
        public const string OrderAutomaticDelivery = "OrderAutomaticDeliveryQueue";
        /// <summary>
        /// 訂單完結後走列印模組
        /// </summary>
        public const string OrderPrint = "OrderPrintQueue";
        /// <summary>
        /// 售後審覈通過後處理
        /// </summary>
        public const string AfterSalesReview = "AfterSalesReview";

        /// <summary>
        /// 日誌佇列
        /// </summary>
        public const string LogingQueue = "LogingQueue";
        /// <summary>
        /// 簡訊發送佇列
        /// </summary>
        public const string SmsQueue = "SmsQueue";

        //使用者相關

        //訂單支付成功后，使用者升級處理
        public const string UserUpGrade = "UserUpGradeQueue";


  
   /// <summary>
   /// 許可權變數配置
   /// </summary>
   public static class Permissions
    {
        public const string Name = "Permission";

        /// <summary>
        /// 目前專案是否啟用IDS4許可權方案
        /// true：表示啟動IDS4
        /// false：表示使用JWT
        public static bool IsUseIds4 = false;
    }

    /// <summary>
    /// 路由變數字首配置
    /// </summary>
    public static class RoutePrefix
    {
        /// <summary>
        /// 字首名
        /// 如果不需要，儘量留空，不要修改
        /// 除非一定要在所有的 api 前統一加上特定字首
        /// </summary>
        public const string Name = "";
    }

}
