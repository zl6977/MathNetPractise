using System;
using System.Collections.Generic;
using System.Text;
using MathNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.OdeSolvers;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.RootFinding;

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
            //先求解方程得到初始斜率的估计值，再进行打靶法迭代。--zzz
            Vector<double>[] res;
            Vector<double> y0;

            double s_guess, s_guess_pre;
            double fais, fais_pre;
            double dfai, ds;
            int count = 0;

            //计算20组s_guess和fais，然后样条插值得到连续函数，再通过解方程，得到使fais=0的初始s_guess
            int M = 50;
            double[] sLst = Vector<double>.Build.Dense(M, i => yb / M * i).ToArray();
            double[] faisLst = new double[M];
            count = 0;
            while (count < M)
            {
                y0 = Vector<double>.Build.DenseOfArray(new[] { ya, sLst[count] });
                // observer_pr ob_pr = this->write_targetFunc_end;
                res = RungeKutta.FourthOrder(y0, xa, xb, N, system);
                faisLst[count] = res[N - 1][0] - yb;
                count++;
            }
            //样条插值得到连续函数
            var cubicSpl = CubicSpline.InterpolateNatural(sLst, faisLst);

            /*如果初始值离解太远，牛顿法会不收敛。故采用Mathnet包中的RobustNewtonRaphson
            double s_cur = 0, s_next;
            count = 0;
            while (count < 1000) 
            {
                fais = cubicSpl.Interpolate(s_cur);
                dfaids = cubicSpl.Differentiate(s_cur);
                if (fais < 1e-5 && fais > -1e-5)
                {
                    break;
                }

                s_next = s_cur - fais / dfaids;
                s_cur = s_next;
                count += 1;
            }*/

            //解方程fais=0,得到初始斜率s_guess。该法先尝试牛顿法，如失败会采用二分法（bisection）。
            s_guess = RobustNewtonRaphson.FindRoot(cubicSpl.Interpolate, cubicSpl.Differentiate, 0, yb, 1e-5);

            //利用解得的s_guess，构造s_guess_pre，目的是求导数dfai/ds。
            if (s_guess == 0)
                s_guess_pre = 1e-2;
            else
                s_guess_pre = s_guess * 0.99;
            //求s_guess_pre对应的fais_pre，目的是求导数dfai/ds。
            y0 = Vector<double>.Build.DenseOfArray(new[] { ya, s_guess_pre });
            res = RungeKutta.FourthOrder(y0, xa, xb, N, system);
            fais_pre = res[N - 1][0] - yb;

            count = 0;
            while (count < 50)
            {
                y0 = Vector<double>.Build.DenseOfArray(new[] { ya, s_guess });
                res = RungeKutta.FourthOrder(y0, xa, xb, N, system);
                fais = res[N - 1][0] - yb;

                dfai = fais - fais_pre;
                ds = s_guess - s_guess_pre;
                if (fais < 1e-5 && fais > -1e-5)
                    break;

                fais_pre = fais;
                s_guess_pre = s_guess;
                s_guess = s_guess - fais * ds / dfai;
                count++;
            }

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



