using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Collections;

namespace Goldnet.Dal
{
    /// <summary>
    /// 评价类方法
    /// </summary>
    public class Assess
    {
        public Assess()
        { }


        #region  成员方法

        /// <summary>
        /// 判断临时保存的考核结果数目
        /// </summary>
        /// <param name="incount"></param>
        /// <returns></returns>
        public string GetSavedAssessCnt(string incount)
        {
            string rtn = OracleOledbBase.GetSingle("SELECT COUNT(*) FROM " + DataUser.HOSPITALSYS + ".ASSESS_RESULT_SAVE WHERE SAVE_ID='" + incount + "'").ToString();
            return rtn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveid"></param>
        /// <returns></returns>
        public string GetAssessArchFlg(string saveid)
        {
            string rtn = OracleOledbBase.GetSingle("SELECT COUNT(*) FROM " + DataUser.HOSPITALSYS + ".ASSESS_INFORMATION  WHERE ASSESS_CODE IN (SELECT ASSESS_CODE FROM " + DataUser.HOSPITALSYS + ".ASSESS_INFORMATION_SAVE WHERE SAVE_ID='" + saveid + "') ").ToString();
            return rtn;
        }

        /// <summary>
        /// 判断是否有临时保存的科室考核数据
        /// </summary>
        /// <param name="incount"></param>
        /// <returns></returns>
        public string GetDeptSavedAssessCnt(string incount, string benginDate)
        {
            string rtn = OracleOledbBase.GetSingle("SELECT COUNT(*) FROM " + DataUser.HOSPITALSYS + ".DEPT_ASSESS_INFORMATION_SAVE WHERE SAVE_ID='" + incount + "' AND START_DATE='" + benginDate + "' ").ToString();
            return rtn;
        }

        /// <summary>
        /// 判断是否有归档的科室考核数据
        /// </summary>
        /// <param name="saveid"></param>
        /// <returns></returns>
        public string GetDeptAssessArchFlg(string saveid, string benginDate)
        {
            string rtn = OracleOledbBase.GetSingle("SELECT COUNT(*) FROM " + DataUser.HOSPITALSYS + ".DEPT_ASSESS_INFORMATION  WHERE ASSESS_CODE IN (SELECT ASSESS_CODE FROM " + DataUser.HOSPITALSYS + ".DEPT_ASSESS_INFORMATION_SAVE WHERE SAVE_ID='" + saveid + "' AND START_DATE='" + benginDate + "' ) ").ToString();
            return rtn;
        }

        /// <summary>
        /// 取得临时保存的考核结果
        /// </summary>
        /// <param name="save_id"></param>
        /// <param name="station_year"></param>
        /// <returns></returns>
        public DataSet GetAssessSavedTemp(string save_id, string station_year)
        {
            string sql = string.Format(@"
SELECT DEPT_CODE,DEPT_NAME, STATION_CODE, PERSON_ID, STATION_NAME AS STATION_NAME, USER_NAME, 
       SUM (DAYS_ON_DUTY) AS DAYS_ON_DUTY,
       SUM (CASE  WHEN BSC_CLASS LIKE '01%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_01,
       SUM (CASE  WHEN BSC_CLASS LIKE '02%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_02,
       SUM (CASE  WHEN BSC_CLASS LIKE '03%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_03,
       SUM (CASE  WHEN BSC_CLASS LIKE '04%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_04,
       --SUM (T_VALUE) AS ALL_VALUE, 
       CASE WHEN SUM (T_VALUE)+1000 >1200 THEN 1200 ELSE SUM (T_VALUE)+1000 END AS ALL_VALUE, 
       SUM (DEST_VALUE) AS DESTVALUE
  FROM (SELECT R.ASSESS_CODE,R.DEPT_CODE,D.DEPT_NAME,R.PERSON_ID,N.NAME AS USER_NAME,R.STATION_CODE, P.STATION_NAME STATION_NAME,R.BSC_CLASS,
               SUM(R.DAYS_ON_DUTY) DAYS_ON_DUTY,  SUM (R.GUIDE_F_VALUE) T_VALUE, SUM (R.GUIDE_VALUE) AS DEST_VALUE
          FROM {2}.SYS_DEPT_DICT D, {2}.SYS_STATION_BASIC_INFORMATION P,
               {3}.ASSESS_RESULT_SAVE R, {4}.NEW_STAFF_INFO N
         WHERE R.STATION_CODE =P.STATION_CODE
           AND R.PERSON_ID = N.STAFF_ID
           AND R.DEPT_CODE =  D.DEPT_CODE
           AND N.ADD_MARK = 1
           AND R.SAVE_ID = '{0}'
           AND P.STATION_YEAR='{1}'
         GROUP BY R.ASSESS_CODE,R.DEPT_CODE, D.DEPT_NAME,
                  R.PERSON_ID, N.NAME, R.BSC_CLASS,
                  R.STATION_CODE, P.STATION_NAME )
 GROUP BY STATION_CODE, STATION_NAME, PERSON_ID, USER_NAME, DEPT_CODE, DEPT_NAME
 ORDER BY DEPT_CODE,STATION_CODE", save_id, station_year, DataUser.COMM, DataUser.HOSPITALSYS, DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取临时保存的绩效考核
        /// </summary>
        /// <param name="save_id"></param>
        /// <param name="station_year"></param>
        /// <returns></returns>
        public DataSet GetDeptAssessSavedTemp(string save_id, string station_year, string benginDate)
        {
            string sql = string.Format(@"
                                    SELECT DEPT_CODE,DEPT_NAME,
                                           SUM (CASE  WHEN BSC_CLASS LIKE '01%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_01,
                                           SUM (CASE  WHEN BSC_CLASS LIKE '02%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_02,
                                           SUM (CASE  WHEN BSC_CLASS LIKE '03%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_03,
                                           SUM (CASE  WHEN BSC_CLASS LIKE '04%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_04,
                                           CASE WHEN SUM (T_VALUE)+1000 >1200 THEN 1200 ELSE SUM (T_VALUE)+1000 END AS ALL_VALUE, 
                                           SUM (DEST_VALUE) AS DESTVALUE
                                      FROM (SELECT R.ASSESS_CODE,R.DEPT_CODE,D.DEPT_NAME,R.BSC_CLASS,
                                                   SUM (R.GUIDE_F_VALUE) T_VALUE, SUM (R.GUIDE_VALUE) AS DEST_VALUE
                                              FROM COMM.SYS_DEPT_DICT D, HOSPITALSYS.DEPT_GATHERS P,
                                                   HOSPITALSYS.DEPT_ASSESS_RESULT_SAVE R
                                             WHERE R.DEPT_CODE =P.DEPT_CODE
                                               AND R.DEPT_CODE =  D.DEPT_CODE
                                               AND P.STATION_YEAR='{1}'
                                               AND R.ASSESS_CODE IN (select ASSESS_CODE from hospitalsys.DEPT_ASSESS_INFORMATION_SAVE where save_id='{0}' and start_date='{2}')
                                             GROUP BY R.ASSESS_CODE,R.DEPT_CODE, D.DEPT_NAME,
                                                      R.BSC_CLASS )
                                     GROUP BY  DEPT_CODE, DEPT_NAME
                                     ORDER BY DEPT_CODE", save_id, station_year, benginDate);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assesscode"></param>
        /// <param name="stationyear"></param>
        /// <param name="deptFilter"></param>
        /// <returns></returns>
        public DataSet GetAssessSavedArch(string assesscode, string stationyear, string deptFilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DEPT_CODE,DEPT_NAME, STATION_CODE, PERSON_ID, STATION_NAME AS STATION_NAME, USER_NAME, 
                                       SUM (DAYS_ON_DUTY) AS DAYS_ON_DUTY,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '01%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_01,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '02%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_02,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '03%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_03,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '04%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_04,
                                       --SUM (T_VALUE) AS ALL_VALUE, 
                                       CASE WHEN SUM (T_VALUE)+1000 >1200 THEN 1200 ELSE SUM (T_VALUE)+1000 END AS ALL_VALUE, 
                                       SUM (DEST_VALUE) AS DESTVALUE
                                  FROM (SELECT R.ASSESS_CODE,R.DEPT_CODE,D.DEPT_NAME,R.PERSON_ID,N.NAME AS USER_NAME,R.STATION_CODE, P.STATION_NAME STATION_NAME,R.BSC_CLASS,
                                               SUM(R.DAYS_ON_DUTY) DAYS_ON_DUTY,  SUM (R.GUIDE_F_VALUE) T_VALUE, SUM (R.GUIDE_VALUE) AS DEST_VALUE
                                          FROM {2}.SYS_DEPT_DICT D, 
                                               {2}.SYS_STATION_BASIC_INFORMATION P,
                                               {3}.ASSESS_RESULT R, 
                                               {4}.NEW_STAFF_INFO N
                                         WHERE R.STATION_CODE =P.STATION_CODE
                                           AND R.PERSON_ID = N.STAFF_ID
                                           AND R.DEPT_CODE =  D.DEPT_CODE
                                           AND N.ADD_MARK = 1
                                           AND R.ASSESS_CODE = '{0}'
                                           AND P.STATION_YEAR='{1}' ", assesscode, stationyear,
                                                                     DataUser.COMM, DataUser.HOSPITALSYS, DataUser.RLZY);
            if (deptFilter != "")
            {
                strSql.Append(" AND D.DEPT_CODE IN (" + deptFilter + ")");
            }

            strSql.Append(@" GROUP BY R.ASSESS_CODE,R.DEPT_CODE, D.DEPT_NAME,
                                      R.PERSON_ID, N.NAME, R.BSC_CLASS,
                                      R.STATION_CODE, P.STATION_NAME )
                     GROUP BY STATION_CODE, STATION_NAME, PERSON_ID, USER_NAME, DEPT_CODE, DEPT_NAME
                     ORDER BY DEPT_CODE,STATION_CODE");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        public DataSet GetDeptAssessSavedArch(string assesscode, string stationyear, string deptFilter, string depttype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DEPT_CODE,DEPT_NAME,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '01%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_01,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '02%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_02,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '03%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_03,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '04%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_04,
                                       CASE WHEN SUM (T_VALUE)+1000 >1200 THEN 1200 ELSE SUM (T_VALUE)+1000 END AS ALL_VALUE, 
                                       SUM (DEST_VALUE) AS DESTVALUE
                                  FROM (SELECT R.ASSESS_CODE,R.DEPT_CODE,D.DEPT_NAME,R.BSC_CLASS,
                                                SUM (R.GUIDE_F_VALUE) T_VALUE, SUM (R.GUIDE_VALUE) AS DEST_VALUE
                                          FROM {2}.SYS_DEPT_DICT D, 
                                               {3}.DEPT_GATHERS P,
                                               {3}.DEPT_ASSESS_RESULT R
                                         WHERE R.DEPT_CODE =P.DEPT_CODE
                                           AND  P.DEPT_CODE=D.DEPT_CODE
                                           AND R.ASSESS_CODE = '{0}'
                                           AND P.STATION_YEAR='{1}' ", assesscode, stationyear,
                                                                     DataUser.COMM, DataUser.HOSPITALSYS);
            if (deptFilter != "")
            {
                strSql.Append(" AND D.DEPT_CODE IN (" + deptFilter + ")");
            }
            if (depttype != "")
            {
                strSql.AppendFormat(" AND D.DEPT_TYPE='{0}'", depttype);
            }

            strSql.Append(@" GROUP BY R.ASSESS_CODE,R.DEPT_CODE, D.DEPT_NAME,
                                      R.BSC_CLASS)
                     GROUP BY DEPT_CODE, DEPT_NAME
                     ORDER BY DEPT_CODE");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 获取科室考核指标结果
        /// </summary>
        /// <param name="assesscode">考核名称</param>
        /// <param name="stationyear">考核年度</param>
        /// <param name="deptFilter">过滤条件</param>
        /// <param name="depttype">科室类别</param>
        /// <param name="deptname">科室名称</param>
        /// <param name="guidecode">指标名称</param>
        /// <returns></returns>
        public DataSet GetDeptAssessSavedArchByGuide(string assesscode, string stationyear, string deptFilter, string depttype, string deptname, string guidecode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT B.DEPT_CODE,
                                       B.DEPT_NAME,
                                       A.GUIDE_CODE,
                                       C.GUIDE_NAME,
                                       A.GUIDE_VALUE,   
                                       A.GUIDE_CAUSE,   
                                       A.GUIDE_FACT,    
                                       A.GUIDE_I_VALUE, 
                                       A.GUIDE_D_VALUE, 
                                       A.GUIDE_F_VALUE  
                                  FROM hospitalsys.DEPT_ASSESS_RESULT A,
                                       COMM.SYS_DEPT_DICT B,
                                       HOSPITALSYS.GUIDE_NAME_DICT C
                                 WHERE B.DEPT_CODE=A.DEPT_CODE
                                   AND C.GUIDE_CODE=A.GUIDE_CODE
                                   AND A.STATION_YEAR='{1}'
                                   AND A.ASSESS_CODE='{0}'", assesscode, stationyear);

            if (depttype != "")
            {
                strSql.AppendFormat(" AND B.DEPT_TYPE ='{0}'", depttype);
            }
            if (deptname != "")
            {
                strSql.AppendFormat(" AND B.DEPT_CODE ='{0}'", deptname);
            }
            if (guidecode != "")
            {
                strSql.AppendFormat(" AND A.GUIDE_CODE ='{0}'", guidecode);
            }

            strSql.Append(@" ORDER BY B.DEPT_CODE,A.GUIDE_CODE ");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }



        public DataSet GetAssessResultTemp(string incount, string stationyear)
        {
            string sql = string.Format(@"
                                SELECT DEPT_CODE,DEPT_NAME, STATION_CODE, PERSON_ID, STATION_NAME AS STATION_NAME, USER_NAME, 
                                       SUM (DAYS_ON_DUTY) AS DAYS_ON_DUTY,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '01%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_01,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '02%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_02,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '03%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_03,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '04%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_04,
                                       --SUM (T_VALUE) AS ALL_VALUE, 
                                       CASE WHEN SUM (T_VALUE)+1000 >1200 THEN 1200 ELSE SUM (T_VALUE)+1000 END AS ALL_VALUE, 
                                       SUM (DEST_VALUE) AS DESTVALUE
                                  FROM (SELECT R.DEPT_CODE,D.DEPT_NAME,R.PERSON_ID,N.NAME AS USER_NAME,R.STATION_CODE, P.STATION_NAME STATION_NAME,R.BSC_CLASS,
                                               SUM(R.DAYS_ON_DUTY) DAYS_ON_DUTY,  SUM (R.GUIDE_F_VALUE) T_VALUE, SUM (R.GUIDE_VALUE) AS DEST_VALUE
                                          FROM {2}.SYS_DEPT_DICT D,  {2}.SYS_STATION_BASIC_INFORMATION P,
                                               {3}.ASSESS_TEMP R, {4}.NEW_STAFF_INFO N
                                         WHERE R.STATION_CODE =P.STATION_CODE
                                           AND R.PERSON_ID = N.STAFF_ID
                                           AND R.DEPT_CODE =  D.DEPT_CODE
                                           AND N.ADD_MARK = 1
                                           AND R.COUNTING = '{0}'
                                           AND P.STATION_YEAR='{1}'
                                         GROUP BY R.DEPT_CODE, D.DEPT_NAME,
                                                  R.PERSON_ID, N.NAME, R.BSC_CLASS,
                                                  R.STATION_CODE, P.STATION_NAME )
                                 GROUP BY STATION_CODE, STATION_NAME, PERSON_ID, USER_NAME, DEPT_CODE, DEPT_NAME
                                 ORDER BY DEPT_CODE,STATION_CODE", incount, stationyear, DataUser.COMM, DataUser.HOSPITALSYS, DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取绩效考核生成的临时数据
        /// </summary>
        /// <param name="incount">登陆人员</param>
        /// <param name="stationyear">年度</param>
        /// <returns></returns>
        public DataSet GetDeptAssessResultTemp(string incount, string stationyear, string benginDate)
        {
            string sql = string.Format(@"
                                SELECT DEPT_CODE,DEPT_NAME, 
                                       SUM (CASE  WHEN BSC_CLASS LIKE '01%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_01,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '02%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_02,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '03%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_03,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '04%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_04,
                                       CASE WHEN SUM (T_VALUE)+1000 >1200 THEN 1200 ELSE SUM (T_VALUE)+1000 END AS ALL_VALUE, 
                                       SUM (DEST_VALUE) AS DESTVALUE
                                  FROM (SELECT R.DEPT_CODE,D.DEPT_NAME,R.BSC_CLASS,
                                              SUM (R.GUIDE_F_VALUE) T_VALUE, SUM (R.GUIDE_VALUE) AS DEST_VALUE
                                          FROM COMM.SYS_DEPT_DICT D,  HOSPITALSYS.DEPT_GATHERS P,
                                               HOSPITALSYS.DEPT_ASSESS_TEMP R
                                         WHERE R.DEPT_CODE =P.DEPT_CODE
                                               AND R.DEPT_CODE =  D.DEPT_CODE
                                           AND R.COUNTING = '{0}'
                                           AND P.STATION_YEAR='{1}'
                                           AND R.START_DATE='{2}'
                                         GROUP BY R.DEPT_CODE, D.DEPT_NAME,
                                                  R.BSC_CLASS )
                                 GROUP BY DEPT_CODE, DEPT_NAME
                                 ORDER BY DEPT_CODE", incount, stationyear, benginDate);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取临时保存的绩效考评结果
        /// </summary>
        /// <param name="incount"></param>
        /// <param name="stationyear"></param>
        /// <returns></returns>
        public DataSet GetDeptAssessResultSave(string incount, string stationyear, string benginDate)
        {
            string sql = string.Format(@"
                                SELECT DEPT_CODE,DEPT_NAME, 
                                       SUM (CASE  WHEN BSC_CLASS LIKE '01%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_01,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '02%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_02,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '03%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_03,
                                       SUM (CASE  WHEN BSC_CLASS LIKE '04%' THEN T_VALUE ELSE 0 END ) AS GUIDE_F_VALUE_04,
                                       CASE WHEN SUM (T_VALUE)+1000 >1200 THEN 1200 ELSE SUM (T_VALUE)+1000 END AS ALL_VALUE, 
                                       SUM (DEST_VALUE) AS DESTVALUE
                                  FROM (SELECT R.DEPT_CODE,D.DEPT_NAME,R.BSC_CLASS,
                                              SUM (R.GUIDE_F_VALUE) T_VALUE, SUM (R.GUIDE_VALUE) AS DEST_VALUE
                                          FROM COMM.SYS_DEPT_DICT D,  HOSPITALSYS.DEPT_GATHERS P,
                                               HOSPITALSYS.DEPT_ASSESS_RESULT_SAVE R
                                         WHERE R.DEPT_CODE =P.DEPT_CODE
                                               AND R.DEPT_CODE =  D.DEPT_CODE
                                           AND R.save_id = '{0}'
                                           AND P.STATION_YEAR='{1}'
                                           AND R.ASSESS_CODE IN (select ASSESS_CODE from hospitalsys.DEPT_ASSESS_INFORMATION_SAVE where save_id='{0}' and start_date='{2}')
                                         GROUP BY R.DEPT_CODE, D.DEPT_NAME,
                                                  R.BSC_CLASS )
                                 GROUP BY DEPT_CODE, DEPT_NAME
                                 ORDER BY DEPT_CODE", incount, stationyear, benginDate);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 临时保存绩效考核结果
        /// </summary>
        /// <param name="ASSESS_NAME">考核名称</param>
        /// <param name="START_DATE">开始时间</param>
        /// <param name="END_DATE">结束时间</param>
        /// <param name="ASSESS_APPRAISER">考核人</param>
        /// <param name="station_year">岗位年度</param>
        /// <param name="counting"></param>
        public string Assess_informationAndresultSave(string ASSESS_NAME, string START_DATE, string END_DATE, string ASSESS_APPRAISER, string station_year, string counting, string save_id)
        {

            string Assecc_code = OracleOledbBase.GetSingle(string.Format("SELECT NVL(MAX(ASSESS_CODE),0) + 1 AS ASSESS_CODE FROM {0}.ASSESS_INFORMATION_SAVE ", DataUser.HOSPITALSYS)).ToString();
            ArrayList SQLStringList = new ArrayList();

            string sqlstr1 = string.Format("DELETE FROM {1}.ASSESS_INFORMATION_SAVE WHERE SAVE_ID='{0}'", save_id, DataUser.HOSPITALSYS);
            SQLStringList.Add(sqlstr1);

            string sqlstr2 = string.Format("DELETE FROM {1}.ASSESS_RESULT_SAVE WHERE SAVE_ID='{0}'", save_id, DataUser.HOSPITALSYS);
            SQLStringList.Add(sqlstr2);


            string sqlstr3 = string.Format(@"INSERT INTO {0}.ASSESS_INFORMATION_SAVE(ASSESS_CODE,ASSESS_NAME,START_DATE,END_DATE,ASSESS_APPRAISER,ASSESS_TIME,save_id) 
                                   VALUES('{1}','{2}','{3}','{4}','{5}',trunc(sysdate),'{6}') ", DataUser.HOSPITALSYS, Assecc_code, ASSESS_NAME, START_DATE, END_DATE, ASSESS_APPRAISER, save_id);
            SQLStringList.Add(sqlstr3);

            string sqlstr4 = string.Format(@" INSERT INTO {0}.ASSESS_RESULT_SAVE (
                    ASSESS_CODE,STATION_CODE,UNIT_CODE,DEPT_CODE,PERSON_ID,
                    GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_FACT,
                    GUIDE_I_VALUE,GUIDE_D_VALUE,GUIDE_F_VALUE,
                    DAYS_ON_DUTY,BSC_CLASS,STATION_YEAR,SAVE_ID )
                    SELECT '{1}',STATION_CODE, CASE WHEN DUTY_ORDER < 3 THEN TO_CHAR(DUTY_ORDER) ELSE PERSON_ID END  AS UNIT_CODE,DEPT_CODE,PERSON_ID,
                           GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_FACT,
                           GUIDE_I_VALUE,GUIDE_D_VALUE,GUIDE_F_VALUE,
                           DAYS_ON_DUTY,BSC_CLASS,'{2}',COUNTING
                    FROM {0}.ASSESS_TEMP WHERE COUNTING='{3}'", DataUser.HOSPITALSYS, Assecc_code, station_year, save_id);
            SQLStringList.Add(sqlstr4);
            string rtn = "";
            try
            {
                OracleBase.ExecuteSqlTran(SQLStringList);
            }
            catch (Exception ee)
            {
                rtn = "考核数据临时保存失败，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 临时保存考核数据
        /// </summary>
        /// <param name="ASSESS_NAME"></param>
        /// <param name="START_DATE"></param>
        /// <param name="END_DATE"></param>
        /// <param name="ASSESS_APPRAISER"></param>
        /// <param name="station_year"></param>
        /// <param name="counting"></param>
        /// <param name="save_id"></param>
        /// <returns></returns>
        public string deptAssess_informationAndresultSave(string ASSESS_NAME, string START_DATE, string END_DATE, string ASSESS_APPRAISER, string station_year, string counting, string save_id)
        {
            string Assecc_code = OracleOledbBase.GetSingle(string.Format("SELECT NVL(MAX(ASSESS_CODE),0) + 1 AS ASSESS_CODE FROM {0}.DEPT_ASSESS_INFORMATION_SAVE ", DataUser.HOSPITALSYS)).ToString();
            ArrayList SQLStringList = new ArrayList();

            string sqlstr1 = string.Format("DELETE FROM {1}.DEPT_ASSESS_INFORMATION_SAVE WHERE SAVE_ID='{0}' AND START_DATE='{2}'", save_id, DataUser.HOSPITALSYS, START_DATE);
            SQLStringList.Add(sqlstr1);

            string sqlstr2 = string.Format("DELETE FROM {1}.DEPT_ASSESS_RESULT_SAVE WHERE SAVE_ID='{0}' AND ASSESS_CODE IN (SELECT ASSESS_CODE FROM {1}.DEPT_ASSESS_INFORMATION_SAVE WHERE SAVE_ID='{0}' AND START_DATE='{2}')", save_id, DataUser.HOSPITALSYS, START_DATE);
            SQLStringList.Add(sqlstr2);


            string sqlstr3 = string.Format(@"INSERT INTO {0}.DEPT_ASSESS_INFORMATION_SAVE(ASSESS_CODE,ASSESS_NAME,START_DATE,END_DATE,ASSESS_APPRAISER,ASSESS_TIME,save_id) 
                                   VALUES({1},'{2}','{3}','{4}','{5}',trunc(sysdate),'{6}') ", DataUser.HOSPITALSYS, Assecc_code, ASSESS_NAME, START_DATE, END_DATE, ASSESS_APPRAISER, save_id);
            SQLStringList.Add(sqlstr3);

            string sqlstr4 = string.Format(@" INSERT INTO {0}.DEPT_ASSESS_RESULT_SAVE (
                    ASSESS_CODE,DEPT_CODE,
                    GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_FACT,
                    GUIDE_I_VALUE,GUIDE_D_VALUE,GUIDE_F_VALUE,
                    BSC_CLASS,STATION_YEAR,SAVE_ID )
                    SELECT {1},DEPT_CODE,
                           GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_FACT,
                           GUIDE_I_VALUE,GUIDE_D_VALUE,GUIDE_F_VALUE,
                           BSC_CLASS,'{2}',COUNTING
                    FROM {0}.DEPT_ASSESS_TEMP WHERE COUNTING='{3}'", DataUser.HOSPITALSYS, Assecc_code, station_year, save_id);
            SQLStringList.Add(sqlstr4);
            string rtn = "";
            try
            {
                OracleBase.ExecuteSqlTran(SQLStringList);
            }
            catch (Exception ee)
            {
                rtn = "考核数据临时保存失败，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 保存修改后的生成得分
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="dept_code"></param>
        /// <param name="bsc_class"></param>
        /// <param name="save_id"></param>
        public void SaveTemAssessInformation(Dictionary<string, string>[] costdetails, string dept_code, string bsc_class, string save_id)
        {
            MyLists listtable = new MyLists();

            for (int i = 0; i < costdetails.Length; i++)
            {
                if (costdetails[i]["GUIDE_CODE"] == null || costdetails[i]["GUIDE_F_VALUE"].Equals("") || save_id.Equals("") || dept_code.Equals(""))
                {
                    continue;
                }

                //StringBuilder strDel = new StringBuilder();
                //strDel.AppendFormat("DELETE HOSPITALSYS.DEPT_ASSESS_RESULT_SAVE WHERE dept_code= '{0}' and save_id='{1}' and guide_code='{2}'", dept_code, save_id, costdetails[i]["GUIDE_CODE"]);

                //List listDel = new List();
                //listDel.StrSql = strDel;
                //listtable.Add(listDel);

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"UPDATE HOSPITALSYS.DEPT_ASSESS_RESULT_SAVE SET GUIDE_F_VALUE={3} WHERE dept_code= '{0}' and save_id='{1}' and guide_code='{2}' ", dept_code, save_id, costdetails[i]["GUIDE_CODE"], costdetails[i]["GUIDE_F_VALUE"]);

                List listAdd = new List();
                listAdd.StrSql = strSql;
                listtable.Add(listAdd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }


        /// <summary>
        /// 绩效考核归档
        /// </summary>
        /// <param name="ASSESS_NAME">归档名称</param>
        /// <param name="START_DATE">开始时间</param>
        /// <param name="END_DATE">结束时间</param>
        /// <param name="ASSESS_APPRAISER">归档人</param>
        /// <param name="station_year">岗位年度</param>
        /// <param name="counting"></param>
        public string AddAssess_informationAndresult(string ASSESS_NAME, string counting)
        {
            string Assecc_code = OracleOledbBase.GetSingle(string.Format("SELECT NVL(MAX(ASSESS_CODE),0)  AS ASSESS_CODE FROM {0}.ASSESS_INFORMATION_SAVE WHERE SAVE_ID='{1}' ", DataUser.HOSPITALSYS, counting)).ToString();
            MyLists SQLStringList = new MyLists();

            //归档表
            List sqlList = new List();
            string sqlstr1 = string.Format(@"INSERT INTO {0}.ASSESS_INFORMATION(ASSESS_CODE,ASSESS_NAME,START_DATE,END_DATE,ASSESS_APPRAISER,ASSESS_TIME) 
                                   SELECT ASSESS_CODE,'{2}',START_DATE,END_DATE,ASSESS_APPRAISER ,trunc(sysdate) FROM {0}.ASSESS_INFORMATION_SAVE WHERE SAVE_ID='{1}' ", DataUser.HOSPITALSYS, counting, ASSESS_NAME);
            sqlList.StrSql = sqlstr1;
            SQLStringList.Add(sqlList);

            //归档明细表
            string sqlstr2 = string.Format(@" INSERT INTO {0}.ASSESS_RESULT (
                    ASSESS_CODE,STATION_CODE,UNIT_CODE,DEPT_CODE,PERSON_ID,
                    GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_FACT,
                    GUIDE_I_VALUE,GUIDE_D_VALUE,GUIDE_F_VALUE,
                    DAYS_ON_DUTY,BSC_CLASS,STATION_YEAR )
                    SELECT  ASSESS_CODE,STATION_CODE,UNIT_CODE,DEPT_CODE,PERSON_ID,
                    GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_FACT,
                    GUIDE_I_VALUE,GUIDE_D_VALUE,GUIDE_F_VALUE,
                    DAYS_ON_DUTY,BSC_CLASS,STATION_YEAR
                    FROM {0}.ASSESS_RESULT_SAVE WHERE SAVE_ID='{1}' AND ASSESS_CODE={2} ", DataUser.HOSPITALSYS, counting, Assecc_code);
            List sqlList2 = new List();
            sqlList2.StrSql = sqlstr2;
            SQLStringList.Add(sqlList2);


            //删除临时表数据
            string sqlstr3 = string.Format(@"DELETE FROM {0}.ASSESS_TEMP WHERE COUNTING='{1}'", DataUser.HOSPITALSYS, counting);
            List sqlList3 = new List();
            sqlList3.StrSql = sqlstr3;
            SQLStringList.Add(sqlList3);


            string rtn = "";
            try
            {
                OracleOledbBase.ExecuteTranslist(SQLStringList);
            }
            catch (Exception ee)
            {
                rtn = "考核数据归档失败，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 归档保存考核数据
        /// </summary>
        /// <param name="ASSESS_NAME"></param>
        /// <param name="counting"></param>
        /// <returns></returns>
        public string deptAddAssess_informationAndresult(string ASSESS_NAME, string counting, string START_DATE)
        {
            string Assecc_code = OracleOledbBase.GetSingle(string.Format("SELECT NVL(ASSESS_CODE,0)  AS ASSESS_CODE FROM {0}.DEPT_ASSESS_INFORMATION_SAVE WHERE SAVE_ID='{1}' and start_date='{2}' ", DataUser.HOSPITALSYS, counting, START_DATE)).ToString();
            ArrayList SQLStringList = new ArrayList();

            string sqlstr4 = string.Format("DELETE FROM {0}.DEPT_ASSESS_INFORMATION WHERE START_DATE='{1}'", DataUser.HOSPITALSYS, START_DATE);
            SQLStringList.Add(sqlstr4);

            string sqlstr5 = string.Format("DELETE FROM {0}.DEPT_ASSESS_RESULT WHERE ASSESS_CODE IN (SELECT ASSESS_CODE FROM {0}.DEPT_ASSESS_INFORMATION WHERE START_DATE='{1}')", DataUser.HOSPITALSYS, START_DATE);
            SQLStringList.Add(sqlstr5);

            //归档主表
            //List sqlList = new List();
            string sqlstr1 = string.Format(@"INSERT INTO {0}.DEPT_ASSESS_INFORMATION(ASSESS_CODE,ASSESS_NAME,START_DATE,END_DATE,ASSESS_APPRAISER,ASSESS_TIME) 
                                   SELECT ASSESS_CODE,'{2}',START_DATE,END_DATE,ASSESS_APPRAISER ,trunc(sysdate) FROM {0}.DEPT_ASSESS_INFORMATION_SAVE WHERE SAVE_ID='{1}' AND ASSESS_CODE={3} ", DataUser.HOSPITALSYS, counting, ASSESS_NAME, Assecc_code);
            //sqlList.StrSql = sqlstr1;
            SQLStringList.Add(sqlstr1);

            //归档明细表
            string sqlstr2 = string.Format(@" INSERT INTO {0}.DEPT_ASSESS_RESULT (
                    ASSESS_CODE,DEPT_CODE,
                    GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_FACT,
                    GUIDE_I_VALUE,GUIDE_D_VALUE,GUIDE_F_VALUE,
                    BSC_CLASS,STATION_YEAR )
                    SELECT  ASSESS_CODE,DEPT_CODE,
                    GUIDE_CODE,GUIDE_VALUE,GUIDE_CAUSE,GUIDE_FACT,
                    GUIDE_I_VALUE,GUIDE_D_VALUE,GUIDE_F_VALUE,
                    BSC_CLASS,STATION_YEAR
                    FROM {0}.DEPT_ASSESS_RESULT_SAVE WHERE SAVE_ID='{1}' AND ASSESS_CODE={2} ", DataUser.HOSPITALSYS, counting, Assecc_code);
            //List sqlList2 = new List();
            //sqlList2.StrSql = sqlstr2;
            SQLStringList.Add(sqlstr2);


            //删除临时表数据
            string sqlstr3 = string.Format(@"DELETE FROM {0}.DEPT_ASSESS_TEMP WHERE COUNTING='{1}'", DataUser.HOSPITALSYS, counting);
            //List sqlList3 = new List();
            //sqlList3.StrSql = sqlstr3;
            SQLStringList.Add(sqlstr3);


            string rtn = "";
            try
            {
                //OracleOledbBase.ExecuteTranslist(SQLStringList);
                OracleBase.ExecuteSqlTran(SQLStringList);
            }
            catch (Exception ee)
            {
                rtn = "考核数据归档失败，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 删除归档考核数据
        /// </summary>
        /// <param name="assesscode"></param>
        /// <returns></returns>
        public string DelAssessSavedArch(string assesscode)
        {
            ArrayList SQLStringList = new ArrayList();
            string sqlstr1 = string.Format(@"DELETE FROM  {0}.ASSESS_INFORMATION  WHERE ASSESS_CODE='{1}' ", DataUser.HOSPITALSYS, assesscode);
            string sqlstr2 = string.Format(@"DELETE FROM  {0}.ASSESS_RESULT  WHERE ASSESS_CODE='{1}' ", DataUser.HOSPITALSYS, assesscode);
            SQLStringList.Add(sqlstr1);
            SQLStringList.Add(sqlstr2);

            string rtn = "";
            try
            {
                OracleBase.ExecuteSqlTran(SQLStringList);
            }
            catch (Exception ee)
            {
                rtn = "考核数据删除失败，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 删除归档数据
        /// </summary>
        /// <param name="assesscode"></param>
        /// <returns></returns>
        public string DelDeptAssessSavedArch(string assesscode)
        {
            ArrayList SQLStringList = new ArrayList();
            string sqlstr1 = string.Format(@"DELETE FROM  {0}.DEPT_ASSESS_INFORMATION  WHERE ASSESS_CODE='{1}' ", DataUser.HOSPITALSYS, assesscode);
            string sqlstr2 = string.Format(@"DELETE FROM  {0}.DEPT_ASSESS_RESULT  WHERE ASSESS_CODE='{1}' ", DataUser.HOSPITALSYS, assesscode);
            SQLStringList.Add(sqlstr1);
            SQLStringList.Add(sqlstr2);

            string rtn = "";
            try
            {
                OracleBase.ExecuteSqlTran(SQLStringList);
            }
            catch (Exception ee)
            {
                rtn = "考核数据删除失败，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 执行生成各岗位相应的指标完成情况的存储过程
        /// </summary>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param> 
        /// <param name="guideyear">岗位年度</param>
        /// <param name="incount"></param>
        public string GetAssess_COMPUTE(string startdate, string enddate, string guideyear, string incount)
        {
            string rtn = "";
            //OracleParameter[] parms = {
            //    new OracleParameter("startdate",startdate),
            //    new OracleParameter("enddate",enddate),
            //    new OracleParameter("guideyear",guideyear),
            //    new OracleParameter("incount",incount)
            //};

            OleDbParameter[] parms ={
                new OleDbParameter("startdate",startdate),
                new OleDbParameter("enddate",enddate),
                new OleDbParameter("guideyear",guideyear),
                new OleDbParameter("incount",incount)
                                    };
            try
            {
                //OracleBase.RunProcedure(string.Format("{0}.ASSESS_COMPUTE", DataUser.HOSPITALSYS), parms);
                OracleOledbBase.RunProcedure(string.Format("{0}.ASSESS_COMPUTE", DataUser.HOSPITALSYS), parms);
            }
            catch (Exception ee)
            {
                rtn = "考核数据生成失败，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 科室绩效考核生成
        /// </summary>
        /// <param name="accountdate"></param>
        /// <param name="guideyear"></param>
        /// <param name="incount"></param>
        /// <returns></returns>
        public string GetdeptAssess_COMPUTE(string startdate, string enddate, string guideyear, string incount)
        {
            string rtn = "";
            OleDbParameter[] parms ={
                new OleDbParameter("startdate",startdate),
                new OleDbParameter("enddate",enddate),
                new OleDbParameter("guideyear",guideyear),
                new OleDbParameter("incount",incount)
                                    };
            try
            {
                OracleOledbBase.RunProcedure(string.Format("{0}.DEPT_ASSESS_COMPUTE", DataUser.HOSPITALSYS), parms);
            }
            catch (Exception ee)
            {
                rtn = "考核数据生成失败，<BR/>原因：" + ee.Message;
            }
            return rtn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationyear"></param>
        /// <param name="incount"></param>
        /// <returns></returns>
        public DataSet GetArchAssess(string stationyear, string incount)
        {
            string strsql = string.Format(@" SELECT ASSESS_CODE,ASSESS_NAME FROM {0}.ASSESS_INFORMATION 
                                              WHERE substr(START_DATE,0,4) = '{1}'  ", DataUser.HOSPITALSYS, stationyear);
            if (!incount.Equals(""))
            {
                strsql = strsql + " AND ASSESS_APPRAISER = '" + incount + "'";
            }
            return OracleOledbBase.ExecuteDataSet(strsql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationyear"></param>
        /// <param name="incount"></param>
        /// <returns></returns>
        public DataSet GetDeptArchAssess(string stationyear, string incount)
        {
            string strsql = string.Format(@" SELECT ASSESS_CODE,ASSESS_NAME FROM {0}.DEPT_ASSESS_INFORMATION 
                                              WHERE substr(START_DATE,0,4) = '{1}'  ", DataUser.HOSPITALSYS, stationyear);
            if (!incount.Equals(""))
            {
                strsql = strsql + " AND ASSESS_APPRAISER = '" + incount + "'";
            }
            return OracleOledbBase.ExecuteDataSet(strsql);
        }
        /// <summary>
        /// 执行岗位指标评测
        /// </summary>
        /// <param name="stationyear">岗位年度</param>
        /// <param name="incount"></param>
        /// <param name="station_id">岗位代码</param>
        public DataSet GetStationTest(string stationyear, string incount, string station_id)
        {
            string strsql = string.Format(@"
                SELECT FLAGS,AA.PERSON_NAME,AA.PERSON_ID,AA.SUMVALUE, 
                    SYS_CONNECT_BY_PATH (GUIDE_NAME||'实际值____'||GUIDE_NAME||'目标值' , ',') AS TITLE ,
                     SYS_CONNECT_BY_PATH (TO_CHAR(NVL(GUIDE_FACT,0),'FM9999999999990.0099')||'____'||TO_CHAR(NVL(GUIDE_CAUSE,0),'FM9999999999990.0099') , ',') AS VALUESTR
                  FROM  (
                 SELECT ROW_NUMBER() OVER(PARTITION BY ASSESS_TEMP.PERSON_ID ORDER BY ASSESS_TEMP.GUIDE_CODE) AS FLAGS ,
                       NVL (ASSESS_TEMP.GUIDE_FACT, 0) AS GUIDE_FACT,
                       NVL (ASSESS_TEMP.GUIDE_CAUSE, 0) AS GUIDE_CAUSE,
                       ASSESS_TEMP.PERSON_ID, ASSESS_TEMP.COUNTING,
                       SUM(NVL(ASSESS_TEMP.GUIDE_F_VALUE,0)) OVER(PARTITION BY ASSESS_TEMP.PERSON_ID ) AS SUMVALUE,
                       ASSESS_TEMP.GUIDE_F_VALUE,
                       REPLACE(GUIDE_NAME_DICT.GUIDE_NAME,',','-') AS GUIDE_NAME , 
                       SRC.PERSON_NAME,
                       SRC.STATION_YEAR, 
                       SRC.GUIDE_CODE
                  FROM (SELECT SYS_STATION_PERSONNEL_INFO.PERSON_ID,
                               SYS_STATION_PERSONNEL_INFO.PERSON_NAME,
                               SYS_STATION_BASIC_INFORMATION.STATION_YEAR, 
                               SYS_STATION_BASIC_INFORMATION.STATION_CODE ,
                               STATION_GUIDE_INFORMATION.GUIDE_CODE
                          FROM {3}.SYS_STATION_BASIC_INFORMATION,
                               {4}.STATION_GUIDE_INFORMATION,
                               {3}.SYS_STATION_PERSONNEL_INFO
                          WHERE SYS_STATION_BASIC_INFORMATION.STATION_CODE = STATION_GUIDE_INFORMATION.STATION_CODE 
                            AND SYS_STATION_BASIC_INFORMATION.STATION_YEAR = STATION_GUIDE_INFORMATION.STATION_YEAR 
                            AND SYS_STATION_BASIC_INFORMATION.STATION_CODE = SYS_STATION_PERSONNEL_INFO.STATION_CODE 
                            AND SYS_STATION_BASIC_INFORMATION.STATION_YEAR = SYS_STATION_PERSONNEL_INFO.STATION_YEAR 
                            AND SYS_STATION_BASIC_INFORMATION.STATION_YEAR = '{0}'
                            AND SYS_STATION_BASIC_INFORMATION.STATION_CODE = '{2}'  ) SRC  
                    LEFT JOIN  {4}.ASSESS_TEMP ASSESS_TEMP 
                      ON SRC.PERSON_ID = ASSESS_TEMP.PERSON_ID AND SRC.STATION_CODE = ASSESS_TEMP.STATION_CODE 
                     AND SRC.GUIDE_CODE = ASSESS_TEMP.GUIDE_CODE
                    LEFT JOIN {4}.GUIDE_NAME_DICT
                      ON SRC.GUIDE_CODE = GUIDE_NAME_DICT.GUIDE_CODE 
                   WHERE UPPER(ASSESS_TEMP.COUNTING) = UPPER('{1}')
                ) AA  
                 WHERE CONNECT_BY_ISLEAF = 1
                 START WITH FLAGS = 1
                CONNECT BY NOCYCLE PRIOR FLAGS +1 = FLAGS   AND  PRIOR PERSON_ID= PERSON_ID ", stationyear, incount, station_id, DataUser.COMM, DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(strsql);
        }
        /// <summary>
        /// 获取人员基本信息
        /// </summary>
        /// <param name="staff_id">人员ID</param>
        /// <returns>DataSet</returns>
        public DataSet GetStaffInfo(string staff_id)
        {
            string strSql = string.Format(@"SELECT N.STAFF_ID, N.DEPT_CODE, N.DEPT_NAME, N.NAME, N.IF_ARMY, N.ADD_MARK,
                                                   N.ISONGUARD, N.BIRTHDAY, N.SEX, N.NATIONALS, N.BONUS_FACTOR,
                                                   N.GOVERNMENT_ALLOWANCE, N.CADRES_CATEGORIES, N.STUDY_OVER_DATE,
                                                   N.DEPT_TYPE, N.TOPEDUCATE, N.STUDY_SPECSORT, N.INHOSPITALDATE,
                                                   N.BASEPAY, N.RETAINTERM, N.JOB, N.JOBDATE, N.STAFFSORT,
                                                   N.BEENROLLEDINDATE, N.WORKDATE, N.DUTY, N.DUTYDATE, N.TECHINCCLASS,
                                                   N.TECHNICCLASSDATE, N.CIVILSERVICECLASS, N.CIVILSERVICECLASSDATE,
                                                   N.SANTSPECSORT, N.ROOTSPECSORT, N.MEDICARDMARK, N.MEDICARD,
                                                   N.INPUT_USER, N.INPUT_DATE, N.USER_DATE, N.GUARDTEAM, N.GUARDGROUP,
                                                   N.GUARDDUTY, N.GUARDTYPE, N.GUARDCHAN, N.GUARDTIME, N.GUARDCAUS,
                                                   N.GUARDREMARK, N.DEPTGROUP, N.HOMEPLACE, N.CERTIFICATE_NO,
                                                   N.MARITAL_STATUS, N.TITLE_LIST, N.EDU1, N.GRADUATE_ACADEMY,
                                                   N.DATE_OF_GRADETITLE, N.RANK, N.TITLE, N.GROUP_ID, N.MEMO, N.MARK_USER,
                                                   N.USER_ID, N.JW_USER_NAME, N.INPUT_CODE, N.IMG_ID, N.JOB_TITLE,
                                                   N.TITLE_DATE, N.EXPERT, N.CREDITHOUR_PERYEAR, N.LEADTECN,
                                                   N.STATION_CODE, N.ISBRAID, S.STATION_NAME
                                              FROM {0}.NEW_STAFF_INFO N, {1}.SYS_STATION_MAINTENANCE_DICT S
                                             WHERE N.STATION_CODE = S.ID(+) AND N.STAFF_ID = '{2}'", DataUser.RLZY, DataUser.COMM, staff_id);
            return OracleOledbBase.ExecuteDataSet(strSql);
        }

        /// <summary>
        /// 获取考核结果临时数据
        /// </summary>
        /// <param name="counting">操作员用户ID</param>
        /// <param name="person_id">staff_id</param>
        /// <param name="bsc_class">指标BSC分类</param>
        /// <returns></returns>
        public DataSet GetResultInfoTemp(string counting, string person_id, string bsc_class)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT S.BSC_CLASS_NAME, S.BSC_CLASS, G.GUIDE_NAME,G.EXPLAIN,
                                         ROUND (S.GUIDE_VALUE, 2) AS GUIDE_VALUE,
                                         S.GUIDE_CAUSE AS GUIDE_CAUSE, 
                                         ROUND (S.GUIDE_FACT, 2) AS GUIDE_FACT,
                                         ROUND (S.GUIDE_I_VALUE, 2) AS GUIDE_I_VALUE,
                                         ROUND (S.GUIDE_D_VALUE, 2) AS GUIDE_D_VALUE,
                                         ROUND (S.GUIDE_F_VALUE, 2) AS GUIDE_F_VALUE
                                    FROM (SELECT   T.*, J.BSC_CLASS_NAME
                                              FROM {0}.ASSESS_TEMP T,
                                                   {0}.JXGL_GUIDE_BSC_CLASS_DICT J
                                             WHERE {1} T.COUNTING = '{2}'
                                               AND SUBSTR (T.BSC_CLASS, 1, 2) = J.BSC_CLASS_CODE
                                               AND T.PERSON_ID = '{3}'
                                          ORDER BY T.STATION_CODE) S,
                                         HOSPITALSYS.GUIDE_NAME_DICT G
                                   WHERE G.GUIDE_CODE = S.GUIDE_CODE
                                ORDER BY S.BSC_CLASS", DataUser.HOSPITALSYS, bsc_class, counting, person_id);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 获取生成的绩效结果
        /// </summary>
        /// <param name="counting"></param>
        /// <param name="dept_code"></param>
        /// <param name="bsc_class"></param>
        /// <returns></returns>
        public DataSet GetdeptResultInfoTemp(string counting, string dept_code, string bsc_class)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT G.GUIDE_CODE,S.BSC_CLASS_NAME, S.BSC_CLASS, G.GUIDE_NAME,G.EXPLAIN,
                                         ROUND (S.GUIDE_VALUE, 2) AS GUIDE_VALUE,
                                         S.GUIDE_CAUSE AS GUIDE_CAUSE, 
                                         ROUND (S.GUIDE_FACT, 2) AS GUIDE_FACT,
                                         ROUND (S.GUIDE_I_VALUE, 2) AS GUIDE_I_VALUE,
                                         ROUND (S.GUIDE_D_VALUE, 2) AS GUIDE_D_VALUE,
                                         ROUND (S.GUIDE_F_VALUE, 2) AS GUIDE_F_VALUE,
                                         CASE WHEN S.GUIDE_F_VALUE>=0 THEN '1' ELSE '0' END FLAG
                                    FROM (SELECT   T.*, J.BSC_CLASS_NAME,L.SORT_NO
                                              FROM HOSPITALSYS.DEPT_ASSESS_TEMP T,
                                                   HOSPITALSYS.JXGL_GUIDE_BSC_CLASS_DICT J,
                                                   HOSPITALSYS.GUIDE_NAME_DICT L
                                             WHERE {1} T.COUNTING = '{2}'
                                               AND SUBSTR (T.BSC_CLASS, 1, 2) = J.BSC_CLASS_CODE
                                               AND T.GUIDE_CODE=L.GUIDE_CODE
                                               AND T.DEPT_CODE = '{3}'
                                          ORDER BY T.DEPT_CODE) S,
                                         HOSPITALSYS.GUIDE_NAME_DICT G
                                   WHERE G.GUIDE_CODE = S.GUIDE_CODE
                                ORDER BY S.BSC_CLASS,S.SORT_NO", DataUser.HOSPITALSYS, bsc_class, counting, dept_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 获取归档的绩效结果
        /// </summary>
        /// <param name="counting"></param>
        /// <param name="dept_code"></param>
        /// <param name="bsc_class"></param>
        /// <returns></returns>
        public DataSet GetdeptResultInfoSave(string counting, string dept_code, string bsc_class)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT G.GUIDE_CODE,S.BSC_CLASS_NAME, S.BSC_CLASS, G.GUIDE_NAME,NVL(f.guide_memo,g.EXPLAIN) EXPLAIN,
                                         ROUND (S.GUIDE_VALUE, 2) AS GUIDE_VALUE,
                                         S.GUIDE_CAUSE AS GUIDE_CAUSE, 
                                         ROUND (S.GUIDE_FACT, 2) AS GUIDE_FACT,
                                         ROUND (S.GUIDE_I_VALUE, 2) AS GUIDE_I_VALUE,
                                         ROUND (S.GUIDE_D_VALUE, 2) AS GUIDE_D_VALUE,
                                         ROUND (S.GUIDE_F_VALUE, 2) AS GUIDE_F_VALUE,
                                         CASE WHEN S.GUIDE_F_VALUE>=0 THEN '1' ELSE '0' END FLAG
                                    FROM (SELECT   T.*, J.BSC_CLASS_NAME,L.SORT_NO,to_date(m.start_date||'01','yyyymmdd') datetime
                                              FROM HOSPITALSYS.DEPT_ASSESS_RESULT_SAVE T,
                                                   HOSPITALSYS.JXGL_GUIDE_BSC_CLASS_DICT J,
                                                   HOSPITALSYS.GUIDE_NAME_DICT L,
                                                   HOSPITALSYS.DEPT_ASSESS_INFORMATION m
                                             WHERE {1} T.save_id = '{2}'
                                               AND T.assess_code=m.assess_code
                                               AND SUBSTR (T.BSC_CLASS, 1, 2) = J.BSC_CLASS_CODE
                                               AND T.GUIDE_CODE=L.GUIDE_CODE
                                               AND T.DEPT_CODE = '{3}'
                                          ORDER BY T.DEPT_CODE) S,
                                         HOSPITALSYS.GUIDE_NAME_DICT G,
                                         (SELECT   a.st_date,a.dept_code,b.link_guide_code,a.guide_memo
                                            FROM   HOSPITALSYS.SYS_MENU_DETAIL a, COMM.SYS_APP_MENU_GUIDE b
                                           WHERE       a.app_menu_id = b.app_menu_id
                                                   AND a.menu_guide_id = b.menu_guide_id
                                         ) F
                                   WHERE G.GUIDE_CODE(+) = S.GUIDE_CODE
                                     and f.dept_code(+)=s.dept_code
                                     and f.link_guide_code(+)=s.guide_code
                                     and f.st_date(+)=s.datetime
                                ORDER BY S.BSC_CLASS,S.SORT_NO", DataUser.HOSPITALSYS, bsc_class, counting, dept_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="counting"></param>
        /// <param name="dept_code"></param>
        /// <param name="bsc_class"></param>
        /// <returns></returns>
        public DataSet GetdeptResultInfoSave2(string counting, string dept_code, string bsc_class, string benginDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"  SELECT   G.GUIDE_CODE,
                                       S.BSC_CLASS_NAME,
                                       S.BSC_CLASS,
                                       G.GUIDE_NAME,
                                       G.EXPLAIN,
                                       ROUND (S.GUIDE_VALUE, 2) AS GUIDE_VALUE,
                                       S.GUIDE_CAUSE AS GUIDE_CAUSE,
                                       ROUND (S.GUIDE_FACT, 2) AS GUIDE_FACT,
                                       ROUND (S.GUIDE_I_VALUE, 2) AS GUIDE_I_VALUE,
                                       ROUND (S.GUIDE_D_VALUE, 2) AS GUIDE_D_VALUE,
                                       ROUND (S.GUIDE_F_VALUE, 2) AS GUIDE_F_VALUE,
                                       CASE WHEN S.GUIDE_F_VALUE >= 0 THEN '1' ELSE '0' END FLAG
                                FROM   (  SELECT   T.*, J.BSC_CLASS_NAME, L.SORT_NO
                                            FROM   HOSPITALSYS.DEPT_ASSESS_RESULT_SAVE T,
                                                   HOSPITALSYS.JXGL_GUIDE_BSC_CLASS_DICT J,
                                                   HOSPITALSYS.GUIDE_NAME_DICT L
                                           WHERE       {1} T.save_id = '{2}'
                                                   AND SUBSTR (T.BSC_CLASS, 1, 2) = J.BSC_CLASS_CODE
                                                   AND T.GUIDE_CODE = L.GUIDE_CODE
                                                   AND T.DEPT_CODE = '{3}'
                                                   AND T.ASSESS_CODE IN (select ASSESS_CODE from hospitalsys.DEPT_ASSESS_INFORMATION_SAVE where save_id='{2}' and start_date='{4}')
                                        ORDER BY   T.DEPT_CODE) S,
                                       HOSPITALSYS.GUIDE_NAME_DICT G
                               WHERE   G.GUIDE_CODE = S.GUIDE_CODE
                            ORDER BY   S.BSC_CLASS, S.SORT_NO", DataUser.HOSPITALSYS, bsc_class, counting, dept_code, benginDate);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 获取存档的考核结果数据
        /// </summary>
        /// <param name="assess_code">考核数据代码</param>
        /// <param name="person_id">人员ID</param>
        /// <param name="bsc_class">BSC分类</param>
        /// <returns>DataSet</returns>
        public DataSet GetResultInfo(string assess_code, string person_id, string bsc_class)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT s.bsc_class_name, s.bsc_class, g.guide_name,g.EXPLAIN,
                                         ROUND (s.guide_value, 2) AS guide_value,
                                         ROUND (s.guide_cause, 2) AS guide_cause,
                                         ROUND (s.guide_fact, 2) AS guide_fact,
                                         ROUND (s.guide_i_value, 2) AS guide_i_value,
                                         ROUND (s.guide_d_value, 2) AS guide_d_value,
                                         ROUND (s.guide_f_value, 2) AS guide_f_value
                                    FROM (SELECT   t.*, j.bsc_class_name
                                              FROM {0}.assess_result t, {0}.jxgl_guide_bsc_class_dict j
                                             WHERE {1} t.assess_code = '{3}'
                                               AND SUBSTR (t.bsc_class, 1, 2) = j.bsc_class_code
                                               AND t.person_id = {2}
                                          ORDER BY t.station_code) s,
                                         {0}.guide_name_dict g
                                   WHERE g.guide_code = s.guide_code
                                ORDER BY s.bsc_class", DataUser.HOSPITALSYS, bsc_class, person_id, assess_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assess_code"></param>
        /// <param name="dept_code"></param>
        /// <param name="bsc_class"></param>
        /// <returns></returns>
        public DataSet GetdeptResultInfo(string assess_code, string dept_code, string bsc_class)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT g.guide_code,s.bsc_class_name, s.bsc_class, g.guide_name,nvl(f.guide_memo,g.EXPLAIN) EXPLAIN,
                                         ROUND (s.guide_value, 2) AS guide_value,
                                         ROUND (s.guide_cause, 2) AS guide_cause,
                                         ROUND (s.guide_fact, 2) AS guide_fact,
                                         ROUND (s.guide_i_value, 2) AS guide_i_value,
                                         ROUND (s.guide_d_value, 2) AS guide_d_value,
                                         ROUND (s.guide_f_value, 2) AS guide_f_value,
                                         CASE WHEN S.GUIDE_F_VALUE >= 0 THEN '1' ELSE '0' END FLAG
                                    FROM (SELECT   t.*, j.bsc_class_name,L.SORT_NO,to_date(m.start_date||'01','yyyymmdd') datetime
                                              FROM {0}.dept_assess_result t, 
                                                   {0}.jxgl_guide_bsc_class_dict j,
                                                   HOSPITALSYS.GUIDE_NAME_DICT L,
                                                   HOSPITALSYS.DEPT_ASSESS_INFORMATION m
                                             WHERE {1} t.assess_code = '{3}'
                                               AND t.assess_code=m.assess_code
                                               AND SUBSTR (t.bsc_class, 1, 2) = j.bsc_class_code
                                               AND T.GUIDE_CODE=L.GUIDE_CODE
                                               AND t.dept_code = {2}
                                          ORDER BY t.dept_code) s,
                                         {0}.guide_name_dict g,
                                        (SELECT   a.st_date,a.dept_code,b.link_guide_code,a.guide_memo
                                                    FROM   HOSPITALSYS.SYS_MENU_DETAIL a, COMM.SYS_APP_MENU_GUIDE b
                                                   WHERE       a.app_menu_id = b.app_menu_id
                                                           AND a.menu_guide_id = b.menu_guide_id
                                                 ) f
                                   WHERE g.guide_code(+)= s.guide_code
                                     and f.dept_code(+)=s.dept_code
                                     and f.link_guide_code(+)=s.guide_code
                                     and f.st_date(+)=s.datetime
                                ORDER BY S.BSC_CLASS,S.SORT_NO", DataUser.HOSPITALSYS, bsc_class, dept_code, assess_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 判断用户是否具备查看指定类别人员的权限
        /// </summary>
        /// <param name="staff_sort">人员类别（干部）</param>
        /// <param name="user_id">用户ID</param>
        /// <returns>DataSet</returns>
        public DataSet GetSortPower(string staff_sort, string user_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT *
                                  FROM {0}.SYS_SPECPOWER_ROLE A,
                                       {0}.SYS_POWER_DETAIL B,
                                       {1}.PERS_SORT_DICT C
                                 WHERE A.ROLE_ID = B.POWER_ID
                                   AND A.TYPE = '1'
                                   AND A.ID = C.SERIAL_NO
                                   AND B.TARGET_ID = TRIM('{2}')
                                   AND C.PERS_SORT_NAME = TRIM('{3}')", DataUser.COMM, DataUser.RLZY, user_id, staff_sort);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 获取收益信息
        /// </summary>
        /// <param name="station_year"></param>
        /// <returns></returns>
        public DataSet GetKSHSXX(string searchdate, string endsearchdate)
        {
            string sql = string.Format(@"SELECT A.ACCOUNT_DEPT_CODE,
                                                  A.ACCOUNT_DEPT_NAME,
                                               B.BB AA,
                                               B.AA-B.BB BB,
                                               B.AA SRZE,
                                               ROUND(DECODE((B.AA),0,0,B.BB/(B.AA)),2) SSZSB,
                                               ROUND(DECODE((B.AA),0,0,(B.AA-B.BB)/(B.AA)),2) JJZSB,
                                               C.CC,
                                               C.DD,
                                               C.CC+C.DD ZCZE,
                                               ROUND(DECODE((B.AA),0,0,(C.CC+C.DD)/(B.AA)),2) ZCZSB,
                                               B.BB-C.CC JSY,
                                               (B.AA)-(C.CC+C.DD) MSY,
                                               ROUND(DECODE(((B.AA)-(C.CC+C.DD)),0,0,(B.BB-C.CC)/((B.AA)-(C.CC+C.DD))),2) MSYB
                                        FROM
                                        (
                                        SELECT A.ACCOUNT_DEPT_CODE,A.ACCOUNT_DEPT_NAME,A.SORT_NO
                                          FROM COMM.SYS_DEPT_DICT A
                                         WHERE A.ATTR='是'
                                           --AND A.dept_snap_date=to_date('{0}','yyyymmdd')
                                        ) A,
                                        (
                                        select B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME,SUM(A.INCOMES) AA,SUM(A.INCOMES_CHARGES) BB
                                          from CBHS.CBHS_INCOMS_INFO_ACCOUNT A,COMM.SYS_DEPT_DICT B
                                         where A.DEPT_CODE = B.DEPT_CODE
                                           --and B.dept_snap_date=to_date('{0}','yyyymmdd')
                                           and A.balance_tag='0'
                                           and A.date_time >= to_date('{0}','yyyymmdd') 
                                           and A.date_time <= last_day(to_date('{1}','yyyymmdd'))
                                         GROUP BY B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME
                                        ) B,
                                        (
                                        select B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME,
                                               SUM(COSTS) CC,
                                               SUM(COSTS_ARMYFREE) DD 
                                          from CBHS.CBHS_DEPT_COST_DETAIL A,COMM.SYS_DEPT_DICT B
                                         where A.DEPT_CODE = B.DEPT_CODE
                                           --AND B.dept_snap_date=to_date('{0}','yyyymmdd')
                                           AND A.accounting_date >= to_date('{0}','yyyymmdd')
                                           AND A.accounting_date <= last_day(to_date('{1}','yyyymmdd'))
                                         GROUP BY B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME
                                        ) C
                                        WHERE A.ACCOUNT_DEPT_CODE = B.ACCOUNT_DEPT_CODE(+)
                                          AND A.ACCOUNT_DEPT_CODE = C.ACCOUNT_DEPT_CODE(+)
                                        ORDER BY A.SORT_NO", searchdate, endsearchdate);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取一类支出
        /// </summary>
        /// <param name="searchdate"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetKSHSXX_YLZC(string searchdate, string deptcode, string endsearchdate)
        {
            string sql = string.Format(@"SELECT C.ITEM_CLASS,
                                               C.ITEM_TYPE,
                                               SUM(COSTS)+SUM(COSTS_ARMYFREE) ZCHJ,
                                               SUM(COSTS) CC,
                                               SUM(COSTS_ARMYFREE) DD
                                          FROM CBHS.CBHS_DEPT_COST_DETAIL A,COMM.SYS_DEPT_DICT B,CBHS.CBHS_COST_ITEM_DICT C
                                         WHERE A.DEPT_CODE = B.DEPT_CODE
                                           AND A.ITEM_CODE = C.ITEM_CODE
                                           --AND B.dept_snap_date=to_date('{0}','yyyymmdd')
                                           AND A.accounting_date >= to_date('{0}','yyyymmdd')
                                           AND A.accounting_date <= last_day(to_date('{2}','yyyymmdd'))
                                           AND B.ACCOUNT_DEPT_CODE='{1}'
                                           
                                         GROUP BY C.ITEM_CLASS,C.ITEM_TYPE
                                         ORDER BY C.ITEM_TYPE", searchdate, deptcode, endsearchdate);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取详细支出
        /// </summary>
        /// <param name="searchdate"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetKSHSXX_XXZC(string searchdate, string deptcode, string endsearchdate)
        {
            string sql = string.Format(@"SELECT C.ITEM_CODE,
                                               C.ITEM_NAME,
                                               SUM(COSTS)+SUM(COSTS_ARMYFREE) ZCHJ,
                                               SUM(COSTS) CC,
                                               SUM(COSTS_ARMYFREE) DD 
                                          FROM CBHS.CBHS_DEPT_COST_DETAIL A,COMM.SYS_DEPT_DICT B,CBHS.CBHS_COST_ITEM_DICT C
                                         WHERE A.DEPT_CODE = B.DEPT_CODE
                                           AND A.ITEM_CODE = C.ITEM_CODE
                                           --AND B.dept_snap_date=to_date('{0}','yyyymmdd')
                                           AND A.accounting_date >= to_date('{0}','yyyymmdd')
                                           AND A.accounting_date <= last_day(to_date('{2}','yyyymmdd'))
                                           AND B.ACCOUNT_DEPT_CODE='{1}'
                                         GROUP BY C.ITEM_CODE,C.ITEM_NAME
                                         ORDER BY C.ITEM_CODE", searchdate, deptcode, endsearchdate);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取报表项目收入
        /// </summary>
        /// <param name="searchdate"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetKSHSXX_BBXMSR(string searchdate, string deptcode, string endsearchdate)
        {
            string sql = string.Format(@"SELECT C.CLASS_TYPE,
                                               C.CLASS_NAME,
                                               SUM(A.INCOMES) SRHJ,
                                               SUM(A.INCOMES_CHARGES) AA,
                                               SUM(A.INCOMES-A.INCOMES_CHARGES) BB
                                          FROM CBHS.CBHS_INCOMS_INFO_ACCOUNT A,COMM.SYS_DEPT_DICT B,CBHS.CBHS_DISTRIBUTION_CALC_SCHM C
                                         WHERE A.DEPT_CODE = B.DEPT_CODE
                                           AND A.RECK_ITEM = C.ITEM_CLASS
                                           --AND B.dept_snap_date=to_date('{0}','yyyymmdd')
                                           AND A.balance_tag='0'
                                           AND A.date_time >= to_date('{0}','yyyymmdd') 
                                           AND A.date_time <= last_day(to_date('{2}','yyyymmdd'))
                                           AND B.ACCOUNT_DEPT_CODE='{1}'
                                         GROUP BY C.CLASS_TYPE,C.CLASS_NAME
                                         ORDER BY C.CLASS_TYPE", searchdate, deptcode, endsearchdate);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取核算项目收入
        /// </summary>
        /// <param name="searchdate"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetKSHSXX_HSXMSR(string searchdate, string deptcode, string endsearchdate)
        {
            string sql = string.Format(@"SELECT C.ITEM_CLASS,
                                               C.ITEM_NAME,
                                               SUM(A.INCOMES) SRHJ,
                                               SUM(A.INCOMES_CHARGES) AA,
                                               SUM(A.INCOMES-A.INCOMES_CHARGES) BB
                                          FROM CBHS.CBHS_INCOMS_INFO_ACCOUNT A,COMM.SYS_DEPT_DICT B,CBHS.CBHS_DISTRIBUTION_CALC_SCHM C
                                         WHERE A.DEPT_CODE = B.DEPT_CODE
                                           AND A.RECK_ITEM = C.ITEM_CLASS
                                           --AND B.dept_snap_date=to_date('{0}','yyyymmdd')
                                           AND A.balance_tag='0'
                                           AND A.date_time >= to_date('{0}','yyyymmdd') 
                                           AND A.date_time <= last_day(to_date('{2}','yyyymmdd'))
                                           AND B.ACCOUNT_DEPT_CODE='{1}'
                                         GROUP BY C.ITEM_CLASS,C.ITEM_NAME
                                         ORDER BY C.ITEM_CLASS", searchdate, deptcode, endsearchdate);
            return OracleOledbBase.ExecuteDataSet(sql);
        }


        /// <summary>
        /// 获取对应科室情况
        /// </summary>
        /// <param name="searchdate"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetKSHSXX_DYKSQK(string searchdate, string deptcode, string endsearchdate)
        {
            string sql = string.Format(@"SELECT A.DEPT_CODE,
                                               A.DEPT_NAME,
                                               B.AA,
                                               B.BB,
                                               B.AA+B.BB SRZE,
                                               ROUND(DECODE((B.AA+B.BB),0,0,B.AA/(B.AA+B.BB)),2) SSZSB,
                                               ROUND(DECODE((B.AA+B.BB),0,0,B.BB/(B.AA+B.BB)),2) JJZSB,
                                               C.CC,
                                               C.DD,
                                               C.CC+C.DD ZCZE,
                                               ROUND(DECODE((B.AA+B.BB),0,0,(C.CC+C.DD)/(B.AA+B.BB)),2) ZCZSB,
                                               B.AA-C.CC JSY,
                                               (B.AA+B.BB)-(C.CC+C.DD) MSY,
                                               ROUND(DECODE((C.CC+C.DD),0,0,C.CC/(C.CC+C.DD)),2) SJZCBZCC,
                                               ROUND(DECODE((C.CC+C.DD),0,0,C.DD/(C.CC+C.DD)),2) JJZCBZCC
                                        FROM
                                        (
                                        SELECT A.DEPT_CODE,A.DEPT_NAME,A.SORT_NO
                                          FROM COMM.SYS_DEPT_DICT A
                                         WHERE --A.dept_snap_date=to_date('{0}','yyyymmdd')
                                           --AND 
                                                 A.ACCOUNT_DEPT_CODE='{1}'
                                        ) A,
                                        (
                                        select B.DEPT_CODE,
                                               B.DEPT_NAME,
                                               SUM(A.INCOMES_CHARGES) AA,
                                               SUM(A.INCOMES-A.INCOMES_CHARGES) BB
                                          from CBHS.CBHS_INCOMS_INFO_ACCOUNT A,COMM.SYS_DEPT_DICT B
                                         where A.DEPT_CODE = B.DEPT_CODE
                                           --AND B.dept_snap_date=to_date('{0}','yyyymmdd')
                                           AND A.balance_tag='0'
                                           AND A.date_time >= to_date('{0}','yyyymmdd') 
                                           AND A.date_time <= last_day(to_date('{2}','yyyymmdd'))
                                           AND B.ACCOUNT_DEPT_CODE='{1}'
                                         GROUP BY B.DEPT_CODE,B.DEPT_NAME
                                        ) B,
                                        (
                                        select B.DEPT_CODE,
                                               B.DEPT_NAME,
                                               SUM(COSTS) CC,
                                               SUM(COSTS_ARMYFREE) DD 
                                          from CBHS.CBHS_DEPT_COST_DETAIL A,COMM.SYS_DEPT_DICT B
                                         where A.DEPT_CODE = B.DEPT_CODE
                                           --AND B.dept_snap_date=to_date('{0}','yyyymmdd')
                                           AND A.accounting_date >= to_date('{0}','yyyymmdd')
                                           AND A.accounting_date <= last_day(to_date('{2}','yyyymmdd'))
                                           AND B.ACCOUNT_DEPT_CODE='{1}'
                                         GROUP BY B.DEPT_CODE,B.DEPT_NAME
                                        ) C
                                        WHERE A.DEPT_CODE = B.DEPT_CODE(+)
                                          AND A.DEPT_CODE = C.DEPT_CODE(+)
                                        ORDER BY A.SORT_NO", searchdate, deptcode, endsearchdate);
            return OracleOledbBase.ExecuteDataSet(sql);
        }
        #endregion  成员方法
    }
}

