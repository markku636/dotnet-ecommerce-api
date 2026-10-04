using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.Utility.Helper
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    namespace App.Package
    {
        public static class SymmetricHelper
        {
            private static readonly ICryptoTransform _decryptorDB;
            private static readonly ICryptoTransform _encryptorDB;
            private static readonly ICryptoTransform _decryptorClient;
            private static readonly ICryptoTransform _encryptorClient;

            static SymmetricHelper() {
                // 存取/拿出資料庫加密/解密
                var rijDB = Rijndael.Create();
                rijDB.Key = Encoding.UTF8.GetBytes(EC226.Configuration.AppSettingsHelper.GetContent("Encryption", "DbKey"));
                rijDB.IV = Encoding.UTF8.GetBytes(EC226.Configuration.AppSettingsHelper.GetContent("Encryption", "DbIV"));
                _decryptorDB = rijDB.CreateDecryptor();
                _encryptorDB = rijDB.CreateEncryptor();

                // 從前端傳進來的加密/解密
                var rijJSClient = Rijndael.Create();
                rijJSClient.Key = Encoding.UTF8.GetBytes(EC226.Configuration.AppSettingsHelper.GetContent("Encryption", "ClientKey"));
                rijJSClient.IV = Encoding.UTF8.GetBytes(EC226.Configuration.AppSettingsHelper.GetContent("Encryption", "ClientIV"));
                _decryptorClient = rijJSClient.CreateDecryptor();
                _encryptorClient = rijJSClient.CreateEncryptor();
            }

            public static string Decrypt(string Input, bool forDB = true) {
                if(string.IsNullOrWhiteSpace(Input)) return string.Empty;

                var bytes = Convert.FromBase64String(Input);
                using (var ms = new MemoryStream(bytes, 0, bytes.Length)) {
                    using (var cs = new CryptoStream(ms, forDB ? _decryptorDB : _decryptorClient, CryptoStreamMode.Read)) {
                        using (var reader = new StreamReader(cs)) {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }

            public static string Encrypt(string Input, bool forDB = true) {
                using (var ms = new MemoryStream()) {
                    using (var cs = new CryptoStream(ms, forDB ? _encryptorDB : _encryptorClient, CryptoStreamMode.Write)) {
                        using (var writer = new StreamWriter(cs)) {
                            writer.Write(Input);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }

        }
    }
}
