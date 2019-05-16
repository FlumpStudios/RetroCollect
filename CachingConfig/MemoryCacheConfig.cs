using System;
using Microsoft.Extensions.Caching.Memory;

namespace CachingConfig
{
    public class MemoryCacheConfig
    {
        public MemoryCache Cache { get; set; }
        public MemoryCacheConfig(int sizeLimit = 1000)
        {
            Cache = new MemoryCache(new MemoryCacheOptions
            {
                //Set the size limit of the cache. have set to a thousand objects for Retro collects, think that should be enough :)
                SizeLimit = sizeLimit
            });
        }
    }
}
