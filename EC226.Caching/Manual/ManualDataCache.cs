using EC226.Caching.MemoryCache;
using EC226.Caching.Redis;
using EC226.Configuration;
using EC226.Utility.Extensions;

namespace EC226.Caching.Manual
{
    /// <summary>
    /// 手動快取呼叫
    /// </summary>
    public static partial class ManualDataCache
    {
        private static IManualCacheManager _instance = null;
        /// <summary>
        /// 靜態實例，外部可直接呼叫
        /// </summary>
        public static IManualCacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (AppSettingsConstVars.RedisUseCache)
                    {
                        _instance = new RedisCacheManager();
                    }
                    else
                    {
                        _instance = new MemoryCacheManager();

                    }
                }
                return _instance;
            }
        }
    }
}
