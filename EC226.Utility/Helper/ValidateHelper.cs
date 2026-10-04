using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsValidator;
using System.Linq;
using System.Reflection;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.Utility.Helper
{
   public class ValidateHelper
   {
      public static Result ModelValidate(object model)
      {
         var result = new Result() { Code = ApiReturnCode.ModelVaildError };

         var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
         var validationResults = new List<ValidationResult>();

         var isVaild = Validator.TryValidateObject(model, validationContext, validationResults, true);

         if (isVaild)
         {
            result.Success();
            return result;
         }

         var errorViews = new List<ErrorView>();

         foreach (var validationResult in validationResults)
         {
            foreach (var memberName in validationResult.MemberNames)
            {
               errorViews.Add(new ErrorView
               {
                  ID = memberName,
                  ErrorObj = new Dictionary<string, string> { { ErrorObjectKey.Message.ToString(), validationResult.ErrorMessage } }
               });
            }
         }

         result.Errors = errorViews;

         return result;
      }

      public static Result ModelValidateByRecursive<T>(T obj)
      {
         var result = new Result() { Code = ApiReturnCode.ModelVaildError };

         List<ValidationResult> validationResults = new List<ValidationResult>();
         var isVaild = TryValidateObjectRecursive(obj, validationResults);

         if (isVaild)
         {
            result.Success();
            return result;
         }

         var errorViews = new List<ErrorView>();

         foreach (var validationResult in validationResults)
         {
            foreach (var memberName in validationResult.MemberNames)
            {
               errorViews.Add(new ErrorView
               {
                  ID = memberName,
                  ErrorObj = new Dictionary<string, string> { { ErrorObjectKey.Message.ToString(), validationResult.ErrorMessage } }
               });
            }
         }

         result.Errors = errorViews;
         return result;

      }

      public static bool TryValidateObjectRecursive<T>(T obj, List<ValidationResult> validationResults)
      {

         var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);

         var isVaild = Validator.TryValidateObject(obj, validationContext, validationResults, true);

         var properties = obj.GetType().GetProperties().Where(prop => prop.CanRead
             && !prop.GetCustomAttributes(typeof(SkipRecursiveValidation), false).Any()
             && prop.GetIndexParameters().Length == 0).ToList();

         foreach (var property in properties)
         {
            if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType) continue;

            var value = obj.GetPropertyValue(property.Name);

            if (value == null) continue;

            var asEnumerable = value as IEnumerable;
            if (asEnumerable != null)
            {
               foreach (var enumObj in asEnumerable)
               {
                  var nestedResults = new List<ValidationResult>();
                  if (!TryValidateObjectRecursive(enumObj, nestedResults))
                  {
                     isVaild = false;
                     foreach (var validationResult in nestedResults)
                     {
                        PropertyInfo property1 = property;
                        validationResults.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                     }
                  };
               }
            }
            else
            {
               var nestedResults = new List<ValidationResult>();
               if (!TryValidateObjectRecursive(value, nestedResults))
               {
                  isVaild = false;
                  foreach (var validationResult in nestedResults)
                  {
                     PropertyInfo property1 = property;
                     validationResults.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                  }
               }
            }
         }

         return isVaild;
      }


      public static Result ExecuteChecks(List<(Func<Result> Check, string Name)> checks)
      {
         foreach (var (Check, Name) in checks)
         {
            var result = Check();

            if (!result.IsSuccess)
            {
               return result;
            }
         }

         // 所有檢查都通過
         return new Result { IsSuccess = true };
      }

   }




}
