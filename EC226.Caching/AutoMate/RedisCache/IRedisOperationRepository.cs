using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace EC226.Caching.AutoMate.RedisCache
{
    /// <summary>
    /// Redis快取介面
    /// </summary>
    public interface IRedisOperationRepository
    {

        //取得 Reids 快取值
        Task<string> Get(string key);

        //取得值，並序列化
        Task<TEntity> Get<TEntity>(string key);

        //儲存
        Task Set(string key, object value, TimeSpan cacheTime);

        //判斷是否存在
        Task<bool> Exist(string key);

        //移除某一個快取值
        Task Remove(string key);

        //全部清除
        Task Clear();

        /// <summary>
        /// 根據key取得RedisValue
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task<RedisValue[]> ListRangeAsync(string redisKey);

        /// <summary>
        /// 在列表頭部插入值。如果鍵不存在，先建立再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        Task<long> ListLeftPushAsync(string redisKey, string redisValue);

        /// <summary>
        /// 在列表尾部插入值。如果鍵不存在，先建立再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        Task<long> ListRightPushAsync(string redisKey, string redisValue);

        /// <summary>
        /// 在列表尾部插入陣列集合。如果鍵不存在，先建立再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue);

        /// <summary>
        /// 移除並返回儲存在該鍵列表的第一個元素  反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task<T> ListLeftPopAsync<T>(string redisKey) where T : class;

        /// <summary>
        /// 移除並返回儲存在該鍵列表的最後一個元素   反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task<T> ListRightPopAsync<T>(string redisKey) where T : class;

        /// <summary>
        /// 移除並返回儲存在該鍵列表的第一個元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task<string> ListLeftPopAsync(string redisKey);

        /// <summary>
        /// 移除並返回儲存在該鍵列表的最後一個元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task<string> ListRightPopAsync(string redisKey);

        /// <summary>
        /// 列表長度
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task<long> ListLengthAsync(string redisKey);

        /// <summary>
        /// 返回在該列表上鍵所對應的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1);

        /// <summary>
        /// 根據索引取得指定位置數據
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop);

        /// <summary>
        /// 刪除List中的元素 並返回刪除的個數
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0);

        /// <summary>
        /// 清空List
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task ListClearAsync(string redisKey);


        /// <summary>
        /// 有序集合/定時任務延遲佇列用的多
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">元素</param>
        /// <param name="score">分數</param>
        Task SortedSetAddAsync(string redisKey, string redisValue, double score);

    }
}
