using System;

namespace mathnetplot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotScatter(dataX, dataY);
            plt.SaveFig("quickstart.png");
        }
    }
}
