using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ScoreTracking.App
{
    [SimpleJob(RuntimeMoniker.Net70)]
    public class TestClass
    {
        int NumberOfItems = 10;
        [Benchmark]
        public string ConcatStringsUsingStringBuilder()
        {
            var sb = new StringBuilder();

            return "Hello World!";
        }
        [Benchmark(Baseline = true)]
        public string ConcatStringsUsingGenericList()
        {
            var list = new List<string>(NumberOfItems);
            return "Hello World!";
        }
    }
}
