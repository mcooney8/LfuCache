using System;
using BenchmarkDotNet.Running;

namespace LfuCacheBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
#if RELEASE

            BenchmarkRunner.Run<LfuBenchmark>();
#else
            var benchmark = new LfuBenchmark();
            benchmark.Capacity = 1024;
            benchmark.EnqueueCount = 10000;
            benchmark.LfuCacheAdd();
#endif
        }
    }
}
