using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EC226.Caching.AutoMate.RedisCache
{
    public class RedisOperationRepository : IRedisOperationRepository
    {
        private readonly ILogger<RedisOperationRepository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisOperationRepository(ILogger<RedisOperationRepository> logger, ConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
            _database = redis.GetDatabase();
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

        public async Task Clear()
        {
            foreach (var endPoint in _redis.GetEndPoints())
            {
                var server = GetServer();
                foreach (var key in server.Keys())
                {
                    await _database.KeyDeleteAsync(key);
                }
            }
        }

        public async Task<bool> Exist(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task<string> Get(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task Remove(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，將object值產生RedisValue
                await _database.StringSetAsync(key, JsonConvert.SerializeObject(value), cacheTime);
            }
        }

        public async Task<TEntity> Get<TEntity>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.HasValue)
            {
                //需要用的反序列化，將Redis儲存的Byte[]，進行反序列化
                return JsonConvert.DeserializeObject<TEntity>(value);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 根據key取得RedisValue
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<RedisValue[]> ListRangeAsync(string redisKey)
        {
            return await _database.ListRangeAsync(redisKey);
        }

        /// <summary>
        /// 在列表頭部插入值。如果鍵不存在，先建立再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync(string redisKey, string redisValue)
        {
            return await _database.ListLeftPushAsync(redisKey, redisValue);
        }
        /// <summary>
        /// 在列表尾部插入值。如果鍵不存在，先建立再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string redisKey, string redisValue)
        {
            return await _database.ListRightPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入陣列集合。如果鍵不存在，先建立再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue)
        {
            var redislist = new List<RedisValue>();
            foreach (var item in redisValue)
            {
                redislist.Add(item);
            }
            return await _database.ListRightPushAsync(redisKey, redislist.ToArray());
        }


        /// <summary>
        /// 移除並返回儲存在該鍵列表的第一個元素  反序列化
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string redisKey) where T : class
        {
            var cacheValue = await _database.ListLeftPopAsync(redisKey);
            if (string.IsNullOrEmpty(cacheValue)) return null;
            var res = JsonConvert.DeserializeObject<T>(cacheValue);
            return res;
        }

        /// <summary>
        /// 移除並返回儲存在該鍵列表的最後一個元素反序列化
        /// 只能是對像集合
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string redisKey) where T : class
        {
            var cacheValue = await _database.ListRightPopAsync(redisKey);
            if (string.IsNullOrEmpty(cacheValue)) return null;
            var res = JsonConvert.DeserializeObject<T>(cacheValue);
            return res;
        }

        /// <summary>
        /// 移除並返回儲存在該鍵列表的第一個元素   
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListLeftPopAsync(string redisKey)
        {
            return await _database.ListLeftPopAsync(redisKey);
        }

        /// <summary>
        /// 移除並返回儲存在該鍵列表的最後一個元素   
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListRightPopAsync(string redisKey)
        {
            return await _database.ListRightPopAsync(redisKey);
        }

        /// <summary>
        /// 列表長度
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string redisKey)
        {
            return await _database.ListLengthAsync(redisKey);
        }

        /// <summary>
        /// 返回在該列表上鍵所對應的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1)
        {
            var result = await _database.ListRangeAsync(redisKey);
            return result.Select(o => o.ToString());
        }

        /// <summary>
        /// 根據索引取得指定位置數據
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop)
        {
            var result = await _database.ListRangeAsync(redisKey, start, stop);
            return result.Select(o => o.ToString());
        }

        /// <summary>
        /// 刪除List中的元素 並返回刪除的個數
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">元素</param>
        /// <param name="type">大於零 : 從表頭開始向表尾搜索，小於零 : 從表尾開始向表頭搜索，等於零：移除表中所有與 VALUE 相等的值</param>
        /// <returns></returns>
        public async Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0)
        {
            return await _database.ListRemoveAsync(redisKey, redisValue, type);
        }

        /// <summary>
        /// 清空List
        /// </summary>
        /// <param name="redisKey"></param>
        public async Task ListClearAsync(string redisKey)
        {
            await _database.ListTrimAsync(redisKey, 1, 0);
        }


        /// <summary>
        /// 有序集合/定時任務延遲佇列用的多
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">元素</param>
        /// <param name="score">分數</param>
        public async Task SortedSetAddAsync(string redisKey, string redisValue, double score)
        {
            await _database.SortedSetAddAsync(redisKey, redisValue, score);
        }

    }
}
