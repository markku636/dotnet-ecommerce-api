namespace EC226.MemberSite.Model.ViewModels
{
    /// <summary>
    /// 銀行端基本資訊
    /// </summary>
    public class BankInfoModel
    {
        /// <summary>
        /// 商店代號
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// AES 加密/SHA256 加密 Key
        /// </summary>
        public string HashKey { get; set; }

        /// <summary>
        /// AES 加密/SHA256 加密 IV
        /// </summary>
        public string HashIV { get; set; }

        /// <summary>
        /// 支付完成 返回商店網址
        /// <para>1.交易完成後，以 Form Post 方式導回商店頁面。</para>
        /// <para>2.若為空值，交易完成後，消費者將停留在智付通付款或取號完成頁面。</para>
        /// <para>3.只接受80與443 Port。</para>
        /// </summary>
        public string ReturnURL { get; set; }

        /// <summary>
        /// 支付通知網址
        /// <para>1.以幕後方式回傳給商店相關支付結果資料</para>
        /// <para>2.只接受80與443 Port。</para>
        /// </summary>
        public string NotifyURL { get; set; }
        
        /// <summary>
        /// 商店取號網址
        /// <para>1.系統取號後以 form post 方式將結果導回商店指定的網址</para>
        /// <para>2.此參數若為空值，則會顯示取號結果在智付通頁面。</para>
        /// </summary>
        public string CustomerURL { get; set; }

        /// <summary>
        /// 授權網址
        /// </summary>
        public string AuthUrl { get; set; }

        /// <summary>
        /// (取消)請退款網址
        /// </summary>
        public string CloseUrl { get; set; }

    }
}