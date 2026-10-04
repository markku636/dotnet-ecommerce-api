using System;
using System.Collections.Generic;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.MemberSite.IServices;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.Repository.UnitOfWork;
using EC226.MemberSite.Model.ViewModels.CMS;

namespace EC226.MemberSite.Services
{
   public class ContentService: IContentService
   {
      private readonly IUnitOfWork _unitOfWork;

      public ContentService(IUnitOfWork unitOfWork)
      {
         _unitOfWork = unitOfWork;
      }

      public Result<List<ContentValue>> GetList()
      {
         var db = _unitOfWork.GetDbClient();
         var list = db.Queryable<ContentValue>()
            .LeftJoin<ContentSchema>((v, s) => v.Key == s.Key)
            .OrderBy((v, s) => s.Sort)
            .Select((v, s) => v)
            .ToList();

         var result = new Result<List<ContentValue>>();
         result.Success();
         result.Data = list;
         result.Count = list.Count;
         return result;
      }

      public Result<ContentValue> GetContent(string key)
      {
         var db = _unitOfWork.GetDbClient();
         var item = db.Queryable<ContentValue>().Single(it => it.Key == key);

         var result = new Result<ContentValue>();
         result.Success();
         result.Data = item;
         result.Count = item == null ? 0 : 1;
         return result;
      }

      public Result ModifyContent(string key, ModifyContentReqBody body)
      {
         var result = new Result();

         try
         {
            var db = _unitOfWork.GetDbClient();

            int updatedRows = db.Updateable<ContentValue>()
               .SetColumns(it => new ContentValue()
               {
                  FormJsonValue = body.FormJsonValue,
                  ModifiedTime = DateTime.Now,
                  ModifiedBy = null,
               })
               .Where(it => it.Key == key)
               .ExecuteCommand();

            if (updatedRows > 0)
            {
               result.Success();
               result.Count = updatedRows;
            }
            else
            {
               result.Code = ApiReturnCode.Not_Exist;
               result.Message = $"Key {key} does not exist.";
            }

         }
         catch (Exception ex)
         {
            result.Code = ApiReturnCode.General_DB_Error;
            result.Message = ex.Message;
         }

         return result;
      }

      public Result<List<ContentSchedule>> GetScheduleList(string key)
      {
         var db = _unitOfWork.GetDbClient();
         var list = db.Queryable<ContentSchedule>()
            .Where(it => it.Key == key)
            .OrderBy(it => it.ScheduledTime)
            .ToList();

         var result = new Result<List<ContentSchedule>>();
         result.Success();
         result.Data = list;
         result.Count = list.Count;
         return result;
      }

      public Result<ContentSchedule> GetScheduleSingle(int id)
      {
         var db = _unitOfWork.GetDbClient();
         var item = db.Queryable<ContentSchedule>().Single(it => it.Id == id);

         var result = new Result<ContentSchedule>();
         result.Success();
         result.Data = item;
         result.Count = item == null ? 0 : 1;
         return result;
      }

      public Result CreateScheduledContent(CreateScheduledContentReqBody body)
      {
         var now = DateTime.Now;
         var result = new Result();

         try
         {
            result = ValidateScheduledTime(body.ScheduledTime);
            if (!result)
            {
               return result;
            }

            var db = _unitOfWork.GetDbClient();

            var checkItem1 = db.Queryable<ContentSchema>().Single(it => it.Key == body.Key);
            if (checkItem1 == null)
            {
               result.Code = ApiReturnCode.Not_Exist;
               result.Message = $"Key {body.Key} does not exist.";
               return result;
            }

            var checkItem2 = db.Queryable<ContentSchedule>()
               .Single(it => it.Key == body.Key && it.ScheduledTime == body.ScheduledTime && it.Status == ContentScheduleStatus.Pending);
            if (checkItem2 != null)
            {
               result.Code = ApiReturnCode.Duplicate_Key_Entry;
               result.Message = $"There is already a scheduled content update for Key {body.Key} at {body.ScheduledTime:MM/dd/yyyy HH:mm:ss}";
               return result;
            }

            var content = new ContentSchedule()
            {
               Key = body.Key,
               SchemaVersionId = body.SchemaVersionId,
               FormJsonValue = body.FormJsonValue,
               ScheduledTime = body.ScheduledTime,
               CreatedTime = now,
               ModifiedTime = now,
            };

            int createdRows = db.Insertable(content).ExecuteCommand();

            if (createdRows > 0)
            {
               result.Success();
               result.Count = createdRows;
            }
            else
            {
               result.Code = ApiReturnCode.Not_Exist;
               result.Message = $"Key {body.Key} does not exist.";
            }
         }
         catch (Exception ex)
         {
            result.Code = ApiReturnCode.General_DB_Error;
            result.Message = ex.Message;
         }

         return result;
      }

      public Result EditScheduledContent(int id, EditScheduledContentReqBody body)
      {
         var now = DateTime.Now;
         var result = new Result();

         try
         {
            result = ValidateScheduledTime(body.ScheduledTime);
            if (!result)
            {
               return result;
            }

            var db = _unitOfWork.GetDbClient();

            var checkItem1 = db.Queryable<ContentSchedule>().Single(it => it.Id == id);
            if (checkItem1 == null)
            {
               result.Code = ApiReturnCode.Not_Exist;
               result.Message = $"Id {id} does not exist.";
               return result;
            }
            else if (checkItem1.Status != ContentScheduleStatus.Pending)
            {
               result.Code = ApiReturnCode.Not_Allowed;
               result.Message = "You cannot modify already released or canceled scheduled content.";
               return result;
            }

            var checkItem2 = db.Queryable<ContentSchedule>()
               .Single(it => it.Key == checkItem1.Key && it.Id != id && it.ScheduledTime == body.ScheduledTime && it.Status == ContentScheduleStatus.Pending);
            if (checkItem2 != null)
            {
               result.Code = ApiReturnCode.Duplicate_Key_Entry;
               result.Message = $"There is already a scheduled content update for Key {checkItem1.Key} at {body.ScheduledTime:MM/dd/yyyy HH:mm:ss}";
               return result;
            }

            int updatedRows = db.Updateable<ContentSchedule>()
               .SetColumns(it => new ContentSchedule()
               {
                  FormJsonValue = body.FormJsonValue,
                  ScheduledTime = body.ScheduledTime,
                  ModifiedTime = now,
               })
               .Where(it => it.Id == id && it.Status == ContentScheduleStatus.Pending)
               .ExecuteCommand();

            result.Success();
            result.Count = updatedRows;

         }
         catch (Exception ex)
         {
            result.Code = ApiReturnCode.General_DB_Error;
            result.Message = ex.Message;
         }

         return result;
      }

      public Result CancelScheduledContent(int id)
      {
         var result = new Result();
         var db = _unitOfWork.GetDbClient();

         var checkItem = db.Queryable<ContentSchedule>().Single(it => it.Id == id);
         if (checkItem == null)
         {
            result.Code = ApiReturnCode.General_DB_Error;
            result.Message = $"Id {id} does not exist.";
            return result;
         }

         int updatedRows = db.Updateable<ContentSchedule>()
            .SetColumns(it => new ContentSchedule()
            {
               Status = ContentScheduleStatus.Canceled,
               CanceledBy = "API",
               ModifiedTime = DateTime.Now,
            })
            .Where(it => it.Id == id && it.Status == ContentScheduleStatus.Pending)
            .ExecuteCommand();

         if (updatedRows == 1)
         {
            result.Success();
            result.Count = 1;
         }
         else
         {
            result.Code = ApiReturnCode.Not_Allowed;
            result.Message = "You cannot cancel already released or canceled scheduled content.";
         }

         return result;
      }

      /// <summary>
      /// Checks that scheduledTime is in the future and is in whole hours.
      /// If it fails, it will return a result with an error message.
      /// </summary>
      public static Result ValidateScheduledTime(DateTime scheduledTime)
      {
         var now = DateTime.Now;
         var result = new Result();

         int comparedTimeNum = DateTime.Compare(now, scheduledTime);
         if (comparedTimeNum >= 0)
         {
            result.Fail("Scheduled time must be in the future", ApiReturnCode.Time_Range_Incorrect);
         }

         if (scheduledTime.Minute != 0 || scheduledTime.Second != 0 || scheduledTime.Millisecond != 0)
         {
            result.Fail("Scheduled time must be whole hours", ApiReturnCode.Time_Range_Incorrect);
         }


         return result;
      }

   }
}
