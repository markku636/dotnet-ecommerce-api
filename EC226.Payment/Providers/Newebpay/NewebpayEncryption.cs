using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EC226.Payment.Providers.Newebpay
{
    /// <summary>
    /// 藍新金流加解密工具
    /// </summary>
    public static class NewebpayEncryption
    {
        /// <summary>
        /// AES-256-CBC 加密 (產生 TradeInfo)
        /// </summary>
        /// <param name="plainText">明文 (TradeInfo JSON 或 QueryString)</param>
        /// <param name="hashKey">HashKey (32 bytes)</param>
        /// <param name="hashIV">HashIV (16 bytes)</param>
        /// <returns>加密後的 Hex 字串</returns>
        public static string AesEncrypt(string plainText, string hashKey, string hashIV)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(hashKey))
                throw new ArgumentNullException(nameof(hashKey));
            if (string.IsNullOrEmpty(hashIV))
                throw new ArgumentNullException(nameof(hashIV));

            var key = Encoding.UTF8.GetBytes(hashKey);
            var iv = Encoding.UTF8.GetBytes(hashIV);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // 轉為 Hex 字串
            return BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// AES-256-CBC 解密
        /// </summary>
        /// <param name="encryptedHex">加密的 Hex 字串</param>
        /// <param name="hashKey">HashKey (32 bytes)</param>
        /// <param name="hashIV">HashIV (16 bytes)</param>
        /// <returns>解密後的明文</returns>
        public static string AesDecrypt(string encryptedHex, string hashKey, string hashIV)
        {
            if (string.IsNullOrEmpty(encryptedHex))
                throw new ArgumentNullException(nameof(encryptedHex));
            if (string.IsNullOrEmpty(hashKey))
                throw new ArgumentNullException(nameof(hashKey));
            if (string.IsNullOrEmpty(hashIV))
                throw new ArgumentNullException(nameof(hashIV));

            var key = Encoding.UTF8.GetBytes(hashKey);
            var iv = Encoding.UTF8.GetBytes(hashIV);

            // Hex 字串轉為 bytes
            var encryptedBytes = HexStringToBytes(encryptedHex);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        /// <summary>
        /// 產生 TradeSha (SHA256 雜湊)
        /// </summary>
        /// <param name="tradeInfo">加密後的 TradeInfo (Hex 字串)</param>
        /// <param name="hashKey">HashKey</param>
        /// <param name="hashIV">HashIV</param>
        /// <returns>SHA256 雜湊值 (大寫)</returns>
        public static string GenerateTradeSha(string tradeInfo, string hashKey, string hashIV)
        {
            if (string.IsNullOrEmpty(tradeInfo))
                throw new ArgumentNullException(nameof(tradeInfo));
            if (string.IsNullOrEmpty(hashKey))
                throw new ArgumentNullException(nameof(hashKey));
            if (string.IsNullOrEmpty(hashIV))
                throw new ArgumentNullException(nameof(hashIV));

            // 組成待雜湊字串: HashKey={key}&{tradeInfo}&HashIV={iv}
            var rawString = $"HashKey={hashKey}&{tradeInfo}&HashIV={hashIV}";

            // SHA256 雜湊
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(rawString);
            var hash = sha256.ComputeHash(bytes);

            // 轉為大寫 Hex 字串
            return BitConverter.ToString(hash).Replace("-", "").ToUpper();
        }

        /// <summary>
        /// 驗證 TradeSha
        /// </summary>
        public static bool ValidateTradeSha(string tradeInfo, string tradeSha, string hashKey, string hashIV)
        {
            var calculatedSha = GenerateTradeSha(tradeInfo, hashKey, hashIV);
            return string.Equals(calculatedSha, tradeSha, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Hex 字串轉為 byte 陣列
        /// </summary>
        private static byte[] HexStringToBytes(string hex)
        {
            hex = hex.Replace("-", "").Replace(" ", "");
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
    }
}
