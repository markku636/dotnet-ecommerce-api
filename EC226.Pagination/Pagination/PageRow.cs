using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC226.Pagination
{
    /// <summary>
    /// 行
    /// </summary>
    public class PageRow
    {
        public PageRow()
        {
            RowData = new List<RowItem>();
        }
        // <summary>
        /// 行数据
        /// </summary>
        public IList<RowItem> RowData { get; set; }
        /// <summary>
        /// 单行数据对象的主键字段名
        /// </summary>
        public string SourcePrimaryKey { get; set; }

        /// <summary>
        /// 父項
        /// </summary>
        public string SourceParentKey { get; set; }

        /// <summary>
        /// 行样式
        /// </summary>
        public string Style { get; set; }

        public List<PageRow> ChildData { get; set; } = new List<PageRow>();
    }
    /// <summary>
    /// 行中单列数据
    /// </summary>
    public class RowItem
    {
        /// <summary>
        /// 对应列编号
        /// </summary>
        public string ColumId { get; set; }
        /// <summary>
        /// 是否是字段列
        /// </summary>
        public bool CheckFieldColum { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
    }
}
