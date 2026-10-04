using System;

namespace EC226.MemberSite.Model.ViewModels.CMS
{
   public class ContentSchema
   {
      public int Id { get; set; }
      public string Key { get; set; }
      public int Sort { get; set; } = 0;
      public string Description { get; set; } = string.Empty;
      public string FormSchema { get; set; } = string.Empty;
      public string TableColumnData { get; set; }
      public DateTime CreatedTime { get; set; }
      public string CreatedBy { get; set; }
      public DateTime ModifiedTime { get; set; }
      public string ModifiedBy { get; set; }
      public string VersionId { get; set; } = string.Empty;
    }
}
