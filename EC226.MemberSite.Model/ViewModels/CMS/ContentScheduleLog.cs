using System;

namespace EC226.MemberSite.Model.ViewModels.CMS
{
    public class ContentScheduleLog
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public string BeforeJsonValue { get; set; }
        public string AfterJsonValue { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
