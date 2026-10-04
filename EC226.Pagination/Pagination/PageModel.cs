using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace EC226.Pagination
{
    /// <summary>
    /// 分頁模型
    /// </summary>
    public class PageModel
    {
        public PageModel()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
            QueryModel = new QueryModel();
            Id = Guid.NewGuid().ToString().Replace(@"/", "").Replace(@"\", "").Replace("-", "_");
        }

        /// <summary>
        /// 頁的唯一標識
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 頁碼從1開始
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每頁顯示數量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 頁總數
        /// </summary>
        public long PageTotal
        {
            get
            {
                #region
                if (DataTotal == 0)
                {
                    return 0;
                }
                else if (DataTotal <= PageSize)
                {
                    return 1;
                }
                else
                {
                    var z = DataTotal / PageSize;
                    var y = DataTotal % PageSize;
                    if (y != 0)
                    {
                        return z + 1;
                    }
                    else
                    {
                        return z;
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// 數據總數
        /// </summary>
        public long DataTotal { get; set; }
        /// <summary>
        /// 查詢準則
        /// </summary>
        public QueryModel QueryModel { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public QueryOrder Order { get; set; }
        /// <summary>
        /// 是否全選
        /// </summary>
        public bool SelectAll { get; set; }
        /// <summary>
        /// 選擇的查詢集合
        /// </summary>
        public IList<string> SelectPrimaryKeys { get; set; }



        ///// <summary>
        ///// 判斷查詢欄位是存在
        ///// </summary>
        ///// <param name="expr"></param>
        ///// <returns></returns>
        public bool CheckSearchName<T>(Expression<Func<T, object>> expr)
        {
            var field = expr.Body.ToString().Split('.').Last();
            if (QueryModel.Items.Any(m => m.Field == field))
                return true;
            else
                return false;
        }
        ///// <summary>
        ///// 查詢欄位值
        ///// </summary>
        ///// <param name="expr"></param>
        ///// <returns></returns>
        public object SearchValue<T>(Expression<Func<T, object>> expr)
        {
            var field = expr.Body.ToString().Split('.').Last();
            var data = this.QueryModel.Items.Where(m => m.Field == field);
            if (data.Count() > 0)
            {
                return data.First().Value;
            }
            return "";
        }
        /// <summary>
        /// 設置當前分頁物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="dataTotal"></param>
        /// <returns></returns>
        public PageModel<T> Record<T>(IEnumerable<T> data, long dataTotal = -1) where T : class
        {
            var model = new PageModel<T>(data, dataTotal);
            model.DataTotal = dataTotal;
            model.PageIndex = this.PageIndex;
            model.PageSize = this.PageSize;
            model.Order = this.Order;
            model.QueryModel = this.QueryModel;

            return model;
        }

    }
    /// <summary>
    /// 分頁模型
    /// </summary>
    public class PageModel<T> : PageModel
    {
        public PageModel(IEnumerable<T> data, Int64 dataTotal = -1)
            : base()
        {
            #region 設置數據總數
            if (data != null)
                DataSource = data;
            else
                DataSource = new List<T>();
            if (dataTotal == -1)
            {
                if (data != null)
                {
                    DataTotal = data.Count();
                }
                else
                {
                    DataTotal = 0;
                }
            }
            else
            {
                DataTotal = dataTotal;
            }
            #endregion
            this.TableClassName = "table table-bordered table-hover";
            Colums = new List<PageColum>();

        }

        public PageModel()
        {

        }

        public virtual List<PageRow> PageRows { get; set; }

        /// <summary>
        /// 資料來源
        /// </summary>
        public virtual IEnumerable<T> DataSource { get; set; }
        /// <summary>
        /// 數據列
        /// </summary>
        public virtual IList<PageColum> Colums { get; set; }
        /// <summary>
        /// 數據的主鍵欄位名
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 是否顯示選擇框
        /// </summary>
        public bool ShowCheckBox { get; set; }//顯示選擇框
        /// <summary>
        /// 列樣式類名
        /// </summary>
        public string TableClassName { get; set; }

        /// <summary>
        /// 是否為樹狀表格
        /// </summary>
        public bool IsTreeGrid { get; set; }

        /// <summary>
        /// 父鍵
        /// </summary>
        public string ParentKey { get; set; }

        /// <summary>
        /// 是否顯示選擇框
        /// </summary>
        /// <param name="show"></param>
        /// <returns></returns>
        public PageModel<T> SetShowCheckBox(bool show = false)
        {
            this.ShowCheckBox = show;
            return this;
        }

        /// <summary>
        /// 設定是否為表格樹
        /// </summary>
        /// <param name="isTreeGrid"></param>
        /// <returns></returns>
        public PageModel<T> ToTreeGrid()
        {
            this.IsTreeGrid = true;
            this.CreateRows();
            return this;
        }

        /// <summary>
        /// 查詢欄位
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public string SearchName(Expression<Func<T, object>> expr)
        {
            return expr.Body.ToString().Split('.').Last();
        }
        ///// <summary>
        ///// 查詢欄位值
        ///// </summary>
        ///// <param name="expr"></param>
        ///// <returns></returns>
        public object SearchValue(Expression<Func<T, object>> expr)
        {
            var data = this.QueryModel.Items.Where(m => m.Field == SearchName(expr));
            if (data.Count() > 0)
            {
                return data.First().Value;
            }
            return "";
        }
        /// <summary>
        /// 設置資料來源的每一行的主鍵欄位
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public PageModel<T> SetPrimaryKey(Expression<Func<T, object>> expr, bool firstCharToLower = true)
        {
            try
            {
                var primaryKey = expr.Body.ToString().Split('.').Last().Replace(")", "").Split(',')[0];
                if (!string.IsNullOrWhiteSpace(primaryKey) && firstCharToLower)
                {
                    primaryKey = primaryKey.Substring(0, 1).ToLower() + primaryKey.Substring(1);
                }


                this.PrimaryKey = primaryKey;

            }
            catch (Exception e)
            {
            }

            return this;
        }

        /// <summary>
        /// 設置父鍵
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public PageModel<T> SetParentKey(Expression<Func<T, object>> expr, bool firstCharToLower = true)
        {
            var parentKey = expr.Body.ToString().Split('.').Last().Replace(")", "");

            if (!string.IsNullOrWhiteSpace(parentKey) && firstCharToLower)
            {
                parentKey = parentKey.Substring(0, 1).ToLower() + parentKey.Substring(1);
            }

            this.ParentKey = parentKey;

            return this;
        }

        /// <summary>
        /// 設置表的樣式類
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public PageModel<T> SetClass(string className)
        {
            this.TableClassName = className;

            return this;
        }

        private void CreateRows()
        {
            var rows = new List<PageRow>();
            //ModelMetadataProvider provider = ModelMetadataProviders.Current;
            //ModelMetadata containerMetadata = new ModelMetadata(provider, null, () => null, typeof(T), null);
            //var properties = containerMetadata.Properties.Select(m => m.PropertyName);
            if (this.DataSource != null && this.DataSource.Any() && this.Colums != null && this.Colums.Count > 0)
            {
                foreach (var data in this.DataSource)
                {
                    var row = new PageRow();
                    if (!string.IsNullOrEmpty(this.PrimaryKey))
                    {
                        var keyValue = data.GetType().GetProperty(this.PrimaryKey, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(data, null);
                        row.SourcePrimaryKey = keyValue == null ? Guid.NewGuid().ToString() : keyValue.ToString();
                    }
                    else
                    {
                        row.SourcePrimaryKey = Guid.NewGuid().ToString();
                    }

                    if (!string.IsNullOrEmpty(this.ParentKey))
                    {
                        var keyValue = data.GetType().GetProperty(this.ParentKey, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(data, null);
                        row.SourceParentKey = keyValue == null ? string.Empty : keyValue.ToString();
                    }
                    //else
                    //{
                    //    row.SourceParentKey = Guid.NewGuid().ToString();
                    //}

                    foreach (var colum in this.Colums)
                    {
                        if (!string.IsNullOrEmpty(colum.Field))
                        {
                            var colValue = data.GetType().GetProperty(colum.Field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(data, null);
                            row.RowData.Add(new RowItem() { ColumId = colum.Id, Value = colValue, CheckFieldColum = true });

                        }
                        else
                        {
                            var html = data.GetType().GetProperty(colum.Field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(data, null);
                            row.RowData.Add(new RowItem() { ColumId = colum.Id, Value = HttpUtility.HtmlEncode(html), CheckFieldColum = false });
                        }
                    }
                    rows.Add(row);
                }
            }

            List<PageRow> treeView = new List<PageRow>();
            var rootNodes = rows.Where(x => string.IsNullOrWhiteSpace(x.SourceParentKey)).ToList();
            var node = rows.Where(x => !string.IsNullOrWhiteSpace(x.SourceParentKey)).ToList();

            if (node.Count > 0)
            {
                foreach (var rootNode in rootNodes)
                {
                    var newTreeView = rootNode;
                    treeView.Add(rootNode);
                    GenerateTree(node, rootNode.SourcePrimaryKey, ref newTreeView);
                }
            }

            this.PageRows = treeView;
        }

        public PageModel<T> ColumFor(Expression<Func<T, object>> expr, string header = "", int width = 200 , bool isHidden = false, bool sort = false, string style = "", bool firstCharToLower = true)
        {
            var exprBody = expr.Body.ToString();

            if (exprBody.Contains("Convert"))
            {
                exprBody = exprBody.Replace("Convert", "").Replace("(", "").Replace(")", "");
            }
            var field = exprBody.Split('.').Last().Split(',').First();//ExpressionHelper.GetExpressionText(expr);
            var test = TypeDescriptor.GetProperties(typeof(T))[field];
            if (string.IsNullOrEmpty(header))
            {
                try
                {
                    foreach (var attr in TypeDescriptor.GetProperties(typeof(T))[field].Attributes)
                    {
                        if (attr is System.ComponentModel.DisplayNameAttribute)
                        {
                            string displayNmae = (attr as System.ComponentModel.DisplayNameAttribute).DisplayName;

                            header = !string.IsNullOrWhiteSpace(displayNmae) ? displayNmae : field;
                            break;
                        }
                    }

                    if (firstCharToLower)
                    {
                        field = field.Substring(0, 1).ToLower() + field.Substring(1);
                    }
                }
                catch//處理比如datetime類型的屬性
                {
                }

                if(string.IsNullOrWhiteSpace(header))
                {
                    header = field;
                }

                this.Colums.Add(new PageColum()
                {
                    Field = field,
                    IsHidden = isHidden,
                    Width = width,
                    Header = header,
                    Style = style,
                    Sort = sort
                });
            }

            return this;
        }

        public void GenerateTree(List<PageRow> nodes, string parentKey, ref PageRow newTreeView)
        {
            if (nodes.Any(x => x.SourceParentKey == parentKey))
            {

                foreach (var item in nodes.Where(x => x.SourceParentKey == parentKey))
                {
                    newTreeView.ChildData.Add(item);

                    var treeView = item;

                    GenerateTree(nodes, item.SourcePrimaryKey, ref treeView);
                }
            }
        }

        /// <summary>
        /// 獲得一個數左右多少個的起始數
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="currentNumber">當前數</param>
        /// <param name="maxNumber">最大數</param>
        /// <param name="showCount">一共多少個</param>
        /// <returns></returns>
        private Int64 GetStartNumber(Int64 currentNumber, Int64 maxNumber, int showCount)
        {
            var arr = new List<Int64>();
            var pAvg = showCount / 2;//左右顯示個數
            for (int i = 1; i <= pAvg; i++)
            {
                if (currentNumber - i > 0)
                {
                    arr.Add(currentNumber - i);
                }
                if (currentNumber + i <= maxNumber)
                {
                    arr.Add(currentNumber + i);
                }
            }
            arr.Add(currentNumber);
            if (arr.Count < 10)
            {
                var le = showCount - arr.Count;
                var min = arr.Min();
                var max = arr.Max();
                for (int i = 1; i <= le; i++)
                {
                    if (min == 1 && max + 1 <= maxNumber)
                    {
                        arr.Add(max + 1);
                    }
                    if (max == maxNumber && min - 1 >= 1)
                    {
                        arr.Add(min - 1);
                    }
                }
            }
            return arr.Min();
        }
    }
}

