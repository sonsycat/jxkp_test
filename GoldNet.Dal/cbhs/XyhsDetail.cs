using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Data.OleDb;
using GoldNet.Comm.DAL.SqlServer;
using GoldNet.Model;

namespace Goldnet.Dal
{
    public class XyhsDetail
    {
        public XyhsDetail()
        {
        }
        #region 院级成本获取
        /// <summary>
        /// 查询院级成本列表
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <returns>DataSet</returns>
        public DataSet GetTotalCosts(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"  SELECT   a.ITEM_CODE, b.ITEM_NAME, SUM (COSTS) COSTS
    FROM   {0}.XYHS_COSTS_TOTAL a, {0}.XYHS_COST_ITEM_DICT b
   WHERE   A.ITEM_CODE = B.ITEM_CODE and TO_CHAR (DATE_TIME, 'yyyymm') = '{1}'
GROUP BY   a.ITEM_CODE, b.ITEM_NAME
ORDER BY   a.ITEM_CODE", DataUser.CBHS, date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 添加默认院级成本
        /// </summary>
        /// <param name="item_code">成本编码</param>
        /// <param name="item_name">成本名称</param>
        public void InsertIntoTotalCosts(string item_code, string item_name)
        {
            string strMaxIdSql = string.Format("SELECT NVL(MAX(ID),0)+1 ID FROM {0}.XYHS_COSTS_TOTAL ", DataUser.CBHS);
            string maxId = OracleOledbBase.ExecuteDataSet(strMaxIdSql).Tables[0].Rows[0][0].ToString();

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT INTO {0}.XYHS_COSTS_TOTAL 
                                        (ID,ITEM_CODE,ITEM_NAME,COSTS)
                                    VALUES('{1}','{2}','{3}',0)", DataUser.CBHS, maxId, item_code, item_name);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        public DataSet GetTotalCosts(string date_time, string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT ID,ITEM_CODE,ITEM_NAME,COSTS,
                                         DATE_TIME,OPERATOR,OPERAT_DATE,MEMO
                                  FROM   {0}.XYHS_COSTS_TOTAL
                                 WHERE   TO_CHAR (DATE_TIME, 'yyyymm') = '{1}' AND ITEM_CODE='{2}'", DataUser.CBHS, date_time, item_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 保存院级成本信息
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="item_code">成本编码</param>
        /// <param name="costs">成本额</param>
        /// <param name="item_name">成本名称</param>
        /// <param name="operators">操作员</param>
        /// <param name="memo">备注</param>
        public void SaveTotalCosts(string data_id, string date_time, string item_code, string costs, string item_name, string operators, string memo)
        {
            StringBuilder strDelSql = new StringBuilder();
            strDelSql.AppendFormat(@"DELETE FROM {0}.XYHS_COSTS_TOTAL 
                                WHERE TO_CHAR(DATE_TIME,'yyyymm')='{1}' AND id='{2}'", DataUser.CBHS, date_time, data_id);
            OracleOledbBase.ExecuteNonQuery(strDelSql.ToString());

            string strMaxIdSql = string.Format("SELECT NVL(MAX(ID),0)+1 ID FROM {0}.XYHS_COSTS_TOTAL ", DataUser.CBHS);
            string maxId = OracleOledbBase.ExecuteDataSet(strMaxIdSql).Tables[0].Rows[0][0].ToString();

            StringBuilder strAddSql = new StringBuilder();
            strAddSql.AppendFormat(@"INSERT INTO {0}.XYHS_COSTS_TOTAL (ID,ITEM_CODE,ITEM_NAME,COSTS,
                                         DATE_TIME,OPERATOR,OPERAT_DATE,MEMO)
                                        VALUES(?,?,?,?,TO_DATE(?,'yyyymm'),?,SYSDATE,?) ", DataUser.CBHS);
            OleDbParameter[] parameteradd = new OleDbParameter[] {
											  new OleDbParameter("id",maxId), 
											  new OleDbParameter("item_code",item_code), 
											  new OleDbParameter("item_name",item_name), 
											  new OleDbParameter("costs",costs), 
											  new OleDbParameter("date_time",date_time), 
											  new OleDbParameter("operator",operators), 
											  new OleDbParameter("memo",memo)
             };
            OracleOledbBase.ExecuteNonQuery(strAddSql.ToString(), parameteradd);
        }
        /// <summary>
        /// 删除院级成本
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="selectRow">成本数组</param>
        public void DelTotalCosts(string date_time, Dictionary<string, string>[] selectRow)
        {

            MyLists listtable = new MyLists();
            for (int i = 0; i < selectRow.Length; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"DELETE FROM {0}.XYHS_COSTS_TOTAL 
                                   WHERE TO_CHAR(DATE_TIME,'yyyymm')=? AND ITEM_CODE=?", DataUser.CBHS);
                OleDbParameter[] parameter = {
											  new OleDbParameter("date_time",date_time),
											  new OleDbParameter("item_code",selectRow[i]["ITEM_CODE"].ToString())
										  };
                List list = new List();
                list.StrSql = strSql;
                list.Parameters = parameter;
                listtable.Add(list);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 彻底删除院级成本
        /// </summary>
        /// <param name="date_time">日期</param>
        /// <param name="selectRow">成本数组</param>
        public void DelTrueTotalCosts(string date_time, Dictionary<string, string>[] selectRow)
        {

            MyLists listtable = new MyLists();
            for (int i = 0; i < selectRow.Length; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"DELETE FROM {0}.XYHS_COSTS_TOTAL 
                                   WHERE (TO_CHAR(DATE_TIME,'yyyymm')=? OR DATE_TIME IS NULl) AND ITEM_CODE=? ", DataUser.CBHS);
                OleDbParameter[] parameter = {
											  new OleDbParameter("date_time",date_time),
											  new OleDbParameter("item_code",selectRow[i]["ITEM_CODE"].ToString())
										  };
                List list = new List();
                list.StrSql = strSql;
                list.Parameters = parameter;
                listtable.Add(list);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 从试图查询院级成本
        /// </summary>
        /// <param name="date_itme">日期（201012）</param>
        /// <returns>DataSet</returns>
        public DataSet GetTotalCostsFromViews(string date_itme)
        {
            string strSql = string.Format(@"SELECT  ST_DATE DATE_TIME,ITEM_CODE, DEPT_CODE, COSTS
                                              FROM  {0}.V_TOTAL_COSTS
                                             WHERE  ST_DATE='{1}'", DataUser.CBHS, date_itme);
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        /// <summary>
        /// 从试图提取院级成本
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        public void SaveTatalCostsFromViews(string date_time)
        {
            MyLists listtable = new MyLists();
            string strDelSql = string.Format(@"DELETE FROM {0}.XYHS_COSTS_TOTAL WHERE TO_CHAR(DATE_TIME,'yyyymm')='{1}'", DataUser.CBHS, date_time);
            List listDel = new List();
            OleDbParameter[] parameterdel = new OleDbParameter[] { };
            listDel.StrSql = strDelSql;
            listDel.Parameters = parameterdel;
            listtable.Add(listDel);

            string strMaxIdSql = string.Format("SELECT NVL(MAX(ID),0) ID FROM {0}.XYHS_COSTS_TOTAL ", DataUser.CBHS);
            string maxId = OracleOledbBase.ExecuteDataSet(strMaxIdSql).Tables[0].Rows[0][0].ToString();

            StringBuilder strAddSql = new StringBuilder();
            strAddSql.AppendFormat(@"INSERT INTO {0}.XYHS_COSTS_TOTAL
                                            (ID,
                                   DATE_TIME,
                                   ITEM_CODE,
                                   DEPT_CODE,
                                   COSTS)
                            SELECT MIN (ID) ID,TO_DATE (ST_DATE, 'yyyymm') DATE_TIME,
                                   ITEM_CODE,DEPT_CODE,SUM (COSTS)
                             FROM (SELECT ROWNUM+{1} AS ID,ST_DATE,
                        ITEM_CODE,
                        DEPT_CODE,
                        COSTS
                                     FROM {0}.V_TOTAL_COSTS
                                    WHERE ST_DATE = '{2}'
                                       OR ST_DATE IS NULL)
                            GROUP BY  ST_DATE,ITEM_CODE, DEPT_CODE", DataUser.CBHS, maxId, date_time);
            OleDbParameter[] parameteradd = new OleDbParameter[] { };
            List listAdd = new List();
            listAdd.StrSql = strAddSql;
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);

            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 从试图提取院级成本
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        public void SaveEXPORT_COSTS(string date_time)
        {
            MyLists listtable = new MyLists();
            string strDelSql = string.Format(@"DELETE FROM {0}.XYHS_DEPT_COST_DETAIL WHERE TO_CHAR(ACCOUNTING_DATE,'yyyymm')='{1}'", DataUser.CBHS, date_time);
            List listDel = new List();
            OleDbParameter[] parameterdel = new OleDbParameter[] { };
            listDel.StrSql = strDelSql;
            listDel.Parameters = parameterdel;
            listtable.Add(listDel);

            StringBuilder strAddSql = new StringBuilder();
            strAddSql.AppendFormat(@"insert into {0}.XYHS_DEPT_COST_DETAIL(DEPT_CODE, ITEM_CODE, ACCOUNTING_DATE, COSTS,GET_TYPE) 
SELECT E.DEPT_CODE,E.ITEM_CODE, TO_DATE(ST_DATE,'yyyyMM') as ACCOUNTING_DATE,E.COSTS,'0' as GET_TYPE  FROM {0}.EXPORT_COSTS E WHERE ST_DATE='{1}'", DataUser.CBHS, date_time);
            OleDbParameter[] parameteradd = new OleDbParameter[] { };
            List listAdd = new List();
            listAdd.StrSql = strAddSql;
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);

            OracleOledbBase.ExecuteTranslist(listtable);
        }
        #endregion 院级成本获取
        #region 一级分摊
        /// <summary>
        /// 查询分摊的一级成本
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="item_code">成本项目编码</param>
        /// <returns>DataSet</returns>
        public DataSet GetDeptClass1Costs(string date_time, string item_code, string deptFilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DECODE(DEPT_CODE,NULL,0,DEPT_CODE) DEPT_CODE,
                                       DECODE(DEPT_CODE,NULL,'合计',MIN(DEPT_NAME)) DEPT_NAME,
                                       MIN(DATE_TIME) DATE_TIME,SUM(TOTAL_COSTS) TOTAL_COSTS,
                                       SUM(COSTS) COSTS,SUM(COSTS_ARMYFREE) COSTS_ARMYFREE,
                                       MIN(GET_TYPE) GET_TYPE,MIN(DATA_LEVEL) DATA_LEVEL,
                                       MIN(DATA_SOURCE) DATA_SOURCE,MIN(BALANCE_TAG) BALANCE_TAG,MIN(MEMO) MEMO
                                FROM (
                                  SELECT B.ACCOUNT_DEPT_CODE DEPT_CODE,B.ACCOUNT_DEPT_NAME DEPT_NAME,A.DATE_TIME,
                                         A.COSTS+A.COSTS_ARMYFREE AS TOTAL_COSTS,
                                         A.COSTS,A.COSTS_ARMYFREE,A.GET_TYPE,A.DATA_LEVEL,
                                         A.DATA_SOURCE,A.BALANCE_TAG,A.MEMO
                                    FROM {0}.XYHS_DEPT_COSTS_DETAIL A, {0}.XYHS_DEPT_DICT B
                                   WHERE A.DEPT_CODE(+) = B.DEPT_CODE
                                     AND GET_TYPE(+) = '3'
                                     AND DATA_LEVEL(+)='一级'
                                     AND TO_CHAR (DATE_TIME(+), 'yyyymm') = '{1}'
                                     AND ITEM_CODE(+) = '{2}'", DataUser.CBHS, date_time, item_code);
            if (deptFilter != null && deptFilter != "")
            {
                strSql.AppendFormat(" AND B.DEPT_CODE IN({0})", deptFilter);
            }
            strSql.AppendFormat(@" ORDER BY B.SORT_NO,B.ACCOUNT_DEPT_CODE
                                ) GROUP BY CUBE(DEPT_CODE)");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 查询具有院级成本下成本项目
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <returns>DataSet</returns>
        public DataSet GetTotalCostsItems(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  ITEM_CODE, ITEM_NAME, COSTS
                                    FROM  {0}.XYHS_COSTS_TOTAL
                                   WHERE  TO_CHAR (DATE_TIME, 'yyyymm') = '{1}'
                                   ORDER BY ITEM_CODE", DataUser.CBHS, date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        ///  查询成本项目的成本总额
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="item_code">成本项目编码</param>
        /// <returns>Double</returns>
        public Double GetItemAllCosts(string date_time, string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  ITEM_CODE, ITEM_NAME, COSTS
                                    FROM  {0}.XYHS_COSTS_TOTAL
                                   WHERE  TO_CHAR (DATE_TIME, 'yyyymm') = '{1}'
                                     AND  ITEM_CODE='{2}'
                                   ORDER BY ITEM_CODE", DataUser.CBHS, date_time, item_code);
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDouble(dt.Rows[0]["COSTS"]);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 删除科室分摊成本
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="item_code">成本编码</param>
        /// <param name="dept_code">科室编码</param>
        public void DelDeptCosts(string date_time, string item_code, Dictionary<string, string>[] selectRow)
        {
            MyLists listtable = new MyLists();
            for (int i = 0; i < selectRow.Length; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"DELETE FROM {0}.XYHS_DEPT_COSTS_DETAIL
                                   WHERE TO_CHAR(DATE_TIME,'yyyymm')=?
                                     AND ITEM_CODE=? AND GET_TYPE='3' AND DATA_LEVEL='一级'
                                     AND DEPT_CODE=? ", DataUser.CBHS, date_time, item_code, selectRow[i]["DEPT_CODE"]);
                OleDbParameter[] parameter = {
											  new OleDbParameter("date_time",date_time),
											  new OleDbParameter("item_code",item_code),
											  new OleDbParameter("dept_code",selectRow[i]["DEPT_CODE"])
										  };
                List list = new List();
                list.StrSql = strSql;
                list.Parameters = parameter;
                listtable.Add(list);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        public void SaveDeptClass1Costs(List<GoldNet.Model.CostDetail> costdetails, string date_time, string item_code, string item_name, string operators)
        {
            MyLists listtable = new MyLists();
            string strDel = string.Format("DELETE FROM {0}.XYHS_DEPT_COSTS_DETAIL WHERE TO_CHAR(DATE_TIME,'yyyymm')=? AND ITEM_CODE=? AND GET_TYPE='3' AND DATA_LEVEL='一级'", DataUser.CBHS);
            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { new OleDbParameter("", date_time), new OleDbParameter("", item_code) };
            listtable.Add(listDel);

            foreach (GoldNet.Model.CostDetail costdetail in costdetails)
            {
                if (costdetail.Total_costs == null || costdetail.Total_costs.Equals("") || costdetail.Dept_code.Equals(""))
                {
                    continue;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.XYHS_DEPT_COSTS_DETAIL 
                                          (ITEM_CODE, ITEM_NAME, DEPT_CODE,DATE_TIME, COSTS, 
                                           GET_TYPE, COSTS_ARMYFREE, DATA_LEVEL, DATA_SOURCE, BALANCE_TAG, MEMO)
                                        VALUES(?,?,?,TO_DATE(?,'yyyymm'),?,?,?,?,?,?,?)", DataUser.CBHS);
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("item_code",item_code),
											  new OleDbParameter("item_name",item_name),
											  new OleDbParameter("dept_code",costdetail.Dept_code),
											  new OleDbParameter("date_time",date_time),
											  new OleDbParameter("costs",costdetail.Costs),
											  new OleDbParameter("get_type",'3'),//3为分摊
											  new OleDbParameter("costs_armyfree",costdetail.Costs_armyfree),
											  new OleDbParameter("data_level","一级"),
											  new OleDbParameter("data_source",""),
											  new OleDbParameter("balance_tag",'0'),
											  new OleDbParameter("memo",costdetail.Memo)
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 执行成本分摊过程
        /// </summary>
        /// <param name="accountdate">日期（201012）</param>
        /// <param name="costitem">成本项目代码</param>
        /// <param name="costvalue">成本总额</param>
        /// <returns>执行结果</returns>
        public string Exec_Sp_Hos_Costs_Deal(string accountdate, string costitem, double costvalue)
        {

            string ProcName = DataUser.CBHS + ".SP_HOS_COSTS_DEAL";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", accountdate),
                                              new OleDbParameter("costitem",costitem),
                                              new OleDbParameter("costvalue",costvalue),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
        #endregion 一级分摊
        #region 四级分摊
        /// <summary>
        /// 查询分摊后成本
        /// </summary>
        /// <param name="date_time">日期</param>
        /// <param name="dept_type">科室类别</param>
        /// <returns>DataSet</returns>
        public DataSet GetDealedCosts(string date_time, string dept_type, string deptFilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DECODE(DEPT_CODE,NULL,'',DEPT_CODE) DEPT_CODE,
                                       DECODE(DEPT_CODE,NULL,'合计',MIN(DEPT_NAME)) DEPT_NAME,
                                       SUM(TOTAL_COSTS) TOTAL_COSTS,
                                       SUM(DIR_COSTS) DIR_COSTS,
                                       SUM(INDIR_COSTS) INDIR_COSTS
                                  FROM(
                                  SELECT   B.ACCOUNT_DEPT_CODE DEPT_CODE,B.ACCOUNT_DEPT_NAME DEPT_NAME,
                                           SUM (A.COSTS + COSTS_ARMYFREE) AS TOTAL_COSTS,
                                           NVL(SUM(CASE WHEN A.GET_TYPE  ='0'
                                                  THEN COSTS + COSTS_ARMYFREE END),0)
                                              AS DIR_COSTS,
                                           NVL(SUM (CASE WHEN A.GET_TYPE = '1' THEN COSTS + COSTS_ARMYFREE END),0)
                                              AS INDIR_COSTS
                                    FROM   {0}.XYHS_DEPT_COSTS_DETAIL A, {0}.XYHS_DEPT_DICT B
                                   WHERE   A.DEPT_CODE = B.DEPT_CODE
                                           AND TO_CHAR (A.DATE_TIME, 'yyyymm') = '{1}'", DataUser.CBHS, date_time);
            if (dept_type != null && dept_type != "")
            {
                strSql.AppendFormat(" AND B.DEPT_TYPE='{0}'", dept_type);
            }
            if (deptFilter != null && deptFilter != "")
            {
                strSql.AppendFormat(" AND B.DEPT_CODE IN({0})", deptFilter);
            }
            strSql.Append(@" GROUP BY   B.ACCOUNT_DEPT_CODE, B.ACCOUNT_DEPT_NAME, B.SORT_NO
                                ORDER BY   B.SORT_NO, B.ACCOUNT_DEPT_CODE)
                                GROUP BY CUBE(DEPT_CODE)");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        public void delxyhscostitem(string itemcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("delete cbhs.XYHS_COST_ITEM_DICT where item_code='{0}'", itemcode);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        /// <summary>
        /// 项目核算列表
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DataSet GetXyhsItemaccount(string datetime)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   a.item_code, b.item_name, SUM (a.incomes) incomes,
         SUM (a.ITEM_COSTS) costs
    FROM {0}.xyhs_dept_account_detail a, (SELECT   a.*, b.class_name as item_name
              FROM   cbhs.xyhs_income_item_dict a,
                     hisdata.reck_item_class_dict b
             WHERE   a.ITEM_CODE = b.CLASS_CODE) b
   WHERE to_char(a.date_time,'yyyyMM') >= '{1}'
     AND a.item_code = b.ITEM_CODE
GROUP BY a.item_code, b.item_name order by a.item_code", DataUser.CBHS, datetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 病种核算列表
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DataSet GetXyhsDiagaccount(string datetime)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"  SELECT   DIAGNOSIS_CODE,
           DIAGNOSIS_NAME,
           A.DEPT_CODE,
           dept_name,
           sum(costs) costs,
           sum(DIAGNOSIS_COSTS) DIAGNOSIS_COSTS,
           SUM (costs - DIAGNOSIS_COSTS) DIAGNOSIS
    FROM   cbhs.XYHS_PATIENT_ACCOUNT_DETAIL a,
           cbhs.XYHS_DIAGNOSIS_ICD b,
           cbhs.XYHS_DEPT_DICT c
   WHERE   A.ICD_CODE = B.ICD_CODE AND a.dept_code = c.dept_code
   and to_char(ST_DATE,'yyyymm')='{0}'
GROUP BY   DIAGNOSIS_CODE, DIAGNOSIS_NAME,A.DEPT_CODE, dept_name",  datetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 病种核算列表
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DataSet GetXyhsDiagaccount(string datetime, string diagcode, string filter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.PATIENT_ID,a.patient_name,b.dept_name,a.ORDERED_BY_DOCTOR,SUM (a.CHARGES) incomes,SUM (a.COSTS-a.CHARGES) INCOMES_CHARGES,sum(a.COSTS) incomes_all,sum(COST_COSTS_CHARGES) costs,sum(COST_COSTS-COST_COSTS_CHARGES) COST_COSTS_CHARGES,SUM (a.costs)-sum(COST_COSTS) COUNT_INCOME,sum(COST_COSTS) costs_all  
    FROM {0}.XYHS_PATIENT_ACCOUNT_DETAIL a,cbhs.XYHS_DEPT_DICT b
   WHERE to_char(a.ST_DATE,'yyyyMM') = '{1}' and a.dept_code=b.dept_code and b.COM_DEPT_FLAGS=0", DataUser.CBHS, datetime);
            

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        public DataSet GetXyhsDiagaccountdetail(string datetime, string diagcode, string filter, string outin)
        {
            
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"  SELECT   DIAGNOSIS_CODE,
           DIAGNOSIS_NAME,
           A.DEPT_CODE,
           dept_name,
           sum(costs) costs,
           sum(DIAGNOSIS_COSTS) DIAGNOSIS_COSTS,
           SUM (costs - DIAGNOSIS_COSTS) DIAGNOSIS
    FROM   cbhs.XYHS_PATIENT_ACCOUNT_DETAIL a,
           cbhs.XYHS_DIAGNOSIS_ICD b,
           cbhs.XYHS_DEPT_DICT c
   WHERE   A.ICD_CODE = B.ICD_CODE AND a.dept_code = c.dept_code
   and to_char(ST_DATE,'yyyymm')='{0}'
GROUP BY   DIAGNOSIS_CODE, DIAGNOSIS_NAME,A.DEPT_CODE, dept_name", DataUser.CBHS, datetime, outin);
           

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 全成本项目
        /// </summary>
        /// <returns></returns>
        public DataSet GetXyhsCostItem()
        {
            string str = @" SELECT   aa.item_code,
           aa.item_name,
           c.item_code || ':' || C.ITEM_NAME ITEM_TYPE,
           aa.FINANCE_ITEM
           ,
           aa.FINANCE_ITEM_gl
    FROM   (SELECT   b.item_code AS item_code,
                     b.item_name AS ITEM_NAME,
                     b.ITEM_TYPE,
                     b.FINANCE_ITEM,
                     b.FINANCE_ITEM_gl
              FROM     
                        cbhs.XYHS_COST_ITEM_DICT b
                     ) aa, cbhs.XYHS_COSTS_DICT c
   WHERE   aa.item_type = c.ITEM_CODE(+)
ORDER BY   aa.item_code";
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        public void DelCostItem(string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0}.XYHS_COST_ITEM_DICT where item_code='{1}'", DataUser.CBHS, item_code);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }

        /// <summary>
        /// 根据类别查询全成本项目
        /// </summary>
        /// <param name="itemtype"></param>
        /// <returns></returns>
        public DataTable GetXyhsCostItemByItemType(string itemtype)
        {
            string str = string.Format(@"select * from (
SELECT   aa.item_code,
         aa.item_name,
         c.item_code||':'||C.ITEM_NAME ITEM_TYPE
  FROM   (SELECT   NVL (b.item_code, A.ITEM_CODE) AS item_code,
         NVL (b.item_name, A.ITEM_NAME) AS ITEM_NAME,
         b.ITEM_TYPE
  FROM      cbhs.CBHS_COST_ITEM_DICT a
         FULL JOIN
            cbhs.XYHS_COST_ITEM_DICT b
         ON a.item_code = B.ITEM_CODE) aa,
         cbhs.XYHS_COSTS_DICT c
 WHERE   aa.item_type = c.ITEM_CODE(+)
order by aa.item_code)
where ITEM_TYPE like '{0}%'", itemtype);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        //全成本收入项目
        public DataSet GetxyhsincomesList(string itemcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.item_class item_code, a.item_name, DECODE (nvl(b.flags,'0'), '0', '否', '是') flags, b.ratio
  FROM {0}.cbhs_distribution_calc_schm a, {0}.xyhs_income_item_dict b
 WHERE a.item_class = b.item_code(+) ", DataUser.CBHS);
            if (itemcode != "")
            {
                str.AppendFormat(" and a.item_class='{0}'", itemcode);
            }
            str.AppendFormat(" order by a.item_class");
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }
        public DataSet GetCost_incomeList(string costcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.ITEM_CODE cost_code,a.ITEM_NAME cost_name,b.INCOME_CODE,b.ITEM_NAME income_name from cbhs.V_XYHS_COST_ITEM_DICT a left join 
(select m.COST_CODE,m.INCOME_CODE,n.ITEM_NAME from cbhs.XYHS_ITEM_COST m,cbhs.CBHS_DISTRIBUTION_CALC_SCHM n where m.INCOME_CODE=n.ITEM_CLASS ) b
on A.ITEM_CODE=B.COST_CODE ");
            if (costcode != "")
            {
                str.AppendFormat(" where a.item_code='{0}'", costcode);
            }
            str.Append(" order by b.income_code,a.item_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }
        public DataSet GetDept_incomeList()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.dept_code,a.dept_name,b.item_code income_code,b.item_name income_name from 
(select x.dept_code,x.item_code,y.dept_name from  cbhs.XYHS_DEPT_ITEM_CODE x,comm.sys_dept_dict y where x.dept_code=y.dept_code) a, 
(select m.DEPT_CODE,m.ITEM_CODE,n.ITEM_NAME from cbhs.XYHS_DEPT_ITEM_CODE m,cbhs.CBHS_DISTRIBUTION_CALC_SCHM n where m.ITEM_CODE=n.ITEM_CLASS ) b
where A.dept_CODE=B.DEPT_CODE and A.ITEM_CODE=b.item_code");

            str.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }
        public DataSet GetDiagnosisList()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.DIAGNOSIS_CODE,a.DIAGNOSIS_NAME from cbhs.XYHS_DIAGNOSIS_DICT a");

            str.Append(" order by a.DIAGNOSIS_CODE");
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }
        public DataSet GetCostType()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select item_code,item_name from cbhs.XYHS_COSTS_DICT");

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public DataTable GetCostitem(string itemcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   NVL(B.ITEM_CODE,A.ITEM_CODE) AS ITEM_CODE,
         NVL(B.ITEM_NAME,A.ITEM_NAME) AS ITEM_NAME,
         c.item_code item_type_code,
         C.ITEM_NAME item_type_name,
         b.FINANCE_ITEM
  FROM   cbhs.CBHS_COST_ITEM_DICT a,
         cbhs.XYHS_COST_ITEM_DICT b,
         cbhs.XYHS_COSTS_DICT c
 WHERE       a.item_code = B.ITEM_CODE(+)
         AND B.ITEM_TYPE = C.ITEM_CODE(+)
         AND a.item_code = '{0}'", itemcode);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        //全成本设置
        public void SaveXyhsItem(string itemcode, string itemtype)
        {
            MyLists listtable = new MyLists();
            string strDel = string.Format("DELETE FROM {0}.XYHS_COST_ITEM_DICT WHERE item_code='{1}'", DataUser.CBHS, itemcode);
            List listDel = new List();
            listDel.StrSql = strDel;
            listtable.Add(listDel);

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT INTO {0}.XYHS_COST_ITEM_DICT 
                                          (item_code,item_type)
                                        VALUES(?,?)", DataUser.CBHS);

            OleDbParameter[] parameteradd = {
											  new OleDbParameter("item_code",itemcode),
											  new OleDbParameter("item_type",itemtype)
											 
										  };
            List listAdd = new List();
            listAdd.StrSql = strSql;
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        //全成本收入设置
        public void SaveXyhsincomesItem(string itemcode, string flags, string ratio)
        {
            if (flags == "是")
                flags = "1";
            if (flags == "否")
                flags = "0";
            MyLists listtable = new MyLists();
            string strDel = string.Format("DELETE FROM {0}.XYHS_INCOME_ITEM_DICT WHERE item_code='{1}'", DataUser.CBHS, itemcode);
            List listDel = new List();
            listDel.StrSql = strDel;
            listtable.Add(listDel);

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT INTO {0}.XYHS_INCOME_ITEM_DICT 
                                          (item_code,FLAGS,RATIO)
                                        VALUES(?,?,?)", DataUser.CBHS);

            OleDbParameter[] parameteradd = {
											  new OleDbParameter("item_code",itemcode),
											  new OleDbParameter("FLAGS",flags),
                                              new OleDbParameter("RATIO",ratio)
											 
										  };
            List listAdd = new List();
            listAdd.StrSql = strSql;
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        ///执行成本分摊过程
        /// </summary>
        /// <param name="accountdate"></param>
        /// <returns></returns>
        public string Exec_Sp_Cost_Deal(string accountdate)
        {

            string ProcName = DataUser.CBHS + ".SP_XYHS_FOURLEVEL_COST";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", accountdate),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
        /// <summary>
        /// 项目核算
        /// </summary>
        /// <param name="accountdate"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string Exec_Sp_Item_Account(string accountdate, string username)
        {

            string ProcName = DataUser.CBHS + ".SP_XYHS_DEPT_ACCOUNT";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", accountdate),
                                              new OleDbParameter("username", username),
                                            rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }
        /// <summary>
        /// 病种核算
        /// </summary>
        /// <param name="accountdate"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string Exec_Sp_Diag_Account(string accountdate, string username)
        {

            string ProcName = DataUser.CBHS + ".sp_xyhs_diag_account";
            //OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            //rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", accountdate),
                                              new OleDbParameter("username", username)};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return "";
        }
        /// <summary>
        /// 返回还未分解的院级成本项目
        /// </summary>
        /// <param name="date_time">日期</param>
        /// <returns>DataSet</returns>
        public DataSet TestCostsDealComplete(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  ITEM_NAME
                                    FROM  {0}.XYHS_COSTS_TOTAL A
                                   WHERE  NOT EXISTS
                                         (SELECT A.ITEM_CODE
                                            FROM {0}.XYHS_DEPT_COSTS_DETAIL B
                                           WHERE A.ITEM_CODE = B.ITEM_CODE
                                             AND TO_CHAR (B.DATE_TIME, 'yyyymm') = '{1}'
                                           GROUP BY A.ITEM_CODE)
                                   AND TO_CHAR (A.DATE_TIME, 'yyyymm') = '{1}'", DataUser.CBHS, date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        //项目核算明细
        public DataSet GetXyhsitemdetail(string itemcode, string datetime)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.item_code, b.item_name, c.dept_name, incomes,a.ITEM_COSTS costs
  FROM cbhs.xyhs_dept_account_detail a,
        (SELECT   a.*, b.class_name AS item_name
            FROM   cbhs.xyhs_income_item_dict a,
                   hisdata.reck_item_class_dict b
           WHERE   a.ITEM_CODE = b.CLASS_CODE) b,
       cbhs.xyhs_dept_dict c
 WHERE a.dept_code = c.dept_code
   AND a.item_code = b.item_code
   AND a.item_code = '{0}'
   and to_char(A.DATE_TIME,'yyyyMM')='{1}'", itemcode, datetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        //病种核算明细
        public DataSet GetXyhsdiagdetail(string diagcode, string datetime)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.diag_code, b.DIAGNOSIS_NAME diag_name, c.dept_name, incomes, costs
  FROM cbhs.XYHS_DIAG_ACCOUNT_DETAIL a,
       hisdata.DIAGNOSIS_DICT b,
       comm.sys_dept_dict c
 WHERE a.dept_code = c.dept_code
   AND a.diag_code = b.DIAGNOSIS_CODE
   AND a.diag_code = '{0}'
   and to_char(A.DATE_TIME,'yyyyMM')='{1}'
   and A.BALANCE_TAG='1'", diagcode, datetime);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 获取科室成本分摊后明细数据
        /// </summary>
        /// <param name="dete_time">日期（201012）</param>
        /// <param name="dept_code">科室编码</param>
        /// <returns>DataSet</returns>
        public DataSet GetCostsDealDetail(string date_time, string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT '1' AS SORT_NO,ITEM_CODE,ITEM_NAME,SUM (COSTS+COSTS_ARMYFREE) COSTS,COST_TYPE
                                  FROM (SELECT A.ITEM_CODE,B.ITEM_NAME,A.COSTS,COSTS_ARMYFREE,
                                               CASE WHEN GET_TYPE ='0' THEN '直接成本'
                                                    WHEN GET_TYPE = '1' THEN '间接成本'
                                               END COST_TYPE
                                          FROM {0}.XYHS_DEPT_COSTS_DETAIL A,CBHS.CBHS_COST_ITEM_DICT B,cbhs.XYHS_DEPT_DICT c
                                         WHERE TO_CHAR(DATE_TIME,'yyyymm')='{1}' and a.dept_code=c.dept_code AND A.ITEM_CODE=B.ITEM_CODE", DataUser.CBHS, date_time);
            if (dept_code != null && dept_code != "")
            {
                strSql.AppendFormat(" AND c.ACCOUNT_DEPT_CODE = '{0}'", dept_code);
            }

            strSql.AppendFormat(@")
                                GROUP BY ITEM_CODE,ITEM_NAME,COST_TYPE
                                UNION ALL
                                SELECT '0' SORT_NO,'' ITEM_CODE,'合计' ITEM_NAME,SUM (COSTS+COSTS_ARMYFREE) COSTS,'' COST_TYPE
                                  FROM (SELECT ITEM_CODE,COSTS,COSTS_ARMYFREE,
                                               CASE WHEN GET_TYPE='0' THEN '直接成本'
                                                    WHEN GET_TYPE = '1' THEN '间接成本'
                                               END COST_TYPE
                                          FROM {0}.XYHS_DEPT_COSTS_DETAIL A,cbhs.XYHS_DEPT_DICT B
                                         WHERE TO_CHAR(DATE_TIME,'yyyymm')='{1}' AND A.DEPT_CODE=B.DEPT_CODE ", DataUser.CBHS, date_time);
            if (dept_code != null && dept_code != "")
            {
                strSql.AppendFormat(" AND B.ACCOUNT_DEPT_CODE = '{0}'", dept_code);
            }
            strSql.AppendFormat(@") ORDER BY SORT_NO, ITEM_CODE");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取科室成本项目构成比数据
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="dept_code">科室编码</param>
        /// <returns>DataSet</returns>
        public DataSet GetChartDataByItem(string date_time, string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   c.ITEM_NAME, round(SUM (A.COSTS),2) COSTS
                                FROM   {0}.XYHS_DEPT_COSTS_DETAIL A,CBHS.XYHS_DEPT_DICT B,cbhs.CBHS_COST_ITEM_DICT c
                               WHERE   TO_CHAR (DATE_TIME, 'yyyymm') = '{1}' and a.item_code=c.item_code 
                                 AND   A.DEPT_CODE=B.DEPT_CODE", DataUser.CBHS, date_time);
            if (dept_code != null && dept_code != "")
            {
                strSql.AppendFormat(" AND B.ACCOUNT_DEPT_CODE = '{0}'", dept_code);
            }
            strSql.Append(" GROUP BY   c.ITEM_NAME");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取科室成本属性构成本比数据
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="dept_code">科室编码</param>
        /// <returns>DataSet</returns>
        public DataSet GetChartDataByType(string date_time, string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT ITEM_NAME, round(SUM (COSTS),2) COSTS
                                  FROM (SELECT CASE WHEN A.GET_TYPE ='0' THEN '直接成本'
                                                    WHEN A.GET_TYPE = '1' THEN  '间接('||c.XYHS_DEPT_TYPE||')'
                                               END ITEM_NAME,A.COSTS
                                              FROM {0}.XYHS_DEPT_COSTS_DETAIL A,CBHS.XYHS_DEPT_DICT B,cbhs.XYHS_DEPT_TYPE_DICT c
                                             WHERE TO_CHAR (DATE_TIME, 'yyyymm') = '{1}'
                                               AND A.DEPT_CODE=B.DEPT_CODE and a.data_level=c.id", DataUser.CBHS, date_time);

            if (dept_code != null && dept_code != "")
            {
                strSql.AppendFormat(" AND B.ACCOUNT_DEPT_CODE = '{0}'", dept_code);
            }
            strSql.Append(" )GROUP BY   ITEM_NAME");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 查询科室收入构成比数据
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="dept_code">科室编码</param>
        /// <returns>DataSet</returns>
        public DataSet GetChartSrData(string date_time, string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  C.ITEM_NAME ITEM_NAME, round(SUM (COSTS),2) COSTS
                                    FROM  {0}.INP_CLASS2_INCOME A, {1}.XYHS_DEPT_DICT B,{1}.CBHS_DISTRIBUTION_CALC_SCHM C
                                   WHERE  A.ORDERED_BY_DEPT = B.DEPT_CODE
                                          AND A.RECK_CLASS= C.ITEM_CLASS
                                          AND TO_CHAR (A.ST_DATE, 'yyyymm') = '{2}' ", DataUser.HISFACT, DataUser.CBHS, date_time);
            if (dept_code != null && dept_code != "")
            {
                strSql.AppendFormat(" AND B.ACCOUNT_DEPT_CODE = '{0}'", dept_code);
            }
            strSql.AppendFormat(@"  GROUP BY  C.ITEM_CLASS,C.ITEM_NAME
                                ORDER BY  C.ITEM_CLASS");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        #endregion 四级分摊
        #region 效益核算
        /// <summary>
        /// 查询效益核算数据
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <returns>DataSet</returns>
        public DataSet GetXyhsDeptData(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT dept_code,
                                       DECODE (dept_code, NULL, '合计', MIN (dept_name)) dept_name,
                                       SUM (incomes) incomes,
                                       SUM (income_charges) income_charges,
                                       SUM (total_costs) total_costs,
                                       SUM (dir_costs) dir_costs,
                                       SUM (indir_costs) indir_costs,
                                       SUM (benefit) benefit
                                  FROM (SELECT a.dept_code,
                                               a.dept_name,
                                               incomes,
                                               income_charges,
                                               total_costs,
                                               dir_costs,
                                               indir_costs,
                                               benefit
                                          FROM {0}.XYHS_DEPT_ACCOUNT a, {0}.XYHS_DEPT_DICT b
                                         WHERE a.dept_code = b.dept_code
                                           AND TO_CHAR (a.date_time, 'yyyymm') = '{1}'
                                         ORDER BY   SORT_NO, dept_code)
                                GROUP BY  CUBE (dept_code)", DataUser.CBHS, date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 执行效益核算过程
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="operators">操作员</param>
        /// <returns>存储过程返回信息</returns>
        public string Exec_Sp_Dept_Account(string date_time, string operators)
        {
            string ProcName = DataUser.CBHS + ".SP_DEPT_ACCOUNT";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time),
                                              new OleDbParameter("operators",operators),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
        /// <summary>
        /// 收益线查询收入
        /// </summary>
        /// <param name="date_time">日期</param>
        /// <param name="dept_code">科室编码</param>
        /// <returns>DataSet</returns>
        public DataSet GetLineChartCharges(string date_time, string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   nvl(SUM (INCOME_CHARGES),0) charges
                                    FROM   {0}.XYHS_DEPT_ACCOUNT a, {0}.xyhs_dept_dict b
                                   WHERE       a.DEPT_CODE = B.DEPT_CODE
                                           AND TO_CHAR (DATE_TIME, 'yyyymm') = '{1}'", DataUser.CBHS, date_time);

            if (dept_code != null && dept_code != "")
            {
                strSql.AppendFormat(" AND b.ACCOUNT_DEPT_CODE = '{0}'", dept_code);
            }
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 成本线查询成本
        /// </summary>
        /// <param name="date_time">日期</param>
        /// <param name="dept_code">科室编码</param>
        /// <returns>DataSet</returns>
        public DataSet GetLineChartCosts(string date_time, string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select nvl(sum (costs+costs_armyfree),0) costs,
                                         sum (decode (cost_property, 1, costs+costs_armyfree, 0)) gd_costs
                                  from   {0}.xyhs_dept_costs_detail a,
                                         {0}.xyhs_dept_dict b,
                                         {0}.cbhs_cost_item_dict c
                                 where       a.dept_code = b.dept_code
                                         and a.item_code = c.item_code
                                         and to_char (date_time, 'yyyymm') = '{1}'", DataUser.CBHS, date_time);
            if (dept_code != null && dept_code != "")
            {
                strSql.AppendFormat(" and b.account_dept_code = '{0}'", dept_code);
            }
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 成本医01
        /// </summary>
        /// <param name="depttype"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost01(string depttype, string begindate, string enddate)
        {
            DataTable table = GetXyhsCostdictSFYL();

            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"   SELECT   DECODE (a.account_dept_code, '', '', MIN (a.account_dept_code))
              AS dept_code,
           CASE
              WHEN a.account_dept_code IS NULL THEN '合计'
              ELSE MIN (a.account_dept_name)
           END ");
            str.AppendFormat("  \"科室名称\" ");

            str.AppendFormat(",round(sum(a.COSTS),2) \"合计\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {

                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1), '{0}', COSTS, 0)),2) \"{1}\"", table.Rows[i]["ITEM_CODE"].ToString(), table.Rows[i]["ITEM_NAME"].ToString());

            }
            str.AppendFormat(@" FROM   (SELECT   detail.*,
                     dept.account_dept_code,
                     dept.account_dept_name,
                     dt.XYHS_DEPT_TYPE,
                     dt.id
              FROM   {0}.xyhs_dept_costs_detail detail,
                     {0}.xyhs_dept_dict dept,
                     {0}.XYHS_DEPT_TYPE_DICT dt
             WHERE       detail.dept_code = dept.dept_code
                     AND detail.GET_TYPE = 0
                     AND dept.DEPT_TYPE = dt.id ", DataUser.CBHS);
            if (depttype != "")
            {
                str.AppendFormat(" and dept.dept_type={0}", depttype);
            }
            str.AppendFormat(@" ) a,{2}.xyhs_cost_item_dict c,
       (select * from {2}.xyhs_costs_dict where SFYL is null or  SFYL='0') d WHERE a.item_code = c.item_code
       AND c.item_type = d.item_code and a.DATE_TIME >= DATE '{0}' AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.CBHS);


            str.AppendFormat(" GROUP BY   ROLLUP (a.ACCOUNT_DEPT_CODE) ");

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        /// <summary>
        /// 成本医01
        /// </summary>
        /// <param name="depttype"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost01_1(string depttype, string begindate, string enddate)
        {
            DataTable table1 = GetXyhsCostdictSFYL1();
            DataTable table2 = GetXyhsCostdictSFYL2();
            DataTable table = GetXyhsCostdictSFYL();
            DataTable table01 = GetXyhsCostdictSFYL01();
            DataTable tableAll = GetXyhsCostdict();

            StringBuilder str = new StringBuilder();
            str.Append(@"
  SELECT   DECODE (a.account_dept_code, '', '', MIN (a.account_dept_code))
              AS dept_code,
           CASE
              WHEN a.account_dept_code IS NULL THEN '合计'
              ELSE MIN (a.account_dept_name)
           END");
            str.AppendFormat("  \"科室名称\"");

            str.AppendFormat(",ROUND (SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1),");
            ///////////////
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" '{0}', COSTS,", table.Rows[i]["ITEM_CODE"].ToString());
            }
            ////////////////

            str.AppendFormat("0)), 2) \"医疗成本合计\"");

            for (int i = 0; i < table1.Rows.Count; i++)
            {

                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1), '{0}', COSTS, 0)),2) \"{1}\"", table1.Rows[i]["ITEM_CODE"].ToString(), table1.Rows[i]["ITEM_NAME"].ToString());

            }
            str.AppendFormat(",ROUND (SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1),");
            ///////////////
            for (int i = 0; i < table01.Rows.Count; i++)
            {
                str.AppendFormat(" '{0}', COSTS,", table01.Rows[i]["ITEM_CODE"].ToString());
            }
            ////////////////
            str.AppendFormat("0)), 2) \"医疗全成本\"");
            for (int i = 0; i < table2.Rows.Count; i++)
            {

                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1), '{0}', COSTS, 0)),2) \"{1}\"", table2.Rows[i]["ITEM_CODE"].ToString(), table2.Rows[i]["ITEM_NAME"].ToString());

            }
            str.AppendFormat(",ROUND (SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1),");
            for (int i = 0; i < tableAll.Rows.Count; i++)
            {

                str.AppendFormat("  '{0}', COSTS,", tableAll.Rows[i]["ITEM_CODE"].ToString());

            }
            str.AppendFormat(" 0)), 2) \"医院全成本\"");
            str.AppendFormat(@" from (SELECT detail.*, dept.account_dept_code, dept.account_dept_name,dt.XYHS_DEPT_TYPE,dt.id
          FROM {0}.xyhs_dept_costs_detail detail, {0}.xyhs_dept_dict dept,CBHS.XYHS_DEPT_TYPE_DICT dt
         WHERE detail.dept_code = dept.dept_code  and detail.GET_TYPE=0 AND dept.DEPT_TYPE = dt.id", DataUser.CBHS);
            if (depttype != "")
            {
                str.AppendFormat(" and dept.dept_type={0}", depttype);
            }
            str.AppendFormat(@" ) a,{2}.xyhs_cost_item_dict c,
            {2}.xyhs_costs_dict d WHERE a.get_type = '0' AND a.item_code = c.item_code
       AND c.item_type = d.item_code and a.DATE_TIME >= DATE '{0}' AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.CBHS);


            str.AppendFormat(" group by ROLLUP (id,XYHS_DEPT_TYPE, a.ACCOUNT_DEPT_CODE) ");

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
            //return OracleBase.Query(str.ToString()).Tables[0];
        }
        public DataTable GetXyhsCostdict()
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.xyhs_costs_dict where CLASSPID='0'", DataUser.CBHS);
            str1.Append(" order by item_code");
            return OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
        }
        public DataTable GetXyhsCostdict(string wheres)
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.xyhs_costs_dict where CLASSPID='0' {1}", DataUser.CBHS, wheres);
            str1.Append(" order by item_code");
            return OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
        }
        /// <summary>
        /// 成本医02
        /// </summary>
        /// <param name="depttype"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost02(string depttype, string begindate, string enddate)
        {

            DataTable table = GetXyhsCostdictSFYL();

            StringBuilder str = new StringBuilder();
            str.AppendFormat("select decode(a.account_dept_code,'','合计',min(a.account_dept_name))  \"A科室名称\"");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1)||a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", table.Rows[i]["ITEM_CODE"].ToString() + "0", table.Rows[i]["ITEM_CODE"].ToString() + "直接成本");
                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1)||a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", table.Rows[i]["ITEM_CODE"].ToString() + "1", table.Rows[i]["ITEM_CODE"].ToString() + "间接成本");
                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1), '{0}', COSTS, 0)),2) \"{1}\"", table.Rows[i]["ITEM_CODE"].ToString(), table.Rows[i]["ITEM_CODE"].ToString() + "全成本");
            }
            str.AppendFormat(" ,round(SUM (DECODE (a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", "0", "Z直接成本");
            str.AppendFormat(" ,round(SUM (DECODE (a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", "1", "Z间接成本");
            str.AppendFormat(" ,round(SUM (NVL ( COSTS, 0)),2) \"{0}\"", "Z全成本");

            str.AppendFormat(@" from (SELECT detail.*, dept.account_dept_code, dept.account_dept_name
          FROM {0}.xyhs_dept_costs_detail detail, {0}.xyhs_dept_dict dept
         WHERE detail.dept_code = dept.dept_code", DataUser.CBHS);
            if (depttype != "")
            {
                str.AppendFormat(" and dept.dept_type={0}", depttype);
            }
            str.AppendFormat(@" ) a,{2}.xyhs_cost_item_dict c,
       (select * from {2}.xyhs_costs_dict where SFYL is null or  SFYL='0') d WHERE  a.item_code = c.item_code
       AND c.item_type = d.item_code and a.DATE_TIME >= DATE '{0}' AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.CBHS);

            str.AppendFormat(" group by cube (a.ACCOUNT_DEPT_CODE)");
            str.AppendFormat(" order by a.account_dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        /// <summary>
        /// 成本医02
        /// </summary>
        /// <param name="depttype"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost02_1(string depttype, string begindate, string enddate)
        {

            DataTable table1 = GetXyhsCostdictSFYL1();
            DataTable table2 = GetXyhsCostdictSFYL2();
            DataTable table = GetXyhsCostdictSFYL();
            DataTable table01 = GetXyhsCostdictSFYL01();

            StringBuilder str = new StringBuilder();
            str.AppendFormat("select decode(a.account_dept_code,'','合计',min(a.account_dept_name))  \"A科室名称\"");
            //医疗成本合计 

            str.AppendFormat(" ,ROUND ( SUM( DECODE ( SUBSTR (c.ITEM_CODE, 0, 1) || a.get_type,");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat("  '{0}0', COSTS,", table.Rows[i]["ITEM_CODE"].ToString());
            }
            str.AppendFormat("0) ) , 2 ) \"X直接成本\"");
            str.AppendFormat(" ,ROUND ( SUM( DECODE ( SUBSTR (c.ITEM_CODE, 0, 1) || a.get_type,");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat("  '{0}1', COSTS,", table.Rows[i]["ITEM_CODE"].ToString());
            }
            str.AppendFormat("0) ) , 2 ) \"X间接成本\"");
            str.AppendFormat(" ,ROUND ( SUM ( DECODE ( SUBSTR (c.ITEM_CODE, 0, 1),");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat("  '{0}', COSTS,", table.Rows[i]["ITEM_CODE"].ToString());
            }
            str.AppendFormat("0) ) , 2) \"X合计\"");

            for (int i = 0; i < table1.Rows.Count; i++)
            {
                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1)||a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", table1.Rows[i]["ITEM_CODE"].ToString() + "0", table1.Rows[i]["ITEM_CODE"].ToString() + "直接成本");
                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1)||a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", table1.Rows[i]["ITEM_CODE"].ToString() + "1", table1.Rows[i]["ITEM_CODE"].ToString() + "间接成本");
                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1), '{0}', COSTS, 0)),2) \"{1}\"", table1.Rows[i]["ITEM_CODE"].ToString(), table1.Rows[i]["ITEM_CODE"].ToString() + "全成本");
            }
            str.AppendFormat(" ,ROUND ( SUM( DECODE ( SUBSTR (c.ITEM_CODE, 0, 1) || a.get_type,");
            for (int i = 0; i < table01.Rows.Count; i++)
            {
                str.AppendFormat("  '{0}0', COSTS,", table01.Rows[i]["ITEM_CODE"].ToString());
            }
            str.AppendFormat("0) ) , 2 ) \"Y直接成本\"");
            str.AppendFormat(" ,ROUND ( SUM( DECODE ( SUBSTR (c.ITEM_CODE, 0, 1) || a.get_type,");
            for (int i = 0; i < table01.Rows.Count; i++)
            {
                str.AppendFormat("  '{0}1', COSTS,", table01.Rows[i]["ITEM_CODE"].ToString());
            }
            str.AppendFormat("0) ) , 2 ) \"Y间接成本\"");
            str.AppendFormat(" ,ROUND ( SUM ( DECODE ( SUBSTR (c.ITEM_CODE, 0, 1),");
            for (int i = 0; i < table01.Rows.Count; i++)
            {
                str.AppendFormat("  '{0}', COSTS,", table01.Rows[i]["ITEM_CODE"].ToString());
            }
            str.AppendFormat("0) ) , 2) \"Y合计\"");

            for (int i = 0; i < table2.Rows.Count; i++)
            {
                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1)||a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", table2.Rows[i]["ITEM_CODE"].ToString() + "0", table2.Rows[i]["ITEM_CODE"].ToString() + "直接成本");
                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1)||a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", table2.Rows[i]["ITEM_CODE"].ToString() + "1", table2.Rows[i]["ITEM_CODE"].ToString() + "间接成本");
                str.AppendFormat(" ,round(SUM (DECODE (SUBSTR (c.ITEM_CODE, 0, 1), '{0}', COSTS, 0)),2) \"{1}\"", table2.Rows[i]["ITEM_CODE"].ToString(), table2.Rows[i]["ITEM_CODE"].ToString() + "全成本");
            }

            str.AppendFormat(" ,round(SUM (DECODE (a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", "0", "Z直接成本");
            str.AppendFormat(" ,round(SUM (DECODE (a.get_type, '{0}', COSTS, 0)),2) \"{1}\"", "1", "Z间接成本");
            str.AppendFormat(" ,round(SUM (NVL ( COSTS, 0)),2) \"{0}\"", "Z全成本");

            str.AppendFormat(@" from (SELECT detail.*, dept.account_dept_code, dept.account_dept_name
          FROM {0}.xyhs_dept_costs_detail detail, {0}.xyhs_dept_dict dept
         WHERE detail.dept_code = dept.dept_code", DataUser.CBHS);
            if (depttype != "")
            {
                str.AppendFormat(" and dept.dept_type={0}", depttype);
            }
            str.AppendFormat(@" ) a,{2}.xyhs_cost_item_dict c,
       {2}.xyhs_costs_dict d WHERE  a.item_code = c.item_code
       AND c.item_type = d.item_code and a.DATE_TIME >= DATE '{0}' AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.CBHS);

            str.AppendFormat(" group by cube (a.ACCOUNT_DEPT_CODE)");
            str.AppendFormat(" order by a.account_dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        public DataTable GetXyhsDept(string depttype)
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.XYHS_DEPT_DICT where attr='是' and show_flag='0'", DataUser.CBHS);
            if (depttype != "")
            {
                str1.AppendFormat(" and dept_type={0}", depttype);
            }
            str1.Append(" order by SORT_NO,dept_code");
            return OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
        }
        /// <summary>
        /// 成本医03
        /// </summary>
        /// <param name="depttype"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost03(string depttype, string begindate, string enddate)
        {
            DataTable table = GetXyhsDept("0");
            //DataTable table = GetXyhsCostdict(wheres);

            StringBuilder str = new StringBuilder();

            StringBuilder str2 = new StringBuilder();
            str2.AppendFormat(@"select a.ACCOUNT_DEPT_CODE, sum(costs)costs from (SELECT detail.*, dept.account_dept_code, dept.account_dept_name
                  FROM CBHS.xyhs_dept_costs_detail detail, CBHS.xyhs_dept_dict dept
                     WHERE detail.dept_code = dept.dept_code  and dept.dept_type=0 ) a,CBHS.xyhs_cost_item_dict c,
                   (select ITEM_CODE,ITEM_name from CBHS.xyhs_costs_dict where CLASSPID='0' and (SFYL is null or  SFYL='0')) d 
               WHERE  a.item_code = c.item_code
                      AND SUBSTR (c.item_code, 0, 1) = d.item_code 
                      and a.DATE_TIME >= DATE '{0}' 
                      AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1)                  
               group by a.ACCOUNT_DEPT_CODE", begindate, enddate);

            DataTable dtt = OracleOledbBase.ExecuteDataSet(str2.ToString()).Tables[0];

            str.Append("select * from (");

            str.AppendFormat("select decode(SUBSTR (d.ITEM_CODE, 0, 1),'','医疗成本合计',min(d.item_name))  \"90项目名称\"");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,trunc(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', COSTS, 0)),2) \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "金额");
                //str.AppendFormat(" ,decode (SUM (nvl (costs, 0)),0,0,round(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', costs, 0))/SUM (nvl (costs, 0)),6))*100 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                if (dtt.Rows.Count < 1)
                {
                    str.AppendFormat(" ,0 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                }
                else
                {
                    bool flag = false;
                    foreach (DataRow dr in dtt.Rows)
                    {
                        if (table.Rows[i]["DEPT_CODE"].ToString() == dr[0].ToString())
                        {
                            flag = true;
                            if (dr[1].ToString() != "0")
                                str.AppendFormat(" ,trunc(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', costs, 0))/{1},6)*100 \"{2}\"", table.Rows[i]["DEPT_CODE"].ToString(), double.Parse(dr[1].ToString()), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                            else
                                str.AppendFormat(" ,0 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                            break;
                        }
                    }
                    if (flag == false)
                    {
                        str.AppendFormat(" ,0 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                    }
                }
            }
            str.AppendFormat(" ,round(SUM (nvl (COSTS, 0)),2) \"{0}\"", "aa医疗成本合计");
            str.AppendFormat(" ,100 \"{0}\"", "bb%");

            str.AppendFormat(@" from (SELECT detail.*, dept.account_dept_code, dept.account_dept_name
          FROM {0}.xyhs_dept_costs_detail detail, {0}.xyhs_dept_dict dept
         WHERE detail.dept_code = dept.dept_code ", DataUser.CBHS);
            if (depttype != "")
            {
                str.AppendFormat(" and dept.dept_type={0}", depttype);
            }
            str.AppendFormat(@" ) a,{2}.xyhs_cost_item_dict c,
       (select ITEM_CODE,ITEM_name from {2}.xyhs_costs_dict where CLASSPID='0' and (SFYL is null or  SFYL='0')) d WHERE  a.item_code = c.item_code
       AND SUBSTR (c.item_code, 0, 1) = d.item_code and a.DATE_TIME >= DATE '{0}' AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.CBHS);
            str.AppendFormat(" group by cube (d.ITEM_CODE)");
            str.Append(" order by d.ITEM_CODE )");
            str.AppendFormat(" union all");
            ////指标部分
            str.AppendFormat(" select  substr(flags,3)flags");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,\"{0}\"", (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "金额");
                str.AppendFormat(" ,\"{0}\"", (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "null");
            }
            str.AppendFormat(" ,\"aa医疗成本合计\",100");
            str.AppendFormat("  from (select flags ");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,round(SUM (DECODE (a.DEPT_CODE, '{0}', incomes, 0)),2) \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "金额");
                str.AppendFormat(" ,null as \"{0}\"", (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "null");
            }
            str.AppendFormat(" ,ROUND (SUM ( incomes), 2) AS \"aa医疗成本合计\",null");

            str.AppendFormat(@" from (
                                  SELECT   DEPT_CODE,
           SUM (INCOMES) AS INCOMES,
           FLAGS,
           DATE_TIME
    FROM   (  
SELECT   a.*
  FROM   (  SELECT   B.ACCOUNT_DEPT_CODE dept_code,
                     SUM (TOTAL_COST) incomes,
                     '1_科室收入' FLAGS,
                     TO_CHAR (st_date, 'yyyyMM') AS DATE_TIME
              FROM   hisfact.dept_income a, CBHS.XYHS_DEPT_DICT b
             WHERE       st_date >= DATE '{0}'
                     AND st_date < ADD_MONTHS (DATE '{1}', 1)
                     AND a.dept_code = b.dept_code
          GROUP BY   TO_CHAR (st_date, 'yyyyMM'), B.ACCOUNT_DEPT_CODE) a,
         CBHS.XYHS_DEPT_DICT b
 WHERE       a.dept_code = b.dept_code
         AND attr = '是'
         AND show_flag = '0'
         AND dept_type = 0
            UNION ALL
              SELECT   B.ACCOUNT_DEPT_CODE dept_code,
                       SUM (INCOMES_DIFFERENCE) incomes,
                       '1_科室收入' FLAGS,
                       TO_CHAR (TO_DATE (st_date, 'yyyy-mm-dd'), 'yyyyMM')
                          AS DATE_TIME
                FROM   cbhs.XYHS_INCOMES_ADJUST a, cbhs.XYHS_DEPT_DICT b
               WHERE   TO_DATE (st_date, 'yyyy-mm-dd') >= DATE '{0}'
                       AND TO_DATE (st_date, 'yyyy-mm-dd') <
                             ADD_MONTHS (DATE '{1}', 1)
                       AND a.dept_code = b.dept_code
            GROUP BY   TO_CHAR (TO_DATE (st_date, 'yyyy-mm-dd'), 'yyyyMM'),
                       B.ACCOUNT_DEPT_CODE)
GROUP BY   DEPT_CODE, FLAGS, DATE_TIME
                                UNION ALL
   
                                select b.ACCOUNT_DEPT_CODE DEPT_CODE,nvl(tatal_cost,0)-nvl(b.costs,0),'2_收入-支出' FLAGS, b.st_date  DATE_TIME 
                                        from 
                                            (
                                               SELECT   DEPT_CODE as ACCOUNT_DEPT_CODE,
           SUM (INCOMES) AS tatal_cost,
           FLAGS,
           DATE_TIME as st_date
    FROM   (  SELECT   a.*
  FROM   (  SELECT   B.ACCOUNT_DEPT_CODE dept_code,
                     SUM (TOTAL_COST) incomes,
                     '1_科室收入' FLAGS,
                     TO_CHAR (st_date, 'yyyyMM') AS DATE_TIME
              FROM   hisfact.dept_income a, CBHS.XYHS_DEPT_DICT b
             WHERE       st_date >= DATE '{0}'
                     AND st_date < ADD_MONTHS (DATE '{1}', 1)
                     AND a.dept_code = b.dept_code
          GROUP BY   TO_CHAR (st_date, 'yyyyMM'), B.ACCOUNT_DEPT_CODE) a,
         CBHS.XYHS_DEPT_DICT b
 WHERE       a.dept_code = b.dept_code
         AND attr = '是'
         AND show_flag = '0'
         AND dept_type = 0
            UNION ALL
              SELECT   B.ACCOUNT_DEPT_CODE dept_code,
                       SUM (INCOMES_DIFFERENCE) incomes,
                       '1_科室收入' FLAGS,
                       TO_CHAR (TO_DATE (st_date, 'yyyy-mm-dd'), 'yyyyMM')
                          AS DATE_TIME
                FROM   cbhs.XYHS_INCOMES_ADJUST a, cbhs.XYHS_DEPT_DICT b
               WHERE   TO_DATE (st_date, 'yyyy-mm-dd') >= DATE '{0}'
                       AND TO_DATE (st_date, 'yyyy-mm-dd') <
                             ADD_MONTHS (DATE '{1}', 1)
                       AND a.dept_code = b.dept_code
            GROUP BY   TO_CHAR (TO_DATE (st_date, 'yyyy-mm-dd'), 'yyyyMM'),
                       B.ACCOUNT_DEPT_CODE)
GROUP BY   DEPT_CODE, FLAGS, DATE_TIME
                                            ) a, 
                                            (
                                                  SELECT   TO_CHAR (DATE_TIME, 'yyyyMM') st_date,
                                                           a.ACCOUNT_DEPT_CODE,
                                                           SUM (COSTS) COSTS
                                                    FROM   (SELECT   detail.*, dept.account_dept_code, dept.account_dept_name
                                                              FROM   CBHS.xyhs_dept_costs_detail detail,
                                                                     CBHS.xyhs_dept_dict dept
                                                             WHERE   detail.dept_code = dept.dept_code AND dept.dept_type = 0)
                                                           a,
                                                           CBHS.xyhs_cost_item_dict c,
                                                           (SELECT   ITEM_CODE, ITEM_name
                                                              FROM   CBHS.xyhs_costs_dict
                                                             WHERE   CLASSPID = '0' AND (SFYL IS NULL OR SFYL = '0')) d
                                                   WHERE       a.item_code = c.item_code
                                                           AND SUBSTR (c.item_code, 0, 1) = d.item_code
                                                           AND a.DATE_TIME >= DATE '{0}'
                                                           AND a.DATE_TIME < ADD_MONTHS (DATE '{1}', 1)
                                                GROUP BY   TO_CHAR (DATE_TIME, 'yyyyMM'), ACCOUNT_DEPT_CODE
                                            )  b 
                                        where  A.ACCOUNT_DEPT_CODE(+)=b.ACCOUNT_DEPT_CODE and a.st_date(+)=b.st_date 
                                UNION ALL

                                 select B.ACCOUNT_DEPT_CODE as dept_code,decode(zcrs,0,0,costs/zcrs) crzgl,'3_床日成本' FLAGS,a.st_date as DATE_TIME  from
                                     (
                                        select to_char(DATE_TIME,'yyyyMM') st_date,B.ACCOUNT_DEPT_CODE,sum(COSTS)COSTS 
                                            from cbhs.XYHS_DEPT_COSTS_DETAIL a,(select * from cbhs.XYHS_DEPT_DICT where OUT_OR_IN=2) b,
                                            (select ITEM_CODE as code from cbhs.xyhs_costs_dict where  SFYL='0') c 
                                            where DATE_TIME>=DATE'{0}' and DATE_TIME<ADD_MONTHS(DATE '{1}',1)
                                               and a.dept_code=b.dept_code  and substr(item_code,0,1) not in c.code                         
                                              group by to_char(DATE_TIME,'yyyyMM'),B.ACCOUNT_DEPT_CODE 
                                     )a,
                                     (
                                        SELECT to_char(a.st_date,'yyyyMM')st_date,ACCOUNT_DEPT_CODE, sum(ZCRS)ZCRS 
                                                FROM hisfact.dept_in_hospital_days a,cbhs.XYHS_DEPT_DICT b
                                                where a.st_date>=DATE'{0}' AND A.ST_DATE<ADD_MONTHS(DATE '{1}',1)
                                                      and a.dept_code=b.dept_code
                                                group by to_char(a.st_date,'yyyyMM'),B.ACCOUNT_DEPT_CODE
                                     )b
                                     where a.ACCOUNT_DEPT_CODE=b.ACCOUNT_DEPT_CODE and a.st_date=b.st_date(+)
                                UNION ALL

                                   select B.ACCOUNT_DEPT_CODE dept_code,decode(mzrc,0,0,COSTS/mzrc),'4_诊次成本' FLAGS,a.st_date  DATE_TIME 
                                        from
                                        (
                                            select to_char(DATE_TIME,'yyyyMM') st_date,B.ACCOUNT_DEPT_CODE,sum(COSTS)COSTS 
                                                from cbhs.XYHS_DEPT_COSTS_DETAIL a,(select * from cbhs.XYHS_DEPT_DICT where   OUT_OR_IN=1) b,
                                                (select ITEM_CODE as code from cbhs.xyhs_costs_dict where  SFYL='0') c 
                                                where DATE_TIME>=DATE'{0}' and DATE_TIME<ADD_MONTHS(DATE '{1}',1)
                                                   and a.dept_code=b.dept_code and substr(item_code,0,1) not in c.code                          
                                                  group by to_char(DATE_TIME,'yyyyMM'),B.ACCOUNT_DEPT_CODE
                                        )a,                        
                                        (
                                            SELECT  to_char(D.VISIT_DATE,'yyyyMM')ST_DATE,c.ACCOUNT_DEPT_CODE,sum(1)MZRC               
                                                    FROM HISDATA.clinic_master D,
                                                        (select * from cbhs.XYHS_DEPT_DICT where   OUT_OR_IN=1) c 
                                                    where VISIT_DATE>=DATE'{0}' and VISIT_DATE<ADD_MONTHS(DATE '{1}',1)
                                                          and RETURNED_OPERATOR is null
                                                          and D.VISIT_DEPT=c.dept_code
                                                         group by to_char(D.VISIT_DATE,'yyyyMM'),c.ACCOUNT_DEPT_CODE
                                        )b
                                        where a.ACCOUNT_DEPT_CODE=b.ACCOUNT_DEPT_CODE and a.st_date=b.st_date(+)
                                ) a
                                where to_date(DATE_TIME,'yyyyMM') >= DATE '{0}' AND to_date(DATE_TIME,'yyyyMM') < ADD_MONTHS(DATE '{1}',1)  
                            group by flags order by flags) ", begindate, enddate);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        /// <summary>
        /// 成本医03
        /// </summary>
        /// <param name="depttype"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost03_1(string depttype, string begindate, string enddate)
        {
            DataTable table = GetXyhsDept("0");

            StringBuilder str = new StringBuilder();

            StringBuilder str2 = new StringBuilder();
            str2.AppendFormat(@"select a.ACCOUNT_DEPT_CODE, sum(costs)costs from (SELECT detail.*, dept.account_dept_code, dept.account_dept_name
                  FROM CBHS.xyhs_dept_costs_detail detail, CBHS.xyhs_dept_dict dept
                     WHERE detail.dept_code = dept.dept_code  and dept.dept_type=0 ) a,CBHS.xyhs_cost_item_dict c,
                   (select ITEM_CODE,ITEM_name from CBHS.xyhs_costs_dict where CLASSPID='0' and (SFYL is null or  SFYL='0')) d 
               WHERE  a.item_code = c.item_code
                      AND SUBSTR (c.item_code, 0, 1) = d.item_code 
                      and a.DATE_TIME >= DATE '{0}' 
                      AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1)                  
               group by a.ACCOUNT_DEPT_CODE", begindate, enddate);
            DataTable dtt = OracleOledbBase.ExecuteDataSet(str2.ToString()).Tables[0];
            /////医疗成本
            str.Append("select * from (");

            str.AppendFormat("select decode(SUBSTR (d.ITEM_CODE, 0, 1),'','医疗成本合计',min(d.item_name))  \"90项目名称\"");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,trunc(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', COSTS, 0)),2) \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "金额");
                //str.AppendFormat(" ,decode (SUM (nvl (costs, 0)),0,0,round(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', costs, 0))/SUM (nvl (costs, 0)),6))*100 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                if (dtt.Rows.Count < 1)
                {
                    str.AppendFormat(" ,0 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                }
                else
                {
                    bool flag = false;
                    foreach (DataRow dr in dtt.Rows)
                    {
                        if (table.Rows[i]["DEPT_CODE"].ToString() == dr[0].ToString())
                        {
                            flag = true;
                            if (dr[1].ToString() != "0")
                                str.AppendFormat(" ,trunc(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', costs, 0))/{1},6)*100 \"{2}\"", table.Rows[i]["DEPT_CODE"].ToString(), double.Parse(dr[1].ToString()), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                            else
                                str.AppendFormat(" ,0 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                            break;
                        }
                    }
                    if (flag == false)
                    {
                        str.AppendFormat(" ,0 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                    }
                }
            }
            str.AppendFormat(" ,round(SUM (nvl (COSTS, 0)),2) \"{0}\"", "aa医疗成本合计");
            str.AppendFormat(" ,100 \"{0}\"", "bb%");

            str.AppendFormat(@" from (SELECT detail.*, dept.account_dept_code, dept.account_dept_name
          FROM {0}.xyhs_dept_costs_detail detail, {0}.xyhs_dept_dict dept
         WHERE detail.dept_code = dept.dept_code ", DataUser.CBHS);
            if (depttype != "")
            {
                str.AppendFormat(" and dept.dept_type={0}", depttype);
            }
            str.AppendFormat(@" ) a,{2}.xyhs_cost_item_dict c,
       (select ITEM_CODE,ITEM_name from {2}.xyhs_costs_dict where CLASSPID='0'and (SFYL is null or  SFYL='0')) d WHERE  a.item_code = c.item_code
       AND SUBSTR (c.item_code, 0, 1) = d.item_code and a.DATE_TIME >= DATE '{0}' AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.CBHS);
            str.AppendFormat(" group by cube (d.ITEM_CODE)");
            str.Append(" order by d.ITEM_CODE )");
            ///政府补助成本
            str.Append(" union all select * from (");
            str.AppendFormat("select decode(SUBSTR (d.ITEM_CODE, 0, 1),'','医疗成本合计',min(d.item_name))  \"90项目名称\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,round(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', COSTS, 0)),2) \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "金额");
                //str.AppendFormat(" ,decode (SUM (nvl (costs, 0)),0,0,round(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', costs, 0))/SUM (nvl (costs, 0)),6))*100 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
                str.AppendFormat(" ,0 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
            }
            str.AppendFormat(" ,round(SUM (nvl (COSTS, 0)),2) \"{0}\"", "aa医疗成本合计");
            str.AppendFormat(" ,100 \"{0}\"", "bb%");

            str.AppendFormat(@" from (SELECT detail.*, dept.account_dept_code, dept.account_dept_name
          FROM {0}.xyhs_dept_costs_detail detail, {0}.xyhs_dept_dict dept
         WHERE detail.dept_code = dept.dept_code ", DataUser.CBHS);
            if (depttype != "")
            {
                str.AppendFormat(" and dept.dept_type={0}", depttype);
            }
            str.AppendFormat(@" ) a,{2}.xyhs_cost_item_dict c,
       (select ITEM_CODE,ITEM_name from {2}.xyhs_costs_dict where CLASSPID='0' and SFYL is null or  SFYL='1') d WHERE  a.item_code = c.item_code
       AND SUBSTR (c.item_code, 0, 1) = d.item_code and a.DATE_TIME >= DATE '{0}' AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.CBHS);
            str.AppendFormat(" group by d.ITEM_CODE ");
            str.Append(" order by d.ITEM_CODE )");
            ///全成本
            str.Append(" union all select * from (");
            str.AppendFormat("select decode(SUBSTR (d.ITEM_CODE, 0, 1),'','医疗全成本合计',min(d.item_name))  \"90项目名称\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,trunc(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', COSTS, 0)),2) \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "金额");
                str.AppendFormat(" ,decode (SUM (nvl (costs, 0)),0,0,round(SUM (DECODE (a.ACCOUNT_DEPT_CODE, '{0}', costs, 0))/SUM (nvl (costs, 0)),6))*100 \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "%");
            }
            str.AppendFormat(" ,trunc(SUM (nvl (COSTS, 0)),2) \"{0}\"", "aa医疗成本合计");
            str.AppendFormat(" ,100 \"{0}\"", "bb%");

            str.AppendFormat(@" from (SELECT detail.*, dept.account_dept_code, dept.account_dept_name
          FROM {0}.xyhs_dept_costs_detail detail, {0}.xyhs_dept_dict dept
         WHERE detail.dept_code = dept.dept_code ", DataUser.CBHS);
            if (depttype != "")
            {
                str.AppendFormat(" and dept.dept_type={0}", depttype);
            }
            str.AppendFormat(@" ) a,{2}.xyhs_cost_item_dict c,
       (select ITEM_CODE,ITEM_name from {2}.xyhs_costs_dict where CLASSPID='0') d WHERE  a.item_code = c.item_code
       AND SUBSTR (c.item_code, 0, 1) = d.item_code and a.DATE_TIME >= DATE '{0}' AND a.DATE_TIME < ADD_MONTHS(DATE '{1}',1) ", begindate, enddate, DataUser.CBHS);
            str.AppendFormat(" group by cube (d.ITEM_CODE) ");
            str.Append(" order by d.ITEM_CODE ) where \"90项目名称\"='医疗全成本合计'");
            ///科室收入
            str.AppendFormat(" union all");
            str.AppendFormat(" select  substr(flags,3)flags");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,\"{0}\"", (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "金额");
                str.AppendFormat(" ,\"{0}\"", (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "null");
            }
            str.AppendFormat(" ,\"aa医疗成本合计\",100");
            str.AppendFormat("  from (select flags ");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(" ,round(SUM (DECODE (a.DEPT_CODE, '{0}', incomes, 0)),2) \"{1}\"", table.Rows[i]["DEPT_CODE"].ToString(), (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "金额");
                str.AppendFormat(" ,null as \"{0}\"", (i < 10 ? ("0" + i.ToString()) : i.ToString()) + "null");
            }
            str.AppendFormat(" ,ROUND (SUM ( incomes), 2) AS \"aa医疗成本合计\",null");

            str.AppendFormat(@" from (
                                SELECT   DEPT_CODE,
           SUM (INCOMES) AS INCOMES,
           FLAGS,
           DATE_TIME
    FROM   (  
SELECT   a.*
  FROM   (  SELECT   B.ACCOUNT_DEPT_CODE dept_code,
                     SUM (TOTAL_COST) incomes,
                     '1_科室收入' FLAGS,
                     TO_CHAR (st_date, 'yyyyMM') AS DATE_TIME
              FROM   hisfact.dept_income a, CBHS.XYHS_DEPT_DICT b
             WHERE       st_date >= DATE '{0}'
                     AND st_date < ADD_MONTHS (DATE '{1}', 1)
                     AND a.dept_code = b.dept_code
          GROUP BY   TO_CHAR (st_date, 'yyyyMM'), B.ACCOUNT_DEPT_CODE) a,
         CBHS.XYHS_DEPT_DICT b
 WHERE       a.dept_code = b.dept_code
         AND attr = '是'
         AND show_flag = '0'
         AND dept_type = 0
            UNION ALL
              SELECT   B.ACCOUNT_DEPT_CODE dept_code,
                       SUM (INCOMES_DIFFERENCE) incomes,
                       '1_科室收入' FLAGS,
                       TO_CHAR (TO_DATE (st_date, 'yyyy-mm-dd'), 'yyyyMM')
                          AS DATE_TIME
                FROM   cbhs.XYHS_INCOMES_ADJUST a, cbhs.XYHS_DEPT_DICT b
               WHERE   TO_DATE (st_date, 'yyyy-mm-dd') >= DATE '{0}'
                       AND TO_DATE (st_date, 'yyyy-mm-dd') <
                             ADD_MONTHS (DATE '{1}', 1)
                       AND a.dept_code = b.dept_code
            GROUP BY   TO_CHAR (TO_DATE (st_date, 'yyyy-mm-dd'), 'yyyyMM'),
                       B.ACCOUNT_DEPT_CODE)
GROUP BY   DEPT_CODE, FLAGS, DATE_TIME
                                UNION ALL
                                --select dept_code,incomes-COSTS,'收入-成本' FLAGS,DATE_TIME from cbhs.XYHS_DEPT_INCOMES_COSTS 
                                select b.ACCOUNT_DEPT_CODE DEPT_CODE,nvl(tatal_cost,0)-nvl(b.costs,0),'2_收入-支出' FLAGS, b.st_date  DATE_TIME 
                                        from 
                                            (
                                               SELECT   DEPT_CODE as ACCOUNT_DEPT_CODE,
           SUM (INCOMES) AS tatal_cost,
           FLAGS,
           DATE_TIME as st_date
    FROM   (  
SELECT   a.*
  FROM   (  SELECT   B.ACCOUNT_DEPT_CODE dept_code,
                     SUM (TOTAL_COST) incomes,
                     '1_科室收入' FLAGS,
                     TO_CHAR (st_date, 'yyyyMM') AS DATE_TIME
              FROM   hisfact.dept_income a, CBHS.XYHS_DEPT_DICT b
             WHERE       st_date >= DATE '{0}'
                     AND st_date < ADD_MONTHS (DATE '{1}', 1)
                     AND a.dept_code = b.dept_code
          GROUP BY   TO_CHAR (st_date, 'yyyyMM'), B.ACCOUNT_DEPT_CODE) a,
         CBHS.XYHS_DEPT_DICT b
 WHERE       a.dept_code = b.dept_code
         AND attr = '是'
         AND show_flag = '0'
         AND dept_type = 0
            UNION ALL
              SELECT   B.ACCOUNT_DEPT_CODE dept_code,
                       SUM (INCOMES_DIFFERENCE) incomes,
                       '1_科室收入' FLAGS,
                       TO_CHAR (TO_DATE (st_date, 'yyyy-mm-dd'), 'yyyyMM')
                          AS DATE_TIME
                FROM   cbhs.XYHS_INCOMES_ADJUST a, cbhs.XYHS_DEPT_DICT b
               WHERE   TO_DATE (st_date, 'yyyy-mm-dd') >= DATE '{0}'
                       AND TO_DATE (st_date, 'yyyy-mm-dd') <
                             ADD_MONTHS (DATE '{1}', 1)
                       AND a.dept_code = b.dept_code
            GROUP BY   TO_CHAR (TO_DATE (st_date, 'yyyy-mm-dd'), 'yyyyMM'),
                       B.ACCOUNT_DEPT_CODE)
GROUP BY   DEPT_CODE, FLAGS, DATE_TIME
                                            ) a, 
                                            (
                                                select to_char(DATE_TIME,'yyyyMM') st_date,B.ACCOUNT_DEPT_CODE,sum(COSTS)COSTS 
                                                    from cbhs.XYHS_DEPT_COSTS_DETAIL a,(SELECT   *
              FROM   CBHS.XYHS_DEPT_DICT
             WHERE    show_flag = '0' AND dept_type = 0) b 
                                                    where DATE_TIME >=DATE'{0}'                   
                                                          and DATE_TIME  <ADD_MONTHS(DATE '{1}',1)
                                                          and a.dept_code=b.dept_code
                                                      group by to_char(DATE_TIME,'yyyyMM'),B.ACCOUNT_DEPT_CODE
                                            )  b 
                                        where  A.ACCOUNT_DEPT_CODE(+)=b.ACCOUNT_DEPT_CODE and a.st_date(+)=b.st_date
                                UNION ALL
                                --select dept_code,CRCB,'床日成本' FLAGS,DATE_TIME from cbhs.XYHS_DEPT_INCOMES_COSTS 
                                 select B.ACCOUNT_DEPT_CODE as dept_code,decode(zcrs,0,0,costs/zcrs) crzgl,'3_床日成本' FLAGS, a.st_date  as DATE_TIME  from
                                     (
                                        select to_char(DATE_TIME,'yyyyMM') st_date,B.ACCOUNT_DEPT_CODE,sum(COSTS)COSTS 
                                                from cbhs.XYHS_DEPT_COSTS_DETAIL a,(select * from cbhs.XYHS_DEPT_DICT where OUT_OR_IN=2) b
                                                where DATE_TIME>=DATE'{0}' and DATE_TIME<ADD_MONTHS(DATE '{1}',1)
                                                   and a.dept_code=b.dept_code                           
                                                  group by to_char(DATE_TIME,'yyyyMM'),B.ACCOUNT_DEPT_CODE
                                     )a,
                                     (
                                        SELECT to_char(a.st_date,'yyyyMM')st_date,ACCOUNT_DEPT_CODE, sum(ZCRS)ZCRS 
                                                FROM hisfact.dept_in_hospital_days a,cbhs.XYHS_DEPT_DICT b
                                                where a.st_date>=DATE'{0}' AND A.ST_DATE<ADD_MONTHS(DATE '{1}',1)
                                                      and a.dept_code=b.dept_code
                                                group by to_char(a.st_date,'yyyyMM'),B.ACCOUNT_DEPT_CODE
                                     )b
                                     where a.ACCOUNT_DEPT_CODE=b.ACCOUNT_DEPT_CODE and a.st_date=b.st_date(+)
                                UNION ALL
                                --select dept_code,ZCCB,'诊次成本' FLAGS,DATE_TIME from cbhs.XYHS_DEPT_INCOMES_COSTS
                                select B.ACCOUNT_DEPT_CODE dept_code,decode(mzrc,0,0,COSTS/mzrc),'4_诊次成本' FLAGS, a.st_date  DATE_TIME 
                                        from
                                        (
                                            select to_char(DATE_TIME,'yyyyMM') st_date,B.ACCOUNT_DEPT_CODE,sum(COSTS)COSTS 
                                                from cbhs.XYHS_DEPT_COSTS_DETAIL a,(select * from cbhs.XYHS_DEPT_DICT where   OUT_OR_IN=1) b 
                                                where DATE_TIME>=DATE'{0}' and DATE_TIME<ADD_MONTHS(DATE '{1}',1)
                                                   and a.dept_code=b.dept_code                           
                                                  group by to_char(DATE_TIME,'yyyyMM'),B.ACCOUNT_DEPT_CODE
                                        )a,                        
                                        (
                                            SELECT  to_char(D.VISIT_DATE,'yyyyMM')ST_DATE,c.ACCOUNT_DEPT_CODE,sum(1)MZRC               
                                                    FROM HISDATA.clinic_master D,
                                                        (select * from cbhs.XYHS_DEPT_DICT where   OUT_OR_IN=1) c 
                                                    where VISIT_DATE>=DATE'{0}' and VISIT_DATE<ADD_MONTHS(DATE '{1}',1)
                                                          and RETURNED_OPERATOR is null
                                                          and D.VISIT_DEPT=c.dept_code
                                                         group by to_char(D.VISIT_DATE,'yyyyMM'),c.ACCOUNT_DEPT_CODE
                                        )b
                                        where a.ACCOUNT_DEPT_CODE=b.ACCOUNT_DEPT_CODE and a.st_date=b.st_date(+)
                                ) a
                                where to_date(DATE_TIME,'yyyyMM') >= DATE '{0}' AND to_date(DATE_TIME,'yyyyMM') < ADD_MONTHS(DATE '{1}',1)  
                            group by flags order by flags )", begindate, enddate);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        public string GetItemCode(string item_type)
        {
            string sql = string.Format(@"select count(*)+1 num from cbhs.XYHS_COST_ITEM_DICT where item_type='{0}'", item_type);

            return OracleOledbBase.ExecuteScalar(sql).ToString();
        }

        //全成本设置
        public void SaveXyhsItem(string itemcode, string itemtype, string itemname, string finance_item, string finance_item_gl)
        {
            MyLists listtable = new MyLists();
            string strDel = string.Format("DELETE FROM {0}.XYHS_COST_ITEM_DICT WHERE item_code='{1}'", DataUser.CBHS, itemcode);
            List listDel = new List();
            listDel.StrSql = strDel;
            listtable.Add(listDel);

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT INTO {0}.XYHS_COST_ITEM_DICT 
                                          (item_code,item_name,item_type,finance_item,finance_item_gl)
                                        VALUES(?,?,?,?,?)", DataUser.CBHS);

            OleDbParameter[] parameteradd = {
                                              new OleDbParameter("item_code",itemcode),
                                              new OleDbParameter("item_name",itemname),
                                              new OleDbParameter("item_type",itemtype),
                                              new OleDbParameter("finance_item",finance_item),
											 new OleDbParameter("finance_item_gl",finance_item_gl)
                                          };
            List listAdd = new List();
            listAdd.StrSql = strSql;
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        public DataTable GetXyhsCostdictSFYL()
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.xyhs_costs_dict where CLASSPID='0' and (SFYL is null or SFYL='0')", DataUser.CBHS);
            str1.Append(" order by item_code");
            return OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
        }

        public DataTable GetXyhsCostdictSFYL01()
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.xyhs_costs_dict where CLASSPID='0' and (SFYL is null or SFYL='0' or SFYL='1')", DataUser.CBHS);
            str1.Append(" order by item_code");
            return OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
        }
        public DataTable GetXyhsCostdictSFYL1()
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.xyhs_costs_dict where CLASSPID='0' and  SFYL='1' ", DataUser.CBHS);
            str1.Append(" order by item_code");
            return OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
        }
        public DataTable GetXyhsCostdictSFYL2()
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat("select * from {0}.xyhs_costs_dict where CLASSPID='0' and  SFYL='2' ", DataUser.CBHS);
            str1.Append(" order by item_code");
            return OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
        }

        /// <summary>
        ///执行同步his科室存储过程
        /// </summary>
        /// <param name="accountdate"></param>
        /// <returns></returns>
        public string Exec_sync_his_Deal()
        {

            string ProcName = DataUser.CBHS + ".sp_XYHS_sync_his_dept";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter(),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }

        /// <summary>
        /// 保存物质数据
        /// </summary>
        /// <param name="dt"></param>
        public void SaveWuzhiZhijieChengben(DataTable dt)
        {
            MyLists listtable = new MyLists();
            foreach (DataRow dr in dt.Rows)
            {
                StringBuilder strDel = new StringBuilder();
                strDel.AppendFormat("DELETE FROM {0}.XYHS_DEPT_COST_DETAIL WHERE ACCOUNTING_DATE=to_date_('{1}','yyyy-MM-dd') AND item_code='{2}'  AND GET_TYPE='0'", DataUser.CBHS, dr["ST_DATE"].ToString(), dr["ITEM_CODE"].ToString());
                List listDel = new List();
                listDel.StrSql = strDel;
                listtable.Add(listDel);

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.XYHS_DEPT_COST_DETAIL 
                                          (DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,COSTS_ARMYFREE,
                                           OPERATOR,OPERATOR_DATE,GET_TYPE,COST_FLAG,BALANCE_TAG,DEPT_TYPE_FLAG,MEMO,PROG_CODE,MEDICINE_COSTS,MANAGE_COSTS)
                                        VALUES(?,?,?,?,?,?,SYSDATE,?,?,?,?,?,?,?,?)", DataUser.CBHS);


                OleDbParameter[] parameteradd = {
											  new OleDbParameter("dept_code",dr["DEPT_CODE"].ToString()),
											  new OleDbParameter("item_code",dr["ITEM_CODE"].ToString()),
											  new OleDbParameter("accounting_date",Convert.ToDateTime(dr["ST_DATE"].ToString())),
											  new OleDbParameter("costs",dr["costs"].ToString()),
											  new OleDbParameter("costs_armyfree",0),   
											  new OleDbParameter("OPERATOR",""),
											  new OleDbParameter("get_type","0"),//0为录入
											  new OleDbParameter("cost_flag",""),
											  new OleDbParameter("balance_tag",""),
											  new OleDbParameter("dept_type_flag",""),
											  new OleDbParameter("memo",""),
											  new OleDbParameter("PROG_CODE",""),
                                              new OleDbParameter("MEDICINE_COSTS",dr["costs"].ToString()),
											  new OleDbParameter("MANAGE_COSTS",0)
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        ///执行获取用友数据过程
        /// </summary>
        /// <param name="accountdate"></param>
        /// <returns></returns>
        public string Exec_YongYou_Cost_Deal(string accountdate)
        {

            string ProcName = DataUser.CBHS + ".SP_XYHS_GET_YongYou_COST";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", accountdate),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }

        /// <summary>
        /// 删除成本
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="gettype"></param>
        public int DelSingCoste(string accounting_date, string gettype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("DELETE FROM HISFACT.DEPT_TOTAL_COSTS WHERE ST_DATE=to_date('{0}','yyyy-MM-dd')  ", accounting_date);
            return OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }

        public string GetDept_code(string dept_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT X.DEPT_CODE FROM CBHS.XYHS_DEPT_DICT X where DEPT_NAME like '%{0}%' ", dept_name);
            DataTable dtt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            if (dtt.Rows.Count > 0)
                return dtt.Rows[0][0].ToString();
            return "";
        }
        public string GetItem_code(string st_date, string item_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select item_code from 
                                    (
                                    SELECT   a.item_code, c.ITEM_NAME||'：'||a.item_name as item_name 
                                        FROM  CBHS.XYHS_COST_ITEM_DICT a, CBHS.XYHS_DEPT_COST_DETAIL b,
                                               CBHS.XYHS_COSTS_DICT C
                                       WHERE   A.ITEM_CODE = b.ITEM_CODE(+)
                                               and a.item_type=c.ITEM_CODE
                                               AND B.ACCOUNTING_DATE(+) = date'{0}'   
                                    )
                                    where ITEM_NAME='{1}'", st_date, item_name);
            DataTable dtt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            if (dtt.Rows.Count > 0)
                return dtt.Rows[0][0].ToString();
            return "";
        }

        /// <summary>
        /// 导入成本项目
        /// </summary>
        /// <param name="dept_code"></param>
        /// <param name="item_code"></param>
        /// <param name="accounting_date"></param>
        /// <param name="gettype"></param>
        /// <param name="medicine_costs"></param>
        /// <param name="manage_costs"></param>
        public void SaveAllSingleCoste(string dept_code, string item_code, string accounting_date, string gettype, int medicine_costs, int manage_costs)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"insert into HISFACT.DEPT_TOTAL_COSTS(ST_DATE,ITEM_CODE,DEPT_CODE,COSTS) values(to_date('{0}','yyyy-MM-dd'),'{1}','{2}','{3}')",
                                accounting_date, item_code, dept_code, medicine_costs + manage_costs);

            //                strSql.AppendFormat(@"INSERT INTO {0}.XYHS_DEPT_COST_DETAIL 
            //                                          (DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,COSTS_ARMYFREE,
            //                                           OPERATOR_DATE,GET_TYPE,MEDICINE_COSTS,MANAGE_COSTS)
            //                                        VALUES('{1}','{2}',to_date('{3}','yyyy-MM-dd'),'{4}','{5}',SYSDATE,'{6}','{7}','{8}')", DataUser.CBHS, dept_code, 
            //                                                                                      item_code, accounting_date,
            //                                                                                      medicine_costs+manage_costs,0,gettype,medicine_costs,manage_costs);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());

        }



        #endregion 效益核算


        #region 成本录入(新添加-Sunyc)

        /// <summary>
        /// 获取成本科目
        /// </summary>
        /// <returns></returns>
        public DataTable CostItemFilter()
        {
            string sql = string.Format("select ITEM_NAME,ITEM_CODE from {0}.XYHS_COST_ITEM_DICT", DataUser.CBHS);

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 获取单项成本录入列表
        /// </summary>
        /// <param name="balance_tag">结算标识</param>
        /// <param name="item_code">成本代码</param>
        /// <param name="date_time">时间（201012）</param>
        /// <returns>DataSet</returns>
        public DataSet GetSingleCost(string item_code, string date_time, string gettype, string progcode)
        {
            progcode = progcode == "全部" ? "" : progcode;
            StringBuilder strSql = new StringBuilder();


            //            strSql.AppendFormat(@"SELECT DEPT_CODE, DECODE (DEPT_CODE,'', '合计',MIN (DEPT_NAME)) DEPT_NAME, SUM (TOTAL_COSTS) TOTAL_COSTS,
            //                                         SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,
            //                                         DECODE (DEPT_CODE, '', '', MIN (MEMO)) MEMO
            //                                    FROM (SELECT   B.DEPT_CODE DEPT_CODE, B.DEPT_NAME dept_name,
            //                                                   SUM (COSTS + COSTS_ARMYFREE) AS TOTAL_COSTS,
            //                                                   SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,MEMO,B.SORT_NO
            //                                              FROM {0}.XYHS_DEPT_COST_DETAIL A, {1}.XYHS_DEPT_DICT B
            //                                             WHERE A.DEPT_CODE(+) = B.DEPT_CODE
            //                                               and b.account_dept_code is not null
            //                                               AND GET_TYPE(+)='{2}'
            //                                               AND ITEM_CODE(+) = '{3}' and b.SHOW_FLAG='0' and B.ATTR='是'", DataUser.CBHS, DataUser.CBHS, gettype, item_code);
            //            if (!progcode.Equals(string.Empty))
            //            {
            //                strSql.AppendFormat(" and a.prog_code(+)='{0}'", progcode);
            //            }
            //            strSql.AppendFormat(@" AND ACCOUNTING_DATE(+) = date'{0}'
            //                                          GROUP BY B.DEPT_CODE, B.DEPT_NAME, MEMO,B.SORT_NO
            //                                          ORDER BY B.SORT_NO,B.DEPT_CODE)
            //                                GROUP BY CUBE (DEPT_CODE)
            //                                ORDER BY DECODE(DEPT_CODE,NULL,-1,MIN(SORT_NO)),DEPT_CODE", date_time);


            //盘锦中心医院用
            strSql.AppendFormat(@"SELECT    a.FINANCE_ITEM,a.FINANCE_ITEM_gl,a.item_code, c.ITEM_NAME||'：'||a.item_name as item_name, b.COSTS,B.MEMO
    FROM  {0}.XYHS_COST_ITEM_DICT a, {0}.XYHS_DEPT_COST_DETAIL b,
           {0}.XYHS_COSTS_DICT C
   WHERE   A.ITEM_CODE = b.ITEM_CODE(+)
           and a.item_type=c.ITEM_CODE
           AND B.ACCOUNTING_DATE(+) = date'{1}'           
ORDER BY   a.item_code", DataUser.CBHS, date_time);

            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }


        /// <summary>
        /// 保存界面成本到数据库
        /// </summary>
        /// <param name="costdetails">成本记录对象</param>
        /// <param name="item_code">成本项目代码</param>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="operators">操作员</param>
        public void SaveSingleCosts(List<GoldNet.Model.CostDetail> costdetails, string item_code, string date_time, string operators, string gettype, string fjfa)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.XYHS_DEPT_COST_DETAIL WHERE ACCOUNTING_DATE=date'{2}'  AND GET_TYPE='{1}'", DataUser.CBHS, gettype, date_time);
            if (!fjfa.Equals(string.Empty))
            {
                strDel.AppendFormat(" and PROG_CODE='{0}'", fjfa);
            }
            List listDel = new List();
            listDel.StrSql = strDel;
            //listDel.Parameters = new OleDbParameter[] { new OleDbParameter("item_code", item_code) };
            listtable.Add(listDel);

            foreach (GoldNet.Model.CostDetail costdetail in costdetails)
            {
             
                double costs_armyfree = Convert.ToDouble(0);

                double medicine_costs = Convert.ToDouble(costdetail.MEDICINE_COSTS == "" ? "0" : costdetail.MEDICINE_COSTS);
                double manage_costs = Convert.ToDouble(costdetail.MANAGE_COSTS == "" ? "0" : costdetail.MANAGE_COSTS);

                double costs = medicine_costs + manage_costs;
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.XYHS_DEPT_COST_DETAIL 
                                          (DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,COSTS_ARMYFREE,
                                           OPERATOR,OPERATOR_DATE,GET_TYPE,COST_FLAG,BALANCE_TAG,DEPT_TYPE_FLAG,MEMO,PROG_CODE,MEDICINE_COSTS,MANAGE_COSTS,FINANCE_ITEM,FINANCE_ITEM_GL)
                                        VALUES(?,?,?,?,?,?,SYSDATE,?,?,?,?,?,?,?,?,?,?)", DataUser.CBHS, date_time);


                OleDbParameter[] parameteradd = {
											  new OleDbParameter("dept_code",costdetail.Dept_code),
											  new OleDbParameter("item_code",costdetail.Item_code),
											  new OleDbParameter("accounting_date",Convert.ToDateTime(date_time)),
											  new OleDbParameter("costs",medicine_costs+manage_costs),
											  new OleDbParameter("costs_armyfree",costs_armyfree),
											  new OleDbParameter("OPERATOR",operators),
											  new OleDbParameter("get_type",gettype),//0为录入
											  new OleDbParameter("cost_flag",""),
											  new OleDbParameter("balance_tag",""),
											  new OleDbParameter("dept_type_flag",""),
											  new OleDbParameter("memo",costdetail.Memo),
											  new OleDbParameter("PROG_CODE",fjfa),
                                              new OleDbParameter("MEDICINE_COSTS",medicine_costs),
											  new OleDbParameter("MANAGE_COSTS",manage_costs),
                                              new OleDbParameter("FINANCE_ITEM",costdetail.FINANCE_ITEM ),
                                              new OleDbParameter("FINANCE_ITEM_GL",costdetail.FINANCE_ITEM_GL )
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// sql提取
        /// </summary>
        /// <param name="stdate"></param>
        public void getsqlcacost(string stdate, string username)
        {
            string yycostsconn = System.Configuration.ConfigurationSettings.AppSettings["costsql"];

            string strdept = string.Format("select SQLEXPRESS,FLAGS,date_name,GROUPBY from cbhs.CBHS_OTHER_SQLEXPRESS where id=1");
            DataTable tablesql = OracleOledbBase.ExecuteDataSet(strdept).Tables[0];
            //
            string stracccosts = System.Configuration.ConfigurationSettings.AppSettings["yyacccosts"];
            string notstraccosts = System.Configuration.ConfigurationSettings.AppSettings["notyyacccosts"];
            string sql = tablesql.Rows[0]["SQLEXPRESS"].ToString() + " " + tablesql.Rows[0]["FLAGS"].ToString() + " " + tablesql.Rows[0]["DATE_NAME"].ToString() + string.Format("={0}", stdate) + " " + tablesql.Rows[0]["GROUPBY"].ToString();
            System.Data.SqlClient.SqlParameter[] cmdParmsyy = new System.Data.SqlClient.SqlParameter[0] { };
            DataTable table = DALBase.ExecuteDataSet(yycostsconn, sql, cmdParmsyy).Tables[0];
            //DataTable table = OracleOledbBase.ExecuteDataSet(sqlselt.ToString()).Tables[0];
            /////////////////////
            string strdel = string.Format("delete {1}.XYHS_DEPT_COST_DETAIL where TO_CHAR (ACCOUNTING_DATE,'yyyymm')='{0}' and GET_TYPE='0'", stdate, DataUser.CBHS);
            OracleOledbBase.ExecuteNonQuery(strdel);
            //string strdate = string.Format("update cbhs.CBHS_DAY_DICT set DAY_NAME='{0}'", stdate);
            //OracleOledbBase.ExecuteNonQuery(strdate);
            MyLists listttrans = new MyLists();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                string str = string.Format("select HIS_DEPT_CODE from {0}.XYHS_DEPT_TO_HISDEPT where XYHS_DEPT_CODE='{1}'", DataUser.CBHS, table.Rows[i]["deptcode"].ToString().Trim());
                string deptcode = "0";
                object dept = OracleOledbBase.GetSingle(str);
                if (dept != null)

                    deptcode = dept.ToString();
                //string strcost = string.Format("select cost_item from {1}.CBHS_COSTS_VS_YYCOSTS where ccode='{0}'", table.Rows[i]["itemcode"].ToString().Trim(), DataUser.CBHS);
                //string itemcode = table.Rows[i]["itemcode"].ToString().Trim();
                //object item = OracleOledbBase.GetSingle(strcost);
                //if (item != null)
                //    itemcode = item.ToString();

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.XYHS_DEPT_COST_DETAIL 
                                          (DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,COSTS_ARMYFREE,
                                           OPERATOR,OPERATOR_DATE,GET_TYPE,COST_FLAG,BALANCE_TAG,DEPT_TYPE_FLAG,MEMO,PROG_CODE,YY_DEPT_CODE)
                                        VALUES(?,?,to_date('{1}','yyyy-mm-dd'),?,?,?,SYSDATE,?,?,?,?,?,?,?)", DataUser.CBHS, stdate + "01");


                OleDbParameter[] parameteradd = {
											  new OleDbParameter("dept_code",deptcode),
											  new OleDbParameter("item_code",table.Rows[i]["itemcode"].ToString().Trim()),
											  new OleDbParameter("costs", Convert.ToDouble( table.Rows[i]["costs"].ToString().Trim())),
											  new OleDbParameter("costs_armyfree","0"),
											  new OleDbParameter("OPERATOR",username),
											  new OleDbParameter("get_type","0"),//0为录入
											  new OleDbParameter("cost_flag",""),
											  new OleDbParameter("balance_tag",""),
											  new OleDbParameter("dept_type_flag",""),
											  new OleDbParameter("memo",""),
											  new OleDbParameter("PROG_CODE",""),
                                              new OleDbParameter("YY_DEPT_CODE",table.Rows[i]["deptcode"].ToString().Trim())
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listttrans.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="months"></param>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public bool GetIsSubmit(string months, string itemcode)
        {
            string str = string.Format("select * from {0}.XYHS_COSTS_SUBMIT where MONTHS=? AND ITEM_CODE=? ", DataUser.CBHS);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("MONTHS",months),
											  new OleDbParameter("ITEM_CODE",itemcode)  
										  };
            DataTable tb = OracleOledbBase.ExecuteDataSet(str.ToString(), parameteradd).Tables[0];
            if (tb.Rows.Count > 0)
            {
                return true;
            }
            else
                return false;

        }


        /// <summary>
        /// 提交成本
        /// </summary>
        /// <param name="month"></param>
        /// <param name="itemcode"></param>
        public void Submitcost(string months, string itemcode, string opertor)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.XYHS_COSTS_SUBMIT WHERE MONTHS=? AND ITEM_CODE=? ", DataUser.CBHS);

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { new OleDbParameter("", months), new OleDbParameter("", itemcode) };
            listtable.Add(listDel);
            //
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"insert into {0}.XYHS_COSTS_SUBMIT(MONTHS,ITEM_CODE,FLAGS,SUBMIT_PERSONS,SUBMIT_DATE) values (?,?,?,?,SYSDATE)", DataUser.CBHS);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("MONTHS",months),
											  new OleDbParameter("ITEM_CODE",itemcode),
											  new OleDbParameter("FLAGS",'1'),
											  new OleDbParameter("SUBMIT_PERSONS",opertor)
											  
										  };
            List listadd = new List();
            listadd.StrSql = str;
            listadd.Parameters = parameteradd;
            listtable.Add(listadd);
            OracleOledbBase.ExecuteTranslist(listtable);

        }


        /// <summary>
        /// 审核成本
        /// </summary>
        /// <param name="months"></param>
        /// <param name="itemcode"></param>
        /// <param name="opertor"></param>
        public void Checkcost(string months, string itemcode, string opertor)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"update {0}.XYHS_COSTS_SUBMIT set CHECK_FLAGS=?,CHECK_NAME=? where months=? and item_code=?", DataUser.CBHS);
            OleDbParameter[] parameteradd = {
                                              new OleDbParameter("CHECK_FLAGS",'1'),
											  new OleDbParameter("CHECK_NAME",opertor),
											  new OleDbParameter("MONTHS",months),
											  new OleDbParameter("ITEM_CODE",itemcode)  
										  };

            OracleOledbBase.ExecuteNonQuery(str.ToString(), parameteradd);

        }


        /// <summary>
        /// 成本取消
        /// </summary>
        /// <param name="months"></param>
        /// <param name="itemcode"></param>
        /// <param name="opertor"></param>
        public void Canclecost(string months, string itemcode, string opertor)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM {0}.XYHS_COSTS_SUBMIT WHERE MONTHS=? AND ITEM_CODE=? ", DataUser.CBHS);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("MONTHS",months),
											  new OleDbParameter("ITEM_CODE",itemcode)  
										  };

            OracleOledbBase.ExecuteNonQuery(str.ToString(), parameteradd);

        }

        /// <summary>
        /// 成本提交信息
        /// </summary>
        /// <param name="months"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetSubmitcost(string months, string userid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.item_code, a.item_name,decode(b.FLAGS,'','','已提交') flags,b.SUBMIT_PERSONS,b.CHECK_NAME,b.COMP_NAME from (
SELECT DISTINCT a.item_code, a.item_name
           FROM {0}.cbhs_cost_item_dict a,
                {1}.sys_power_detail b,
                {1}.sys_specpower_role c
          WHERE a.item_code = c.ID
            AND c.TYPE = {2}
            AND c.role_id = b.power_id
            AND UPPER (b.target_id) = '{3}'
       ORDER BY a.item_code) a left join 
       {0}.CBHS_COSTS_SUBMIT b on a.item_code=b.ITEM_CODE and b.MONTHS='{4}' order by item_code", DataUser.CBHS, DataUser.COMM, Constant.COST_SPEPOWER, userid, months);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 删除单行录入的成本项目
        /// </summary>
        /// <param name="date_time">时间（201012）</param>
        /// <param name="item_code">成本项目代码</param>
        /// <param name="dept_code">科室代码</param>
        public void DelSingleCosts(string date_time, string item_code, string dept_code, string gettype, string progcode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"DELETE FROM {0}.XYHS_DEPT_COST_DETAIL
                                  WHERE ACCOUNTING_DATE = date'{1}'
                                     and get_type='{2}'", DataUser.CBHS, date_time, gettype);
            if (dept_code != null && !dept_code.Equals(""))
            {
                strSql.AppendFormat(" AND ITEM_CODE = '{0}'", dept_code);
            }
            //if (progcode != "")
            //{
            //    strSql.AppendFormat(" and PROG_CODE='{0}'", progcode);
            //}
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());

        }

        //        //全成本设置
        public void SaveXyhsItem(string itemcode, string itemtype, string itemname, string finance_item)
        {
            MyLists listtable = new MyLists();
            string strDel = string.Format("DELETE FROM {0}.XYHS_COST_ITEM_DICT WHERE item_code='{1}'", DataUser.CBHS, itemcode);
            List listDel = new List();
            listDel.StrSql = strDel;
            listtable.Add(listDel);

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT INTO {0}.XYHS_COST_ITEM_DICT 
                                          (item_code,item_name,item_type,finance_item)
                                        VALUES(?,?,?,?)", DataUser.CBHS);

            OleDbParameter[] parameteradd = {
                                              new OleDbParameter("item_code",itemcode),
                                              new OleDbParameter("item_name",itemname),
                                              new OleDbParameter("item_type",itemtype),
                                              new OleDbParameter("finance_item",finance_item)
											 
                                          };
            List listAdd = new List();
            listAdd.StrSql = strSql;
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 获取科级核算毛收入
        /// </summary>
        /// <param name="st_date"></param>
        /// <returns></returns>
        public DataTable GetDeptIncomes(string st_date)
        {
            string sql = string.Format(@"SELECT   SUM (costs) costs
  FROM   (SELECT   SUM (costs) costs
            FROM   HISFACT.OUTP_CLASS2_INCOME
           WHERE   to_char(st_date,'yyyymm') = '{0}'
                   
          UNION ALL
          SELECT   SUM (costs) costs
            FROM   HISFACT.INP_CLASS2_INCOME
           WHERE   to_char(st_date,'yyyymm') = '{0}')", st_date);

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 添加全成本收入调整
        /// </summary>
        /// <param name="st_date"></param>
        /// <param name="incomes_adjust"></param>
        /// <param name="incmes_d"></param>
        /// <param name="dept_code"></param>
        /// <param name="dept_name"></param>
        public void AddXyhsIncomesAdjust(string st_date, string incomes_adjust, string incmes_d, string dept_code, string dept_name)
        {
            string sql = string.Format(@"INSERT INTO CBHS.XYHS_INCOMES_ADJUST (ST_DATE,
                                      DEPT_CODE,
                                      INCOMES_ADJUST,
                                      INCOMES_DIFFERENCE,
                                      DEPT_NAME,
                                      ID)
  VALUES('{0}','{1}',{2},{3},'{4}',{5})", st_date, dept_code, incomes_adjust, incmes_d, dept_name, OracleOledbBase.GetMaxID("id", "CBHS.XYHS_INCOMES_ADJUST"));

            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新全成本收入调整
        /// </summary>
        /// <param name="st_date"></param>
        /// <param name="incomes_adjust"></param>
        /// <param name="incmes_d"></param>
        /// <param name="dept_code"></param>
        /// <param name="dept_name"></param>
        /// <param name="id"></param>
        public void UpdateXyhsIncomesAdjust(string st_date, string incomes_adjust, string incmes_d, string dept_code, string dept_name, string id)
        {
            string sql = string.Format(@"update CBHS.XYHS_INCOMES_ADJUST 
                                        set   ST_DATE='{0}',
                                              DEPT_CODE='{1}',
                                              INCOMES_ADJUST='{2}',
                                              INCOMES_DIFFERENCE='{3}',
                                              DEPT_NAME='{4}' where  ID='{5}'", st_date, dept_code, incomes_adjust, incmes_d, dept_name, id);
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取收入调整
        /// </summary>
        /// <returns></returns>
        public DataSet GetXyhsIncomesAdjust()
        {
            string sql = "select * from CBHS.XYHS_INCOMES_ADJUST ";
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        public DataSet GetXyhsIncomesAdjustByid(string id)
        {
            string sql = string.Format("select * from CBHS.XYHS_INCOMES_ADJUST where id={0}", id);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        public void DelXyhsIncomesAdjustByid(string id)
        {
            string sql = string.Format(@"delete from CBHS.XYHS_INCOMES_ADJUST where id={0}", id);
            OracleOledbBase.ExecuteNonQuery(sql);
        }




        #endregion


        #region 按卫生部文件调整（sunyc 2016-4-22）

        /// <summary>
        /// 表7 医院各科室医疗成本三级分摊表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost07( string begindate, string enddate)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("SELECT   DEPT_NAME AS \"医院科室名称\",'' AS \"标准科室名称\",ROWNUM AS \"行次\" ");
            str.AppendFormat(",round(YLCB,2) AS \"医疗成本\",round(ZJCB,2) AS \"直接成本\",round(XJ,2) AS \"小计\",round(FTXZCB,2) AS \"分摊行政后勤类科室成本\"  ");
            str.AppendFormat(",round(FTFZCB,2) AS \"分摊医疗辅助类科室成本\",round(FTYJCB,2) AS \"分摊医疗技术类科室成本\" ");

            str.AppendFormat(@"
 FROM   (  SELECT   GROUPING (aaa) aa_g,
                     GROUPING (DEPT_TYPE) DEPT_TYPE_g,
                     GROUPING (ACCOUNT_DEPT_CODE) ACCOUNT_DEPT_CODE_g,
                     aaa,
                     DEPT_TYPE,
                     ACCOUNT_DEPT_CODE,
                     case 
            when GROUPING (aaa)=0 and GROUPING (DEPT_TYPE)=0 and GROUPING (ACCOUNT_DEPT_CODE)=1 and aaa=0 and dept_type=0 then '临床服务类小计'
            when GROUPING (aaa)=0 and GROUPING (DEPT_TYPE)=0 and GROUPING (ACCOUNT_DEPT_CODE)=1 and aaa=0 and dept_type=1 then '医疗技术类小计'
            when GROUPING (aaa)=0 and GROUPING (DEPT_TYPE)=0 and GROUPING (ACCOUNT_DEPT_CODE)=1 and aaa=0 and dept_type=2 then '医疗辅助类小计'
            when GROUPING (aaa)=0 and GROUPING (DEPT_TYPE)=1 and GROUPING (ACCOUNT_DEPT_CODE)=1 and aaa=0  then '医疗业务成本小计'
            when GROUPING (aaa)=0 and GROUPING (DEPT_TYPE)=0 and GROUPING (ACCOUNT_DEPT_CODE)=1 and aaa=1 and dept_type=3 then '行政后勤类小计'
            when GROUPING (aaa)=0 and GROUPING (DEPT_TYPE)=1 and GROUPING (ACCOUNT_DEPT_CODE)=1 and aaa=1  then 'no'
            when GROUPING (aaa)=1 and GROUPING (DEPT_TYPE)=1 and GROUPING (ACCOUNT_DEPT_CODE)=1  then '合计'
            else
           min(account_dept_name)
           end dept_name,
                     SUM (YLCB) AS YLCB,
                     SUM (ZJCB) AS ZJCB,
                     SUM (XJ) AS XJ,
                     SUM (FTXZCB) AS FTXZCB,
                     SUM (FTFZCB) AS FTFZCB,
                     SUM (FTYJCB) AS FTYJCB
              FROM   (  SELECT   dept_type,
                                 CASE WHEN dept_type < 3 THEN 0 ELSE 1 END aaa,
                                 account_dept_code,
                                 account_dept_name,
                                 SUM (costs) AS ylcb,
                                 SUM(CASE
                                        WHEN get_type = '0' THEN costs
                                        ELSE 0
                                     END)
                                    AS zjcb,
                                 SUM(CASE
                                        WHEN get_type <> '0' THEN costs
                                        ELSE 0
                                     END)
                                    AS xj,
                                 SUM(CASE
                                        WHEN DATA_SOURCE = '3' THEN costs
                                        ELSE 0
                                     END)
                                    AS ftxzcb,
                                 SUM(CASE
                                        WHEN DATA_SOURCE = '2' THEN costs
                                        ELSE 0
                                     END)
                                    AS ftfzcb,
                                 SUM(CASE
                                        WHEN DATA_SOURCE = '1' THEN costs
                                        ELSE 0
                                     END)
                                    AS ftyjcb
                          FROM   (SELECT   detail.*,
                                           dept.account_dept_code,
                                           dept.account_dept_name,
                                           dept.dept_type,
                                           SORT_NO
                                    FROM   CBHS.xyhs_dept_costs_detail detail,
                                           CBHS.xyhs_dept_dict dept
                                   WHERE   detail.dept_code = dept.dept_code
                                           AND detail.DATE_TIME >=
                                                 DATE '{0}'
                                           AND detail.DATE_TIME <
                                                 ADD_MONTHS (DATE '{1}',
                                                             1))
                      GROUP BY   dept_type,
                                 account_dept_code,
                                 account_dept_name,
                                 SORT_NO
                      ORDER BY   dept_type, SORT_NO)
          GROUP BY   ROLLUP (aaa, dept_type, account_dept_code))
 WHERE   dept_name <> 'no'", begindate, enddate);

          
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }


        /// <summary>
        /// 表8 医院医技科室分摊行政后勤和医辅成本损益表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost08(string begindate, string enddate)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("SELECT   DEPT_NAME AS \"医院科室名称\",'' AS \"标准科室名称\",ROWNUM AS \"行次\" ");
            str.AppendFormat(",round(XY,2) AS \"收入-成本\",round(SR,2) AS \"收入\",round(YLCB,2) AS \"医疗成本\",round(ZJCB,2) AS \"直接成本\"  ");
            str.AppendFormat(",round(XJ,2) AS \"小计\",round(FTXZCB,2) AS \"分摊行政后勤类科室成本\",round(FTFZCB,2) AS \"分摊医疗辅助类科室成本\" ");

            str.AppendFormat(@"
 FROM   (  SELECT   CASE
                        WHEN ACCOUNT_DEPT_CODE IS NULL THEN '合计'
                        ELSE MIN (ACCOUNT_DEPT_name)
                     END
                        dept_name,
                     SUM (xy) xy,
                     SUM (sr) sr,
                     SUM (YLCB) YLCB,
                     SUM (ZJCB) ZJCB,
                     SUM (XJ) XJ,
                     SUM (FTXZCB) FTXZCB,
                     SUM (FTFZCB) FTFZCB
              FROM   (SELECT   costs.ACCOUNT_DEPT_CODE,
                               costs.ACCOUNT_DEPT_name,
                               income.costs - YLCB AS xy,
                               income.costs AS sr,
                               YLCB,
                               ZJCB,
                               XJ,
                               FTXZCB,
                               FTFZCB
                        FROM   (  SELECT   ACCOUNT_DEPT_CODE,
                                           ACCOUNT_DEPT_NAME,
                                           SUM (costs) AS costs
                                    FROM   (SELECT   performed_by, costs
                                              FROM   hisfact.outp_class2_income a
                                             WHERE   a.st_date >=
                                                        DATE '{0}'
                                                     AND a.st_date <
                                                           ADD_MONTHS (
                                                              DATE '{1}',
                                                              1
                                                           )
                                            UNION ALL
                                            SELECT   performed_by, costs
                                              FROM   hisfact.inp_class2_income a
                                             WHERE   a.st_date >=
                                                        DATE '{0}'
                                                     AND a.st_date <
                                                           ADD_MONTHS (
                                                              DATE '{1}',
                                                              1
                                                           )) a,
                                           CBHS.xyhs_dept_dict b
                                   WHERE   a.performed_by = b.dept_code
                                           AND b.dept_type = '1'
                                GROUP BY   ACCOUNT_DEPT_CODE, ACCOUNT_DEPT_NAME)
                               income,
                               (  SELECT   account_dept_code,
                                           account_dept_name,
                                           SUM (costs) AS ylcb,
                                           SUM(CASE
                                                  WHEN get_type = '0' THEN costs
                                                  ELSE 0
                                               END)
                                              AS zjcb,
                                           SUM(CASE
                                                  WHEN get_type <> '0' THEN costs
                                                  ELSE 0
                                               END)
                                              AS xj,
                                           SUM(CASE
                                                  WHEN DATA_SOURCE = '3'
                                                  THEN
                                                     costs
                                                  ELSE
                                                     0
                                               END)
                                              AS ftxzcb,
                                           SUM(CASE
                                                  WHEN DATA_SOURCE = '2'
                                                  THEN
                                                     costs
                                                  ELSE
                                                     0
                                               END)
                                              AS ftfzcb
                                    FROM   (SELECT   detail.*,
                                                     dept.account_dept_code,
                                                     dept.account_dept_name,
                                                     dept.dept_type,
                                                     SORT_NO
                                              FROM   CBHS.xyhs_dept_costs_detail detail,
                                                     CBHS.xyhs_dept_dict dept
                                             WHERE   detail.dept_code =
                                                        dept.dept_code
                                                     AND detail.DATE_TIME >=
                                                           DATE '{0}'
                                                     AND detail.DATE_TIME <
                                                           ADD_MONTHS (
                                                              DATE '{1}',
                                                              1
                                                           )
                                                     AND DEPT_TYPE = '1')
                                GROUP BY   account_dept_code, account_dept_name)
                               costs
                       WHERE   income.ACCOUNT_DEPT_CODE(+) =
                                  costs.ACCOUNT_DEPT_CODE)
          GROUP BY   ROLLUP (ACCOUNT_DEPT_CODE))", begindate, enddate);


            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }



        /// <summary>
        /// 表9 医院临床科室分摊医技和医辅科室成本损益表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost09(string begindate, string enddate)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("SELECT   DEPT_NAME AS \"医院科室名称\",'' AS \"标准科室名称\",ROWNUM AS \"行次\" ");
            str.AppendFormat(",round(XY,2) AS \"收入-成本\",round(SR,2) AS \"收入\",round(YLCB,2) AS \"医疗成本\",round(ZJCB,2) AS \"直接成本\"  ");
            str.AppendFormat(",round(XJ,2) AS \"小计\",round(FTXZCB,2) AS \"分摊行政后勤类科室成本\",round(FTFZCB,2) AS \"分摊医疗辅助类科室成本\" ");

            str.AppendFormat(@"
  FROM   ( 
  SELECT   CASE
                        WHEN ACCOUNT_DEPT_CODE IS NULL THEN '合计'
                        ELSE MIN (ACCOUNT_DEPT_name)
                     END
                        dept_name,
           SUM (xy) AS xy,
           sum(sr) as sr,
           sum(YLCB) as YLCB,
           sum(ZJCB) as ZJCB,
           sum(XJ) as XJ,
           sum(FTXZCB) as FTXZCB,
           sum(FTFZCB) as FTFZCB
    FROM   (SELECT   costs.ACCOUNT_DEPT_CODE,
                     costs.ACCOUNT_DEPT_name,
                     income.costs - YLCB AS xy,
                     income.costs AS sr,
                     YLCB,
                     ZJCB,
                     XJ,
                     FTXZCB,
                     FTFZCB
              FROM   (  SELECT   ACCOUNT_DEPT_CODE,
                                 ACCOUNT_DEPT_NAME,
                                 SUM (costs) AS costs
                          FROM   (SELECT   ORDERED_BY_DEPT, costs
                                    FROM   hisfact.outp_class2_income a
                                   WHERE   a.st_date >= DATE '{0}'
                                           AND a.st_date <
                                                 ADD_MONTHS (DATE '{1}',
                                                             1)
                                  UNION ALL
                                  SELECT   ORDERED_BY_DEPT, costs
                                    FROM   hisfact.inp_class2_income a
                                   WHERE   a.st_date >= DATE '{0}'
                                           AND a.st_date <
                                                 ADD_MONTHS (DATE '{1}',
                                                             1)) a,
                                 CBHS.xyhs_dept_dict b
                         WHERE   a.ORDERED_BY_DEPT = b.dept_code
                                 AND b.dept_type = '0'
                      GROUP BY   ACCOUNT_DEPT_CODE, ACCOUNT_DEPT_NAME) income,
                     (  SELECT   account_dept_code,
                                 account_dept_name,
                                 SUM (costs) AS ylcb,
                                 SUM(CASE
                                        WHEN get_type = '0' THEN costs
                                        ELSE 0
                                     END)
                                    AS zjcb,
                                 SUM(CASE
                                        WHEN get_type <> '0'
                                             AND DATA_SOURCE <> '3'
                                        THEN
                                           costs
                                        ELSE
                                           0
                                     END)
                                    AS xj,
                                 SUM(CASE
                                        WHEN DATA_SOURCE = '2' THEN costs
                                        ELSE 0
                                     END)
                                    AS ftxzcb,
                                 SUM(CASE
                                        WHEN DATA_SOURCE = '1' THEN costs
                                        ELSE 0
                                     END)
                                    AS ftfzcb
                          FROM   (SELECT   detail.*,
                                           dept.account_dept_code,
                                           dept.account_dept_name,
                                           dept.dept_type,
                                           SORT_NO
                                    FROM   CBHS.xyhs_dept_costs_detail detail,
                                           CBHS.xyhs_dept_dict dept
                                   WHERE   detail.dept_code = dept.dept_code
                                           AND detail.DATE_TIME >=
                                                 DATE '{0}'
                                           AND detail.DATE_TIME <
                                                 ADD_MONTHS (DATE '{1}',
                                                             1)
                                           AND DEPT_TYPE = '0')
                      GROUP BY   account_dept_code, account_dept_name) costs
             WHERE   income.ACCOUNT_DEPT_CODE(+) = costs.ACCOUNT_DEPT_CODE)
GROUP BY   ROLLUP (ACCOUNT_DEPT_CODE))", begindate, enddate);


            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 表10 医院临床服务类科室业务收入明细及损益表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost10(string begindate, string enddate)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("SELECT   aaa.account_dept_name AS \"医院科室名称\",'' AS \"标准科室名称\",ROWNUM AS \"行次\" ");
            str.AppendFormat(",round(aaa.costs-bbb.costs,2) AS \"收入-成本\",round(bbb.costs,2) AS \"医疗成本合计\",round(aaa.COSTS,2) AS \"医疗收入合计\",round(OUT_COSTS,2) AS \"门诊收入合计\"  ");
            str.AppendFormat(",round(OUT_GHSR,2) AS \"0挂号收入\",round(OUT_ZC,2) AS \"0诊察收入\",round(OUT_JC,2) AS \"0检查收入\" ");

            str.AppendFormat(",round(OUT_ZL,2) AS \"0治疗收入\",round(OUT_SS,2) AS \"0手术收入\",round(OUT_HY,2) AS \"0化验收入\" ");
            str.AppendFormat(",round(OUT_WSCL,2) AS \"0卫生材料收入\",round(OUT_YP,2) AS \"0药品收入\",round(OUT_YSFWF,2) AS \"0药事服务费收入\",round(OUT_QT,2) AS \"0其他门诊收入\"  ");
            str.AppendFormat(",round(OUT_JSCE,2) AS \"0门诊结算差额\",round(IN_COSTS,2) AS \"住院收入合计\",round(IN_CW,2) AS \"1床位收入\" ");
            str.AppendFormat(",round(IN_ZC,2) AS \"1诊察收入\",round(IN_JC,2) AS \"1检查收入\",round(IN_ZL,2) AS \"1治疗收入\" ");
            str.AppendFormat(",round(IN_SS,2) AS \"1手术收入\",round(IN_HY,2) AS \"1化验收入\",round(IN_HL,2) AS \"1护理收入\" ");
            str.AppendFormat(",round(IN_WSCL,2) AS \"1卫生材料收入\",round(IN_YP,2) AS \"1药品收入\",round(IN_YSFWF,2) AS \"1药事服务费收入\" ");
            str.AppendFormat(",round(IN_QT,2) AS \"1其他住院收入\",round(IN_JSCE,2) AS \"住院结算差额\",round(QT,2) AS \"其他业务收入\" ");
            

            str.AppendFormat(@"
  FROM   (  SELECT   account_dept_code,account_dept_name,
                     SUM (costs) AS costs,
                     SUM (CASE WHEN out_in = 0 THEN costs ELSE 0 END)
                        AS out_costs,                           --门诊收入合计
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'B1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_ghsr,                                --挂号收入
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'C1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_zc,                                  --诊察收入
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'D1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_jc,                                  --检查收入
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'E1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_zl,                                  --治疗收入
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'F1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_ss,                                  --手术收入
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'I1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_hy,                                  --化验收入
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'G1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_wscl,                            --卫生材料收入
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'J1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_yp,                                  --药品收入
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'K1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_ysfwf,                         --药事服务费收入
                     SUM(CASE
                            WHEN out_in = 0
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'L1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_qt,                              --其他门诊收入
                     SUM(CASE
                            WHEN out_in = 0 AND SUBSTR (RECK_CLASS, 0, 2) = ''
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS out_jsce,                            --门诊结算差额
                     SUM (CASE WHEN out_in = 1 THEN costs ELSE 0 END)
                        AS in_costs,                            --住院收入合计
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'A1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_cw,                                   --床位收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'C1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_zc,                                   --诊察收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'D1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_jc,                                   --检查收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'E1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_zl,                                   --治疗收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'F1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_ss,                                   --手术收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'I1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_hy,                                   --化验收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'H1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_hl,                                   --护理收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'G1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_wscl,                             --卫生材料收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'J1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_yp,                                   --药品收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'K1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_ysfwf,                          --药事服务费收入
                     SUM(CASE
                            WHEN out_in = 1
                                 AND SUBSTR (RECK_CLASS, 0, 2) = 'L1'
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_qt,                               --其他住院收入
                     SUM(CASE
                            WHEN out_in = 1 AND SUBSTR (RECK_CLASS, 0, 2) = ''
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS in_jsce,                             --住院结算差额
                     SUM(CASE
                            WHEN SUBSTR (RECK_CLASS, 0, 2) NOT IN
                                       ('A1',
                                        'B1',
                                        'C1',
                                        'D1',
                                        'E1',
                                        'F1',
                                        'G1',
                                        'H1',
                                        'I1',
                                        'J1',
                                        'K1',
                                        'L1')
                            THEN
                               costs
                            ELSE
                               0
                         END)
                        AS qt                                           --其它
              FROM   (  SELECT   ORDERED_BY_DEPT,
                                 RECK_CLASS,
                                 SUM (costs) AS costs,
                                 0 AS out_in
                          FROM   hisfact.outp_class2_income a
                         WHERE   a.st_date >= DATE '{0}'
                                 AND a.st_date <
                                       ADD_MONTHS (DATE '{1}', 1)
                      GROUP BY   ORDERED_BY_DEPT, RECK_CLASS
                      UNION ALL
                        SELECT   ORDERED_BY_DEPT,
                                 RECK_CLASS,
                                 SUM (costs) AS costs,
                                 1 AS out_in
                          FROM   hisfact.inp_class2_income a
                         WHERE   a.st_date >= DATE '{0}'
                                 AND a.st_date <
                                       ADD_MONTHS (DATE '{1}', 1)
                      GROUP BY   ORDERED_BY_DEPT, RECK_CLASS) aa,
                     cbhs.xyhs_dept_dict bb
             WHERE   aa.ORDERED_BY_DEPT = bb.dept_code
          GROUP BY   account_dept_code,account_dept_name) aaa,
         (  SELECT   SUM (detail.costs) AS costs, dept.account_dept_code
              FROM   CBHS.xyhs_dept_costs_detail detail,
                     CBHS.xyhs_dept_dict dept
             WHERE       detail.dept_code = dept.dept_code
                     AND detail.DATE_TIME >= DATE '{0}'
                     AND detail.DATE_TIME < ADD_MONTHS (DATE '{1}', 1)
                     AND DEPT_TYPE = '0'
          GROUP BY   dept.account_dept_code) bbb
 WHERE   aaa.account_dept_code = bbb.account_dept_code", begindate, enddate);


            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 表11 医院临床服务类科室业务收入明细及损益表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost11(string begindate, string enddate)
        {
            StringBuilder str = new StringBuilder();

            


            str.AppendFormat(@"
with abc as
 (select dept_type,
         king_name,
         account_dept_code,
         account_dept_name,
         SUM(person_num) person_num,
         SUM(decode(kind_code, '医疗', person_num, 0)) 医疗人数小计,
         SUM(decode(kind_code,
                    '医疗',
                    round((costs + costs_person + costs_dept) / person_num, 2),
                    0)) 医疗人均工资小计,
         SUM(decode(kind_code, '医疗', costs, 0)) 医疗基本工资,
         0 医疗绩效工资,
         SUM(decode(kind_code, '医疗', costs_person + costs_dept, 0)) 医疗社会保障费,
         SUM(decode(kind_code, '护理', person_num, 0)) 护理人数小计,
         SUM(decode(kind_code,
                    '护理',
                    round((costs + costs_person + costs_dept) / person_num, 2),
                    0)) 护理人均工资小计,
         SUM(decode(kind_code, '护理', costs, 0)) 护理基本工资,
         0 护理绩效工资,
         SUM(decode(kind_code, '护理', costs_person + costs_dept, 0)) 护理社会保障费,
         SUM(decode(kind_code, '医技', person_num, 0)) 医技人数小计,
         SUM(decode(kind_code,
                    '医技',
                    round((costs + costs_person + costs_dept) / person_num, 2),
                    0)) 医技人均工资小计,
         SUM(decode(kind_code, '医技', costs, 0)) 医技基本工资,
         0 医技绩效工资,
         SUM(decode(kind_code, '医技', costs_person + costs_dept, 0)) 医技社会保障费,
         SUM(decode(kind_code, '管理', person_num, 0)) 管理人数小计,
         SUM(decode(kind_code,
                    '管理',
                    round((costs + costs_person + costs_dept) / person_num, 2),
                    0)) 管理人均工资小计,
         SUM(decode(kind_code, '管理', costs, 0)) 管理基本工资,
         0 管理绩效工资,
         SUM(decode(kind_code, '管理', costs_person + costs_dept, 0)) 管理社会保障费,
         SUM(decode(kind_code,
                    '医疗',
                    0,
                    '护理',
                    0,
                    '医技',
                    0,
                    '管理',
                    0,
                    person_num)) 其他人数小计,
         SUM(decode(kind_code,
                    '医疗',
                    0,
                    '护理',
                    0,
                    '医技',
                    0,
                    '管理',
                    0,
                    round((costs + costs_person + costs_dept) / person_num, 2))) 其他人均工资小计,
         SUM(decode(kind_code,
                    '医疗',
                    0,
                    '护理',
                    0,
                    '医技',
                    0,
                    '管理',
                    0,
                    costs)) 其他基本工资,
         0 其他绩效工资,
         SUM(decode(kind_code,
                    '医疗',
                    0,
                    '护理',
                    0,
                    '医技',
                    0,
                    '管理',
                    0,
                    costs_person + costs_dept)) 其他社会保障费,
         sum(costs) + sum(costs_person) + sum(costs_dept) 工资合计,
         ROUND((sum(costs) + sum(costs_person) + sum(costs_dept)) /
               SUM(person_num),
               2) 科室人均
    from (select dept_type,
                 king_name,
                 account_dept_code,
                 account_dept_name,
                 sum(costs) costs,
                 sum(costs_person) costs_person,
                 sum(costs_dept) costs_dept,
                 kind_code,
                 count(emp_code) person_num
            from (select c.dept_type,
                         d.king_name,
                         c.account_dept_code,
                         c.account_dept_name,
                         a. emp_code,
                         b.kind_code,
                         sum(case
                               when substr(a.item_code, 1, 2) in
                                    ('01', '02', '03', '04') then
                                a. money_person
                               else
                                0
                             end) costs,
                         sum(case
                               when substr(a.item_code, 1, 2) = '07' then
                                a.money_person
                               else
                                0
                             end) costs_person,
                         sum(case
                               when substr(a.item_code, 1, 2) = '07' then
                                a.money_dept
                               else
                                0
                             end) costs_dept
                    from DELIT.HR_SALARY_COSTMONTH a,
                         DELIT.hr_sys_emp          b,
                         DELIT.hr_dept_dict        c,
                         DELIT.hr_dept_kind_dict   d
                   WHERE DYDATE >= DATE '{0}'
                     and DYDATE <  ADD_MONTHS (DATE '{1}', 1)
                     and a.emp_code = b.emp_code
                     and a.dept_code = c.dept_code
                     and c.dept_type = d.kind_code
                   group by c.dept_type,
                            d.king_name,
                            c.account_dept_code,
                            c.account_dept_name,
                            a. emp_code,
                            b.kind_code)
           group by dept_type,
                    king_name,
                    account_dept_code,
                    account_dept_name,
                    kind_code)
   GROUP BY dept_type, king_name, account_dept_code, account_dept_name)

select DEPT_TYPE,  ACCOUNT_DEPT_CODE,
       account_dept_name as 医院科室名称,'' as 标准科室名称,
       科室人均,
       医疗人数小计,
       round(医疗人均工资小计,2) as 医疗人均工资小计,
       医疗基本工资,
       医疗绩效工资,
       医疗社会保障费,
       护理人数小计,
       round(护理人均工资小计,2) as 护理人均工资小计,
       护理基本工资,
       护理绩效工资,
       护理社会保障费,
       医技人数小计,
       round(医技人均工资小计,2) as 医技人均工资小计,
       医技基本工资,
       医技绩效工资,
       医技社会保障费,
       管理人数小计,
       round(管理人均工资小计,2) as 管理人均工资小计,
       管理基本工资,
       管理绩效工资,
       管理社会保障费,
       其他人数小计,
       round(其他人均工资小计,2) as 其他人均工资小计,
       其他基本工资,
       其他绩效工资,
       其他社会保障费
  from abc
union all
select DEPT_TYPE, '9999' ACCOUNT_DEPT_CODE,
       '临床服务类科室小计','' as 标准科室名称,
       ROUND(SUM(工资合计) / SUM(person_num), 2) 科室人均,
       SUM(医疗人数小计),
       round((SUM(医疗绩效工资) + SUM(医疗基本工资) + SUM(医疗社会保障费)) /
       DECODE(SUM(医疗人数小计), 0, 1, SUM(医疗人数小计)),2) 医疗人均工资小计,
       SUM(医疗基本工资),
       SUM(医疗绩效工资),
       SUM(医疗社会保障费),
       SUM(护理人数小计),
       round((SUM(护理基本工资) + SUM(护理绩效工资) + SUM(护理社会保障费)) /
       DECODE(SUM(护理人数小计), 0, 1, SUM(护理人数小计)),2) 护理人均工资小计,
       SUM(护理基本工资),
       SUM(护理绩效工资),
       SUM(护理社会保障费),
       SUM(医技人数小计),
       round((SUM(医技基本工资) + SUM(医技绩效工资) + SUM(医技社会保障费)) /
       DECODE(SUM(医技人数小计), 0, 1, SUM(医技人数小计)),2) 医技人均工资小计,
       SUM(医技基本工资),
       SUM(医技绩效工资),
       SUM(医技社会保障费),
       SUM(管理人数小计),
       round((SUM(管理基本工资) + SUM(管理绩效工资) + SUM(管理社会保障费)) /
       DECODE(SUM(管理人数小计), 0, 1, SUM(管理人数小计)),2) 管理人均工资小计,
       SUM(管理基本工资),
       SUM(管理绩效工资),
       SUM(管理社会保障费),
       SUM(其他人数小计),
       round((SUM(其他基本工资) + SUM(其他绩效工资) + SUM(其他社会保障费)) /
       DECODE(SUM(其他人数小计), 0, 1, SUM(其他人数小计)),2) 其他人均工资小计,
       SUM(其他基本工资),
       SUM(其他绩效工资),
       SUM(其他社会保障费)
  from abc
 where dept_type = '0'
 GROUP BY dept_type, king_name
union all
select DEPT_TYPE, '9999' ACCOUNT_DEPT_CODE,
       '医疗技术类小计','' as 标准科室名称,
       ROUND(SUM(工资合计) / SUM(person_num), 2) 科室人均,
       SUM(医疗人数小计),
       round((SUM(医疗绩效工资) + SUM(医疗基本工资) + SUM(医疗社会保障费)) /
       DECODE(SUM(医疗人数小计), 0, 1, SUM(医疗人数小计)),2) 医疗人均工资小计,
       SUM(医疗基本工资),
       SUM(医疗绩效工资),
       SUM(医疗社会保障费),
       SUM(护理人数小计),
       round((SUM(护理基本工资) + SUM(护理绩效工资) + SUM(护理社会保障费)) /
       DECODE(SUM(护理人数小计), 0, 1, SUM(护理人数小计)),2) 护理人均工资小计,
       SUM(护理基本工资),
       SUM(护理绩效工资),
       SUM(护理社会保障费),
       SUM(医技人数小计),
       round((SUM(医技基本工资) + SUM(医技绩效工资) + SUM(医技社会保障费)) /
       DECODE(SUM(医技人数小计), 0, 1, SUM(医技人数小计)),2) 医技人均工资小计,
       SUM(医技基本工资),
       SUM(医技绩效工资),
       SUM(医技社会保障费),
       SUM(管理人数小计),
       round((SUM(管理基本工资) + SUM(管理绩效工资) + SUM(管理社会保障费)) /
       DECODE(SUM(管理人数小计), 0, 1, SUM(管理人数小计)),2) 管理人均工资小计,
       SUM(管理基本工资),
       SUM(管理绩效工资),
       SUM(管理社会保障费),
       SUM(其他人数小计),
       round((SUM(其他基本工资) + SUM(其他绩效工资) + SUM(其他社会保障费)) /
       DECODE(SUM(其他人数小计), 0, 1, SUM(其他人数小计)),2) 其他人均工资小计,
       SUM(其他基本工资),
       SUM(其他绩效工资),
       SUM(其他社会保障费)
  from abc
 where dept_type = '1'
 GROUP BY dept_type, king_name
union all
select DEPT_TYPE, '9999' ACCOUNT_DEPT_CODE,
       '医疗辅助类小计','' as 标准科室名称,
       ROUND(SUM(工资合计) / SUM(person_num), 2) 科室人均,
       SUM(医疗人数小计),
       round((SUM(医疗绩效工资) + SUM(医疗基本工资) + SUM(医疗社会保障费)) /
       DECODE(SUM(医疗人数小计), 0, 1, SUM(医疗人数小计)),2) 医疗人均工资小计,
       SUM(医疗基本工资),
       SUM(医疗绩效工资),
       SUM(医疗社会保障费),
       SUM(护理人数小计),
       round((SUM(护理基本工资) + SUM(护理绩效工资) + SUM(护理社会保障费)) /
       DECODE(SUM(护理人数小计), 0, 1, SUM(护理人数小计)),2) 护理人均工资小计,
       SUM(护理基本工资),
       SUM(护理绩效工资),
       SUM(护理社会保障费),
       SUM(医技人数小计),
       round((SUM(医技基本工资) + SUM(医技绩效工资) + SUM(医技社会保障费)) /
       DECODE(SUM(医技人数小计), 0, 1, SUM(医技人数小计)),2) 医技人均工资小计,
       SUM(医技基本工资),
       SUM(医技绩效工资),
       SUM(医技社会保障费),
       SUM(管理人数小计),
       round((SUM(管理基本工资) + SUM(管理绩效工资) + SUM(管理社会保障费)) /
       DECODE(SUM(管理人数小计), 0, 1, SUM(管理人数小计)),2) 管理人均工资小计,
       SUM(管理基本工资),
       SUM(管理绩效工资),
       SUM(管理社会保障费),
       SUM(其他人数小计),
       round((SUM(其他基本工资) + SUM(其他绩效工资) + SUM(其他社会保障费)) /
       DECODE(SUM(其他人数小计), 0, 1, SUM(其他人数小计)),2) 其他人均工资小计,
       SUM(其他基本工资),
       SUM(其他绩效工资),
       SUM(其他社会保障费)
  from abc
 where dept_type = '2'
 GROUP BY dept_type, king_name
union all
select DEPT_TYPE, '9999' ACCOUNT_DEPT_CODE,
       '行政后勤类小计','' as 标准科室名称,
       ROUND(SUM(工资合计) / SUM(person_num), 2) 科室人均,
       SUM(医疗人数小计),
       round((SUM(医疗绩效工资) + SUM(医疗基本工资) + SUM(医疗社会保障费)) /
       DECODE(SUM(医疗人数小计), 0, 1, SUM(医疗人数小计)),2) 医疗人均工资小计,
       SUM(医疗基本工资),
       SUM(医疗绩效工资),
       SUM(医疗社会保障费),
       SUM(护理人数小计),
       round((SUM(护理基本工资) + SUM(护理绩效工资) + SUM(护理社会保障费)) /
       DECODE(SUM(护理人数小计), 0, 1, SUM(护理人数小计)),2) 护理人均工资小计,
       SUM(护理基本工资),
       SUM(护理绩效工资),
       SUM(护理社会保障费),
       SUM(医技人数小计),
       round((SUM(医技基本工资) + SUM(医技绩效工资) + SUM(医技社会保障费)) /
       DECODE(SUM(医技人数小计), 0, 1, SUM(医技人数小计)),2) 医技人均工资小计,
       SUM(医技基本工资),
       SUM(医技绩效工资),
       SUM(医技社会保障费),
       SUM(管理人数小计),
       round((SUM(管理基本工资) + SUM(管理绩效工资) + SUM(管理社会保障费)) /
       DECODE(SUM(管理人数小计), 0, 1, SUM(管理人数小计)),2) 管理人均工资小计,
       SUM(管理基本工资),
       SUM(管理绩效工资),
       SUM(管理社会保障费),
       SUM(其他人数小计),
       round((SUM(其他基本工资) + SUM(其他绩效工资) + SUM(其他社会保障费)) /
       DECODE(SUM(其他人数小计), 0, 1, SUM(其他人数小计)),2) 其他人均工资小计,
       SUM(其他基本工资),
       SUM(其他绩效工资),
       SUM(其他社会保障费)
  from abc
 where dept_type = '3'
 GROUP BY dept_type, king_name
 ORDER BY DEPT_TYPE, ACCOUNT_DEPT_CODE
", begindate, enddate);


            return OracleBase.Query(str.ToString()).Tables[0];
        }


        /// <summary>
        /// 表12 医院临床服务类科室业务收入明细及损益表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost12(string begindate, string enddate)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("SELECT   DEPT_NAME AS \"医院科室名称\",'' AS \"标准科室名称\",ROWNUM AS \"行次\" ");
            str.AppendFormat(",round(BGF,2) AS \"办公费\",round(YSF,2) AS \"印刷费\",round(ZXF,2) AS \"咨询费\" ");

            str.AppendFormat(",round(SXF,2) AS \"手续费\",round(SF,2) AS \"水费\",round(DF,2) AS \"电费\" ");
            str.AppendFormat(",round(QNF,2) AS \"取暖费\",round(YDF,2) AS \"邮电费\",round(WYGLF,2) AS \"物业管理费\" ");
            str.AppendFormat(",round(CLF,2) AS \"差旅费\",round(YGCGF,2) AS \"因公出国（境）费用\",round(WXF,2) AS \"维修（护）费\" ");
            str.AppendFormat(",round(ZLF,2) AS \"租赁费\",round(HYF,2) AS \"会议费\",round(PXF,2) AS \"培训费\" ");
            str.AppendFormat(",round(GWJDF,2) AS \"公务接待费\",round(DZYHP,2) AS \"低值易耗品\",round(LWF,2) AS \"劳务费\" ");
            str.AppendFormat(",round(WTYWF,2) AS \"委托业务费\",round(GHJF,2) AS \"工会经费\",round(FLF,2) AS \"福利费\" ");
            str.AppendFormat(",round(GWYCYXWHF,2) AS \"公务用车运行维护费\",round(QTJTFY,2) AS \"其他交通费用\",round(QTSPHFWZC,2) AS \"其他商品和服务支出\" ");


            str.AppendFormat(@"
 FROM   (  SELECT   dept_type,
                     dept.account_dept_code,
                     CASE
                        WHEN dept_type = 0 AND dept.account_dept_code IS NULL
                        THEN
                           '临床服务类小计'
                        WHEN dept_type = 1 AND dept.account_dept_code IS NULL
                        THEN
                           '医疗技术类小计'
                        WHEN dept_type = 2 AND dept.account_dept_code IS NULL
                        THEN
                           '医疗辅助类小计'
                        WHEN dept_type = 3 AND dept.account_dept_code IS NULL
                        THEN
                           '行政后勤类小计'
                        WHEN dept_type IS NULL
                             AND dept.account_dept_code IS NULL
                        THEN
                           '合计'
                        ELSE
                           MIN (dept.account_dept_NAME)
                     END
                        dept_name,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GA01' THEN detail.costs
                            ELSE 0
                         END)
                        AS BGF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GB01' THEN detail.costs
                            ELSE 0
                         END)
                        AS YSF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GC0' THEN detail.costs
                            ELSE 0
                         END)
                        AS ZXF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GD0' THEN detail.costs
                            ELSE 0
                         END)
                        AS SXF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GE0' THEN detail.costs
                            ELSE 0
                         END)
                        AS SF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GF0' THEN detail.costs
                            ELSE 0
                         END)
                        AS DF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GH0' THEN detail.costs
                            ELSE 0
                         END)
                        AS QNF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GG0' THEN detail.costs
                            ELSE 0
                         END)
                        AS YDF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GI0' THEN detail.costs
                            ELSE 0
                         END)
                        AS WYGLF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GJ0' THEN detail.costs
                            ELSE 0
                         END)
                        AS CLF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GK0' THEN detail.costs
                            ELSE 0
                         END)
                        AS YGCGF,
                     SUM(CASE
                            WHEN SUBSTR (ITEM_CODE, 0, 2) = 'GL'
                            THEN
                               detail.costs
                            ELSE
                               0
                         END)
                        AS WXF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GM0' THEN detail.costs
                            ELSE 0
                         END)
                        AS ZLF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GN0' THEN detail.costs
                            ELSE 0
                         END)
                        AS HYF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GO0' THEN detail.costs
                            ELSE 0
                         END)
                        AS PXF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GP0' THEN detail.costs
                            ELSE 0
                         END)
                        AS GWJDF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GR01' THEN detail.costs
                            ELSE 0
                         END)
                        AS DZYHP,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GS0' THEN detail.costs
                            ELSE 0
                         END)
                        AS LWF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GT0' THEN detail.costs
                            ELSE 0
                         END)
                        AS WTYWF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GU0' THEN detail.costs
                            ELSE 0
                         END)
                        AS GHJF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GV0' THEN detail.costs
                            ELSE 0
                         END)
                        AS FLF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GT01' THEN detail.costs
                            ELSE 0
                         END)
                        AS GWYCYXWHF,
                     SUM(CASE
                            WHEN ITEM_CODE = 'GT01' THEN detail.costs
                            ELSE 0
                         END)
                        AS QTJTFY,
                     SUM(CASE
                            WHEN SUBSTR (ITEM_CODE, 0, 2) = 'GX'
                            THEN
                               detail.costs
                            ELSE
                               0
                         END)
                        AS QTSPHFWZC
              FROM   CBHS.xyhs_dept_costs_detail detail,
                     CBHS.xyhs_dept_dict dept
             WHERE       detail.dept_code = dept.dept_code
                     AND detail.DATE_TIME >= DATE '{0}'
                     AND detail.DATE_TIME < ADD_MONTHS (DATE '{1}', 1)
                     AND get_type = '0'
          GROUP BY   ROLLUP (dept_type, dept.account_dept_code))", begindate, enddate);


            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }


        /// <summary>
        /// 表13 医院临床服务类科室业务收入明细及损益表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost13(string begindate, string enddate)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"
WITH cr
       AS (  SELECT   ACCOUNT_DEPT_CODE dept_code, SUM (ZCRS) ZCRS
               FROM   hisfact.dept_in_hospital_days a, cbhs.XYHS_DEPT_DICT b
              WHERE   a.st_date >= TO_DATE ('{0}', 'yyyymmdd')
                      AND A.ST_DATE <
                            ADD_MONTHS (TO_DATE ('{1}', 'yyyymmdd'), 1)
                      AND a.dept_code = b.dept_code
           GROUP BY   TO_CHAR (a.st_date, 'yyyyMM'), B.ACCOUNT_DEPT_CODE),
    zc
       AS (  SELECT   c.ACCOUNT_DEPT_CODE dept_code, SUM (1) MZRC
               FROM   HISDATA.clinic_master D, (SELECT   *
                                                  FROM   cbhs.XYHS_DEPT_DICT
                                                 WHERE   OUT_OR_IN = 1) c
              WHERE       RETURNED_OPERATOR IS NULL
                      AND D.VISIT_DEPT = c.dept_code
                      AND VISIT_DATE >= TO_DATE ('{0}', 'yyyymmdd')
                      AND VISIT_DATE <
                            ADD_MONTHS (TO_DATE ('{1}', 'yyyymmdd'), 1)
           GROUP BY   TO_CHAR (D.VISIT_DATE, 'yyyyMM'), c.ACCOUNT_DEPT_CODE),
    cy
       AS (  SELECT   dept_discharge_from dept_code, COUNT ( * ) cy_rs
               FROM   hisdata.pat_visit
              WHERE   DISCHARGE_DATE_TIME >=
                         TO_DATE ('{0}', 'yyyymmdd')
                      AND DISCHARGE_DATE_TIME <=
                            TO_DATE ('{1}', 'yyyymmdd')
                      AND dept_discharge_from IS NOT NULL
           GROUP BY   dept_discharge_from),
    incomes
       AS (  SELECT   account_dept_code dept_code,
                      account_dept_name,
                      SUM (COSTS) AS income,
                      sum(case when substr(RECK_CLASS,0,1) = 'A' then COSTS else 0 end) as yp_income
               FROM   (  SELECT   ORDERED_BY_DEPT dept_code,
                                  RECK_CLASS,
                                  SUM (costs) AS costs,
                                  0 AS out_in
                           FROM   hisfact.outp_class2_income a
                          WHERE   a.st_date >= TO_DATE ('{0}', 'yyyymmdd')
                                  AND a.st_date <
                                        ADD_MONTHS (TO_DATE ('{1}', 'yyyymmdd'), 1)
                       GROUP BY   ORDERED_BY_DEPT, RECK_CLASS
                       UNION ALL
                         SELECT   ORDERED_BY_DEPT,
                                  RECK_CLASS,
                                  SUM (costs) AS costs,
                                  1 AS out_in
                           FROM   hisfact.inp_class2_income a
                          WHERE   a.st_date >= TO_DATE ('{0}', 'yyyymmdd')
                                  AND a.st_date <
                                        ADD_MONTHS (TO_DATE ('{1}', 'yyyymmdd'), 1)
                       GROUP BY   ORDERED_BY_DEPT, RECK_CLASS) aa,
                      cbhs.xyhs_dept_dict bb
              WHERE   aa.dept_code = bb.dept_code
           GROUP BY   account_dept_code, account_dept_name),
    costss
       AS (  SELECT   SUM (detail.costs) AS costs,
                      dept.account_dept_code AS dept_code
               FROM   CBHS.xyhs_dept_costs_detail detail,
                      CBHS.xyhs_dept_dict dept
              WHERE       detail.dept_code = dept.dept_code
                      AND detail.DATE_TIME >= TO_DATE ('{0}', 'yyyymmdd')
                      AND detail.DATE_TIME < ADD_MONTHS (TO_DATE ('{1}', 'yyyymmdd'), 1)
                      AND DEPT_TYPE = '0'
           GROUP BY   dept.account_dept_code)
SELECT   aa.dept_code,
         aa.account_dept_name,
         '' as bzksmc,
         rownum as hc,
         zc.MZRC,
         cr.ZCRS,
         cy.cy_rs,
       round( case when aa.income=0 then 0 else  zc.MZRC/aa.income end,2) as zc_sf,
       round( case when bb.costs=0 then 0 else  zc.MZRC/bb.costs end,2) as zc_cb,
       round( case when aa.yp_income=0 then 0 else  zc.MZRC/aa.yp_income end ,2) as zc_ypsf,
       round( case when aa.income=0 then 0 else  zc.MZRC/aa.income end  -  case when bb.costs=0 then 0 else  zc.MZRC/bb.costs end ,2) as zc_jy,
        
       round( case when aa.income=0 then 0 else  cr.ZCRS/aa.income end ,2) as cr_sf,
       round( case when bb.costs=0 then 0 else  cr.ZCRS/bb.costs end ,2) as cr_cb,
       round( case when aa.yp_income=0 then 0 else  cr.ZCRS/aa.yp_income end ,2) as cr_ypsf,
       round( case when aa.income=0 then 0 else  cr.ZCRS/aa.income end  -  case when bb.costs=0 then 0 else  cr.ZCRS/bb.costs end ,2) as cr_jy,
        
       round( case when aa.income=0 then 0 else  cy.cy_rs/aa.income end ,2) as cy_sf,
       round( case when bb.costs=0 then 0 else  cy.cy_rs/bb.costs end ,2) as cy_cb,
       round( case when aa.yp_income=0 then 0 else  cy.cy_rs/aa.yp_income end ,2) as cy_ypsf,
       round( case when aa.income=0 then 0 else  cy.cy_rs/aa.income end  -  case when bb.costs=0 then 0 else  cy.cy_rs/bb.costs end ,2) as cy_jy
  FROM   incomes aa,
         costss bb,
         cr,
         zc,
         cy
 WHERE       aa.dept_code = bb.dept_code(+)
         AND aa.dept_code = cr.dept_code(+)
         AND aa.dept_code = zc.dept_code(+)
         AND aa.dept_code = cy.dept_code(+)", begindate, enddate);

            return OracleBase.Query(str.ToString()).Tables[0];
        
        }

         /// <summary>
        /// 表14 医院基本情况分析表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataTable GetCost14(string begindate, string enddate)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"select * from cbhs.v_yyjbqk 
where st_date >= to_date('{0}','yyyymmdd') and st_date < ADD_MONTHS (TO_DATE ('{1}', 'yyyymmdd'), 1) ", begindate, enddate);
            return OracleBase.Query(str.ToString()).Tables[0];

        }

        #endregion



    }
}
