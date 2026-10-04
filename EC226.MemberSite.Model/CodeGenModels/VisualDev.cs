using SqlSugar;

namespace EC226.MemberSite.Model.CodeGenModels.Models
{
   public class VisualDev
   {
      [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
      public int Id { get; set; }      

      public string ApiId { get; set; }
      
      public int Sort { get; set; } 

      public string Description { get; set; }
      // 状态(0-暂存（默认），1-发布)
      public int State { get; set; }      

      [SugarColumn(IsNullable = true, ColumnDataType = "LONGTEXT", ColumnDescription = "表单配置JSON")]
      public string FormData { get;set;}
      // 列表配置JSON
      [SugarColumn(IsNullable = true, ColumnDataType = "LONGTEXT", ColumnDescription = "列表配置JSON")]
      public string ColumnData {get;set;}
   }
}
