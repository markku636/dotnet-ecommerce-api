using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Web;
using EC226.Payment.Abstractions;
using Microsoft.AspNetCore.Http;

namespace EC226.Payment.Providers.Newebpay
{
    /// <summary>
    /// 藍新金流 Provider
    /// </summary>
    public class NewebpayPaymentProvider : IPaymentProvider
    {
        private readonly NewebpayConfig _config;

        public PaymentProviderType ProviderType => PaymentProviderType.Newebpay;

        public NewebpayPaymentProvider(NewebpayConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// 建立付款請求，產生 HTML Form
        /// </summary>
        public PaymentResponse CreatePayment(PaymentRequest request)
        {
            try
            {
                ValidateRequest(request);

                // 1. 建立 TradeInfo 參數
                var tradeInfo = BuildTradeInfo(request);

                // 2. 將 TradeInfo 轉為 QueryString 格式
                var tradeInfoString = ToQueryString(tradeInfo);

                // 3. AES 加密
                var encryptedTradeInfo = NewebpayEncryption.AesEncrypt(tradeInfoString, _config.HashKey, _config.HashIV);

                // 4. SHA256 雜湊產生 TradeSha
                var tradeSha = NewebpayEncryption.GenerateTradeSha(encryptedTradeInfo, _config.HashKey, _config.HashIV);

                // 5. 產生 HTML Form
                var formParameters = new Dictionary<string, string>
                {
                    ["MerchantID"] = _config.MerchantID,
                    ["TradeInfo"] = encryptedTradeInfo,
                    ["TradeSha"] = tradeSha,
                    ["Version"] = _config.Version
                };

                var htmlForm = GenerateHtmlForm(formParameters);

                return PaymentResponse.CreateSuccess(htmlForm, request.OrderNo, JsonSerializer.Serialize(tradeInfo));
            }
            catch (Exception ex)
            {
                return PaymentResponse.CreateFailure("CREATE_PAYMENT_ERROR", ex.Message);
            }
        }

        /// <summary>
        /// 驗證並解析回調通知 (From Form POST)
        /// </summary>
        public PaymentCallbackResult ValidateCallback(IFormCollection form)
        {
            try
            {
                var tradeInfo = form["TradeInfo"].ToString();
                var tradeSha = form["TradeSha"].ToString();

                return ValidateAndParseCallback(tradeInfo, tradeSha);
            }
            catch (Exception ex)
            {
                return PaymentCallbackResult.CreateInvalid($"解析回調失敗: {ex.Message}");
            }
        }

        /// <summary>
        /// 驗證並解析回調通知 (From Encrypted Data)
        /// </summary>
        public PaymentCallbackResult ValidateCallback(string encryptedData)
        {
            try
            {
                // 解密 TradeInfo
                var decryptedData = NewebpayEncryption.AesDecrypt(encryptedData, _config.HashKey, _config.HashIV);
                var result = JsonSerializer.Deserialize<NewebpayCallbackData>(decryptedData);

                return ParseCallbackData(result, decryptedData);
            }
            catch (Exception ex)
            {
                return PaymentCallbackResult.CreateInvalid($"解密回調失敗: {ex.Message}");
            }
        }

        /// <summary>
        /// 產生回應給金流商的內容
        /// </summary>
        public string GenerateCallbackResponse(bool success)
        {
            // 藍新不需要特別回應
            return success ? "SUCCESS" : "ERROR";
        }

        #region Private Methods

        private void ValidateRequest(PaymentRequest request)
        {
            if (string.IsNullOrEmpty(request.OrderNo))
                throw new ArgumentException("訂單編號不可為空", nameof(request.OrderNo));
            if (request.OrderNo.Length > 20)
                throw new ArgumentException("訂單編號不可超過 20 字", nameof(request.OrderNo));
            if (request.Amount <= 0)
                throw new ArgumentException("交易金額必須大於 0", nameof(request.Amount));
            if (string.IsNullOrEmpty(request.ItemDescription))
                throw new ArgumentException("商品描述不可為空", nameof(request.ItemDescription));
            if (request.ItemDescription.Length > 50)
                throw new ArgumentException("商品描述不可超過 50 字", nameof(request.ItemDescription));
        }

        private Dictionary<string, string> BuildTradeInfo(PaymentRequest request)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            var tradeInfo = new Dictionary<string, string>
            {
                ["MerchantID"] = _config.MerchantID,
                ["RespondType"] = "JSON",
                ["TimeStamp"] = timestamp,
                ["Version"] = _config.Version,
                ["MerchantOrderNo"] = request.OrderNo,
                ["Amt"] = Convert.ToInt32(request.Amount).ToString(),
                ["ItemDesc"] = request.ItemDescription,
                ["Email"] = request.BuyerEmail ?? "",
                ["LoginType"] = _config.LoginType.ToString()
            };

            // 通知網址
            if (!string.IsNullOrEmpty(_config.NotifyUrl))
                tradeInfo["NotifyURL"] = GetFullUrl(_config.NotifyUrl);

            if (!string.IsNullOrEmpty(_config.ReturnUrl))
                tradeInfo["ReturnURL"] = GetFullUrl(_config.ReturnUrl);

            if (!string.IsNullOrEmpty(_config.CustomerUrl))
                tradeInfo["CustomerURL"] = GetFullUrl(_config.CustomerUrl);

            if (!string.IsNullOrEmpty(_config.ClientBackUrl))
                tradeInfo["ClientBackURL"] = GetFullUrl(_config.ClientBackUrl);

            // 語系
            if (!string.IsNullOrEmpty(_config.LangType))
                tradeInfo["LangType"] = _config.LangType;

            // 交易限制
            if (request.TradeLimit > 0)
                tradeInfo["TradeLimit"] = request.TradeLimit.ToString();

            // 繳費期限 (非即時交易)
            if (request.ExpireDate.HasValue)
                tradeInfo["ExpireDate"] = request.ExpireDate.Value.ToString("yyyyMMdd");

            // 付款方式
            SetPaymentMethods(tradeInfo, request.PaymentMethod);

            return tradeInfo;
        }

        private void SetPaymentMethods(Dictionary<string, string> tradeInfo, PaymentMethodType method)
        {
            // 預設全部關閉
            switch (method)
            {
                case PaymentMethodType.Credit:
                    tradeInfo["CREDIT"] = "1";
                    break;
                case PaymentMethodType.WebATM:
                    tradeInfo["WEBATM"] = "1";
                    break;
                case PaymentMethodType.ATM:
                    tradeInfo["VACC"] = "1";
                    break;
                case PaymentMethodType.CVS:
                    tradeInfo["CVS"] = "1";
                    break;
                case PaymentMethodType.BARCODE:
                    tradeInfo["BARCODE"] = "1";
                    break;
                case PaymentMethodType.GooglePay:
                    tradeInfo["ANDROIDPAY"] = "1";
                    break;
                case PaymentMethodType.SamsungPay:
                    tradeInfo["SAMSUNGPAY"] = "1";
                    break;
                case PaymentMethodType.ALL:
                default:
                    // 開啟所有付款方式
                    tradeInfo["CREDIT"] = "1";
                    tradeInfo["VACC"] = "1";
                    tradeInfo["CVS"] = "1";
                    tradeInfo["BARCODE"] = "1";
                    break;
            }
        }

        private string ToQueryString(Dictionary<string, string> parameters)
        {
            var pairs = parameters
                .Where(p => !string.IsNullOrEmpty(p.Value))
                .Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}");
            return string.Join("&", pairs);
        }

        private string GetFullUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            if (url.StartsWith("http://") || url.StartsWith("https://"))
                return url;

            return url;
        }

        private string GenerateHtmlForm(Dictionary<string, string> parameters)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html><head><meta charset=\"utf-8\"></head><body>");
            sb.AppendLine($"<form id=\"newebpay-form\" method=\"post\" action=\"{_config.ApiUrl}\">");

            foreach (var param in parameters)
            {
                sb.AppendLine($"<input type=\"hidden\" name=\"{param.Key}\" value=\"{HttpUtility.HtmlEncode(param.Value)}\" />");
            }

            sb.AppendLine("<input type=\"submit\" value=\"前往付款\" style=\"display:none;\" />");
            sb.AppendLine("</form>");
            sb.AppendLine("<script>document.getElementById('newebpay-form').submit();</script>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

        private PaymentCallbackResult ValidateAndParseCallback(string tradeInfo, string tradeSha)
        {
            // 1. 驗證 TradeSha
            if (!NewebpayEncryption.ValidateTradeSha(tradeInfo, tradeSha, _config.HashKey, _config.HashIV))
            {
                return PaymentCallbackResult.CreateInvalid("TradeSha 驗證失敗");
            }

            // 2. 解密 TradeInfo
            var decryptedData = NewebpayEncryption.AesDecrypt(tradeInfo, _config.HashKey, _config.HashIV);

            // 3. 解析 JSON
            var callbackData = JsonSerializer.Deserialize<NewebpayCallbackData>(decryptedData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return ParseCallbackData(callbackData, decryptedData);
        }

        private PaymentCallbackResult ParseCallbackData(NewebpayCallbackData data, string rawData)
        {
            var isPaid = data.Status == "SUCCESS";
            var result = data.Result;

            var callbackResult = new PaymentCallbackResult
            {
                IsValid = true,
                IsPaid = isPaid,
                OrderNo = result?.MerchantOrderNo ?? "",
                TradeNo = result?.TradeNo ?? "",
                Amount = result?.Amt ?? 0,
                PaymentType = result?.PaymentType ?? "",
                StatusCode = data.Status,
                StatusMessage = data.Message,
                RawData = rawData,
                ResponseContent = "SUCCESS"
            };

            // 解析付款時間
            if (!string.IsNullOrEmpty(result?.PayTime) &&
                DateTime.TryParse(result.PayTime, out var paymentTime))
            {
                callbackResult.PaymentTime = paymentTime;
            }

            // ATM/CVS 取號資訊
            if (!string.IsNullOrEmpty(result?.BankCode))
                callbackResult.BankCode = result.BankCode;

            if (!string.IsNullOrEmpty(result?.CodeNo))
                callbackResult.PaymentCode = result.CodeNo;

            if (!string.IsNullOrEmpty(result?.ExpireDate) &&
                DateTime.TryParse(result.ExpireDate, out var expireDate))
            {
                callbackResult.ExpireDate = expireDate;
            }

            // 超商條碼
            if (!string.IsNullOrEmpty(result?.Barcode_1))
                callbackResult.Barcode1 = result.Barcode_1;
            if (!string.IsNullOrEmpty(result?.Barcode_2))
                callbackResult.Barcode2 = result.Barcode_2;
            if (!string.IsNullOrEmpty(result?.Barcode_3))
                callbackResult.Barcode3 = result.Barcode_3;

            return callbackResult;
        }

        #endregion
    }

    #region Internal Models

    /// <summary>
    /// 藍新回調資料模型
    /// </summary>
    internal class NewebpayCallbackData
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public NewebpayCallbackResult Result { get; set; }
    }

    /// <summary>
    /// 藍新回調結果模型
    /// </summary>
    internal class NewebpayCallbackResult
    {
        public string MerchantID { get; set; }
        public decimal Amt { get; set; }
        public string TradeNo { get; set; }
        public string MerchantOrderNo { get; set; }
        public string PaymentType { get; set; }
        public string RespondType { get; set; }
        public string PayTime { get; set; }
        public string IP { get; set; }
        public string EscrowBank { get; set; }

        // ATM 相關
        public string BankCode { get; set; }
        public string CodeNo { get; set; }
        public string ExpireDate { get; set; }

        // 超商條碼
        public string Barcode_1 { get; set; }
        public string Barcode_2 { get; set; }
        public string Barcode_3 { get; set; }

        // 信用卡相關
        public string AuthCode { get; set; }
        public string Card4No { get; set; }
        public string Card6No { get; set; }
    }

    #endregion
}
