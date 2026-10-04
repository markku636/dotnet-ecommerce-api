using Microsoft.Extensions.Caching.Memory;
using System;

namespace EC226.Caching.AutoMate.MemoryCache
{
    /// <summary>
    /// 實例化快取介面ICaching
    /// </summary>
    public class MemoryCaching : ICachingProvider
    {
        //引用Microsoft.Extensions.Caching.Memory;這個和.net 還是不一樣，沒有了Httpruntime了
        private readonly IMemoryCache _cache;
        //還是通過建構函式的方法，取得
        public MemoryCaching(IMemoryCache cache)
        {
            _cache = cache;
        }

        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue, int timeSpan)
        {
            _cache.Set(cacheKey, cacheValue, TimeSpan.FromSeconds(timeSpan * 60));
        }
    }

}
