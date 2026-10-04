namespace EC226.MemberSite.Model.Enums
{
    public enum ReturnType
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 失敗
        /// </summary>
        Fail = 1,

        /// <summary>
        /// 傳入資料不被驗證
        /// </summary>
        ModelVaildError = 2,
        
        /// <summary>
        /// SessionTimeOut
        /// </summary>
        SessionTimeOut = 3
    }
}