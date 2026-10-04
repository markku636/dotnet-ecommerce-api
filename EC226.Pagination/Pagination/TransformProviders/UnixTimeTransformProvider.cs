

namespace EC226.Pagination
{
    using System;
    using System.Collections.Generic;
    

    public class UnixTimeTransformProvider : ITransformProvider
    {
        //Match«¬§O
        public bool Match(ConditionItem item, Type type)
        {
            if (item.Value == null)
                return false;
            
            var elementType = TypeUtil.GetUnNullableType(type);
            return ((elementType == typeof (int) && !(item.Value is int))
                    || (elementType == typeof (long) && !(item.Value is long))
                    || (elementType == typeof (DateTime) && !(item.Value is DateTime))
                   )
                   && item.Value.ToString().Contains("-");
        }

        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            DateTime willTime;
            if (DateTime.TryParse(item.Value.ToString(), out willTime))
            {
                var method = item.Method;
                
                if (method == QueryMethod.LessThan || method == QueryMethod.LessThanOrEqual)
                {
                    method = QueryMethod.DateTimeLessThanOrEqual;
                    if (willTime.Hour == 0 && willTime.Minute == 0 && willTime.Second == 0)
                    {
                        willTime = willTime.AddDays(1).AddMilliseconds(-1);
                    }
                }
                object value = null;
                if (type == typeof(DateTime))
                {
                    value = willTime;
                }
                else if (type == typeof(int))
                {
                    value = (int)UnixTime.FromDateTime(willTime);
                }else if (type == typeof(long))
                {
                    value = UnixTime.FromDateTime(willTime);
                }
                else
                {
                    value = willTime;
                }
                return new[] { new ConditionItem(item.Field, method, value) };
            }

            return new[] { new ConditionItem(item.Field, item.Method, Convert.ChangeType(item.Value,type)) };
        }
    }
}