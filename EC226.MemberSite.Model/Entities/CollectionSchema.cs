using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC226.MemberSite.Model.CodeGenModels;
using SqlSugar;



namespace EC226.MemberSite.Model.Entities
{

   [SugarTable("CollectionSchema")]
   public class CollectionSchema
   {
      [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
      public int Id { get; set; } // 主鍵，自動遞增

      public string FunctionName { get; set; } // Function 名稱

      public string FunctionKey { get; set; } // Function 鍵

      public int Sort { get; set; } // 排序

      public int DatabaseId { get; set; } // 資料庫 ID

      public int MainTableId { get; set; } // 主要資料表 ID

      public string FormSchema { get; set; } // 表單結構

      [SugarColumn(IsNullable = false, DefaultValue = "CURRENT_TIMESTAMP")]
      public DateTime CreatedTime { get; set; } // 創建時間

      public string CreatedBy { get; set; } // 創建者

      [SugarColumn(IsNullable = false, DefaultValue = "CURRENT_TIMESTAMP")]
      public DateTime ModifiedTime { get; set; } // 修改時間

      public string ModifiedBy { get; set; } // 修改者


      [Navigate(NavigateType.OneToMany, nameof(CollectionForeignDataSource.SchemaId), nameof(Id))]
      public List<CollectionForeignDataSource> CollectionForeignDataSources {  get; set; }
   }

   // 外鍵表: CollectionForeignDataSource
   [SugarTable("CollectionForeignDataSource")]
   public class CollectionForeignDataSource
   {
      [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
      public int Id { get; set; } // 主鍵，自動遞增

      public int TableId { get; set; } // 表格 ID (外鍵，參考主表)

      public int ForeignKeyId { get; set; } // 外鍵

      public int RelatedKeyId { get; set; } // 關聯的鍵

      public int RelatedType { get; set; } // 關聯的類型

      public int SchemaId { get; set; }
   }


   public class CollectionSchemaViewModel
   {
      public CollectionSchema CollectionSchema { get; set; }

      public List<CodeColumns> CodeColumns { get; set; }

      public List<CodeTable> Tables { get; set; }
   }



}
