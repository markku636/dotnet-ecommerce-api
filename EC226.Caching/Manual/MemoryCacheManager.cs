using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EC226.Caching.Manual;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace EC226.Caching.MemoryCache
{
   /// <summary>
   /// Memory cache manager for handling caching operations asynchronously.
   /// </summary>
   public class MemoryCacheManager: IManualCacheManager
   {
      private static volatile Microsoft.Extensions.Caching.Memory.MemoryCache _memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache((IOptions<MemoryCacheOptions>)new MemoryCacheOptions());

      public async Task<bool> ExistsAsync(string key)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key));
         bool exists = _memoryCache.TryGetValue(key, out _);
         return await Task.FromResult(exists);
      }

      public async Task<bool> SetAsync(string key, object value, int expiresIn = 0)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key));
         if (value == null)
            throw new ArgumentNullException(nameof(value));

         var options = new MemoryCacheEntryOptions()
             .SetSlidingExpiration(TimeSpan.FromSeconds(120));
         if (expiresIn > 0)
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(expiresIn));
         else
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(1440));

         _memoryCache.Set(key, value, options);
         return await ExistsAsync(key);
      }



      public async Task RemoveAsync(string key)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key));
         _memoryCache.Remove(key);
         await Task.CompletedTask;
      }

      public async Task RemoveAllAsync(IEnumerable<string> keys)
      {
         if (keys == null)
            throw new ArgumentNullException(nameof(keys));

         foreach (var key in keys)
            _memoryCache.Remove(key);
         await Task.CompletedTask;
      }

      public async Task<T> GetAsync<T>(string key)
      {
         if (key == null)
            throw new ArgumentNullException(nameof(key));
         T value = _memoryCache.Get<T>(key);
         return await Task.FromResult(value);
      }

      public async Task<object> GetAsync(string key)
      {
         if (key is null)
            throw new ArgumentNullException(nameof(key));
         var value = _memoryCache.Get(key);
         return await Task.FromResult(value);
      }

      public async Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys)
      {
         if (keys == null)
            throw new ArgumentNullException(nameof(keys));

         var dict = new Dictionary<string, object>();
         foreach (var key in keys)
            dict[key] = _memoryCache.Get(key);
         return await Task.FromResult(dict as IDictionary<string, object>);
      }

      public async Task RemoveCacheAllAsync()
      {
         var keys = GetCacheKeys();
         foreach (var key in keys)
            _memoryCache.Remove(key);
         await Task.CompletedTask;
      }

      public async Task RemoveCacheRegexAsync(string pattern)
      {
         var keys = await SearchCacheRegexAsync(pattern);
         foreach (var key in keys)
            _memoryCache.Remove(key);
         await Task.CompletedTask;
      }

      public async Task<IList<string>> SearchCacheRegexAsync(string pattern)
      {
         var keys = GetCacheKeys();
         var matchedKeys = keys.Where(key => Regex.IsMatch(key, pattern)).ToList();
         return await Task.FromResult(matchedKeys as IList<string>);
      }

      /// <summary>
      /// Retrieves all cache keys.
      /// </summary>
      /// <returns>A list of cache keys.</returns>
      public List<string> GetCacheKeys()
      {
         const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
         var entriesField = _memoryCache.GetType().GetField("_entries", flags);
         if (entriesField == null)
            return new List<string>();

         var entries = entriesField.GetValue(_memoryCache) as IDictionary;
         var keys = new List<string>();
         if (entries != null)
            foreach (DictionaryEntry entry in entries)
               keys.Add(entry.Key.ToString());
         return keys;
      }
   }
}
