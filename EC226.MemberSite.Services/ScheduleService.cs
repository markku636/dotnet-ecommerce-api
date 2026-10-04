using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels.CMS;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Repository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.Services
{
   public class ScheduleService: IScheduleService
   {
      private readonly IUnitOfWork _unitOfWork;
      public ScheduleService(IUnitOfWork unitOfWork)
      {
         _unitOfWork = unitOfWork;
      }

      public Result TriggerSchedule()
      {
         var result = new Result();
         var now = DateTime.Now;

         var noNeedToPublishListIds = new List<int>();
         var needToPublishList = new List<ScheduleJoinValueViewModel>();
         var currentKeys = new List<string>();

         try
         {
            
            var list = _unitOfWork.GetDbClient().Queryable<ContentSchedule>()
                .LeftJoin<ContentValue>((s, v) => v.Key == s.Key)
                .Where(s => s.ScheduledTime <= now && (s.Status == ContentScheduleStatus.Pending || s.Status == ContentScheduleStatus.Failed))
                .OrderByDescending(s => s.ScheduledTime)
                .Select((s, v) => new ScheduleJoinValueViewModel
                {
                   Id = s.Id,
                   LangId = s.LangId,
                   Key = s.Key,
                   SchemaVersionId = s.SchemaVersionId,
                   FormJsonValue = s.FormJsonValue,
                   ScheduledTime = s.ScheduledTime,
                   CreatedTime = s.CreatedTime,
                   CreatedBy = s.CreatedBy,
                   ModifiedTime = s.ModifiedTime,
                   ModifiedBy = s.ModifiedBy,
                   Status = s.Status,
                   CanceledBy = s.CanceledBy,
                   BeforeJsonValue = v.FormJsonValue
                })
                .ToList();

            for (int i = 0; i < list.Count; i++)
            {
               var item = list[i];
               if (currentKeys.Contains(item.Key))
               {
                  noNeedToPublishListIds.Add(item.Id);
               }
               else
               {
                  needToPublishList.Add(item);
                  currentKeys.Add(item.Key);
               }
            }

            int updatedRowsCSCanceled = _unitOfWork.GetDbClient().Updateable<ContentSchedule>()
                .SetColumns(it => new ContentSchedule()
                {
                   Status = ContentScheduleStatus.Canceled,
                   CanceledBy = "TriggerAPI",
                   ModifiedTime = now,
                })
                .Where(it => noNeedToPublishListIds.Contains(it.Id))
                .ExecuteCommand();

            PublishScheduledContent(needToPublishList, "TriggerAPI");
         }
         catch (Exception ex)
         {
            result.Code = ApiReturnCode.General_DB_Error;
            result.Message = ex.Message;
         }

         result.Success();
         result.Count = needToPublishList.Count;
         return result;
      }

      public void PublishScheduledContent()
      {
         try
         {
            Console.WriteLine(DateTime.Now.ToString());
            var now = DateTime.Now;
            var twentyMinutesBefore = now.AddMinutes(-20);
            var twentyMinutesAfter = now.AddMinutes(20);

            // Select scheduled content that is scheduled to be published now

            var needToPublishList = _unitOfWork.GetDbClient().Queryable<ContentSchedule>()
                .LeftJoin<ContentValue>((s, v) => v.Key == s.Key)
                .Where(s => s.ScheduledTime <= twentyMinutesAfter && s.ScheduledTime >= twentyMinutesBefore
                    && s.Status == ContentScheduleStatus.Pending)
                .Select((s, v) => new ScheduleJoinValueViewModel
                {
                   Id = s.Id,
                   LangId = s.LangId,
                   Key = s.Key,
                   SchemaVersionId = s.SchemaVersionId,
                   FormJsonValue = s.FormJsonValue,
                   ScheduledTime = s.ScheduledTime,
                   CreatedTime = s.CreatedTime,
                   CreatedBy = s.CreatedBy,
                   ModifiedTime = s.ModifiedTime,
                   ModifiedBy = s.ModifiedBy,
                   Status = s.Status,
                   CanceledBy = s.CanceledBy,
                   BeforeJsonValue = v.FormJsonValue
                })
                .ToList();
            var keys = needToPublishList.Select(it => it.Key).ToList();
            PublishScheduledContent(needToPublishList, "CronJob");

            var historicalList = _unitOfWork.GetDbClient().Queryable<ContentSchedule>()
                .Where(
                    s => s.ScheduledTime <= twentyMinutesAfter
                    && (s.Status == ContentScheduleStatus.Pending || s.Status == ContentScheduleStatus.Failed)
                    )
                .ToList();

            // Update status of historical "Pending" or "Failed" scheduled content with the same keys as "Canceled"
            var canceledIds = historicalList.Where(it => keys.Contains(it.Key)).Select(it => it.Id).ToList();
            int updatedRowsCSCanceled = _unitOfWork.GetDbClient().Updateable<ContentSchedule>()
               .SetColumns(it => new ContentSchedule()
               {
                  Status = ContentScheduleStatus.Canceled,
                  CanceledBy = "CronJob",
                  ModifiedTime = now,
               })
               .Where(it => canceledIds.Contains(it.Id))
               .ExecuteCommand();

            // For the other keys, update status of historical "Pending" scheduled content as "Failed"
            var failedIds = historicalList.Where(it => !keys.Contains(it.Key)).Select(it => it.Id).ToList();
            int updatedRowsCSFailed = _unitOfWork.GetDbClient().Updateable<ContentSchedule>()
                .SetColumns(it => new ContentSchedule()
                {
                   Status = ContentScheduleStatus.Failed,
                })
                .Where(it => failedIds.Contains(it.Id))
                .ExecuteCommand();

         }
         catch (Exception ex)
         {
            // TODO!! add error logs
         }
      }

      public  void PublishScheduledContent(List<ScheduleJoinValueViewModel> needToPublishList, string modifiedBy)
      {
         var now = DateTime.Now;        

         foreach (var item in needToPublishList)
         {
            // 1. Update ContentValue
            
            int updatedRowsCV = _unitOfWork.GetDbClient().Updateable<ContentValue>()
                .SetColumns(it => new ContentValue()
                {
                   FormJsonValue = item.FormJsonValue,
                   ModifiedBy = modifiedBy,
                   ModifiedTime = now,
                })
                .Where(it => it.Key == item.Key)
                .ExecuteCommand();

            // 2. Insert entry to ContentScheduleLog
            
            var contentScheduleLogItem = new ContentScheduleLog()
            {
               ScheduleId = item.Id,
               BeforeJsonValue = item.BeforeJsonValue,
               AfterJsonValue = item.FormJsonValue,
               CreatedTime = now,
            };
            _unitOfWork.GetDbClient().Insertable(contentScheduleLogItem).ExecuteCommand();

            // 3. Update ContentSchedule status
            int updatedRowsCS2 = _unitOfWork.GetDbClient().Updateable<ContentSchedule>()
            .SetColumns(it => new ContentSchedule()
            {
               Status = ContentScheduleStatus.Done,
            })
            .Where(it => it.Id == item.Id)
            .ExecuteCommand();
         }
      }

   }
}
