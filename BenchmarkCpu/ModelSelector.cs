using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

namespace BenchmarkCpu
{
    abstract class ApproximationModel
    {
        public abstract ModelScore Fit(double[] x, double[] y);
    }

    class ConstantModel : ApproximationModel
    {
        public override ModelScore Fit(double[] x, double[] y)
        {
            var @const = MathNet.Numerics.Fit.LinearCombination(x, y, p => 1.0);
            return new ModelScore(1, p => @const[0], x, y);
        }
    }

    class LogModel : ApproximationModel
    {
        public override ModelScore Fit(double[] x, double[] y)
        {
            var log = MathNet.Numerics.Fit.LinearCombination(x, y, p => 1.0, p => Math.Log(p, 2));
            return new ModelScore(2, p => log[0] + log[1] * Math.Log(p, 2), x, y);
        }
    }


    class LinearModel : ApproximationModel
    {
        public override ModelScore Fit(double[] x, double[] y)
        {
            var line = MathNet.Numerics.Fit.Line(x, y);
            return new ModelScore(2, p => line.Item1 + line.Item2 * p, x, y);
        }
    }


    class Poly2Model : ApproximationModel
    {
        public override ModelScore Fit(double[] x, double[] y)
        {
            var poly = MathNet.Numerics.Fit.Polynomial(x, y, 2);
            return new ModelScore(3, p => poly[0] + poly[1] * p + poly[2] * p * p, x, y);
        }
    }

    class NLogNModel : ApproximationModel
    {
        public override ModelScore Fit(double[] x, double[] y)
        {
            var nLogN = MathNet.Numerics.Fit.LinearCombination(x, y, p => 1.0, p => p * Math.Log(p, 2));
            return new ModelScore(2, p => nLogN[0] + nLogN[1] * p * Math.Log(p, 2), x, y);
        }
    }


    public class ModelSelector
    {

        public BigO FindO(double[] iterationCounts, double[] microseconds)
        {
            var n = microseconds.Length;

            var candidates = new Dictionary<BigO, ApproximationModel>
            {
                [BigO.Constant] = new ConstantModel(),
                [BigO.LogN] = new LogModel(),
                [BigO.N] = new LinearModel(),
                [BigO.NSquared] = new Poly2Model(),
                [BigO.NLogN] = new NLogNModel(),
            };

            var bestAic = 1000.0;
            var result = BigO.HardToDetect;

            foreach (var model in candidates)
            {
                var score = model.Value.Fit(iterationCounts, microseconds);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Evaluating model {model.Key}: aic: {score.Aic} aicc: {score.AicC} r2: {score.R2}");
                Console.ResetColor();

                if (score.Aic < bestAic && score.R2 > 0.9)
                {
                    bestAic = score.Aic;
                    result = model.Key;
                }
            }
            // TODO Burnham and Anderson (2003) give the following rule of thumb for interpreting the ΔAIC Scores:
            // Δ AIC = AIC[i] – min(AIC)
            // Δ AIC < 2     : substantial evidence for the model.
            // 3 > Δ AIC < 7 : less support for the model.
            // Δ AIC > 10    : the model is unlikely.


            return result;
        }
    }
}