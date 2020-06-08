using System;
using System.Linq;
using MathNet.Numerics;

namespace BenchmarkCpu
{
    internal struct ModelScore
    {
        /// <summary>
        /// Residual sum of squares
        /// </summary>
        public readonly double Rss;

        /// <summary>
        /// Akaike information criterion
        /// </summary>
        public readonly double Aic;

        /// <summary>
        /// Akaike information criterion corrected for small numbers of observations
        /// </summary>
        public readonly double AicC;

        /// <summary>
        /// Mean square error
        /// </summary>
        public readonly double Mse;

        /// <summary>
        /// Root of Mean square error
        /// </summary>
        public readonly double Rmse;

        public readonly double R2;

        /// <summary>
        /// Calculate Akaike information criterion using Burnhan & Anderson shortcut for Least square fitting we used
        /// </summary>
        /// <param name="k">number of model parameters</param>
        /// <param name="n">number of points</param>
        /// <param name="rss">residual square sum</param>
        private static double CalcAic(int k, int n, double rss)
        {
            // C depends only on input data so we can use just any constant here if we select best value among different models 
            // and don't care about absolute AIC value
            var someAdditionDependentOnInputOnly = 0; // (n * Math.Log(n) + 2 * C)

            return 2 * k + n * Math.Log(rss / n) - someAdditionDependentOnInputOnly;
        }

        /// <summary>
        /// When the sample size is small, there is a substantial probability that
        /// AIC will select models that have too many parameters.
        /// Recommended criteria: n/k less 40 
        /// </summary>
        private static double CalcAicC(int k, int n, double rss)
        {
            return CalcAic(k, n, rss) + (2.0 * k * k + 2.0 * k) / (n - k - 1);
        }

        private static double Square(double x) => x * x;

        public ModelScore(int nParams, Func<double, double> predictor, double[] x, double[] observed) : this()
        {
            int k = nParams, n = x.Length;
            var predicted = x.Select(predictor).ToArray();
            Rss = predicted.Zip(observed, (p, o) => Square(p - o)).Sum();
            Mse = Rss / n;
            Rmse = Math.Sqrt(Mse);
            var observedMean = observed.Average();
            var tss = Math.Sqrt(observed.Select(d => Square(d - observedMean)).Sum());
            R2 = GoodnessOfFit.CoefficientOfDetermination(predicted, observed);
            Aic = CalcAic(k, n, Rss);
            AicC = CalcAicC(k, n, Aic);
            // Note that AIC tells nothing about the absolute quality of a model, only the quality relative to other models.
            // Lets compare the rss to original values to approve only good accuracy
        }
    }
}