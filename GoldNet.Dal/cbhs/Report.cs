using System;
using System.Text;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    /// <summary>
    /// 报表查询
    /// </summary>
    public class Report
    {
        /// <summary>
        /// 核算科目医疗收入
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="balance">0 发生，1实现</param>
        /// <param name="deptcode">科室</param>
        /// <returns></returns>
        public DataTable GetAccountSubjectIncome(string begindate, string enddate, string balance, string deptcode, string hospro)
        {
            if (hospro == "0")
            {
                // string sql = "with totalReck as (select reck_item,  ";

                //
                string str = string.Format(@"select CASE WHEN GROUPING (ITEM_NAME) = 1 AND GROUPING (CLASS_NAME) = 1 THEN ' 合计' else class_name end class_name,
CASE WHEN GROUPING (ITEM_NAME) = 1 AND GROUPING (CLASS_NAME) = 0 THEN CLASS_NAME||'' else  '　　' || ITEM_NAME end item_name,sum(accountincomes) accountincomes,
sum(facaccountincomes) facaccountincomes,
sum(armaccountincomes) armaccountincomes,
sum(hrealincomes) hrealincomes,
sum(hvalationincomes) hvalationincomes,
sum(crealincomes) crealincomes,
sum(cvalationincomes) cvalationincomes
from (
  SELECT   reck_item,B.ITEM_NAME, B.CLASS_TYPE||B.CLASS_NAME class_name,
           NVL (
              (  SUM (NVL (incomes, 0))
               - SUM (NVL (incomes_charges, 0))
               + SUM (NVL (incomes_charges, 0))),
              0
           )
              accountincomes,
           NVL (SUM (NVL (incomes_charges, 0)), 0) facaccountincomes,
           NVL (SUM (NVL (incomes, 0) - NVL (incomes_charges, 0)), 0)
              armaccountincomes,
           NVL (
              SUM (CASE WHEN costs_tag = '0' THEN NVL (incomes_charges, 0) END),
              0
           )
              AS hrealincomes,
           NVL (
              SUM(CASE
                     WHEN costs_tag = '0'
                     THEN
                        NVL (incomes, 0) - NVL (incomes_charges, 0)
                  END),
              0
           )
              AS hvalationincomes,
           NVL (
              SUM (CASE WHEN costs_tag = '1' THEN NVL (incomes_charges, 0) END),
              0
           )
              AS crealincomes,
           NVL (
              SUM(CASE
                     WHEN costs_tag = '1'
                     THEN
                        NVL (incomes, 0) - NVL (incomes_charges, 0)
                  END),
              0
           )
              AS cvalationincomes
                FROM   cbhs.CBHS_INCOMS_INFO_ACCOUNT a,cbhs.CBHS_DISTRIBUTION_CALC_SCHM b
               WHERE       (a.balance_tag = '{0}') and A.RECK_ITEM=B.ITEM_CLASS
                       AND date_time >= TO_DATE ('{1}', 'yyyymmdd')
                       AND date_time < ADD_MONTHS (TO_DATE ('{2}', 'yyyymmdd'), 1) {3}
            GROUP BY   reck_item,B.ITEM_NAME, B.CLASS_TYPE,B.CLASS_NAME
            )
            group by rollup ( CLASS_NAME,ITEM_NAME)
            order by CLASS_NAME,ITEM_NAME", balance, begindate, enddate, deptcode);
                DataSet ds = OracleOledbBase.ExecuteDataSet(str);

                return ds.Tables[0];
            }
            else
            {
                StringBuilder str = new StringBuilder();

                // string sql = "with totalReck as (select reck_item,  ";
                str.Append("select reck_item,  ");
                str.Append(" nvl(sum(nvl(incomes_charges,0)),0) accountincomes, ");
                str.Append(" nvl(sum(nvl(incomes_charges,0)),0) facaccountincomes,");
                str.Append(" nvl(sum(nvl(incomes,0)-nvl(incomes_charges,0)),0) armaccountincomes, ");
                str.Append(" nvl(sum(case when costs_tag='0' then nvl(incomes_charges,0) end),0) as hrealincomes, ");
                str.Append(" nvl(sum(case when costs_tag='0' then nvl(incomes,0)-nvl(incomes_charges,0) end),0) as hvalationincomes, ");
                str.Append(" nvl(sum(case when costs_tag='1' then nvl(incomes_charges,0) end),0) as crealincomes,");
                str.Append(" nvl(sum(case when costs_tag='1' then nvl(incomes,0)-nvl(incomes_charges,0) end),0) as cvalationincomes ");
                str.Append(" from cbhs.CBHS_INCOMS_INFO a where ");
                //str.Append( " dept_code in (select dept_code from comm.SYS_DEPT_DICT where show_flag=0)");
                str.AppendFormat("  (a.balance_tag='{0}')  ", balance);
                str.AppendFormat(" and date_time>=to_date('{0}','yyyymmdd') and  date_time<add_months(to_date('{1}','yyyymmdd'),1)  {2}", begindate, enddate, deptcode);
                str.Append(" group by reck_item order by reck_item ");

                string sql = " select  ";
                sql += " bb.ID,bb.class_code,bb.class_name, ";
                sql += " nvl(aa.accountincomes,0) accountincomes, ";
                sql += " nvl(aa.facaccountincomes,0) facaccountincomes, ";
                sql += " nvl(aa.armaccountincomes,0) armaccountincomes, ";
                sql += " nvl(aa.hrealincomes,0) hrealincomes, ";
                sql += " nvl(aa.hvalationincomes,0) hvalationincomes, ";
                sql += " nvl(aa.crealincomes,0) crealincomes, ";
                sql += " nvl(aa.cvalationincomes,0) cvalationincomes ";
                sql += "  from  ";
                sql += " (select 'ALL' as reck_item,sum(accountincomes) accountincomes, ";
                sql += " sum(facaccountincomes) facaccountincomes,sum(armaccountincomes) armaccountincomes, ";
                sql += " sum(hrealincomes) hrealincomes,sum(hvalationincomes) hvalationincomes, ";
                sql += " sum(crealincomes) crealincomes,sum(cvalationincomes) cvalationincomes ";
                sql += " from  (" + str.ToString() + ") ";
                sql += " union all ";
                sql += " select reck_item,sum(accountincomes) accountincomes, ";
                sql += " sum(facaccountincomes) facaccountincomes,sum(armaccountincomes) armaccountincomes, ";
                sql += " sum(hrealincomes) hrealincomes,sum(hvalationincomes) hvalationincomes, ";
                sql += " sum(crealincomes) crealincomes,sum(cvalationincomes) cvalationincomes ";
                sql += " from ( ";
                sql += " select  substr(RECK_ITEM,1,1) reck_item,sum(accountincomes) accountincomes, ";
                sql += " sum(facaccountincomes) facaccountincomes,sum(armaccountincomes) armaccountincomes, ";
                sql += " sum(hrealincomes) hrealincomes,sum(hvalationincomes) hvalationincomes, ";
                sql += " sum(crealincomes) crealincomes,sum(cvalationincomes) cvalationincomes ";
                sql += " from (" + str.ToString() + ") group by substr(RECK_ITEM,1,1) ";
                sql += " union all ";
                sql += " select  reck_item,sum(accountincomes) accountincomes, ";
                sql += " sum(facaccountincomes) facaccountincomes,sum(armaccountincomes) armaccountincomes, ";
                sql += " sum(hrealincomes) hrealincomes,sum(hvalationincomes) hvalationincomes, ";
                sql += " sum(crealincomes) crealincomes,sum(cvalationincomes) cvalationincomes ";
                sql += " from (" + str.ToString() + ") group by reck_item )  group by reck_item) aa, ";
                sql += " ( ";
                sql += " SELECT 0 ID, 'ALL' class_code, '合计' class_name ";
                sql += "   FROM DUAL ";
                sql += " UNION ALL ";
                sql += " SELECT ROWNUM ID, d.* ";
                sql += "   FROM (SELECT   class_code,  ";
                sql += "         case when length(class_code)>1 then  '　　' || class_name else class_name end as class_name ";
                sql += string.Format("             FROM {0}.reck_item_class_dict ", DataUser.HISFACT);
                sql += "         ORDER BY class_code) d ";
                sql += " union all ";
                sql += " SELECT 999 ID, '*' class_code, '*' class_name ";
                sql += "   FROM DUAL ";
                sql += " ) bb ";
                sql += " where aa.reck_item(+)=bb.class_code order by bb.id ";
                DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 查询门诊及住院毛收入（核算前）
        /// </summary>
        /// <param name="begindate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="balance">收付或者发生</param>
        /// <param name="deptcode">科室编码</param>
        /// <param name="incomestype">开单或执行</param>
        /// <returns></returns>
        public DataTable GetOutInp_Income(string begindate, string enddate, string balance, string deptcode, string incomestype)
        {
            string in_tablename = "";
            if (balance == "0")
                in_tablename = "HISFACT.INP_CLASS2_INCOME";
            else
                in_tablename = "HISFACT.INP_CLASS2_INCOME_SETTLE";

            string intype = "";
            if (incomestype == "0")
                intype = "ORDERED_BY_DEPT";
            else
                intype = "PERFORMED_BY";

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"
  SELECT C.ACCOUNT_DEPT_NAME,
       C.ACCOUNT_DEPT_CODE,
       SUM(DECODE(RECK_CLASS, '01', COSTS, 0)) XY,
       SUM(DECODE(RECK_CLASS, '02', COSTS, 0)) ZCY,
       SUM(DECODE(RECK_CLASS, '51000101', COSTS, 0)) YNZJ,
       SUM(DECODE(RECK_CLASS, '01', COSTS, 0)) +
       SUM(DECODE(RECK_CLASS, '02', COSTS, 0)) +
       SUM(DECODE(RECK_CLASS, '51000101', COSTS, 0)) XYHJ,
       SUM(DECODE(RECK_CLASS, '03', COSTS, 0)) ZYP,
       SUM(DECODE(RECK_CLASS, '04', COSTS, 0)) XYP,
       SUM(DECODE(RECK_CLASS, '05', COSTS, 0)) FJMJ,
       SUM(DECODE(RECK_CLASS, '47', COSTS, 0)) PFKL,
       SUM(DECODE(RECK_CLASS, '51', COSTS, 0)) YPJP,
       SUM(DECODE(RECK_CLASS, '03', COSTS, 0)) +
       SUM(DECODE(RECK_CLASS, '04', COSTS, 0)) +
       SUM(DECODE(RECK_CLASS, '05', COSTS, 0)) +
       SUM(DECODE(RECK_CLASS, '47', COSTS, 0)) +
       SUM(DECODE(RECK_CLASS, '51', COSTS, 0)) ZYHJ
    FROM   (SELECT   ST_DATE,
                     RECK_CLASS,
                     {3} dept_code_by,
                     COSTS
              FROM   HISFACT.OUTP_CLASS2_INCOME
             WHERE   TO_CHAR (ST_DATE, 'YYYYMM') >= '{0}'
                     AND TO_CHAR (ST_DATE, 'YYYYMM') <= '{1}'
            UNION ALL
            SELECT   ST_DATE,
                     RECK_CLASS,
                     {3} dept_code_by,
                     COSTS
              FROM   {2}
             WHERE   TO_CHAR (ST_DATE, 'YYYYMM') >= '{0}'
                     AND TO_CHAR (ST_DATE, 'YYYYMM') <= '{1}') A,
           hisdata.RECK_ITEM_CLASS_DICT B,
           COMM.SYS_DEPT_dict C
   WHERE       A.RECK_CLASS = B.CLASS_CODE(+)
           AND A.dept_code_by = C.DEPT_CODE(+)
    AND A.DEPT_CODE_BY IN (SELECT DEPT_CODE FROM PERFORMANCE.SET_ACCOUNTDEPTTYPE WHERE DEPT_TYPE='20001')
GROUP BY C.ACCOUNT_DEPT_NAME,C.ACCOUNT_DEPT_CODE
 ORDER BY C.ACCOUNT_DEPT_CODE", begindate, enddate, in_tablename, intype);

            DataSet ds = OracleBase.Query(sql.ToString());

            return ds.Tables[0];
        }

        public DataTable GetIncode_YZB(string begindate, string enddate,string tableype)
        {
            string tablename = "";
            if (tableype == "0")
            {
                tablename = "hisfact.INP_CLASS2_INCOME";
            }
            else
            {
                tablename = "hisfact.INP_CLASS2_INCOME_SETTLE";
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"
  SELECT   aa.DEPT_NAME,
           HJ,
           YP,
           JMYP,
           JY,           
           JC,
           ZL,
           FS,
           SS,
           SX,
           HL,
           GH,
           CW,
           QT,
           ZLSR,
           ZLF,
           MZ,
           ZLFF,
          round( YP/(HJ-jmyp)*100,2) YZB
    FROM   (  SELECT   account_dept_code,
                       DECODE (b.account_dept_code, '', '合计', MIN (b.account_dept_name)) dept_name,
                       ROUND (SUM (INCOMES), 2) hj,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'A', INCOMES, 0)), 2) yp,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'B', INCOMES, 0)), 2) jy,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'Z', INCOMES, 0)), 2) jmyp,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'C', INCOMES, 0)), 2) jc,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'D', INCOMES, 0)), 2) zl,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'E', INCOMES, 0)), 2) fs,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'F', INCOMES, 0)), 2) ss,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'G', INCOMES, 0)), 2) sx,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'H', INCOMES, 0)), 2) hl,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'I', INCOMES, 0)), 2) gh,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'J', INCOMES, 0)), 2) cw,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'K', INCOMES, 0)), 2) qt,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'M', INCOMES, 0)), 2)
                          zlsr,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'N', INCOMES, 0)), 2)
                          zlf,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'O', INCOMES, 0)), 2) mz,
                       ROUND (SUM (DECODE (c.CLASS_TYPE, 'X', INCOMES, 0)), 2)
                          zlff
                FROM   (  SELECT   ORDERED_BY_DEPT,
                                   RECK_CLASS,
                                   SUM (CHARGES) INCOMES
                            FROM   hisfact.OUTP_CLASS2_INCOME
                           WHERE   ST_DATE >= TO_DATE ('{0}', 'yyyymm')
                                   AND ST_DATE <=
                                         TO_DATE ('{1}', 'yyyymm')
                        GROUP BY   ORDERED_BY_DEPT, RECK_CLASS
                        UNION ALL
                          SELECT   ORDERED_BY_DEPT,
                                   RECK_CLASS,
                                   SUM (CHARGES) INCOMES
                            FROM   {2}
                           WHERE   ST_DATE >= TO_DATE ('{0}', 'yyyymm')
                                   AND ST_DATE <=
                                         TO_DATE ('{1}', 'yyyymm')
                        GROUP BY   ORDERED_BY_DEPT, RECK_CLASS) a,
                       COMM.sys_dept_dict b,
                       CBHS.CBHS_DISTRIBUTION_CALC_SCHM c
               WHERE   a.RECK_CLASS = c.item_class
                       AND a.ORDERED_BY_DEPT = b.dept_code
            GROUP BY   CUBE( account_dept_code)) aa,
           comm.sys_dept_dict bb
   WHERE   aa.account_dept_code = bb.dept_code(+)
ORDER BY   bb.SORT_NO", begindate,enddate,tablename);

            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }

        /// <summary>
        /// 查询门诊及住院核算收入
        /// </summary>
        /// <param name="begindate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="balance">收付或者发生</param>
        /// <param name="deptcode">科室编码</param>
        /// <param name="incomestype">开单或执行</param>
        /// <returns></returns>
        public DataTable GetAdjust_Income(string begindate, string enddate, string balance, string deptcode)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"/* Formatted on 2016/11/5 14:46:43 (QP5 v5.115.810.9015) */
  SELECT   GROUPING (C.ACCOUNT_DEPT_name) aaa,
           MAX (C.ACCOUNT_DEPT_CODE) DEPT_CODE,
           CASE
              WHEN C.ACCOUNT_DEPT_name IS NULL
                   AND GROUPING (C.ACCOUNT_DEPT_name) = 0
              THEN
                 '无科室'
              WHEN C.ACCOUNT_DEPT_name IS NULL
                   AND GROUPING (C.ACCOUNT_DEPT_name) = 1
              THEN
                 '合计'
              ELSE
                 MAX (C.ACCOUNT_DEPT_name)
           END
              dept_name,
           SUM(CASE
                  WHEN     SUBSTR (GROUP_NO, 0, 1) = 'A'
                       AND GROUP_NAME NOT LIKE '%氧气%'
                       AND GROUP_NAME NOT LIKE '%高压氧%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS ZL,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'A'
                       AND GROUP_NAME LIKE '%高压氧%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS GYY,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'A'
                       AND GROUP_NAME LIKE '%氧气%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS YQ,
           SUM (
              CASE WHEN SUBSTR (GROUP_NO, 0, 1) = 'A' THEN INCOMES ELSE 0 END
           )
              AS HJ,
           SUM(CASE
                  WHEN     SUBSTR (GROUP_NO, 0, 1) = 'B'
                       AND GROUP_NAME LIKE '%ICU%'
                       AND GROUP_NAME NOT LIKE '%ICU氧气%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS ICUZL,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'B'
                       AND GROUP_NAME LIKE '%ICU氧气%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS LCUYQ,
           SUM (
              CASE WHEN SUBSTR (GROUP_NO, 0, 1) = 'B' THEN INCOMES ELSE 0 END
           )
              AS ICUHJ,
           SUM(CASE
                  WHEN     SUBSTR (GROUP_NO, 0, 1) = 'C'
                       AND GROUP_NAME LIKE '%手术%'
                       AND GROUP_NAME NOT LIKE '%手术氧气%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS SSF,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'C'
                       AND GROUP_NAME LIKE '%手术氧气%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS SSYQ,
           SUM (
              CASE WHEN SUBSTR (GROUP_NO, 0, 1) = 'C' THEN INCOMES ELSE 0 END
           )
              AS SSHJ,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'D'
                       AND GROUP_NAME LIKE '%放射%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS FSF,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'D' AND GROUP_NAME LIKE '%CT%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS CTF,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'D'
                       AND GROUP_NAME LIKE '%电诊%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS DZF,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'D'
                       AND GROUP_NAME LIKE '%检验%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS JYF,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'D'
                       AND GROUP_NAME LIKE '%腔镜%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS QJF,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'D'
                       AND GROUP_NAME LIKE '%病理%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS BLF,
           SUM (
              CASE WHEN SUBSTR (GROUP_NO, 0, 1) = 'D' THEN INCOMES ELSE 0 END
           )
              AS FCDJQBHJ,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'E' AND GROUP_NAME LIKE '%药%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS Y,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'F'
                       AND GROUP_NAME LIKE '%卫材%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS WC,
           SUM(CASE
                  WHEN SUBSTR (GROUP_NO, 0, 1) = 'G' AND GROUP_NAME LIKE '%血%'
                  THEN
                     INCOMES
                  ELSE
                     0
               END)
              AS XF,
           SUM (
              CASE WHEN SUBSTR (GROUP_NO, 0, 1) = 'A' THEN INCOMES ELSE 0 END
           )
           + SUM (
                CASE WHEN SUBSTR (GROUP_NO, 0, 1) = 'B' THEN INCOMES ELSE 0 END
             )
           + SUM (
                CASE WHEN SUBSTR (GROUP_NO, 0, 1) = 'C' THEN INCOMES ELSE 0 END
             )
           + SUM (
                CASE WHEN SUBSTR (GROUP_NO, 0, 1) = 'D' THEN INCOMES ELSE 0 END
             )
           + SUM(CASE
                    WHEN SUBSTR (GROUP_NO, 0, 1) = 'E'
                         AND GROUP_NAME LIKE '%药%'
                    THEN
                       INCOMES
                    ELSE
                       0
                 END)
           + SUM(CASE
                    WHEN SUBSTR (GROUP_NO, 0, 1) = 'F'
                         AND GROUP_NAME LIKE '%卫材%'
                    THEN
                       INCOMES
                    ELSE
                       0
                 END)
           + SUM(CASE
                    WHEN SUBSTR (GROUP_NO, 0, 1) = 'G'
                         AND GROUP_NAME LIKE '%血%'
                    THEN
                       INCOMES
                    ELSE
                       0
                 END)
              AS ZJ
    FROM   cbhs.CBHS_INCOMS_INFO A,
           hisdata.RECK_ITEM_CLASS_DICT B,
           COMM.SYS_DEPT_dict C
   WHERE       A.RECK_item = B.CLASS_CODE(+)
           AND A.dept_code = C.DEPT_CODE(+)
           AND TO_CHAR (a.DATE_time, 'YYYYMM') >= '{0}'
           AND TO_CHAR (a.DATE_time, 'YYYYMM') <= '{1}'
           AND a.balance_tag = '{2}'
GROUP BY   ROLLUP (C.ACCOUNT_DEPT_name)
ORDER BY   aaa, dept_code", begindate, enddate, balance);

            DataSet ds = OracleBase.Query(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="balance"></param>
        /// <param name="deptcode"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetAccountDeptIncome(string begindate, string enddate, string balance, string deptcode, string hospro)
        {
            if (hospro == "0")
            {
                // string sql = "with totalReck as (select reck_item,  ";

                //
                string str = string.Format(@"select CASE WHEN GROUPING (ITEM_NAME) = 1 AND GROUPING (CLASS_NAME) = 1 THEN ' 合计' else class_name end class_name,
                CASE WHEN GROUPING (ITEM_NAME) = 1 AND GROUPING (CLASS_NAME) = 0 THEN CLASS_NAME||'' else  '　　' || ITEM_NAME end item_name,sum(accountincomes) accountincomes,
                sum(facaccountincomes) facaccountincomes,sum(armaccountincomes) armaccountincomes,sum(hrealincomes) hrealincomes,sum(hvalationincomes) hvalationincomes,sum(crealincomes) crealincomes,sum(cvalationincomes) cvalationincomes
                from (
                  SELECT   reck_item,B.ITEM_NAME, B.CLASS_TYPE||B.CLASS_NAME class_name,
                           NVL (
                              (  SUM (NVL (incomes, 0))
                               - SUM (NVL (incomes_charges, 0))
                               + SUM (NVL (incomes_charges, 0))),
                              0
                           )
                              accountincomes,
                           NVL (SUM (NVL (incomes_charges, 0)), 0) facaccountincomes,
                           NVL (SUM (NVL (incomes, 0) - NVL (incomes_charges, 0)), 0)
                              armaccountincomes,
                           NVL (
                              SUM (CASE WHEN costs_tag = '0' THEN NVL (incomes_charges, 0) END),
                              0
                           )
                              AS hrealincomes,
                           NVL (
                              SUM(CASE
                                     WHEN costs_tag = '0'
                                     THEN
                                        NVL (incomes, 0) - NVL (incomes_charges, 0)
                                  END),
                              0
                           )
                              AS hvalationincomes,
                           NVL (
                              SUM (CASE WHEN costs_tag = '1' THEN NVL (incomes_charges, 0) END),
                              0
                           )
                              AS crealincomes,
                           NVL (
                              SUM(CASE
                                     WHEN costs_tag = '1'
                                     THEN
                                        NVL (incomes, 0) - NVL (incomes_charges, 0)
                                  END),
                              0
                           )
                              AS cvalationincomes
                                FROM   cbhs.CBHS_INCOMS_INFO_ACCOUNT a,cbhs.CBHS_DISTRIBUTION_CALC_SCHM b
                               WHERE       (a.balance_tag = '{0}') and A.RECK_ITEM=B.ITEM_CLASS
                                       AND date_time >= TO_DATE ('{1}', 'yyyymmdd')
                                       AND date_time < ADD_MONTHS (TO_DATE ('{2}', 'yyyymmdd'), 1) {3}
                            GROUP BY   reck_item,B.ITEM_NAME, B.CLASS_TYPE,B.CLASS_NAME
                            )
                            group by rollup ( CLASS_NAME,ITEM_NAME)
                            order by CLASS_NAME,ITEM_NAME", balance, begindate, enddate, deptcode);
                DataSet ds = OracleOledbBase.ExecuteDataSet(str);

                return ds.Tables[0];
            }
            else
            {
                StringBuilder str = new StringBuilder();

                // string sql = "with totalReck as (select reck_item,  ";
                str.Append("select reck_item,  ");
                str.Append(" nvl(sum(nvl(incomes_charges,0)),0) accountincomes, ");
                str.Append(" nvl(sum(nvl(incomes_charges,0)),0) facaccountincomes,");
                str.Append(" nvl(sum(nvl(incomes,0)-nvl(incomes_charges,0)),0) armaccountincomes, ");
                str.Append(" nvl(sum(case when costs_tag='0' then nvl(incomes_charges,0) end),0) as hrealincomes, ");
                str.Append(" nvl(sum(case when costs_tag='0' then nvl(incomes,0)-nvl(incomes_charges,0) end),0) as hvalationincomes, ");
                str.Append(" nvl(sum(case when costs_tag='1' then nvl(incomes_charges,0) end),0) as crealincomes,");
                str.Append(" nvl(sum(case when costs_tag='1' then nvl(incomes,0)-nvl(incomes_charges,0) end),0) as cvalationincomes ");
                str.Append(" from cbhs.CBHS_INCOMS_INFO a where ");
                //str.Append( " dept_code in (select dept_code from comm.SYS_DEPT_DICT where show_flag=0)");
                str.AppendFormat("  (a.balance_tag='{0}')  ", balance);
                str.AppendFormat(" and date_time>=to_date('{0}','yyyymmdd') and  date_time<add_months(to_date('{1}','yyyymmdd'),1)  {2}", begindate, enddate, deptcode);
                str.Append(" group by reck_item order by reck_item ");

                string sql = " select  ";
                sql += " bb.ID,bb.class_code,bb.class_name, ";
                sql += " nvl(aa.accountincomes,0) accountincomes, ";
                sql += " nvl(aa.facaccountincomes,0) facaccountincomes, ";
                sql += " nvl(aa.armaccountincomes,0) armaccountincomes, ";
                sql += " nvl(aa.hrealincomes,0) hrealincomes, ";
                sql += " nvl(aa.hvalationincomes,0) hvalationincomes, ";
                sql += " nvl(aa.crealincomes,0) crealincomes, ";
                sql += " nvl(aa.cvalationincomes,0) cvalationincomes ";
                sql += "  from  ";
                sql += " (select 'ALL' as reck_item,sum(accountincomes) accountincomes, ";
                sql += " sum(facaccountincomes) facaccountincomes,sum(armaccountincomes) armaccountincomes, ";
                sql += " sum(hrealincomes) hrealincomes,sum(hvalationincomes) hvalationincomes, ";
                sql += " sum(crealincomes) crealincomes,sum(cvalationincomes) cvalationincomes ";
                sql += " from  (" + str.ToString() + ") ";
                sql += " union all ";
                sql += " select reck_item,sum(accountincomes) accountincomes, ";
                sql += " sum(facaccountincomes) facaccountincomes,sum(armaccountincomes) armaccountincomes, ";
                sql += " sum(hrealincomes) hrealincomes,sum(hvalationincomes) hvalationincomes, ";
                sql += " sum(crealincomes) crealincomes,sum(cvalationincomes) cvalationincomes ";
                sql += " from ( ";
                sql += " select  substr(RECK_ITEM,1,1) reck_item,sum(accountincomes) accountincomes, ";
                sql += " sum(facaccountincomes) facaccountincomes,sum(armaccountincomes) armaccountincomes, ";
                sql += " sum(hrealincomes) hrealincomes,sum(hvalationincomes) hvalationincomes, ";
                sql += " sum(crealincomes) crealincomes,sum(cvalationincomes) cvalationincomes ";
                sql += " from (" + str.ToString() + ") group by substr(RECK_ITEM,1,1) ";
                sql += " union all ";
                sql += " select  reck_item,sum(accountincomes) accountincomes, ";
                sql += " sum(facaccountincomes) facaccountincomes,sum(armaccountincomes) armaccountincomes, ";
                sql += " sum(hrealincomes) hrealincomes,sum(hvalationincomes) hvalationincomes, ";
                sql += " sum(crealincomes) crealincomes,sum(cvalationincomes) cvalationincomes ";
                sql += " from (" + str.ToString() + ") group by reck_item )  group by reck_item) aa, ";
                sql += " ( ";
                sql += " SELECT 0 ID, 'ALL' class_code, '合计' class_name ";
                sql += "   FROM DUAL ";
                sql += " UNION ALL ";
                sql += " SELECT ROWNUM ID, d.* ";
                sql += "   FROM (SELECT   class_code,  ";
                sql += "         case when length(class_code)>1 then  '　　' || class_name else class_name end as class_name ";
                sql += string.Format("             FROM {0}.reck_item_class_dict ", DataUser.HISFACT);
                sql += "         ORDER BY class_code) d ";
                sql += " union all ";
                sql += " SELECT 999 ID, '*' class_code, '*' class_name ";
                sql += "   FROM DUAL ";
                sql += " ) bb ";
                sql += " where aa.reck_item(+)=bb.class_code order by bb.id ";
                DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 住院开单实际收入
        /// </summary>
        /// <param name="begindate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="balance">0 发生，1实现</param>
        /// <param name="deptcode">科室</param>
        /// <param name="depttype">0三级科室，1二级科室</param>
        /// <returns></returns>
        public DataTable GetHospitalOrderrealIncome(string begindate, string enddate, string balance, string deptcode, string depttype)
        {

            OleDbParameter[] parameters = {
                   new OleDbParameter("V_STARTDATE",begindate), 
                   new OleDbParameter("V_ENDDATE", enddate),
                   new OleDbParameter("BALANCE",balance), 
                   new OleDbParameter("DEPTTYPE", depttype),
                   new OleDbParameter("DEPTCODE", deptcode),
                   new OleDbParameter("REPORT_TYPE","hio"),
                   new OleDbParameter("GROUPBYDEPT", "ORDERED_BY_DEPT"),
                   new OleDbParameter("INCOMECOLUMNS", "CHARGES"),
                   new OleDbParameter("HOSClINIC", "0")
                };
            OracleOledbBase.RunProcedure("CBHS.SP_REPORT_INCOMES", parameters);
            DataSet ds = OracleOledbBase.ExecuteDataSet("select * from CBHS.CBHS_REPORT_TMP");
            return ds.Tables[0];

        }

        /// <summary>
        /// 住院开单计价收入
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="balance">0 发生，1实现</param>
        /// <param name="deptcode">科室</param>
        /// <param name="depttype">0三级科室，1二级科室</param>
        /// <returns></returns>
        public DataTable GetHospitalOrderValuationIncome(string begindate, string enddate, string balance, string deptcode, string depttype)
        {
            OleDbParameter[] parameters = {
                   new OleDbParameter("V_STARTDATE",begindate), 
                   new OleDbParameter("V_ENDDATE", enddate),
                   new OleDbParameter("BALANCE",balance), 
                   new OleDbParameter("DEPTTYPE", depttype),
                   new OleDbParameter("DEPTCODE", deptcode),
                   new OleDbParameter("REPORT_TYPE","hio"),
                   new OleDbParameter("GROUPBYDEPT", "ORDERED_BY_DEPT"),
                   new OleDbParameter("INCOMECOLUMNS", "COUNT_INCOME"),
                   new OleDbParameter("HOSClINIC", "0")
                };
            OracleOledbBase.RunProcedure("CBHS.SP_REPORT_INCOMES", parameters);
            DataSet ds = OracleOledbBase.ExecuteDataSet("select * from CBHS.CBHS_REPORT_TMP");
            return ds.Tables[0];
        }

        /// <summary>
        /// 护理门诊
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetWardoutp(string begindate, string enddate, string deptcode, string costtype, string depttype, string incomestype)
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.cbhs_distribution_calc_schm ", DataUser.CBHS);
            if (costtype == "2")
            {
                str1.Append(" where ITEM_CLASS like 'A%'");
            }
            str1.Append(" order by work_rate");
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select b.account_dept_code dept_code, decode(b.account_dept_code,'','合计',min(b.account_dept_name)) \"科室名称\"");
            if (costtype == "0")
            {
                str.AppendFormat(",sum(count_income) \"合计\"");
            }
            else if (costtype == "1")
            {
                str.AppendFormat(",sum(charges) \"合计\"");
            }
            else
                str.AppendFormat(",sum(TRADE_PRICE) \"合计\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (costtype == "0")
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', COUNT_INCOME, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
                else if (costtype == "1")
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', charges, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
                else
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', TRADE_PRICE, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
            }
            str.AppendFormat(" FROM {2}.outp_class2_income a, {3}.sys_dept_dict b WHERE st_date >= DATE '{0}' AND st_date < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.HISFACT, DataUser.COMM);
            if (incomestype == "0")
            {
                str.AppendFormat(" and a.ORDERED_BY_DEPT=b.dept_code");
            }
            else if (incomestype == "1")
            {
                str.AppendFormat(" and a.PERFORMED_BY=b.dept_code");
            }
            else
            {
                str.AppendFormat(" and a.WARD_CODE=b.dept_code");
            }
            if (deptcode != "")
            {
                str.AppendFormat(" and b.dept_code in ({0})", deptcode);
            }
            if (depttype != "")
            {
                str.AppendFormat(" and b.dept_type='{0}'", depttype);
            }
            str.AppendFormat(" group by cube(b.account_dept_code)");
            str.AppendFormat(" order by b.account_dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }


        /// <summary>
        /// 全院收入
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="deptcode"></param>
        /// <param name="costtype"></param>
        /// <param name="depttype"></param>
        /// <param name="incomestype"></param>
        /// <returns></returns>
        public DataTable GetWardall(string begindate, string enddate, string deptcode, string costtype, string depttype, string incomestype,string comestype)
        {
            StringBuilder str1 = new StringBuilder();
            StringBuilder sql = new StringBuilder();

            if (comestype == "0")
            {
                sql.AppendFormat(@" (select st_date,reck_class,ordered_by_dept,ordered_by_doctor,performed_by,costs,charges,count_income,ward_code,
                                            count_unity,patient_id,patient_name,trade_price,other_dept,st_date_old from HISFACT.INP_CLASS2_INCOME
	                                union all
	                                select st_date,reck_class,ordered_by_dept,ordered_by_doctor,performed_by,costs,charges,count_income,ward_code,
                                    count_unity,patient_id,patient_name,trade_price,other_dept,st_date_old from HISFACT.outp_class2_income) ");
            }
            else if(comestype == "1")
            {
                sql.AppendFormat(@" HISFACT.outp_class2_income ");
            }
            else if (comestype == "2")
            {
                sql.AppendFormat(@" HISFACT.INP_CLASS2_INCOME ");
            }

            str1.AppendFormat("select * from {0}.cbhs_distribution_calc_schm ", DataUser.CBHS);
            if (costtype == "2")
            {
                str1.Append(" where ITEM_CLASS like 'A%'");
            }
            str1.Append(" order by work_rate");
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select b.account_dept_code dept_code, decode(b.account_dept_code,'','合计',min(b.account_dept_name)) \"科室名称\"");
            if (costtype == "0")
            {
                str.AppendFormat(",sum(count_income) \"合计\"");
            }
            else if (costtype == "1")
            {
                str.AppendFormat(",sum(charges) \"合计\"");
            }
            else
                str.AppendFormat(",sum(TRADE_PRICE) \"合计\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (costtype == "0")
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', COUNT_INCOME, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
                else if (costtype == "1")
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', charges, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
                else
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', TRADE_PRICE, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
            }

            str.AppendFormat(" FROM {4} a, {3}.sys_dept_dict b WHERE st_date >= DATE '{0}' AND st_date < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.HISFACT, DataUser.COMM,sql);

            //str.AppendFormat(" FROM {4} a, {3}.sys_dept_dict b ", begindate, enddate, DataUser.HISFACT, DataUser.COMM, sql);

            if (incomestype == "0")
            {
                str.AppendFormat(" and a.ORDERED_BY_DEPT=b.dept_code");
            }
            else if (incomestype == "1")
            {
                str.AppendFormat(" and a.PERFORMED_BY=b.dept_code");
            }
            else
            {
                str.AppendFormat(" and a.WARD_CODE=b.dept_code");
            }
            if (deptcode != "")
            {
                str.AppendFormat(" and b.dept_code in ({0})", deptcode);
            }
            if (depttype != "")
            {
                str.AppendFormat(" and b.dept_type='{0}'", depttype);
            }
            str.AppendFormat(" group by cube(b.account_dept_code)");
            str.AppendFormat(" order by b.account_dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 器械查询
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetMtrlDetail(string begindate, string enddate, string flags)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select to_char(ACCOUNT_DATE,'yyyy-MM-dd') ACCOUNT_DATE,MTRL_NAME,MTRL_CODE,MTRL_SPEC,UNITS,QUANTITY,PURCHASE_PRICE,RETAIL_PRICE,CHAJIA from {0}.MTRL_DETAIL where  ACCOUNT_DATE >= DATE '{1}' AND ACCOUNT_DATE < ADD_MONTHS(DATE '{2}',1) and flags={3}", DataUser.HISFACT, begindate, enddate, flags);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 收容
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetAHospitalsysDetail(string begindate, string enddate, string deptcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select to_char(ST_DATE,'yyyy-MM-dd') ST_DATE,DOCTOR_IN_CHARGE,PATIENT_ID from {0}.ahospitalsys_detail where  ST_DATE >= DATE '{1}' AND ST_DATE < ADD_MONTHS(DATE '{2}',1) ", DataUser.HISFACT, begindate, enddate);
            if (deptcode != "")
            {
                str.AppendFormat(" and dept_code in ({0})", deptcode);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetAHospitalsys(string begindate, string enddate, string deptcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select to_char(ST_DATE,'yyyy-MM-dd') ST_DATE,DOCTOR_IN_CHARGE,WARD_NAME,PERSONS from {0}.ahospitalsys where  ST_DATE >= DATE '{1}' AND ST_DATE < ADD_MONTHS(DATE '{2}',1) ", DataUser.HISFACT, begindate, enddate);
            if (deptcode != "")
            {
                str.AppendFormat(" and dept_code in ({0})", deptcode);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 护理住院收入
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="deptcode"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public DataTable GetWardinp(string begindate, string enddate, string deptcode, string balance, string costtype, string depttype, string incomestype)
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.cbhs_distribution_calc_schm ", DataUser.CBHS);
            if (costtype == "2")
            {
                str1.Append(" where ITEM_CLASS like 'A%'");
            }
            str1.Append(" order by work_rate");
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];

            StringBuilder str = new StringBuilder();
            str.AppendFormat("select b.account_dept_code dept_code, decode(b.account_dept_code,'','合计',min(b.account_dept_name))  \"科室名称\"");
            if (costtype == "0")
            {
                str.AppendFormat(",sum(count_income) \"合计\"");
            }
            else if (costtype == "1")
            {
                str.AppendFormat(",sum(charges) \"合计\"");
            }
            else
                str.AppendFormat(",sum(TRADE_PRICE) \"合计\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (costtype == "0")
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', COUNT_INCOME, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
                else if (costtype == "1")
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', charges, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
                else
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', TRADE_PRICE, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
            }
            if (balance == "1")
            {
                str.AppendFormat(" from {0}.INP_CLASS2_INCOME_SETTLE a,", DataUser.HISFACT);
            }
            else
            {
                str.AppendFormat(" from {0}.INP_CLASS2_INCOME a,", DataUser.HISFACT);
            }
            str.AppendFormat(" {2}.sys_dept_dict b WHERE st_date >= DATE '{0}' AND st_date < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.COMM);
            if (incomestype == "0")
            {
                str.AppendFormat(" and a.ORDERED_BY_DEPT=b.dept_code");
            }
            else if (incomestype == "1")
            {
                str.AppendFormat(" and a.PERFORMED_BY=b.dept_code");
            }
            else
            {
                str.AppendFormat(" and a.WARD_CODE=b.dept_code");
            }
            if (deptcode != "")
            {
                str.AppendFormat(" and b.dept_code in ({0})", deptcode);
            }
            if (depttype != "")
            {
                str.AppendFormat(" and b.dept_type='{0}'", depttype);
            }

            str.AppendFormat(" group by cube (b.ACCOUNT_DEPT_CODE)");
            str.AppendFormat(" order by b.account_dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 成本明细
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="balance">结算标识</param>
        /// <param name="depttype"></param>
        /// <returns></returns>
        public DataTable GetCostReport(string begindate, string enddate, string balance, string costtype, string depttype)
        {
            //获取成本项目字典，形成列表的列名
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.CBHS_COST_ITEM_DICT ", DataUser.CBHS);
            str1.Append(" order by item_code");
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];

            StringBuilder str = new StringBuilder();
            str.AppendFormat("select b.account_dept_code dept_code, decode(b.account_dept_code,'','合计',min(b.account_dept_name))  \"科室名称\"");
            if (costtype == "0")
            {
                str.AppendFormat(",sum(COSTS_ARMYFREE) \"合计\"");
            }
            else if (costtype == "1")
            {
                str.AppendFormat(",sum(COSTS) \"合计\"");
            }
            else
            {
                str.AppendFormat(",sum(COSTS_ARMYFREE+COSTS) \"合计\"");
            }

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (costtype == "0")
                {
                    str.AppendFormat(" ,SUM (DECODE (a.ITEM_CODE, '{0}', COSTS_ARMYFREE, 0)) \"{1}\"", table.Rows[i]["ITEM_CODE"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
                else if (costtype == "1")
                {
                    str.AppendFormat(" ,SUM (DECODE (a.ITEM_CODE, '{0}', COSTS, 0)) \"{1}\"", table.Rows[i]["ITEM_CODE"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
                else
                {
                    str.AppendFormat(" ,SUM (DECODE (a.ITEM_CODE, '{0}', COSTS_ARMYFREE+COSTS, 0)) \"{1}\"", table.Rows[i]["ITEM_CODE"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
            }

            str.AppendFormat(" from {0}.CBHS_DEPT_COST_DETAIL a,", DataUser.CBHS);

            str.AppendFormat(" {2}.sys_dept_dict b WHERE ACCOUNTING_DATE >= DATE '{0}' AND ACCOUNTING_DATE < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.COMM);
            str.AppendFormat(" and a.DEPT_CODE=b.dept_code and (a.BALANCE_TAG='{0}' or a.BALANCE_TAG is null)", balance);
            if (depttype != "")
            {
                str.AppendFormat(" and b.dept_type='{0}'", depttype);
            }

            //str.AppendFormat(" group by cube (b.ACCOUNT_DEPT_CODE),b.SORT_NO");
            str.AppendFormat(" group by b.SORT_NO,b.account_dept_code");
            str.AppendFormat(" order by b.SORT_NO");

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 收入明细
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="balance"></param>
        /// <param name="costtype"></param>
        /// <param name="depttype"></param>
        /// <returns></returns>
        public DataTable GetIncomesReport(string begindate, string enddate, string balance, string costtype, string depttype)
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.CBHS_INCOM_TYPE_DICT ", DataUser.CBHS);
            str1.Append(" order by INCOM_TYPE_CODE");
            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];

            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT   CASE WHEN GROUPING (b.account_dept_name) = 1 AND GROUPING (costs_tag) = 1 THEN '合计' ELSE b.account_dept_name END \"科室名称\", CASE WHEN GROUPING (b.account_dept_name) = 0 AND GROUPING (costs_tag) = 1 THEN '小计' ELSE (CASE WHEN a.costs_tag = '0' THEN '住院' when a.costs_tag='1' then '门诊' ELSE '' END) END \"类别\",");
            if (costtype == "0")
            {
                str.AppendFormat(" round(sum(INCOMES-INCOMES_CHARGES),2) \"合计\"");
            }
            else if (costtype == "1")
                str.AppendFormat(" round(sum(INCOMES_CHARGES),2) \"合计\"");
            else
                str.AppendFormat(" round(sum(INCOMES),2) \"合计\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (costtype == "0")
                {
                    str.AppendFormat(" ,round(SUM (DECODE (c.CLASS_TYPE, '{0}', INCOMES-INCOMES_CHARGES, 0)),2) \"{1}\"", table.Rows[i]["INCOM_TYPE_CODE"].ToString(), table.Rows[i]["INCOM_TYPE_NAME"].ToString());
                }
                else if (costtype == "1")
                {
                    str.AppendFormat(" ,round(SUM (DECODE (c.CLASS_TYPE, '{0}', INCOMES_CHARGES, 0)),2) \"{1}\"", table.Rows[i]["INCOM_TYPE_CODE"].ToString(), table.Rows[i]["INCOM_TYPE_NAME"].ToString());
                }
                else
                {
                    str.AppendFormat(" ,round(SUM (DECODE (c.CLASS_TYPE, '{0}', INCOMES, 0)),2) \"{1}\"", table.Rows[i]["INCOM_TYPE_CODE"].ToString(), table.Rows[i]["INCOM_TYPE_NAME"].ToString());
                }
            }

            str.AppendFormat(" from {0}.CBHS_INCOMS_INFO_ACCOUNT a,", DataUser.CBHS);

            str.AppendFormat(" {2}.sys_dept_dict b,{3}.CBHS_DISTRIBUTION_CALC_SCHM c WHERE a.reck_item=c.item_class and a.DATE_TIME >= DATE '{0}' AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.COMM, DataUser.CBHS);
            str.AppendFormat(" and a.DEPT_CODE=b.dept_code and a.BALANCE_TAG='{0}'", balance);
            if (depttype != "")
            {
                str.AppendFormat(" and b.dept_type='{0}'", depttype);
            }

            str.AppendFormat(" GROUP BY rollup (b.account_dept_name, costs_tag)");
            str.AppendFormat(" order by b.account_dept_name");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 查询军卫数据
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="deptcode"></param>
        /// <param name="balance"></param>
        /// <param name="costtype"></param>
        /// <param name="depttype"></param>
        /// <param name="incomestype"></param>
        /// <returns></returns>
        public DataTable GetHisreport(string begindate, string enddate, string deptcode, string balance, string costtype, string depttype, string incomestype)
        {
            string str1 = string.Format("select * from {0}.cbhs_distribution_calc_schm order by item_class", DataUser.CBHS);
            DataTable table = OracleOledbBase.ExecuteDataSet(str1).Tables[0];
            StringBuilder str = new StringBuilder();
            if (incomestype == "0")
            {
                str.AppendFormat("select a.count_unity dept_code, decode(a.count_unity,'','合计',min(b.dept_name))  \"科室名称\"");
            }
            else
            {
                str.AppendFormat("select a.PERFORMED_BY dept_code, decode(a.PERFORMED_BY,'','合计',min(b.dept_name))  \"科室名称\"");
            }
            if (costtype == "0")
            {
                str.AppendFormat(",sum(count_income) \"合计\"");
            }
            else
                str.AppendFormat(",sum(charges) \"合计\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (costtype == "0")
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', COUNT_INCOME, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
                else
                {
                    str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', charges, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());
                }
            }
            if (depttype == "1")
            {
                if (balance == "1")
                {
                    str.AppendFormat(" from {0}.INP_CLASS2_INCOME_SETTLE a,", DataUser.HISFACT);
                }
                else
                {
                    str.AppendFormat(" from {0}.INP_CLASS2_INCOME a,", DataUser.HISFACT);
                }
            }
            else
            {
                str.AppendFormat(" from {0}.OUTP_CLASS2_INCOME a,", DataUser.HISFACT);
            }
            str.AppendFormat(" {2}.sys_dept_dict b WHERE st_date >= DATE '{0}' AND st_date < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.COMM);
            if (incomestype == "0")
            {
                str.AppendFormat(" and a.COUNT_UNITY=b.dept_code(+)");
            }
            else
            {
                str.AppendFormat(" and a.PERFORMED_BY=b.dept_code(+)");
            }

            if (deptcode != "")
            {
                str.AppendFormat(" and b.dept_code in ({0})", deptcode);
            }

            if (incomestype == "0")
            {
                str.AppendFormat(" group by rollup (a.count_unity)");
                str.AppendFormat(" order by a.count_unity");
            }
            else
            {
                str.AppendFormat(" group by rollup (a.PERFORMED_BY)");
                str.AppendFormat(" order by a.PERFORMED_BY");
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 医生明细
        /// </summary>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <param name="depttype"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetDoctorDetail(string stardate, string enddate, string depttype, string deptcode,string incometype)
        {
            string str1 = string.Format("select * from {0}.cbhs_distribution_calc_schm order by item_class", DataUser.CBHS);
            DataTable table = OracleOledbBase.ExecuteDataSet(str1).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select a.ORDERED_BY_DOCTOR,decode(a.ORDERED_BY_DOCTOR,'','合计',min(b.user_name))  \"医生\"");
            str.AppendFormat(",sum(COSTS) \"合计\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {

                str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', COSTS, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());

            }

          
            if (depttype == "1")
            {
                if (incometype == "1")
                {
                    str.AppendFormat(" from {0}.inp_class2_income_settle a,", DataUser.HISFACT);
                }
                else
                {
                    str.AppendFormat(" from {0}.inp_class2_income a,", DataUser.HISFACT);
                }
            }
            else
            {
                str.AppendFormat(" from {0}.OUTP_CLASS2_INCOME a,", DataUser.HISFACT);
            }
            str.AppendFormat(" hisdata.users b, COMM.sys_dept_dict c WHERE st_date >= DATE '{0}' AND st_date <= DATE '{1}' ", stardate, enddate);
            str.AppendFormat(" and a.ORDERED_BY_DOCTOR=b.db_user and a.ORDERED_BY_DEPT=c.dept_code ");

            if (deptcode != "")
            {
                str.AppendFormat(" and c.ACCOUNT_DEPT_CODE='{0}' ", deptcode);
            }


            str.AppendFormat(" group by rollup (a.ORDERED_BY_DOCTOR)");

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 病人明细
        /// </summary>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <param name="depttype"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetPatientDetail(string stardate, string enddate, string depttype, string deptcode)
        {
            string str1 = string.Format("select * from {0}.cbhs_distribution_calc_schm order by item_class", DataUser.CBHS);
            DataTable table = OracleOledbBase.ExecuteDataSet(str1).Tables[0];
            StringBuilder str = new StringBuilder();
            //str.AppendFormat("select a.PATIENT_NAME,nvl(a.PATIENT_NAME,'合计')  \"病人\"");

            str.AppendFormat("select a.patient_id, decode(a.patient_id,'','合计',min(a.patient_name))  \"病人\"");

            str.AppendFormat(",sum(COSTS) \"合计\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {

                str.AppendFormat(" ,SUM (DECODE (a.reck_class, '{0}', COSTS, 0)) \"{1}\"", table.Rows[i]["ITEM_CLASS"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());

            }
            if (depttype == "1")
            {
                str.AppendFormat(" from {0}.INP_CLASS2_INCOME a,", DataUser.HISFACT);
            }
            else
            {
                str.AppendFormat(" from {0}.OUTP_CLASS2_INCOME a,", DataUser.HISFACT);
            }
            str.AppendFormat(" {2}.sys_dept_dict b WHERE st_date >= DATE '{0}' AND st_date <= DATE '{1}' ", stardate, enddate, DataUser.COMM);
            str.AppendFormat(" and a.ORDERED_BY_DEPT=b.dept_code");

            if (deptcode != "")
            {
                str.AppendFormat(" and b.account_dept_code = '{0}'", deptcode);
            }

            str.AppendFormat(" group by cube (a.patient_id)");
            //str.AppendFormat(" group by rollup (a.ORDERED_BY_DOCTOR)");

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <param name="tablename">表</param>
        /// <param name="deptcode">科室代码</param>
        /// <param name="groupbydept">统计科室</param>
        /// <param name="incomescolums">统计列</param>
        /// <param name="accountdept">二级科三级科</param>
        /// <returns></returns>
        public DataTable GetIncomDetail(string stardate, string enddate, string tablename, string deptcode, string groupbydept, string incomescolums, string accountdept, string doctorname)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT to_char(st_date,'yyyy-MM-dd') st_date, b.dept_name,d.dept_name count_unity,c.item_name,
       ordered_by_doctor,patient_id,patient_name,sum({0}) costs
  FROM {1}.{2} a,
       {3}.sys_dept_dict b,
       {3}.sys_dept_dict d,
       {4}.cbhs_distribution_calc_schm c
 WHERE {5} = b.dept_code(+) and a.count_unity=d.dept_code(+) AND a.reck_class = c.item_class(+)  and a.{5} in (select dept_code from {3}.sys_dept_dict where {6}='{7}')
and a.ST_DATE>=to_date('{8}','yyyy-MM-dd') and a.ST_DATE<ADD_MONTHS(to_date('{9}','yyyy-MM-dd'),1)", incomescolums, DataUser.HISFACT, tablename, DataUser.COMM, DataUser.CBHS, groupbydept, accountdept, deptcode, stardate, enddate);
            if (doctorname != "")
            {
                str.AppendFormat(" and (a.ORDERED_BY_DOCTOR like '{0}%' or a.patient_id like '{0}%' or a.patient_name like '{0}%')", doctorname);
            }
            str.Append(@" group by st_date, b.dept_name, reck_class, c.item_name,count_unity,d.dept_name,
       ordered_by_doctor,patient_id,patient_name order by st_date,b.dept_name");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 医生病人明细
        /// </summary>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <param name="tablename"></param>
        /// <param name="doctorname"></param>
        /// <returns></returns>
        public DataTable GetDoctorDetail(string stardate, string enddate, string tablename, string doctorname, string patientid, string accountdeptcode)
        {
            StringBuilder str = new StringBuilder();
            if (tablename == "OUTP_BILL_ITEMS")
            {
                str.AppendFormat(@"SELECT to_char(visit_date,'yyyy-MM-dd HH24:MI') billing_date_time,");
            }
            else
            {
                str.AppendFormat(@"SELECT to_char(billing_date_time,'yyyy-MM-dd HH24:MI') billing_date_time,");
            }
            str.AppendFormat(@"patient_id, patient_name, d.class_name, item_name,
       amount, units, b.dept_name ordered_by,c.dept_name performed_by, costs, charges
  FROM {0}.{1} a,{2}.sys_dept_dict b,{0}.dept_dict c,{0}.BILL_ITEM_CLASS_DICT d
  where  ", DataUser.HISDATA, tablename, DataUser.COMM);
            if (tablename == "OUTP_BILL_ITEMS")
            {
                str.AppendFormat("  a.VISIT_DATE>=to_date('{0}','yyyy-MM-dd')  and a.VISIT_DATE<to_date('{1}','yyyy-MM-dd')+1 ", stardate, enddate);
            }
            else
            {
                str.AppendFormat("  a.billing_date_time>=to_date('{0}','yyyy-MM-dd')  and a.billing_date_time<to_date('{1}','yyyy-MM-dd')+1 ", stardate, enddate);
            }
            if (doctorname != "")
            {
                if (tablename == "OUTP_BILL_ITEMS")
                    str.AppendFormat(" and a.ORDERED_BY_DOCTOR = '{0}'", doctorname);
                else str.AppendFormat(" and a.DOCTOR_IN_CHARGE = '{0}'", doctorname);
            }
            if (patientid != "")
            {
                str.AppendFormat(" and (patient_id = '{0}' or patient_name = '{0}')", patientid);
            }
            str.AppendFormat(" and A.ORDERED_BY=B.DEPT_code(+) and A.PERFORMED_BY=C.DEPT_CODE(+) and A.ITEM_CLASS=D.CLASS_CODE(+)");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 住院执行实际收入
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="balance">0 发生，1实现</param>
        /// <param name="deptcode">科室</param>
        /// <param name="depttype">0三级科室，1二级科室</param>
        /// <returns></returns>
        public DataTable GetHospitalPerformRealIncome(string begindate, string enddate, string balance, string deptcode, string depttype)
        {

            OleDbParameter[] parameters = {
                   new OleDbParameter("V_STARTDATE",begindate), 
                   new OleDbParameter("V_ENDDATE", enddate),
                   new OleDbParameter("BALANCE",balance), 
                   new OleDbParameter("DEPTTYPE", depttype),
                   new OleDbParameter("DEPTCODE", deptcode),
                   new OleDbParameter("REPORT_TYPE","hio"),
                   new OleDbParameter("GROUPBYDEPT", "PERFORMED_BY"),
                   new OleDbParameter("INCOMECOLUMNS", "CHARGES"),
                   new OleDbParameter("HOSClINIC", "0")
                };
            OracleOledbBase.RunProcedure("CBHS.SP_REPORT_INCOMES", parameters);
            DataSet ds = OracleOledbBase.ExecuteDataSet("select * from CBHS.CBHS_REPORT_TMP");
            return ds.Tables[0];
        }

        /// <summary>
        /// 科室药占比
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetDeptMedPro(string balance, string stardate, string enddate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT b.account_dept_code,B.ACCOUNT_DEPT_NAME, SUM (decode(SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A1', 0,decode(a.costs_tag,'1',a.incomes_charges,0))) MZ_charges,
           SUM (DECODE (SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A1', a.incomes_charges, 0)
               ) MZY_charges,
               round(decode(nvl(SUM (decode(a.COSTS_TAG,'1',a.incomes_charges,0)),0),0,0,SUM (DECODE (SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A1', a.incomes_charges, 0)
               )/SUM (decode(a.COSTS_TAG,'1',a.incomes_charges,0))),4)*100||'%' MZY_charges_ral,
               SUM (decode(SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A0', 0,decode(a.costs_tag,'0',a.incomes_charges,0))) ZY_charges,
           SUM (DECODE (SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A0', a.incomes_charges, 0)
               ) ZYY_charges,
               round(decode(nvl(SUM (decode(a.COSTS_TAG,'0',a.incomes_charges,0)),0),0,0,SUM (DECODE (SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A0', a.incomes_charges, 0)
               )/SUM (decode(a.COSTS_TAG,'0',a.incomes_charges,0))),4)*100||'%' ZYY_charges_ral,
               
               SUM (decode(SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A1', 0,decode(a.costs_tag,'1',A.incomes-a.incomes_charges,0))) MZ_incomes,
           SUM (DECODE (SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A1', A.incomes-a.incomes_charges, 0)
               ) MZY_incomes,
               round(decode(nvl(SUM (decode(a.COSTS_TAG,'1',A.incomes-a.incomes_charges,0)),0),0,0,SUM (DECODE (SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A1', A.incomes-a.incomes_charges, 0)
               )/SUM (decode(a.COSTS_TAG,'1',A.incomes-a.incomes_charges,0))),4)*100||'%' MZY_incomes_ral,
               SUM (decode(SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A0', 0,decode(a.costs_tag,'0',A.incomes-a.incomes_charges,0))) ZY_incomes,
           SUM (DECODE (SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A0', A.incomes-a.incomes_charges, 0)
               ) ZYY_incomes,
               round(decode(nvl(SUM (decode(a.COSTS_TAG,'0',A.incomes-a.incomes_charges,0)),0),0,0,SUM (DECODE (SUBSTR (a.reck_item, 0, 1)||a.COSTS_TAG, 'A0', A.incomes-a.incomes_charges, 0)
               )/SUM (decode(a.COSTS_TAG,'0',A.incomes-a.incomes_charges,0))),4)*100||'%' ZYY_incomes_ral
      FROM {3}.cbhs_incoms_info a,{4}.sys_dept_dict b  where to_date(to_char(date_time,'yyyyMM'),'yyyyMM')>=to_date('{0}','yyyyMM') and to_date(to_char(date_time,'yyyyMM'),'yyyyMM')<=to_date('{1}','yyyyMM') and A.DEPT_CODE=b.dept_code
      AND A.BALANCE_TAG='{2}'
      group by b.account_dept_code,B.ACCOUNT_DEPT_NAME order by b.account_dept_code", stardate, enddate, balance, DataUser.CBHS, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetDeptCostAnaly(string balance, string stardate, string enddate)
        {
            string inptable = "INP_CLASS2_INCOME";
            if (balance == "1")
            {
                inptable = "INP_CLASS2_INCOME_SETTLE";
            }
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select N.DEPT_CODE_SECOND,N.DEPT_NAME_SECOND,
            sum(mz_charges) mz_charges,sum(dfmjzrc) dfmjzrc,round(decode(nvl(sum(dfmjzrc),0),0,0,nvl(sum(mz_charges),0)/sum(dfmjzrc)),2) mz_charges_ral,
            sum(zy_charges) zy_charges,sum(dfzyrc) dfzyrc,round(decode(nvl(sum(dfzyrc),0),0,0,nvl(sum(zy_charges),0)/sum(dfzyrc)),2) zy_charges_ral,
            sum(mz_incomes) mz_incomes,sum(jmmjzrc) jmmjzrc,round(decode(nvl(sum(jmmjzrc),0),0,0,nvl(sum(mz_incomes),0)/sum(jmmjzrc)),2) mz_incomes_ral,
            sum(zy_incomes) zy_incomes,sum(jmzyrc) jmzyrc,round(decode(nvl(sum(jmzyrc),0),0,0,nvl(sum(zy_incomes),0)/sum(jmzyrc)),2) zy_incomes_ral
             from (SELECT a.COUNT_UNITY dept_code, a.CHARGES mz_charges,
                   0 dfmjzrc, 0 zy_charges,
                   0 dfzyrc,a.COUNT_INCOME mz_incomes,
                   0 jmmjzrc, 0 zy_incomes,
                   0 jmzyrc
              FROM hisfact.OUTP_CLASS2_INCOME a
             WHERE  to_date(to_char(ST_DATE,'yyyyMM'),'yyyyMM')>=to_date('{0}','yyyyMM')
               AND to_date(to_char(ST_DATE,'yyyyMM'),'yyyyMM')<=to_date('{1}','yyyyMM')
               and SUBSTR(a.RECK_CLASS,0,1)!='A'
               union all
               SELECT a.COUNT_UNITY dept_code, 0 mz_charges,
                   0 dfmjzrc, a.CHARGES zy_charges,
                   0 dfzyrc,0 mz_incomes,
                   0 jmmjzrc, a.COUNT_INCOME zy_incomes,
                   0 jmzyrc
              FROM hisfact.{2} a
             WHERE  to_date(to_char(ST_DATE,'yyyyMM'),'yyyyMM')>=to_date('{0}','yyyyMM')
               AND to_date(to_char(ST_DATE,'yyyyMM'),'yyyyMM')<=to_date('{1}','yyyyMM')
               and SUBSTR(a.RECK_CLASS,0,1)!='A'
               union all
            SELECT dept_code, 0 mz_charges, (dfmzrc + dfjzrc) dfmjzrc, 0 zy_charges,
                   0 dfzyrc, 0 mz_incomes, (jdmzrc + jdjzrc) jmmjzrc, 0 zy_incomes,
                   0 jmzyrc
              FROM hisfact.dept_outp_workload a
              where to_date(to_char(st_date,'yyyyMM'),'yyyyMM')>=to_date('{0}','yyyyMM')
              and to_date(to_char(st_date,'yyyyMM'),'yyyyMM')<=to_date('{1}','yyyyMM')
              union all
              SELECT dept_code, 0 mz_charges, 0 dfmjzrc, 0 zy_charges,
                   DFCYRS dfzyrc, 0 mz_incomes, 0 jmmjzrc, 0 zy_incomes,
                   JDCYRS jmzyrc
              FROM hisfact.dept_pat_visit a
              where to_date(to_char(st_date,'yyyyMM'),'yyyyMM')>=to_date('{0}','yyyyMM')
              and to_date(to_char(st_date,'yyyyMM'),'yyyyMM')<=to_date('{1}','yyyyMM')) m,comm.sys_dept_dict n
              where m.dept_code=n.dept_code
              group by N.DEPT_CODE_SECOND,N.DEPT_NAME_SECOND", stardate, enddate, inptable);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 住院执行计价收入
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="balance">0 发生，1实现</param>
        /// <param name="deptcode">科室</param>
        /// <param name="depttype">0三级科室，1二级科室</param>
        /// <returns></returns>
        public DataTable GetHospitalPerformValuationIncome(string begindate, string enddate, string balance, string deptcode, string depttype)
        {

            OleDbParameter[] parameters = {
                   new OleDbParameter("V_STARTDATE",begindate), 
                   new OleDbParameter("V_ENDDATE", enddate),
                   new OleDbParameter("BALANCE",balance), 
                   new OleDbParameter("DEPTTYPE", depttype),
                   new OleDbParameter("DEPTCODE", deptcode),
                   new OleDbParameter("REPORT_TYPE","hio"),
                   new OleDbParameter("GROUPBYDEPT", "PERFORMED_BY"),
                   new OleDbParameter("INCOMECOLUMNS", "COUNT_INCOME"),
                   new OleDbParameter("HOSClINIC", "0")
                };
            OracleOledbBase.RunProcedure("CBHS.SP_REPORT_INCOMES", parameters);
            DataSet ds = OracleOledbBase.ExecuteDataSet("select * from CBHS.CBHS_REPORT_TMP");
            return ds.Tables[0];
        }

        /// <summary>
        /// 门诊开单实际收入
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="deptcode">科室</param>
        /// <param name="depttype">0三级科室，1二级科室</param>
        /// <returns></returns>
        public DataTable GetClinicOrderRealIncome(string begindate, string enddate, string deptcode, string depttype)
        {

            OleDbParameter[] parameters = {
                   new OleDbParameter("V_STARTDATE",begindate), 
                   new OleDbParameter("V_ENDDATE", enddate),
                   new OleDbParameter("BALANCE","0"), 
                   new OleDbParameter("DEPTTYPE", depttype),
                   new OleDbParameter("DEPTCODE", deptcode),
                   new OleDbParameter("REPORT_TYPE","hio"),
                   new OleDbParameter("GROUPBYDEPT", "ORDERED_BY_DEPT"),
                   new OleDbParameter("INCOMECOLUMNS", "CHARGES"),
                   new OleDbParameter("HOSClINIC", "1")
                };
            OracleOledbBase.RunProcedure("CBHS.SP_REPORT_INCOMES", parameters);
            DataSet ds = OracleOledbBase.ExecuteDataSet("select * from CBHS.CBHS_REPORT_TMP");
            return ds.Tables[0];
        }

        /// <summary>
        /// 门诊开单计价收入
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="deptcode">科室</param>
        /// <param name="depttype">0三级科室，1二级科室</param>
        /// <returns></returns>
        public DataTable GetClinicOrderValuationIncome(string begindate, string enddate, string deptcode, string depttype)
        {

            OleDbParameter[] parameters = {
                   new OleDbParameter("V_STARTDATE",begindate), 
                   new OleDbParameter("V_ENDDATE", enddate),
                   new OleDbParameter("BALANCE","0"), 
                   new OleDbParameter("DEPTTYPE", depttype),
                   new OleDbParameter("DEPTCODE", deptcode),
                   new OleDbParameter("REPORT_TYPE","hio"),
                   new OleDbParameter("GROUPBYDEPT", "ORDERED_BY_DEPT"),
                   new OleDbParameter("INCOMECOLUMNS", "COUNT_INCOME"),
                   new OleDbParameter("HOSClINIC", "1")
                };
            OracleOledbBase.RunProcedure("CBHS.SP_REPORT_INCOMES", parameters);
            DataSet ds = OracleOledbBase.ExecuteDataSet("select * from CBHS.CBHS_REPORT_TMP");
            return ds.Tables[0];
        }

        /// <summary>
        /// 门诊执行实际收入
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="deptcode">科室</param>
        /// <param name="depttype">0三级科室，1二级科室</param>
        /// <returns></returns>
        public DataTable GetClinicPerformRealIncome(string begindate, string enddate, string deptcode, string depttype)
        {

            OleDbParameter[] parameters = {
                   new OleDbParameter("V_STARTDATE",begindate), 
                   new OleDbParameter("V_ENDDATE", enddate),
                   new OleDbParameter("BALANCE","0"), 
                   new OleDbParameter("DEPTTYPE", depttype),
                   new OleDbParameter("DEPTCODE", deptcode),
                   new OleDbParameter("REPORT_TYPE","hio"),
                   new OleDbParameter("GROUPBYDEPT", "PERFORMED_BY"),
                   new OleDbParameter("INCOMECOLUMNS", "CHARGES"),
                   new OleDbParameter("HOSClINIC", "1")
                };
            OracleOledbBase.RunProcedure("CBHS.SP_REPORT_INCOMES", parameters);
            DataSet ds = OracleOledbBase.ExecuteDataSet("select * from CBHS.CBHS_REPORT_TMP");
            return ds.Tables[0];
        }

        /// <summary>
        /// 门诊执行计价收入
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="deptcode">科室</param>
        /// <param name="depttype">0三级科室，1二级科室</param>
        /// <returns></returns>
        public DataTable GetClinicPerformValuationIncome(string begindate, string enddate, string deptcode, string depttype)
        {

            OleDbParameter[] parameters = {
                   new OleDbParameter("V_STARTDATE",begindate), 
                   new OleDbParameter("V_ENDDATE", enddate),
                   new OleDbParameter("BALANCE","0"), 
                   new OleDbParameter("DEPTTYPE", depttype),
                   new OleDbParameter("DEPTCODE", deptcode),
                   new OleDbParameter("REPORT_TYPE","hio"),
                   new OleDbParameter("GROUPBYDEPT", "PERFORMED_BY"),
                   new OleDbParameter("INCOMECOLUMNS", "COUNT_INCOME"),
                   new OleDbParameter("HOSClINIC", "1")
                };
            OracleOledbBase.RunProcedure("CBHS.SP_REPORT_INCOMES", parameters);
            DataSet ds = OracleOledbBase.ExecuteDataSet("select * from CBHS.CBHS_REPORT_TMP");
            return ds.Tables[0];
        }

        /// <summary>
        /// 全院成本核算信息汇总表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetCostAccountTotal(string starttime, string endtime, string balance, string dept_code)
        {
            string sql = " select ACCOUNT_DEPT_CODE dept_code,decode(account_dept_code,'','合计', min(account_dept_name)) dept_name, ";
            sql += "round(sum(NVL(crealincomes,0)),2) crealincomes,round(sum(NVL(hrealincomes,0)),2) hrealincomes, ";
            sql += "round(sum(NVL(facaccountincomes,0)),2) facaccountincomes,round(sum(NVL(COSTS,0)),2) COSTS, ";
            sql += "round(sum(NVL(facaccountincomes,0)-NVL(COSTS,0)),2) profit, ";
            sql += "round(sum(NVL(cvalationincomes,0)),2) cvalationincomes,round(sum(NVL(hvalationincomes,0)),2) hvalationincomes, ";
            sql += "round(sum(NVL(armaccountincomes,0)),2) armaccountincomes,round(sum(NVL(armycosts,0)),2) armycosts ";
            sql += " from  ";
            sql += "( ";
            sql += "    SELECT DEPT_CODE,DEPT_NAME,nvl(ACCOUNT_DEPT_CODE,DEPT_CODE) ACCOUNT_DEPT_CODE, ";
            sql += "        nvl(ACCOUNT_DEPT_NAME,DEPT_NAME) ACCOUNT_DEPT_NAME,SORT_NO FROM COMM.SYS_DEPT_DICT  ";
            //sql += "     where DEPT_TYPE in ('0','1','2')  and  show_flag=0 ";
            sql += "     where   show_flag=0 ";
            sql += "        order by SORT_NO    ";
            sql += " ) aa ";
            sql += " left join  ";
            sql += " ( ";
            sql += "     select DEPT_CODE,  ";
            sql += "     nvl((sum(nvl(incomes,0))-sum(nvl(incomes_charges,0))+sum(nvl(incomes_charges,0))),0) accountincomes,  ";
            sql += "     nvl(sum(nvl(incomes_charges,0)),0) facaccountincomes, ";
            sql += "     nvl(sum(nvl(incomes,0)-nvl(incomes_charges,0)),0) armaccountincomes,  ";
            sql += "     nvl(sum(case when costs_tag='0' then nvl(incomes_charges,0) end),0) as hrealincomes, ";
            sql += "     nvl(sum(case when costs_tag='0' then nvl(incomes,0)-nvl(incomes_charges,0) end),0) as hvalationincomes,  ";
            sql += "     nvl(sum(case when costs_tag='1' then nvl(incomes_charges,0) end),0) as crealincomes, ";
            sql += "     nvl(sum(case when costs_tag='1' then nvl(incomes,0)-nvl(incomes_charges,0) end),0) as cvalationincomes  ";
            sql += "     from cbhs.CBHS_INCOMS_INFO_ACCOUNT a where (a.balance_tag='" + balance + "')   ";
            sql += "     and date_time>=to_date('" + starttime + "','yyyymmdd') and  date_time<add_months(to_date('" + endtime + "','yyyymmdd'),1)  ";
            sql += "     group by DEPT_CODE  ";
            sql += " ) bb on AA.DEPT_CODE=bb.DEPT_CODE ";
            sql += " left join  ";
            sql += " ( ";
            sql += "    select dept_code, ";
            sql += "    nvl(sum(costs),0) as COSTS, ";
            sql += "    nvl(sum(costs_armyfree),0) as armycosts  ";
            sql += "    from cbhs.cbhs_dept_cost_detail ";
            sql += "    where accounting_date>=to_date('" + starttime + "','yyyymmdd') and   ";
            sql += "    accounting_date<add_months(to_date('" + endtime + "','yyyymmdd'),1)  and ";
            sql += "    (BALANCE_TAG='" + balance + "'or BALANCE_TAG is null) ";
            sql += "    group by dept_code ";
            sql += "    order by dept_code ";
            sql += " )cc on AA.dept_code=cc.dept_code ";
            if (dept_code != "")
            {
                sql += " where ACCOUNT_DEPT_CODE in (" + dept_code + ") and ACCOUNT_DEPT_CODE in ( select dept_code from  comm.sys_dept_dict where  show_flag=0 )";
            }
            else
            {
                sql += " where  ACCOUNT_DEPT_CODE in ( select dept_code from  comm.sys_dept_dict where  show_flag=0 ) ";
            }
            sql += " group by cube(ACCOUNT_DEPT_CODE) ";
            sql += " order by ACCOUNT_DEPT_CODE";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 全院成本核算效益
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetHospitalCostBenefit(string starttime, string endtime, string balance, string depttype, string deptcode)
        {
            string sql = "select account_dept_code dept_code, decode(account_dept_code,'','合计', min(account_dept_name)) dept_name, ";
            sql += " round(SUM (NVL (facaccountincomes, 0)),2) facaccountincomes, ";
            sql += " round(SUM (NVL (costs, 0)),2) costs, ";
            sql += " round(SUM (NVL (facaccountincomes, 0) - NVL (costs, 0)),2) profit, ";
            sql += " round(SUM (NVL (armaccountincomes, 0)),2) armaccountincomes, ";
            sql += " round(SUM (NVL (armycosts, 0)),2) armycosts,round(sum(medincomes),2) medincomes ";
            sql += " FROM (SELECT   dept_code, dept_name, ";
            sql += "      NVL (account_dept_code, dept_code) account_dept_code, ";
            sql += "      NVL (account_dept_name, dept_name) account_dept_name, ";
            sql += "      sort_no ";
            sql += "  FROM comm.sys_dept_dict ";
            //sql += "   WHERE dept_type IN ('0', '1', '2') and show_flag=0 ";
            sql += "   WHERE  1=1 ";
            if (deptcode != "")
            {
                sql += string.Format(" and dept_code in ({0})", deptcode);
            }
            if (depttype != "")
            {
                sql += string.Format(" and dept_type='{0}'", depttype);
            }
            sql += "   ORDER BY sort_no) aa ";
            sql += "  LEFT JOIN ";
            sql += "   (SELECT  account_dept_code dept_code, ";
            sql += "            NVL (SUM (NVL (incomes_charges, 0)), 0) facaccountincomes, ";
            sql += "            NVL (SUM (NVL (incomes, 0) - NVL (incomes_charges, 0)),0) armaccountincomes,sum(case when A.RECK_ITEM like 'A%' then a.incomes_charges else 0 end) medincomes ";
            sql += "       FROM cbhs.cbhs_incoms_info_ACCOUNT a,comm.sys_dept_dict b ";
            sql += "      WHERE a.dept_code=b.dept_code AND a.balance_tag = '" + balance + "' ";
            sql += "        AND date_time >= TO_DATE ('" + starttime + "', 'yyyymmdd') ";
            sql += "        AND date_time < ADD_MONTHS (TO_DATE ('" + endtime + " ', 'yyyymmdd'), 1) ";
            sql += "  GROUP BY account_dept_code) bb ON aa.dept_code = bb.dept_code ";
            sql += "  LEFT JOIN ";
            sql += "   (SELECT  account_dept_code dept_code, NVL (SUM (costs), 0) AS costs, ";
            sql += "            NVL (SUM (costs_armyfree), 0) AS armycosts ";
            sql += "       FROM cbhs.cbhs_dept_cost_detail a,comm.sys_dept_dict b ";
            sql += "      WHERE a.dept_code=b.dept_code AND accounting_date >= TO_DATE ('" + starttime + "', 'yyyymmdd') ";
            sql += "        AND accounting_date <  ADD_MONTHS (TO_DATE ('" + endtime + "', 'yyyymmdd'), 1) ";
            sql += "        AND (balance_tag = '" + balance + "' or BALANCE_TAG is null) ";
            sql += "   GROUP BY account_dept_code ";
            sql += "   ) cc ON aa.dept_code = cc.dept_code ";
            //if (dept_code != "")
            //{
            //    sql += " where ACCOUNT_DEPT_CODE in (" + dept_code + ") and ACCOUNT_DEPT_CODE in ( select dept_code from  comm.sys_dept_dict where  show_flag=0 )";
            //}
            //else
            {
                sql += " where  ACCOUNT_DEPT_CODE in ( select dept_code from  comm.sys_dept_dict where  show_flag=0 ) ";
            }
            sql += " GROUP BY cube(account_dept_code) ";
            sql += " ORDER BY account_dept_code";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetHospitalCostBenefit_new(string starttime, string endtime, string balance, string deptcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"  SELECT   account_dept_code dept_code,
                                           DECODE (account_dept_code, '', '合计', MIN (account_dept_name)) dept_name,
                                           ROUND (SUM (NVL (facaccountincomes, 0)), 2) facaccountincomes,
                                           ROUND (SUM (NVL (costs, 0)), 2) costs,
                                           ROUND (SUM (NVL (facaccountincomes, 0) - NVL (costs, 0)), 2) profit,
                                           ROUND (SUM (NVL (armaccountincomes, 0)), 2) armaccountincomes,
                                           ROUND (SUM (NVL (armycosts, 0)), 2) armycosts,
                                           ROUND (SUM (medincomes), 2) medincomes
                                    FROM         (  SELECT   dept_code,
                                                             dept_name,
                                                             dept_code account_dept_code,
                                                             dept_name account_dept_name,
                                                             sort_no
                                                      FROM   comm.sys_dept_dict
                                                     WHERE   account_dept_code='{0}'
                                                  ORDER BY   sort_no
                                                 ) aa
                                              LEFT JOIN
                                                 (  SELECT   a.dept_code,
                                                             NVL (SUM (NVL (incomes_charges, 0)), 0) facaccountincomes,
                                                             NVL (SUM (NVL (incomes, 0) - NVL (incomes_charges, 0)),0) armaccountincomes,
                                                             SUM(CASE
                                                                    WHEN A.RECK_ITEM LIKE 'A%'
                                                                    THEN
                                                                       a.incomes_charges
                                                                    ELSE
                                                                       0
                                                                 END)
                                                                medincomes
                                                      FROM   cbhs.cbhs_incoms_info_ACCOUNT a
                                                     WHERE       a.balance_tag = '{3}'
                                                             AND date_time >= TO_DATE ('{1}', 'yyyymmdd')
                                                             AND date_time <  ADD_MONTHS (TO_DATE ('{2} ', 'yyyymmdd'),1)
                                                  GROUP BY   a.dept_code
                                                 ) bb
                                              ON aa.dept_code = bb.dept_code
                                           LEFT JOIN
                                              (  SELECT   a.dept_code,
                                                          NVL (SUM (costs), 0) AS costs,
                                                          NVL (SUM (costs_armyfree), 0) AS armycosts
                                                   FROM   cbhs.cbhs_dept_cost_detail a
                                                  WHERE   accounting_date >= TO_DATE ('{1}', 'yyyymmdd')
                                                          AND accounting_date < ADD_MONTHS (TO_DATE ('{2}', 'yyyymmdd'), 1)
                                                          AND (balance_tag = '{3}' OR BALANCE_TAG IS NULL)
                                               GROUP BY   a.dept_code) cc
                                           ON aa.dept_code = cc.dept_code
                                GROUP BY   CUBE (account_dept_code)
                                ORDER BY   account_dept_code", deptcode, starttime, endtime, balance);

            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
            return ds.Tables[0];
        }

        /// <summary>
        /// 单位设备折旧明细
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="reck_code"></param>
        /// <param name="smonth"></param>
        /// <returns></returns>
        public DataTable GetEquipmentDepreciation(DateTime starttime, string balance, string dept_code, string reck_code, int smonth, string hospro)
        {

            string sqlMonth = "";
            string sqlMonth1 = "";
            string sqlMonth2 = "";
            string sqlMonth3 = "";
            string sqlTotal = "";
            string sqlTotal1 = "";
            string sqlTotal2 = "";
            for (int i = 0; i < smonth; i++)
            {
                string nowmonth = starttime.Month.ToString();
                if (nowmonth.Length == 1)
                {
                    nowmonth = "0" + nowmonth;
                }
                string startdate = starttime.Year.ToString() + nowmonth + "01";

                sqlMonth += "bb.\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                sqlMonth1 += "nvl(sum(\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"),0) as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                sqlMonth2 += "b.\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                sqlMonth3 += " sum(case when accounting_date>=to_date('" + startdate + "','yyyymmdd') ";
                if (hospro == "0")
                {
                    sqlMonth3 += " and  accounting_date<ADD_MONTHS(to_date('" + startdate + "','yyyymmdd'),1) then costs+costs_armyfree else 0 end) ";
                }
                else
                {
                    sqlMonth3 += " and  accounting_date<ADD_MONTHS(to_date('" + startdate + "','yyyymmdd'),1) then costs else 0 end) ";
                }
                sqlMonth3 += " as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                sqlTotal += "bb.\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"+";
                sqlTotal1 += " \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                sqlTotal2 += " sum(\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\") as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                starttime = starttime.AddMonths(1);
            }

            sqlMonth = sqlMonth.Substring(0, sqlMonth.Length - 1);
            sqlMonth1 = sqlMonth1.Substring(0, sqlMonth1.Length - 1);
            sqlMonth2 = sqlMonth2.Substring(0, sqlMonth2.Length - 1);
            sqlMonth3 = sqlMonth3.Substring(0, sqlMonth3.Length - 1);
            sqlTotal = sqlTotal.Substring(0, sqlTotal.Length - 1) + " as \"累计额\"";
            sqlTotal1 = sqlTotal1.Substring(0, sqlTotal1.Length - 1);
            sqlTotal2 = sqlTotal2.Substring(0, sqlTotal2.Length - 1);

            string sql = " select  dept_code,dept_name \"科室\",\"累计额\"," + sqlTotal1 + " from ";
            sql += " (select aa.dept_code,aa.dept_name," + sqlTotal + "," + sqlMonth + " from ";
            sql += " (select * from comm.SYS_DEPT_DICT) aa ";
            sql += " inner join ";
            sql += " ( ";
            sql += " select ACCOUNT_DEPT_CODE dept_code," + sqlMonth1 + " ";
            sql += " from ( ";
            sql += " select A.ACCOUNT_DEPT_CODE," + sqlMonth2 + " from ";
            sql += " (select * from comm.SYS_DEPT_DICT) a ";
            sql += " left join  ";
            sql += " (select  ";
            sql += " dept_code, " + sqlMonth3 + "";
            sql += " from cbhs.cbhs_dept_cost_detail ";
            sql += " where (BALANCE_TAG='" + balance + "' or BALANCE_TAG is null)";
            if (reck_code != "")
            {
                sql += " and ITEM_CODE ='" + reck_code + "' ";
            }
            sql += " group by dept_code) b ";
            sql += " on a.dept_code=b.dept_code ";
            sql += " order by SORT_NO ";
            sql += " ) ";
            sql += " group by ACCOUNT_DEPT_CODE ";
            sql += " ) bb ";
            sql += " on AA.DEPT_CODE=bb.dept_code ";
            if (dept_code != "")
            {
                sql += " where aa.dept_code in (" + dept_code + ") ";
            }
            sql += " order by AA.SORT_NO) ";
            sql += "  union all ";
            sql += " select '999','合计',sum(\"累计额\") ," + sqlTotal2 + " from  ";
            sql += " (select aa.dept_code,aa.dept_name," + sqlTotal + "," + sqlMonth + " from ";
            sql += " (select * from comm.SYS_DEPT_DICT) aa ";
            sql += " inner join ";
            sql += " ( ";
            sql += " select ACCOUNT_DEPT_CODE dept_code," + sqlMonth1 + " ";
            sql += " from ( ";
            sql += " select A.ACCOUNT_DEPT_CODE," + sqlMonth2 + " from ";
            sql += " (select * from comm.SYS_DEPT_DICT) a ";
            sql += " left join  ";
            sql += " (select  ";
            sql += " dept_code, " + sqlMonth3 + "";
            sql += " from cbhs.cbhs_dept_cost_detail ";
            sql += " where (BALANCE_TAG='" + balance + "' or BALANCE_TAG is null)";
            if (reck_code != "")
            {
                sql += " and ITEM_CODE = '" + reck_code + "' ";
            }
            sql += " group by dept_code) b ";
            sql += " on a.dept_code=b.dept_code ";
            sql += " order by SORT_NO ";
            sql += " ) ";
            sql += " group by ACCOUNT_DEPT_CODE ";
            sql += " ) bb ";
            sql += " on AA.DEPT_CODE=bb.dept_code ";
            if (dept_code != "")
            {
                sql += " where aa.dept_code in (" + dept_code + ") ";
            }
            sql += " order by AA.SORT_NO) ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 科室成本明细查询
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="dept_code"></param>
        /// <param name="reck_code"></param>
        /// <param name="operatename"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetDeptCostDetail(string starttime, string endtime, string balance, string dept_code, string reck_code, string operatename)
        {
            string condition = " where ( BALANCE_TAG='" + balance + "' or BALANCE_TAG is null) and ACCOUNTING_DATE>=to_date('" + starttime + "','yyyymmdd') and  ACCOUNTING_DATE<add_months(to_date('" + endtime + "','yyyymmdd'),1) ";
            if (dept_code != "")
            {
                condition += " and DEPT_CODE in (" + dept_code + ")";
            }
            if (reck_code != "")
            {
                condition += " and ITEM_CODE='" + reck_code + "'";

            }
            if (operatename != "")
            {
                condition += " and OPERATOR like '" + operatename + "%'";

            }

            string sql = " select * from  ";
            sql += " (select nvl(b.ACCOUNT_DEPT_CODE,9999) dept_code, ";
            sql += " nvl(b.ACCOUNT_DEPT_NAME,'未知科室') dept_name, ";
            sql += " c.ITEM_NAME,a.cost_date,sum(COSTS) COSTS, ";
            sql += " sum(COSTS_ARMYFREE) COSTS_ARMYFREE,sum(TotalCost) TotalCost,max(a.OPERATORNAME) OPERATORNAME ";
            sql += " from ";
            sql += " (select DEPT_CODE,ITEM_CODE,to_char(ACCOUNTING_DATE,'yyyy-mm') cost_date, ";
            sql += " sum(COSTS) COSTS,sum(COSTS_ARMYFREE) COSTS_ARMYFREE,sum(COSTS+COSTS_ARMYFREE) TotalCost,OPERATOR as OPERATORNAME ";
            sql += " from ";
            sql += " cbhs.cbhs_dept_cost_detail " + condition;
            sql += " group by DEPT_CODE,ITEM_CODE,to_char(ACCOUNTING_DATE,'yyyy-mm'),OPERATOR) a ";
            sql += " left join (select * from comm.SYS_DEPT_DICT where show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0))  b ";
            sql += " on a.DEPT_CODE=b.DEPT_CODE ";
            sql += " left join cbhs.CBHS_COST_ITEM_DICT c ";
            sql += " on a.item_code=c.item_code ";
            sql += " group by  a.cost_date,b.ACCOUNT_DEPT_CODE,b.ACCOUNT_DEPT_NAME,c.ITEM_NAME ";
            sql += " order by ITEM_NAME,cost_date,ACCOUNT_DEPT_CODE) bb ";
            sql += " union  ";
            sql += " select '999999' dept_code,'合计' dept_name,null item_name,null cost_date, ";
            sql += " sum(COSTS) COSTS,sum(COSTS_ARMYFREE) COSTS_ARMYFREE,sum(TotalCost) TotalCost,null OPERATORNAME ";
            sql += " from ";
            sql += " ( ";
            sql += "     select DEPT_CODE,ITEM_CODE,to_char(ACCOUNTING_DATE,'yyyy-mm') cost_date, ";
            sql += "     sum(COSTS) COSTS,sum(COSTS_ARMYFREE) COSTS_ARMYFREE,sum(COSTS+COSTS_ARMYFREE) TotalCost,OPERATOR as OPERATORNAME ";
            sql += "     from ";
            sql += "     cbhs.cbhs_dept_cost_detail " + condition;
            sql += "     group by DEPT_CODE,ITEM_CODE,to_char(ACCOUNTING_DATE,'yyyy-mm'),OPERATOR ";
            sql += " ) aa ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetINcomeType()
        {
            DataSet ds = OracleOledbBase.ExecuteDataSet("select * from cbhs.CBHS_INCOM_TYPE_DICT");
            return ds.Tables[0];
        }

        /// <summary>
        /// 项目区间分布情况
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="reck_code"></param>
        /// <param name="smonth"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetSimpleIncomeDetail(DateTime starttime, string balance, string dept_code, string reck_code, int smonth, string hospro)
        {
            string sqlFilter = " and BALANCE_TAG='" + balance + "'";
            if (dept_code != "")
            {
                sqlFilter += " and DEPT_CODE in (" + dept_code + ")";
            }
            if (reck_code != "")
            {
                sqlFilter += " and b.CLASS_TYPE='" + reck_code + "'";
            }

            string sqlMonth = "";
            string sqlMonth1 = "";
            string sqlMonth2 = "";
            string sqlTotal = "";
            for (int i = 0; i < smonth; i++)
            {
                string nowmonth = starttime.Month.ToString();
                if (nowmonth.Length == 1)
                {
                    nowmonth = "0" + nowmonth;
                }
                string startdate = starttime.Year.ToString() + nowmonth + "01";

                sqlMonth += "\"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                if (hospro == "0")
                {
                    sqlMonth += "\"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                }


                sqlMonth1 += "nvl(sum(\"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"),0) as \"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                if (hospro == "0")
                {
                    sqlMonth1 += "nvl(sum(\"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"),0) as \"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                }

                sqlMonth2 += " sum(case when date_time>=to_date('" + startdate + "','yyyymmdd') ";
                sqlMonth2 += " and  date_time<ADD_MONTHS(to_date('" + startdate + "','yyyymmdd'),1) then INCOMES_CHARGES else 0 end) as \"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                if (hospro == "0")
                {
                    sqlMonth2 += " sum(case when date_time>=to_date('" + startdate + "','yyyymmdd') ";
                    sqlMonth2 += " and  date_time<ADD_MONTHS(to_date('" + startdate + "','yyyymmdd'),1) then incomes-INCOMES_CHARGES else 0 end) as \"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                }

                sqlTotal += "sum(\"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\") as \"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                if (hospro == "0")
                {
                    sqlTotal += "sum(\"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\") as \"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                }


                starttime = starttime.AddMonths(1);
            }

            sqlMonth = sqlMonth.Substring(0, sqlMonth.Length - 1);
            sqlMonth1 = sqlMonth1.Substring(0, sqlMonth1.Length - 1);
            sqlMonth2 = sqlMonth2.Substring(0, sqlMonth2.Length - 1);
            sqlTotal = sqlTotal.Substring(0, sqlTotal.Length - 1);


            string sql = " select dept_code ,\"科室\"," + sqlMonth + "";
            sql += " from";
            sql += " (";
            sql += " select aa.dept_code,nvl(dept_name,'未知科室') \"科室\"," + sqlMonth + "";
            sql += " from";
            sql += " (";
            sql += " select nvl(ACCOUNT_DEPT_CODE,8888) dept_code," + sqlMonth1 + "   from ";
            sql += " (select  dept_code, ";
            sql += "  " + sqlMonth2 + "";
            sql += " from cbhs.cbhs_incoms_info_ACCOUNT a,cbhs.CBHS_DISTRIBUTION_CALC_SCHM b where a.RECK_ITEM=b.ITEM_CLASS " + sqlFilter;
            sql += " group by dept_code) a left join  ";
            sql += " (select * from COMM.SYS_DEPT_DICT where DEPT_TYPE in ('0','1','2') and show_flag=0) b   ";
            sql += " on a.dept_code=b.dept_code ";
            sql += " group by ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME ";
            sql += " ) aa ";
            sql += " left join  ";
            sql += " (select * from comm.SYS_DEPT_DICT where show_flag=0) bb ";
            sql += " on aa.dept_code=bb.dept_code ";
            sql += " order by sort_no ";
            sql += " ) aaa ";
            sql += " union ";
            sql += " select '999999','合计'," + sqlTotal + "";
            sql += " from ";
            sql += " ( ";
            sql += " select nvl(ACCOUNT_DEPT_CODE,8888) dept_code, ";
            sql += " " + sqlTotal + "  from ";
            sql += " (select  dept_code, ";
            sql += " " + sqlMonth2 + "";
            sql += " from cbhs.cbhs_incoms_info_ACCOUNT a,cbhs.CBHS_DISTRIBUTION_CALC_SCHM b where  a.RECK_ITEM=b.ITEM_CLASS" + sqlFilter;
            sql += " group by dept_code) a left join  ";
            sql += " (select * from COMM.SYS_DEPT_DICT where DEPT_TYPE in ('0','1','2') and show_flag=0) b   ";
            sql += " on a.dept_code=b.dept_code ";
            sql += " group by ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME ";
            sql += " ) bbb ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="reck_code"></param>
        /// <param name="smonth"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetSimpledeptIncomeDetail(DateTime starttime, string balance, string dept_code, string reck_code, int smonth, string hospro)
        {
            string sqlFilter = " and BALANCE_TAG='" + balance + "'";
            if (dept_code != "")
            {
                sqlFilter += " and c.account_dept_code = '" + dept_code + "'";
            }
            if (reck_code != "")
            {
                sqlFilter += " and b.CLASS_TYPE='" + reck_code + "'";
            }

            string sqlMonth = "";
            string sqlMonth1 = "";
            string sqlMonth2 = "";
            string sqlTotal = "";
            for (int i = 0; i < smonth; i++)
            {
                string nowmonth = starttime.Month.ToString();
                if (nowmonth.Length == 1)
                {
                    nowmonth = "0" + nowmonth;
                }
                string startdate = starttime.Year.ToString() + nowmonth + "01";

                sqlMonth += "\"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                if (hospro == "0")
                {
                    sqlMonth += "\"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                }


                sqlMonth1 += "nvl(sum(\"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"),0) as \"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                if (hospro == "0")
                {
                    sqlMonth1 += "nvl(sum(\"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"),0) as \"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                }

                sqlMonth2 += " sum(case when date_time>=to_date('" + startdate + "','yyyymmdd') ";
                sqlMonth2 += " and  date_time<ADD_MONTHS(to_date('" + startdate + "','yyyymmdd'),1) then INCOMES_CHARGES else 0 end) as \"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                if (hospro == "0")
                {
                    sqlMonth2 += " sum(case when date_time>=to_date('" + startdate + "','yyyymmdd') ";
                    sqlMonth2 += " and  date_time<ADD_MONTHS(to_date('" + startdate + "','yyyymmdd'),1) then incomes-INCOMES_CHARGES else 0 end) as \"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                }

                sqlTotal += "sum(\"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\") as \"实收" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                if (hospro == "0")
                {
                    sqlTotal += "sum(\"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\") as \"计价" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                }


                starttime = starttime.AddMonths(1);
            }

            sqlMonth = sqlMonth.Substring(0, sqlMonth.Length - 1);
            sqlMonth1 = sqlMonth1.Substring(0, sqlMonth1.Length - 1);
            sqlMonth2 = sqlMonth2.Substring(0, sqlMonth2.Length - 1);
            sqlTotal = sqlTotal.Substring(0, sqlTotal.Length - 1);


            string sql = string.Format(@" SELECT   nvl(item_name,'合计') item_name,{0}   FROM   cbhs.cbhs_incoms_info_ACCOUNT a,
                                                   cbhs.CBHS_DISTRIBUTION_CALC_SCHM b,
                                                   comm.sys_dept_dict c
                                           WHERE       a.RECK_ITEM = b.ITEM_CLASS
                                                    and a.dept_code=c.dept_code {1} GROUP BY ROLLUP (item_name)", sqlMonth2, sqlFilter);


            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 核算单位成本报表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetAccountDeptCost(string starttime, string endtime, string balance, string dept_code, string hospro)
        {
            string filter = " where (BALANCE_TAG='" + balance + "' or BALANCE_TAG is null) and ACCOUNTING_DATE>=to_date('" + starttime + "','yyyymmdd') and  ACCOUNTING_DATE<add_months(to_date('" + endtime + "','yyyymmdd'),1) ";
            if (dept_code != "")
            {
                filter += " and DEPT_CODE in (" + dept_code + ")";
            }
            string incomesql = " select ITEM_CODE,ITEM_NAME from cbhs.CBHS_COST_ITEM_DICT order by ITEM_CODE";
            DataSet ds = OracleOledbBase.ExecuteDataSet(incomesql);
            string ss = "";
            string jj = "";
            string total = "";
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return new DataTable();
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                total += " sum(\"" + dr["ITEM_NAME"].ToString() + "\") as \"" + dr["ITEM_NAME"].ToString() + "\",";
                ss += " sum(case when ITEM_CODE='" + dr["ITEM_CODE"].ToString() + "' then COSTS else 0 end) \"" + dr["ITEM_NAME"].ToString() + "\" ,";
                if (hospro == "0")
                {
                    jj += " sum(case when ITEM_CODE='" + dr["ITEM_CODE"].ToString() + "' then COSTS_ARMYFREE else 0 end) \"" + dr["ITEM_NAME"].ToString() + "\" ,";
                }
            }

            total = total.Substring(0, total.Length - 1);
            ss = ss.Substring(0, ss.Length - 1);
            if (hospro == "0")
            {
                jj = jj.Substring(0, jj.Length - 1);
            }


            string sql = " select nvl(ACCOUNT_DEPT_CODE,'8888') dept_code,nvl(ACCOUNT_DEPT_NAME,'未知科室') \"科室\", ";
            sql += " case when ATypeCode=1 then '实际成本' else '计价成本' end as \"核算类别\", ";
            sql += " " + total + " ";
            sql += " from ";
            sql += " ( ";
            sql += " select DEPT_CODE,1 as ATypeCode, ";
            sql += " " + ss + " ";
            sql += " from cbhs.CBHS_DEPT_COST_DETAIL a ";
            sql += filter;
            sql += " group by DEPT_CODE ";
            if (hospro == "0")
            {
                sql += " union ";
                sql += " select DEPT_CODE,2 as ATypeCode, ";
                sql += "" + jj + "";
                sql += " from cbhs.CBHS_DEPT_COST_DETAIL a ";
                sql += filter;
                sql += " group by DEPT_CODE ";
            }
            sql += " ) aa ";
            sql += " left join  ";
            sql += " (select * from comm.SYS_DEPT_DICT where show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)) bb ";
            sql += " on aa.dept_code=bb.dept_code ";
            sql += " group by ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME,ATypeCode ";
            sql += " order by ACCOUNT_DEPT_CODE ";

            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql);
            return dsSql.Tables[0];
        }

        /// <summary>
        /// 核算单位收入报表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetAccountDeptIncome_new(string starttime, string endtime, string balance, string dept_code, string centerincome)
        {
            string filter = "";
            string strcenter = "";
            if (dept_code != "")
            {
                filter += " AND A.DEPT_CODE='" + dept_code + "'";
            }

            //if (centerincome == "1")
            //{
            //    strcenter = "AND A.RECK_ITEM not in ('D14','D18','E10','E12','E15','E19','d20')";
            //}

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"SELECT NVL(B.ACCOUNT_DEPT_CODE,999999) DEPT_CODE,
                                      NVL(B.ACCOUNT_DEPT_NAME,'其他') DEPT_NAME,
                                      SUM(INCOMES) HJ,
                                      SUM(DECODE(COSTS_TAG,0,INCOMES,0)) ZYHJ,
                                      SUM(DECODE(COSTS_TAG,0,INCOMES_CHARGES,0)) ZYSS,
                                      SUM(DECODE(COSTS_TAG,0,INCOMES-INCOMES_CHARGES,0)) ZYJD,
                                      SUM(DECODE(COSTS_TAG,1,INCOMES,0)) MZHJ,
                                      SUM(DECODE(COSTS_TAG,1,INCOMES_CHARGES,0)) MZSS,
                                      SUM(DECODE(COSTS_TAG,1,INCOMES-INCOMES_CHARGES,0)) MZJD
                              FROM    CBHS.CBHS_INCOMS_INFO_ACCOUNT A, 
                                      COMM.SYS_DEPT_DICT B
                             WHERE       A.DEPT_CODE = B.DEPT_CODE(+)
                                     AND A.DATE_TIME >= TO_DATE ('{0}', 'yyyymmdd')
                                     AND A.DATE_TIME < ADD_MONTHS(TO_DATE ('{1}', 'yyyymmdd'),1)
                                     AND A.BALANCE_TAG='{2}'
                                     {3}
                                     {4}
                            GROUP BY B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME
                            ORDER BY NVL(B.ACCOUNT_DEPT_CODE,999999)", starttime, endtime, balance, filter, strcenter);

            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }
        /// <summary>
        /// 查询辅检总额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetAccountDeptZCFCount(string starttime, string dept_code, string balance)
        {
            string filter = "";

            if (dept_code != "")
            {
                filter += " AND A.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();
            if (balance == "1")
            {

                sql.AppendFormat(@" SELECT A.DEPT_NAME 科室名称,
       A.USER_NAME 人员名称,
       round(SUM(A.COSTS),1)分值  
  FROM PERFORMANCE.MZFJ A
   where to_char(a.st_date,'yyyymm')='{0}'
    {2} 
  GROUP BY A.DEPT_NAME,
       A.USER_NAME
    
    ", starttime, balance, filter);
            }
            if (balance == "2")
            {

                sql.AppendFormat(@"  SELECT A.DEPT_NAME 科室名称,
       A.USER_NAME 人员名称,
       round(SUM(A.COSTS),1)分值
  FROM PERFORMANCE.MZZCF A
   where to_char(a.st_date,'yyyymm')='{0}'
    {2} 
  GROUP BY A.DEPT_NAME,
       A.USER_NAME
       
                      
    ", starttime, balance, filter);
            }
            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }

        /// <summary>
        /// 查询诊疗项目金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetAccountDeptZCF(string starttime, string dept_code, string balance)
        {
            string filter = "";
          
            if (dept_code != "")
            {
                filter += " AND A.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();
            if (balance == "1")
            {
                //      CASE WHEN SUM(A.AMOUNT)=0 THEN 0 ELSE  SUM(A.COSTS)/SUM(A.AMOUNT) end 单价,
                sql.AppendFormat(@" SELECT A.DEPT_NAME 科室名称,
       A.USER_NAME 人员名称,
       SUM(A.AMOUNT) 项目数量,
        round(SUM(A.COSTS),1)分值,  
       A.CLASS_NAME 项目名称
  FROM PERFORMANCE.MZFJ A
   where to_char(a.st_date,'yyyymm')='{0}'
    {2} 
  GROUP BY A.DEPT_NAME,
       A.USER_NAME,
          A.CLASS_NAME,A.PRICE
             
    ", starttime, balance, filter);
            }
            if (balance == "2")
            {
                 //CASE WHEN SUM(A.AMOUNT)=0 THEN 0 ELSE    SUM(A.COSTS)/SUM(A.AMOUNT) end   单价,
                sql.AppendFormat(@"  SELECT A.DEPT_NAME 科室名称,
       A.USER_NAME 人员名称,
       SUM(A.AMOUNT) 项目数量,
        round(SUM(A.COSTS),1)分值,
       A.CLASS_NAME 项目名称
  FROM PERFORMANCE.MZZCF A
   where to_char(a.st_date,'yyyymm')='{0}'
    {2} 
  GROUP BY A.DEPT_NAME,
       A.USER_NAME,
          A.CLASS_NAME,
          A.PRICE 
                      
    ", starttime, balance, filter);
            } 
           DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
           return dsSql.Tables[0];
        }
        /// <summary>
        /// 查询挂号金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>

        public DataTable GetAccountDeptGGH(string starttime,string dept_code)
        {
            string filter = "";

            if (dept_code != "")
            {
                filter += " AND A.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"  SELECT A.dept_name 科室名称,
       A.DOCTOR_USER 执行人,
       SUM(A.TCBX)统筹报销,
       SUM(A.GRZF)个人账户支付,
       SUM(A.COSTS)现金,
       SUM(A.ZE)总额
  FROM HISDATA.GH_ITEMS A
 WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
{1}
 GROUP BY A.dept_name,
       A.DOCTOR_USER
 ORDER BY A.DOCTOR_USER                       
    ", starttime, filter);


            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }

        /// <summary>
        /// 查询科室收入
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>

        public DataTable GetAccountDeptcosts(string starttime, string dept_code)
        {
            string filter = "";

            if (dept_code != "")
            {
                filter += " AND A.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT SUM(AMOUNT) 项目次数,
       SUM(COSTS) 分值,
       CLASS_NAME 项目名称,
       DEPT_NAME 科室名称
  FROM (SELECT A.DEPT_NAME DEPT_NAME,
               SUM(A.AMOUNT) AMOUNT,
               SUM(A.COSTS) COSTS,
               A.CLASS_NAME CLASS_NAME
          FROM PERFORMANCE.DEPT_COSTS A
         WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}' 
         {1}
           AND A.COSTS IS NOT NULL
         GROUP BY A.DEPT_NAME, A.CLASS_NAME
        UNION ALL
        SELECT dept_name ||'合计' DEPT_NAME,
           SUM(A.AMOUNT) 项目次数, 
           SUM(A.COSTS) 分值,
            ''
          FROM PERFORMANCE.DEPT_COSTS A
         WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
         {1}
           AND A.COSTS IS NOT NULL
         GROUP BY A.DEPT_NAME, A.CLASS_NAME)
 GROUP BY CLASS_NAME, DEPT_NAME 
 order by  DEPT_NAME  asc          
    ", starttime, filter);


            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }

        public DataTable GetAccountDept_costs(string starttime, string dept_code)
        {
            string filter = "";

            if (dept_code != "")
            {
                filter += " AND A.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();
            //CASE WHEN SUM(A.AMOUNT)=0 THEN 0 ELSE  SUM(A.COSTS)/SUM(A.AMOUNT) end price,
              //price 单价,
        //      UNION ALL
        //SELECT dept_name ||'合计' DEPT_NAME,
        //   SUM(A.AMOUNT) 项目次数, 
        //   SUM(A.COSTS) 分值, 
        //    '',
        //    ''
        //  FROM PERFORMANCE.DEPT_COSTS A
        // WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}'
        // {1}
        //   AND A.COSTS IS NOT NULL
        // GROUP BY A.DEPT_NAME
            sql.AppendFormat(@"SELECT SUM(AMOUNT) 项目次数,
       item_name 项目明细,
       DEPT_NAME 科室名称,
       class_name 项目名称,
       SUM(COSTS) 分值
  FROM (SELECT A.DEPT_NAME DEPT_NAME,
               SUM(A.AMOUNT) AMOUNT,
               SUM(A.COSTS) COSTS,
               A.item_name item_name,
               a.class_name class_name
          FROM PERFORMANCE.DEPT_COSTS A
         WHERE TO_CHAR(A.ST_DATE, 'yyyymm') = '{0}' 
         {1}
           AND A.COSTS IS NOT NULL
         GROUP BY A.DEPT_NAME, A.item_name,a.price,a.class_name
      )
 GROUP BY item_name, DEPT_NAME,class_name
 order by  DEPT_NAME  asc          
    ", starttime, filter);


            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }
        /// <summary>
        /// 查询成本明细
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>

        public DataTable GetChuruku_Seclect(string starttime, string dept_code)
        {
            string filter = "";

            if (dept_code != "")
            {
                filter += " AND A.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@" 
       select sum(a.amount) 数量,
            a.dept_name 科室名称,
       sum(a.costs) 成本金额,
       a.item_name 物资名称,
       a.ipec 规格,
       a.adress 产地,
       a.unit 单位,
       a.Storge 库房标识
  from hisdata.dept_export a
 where a.st_date = to_date('{0}', 'yyyymm') 
{1}
 group by a.item_name, a.ipec, a.adress, a.unit,a.Storge,a.dept_name
order by a.dept_name
    ", starttime, filter);


            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }

        /// <summary>
        /// 查询奖金明细
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetAccountDept_PERFORMANCE(string starttime, string dept_code)
        {
            string filter = "";

            if (dept_code != "")
            {
                filter += " AND A.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"select 
       b.dept_name 科室名称,
       sum(a.jcgz) 基础工资,
       sum(a.jx) 绩效,
       sum(a.xj) 小计,
       sum(a.jxbt) 绩效补,
       sum(a.dsb) 独生补,
       sum(a.zjj) 住基金,
       sum(a.yb)医保,
       sum(a.kdb)扣大病,
       sum(a.kzm)扣账面,
       sum(a.kk)扣款,
       sum(a.zjk)质检扣,
       sum(a.sj)税金,
       sum(a.hj)合计,
       sum(a.sf)实领工资
  from PERFORMANCE.JIANGJIN a,comm.sys_dept_dict b 
       where a.st_date={0} 
       and a.dept_code=b.dept_code
       {1}
       group by b.dept_name,b.sort_no
       order by b.sort_no  
    ", starttime, filter);


            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }
        /// <summary>
        /// 查询草药金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>

        public DataTable GetAccountDeptCy(string starttime, string dept_code)
        {
            string filter = "";

            if (dept_code != "")
            {
                filter += " AND D.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"  SELECT d.dept_name 科室名称,b.user_name 人员姓名,round(SUM(COSTS)*0.15,1) 金额
    FROM HISFACT.outp_class2_income A, HISDATA.USERS B,hisdata.reck_item_class_dict c,hisdata.dept_dict d
    WHERE TO_CHAR(a.ST_DATE, 'yyyymm') = '{0}'
    AND a.ordered_by_doctor=b.user_id
    AND a.reck_class=c.class_code
    and a.ordered_by_dept=d.dept_code
    AND b.user_dept=d.dept_code
    AND c.CLASS_NAME IN('中草药')
    AND d.dept_name NOT LIKE '%专家%'
    {1}
    GROUP BY b.user_name,d.dept_name
    ORDER BY 科室名称
    
                         
    ", starttime, filter);


            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }
        /// <summary>
        /// 获取科室工作量
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>

        public DataTable GetAccountDeptGZL(string stardate, string enddate, string balance, string dept_code)
        {
            string filter = "";

            if (dept_code != "")
            {
                filter += " AND D.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();

            if (balance == "1")
            {
                sql.AppendFormat(@" SELECT SUM(ZJF) ZJF, JF, JBMC, XMMC, DEPT_NAME,sum(AMOUNT)AMOUNT
  FROM (SELECT SUM(B.INP_GRADE * A.AMOUNT) ZJF,
             B.INP_GRADE JF,
               sum(A.AMOUNT) AMOUNT,
               A.ITEM_NAME XMMC,
               B.ITEM_CLASS JBMC,
               D.ACCOUNT_DEPT_NAME DEPT_NAME,
               D.ACCOUNT_DEPT_CODE ACCOUNT_DEPT_CODE
          FROM HISDATA.INP_BILL_DETAIL A,
               HISDATA.LEIBIE_DICT     B,
               HISDATA.LEIBIE          C,
               COMM.SYS_DEPT_DICT      D
              
         WHERE A.ITEM_CODE = C.ITEM_CODE
           AND B.TYPE_CODE = C.GRADE_CLASS
           AND B.ITEM_CLASS = C.GRADE
           AND A.PERFORMED_BY = D.DEPT_CODE
           AND B.TYPE_CODE ={2}
           AND to_char(A.BILLING_DATE_TIME,'yyyy-mm-dd') >= '{0}'
           AND to_char(A.BILLING_DATE_TIME,'yyyy-mm-dd') < '{1}'
           {3}
           AND B.ITEM_NAME NOT IN ('住院诊察费1')
         GROUP BY D.ACCOUNT_DEPT_NAME,
                   B.INP_GRADE,
                  A.AMOUNT,
                  B.ITEM_CLASS,
                  D.ACCOUNT_DEPT_CODE,
                  A.ITEM_NAME
        UNION ALL
        SELECT  SUM(B.INP_GRADE * A.AMOUNT) ZJF,
                B.INP_GRADE JF,
               sum(A.AMOUNT) AMOUNT,
               A.ITEM_NAME XMMC,
               B.ITEM_CLASS JBMC,
               D.ACCOUNT_DEPT_NAME DEPT_NAME,
               D.ACCOUNT_DEPT_CODE ACCOUNT_DEPT_CODE
          FROM HISDATA.OUTP_BILL_ITEMS A,
               HISDATA.LEIBIE_DICT     B,
               HISDATA.LEIBIE          C,
               COMM.SYS_DEPT_DICT      D
             
         WHERE A.ITEM_CODE = C.ITEM_CODE
           AND B.TYPE_CODE = C.GRADE_CLASS
           AND B.ITEM_CLASS = C.GRADE
           AND B.TYPE_CODE = {2}
           AND A.PERFORMED_BY = D.DEPT_CODE
           AND to_char(A.VISIT_DATE,'yyyy-mm-dd') >= '{0}'
           AND to_char(A.VISIT_DATE,'yyyy-mm-dd') < '{1}'
             {3}
           AND B.ITEM_NAME NOT IN ('住院诊察费1')
         GROUP BY D.ACCOUNT_DEPT_NAME,
                   B.INP_GRADE,
                  A.AMOUNT,
                  B.ITEM_CLASS,
                  D.ACCOUNT_DEPT_CODE,
                  A.ITEM_NAME)
 GROUP BY JF, JBMC, XMMC, DEPT_NAME
 ORDER BY XMMC
", stardate, enddate, balance, filter);

            }
            if (balance == "2")
            {
                sql.AppendFormat(@"SELECT SUM(ZJF) ZJF, JF, JBMC, XMMC, DEPT_NAME,sum(AMOUNT)AMOUNT
  FROM (SELECT SUM(B.INP_GRADE * A.AMOUNT) ZJF,
               B.INP_GRADE  JF,
              sum(A.AMOUNT) AMOUNT,
               A.ITEM_NAME XMMC,
               B.ITEM_CLASS JBMC,
               D.ACCOUNT_DEPT_NAME DEPT_NAME,
               D.ACCOUNT_DEPT_CODE ACCOUNT_DEPT_CODE
          FROM HISDATA.INP_BILL_DETAIL A,
               HISDATA.LEIBIE_DICT     B,
               HISDATA.LEIBIE          C,
               COMM.SYS_DEPT_DICT      D
             
         WHERE A.ITEM_CODE = C.ITEM_CODE
           AND B.TYPE_CODE = C.GRADE_CLASS
           AND B.ITEM_CLASS = C.GRADE
           AND B.TYPE_CODE ={2}
           AND A.PERFORMED_BY = D.DEPT_CODE
           AND to_char(A.BILLING_DATE_TIME,'yyyy-mm-dd') >= '{0}'
           AND to_char(A.BILLING_DATE_TIME,'yyyy-mm-dd') < '{1}'
           {3}
           AND B.ITEM_NAME NOT IN ('住院诊察费1')
         GROUP BY D.ACCOUNT_DEPT_NAME,
                  B.INP_GRADE,
                  A.AMOUNT,
                  B.ITEM_CLASS,
                  D.ACCOUNT_DEPT_CODE,
                  A.ITEM_NAME
        UNION ALL
        SELECT  SUM(B.INP_GRADE * A.AMOUNT) ZJF,
               B.INP_GRADE  JF,
              sum(A.AMOUNT)AMOUNT,
               A.ITEM_NAME XMMC,
               B.ITEM_CLASS JBMC,
               D.ACCOUNT_DEPT_NAME DEPT_NAME,
               D.ACCOUNT_DEPT_CODE ACCOUNT_DEPT_CODE
          FROM HISDATA.OUTP_BILL_ITEMS A,
               HISDATA.LEIBIE_DICT     B,
               HISDATA.LEIBIE          C,
               COMM.SYS_DEPT_DICT      D
              
         WHERE A.ITEM_CODE = C.ITEM_CODE
           AND B.TYPE_CODE = C.GRADE_CLASS
           AND B.ITEM_CLASS = C.GRADE
           AND B.TYPE_CODE = {2}
           AND A.PERFORMED_BY = D.DEPT_CODE
          AND to_char(A.VISIT_DATE,'yyyy-mm-dd') >= '{0}'
           AND to_char(A.VISIT_DATE,'yyyy-mm-dd') < '{1}'
             {3}  
           AND B.ITEM_NAME NOT IN ('住院诊察费1')
         GROUP BY D.ACCOUNT_DEPT_NAME,
                  B.INP_GRADE,
                  A.AMOUNT,
                  B.ITEM_CLASS,
                  D.ACCOUNT_DEPT_CODE,
                  A.ITEM_NAME)
 GROUP BY JF, JBMC, XMMC, DEPT_NAME
 ORDER BY XMMC
", stardate, enddate, balance, filter);
            }
            if (balance == "3")
            {

                sql.AppendFormat(@"SELECT SUM(ZJF) ZJF, JF, JBMC, XMMC, DEPT_NAME,sum(AMOUNT)AMOUNT
  FROM (SELECT SUM(B.INP_GRADE * A.AMOUNT) ZJF,
              B.INP_GRADE  JF,
               sum(A.AMOUNT)AMOUNT,
               A.ITEM_NAME XMMC,
               B.ITEM_CLASS JBMC,
               D.ACCOUNT_DEPT_NAME DEPT_NAME,
               D.ACCOUNT_DEPT_CODE ACCOUNT_DEPT_CODE
          FROM HISDATA.INP_BILL_DETAIL A,
               HISDATA.LEIBIE_DICT     B,
               HISDATA.LEIBIE          C,
               COMM.SYS_DEPT_DICT      D,
               HISDATA.USERS           E
         WHERE A.ITEM_CODE = C.ITEM_CODE
           AND B.TYPE_CODE = C.GRADE_CLASS
           AND B.ITEM_CLASS = C.GRADE
           AND A.DOCTOR_USER = E.DB_USER
           AND B.TYPE_CODE ={2}
           AND E.USER_DEPT = D.DEPT_CODE
           AND to_char(A.BILLING_DATE_TIME,'yyyy-mm-dd') >= '{0}'
           AND to_char(A.BILLING_DATE_TIME,'yyyy-mm-dd') < '{1}'
           {3}
           AND B.ITEM_NAME NOT IN ('住院诊察费1')
         GROUP BY D.ACCOUNT_DEPT_NAME,
                  B.INP_GRADE,
                  B.ITEM_CLASS,
                  D.ACCOUNT_DEPT_CODE,
                  A.ITEM_NAME
        UNION ALL
        SELECT  SUM(B.INP_GRADE * A.AMOUNT) ZJF,
             B.INP_GRADE  JF,
               sum(A.AMOUNT) AMOUNT,
               A.ITEM_NAME XMMC,
               B.ITEM_CLASS JBMC,
               D.ACCOUNT_DEPT_NAME DEPT_NAME,
               D.ACCOUNT_DEPT_CODE ACCOUNT_DEPT_CODE
          FROM HISDATA.OUTP_BILL_ITEMS A,
               HISDATA.LEIBIE_DICT     B,
               HISDATA.LEIBIE          C,
               COMM.SYS_DEPT_DICT      D,
               HISDATA.USERS           E
         WHERE A.ITEM_CODE = C.ITEM_CODE
           AND B.TYPE_CODE = C.GRADE_CLASS
           AND B.ITEM_CLASS = C.GRADE
           AND B.TYPE_CODE = {2}
           AND A.ORDERED_BY_DOCTOR = E.DB_USER
           AND E.USER_DEPT = D.DEPT_CODE
           AND to_char(A.VISIT_DATE,'yyyy-mm-dd') >= '{0}'
           AND to_char(A.VISIT_DATE,'yyyy-mm-dd') < '{1}'
             {3}
           AND B.ITEM_NAME NOT IN ('住院诊察费1')
         GROUP BY D.ACCOUNT_DEPT_NAME,
                  B.INP_GRADE,
                  B.ITEM_CLASS,
                  D.ACCOUNT_DEPT_CODE,
                  A.ITEM_NAME)
 GROUP BY JF, JBMC, XMMC, DEPT_NAME
 ORDER BY XMMC
", stardate, enddate, balance, filter);

            }
            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }

          /// <summary>
        /// 获取个人工作量
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>

        public DataTable GetAccountDeptGR_GZL(string stardate, string enddate,  string dept_code)
        {
            string filter = "";

            if (dept_code != "")
            {
                filter += " AND D.DEPT_CODE='" + dept_code + "'";
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"SELECT SUM(ZJF) ZJF, JF, JBMC, XMMC, DEPT_NAME,USER_NAME,sum(AMOUNT)AMOUNT
  FROM (SELECT SUM(B.INP_GRADE * A.AMOUNT) ZJF,
              B.INP_GRADE  JF,
               sum(A.AMOUNT)AMOUNT,
               A.ITEM_NAME XMMC,
               B.ITEM_CLASS JBMC,
               D.ACCOUNT_DEPT_NAME DEPT_NAME,
               D.ACCOUNT_DEPT_CODE ACCOUNT_DEPT_CODE,
               E.USER_NAME USER_NAME
          FROM HISDATA.INP_BILL_DETAIL A,
               HISDATA.LEIBIE_DICT     B,
               HISDATA.LEIBIE          C,
               COMM.SYS_DEPT_DICT      D,
               HISDATA.USERS           E
         WHERE A.ITEM_CODE = C.ITEM_CODE
           AND B.TYPE_CODE = C.GRADE_CLASS
           AND B.ITEM_CLASS = C.GRADE
           AND A.DOCTOR_USER = E.DB_USER
           AND B.TYPE_CODE =3
           {2}
           AND E.USER_DEPT = D.DEPT_CODE
           AND to_char(A.BILLING_DATE_TIME,'yyyy-mm-dd') >= '{0}'
           AND to_char(A.BILLING_DATE_TIME,'yyyy-mm-dd') < '{1}'
           AND B.ITEM_NAME NOT IN ('住院诊察费1')
         GROUP BY D.ACCOUNT_DEPT_NAME,
                  B.INP_GRADE,
                  B.ITEM_CLASS,
                  D.ACCOUNT_DEPT_CODE,
                  A.ITEM_NAME,
                  E.USER_NAME
        UNION ALL
        SELECT  SUM(B.INP_GRADE * A.AMOUNT) ZJF,
             B.INP_GRADE  JF,
               sum(A.AMOUNT) AMOUNT,
               A.ITEM_NAME XMMC,
               B.ITEM_CLASS JBMC,
               D.ACCOUNT_DEPT_NAME DEPT_NAME,
               D.ACCOUNT_DEPT_CODE ACCOUNT_DEPT_CODE,
                E.USER_NAME
          FROM HISDATA.OUTP_BILL_ITEMS A,
               HISDATA.LEIBIE_DICT     B,
               HISDATA.LEIBIE          C,
               COMM.SYS_DEPT_DICT      D,
               HISDATA.USERS           E
         WHERE A.ITEM_CODE = C.ITEM_CODE
           AND B.TYPE_CODE = C.GRADE_CLASS
           AND B.ITEM_CLASS = C.GRADE
           AND B.TYPE_CODE = 3
           AND A.ORDERED_BY_DOCTOR = E.DB_USER
           AND E.USER_DEPT = D.DEPT_CODE
           AND to_char(A.VISIT_DATE,'yyyy-mm-dd') >= '{0}'
           AND to_char(A.VISIT_DATE,'yyyy-mm-dd') < '{1}'
            {2}
           AND B.ITEM_NAME NOT IN ('住院诊察费1')
         GROUP BY D.ACCOUNT_DEPT_NAME,
                  B.INP_GRADE,
                  B.ITEM_CLASS,
                  D.ACCOUNT_DEPT_CODE,
                  A.ITEM_NAME,
                   E.USER_NAME)
 GROUP BY JF, JBMC, XMMC, DEPT_NAME,USER_NAME
 ORDER BY USER_NAME
", stardate, enddate, filter);

            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetAccountDeptIncomeYS_new(string starttime, string endtime, string balance, string dept_code, string hospro, string centerincome)
        {
            string filter = "";
            string strcenter = "";

            if (dept_code != "")
            {
                filter += " AND A.DEPT_CODE='" + dept_code + "'";
            }
            if (centerincome == "1")
            {
                strcenter = "AND B.reck_class not in ('D14','D18','E10','E12','E15','E19','d20')";
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"SELECT 
                                    NVL(A.ACCOUNT_DEPT_CODE,999999) DEPT_CODE,
                                    NVL(A.ACCOUNT_DEPT_NAME,'其他') DEPT_NAME,
                                    SUM(COSTS) HJ,
                                    SUM(DECODE(COSTS_TAG,'1',COSTS,0)) ZYHJ,
                                    SUM(DECODE(COSTS_TAG,'1',CHARGES,0)) ZYSS,
                                    SUM(DECODE(COSTS_TAG,'1',INCOME,0)) ZYJD,
                                    SUM(DECODE(COSTS_TAG,'0',COSTS,0)) MZHJ,
                                    SUM(DECODE(COSTS_TAG,'0',CHARGES,0)) MZSS,
                                    SUM(DECODE(COSTS_TAG,'0',INCOME,0)) MZJD
                                FROM  COMM.SYS_DEPT_DICT A,
                                      HISFACT.DEPT_INCOME_PER_MON B
                                WHERE A.DEPT_CODE(+)=B.DEPT_CODE
                                  AND B.ST_DATE >= TO_DATE('{0}','yyyymmdd')
                                  AND B.ST_DATE <  ADD_MONTHS(TO_DATE ('{1}', 'yyyymmdd'),1)
                                  {4}
                                  AND B.OP_TAG='{2}'
                                  {3}
                                GROUP BY A.ACCOUNT_DEPT_CODE,A.ACCOUNT_DEPT_NAME  
                                ORDER BY NVL(A.ACCOUNT_DEPT_CODE,999999)", starttime, endtime, hospro, filter, strcenter);

            DataSet dsSql = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return dsSql.Tables[0];
        }

        /// <summary>
        /// 各单位核算收入表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="smonth"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable GetAccountDeptMonthIncome(DateTime starttime, string balance, string dept_code, int smonth, string hospro)
        {
            string sqlFilter = " where BALANCE_TAG='" + balance + "'";
            if (dept_code != "")
            {
                sqlFilter += " and DEPT_CODE in (" + dept_code + ")";
            }

            string sqlMonth = "";
            string sqlMonth1 = "";
            string sqlMonth2 = "";
            string sqlMonth3 = "";
            string sqlTotal = "";
            string sqlTotal1 = "";
            for (int i = 0; i < smonth; i++)
            {
                string nowmonth = starttime.Month.ToString();
                if (nowmonth.Length == 1)
                {
                    nowmonth = "0" + nowmonth;
                }
                string startdate = starttime.Year.ToString() + nowmonth + "01";

                sqlMonth += "\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                sqlMonth3 += "\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"+";


                sqlMonth1 += "nvl(sum(\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"),0) as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                if (hospro == "0")
                {
                    sqlMonth2 += " sum(case when date_time>=to_date('" + startdate + "','yyyymmdd') ";
                    sqlMonth2 += " and  date_time<ADD_MONTHS(to_date('" + startdate + "','yyyymmdd'),1) then incomes else 0 end) as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";
                }
                else
                {
                    sqlMonth2 += " sum(case when date_time>=to_date('" + startdate + "','yyyymmdd') ";
                    sqlMonth2 += " and  date_time<ADD_MONTHS(to_date('" + startdate + "','yyyymmdd'),1) then INCOMES_CHARGES else 0 end) as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                }

                sqlTotal += "sum(\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\") as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                sqlTotal1 += "sum(\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\") +";

                starttime = starttime.AddMonths(1);
            }

            sqlMonth = sqlMonth.Substring(0, sqlMonth.Length - 1);
            sqlMonth1 = sqlMonth1.Substring(0, sqlMonth1.Length - 1);
            sqlMonth2 = sqlMonth2.Substring(0, sqlMonth2.Length - 1);
            sqlMonth3 = sqlMonth3.Substring(0, sqlMonth3.Length - 1) + " as \"累计额\" ";
            sqlTotal = sqlTotal.Substring(0, sqlTotal.Length - 1);
            sqlTotal1 = sqlTotal1.Substring(0, sqlTotal1.Length - 1);

            string sql = " select dept_code,\"科室\"," + sqlMonth3 + "," + sqlMonth + "";
            sql += " from";
            sql += " (";
            sql += " select aa.dept_code,nvl(dept_name,'未知科室') \"科室\"," + sqlMonth + "";
            sql += " from";
            sql += " (";
            sql += " select nvl(ACCOUNT_DEPT_CODE,8888) dept_code," + sqlMonth1 + "   from ";
            sql += " (select  dept_code, ";
            sql += "  " + sqlMonth2 + "";
            sql += " from cbhs.cbhs_incoms_info_ACCOUNT " + sqlFilter;
            sql += " group by dept_code) a left join  ";
            sql += " (select * from COMM.SYS_DEPT_DICT where DEPT_TYPE in ('0','1','2') and show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)) b   ";
            sql += " on a.dept_code=b.dept_code ";
            sql += " group by ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME ";
            sql += " ) aa ";
            sql += " left join  ";
            sql += " (select * from comm.SYS_DEPT_DICT where show_flag=0) bb ";
            sql += " on aa.dept_code=bb.dept_code ";
            sql += " order by sort_no ";
            sql += " ) aaa ";
            sql += " union ";
            sql += " select '999999','合计'," + sqlTotal1 + "," + sqlTotal + "";
            sql += " from ";
            sql += " ( ";
            sql += " select nvl(ACCOUNT_DEPT_CODE,8888) dept_code, ";
            sql += " " + sqlTotal + "  from ";
            sql += " (select  dept_code, ";
            sql += " " + sqlMonth2 + "";
            sql += " from cbhs.cbhs_incoms_info_ACCOUNT " + sqlFilter;
            sql += " group by dept_code) a left join  ";
            sql += " (select * from COMM.SYS_DEPT_DICT where DEPT_TYPE in ('0','1','2') and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)) b   ";
            sql += " on a.dept_code=b.dept_code ";
            sql += " group by ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME ";
            sql += " ) bbb ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 各单位月份纯收入表
        /// <param name="starttime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetAccountDeptMonthPurIncome(DateTime starttime, string balance, string dept_code, int smonth)
        {
            string sqlFilter = " and BALANCE_TAG='" + balance + "'";
            if (dept_code != "")
            {
                sqlFilter += " and a.DEPT_CODE in (" + dept_code + ")";
            }

            string sqlMonth = "";
            string sqlMonth1 = "";
            string sqlMonth2 = "";
            string sqlMonth3 = "";
            string sqlTotal = "";
            string sqlTotal1 = "";
            for (int i = 0; i < smonth; i++)
            {
                string nowmonth = starttime.Month.ToString();
                if (nowmonth.Length == 1)
                {
                    nowmonth = "0" + nowmonth;
                }
                string startdate = starttime.Year.ToString() + nowmonth + "01";

                sqlMonth += "\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                sqlMonth3 += "\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"+";


                sqlMonth1 += "nvl(sum(\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\"),0) as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";


                sqlMonth2 += " sum(case when DATE_TIME>=to_date('" + startdate + "','yyyymmdd') ";
                sqlMonth2 += " and  DATE_TIME<ADD_MONTHS(to_date('" + startdate + "','yyyymmdd'),1) then NET_INCOME else 0 end) as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";


                sqlTotal += "sum(\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\") as \"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\",";

                sqlTotal1 += "sum(\"" + starttime.Year.ToString() + "-" + starttime.Month.ToString() + "\") +";

                starttime = starttime.AddMonths(1);
            }

            sqlMonth = sqlMonth.Substring(0, sqlMonth.Length - 1);
            sqlMonth1 = sqlMonth1.Substring(0, sqlMonth1.Length - 1);
            sqlMonth2 = sqlMonth2.Substring(0, sqlMonth2.Length - 1);
            sqlMonth3 = sqlMonth3.Substring(0, sqlMonth3.Length - 1) + " as \"累计额\" ";
            sqlTotal = sqlTotal.Substring(0, sqlTotal.Length - 1);
            sqlTotal1 = sqlTotal1.Substring(0, sqlTotal1.Length - 1);

            string sql = " select dept_code,dept_name \"科室\" ," + sqlMonth + " ";
            sql += " from ( ";
            sql += " select nvl(aa.dept_code,'8888') dept_code,nvl(bb.dept_name,'未知科室') dept_name,";
            sql += sqlMonth;
            sql += " from ";
            sql += " ( ";
            sql += " select ACCOUNT_DEPT_CODE DEPT_CODE, ";
            sql += sqlMonth2;
            sql += " from ";
            sql += " cbhs.CBHS_DEPT_ACCOUNT_DETAIL a, ";
            sql += " (select * from COMM.SYS_DEPT_DICT where DEPT_TYPE in ('0','1','2') and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)) b ";
            sql += " where a.DEPT_CODE=b.DEPT_CODE(+) " + sqlFilter;
            sql += " group by ACCOUNT_DEPT_CODE ";
            sql += " ) aa, ";
            sql += "  (select * from comm.SYS_DEPT_DICT where show_flag=0) bb ";
            sql += " where aa.dept_code=bb.dept_code(+) ";
            sql += " order by bb.SORT_NO,aa.dept_code ) aaa ";
            sql += " union all ";
            sql += " select '99999','合计', ";
            sql += sqlTotal;
            sql += " from ";
            sql += " ( ";
            sql += " select ACCOUNT_DEPT_CODE DEPT_CODE, ";
            sql += sqlMonth2;
            sql += " from ";
            sql += " cbhs.CBHS_DEPT_ACCOUNT_DETAIL a, ";
            sql += " (select * from COMM.SYS_DEPT_DICT where DEPT_TYPE in ('0','1','2') and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)) b ";
            sql += " where a.DEPT_CODE=b.DEPT_CODE(+) " + sqlFilter;
            sql += " group by ACCOUNT_DEPT_CODE ";
            sql += " ) ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        /// <summary>
        ///同期毛收入对比统计
        /// <summary>
        public DataTable GetGrossIncomeCompare(string starttime, string endtime, string balance, string dept_code, string hospro)
        {
            string deptcode = "";
            if (dept_code != "")
            {
                deptcode = " and dept_code in (" + dept_code + ")";
            }
            string start = Convert.ToDateTime(starttime).ToString("yyyyMMdd");
            string end = Convert.ToDateTime(endtime).AddMonths(1).ToString("yyyyMMdd");
            string Tstarttime = Convert.ToDateTime(starttime).AddYears(-1).ToString("yyyyMMdd");
            string Tendtime = Convert.ToDateTime(endtime).AddMonths(1).AddYears(-1).ToString("yyyyMMdd");

            string sql = " SELECT dept_type, ROW_NUMBER () OVER (ORDER BY dept_type) AS ID, ";
            sql += "  dept_name, mz1, zy1, total1, mz2, zy2, total2,";
            sql += "   (CASE";
            sql += "       WHEN mz2 = 0";
            sql += "          THEN 0";
            sql += "       ELSE ROUND ((mz1 - mz2) / mz2, 3)";
            sql += "    END) AS mz3,";
            sql += "   (CASE";
            sql += "       WHEN zy2 = 0";
            sql += "          THEN 0";
            sql += "       ELSE ROUND ((zy1 - zy2) / zy2, 3)";
            sql += "    END) AS zy3,";
            sql += "   (CASE";
            sql += "       WHEN total2 = 0";
            sql += "          THEN 0";
            sql += "       ELSE ROUND ((total1 - total2) / total2, 3)";
            sql += "    END";
            sql += "   ) total3,";
            sql += "   (total1 - total2) AS totaladd";
            sql += "   FROM (SELECT '辅诊科室' dept_type, dept_name, nvl(mz1,0) mz1, nvl(zy1,0) zy1 ,";
            sql += "      NVL (mz1, 0) + NVL (zy1, 0) AS total1, nvl(mz2,0) mz2, nvl(zy2,0) zy2,";
            sql += "      NVL (mz2, 0) + NVL (zy2, 0) AS total2";
            sql += "      FROM (SELECT   dept.ACCOUNT_DEPT_CODE,";
            sql += "           SUM(CASE WHEN st_date >=TO_DATE ('" + start + "', 'yyyymmdd') AND st_date < TO_DATE ('" + end + "', 'yyyymmdd') ";
            sql += "            THEN income.charges END) AS mz1,";
            sql += "             SUM(CASE WHEN st_date >=TO_DATE ('" + Tstarttime + "', 'yyyymmdd') AND st_date < TO_DATE ('" + Tendtime + "', 'yyyymmdd')";
            sql += "             THEN income.charges END) AS mz2";
            sql += "      FROM ";
            sql += "      hisfact.outp_class2_income income,";
            sql += "             comm.SYS_DEPT_DICT dept";
            sql += "     WHERE DEPT_TYPE=1 and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)";
            sql += "       AND dept.dept_code = income.performed_by";
            sql += "   GROUP BY dept.ACCOUNT_DEPT_CODE) mzincomes,";
            sql += "   (SELECT   dept.ACCOUNT_DEPT_CODE,";
            sql += "              SUM(CASE WHEN st_date >=TO_DATE ('" + start + "', 'yyyymmdd') AND st_date < TO_DATE ('" + end + "', 'yyyymmdd') ";
            sql += "             THEN income.charges END) AS zy1,";
            sql += "             SUM(CASE WHEN st_date >=TO_DATE ('" + Tstarttime + "', 'yyyymmdd') AND st_date < TO_DATE ('" + Tendtime + "', 'yyyymmdd') ";
            sql += "             THEN income.charges END)AS zy2";
            sql += "        FROM ";
            if (balance == "1")
            {
                sql += "        hisfact.INP_CLASS2_INCOME_SETTLE income,";
            }
            else
            {
                sql += "        hisfact.inp_class2_income income,";
            }
            sql += "            comm.SYS_DEPT_DICT  dept";
            sql += "       WHERE DEPT_TYPE=1 and  show_flag=0  and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)";
            sql += "         AND dept.dept_code = income.performed_by";
            sql += "    GROUP BY dept.ACCOUNT_DEPT_CODE) zyincomes,";
            sql += "    (select account_dept_code dept_code,account_dept_name dept_name";
            sql += "    from comm.sys_dept_dict  WHERE DEPT_TYPE=1 and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0) ";
            sql += " " + deptcode + " group by account_dept_code,account_dept_name) dept";
            sql += "      WHERE dept.dept_code = mzincomes.ACCOUNT_DEPT_CODE(+)";
            sql += "        AND dept.dept_code = zyincomes.ACCOUNT_DEPT_CODE(+)";
            sql += "     UNION ALL";
            sql += "      SELECT '临床及门诊科室' dept_type, dept_name, nvl(mz1,0) mz1, nvl(zy1,0) zy1,";
            sql += "            NVL (mz1, 0) + NVL (zy1, 0) AS total1, nvl(mz2,0) mz2, nvl(zy2,0) zy2,";
            sql += "            NVL (mz2, 0) + NVL (zy2, 0) AS total2";
            sql += "       FROM (SELECT   dept.ACCOUNT_DEPT_CODE,";
            sql += "               SUM(CASE WHEN st_date >=TO_DATE ('" + start + "', 'yyyymmdd') AND st_date < TO_DATE ('" + end + "', 'yyyymmdd')";
            sql += "                         THEN income.charges  END ) AS mz1,";
            sql += "               SUM(CASE WHEN st_date >=TO_DATE ('" + Tstarttime + "', 'yyyymmdd') AND st_date < TO_DATE ('" + Tendtime + "', 'yyyymmdd')";
            sql += "                         THEN income.charges  END ) AS mz2";
            sql += "         FROM ";
            sql += "         hisfact.outp_class2_income income,";
            sql += "              comm.sys_dept_dict dept";
            sql += "        WHERE DEPT_TYPE=0 and  show_flag=0 ";
            sql += "         AND dept.dept_code = income.ordered_by_dept";
            sql += "      GROUP BY dept.ACCOUNT_DEPT_CODE) mzincomes,";
            sql += "     (SELECT   dept.ACCOUNT_DEPT_CODE,";
            sql += "               SUM(CASE WHEN st_date >= TO_DATE ('" + start + "', 'yyyymmdd') AND st_date < TO_DATE ('" + end + "', 'yyyymmdd')";
            sql += "                         THEN income.charges  END ) AS zy1,";
            sql += "               SUM(CASE WHEN st_date >= TO_DATE ('" + Tstarttime + "', 'yyyymmdd') AND st_date < TO_DATE ('" + Tendtime + "', 'yyyymmdd')";
            sql += "                         THEN income.charges  END) AS zy2";
            sql += "          FROM ";
            if (balance == "1")
            {
                sql += "  hisfact.INP_CLASS2_INCOME_SETTLE income,";
            }
            else
            {
                sql += "  hisfact.inp_class2_income income,";
            }
            sql += "            comm.sys_dept_dict dept";
            sql += "       WHERE DEPT_TYPE=0 and  show_flag=0  and dept.ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)";
            sql += "          AND dept.dept_code = income.ordered_by_dept";
            sql += "     GROUP BY dept.ACCOUNT_DEPT_CODE) zyincomes,";
            sql += "     (select account_dept_code dept_code,account_dept_name dept_name";
            sql += "     from comm.sys_dept_dict  WHERE DEPT_TYPE=0 and  show_flag=0  and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0) ";
            sql += "     " + deptcode + " group by account_dept_code,account_dept_name) dept";
            sql += "    WHERE dept.dept_code = mzincomes.ACCOUNT_DEPT_CODE(+)";
            sql += "      AND dept.dept_code = zyincomes.ACCOUNT_DEPT_CODE(+))";
            //sql += "  UNION ALL";
            //sql += "  SELECT '合计' dept_type, 0 ID, '合计' dept_name, smz1, szy1,";
            //sql += "         (smz1 + szy1) total1, smz2, szy2, (smz2 + szy2) tatal2,";
            //sql += "         ROUND ((smz1 - smz2) / smz2, 3) mzzf,";
            //sql += "        ROUND ((szy1 - szy2) / szy2, 3) zyzf,";
            //sql += "        ROUND ((smz1 + szy1 - smz2 - szy2) / (smz2 / szy2), 3) tzf,";
            //sql += "       (smz1 + szy1 - smz2 - szy2) txzf";
            //sql += "   FROM (SELECT nvl((SELECT SUM (a.charges)";
            //sql += "                 FROM ";
            //sql += "                hisfact.outp_class2_income a";
            //sql += "               WHERE a.st_date >= TO_DATE ('" + start + "', 'yyyymmdd')";
            //sql += "                 AND st_date < TO_DATE ('" + end + "', 'yyyymmdd')),0) smz1,";
            //sql += "             nvl((SELECT SUM (a.charges)";
            //sql += "                FROM ";
            //if (balance == "1")
            //{
            //    sql += "        hisfact.INP_CLASS2_INCOME_SETTLE a where 1=1 " + deptcode;
            //}
            //else
            //{
            //    sql += "        hisfact.inp_class2_income a where 1=1" + deptcode;
            //}
            //sql += "              and a.st_date >= TO_DATE ('" + start + "', 'yyyymmdd')";
            //sql += "               AND st_date < TO_DATE ('" + end + "', 'yyyymmdd')),0) szy1,";
            //sql += "           nvl((SELECT SUM (a.charges)";
            //sql += "                FROM ";
            //sql += "              hisfact.outp_class2_income a where 1=1 " + deptcode;
            //sql += "             and a.st_date >= TO_DATE ('" + Tstarttime + "', 'yyyymmdd')";
            //sql += "                AND st_date < TO_DATE ('" + Tendtime + "', 'yyyymmdd')),0) smz2,";
            //sql += "          nvl((SELECT SUM (a.charges)";
            //sql += "          FROM ";
            //if (balance == "1")
            //{
            //    sql += "        hisfact.INP_CLASS2_INCOME_SETTLE a where 1=1 " + deptcode;
            //}
            //else
            //{
            //    sql += "        hisfact.inp_class2_income a where 1=1 " + deptcode;
            //}
            //sql += "          and a.st_date >= TO_DATE ('" + Tstarttime + "', 'yyyymmdd')";
            //sql += "           AND st_date < TO_DATE ('" + Tendtime + "', 'yyyymmdd')),0) szy2";
            //sql += "     FROM DUAL) a";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        ///收入按年度对比
        /// <summary>
        public DataTable GetYearIncomeCompare(string starttime, string endtime, string balance, string dept_code, string hospro)
        {
            string strSqlcol = "select ACCOUNT_DEPT_CODE dept_code,ACCOUNT_DEPT_NAME dept_name from comm.sys_dept_dict  where  DEPT_TYPE in ('0','1','2') and  show_flag=0  and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)  ";
            if (dept_code != "")
            {
                strSqlcol += " and dept_code in (" + dept_code + ")";
            }
            strSqlcol += (" group by ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME  ");
            strSqlcol += (" union select '88888' as dept_code,'未知科室' as dept_name from dual ");
            DataSet dscol = OracleOledbBase.ExecuteDataSet(strSqlcol);


            string sql1 = "";
            string sql2 = "";
            string sql3 = "";
            string sql4 = "";
            for (int i = 0; i < dscol.Tables[0].Rows.Count; i++)
            {
                string deptname = dscol.Tables[0].Rows[i]["dept_name"].ToString();
                if (deptname.Length > 15)
                    deptname = deptname.Substring(deptname.Length - 15, 15);
                sql1 += "sum(case when dept_code='" + dscol.Tables[0].Rows[i]["dept_code"] + "' then incomes_charges else 0 end) as \"" + deptname + "\",";

                sql2 += "sum(case when dept_code='" + dscol.Tables[0].Rows[i]["dept_code"] + "' then charges else 0 end) as \"" + deptname + "\",";


            }

            sql3 += "sum(incomes_charges) as \"小计\",";

            sql4 += "sum(charges) as \"小计\",";

            sql1 = sql1.Substring(0, sql1.Length - 1);
            sql2 = sql2.Substring(0, sql2.Length - 1);

            string sql = " select years as \"年度\",'实际收入' as \"收入类别\"," + sql3;
            sql += sql1;
            sql += " from ( ";
            sql += " select to_char(DATE_TIME,'yyyy') Years, ";
            sql += " nvl(ACCOUNT_DEPT_CODE,'88888') dept_code,sum(incomes_charges) incomes_charges ";
            sql += " from ";
            sql += " cbhs.cbhs_incoms_info_ACCOUNT a, ";
            sql += " (select * from comm.sys_dept_dict where DEPT_TYPE in ('0','1','2') and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0 )) b ";
            sql += " where a.dept_code=b.dept_code(+)  and BALANCE_TAG='" + balance + "' and ( DATE_TIME>=to_date('" + starttime + "','yyyy-mm-dd') and DATE_TIME<to_date('" + endtime + "','yyyy-mm-dd'))";
            if (dept_code != "")
            {
                sql += " and b.dept_code in (" + dept_code + ")";
            }
            sql += " group by ACCOUNT_DEPT_CODE,to_char(DATE_TIME,'yyyy')) ";
            sql += " group by years ";
            if (hospro == "0")
            {
                sql += " union  ";
                sql += " select years as \"年度\",'计价收入' as \"收入类别\"," + sql4;
                sql += sql2;
                sql += " from ( ";
                sql += " select to_char(DATE_TIME,'yyyy') Years, ";
                sql += " nvl(ACCOUNT_DEPT_CODE,'88888') dept_code,sum(incomes-incomes_charges) charges ";
                sql += " from ";
                sql += " cbhs.cbhs_incoms_info_ACCOUNT a, ";
                sql += " (select * from comm.sys_dept_dict where DEPT_TYPE in ('0','1','2') and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0 )) b ";
                sql += " where a.dept_code=b.dept_code(+)  and BALANCE_TAG='" + balance + "' and ( DATE_TIME>=to_date('" + starttime + "','yyyy-mm-dd') and DATE_TIME<to_date('" + endtime + "','yyyy-mm-dd')) ";
                if (dept_code != "")
                {
                    sql += " and b.dept_code in (" + dept_code + ")";
                }
                sql += " group by ACCOUNT_DEPT_CODE,to_char(DATE_TIME,'yyyy')) ";
                sql += " group by years ";
            }

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        ///收入按月份对比
        /// <summary>
        public DataTable GetMonthIncomeCompare(string starttime, string endtime, string balance, string dept_code, string hospro)
        {
            string strSqlcol = "select ACCOUNT_DEPT_CODE dept_code,ACCOUNT_DEPT_NAME dept_name from comm.sys_dept_dict  where  DEPT_TYPE in ('0','1','2') and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)  ";
            if (dept_code != "")
            {
                strSqlcol += " and dept_code in (" + dept_code + ")";
            }
            strSqlcol += (" group by ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME  ");
            strSqlcol += (" union select '88888' as dept_code,'未知科室' as dept_name from dual ");
            DataSet dscol = OracleOledbBase.ExecuteDataSet(strSqlcol);


            string sql1 = "";
            string sql2 = "";
            string sql3 = "";
            string sql4 = "";
            for (int i = 0; i < dscol.Tables[0].Rows.Count; i++)
            {
                string deptname = dscol.Tables[0].Rows[i]["dept_name"].ToString();
                if (deptname.Length > 15)
                    deptname = deptname.Substring(deptname.Length - 15, 15);
                sql1 += "sum(case when dept_code='" + dscol.Tables[0].Rows[i]["dept_code"] + "' then incomes_charges else 0 end) as \"" + deptname + "\",";

                sql2 += "sum(case when dept_code='" + dscol.Tables[0].Rows[i]["dept_code"] + "' then charges else 0 end) as \"" + deptname + "\",";


            }

            sql3 += "sum(incomes_charges) as \"小计\",";

            sql4 += "sum(charges) as \"小计\",";

            sql1 = sql1.Substring(0, sql1.Length - 1);
            sql2 = sql2.Substring(0, sql2.Length - 1);

            string sql = " select years as \"月份\",'实际收入' as \"收入类别\"," + sql3;
            sql += sql1;
            sql += " from ( ";
            sql += " select to_char(DATE_TIME,'yyyy-MM') Years, ";
            sql += " nvl(ACCOUNT_DEPT_CODE,'88888') dept_code,sum(incomes_charges) incomes_charges ";
            sql += " from ";
            sql += " cbhs.cbhs_incoms_info_ACCOUNT a, ";
            sql += " (select * from comm.sys_dept_dict where DEPT_TYPE in ('0','1','2')  and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)) b ";
            sql += " where a.dept_code=b.dept_code(+)  and BALANCE_TAG='" + balance + "' and ( DATE_TIME>=to_date('" + starttime + "','yyyy-mm-dd') and DATE_TIME<ADD_MONTHS(to_date('" + endtime + "','yyyy-mm-dd'),1))";
            if (dept_code != "")
            {
                sql += " and b.dept_code in (" + dept_code + ")";
            }
            sql += " group by ACCOUNT_DEPT_CODE,to_char(DATE_TIME,'yyyy-MM')) ";
            sql += " group by years ";
            if (hospro == "0")
            {
                sql += " union  ";
                sql += " select years as \"月份\",'计价收入' as \"收入类别\"," + sql4;
                sql += sql2;
                sql += " from ( ";
                sql += " select to_char(DATE_TIME,'yyyy-MM') Years, ";
                sql += " nvl(ACCOUNT_DEPT_CODE,'88888') dept_code,sum(incomes-incomes_charges) charges ";
                sql += " from ";
                sql += " cbhs.cbhs_incoms_info_ACCOUNT a, ";
                sql += " (select * from comm.sys_dept_dict where DEPT_TYPE in ('0','1','2') and  show_flag=0 and ACCOUNT_DEPT_CODE in (select dept_code from comm.sys_dept_dict where show_flag = 0)) b ";
                sql += " where a.dept_code=b.dept_code(+)  and BALANCE_TAG='" + balance + "' and ( DATE_TIME>=to_date('" + starttime + "','yyyy-mm-dd') and DATE_TIME<ADD_MONTHS(to_date('" + endtime + "','yyyy-mm-dd'),1)) ";
                if (dept_code != "")
                {
                    sql += " and b.dept_code in (" + dept_code + ")";
                }
                sql += " group by ACCOUNT_DEPT_CODE,to_char(DATE_TIME,'yyyy-MM')) ";
                sql += " group by years ";
            }

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 核算科目医疗收入统计报表--收入报表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable DeptCostAccountIncome(string starttime, string endtime, string balance, string dept_code, string hospro)
        {
            string condition = " where DATE_TIME>=to_date('" + starttime + "','yyyymmdd') and  DATE_TIME<add_months(to_date('" + endtime + "','yyyymmdd'),1) ";
            condition += " and balance_tag='" + balance + "' ";
            if (dept_code != "")
            {
                condition += dept_code;
            }
            string sql = " select ";
            sql += " RECK_ITEM,b.CLASS_NAME INCOM_TYPE_NAME,b.ITEM_NAME, ";
            sql += " to_char(round(sum(INCOMES_CHARGES),2)) INCOMES_CHARGES, ";
            if (hospro == "0")
            {
                sql += " to_char(round(sum(INCOMES-INCOMES_CHARGES),2)) CHARGES, ";
            }
            if (hospro == "0")
            {
                sql += " to_char(round(sum(INCOMES),2)) INCOMES, ";
            }
            else
            {
                sql += " to_char(round(sum(INCOMES_CHARGES),2)) INCOMES, ";
            }
            if (hospro == "0")
            {
                sql += " to_char(round((sum(INCOMES)/(select sum(INCOMES) from cbhs.CBHS_INCOMS_INFO a " + condition + "))*100,2)) as RATE ";
            }
            else
            {
                sql += " to_char(round((sum(INCOMES_CHARGES)/(select sum(INCOMES_CHARGES) from cbhs.CBHS_INCOMS_INFO  a " + condition + "))*100,2)) as RATE ";
            }
            //sql += @" ,0 armcosts ";
            sql += " from ";
            sql += " (select * from cbhs.CBHS_INCOMS_INFO a " + condition + ") a, ";
            sql += " cbhs.CBHS_DISTRIBUTION_CALC_SCHM b, ";
            sql += string.Format(" comm.sys_dept_dict n", DataUser.HISFACT);
            sql += " where a.RECK_ITEM=b.ITEM_CLASS(+) and A.DEPT_CODE=n.dept_code";
            sql += " group by b.CLASS_NAME,b.CLASS_TYPE, b.ITEM_NAME, a.RECK_ITEM ";
            sql += " order by b.CLASS_TYPE,a.RECK_ITEM ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable DeptCostAccountIncomen(string starttime, string endtime, string balance, string dept_code, string hospro)
        {
            string condition = " a.DATE_TIME>=to_date('" + starttime + "','yyyymmdd') and  a.DATE_TIME<add_months(to_date('" + endtime + "','yyyymmdd'),1) ";
            condition += " and a.balance_tag='" + balance + "' ";
            if (dept_code != "")
            {
                condition += dept_code;
            }
            string sql = " SELECT ";
            sql += " RECK_ITEM,INCOM_TYPE_NAME,ITEM_NAME,INCOMES_CHARGES,CHARGES,INCOMES, ";
            sql += " ROUND(INCOMES/SUM(INCOMES) OVER()*100,2) RATE";
            sql += " FROM ( SELECT ";
            sql += " RECK_ITEM,b.CLASS_NAME INCOM_TYPE_NAME,b.ITEM_NAME, ";
            sql += " ROUND(SUM(INCOMES_CHARGES),2) INCOMES_CHARGES, ";
            if (hospro == "0")
            {
                sql += " ROUND(SUM(INCOMES-INCOMES_CHARGES),2) CHARGES, ";
            }
            if (hospro == "0")
            {
                sql += " ROUND(SUM(INCOMES),2) INCOMES ";
            }
            else
            {
                sql += " round(sum(INCOMES_CHARGES),2) INCOMES ";
            }
            //if (hospro == "0")
            //{
            //    sql += " round((sum(INCOMES)/MAX(INCOMES_SUM))*100,2) as RATE ";
            //}
            //else
            //{
            //    sql += " round((sum(INCOMES_CHARGES)/MAX(INCOMES_CHARGES_SUM))*100,2) as RATE ";
            //}
            //sql += @" ,0 armcosts ";
            sql += " from ";
            sql += " cbhs.CBHS_DISTRIBUTION_CALC_SCHM b, ";
            sql += " comm.sys_dept_dict n,";
            //sql += "(select sum(INCOMES_CHARGES) INCOMES_CHARGES_SUM,sum(INCOMES) INCOMES_SUM from cbhs.CBHS_INCOMS_INFO  a where" + condition + ") d,";
            //sql += " cbhs.CBHS_INCOMS_INFO_ACCOUNT a ";
            sql += " cbhs.cbhs_incoms_info_ACCOUNT a ";
            sql += " where a.RECK_ITEM=b.ITEM_CLASS(+) and A.DEPT_CODE=n.dept_code(+) and " + condition;
            sql += " group by b.CLASS_NAME,b.CLASS_TYPE, b.ITEM_NAME, a.RECK_ITEM ";
            sql += " order by b.CLASS_TYPE,a.RECK_ITEM )";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 核算科目医疗收入统计报表--成本报表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable DeptCostAccountCost(string starttime, string endtime, string balance, string dept_code, string hospro)
        {
            string condition = " a.ACCOUNTING_DATE>=to_date('" + starttime + "','yyyymmdd') and  a.ACCOUNTING_DATE<add_months(to_date('" + endtime + "','yyyymmdd'),1) ";
            condition += " and (a.balance_tag='" + balance + "' or a.balance_tag is null)";
            if (dept_code != "")
            {
                condition += dept_code;
            }

            string sql = " SELECT ";
            sql += " CASE WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code||c.COST_TYPE_NAME) = 1 THEN ' ' ELSE c.cost_type_code||c.COST_TYPE_NAME END COST_TYPE_NAME,";
            sql += " CASE WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code||c.COST_TYPE_NAME) = 0 THEN c.cost_type_code||c.COST_TYPE_NAME ||'' WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code||c.COST_TYPE_NAME) = 1 THEN ' 合计' ELSE '　　' ||b.ITEM_NAME END ITEM_NAME, ";
            sql += " CASE WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code || c.COST_TYPE_NAME) = 1 THEN 'A'||MAX(A.ITEM_CODE) WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code || c.COST_TYPE_NAME) = 0 THEN 'B'||MAX(A.ITEM_CODE) ELSE MAX(A.ITEM_CODE) END ITEM_CODE,";
            sql += " sum(COSTS) COSTS,";
            if (hospro == "0")
            {
                sql += " sum(COSTS_ARMYFREE) COSTS_ARMYFREE, ";
            }
            if (hospro == "0")
            {
                sql += " sum(COSTS+COSTS_ARMYFREE) TOTALCOST ";
                //sql += " round(sum(COSTS+COSTS_ARMYFREE)/(select sum(COSTS+COSTS_ARMYFREE) from cbhs.CBHS_DEPT_COST_DETAIL a " + condition + ")*100,2) as RATE ";
            }
            else
            {
                sql += " sum(COSTS) TOTALCOST ";
                //sql += " round(sum(COSTS)/(select sum(COSTS) from cbhs.CBHS_DEPT_COST_DETAIL a " + condition + ")*100,2) as RATE ";
            }
            sql += " from ";
            //sql += " (select * from cbhs.CBHS_DEPT_COST_DETAIL a " + condition + ") a, ";
            sql += " cbhs.CBHS_COST_ITEM_DICT b, ";
            sql += " cbhs.CBHS_COST_TYPE_DICT c, ";
            sql += " cbhs.CBHS_DEPT_COST_DETAIL a ";
            sql += " where a.ITEM_CODE=b.ITEM_CODE ";
            sql += "   and SUBSTR (a.ITEM_CODE, 0, 1) = c.COST_TYPE_CODE and " + condition;
            sql += " group by ROLLUP (c.cost_type_code||c.COST_TYPE_NAME, b.ITEM_NAME) ";
            sql += " order by c.cost_type_code||c.COST_TYPE_NAME, b.ITEM_NAME DESC ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable DeptCostAccountCostByType(string starttime, string endtime, string balance, string dept_code, string hospro, string type)
        {
            //成本分解前的成本数据，来自单项成本录入
            string tb = "CBHS.CBHS_DEPT_COST_DETAIL_COMMIT";

            if (type == "1")
            {
                //成本分解后数据表
                tb = "CBHS.CBHS_DEPT_COST_DETAIL";
            }

            string condition = " a.ACCOUNTING_DATE>=to_date('" + starttime + "','yyyymmdd') and  a.ACCOUNTING_DATE<add_months(to_date('" + endtime + "','yyyymmdd'),1) ";
            condition += " and (a.balance_tag='" + balance + "' or a.balance_tag is null)";
            if (dept_code != "")
            {
                condition += dept_code;
            }

            string sql = " SELECT ";
            sql += " CASE WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code||c.COST_TYPE_NAME) = 1 THEN ' 合计' ELSE c.cost_type_code||c.COST_TYPE_NAME END COST_TYPE_NAME,";
            sql += " CASE WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code||c.COST_TYPE_NAME) = 0 THEN c.cost_type_code||c.COST_TYPE_NAME ||'' WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code||c.COST_TYPE_NAME) = 1 THEN ' 合计' ELSE '　　' ||b.ITEM_NAME END ITEM_NAME, ";
            sql += " CASE WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code || c.COST_TYPE_NAME) = 1 THEN 'A'||MAX(A.ITEM_CODE) WHEN GROUPING (b.ITEM_NAME) = 1 AND GROUPING (c.cost_type_code || c.COST_TYPE_NAME) = 0 THEN 'B'||MAX(A.ITEM_CODE) ELSE MAX(A.ITEM_CODE) END ITEM_CODE,";
            sql += " sum(COSTS) COSTS,";
            if (hospro == "0")
            {
                sql += " sum(COSTS_ARMYFREE) COSTS_ARMYFREE, ";
            }
            if (hospro == "0")
            {
                sql += " sum(COSTS+COSTS_ARMYFREE) TOTALCOST ";
                //sql += " round(sum(COSTS+COSTS_ARMYFREE)/(select sum(COSTS+COSTS_ARMYFREE) from cbhs.CBHS_DEPT_COST_DETAIL a " + condition + ")*100,2) as RATE ";
            }
            else
            {
                sql += " sum(COSTS) TOTALCOST ";
                //sql += " round(sum(COSTS)/(select sum(COSTS) from cbhs.CBHS_DEPT_COST_DETAIL a " + condition + ")*100,2) as RATE ";
            }
            sql += " from ";
            //sql += " (select * from cbhs.CBHS_DEPT_COST_DETAIL a " + condition + ") a, ";
            sql += " cbhs.CBHS_COST_ITEM_DICT b, ";
            sql += " cbhs.CBHS_COST_TYPE_DICT c, ";
            sql += " " + tb + " a ";
            sql += " where a.ITEM_CODE=b.ITEM_CODE ";
            sql += "   and SUBSTR (a.ITEM_CODE, 0, 1) = c.COST_TYPE_CODE and " + condition;
            sql += " group by ROLLUP (c.cost_type_code||c.COST_TYPE_NAME, b.ITEM_NAME) ";
            sql += " order by c.cost_type_code||c.COST_TYPE_NAME, b.ITEM_NAME DESC ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 分解成本详细
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="gettype"></param>
        /// <returns></returns>
        public DataTable DeptHospitalCostDetail(string starttime, string endtime, string gettype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.dept_code,b.dept_name, c.item_name, a.costs, a.costs_armyfree, to_char(a.ACCOUNTING_DATE,'yyyy-mm-dd') ACCOUNTING_DATE,
        d.gettype get_type, decode (a.cost_flag,'0','住院','1','门诊') cost_flag, a.memo
  FROM {0}.cbhs_dept_cost_detail a,
       {1}.sys_dept_dict b,
       {0}.cbhs_cost_item_dict c,
       {0}.CBHS_COST_ITEM_GETTYPE d
 WHERE  a.ACCOUNTING_DATE>=to_date('{2}','yyyy-mm-dd') and a.ACCOUNTING_DATE<add_months(to_date('{3}','yyyy-mm-dd'),1)  AND a.dept_code = b.dept_code
   AND a.item_code = c.item_code and a.get_type=d.id", DataUser.CBHS, DataUser.COMM, starttime, endtime);

            if (gettype != "")
            {
                str.AppendFormat(" and d.id={0}", gettype);
            }

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 成本明细
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable DeptCostAccountCostDetail(string itemcode, string starttime, string endtime, string balance, string dept_code, string gettype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.dept_code,b.dept_name, c.item_name, sum(a.costs) costs, sum(a.costs_armyfree) costs_armyfree,sum(a.costs+a.costs_armyfree) totalcost, to_char(a.ACCOUNTING_DATE,'yyyy-mm-dd') ACCOUNTING_DATE
                              FROM {0}.cbhs_dept_cost_detail_commit a,
                                   {1}.sys_dept_dict b,
                                   {0}.cbhs_cost_item_dict c,
                                   {0}.CBHS_COST_ITEM_GETTYPE d
                             WHERE a.item_code = '{2}' and (a.BALANCE_TAG='{3}' or a.BALANCE_TAG is null) and a.ACCOUNTING_DATE>=to_date('{4}','yyyy-mm-dd') and a.ACCOUNTING_DATE<add_months(to_date('{5}','yyyy-mm-dd'),1)  AND a.dept_code = b.dept_code
                               AND a.item_code = c.item_code and a.get_type=d.id", DataUser.CBHS, DataUser.COMM, itemcode, balance, starttime, endtime);
            if (dept_code != "")
            {
                str.AppendFormat(" and b.ACCOUNT_DEPT_CODE in ({0})", dept_code);
            }
            if (gettype != "")
            {
                str.AppendFormat(" and d.id={0}", gettype);
            }
            str.Append(" group by a.dept_code,b.dept_name, c.item_name,a.ACCOUNTING_DATE,d.gettype order by b.dept_name,a.ACCOUNTING_DATE");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="gettype"></param>
        /// <returns></returns>
        public DataTable DeptCost_CostDetail(string itemcode, string starttime, string endtime, string balance, string dept_code, string gettype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.dept_code,b.dept_name, c.item_name, sum(a.costs) costs, sum(a.costs_armyfree) costs_armyfree, to_char(a.ACCOUNTING_DATE,'yyyy-mm-dd') ACCOUNTING_DATE,
                                    d.gettype get_type, decode (a.cost_flag,'0','住院','1','门诊') cost_flag,'' memo
                              FROM {0}.CBHS_DEPT_COST_DETAIL a,
                                   {1}.sys_dept_dict b,
                                   {0}.cbhs_cost_item_dict c,
                                   {0}.CBHS_COST_ITEM_GETTYPE d
                             WHERE a.item_code = '{2}' and (a.BALANCE_TAG='{3}' or a.BALANCE_TAG is null) and a.ACCOUNTING_DATE>=to_date('{4}','yyyy-mm-dd') and a.ACCOUNTING_DATE<add_months(to_date('{5}','yyyy-mm-dd'),1)  AND a.dept_code = b.dept_code
                               AND a.item_code = c.item_code and a.get_type=d.id", DataUser.CBHS, DataUser.COMM, itemcode, balance, starttime, endtime);
            if (dept_code != "")
            {
                str.AppendFormat(" and b.ACCOUNT_DEPT_CODE in ({0})", dept_code);
            }
            if (gettype != "")
            {
                str.AppendFormat(" and d.id={0}", gettype);
            }
            str.Append(" group by a.dept_code,b.dept_name, c.item_name,a.ACCOUNTING_DATE,d.gettype,a.cost_flag order by b.dept_name,a.ACCOUNTING_DATE");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 成本详细
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable CostDetail(string itemcode, string starttime, string endtime, string dept_code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select b.dept_name,a.* from {0}.CBHS_COST_DETAIL a,{1}.sys_dept_dict b where a.DEPT_CODE=b.dept_code
and a.st_date>=to_date({2},'yyyy-mm-dd') and a.st_date<add_months(to_date({3},'yyyy-mm-dd'),1) and a.item_code='{4}' and a.dept_code='{5}'", DataUser.CBHS, DataUser.COMM, starttime, endtime, itemcode, dept_code);

            str.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 核算科目医疗收入统计报表--利润报表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="balance"></param>
        /// <param name="dept_code"></param>
        /// <param name="hospro"></param>
        /// <returns></returns>
        public DataTable DeptCostAccountCost(string starttime, string endtime, string balance, string dept_code)
        {
            string condition = " where DATE_TIME>=to_date('" + starttime + "','yyyymmdd') and  DATE_TIME<add_months(to_date('" + endtime + "','yyyymmdd'),1) ";
            condition += " and (BALANCE_TAG='" + balance + "')";
            if (dept_code != "")
            {
                condition += dept_code;
            }
            string sql = " select sum(GROSS_INCOME) GROSS_INCOME, ";
            sql += " sum(NET_INCOME) NET_INCOME, ";
            sql += " sum(GROSS_INCOME-NET_INCOME) ARMY_INCOME ";
            sql += " from cbhs.CBHS_DEPT_ACCOUNT_DETAIL a" + condition;

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 科室收入明细
        /// </summary>
        /// <param name="startime"></param>
        /// <param name="endtime"></param>
        /// <param name="tablename"></param>
        /// <param name="deptcode"></param>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public DataTable GetDoctorDeatil(string startime, string endtime, string tablename, string deptcode, string itemcode)
        {
            MyLists list = new MyLists();
            List listdel = new List();
            listdel.StrSql = string.Format("delete {0}.CBHS_DOCTOR_DETAIL", DataUser.CBHS);
            listdel.Parameters = new OleDbParameter[] { };
            list.Add(listdel);

            StringBuilder str = new StringBuilder();
            str.AppendFormat("insert into {0}.CBHS_DOCTOR_DETAIL(STARTIME,ENDTIME,TABLENAME,DEPTCODE,ITEMCODE) values (?,?,?,?,?)", DataUser.CBHS);

            OleDbParameter[] parameters = {
					new OleDbParameter("STARTIME", Convert.ToDateTime(startime)),
					new OleDbParameter("ENDTIME", Convert.ToDateTime(endtime)),
					new OleDbParameter("TABLENAME", tablename),
					new OleDbParameter("DEPTCODE", deptcode),
                    new OleDbParameter("ITEMCODE", itemcode)
					};
            List listadd = new List();
            listadd.StrSql = str.ToString();
            listadd.Parameters = parameters;
            list.Add(listadd);
            OracleOledbBase.ExecuteTranslist(list);
            string selstr = string.Format("select * from {0}.{1}", DataUser.HISFACT, tablename);
            return OracleOledbBase.ExecuteDataSet(selstr).Tables[0];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptcode"></param>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public DataTable GetClinicDetail(string startime, string endtime, string deptcode, string itemcode)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"SELECT   a.visit_dept dept_code,
           b.dept_name,
           DOCTOR,
           a.clinic_type,min(c.w_numbers) w_numbers,min(c.h_numbers) h_numbers,
            sum(case when 
                    TO_DATE (TO_CHAR (A.VISIT_DATE, 'yyyy-mm-dd'),
                             'yyyy-mm-dd') NOT IN
                       (select ST_DATE from performance.OUT_REST_BONUS_LIST) then 1 else 0 end) wnum,
             sum(case when 
                    TO_DATE (TO_CHAR (A.VISIT_DATE, 'yyyy-mm-dd'),
                             'yyyy-mm-dd') IN
                       (select ST_DATE from performance.OUT_REST_BONUS_LIST where WEEKS_ID in (1,2)) then 1 else 0 end) hnum
    FROM   hisdata.CLINIC_MASTER a, comm.sys_dept_dict b,performance.OUT_CLINIC_SET c
   WHERE       VISIT_DATE >= TO_DATE ('{0}', 'yyyymmdd')
           AND VISIT_DATE < add_months(TO_DATE ('{1}', 'yyyymmdd'),1)
           AND a.VISIT_DEPT = b.dept_code and A.CLINIC_TYPE=C.CLINIC_TYPE
           ", startime, endtime);

            if (!deptcode.Equals(""))
            {
                sql.Append(deptcode);
            }

            if (itemcode.Equals(""))
            {
                sql.Append(" and a.clinic_type IN (select CLINIC_TYPE from performance.OUT_CLINIC_SET)");
            }
            else
            {
                sql.AppendFormat(" AND a.clinic_type = '{0}'", itemcode);
            }


            sql.Append(@"
GROUP BY   a.visit_dept,
           b.dept_name,
           DOCTOR,
           a.clinic_type");

            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptcode"></param>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public DataTable GetPatientIncomeDetail(string startime, string endtime, string deptcode, string itemcode)
        {
            string table = "OUTP_BILL_ITEMS";
            string colname = "VISIT_DATE";
            if (itemcode == "1")
            {
                table = "INP_BILL_DETAIL";
                colname = "BILLING_DATE_TIME";
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"  SELECT   b.dept_code,
           b.dept_name,
           PATIENT_NAME,
           PATIENT_ID,
           TO_CHAR ({2}, 'yyyy-mm-dd') AS VISIT_DATE,
           RCPT_NO,
           SUM (COSTS) AS COSTS,
           SUM (CHARGES) AS CHARGES
    FROM   hisdata.{3} a, comm.sys_dept_dict b,hisdata.ITEM_CODE_LIST c
   WHERE       {2} >= TO_DATE ('{0}', 'yyyymmdd')
           AND {2} < add_months(TO_DATE ('{1}', 'yyyymmdd'),1)
           AND a.ORDERED_BY = b.dept_code and  a.ITEM_CODE =c.item_code
GROUP BY   b.dept_code,
           b.dept_name,
           PATIENT_NAME,
           PATIENT_ID,
           TO_CHAR ({2}, 'yyyy-mm-dd'),
           RCPT_NO
           ", startime, endtime, colname, table);



            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
            //return OracleBase.Query(sql.ToString()).Tables[0];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetNew_CostsDetail(string date)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT   a.cost_type_name \"项目\",a.costs / 10000 \"本期数据\", a.account \"占比\", b.old_costs / 10000 \"去年同期\",(a.costs - b.old_costs) / b.old_costs \"增长率\"  ");
            sql.AppendFormat(@" FROM   (  SELECT   m.cost_type_code,
                     m.cost_type_name,
                     t.accounting_date,
                     SUM (t.costs) costs,
                     SUM (t.costs)
                     / (SELECT   SUM (t.costs) sumcosts
                          FROM   cbhs.cbhs_dept_cost_detail t
                         WHERE   TO_CHAR (t.accounting_date, 'yyyy-mm') =
                                    '{0}')
                        account
              FROM   cbhs.cbhs_dept_cost_detail t, cbhs.cbhs_cost_type_dict m
             WHERE       TO_CHAR (t.accounting_date, 'yyyy-mm') = '{0}'
                     AND SUBSTR (t.item_code, 1, 1) = m.cost_type_code
                     AND (t.balance_tag IS NULL OR t.balance_tag = 0)
          GROUP BY   m.cost_type_code, m.cost_type_name, t.accounting_date) a,
         (  SELECT   m.cost_type_code,
                     m.cost_type_name,
                     t.accounting_date,
                     SUM (t.costs) old_costs
              FROM   cbhs.cbhs_dept_cost_detail t, cbhs.cbhs_cost_type_dict m
             WHERE       TO_CHAR (t.accounting_date, 'yyyy-mm') = to_char(add_months(to_date('{0}','yyyy-mm'),-12),'yyyy-mm')
                     AND SUBSTR (t.item_code, 1, 1) = m.cost_type_code
                     AND (t.balance_tag IS NULL OR t.balance_tag = 0)
          GROUP BY   m.cost_type_code, m.cost_type_name, t.accounting_date) b
 WHERE   a.cost_type_code = b.cost_type_code
UNION ALL
SELECT   '总计',
         sumcosts / 10000,
         1,
         old_costs / 10000,
         (sumcosts - old_costs) / old_costs
  FROM   (SELECT   (SELECT   SUM (costs)
                      FROM   cbhs.cbhs_dept_cost_detail t
                     WHERE   TO_CHAR (t.accounting_date, 'yyyy-mm') =
                                '{0}'
                             AND (t.balance_tag IS NULL OR t.balance_tag = 0))
                      sumcosts,
                   (SELECT   SUM (costs)
                      FROM   cbhs.cbhs_dept_cost_detail t
                     WHERE   TO_CHAR (t.accounting_date, 'yyyy-mm') =
                                to_char(add_months(to_date('{0}','yyyy-mm'),-12),'yyyy-mm')
                             AND (t.balance_tag IS NULL OR t.balance_tag = 0))
                      old_costs
            FROM   DUAL)", date);
            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetNew_IncomeDetail(string date)
        {
            StringBuilder sql = new StringBuilder();

            //            sql.AppendFormat(@"
            //SELECT   n.item_name AS item_name,
            //         n.z_incomes / 10000 AS z_b_income,
            //         m.z_incomes / 10000 AS z_t_income,
            //         (n.z_incomes - m.z_incomes) / m.z_incomes z_zzl,
            //         n.d_incomes / 10000 d_b_income,
            //         m.d_incomes / 10000 d_t_income,
            //         (n.d_incomes - m.d_incomes) / m.d_incomes d_zzl,
            //         n.j_incomes / 10000 AS j_b_income,
            //         m.j_incomes / 10000 AS j_t_income,
            //         (n.j_incomes - m.j_incomes) / m.j_incomes j_zzl
            //  FROM   (  SELECT   a.CLASS_CODE,
            //                     a.CLASS_NAME item_name,
            //                     SUM (t.incomes) z_incomes,
            //                     SUM (t.incomes_charges) d_incomes,
            //                     SUM (t.incomes - t.incomes_charges) j_incomes
            //              FROM   cbhs.CBHS_INCOMS_INFO_ACCOUNT t, HISFACT.reck_item_class_dict a
            //             WHERE       TO_CHAR (t.date_time, 'yyyy-mm') = '{0}'
            //                     AND a.CLASS_CODE = SUBSTR (t.reck_item, 1, 1)
            //                     --and length(a.CLASS_CODE) = 1
            //
            //                     AND t.balance_tag = 0
            //                     AND a.CLASS_CODE <> 'I'
            //                     AND A.CLASS_CODE <> 'G'
            //                     AND A.CLASS_CODE <> 'K'
            //          GROUP BY   a.CLASS_CODE, a.CLASS_NAME
            //          UNION ALL
            //            SELECT   'K',
            //                     '其他',
            //                     SUM (incomes),
            //                     SUM (d_incomes),
            //                     SUM (j_incomes)
            //              FROM   (  SELECT   SUM (t.incomes) incomes,
            //                                 SUM (t.incomes_charges) d_incomes,
            //                                 SUM (t.incomes - t.incomes_charges) j_incomes
            //                          FROM   cbhs.CBHS_INCOMS_INFO_ACCOUNT t,
            //                                 HISFACT.reck_item_class_dict a
            //                         WHERE       TO_CHAR (t.date_time, 'yyyy-mm') = '{0}'
            //                                 AND a.CLASS_CODE = SUBSTR (t.reck_item, 1, 1)
            //                                 --and length(a.CLASS_CODE) = 1
            //                                 AND t.balance_tag = 0
            //                                 AND (   a.CLASS_CODE = 'K'
            //                                      OR a.CLASS_CODE = 'I'
            //                                      OR a.CLASS_CODE = 'G')
            //                      GROUP BY   a.CLASS_CODE, a.CLASS_NAME)
            //          GROUP BY   'K', '其他') n,
            //         (  SELECT   a.CLASS_CODE,
            //                     a.CLASS_NAME item_name,
            //                     SUM (t.incomes) z_incomes,
            //                     SUM (t.incomes_charges) d_incomes,
            //                     SUM (t.incomes - t.incomes_charges) j_incomes
            //              FROM   cbhs.CBHS_INCOMS_INFO_ACCOUNT t, HISFACT.reck_item_class_dict a
            //             WHERE       TO_CHAR (t.date_time, 'yyyy-mm') = to_char(add_months(to_date('{0}','yyyy-mm'),-12),'yyyy-mm')
            //                     AND a.CLASS_CODE = SUBSTR (t.reck_item, 1, 1)
            //                     --and length(a.CLASS_CODE) = 1
            //                     AND t.balance_tag = 0
            //                     AND a.CLASS_CODE <> 'I'
            //                     AND A.CLASS_CODE <> 'G'
            //                     AND A.CLASS_CODE <> 'K'
            //          GROUP BY   a.CLASS_CODE, a.CLASS_NAME
            //          UNION ALL
            //            SELECT   'K',
            //                     '其他',
            //                     SUM (incomes),
            //                     SUM (d_incomes),
            //                     SUM (j_incomes)
            //              FROM   (  SELECT   SUM (t.incomes) incomes,
            //                                 SUM (t.incomes_charges) d_incomes,
            //                                 SUM (t.incomes - t.incomes_charges) j_incomes
            //                          FROM   cbhs.CBHS_INCOMS_INFO_ACCOUNT t,
            //                                 HISFACT.reck_item_class_dict a
            //                         WHERE       TO_CHAR (t.date_time, 'yyyy-mm') = to_char(add_months(to_date('{0}','yyyy-mm'),-12),'yyyy-mm')
            //                                 AND a.CLASS_CODE = SUBSTR (t.reck_item, 1, 1)
            //                                 --and length(a.CLASS_CODE) = 1
            //                                 AND t.balance_tag = 0
            //                                 AND (   a.CLASS_CODE = 'K'
            //                                      OR a.CLASS_CODE = 'I'
            //                                      OR a.CLASS_CODE = 'G')
            //                      GROUP BY   a.CLASS_CODE, a.CLASS_NAME)
            //          GROUP BY   'K', '其他') m
            // WHERE   n.class_code = m.class_code
            //UNION ALL
            //SELECT   '合计',
            //         o.incomes / 10000,
            //         p.incomes / 10000,
            //         (o.incomes - p.incomes) / p.incomes,
            //         o.d_incomes / 10000,
            //         p.d_incomes / 10000,
            //         (o.d_incomes - p.d_incomes) / p.d_incomes,
            //         o.j_incomes / 10000,
            //         p.j_incomes / 10000,
            //         (o.j_incomes - p.j_incomes) / p.j_incomes
            //  FROM   (SELECT   SUM (t.incomes) incomes,
            //                   SUM (t.incomes_charges) d_incomes,
            //                   SUM (t.incomes - t.incomes_charges) j_incomes
            //            FROM   cbhs.CBHS_INCOMS_INFO_ACCOUNT t, HISFACT.reck_item_class_dict a
            //           WHERE       TO_CHAR (t.date_time, 'yyyy-mm') = '{0}'
            //                   AND a.CLASS_CODE = SUBSTR (t.reck_item, 1, 1)
            //                   AND t.balance_tag = 0) o,
            //         (SELECT   SUM (t.incomes) incomes,
            //                   SUM (t.incomes_charges) d_incomes,
            //                   SUM (t.incomes - t.incomes_charges) j_incomes
            //            FROM   cbhs.CBHS_INCOMS_INFO t, HISFACT.reck_item_class_dict a
            //           WHERE       TO_CHAR (t.date_time, 'yyyy-mm') = to_char(add_months(to_date('{0}','yyyy-mm'),-12),'yyyy-mm')
            //                   AND a.CLASS_CODE = SUBSTR (t.reck_item, 1, 1)
            //                   AND t.balance_tag = 0) p
            //", date);
            OracleOledbBase.ExecuteNonQuery(string.Format("update  cbhs.CBHS_DATE_TIME set DATE_TIME='{0}'", date));
            return OracleOledbBase.ExecuteDataSet("select * from cbhs.V_INCOMEDETAIL").Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dept_type"></param>
        /// <returns></returns>
        public DataTable GetNew_IncomeCostsDetail(string date, string enddate, string dept_type)
        {
            //1开单0执行
            string optag = "1";
            if (dept_type.Equals("50001") || dept_type.Equals("60001"))
            {
                optag = "0";
            }

            if (!dept_type.Equals(""))
            {
                dept_type = "AND   v.dept_type = '" + dept_type + "'";
            }
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT  A.DEPT_CODE,
                                        A.DEPT_NAME,
                                        ROUND(SUM(NVL(B.BQ_HJ,0)+NVL(C.BQ_HJ_YP,0)),2)/10000 Z_INCOME,
                                        ROUND(SUM(NVL(B.BQ_SS,0)+NVL(C.BQ_SS_YP,0)),2)/10000 SJ_INCOME,
                                        ROUND(SUM(NVL(B.BQ_JD,0)+NVL(C.BQ_JD_YP,0)),2)/10000 JJ_INCOME,
                                        ROUND(SUM(NVL(D.BQ_HJ_CB,0)+NVL(C.BQ_HJ_YP_CB,0)),2)/10000 Z_COSTS,
                                        ROUND(SUM(NVL(B.BQ_HJ,0)+NVL(C.BQ_HJ_YP,0))-SUM(NVL(D.BQ_HJ_CB,0)+NVL(C.BQ_HJ_YP_CB,0)),2)/10000 Z_LIRUN,    --???(????-???)
                                        ROUND(SUM(NVL(B.BQ_SS,0)+NVL(C.BQ_SS_YP,0))-SUM(NVL(D.BQ_HJ_CB,0)+NVL(C.BQ_HJ_YP_CB,0)),2)/10000 J_LIRUN,
                                        
                                        ROUND(SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0)),2)/10000 TQ_INCOME,
                                        ROUND(SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0)),2)/10000 TQ_SJ_INCOME,
                                        ROUND(SUM(NVL(B.TQ_JD,0)+NVL(C.TQ_JD_YP,0)),2)/10000 TQ_JJ_INCOME,
                                        ROUND(SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)),2)/10000 TQ_Z_COSTS,
                                        ROUND(SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)),2)/10000 TQ_Z_LIRUN, --???(????-???)
                                        ROUND(SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)),2)/10000 TQ_J_LIRUN,
                                        
                                        ROUND(SUM(NVL(B.BQ_HJ,0)+NVL(C.BQ_HJ_YP,0))-SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0)),2)/10000 TBZL_Z_INCOME,
                                        ROUND(SUM(NVL(B.BQ_SS,0)+NVL(C.BQ_SS_YP,0))-SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0)),2)/10000 TBZL_SJ_INCOME,
                                        ROUND(SUM(NVL(B.BQ_JD,0)+NVL(C.BQ_JD_YP,0))-SUM(NVL(B.TQ_JD,0)+NVL(C.TQ_JD_YP,0)),2)/10000 TBZL_JJ_INCOME,
                                        ROUND(SUM(NVL(D.BQ_HJ_CB,0)+NVL(C.BQ_HJ_YP_CB,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)),2)/10000 TBZL_Z_COSTS,
                                        ROUND((SUM(NVL(B.BQ_HJ,0)+NVL(C.BQ_HJ_YP,0))-SUM(NVL(D.BQ_HJ_CB,0)+NVL(C.BQ_HJ_YP_CB,0)))-(SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0))),2)/10000 TBZL_Z_LIRUN, --???(????-???)
                                        ROUND((SUM(NVL(B.BQ_SS,0)+NVL(C.BQ_SS_YP,0))-SUM(NVL(D.BQ_HJ_CB,0)+NVL(C.BQ_HJ_YP_CB,0)))-(SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0))),2)/10000 TBZL_J_LIRUN,
                                        
                                        DECODE(ROUND(SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0)),2),0,0,ROUND((SUM(NVL(B.BQ_HJ,0)+NVL(C.BQ_HJ_YP,0))-SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0)))/SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0))*100,2)) TBZF_Z_INCOME,
                                        DECODE(ROUND(SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0)),2),0,0,ROUND((SUM(NVL(B.BQ_SS,0)+NVL(C.BQ_SS_YP,0))-SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0)))/SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0))*100,2)) TBZF_SS_INCOME,
                                        DECODE(ROUND(SUM(NVL(B.TQ_JD,0)+NVL(C.TQ_JD_YP,0)),2),0,0,ROUND((SUM(NVL(B.BQ_JD,0)+NVL(C.BQ_JD_YP,0))-SUM(NVL(B.TQ_JD,0)+NVL(C.TQ_JD_YP,0)))/SUM(NVL(B.TQ_JD,0)+NVL(C.TQ_JD_YP,0))*100,2)) TBZF_JJ_INCOME,
                                        DECODE(ROUND(SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)),2),0,0,ROUND((SUM(NVL(D.BQ_HJ_CB,0)+NVL(C.BQ_HJ_YP_CB,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)))/SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0))*100,2)) TBZF_Z_COSTS,
                                        DECODE(ROUND(SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)),2),0,0,ROUND(((SUM(NVL(B.BQ_HJ,0)+NVL(C.BQ_HJ_YP,0))-SUM(NVL(D.BQ_HJ_CB,0)+NVL(C.BQ_HJ_YP_CB,0)))-(SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0))))/(SUM(NVL(B.TQ_HJ,0)+NVL(C.TQ_HJ_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)))*100,2)) TBZF_Z_LIRUN,
                                        DECODE(ROUND(SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)),2),0,0,ROUND(((SUM(NVL(B.BQ_SS,0)+NVL(C.BQ_SS_YP,0))-SUM(NVL(D.BQ_HJ_CB,0)+NVL(C.BQ_SS_YP_CB,0)))-(SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0))))/(SUM(NVL(B.TQ_SS,0)+NVL(C.TQ_SS_YP,0))-SUM(NVL(D.TQ_HJ_CB,0)+NVL(C.TQ_HJ_YP_CB,0)))*100,2)) TBZF_J_LIRUN
                                        
                                FROM
                                (
                                SELECT   w.* 
                                  FROM   PERFORMANCE.SET_ACCOUNTDEPTTYPE_REPORT v,    --科室信息
                                         comm.sys_dept_dict w
                                 WHERE   v.dept_code = w.dept_code
                                   {1}
                                ) A,
                                (
                                SELECT   B.ACCOUNT_DEPT_CODE,
                                         B.ACCOUNT_DEPT_NAME,
                                        SUM(CASE WHEN A.date_time >= TO_DATE ('{0}', 'yyyymmdd') AND A.date_time < TO_DATE ('{2}', 'yyyymmdd') THEN INCOMES ELSE 0 END) BQ_HJ,
                                        SUM(CASE WHEN A.date_time >= TO_DATE ('{0}', 'yyyymmdd') AND A.date_time < TO_DATE ('{2}', 'yyyymmdd') THEN INCOMES_CHARGES ELSE 0 END) BQ_SS,
                                        SUM(CASE WHEN A.date_time >= TO_DATE ('{0}', 'yyyymmdd') AND A.date_time < TO_DATE ('{2}', 'yyyymmdd') THEN INCOMES-INCOMES_CHARGES ELSE 0 END) BQ_JD,
                                        
                                        SUM(CASE WHEN A.date_time >= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.date_time < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN INCOMES ELSE 0 END) TQ_HJ,
                                        SUM(CASE WHEN A.date_time >= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.date_time < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN INCOMES_CHARGES ELSE 0 END) TQ_SS,
                                        SUM(CASE WHEN A.date_time >= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.date_time < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN INCOMES-INCOMES_CHARGES ELSE 0 END) TQ_JD
                                  FROM   CBHS.CBHS_INCOMS_INFO_ACCOUNT A,    --核算后收入
                                         COMM.SYS_DEPT_DICT B
                                 WHERE   A.DEPT_CODE=B.DEPT_CODE
                                   AND   ((A.date_time >= TO_DATE ('{0}', 'yyyymmdd') AND A.date_time < TO_DATE ('{2}', 'yyyymmdd'))
                                    OR    (A.date_time >= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.date_time < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12)))
                                   AND   A.RECK_ITEM NOT IN ('A01','A02','A03','A04','A05','A06','D14','D18','E10','E12','E15','E19','d20')
                                   AND   A.BALANCE_TAG='0'
                                GROUP BY B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME
                                ) B,
                                (
                                SELECT  B.ACCOUNT_DEPT_CODE,
                                        B.ACCOUNT_DEPT_NAME,
                                        SUM(CASE WHEN A.ST_DATE>= TO_DATE ('{0}', 'yyyymmdd') AND A.ST_DATE < TO_DATE ('{2}', 'yyyymmdd') THEN COSTS ELSE 0 END) BQ_HJ_YP,
                                        SUM(CASE WHEN A.ST_DATE>= TO_DATE ('{0}', 'yyyymmdd') AND A.ST_DATE < TO_DATE ('{2}', 'yyyymmdd') THEN CHARGES ELSE 0 END) BQ_SS_YP,
                                        SUM(CASE WHEN A.ST_DATE>= TO_DATE ('{0}', 'yyyymmdd') AND A.ST_DATE < TO_DATE ('{2}', 'yyyymmdd') THEN INCOME ELSE 0 END) BQ_JD_YP,
                                        
                                        SUM(CASE WHEN A.ST_DATE>= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.ST_DATE < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN COSTS ELSE 0 END) TQ_HJ_YP,
                                        SUM(CASE WHEN A.ST_DATE>= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.ST_DATE < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN CHARGES ELSE 0 END) TQ_SS_YP,
                                        SUM(CASE WHEN A.ST_DATE>= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.ST_DATE < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN INCOME ELSE 0 END) TQ_JD_YP,
                                        
                                        SUM(CASE WHEN A.ST_DATE>= TO_DATE ('{0}', 'yyyymmdd') AND A.ST_DATE < TO_DATE ('{2}', 'yyyymmdd') THEN COSTS ELSE 0 END)/1.15 BQ_HJ_YP_CB,
                                        SUM(CASE WHEN A.ST_DATE>= TO_DATE ('{0}', 'yyyymmdd') AND A.ST_DATE < TO_DATE ('{2}', 'yyyymmdd') THEN CHARGES ELSE 0 END)/1.15 BQ_SS_YP_CB,
                                        SUM(CASE WHEN A.ST_DATE>= TO_DATE ('{0}', 'yyyymmdd') AND A.ST_DATE < TO_DATE ('{2}', 'yyyymmdd') THEN INCOME ELSE 0 END)/1.15 BQ_JD_YP_CB,
                                        
                                        SUM(CASE WHEN A.ST_DATE>= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.ST_DATE < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN COSTS ELSE 0 END)/1.15 TQ_HJ_YP_CB,
                                        SUM(CASE WHEN A.ST_DATE>= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.ST_DATE < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN CHARGES ELSE 0 END)/1.15 TQ_SS_YP_CB,
                                        SUM(CASE WHEN A.ST_DATE>= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.ST_DATE < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN INCOME ELSE 0 END)/1.15 TQ_JD_YP_CB
                                        
                                  FROM  HISFACT.DEPT_INCOME_PER_MON A,    --核算前收入
                                        COMM.SYS_DEPT_DICT B
                                 WHERE  A.DEPT_CODE=B.DEPT_CODE
                                   AND  ((A.ST_DATE>= TO_DATE ('{0}', 'yyyymmdd') AND A.ST_DATE < TO_DATE ('{2}', 'yyyymmdd'))
                                    OR   (A.ST_DATE>= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) AND A.ST_DATE < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12)))
                                   AND  A.RECK_CLASS IN ('A01','A02','A03','A04','A05','A06')
                                   AND  A.OP_TAG='{3}'
                                GROUP BY B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME
                                ) C,
                                (
                                SELECT  B.ACCOUNT_DEPT_CODE,
                                        B.ACCOUNT_DEPT_NAME,
                                        SUM(CASE WHEN a.accounting_date >= TO_DATE('{0}','yyyymmdd') AND a.accounting_date < TO_DATE('{2}','yyyymmdd') THEN COSTS+COSTS_ARMYFREE ELSE 0 END) BQ_HJ_CB,
                                        SUM(CASE WHEN a.accounting_date >= TO_DATE('{0}','yyyymmdd') AND a.accounting_date < TO_DATE('{2}','yyyymmdd') THEN COSTS ELSE 0 END) BQ_SS_CB,
                                        SUM(CASE WHEN a.accounting_date >= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) and a.accounting_date < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN COSTS+COSTS_ARMYFREE ELSE 0 END) TQ_HJ_CB,
                                        SUM(CASE WHEN a.accounting_date >= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) and a.accounting_date < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) THEN COSTS ELSE 0 END) TQ_SS_CB
                                FROM    cbhs.cbhs_dept_cost_detail A,
                                        comm.sys_dept_dict B
                                WHERE   A.dept_code=B.dept_code
                                  AND   (a.balance_tag = 0 OR a.balance_tag IS NULL)
                                  AND   ((a.accounting_date >= TO_DATE('{0}','yyyymmdd') AND a.accounting_date < TO_DATE('{2}','yyyymmdd'))
                                   OR    (a.accounting_date >= ADD_MONTHS(TO_DATE ('{0}', 'yyyymmdd'),-12) and a.accounting_date < ADD_MONTHS(TO_DATE ('{2}', 'yyyymmdd'),-12) ))
                                GROUP BY B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME
                                ) D
                                WHERE A.DEPT_CODE=B.ACCOUNT_DEPT_CODE(+)
                                  AND A.DEPT_CODE=C.ACCOUNT_DEPT_CODE(+)
                                  AND A.DEPT_CODE=D.ACCOUNT_DEPT_CODE(+)
                                GROUP BY A.DEPT_CODE,A.DEPT_NAME", date, dept_type, enddate, optag);

            return OracleBase.QuerySet(sql.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAccountType()
        {
            string sql = @"select type_code,type_name from performance.SET_ACCOUNTTYPE order by sort";

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetNew_PerCapitaIncome(string date)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT  A.account_dept_name \"科室\",");
            sql.Append("ROUND(SUM(B.BQ_HJ+C.BQ_HJ_YP)-SUM(D.BQ_HJ_CB+C.BQ_HJ_YP_CB),2)/10000 \"总利润\",");
            sql.Append("ROUND(SUM(B.BQ_SS+C.BQ_SS_YP)-SUM(D.BQ_HJ_CB+C.BQ_SS_YP_CB),2)/10000 \"净利润\",");
            sql.Append("max (A.ygs) \"员工数\",");
            sql.Append("ROUND(DECODE(max(A.ygs),0,0,ROUND(SUM(B.BQ_HJ+C.BQ_HJ_YP)-SUM(D.BQ_HJ_CB+C.BQ_HJ_YP_CB),2)/10000/max(A.ygs)),2) \"人均收益\",");
            sql.Append("ROUND(DECODE(max(A.ygs),0,0,ROUND(SUM(B.BQ_SS+C.BQ_SS_YP)-SUM(D.BQ_HJ_CB+C.BQ_SS_YP_CB),2)/10000/max(A.ygs)),2) \"人均净收益\" ");
            sql.AppendFormat(@" FROM
                (
                SELECT   s.account_dept_code,
                         s.account_dept_name,
                         COUNT (t.name) ygs
                  FROM   rlzy.new_staff_info t, 
                         comm.sys_dept_dict s
                 WHERE   t.isonguard = '是' AND t.dept_code = s.dept_code
                GROUP BY   s.account_dept_code, s.account_dept_name
                ) A,
                (
                SELECT   B.ACCOUNT_DEPT_CODE,
                         B.ACCOUNT_DEPT_NAME,
                        SUM(INCOMES) BQ_HJ,
                        SUM(INCOMES_CHARGES) BQ_SS,
                        SUM(INCOMES-INCOMES_CHARGES) BQ_JD
                                                        
                  FROM   CBHS.CBHS_INCOMS_INFO_ACCOUNT A,
                         COMM.SYS_DEPT_DICT B
                 WHERE   A.DEPT_CODE=B.DEPT_CODE
                   AND   A.DATE_TIME = TO_DATE ('{0}', 'yyyymmdd')
                   AND   A.RECK_ITEM NOT IN ('A01','A02','A03','A04','A05','A06')
                   AND   A.BALANCE_TAG='0'
                GROUP BY B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME
                ) B,
                (
                SELECT  B.ACCOUNT_DEPT_CODE,
                        B.ACCOUNT_DEPT_NAME,
                        SUM(COSTS) BQ_HJ_YP,
                        SUM(CHARGES) BQ_SS_YP,
                        SUM(INCOME) BQ_JD_YP,
                                                       
                        SUM(COSTS)/1.15 BQ_HJ_YP_CB,
                        SUM(CHARGES)/1.15 BQ_SS_YP_CB,
                        SUM(INCOME)/1.15 BQ_JD_YP_CB
                                                        
                  FROM  HISFACT.DEPT_INCOME_MON A,
                        COMM.SYS_DEPT_DICT B
                 WHERE  A.DEPT_CODE=B.DEPT_CODE
                   AND  A.ST_DATE=TO_DATE ('{0}', 'yyyymmdd')
                   AND  A.RECK_CLASS IN ('A01','A02','A03','A04','A05','A06')
                GROUP BY B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME
                ) C,
                (
                SELECT  B.ACCOUNT_DEPT_CODE,
                        B.ACCOUNT_DEPT_NAME,
                        SUM(COSTS+COSTS_ARMYFREE) BQ_HJ_CB,
                        SUM(COSTS) BQ_SS_CB
                FROM    cbhs.cbhs_dept_cost_detail A,
                        comm.sys_dept_dict B
                WHERE   A.dept_code=B.dept_code
                  AND   (a.balance_tag = 0 OR a.balance_tag IS NULL)
                  AND   TO_CHAR (a.accounting_date, 'yyyymmdd') = '{0}'
                GROUP BY B.ACCOUNT_DEPT_CODE,B.ACCOUNT_DEPT_NAME
                ) D
                WHERE A.account_dept_code=B.ACCOUNT_DEPT_CODE
                  AND A.account_dept_code=C.ACCOUNT_DEPT_CODE
                  AND A.account_dept_code=D.ACCOUNT_DEPT_CODE
                GROUP BY A.account_dept_code,A.ACCOUNT_DEPT_NAME", date);

            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetNew_Earnings(string date)
        {
            string sql1 = "SELECT   dept_name, ROUND (bqsj, 2) AS bqsj, ROUND (qnsj, 2) as qntq, ROUND (zsl, 2) AS zzl";
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"
WITH a
       AS (SELECT   a.class_name,
                    a.zsr,
                    a.ss,
                    a.xs,
                    b.tq_zsr,
                    b.tq_ss,
                    b.tq_xs
             FROM   (  SELECT   CASE
                                   WHEN a.CLASS_CODE <> 'A' THEN 'n'
                                   ELSE 'A'
                                END
                                   CLASS_CODE,
                                CASE
                                   WHEN a.CLASS_NAME <> '药品' THEN '医疗'
                                   ELSE '药品'
                                END
                                   CLASS_NAME,
                                SUM (t.incomes) / 10000 zsr,
                                SUM (t.incomes_charges) / 10000 ss,
                                (SUM (t.incomes) - SUM (t.incomes_charges))
                                / 10000
                                   xs
                         FROM   cbhs.CBHS_INCOMS_INFO_ACCOUNT t,
                                HISFACT.reck_item_class_dict a
                        WHERE   TO_CHAR (t.date_time, 'yyyy-mm') = '{0}' --and substr(t.reck_item,1,1) = 'A'
                                AND a.CLASS_CODE = SUBSTR (t.reck_item, 1, 1)
                                AND t.balance_tag = 0
                     GROUP BY   CASE
                                   WHEN a.CLASS_CODE <> 'A' THEN 'n'
                                   ELSE 'A'
                                END,
                                CASE
                                   WHEN a.CLASS_NAME <> '药品' THEN '医疗'
                                   ELSE '药品'
                                END) a,
                    (  SELECT   CASE
                                   WHEN a.CLASS_CODE <> 'A' THEN 'n'
                                   ELSE 'A'
                                END
                                   CLASS_CODE,
                                CASE
                                   WHEN a.CLASS_NAME <> '药品' THEN '医疗'
                                   ELSE '药品'
                                END
                                   CLASS_NAME,
                                SUM (t.incomes) / 10000 tq_zsr,
                                SUM (t.incomes_charges) / 10000 tq_ss,
                                (SUM (t.incomes) - SUM (t.incomes_charges))
                                / 10000
                                   tq_xs
                         FROM   cbhs.CBHS_INCOMS_INFO_ACCOUNT t,
                                HISFACT.reck_item_class_dict a
                        WHERE   TO_CHAR (t.date_time, 'yyyy-mm') = to_char(add_months(to_date('{0}','yyyy-mm'),-12),'yyyy-mm') --and substr(t.reck_item,1,1) = 'A'
                                AND a.CLASS_CODE = SUBSTR (t.reck_item, 1, 1)
                                AND t.balance_tag = 0
                     GROUP BY   CASE
                                   WHEN a.CLASS_CODE <> 'A' THEN 'n'
                                   ELSE 'A'
                                END,
                                CASE
                                   WHEN a.CLASS_NAME <> '药品' THEN '医疗'
                                   ELSE '药品'
                                END) b
            WHERE   a.CLASS_CODE = b.class_code),
    b
       AS (SELECT   i.zcb,
                    i.ylcb,
                    i.ypcb,
                    o.tp_zcb,
                    o.tp_ylcb,
                    o.tp_ypcb
             FROM   (SELECT   SUM (COSTS) / 10000 AS zcb,             --总成本
                              SUM(DECODE (SUBSTR (b.ITEM_CODE, 0, 1),
                                          'C', 0,
                                          COSTS))
                              / 10000
                                 AS ylcb,                      --其中:医疗成本
                              SUM(DECODE (SUBSTR (b.ITEM_CODE, 0, 1),
                                          'C', COSTS,
                                          0))
                              / 10000
                                 AS ypcb                            --药品成本
                       FROM   cbhs.cbhs_dept_cost_detail b
                      WHERE   (b.balance_tag = 0 OR b.balance_tag IS NULL)
                              AND TO_CHAR (b.accounting_date, 'yyyy-mm') =
                                    '{0}') i,
                    (SELECT   '',
                              SUM (COSTS) / 10000 AS tp_zcb,          --总成本
                              SUM(DECODE (SUBSTR (b.ITEM_CODE, 0, 1),
                                          'C', 0,
                                          COSTS))
                              / 10000
                                 AS tp_ylcb,                   --其中:医疗成本
                              SUM(DECODE (SUBSTR (b.ITEM_CODE, 0, 1),
                                          'C', COSTS,
                                          0))
                              / 10000
                                 AS tp_ypcb                         --药品成本
                       FROM   cbhs.cbhs_dept_cost_detail b
                      WHERE   (b.balance_tag = 0 OR b.balance_tag IS NULL)
                              AND TO_CHAR (b.accounting_date, 'yyyy-mm') =
                                    to_char(add_months(to_date('{0}','yyyy-mm'),-12),'yyyy-mm')) o)
{1}
  FROM   (SELECT   '总医药收入' dept_name,                            --总收入
                   SUM (a.zsr) bqsj,
                   SUM (a.tq_zsr) qnsj,
                   (SUM (a.zsr) - SUM (a.tq_zsr)) / SUM (a.tq_zsr) zsl
            FROM   a
          UNION ALL
          SELECT   a.class_name,
                   a.zsr,
                   a.tq_zsr,
                   (a.zsr - a.tq_zsr) / a.tq_zsr
            FROM   a
          UNION ALL
          SELECT   '实际医药收入',
                   SUM (a.ss),
                   SUM (a.tq_ss),
                   (SUM (a.ss) - SUM (a.tq_ss)) / SUM (a.tq_ss)
            FROM   a
          UNION ALL
          SELECT   a.class_name,
                   a.ss,
                   a.tq_ss,
                   (a.ss - a.tq_ss) / a.tq_ss
            FROM   a
          UNION ALL
          SELECT   '军人医药消耗',
                   SUM (a.xs),
                   SUM (a.tq_xs),
                   (SUM (a.xs) - SUM (a.tq_xs)) / SUM (a.tq_xs)
            FROM   a
          UNION ALL
          SELECT   a.class_name,
                   a.xs,
                   a.tq_xs,
                   (a.xs - a.tq_xs) / a.tq_xs
            FROM   a
          UNION ALL
          SELECT   '总成本',
                   SUM (b.zcb),
                   SUM (b.tp_zcb),
                   (SUM (b.zcb) - SUM (b.tp_zcb)) / SUM (b.tp_zcb)
            FROM   b
          UNION ALL
          SELECT   '医疗成本',
                   SUM (ylcb),
                   SUM (tp_ylcb),
                   (SUM (ylcb) - SUM (tp_ylcb)) / SUM (tp_ylcb)
            FROM   b
          UNION ALL
          SELECT   '药品成本',
                   SUM (ypcb),
                   SUM (tp_ypcb),
                   (SUM (ypcb) - SUM (tp_ypcb)) / SUM (tp_ypcb)
            FROM   b
          UNION ALL
          SELECT   '总利润',                                          --总利润
                   SUM (a.zsr) - SUM (b.zcb),
                   SUM (a.tq_zsr) - SUM (b.tp_zcb),
                   ( (SUM (a.zsr) - SUM (b.zcb))
                    - (SUM (a.tq_zsr) - SUM (b.tp_zcb)))
                   / (SUM (a.tq_zsr) - SUM (b.tp_zcb))
            FROM   a, b
          UNION ALL
          SELECT   '总利润率',
                   SUM (a.zsr) / SUM (b.zcb),
                   SUM (a.tq_zsr) / SUM (b.tp_zcb),
                   SUM (a.zsr) / SUM (b.zcb)
                   - SUM (a.tq_zsr) / SUM (b.tp_zcb)
            FROM   a, b
          UNION ALL
          SELECT   '医疗利润',
                   a.zsr - b.ylcb,
                   a.tq_zsr - b.tp_zcb,
                   ( (a.zsr - b.ylcb) - (a.tq_zsr - b.tp_ylcb))
                   / (a.tq_zsr - b.tp_ylcb)
            FROM   a, b
           WHERE   a.CLASS_NAME = '医疗'
          UNION ALL
          SELECT   '医疗利润率',
                   (a.zsr - b.ylcb) / a.zsr,
                   (a.tq_zsr - b.tp_ylcb) / a.tq_zsr,
                   (a.zsr - b.ylcb) / a.zsr
                   - (a.tq_zsr - b.tp_ylcb) / a.tq_zsr
            FROM   a, b
           WHERE   a.class_name = '医疗'
          UNION ALL
          SELECT   '药品利润',
                   a.zsr - b.ypcb,
                   a.tq_zsr - b.tp_zcb,
                   ( (a.zsr - b.ypcb) - (a.tq_zsr - b.tp_zcb))
                   / (a.tq_zsr - b.tp_zcb)
            FROM   a, b
           WHERE   a.CLASS_NAME = '药品'
          UNION ALL
          SELECT   '药品利润率',
                   (a.zsr - b.ypcb) / a.zsr,
                   (a.tq_zsr - b.tp_ypcb) / a.tq_zsr,
                   (a.zsr - b.ypcb) / a.zsr
                   - (a.tq_zsr - b.tp_ypcb) / a.tq_zsr
            FROM   a, b
           WHERE   a.CLASS_NAME = '药品'
          UNION ALL
          SELECT   '净利润',                                          --净利润
                   SUM (a.ss) - SUM (b.zcb),
                   SUM (a.tq_ss) - SUM (b.tp_zcb),
                   ( (SUM (a.ss) - SUM (b.zcb))
                    - (SUM (a.tq_ss) - SUM (b.tp_zcb)))
                   / (SUM (a.tq_ss) - SUM (b.tp_zcb))
            FROM   a, b
          UNION ALL
          SELECT   '净利润率',                                      --净利润率
                   (SUM (a.ss) - SUM (b.zcb)) / SUM (a.ss),
                   (SUM (a.tq_ss) - SUM (b.tp_zcb)) / SUM (a.tq_ss),
                   (SUM (a.ss) - SUM (b.zcb)) / SUM (a.ss)
                   - (SUM (a.tq_ss) - SUM (b.tp_zcb)) / SUM (a.tq_ss)
            FROM   a, b
          UNION ALL
          SELECT   '医疗利润',                                    --医疗净利润
                   a.ss - b.ylcb,
                   a.tq_ss - b.tp_ylcb,
                   ( (a.ss - b.ylcb) - (a.tq_ss - b.tp_ylcb))
                   / (a.tq_ss - b.tp_ylcb)
            FROM   a, b
           WHERE   a.CLASS_NAME = '医疗'
          UNION ALL
          SELECT   '药品利润',                                    --药品净利润
                   a.ss - b.ypcb,
                   a.tq_ss - b.tp_ypcb,
                   ( (a.ss - b.ypcb) - (a.tq_ss - b.tp_ypcb))
                   / (a.tq_ss - b.tp_ypcb)
            FROM   a, b
           WHERE   a.CLASS_NAME = '药品')", date, sql1);
            OracleOledbBase.ExecuteNonQuery(string.Format("update  cbhs.CBHS_DATE_TIME set DATE_TIME='{0}'", date));
            return OracleBase.QuerySet("select * from cbhs.CBHS_EARNINGS").Tables[0];

        }


        public DataTable Fenxibaobiao(string stdate, string enddate)
        {
          
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select nvl(a.dept_code, b.dept_code) 科室代码,
       nvl(a.dept_name, b.dept_name) 科室名称,
       nvl(a.dangqushouru, 0) 当期收入,
       nvl(a.duibishouru, 0) 对期收入,
       nvl(a.dangqushouru, 0) - nvl(a.duibishouru, 0) 收入差值,
       (nvl(a.dangqushouru, 0) - nvl(a.duibishouru, 0)) / case
         when nvl(a.duibishouru, 0) = 0 then
          null else a.duibishouru
       end 收入差异率,
       nvl(b.dangqichengben, 0) 当期成本,
       nvl(b.duibichengben, 0) 对期成本,
       nvl(b.dangqichengben, 0) - nvl(b.duibichengben, 0) 成本差值,
       (nvl(b.dangqichengben, 0) - nvl(b.duibichengben, 0)) / case
         when nvl(b.duibichengben, 0) = 0 then
          null else b.duibichengben
       end 成本差异率,
       nvl(a.dangqushouru, 0) - nvl(b.dangqichengben, 0) 当期结余,
       nvl(a.duibishouru, 0) - nvl(b.duibichengben, 0) 对期结余,
       nvl(a.dangqushouru, 0) - nvl(b.dangqichengben, 0) -
       (nvl(a.duibishouru, 0) - nvl(b.duibichengben, 0)) 结余差值,
       (nvl(a.dangqushouru, 0) - nvl(b.dangqichengben, 0) -
       (nvl(a.duibishouru, 0) - nvl(b.duibichengben, 0))) / case
         when (nvl(a.duibishouru, 0) - nvl(b.duibichengben, 0)) = 0 then
          null else nvl(a.duibishouru, 0) - nvl(b.duibichengben, 0)
       end 结余差异率
  from ( -- 当期收入
        select c.dept_code, c.dept_name, dangqushouru, duibishouru
          from (select dept_code, dept_name, sum(shouru) dangqushouru
                   from (select b.account_dept_code dept_code,
                                b.account_dept_name dept_name,
                                sum(a.costs) shouru
                           from hisfact.inp_class2_income a,
                                comm.sys_dept_dict        b
                          where st_date >= DATE
                          '{0}'
                            AND st_date < ADD_MONTHS(DATE '{0}', 1)
                            and a.ordered_by_dept = b.dept_code
                          group by b.account_dept_code, b.account_dept_name
                         union all
                         select b.account_dept_code dept_code,
                                b.account_dept_name,
                                sum(a.costs) shouru
                           from hisfact.outp_class2_income a,
                                comm.sys_dept_dict         b
                          where st_date >= DATE
                          '{0}'
                            AND st_date < ADD_MONTHS(DATE '{0}', 1)
                            and a.ordered_by_dept = b.dept_code
                          group by b.account_dept_code, b.account_dept_name)
                  group by dept_code, dept_name) c,
                
                (select dept_code, dept_name, sum(shouru) duibishouru
                   from (select b.account_dept_code dept_code,
                                b.account_dept_name dept_name,
                                sum(a.costs) shouru
                           from hisfact.inp_class2_income a,
                                comm.sys_dept_dict        b
                          where st_date >= DATE
                          '{1}'
                            AND st_date < ADD_MONTHS(DATE '{1}', 1)
                            and a.ordered_by_dept = b.dept_code
                          group by b.account_dept_code, b.account_dept_name
                         union all
                         select b.account_dept_code dept_code,
                                b.account_dept_name,
                                sum(a.costs) duibishouru
                           from hisfact.outp_class2_income a,
                                comm.sys_dept_dict         b
                          where st_date >= DATE
                          '{1}'
                            AND st_date < ADD_MONTHS(DATE '{1}', 1)
                            and a.ordered_by_dept = b.dept_code
                          group by b.account_dept_code, b.account_dept_name)
                  group by dept_code, dept_name) d
         where c.dept_code = d.dept_code) a,
       ( --当期成本
        select c.dept_code, c.dept_name, dangqichengben, duibichengben
          from (select b.account_dept_code dept_code,
                        b.account_dept_name dept_name,
                        sum(a.costs) dangqichengben
                   from CBHS.CBHS_DEPT_COST_DETAIL a, comm.sys_dept_dict b
                  where accounting_date >= DATE
                  '{0}'
                    AND a.accounting_date < ADD_MONTHS(DATE '{0}', 1)
                    and a.dept_code = b.dept_code
                  group by b.account_dept_code, b.account_dept_name) c,
                (select b.account_dept_code dept_code,
                        b.account_dept_name dept_name,
                        sum(a.costs) duibichengben --对比成本
                   from CBHS.CBHS_DEPT_COST_DETAIL a, comm.sys_dept_dict b
                  where accounting_date >= DATE
                  '{1}'
                    AND a.accounting_date < ADD_MONTHS(DATE '{1}', 1)
                    and a.dept_code = b.dept_code
                  group by b.account_dept_code, b.account_dept_name) d
         where c.dept_code(+) = d.dept_code) b
 where a.dept_code(+) = b.dept_code
 order by nvl(a.dept_code, b.dept_code)", stdate, enddate);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];

        }
    }
}
