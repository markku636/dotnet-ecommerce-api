using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EC226.Caching.Manual;
using EC226.Configuration;
using EC226.Utility.Extensions;
using StackExchange.Redis;

namespace EC226.Caching.Redis
{
   public class RedisCacheManager: IManualCacheManager
   {
      private readonly string _redisConnectionString;

      private volatile ConnectionMultiplexer _redisConnection;
      private readonly object _redisConnectionLock = new object();

      public RedisCacheManager()
      {
         string redisConfiguration = AppSettingsConstVars.RedisConfigConnectionString;
         if (string.IsNullOrWhiteSpace(redisConfiguration))
         {
            throw new ArgumentException("Redis config is empty", nameof(redisConfiguration));
         }
         _redisConnectionString = redisConfiguration;
         _redisConnection = GetRedisConnection().Result; // Ensure connection is established at construction.
      }

      private Task<ConnectionMultiplexer> GetRedisConnection()
      {

         if (_redisConnection != null && _redisConnection.IsConnected)
         {
            return Task.FromResult(_redisConnection);
         }

         lock (_redisConnectionLock)
         {
            _redisConnection?.Dispose();
            try
            {
               _redisConnection = ConnectionMultiplexer.Connect(_redisConnectionString);
            }
            catch (Exception)
            {
               throw new Exception("Failed to connect to Redis. Please ensure the Redis service is running and accessible.");
            }
         }
                  
         return Task.FromResult(_redisConnection);
      }

      public async Task<bool> ExistsAsync(string key)
      {
         return await _redisConnection.GetDatabase().KeyExistsAsync(key);
      }

      public async Task<bool> SetAsync(string key, object value, int expiresIn = 0)
      {
         if (value != null)
         {
            if (expiresIn > 0)
            {
               return await _redisConnection.GetDatabase().StringSetAsync(key, SerializeExtensions.Serialize(value), TimeSpan.FromMinutes(expiresIn));
            }
            else
            {
               return await _redisConnection.GetDatabase().StringSetAsync(key, SerializeExtensions.Serialize(value));
            }
         }
         return false;
      }

      public async Task RemoveAsync(string key)
      {
         await _redisConnection.GetDatabase().KeyDeleteAsync(key);
      }

      public async Task RemoveAllAsync(IEnumerable<string> keys)
      {
         var redisKeys = keys.Select(k => (RedisKey)k).ToArray();
         await _redisConnection.GetDatabase().KeyDeleteAsync(redisKeys);
      }

      public async Task<T> GetAsync<T>(string key)
      {
         var value = await _redisConnection.GetDatabase().StringGetAsync(key);
         if (value.HasValue)
         {
            return SerializeExtensions.Deserialize<T>(value);
         }
         return default;
      }

      public async Task<object> GetAsync(string key)
      {
         var value = await _redisConnection.GetDatabase().StringGetAsync(key);
         return value;
      }

      public async Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys)
      {
         if (keys == null)
            throw new ArgumentNullException(nameof(keys));

         var dict = new Dictionary<string, object>();
         foreach (var key in keys)
         {
            var value = await _redisConnection.GetDatabase().StringGetAsync(key);
            dict.Add(key, value);
         }
         return dict;
      }

      public async Task RemoveCacheAllAsync()
      {
         var endpoints = _redisConnection.GetEndPoints();
         foreach (var endPoint in endpoints)
         {
            var server = _redisConnection.GetServer(endPoint);
            var keys = server.Keys().ToArray();
            await _redisConnection.GetDatabase().KeyDeleteAsync(keys);
         }
      }

      public async Task<IList<string>> SearchCacheRegexAsync(string pattern)
      {
         var list = new List<string>();
         var script = "return redis.call('keys', @pattern)";
         var prepared = LuaScript.Prepare(script);
         var redisResult = await _redisConnection.GetDatabase().ScriptEvaluateAsync(prepared, new { pattern });
         if (!redisResult.IsNull)
         {
            foreach (var key in (RedisKey[])redisResult)
            {
               list.Add(await _redisConnection.GetDatabase().StringGetAsync(key));
            }
         }
         return list;
      }

      public async Task RemoveCacheRegexAsync(string pattern)
      {
         var script = "return redis.call('keys', @pattern)";
         var prepared = LuaScript.Prepare(script);

         // Asynchronously evaluate the script and wait for the result
         var redisResult = await _redisConnection.GetDatabase().ScriptEvaluateAsync(prepared, new { pattern });

         // Check if the result is not null and then proceed to delete the keys
         if (!redisResult.IsNull)
         {
            // Asynchronously delete the keys and wait for the operation to complete
            await _redisConnection.GetDatabase().KeyDeleteAsync((RedisKey[])redisResult);
         }
      }
   }
}
