
using System.Collections.Generic;
using SqlSugar;

namespace EC226.MemberSite.Model.ViewModels.Basics
{
    /// <summary>
    ///     程式碼產生器下拉數據列表實體
    /// </summary>
    public class DbTableInfoTree
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public DbObjectType DbObjectType { get; set; }
    }

    /// <summary>
    ///     表名帶欄位
    /// </summary>
    public class DbTableInfoAndColumns
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DbColumnInfo> columns { get; set; } = null;
        public DbObjectType DbObjectType { get; set; }
    }
}
