using Microsoft.Extensions.Caching.Memory;
using System;
using CachingConfig;

namespace Caching
{
    public class CachingManager : ICachingManager
    {
        private readonly IMemoryCache _cache;


        private TimeSpan _timeSpan = TimeSpan.FromMinutes(10);
        private bool _useRollingInterval = false;
        private long _ObjectSize = 0;

        public CachingManager(MemoryCacheConfig memoryCache)
        {
            _cache = memoryCache.Cache;
        }

        /// <summary>
        /// Attempt to get the object from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCache<T>(CacheKeys key)
        {
            return (T)_cache.Get(key.ToString());
        }

        /// <summary>
        /// Overload GetCache to allow string as Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCache<T>(string key)
        {
            return (T)_cache.Get(key);
        }


        /// <summary>
        /// Set cache in memory and return original object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        /// <param name="useRollingInterval"></param>
        /// <returns></returns>
        public T SetCache<T>(T obj, CacheKeys key, TimeSpan? timeSpan = null, bool useRollingInterval = false, int size = 1)
        {
            if (timeSpan != null) _timeSpan = (TimeSpan)timeSpan;
            _useRollingInterval = useRollingInterval;
            _ObjectSize = size;  
            _cache.Set(key.ToString(), obj, GetCachingOption());
            return obj;
        }
        /// <summary>
        /// OVerload cache set to accept string as key instead of CacheKeys enum 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        /// <param name="useRollingInterval"></param>
        /// <returns></returns>
        public T SetCache<T>(T obj, string key, TimeSpan? timeSpan = null, bool useRollingInterval = false, int size = 1)
        {
            if (timeSpan != null) _timeSpan = (TimeSpan)timeSpan;
            _useRollingInterval = useRollingInterval;
            _ObjectSize = size;
            _cache.Set(key, obj, GetCachingOption());
            return obj;
        }

        /// <summary>
        /// Remove cache on requested key
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCache(CacheKeys key)
        {
            _cache.Remove(key.ToString());
        }

        /// <summary>
        /// Clear cache on all keys
        /// </summary>
        public void ClearAllCache()
        {
            foreach (var key in Enum.GetNames(typeof(CacheKeys)))
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// Set the caching options
        /// </summary>
        /// <returns></returns>
        private MemoryCacheEntryOptions GetCachingOption()
        {
            //Set default timespan for caching if none provided
            var cacheEntryOptions = new MemoryCacheEntryOptions();

            if (_useRollingInterval) cacheEntryOptions.SetAbsoluteExpiration(_timeSpan);
            else cacheEntryOptions.SetSlidingExpiration(_timeSpan);

            cacheEntryOptions.SetSize(_ObjectSize);
            return cacheEntryOptions;
        }
    }

        public enum CacheKeys
        {
            TestKey,
            GameList
        }
    
}