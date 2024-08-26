using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class Income_create
    {
        public Income_create()
        {

        }

        #region 成员方法
        /// <summary>
        /// 获取收入生成状态
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public DataSet GetTask(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT ID, ST_DATE, TO_CHAR(TASK_TIME,'yyyy-mm-dd hh24:mi:ss') TASK_TIME, TASK_NAME, TASK_DEPICT, TASK_STATE,TASK_ERR
                                      FROM {0}.CBHS_INCOME_TASK
                                     WHERE TO_CHAR(ST_DATE,'yyyymm')='{1}'", DataUser.CBHS, date_time);
            DataSet ds = OracleOledbBase.ExecuteDataSet(strSql.ToString());
            if (ds.Tables[0].Rows.Count < 1)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@"SELECT ID, ST_DATE, TO_CHAR(TASK_TIME,'yyyy-mm-dd hh24:mi:ss') TASK_TIME, TASK_NAME, TASK_DEPICT, TASK_STATE,TASK_ERR
                                      FROM {0}.CBHS_INCOME_TASK WHERE ST_DATE is null", DataUser.CBHS);
                return OracleOledbBase.ExecuteDataSet(sql.ToString());
            }
            else
            {
                return ds;
            }
        }

        /// <summary>
        /// 保存操作日志
        /// </summary>
        /// <param name="dt">操作日志</param>
        /// <param name="date_time">日期（201012）</param>
        public void SaveTask(DataTable dt, string date_time)
        {

            MyLists listtable = new MyLists();
            string strDel = string.Format("DELETE FROM {0}.CBHS_INCOME_TASK WHERE TO_CHAR(ST_DATE,'yyyymm')='{1}'", DataUser.CBHS, date_time);
            List listDel = new List();
            listDel.StrSql = strDel;
            listtable.Add(listDel);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.CBHS_INCOME_TASK 
                                          (ID,ST_DATE,TASK_TIME,TASK_NAME,TASK_DEPICT,
                                           TASK_STATE,TASK_ERR)
                                        VALUES(?,TO_DATE(?,'yyyymm'),TO_DATE(?,'yyyy-mm-dd hh24:mi:ss'),?,?,?,?)", DataUser.CBHS);

                OleDbParameter[] parameteradd = {
											  new OleDbParameter("ID",dt.Rows[i]["ID"]),
											  new OleDbParameter("ST_DATE",date_time),
											  new OleDbParameter("TASK_TIME",dt.Rows[i]["TASK_TIME"]),
											  new OleDbParameter("TASK_NAME",dt.Rows[i]["TASK_NAME"]),
											  new OleDbParameter("TASK_DEPICT",dt.Rows[i]["TASK_DEPICT"]),
											  new OleDbParameter("TASK_STATE",dt.Rows[i]["TASK_STATE"]),
											  new OleDbParameter("TASK_ERR",dt.Rows[i]["TASK_ERR"])
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }

        /// <summary>
        /// 分解前发生收入
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <returns></returns>
        public DataSet GetIncomes(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@" SELECT   DECODE (SORT_NO, NULL, '合计', MIN (ITEM_NAME)) ITEM_NAME,
                                             SUM (COSTS) COSTS, SUM (CHARGES) CHARGES,
                                             SUM (COUNT_INCOME) COUNT_INCOME
                                        FROM (SELECT '1' AS SORT_NO, '门诊收入' AS ITEM_NAME,
                                                     ROUND (NVL (SUM (COSTS), 0), 2) COSTS,
                                                     ROUND (NVL (SUM (CHARGES), 0), 2) CHARGES,
                                                     ROUND (NVL (SUM (COUNT_INCOME), 0), 2) COUNT_INCOME
                                                FROM {0}.OUTP_CLASS2_INCOME
                                               WHERE TO_CHAR (ST_DATE, 'yyyymm') = '{2}' AND RECK_CLASS <> '*'
                                              UNION
                                              SELECT '2' AS SORT_NO, '门诊附加收入' AS ITEM_CODE,
                                                     ROUND (NVL (SUM (INCOMES), 0), 2) COSTS,
                                                     ROUND (NVL (SUM (INCOMES_CHARGES), 0), 2) CHARGES,
                                                     ROUND (NVL (SUM (INCOMES - INCOMES_CHARGES), 0),2) COUNT_INCOME
                                                FROM {1}.CBHS_DEPT_APPENDED_INCOME
                                               WHERE TO_CHAR (ACCOUNTING_DATE, 'yyyymm') = '{2}'
                                                 AND ACCOUNT_TYPE = '0'
                                                 AND INCOM_TYPE = '1'
                                              UNION
                                              SELECT '3' AS SORT_NO, '住院收入' AS ITEM_NAME,
                                                     ROUND (NVL (SUM (COSTS), 0), 2) COSTS,
                                                     ROUND (NVL (SUM (CHARGES), 0), 2) CHARGES,
                                                     ROUND (NVL (SUM (COUNT_INCOME), 0), 2) COUNT_INCOME
                                                FROM {0}.INP_CLASS2_INCOME
                                               WHERE TO_CHAR (ST_DATE, 'yyyymm') = '{2}' AND RECK_CLASS <> '*'
                                              UNION
                                              SELECT '4' AS SORT_NO, '住院附加收入' AS ITEM_CODE,
                                                     ROUND (NVL (SUM (INCOMES), 0), 2) COSTS,
                                                     ROUND (NVL (SUM (INCOMES_CHARGES), 0), 2) CHARGES,
                                                     ROUND (NVL (SUM (INCOMES - INCOMES_CHARGES), 0),2) COUNT_INCOME
                                                FROM {1}.CBHS_DEPT_APPENDED_INCOME
                                               WHERE TO_CHAR (ACCOUNTING_DATE, 'yyyymm') = '{2}'
                                                 AND ACCOUNT_TYPE = '0'
                                                 AND INCOM_TYPE = '0'
                                              UNION
                                              SELECT '5' AS SORT_NO, '门诊其他' AS ITEM_NAME,
                                                     ROUND (NVL (SUM (COSTS), 0), 2) COSTS,
                                                     ROUND (NVL (SUM (CHARGES), 0), 2) CHARGES,
                                                     ROUND (NVL (SUM (COUNT_INCOME), 0), 2) COUNT_INCOME
                                                FROM {0}.OUTP_CLASS2_INCOME
                                               WHERE TO_CHAR (ST_DATE, 'yyyymm') = '{2}' AND RECK_CLASS = '*'
                                              UNION
                                              SELECT '6' AS SORT_NO, '住院其他' AS ITEM_NAME,
                                                     ROUND (NVL (SUM (COSTS), 0), 2) ITEM_VALUE,
                                                     ROUND (NVL (SUM (CHARGES), 0), 2) CHARGES,
                                                     ROUND (NVL (SUM (COUNT_INCOME), 0), 2) COUNT_INCOME
                                                FROM {0}.INP_CLASS2_INCOME
                                               WHERE TO_CHAR (ST_DATE, 'yyyymm') = '{2}' AND RECK_CLASS = '*')
                                    GROUP BY CUBE (SORT_NO)
                                    ORDER BY DECODE (SORT_NO, NULL, -1, SORT_NO)", DataUser.HISFACT, DataUser.CBHS, date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 分解前结算收入
        /// </summary>
        /// <param name="date_time"></param>
        /// <returns></returns>
        public DataSet GetSettleIncomes(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DECODE (SORT_NO, NULL, '合计', MIN (ITEM_NAME)) ITEM_NAME,
                                         SUM (COSTS) COSTS, SUM (CHARGES) CHARGES,
                                         SUM (COUNT_INCOME) COUNT_INCOME
                                    FROM (SELECT '1' AS SORT_NO, '门诊收入' AS ITEM_NAME,
                                                 ROUND (NVL (SUM (COSTS), 0), 2) COSTS,
                                                 ROUND (NVL (SUM (CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (COUNT_INCOME), 0), 2) COUNT_INCOME
                                            FROM {0}.OUTP_CLASS2_INCOME
                                           WHERE TO_CHAR (ST_DATE, 'yyyymm') = '{2}' AND RECK_CLASS <> '*'
                                          UNION
                                          SELECT '2' AS SORT_NO, '门诊附加收入' AS ITEM_CODE,
                                                 ROUND (NVL (SUM (INCOMES), 0), 2) COSTS,
                                                 ROUND (NVL (SUM (INCOMES_CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (INCOMES - INCOMES_CHARGES), 0),2) COUNT_INCOME
                                            FROM {1}.CBHS_DEPT_APPENDED_INCOME
                                           WHERE TO_CHAR (ACCOUNTING_DATE, 'yyyymm') = '{2}'
                                             AND INCOM_TYPE = '1'
                                          UNION
                                          SELECT '3' AS SORT_NO, '住院收入' AS ITEM_NAME,
                                                 ROUND (NVL (SUM (COSTS), 0), 2) COSTS,
                                                 ROUND (NVL (SUM (CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (COUNT_INCOME), 0), 2) COUNT_INCOME
                                            FROM {0}.INP_CLASS2_INCOME_SETTLE
                                           WHERE TO_CHAR (ST_DATE, 'yyyymm') = '{2}' AND RECK_CLASS <> '*'
                                          UNION
                                          SELECT '4' AS SORT_NO, '住院附加收入' AS ITEM_CODE,
                                                 ROUND (NVL (SUM (INCOMES), 0), 2) COSTS,
                                                 ROUND (NVL (SUM (INCOMES_CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (INCOMES - INCOMES_CHARGES), 0),2) COUNT_INCOME
                                            FROM {1}.CBHS_DEPT_APPENDED_INCOME
                                           WHERE TO_CHAR (ACCOUNTING_DATE, 'yyyymm') = '{2}'
                                             AND INCOM_TYPE = '0'
                                             AND ACCOUNT_TYPE='1'
                                          UNION
                                          SELECT '5' AS SORT_NO, '门诊其他' AS ITEM_NAME,
                                                 ROUND (NVL (SUM (COSTS), 0), 2) COSTS,
                                                 ROUND (NVL (SUM (CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (COUNT_INCOME), 0), 2) COUNT_INCOME
                                            FROM {0}.OUTP_CLASS2_INCOME
                                           WHERE TO_CHAR (ST_DATE, 'yyyymm') = '{2}' AND RECK_CLASS = '*'
                                          UNION
                                          SELECT '6' AS SORT_NO, '住院其他' AS ITEM_NAME,
                                                 ROUND (NVL (SUM (COSTS), 0), 2) COSTS,
                                                 ROUND (NVL (SUM (CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (COUNT_INCOME), 0), 2) COUNT_INCOME
                                            FROM {0}.INP_CLASS2_INCOME_SETTLE
                                           WHERE TO_CHAR (ST_DATE, 'yyyymm') = '{2}' AND RECK_CLASS = '*')
                                GROUP BY CUBE (SORT_NO)
                                ORDER BY DECODE (SORT_NO, NULL, -1, SORT_NO)", DataUser.HISFACT, DataUser.CBHS, date_time);

            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 分解后收入
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="balance_tag"></param>
        /// <returns></returns>
        public DataSet GetIncomesCreated(string date_time, string balance_tag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DECODE (SORT_NO, NULL, '合计', MIN (ITEM_NAME)) ITEM_NAME,
                                         SUM (COSTS) COSTS, SUM (CHARGES) CHARGES,
                                         SUM (COUNT_INCOME) COUNT_INCOME
                                    FROM (SELECT '1' AS SORT_NO, '门诊收入' AS ITEM_NAME,
                                                 ROUND (NVL (SUM (INCOMES), 0), 2) AS COSTS,
                                                 ROUND (NVL (SUM (INCOMES_CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (INCOMES - INCOMES_CHARGES), 0),2) COUNT_INCOME
                                            FROM {0}.CBHS_INCOMS_INFO_ACCOUNT
                                           WHERE TO_CHAR (DATE_TIME, 'yyyymm') = '{1}'
                                             AND APPEND_FLAG = '0'
                                             AND BALANCE_TAG = '{2}'
                                             AND COSTS_TAG = '1'
                                          UNION
                                          SELECT '2' AS SORT_NO, '门诊附加收入' AS ITEM_NAME,
                                                 ROUND (NVL (SUM (INCOMES), 0), 2) AS COSTS,
                                                 ROUND (NVL (SUM (INCOMES_CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (INCOMES - INCOMES_CHARGES), 0),2) COUNT_INCOME
                                            FROM {0}.CBHS_INCOMS_INFO_ACCOUNT
                                           WHERE TO_CHAR (DATE_TIME, 'yyyymm') = '{1}'
                                             AND APPEND_FLAG = '1'
                                             AND BALANCE_TAG = '{2}'
                                             AND COSTS_TAG = '1'
                                          UNION
                                          SELECT '3' AS SORT_NO, '住院收入' AS ITEM_NAME,
                                                 ROUND (NVL (SUM (INCOMES), 0), 2) AS COSTS,
                                                 ROUND (NVL (SUM (INCOMES_CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (INCOMES - INCOMES_CHARGES), 0),2) COUNT_INCOME
                                            FROM {0}.CBHS_INCOMS_INFO_ACCOUNT
                                           WHERE TO_CHAR (DATE_TIME, 'yyyymm') = '{1}'
                                             AND APPEND_FLAG = '0'
                                             AND BALANCE_TAG = '{2}'
                                             AND COSTS_TAG = '0'
                                          UNION
                                          SELECT '4' AS SORT_NO, '住院附加收入' AS ITEM_NAME,
                                                 ROUND (NVL (SUM (INCOMES), 0), 2) AS COSTS,
                                                 ROUND (NVL (SUM (INCOMES_CHARGES), 0), 2) CHARGES,
                                                 ROUND (NVL (SUM (INCOMES - INCOMES_CHARGES), 0),2) COUNT_INCOME
                                            FROM {0}.CBHS_INCOMS_INFO_ACCOUNT
                                           WHERE TO_CHAR (DATE_TIME, 'yyyymm') = '{1}'
                                             AND APPEND_FLAG = '1'
                                             AND BALANCE_TAG = '{2}'
                                             AND COSTS_TAG = '0')
                                GROUP BY CUBE (SORT_NO)
                                ORDER BY DECODE (SORT_NO, NULL, -1, SORT_NO)", DataUser.CBHS, date_time, balance_tag);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 执行门诊收入数据生成过程
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        public string Exec_Sp_Income_Auto_Acc(string date_time)
        {
            string ProcName = DataUser.CBHS + ".SP_INCOME_AUTO_ACC";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("incomedate", date_time),rtnmsg };
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }

        /// <summary>
        /// 提取HIS原始数据
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public string Exec_Sp_Extrac_Yxt_data_Pre(string startdate, string enddate)
        {
            string ProcName = DataUser.HISDATA + ".SP_EXTRAC_YXT_DATA_PRE";
            OleDbParameter[] parameteradd = { new OleDbParameter("startdate", startdate),
                                              new OleDbParameter("enddate",enddate)
                                            };
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }

        /// <summary>
        /// 提取HIS后检查数据
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public string Exec_Sp_Extrac_Yxt_data(string startdate, string enddate)
        {
            string ProcName = DataUser.HISDATA + ".SP_EXTRAC_YXT_DATA";
            OleDbParameter[] parameteradd = { new OleDbParameter("startdate", startdate),
                                              new OleDbParameter("enddate",enddate)
                                            };
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public string Exec_Sp_Income_Calc_Per(string startdate, string enddate)
        {
            string ProcName = DataUser.HISFACT + ".SP_INCOME_CALC_PER";
            OleDbParameter[] parameteradd = { new OleDbParameter("startdate", startdate),
                                              new OleDbParameter("enddate",enddate)
                                            };
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date_time"></param>
        /// <returns></returns>
        public string Exec_sp_xyhs_patient_cost(string date_time)
        {
            //string ProcName = DataUser.CBHS + ".sp_xyhs_patient_cost";

            //OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time) };
            //OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date_time"></param>
        /// <returns></returns>
        public string Exec_sp_patien_account(string date_time)
        {
            string ProcName = DataUser.CBHS + ".sp_patien_account";

            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time) };
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }

        /// <summary>
        /// 执行住院收入数据生成过程
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        public string Exec_Sp_Income_Auto_Hos_Acc(string date_time)
        {
            string ProcName = DataUser.CBHS + ".SP_INCOME_AUTO_HOS_ACC";
             OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("incomedate", date_time),rtnmsg };
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }

        /// <summary>
        /// 执行折算成本过程
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        public string Exec_Sp_Income_To_Cost(string date_time)
        {
            string ProcName = DataUser.CBHS + ".SP_INCOME_TO_COST";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("incomedate", date_time),rtnmsg };
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }

        #endregion 成员方法
    }
}
