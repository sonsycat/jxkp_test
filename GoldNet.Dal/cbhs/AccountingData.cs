using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class AccountingData
    {
        public AccountingData()
        {
        }
        /// <summary>
        /// 验证核算数据是否生成
        /// </summary>
        /// <param name="date_time"></param>
        /// <returns></returns>
        public bool IsAccount(string date_time)
        {
            string strSql = "SELECT * FROM CBHS.CBHS_DEPT_ACCOUNT_DETAIL WHERE TO_CHAR (DATE_TIME, 'yyyymm') = '"+date_time+"'";
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;//已经生成
            }
            else
            {
                return false;
            }
        }

        #region 奖金核算
        /// <summary>
        /// 查询科室核算数据
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="account_sign">收入结算标识</param>
        /// <returns>DataSet</returns>
        public DataSet GetAccountdata(string date_time, string account_sign)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.AppendFormat(@"SELECT dept_code,
                                         DECODE (dept_code, NULL, '合计', MIN (dept_name)) dept_name,
                                         SUM (incomes_charges) incomes_charges,
                                         SUM (incomes - incomes_charges) income_count, SUM (incomes) incomes,
                                         SUM (cost_fac) cost_fac, SUM (cost_arm) cost_arm, SUM (costs) costs,
                                         SUM (net_income) net_income, SUM (gross_income) gross_income,
                                         ROUND (AVG (dept_lrl), 2) dept_lrl,
                                         ROUND (AVG (dept_cbl), 2) dept_cbl,
                                         ROUND (AVG (dept_gdcbl), 2) dept_gdcbl,
                                         ROUND (AVG (dept_bdcbl), 2) dept_bdcbl,
                                         ROUND (AVG (dept_ypsrl), 2) dept_ypsrl
                                    FROM (SELECT   NVL (b.dept_code, 0) dept_code,
                                                   NVL (b.dept_name, '其他') dept_name, incomes,
                                                   incomes_charges, incomes + incomes_charges total_incomes,
                                                   cost_fac, cost_arm, cost_fac + cost_arm costs, net_income,
                                                   gross_income, dept_lrl, dept_cbl, dept_gdcbl, dept_bdcbl,
                                                   dept_ypsrl
                                              FROM {0}.cbhs_dept_account_detail a,
                                                   (SELECT years || LPAD (TO_CHAR (months), 2, '0') st_date,
                                                           dept_id dept_code, dept_name
                                                      FROM {1}.set_checkpersons) b
                                             WHERE a.dept_code = b.dept_code(+)
                                               AND TO_CHAR (date_time, 'yyyymm') = b.st_date(+)
                                               AND TO_CHAR (date_time, 'yyyymm') = '{2}'
                                               AND balance_tag = '{3}'
                                          ORDER BY NVL (b.dept_code, 0))
                                GROUP BY CUBE (dept_code)", DataUser.CBHS, DataUser.PERFORMANCE, date_time, account_sign);
            return OracleOledbBase.ExecuteDataSet(strsql.ToString());
        }
        /// <summary>
        /// 执行核算数据生成过
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <returns>string</returns>
        public string AccountDataCreate(string date_time, string ypcode)
        {
            string ProcName = DataUser.CBHS + ".SP_INCOME_COST_ACCOUNT";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time),
                                              new OleDbParameter("ypcode", ypcode),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
        /// <summary>
        ///  删除核算数据
        /// </summary>
        /// <param name="date_time">日期(201012)</param>
        public void DelAccountData(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"delete from {0}.cbhs_dept_account_detail
                                   where to_char(DATE_TIME,'yyyymm')='{1}'",DataUser.CBHS,date_time);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        #endregion 奖金核算

        #region 单项目核算
        /// <summary>
        /// 单项目核算数据
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="account_sign">收入结算标识</param>
        /// <returns>DataSet</returns>
        public DataSet GetItemAccountData(string date_time, string account_sign)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT ITEM_CODE,
                                         DECODE (ITEM_CODE, NULL, '合计', MIN (ITEM_NAME)) ITEM_NAME,
                                         SUM (INCOMES_DF) INCOMES_DF, SUM (INCOMES_JD) INCOMES_JD,
                                         SUM (INCOMES_DF + INCOMES_JD) INCOMES, SUM (COSTS_ZJ) COSTS_ZJ,
                                         SUM (COSTS_JJ) COSTS_JJ, SUM (COSTS_ZJ + COSTS_JJ) COSTS,
                                         SUM (BENEFIT) BENEFIT
                                    FROM {0}.CBHS_INCOME_ITEM_ACCOUNT
                                   WHERE BALANCE_TAG = '{1}' AND TO_CHAR (ST_DATE, 'yyyymm') = '{2}'
                                GROUP BY CUBE (ITEM_CODE)",
                                DataUser.CBHS,account_sign,date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 执行单项目核算过程
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="costvalue">全院管理总成本</param>
        /// <returns>执行结果</returns>
        public string Exec_Sp_Income_Item_Account(string date_time, double costvalue)
        {
            string ProcName = DataUser.CBHS + ".SP_INCOME_ITEM_ACCOUNT";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time),
                                              new OleDbParameter("costvalue", costvalue),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
        /// <summary>
        /// 计算全院管理总成本指标并返回值
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="guide_code">指标代码</param>
        /// <returns>指标值</returns>
        public double GetMsgCosts(string date_time, string guide_code)
        {
            string ProcName = DataUser.HOSPITALSYS + ".GUIDE_VALUE_ADD";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("tjny", date_time),
                                              new OleDbParameter("guidecode", guide_code)};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT guide_value
                                  FROM {0}.guide_value
                                 WHERE guide_code = '{1}' AND tjyf = '{2}' AND unit_code = '00'"
                                 ,DataUser.HOSPITALSYS,guide_code,date_time);
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDouble(dt.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }
        #endregion 单项目核算

        #region 病种核算
        /// <summary>
        /// 获取病种核算数据
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <returns>DataSet</returns>
        public DataSet GetDiagAccountData(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DECODE (DIAGNOSIS_NAME,NULL, '合计',DIAGNOSIS_NAME) DIAGNOSIS_NAME, 
                                         SUM (INCOME_DF) INCOME_DF,
                                         SUM (INCOME_JD) INCOME_JD, SUM (INCOME_DF + INCOME_JD) INCOMES,
                                         SUM (COSTS_ZJ) COSTS_ZJ, SUM (COSTS_JJ) COSTS_JJ,
                                         SUM (COSTS_ZJ + COSTS_JJ) COSTS,
                                         SUM (INCOME_DF + INCOME_JD - COSTS_ZJ - COSTS_JJ) AS BENEFIT
                                    FROM {0}.CBHS_DIAG_ACCOUNT
                                   WHERE TO_CHAR (ST_DATE, 'yyyymm') = '{1}'
                                GROUP BY CUBE (DIAGNOSIS_NAME)",DataUser.CBHS,date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 执行病种核算过程
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <returns></returns>
        public string Exec_Sp_Diag_Account(string date_time)
        {
            string ProcName = DataUser.CBHS + ".SP_DIAG_ACCOUNT";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
        #endregion 病种核算
    }
}
