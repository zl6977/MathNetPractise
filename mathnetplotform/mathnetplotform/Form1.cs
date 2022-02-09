using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using MathNet.Numerics.OdeSolvers;
using MathNet.Numerics.Interpolation;
using ScottPlot;
using ScottPlot.WinForms;

namespace mathnetplotform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DataTable dt = new DataTable();
            dataGridView1.DataSource = dt;
            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("Value", typeof(double));
            dt.Rows.Add("max_k", bendStiffener.max_k);
            dt.Rows.Add("D1", bendStiffener.D1);
            dt.Rows.Add("d1", bendStiffener.d1);
            dt.Rows.Add("d2", bendStiffener.d2);
            dt.Rows.Add("L1", bendStiffener.L1);
            dt.Rows.Add("L2", bendStiffener.L2);
            dt.Rows.Add("L3", bendStiffener.L3);
            dt.Rows.Add("EIp", bendStiffener.EIp);
            dt.Rows.Add("Es", bendStiffener.Es);
            dt.Rows.Add("F", bendStiffener.F);
            dt.Rows.Add("q", bendStiffener.q);
        }

        BendStiffener bendStiffener = new BendStiffener
        {
            max_k = 0.3,
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

        private void button_calc_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            bendStiffener.max_k = (double)dt.Rows[0][1];
            bendStiffener.D1 = (double)dt.Rows[1][1];
            bendStiffener.d1 = (double)dt.Rows[2][1];
            bendStiffener.d2 = (double)dt.Rows[3][1];
            bendStiffener.L1 = (double)dt.Rows[4][1];
            bendStiffener.L2 = (double)dt.Rows[5][1];
            bendStiffener.L3 = (double)dt.Rows[6][1];
            bendStiffener.EIp = (double)dt.Rows[7][1];
            bendStiffener.Es = (double)dt.Rows[8][1];
            bendStiffener.F = (double)dt.Rows[9][1];
            bendStiffener.q = (double)dt.Rows[10][1];

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

            formsPlot1.Reset();
            var plt = new ScottPlot.Plot(800, 600);
            //plt.PlotScatter(x, y, label: "y", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));

            //plt.PlotSignal(new[,] { x, y });
            //plt.SaveFig("quickstart.png");

            double[] lineX = new double[] { 0, 5 };
            double[] lineY = new double[] { bendStiffener.max_k, bendStiffener.max_k };
            formsPlot1.plt.PlotScatter(lineX, lineY, label: "Pass", lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));

            double[] max_pnt = new double[] { x[Array.IndexOf(dydx, dydx.Max())], dydx.Max() };

            formsPlot1.plt.PlotPoint(max_pnt[0], max_pnt[1], color: Color.Magenta, markerSize: 15);

            formsPlot1.plt.PlotScatter(x, dydx, label: "dydx", lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            formsPlot1.plt.PlotScatter(x, y, label: "y", lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            //plt.PlotScatter(x, dydx, label: "dydx", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            formsPlot1.plt.Grid(true);
            formsPlot1.plt.Legend(fontSize: 20, location: legendLocation.lowerLeft);
            formsPlot1.Render();
        }


        private void button_calc_Click_1(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            bendStiffener.max_k = (double)dt.Rows[0][1];
            bendStiffener.D1 = (double)dt.Rows[1][1];
            bendStiffener.d1 = (double)dt.Rows[2][1];
            bendStiffener.d2 = (double)dt.Rows[3][1];
            bendStiffener.L1 = (double)dt.Rows[4][1];
            bendStiffener.L2 = (double)dt.Rows[5][1];
            bendStiffener.L3 = (double)dt.Rows[6][1];
            bendStiffener.EIp = (double)dt.Rows[7][1];
            bendStiffener.Es = (double)dt.Rows[8][1];
            bendStiffener.F = (double)dt.Rows[9][1];
            bendStiffener.q = (double)dt.Rows[10][1];

            int N = 10000;
            //Vector<double>[] res = bendStiffener.bvpsolver_zzz(bendStiffener.Ode_to_solve, 0.0, bendStiffener.q, 0.0, 5.0, N);
            double[] x = Vector<double>.Build.Dense(N, i => i * 5.0 / N).ToArray();
            double[] y = new double[N];
            double[] dydx = new double[N];

            //initialisation of plot
            formsPlot1.Reset();
            var plt = new ScottPlot.Plot(800, 600);
            //plt.PlotScatter(x, y, label: "y", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));

            //plt.PlotSignal(new[,] { x, y });
            //plt.SaveFig("quickstart.png");

            double[] lineX = new double[] { 0, 5 };
            double[] lineY = new double[] { bendStiffener.max_k, bendStiffener.max_k };
            formsPlot1.plt.PlotScatter(lineX, lineY, label: "Pass", lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));

            //double[] max_pnt = new double[] { x[Array.IndexOf(dydx, dydx.Max())], dydx.Max() };

            //formsPlot1.plt.PlotPoint(max_pnt[0], max_pnt[1], color: Color.Magenta, markerSize: 15);
            //initialisation of plot
            //--------
            double ya = 0;
            double yb = bendStiffener.q;
            double xa = 0;
            double xb = 5;
            Func<double, Vector<double>, Vector<double>> system = bendStiffener.Ode_to_solve;

            Vector<double>[] res;
            Vector<double> y0;

            double s_guess = (yb - ya) / (xb - xa), s_guess_pre = 0, s_guess_n;
            double fais = yb, fais_pre = 0, dfai, ds;
            //bool changeDirectionFlag = false;
            int countTimes = 100;
            double[] faisLst = new double[countTimes];
            double[] sLst = new double[countTimes];

            for (int count = 0; count < countTimes; count++)
            {
                y0 = Vector<double>.Build.DenseOfArray(new[] { ya, s_guess_pre });
                // observer_pr ob_pr = this->write_targetFunc_end;
                res = RungeKutta.FourthOrder(y0, xa, xb, N, system);
                fais_pre = res[N - 1][0] - yb;

                y0 = Vector<double>.Build.DenseOfArray(new[] { ya, s_guess });
                // observer_pr ob_pr = this->write_targetFunc_end;
                res = RungeKutta.FourthOrder(y0, xa, xb, N, system);
                fais = res[N - 1][0] - yb;


                ds = -fais * (s_guess - s_guess_pre) / (fais - fais_pre);
                s_guess_n = s_guess + ds;

                s_guess_pre = s_guess;
                s_guess = s_guess_n;
                fais_pre = fais;

                ////dfaids = (fais - fais_pre) / (s_guess - s_guess_pre);
                //dfai = fais - fais_pre;
                //ds = s_guess - s_guess_pre;

                //s_guess_pre = s_guess;
                ////s_guess = s_guess - fais / dfaids;
                //s_guess = s_guess - fais * ds / dfai;
                ////s_guess = s_guess - fais / (fais - fais_pre) * (s_guess - s_guess_pre);

                faisLst[count] = fais;
                sLst[count] = s_guess;
                //formsPlot1.plt.PlotPoint(s_guess, fais, color: Color.Magenta, markerSize: 15);
                //---------
                double[][] dydx2 = new double[countTimes][];
                for (int i = 0; i < N; i++)
                {
                    double[] temp = res[i].ToArray();
                    y[i] = temp[0];
                    dydx[i] = temp[1];
                    //Console.WriteLine(t[i]);
                }
                dydx2[count] = (double[])dydx.Clone();

                //formsPlot1.plt.PlotScatter(x, dydx2[count], label: "dydx" + count.ToString(), lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
                //formsPlot1.plt.PlotScatter(x, (double[])y.Clone(), label: "y" + count.ToString(), lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            }
            formsPlot1.plt.PlotScatter(x, dydx, label: "dydx", lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            formsPlot1.plt.PlotScatter(sLst, faisLst, label: "fais-s", lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            //plt.PlotScatter(x, dydx, label: "dydx", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            formsPlot1.plt.Grid(true);
            formsPlot1.plt.Legend(fontSize: 20, location: legendLocation.lowerLeft);
            formsPlot1.Render();
        }

        private void button_calc_Click_3(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            bendStiffener.max_k = (double)dt.Rows[0][1];
            bendStiffener.D1 = (double)dt.Rows[1][1];
            bendStiffener.d1 = (double)dt.Rows[2][1];
            bendStiffener.d2 = (double)dt.Rows[3][1];
            bendStiffener.L1 = (double)dt.Rows[4][1];
            bendStiffener.L2 = (double)dt.Rows[5][1];
            bendStiffener.L3 = (double)dt.Rows[6][1];
            bendStiffener.EIp = (double)dt.Rows[7][1];
            bendStiffener.Es = (double)dt.Rows[8][1];
            bendStiffener.F = (double)dt.Rows[9][1];
            bendStiffener.q = (double)dt.Rows[10][1];

            int N = 10000;
            //Vector<double>[] res = bendStiffener.bvpsolver_zzz(bendStiffener.Ode_to_solve, 0.0, bendStiffener.q, 0.0, 5.0, N);
            double[] x = Vector<double>.Build.Dense(N, i => i * 5.0 / N).ToArray();
            double[] y = new double[N];
            double[] dydx = new double[N];

            //initialisation of plot
            formsPlot1.Reset();
            var plt = new ScottPlot.Plot(800, 600);
            //plt.PlotScatter(x, y, label: "y", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));

            //plt.PlotSignal(new[,] { x, y });
            //plt.SaveFig("quickstart.png");

            double[] lineX = new double[] { 0, 5 };
            double[] lineY = new double[] { bendStiffener.max_k, bendStiffener.max_k };
            //formsPlot1.plt.PlotScatter(lineX, lineY, label: "Pass", lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));

            //double[] max_pnt = new double[] { x[Array.IndexOf(dydx, dydx.Max())], dydx.Max() };

            //formsPlot1.plt.PlotPoint(max_pnt[0], max_pnt[1], color: Color.Magenta, markerSize: 15);
            //initialisation of plot
            //--------
            double ya = 0;
            double yb = bendStiffener.q;
            double xa = 0;
            double xb = 5;
            Func<double, Vector<double>, Vector<double>> system = bendStiffener.Ode_to_solve;

            Vector<double>[] res;
            Vector<double> y0;

            //double s_guess = (yb - ya) / (xb - xa), s_guess_pre = 0;

            double fais = yb, fais_pre = 0, dfaids = 0, dfai, ds;
            //bool changeDirectionFlag = false;
            int countTimes = 1000;
            double[] faisLst = new double[countTimes];
            double[] sLst = new double[countTimes];
            sLst = Vector<double>.Build.Dense(N, i => -0.05 + i * 0.3 / countTimes).ToArray();

            for (int count = 0; count < countTimes; count++)
            {
                y0 = Vector<double>.Build.DenseOfArray(new[] { ya, sLst[count] });
                // observer_pr ob_pr = this->write_targetFunc_end;
                res = RungeKutta.FourthOrder(y0, xa, xb, N, system);
                fais_pre = fais;
                fais = res[N - 1][0] - yb;
                ////dfaids = (fais - fais_pre) / (s_guess - s_guess_pre);
                //dfai = fais - fais_pre;
                //ds = s_guess - s_guess_pre;

                //s_guess_pre = s_guess;
                ////s_guess = s_guess - fais / dfaids;
                //s_guess = s_guess - fais * ds / dfai;
                ////s_guess = s_guess - fais / (fais - fais_pre) * (s_guess - s_guess_pre);

                faisLst[count] = fais;
                //sLst[count] = s_guess;
                formsPlot1.plt.PlotPoint(sLst[count], fais, color: Color.Magenta, markerSize: 15);
                //---------
                double[][] dydx2 = new double[countTimes][];
                for (int i = 0; i < N; i++)
                {
                    double[] temp = res[i].ToArray();
                    y[i] = temp[0];
                    dydx[i] = temp[1];
                    //Console.WriteLine(t[i]);
                }
                dydx2[count] = (double[])dydx.Clone();

                //formsPlot1.plt.PlotScatter(x, dydx2[count], label: "dydx" + count.ToString(), lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            }
            //formsPlot1.plt.PlotScatter(x, dydx, label: "dydx", lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            formsPlot1.plt.PlotScatter(sLst, faisLst, label: "fais-s", lineWidth: 5, markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            //plt.PlotScatter(x, dydx, label: "dydx", markerShape: (MarkerShape)Enum.Parse(typeof(MarkerShape), "none"));
            formsPlot1.plt.Grid(true);
            formsPlot1.plt.Legend(fontSize: 20, location: legendLocation.lowerLeft);
            formsPlot1.Render();
        }

        private void button_default_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("Value", typeof(double));
            dt.Rows.Add("max_k", 0.3);
            dt.Rows.Add("D1", 0.7);
            dt.Rows.Add("d1", 0.18);
            dt.Rows.Add("d2", 0.2);
            dt.Rows.Add("L1", 0.1);
            dt.Rows.Add("L2", 2.3);
            dt.Rows.Add("L3", 0.2);
            dt.Rows.Add("EIp", 1e4);
            dt.Rows.Add("Es", 4.5e6);
            dt.Rows.Add("F", 2e4);
            dt.Rows.Add("q", 0.52);
            dataGridView1.DataSource = dt;
        }
    }
}
