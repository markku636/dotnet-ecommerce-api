namespace EC226.MemberSite.Model.ViewModels.UI
{
    /// <summary>
    ///     LayUIAdmin後端UI回撥Json實體
    /// </summary>
    public class AdminUiCallBack
    {
        /// <summary>
        ///     狀態碼(ok = 0, error = 1, timeout = 401)
        /// </summary>
        public int code { get; set; } = 1;

        /// <summary>
        ///     可選。資訊內容。
        /// </summary>
        public string msg { get; set; } = "空數據返回";

        public object data { get; set; }

        public object otherData { get; set; }

        public int count { get; set; }
    }
}
