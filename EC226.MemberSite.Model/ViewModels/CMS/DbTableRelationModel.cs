using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels.CMS
{

   public class DbTableRelationModel
   {
      /// <summary>
      /// 类型：1-主表、0-子表.
      /// </summary>
      public string TypeId { get; set; }

      /// <summary>
      /// 表名.
      /// </summary>
      public string Table { get; set; }

      /// <summary>
      /// 说明.
      /// </summary>
      public string TableName { get; set; }

      /// <summary>
      /// 主键.
      /// </summary>
      public string TableKey { get; set; }

      /// <summary>
      /// 外键字段.
      /// </summary>
      public string TableField { get; set; }

      /// <summary>
      /// 关联主表.
      /// </summary>
      public string RelationTable { get; set; }

      /// <summary>
      /// 关联主键.
      /// </summary>
      public string RelationField { get; set; }

      /// <summary>
      /// 功能名称.
      /// </summary>
      public string ClassName { get; set; }
   }

}
