using System;
using System.Collections.Generic;

namespace EC226.Payment.Abstractions
{
    /// <summary>
    /// 統一付款請求模型
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// 訂單編號 (唯一值)
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 商品描述/交易描述
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// 商品名稱 (多筆以 # 分隔)
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 買家 Email
        /// </summary>
        public string BuyerEmail { get; set; }

        /// <summary>
        /// 買家姓名
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// 買家電話
        /// </summary>
        public string BuyerPhone { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public PaymentMethodType PaymentMethod { get; set; } = PaymentMethodType.ALL;

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TradeDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 自訂欄位 1
        /// </summary>
        public string CustomField1 { get; set; }

        /// <summary>
        /// 自訂欄位 2
        /// </summary>
        public string CustomField2 { get; set; }

        /// <summary>
        /// 自訂欄位 3
        /// </summary>
        public string CustomField3 { get; set; }

        /// <summary>
        /// 自訂欄位 4
        /// </summary>
        public string CustomField4 { get; set; }

        /// <summary>
        /// 額外資料 (依金流商需求)
        /// </summary>
        public Dictionary<string, string> ExtraData { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 交易有效秒數 (0 表示不限制)
        /// </summary>
        public int TradeLimit { get; set; } = 0;

        /// <summary>
        /// 繳費期限 (非即時交易用，如 ATM/CVS)
        /// </summary>
        public DateTime? ExpireDate { get; set; }
    }

    /// <summary>
    /// 統一付款方式類型
    /// </summary>
    public enum PaymentMethodType
    {
        /// <summary>
        /// 不指定 (顯示所有付款方式)
        /// </summary>
        ALL,

        /// <summary>
        /// 信用卡
        /// </summary>
        Credit,

        /// <summary>
        /// 網路 ATM
        /// </summary>
        WebATM,

        /// <summary>
        /// ATM 轉帳
        /// </summary>
        ATM,

        /// <summary>
        /// 超商代碼繳費
        /// </summary>
        CVS,

        /// <summary>
        /// 超商條碼繳費
        /// </summary>
        BARCODE,

        /// <summary>
        /// 銀聯卡
        /// </summary>
        UnionPay,

        /// <summary>
        /// Google Pay
        /// </summary>
        GooglePay,

        /// <summary>
        /// Samsung Pay
        /// </summary>
        SamsungPay
    }
}
