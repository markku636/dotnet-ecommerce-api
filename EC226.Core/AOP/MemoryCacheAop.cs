
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using EC226.Caching.AutoMate.MemoryCache;
using EC226.Core.Attribute;

namespace EC226.Core.AOP
{
    /// <summary>
    /// 面向切換的記憶體快取使用
    /// </summary>
    public class MemoryCacheAop : CacheAopBase
    {
        //通過注入的方式，把快取操作介面通過建構函式注入
        private readonly ICachingProvider _cache;
        public MemoryCacheAop(ICachingProvider cache)
        {
            _cache = cache;
        }

        //Intercept方法是攔截的關鍵所在，也是IInterceptor介面中的唯一定義
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //對當前方法的特性驗證
            //如果需要驗證
            var CachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute));
            if (CachingAttribute is CachingAttribute qCachingAttribute)
            {
                //取得自定義快取鍵
                var cacheKey = CustomCacheKey(invocation);
                //根據key取得相應的快取值
                var cacheValue = _cache.Get(cacheKey);
                if (cacheValue != null)
                {
                    //將目前取得到的快取值，賦值給目前執行方法
                    invocation.ReturnValue = cacheValue;
                    return;
                }
                //去執行當前的方法
                invocation.Proceed();
                //存入快取
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _cache.Set(cacheKey, invocation.ReturnValue, qCachingAttribute.AbsoluteExpiration);
                }
            }
            else
            {
                invocation.Proceed();//直接執行被攔截方法
            }
        }

    }
}
