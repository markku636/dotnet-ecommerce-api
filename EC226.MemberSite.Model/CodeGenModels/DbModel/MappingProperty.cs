using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.CodeGenModels 
{
    public class MappingProperty
    {
        public int DbId { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string TagId { get; set; }
    }
}
