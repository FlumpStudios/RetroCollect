using System;

namespace Caching
{
    public interface ICachingManager
    {
        void ClearAllCache();
        T GetCache<T>(CacheKeys key);
        T GetCache<T>(string key);
        void RemoveCache(CacheKeys key);
        T SetCache<T>(T obj, CacheKeys key, TimeSpan? timeSpan = null, bool useRollingInterval = false, int size = 1);
        T SetCache<T>(T obj, string key, TimeSpan? timeSpan = null, bool useRollingInterval = false, int size = 1);
    }
}