using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.MemberSite.IServices
{
   public interface IScheduleService
   {
      Result TriggerSchedule();
   }
}
