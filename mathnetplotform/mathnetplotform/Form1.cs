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
            dt.Rows.Add("d1",0.18 );
            dt.Rows.Add("d2",0.2 );
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
