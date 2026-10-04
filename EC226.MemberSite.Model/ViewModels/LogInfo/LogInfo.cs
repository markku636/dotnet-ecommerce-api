
using System;

namespace EC226.MemberSite.Model.ViewModels.LogInfo
{
    /// <summary>
    ///     日誌實體
    /// </summary>
    public class LogInfo
    {
        public DateTime Datetime { get; set; }
        public string Content { get; set; }
        public string IP { get; set; }
        public string LogColor { get; set; }
        public int Import { get; set; } = 0;
    }
}
