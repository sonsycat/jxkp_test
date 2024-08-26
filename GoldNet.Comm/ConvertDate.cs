using System;
using System.Globalization;
namespace GoldNet.Comm
{
    /// <summary>
    /// ConvertDateTime 
    /// </summary>
    public  class ConvertDate
    {
        public  ConvertDate()
        { }
        //*******************************************************************//
        #region 根据当前日期确定当前是星期几
        public static string GetWeekDay(string strDate)
        {
            try
            {
                //需要判断的时间
                DateTime dTime = Convert.ToDateTime(strDate);
                return GetWeekDay(dTime);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static string GetWeekDay(DateTime dTime)
        {
            try
            {
                //确定星期几
                int index = (int)dTime.DayOfWeek;
                return GetWeekDay(index);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion
        //*******************************************************************//
        #region 获取当前年的最大周数
        public static int GetMaxWeekOfYear(int year)
        {
            try
            {
                DateTime tempDate = new DateTime(year, 12, 31);
                int tempDayOfWeek = (int)tempDate.DayOfWeek;
                if (tempDayOfWeek != 0)
                {
                    tempDate = tempDate.Date.AddDays(-tempDayOfWeek);
                }
                return GetWeekIndex(tempDate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static int GetMaxWeekOfYear(DateTime dTime)
        {
            try
            {
                return GetMaxWeekOfYear(dTime.Year);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        //*******************************************************************//
        #region 根据时间获取当前是第几周
        //如果12月31号与下一年的1月1好在同一个星期则算下一年的第一周
        public static int GetWeekIndex(DateTime dTime)
        {
            try
            {
                //需要判断的时间
                //DateTime dTime = Convert.ToDateTime(strDate);
                //确定此时间在一年中的位置
                int dayOfYear = dTime.DayOfYear;
                //DateTime tempDate = new DateTime(dTime.Year,1,6,calendar);
                //当年第一天
                DateTime tempDate = new DateTime(dTime.Year, 1, 1);
                //确定当年第一天
                int tempDayOfWeek = (int)tempDate.DayOfWeek;
                tempDayOfWeek = tempDayOfWeek == 0 ? 7 : tempDayOfWeek;
                //确定星期几
                int index = (int)dTime.DayOfWeek;
                index = index == 0 ? 7 : index;
                //当前周的范围
                DateTime retStartDay = dTime.AddDays(-(index - 1));
                DateTime retEndDay = dTime.AddDays(7 - index);
                //确定当前是第几周
                int weekIndex = (int)Math.Ceiling(((double)dayOfYear + tempDayOfWeek - 1) / 7);

                if (retStartDay.Year < retEndDay.Year)
                {
                    weekIndex = 1;
                }
                return weekIndex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //string retVal = retStartDay.ToString("yyyy/MM/dd") + "～" + retEndDay.ToString("yyyy/MM/dd");

        }
        public static int GetWeekIndex(string strDate)
        {
            try
            {
                //需要判断的时间
                DateTime dTime = Convert.ToDateTime(strDate);
                return GetWeekIndex(dTime);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        //*******************************************************************//
        #region 根据时间取周的日期范围
        public static string GetWeekRange(string strDate)
        {
            try
            {
                //需要判断的时间
                DateTime dTime = Convert.ToDateTime(strDate);
                return GetWeekRange(dTime);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetWeekRange(DateTime dTime)
        {
            try
            {
                int index = (int)dTime.DayOfWeek;
                index = index == 0 ? 7 : index;
                //当前周的范围
                DateTime retStartDay = dTime.AddDays(-(index - 1));
                DateTime retEndDay = dTime.AddDays(7 - index);
                return WeekRangeToString(retStartDay, retEndDay);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetWeekRange(int year, int weekIndex)
        {
            try
            {
                if (weekIndex < 1)
                {
                    throw new Exception("请输入大于0的整数");
                }

                int allDays = (weekIndex - 1) * 7;
                //确定当年第一天
                DateTime firstDate = new DateTime(year, 1, 1);
                int firstDayOfWeek = (int)firstDate.DayOfWeek;
                firstDayOfWeek = firstDayOfWeek == 0 ? 7 : firstDayOfWeek;
                //周开始日
                int startAddDays = allDays + (1 - firstDayOfWeek);
                DateTime weekRangeStart = firstDate.AddDays(startAddDays);
                //周结束日
                int endAddDays = allDays + (7 - firstDayOfWeek);
                DateTime weekRangeEnd = firstDate.AddDays(endAddDays);
                if (weekRangeStart.Year > year ||
                 (weekRangeStart.Year == year && weekRangeEnd.Year > year))
                {
                    throw new Exception("今年没有第" + weekIndex + "周。");
                }
                return WeekRangeToString(weekRangeStart, weekRangeEnd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetWeekRange(int weekIndex)
        {
            try
            {
                return GetWeekRange(DateTime.Now.Year, weekIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static string WeekRangeToString(DateTime weekRangeStart, DateTime weekRangeEnd)
        {
            string strWeekRangeStart = weekRangeStart.ToString("yyyy/MM/dd");
            string strWeekRangeend = weekRangeEnd.AddDays(0).ToString("yyyy/MM/dd");
            return strWeekRangeStart + "～" + strWeekRangeend;
        }
        #endregion
        //*******************************************************************//
        #region 转换星期的表示方法
        private static string GetWeekDay(int index)
        {
            string retVal = string.Empty;
            switch (index)
            {
                case 0:
                    {
                        retVal = "星期日";
                        break;
                    }
                case 1:
                    {
                        retVal = "星期一";
                        break;
                    }
                case 2:
                    {
                        retVal = "星期二";
                        break;
                    }
                case 3:
                    {
                        retVal = "星期三";
                        break;
                    }
                case 4:
                    {
                        retVal = "星期四";
                        break;
                    }
                case 5:
                    {
                        retVal = "星期五";
                        break;
                    }
                case 6:
                    {
                        retVal = "星期六";
                        break;
                    }
            }
            return retVal;
        }
        #endregion
    }
}