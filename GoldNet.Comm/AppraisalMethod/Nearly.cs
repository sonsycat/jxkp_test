using System;
using System.Collections.Generic;
using System.Text;

namespace Goldnet.Comm.AppraisalMethod
{
    /// <summary>
    /// Nearly评价方法
    /// </summary>
    public class Nearly
    {
        public Nearly()
        {
        }
        public double[] EvaluationOper(double[,] val, double[] high, double[] qz)
        {
            //归一化处理
            double[,] k = UnitaryOper(val, high);

            //最优点
            double[] good = getGood(k);

            //最劣点
            double[] bad = getBad(k);

            //与最优点距离
            double[] disgood = distanceGood(k, good, qz);

            //与最劣点的距离
            double[] disbad = distanceBad(k, bad, qz);

            //获取最优点距离中的最小值
            double mingood = MinGood(disgood);

            //获取最劣点距离中的最大值
            double maxbad = MaxBad(disbad);

            //求得分
            double[] c = osculation(disgood, disbad, mingood, maxbad);
            return c;
        }
        /**
         * 指标归一化处理
         */
        private double[,] UnitaryOper(double[,] val, double[] high)
        {
            double addsum = 0;
            double[,] unitary = new double[val.GetLength(0), val.GetLength(1)];
            for (int k = 0; k < val.GetLength(0); k++)
            {
                for (int j = 0; j < val.GetLength(1); j++)
                {
                    for (int i = 0; i < val.GetLength(0); i++)
                    {
                        addsum = addsum + Math.Pow(val[i, j], 2);
                    }
                    double b;
                    if (Math.Sqrt(addsum) == 0)
                    {
                        b = 0;
                    }
                    else
                    {
                        b = getRound(val[k, j] / Math.Sqrt(addsum));
                    }
                    addsum = 0;
                    double higval = 1;
                    if (high[j] == -10 || high[j] == -11)
                    {
                        higval = -1;
                    }
                    else
                    {
                        higval = 1;
                    }
                    unitary[k, j] = b * higval; //将整个指标转化成正向指标
                }
            }
            return unitary;
        }
        /**
         * 获取有限方案中的最优点
         */
        private double[] getGood(double[,] val)
        {
            double max = -1;
            double[] good = new double[val.GetLength(1)];
            for (int j = 0; j < val.GetLength(1); j++)
            {
                for (int i = 0; i < val.GetLength(0); i++)
                {
                    if (max < val[i, j])
                    {
                        max = val[i, j];
                    }
                }
                good[j] = max;
                max = -1;
            }
            return good;
        }
        /**
         * 获取有限方案中的最劣点
         */
        private double[] getBad(double[,] val)
        {
            double min = 1;
            double[] bad = new double[val.GetLength(1)];
            for (int j = 0; j < val.GetLength(1); j++)
            {
                for (int i = 0; i < val.GetLength(0); i++)
                {
                    if (min > val[i, j])
                    {
                        min = val[i, j];
                    }
                }
                bad[j] = min;
                min = 1;
            }
            return bad;
        }

        /**
         * 获取与最优点的距离
         */
        private double[] distanceGood(double[,] val, double[] good, double[] qz)
        {
            double[] disgood = new double[val.GetLength(0)];
            double addsum = 0;
            for (int i = 0; i < val.GetLength(0); i++)
            {
                for (int j = 0; j < val.GetLength(1); j++)
                {
                    addsum = addsum + Math.Pow(qz[j] * (val[i, j] - good[j]), 2);
                }
                double b = getRound(Math.Sqrt(addsum));
                addsum = 0;
                disgood[i] = b;
            }
            return disgood;
        }
        /**
         * 获取与最劣点的距离
         */
        private double[] distanceBad(double[,] val, double[] bad, double[] qz)
        {
            double[] disbad = new double[val.GetLength(0)];
            double addsum = 0;
            for (int i = 0; i < val.GetLength(0); i++)
            {
                for (int j = 0; j < val.GetLength(1); j++)
                {
                    addsum = addsum + Math.Pow(qz[j] * (val[i, j] - bad[j]), 2);
                }
                double b = getRound(Math.Sqrt(addsum));
                addsum = 0;
                disbad[i] = b;
            }
            return disbad;
        }
        //求最优距离中的最小值
        private double MinGood(double[] good)
        {
            double min = 1;
            //double []good=new double [good.length];
            for (int i = 0; i < good.Length; i++)
            {
                if (min > good[i])
                {
                    min = good[i];
                }
            }
            return min;
        }
        //求最劣距离中的最大值
        private double MaxBad(double[] bad)
        {
            double max = -1;
            //double []good=new double [good.length];
            for (int i = 0; i < bad.Length; i++)
            {
                if (max < bad[i])
                {
                    max = bad[i];
                }
            }
            return max;
        }

        //求密切值
        private double[] osculation(double[] disgood, double[] disbad, double mingood, double maxbad)
        {
            double[] c = new double[disgood.Length];
            for (int i = 0; i < disgood.Length; i++)
            {
                if (mingood > 0 && maxbad > 0)
                {
                    c[i] = getRound((disgood[i] / mingood) - (disbad[i] / maxbad)) * 1;
                }
                else
                {
                    c[i] = 0;
                }
            }
            return c;
        }
        //获取小数精度
        private double getRound(double dSource)
        {
            double iRound;
            Decimal deSource = new Decimal(dSource);

            iRound = double.Parse(deSource.ToString("0.000"));
            return iRound;
        }
    }
}
