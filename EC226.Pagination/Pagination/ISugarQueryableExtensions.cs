//-----------------------------------------------------------------------
// <copyright file="ISugarQueryableExtensions.cs">
// </copyright>
// <author>Zou Jian</author>
// <addtime>2010-09-03</addtime>
//-----------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EC226.Pagination
{
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using SqlSugar;


   /// <summary>
   /// 对ISugarQueryable的扩展方法
   /// </summary>
   public static class SugarQueryableExtensions
   {

      //PageModel
      public static ISugarQueryable<TEntity> WhereAndOrder<TEntity>(this ISugarQueryable<TEntity> table, PageModel model, string prefix = "") where TEntity : class
      {
         Contract.Requires(table != null);
         return Where<TEntity>(table, model.QueryModel).OrderBy(model.Order);

         //.Skip(pageSize * (page - 1)).Take(pageSize)
      }


      /// <summary>
      /// zoujian add , 使ISugarQueryable支持QueryModel
      /// </summary>
      /// <typeparam name="TEntity"></typeparam>
      /// <param name="table">ISugarQueryable的查询对象</param>
      /// <param name="model">QueryModel对象</param>
      /// <param name="prefix">使用前缀区分查询条件</param>
      /// <returns></returns>
      public static ISugarQueryable<TEntity> Where<TEntity>(this ISugarQueryable<TEntity> table, QueryModel model, string prefix = "") where TEntity : class
      {
         Contract.Requires(table != null);
         return Where<TEntity>(table, model.Items, prefix);
      }

      private static ISugarQueryable<TEntity> Where<TEntity>(ISugarQueryable<TEntity> table, IEnumerable<ConditionItem> items, string prefix = "")
      {
         Contract.Requires(table != null);
         IEnumerable<ConditionItem> filterItems =
             string.IsNullOrWhiteSpace(prefix)
                 ? items.Where(c => string.IsNullOrEmpty(c.Prefix))
                 : items.Where(c => c.Prefix == prefix);
         if (filterItems.Count() == 0) return table;
         return new SugarQueryableSearcher<TEntity>(table, filterItems).Search();
      }


      //排序For ISugarQueryable
      public static ISugarQueryable<TEntity> OrderBy<TEntity>(this ISugarQueryable<TEntity> table, QueryOrder order) where TEntity : class
      {
         if (order == null)
            return table;

         if (order.Order == OrderType.Asc)
         {
            return OrderBy<TEntity>(table, order.Field);
         }
         else
         {
            return OrderByDescending<TEntity>(table, order.Field);
         }
      }

      private static MethodInfo orderbyInfo = null;
      private static MethodInfo orderbyDecInfo = null;
      public static ISugarQueryable<TEntity> OrderBy<TEntity>(this ISugarQueryable<TEntity> query, string property) where TEntity : class
      {

         Type entityType = typeof(TEntity);
         Type entityPropertyType = entityType.GetProperty(property).PropertyType;

         var orderPara = Expression.Parameter(entityType, "o");
         var orderExpr = Expression.Lambda(Expression.Property(orderPara, property), orderPara);

         if (orderbyInfo == null)
         {
            //因為呼叫OrderBy需要知道型別，不知道的情況下無法直接呼叫，所以用反射的方式呼叫
            //泛型的GetMethod很難，所以用GetMethods在用Linq取出Method，找到後快取。
            orderbyInfo = typeof(Queryable).GetMethods().Single(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
         }

         //因為是泛型Mehtod要呼叫MakeGenericMethod決定泛型型別
         return orderbyInfo.MakeGenericMethod(new Type[] { entityType, entityPropertyType }).Invoke(null, new object[] { query, orderExpr }) as ISugarQueryable<TEntity>;
      }

      public static ISugarQueryable<TEntity> OrderByDescending<TEntity>(this ISugarQueryable<TEntity> query, string property)
      {
         Type entityType = typeof(TEntity);
         Type entityPropertyType = entityType.GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).PropertyType;

         var orderPara = Expression.Parameter(entityType, "o");
         var orderExpr = Expression.Lambda(Expression.Property(orderPara, property), orderPara);

         if (orderbyDecInfo == null)
         {
            orderbyDecInfo = typeof(Queryable).GetMethods().Single(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
         }

         return orderbyDecInfo.MakeGenericMethod(new Type[] { entityType, entityPropertyType }).Invoke(null, new object[] { query, orderExpr }) as ISugarQueryable<TEntity>;
      }

      public static ISugarQueryable<TEntity> SortBy<TEntity>(this ISugarQueryable<TEntity> query, string property, bool isSortByASC = true) where TEntity : class
      {
         try
         {
            if (string.IsNullOrWhiteSpace(property))
            {
               return query;
            }

            MethodInfo orderbyInfo = null;
            Type entityType = typeof(TEntity);
            Type propertyType = entityType.GetProperty(property)?.PropertyType;

            var param = Expression.Parameter(typeof(TEntity), "p");
            var keyParts = property.Split('.');

            string orderMethodName = isSortByASC ? "OrderBy" : "OrderByDescending";

            Expression parent = param;

            for (int i = 0; i < keyParts.Length; i++)
            {
               var keyPart = keyParts[i];
               parent = Expression.Property(parent, keyPart);

               if (keyParts.Length > 1)
               {
                  if (i == 0)
                  {
                     propertyType = entityType.GetProperty(keyPart).PropertyType;
                  }
                  else
                  {
                     propertyType = propertyType.GetProperty(keyPart).PropertyType;
                  }
               }
            }

            var orderExpr = Expression.Lambda(parent, param);

            if (orderbyInfo == null)
            {
               //因為呼叫OrderBy需要知道型別，不知道的情況下無法直接呼叫，所以用反射的方式呼叫
               //泛型的GetMethod很難，所以用GetMethods在用Linq取出Method，找到後快取。
               orderbyInfo = typeof(Queryable).GetMethods().Single(x => x.Name == orderMethodName && x.GetParameters().Length == 2);
            }

            //因為是泛型Mehtod要呼叫MakeGenericMethod決定泛型型別
            return orderbyInfo.MakeGenericMethod(new Type[] { entityType, propertyType }).Invoke(null, new object[] { query, orderExpr }) as ISugarQueryable<TEntity>;

         }
         catch (Exception ex)
         {
            return query;
         }
      }



   }
}
