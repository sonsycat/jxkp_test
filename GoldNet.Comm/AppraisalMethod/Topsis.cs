using System;
using System.Collections.Generic;
using System.Text;

namespace Goldnet.Comm.AppraisalMethod
{
    /// <summary>
    /// TOPSIS评价方法
    /// </summary>
    public class Topsis
    {
        /// <summary>
        /// 
        /// </summary>
        public Topsis()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="high"></param>
        /// <param name="qz"></param>
        /// <returns></returns>
        public double[] EvaluationOper(double[,] val, double[] high, double[] qz)
        {
            //趋势性变换
            double[,] a = TrendOper(val, high);
            //归一化处理
            double[,] k = UnitaryOper(a, high);
            //最优点
            double[] good = getGood(k);
            //最劣点
            double[] bad = getBad(k);
            //与最优点距离
            double[] disgood = distanceGood(k, good, qz);
            //与最劣点的距离
            double[] disbad = distanceBad(k, bad, qz);
            //求得分
            double[] c = osculation(disgood, disbad);
            return c;
        }
        /**
         * 进行趋势性变换
         */
        private double[,] TrendOper(double[,] val, double[] high)
        {
            //double addsum = 0;

            double[,] unitary = new double[val.GetLength(0), val.GetLength(1)];


            //循环矩阵列
            for (int j = 0; j < val.GetLength(1); j++)
            {
                //循环矩阵行
                for (int k = 0; k < val.GetLength(0); k++)
                {
                    //进行同趋势化处理
                    double b = 0;
                    if (high[j] == -11)
                    {
                        //低优绝对指标
                        if (val[k, j] == 0)
                        {
                            b = 0;
                        }
                        else
                        {
                            b = getRound(1 / val[k, j]) * 100;
                        }
                    }
                    if (high[j] == -10)
                    {
                        //低优相对指标
                        b = getRound(100 - val[k, j]);
                    }
                    if (high[j] == 1)
                    {
                        //高优指标
                        b = val[k, j];
                    }
                    unitary[k, j] = b;
                }
            }
            return unitary;
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
                    unitary[k, j] = b;
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
        //求相对接近度
        private double[] osculation(double[] disgood, double[] disbad)
        {
            double[] c = new double[disgood.Length];
            for (int i = 0; i < disgood.Length; i++)
            {
                if ((disgood[i] + disbad[i]) > 0)
                {
                    c[i] = getRound(disbad[i] / (disgood[i] + disbad[i])) * 100;
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
