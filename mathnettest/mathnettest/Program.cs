using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.OdeSolvers;

using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace Sys
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 1000000;
            Vector<double> y0 = Vector<double>.Build.Dense(new[] { -7.0 / 4.0, 55.0 / 8.0 });
            Func<double, Vector<double>, Vector<double>> der = DerivativeMaker();

            Vector<double>[] res = RungeKutta.FourthOrder(y0, 0, 10, N, der);

            double[] x = new double[N];
            double[] y = new double[N];
            for (int i = 0; i < N; i++)
            {
                double[] temp = res[i].ToArray();
                x[i] = temp[0];
                y[i] = temp[1];
            }

            //Test
            Console.WriteLine(y[N / 10]); // gives 164,537981852489
            Console.WriteLine(Math.Exp(-1) + 3 * Math.Exp(4) - 5.0 / 2 + 23.0 / 8); //gives 164,537329540604, which is y(1)


            Console.ReadKey();

        }

        static Func<double, Vector<double>, Vector<double>> DerivativeMaker()
        {
            return (t, Z) =>
            {
                double[] A = Z.ToArray();
                double x = A[0];
                double y = A[1];

                return Vector<double>.Build.Dense(new[] { x + 2 * y + 2 * t, 3 * x + 2 * y - 4 * t });

            };
        }
    }
}

namespace Winforms.Cartesian.BasicLine
{
    public partial class BasicLineExample : Form
    {
        public BasicLineExample()
        {
            InitializeComponent();

            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> {4, 6, 5, 2, 7}
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {6, 7, 3, 4, 6},
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {5, 2, 8, 3},
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Month",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Sales",
                LabelFormatter = value => value.ToString("C")
            });

            cartesianChart1.LegendLocation = LegendLocation.Right;

            //modifying the series collection will animate and update the chart
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { 5, 3, 2, 4, 5 },
                LineSmoothness = 0, //straight lines, 1 really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            cartesianChart1.Series[2].Values.Add(5d);


            cartesianChart1.DataClick += CartesianChart1OnDataClick;
        }

        private void CartesianChart1OnDataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show("You clicked (" + chartPoint.X + "," + chartPoint.Y + ")");
        }
    }
}