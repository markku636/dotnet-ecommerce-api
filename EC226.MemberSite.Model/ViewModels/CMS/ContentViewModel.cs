using System;


namespace EC226.MemberSite.Model.ViewModels.CMS
{
    public class ModifyContentReqBody
    {
        public string FormJsonValue { get; set; }
    }
    public class CreateScheduledContentReqBody
    {
        public string Key { get; set; }
        public string SchemaVersionId { get; set; }
        public string FormJsonValue { get; set; }
        public DateTime ScheduledTime { get; set; }
    }
    public class EditScheduledContentReqBody
    {
        public string FormJsonValue { get; set; }
        public DateTime ScheduledTime { get; set; }
    }
}
