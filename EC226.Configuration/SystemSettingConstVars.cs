
namespace EC226.Configuration
{
    /// <summary>
    /// 平臺設定欄位快取名稱定義
    /// </summary>
    public static class SystemSettingConstVars
    {
        /// <summary>
        /// 平臺名稱
        /// </summary>
        public const string ShopName = "shopName";

        /// <summary>
        /// 平臺描述
        /// </summary>
        public const string ShopDesc = "shopDesc";

        /// <summary>
        /// 平臺地址
        /// </summary>
        public const string ShopAddress = "shopAddress";

        /// <summary>
        /// 備案資訊
        /// </summary>
        public const string ShopBeiAn = "shopBeiAn";

        /// <summary>
        /// 平臺logo
        /// </summary>
        public const string ShopLogo = "shopLogo";

        /// <summary>
        /// Favicon圖示
        /// </summary>
        public const string ShopFavicon = "shopFavicon";

        /// <summary>
        /// 預設圖
        /// </summary>
        public const string ShopDefaultImage = "shopDefaultImage";

        /// <summary>
        /// 商家手機號
        /// </summary>
        public const string ShopMobile = "shopMobile";

        /// <summary>
        /// 開啟門店自提
        /// </summary>
        public const string StoreSwitch = "storeSwitch";

        /// <summary>
        /// 分類樣式
        /// </summary>
        public const string CateStyle = "cateStyle";

        /// <summary>
        /// H5分類樣式
        /// </summary>
        public const string CateType = "cateType";

        /// <summary>
        /// 訂單取消時間
        /// </summary>
        public const string OrderCancelTime = "orderCancelTime";

        /// <summary>
        /// 訂單完成時間
        /// </summary>
        public const string OrderCompleteTime = "orderCompleteTime";

        /// <summary>
        /// 訂單確認收貨時間
        /// </summary>
        public const string OrderAutoSignTime = "orderAutoSignTime";

        /// <summary>
        /// 訂單自動評價時間
        /// </summary>
        public const string OrderAutoEvalTime = "orderAutoEvalTime";

        /// <summary>
        /// 訂單提醒付款時間
        /// </summary>
        public const string RemindOrderTime = "remindOrderTime";

        /// <summary>
        /// 門店訂單自動發貨
        /// </summary>
        public const string StoreOrderAutomaticDelivery = "storeOrderAutomaticDelivery";

        //分銷功能（老分銷）=============================================================
        /// <summary>
        /// 是否開啟分銷
        /// </summary>
        public const string OpenDistribution = "openDistribution";
        /// <summary>
        /// 使用者須知：成為分銷商后，可以取得傭金，使用者只可被推薦一次，越早推薦越返利越多哦。
        /// </summary>
        public const string DistributionNotes = "distributionNotes";
        /// <summary>
        /// 分銷協議
        /// </summary>
        public const string DistributionAgreement = "distributionAgreement";
        /// <summary>
        /// 是否開啟店舖
        /// </summary>
        public const string DistributionStore = "distributionStore";

        /// <summary>
        /// 顯示邀請人資訊
        /// </summary>
        public const string ShowInviterInfo = "showInviterInfo";


        /// <summary>
        /// 分銷層級1,2層
        /// </summary>
        public const string DistributionLevel = "distributionLevel";
        /// <summary>
        /// 成為分銷商條件：1無條件(需要審覈),2申請(需要審覈),3無條件
        /// </summary>
        public const string DistributionType = "distributionType";
        /// <summary>
        /// 消費自動成為分銷商：元
        /// </summary>
        public const string DistributionMoney = "distributionMoney";
        /// <summary>
        /// 購買商品成為分銷商：1關閉，2任意商品，3指定商品
        /// </summary>
        public const string DistributionGoods = "distributionGoods";
        /// <summary>
        /// 購買商品成為分銷商指定商品序列號
        /// </summary>
        public const string DistributionGoodsId = "distributionGoodsId";
        /// <summary>
        /// 傭金型別：1百分比，2固定金額
        /// </summary>
        public const string CommissionType = "commissionType";
        /// <summary>
        /// 一級傭金
        /// </summary>
        public const string CommissionFirst = "commissionFirst";
        /// <summary>
        /// 二級傭金
        /// </summary>
        public const string CommissionSecond = "commissionSecond";
        /// <summary>
        /// 三級傭金
        /// </summary>
        public const string CommissionThird = "commissionThird";


        /// <summary>
        /// 庫存警報數量
        /// </summary>
        public const string GoodsStocksWarn = "goodsStocksWarn";



        /// <summary>
        /// 退貨聯繫人
        /// </summary>
        public const string ReshipName = "reshipName";

        /// <summary>
        /// 退貨聯繫方式
        /// </summary>
        public const string ReshipMobile = "reshipMobile";

        /// <summary>
        /// 退貨區域
        /// </summary>
        public const string ReshipAreaId = "reshipAreaId";

        /// <summary>
        /// 退貨詳細地址
        /// </summary>
        public const string ReshipAddress = "reshipAddress";

        /// <summary>
        /// 退貨座標
        /// </summary>
        public const string ReshipCoordinate = "reshipCoordinate";

        /// <summary>
        /// 簽到獎勵型別
        /// </summary>
        public const string SignPointType = "signPointType";

        /// <summary>
        /// 隨機獎勵積分最小值
        /// </summary>
        public const string SignRandomMin = "signRandomMin";

        /// <summary>
        /// 隨機獎勵積分最大值
        /// </summary>
        public const string SignRandomMax = "signRandomMax";

        /// <summary>
        /// 首次獎勵積分
        /// </summary>
        public const string FirstSignPoint = "firstSignPoint";

        /// <summary>
        /// 連續簽到追加
        /// </summary>
        public const string ContinuitySignAdditional = "continuitySignAdditional";

        /// <summary>
        /// 單日最大獎勵
        /// </summary>
        public const string SignMostPoint = "signMostPoint";

        /// <summary>
        /// 開啟積分功能
        /// </summary>
        public const string PointSwitch = "pointSwitch";

        /// <summary>
        /// 訂單積分折現比例
        /// </summary>
        public const string PointDiscountedProportion = "pointDiscountedProportion";

        /// <summary>
        /// 訂單積分使用比例
        /// </summary>
        public const string OrdersPointProportion = "ordersPointProportion";

        /// <summary>
        /// 訂單積分獎勵比例
        /// </summary>
        public const string OrdersRewardProportion = "ordersRewardProportion";

        /// <summary>
        /// 指定特殊日期狀態
        /// </summary>
        public const string SignAppointDateStatus = "signAppointDateStatus";

        /// <summary>
        /// 指定特殊日期
        /// </summary>
        public const string SignAppointDate = "signAppointDate";

        /// <summary>
        /// 指定日期獎勵型別
        /// </summary>
        public const string SignAppointDataType = "signAppointDataType";

        /// <summary>
        /// 指定日期倍率
        /// </summary>
        public const string SignAppointDateRate = "signAppointDateRate";

        /// <summary>
        /// 指定日期追加
        /// </summary>
        public const string SignAppointDateAdditional = "signAppointDateAdditional";




        //小程式設定============================================================================
        /// <summary>
        /// 小程式部署URL
        /// </summary>
        public const string WxUrl = "wxUrl";
        /// <summary>
        /// 小程式名稱
        /// </summary>
        public const string WxNickName = "wxNickName";

        /// <summary>
        /// 小程式AppId
        /// </summary>
        public const string WxAppid = "wxAppid";

        /// <summary>
        /// 小程式AppSecret
        /// </summary>
        public const string WxAppSecret = "wxAppSecret";

        /// <summary>
        /// 小程式TOKEN
        /// </summary>
        public const string WxToken = "wxToken";

        /// <summary>
        /// 小程式EncodingAESKey
        /// </summary>
        public const string WxEncodeaeskey = "wxEncodeaeskey";


        /// <summary>
        /// 原始Id
        /// </summary>
        public const string WxUserName = "wxUserName";

        /// <summary>
        /// 主體資訊
        /// </summary>
        public const string WxPrincipalName = "wxPrincipalName";

        /// <summary>
        /// 簡介
        /// </summary>
        public const string WxSignature = "wxSignature";


        //公眾號設定============================================================================

        /// <summary>
        /// 公眾號部署URL
        /// </summary>
        public const string WxOfficialUrl = "wxOfficialUrl";
        /// <summary>
        /// 公眾號名稱
        /// </summary>
        public const string WxOfficialName = "wxOfficialName";

        /// <summary>
        /// 微訊號
        /// </summary>
        public const string WxOfficialId = "wxOfficialId";

        /// <summary>
        /// AppId
        /// </summary>
        public const string WxOfficialAppid = "wxOfficialAppid";

        /// <summary>
        /// AppSecret
        /// </summary>
        public const string WxOfficialAppSecret = "wxOfficialAppSecret";

        /// <summary>
        /// 公眾號原始ID
        /// </summary>
        public const string WxOfficialSourceId = "wxOfficialSourceId";

        /// <summary>
        /// 微信驗證TOKEN
        /// </summary>
        public const string WxOfficialToken = "wxOfficialToken";

        /// <summary>
        /// EncodingAESKey
        /// </summary>
        public const string WxOfficialEncodeaeskey = "wxOfficialEncodeaeskey";

        /// <summary>
        /// 公眾號型別
        /// </summary>
        public const string WxOfficialType = "wxOfficialType";
        /// <summary>
        /// 公眾號二維碼
        /// </summary>
        public const string WxOfficialQrCode = "wxOfficialQrCode";


        // 提現設定============================================================================
        /// <summary>
        /// 最低提現金額
        /// </summary>
        public const string TocashMoneyLow = "tocashMoneyLow";

        /// <summary>
        /// 提現服務費率
        /// </summary>
        public const string TocashMoneyRate = "tocashMoneyRate";

        /// <summary>
        /// 每日提現上限
        /// </summary>
        public const string TocashMoneyLimit = "tocashMoneyLimit";


        //其他設定============================================================================

        /// <summary>
        /// 騰訊地圖key
        /// </summary>
        public const string QqMapKey = "qqMapKey";

        /// <summary>
        /// 公司編號
        /// </summary>
        public const string Kuaidi100Customer = "kuaidi100Customer";

        /// <summary>
        /// 授權key
        /// </summary>
        public const string Kuaidi100Key = "kuaidi100Key";


        //搜索發現關鍵字============================================================================
        /// <summary>
        /// 搜索發現關鍵詞
        /// </summary>
        public const string RecommendKeys = "recommendKeys";


        //統計程式碼============================================================================
        /// <summary>
        /// 百度統計程式碼
        /// </summary>
        public const string StatisticsCode = "statisticsCode";


        //發票開關============================================================================
        /// <summary>
        /// 發票功能
        /// </summary>
        public const string InvoiceSwitch = "invoiceSwitch";


        //第三方的登陸的時候，是否需要繫結手機號碼，強烈建議使用者開啟，除非只在微信小程式內使用============================================================================
        //1繫結，2不繫結
        /// <summary>
        /// 繫結手機號碼
        /// </summary>
        public const string IsBindMobile = "isBindMobile";

        //支付寶小程式appid============================================================================

        /// <summary>
        /// 支付寶小程式appid
        /// </summary>
        public const string MpAlipayAppid = "mpAlipayAppid";

        /// <summary>
        /// 分享圖片
        /// </summary>
        public const string ShareImage = "shareImage";

        /// <summary>
        /// 分享標題
        /// </summary>
        public const string ShareTitle = "shareTitle";

        /// <summary>
        /// 分享描述
        /// </summary>
        public const string ShareDesc = "shareDesc";

        /// <summary>
        /// 關於我們文章
        /// </summary>
        public const string AboutArticleId = "aboutArticleId";

        /// <summary>
        /// 關於我們文章
        /// </summary>
        public const string AboutArticle = "aboutArticle";

        /// <summary>
        /// 客服ID
        /// </summary>
        public const string EntId = "entId";

        /// <summary>
        /// 使用者協議
        /// </summary>
        public const string UserAgreementId = "userAgreementId";

        /// <summary>
        /// 使用者協議
        /// </summary>
        public const string UserAgreement = "userAgreement";

        /// <summary>
        /// 隱私政策
        /// </summary>
        public const string PrivacyPolicyId = "privacyPolicyId";

        /// <summary>
        /// 隱私政策
        /// </summary>
        public const string PrivacyPolicy = "privacyPolicy";

        /// <summary>
        /// 顯示門店列表
        /// </summary>
        public const string ShowStoresSwitch = "showStoresSwitch";

        /// <summary>
        /// 顯示充值功能
        /// </summary>
        public const string ShowStoreBalanceRechargeSwitch = "showStoreBalanceRechargeSwitch";

        //第三方介面============================================================================
        /// <summary>
        /// 易源介面授權key
        /// </summary>
        public const string ShowApiAppid = "showApiAppid";
        /// <summary>
        /// 易源介面授權金鑰
        /// </summary>
        public const string ShowApiSecret = "showApiSecret";


        //簡訊平臺============================================================================
        /// <summary>
        /// 是否開啟簡訊
        /// </summary>
        public const string SmsEnabled = "smsEnabled";
        /// <summary>
        /// 使用者ID
        /// </summary>
        public const string SmsUserId = "smsUserId";
        /// <summary>
        /// 使用者賬號
        /// </summary>
        public const string SmsAccount = "smsAccount";
        /// <summary>
        /// 使用者密碼
        /// </summary>
        public const string SmsPassword = "smsPassword";
        /// <summary>
        /// 簡訊api地址
        /// </summary>
        public const string SmsApiUrl = "smsApiUrl";
        /// <summary>
        /// 簡訊簽名
        /// </summary>
        public const string SmsSignature = "smsSignature";


        /// <summary>
        /// 賬戶註冊-簡訊內容模板
        /// </summary>
        public const string SmsTplForReg = "smsTplForReg";

        /// <summary>
        /// 賬戶登錄-簡訊內容模板
        /// </summary>
        public const string SmsTplForLogin = "smsTplForLogin";

        /// <summary>
        /// 驗證驗證碼-簡訊內容模板
        /// </summary>
        public const string SmsTplForVeri = "smsTplForVeri";

        /// <summary>
        /// 下單成功時-簡訊內容模板
        /// </summary>
        public const string SmsTplForCreateOrder = "smsTplForCreateOrder";

        /// <summary>
        /// 訂單支付成功時-簡訊內容模板
        /// </summary>
        public const string SmsTplForOrderPayed = "smsTplForOrderPayed";

        /// <summary>
        /// 訂單催付提醒-簡訊內容模板
        /// </summary>
        public const string SmsTplForRemindOrderPay = "smsTplForRemindOrderPay";

        /// <summary>
        /// 訂單發貨通知-簡訊內容模板
        /// </summary>
        public const string SmsTplForDeliveryNotice = "smsTplForDeliveryNotice";

        /// <summary>
        /// 售後確認通過-簡訊內容模板
        /// </summary>
        public const string SmsTplForAfterSalesPass = "smsTplForAfterSalesPass";

        /// <summary>
        /// 使用者退款成功通知-簡訊內容模板
        /// </summary>
        public const string SmsTplForRefundSuccess = "smsTplForRefundSuccess";

        /// <summary>
        /// 訂單付款成功平臺通知-簡訊內容模板
        /// </summary>
        public const string SmsTplForSellerOrderNotice = "smsTplForSellerOrderNotice";

        /// <summary>
        /// 通用型別-簡訊內容模板
        /// </summary>
        public const string SmsTplForCommon = "smsTplForCommon";

        //網路印表機============================================================================
        /// <summary>
        /// 是否開啟
        /// </summary>
        public static readonly string NetWorkPrinterEnabled = "netWorkPrinterEnabled";
        /// <summary>
        /// 應用ID
        /// </summary>
        public static readonly string NetWorkPrinterClientId = "netWorkPrinterClientId";
        /// <summary>
        /// 應用金鑰
        /// </summary>
        public static readonly string NetWorkPrinterClientSecret = "netWorkPrinterClientSecret";
        /// <summary>
        /// 印表機裝置號
        /// </summary>
        public static readonly string NetWorkPrinterMachineCode = "netWorkPrinterMachineCode";
        /// <summary>
        /// 印表機終端金鑰
        /// </summary>
        public static readonly string NetWorkPrinterMsign = "netWorkPrinterMsign";
        /// <summary>
        /// 印表機名稱
        /// </summary>
        public static readonly string NetWorkPrinterPrinterName = "netWorkPrinterPrinterName";
        /// <summary>
        /// 印表機設定聯繫方式
        /// </summary>
        public static readonly string NetWorkPrinterPhone = "netWorkPrinterPhone";

        //代理模組============================================================================

        /// <summary>
        /// 是否開啟代理模組
        /// </summary>
        public static readonly string IsOpenAgent = "isOpenAgent";

        /// <summary>
        /// 是否顯示代理模組申請及管理入口
        /// </summary>
        public static readonly string IsShowAgentPortal = "isShowAgentPortal";

        /// <summary>
        /// 使用者須知：
        /// </summary>
        public const string AgentNotes = "agentNotes";
        /// <summary>
        /// 分銷協議:
        /// </summary>
        public const string AgentAgreement = "agentAgreement";
        /// <summary>
        /// 是否允許代理代購服務
        /// </summary>
        public const string IsAllowProcurementService = "isAllowProcurementService";


        //附件儲存============================================================================

        /// <summary>
        /// 儲存方式
        /// </summary>
        public static readonly string FilesStorageType = "filesStorageType";
        /// <summary>
        /// 儲存路徑
        /// </summary>
        public static readonly string FilesStoragePath = "filesStoragePath";
        /// <summary>
        /// 檔案後綴型別
        /// </summary>
        public static readonly string FilesStorageFileSuffix = "filesStorageFileSuffix";
        /// <summary>
        /// 檔案最大大小M
        /// </summary>
        public static readonly string FilesStorageFileMaxSize = "filesStorageFileMaxSize";

        /// <summary>
        /// 雲端儲存繫結域名
        /// </summary>
        public static readonly string FilesStorageBucketBindUrl = "filesStorageBucketBindUrl";
        /// <summary>
        /// 雲端儲存授權賬戶
        /// </summary>
        public static readonly string FilesStorageAccessKeyId = "filesStorageAccessKeyId";
        /// <summary>
        /// 雲端儲存授權金鑰
        /// </summary>
        public static readonly string FilesStorageAccessKeySecret = "filesStorageAccessKeySecret";

        /// <summary>
        /// 騰訊云賬戶標識
        /// </summary>
        public static readonly string FilesStorageTencentAccountId = "filesStorageTencentAccountId";
        /// <summary>
        /// 騰訊雲端儲存桶地域
        /// </summary>
        public static readonly string FilesStorageTencentCosRegion = "filesStorageTencentCosRegion";
        /// <summary>
        /// 騰訊雲端儲存桶名稱
        /// </summary>
        public static readonly string FilesStorageTencentBucketName = "filesStorageTencentBucketName";

        /// <summary>
        /// 阿里云節點
        /// </summary>
        public static readonly string FilesStorageAliYunEndpoint = "filesStorageAliYunEndpoint";
        /// <summary>
        /// 阿里云桶名稱
        /// </summary>
        public static readonly string FilesStorageAliYunBucketName = "filesStorageAliYunBucketName";

        /// <summary>
        /// 七牛云桶名稱
        /// </summary>
        public static readonly string FilesStorageQiNiuBucketName = "filesStorageQiNiuBucketName";



    }
}
