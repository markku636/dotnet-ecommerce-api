
using System.Collections.Generic;
using EC226.MemberSite.Model.ViewModels.Basics;

namespace EC226.Configuration
{
    /// <summary>
    /// 全域性基礎配置字典型別
    /// </summary>
    public static class SystemSettingDictionary
    {

        /// <summary>
        /// 取得系統配置字典，不匹配資料庫(1是2否)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, DictionaryKeyValues> GetConfig()
        {
            Dictionary<string, DictionaryKeyValues> di = new Dictionary<string, DictionaryKeyValues>();
            //平臺設定
            di.Add(SystemSettingConstVars.ShopName, new DictionaryKeyValues() { sKey = "平臺名稱", sValue = "核心內容管理系統" });
            di.Add(SystemSettingConstVars.ShopDesc, new DictionaryKeyValues() { sKey = "平臺描述", sValue = "平臺描述會展示在前臺及微信分享描述" });
            di.Add(SystemSettingConstVars.ShopAddress, new DictionaryKeyValues() { sKey = "平臺地址", sValue = "我的平臺地址" });
            di.Add(SystemSettingConstVars.ShopBeiAn, new DictionaryKeyValues() { sKey = "備案資訊", sValue = "網站備案資訊" });
            di.Add(SystemSettingConstVars.ShopLogo, new DictionaryKeyValues() { sKey = "平臺logo", sValue = "" });
            di.Add(SystemSettingConstVars.ShopFavicon, new DictionaryKeyValues() { sKey = "Favicon圖示", sValue = "" });
            di.Add(SystemSettingConstVars.ShopDefaultImage, new DictionaryKeyValues() { sKey = "預設圖", sValue = "" });
            di.Add(SystemSettingConstVars.StoreSwitch, new DictionaryKeyValues() { sKey = "開啟門店自提", sValue = "2" });
            di.Add(SystemSettingConstVars.CateStyle, new DictionaryKeyValues() { sKey = "分類樣式", sValue = "3" });
            di.Add(SystemSettingConstVars.CateType, new DictionaryKeyValues() { sKey = "H5分類樣式", sValue = "1" });
            di.Add(SystemSettingConstVars.AboutArticleId, new DictionaryKeyValues() { sKey = "關於我們文章", sValue = "2" });
            di.Add(SystemSettingConstVars.AboutArticle, new DictionaryKeyValues() { sKey = "關於我們文章", sValue = "" });
            di.Add(SystemSettingConstVars.UserAgreementId, new DictionaryKeyValues() { sKey = "使用者協議", sValue = "3" });
            di.Add(SystemSettingConstVars.UserAgreement, new DictionaryKeyValues() { sKey = "使用者協議", sValue = "" });
            di.Add(SystemSettingConstVars.PrivacyPolicyId, new DictionaryKeyValues() { sKey = "隱私政策", sValue = "4" });
            di.Add(SystemSettingConstVars.PrivacyPolicy, new DictionaryKeyValues() { sKey = "隱私政策", sValue = "" });

            di.Add(SystemSettingConstVars.ShowStoresSwitch, new DictionaryKeyValues() { sKey = "顯示門店列表", sValue = "2" });
            di.Add(SystemSettingConstVars.ShowStoreBalanceRechargeSwitch, new DictionaryKeyValues() { sKey = "顯示充值功能", sValue = "2" });

            //搜索發現關鍵字
            di.Add(SystemSettingConstVars.RecommendKeys, new DictionaryKeyValues() { sKey = "搜索發現關鍵詞", sValue = "核心,內容,管理,系統" });
            //分享設定
            di.Add(SystemSettingConstVars.ShareImage, new DictionaryKeyValues() { sKey = "分享圖片", sValue = "" });
            di.Add(SystemSettingConstVars.ShareTitle, new DictionaryKeyValues() { sKey = "分享標題", sValue = "優質好店邀您共享" });
            di.Add(SystemSettingConstVars.ShareDesc, new DictionaryKeyValues() { sKey = "分享描述", sValue = "" });
            //會員設定
            di.Add(SystemSettingConstVars.ShopMobile, new DictionaryKeyValues() { sKey = "商家手機號", sValue = "" });
            //1繫結，2不繫結-第三方的登陸的時候，是否需要繫結手機號碼，強烈建議使用者開啟，除非只在微信小程式內使用
            di.Add(SystemSettingConstVars.IsBindMobile, new DictionaryKeyValues() { sKey = "繫結手機號碼", sValue = "1" });
            //商品設定
            di.Add(SystemSettingConstVars.GoodsStocksWarn, new DictionaryKeyValues() { sKey = "庫存警報數量", sValue = "10" });

            //訂單管理
            di.Add(SystemSettingConstVars.OrderCancelTime, new DictionaryKeyValues() { sKey = "訂單取消時間", sValue = "1" });
            di.Add(SystemSettingConstVars.OrderCompleteTime, new DictionaryKeyValues() { sKey = "訂單完成時間", sValue = "30" });
            di.Add(SystemSettingConstVars.OrderAutoSignTime, new DictionaryKeyValues() { sKey = "訂單確認收貨時間", sValue = "20" });
            di.Add(SystemSettingConstVars.OrderAutoEvalTime, new DictionaryKeyValues() { sKey = "訂單自動評價時間", sValue = "30" });
            di.Add(SystemSettingConstVars.RemindOrderTime, new DictionaryKeyValues() { sKey = "訂單提醒付款時間", sValue = "1" });
            di.Add(SystemSettingConstVars.ReshipName, new DictionaryKeyValues() { sKey = "退貨聯繫人", sValue = "" });
            di.Add(SystemSettingConstVars.ReshipMobile, new DictionaryKeyValues() { sKey = "退貨聯繫方式", sValue = "" });
            di.Add(SystemSettingConstVars.ReshipAreaId, new DictionaryKeyValues() { sKey = "退貨區域", sValue = "" });
            di.Add(SystemSettingConstVars.ReshipAddress, new DictionaryKeyValues() { sKey = "退貨詳細地址", sValue = "" });
            di.Add(SystemSettingConstVars.ReshipCoordinate, new DictionaryKeyValues() { sKey = "退貨座標", sValue = "" });


            di.Add(SystemSettingConstVars.StoreOrderAutomaticDelivery, new DictionaryKeyValues() { sKey = "門店自提自動發貨", sValue = "2" });



            //分銷功能

            di.Add(SystemSettingConstVars.OpenDistribution, new DictionaryKeyValues() { sKey = "是否開啟三級分銷", sValue = "1" });
            di.Add(SystemSettingConstVars.DistributionNotes, new DictionaryKeyValues() { sKey = "使用者須知", sValue = "" });
            di.Add(SystemSettingConstVars.DistributionAgreement, new DictionaryKeyValues() { sKey = "分銷協議", sValue = "" });
            di.Add(SystemSettingConstVars.DistributionStore, new DictionaryKeyValues() { sKey = "是否開啟店舖", sValue = "2" });
            //di.Add(GlobalSettingConstVars.FirstPushAward, new DictionaryKeyValues() { sKey = "直推獎勵", sValue = "0" });
            //di.Add(GlobalSettingConstVars.SecondPushAward, new DictionaryKeyValues() { sKey = "次推獎勵", sValue = "0" });
            di.Add(SystemSettingConstVars.ShowInviterInfo, new DictionaryKeyValues() { sKey = "是否顯示邀請人資訊", sValue = "2" });


            di.Add(SystemSettingConstVars.DistributionLevel, new DictionaryKeyValues() { sKey = "分銷層級", sValue = "2" });
            di.Add(SystemSettingConstVars.DistributionType, new DictionaryKeyValues() { sKey = "成為分銷商條件", sValue = "1" });
            di.Add(SystemSettingConstVars.DistributionMoney, new DictionaryKeyValues() { sKey = "消費自動成為分銷商", sValue = "100" });
            di.Add(SystemSettingConstVars.DistributionGoods, new DictionaryKeyValues() { sKey = "購買商品成為分銷商", sValue = "1" });
            di.Add(SystemSettingConstVars.DistributionGoodsId, new DictionaryKeyValues() { sKey = "購買商品成為分銷商指定商品序列號", sValue = "0" });

            di.Add(SystemSettingConstVars.CommissionType, new DictionaryKeyValues() { sKey = "傭金型別", sValue = "1" });
            di.Add(SystemSettingConstVars.CommissionFirst, new DictionaryKeyValues() { sKey = "一級傭金", sValue = "0" });
            di.Add(SystemSettingConstVars.CommissionSecond, new DictionaryKeyValues() { sKey = "二級傭金", sValue = "0" });
            di.Add(SystemSettingConstVars.CommissionThird, new DictionaryKeyValues() { sKey = "三級傭金", sValue = "0" });

            //代理功能
            di.Add(SystemSettingConstVars.IsOpenAgent, new DictionaryKeyValues() { sKey = "是否開啟代理模組", sValue = "1" });
            di.Add(SystemSettingConstVars.IsShowAgentPortal, new DictionaryKeyValues() { sKey = "前端顯示入口", sValue = "1" });
            di.Add(SystemSettingConstVars.AgentNotes, new DictionaryKeyValues() { sKey = "使用者須知", sValue = "" });
            di.Add(SystemSettingConstVars.AgentAgreement, new DictionaryKeyValues() { sKey = "代理協議", sValue = "" });
            di.Add(SystemSettingConstVars.IsAllowProcurementService, new DictionaryKeyValues() { sKey = "是否允許代理代購服務", sValue = "1" });

            //積分設定
            di.Add(SystemSettingConstVars.SignPointType, new DictionaryKeyValues() { sKey = "簽到獎勵型別", sValue = "2" });
            di.Add(SystemSettingConstVars.SignRandomMin, new DictionaryKeyValues() { sKey = "隨機獎勵積分最小值", sValue = "1", });
            di.Add(SystemSettingConstVars.SignRandomMax, new DictionaryKeyValues() { sKey = "隨機獎勵積分最大值", sValue = "10" });
            di.Add(SystemSettingConstVars.FirstSignPoint, new DictionaryKeyValues() { sKey = "首次獎勵積分", sValue = "1" });
            di.Add(SystemSettingConstVars.ContinuitySignAdditional, new DictionaryKeyValues() { sKey = "連續簽到追加", sValue = "1" });
            di.Add(SystemSettingConstVars.SignMostPoint, new DictionaryKeyValues() { sKey = "單日最大獎勵", sValue = "10" });
            di.Add(SystemSettingConstVars.PointSwitch, new DictionaryKeyValues() { sKey = "開啟積分功能", sValue = "1" });
            di.Add(SystemSettingConstVars.PointDiscountedProportion, new DictionaryKeyValues() { sKey = "訂單積分折現比例", sValue = "100" });
            di.Add(SystemSettingConstVars.OrdersPointProportion, new DictionaryKeyValues() { sKey = "訂單積分使用比例", sValue = "10" });
            di.Add(SystemSettingConstVars.OrdersRewardProportion, new DictionaryKeyValues() { sKey = "訂單積分獎勵比例", sValue = "1" });

            di.Add(SystemSettingConstVars.SignAppointDateStatus, new DictionaryKeyValues() { sKey = "指定特殊日期狀態", sValue = "false" });
            di.Add(SystemSettingConstVars.SignAppointDate, new DictionaryKeyValues() { sKey = "指定特殊日期", sValue = "" });
            di.Add(SystemSettingConstVars.SignAppointDataType, new DictionaryKeyValues() { sKey = "指定日期獎勵型別", sValue = "1" });
            di.Add(SystemSettingConstVars.SignAppointDateRate, new DictionaryKeyValues() { sKey = "指定日期倍率", sValue = "2" });
            di.Add(SystemSettingConstVars.SignAppointDateAdditional, new DictionaryKeyValues() { sKey = "指定日期追加", sValue = "10" });

            // 提現設定
            di.Add(SystemSettingConstVars.TocashMoneyLow, new DictionaryKeyValues() { sKey = "最低提現金額", sValue = "0.01" });
            di.Add(SystemSettingConstVars.TocashMoneyRate, new DictionaryKeyValues() { sKey = "提現服務費率", sValue = "0" });
            di.Add(SystemSettingConstVars.TocashMoneyLimit, new DictionaryKeyValues() { sKey = "每日提現上限", sValue = "0" });

            //小程式設定
            di.Add(SystemSettingConstVars.WxUrl, new DictionaryKeyValues() { sKey = "小程式部署URL", sValue = "https://", });
            di.Add(SystemSettingConstVars.WxNickName, new DictionaryKeyValues() { sKey = "小程式名稱", sValue = "CoreShop", });
            di.Add(SystemSettingConstVars.WxAppid, new DictionaryKeyValues() { sKey = "AppId", sValue = "", });
            di.Add(SystemSettingConstVars.WxAppSecret, new DictionaryKeyValues() { sKey = "AppSecret", sValue = "" });
            di.Add(SystemSettingConstVars.WxToken, new DictionaryKeyValues() { sKey = "小程式驗證TOKEN", sValue = "", });
            di.Add(SystemSettingConstVars.WxEncodeaeskey, new DictionaryKeyValues() { sKey = "小程式EncodingAESKey", sValue = "" });
            di.Add(SystemSettingConstVars.WxUserName, new DictionaryKeyValues() { sKey = "原始Id", sValue = "", });
            di.Add(SystemSettingConstVars.WxPrincipalName, new DictionaryKeyValues() { sKey = "主體資訊", sValue = "核心內容管理系統", });
            di.Add(SystemSettingConstVars.WxSignature, new DictionaryKeyValues() { sKey = "簡介", sValue = "核心內容管理系統", });

            //公眾號設定
            di.Add(SystemSettingConstVars.WxOfficialUrl, new DictionaryKeyValues() { sKey = "公眾號部署URL", sValue = "https://", });
            di.Add(SystemSettingConstVars.WxOfficialName, new DictionaryKeyValues() { sKey = "公眾號名稱", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialId, new DictionaryKeyValues() { sKey = "微訊號", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialAppid, new DictionaryKeyValues() { sKey = "AppId", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialAppSecret, new DictionaryKeyValues() { sKey = "AppSecret", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialSourceId, new DictionaryKeyValues() { sKey = "公眾號原始ID", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialToken, new DictionaryKeyValues() { sKey = "微信驗證TOKEN", sValue = "", });
            di.Add(SystemSettingConstVars.WxOfficialEncodeaeskey, new DictionaryKeyValues() { sKey = "EncodingAESKey", sValue = "" });
            di.Add(SystemSettingConstVars.WxOfficialType, new DictionaryKeyValues() { sKey = "公眾號型別", sValue = "service" });
            di.Add(SystemSettingConstVars.WxOfficialQrCode, new DictionaryKeyValues() { sKey = "公眾號二維碼", sValue = "" });

            //其他設定
            di.Add(SystemSettingConstVars.QqMapKey, new DictionaryKeyValues() { sKey = "騰訊地圖key", sValue = "" });
            di.Add(SystemSettingConstVars.Kuaidi100Customer, new DictionaryKeyValues() { sKey = "公司編號", sValue = "" });
            di.Add(SystemSettingConstVars.Kuaidi100Key, new DictionaryKeyValues() { sKey = "授權key", sValue = "" });

            //統計程式碼
            di.Add(SystemSettingConstVars.StatisticsCode, new DictionaryKeyValues() { sKey = "百度統計程式碼", sValue = "" });
            //發票開關
            di.Add(SystemSettingConstVars.InvoiceSwitch, new DictionaryKeyValues() { sKey = "發票功能", sValue = "1" });
            //支付寶小程式appid
            di.Add(SystemSettingConstVars.MpAlipayAppid, new DictionaryKeyValues() { sKey = "支付寶小程式appid", sValue = "" });
            //客服ID
            di.Add(SystemSettingConstVars.EntId, new DictionaryKeyValues() { sKey = "客服ID", sValue = "" });
            //易源介面授權
            di.Add(SystemSettingConstVars.ShowApiAppid, new DictionaryKeyValues() { sKey = "AppId", sValue = "" });
            di.Add(SystemSettingConstVars.ShowApiSecret, new DictionaryKeyValues() { sKey = "授權Secret", sValue = "" });

            //凱信通簡訊設定
            di.Add(SystemSettingConstVars.SmsEnabled, new DictionaryKeyValues() { sKey = "是否開啟簡訊", sValue = "1" });
            di.Add(SystemSettingConstVars.SmsUserId, new DictionaryKeyValues() { sKey = "使用者ID", sValue = "" });
            di.Add(SystemSettingConstVars.SmsAccount, new DictionaryKeyValues() { sKey = "賬號", sValue = "" });
            di.Add(SystemSettingConstVars.SmsPassword, new DictionaryKeyValues() { sKey = "密碼", sValue = "" });
            di.Add(SystemSettingConstVars.SmsApiUrl, new DictionaryKeyValues() { sKey = "Api地址", sValue = "http://sms.EC226:9999/sms.aspx" });
            di.Add(SystemSettingConstVars.SmsSignature, new DictionaryKeyValues() { sKey = "簡訊簽名", sValue = "" });

            //附件儲存
            di.Add(SystemSettingConstVars.FilesStorageType, new DictionaryKeyValues() { sKey = "儲存方式", sValue = "LocalStorage" });
            di.Add(SystemSettingConstVars.FilesStoragePath, new DictionaryKeyValues() { sKey = "儲存路徑", sValue = "/upload/" });
            di.Add(SystemSettingConstVars.FilesStorageFileSuffix, new DictionaryKeyValues() { sKey = "檔案後綴型別", sValue = "gif,jpg,jpeg,png,bmp,xls,xlsx,doc,pdf,mp4,WebM,Ogv" });
            di.Add(SystemSettingConstVars.FilesStorageFileMaxSize, new DictionaryKeyValues() { sKey = "檔案最大大小", sValue = "10" });
            di.Add(SystemSettingConstVars.FilesStorageBucketBindUrl, new DictionaryKeyValues() { sKey = "雲端儲存繫結域名", sValue = "http://www.coreshop.cn/" });
            di.Add(SystemSettingConstVars.FilesStorageAccessKeyId, new DictionaryKeyValues() { sKey = "雲端儲存授權賬戶", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageAccessKeySecret, new DictionaryKeyValues() { sKey = "雲端儲存授權金鑰", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageTencentAccountId, new DictionaryKeyValues() { sKey = "騰訊云賬戶標識", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageTencentCosRegion, new DictionaryKeyValues() { sKey = "騰訊云桶地域", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageTencentBucketName, new DictionaryKeyValues() { sKey = "騰訊云桶名稱", sValue = "" });
            di.Add(SystemSettingConstVars.FilesStorageAliYunEndpoint, new DictionaryKeyValues() { sKey = "阿里云節點", sValue = "https://oss-cn-shenzhen.aliyuncs.com" });
            di.Add(SystemSettingConstVars.FilesStorageAliYunBucketName, new DictionaryKeyValues() { sKey = "阿里云桶名稱", sValue = "CoreShop" });

            di.Add(SystemSettingConstVars.FilesStorageQiNiuBucketName, new DictionaryKeyValues() { sKey = "七牛云桶名稱", sValue = "CoreShop" });

            //簡訊發送內容模板
            di.Add(SystemSettingConstVars.SmsTplForReg, new DictionaryKeyValues() { sKey = "賬戶註冊", sValue = "您正在註冊賬號，驗證碼是{code}，請勿告訴他人。" });
            di.Add(SystemSettingConstVars.SmsTplForLogin, new DictionaryKeyValues() { sKey = "賬戶登錄", sValue = "您正在登陸賬號，驗證碼是{code}，請勿告訴他人。" });
            di.Add(SystemSettingConstVars.SmsTplForVeri, new DictionaryKeyValues() { sKey = "驗證驗證碼", sValue = "您的驗證碼是{code}，請勿告訴他人。" });
            di.Add(SystemSettingConstVars.SmsTplForCreateOrder, new DictionaryKeyValues() { sKey = "下單成功時", sValue = "恭喜您，訂單建立成功，祝您購物愉快。" });
            di.Add(SystemSettingConstVars.SmsTplForOrderPayed, new DictionaryKeyValues() { sKey = "訂單支付成功時", sValue = "恭喜您，訂單支付成功，祝您購物愉快。" });
            di.Add(SystemSettingConstVars.SmsTplForRemindOrderPay, new DictionaryKeyValues() { sKey = "訂單催付提醒", sValue = "您的訂單還有1個小時就要取消了，請及時進行支付。" });
            di.Add(SystemSettingConstVars.SmsTplForDeliveryNotice, new DictionaryKeyValues() { sKey = "訂單發貨通知", sValue = "您好，您的訂單已經發貨。" });
            di.Add(SystemSettingConstVars.SmsTplForAfterSalesPass, new DictionaryKeyValues() { sKey = "售後確認通過", sValue = "您好，您的售後已經通過。" });
            di.Add(SystemSettingConstVars.SmsTplForRefundSuccess, new DictionaryKeyValues() { sKey = "使用者退款成功通知", sValue = "使用者您好，您的退款已經處理，請確認。" });
            di.Add(SystemSettingConstVars.SmsTplForSellerOrderNotice, new DictionaryKeyValues() { sKey = "訂單付款成功平臺通知", sValue = "您有新的訂單了，請及時處理。" });
            di.Add(SystemSettingConstVars.SmsTplForCommon, new DictionaryKeyValues() { sKey = "通用型別", sValue = "歡迎您訪問我們的微信小程式，有問題請聯繫客服。" });


            return di;
        }

        /// <summary>
        /// 取得促銷新增參數型別字典
        /// </summary>
        /// <returns></returns>
        public static List<CommonKeyValues> GetPromotionConditionType()
        {
            var list = new List<CommonKeyValues>
            {
                new CommonKeyValues() {sDescription = "所有商品滿足條件", sValue = "goods", sKey = "GOODS_ALL"},
                new CommonKeyValues() {sDescription = "指定某些商品滿足條件", sValue = "goods", sKey = "GOODS_IDS"},
                new CommonKeyValues() {sDescription = "指定商品分類滿足條件", sValue = "goods", sKey = "GOODS_CATS"},
                new CommonKeyValues() {sDescription = "指定商品品牌滿足條件", sValue = "goods", sKey = "GOODS_BRANDS"},
                new CommonKeyValues() {sDescription = "訂單滿XX金額滿足條件", sValue = "order", sKey = "ORDER_FULL"},
                new CommonKeyValues() {sDescription = "使用者符合指定等級", sValue = "user", sKey = "USER_GRADE"}
            };
            return list;
        }


        /// <summary>
        /// 取得促銷新增結果型別字典
        /// </summary>
        /// <returns></returns>
        public static List<CommonKeyValues> GetPromotionResultType()
        {
            var list = new List<CommonKeyValues>
            {
                new CommonKeyValues() {sDescription = "指定商品減固定金額", sValue = "goods", sKey = "GOODS_REDUCE"},
                new CommonKeyValues() {sDescription = "指定商品打X折", sValue = "goods", sKey = "GOODS_DISCOUNT"},
                new CommonKeyValues() {sDescription = "指定商品一口價", sValue = "goods", sKey = "GOODS_ONE_PRICE"},
                new CommonKeyValues() {sDescription = "訂單減指定金額", sValue = "order", sKey = "ORDER_REDUCE"},
                new CommonKeyValues() {sDescription = "訂單打X折", sValue = "order", sKey = "ORDER_DISCOUNT"},
                new CommonKeyValues() {sDescription = "指定商品每第幾件減指定金額", sValue = "goods", sKey = "GOODS_HALF_PRICE"}
            };
            return list;
        }



        /// <summary>
        /// 取得系統預設發貨物流方式
        /// </summary>
        /// <returns></returns>
        public static List<CommonKeyValues> GetSystemLogistics()
        {
            var list = new List<CommonKeyValues>
            {
                new CommonKeyValues() {sDescription = "本地同城配送", sValue = "無", sKey = "benditongcheng"},
                new CommonKeyValues() {sDescription = "本地上門自提", sValue = "無", sKey = "shangmenziti"},
            };
            return list;
        }



    }
}
