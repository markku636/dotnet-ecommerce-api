
using System.Collections;
using System.Collections.Generic;

namespace EC226.MemberSite.Model.ViewModels.UI
{

   /// <summary>
   ///     驗證錯誤資訊檢視模型
   /// </summary>
   public class ErrorView
   {

      /// <summary>
      ///     錯誤欄位 ( 錯誤欄位/ cartitemID )
      /// </summary>

      public string ID { get; set; }


      public string DataType { get; set; }

      /// <summary>
      ///     錯誤內容ErrorMessage
      /// </summary>


      public Dictionary<string, string> ErrorObj { get; set; }


   }


   public enum ErrorObjectKey
   {
      Message,
      Stock,
      ProductType,
   }

   /// <summary>
   /// 一般的Result(沒回傳值)
   /// </summary>
   public class Result
   {
      /// <summary>
      /// 初始化
      /// </summary>
      public Result(string message = "Fail!")
      {
         Code = ApiReturnCode.General_Error;
         Errors = new List<ErrorView>();
      }

      public int Count { get; set; }

      public string Data { get; set; }
      public int ID { get; set; }

      /// <summary>
      /// 是否成功
      /// </summary>
      public bool IsSuccess { get; set; }

      public ApiReturnCode Code { get; set; }

      public string Message { get; set; }

      /// <summary>
      /// 是否執行其他的方法
      /// </summary>
      //public string Action { get; set; }

      public List<ErrorView> Errors { get; set; } = new List<ErrorView>();

      public Result Success(string message = "Success")
      {
         IsSuccess = true;
         Code = ApiReturnCode.Success;
         Message = message;
         return this;
      }

      public void Fail(string message = "Fail", ApiReturnCode errorCode = ApiReturnCode.General_Error)
      {
         IsSuccess = false;
         Code = errorCode;
         Message = message;
      }

      /// <summary>
      /// 可以直接拿來判斷
      /// </summary>
      /// <param name="other"></param>
      public static implicit operator bool(Result other)
      {
         return other.IsSuccess;
      }
   }

   /// <summary> 
   /// 泛行的Result 
   /// </summary> 
   /// <typeparam name="T"></typeparam> 
   public class Result<T>: Result where T : class, new()
   {
      public Result()
      {
         Data = new T();
         Code = ApiReturnCode.General_Error;
         Errors = new List<ErrorView>();
      }

      /// <summary> 
      /// 原本的資料 
      /// </summary> 
      public T Data { get; set; }

      public static implicit operator bool(Result<T> other)
      {
         return other.IsSuccess;
      }
   }
}
