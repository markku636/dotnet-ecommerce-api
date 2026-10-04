namespace EC226.MemberSite.Model.ViewModels
{
    public class TradeInfo
    {
        /// <summary>
        /// * 商店代號
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// * 回傳格式
        /// <para>JSON 或是 String</para>
        /// </summary>
        public string RespondType { get; set; }

        /// <summary>
        /// * TimeStamp
        /// <para>自從 Unix 纪元（格林威治時間 1970 年 1 月 1 日 00:00:00）到當前時間的秒數。</para>
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// * 串接程式版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 語系
        /// <para>英文版參數為en、繁體中文版參數為zh-tw</para>
        /// </summary>
        public string LangType { get; set; }

        /// <summary>
        /// * 商店訂單編號
        /// <para>限英、數字、_ 格式</para>
        /// <para>長度限制為20字</para>
        /// <para>同一商店中此編號不可重覆。</para>
        /// </summary>
        public string MerchantOrderNo { get; set; }

        /// <summary>
        /// * 訂單金額
        /// </summary>
        public decimal Amt { get; set; }

        /// <summary>
        /// * 商品資訊
        /// <para>1.限制長度為50字。</para>
        /// <para>2.編碼為Utf-8格式。</para>
        /// <para>3.請勿使用斷行符號、單引號等特殊符號，避免無法顯示完整付款頁面。</para>
        /// <para>4.若使用特殊符號，系統將自動過濾。</para>
        /// </summary>
        public string ItemDesc { get; set; }

        /// <summary>
        /// 交易限制秒數
        /// <para>1.限制交易的秒數，當秒數倒數至0時，交易當做失敗。</para>
        /// <para>2.僅可接受數字格式。</para>
        /// <para>3.秒數下限為60秒，當秒數介於1 ~59秒時，會以60秒計算。</para>
        /// <para>4.秒數上限為900秒，當超過900秒時，會以900秒計算。</para>
        /// <para>5.若未帶此參數，或是為0時，會視作為不啟用交易限制秒數。</para>
        /// </summary>
        public int TradeLimit { get; set; }

        /// <summary>
        /// 繳費有效期限(適用於非即時交易)
        /// <para>1.格式為 date('Ymd') ，例：20140620</para>
        /// <para>2.此參數若為空值，系統預設為7天。自取號時間起算至第7天23:59:59。 例：2014-06-23 14:35:51完成取號，則繳費有效期限為2014-06-29 23:59:59。</para>
        /// <para>3.可接受最大值為180天。</para>
        /// </summary>
        public string ExpireDate { get; set; }

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
        /// 支付取消 返回商店網址
        /// <para>1.當交易取消時，平台會出現返回鈕，使消費者依以此參數網址返回商店指定的頁面。</para>
        /// <para>2.此參數若為空值時，則無返回鈕。</para>
        /// </summary>
        public string ClientBackURL { get; set; }

        /// <summary>
        /// * 付款人電子信箱
        /// <para>於交易完成或付款完成時，通知付款人使用。</para>
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 付款人電子信箱 是否開放修改
        /// <para>1=可修改 0=不可修改</para>
        /// </summary>
        public int EmailModify { get; set; }

        /// <summary>
        /// * 智付通會員
        /// <para>0=不須登入智付通會員</para>
        /// <para>1=須要登入智付通會員</para>
        /// </summary>
        public int LoginType { get; set; }

        /// <summary>
        /// 商店備註
        /// <para>1.限制長度為300字。</para>
        /// <para>2.若有提供此參數，將會於MPG頁面呈現商店備註內容。</para>
        /// </summary>
        public string OrderComment { get; set; }

        /// <summary>
        /// 信用卡 一次付清啟用
        /// <para>設定是否啟用信用卡一次付清支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0或者未有此參數=不啟用</para>
        /// </summary>
        public int? CREDIT { get; set; }

        /// <summary>
        /// Google Pay 啟用
        /// <para>設定是否啟用Google Pay支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0或者未有此參數=不啟用</para>
        /// </summary>
        public int? ANDROIDPAY { get; set; }

        /// <summary>
        /// Samsung Pay 啟用
        /// <para>設定是否啟用Samsung Pay支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0或者未有此參數=不啟用</para>
        /// </summary>
        public int? SAMSUNGPAY { get; set; }

        /// <summary>
        /// 信用卡 分期付款啟用
        /// <para>1. 此欄位值=1 時，即代表開啟所有分期期別，且不可帶入其他期別參數。</para>
        /// <para>2. 此欄位值為下列數值時，即代表開啟該分期期別。(3=分 3 期功能、6=分 6 期功能、12=分 12 期功能、18=分 18 期功能、24=分 24 期功能、30=分 30 期功能)</para>
        /// <para>同時開啟多期別時，將此參數用"，"(半形)分隔，例如：3,6,12，代表開啟分3、6、12 期的功能。</para>
        /// <para>此欄位值=0或無值時，即代表不開啟分期。</para>
        /// </summary>
        public string InstFlag { get; set; }

        /// <summary>
        /// 信用卡 紅利啟用
        /// <para>設定是否啟用信用卡紅利支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0或者未有此參數=不啟用</para>
        /// </summary>
        public int? CreditRed { get; set; }

        /// <summary>
        /// 信用卡 銀聯卡啟用
        /// <para>設定是否啟用銀聯卡支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0或者未有此參數=不啟用</para>
        /// </summary>
        public int? UNIONPAY { get; set; }

        /// <summary>
        /// WEBATM啟用
        /// <para>設定是否啟用WEBATM支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0 或者未有此參數，即代表不開啟</para>
        /// </summary>
        public int? WEBATM { get; set; }

        /// <summary>
        /// ATM 轉帳啟用
        /// <para>設定是否啟用ATM 轉帳支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0 或者未有此參數，即代表不開啟</para>
        /// </summary>
        public int? VACC { get; set; }

        /// <summary>
        /// 超商代碼繳費啟用
        /// <para>設定是否啟用超商代碼繳費支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0 或者未有此參數，即代表不開啟</para>
        /// <para>當該筆訂單金額小於 30 元或超過 2 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。</para>
        /// </summary>
        public int? CVS { get; set; }

        /// <summary>
        /// 超商條碼繳費啟用
        /// <para>設定是否啟用超商條碼繳費支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0 或者未有此參數，即代表不開啟</para>
        /// <para>當該筆訂單金額小於 20 元或超過 4 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。</para>
        /// </summary>
        public int? BARCODE { get; set; }

        /// <summary>
        /// Pay2go 電子錢包啟用
        /// <para>設定是否啟用Pay2go 電子錢包支付方式。</para>
        /// <para>1 =啟用</para>
        /// <para>0 或者未有此參數，即代表不開啟</para>
        /// </summary>
        public int? P2G { get; set; }

        /// <summary>
        /// 物流啟用
        /// <para>1.使用前，須先登入智付通會員專區啟用物流並設定退貨門市與取貨人相關資訊。
        /// <para>  1 = 啟用超商取貨不付款<para>
        /// <para>  2 = 啟用超商取貨付款<para>
        /// <para>  3 = 啟用超商取貨不付款及超商取貨付款<para>
        /// <para>  0 或者未有此參數，即代表不開啟。<para>
        /// <para>2.當該筆訂單金額小於 30 元或大於 2 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。</para>
        /// </summary>
        public int? CVSCOM { get; set; }

    }
}