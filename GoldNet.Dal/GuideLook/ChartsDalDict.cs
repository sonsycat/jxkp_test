using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Data.OracleClient;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class ChartsDalDict
    {
        public ChartsDalDict()
		{}

        /// <summary>
        /// 查询病种分析图表
        /// </summary>
        /// <param name="name">病种名</param>
        /// <param name="medparam">医药成本</param>
        /// <param name="matparam">材料成本</param>
        /// <param name="type">DEPT_OPERATION_DICT：手术病种</param>
        /// <returns></returns>
        public DataSet getDiseaseAnalyseOperationCharts(string name,string medparam,string matparam,string type) 
        {
            StringBuilder str = new StringBuilder();

            string TableName = "";

            if (type.Equals("DEPT_OPERATION_DICT")) 
            {
                TableName = "OPERATION_ANALYSIS";
            }
            else 
            {
                TableName = "DISEASE_ANALYSIS";
            }

            str.AppendFormat("select JBPM,COSTS,(COSTS-MEDICINE_COST-MATERIAL_COSTS+MEDICINE_COST*{2}+MATERIAL_COSTS*{3}) BENEFIT,DAY_MARKING,SUM(COSTS) OVER( PARTITION BY jbpm) TOTALCOSTS , SUM(COSTS-MEDICINE_COST-MATERIAL_COSTS+MEDICINE_COST*0.15+MATERIAL_COSTS*0.05) OVER( PARTITION BY jbpm) TOTALBENEFIT from {0}.{4} where jbpm='{1}' order by to_number(day_marking)", DataUser.HISFACT, name, medparam, matparam, TableName);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
                
        }

        /// <summary>
        /// 查询在院天数
        /// </summary>
        /// <param name="name">病种名<</param>
        /// <param name="type">DEPT_OPERATION_DICT：手术病种</param>
        /// <returns></returns>
        public DataSet getDisdeaseInhospitaldates(string name,string type) 
        {
            StringBuilder str = new StringBuilder();

            string TableName = "";

            if (type.Equals("DEPT_OPERATION_DICT"))
            {
                TableName = "OPERATION_ANALYSIS_PRE3";
            }
            else
            {
                TableName = "DISEASE_ANALYSIS_PRE3";
            }

            str.AppendFormat("select * from {1}.{2} where jbpm='{0}'", name,DataUser.HISFACT,TableName);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询面饼型柱型图
        /// </summary>
        /// <param name="guidecode">指标</param>
        /// <param name="organ">组织类别</param>
        /// <param name="depttype">部门类别</param>
        /// <param name="deptattr">部门属性</param>
        /// <param name="years">年度</param>
        /// <param name="station">岗位</param>
        /// <param name="where">人员查询条件</param>
        /// <returns></returns>
        public DataSet FlashChartSql(string guidecode, string organ, string depttype, string deptattr, string years, string station, string where,string incount,string offset,string DeptPower)
        {
            string[] year = years.Split('-');

            DateTime TimeforGuide = Convert.ToDateTime(year[0] + "-" + year[1] + "-01").AddMonths(0);
            DateTime TimeforGuideTo = Convert.ToDateTime(year[2] + "-" + year[3] + "-01").AddMonths(1).AddDays(-1);

            //开始日期
            string fromDate = TimeforGuide.AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");
            //结束日期
            string ToDate = TimeforGuideTo.AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");

            OleDbParameter[] parameters = {
                                                                new OleDbParameter("tjrq",fromDate+","+ToDate),
                                                                new OleDbParameter("guidecode", guidecode),
                                                                new OleDbParameter("incount", incount)
                                                            };
            OracleOledbBase.RunProcedure("hospitalsys.GUIDE_VALUE_ADD_SUM", parameters);

            StringBuilder str = new StringBuilder();
            string power = "";
            switch (organ)
            {

                case "01":
//                    str.AppendFormat(@"  SELECT '院' AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.GUIDE_NAME,UNIT_CODE,GUIDE_CODE FROM (
//                                            SELECT   A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
//                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE,A.UNIT_CODE
//                                                FROM {0}.guide_value_SUM A 
//                                                LEFT JOIN HOSPITALSYS.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
//                                               WHERE A.GUIDE_TYPE = 'Y'
//                                                 AND A.UNIT_CODE = '00'
//                                                 AND A.GUIDE_CODE = '{3}'
//                                                 AND A.STARTDATE >=  ADD_MONTHS(to_date({3},'YYYY-mm-dd'),{5})
//                                                 AND A.ENDDATE <= ADD_MONTHS(to_date({4},'YYYY-mm-dd'),{5})
//                                            GROUP BY A.GUIDE_CODE,UNIT_CODE
//                                         )T1 ", DataUser.HOSPITALSYS, fromDate, ToDate, guidecode,offset);
                    break;

                case "02":
                    power = DeptPower == "" ? "" : "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    str.AppendFormat(@" SELECT T2.DEPT_NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.GUIDE_NAME,T2.DEPT_CODE AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.ACCOUNT_DEPT_CODE AS UNIT_CODE
                                                FROM {0}.guide_value_SUM A 
                                                LEFT JOIN {1}.SYS_DEPT_DICT B ON A.UNIT_CODE = B.DEPT_CODE
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'K'
                                                 AND A.GUIDE_CODE = '{2}'
                                                 AND TO_CHAR(A.STARTDATE) =  TO_CHAR(to_date({3},'YYYY-mm-dd'))
                                                 AND TO_CHAR(A.ENDDATE) = TO_CHAR(to_date({4},'YYYY-mm-dd'))
                                            GROUP BY A.GUIDE_CODE, B.ACCOUNT_DEPT_CODE
                                         )T1,{1}.SYS_DEPT_DICT T2
                                        WHERE T1.UNIT_CODE = T2.DEPT_CODE AND T2.ATTR='是' {6}", DataUser.HOSPITALSYS, DataUser.COMM, guidecode, fromDate, ToDate,offset,power);
                    if (station != "")
                    {
                        str.AppendFormat("  AND T2.DEPT_CODE in ({0}) ", station);
                    }
                    break;

                case "03":
                    if (DeptPower == "'-1'")
                    {
                        power = "AND T2.STAFF_ID = '" + incount + "'";
                    }
                    else if (DeptPower != "")
                    {
                        power = "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    }
                    str.AppendFormat(@"  SELECT T2.NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.GUIDE_NAME,T2.STAFF_ID AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.STAFF_ID AS UNIT_CODE
                                                FROM {0}.guide_value_SUM A 
                                                LEFT JOIN {1}.NEW_STAFF_INFO B ON A.UNIT_CODE = B.STAFF_ID
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'R'
                                                 AND A.GUIDE_CODE = '{2}'
                                                 AND TO_CHAR(A.STARTDATE) =  TO_CHAR(to_date({3},'YYYY-mm-dd'))
                                                 AND TO_CHAR(A.ENDDATE) = TO_CHAR(to_date({4},'YYYY-mm-dd'))
                                              GROUP BY A.GUIDE_CODE, B.STAFF_ID
                                         )T1,{1}.NEW_STAFF_INFO T2
                                        WHERE T1.UNIT_CODE = T2.STAFF_ID AND T2.ADD_MARK='1' {6}", DataUser.HOSPITALSYS, DataUser.RLZY, guidecode, fromDate, ToDate,offset,power);
                    if (!station.Equals(""))
                    {
                        str.AppendFormat("  AND  T2.DEPT_CODE in ( ");
                        str.AppendFormat("          SELECT DEPT_CODE FROM {0}.SYS_STATION_BASIC_INFORMATION ", DataUser.COMM);
                        str.AppendFormat("          WHERE STATION_YEAR = '{1}' AND STATION_DICT_CODE = {0}   )", station, DateTime.Now.Year.ToString());
                    }
                    if (!where.Equals(""))
                    {
                        str.AppendFormat(where);
                    }
                    break;
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询柱型图
        /// </summary>
        /// <param name="guidecode"></param>
        /// <param name="organ"></param>
        /// <param name="depttype"></param>
        /// <param name="deptattr"></param>
        /// <param name="years"></param>
        /// <param name="station"></param>
        /// <param name="where"></param>
        /// <param name="incount"></param>
        /// <param name="offset"></param>
        /// <param name="DeptPower"></param>
        /// <returns></returns>
        public DataSet FlashChartSqlBar(string guidecode, string organ, string depttype, string deptattr, string years, string station, string where, string incount, string offset, string DeptPower)
        {
            string[] year = years.Split('-');

            DateTime TimeforGuide = Convert.ToDateTime(year[0] + "-" + year[1] + "-01").AddMonths(0);
            DateTime TimeforGuideTo = Convert.ToDateTime(year[2] + "-" + year[3] + "-01").AddMonths(1).AddDays(-1);

            string ToDate = "";
            string TQToDate = "";
            string fromDate = TimeforGuide.AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");
            string TQfromDate = TimeforGuide.AddMonths(-12).AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");

            //if (years.Equals(DateTime.Today.Year.ToString()))
            //{
            //    ToDate = DateTime.Today.ToString("yyyyMMdd");
            //    TQToDate = DateTime.Today.AddMonths(-12).ToString("yyyyMMdd");
            //}
            //else
            //{
            ToDate = TimeforGuideTo.AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");
            TQToDate = TimeforGuideTo.AddMonths(-12).AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");
            //}

            OleDbParameter[] parameters = {
                                                new OleDbParameter("tjrq",fromDate+","+ToDate),
                                                new OleDbParameter("tqrq",TQfromDate+","+TQToDate),
                                                new OleDbParameter("guidecode", guidecode),
                                                new OleDbParameter("incount", incount)
                                          };
            OracleOledbBase.RunProcedure("hospitalsys.GUIDE_VALUE_ADD_SUM_PRC", parameters);

            StringBuilder str = new StringBuilder();
            string power = "";
            switch (organ)
            {

                case "01":
                    //                    str.AppendFormat(@"  SELECT '院' AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.GUIDE_NAME,UNIT_CODE,GUIDE_CODE FROM (
                    //                                            SELECT   A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                    //                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE,A.UNIT_CODE
                    //                                                FROM {0}.guide_value_SUM A 
                    //                                                LEFT JOIN HOSPITALSYS.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                    //                                               WHERE A.GUIDE_TYPE = 'Y'
                    //                                                 AND A.UNIT_CODE = '00'
                    //                                                 AND A.GUIDE_CODE = '{3}'
                    //                                                 AND A.STARTDATE >=  ADD_MONTHS(to_date({3},'YYYY-mm-dd'),{5})
                    //                                                 AND A.ENDDATE <= ADD_MONTHS(to_date({4},'YYYY-mm-dd'),{5})
                    //                                            GROUP BY A.GUIDE_CODE,UNIT_CODE
                    //                                         )T1 ", DataUser.HOSPITALSYS, fromDate, ToDate, guidecode,offset);
                    break;

                case "02":
                    power = DeptPower == "" ? "" : "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    str.AppendFormat(@" SELECT T2.DEPT_NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.TQ_GUIDE_VALUE,T1.GUIDE_NAME,T2.DEPT_CODE AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.GUIDE_CODE,
                                                     MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (CASE WHEN TO_CHAR(STARTDATE,'yyyy')=TO_CHAR(to_date({3},'YYYY-mm-dd'),'yyyy') then GUIDE_VALUE else 0 end) AS GUIDE_VALUE, 
                                                     SUM (CASE WHEN TO_CHAR(STARTDATE,'yyyy')=to_char(add_months(to_date({3},'YYYY-mm-dd'),-12),'yyyy') then GUIDE_VALUE else 0 end) AS TQ_GUIDE_VALUE,
                                                     B.ACCOUNT_DEPT_CODE AS UNIT_CODE
                                                FROM {0}.guide_value_SUM A 
                                                LEFT JOIN {1}.SYS_DEPT_DICT B ON A.UNIT_CODE = B.DEPT_CODE
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'K'
                                                 AND A.GUIDE_CODE = '{2}'
                                                 AND A.COUNTING='{7}'
                                                 AND ((TO_CHAR(A.STARTDATE) =  TO_CHAR(to_date({3},'YYYY-mm-dd'))
                                                 AND TO_CHAR(A.ENDDATE) = TO_CHAR(to_date({4},'YYYY-mm-dd')))
                                                 OR (TO_CHAR(A.STARTDATE) =  TO_CHAR(add_months(to_date({3},'YYYY-mm-dd'),-12))
                                                 AND TO_CHAR(A.ENDDATE) = TO_CHAR(add_months(to_date({4},'YYYY-mm-dd'),-12))))
                                            GROUP BY A.GUIDE_CODE, B.ACCOUNT_DEPT_CODE
                                         )T1,{1}.SYS_DEPT_DICT T2
                                        WHERE T1.UNIT_CODE = T2.DEPT_CODE AND T2.ATTR='是' {6}", DataUser.HOSPITALSYS, DataUser.COMM, guidecode, fromDate, ToDate, offset, power, incount);
                    if (station != "")
                    {
                        str.AppendFormat("  AND T2.DEPT_CODE in ({0}) ", station);
                    }

                    str.Append("  ORDER BY T1.GUIDE_VALUE DESC ");
                    break;

                case "03":
                    if (DeptPower == "'-1'")
                    {
                        power = "AND T2.STAFF_ID = '" + incount + "'";
                    }
                    else if (DeptPower != "")
                    {
                        power = "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    }
                    str.AppendFormat(@"  SELECT T2.NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.TQ_GUIDE_VALUE,T1.GUIDE_NAME,T2.STAFF_ID AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.GUIDE_CODE,
                                                     MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (CASE WHEN TO_CHAR(STARTDATE,'yyyy')=TO_CHAR(to_date({3},'YYYY-mm-dd'),'yyyy') then GUIDE_VALUE else 0 end) AS GUIDE_VALUE, 
                                                     SUM (CASE WHEN TO_CHAR(STARTDATE,'yyyy')=to_char(add_months(to_date({3},'YYYY-mm-dd'),-12),'yyyy') then GUIDE_VALUE else 0 end) AS TQ_GUIDE_VALUE,
                                                     B.STAFF_ID AS UNIT_CODE
                                                FROM {0}.guide_value_SUM A 
                                                LEFT JOIN {1}.NEW_STAFF_INFO B ON A.UNIT_CODE = B.STAFF_ID
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'R'
                                                 AND A.GUIDE_CODE = '{2}'
                                                 AND A.COUNTING='{7}'
                                                 AND ((TO_CHAR(A.STARTDATE) =  TO_CHAR(to_date({3},'YYYY-mm-dd'))
                                                 AND TO_CHAR(A.ENDDATE) = TO_CHAR(to_date({4},'YYYY-mm-dd')))
                                                 OR (TO_CHAR(A.STARTDATE) =  TO_CHAR(add_months(to_date({3},'YYYY-mm-dd'),-12))
                                                 AND TO_CHAR(A.ENDDATE) = TO_CHAR(add_months(to_date({4},'YYYY-mm-dd'),-12))))
                                              GROUP BY A.GUIDE_CODE, B.STAFF_ID
                                         )T1,{1}.NEW_STAFF_INFO T2
                                        WHERE T1.UNIT_CODE = T2.STAFF_ID AND T2.ADD_MARK='1' {6}", DataUser.HOSPITALSYS, DataUser.RLZY, guidecode, fromDate, ToDate, offset, power, incount);
                    if (!station.Equals(""))
                    {
                        str.AppendFormat("  AND  T2.DEPT_CODE in ( ");
                        str.AppendFormat("          SELECT DEPT_CODE FROM {0}.SYS_STATION_BASIC_INFORMATION ", DataUser.COMM);
                        str.AppendFormat("          WHERE STATION_YEAR = '{1}' AND STATION_DICT_CODE = {0}   )", station, DateTime.Now.Year.ToString());
                    }
                    if (!where.Equals(""))
                    {
                        str.AppendFormat(where);
                    }
                    break;
            }

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        /// <summary>
        /// 查询弹出页面趋势图
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="organ">组织类别</param>
        /// <param name="guidecode">指标</param>
        /// <returns></returns>
        public DataSet GetPopUpChartsLine(string year, string organ, string guidecode, string unit_code,string offsetDate,string DeptPower,string StaffPower)
        {
            StringBuilder str = new StringBuilder();

            string[] years = year.Split('-');

            DateTime TimeforGuide = Convert.ToDateTime(years[0] + "-" + years[1] + "-01").AddMonths(0);
            DateTime TimeforGuideTo = Convert.ToDateTime(years[2] + "-" + years[3] + "-01").AddMonths(1).AddDays(-1);

            string fromDate = TimeforGuide.ToString("yyyyMM");
            string ToDate = TimeforGuideTo.ToString("yyyyMM");

            string ViewDate = (Convert.ToInt32(offsetDate) * -1).ToString();

            if (guidecode.IndexOf("1") == 0) { organ = "01"; }
            if (guidecode.IndexOf("2") == 0) { organ = "02"; }
            if (guidecode.IndexOf("3") == 0) { organ = "03"; }
            string power = "";
            switch (organ)
            {
                case "01":
                    str.AppendFormat(@" SELECT T2.DEPT_NAME AS NAME ,T1.GUIDE_VALUE ,TO_CHAR(ADD_MONTHS(TO_DATE(T1.TJYF,'YYYYMM'),{6}),'YYYYMM') as TJYF,T1.GUIDE_NAME,T2.DEPT_CODE AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.TJYF, A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.ACCOUNT_DEPT_CODE AS UNIT_CODE
                                                FROM {0}.GUIDE_VALUE A 
                                                LEFT JOIN {1}.SYS_DEPT_DICT B ON A.UNIT_CODE = B.DEPT_CODE
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'K'
                                                 AND A.GUIDE_CODE = (SELECT DEPT_GUIDE_CODE FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER T1 WHERE T1.HOSPITAL_GUIDE_CODE='{2}')
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') >= ADD_MONTHS(TO_DATE('{3}','YYYYMM'),{5})
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') <= ADD_MONTHS(TO_DATE('{4}','YYYYMM'),{5})
                                            GROUP BY A.GUIDE_CODE, A.TJYF, B.ACCOUNT_DEPT_CODE
                                         )T1,COMM.SYS_DEPT_DICT T2
                                        WHERE T1.UNIT_CODE = T2.DEPT_CODE AND T2.ATTR='是'"
                                        , DataUser.HOSPITALSYS, DataUser.COMM, guidecode, fromDate, ToDate, offsetDate, ViewDate);
                    break;

                case "02":
                    power = DeptPower == "" ? "" : "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    str.AppendFormat(@"  SELECT T2.NAME AS NAME ,T1.GUIDE_VALUE ,TO_CHAR(ADD_MONTHS(TO_DATE(T1.TJYF,'YYYYMM'),{7}),'YYYYMM') as TJYF,T1.GUIDE_NAME,T2.STAFF_ID AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.TJYF, A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.STAFF_ID AS UNIT_CODE
                                                FROM {0}.GUIDE_VALUE A 
                                                LEFT JOIN {1}.NEW_STAFF_INFO B ON A.UNIT_CODE = B.STAFF_ID
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'R'
                                                 AND A.GUIDE_CODE = (SELECT MEMBER_GUIDE_CODE FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER T1 WHERE T1.DEPT_GUIDE_CODE='{2}')
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') >= ADD_MONTHS(TO_DATE('{3}','YYYYMM'),{6})
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') <= ADD_MONTHS(TO_DATE('{4}','YYYYMM'),{6})
                                            GROUP BY A.GUIDE_CODE, A.TJYF, B.STAFF_ID
                                         )T1,{1}.NEW_STAFF_INFO T2
                                        WHERE T1.UNIT_CODE = T2.STAFF_ID AND T2.ADD_MARK='1' AND DEPT_CODE = '{5}' {8}", DataUser.HOSPITALSYS, DataUser.RLZY, guidecode, fromDate, ToDate, unit_code, offsetDate, ViewDate,power);
                    break;

                case "03":
                    if (DeptPower == "'-1'")
                    {
                        power = "AND T2.STAFF_ID = '" + StaffPower + "'";
                    }
                    else if (DeptPower != "")
                    {
                        power = "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    }
                    str.AppendFormat(@"  SELECT T2.NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,TO_CHAR(ADD_MONTHS(TO_DATE(T1.TJYF,'YYYYMM'),{6}),'YYYYMM') as TJYF,T1.GUIDE_NAME,T2.STAFF_ID AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.TJYF, A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.STAFF_ID AS UNIT_CODE
                                                FROM {0}.GUIDE_VALUE A 
                                                LEFT JOIN {1}.NEW_STAFF_INFO B ON A.UNIT_CODE = B.STAFF_ID
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'R'
                                                 AND A.GUIDE_CODE = '{2}'
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') = ADD_MONTHS(TO_DATE('{3}','YYYYMM'),{5})
                                            GROUP BY A.GUIDE_CODE, A.TJYF, B.STAFF_ID
                                         )T1,{1}.NEW_STAFF_INFO T2
                                        WHERE T1.UNIT_CODE = T2.STAFF_ID AND T2.ADD_MARK='1' AND T2.STAFF_ID = '{4}' {7}", DataUser.HOSPITALSYS, DataUser.RLZY, guidecode, fromDate,unit_code,offsetDate,ViewDate,power);
                    break;
                
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 查询弹出页面饼型柱型图
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="organ">组织类别</param>
        /// <param name="guidecode">指标</param>
        /// <param name="unit_code">人员，部门ID</param>
        /// <returns></returns>
        public DataSet GetPopUpChartsPieAndBar(string year, string organ, string guidecode, string unit_code, string incount,string offset,string deptPower) 
        {
            string[] years = year.Split('-');

            DateTime TimeforGuide;
            DateTime TimeforGuideTo;

            if (years.Length > 1)
            {
                TimeforGuide = Convert.ToDateTime(years[0] + "-" + years[1] + "-01").AddMonths(0);
                TimeforGuideTo = Convert.ToDateTime(years[2] + "-" + years[3] + "-01").AddMonths(1).AddDays(-1);
            }
            else
            {
                TimeforGuide = Convert.ToDateTime(years[0].Substring(0, 4) + "-" + years[0].Substring(4, 2) + "-01").AddMonths(0);
                TimeforGuideTo = Convert.ToDateTime(years[0].Substring(0, 4) + "-" + years[0].Substring(4, 2) + "-01").AddMonths(1).AddDays(-1);
            }

            StringBuilder str = new StringBuilder();

            string fromDate = TimeforGuide.ToString("yyyyMM");
            string ToDate = TimeforGuideTo.ToString("yyyyMM");

            string ViewDate = (Convert.ToInt32(offset) * -1).ToString();
            if (guidecode.IndexOf("1") == 0) { organ = "01"; }
            if (guidecode.IndexOf("2") == 0) { organ = "02"; }
            if (guidecode.IndexOf("3") == 0) { organ = "03"; }
            GuideDalDict gdal = new GuideDalDict();

            if (organ == "01")
            {
                //院权限
                //TimeforGuide = Convert.ToDateTime(year.Substring(0,4) + "-01-01").AddMonths(0);
                fromDate = TimeforGuide.AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");
                ToDate = TimeforGuideTo.AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");

                DataTable dt = gdal.getGuideByHosDeptGuideCode(guidecode, organ).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    guidecode = dt.Rows[0][0].ToString();
                }
                else 
                {
                    DataSet l_ds = new DataSet();
                    return l_ds;
                }
                OleDbParameter[] parameters = {
                                                new OleDbParameter("tjrq",fromDate+","+ToDate),
                                                new OleDbParameter("guidecode", guidecode),
                                                new OleDbParameter("incount", incount)
                                              };
                OracleOledbBase.RunProcedure("hospitalsys.GUIDE_VALUE_ADD_SUM", parameters);
            }
            else if (organ == "02")
            {
                //科权限
                DataTable dt = gdal.getGuideByHosDeptGuideCode(guidecode, organ).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    guidecode = dt.Rows[0][0].ToString();
                }
                else
                {
                    DataSet l_ds = new DataSet();
                    l_ds.Tables.Add(new DataTable());
                    return l_ds;
                }
                //fromDate = year;
            } 
            else if (organ == "04") 
            {
                //人员权限
                //fromDate = year;
            }

            string power = "";

            switch (organ)
            {
                case "01":
                    //院当月柱，饼型图
                    str.AppendFormat(@" SELECT T2.DEPT_NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.GUIDE_NAME,T2.DEPT_CODE AS UNIT_CODE,GUIDE_CODE FROM (
                                                                SELECT   A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                                         SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.ACCOUNT_DEPT_CODE AS UNIT_CODE
                                                                    FROM {0}.guide_value_SUM A 
                                                                    LEFT JOIN {1}.SYS_DEPT_DICT B ON A.UNIT_CODE = B.DEPT_CODE
                                                                    LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                                                   WHERE A.GUIDE_TYPE = 'K'
                                                                     AND A.GUIDE_CODE = '{2}'
                                                                     AND TO_CHAR(A.STARTDATE) =  TO_CHAR(to_date({3},'YYYY-mm-dd'))
                                                                     AND TO_CHAR(A.ENDDATE) = TO_CHAR(to_date({4},'YYYY-mm-dd'))
                                                                GROUP BY A.GUIDE_CODE, B.ACCOUNT_DEPT_CODE
                                                             )T1,{1}.SYS_DEPT_DICT T2
                                                            WHERE T1.UNIT_CODE = T2.DEPT_CODE AND T2.ATTR='是'", DataUser.HOSPITALSYS, DataUser.COMM, guidecode, fromDate, ToDate,offset);


                    break;
                case "02":
                    //科当月柱，饼型图
                    power = deptPower == "" ? "" : "AND T2.DEPT_CODE IN (" + deptPower + ")";
                    str.AppendFormat(@"  SELECT T2.NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,TO_CHAR(ADD_MONTHS(TO_DATE(T1.TJYF,'YYYYMM'),{6}),'YYYYMM') AS TJYF,T1.GUIDE_NAME,T2.STAFF_ID AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.TJYF, A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.STAFF_ID AS UNIT_CODE
                                                FROM {0}.GUIDE_VALUE A 
                                                LEFT JOIN {1}.NEW_STAFF_INFO B ON A.UNIT_CODE = B.STAFF_ID
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'R'
                                                 AND A.GUIDE_CODE = '{2}'
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') = ADD_MONTHS(TO_DATE('{3}','YYYYMM'),{5})
                                            GROUP BY A.GUIDE_CODE, A.TJYF, B.STAFF_ID
                                         )T1,{1}.NEW_STAFF_INFO T2
                                        WHERE T1.UNIT_CODE = T2.STAFF_ID AND T2.ADD_MARK='1'  AND T2.DEPT_CODE = '{4}' {7}", DataUser.HOSPITALSYS, DataUser.RLZY, guidecode, fromDate, unit_code, offset, ViewDate,power);
                    break;

                case "04":
                    //人员当月柱，饼型图
                    if (deptPower == "'-1'")
                    {
                        power = "AND T2.STAFF_ID = '" + incount + "'";
                    }
                    else if (deptPower != "")
                    {
                        power = "AND T2.DEPT_CODE IN (" + deptPower + ")";
                    }
                    str.AppendFormat(@"  SELECT T2.NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,TO_CHAR(ADD_MONTHS(TO_DATE(T1.TJYF,'YYYYMM'),{6}),'YYYYMM') AS TJYF,T1.GUIDE_NAME,T2.STAFF_ID AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.TJYF, A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.STAFF_ID AS UNIT_CODE
                                                FROM {0}.GUIDE_VALUE A 
                                                LEFT JOIN {1}.NEW_STAFF_INFO B ON A.UNIT_CODE = B.STAFF_ID
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'R'
                                                 AND A.GUIDE_CODE = '{2}'
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') = ADD_MONTHS(TO_DATE('{3}','YYYYMM'),{5})
                                            GROUP BY A.GUIDE_CODE, A.TJYF, B.STAFF_ID
                                         )T1,{1}.NEW_STAFF_INFO T2
                                        WHERE T1.UNIT_CODE = T2.STAFF_ID AND T2.ADD_MARK='1'  AND T2.STAFF_ID = '{4}' {7}", DataUser.HOSPITALSYS, DataUser.RLZY, guidecode, fromDate, unit_code, offset, ViewDate,power);
                    break;
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 查询趋势图
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="organ">组织类别</param>
        /// <param name="guidecode">指标</param>
        /// <param name="deptattr">部门属性</param>
        /// <param name="station">岗位</param>
        /// <param name="where">人员条件</param>
        /// <param name="depttype">部门类别</param>
        /// <returns></returns>
        public DataSet GetChartsLine(string year, string organ, string guidecode, string deptattr, string station, string where, string depttype,string offsetDate,string DeptPower,string staffPower)
        {
            string[] years = year.Split('-');

            StringBuilder str = new StringBuilder();
            DateTime TimeforGuide = Convert.ToDateTime(years[0] + "-"+years[1]+"-01").AddMonths(0);
            DateTime TimeforGuideTo = Convert.ToDateTime(years[2] + "-" + years[3] + "-01").AddMonths(0);
            //开始日期
            string fromDate = TimeforGuide.ToString("yyyyMM");
            //结束日期
            string ToDate = TimeforGuideTo.ToString("yyyyMM");
            string ViewDate = (Convert.ToInt32(offsetDate) * -1).ToString();

            string power = "";
            switch (organ) 
            {
                case "03":
                    if (DeptPower == "'-1'") 
                    {
                        power = "AND T2.STAFF_ID = '" + staffPower + "'";
                    }
                    else if (DeptPower != "") 
                    {
                        power = "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    }
                    str.AppendFormat(@"  SELECT T2.NAME AS NAME ,T1.GUIDE_VALUE ,TO_CHAR(ADD_MONTHS(TO_DATE(T1.TJYF,'YYYYMM'),{6}),'YYYYMM') as TJYF,T1.GUIDE_NAME,T2.STAFF_ID AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.TJYF, A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.STAFF_ID AS UNIT_CODE
                                                FROM {0}.GUIDE_VALUE_PER A 
                                                LEFT JOIN {1}.NEW_STAFF_INFO B ON A.UNIT_CODE = B.STAFF_ID
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE 
                                                 A.GUIDE_CODE = '{2}'
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') >= ADD_MONTHS(TO_DATE('{3}','YYYYMM'),{5})
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') <= ADD_MONTHS(TO_DATE('{4}','YYYYMM'),{5})
                                            GROUP BY A.GUIDE_CODE,  A.TJYF, B.STAFF_ID
                                         )T1,{1}.NEW_STAFF_INFO T2
                                        WHERE T1.UNIT_CODE = T2.STAFF_ID AND T2.ADD_MARK='1' {7}", DataUser.HOSPITALSYS, DataUser.RLZY, guidecode, fromDate, ToDate, offsetDate, ViewDate, power);
                    if (!station.Equals(""))
                    {
                        str.AppendFormat("  and  T2.DEPT_CODE in ( ");
                        str.AppendFormat("          SELECT DEPT_CODE FROM {0}.SYS_STATION_BASIC_INFORMATION ", DataUser.COMM);
                        str.AppendFormat("          WHERE STATION_YEAR = '{1}' AND STATION_DICT_CODE = {0}   )",station,year);
                    }
                    if (!where.Equals(""))
                    {
                        str.AppendFormat(where);
                    }
                    break;
                case "02":

                    power = DeptPower == "" ? "" : "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    
                    str.AppendFormat(@" SELECT T2.DEPT_NAME AS NAME ,T1.GUIDE_VALUE ,TO_CHAR(ADD_MONTHS(TO_DATE(T1.TJYF,'YYYYMM'),{6}),'YYYYMM') as TJYF,T1.GUIDE_NAME,T2.DEPT_CODE AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.TJYF, A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.ACCOUNT_DEPT_CODE AS UNIT_CODE
                                                FROM {0}.GUIDE_VALUE A 
                                                LEFT JOIN {1}.SYS_DEPT_DICT B ON A.UNIT_CODE = B.DEPT_CODE
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'K'
                                                 AND A.GUIDE_CODE = '{2}'
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') >= ADD_MONTHS(TO_DATE('{3}','YYYYMM'),{5})
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') <= ADD_MONTHS(TO_DATE('{4}','YYYYMM'),{5})
                                            GROUP BY A.GUIDE_CODE,  A.TJYF, B.ACCOUNT_DEPT_CODE
                                         )T1,{1}.SYS_DEPT_DICT T2
                                        WHERE T1.UNIT_CODE = T2.DEPT_CODE AND T2.ATTR='是' {7}"
                        , DataUser.HOSPITALSYS, DataUser.COMM, guidecode, fromDate, ToDate, offsetDate, ViewDate,power);
                    if (station != "")
                    {
                        str.AppendFormat("  AND T2.DEPT_CODE in ({0}) ", station);
                    }
                    break;
                case "01":
                    str.AppendFormat(@"  SELECT '院' AS NAME ,T1.GUIDE_VALUE ,TO_CHAR(ADD_MONTHS(TO_DATE(T1.TJYF,'YYYYMM'),{5}),'YYYYMM') as TJYF,T1.GUIDE_NAME,UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.TJYF, A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE,A.UNIT_CODE
                                                FROM {0}.GUIDE_VALUE A 
                                                LEFT JOIN HOSPITALSYS.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'Y'
                                                 AND A.UNIT_CODE = '00'
                                                 AND A.GUIDE_CODE = '{3}'
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') >= ADD_MONTHS(TO_DATE('{1}','YYYYMM'),{4})
                                                 AND TO_DATE(A.TJYF, 'YYYYMM') <= ADD_MONTHS(TO_DATE('{2}','YYYYMM'),{4})
                                            GROUP BY A.GUIDE_CODE, A.TJYF,UNIT_CODE
                                         )T1 ", DataUser.HOSPITALSYS, fromDate, ToDate, guidecode, offsetDate, ViewDate);
                    break;
            }

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GuideCode"></param>
        /// <param name="DeptCode"></param>
        /// <param name="power"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataSet getGuideByDeptChart(string GuideCode, string DeptCode, string power, string year)
        {
            StringBuilder str = new StringBuilder();
            DateTime TimeforGuide = Convert.ToDateTime(year + "-01-01").AddMonths(0);
            string fromDate = TimeforGuide.ToString("yyyyMM");
            string ToDate = TimeforGuide.AddMonths(12).ToString("yyyyMM");
            string ViewDate = (Convert.ToInt32(GetConfig.GetConfigString("dateoffset")) * -1).ToString();
            //power = power == "" ? "" : "AND T2.DEPT_CODE IN (" + power + ")";
            str.AppendFormat(@" SELECT T2.DEPT_NAME AS NAME ,T1.GUIDE_VALUE ,TO_CHAR(ADD_MONTHS(TO_DATE(T1.TJYF,'YYYYMM'),{6}),'YYYYMM') AS TJYF,T1.GUIDE_NAME,T2.DEPT_CODE AS UNIT_CODE,GUIDE_CODE FROM (
                                SELECT   A.TJYF, A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                         SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.ACCOUNT_DEPT_CODE AS UNIT_CODE
                                    FROM {0}.GUIDE_VALUE A 
                                    LEFT JOIN {1}.SYS_DEPT_DICT B ON A.UNIT_CODE = B.DEPT_CODE
                                    LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                   WHERE A.GUIDE_TYPE = 'K'
                                     AND A.GUIDE_CODE IN ({2})
                                     AND TO_DATE(A.TJYF, 'YYYYMM') >= ADD_MONTHS(TO_DATE('{4}','YYYYMM'),{7})
                                     AND TO_DATE(A.TJYF, 'YYYYMM') <= ADD_MONTHS(TO_DATE('{5}','YYYYMM'),{7})
                                GROUP BY A.GUIDE_CODE,  A.TJYF, B.ACCOUNT_DEPT_CODE
                             )T1,{1}.SYS_DEPT_DICT T2
                            WHERE T1.UNIT_CODE = T2.DEPT_CODE AND T2.ATTR='是' AND  T2.DEPT_CODE = '{3}'", DataUser.HOSPITALSYS, DataUser.COMM, GuideCode, DeptCode, fromDate, ToDate,ViewDate, GetConfig.GetConfigString("dateoffset"));
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        /// <summary>
        /// 查询面饼型柱型图
        /// </summary>
        /// <param name="guidecode">指标</param>
        /// <param name="organ">组织类别</param>
        /// <param name="depttype">部门类别</param>
        /// <param name="deptattr">部门属性</param>
        /// <param name="years">年度</param>
        /// <param name="station">岗位</param>
        /// <param name="where">人员查询条件</param>
        /// <returns></returns>
        public DataSet FlashChartSql(string guidecode, string organ, string depttype, string deptattr, string years, string station, string where, string incount, string offset, string DeptPower,string FromMonth,string ToMonth)
        {
            //DateTime TimeforGuide = Convert.ToDateTime(years + "-01-01").AddMonths(0);
            string FromMMonth = FromMonth.Length == 1 ? "0" + FromMonth : FromMonth;

            string fromDate = Convert.ToDateTime(years +"-" +FromMMonth + "-01").AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");

            string ToDateNoOffset = years + "-"+(ToMonth.Length == 1 ? "0" + ToMonth : ToMonth) +"-"+ DateTime.DaysInMonth(Convert.ToInt32(years), Convert.ToInt32(ToMonth)).ToString();
            string ToDate = Convert.ToDateTime(ToDateNoOffset).AddMonths(Convert.ToInt32(offset)).ToString("yyyyMMdd");

            OleDbParameter[] parameters = {
                                                                new OleDbParameter("tjrq",fromDate+","+ToDate),
                                                                new OleDbParameter("guidecode", guidecode),
                                                                new OleDbParameter("incount", incount)
                                                            };
            OracleOledbBase.RunProcedure("hospitalsys.GUIDE_VALUE_ADD_SUM", parameters);

            StringBuilder str = new StringBuilder();
            string power = "";
            switch (organ)
            {

                case "01":
//                    str.AppendFormat(@"  SELECT '院' AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.GUIDE_NAME,UNIT_CODE,GUIDE_CODE FROM (
//                                            SELECT   A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
//                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE,A.UNIT_CODE
//                                                FROM {0}.guide_value_SUM A 
//                                                LEFT JOIN HOSPITALSYS.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
//                                               WHERE A.GUIDE_TYPE = 'Y'
//                                                 AND A.UNIT_CODE = '00'
//                                                 AND A.GUIDE_CODE = '{3}'
//                                                 AND A.STARTDATE >=  ADD_MONTHS(to_date({3},'YYYY-mm-dd'),{5})
//                                                 AND A.ENDDATE <= ADD_MONTHS(to_date({4},'YYYY-mm-dd'),{5})
//                                            GROUP BY A.GUIDE_CODE,UNIT_CODE
//                                         )T1 ", DataUser.HOSPITALSYS, fromDate, ToDate, guidecode, offset);
                    break;

                case "02":
                    power = (DeptPower == "" || DeptPower == null) ? "" : "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    str.AppendFormat(@" SELECT T2.DEPT_NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.GUIDE_NAME,T2.DEPT_CODE AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.ACCOUNT_DEPT_CODE AS UNIT_CODE
                                                FROM {0}.guide_value_SUM A 
                                                LEFT JOIN {1}.SYS_DEPT_DICT B ON A.UNIT_CODE = B.DEPT_CODE
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'K'
                                                 AND A.GUIDE_CODE = '{2}'
                                                  AND TO_CHAR(A.STARTDATE) =  TO_CHAR(to_date({3},'YYYY-mm-dd'))
                                                 AND TO_CHAR(A.ENDDATE) = TO_CHAR(to_date({4},'YYYY-mm-dd'))
                                            GROUP BY A.GUIDE_CODE, B.ACCOUNT_DEPT_CODE
                                         )T1,{1}.SYS_DEPT_DICT T2
                                        WHERE T1.UNIT_CODE = T2.DEPT_CODE AND T2.ATTR='是' {6}", DataUser.HOSPITALSYS, DataUser.COMM, guidecode, fromDate, ToDate, offset, power);
                    if (station != "")
                    {
                        str.AppendFormat("  AND T2.DEPT_CODE in ({0}) ", station);
                    }
                    break;

                case "03":
                    if (DeptPower == "'-1'")
                    {
                        power = "AND T2.STAFF_ID = '" + incount + "'";
                    }
                    else if (DeptPower != "")
                    {
                        power = "AND T2.DEPT_CODE IN (" + DeptPower + ")";
                    }
                    str.AppendFormat(@"  SELECT T2.NAME AS DEPT_NAME ,T1.GUIDE_VALUE ,T1.GUIDE_NAME,T2.STAFF_ID AS UNIT_CODE,GUIDE_CODE FROM (
                                            SELECT   A.GUIDE_CODE,MAX(C.GUIDE_NAME) GUIDE_NAME,
                                                     SUM (GUIDE_VALUE) AS GUIDE_VALUE, B.STAFF_ID AS UNIT_CODE
                                                FROM {0}.guide_value_SUM A 
                                                LEFT JOIN {1}.NEW_STAFF_INFO B ON A.UNIT_CODE = B.STAFF_ID
                                                LEFT JOIN {0}.GUIDE_NAME_DICT C  ON A.GUIDE_CODE = C.GUIDE_CODE
                                               WHERE A.GUIDE_TYPE = 'R'
                                                 AND A.GUIDE_CODE = '{2}'
                                                 AND TO_CHAR(A.STARTDATE) =  TO_CHAR(to_date({3},'YYYY-mm-dd'))
                                                 AND TO_CHAR(A.ENDDATE) = TO_CHAR(to_date({4},'YYYY-mm-dd'))
                                              GROUP BY A.GUIDE_CODE, B.STAFF_ID
                                         )T1,{1}.NEW_STAFF_INFO T2
                                        WHERE T1.UNIT_CODE = T2.STAFF_ID AND T2.ADD_MARK='1' {6}", DataUser.HOSPITALSYS, DataUser.RLZY, guidecode, fromDate, ToDate, offset, power);
                    if (!station.Equals(""))
                    {
                        str.AppendFormat("  AND  T2.DEPT_CODE in ( ");
                        str.AppendFormat("          SELECT DEPT_CODE FROM {0}.SYS_STATION_BASIC_INFORMATION ", DataUser.COMM);
                        str.AppendFormat("          WHERE STATION_YEAR = '{1}' AND STATION_DICT_CODE = {0}   )", station, DateTime.Now.Year.ToString());
                    }
                    if (!where.Equals(""))
                    {
                        str.AppendFormat(where);
                    }
                    break;
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
    }
}
