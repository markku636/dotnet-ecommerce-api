using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Web;
using EC226.Payment.Abstractions;
using Microsoft.AspNetCore.Http;

namespace EC226.Payment.Providers.ECPay
{
    /// <summary>
    /// 綠界金流 Provider
    /// </summary>
    public class ECPayPaymentProvider : IPaymentProvider
    {
        private readonly ECPayConfig _config;

        public PaymentProviderType ProviderType => PaymentProviderType.ECPay;

        public ECPayPaymentProvider(ECPayConfig config)
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

                var parameters = BuildParameters(request);
                var checkMacValue = GenerateCheckMacValue(parameters);
                parameters["CheckMacValue"] = checkMacValue;

                var htmlForm = GenerateHtmlForm(parameters);

                return PaymentResponse.CreateSuccess(htmlForm, request.OrderNo, JsonSerializer.Serialize(parameters));
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
                var parameters = form.Keys.ToDictionary(k => k, k => form[k].ToString());
                return ValidateAndParseCallback(parameters);
            }
            catch (Exception ex)
            {
                return PaymentCallbackResult.CreateInvalid($"解析回調失敗: {ex.Message}");
            }
        }

        /// <summary>
        /// 驗證並解析回調通知 (From Encrypted Data) - ECPay 不使用此方法
        /// </summary>
        public PaymentCallbackResult ValidateCallback(string encryptedData)
        {
            return PaymentCallbackResult.CreateInvalid("ECPay 不支援加密資料回調");
        }

        /// <summary>
        /// 產生回應給金流商的內容
        /// </summary>
        public string GenerateCallbackResponse(bool success)
        {
            return success ? "1|OK" : "0|ERROR";
        }

        #region Private Methods

        private void ValidateRequest(PaymentRequest request)
        {
            if (string.IsNullOrEmpty(request.OrderNo))
                throw new ArgumentException("訂單編號不可為空", nameof(request.OrderNo));
            if (request.Amount <= 0)
                throw new ArgumentException("交易金額必須大於 0", nameof(request.Amount));
            if (string.IsNullOrEmpty(request.ItemDescription))
                throw new ArgumentException("商品描述不可為空", nameof(request.ItemDescription));
        }

        private Dictionary<string, string> BuildParameters(PaymentRequest request)
        {
            var tradeNo = GenerateTradeNo(request.OrderNo);

            var parameters = new Dictionary<string, string>
            {
                ["MerchantID"] = _config.MerchantID,
                ["MerchantTradeNo"] = tradeNo,
                ["MerchantTradeDate"] = request.TradeDate.ToString("yyyy/MM/dd HH:mm:ss"),
                ["PaymentType"] = "aio",
                ["TotalAmount"] = Convert.ToInt32(request.Amount).ToString(),
                ["TradeDesc"] = HttpUtility.UrlEncode(request.ItemDescription),
                ["ItemName"] = string.IsNullOrEmpty(request.ItemName) ? request.ItemDescription : request.ItemName,
                ["ReturnURL"] = GetFullUrl(_config.NotifyUrl),
                ["ChoosePayment"] = ConvertPaymentMethod(request.PaymentMethod),
                ["EncryptType"] = _config.EncryptType.ToString()
            };

            // 選填參數
            if (!string.IsNullOrEmpty(_config.ReturnUrl))
                parameters["ClientBackURL"] = GetFullUrl(_config.ReturnUrl);

            if (!string.IsNullOrEmpty(_config.StoreID))
                parameters["StoreID"] = _config.StoreID;

            if (_config.IsPlatform)
                parameters["PlatformID"] = _config.MerchantID;

            if (!string.IsNullOrEmpty(request.CustomField1))
                parameters["CustomField1"] = request.CustomField1;
            if (!string.IsNullOrEmpty(request.CustomField2))
                parameters["CustomField2"] = request.CustomField2;
            if (!string.IsNullOrEmpty(request.CustomField3))
                parameters["CustomField3"] = request.CustomField3;
            if (!string.IsNullOrEmpty(request.CustomField4))
                parameters["CustomField4"] = request.CustomField4;

            if (!string.IsNullOrEmpty(request.Remark))
                parameters["Remark"] = request.Remark;

            return parameters;
        }

        private string GenerateTradeNo(string orderNo)
        {
            // 綠界交易編號最長 20 碼
            var maxLength = 20;
            if (orderNo.Length >= maxLength)
                return orderNo.Substring(0, maxLength);

            var randomLength = maxLength - orderNo.Length;
            var random = new Random();
            var randomPart = new string(Enumerable.Range(0, randomLength)
                .Select(_ => "0123456789"[random.Next(10)]).ToArray());

            return orderNo + randomPart;
        }

        private string ConvertPaymentMethod(PaymentMethodType method)
        {
            return method switch
            {
                PaymentMethodType.Credit => "Credit",
                PaymentMethodType.WebATM => "WebATM",
                PaymentMethodType.ATM => "ATM",
                PaymentMethodType.CVS => "CVS",
                PaymentMethodType.BARCODE => "BARCODE",
                PaymentMethodType.UnionPay => "Credit", // 銀聯卡走信用卡通道
                _ => "ALL"
            };
        }

        private string GetFullUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            // 如果已是完整網址則直接返回
            if (url.StartsWith("http://") || url.StartsWith("https://"))
                return url;

            // 否則需要組合完整網址 (這裡假設由外部傳入完整網址)
            return url;
        }

        private string GenerateCheckMacValue(Dictionary<string, string> parameters)
        {
            // 1. 按 Key 排序
            var sortedParams = parameters
                .Where(p => !string.IsNullOrEmpty(p.Value))
                .OrderBy(p => p.Key)
                .Select(p => $"{p.Key}={p.Value}");

            // 2. 組成字串
            var paramString = string.Join("&", sortedParams);
            var rawString = $"HashKey={_config.HashKey}&{paramString}&HashIV={_config.HashIV}";

            // 3. URL Encode 並轉小寫
            var encodedString = HttpUtility.UrlEncode(rawString).ToLower();

            // 4. SHA256 或 MD5
            string hashValue;
            if (_config.EncryptType == 1)
            {
                hashValue = ComputeSHA256(encodedString);
            }
            else
            {
                hashValue = ComputeMD5(encodedString);
            }

            // 5. 轉大寫
            return hashValue.ToUpper();
        }

        private string ComputeSHA256(string input)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        private string ComputeMD5(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        private string GenerateHtmlForm(Dictionary<string, string> parameters)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html><head><meta charset=\"utf-8\"></head><body>");
            sb.AppendLine($"<form id=\"ecpay-form\" method=\"post\" action=\"{_config.ApiUrl}\">");

            foreach (var param in parameters)
            {
                sb.AppendLine($"<input type=\"hidden\" name=\"{param.Key}\" value=\"{HttpUtility.HtmlEncode(param.Value)}\" />");
            }

            sb.AppendLine("<input type=\"submit\" value=\"前往付款\" style=\"display:none;\" />");
            sb.AppendLine("</form>");
            sb.AppendLine("<script>document.getElementById('ecpay-form').submit();</script>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

        private PaymentCallbackResult ValidateAndParseCallback(Dictionary<string, string> parameters)
        {
            // 取得 CheckMacValue
            if (!parameters.TryGetValue("CheckMacValue", out var receivedCheckMac))
            {
                return PaymentCallbackResult.CreateInvalid("缺少 CheckMacValue");
            }

            // 移除 CheckMacValue 後重新計算
            var paramsForCheck = parameters
                .Where(p => p.Key != "CheckMacValue")
                .ToDictionary(p => p.Key, p => p.Value);

            var calculatedCheckMac = GenerateCheckMacValue(paramsForCheck);

            if (!string.Equals(receivedCheckMac, calculatedCheckMac, StringComparison.OrdinalIgnoreCase))
            {
                return PaymentCallbackResult.CreateInvalid("CheckMacValue 驗證失敗");
            }

            // 解析結果
            var rtnCode = parameters.GetValueOrDefault("RtnCode", "0");
            var isPaid = rtnCode == "1";

            var result = new PaymentCallbackResult
            {
                IsValid = true,
                IsPaid = isPaid,
                OrderNo = ExtractOrderNo(parameters.GetValueOrDefault("MerchantTradeNo", "")),
                TradeNo = parameters.GetValueOrDefault("TradeNo", ""),
                Amount = decimal.TryParse(parameters.GetValueOrDefault("TradeAmt", "0"), out var amt) ? amt : 0,
                PaymentType = parameters.GetValueOrDefault("PaymentType", ""),
                StatusCode = rtnCode,
                StatusMessage = parameters.GetValueOrDefault("RtnMsg", ""),
                CustomField1 = parameters.GetValueOrDefault("CustomField1", ""),
                CustomField2 = parameters.GetValueOrDefault("CustomField2", ""),
                CustomField3 = parameters.GetValueOrDefault("CustomField3", ""),
                CustomField4 = parameters.GetValueOrDefault("CustomField4", ""),
                RawData = JsonSerializer.Serialize(parameters),
                ResponseContent = isPaid ? "1|OK" : "0|ERROR"
            };

            // 解析付款時間
            if (parameters.TryGetValue("PaymentDate", out var paymentDateStr) &&
                DateTime.TryParse(paymentDateStr, out var paymentDate))
            {
                result.PaymentTime = paymentDate;
            }

            return result;
        }

        private string ExtractOrderNo(string merchantTradeNo)
        {
            // 移除後面的隨機碼，取得原始訂單編號
            // 這裡需要根據實際的訂單編號格式來調整
            return merchantTradeNo;
        }

        #endregion
    }
}
