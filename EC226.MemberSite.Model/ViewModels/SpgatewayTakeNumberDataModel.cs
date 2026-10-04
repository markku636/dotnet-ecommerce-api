namespace EC226.MemberSite.Model.ViewModels
{
    /// <summary>
    /// 智付通取號完成系統回傳解密後資料
    /// <para>非即時交易支付方式：ATM 轉帳(VACC)、超商代碼繳費(CVS) 、超商條碼繳費(BARCODE)、超商取貨付款(CVSCOM)。</para>
    /// </summary>
    public class SpgatewayTakeNumberDataModel
    {

        /// <summary>
        /// 回傳狀態
        /// <para>1.若交易付款成功，則回傳SUCCESS。</para>
        /// <para>2.若交易付款失敗，則回傳錯誤代碼。</para>
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 回傳訊息(敘述此次交易狀態。)
        /// </summary>
        public string Message { get; set; }

        #region ATM 轉帳、超商代碼繳費、超商條碼繳費共同回傳參數

        /// <summary>
        /// 商店代號(智付通商店代號)
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int Amt { get; set; }

        /// <summary>
        /// 智付通交易序號
        /// <para>智付通在此筆交易取號成功時所產生的序號。</para>
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 商店訂單編號
        /// <para>商店自訂訂單編號。</para>
        /// </summary>
        public string MerchantOrderNo { get; set; }

        /// <summary>
        /// 支付方式
        /// <para>CREDIT 信用卡         即時交易</para>
        /// <para>WEBATM WebATM         即時交易</para>
        /// <para>VACC ATM轉帳        非即時交易</para>
        /// <para>CVS 超商代碼繳費   非即時交易</para>
        /// <para>BARCODE 超商條碼繳費   非即時交易</para>
        /// <para>CVSCOM 超商取貨付款   非即時交易</para>
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// 繳費截止日期
        /// <para>回傳格式為 yyyy-mm-dd。</para>
        /// <para>註:超商取貨付款不回傳此參數。</para>
        /// </summary>
        public string ExpireDate { get; set; }

        #endregion ATM 轉帳、超商代碼繳費、超商條碼繳費共同回傳參數

        #region ATM 轉帳回傳參數

        /// <summary>
        /// 金融機構代碼
        /// <para>1.若取號成功，此欄位呈現數值。</para>
        /// <para>2.若取號失敗，此欄位呈現空值。</para>
        /// </summary>
        public string BankCode { get; set; }

        #endregion

        #region ATM 轉帳、超商代碼繳費回傳參數

        /// <summary>
        /// 繳費代碼
        /// <para>1.若取號成功，此欄位呈現數值。</para>
        /// <para>2.若取號失敗，此欄位呈現空值。</para>
        /// </summary>
        public string CodeNo { get; set; }

        #endregion

        #region 超商條碼繳費回傳參數

        /// <summary>
        /// 第一段條碼
        /// <para>1.若取號成功，此欄位呈現數值。</para>
        /// <para>2.若取號失敗，此欄位呈現空值。</para>
        /// </summary>
        public string Barcode_1 { get; set; }

        /// <summary>
        /// 第二段條碼
        /// <para>1.若取號成功，此欄位呈現數值。</para>
        /// <para>2.若取號失敗，此欄位呈現空值。</para>
        /// </summary>
        public string Barcode_2 { get; set; }

        /// <summary>
        /// 第三段條碼
        /// <para>1.若取號成功，此欄位呈現數值。</para>
        /// <para>2.若取號失敗，此欄位呈現空值。</para>
        /// </summary>
        public string Barcode_3 { get; set; }

        #endregion

        #region 超商物流回傳參數

        /// <summary>
        /// 超商門市編號
        /// <para>取貨門市編號。</para>
        /// </summary>
        public string StoreCode { get; set; }

        /// <summary>
        /// 超商門市名稱
        /// <para>取貨門市中文名稱</para>
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 超商類別名稱
        /// <para>回傳[全家] 、[OK] 、[萊爾富]</para>
        /// </summary>
        public string StoreType { get; set; }

        /// <summary>
        /// 超商門市地址
        /// <para>取貨門市地址</para>
        /// </summary>
        public string StoreAddr { get; set; }

        /// <summary>
        /// 取件交易方式
        /// <para>1 = 取貨付款</para>
        /// <para>3 = 取貨不付款</para>
        /// </summary>
        public int? TradeType { get; set; }

        /// <summary>
        /// 取貨人
        /// <para>取貨人姓名</para>
        /// </summary>
        public string CVSCOMName { get; set; }

        /// <summary>
        /// 取貨人手機號碼
        /// <para>取貨人手機號碼</para>
        /// </summary>
        public string CVSCOMPhone { get; set; }

        #endregion
    }
}