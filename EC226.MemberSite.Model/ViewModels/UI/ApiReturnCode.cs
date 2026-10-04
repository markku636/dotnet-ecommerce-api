namespace EC226.MemberSite.Model.ViewModels.UI
{
   /**   
   * [注意]
   * 新增 ApiReturnCode 時：
   * 1. 時前端專案 enum ApiReturnCode 也要更新
   * 2. Excel表也要新增key跟翻譯，key 形式是 api-return-code-{number}
   *    例如 api-return-code-30001
   *    (多語系對照表請放在團隊內部文件)
   */
   public enum ApiReturnCode
   {
      // 成功 > 2xxxx
      Success = 20000,

      // 輸入參數相關 3xxxx
      Invalid_Parameter = 30001,
      Time_Range_Incorrect = 30004,//"時間段格式不正確"
      Zip_Code_Incorrect = 30005,//"郵遞區號格式不正確"
      Stock_Not_Enough = 30006,//"庫存不足"
      Out_Of_Stock = 30007,//"無庫存"

      // 登入相關 400xx
      Invalid_Access_Token = 40001,
      Already_Login_In_Other_Place = 40002,
      Access_Token_Expired = 40003,
      User_Not_Exist = 40004,
      Username_Or_Password_Is_Incorrect = 40005,
      Turnstile_Fail = 40006,
      Account_Not_Activated = 40007,
      Verification_Code_Fail = 40008, // 重設密碼驗證碼錯誤

      // 註冊相關 401xx
      Username_Is_Repeat = 40101,
      Account_Activation_Fail = 40102,

      // 購物車相關 41xxx
      Country_Group_Not_Allowed = 41001, // 國家不允許
      Country_Not_Allowed = 41002,
      General_Payment_Fail = 41003,
      Shipping_Method_Not_Existed = 41004,
      Shipping_Methods_Country_Not_Allow = 41005,
      CustNo_Empty = 41006,
      Pre_Check_Skus_Stock_Fail = 41007,
      Pre_Check_Model_Stock_Fail = 41008,
      CustNo_Already_Used = 41009,
      Quantity_Less_Than_1 = 41010,

      // 支付相關 42xxx
      PaymentCode_Not_Existed = 42001,
      Payment_Methods_Country_Not_Allow = 42002,
      PaymentIsDsiable = 42003,

      // 結帳相關 43xxx
      TempOrderVaildError = 43001,
      OrderVaildError = 43002,
      Get_Final_Amount_Fail = 43003,
      ModelVaildError = 43004,

      // 一般
      Authorize = 40100,
      Forbidden = 40300,
      TooManyRequests = 44101,

      // 伺服器錯誤 5xxxx
      Internal_Error = 50000,
      Third_Party_Fail = 50001,

      // 資料庫 6xxxx
      General_DB_Error = 60000,
      Duplicate_Key_Entry = 60001,
      Not_Exist = 60002,
      Not_Allowed = 60003,

      // 通用
      General_Error = 99999,
      Invild_Parameter = 100000,
      Email_Exist = 100001,
   }
}