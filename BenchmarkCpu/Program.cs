using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using MathNet.Numerics.Interpolation;

namespace BenchmarkCpu
{
    public enum BigO
    {
        /// <summary>
        /// O(1)
        /// </summary>
        Constant,

        /// <summary>
        /// O(Log(2, N))
        /// </summary>
        LogN,

        /// <summary>
        /// O(N)
        /// </summary>
        N,

        /// <summary>
        /// O(N * Log(N))
        /// </summary>
        NLogN,

        /// <summary>
        /// O(N*N)
        /// </summary>
        NSquared,

        /// <summary>
        /// Very fast grow - Either polynomial with power grater than 2 or exponential 
        /// </summary>
        NP,

        /// <summary>
        /// some tricky dependency
        /// </summary>
        HardToDetect,


    }

    [RPlotExporter, RankColumn]
    public class ReassembleMarkdownBenchmark
    {
        private string data;
        private ReassembleMarkdown subject;


        [Params(10000, 20000, 30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000, 110000, 120000, 130000, 140000, 150000, 160000, 170000, 180000, 190000, 200000)]
        public int N;
        public int PercentOfMarkdownChars = 10;


        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(42);

            var chars = new char[N];
            for (int i = 0; i < chars.Length; i++)
            {
                if (random.Next(100) < PercentOfMarkdownChars)
                {
                    char c = ReassembleMarkdown.markdownCharacters[
                        random.Next(ReassembleMarkdown.markdownCharacters.Length)];
                    chars[i] = c;
                }
                else
                    chars[i] = (char) random.Next(255);
            }

            data = new string(chars);
            subject = new ReassembleMarkdown();
        }

        [Benchmark]
        public string OldMethod() => subject.OldEscapeMarkDownCharacters(data);

        [Benchmark]
        public string NewMethod() => subject.NewEscapeMarkDownCharacters(data);
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var summary = BenchmarkRunner.Run<ReassembleMarkdownBenchmark>();
                // Uncomment if you want to debug the after-benchmark logic (and recompile in debug mode)
                //var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig()).First();
                Console.WriteLine(summary);
                
                var workloads = summary.Reports.Select(r => r.BenchmarkCase.Descriptor.WorkloadMethod.Name).Distinct().ToArray();

                foreach (var workload in workloads)
                {
                    var workloadNumbers = summary.Reports
                        .Where(r => r.BenchmarkCase.Descriptor.WorkloadMethod.Name == workload &&
                                    r.BenchmarkCase.Parameters.Items[0].Name == "N")
                        .Select(r => new
                        {
                            median = (double) r.ResultStatistics.Percentiles.P50,
                            n = (int) r.BenchmarkCase.Parameters.Items[0].Value

                        }).ToArray();
                    var bigO = new ModelSelector().FindO(
                        workloadNumbers.Select(w => (double) w.n).ToArray(),
                        workloadNumbers.Select(w => w.median).ToArray());
                    Console.WriteLine($"Workload {workload}: {bigO}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }

    }

}
