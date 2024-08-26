using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
using System.Data.OleDb;

namespace Goldnet.Dal.Properties.Bound
{
    public class OperationDal
    {
        /// <summary>
        /// 查询手术预约根据住院好（住院次数默认查询最大的）
        /// </summary>
        /// <param name="patent_id"></param>
        /// <returns></returns>
        public DataTable GetOerationByPid(string patent_id)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT   to_char(SCHEDULED_DATE_TIME,'yyyy-mm-dd') as SCHEDULED_DATE_TIME,
         PATIENT_ID,
         DIAG_BEFORE_OPERATION AS OPERATION_NAME,
         SURGEON,
         FIRST_ASSISTANT,
         SECOND_ASSISTANT,
         THIRD_ASSISTANT,
         FOURTH_ASSISTANT,
         ANESTHESIA_DOCTOR,
         ANESTHESIA_ASSISTANT,
         FIRST_OPERATION_NURSE,
         SECOND_OPERATION_NURSE,
         FIRST_SUPPLY_NURSE,
         SECOND_SUPPLY_NURSE
  FROM   HISDATA.OPERATION_SCHEDULE
 WHERE   PATIENT_ID = '{0}'
         AND visit_id = (SELECT   MAX (visit_id)
                           FROM   HISDATA.OPERATION_SCHEDULE
                          WHERE   PATIENT_ID = '{0}')", patent_id);

            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }
        public DataTable GetGongMen_Zhen(string date, string end, string item_name)
        {
            StringBuilder sql = new StringBuilder();
            if (item_name.Equals("住院人数"))
            {
                sql.AppendFormat(@" SELECT DEPT_NAME, USER_NAME, SUM(AMOUNT) AMOUNT
                                       FROM HISDATA.V_INP_AMOUNT
                                      WHERE VAE11 BETWEEN TO_DATE('{0}', 'yyyy-mm-dd hh24:mi:ss') AND
                                            TO_DATE('{1}', 'yyyy-mm-dd hh24:mi:ss')
                                      GROUP BY DEPT_NAME, USER_NAME
                                      ORDER BY DEPT_NAME, USER_NAME", date, end);
            }
            if (item_name.Equals("挂号费"))
            {
                sql.AppendFormat(@"SELECT (SELECT DEPT_NAME
                                    FROM HISDATA.DEPT_DICT
                                    WHERE DEPT_CODE = A.ORDERED_BY) DEPT_NAME,
                                (SELECT USER_NAME
                                    FROM HISDATA.USERS
                                    WHERE DB_USER = A.ORDERED_BY_DOCTOR) USER_NAME,
                                SUM(A.AMOUNT) AMOUNT
                            FROM HISDATA.OUTP_BILL_ITEMS A
                            WHERE A.VISIT_DATE BETWEEN TO_DATE('{0}', 'yyyy-mm-dd hh24:mi:ss') AND
                                TO_DATE('{1}', 'yyyy-mm-dd hh24:mi:ss')
                            AND A.CLASS_ON_RECKONING = '35'
                            GROUP BY A.ORDERED_BY, A.ORDERED_BY_DOCTOR
                            ORDER BY A.ORDERED_BY, A.ORDERED_BY_DOCTOR", date, end);
            }
            if (item_name.Equals("实际占床日"))
            {
                sql.AppendFormat(@"SELECT DEPT_NAME, NULL USER_NAME, SUM(AMOUNT) AMOUNT
                                      FROM hisdata.inp_bed
                                     WHERE st_date BETWEEN TO_DATE('{0}', 'yyyy-mm-dd hh24:mi:ss') AND
                                           TO_DATE('{1}', 'yyyy-mm-dd hh24:mi:ss')
                                     GROUP BY DEPT_NAME
                                     ORDER BY DEPT_NAME", date, end);
            }
            //return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];            
            return OracleBase.Query(sql.ToString()).Tables[0];
        }
        public DataTable GetQUANZHONG(string startdate)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT ST_DATE,
                                       NVL(INCOME, 0) INCOME,
                                       NVL(COST, 0) COST,
                                       NVL(INCOME_COST, 0) INCOME_COST,
                                       NVL(JXBL, 0) JXBL,
                                       NVL(GFJX, 0) GFJX,
                                       NVL(YSRS, 0) YSRS,
                                       NVL(HSRS, 0) HSRS,
                                       NVL(YJRS, 0) YJRS,
                                       NVL(YAOJURS, 0) YAOJURS,
                                       NVL(CKRS, 0) CKRS,
                                       NVL(XZRS, 0) XZRS,
                                       NVL(YSXS, 0) YSXS,
                                       NVL(HSXS, 0) HSXS,
                                       NVL(YJXS, 0) YJXS,
                                       NVL(YAOJUXS, 0) YAOJUXS,
                                       NVL(CKXS, 0) CKXS,
                                       NVL(XZXS, 0) XZXS,
                                       NVL(YSBL, 0) YSBL,
                                       NVL(HSBL, 0) HSBL,
                                       NVL(YJBL, 0) YJBL,
                                       NVL(YAOJUBL, 0) YAOJUBL,
                                       NVL(CKBL, 0) CKBL,
                                       NVL(XZBL, 0) XZBL,
                                       NVL(YSZE, 0) YSZE,
                                       NVL(HSZE, 0) HSZE,
                                       NVL(YJZE, 0) YJZE,
                                       NVL(YAOJUZE, 0) YAOJUZE,
                                       NVL(CKZE, 0) CKZE,
                                       NVL(XZZE, 0) XZZE,
                                       NVL(YSXYBL, 0) YSXYBL,
                                       NVL(YSYJBL, 0) YSYJBL,
                                       NVL(HSXYBL, 0) HSXYBL,
                                       NVL(HSYJBL, 0) HSYJBL,
                                       NVL(YJXYBL, 0) YJXYBL,
                                       NVL(YJYJBL, 0) YJYJBL,
                                       NVL(YSXYJE, 0) YSXYJE,
                                       NVL(YSYJEJ, 0) YSYJEJ,
                                       NVL(HSXYJE, 0) HSXYJE,
                                       NVL(HSYJJE, 0) HSYJJE,
                                       NVL(YJXYJE, 0) YJXYJE,
                                       NVL(YJYJJE, 0) YJYJJE，
                                       NVL(HSZCBL,0) HSZCBL,
                                       NVL(HSZCEJ,0) HSZCEJ,
                                       NVL(MZHSJE,0) MZHSJE,
                                       NVL(HLLSBL,0) HLLSBL,
                                       NVL(BLLSJE,0) BLLSJE,

                                        NVL(YLDZE,0) YLDZE,
                                       NVL(YZJCXS,0) YZJCXS,
                                       NVL(YZZXS,0) YZZXS,
                                       NVL(YLDBL,0) YLDBL
                                  FROM HISDATA.QUAN_ZHONG  WHERE ST_DATE = '{0}'", startdate);

            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }
       
        public bool COSTS(string startdate)
        {
            string sqlguide = " SELECT nvl(SUM(COSTS_ARMYFREE + COSTS),0)  FROM CBHS.CBHS_DEPT_COST_DETAIL A WHERE TO_CHAR(ACCOUNTING_DATE, 'yyyymm') ='" + startdate + "'";
            string ds = OracleOledbBase.ExecuteScalar(sqlguide.ToString()).ToString();
            Double aa = Convert.ToDouble(ds);
            if (aa > 0)
            {


                return true;

            }

            return false;

        }
        public void UpdateQUANZHONG(Double JXBL, Double YSRS, Double HSRS, Double YJRS, Double YAOJURS, Double CKRS, Double XZRS, Double YSXS, Double HSXS, Double YJXS, Double YAOJUXS, Double CKXS,
            Double XZXS, Double YZJCXS, Double YZZXS, string stardate)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            StringBuilder sql3 = new StringBuilder();
            StringBuilder sql4 = new StringBuilder();
            Double YSBL = YSXS * YSRS / (YSXS * YSRS + HSXS * HSRS + YAOJUXS * YAOJURS + YJXS * YJRS + CKXS * CKRS + XZXS * XZRS + YZJCXS * YZZXS);
            Double HSBL = HSXS * HSRS / (YSXS * YSRS + HSXS * HSRS + YAOJUXS * YAOJURS + YJXS * YJRS + CKXS * CKRS + XZXS * XZRS + YZJCXS * YZZXS);
            Double YJBL = YJXS * YJRS / (YSXS * YSRS + HSXS * HSRS + YAOJUXS * YAOJURS + YJXS * YJRS + CKXS * CKRS + XZXS * XZRS + YZJCXS * YZZXS);
            Double YLDBL = YZJCXS * YZZXS / (YSXS * YSRS + HSXS * HSRS + YAOJUXS * YAOJURS + YJXS * YJRS + CKXS * CKRS + XZXS * XZRS + YZJCXS * YZZXS);
            Double YAOJUBL = YAOJUXS * YAOJURS / (YSXS * YSRS + HSXS * HSRS + YAOJUXS * YAOJURS + YJXS * YJRS + CKXS * CKRS + XZXS * XZRS + YZJCXS * YZZXS);
            Double CKBL = CKXS * CKRS / (YSXS * YSRS + HSXS * HSRS + YAOJUXS * YAOJURS + YJXS * YJRS + CKXS * CKRS + XZXS * XZRS + YZJCXS * YZZXS);
            Double XZBL = XZXS * XZRS / (YSXS * YSRS + HSXS * HSRS + YAOJUXS * YAOJURS + YJXS * YJRS + CKXS * CKRS + XZXS * XZRS + YZJCXS * YZZXS);
            sql.AppendFormat(@"SELECT   
           SUM (INCOMES) INCOME
    FROM   CBHS.CBHS_INCOMS_INFO_ACCOUNT A, COMM.SYS_DEPT_DICT B
   WHERE       A.DEPT_CODE = B.DEPT_CODE(+)
           AND A.DATE_TIME = TO_DATE ('{0}', 'yyyymm')
           AND A.RECK_ITEM NOT IN
                    ('D14', 'D18', 'E10', 'E12', 'E15', 'E19', 'd20')
           AND A.BALANCE_TAG = '0'
           AND A.RECK_ITEM NOT IN
                    ('D14', 'D18', 'E10', 'E12', 'E15', 'E19', 'd20')", stardate);  
            
            Double INCOME = Convert.ToDouble(OracleOledbBase.ExecuteScalar(sql.ToString()).ToString());
            sql1.AppendFormat(@" 
  SELECT  SUM(costs)COST
           FROM   CBHS.CBHS_DEPT_COST_DETAIL a, COMM.sys_dept_dict b
   WHERE   TO_CHAR(a.ACCOUNTING_DATE, 'yyyymm') = '{0}'
           AND a.DEPT_CODE = b.dept_code
           AND (a.BALANCE_TAG = '0' OR a.BALANCE_TAG IS NULL)
          AND b.dept_code='110000'", stardate);
            Double COST = Convert.ToDouble(OracleOledbBase.ExecuteScalar(sql1.ToString()).ToString());
            Double INCOME_COST = INCOME - COST;
            Double GFJX = INCOME_COST * JXBL;
            Double YSZE = GFJX * YSBL;
            Double HSZE = GFJX * HSBL;
//            sql3.AppendFormat(@" SELECT SUM(A.GUIDEPERCENT) ZC
//  FROM PERFORMANCE.SET_DEPT_BONUSPERCENT A
// WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
//   AND A.DEPT_CODE IN (SELECT DEPT_CODE
//                         FROM COMM.SYS_DEPT_DICT A
//                        WHERE A.OUT_OR_IN = 0
//                          AND (A.DEPT_CODE LIKE '99%' OR dept_code='060001'))
//   AND A.BONUSGUIDE_ID = 2 ", stardate);
//            Double XS = Convert.ToDouble(OracleOledbBase.ExecuteScalar(sql3.ToString()).ToString());
            //sql4.AppendFormat(@" SELECT COUNT(1) FROM rlzy.new_staff_info WHERE dept_code='990018' ", stardate);
            //Double RS = Convert.ToDouble(OracleOledbBase.ExecuteScalar(sql4.ToString()).ToString());            
            Double YJZE = GFJX * YJBL;
            Double YAOJUZE = GFJX * YAOJUBL;
            Double CKZE = GFJX * CKBL;
            Double XZZE = GFJX * XZBL;
            Double YLDZE = GFJX * YLDBL;        
            sql2.AppendFormat(@"INSERT INTO HISDATA.QUAN_ZHONG ( ST_DATE,
                                                               INCOME,
                                                               COST,
                                                               INCOME_COST,
                                                               JXBL,
                                                               GFJX,
                                                               YSRS,
                                                               HSRS,
                                                               YJRS,
                                                               YAOJURS,
                                                               CKRS,
                                                               XZRS,
                                                               YSXS,
                                                               HSXS,
                                                               YJXS,
                                                               YAOJUXS,
                                                               CKXS,
                                                               XZXS,
                                                               YSBL,
                                                               HSBL,
                                                               YJBL,
                                                               YAOJUBL,
                                                               CKBL,
                                                               XZBL,
                                                               YSZE,
                                                               HSZE,
                                                               YJZE,
                                                               YAOJUZE,
                                                               CKZE,
                                                               XZZE,
                                                               YLDZE,
                                                               YZJCXS,
                                                               YZZXS,
                                                               YLDBL) VALUES 
                                                               ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}'
                                                               ,'{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}'
                                                               ,'{27}','{28}','{29}','{30}','{31}','{32}','{33}') ", stardate, INCOME, COST, INCOME_COST, JXBL, GFJX, YSRS, HSRS, YJRS, YAOJURS,
                                                                                        CKRS, XZRS, YSXS, HSXS, YJXS, YAOJUXS, CKXS, XZXS, YSBL, HSBL, YJBL, YAOJUBL, CKBL,
                                                                                        XZBL, YSZE, HSZE, YJZE, YAOJUZE, CKZE, XZZE, YLDZE, YZJCXS, YZZXS, YLDBL);
            OracleOledbBase.ExecuteNonQuery(sql2.ToString());


        }
        public void deleteQUANZHONG(string stardate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"DELETE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE='{0}' ", stardate);
            OracleOledbBase.ExecuteNonQuery(sql.ToString());
        }
        public DataTable GetJieJiaRi_OUP(string date)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder strDel = new StringBuilder();
            sql.AppendFormat(@"SELECT (SELECT USER_NAME
          FROM HISDATA.USERS
         WHERE DB_USER = ORDERED_BY_DOCTOR) USER_NAME,
       SUM(MZZCF) ZCF
  FROM (SELECT ORDERED_BY_DOCTOR, SUM(A.COSTS) * 0.2 MZZCF
          FROM HISFACT.OUTP_CLASS2_INCOME A
         WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
           AND A.RECK_CLASS = '36'
           AND A.ORDERED_BY_DOCTOR IN  ('1032', '1178', '1046','1200','1311')
         GROUP BY ORDERED_BY_DOCTOR
        UNION ALL
        SELECT USER_ID, SUM(A.COSTS) * 0.3 MZJR
          FROM HISDATA.JIEJIRI_OUP A
         WHERE A.ST_DATE = '{0}'
           AND A.USER_ID IN  ('1032', '1178', '1046','1200','1311')
         GROUP BY USER_ID
        UNION ALL
        SELECT ORDERED_BY_DOCTOR, SUM(HZF) HZF
          FROM (SELECT ORDERED_BY_DOCTOR, SUM(A.COSTS) * 0.2 HZF
                  FROM HISFACT.OUTP_CLASS2_INCOME A, COMM.SYS_DEPT_INFO B
                 WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
                   AND TO_CHAR(B.DEPT_SNAP_DATE, 'yyyymm') = '{0}'
                   AND A.ORDERED_BY_DEPT = B.DEPT_CODE
                   AND A.RECK_CLASS = '26'
                   AND A.ORDERED_BY_DOCTOR IN  ('1032', '1178', '1046','1200','1311')
                 GROUP BY ORDERED_BY_DOCTOR
                UNION ALL
                SELECT ORDERED_BY_DOCTOR, SUM(A.COSTS) * 0.2 HZF
                  FROM HISFACT.INP_CLASS2_INCOME A, COMM.SYS_DEPT_INFO B
                 WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
                   AND TO_CHAR(B.DEPT_SNAP_DATE, 'yyyymm') = '{0}'
                   AND A.ORDERED_BY_DEPT = B.DEPT_CODE
                   AND A.RECK_CLASS = '26'
                   AND A.ORDERED_BY_DOCTOR IN  ('1032', '1178', '1046','1200','1311')
                 GROUP BY ORDERED_BY_DOCTOR)
         GROUP BY ORDERED_BY_DOCTOR)
 GROUP BY ORDERED_BY_DOCTOR", date);
            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }
        public DataTable GetJGongZuo_ChaXun()
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder strDel = new StringBuilder();
            sql.AppendFormat(@"SELECT DEPT_CODE,
       DEPT_NAME,
       CLASS_NAME,
       ITEM_CODE,
       ITEM_NAME,
       ITEM_UNIT,
       ITEM_PRICE,
       PANDU,
       ZHIXING
  FROM HISDATA.LEIBIE
 ORDER BY DEPT_CODE, CLASS_NAME, ITEM_PRICE");
            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }
        public DataTable GetZhenCha_ChaXun(string date, string dept_code)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"SELECT (SELECT DEPT_NAME
          FROM COMM.SYS_DEPT_DICT
         WHERE DEPT_CODE = AA.DEPT_CODE) DEPT_NAME,
       (SELECT USER_NAME FROM HISDATA.USERS WHERE DB_USER = AA.USER_ID) USER_NAME,
       SUM(MZZCF) MZZCF,
       SUM(JRZCF) JRZCF,
       SUM(HZF) HZF,
       SUM(MZZCF) + SUM(JRZCF) + SUM(HZF) HJ
  FROM (SELECT A.ORDERED_BY_DEPT DEPT_CODE,
               A.ORDERED_BY_DOCTOR USER_ID,
               SUM(NVL(A.COSTS, 0)) * 0.2 MZZCF,
               0 JRZCF,
               0 HZF
          FROM HISFACT.OUTP_CLASS2_INCOME A
         WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
           AND A.RECK_CLASS = '36'
           AND A.ORDERED_BY_DOCTOR NOT IN
               (SELECT USER_ID FROM HISDATA.NOT_JX)
         GROUP BY ORDERED_BY_DEPT, A.ORDERED_BY_DOCTOR
        UNION ALL
        SELECT A.DEPT_CODE DEPT_CODE,
               A.USER_ID,
               0 MZZCF,
               SUM(NVL(A.COSTS, 0)) * 0.3 JRZCF,
               0 HZF
          FROM HISDATA.JIEJIRI_OUP A
         WHERE A.ST_DATE = '{0}'
           AND A.USER_ID NOT IN (SELECT USER_ID FROM HISDATA.NOT_JX)
           AND A.DEPT_CODE <> '020024'
         GROUP BY A.DEPT_CODE, A.USER_ID
        UNION ALL
        SELECT DEPT_CODE, ORDERED_BY_DOCTOR, 0, 0, SUM(HZF) HZF
          FROM (SELECT A.ORDERED_BY_DEPT DEPT_CODE,
                       SUM(NVL(A.COSTS, 0)) * 0.2 HZF,
                       ORDERED_BY_DOCTOR
                  FROM HISFACT.OUTP_CLASS2_INCOME A
                 WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
                   AND A.RECK_CLASS = '26'
                   AND A.ORDERED_BY_DOCTOR NOT IN
                       (SELECT USER_ID FROM HISDATA.NOT_JX)
                 GROUP BY ORDERED_BY_DEPT, ORDERED_BY_DOCTOR
                UNION ALL
                SELECT A.ORDERED_BY_DEPT DEPT_CODE,
                       SUM(NVL(A.COSTS, 0)) * 0.2 HZF,
                       ORDERED_BY_DOCTOR
                  FROM HISFACT.INP_CLASS2_INCOME A
                 WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
                   AND A.RECK_CLASS = '26'
                   AND A.ORDERED_BY_DOCTOR NOT IN
                       (SELECT USER_ID FROM HISDATA.NOT_JX)
                 GROUP BY ORDERED_BY_DEPT, ORDERED_BY_DOCTOR) A
         GROUP BY DEPT_CODE, ORDERED_BY_DOCTOR) AA
 WHERE DEPT_CODE =
       (SELECT DEPT_CODE FROM RLZY.NEW_STAFF_INFO WHERE USER_ID = '{1}')
 GROUP BY DEPT_CODE, USER_ID
 ORDER BY DEPT_CODE", date, dept_code);
            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];

        }
        public DataTable GetHL_ZYJS(string date, string end, string dept_code)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"SELECT (SELECT DEPT_NAME
                                  FROM COMM.SYS_DEPT_DICT
                                 WHERE DEPT_CODE = AA.DEPT_CODE1) DEPT_NAME,
                               SUM(COSTS) COSTS,
                               SUM(AMOUNT) AMOUNT,
                               ITEM_CODE,
                               ITEM_NAME,
                               来源 LY
                          FROM (SELECT CASE
                                         WHEN ORDERED_BY = '0040000012' THEN
                                          '990004'
                                         WHEN ORDERED_BY = '020013' THEN
                                          '990010'
                                         WHEN ORDERED_BY IN ('020014',
                                                             '020020',
                                                             '020017',
                                                             '020043',
                                                             '200450',
                                                             '040004') THEN
                                          '990008'
                                         WHEN ORDERED_BY = '020018' THEN
                                          '990001'
                                         WHEN ORDERED_BY = '020019' THEN
                                          '990002'
                                         WHEN ORDERED_BY = '020040' THEN
                                          '990003'
                                       END DEPT_CODE1,
                                       SUM(COSTS) COSTS,
                                       SUM(AMOUNT) AMOUNT,
                                       ITEM_CODE,
                                       ITEM_NAME,
                                       来源
                                  FROM (SELECT A.ORDERED_BY ORDERED_BY,
                                               SUM(A.COSTS) COSTS,
                                               SUM(A.AMOUNT) AMOUNT,
                                               B.ITEM_CODE,
                                               B.ITEM_NAME,
                                               '住院' 来源
                                          FROM HISDATA.INP_BILL_DETAIL A, HISDATA.HUSHI_GZL B
                                         WHERE A.ITEM_CODE = B.ITEM_CODE
                                           AND TO_CHAR(BILLING_DATE_TIME, 'yyyymmdd') >= '{0}'
                                           AND TO_CHAR(BILLING_DATE_TIME, 'yyyymmdd') < '{1}'
                                         GROUP BY A.ORDERED_BY, B.ITEM_CODE, B.ITEM_NAME
                                        UNION ALL
                                        SELECT C.ORDERED_BY,
                                               SUM(C.COSTS) COSTS,
                                               SUM(C.AMOUNT) AMOUNT,
                                               B.ITEM_CODE,
                                               B.ITEM_NAME,
                                               '门诊' 来源
                                          FROM HISDATA.OUTP_BILL_ITEMS C, HISDATA.HUSHI_GZL B
                                         WHERE C.ITEM_NAME = B.ITEM_CODE
                                           AND TO_CHAR(C.VISIT_DATE, 'yyyymmdd') >= '{0}'
                                           AND TO_CHAR(C.VISIT_DATE, 'yyyymmdd') < '{1}'
                                         GROUP BY C.ORDERED_BY, B.ITEM_CODE, B.ITEM_NAME)
                                 GROUP BY ORDERED_BY, ITEM_CODE, ITEM_NAME, 来源) AA
                         WHERE DEPT_CODE1 IS NOT NULL
                           AND DEPT_CODE1 LIKE '{2}%'
                         GROUP BY DEPT_CODE1, ITEM_CODE, ITEM_NAME, 来源
                         ORDER BY DEPT_CODE1, 来源", date, end, dept_code);
            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }
        public DataTable GetWZ_KC(string dept_code)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"  SELECT ST_DATE,
         DEPT_NAME,
         ITEM_CODE,
         ITEM_NAME,
         ITEM_SPEC,
         UNITS,
         SUM (QC_AMOUNT) QC_AMOUNT,
         SUM (INP_AMOUNT) INP_AMOUNT,
         SUM (OUT_AMOUNT) OUT_AMOUNT,
         SUM (AMOUNT) AMOUNT,
         CLASS_NAME
    FROM (SELECT T.ST_DATE,
                 (SELECT ACCOUNT_DEPT_NAME DEPT_NAME
                    FROM COMM.SYS_DEPT_DICT
                   WHERE DEPT_CODE = T.DEPT_CODE)
                    DEPT_NAME,
                 T.ITEM_CODE,
                 (SELECT ITEM_NAME
                    FROM HISDATA.PRICE_LIST
                   WHERE ITEM_CODE = T.ITEM_CODE)
                    ITEM_NAME,
                 (SELECT ITEM_SPEC
                    FROM HISDATA.PRICE_LIST
                   WHERE ITEM_CODE = T.ITEM_CODE)
                    ITEM_SPEC,
                 (SELECT UNITS
                    FROM HISDATA.PRICE_LIST
                   WHERE ITEM_CODE = T.ITEM_CODE)
                    UNITS,
                 T.QC_AMOUNT,
                 T.INP_AMOUNT,
                 T.OUT_AMOUNT,
                 T.AMOUNT,
                 (SELECT CLASS_NAME
                    FROM HISDATA.ITEM_DICT_VIEW
                   WHERE ITEM_CODE = T.ITEM_CODE)
                    CLASS_NAME
            FROM HISDATA.WULIU_KC T
           WHERE T.ST_DATE = TO_CHAR (SYSDATE - 1, 'yyyymmdd')
           AND DEPT_CODE IN (SELECT DEPT_CODE
                         FROM COMM.SYS_DEPT_DICT
                        WHERE ACCOUNT_DEPT_NAME LIKE '{0}%')) T
   
GROUP BY ST_DATE,
         DEPT_NAME,
         ITEM_CODE,
         ITEM_NAME,
         ITEM_SPEC,
         UNITS,
         CLASS_NAME
ORDER BY CLASS_NAME, DEPT_NAME", dept_code);


            return OracleBase.Query(sql.ToString()).Tables[0];
        }
        public DataTable GetWZ_KC_QJ(string begin_date,string end_date,string dept_code)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"WITH QC AS
 (SELECT ITEM_CODE,
         QC_AMOUNT,
         INP_AMOUNT,
         OUT_AMOUNT,
         AMOUNT,
         DEPT_CODE,
         (SELECT ITEM_NAME
            FROM HISDATA.PRICE_LIST
           WHERE ITEM_CODE = A.ITEM_CODE) ITEM_NAME,
         (SELECT ITEM_SPEC
            FROM HISDATA.PRICE_LIST
           WHERE ITEM_CODE = A.ITEM_CODE) ITEM_SPEC,
         (SELECT UNITS FROM HISDATA.PRICE_LIST WHERE ITEM_CODE = A.ITEM_CODE) UNITS,
         (SELECT CLASS_NAME
            FROM HISDATA.ITEM_DICT_VIEW
           WHERE ITEM_CODE = A.ITEM_CODE) CLASS_NAME
    FROM HISDATA.WULIU_KC A
   WHERE DEPT_CODE IN (SELECT DEPT_CODE
                         FROM COMM.SYS_DEPT_DICT
                        WHERE ACCOUNT_DEPT_NAME LIKE '{2}%')
     AND ST_DATE = '{0}'),
QM AS
 (SELECT ITEM_CODE,
         QC_AMOUNT,
         INP_AMOUNT,
         OUT_AMOUNT,
         AMOUNT,
         DEPT_CODE,
         (SELECT ITEM_NAME
            FROM HISDATA.PRICE_LIST
           WHERE ITEM_CODE = A.ITEM_CODE) ITEM_NAME,
         (SELECT ITEM_SPEC
            FROM HISDATA.PRICE_LIST
           WHERE ITEM_CODE = A.ITEM_CODE) ITEM_SPEC,
         (SELECT UNITS FROM HISDATA.PRICE_LIST WHERE ITEM_CODE = A.ITEM_CODE) UNITS,
         (SELECT CLASS_NAME
            FROM HISDATA.ITEM_DICT_VIEW
           WHERE ITEM_CODE = A.ITEM_CODE) CLASS_NAME
    FROM HISDATA.WULIU_KC A
   WHERE DEPT_CODE IN (SELECT DEPT_CODE
                         FROM COMM.SYS_DEPT_DICT
                        WHERE ACCOUNT_DEPT_NAME LIKE '{2}%')
     AND ST_DATE = '{1}'),
QJ AS
 (SELECT ITEM_CODE,
         DEPT_CODE,
         SUM(QC_AMOUNT) QC_AMOUNT,
         SUM(INP_AMOUNT) INP_AMOUNT,
         SUM(OUT_AMOUNT) OUT_AMOUNT,
         SUM(AMOUNT) AMOUNT
    FROM HISDATA.WULIU_KC A
   WHERE DEPT_CODE IN (SELECT DEPT_CODE
                         FROM COMM.SYS_DEPT_DICT
                        WHERE ACCOUNT_DEPT_NAME LIKE '{2}%')
     AND ST_DATE < '{1}'
     AND ST_DATE > '{0}'
   GROUP BY ITEM_CODE,DEPT_CODE)
SELECT '{0}' GEGIN_DATE,
       '{1}' END_DATE,NVL(T1.DEPT_CODE, T2.DEPT_CODE),
       (SELECT DEPT_NAME  FROM COMM.SYS_DEPT_DICT AA WHERE AA.DEPT_CODE=NVL(T1.DEPT_CODE, T2.DEPT_CODE)) DEPT_NAME,
       NVL(T1.ITEM_CODE, T2.ITEM_CODE) ITEM_CODE,
       NVL(T1.ITEM_NAME, T2.ITEM_NAME) ITEM_NAME,
       NVL(T1.ITEM_SPEC, T2.ITEM_SPEC) ITEM_SPEC,
       NVL(T1.UNITS, T2.UNITS) UNITS,
       NVL(T2.QC_AMOUNT, 0) QC_AMOUNT,
       NVL(T1.INP_AMOUNT, 0) + NVL(T2.INP_AMOUNT, 0) +
       NVL((SELECT SUM(INP_AMOUNT)
             FROM QJ AA
            WHERE AA.ITEM_CODE = NVL(T1.ITEM_CODE, T2.ITEM_CODE)
            AND AA.DEPT_CODE = NVL(T1.DEPT_CODE, T2.DEPT_CODE)),
           0) INP_AMOUNT,
       NVL(T1.OUT_AMOUNT, 0) + NVL(T2.OUT_AMOUNT, 0) +
       NVL((SELECT SUM(OUT_AMOUNT)
             FROM QJ AA
            WHERE AA.ITEM_CODE = NVL(T1.ITEM_CODE, T2.ITEM_CODE)
            AND AA.DEPT_CODE = NVL(T1.DEPT_CODE, T2.DEPT_CODE)),
           0) OUT_AMOUNT,
       NVL(T1.AMOUNT, 0) AMOUNT,
       NVL(T1.CLASS_NAME, T2.CLASS_NAME) CLASS_NAME
  FROM QC T2
  FULL JOIN QM T1
    ON T1.ITEM_CODE = T2.ITEM_CODE    
    AND T1.DEPT_CODE=T2.DEPT_CODE
", begin_date, end_date, dept_code);


            return OracleBase.Query(sql.ToString()).Tables[0];
        }
        public DataTable GetWZ_KC_QJ_QY(string begin_date, string end_date)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"
WITH QC AS
 (SELECT ITEM_CODE,
         QC_AMOUNT,
         INP_AMOUNT,
         OUT_AMOUNT,
         AMOUNT,
         DEPT_CODE,
         (SELECT ITEM_NAME
            FROM HISDATA.PRICE_LIST
           WHERE ITEM_CODE = A.ITEM_CODE) ITEM_NAME,
         (SELECT ITEM_SPEC
            FROM HISDATA.PRICE_LIST
           WHERE ITEM_CODE = A.ITEM_CODE) ITEM_SPEC,
         (SELECT UNITS FROM HISDATA.PRICE_LIST WHERE ITEM_CODE = A.ITEM_CODE) UNITS,
         (SELECT CLASS_NAME
            FROM HISDATA.ITEM_DICT_VIEW
           WHERE ITEM_CODE = A.ITEM_CODE) CLASS_NAME
    FROM HISDATA.WULIU_KC A
   WHERE  ST_DATE = '{0}'),
QM AS
 (SELECT ITEM_CODE,
         QC_AMOUNT,
         INP_AMOUNT,
         OUT_AMOUNT,
         AMOUNT,
         DEPT_CODE,
         (SELECT ITEM_NAME
            FROM HISDATA.PRICE_LIST
           WHERE ITEM_CODE = A.ITEM_CODE) ITEM_NAME,
         (SELECT ITEM_SPEC
            FROM HISDATA.PRICE_LIST
           WHERE ITEM_CODE = A.ITEM_CODE) ITEM_SPEC,
         (SELECT UNITS FROM HISDATA.PRICE_LIST WHERE ITEM_CODE = A.ITEM_CODE) UNITS,
         (SELECT CLASS_NAME
            FROM HISDATA.ITEM_DICT_VIEW
           WHERE ITEM_CODE = A.ITEM_CODE) CLASS_NAME
    FROM HISDATA.WULIU_KC A
   WHERE  ST_DATE = '{1}'),
QJ AS
 (SELECT ITEM_CODE,
         DEPT_CODE,
         SUM(QC_AMOUNT) QC_AMOUNT,
         SUM(INP_AMOUNT) INP_AMOUNT,
         SUM(OUT_AMOUNT) OUT_AMOUNT,
         SUM(AMOUNT) AMOUNT
    FROM HISDATA.WULIU_KC A
   WHERE ST_DATE < '{0}'
     AND ST_DATE > '{1}'
   GROUP BY ITEM_CODE,DEPT_CODE)
SELECT  '{0}' GEGIN_DATE,
        '{1}' END_DATE ,NVL(T1.DEPT_CODE, T2.DEPT_CODE),
       (SELECT DEPT_NAME  FROM COMM.SYS_DEPT_DICT AA WHERE AA.DEPT_CODE=NVL(T1.DEPT_CODE, T2.DEPT_CODE)) DEPT_NAME,
       NVL(T1.ITEM_CODE, T2.ITEM_CODE) ITEM_CODE,
       NVL(T1.ITEM_NAME, T2.ITEM_NAME) ITEM_NAME,
       NVL(T1.ITEM_SPEC, T2.ITEM_SPEC) ITEM_SPEC,
       NVL(T1.UNITS, T2.UNITS) UNITS,
       NVL(T2.QC_AMOUNT, 0) QC_AMOUNT,
       NVL(T1.INP_AMOUNT, 0) + NVL(T2.INP_AMOUNT, 0) +
       NVL((SELECT SUM(INP_AMOUNT)
             FROM QJ AA
            WHERE AA.ITEM_CODE = NVL(T1.ITEM_CODE, T2.ITEM_CODE)
            AND AA.DEPT_CODE = NVL(T1.DEPT_CODE, T2.DEPT_CODE)),
           0) INP_AMOUNT,
       NVL(T1.OUT_AMOUNT, 0) + NVL(T2.OUT_AMOUNT, 0) +
       NVL((SELECT SUM(OUT_AMOUNT)
             FROM QJ AA
            WHERE AA.ITEM_CODE = NVL(T1.ITEM_CODE, T2.ITEM_CODE)
            AND AA.DEPT_CODE = NVL(T1.DEPT_CODE, T2.DEPT_CODE)),
           0) OUT_AMOUNT,
       NVL(T1.AMOUNT, 0) AMOUNT,
       NVL(T1.CLASS_NAME, T2.CLASS_NAME) CLASS_NAME
  FROM QC T2
  FULL JOIN QM T1
    ON T1.ITEM_CODE = T2.ITEM_CODE
    AND T1.DEPT_CODE=T2.DEPT_CODE", begin_date, end_date);


            return OracleBase.Query(sql.ToString()).Tables[0];
        }
    }
}
