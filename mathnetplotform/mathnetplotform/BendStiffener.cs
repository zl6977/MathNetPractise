using System;
using System.Collections.Generic;
using System.Text;
using MathNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.OdeSolvers;

namespace mathnetplotform
{
    class BendStiffener
    {
        public double max_k { get; set; }
        public double D1 { get; set; }
        public double d1 { get; set; }
        public double d2 { get; set; }
        public double L1 { get; set; }
        public double L2 { get; set; }
        public double L3 { get; set; }
        public double EIp { get; set; }
        public double Es { get; set; }
        public double F { get; set; }
        public double q { get; set; }

        //Func<double, Vector<double>, Vector<double>> Ode_to_solve = (x, Y) =>
        //{
        //	double[] dydx = new double[2];
        //	dydx[0] = Y[1];
        //	dydx[1] = -(dEI(x) * Y[1] + F * Math.Sin(q - Y[0])) / EI(x);
        //	Vector<double> result;
        //	result = Vector<double>.Build.DenseOfArray(dydx);
        //	return result;
        //};
        public Vector<double> Ode_to_solve(double x, Vector<double> Y)
        {
            double[] dYdx = new double[2];
            dYdx[0] = Y[1];
            dYdx[1] = -(dEI(x) * Y[1] + F * Math.Sin(q - Y[0])) / EI(x);
            Vector<double> result;
            result = Vector<double>.Build.DenseOfArray(dYdx);
            return result;
        }


        public Vector<double>[] bvpsolver_zzz(Func<double, Vector<double>, Vector<double>> system,
                             double ya, double yb,
                             double xa, double xb,
                             int N)
        {
            Vector<double>[] res;
            Vector<double> y0;
            double s_guess = (yb - ya) / (xb - xa), s_guess_pre = 0;
            double fais = yb, fais_pre = 0, dfaids = 0;
            do
            {
                y0 = Vector<double>.Build.DenseOfArray(new[] { ya, s_guess });
                // observer_pr ob_pr = this->write_targetFunc_end;
                res = RungeKutta.FourthOrder(y0, xa, xb, N, system);
                fais_pre = fais;
                fais = res[N - 1][0] - yb;
                dfaids = (fais - fais_pre) / (s_guess - s_guess_pre);
                s_guess_pre = s_guess;
                s_guess = s_guess - fais / dfaids;
                //count++;
            } while (fais > 0.001 * yb + 1e-6 || fais < -0.001 * yb - 1e-6);

            return res;
        }
        double EI(double x)
        {
            double Ds, EI, Is;
            if (x < L1)
            {
                Ds = D1;
                Is = Math.PI / 64 * (Math.Pow(Ds, 4) - Math.Pow(d1, 4));
                EI = Es * Is + EIp;
            }
            else if (x < L1 + L2)
            {
                Ds = D1 - ((D1 - d2) / L2) * (x - L1);
                Is = Math.PI / 64 * (Math.Pow(Ds, 4) - Math.Pow(d1, 4));
                EI = Es * Is + EIp;
            }
            else if (x < L1 + L2 + L3)
            {
                Ds = d2;
                Is = Math.PI / 64 * (Math.Pow(Ds, 4) - Math.Pow(d1, 4));
                EI = Es * Is + EIp;
            }
            else
            {
                Ds = d1;
                Is = Math.PI / 64 * (Math.Pow(Ds, 4) - Math.Pow(d1, 4));
                EI = Es * Is + EIp;
            }
            return EI;
        }
        double dEI(double x)
        {
            double Ds, dEI;
            if (x < L1)
                dEI = 0;
            else if (x < L1 + L2)
            {
                Ds = D1 - ((D1 - d2) / L2) * (x - L1);
                dEI = -Es * Math.PI / 64 * Math.Pow(Ds, 3) * 4 * (D1 - d2) / L2;
            }
            else if (x < L1 + L2 + L3)
                dEI = 0;
            else
                dEI = 0;
            return dEI;
        }
    }
}



