using System;
using System.Collections.Generic;
using System.Text;

namespace Goldnet.Comm.AppraisalMethod
{
    /// <summary>
    /// RSR评价方法
    /// </summary>
    public class RSR
    {
        public RSR()
        {
        }
        public double[] EvaluationOper(double[,] val, double[] high, double[] qz)
        {
            //趋势性变换
            double[,] a = TrendOper(val, high);
            //归一化处理
            double[,] k = UnitaryOper(a, high);
            //最优点
            //double[] good = getGood(k);
            ////最劣点
            //double[] bad = getBad(k);
            ////与最优点距离
            //double[] disgood = distanceGood(k, good, qz);
            ////与最劣点的距离
            //double[] disbad = distanceBad(k, bad, qz);
            //求得分
            double[] c = osculation(k, qz);
            return c;
        }
        /**
        * 进行趋势性变换
        */
        private double[,] TrendOper(double[,] val, double[] high)
        {
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
        //求值
        private double[] osculation(double[,] val, double[] qz)
        {
            double[] value = new double[val.GetLength(0)];
            for (int i = 0; i < val.GetLength(0); i++)
            {
                double addsum = 0;
                for (int j = 0; j < val.GetLength(1); j++)
                {
                    addsum = addsum + qz[j] * val[i, j];
                }
                // double b = getRound(addsum / val.GetLength(1));
                double b = getRound(addsum);
                addsum = 0;
                value[i] = b * 100;
            }
            //

            return value;
        }
        //获取小数精度
        private double getRound(double dSource)
        {
            double iRound;
            Decimal deSource = new Decimal(dSource);
            iRound = double.Parse(deSource.ToString("0.000"));
            return iRound;
        }
        ///// <summary>
        ///// 排序,求秩
        ///// </summary>
        ///// <param name="R"></param>
        //public void BubbleSort(double[,] R)
        //{
        //    int i, j, temp;
        //    bool exchange;
        //    for (int m = 0; m < R.GetLength(1); m++)
        //    {
        //        for (i = 0; i < R.GetLength(0); i++)
        //        {
        //            exchange = false;
        //            for (j = R.GetLength(0) - 2; j >= i; j--)
        //            {
        //                if (R[j + 1,m] < R[j,m])
        //                {
        //                    temp = R[j + 1,m];
        //                    R[j + 1,m] = R[j,m];
        //                    R[j,m] = temp;
        //                    exchange = true;
        //                }
        //            }
        //            if (!exchange)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    double[,] L = new double[R.GetLength(0),R.GetLength(1)];
        //    for (int n = 0; n < R.GetLength(1); n++)
        //    {
        //        for (int a = 0; a < R.GetLength(0); a++)
        //        {
        //            int x = 0;//相同数个数
        //            int y = 1;//秩
        //            for (int b = 0; b < R.GetLength(1); b++)
        //            {
        //                if (R[a,n] == R[b,n])
        //                {
        //                    x += 1;
        //                    y += b;
        //                }
        //            }
        //            L[a, n] = Math.Round(y / x,2);
        //        }
        //    }

        //}

    }
}
