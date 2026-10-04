using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels.CMS
{
   public class TableModel: DbTableRelationModel
   {
      /// <summary>
      /// 控件key.
      /// </summary>
      public string ControlKey { get; set; }

      /// <summary>
      /// 列字段.
      /// </summary>
      public List<EntityFieldModel> Fields { get; set; }
   }

}
