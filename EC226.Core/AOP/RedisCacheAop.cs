
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using EC226.Caching.AutoMate.RedisCache;
using EC226.Core.Attribute;

namespace EC226.Core.AOP
{
    /// <summary>
    /// 面向切面的Redis快取使用
    /// </summary>
    public class RedisCacheAop : CacheAopBase
    {
        //通過注入的方式，把快取操作介面通過建構函式注入
        private readonly IRedisOperationRepository _cache;
        public RedisCacheAop(IRedisOperationRepository cache)
        {
            _cache = cache;
        }

        //Intercept方法是攔截的關鍵所在，也是IInterceptor介面中的唯一定義
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (method.ReturnType == typeof(void) || method.ReturnType == typeof(Task))
            {
                invocation.Proceed();
                return;
            }
            //對當前方法的特性驗證
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) is CachingAttribute qCachingAttribute)
            {
                //取得自定義快取鍵
                var cacheKey = CustomCacheKey(invocation);
                //注意是 string 型別，方法GetValue
                var cacheValue = _cache.Get(cacheKey).Result;
                if (cacheValue != null)
                {
                    //將目前取得到的快取值，賦值給目前執行方法
                    var returnType = typeof(Task).IsAssignableFrom(method.ReturnType) ? method.ReturnType.GenericTypeArguments.FirstOrDefault() : method.ReturnType;

                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(cacheValue, returnType);
                    invocation.ReturnValue = (typeof(Task).IsAssignableFrom(method.ReturnType)) ? Task.FromResult(result) : result;
                    return;
                }
                //去執行當前的方法
                invocation.Proceed();

                //存入快取
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    object response;

                    //Type type = invocation.ReturnValue?.GetType();
                    var type = invocation.Method.ReturnType;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        var resultProperty = type.GetProperty("Result");
                        response = resultProperty.GetValue(invocation.ReturnValue);
                    }
                    else
                    {
                        response = invocation.ReturnValue;
                    }
                    response ??= string.Empty;

                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(qCachingAttribute.AbsoluteExpiration)).Wait();
                }
            }
            else
            {
                invocation.Proceed();//直接執行被攔截方法
            }
        }

    }
}
