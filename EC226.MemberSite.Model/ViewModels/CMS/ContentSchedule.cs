using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SqlSugar.DbConvert;
using SqlSugar;


namespace EC226.MemberSite.Model.ViewModels.CMS
{
   public class ContentSchedule
   {
      public int Id { get; set; }
      public int LangId { get; set; } = 1;
      public string Key { get; set; }
      public string SchemaVersionId { get; set; }
      public string FormJsonValue { get; set; }
      public DateTime ScheduledTime { get; set; }
      public DateTime CreatedTime { get; set; }
      public string CreatedBy { get; set; }
      public DateTime ModifiedTime { get; set; }
      public string ModifiedBy { get; set; }
      [SugarColumn(ColumnDataType = "enum('Pending', 'Done', 'Failed', 'Canceled')", SqlParameterDbType = typeof(EnumToStringConvert))]
      public ContentScheduleStatus Status { get; set; } = ContentScheduleStatus.Pending;
      public string CanceledBy { get; set; }
   }

   [JsonConverter(typeof(StringEnumConverter))]
   public enum ContentScheduleStatus {
      [EnumMember(Value = "Pending")]
      Pending,
      [EnumMember(Value = "Done")]
      Done,
      [EnumMember(Value = "Failed")]
      Failed,
      [EnumMember(Value = "Canceled")]
      Canceled
   }
}
