using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels
{
   public class SaveDataRequest
   {
      public string FunctionName { get; set; }
      public string FunctionKey { get; set; }
      public string Sort { get; set; }
      public int Database { get; set; }
      public int MainTable { get; set; }
      public List<DataSource> DataSource { get; set; }
   }

   public class DataSource
   {
      public int TableId { get; set; }
      public int ForeignKeyId { get; set; }
      public int RelatedKeyId { get; set; }
      public int RelatedType { get; set; }
   }
}
