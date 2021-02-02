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
            BendStiffener bendStiffener = new BendStiffener
            {
                max_k = 0.3f,
                D1 = 0.7,
                d1 = 0.18,
                d2 = 0.2,
                L1 = 0.1,
                L2 = 2.3,
                L3 = 0.2,
                EIp = 1e4,
                Es = 4.5e6,
                F = 2e4,
                q = 0.52
            };

            int N = 10000;

            Vector<double>[] res = bendStiffener.bvpsolver_zzz(bendStiffener.Ode_to_solve, 0.0, bendStiffener.q, 0.0, 5.0, N);
            double[] x = Vector<double>.Build.Dense(N, i => i * 5.0 / N).ToArray();
            double[] y = new double[N];
            double[] dydx = new double[N];

            for (int i = 0; i < N; i++)
            {
                double[] temp = res[i].ToArray();
                y[i] = temp[0];
                dydx[i] = temp[1];
                //Console.WriteLine(t[i]);
            }
            var plt = new ScottPlot.Plot(800, 600);
            //plt.PlotScatter(x, y, label: "y", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            plt.PlotScatter(x, dydx, label: "dydx", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            plt.Grid(false);
            plt.Legend(fontSize: 10);
            //plt.PlotSignal(new[,] { x, y });
            plt.SaveFig("quickstart.png");


            Console.WriteLine("plot finished"); // gives 164,537981852489
            //Test
            //Console.WriteLine(y[N / 10]); // gives 164,537981852489
            //Console.WriteLine(Math.Exp(-1) + 3 * Math.Exp(4) - 5.0 / 2 + 23.0 / 8); //gives 164,537329540604, which is y(1)

            Console.ReadKey();
        }
    }
}