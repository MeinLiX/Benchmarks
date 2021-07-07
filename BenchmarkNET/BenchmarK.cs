using System;
using System.Diagnostics;

namespace BenchmarkNET
{
    public static class BenchmarK
    {
        public static void TimeCheck(string Name, Action act)
        {
            GC.Collect();
            Stopwatch sw = Stopwatch.StartNew();

            act.Invoke();

            sw.Stop();
            Console.WriteLine($"{Name}: {sw.ElapsedMilliseconds} ms");
        }
    }
}
