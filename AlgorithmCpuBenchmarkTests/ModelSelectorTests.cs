using System;
using BenchmarkCpu;
using Xunit;

namespace AlgorithmCpuBenchmarkTests
{
    public class ModelSelectorTests
    {
        [Fact]
        public void TestLinear()
        {
            var x = new[] {1000.0, 2000.0, 3000.0, 5000.0, 10000.0, 20000.0 };
            var y = new [] {5.621, 11.428, 17.792, 29.777, 61.806, 125.535 };
            ModelSelector s = new ModelSelector();
            var actual = s.FindO(x, y);
            Assert.Equal(BigO.N, actual);
        }      
        
        [Fact]
        public void TestSquare()
        {
            var x = new[] { 1000.0, 2000.0, 3000.0, 5000.0, 10000.0, 20000.0 };
            var y = new [] { 139.297, 354.560, 619.220, 1273.384, 4765.155, 17725.106 };

            ModelSelector s = new ModelSelector();
            var actual = s.FindO(x, y);
            Assert.Equal(BigO.NSquared, actual);
        }
    }
}
