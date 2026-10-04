
namespace EC226.Pagination
{
    using System;

    /// <summary>
    /// Type類的處理工具類
    /// </summary>
    public class TypeUtil
    {
        /// <summary>
        /// 獲取可空類型的實際類型
        /// </summary>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static Type GetUnNullableType(Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                //如果是泛型方法，且泛型類型為Nullable<>則視為可空類型
                //並使用NullableConverter轉換器進行轉換
                var nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return conversionType;
        }
    }
}