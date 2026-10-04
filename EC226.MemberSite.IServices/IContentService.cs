using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC226.MemberSite.Model.ViewModels.CMS;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.IServices
{
   public interface IContentService
   {
      Result<List<ContentValue>> GetList();
      Result<ContentValue> GetContent(string key);
      Result ModifyContent(string key, ModifyContentReqBody body);
      Result<List<ContentSchedule>> GetScheduleList(string key);
      Result<ContentSchedule> GetScheduleSingle(int id);
      Result CreateScheduledContent(CreateScheduledContentReqBody body);
      Result EditScheduledContent(int id, EditScheduledContentReqBody body);
      Result CancelScheduledContent(int id);
   }
}
