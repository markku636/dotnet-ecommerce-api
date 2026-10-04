namespace EC226.Caching.AutoMate.MemoryCache
{
    /// <summary>
    /// 簡單的快取介面，只有查詢和新增，以後會進行擴充套件
    /// </summary>
    public interface ICachingProvider
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue, int timeSpan);
    }
}
