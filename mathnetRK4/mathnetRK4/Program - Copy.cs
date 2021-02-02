using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.OdeSolvers;
using ScottPlot;

namespace mathnetRK4
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 10000;
            Vector<double> y0 = Vector<double>.Build.Dense(new[] { -7.0 / 4.0, 55.0 / 8.0 });
            //Func<double, Vector<double>, Vector<double>> der = DerivativeMaker();

            Vector<double>[] res = RungeKutta.FourthOrder(y0, 0, 10, N, DerivativeMaker_zzz2);

            double[] t = Vector<double>.Build.Dense(N, i => i * 10.0 / N).ToArray();
            double[] x = new double[N];
            double[] y = new double[N];
            for (int i = 0; i < N; i++)
            {
                double[] temp = res[i].ToArray();
                x[i] = temp[0];
                y[i] = temp[1];
                //Console.WriteLine(t[i]);
            }

            var plt = new ScottPlot.Plot(800, 600);
            plt.PlotScatter(t, x, label: "x", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            plt.PlotScatter(t, y, label: "y", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            plt.Grid(false);
            plt.Legend(fontSize: 10);
            //plt.PlotSignal(new[,] { x, y });
            plt.SaveFig("quickstart.png");


            //Test
            Console.WriteLine(y[N / 10]); // gives 164,537981852489
            Console.WriteLine(Math.Exp(-1) + 3 * Math.Exp(4) - 5.0 / 2 + 23.0 / 8); //gives 164,537329540604, which is y(1)

            Console.ReadKey();
        }

        //written by the origianl author, but I think it is a little wierd as can not found anywhere else
        //NOT recommended, as it is not as good as "DerivativeMaker_zzz2" which can be used directly in the RK4 solver
        //lamda expression: (input-parameters) => { <sequence-of-statements> }
        public static Func<double, Vector<double>, Vector<double>> DerivativeMaker()
        {
            return (t, Z) =>
            {
                double[] A = Z.ToArray();
                double x = A[0];
                double y = A[1];

                return Vector<double>.Build.Dense(new[] { x + 2 * y + 2 * t, 3 * x + 2 * y - 4 * t });
            };
        }

        //the most normal form
        static Vector<double> DerivativeMaker_zzz1(double t, Vector<double> Z)
        {
            double[] A = Z.ToArray();
            double x = A[0];
            double y = A[1];

            return Vector<double>.Build.Dense(new[] { x + 2 * y + 2 * t, 3 * x + 2 * y - 4 * t });
        }

        //lamda expression: function_name = (input-parameters) => { <sequence-of-statements> };
        static Func<double, Vector<double>, Vector<double>> DerivativeMaker_zzz2 = (t, Z) =>
       {
           double[] A = Z.ToArray();
           double x = A[0];
           double y = A[1];

           return Vector<double>.Build.Dense(new[] { x + 2 * y + 2 * t, 3 * x + 2 * y - 4 * t });
       };

    }
}