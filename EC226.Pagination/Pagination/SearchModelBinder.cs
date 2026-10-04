using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EC226.Pagination
{
    public class SearchModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var model = (PageModel)(bindingContext.Model ?? new PageModel());
            var dict = bindingContext.ActionContext.HttpContext.Request.Query.ToDictionary(x => x.Key, y => y.Value);

            var keys = dict.Keys.Where(c => c.StartsWith("[") || c.Contains("PageModel_Order"));//我認為只有[開頭的為需要處理的
            if (keys.Count() != 0)
            {
                foreach (var key in keys)
                {
                    if (!key.StartsWith("[")) continue;
                    var val = dict[key].ToString(); // 未測試

                    //處理無值的情況
                    if (string.IsNullOrEmpty(val)) continue;
                    if (key == "[PageModel_Order]")//排序
                    {
                        var arr = val.Split('|');
                        if (arr.Length == 2)
                        {
                            var field = ToTitleCase(arr[0]);
                            var direction = ToTitleCase(arr[1]);

                            model.Order = new QueryOrder() { Field = field, Order = (OrderType)Enum.Parse(typeof(OrderType), direction) };
                        }
                        continue;
                    }
                    if (key == "[PageModel_PageIndex]")//頁碼
                    {
                        try
                        {
                            var pindex = int.Parse(val);
                            if (pindex > 1)
                                model.PageIndex = pindex;
                            else
                                model.PageIndex = 1;
                            //if (keys.Contains("[PageModel_PageTotal]"))
                            //{
                            //    var pt = int.Parse(dict["[PageModel_PageTotal]"]);
                            //    if (pindex > pt)
                            //    {
                            //        model.PageIndex = 1;
                            //    }
                            //}
                        }
                        catch
                        {
                            model.PageIndex = 1;
                        }
                        continue;
                    }
                    if (key == "[PageModel_PageTotal]")
                    {
                        continue;
                    }
                    if (key == "[PageModel_PageSize]")//每頁顯示數量
                    {
                        try
                        {
                            var pSize = int.Parse(val);
                            if (pSize > 0)
                                model.PageSize = pSize;
                            else
                                model.PageSize = 20;
                        }
                        catch
                        {
                            model.PageSize = 20;
                        }
                        continue;
                    }
                    if (key == "[PageModel_CheckSearch]")
                    {
                        if (val == "1")
                        {
                            //model.che = 1;
                        }
                        continue;
                    }
                    if (key == "[PageModel_SelectIds]")
                    {
                        if (val == "All")
                        {
                            model.SelectAll = true;
                        }
                        else
                        {
                            model.SelectAll = false;
                            model.SelectPrimaryKeys = val.Split('|').Where(m => m != "").ToList();
                        }
                        continue;
                    }
                    if (key == "[psome_name]")
                    {
                        continue;
                    }


                    AddSearchItem(model, key, val);
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }


        /// <summary>
        /// 將一組key=value添加入QueryModel.Items
        /// </summary>
        /// <param name="model">QueryModel</param>
        /// <param name="key">當前項的HtmlName</param>
        /// <param name="val">當前項的值</param>
        public static void AddSearchItem(PageModel model, string key, string val)
        {
            string field = "", prefix = "", orGroup = "", method = "";
            var keywords = key.Split(']', ')', '}');
            //將Html中的name分割為我想要的幾個部分
            foreach (var keyword in keywords)
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }

                if (Char.IsLetterOrDigit(keyword[0])) field = keyword;
                var last = keyword.Substring(1);
                if (keyword[0] == '(') prefix = last;
                if (keyword[0] == '[') method = last;
                if (keyword[0] == '{') orGroup = last;
            }
            if (string.IsNullOrEmpty(method)) return;
            if (!string.IsNullOrEmpty(field))
            {
                var fields = field.Split(',');
                foreach (var childFiles in fields)
                {
                    var item = new ConditionItem
                    {
                        //Name後面加入"_suffix"這個會被替代(解決命名重覆 ViewBag Binding不到的問題)，此為處理掉這些字串
                        Field = (childFiles.IndexOf("_suffix", StringComparison.Ordinal) > -1) ? childFiles.Substring(0,
                            childFiles.IndexOf("_suffix", StringComparison.Ordinal)) : childFiles,
                        Value = val.Trim(),
                        Prefix = prefix,
                        OrGroup = orGroup,
                        Method = (QueryMethod)Enum.Parse(typeof(QueryMethod), method)
                    };
                    if (val != "0")
                    {
                        model.QueryModel.Items.Add(item);
                    }
                }
            }
        }

        public class SearchModelBinderProvider : IModelBinderProvider
        {
            public IModelBinder GetBinder(ModelBinderProviderContext context)
            {
                if (context.Metadata.ModelType == typeof(PageModel))
                    return new SearchModelBinder();

                return null;
            }
        }

        private static string ToTitleCase(string transformString)
        {
            if (string.IsNullOrEmpty(transformString))
                return transformString;
            transformString = transformString.ToLower();
            return transformString.Substring(0, 1).ToUpper() + (transformString.Length > 1 ? transformString.Substring(1) : "");
        }


    }

}

