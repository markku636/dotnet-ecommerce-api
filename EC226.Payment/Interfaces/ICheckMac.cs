using System.Collections.Generic;

namespace EC226.Payment.Interfaces
{
    /// <summary>
    /// 綠界服務介接的檢查碼機制。
    /// </summary>
    public interface ICheckMac
    {
        /// <summary>
        /// 產生檢查碼。
        /// </summary>
        string GetValue(IGeneralPayment payment, EHashAlgorithm encryptType = EHashAlgorithm.SHA256);
    }
}