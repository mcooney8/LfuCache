using System.Runtime.Caching.Generic;
using BenchmarkDotNet.Attributes;
using LfuCache;

namespace LfuCacheBenchmarks
{
    [MemoryDiagnoser]
    public class LfuBenchmark
    {
        [Params(100)]
        public int Capacity { get; set; }

        [Params(1000)]
        public int EnqueueCount { get; set; }

        [Benchmark]
        public void LfuCacheAdd()
        {
            var lfuCache = new LfuCache<int, string>(Capacity);
            for (int i = 0; i < EnqueueCount; i++)
            {
                lfuCache.Add(i, i.ToString());
            }
        }

        [Benchmark]
        public void LfuCacheGet()
        {
            int key = 1;
            string value = "ABC";
            var lfuCache = new LfuCache<int, string>(Capacity);
            lfuCache.Add(key, value);
            for (int i = 0; i < EnqueueCount; i++)
            {
                lfuCache.Get(key);
            }
        }

        [Benchmark]
        public void GenericCacheAdd()
        {
            var lfuCache = new MemoryCache<int, string>(Capacity);
            lfuCache.SetPolicy(typeof(LfuEvictionPolicy<,>));
            for (int i = 0; i < EnqueueCount; i++)
            {
                lfuCache.Add(i, i.ToString());
            }
        }

        [Benchmark]
        public void GenericCacheGet()
        {
            int key = 1;
            string value = "ABC";
            var lfuCache = new MemoryCache<int, string>(Capacity);
            lfuCache.Add(key, value);
            for (int i = 0; i < EnqueueCount; i++)
            {
                lfuCache.Get(key);
            }
        }
    }
}
