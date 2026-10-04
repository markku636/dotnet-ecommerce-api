namespace EC226.Pagination
{
    using System.Collections.Generic;

    /// <summary>
    /// 用戶自動收集搜索條件的Model
    /// </summary>
    public class QueryModel
    {
        public QueryModel()
        {
            Items = new List<ConditionItem>();
        }
        /// <summary>
        /// 查詢條件
        /// </summary>
        public List<ConditionItem> Items { get; set; }

    }
}