using System;

namespace EC226.MemberSite.Model.ViewModels.CMS
{
   public class ContentValue
   {
      public int Id { get; set; }
      public int LangId { get; set; } = 1;
      public string Key { get; set; }
      public string SchemaVersionId { get; set; } = string.Empty;
      public string FormJsonValue { get; set; } = string.Empty;
      public DateTime ModifiedTime { get; set; }
      public string ModifiedBy { get; set; }
   }
}
