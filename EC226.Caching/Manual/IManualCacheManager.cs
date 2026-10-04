using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EC226.Caching.Manual
{
   /// <summary>
   /// Interface for manual cache management with asynchronous operations.
   /// </summary>
   public interface IManualCacheManager
   {
      /// <summary>
      /// Verifies if a cache item exists asynchronously.
      /// </summary>
      /// <param name="key">Cache key</param>
      /// <returns>A task that represents the asynchronous operation and contains the result.</returns>
      Task<bool> ExistsAsync(string key);

      /// <summary>
      /// Adds an item to the cache asynchronously.
      /// </summary>
      /// <param name="key">Cache key</param>
      /// <param name="value">Cache value</param>
      /// <param name="expiresIn">Expiration time in minutes</param>
      /// <returns>A task that represents the asynchronous operation and contains the result.</returns>
      Task<bool> SetAsync(string key, object value, int expiresIn = 0);

      /// <summary>
      /// Removes a cache item asynchronously.
      /// </summary>
      /// <param name="key">Cache key</param>
      Task RemoveAsync(string key);

      /// <summary>
      /// Removes multiple cache items asynchronously.
      /// </summary>
      /// <param name="keys">Collection of cache keys</param>
      Task RemoveAllAsync(IEnumerable<string> keys);

      /// <summary>
      /// Retrieves a cache item asynchronously.
      /// </summary>
      /// <param name="key">Cache key</param>
      /// <returns>A task that represents the asynchronous operation and contains the cache item.</returns>
      Task<T> GetAsync<T>(string key);

      /// <summary>
      /// Retrieves a cache item asynchronously.
      /// </summary>
      /// <param name="key">Cache key</param>
      /// <returns>A task that represents the asynchronous operation and contains the cache item.</returns>
      Task<object> GetAsync(string key);

      /// <summary>
      /// Retrieves a collection of cache items asynchronously.
      /// </summary>
      /// <param name="keys">Collection of cache keys</param>
      /// <returns>A task that represents the asynchronous operation and contains the collection of cache items.</returns>
      Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys);

      /// <summary>
      /// Removes all cache items asynchronously.
      /// </summary>
      Task RemoveCacheAllAsync();

      /// <summary>
      /// Removes cache items matching a pattern asynchronously.
      /// </summary>
      /// <param name="pattern">Pattern to match cache keys</param>
      Task RemoveCacheRegexAsync(string pattern);

      /// <summary>
      /// Searches for cache items matching a pattern asynchronously.
      /// </summary>
      /// <param name="pattern">Pattern to match cache keys</param>
      /// <returns>A task that represents the asynchronous operation and contains a list of matching keys.</returns>
      Task<IList<string>> SearchCacheRegexAsync(string pattern);
   }
}
