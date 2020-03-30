using Accord.Math;
using Accord.Math.Optimization;

namespace PersonalBlog.Services.Helpers
{
    public static class LinearRegression
    {
        public static double[] Learn(double[][] X, double[] Y, double[] weights, double lambda = 0)
        {            
            var optimizationAlgorithm = new BroydenFletcherGoldfarbShanno(weights.Length);
            double m = Y.Length;

            optimizationAlgorithm.Function = (double[] t) =>
                (1.0 / 2.0 * m) * (X.Dot(t).Subtract(Y).Pow(2).Sum()) + (lambda / 2.0 * m) * (t.Pow(2).Sum());

            optimizationAlgorithm.Gradient = (double[] t) =>
                X.TransposeAndDot(X.Dot(t).Subtract(Y)).Divide(m).Add(t.Multiply(lambda / m));

            optimizationAlgorithm.Minimize(weights);
            return optimizationAlgorithm.Solution
                .To<double[]>();
        }
    }
}
