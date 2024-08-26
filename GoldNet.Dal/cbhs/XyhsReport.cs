using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Data.OleDb;
using GoldNet.Model;


namespace Goldnet.Dal.cbhs
{
    public class XyhsReport
    {
        public DataTable GetDirectCostDetail(string sdata, string begindate, string enddate)
        {
            string sql = string.Format(@"
WITH c1
       AS (  SELECT   DEPT_CODE,
                      SUM (HJ_BQ_INCOME) AS HJ_BQ_INCOME,
                      SUM (HJ_LJ_INCOME) AS HJ_LJ_INCOME,
                      SUM (YP_BQ_INCOME) AS YP_BQ_INCOME,
                      SUM (YP_LJ_INCOME) AS YP_LJ_INCOME,
                      SUM (SR_BQ_INCOME) AS SR_BQ_INCOME,
                      SUM (SR_LJ_INCOME) AS SR_LJ_INCOME
               FROM   (  SELECT   ordered_by_dept dept_code,
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS hj_bq_income,           --合计本期收入
                                  SUM (costs) AS hj_lj_income,  --合计累计收入
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                              AND SUBSTR (RECK_CLASS, 0, 1) = 'A'
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS yp_bq_income,           --药品本期收入
                                  SUM(CASE
                                         WHEN SUBSTR (RECK_CLASS, 0, 1) = 'A'
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS yp_lj_income,           --药品累计收入
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                              AND SUBSTR (RECK_CLASS, 0, 1) <>
                                                    'A'
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS sr_bq_income,         --非药品本期收入
                                  SUM(CASE
                                         WHEN SUBSTR (RECK_CLASS, 0, 1) <> 'A'
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS sr_lj_income          --非药品累计收入
                           FROM   HISFACT.OUTP_CLASS2_INCOME
                          WHERE   st_date >= TO_DATE ('{0}', 'yyyymmdd')
                                  AND st_date < TO_DATE ('{2}', 'yyyymmdd')
                       GROUP BY   ordered_by_dept
                       UNION ALL
                         SELECT   ordered_by_dept dept_code,
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS hj_bq_income,           --合计本期收入
                                  SUM (costs) AS hj_lj_income,  --合计累计收入
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                              AND SUBSTR (RECK_CLASS, 0, 1) = 'A'
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS yp_bq_income,           --药品本期收入
                                  SUM(CASE
                                         WHEN SUBSTR (RECK_CLASS, 0, 1) = 'A'
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS yp_lj_income,           --药品累计收入
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                              AND SUBSTR (RECK_CLASS, 0, 1) <>
                                                    'A'
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS sr_bq_income,         --非药品本期收入
                                  SUM(CASE
                                         WHEN SUBSTR (RECK_CLASS, 0, 1) <> 'A'
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS sr_lj_income          --非药品累计收入
                           FROM   HISFACT.INP_CLASS2_INCOME
                          WHERE   st_date >= TO_DATE ('{0}', 'yyyymmdd')
                                  AND st_date < TO_DATE ('{2}', 'yyyymmdd')
                       GROUP BY   ordered_by_dept)
           GROUP BY   DEPT_CODE),
    c2
       AS (  SELECT   dept_code,
                      SUM(CASE
                             WHEN date_time >= TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                costs
                             ELSE
                                0
                          END)
                         AS hj_bq_costs,                        --合计本期成本
                      SUM (costs) AS hj_lj_costs,               --合计累计成本
                      SUM(CASE
                             WHEN date_time >= TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                                  AND b.ITEM_CODE = 'A'
                             THEN
                                costs
                             ELSE
                                0
                          END)
                         AS yp_bq_costs,                        --药品本期成本
                      SUM (CASE WHEN b.ITEM_CODE = 'A' THEN costs ELSE 0 END)
                         AS yp_lj_costs,                        --药品累计成本
                      SUM(CASE
                             WHEN date_time >= TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                                  AND b.ITEM_CODE <> 'A'
                             THEN
                                costs
                             ELSE
                                0
                          END)
                         AS sr_bq_costs,                      --非药品本期成本
                      SUM (CASE WHEN b.ITEM_CODE <> 'A' THEN costs ELSE 0 END)
                         AS sr_lj_costs                       --非药品累计成本
               FROM   cbhs.XYHS_DEPT_COSTS_DETAIL a, cbhs.XYHS_COST_ITEM_DICT b
              WHERE       date_time >= TO_DATE ('{0}', 'yyyymmdd')
                      AND date_time < TO_DATE ('{2}', 'yyyymmdd')
                      AND a.ITEM_CODE = b.ITEM_CODE
           GROUP BY   DEPT_CODE),
    c3
       AS (SELECT   c1.dept_code,
                    NVL (c1.HJ_BQ_INCOME, 0) AS HJ_BQ_INCOME,
                    NVL (c1.HJ_LJ_INCOME, 0) AS HJ_LJ_INCOME,
                    NVL (c2.hj_bq_costs, 0) AS hj_bq_costs,
                    NVL (c2.hj_lj_costs, 0) AS hj_lj_costs,
                    NVL (c1.HJ_BQ_INCOME, 0) - NVL (c2.hj_bq_costs, 0)
                       AS hj_bq_proceeds,
                    NVL (c1.HJ_LJ_INCOME, 0) - NVL (c2.hj_lj_costs, 0)
                       AS hj_lj_proceeds,
                    NVL (c1.YP_BQ_INCOME, 0) AS YP_BQ_INCOME,
                    NVL (c1.YP_LJ_INCOME, 0) AS YP_LJ_INCOME,
                    NVL (c2.YP_BQ_costs, 0) AS YP_BQ_costs,
                    NVL (c2.YP_LJ_costs, 0) AS YP_LJ_costs,
                    NVL (c1.YP_BQ_INCOME, 0) - NVL (c2.YP_BQ_costs, 0)
                       AS YP_BQ_proceeds,
                    NVL (c1.YP_LJ_INCOME, 0) - NVL (c2.YP_LJ_costs, 0)
                       AS YP_LJ_proceeds,
                    NVL (c1.SR_BQ_INCOME, 0) AS SR_BQ_INCOME,
                    NVL (c1.SR_LJ_INCOME, 0) AS SR_LJ_INCOME,
                    NVL (c2.SR_BQ_costs, 0) AS SR_BQ_costs,
                    NVL (c2.SR_LJ_costs, 0) AS SR_LJ_costs,
                    NVL (c1.SR_BQ_INCOME, 0) - NVL (c2.SR_BQ_costs, 0)
                       AS SR_BQ_proceeds,
                    NVL (c1.SR_LJ_INCOME, 0) - NVL (c2.SR_LJ_costs, 0)
                       AS SR_LJ_proceeds
             FROM   c1, c2
            WHERE   c1.dept_code = c2.dept_code(+)),
    c4
       AS (  SELECT   dept_code,
                      CASE
                         WHEN attr = '不是' THEN '　　' || dept_name
                         ELSE dept_name
                      END
                         dept_name,
                      ACCOUNT_DEPT_CODE,
                      ACCOUNT_DEPT_name,
                      ATTR
               FROM   cbhs.XYHS_DEPT_DICT
           ORDER BY   ACCOUNT_DEPT_CODE, ATTR DESC)
SELECT   c4.dept_name as HJ_BQ_I科室,
         c3.HJ_BQ_INCOME as HJ_BQ_I本期,
         c3.HJ_LJ_INCOME as HJ_LJ_I累计,
         c3.HJ_BQ_COSTS as HJ_BQ_C本期,
         c3.HJ_LJ_COSTS as HJ_LJ_C累计,
         c3.HJ_BQ_PROCEEDS as HJ_BQ_P本期,
         c3.HJ_LJ_PROCEEDS as HJ_LJ_P累计,
         c3.YP_BQ_INCOME as YP_BQ_I本期, 
         c3.YP_LJ_INCOME as YP_LJ_I累计,
         c3.YP_BQ_COSTS as YP_BQ_C本期,
         c3.YP_LJ_COSTS as YP_LJ_C累计,
         c3.YP_BQ_PROCEEDS as YP_BQ_P本期,
         c3.YP_LJ_PROCEEDS as YP_LJ_P累计,
         c3.SR_BQ_INCOME as SR_BQ_I本期,
         c3.SR_LJ_INCOME as SR_LJ_I累计,
         c3.SR_BQ_COSTS as SR_BQ_C本期,
         c3.SR_LJ_COSTS as SR_LJ_C累计,
         c3.SR_BQ_PROCEEDS as SR_BQ_P本期,
         c3.SR_LJ_PROCEEDS as SR_LJ_P累计
  FROM   c3, c4
 WHERE   c4.dept_code = c3.dept_code(+)", sdata, begindate,enddate);

            return OracleBase.Query(sql).Tables[0];
        }


        public DataTable GetDirectCureAnalyse(string sdata, string begindate, string enddate)
        {
            string sql = string.Format(@"
WITH c1
       AS (  SELECT   dept_code,
                      SUM (lj_income) AS lj_income,
                      SUM (bq_income) AS bq_income
               FROM   (  SELECT   a.ORDERED_BY_DEPT dept_code,
                                  SUM (costs) AS lj_income,
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS bq_income
                           FROM   HISFACT.INP_CLASS2_INCOME a,
                                  CBHS.XYHS_DEPT_DICT b
                          WHERE       st_date >= TO_DATE ('{1}', 'yyyymmdd')
                                  AND st_date < TO_DATE ('{2}', 'yyyymmdd')
                                  AND ORDERED_BY_DEPT = PERFORMED_BY
                                  AND a.ORDERED_BY_DEPT = B.DEPT_CODE
                                  AND b.dept_type = '4'
                       GROUP BY   a.ORDERED_BY_DEPT
                       UNION ALL
                         SELECT   a.ORDERED_BY_DEPT dept_code,
                                  SUM (costs) AS lj_income,
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS bq_income
                           FROM   HISFACT.outP_CLASS2_INCOME a,
                                  CBHS.XYHS_DEPT_DICT b
                          WHERE       st_date >= TO_DATE ('{0}', 'yyyymmdd')
                                  AND st_date < TO_DATE ('{2}', 'yyyymmdd')
                                  AND ORDERED_BY_DEPT = PERFORMED_BY
                                  AND a.ORDERED_BY_DEPT = B.DEPT_CODE
                                  AND b.dept_type = '4'
                       GROUP BY   a.ORDERED_BY_DEPT)
           GROUP BY   dept_code),
    c2
       AS (  SELECT   a.dept_code,
                      SUM (costs) AS lj_costs,
                      SUM(CASE
                             WHEN date_time >= TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                costs
                             ELSE
                                0
                          END)
                         AS bq_costs
               FROM   cbhs.XYHS_DEPT_COSTS_DETAIL a, CBHS.XYHS_DEPT_DICT b
              WHERE       date_time >= TO_DATE ('{0}', 'yyyymmdd')
                      AND date_time < TO_DATE ('{2}', 'yyyymmdd')
                      AND data_level <> 3
                      AND a.dept_code = b.dept_code
                      AND B.DEPT_TYPE = 4
           GROUP BY   a.dept_code),
    c3
       AS (  SELECT   dept_code,
                      CASE
                         WHEN attr = '不是' THEN '　　' || dept_name
                         ELSE dept_name
                      END
                         dept_name,
                      ACCOUNT_DEPT_CODE,
                      ACCOUNT_DEPT_name,
                      ATTR
               FROM   cbhs.XYHS_DEPT_DICT
           ORDER BY   ACCOUNT_DEPT_CODE, ATTR DESC),
    c4 AS (SELECT   c1.dept_code,
                    c1.bq_income,
                    c1.lj_income,
                    c2.bq_costs,
                    c2.lj_costs,
                    c1.bq_income - c2.bq_costs AS bq_q,
                    c1.lj_income - c2.lj_costs AS lj_q
             FROM   c1, c2
            WHERE   c1.dept_code = c2.dept_code)
SELECT   c3.dept_name as bq_i_科室,
         round(c4.bq_income,2) as bq_i_本期,
         round(c4.lj_income,2) as lj_i_累计,
         round(c4.bq_costs,2) as bq_c_本期,
         round(c4.lj_costs,2) as lj_c_累计,
        round(c4.bq_q,2) as bq_q_本期,
         round(c4.lj_q,2) as lj_q_累计
  FROM   c3, c4
 WHERE   c3.dept_code = c4.dept_code(+)", sdata, begindate, enddate);

            return OracleBase.Query(sql).Tables[0];
        }


        public DataTable GetTechnologyAnalyse(string sdata, string begindate, string enddate)
        {
            string sql = string.Format(@"
WITH c1
       AS (  SELECT   dept_code,
                      SUM (lj_income) AS lj_income,
                      SUM (bq_income) AS bq_income
               FROM   (  SELECT   a.PERFORMED_BY dept_code,
                                  SUM (costs) AS lj_income,
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS bq_income
                           FROM   HISFACT.INP_CLASS2_INCOME a,
                                  CBHS.XYHS_DEPT_DICT b
                          WHERE       st_date >= TO_DATE ('{0}', 'yyyymmdd')
                                  AND st_date < TO_DATE ('{2}', 'yyyymmdd')
                                  AND a.PERFORMED_BY = B.DEPT_CODE
                                  AND b.dept_type = '3'
                       GROUP BY   a.PERFORMED_BY
                       UNION ALL
                         SELECT   a.PERFORMED_BY dept_code,
                                  SUM (costs) AS lj_income,
                                  SUM(CASE
                                         WHEN st_date >=
                                                 TO_DATE ('{1}', 'yyyymmdd')
                                              AND st_date <
                                                    TO_DATE ('{2}',
                                                             'yyyymmdd')
                                         THEN
                                            costs
                                         ELSE
                                            0
                                      END)
                                     AS bq_income
                           FROM   HISFACT.outP_CLASS2_INCOME a,
                                  CBHS.XYHS_DEPT_DICT b
                          WHERE       st_date >= TO_DATE ('{0}', 'yyyymmdd')
                                  AND st_date < TO_DATE ('{1}', 'yyyymmdd')
                                  AND a.PERFORMED_BY = B.DEPT_CODE
                                  AND b.dept_type = '3'
                       GROUP BY   a.PERFORMED_BY)
           GROUP BY   dept_code),
    c2
       AS (  SELECT   a.dept_code,
                      SUM (costs) AS lj_costs,
                      SUM(CASE
                             WHEN date_time >= TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                costs
                             ELSE
                                0
                          END)
                         AS bq_costs
               FROM   cbhs.XYHS_DEPT_COSTS_DETAIL a, CBHS.XYHS_DEPT_DICT b
              WHERE       date_time >= TO_DATE ('{0}', 'yyyymmdd')
                      AND date_time < TO_DATE ('{2}', 'yyyymmdd')
                      AND a.dept_code = b.dept_code
                      AND B.DEPT_TYPE = 3
           GROUP BY   a.dept_code),
    c3
       AS (  SELECT   dept_code,
                      CASE
                         WHEN attr = '不是' THEN '　　' || dept_name
                         ELSE dept_name
                      END
                         dept_name,
                      ACCOUNT_DEPT_CODE,
                      ACCOUNT_DEPT_name,
                      ATTR
               FROM   cbhs.XYHS_DEPT_DICT
           ORDER BY   ACCOUNT_DEPT_CODE, ATTR DESC),
    c4 AS (SELECT   c1.dept_code,
                    c1.bq_income,
                    c1.lj_income,
                    c2.bq_costs,
                    c2.lj_costs,
                    c1.bq_income - c2.bq_costs AS bq_q,
                    c1.lj_income - c2.lj_costs AS lj_q
             FROM   c1, c2
            WHERE   c1.dept_code = c2.dept_code)
SELECT   c3.dept_name as bq_i_科室,
         round(c4.bq_income,2) as bq_i_本期,
         round(c4.lj_income,2) as lj_i_累计,
         round(c4.bq_costs,2) as bq_c_本期,
         round(c4.lj_costs,2) as lj_c_累计,
         round(c4.bq_q,2) as bq_q_本期,
         round(c4.lj_q,2) as lj_q_累计
  FROM   c3, c4
 WHERE   c3.dept_code = c4.dept_code(+)", sdata, begindate, enddate);

            return OracleBase.Query(sql).Tables[0];
        }


        public DataTable GetCostsClassAnalyse(string sdata, string begindate, string enddate)
        {
            string sql = string.Format(@"
WITH c1
       AS (  SELECT   dept_code,
                      SUM(CASE
                             WHEN date_time >= TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                costs
                             ELSE
                                0
                          END)
                         AS bq_sjcb,
                      SUM (a.costs) AS lj_sjcb,
                      SUM(CASE
                             WHEN b.item_type = 'A'
                                  AND date_time >=
                                        TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                a.costs
                             ELSE
                                0
                          END)
                         bq_A,
                      SUM (CASE WHEN b.item_type = 'A' THEN a.costs ELSE 0 END)
                         lj_A,
                      SUM(CASE
                             WHEN b.item_type = 'B'
                                  AND date_time >=
                                        TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                a.costs
                             ELSE
                                0
                          END)
                         bq_B,
                      SUM (CASE WHEN b.item_type = 'B' THEN a.costs ELSE 0 END)
                         lj_B,
                      SUM(CASE
                             WHEN b.item_type = 'C'
                                  AND date_time >=
                                        TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                a.costs
                             ELSE
                                0
                          END)
                         bq_C,
                      SUM (CASE WHEN b.item_type = 'C' THEN a.costs ELSE 0 END)
                         lj_C,
                      SUM(CASE
                             WHEN b.item_type = 'D'
                                  AND date_time >=
                                        TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                a.costs
                             ELSE
                                0
                          END)
                         bq_D,
                      SUM (CASE WHEN b.item_type = 'D' THEN a.costs ELSE 0 END)
                         lj_D,
                      SUM(CASE
                             WHEN b.item_type = 'E'
                                  AND date_time >=
                                        TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                a.costs
                             ELSE
                                0
                          END)
                         bq_E,
                      SUM (CASE WHEN b.item_type = 'E' THEN a.costs ELSE 0 END)
                         lj_E,
                      SUM(CASE
                             WHEN b.item_type = 'F'
                                  AND date_time >=
                                        TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                a.costs
                             ELSE
                                0
                          END)
                         bq_F,
                      SUM (CASE WHEN b.item_type = 'F' THEN a.costs ELSE 0 END)
                         lj_F,
                      SUM(CASE
                             WHEN b.item_type = 'G'
                                  AND date_time >=
                                        TO_DATE ('{1}', 'yyyymmdd')
                                  AND date_time <
                                        TO_DATE ('{2}', 'yyyymmdd')
                             THEN
                                a.costs
                             ELSE
                                0
                          END)
                         bq_G,
                      SUM (CASE WHEN b.item_type = 'G' THEN a.costs ELSE 0 END)
                         lj_G
               FROM   cbhs.XYHS_DEPT_COSTS_DETAIL a, CBHS.XYHS_COST_ITEM_DICT b
              WHERE       date_time >= TO_DATE ('{0}', 'yyyymmdd')
                      AND date_time < TO_DATE ('{2}', 'yyyymmdd')
                      AND a.item_code = b.item_code
           GROUP BY   dept_code),
    c2
       AS (SELECT   DEPT_CODE,
                    BQ_SJCB,
                    LJ_SJCB,
                    BQ_A,
                    LJ_A,
                    CASE WHEN BQ_A = 0 THEN 0 ELSE BQ_A / BQ_SJCB END
                       AS bq_bl_a,
                    CASE WHEN LJ_A = 0 THEN 0 ELSE LJ_A / LJ_SJCB END
                       AS lj_bl_a,
                    BQ_B,
                    LJ_B,
                    CASE WHEN BQ_B = 0 THEN 0 ELSE BQ_B / BQ_SJCB END
                       AS bq_bl_b,
                    CASE WHEN LJ_B = 0 THEN 0 ELSE LJ_B / LJ_SJCB END
                       AS lj_bl_b,
                    BQ_C,
                    LJ_C,
                    CASE WHEN BQ_C = 0 THEN 0 ELSE BQ_C / BQ_SJCB END
                       AS bq_bl_c,
                    CASE WHEN LJ_C = 0 THEN 0 ELSE LJ_C / LJ_SJCB END
                       AS lj_bl_c,
                    BQ_D,
                    LJ_D,
                    CASE WHEN BQ_D = 0 THEN 0 ELSE BQ_D / BQ_SJCB END
                       AS bq_bl_d,
                    CASE WHEN LJ_D = 0 THEN 0 ELSE LJ_D / LJ_SJCB END
                       AS lj_bl_d,
                    BQ_E,
                    LJ_E,
                    CASE WHEN BQ_E = 0 THEN 0 ELSE BQ_E / BQ_SJCB END
                       AS bq_bl_e,
                    CASE WHEN LJ_E = 0 THEN 0 ELSE LJ_E / LJ_SJCB END
                       AS lj_bl_e,
                    BQ_F,
                    LJ_F,
                    CASE WHEN BQ_F = 0 THEN 0 ELSE BQ_F / BQ_SJCB END
                       AS bq_bl_f,
                    CASE WHEN LJ_F = 0 THEN 0 ELSE LJ_F / LJ_SJCB END
                       AS lj_bl_f,
                    BQ_G,
                    LJ_G,
                    CASE WHEN BQ_g = 0 THEN 0 ELSE BQ_g / BQ_SJCB END
                       AS bq_bl_g,
                    CASE WHEN LJ_g = 0 THEN 0 ELSE LJ_g / LJ_SJCB END
                       AS lj_bl_g
             FROM   c1),
    c3
       AS (  SELECT   dept_code,
                      CASE
                         WHEN attr = '不是' THEN '　　' || dept_name
                         ELSE dept_name
                      END
                         dept_name,
                      ACCOUNT_DEPT_CODE,
                      ACCOUNT_DEPT_name,
                      ATTR
               FROM   cbhs.XYHS_DEPT_DICT
           ORDER BY   ACCOUNT_DEPT_CODE, ATTR DESC)
SELECT   DEPT_NAME as BQ_cb_s科室,
         round(BQ_SJCB,2) as BQ_cb_s本期,
         round(LJ_SJCB,2) as LJ_cb_S累计,
         round(BQ_A,2) as BQ_cb_A本期,
         round(LJ_A,2) as LJ_cb_A累计,
         round(BQ_BL_A,2) as BQ_BL_A本期,
         round(LJ_BL_A,2) as LJ_BL_A累计,
         round(BQ_B,2) as BQ_cb_B本期,
         round(LJ_B,2) as LJ_cb_B累计,
         round(BQ_BL_B,2) as BQ_BL_B本期,
         round(LJ_BL_B,2) as LJ_BL_B累计,
         round(BQ_C,2) as BQ_cb_C本期,
         round(LJ_C,2) as LJ_cb_C累计,
         round(BQ_BL_C,2) as BQ_BL_C本期,
         round(LJ_BL_C,2) as LJ_BL_C累计,
         round(BQ_D,2) as BQ_cb_D本期,
         round(LJ_D,2) as LJ_cb_D累计,
         round(BQ_BL_D,2) as BQ_BL_D本期,
         round(LJ_BL_D,2) as LJ_BL_D累计,
         round(BQ_E,2) as BQ_cb_E本期,
         round(LJ_E,2) as LJ_cb_E累计,
         round(BQ_BL_E,2) as BQ_BL_E本期,
         round(LJ_BL_E,2) as LJ_BL_E累计,
         round(BQ_F,2) as BQ_cb_F本期,
         round(LJ_F,2) as LJ_cb_F累计,
         round(BQ_BL_F,2) as BQ_BL_F本期,
         round(LJ_BL_F,2) as LJ_BL_F累计,
         round(BQ_G,2) as BQ_cb_G本期,
         round(LJ_G,2) as LJ_cb_G累计,
         round(BQ_BL_G,2) as BQ_BL_G本期,
         round(LJ_BL_G,2) as LJ_BL_G累计
  FROM   c2, c3
 WHERE   c2.dept_code = c3.dept_code", sdata, begindate, enddate);

            return OracleBase.Query(sql).Tables[0];
        }






    }
}
