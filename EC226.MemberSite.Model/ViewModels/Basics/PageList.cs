using System;
using System.Collections.Generic;
using System.Linq;

namespace EC226.MemberSite.Model.ViewModels.Basics
{
    /// <summary>
    ///     分頁元件實體類
    /// </summary>
    /// <typeparam name="T">泛型實體</typeparam>
    [Serializable]
    public class PageList<T> : List<T>, IPageList<T>
    {
		public PageList() {
		}

		/// <summary>
		///     建構函式
		/// </summary>
		/// <param name="source">數據源</param>
		/// <param name="pageIndex">分頁索引</param>
		/// <param name="pageSize">分頁大小</param>
		public PageList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var total = source.Count();
            TotalCount = total;
            TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;

            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        ///     建構函式
        /// </summary>
        /// <param name="source">數據源</param>
        /// <param name="pageIndex">分頁索引</param>
        /// <param name="pageSize">分頁大小</param>
        public PageList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        ///     建構函式
        /// </summary>
        /// <param name="source">數據源</param>
        /// <param name="pageIndex">分頁索引</param>
        /// <param name="pageSize">分頁大小</param>
        /// <param name="totalCount">總記錄數</param>
        public PageList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source);
        }

        /// <summary>
        ///     分頁索引
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        ///     分頁大小
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        ///     總記錄數
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        ///     總頁數
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        ///     是否有上一頁
        /// </summary>
        public bool HasPreviousPage => PageIndex > 0;

        /// <summary>
        ///     是否有下一頁
        /// </summary>
        public bool HasNextPage => PageIndex + 1 < TotalPages;
    }
}
