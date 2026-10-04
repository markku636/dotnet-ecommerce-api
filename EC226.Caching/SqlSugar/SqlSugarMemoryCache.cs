using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SqlSugar;

namespace EC226.Caching.SqlSugar
{
    public class SqlSugarMemoryCache : ICacheService
    {
        MemoryCacheHelper cache = new MemoryCacheHelper();
        public void Add<V>(string key, V value)
        {
            cache.Set(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            cache.Set(key, value, cacheDurationInSeconds);
        }

        public bool ContainsKey<V>(string key)
        {
            return cache.Exists(key);
        }

        public V Get<V>(string key)
        {
            return cache.Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            return cache.GetCacheKeys();
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (cache.Exists(cacheKey))
            {
                return cache.Get<V>(cacheKey);
            }
            else
            {
                var result = create();
                cache.Set(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<V>(string key)
        {
            cache.Remove(key);
        }
    }
    public class MemoryCacheHelper
    {
        private static readonly Microsoft.Extensions.Caching.Memory.MemoryCache Cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 驗證快取項是否存在
        /// </summary>
        /// <param name="key">快取Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return Cache.TryGetValue(key, out _);
        }

        /// <summary>
        /// 新增快取
        /// </summary>
        /// <param name="key">快取Key</param>
        /// <param name="value">快取Value</param>
        /// <param name="expiresSliding">滑動過期時長（如果在過期時間內有操作，則以目前時間點延長過期時間）</param>
        /// <param name="expiressAbsoulte">絕對過期時長</param>
        /// <returns></returns>
        public bool Set(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Cache.Set(key, value,
                new MemoryCacheEntryOptions().SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte));
            return Exists(key);
        }

        /// <summary>
        /// 新增快取
        /// </summary>
        /// <param name="key">快取Key</param>
        /// <param name="value">快取Value</param>
        /// <param name="expiresIn">快取時長</param>
        /// <param name="isSliding">是否滑動過期（如果在過期時間內有操作，則以目前時間點延長過期時間）</param>
        /// <returns></returns>
        public bool Set(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Cache.Set(key, value,
                isSliding
                    ? new MemoryCacheEntryOptions().SetSlidingExpiration(expiresIn)
                    : new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiresIn));

            return Exists(key);
        }

        /// <summary>
        /// 新增快取
        /// </summary>
        /// <param name="key">快取Key</param>
        /// <param name="value">快取Value</param>
        /// <returns></returns>
        public void Set(string key, object value)
        {
            Set(key, value, TimeSpan.FromDays(1));
        }

        /// <summary>
        /// 新增快取
        /// </summary>
        /// <param name="key">快取Key</param>
        /// <param name="value">快取Value</param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public void Set(string key, object value, TimeSpan ts)
        {
            Set(key, value, ts, false);
        }

        /// <summary>
        /// 新增快取
        /// </summary>
        /// <param name="key">快取Key</param>
        /// <param name="value">快取Value</param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public void Set(string key, object value, int seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            Set(key, value, ts, false);
        }

        /// <summary>
        /// 刪除快取
        /// </summary>
        /// <param name="key">快取Key</param>
        /// <returns></returns>
        public void Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            Cache.Remove(key);
        }

        /// <summary>
        /// 批量刪除快取
        /// </summary>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            keys.ToList().ForEach(item => Cache.Remove(item));
        }


        #region 取得快取

        /// <summary>
        /// 取得快取
        /// </summary>
        /// <param name="key">快取Key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return Cache.Get<T>(key);
        }

        /// <summary>
        /// 取得快取
        /// </summary>
        /// <param name="key">快取Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return Cache.Get(key);
        }

        /// <summary>
        /// 取得快取集合
        /// </summary>
        /// <param name="keys">快取Key集合</param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            var dict = new Dictionary<string, object>();
            keys.ToList().ForEach(item => dict.Add(item, Cache.Get(item)));
            return dict;
        }
        #endregion

        /// <summary>
        /// 刪除所有快取
        /// </summary>
        public void RemoveCacheAll()
        {
            var l = GetCacheKeys();
            foreach (var s in l)
            {
                Remove(s);
            }
        }

        /// <summary>
        /// 刪除匹配到的快取
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public void RemoveCacheRegex(string pattern)
        {
            IList<string> l = SearchCacheRegex(pattern);
            foreach (var s in l)
            {
                Remove(s);
            }
        }

        /// <summary>
        /// 搜索 匹配到的快取
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IList<string> SearchCacheRegex(string pattern)
        {
            var cacheKeys = GetCacheKeys();
            var l = cacheKeys.Where(k => Regex.IsMatch(k, pattern)).ToList();
            return l.AsReadOnly();
        }

        /// <summary>
        /// 取得所有快取鍵
        /// </summary>
        /// <returns></returns>
        public List<string> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = Cache.GetType().GetField("_entries", flags).GetValue(Cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
    }
}
