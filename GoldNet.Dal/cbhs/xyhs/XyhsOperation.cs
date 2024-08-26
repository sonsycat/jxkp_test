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
    public class XyhsOperation
    {
        public XyhsOperation()
        {

        }
        /// <summary>
        /// 全成本科室
        /// </summary>
        /// <param name="inputcode"></param>
        /// <param name="deptfilter"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetAllDept(string inputcode, string deptfilter, string deptcode)
        {
            StringBuilder strSql = new StringBuilder();
            if (deptcode != "")
            {
                strSql.AppendFormat("select a.dept_code, a.dept_name,a.input_code from {0}.XYHS_DEPT_DICT a where  a.dept_code = '{1}' and a.SHOW_FLAG='0' ", DataUser.CBHS, deptcode);
                strSql.Append(" union all ");
                strSql.AppendFormat("select a.dept_code, a.dept_name,a.input_code from {0}.XYHS_DEPT_DICT a where   a.input_code like ? and a.dept_code!='{1}' and a.SHOW_FLAG='0'", DataUser.CBHS, deptcode);
                if (!deptfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", deptfilter);
                }

            }
            else
            {
                strSql.AppendFormat("select a.dept_code, a.dept_name,a.input_code from {0}.XYHS_DEPT_DICT a where   a.SHOW_FLAG='0' and a.input_code like ? ", DataUser.CBHS);
                if (!deptfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", deptfilter);
                }
                strSql.Append(" order by a.SORT_NO");
            }
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(), cmdPara);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputcode"></param>
        /// <param name="costsfilter"></param>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public DataSet GetAllCost(string inputcode, string costsfilter, string itemcode)
        {
            StringBuilder strSql = new StringBuilder();
            if (itemcode != "")
            {
                strSql.AppendFormat("select a.item_code, a.item_name,a.input_code from {0}.XYHS_COST_ITEM_DICT a where  a.item_code = '{1}'", DataUser.CBHS, itemcode);
                strSql.Append(" union all ");
                strSql.AppendFormat("select a.item_code, a.item_name,a.input_code from {0}.XYHS_COST_ITEM_DICT a where   a.input_code like ? and a.item_code!='{1}'", DataUser.CBHS, itemcode);
                if (!costsfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", costsfilter);
                }

            }
            else
            {
                strSql.AppendFormat("select a.item_code, a.item_name,a.input_code from {0}.XYHS_COST_ITEM_DICT a where   a.input_code like ? ", DataUser.CBHS);
                if (!costsfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", costsfilter);
                }
                strSql.Append(" order by a.item_code");
            }
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(), cmdPara);
        }
        /// <summary>
        /// 分解方案
        /// </summary>
        /// <param name="inputcode"></param>
        /// <param name="costsfilter"></param>
        /// <returns></returns>
        public DataSet GetAllProg(string inputcode, string costsfilter)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendFormat("select a.prog_code, a.prog_name,a.input_code from {0}.XYHS_COST_APPOR_PROG_DICT a where   a.input_code like ? ", DataUser.CBHS);
            if (!costsfilter.Equals(""))
            {
                strSql.AppendFormat(" and {0}", costsfilter);
            }
            strSql.Append(" order by a.prog_code");

            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(), cmdPara);
        }
        /// <summary>
        /// 管理成本列表
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="gettype"></param>
        /// <returns></returns>
        public DataSet GetMangerCost(string date_time, string gettype)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   A.ROWID ID,
         A.ITEM_CODE,
         B.ITEM_NAME,
         A.DATE_TIME,
         round(A.COSTS,2) COSTS,
         A.PROG_CODE,
         C.PROG_NAME,
         A.MEMO
  FROM   CBHS.XYHS_MANAGER_COSTS A,
         CBHS.XYHS_COST_ITEM_DICT B,
         CBHS.XYHS_COST_APPOR_PROG_DICT C
 WHERE   A.ITEM_CODE = B.ITEM_CODE AND A.PROG_CODE = C.PROG_CODE AND A.DATE_TIME=DATE'{0}' AND A.GET_TYPE={1}", date_time, gettype);

            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 科室成本列表
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="gettype"></param>
        /// <returns></returns>
        public DataSet GetdDeptCost(string date_time, string gettype)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   A.ROWID ID,
         A.ITEM_CODE,
         B.ITEM_NAME,
         A.DATE_TIME,
         round(A.COSTS,2) COSTS,
         A.DEPT_CODE,
         C.DEPT_NAME,
         A.MEMO
  FROM   CBHS.XYHS_DEPT_COSTS_INPUT A,
         CBHS.XYHS_COST_ITEM_DICT B,
         CBHS.XYHS_DEPT_DICT C
 WHERE   A.ITEM_CODE = B.ITEM_CODE AND A.DEPT_CODE = C.DEPT_CODE AND A.DATE_TIME=DATE'{0}' AND A.GET_TYPE={1}", date_time, gettype);

            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 科室成本提取
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="deptcode"></param>
        /// <param name="itemcode"></param>
        /// <param name="gettype"></param>
        /// <returns></returns>
        public DataSet GetdDeptCostext(string date_time,string deptcode,string itemcode,string costtype)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   A.ROWID ID,
         A.ITEM_CODE,
         B.ITEM_NAME,
         A.DATE_TIME,
         round(A.COSTS,2) COSTS,
         A.DEPT_CODE,
         C.DEPT_NAME
  FROM   CBHS.XYHS_DEPT_COSTS_EXT A,
         CBHS.XYHS_COST_ITEM_DICT B,
         CBHS.XYHS_DEPT_DICT C
 WHERE   A.ITEM_CODE = B.ITEM_CODE AND A.DEPT_CODE = C.DEPT_CODE AND A.DATE_TIME=DATE'{0}' AND A.COST_TYPE={1}", date_time, costtype);
            if (deptcode != "")
                strSql.AppendFormat(" and a.dept_code='{0}'",deptcode);
            if (itemcode != "")
                strSql.AppendFormat(" and a.item_code='{0}'",itemcode);

            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 科室成本归集
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="deptcode"></param>
        /// <param name="itemcode"></param>
        /// <param name="costtype"></param>
        /// <returns></returns>
        public DataSet GetdDeptCostunion(string date_time, string deptcode, string itemcode, string costtype)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT
         A.ITEM_CODE,
         B.ITEM_NAME,
         A.DATE_TIME,
         round(sum(A.COSTS),2) COSTS,
         A.DEPT_CODE,
         C.DEPT_NAME
  FROM   CBHS.XYHS_DEPT_COSTS_UNION A,
         CBHS.XYHS_COST_ITEM_DICT B,
         CBHS.XYHS_DEPT_DICT C
 WHERE   A.ITEM_CODE = B.ITEM_CODE AND A.DEPT_CODE = C.DEPT_CODE AND A.DATE_TIME=DATE'{0}'", date_time);
            if (deptcode != "")
                strSql.AppendFormat(" and a.dept_code='{0}'", deptcode);
            if (itemcode != "")
                strSql.AppendFormat(" and a.item_code='{0}'", itemcode);
            strSql.AppendFormat(" group by a.item_code,b.item_name,a.date_time,a.dept_code,c.dept_name order by a.item_code,a.dept_code");

            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 删除管理成本
        /// </summary>
        /// <param name="id"></param>
        public void DelManagerCosts(string itemcode,string progcode,string date,string gettype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"DELETE FROM {0}.XYHS_MANAGER_COSTS
                                  WHERE item_code = '{1}'and date_time=date'{2}' and prog_code='{3}' and get_type={4}", DataUser.CBHS,itemcode,date,progcode,gettype);
        
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        /// <summary>
        /// 删除科室成本
        /// </summary>
        /// <param name="id"></param>
        public void DelDeptCosts(string itemcode, string deptcode, string date, string gettype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"DELETE FROM {0}.XYHS_DEPT_COSTS_INPUT
                                  WHERE item_code = '{1}'and date_time=date'{2}' and dept_code='{3}' and get_type={4}", DataUser.CBHS, itemcode, date, deptcode, gettype);

            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        public void DeltohisDept(string hisdeptcode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"DELETE FROM {0}.XYHS_DEPT_TO_HISDEPT
                                  WHERE HIS_DEPT_CODE = '{1}'", DataUser.CBHS, hisdeptcode);

            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        /// <summary>
        /// 保存管理成本
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="date_time"></param>
        /// <param name="operators"></param>
        /// <param name="gettype"></param>
        public void SaveManagerCosts(Dictionary<string, string>[] costdetails, string date_time, string operators, string gettype)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();

            //删除已录入的成本
            strDel.AppendFormat("DELETE FROM {0}.XYHS_MANAGER_COSTS WHERE date_time=date'{2}' AND  GET_TYPE='{1}'", DataUser.CBHS, gettype, date_time);
         
            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { };
            listtable.Add(listDel);
            for (int i = 0; i < costdetails.Length; i++)
            {
                if (costdetails[i]["COSTS"] == null || costdetails[i]["COSTS"].Equals("") || costdetails[i]["ITEM_CODE"].Equals("") || costdetails[i]["PROG_CODE"].Equals(""))
                {
                    continue;
                }

                else
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendFormat(@"INSERT INTO {0}.XYHS_MANAGER_COSTS 
                                          (ITEM_CODE,DATE_TIME,COSTS,PROG_CODE,
                                           OPUSER,MEMO,GET_TYPE)
                                        VALUES(?,?,?,?,?,?,?)", DataUser.CBHS);
                    double costs = Convert.ToDouble(costdetails[i]["COSTS"]);
                    OleDbParameter[] parameteradd = {
											  new OleDbParameter("item_code",costdetails[i]["ITEM_CODE"]),
											  new OleDbParameter("accounting_date",Convert.ToDateTime(date_time)),
											  new OleDbParameter("costs",costs),
											  new OleDbParameter("prog_code",costdetails[i]["PROG_CODE"]),
											  new OleDbParameter("OPERATOR",operators),
		
											  new OleDbParameter("memo",costdetails[i]["MEMO"]),
											  new OleDbParameter("GET_TYPE","0")
										  };
                    List listAdd = new List();
                    listAdd.StrSql = strSql;
                    listAdd.Parameters = parameteradd;
                    listtable.Add(listAdd);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 保存科室对照
        /// </summary>
        /// <param name="deptdetails"></param>
        /// <param name="deptcode"></param>
        public void Savetohisdept(Dictionary<string, string>[] deptdetails,string deptcode)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();

           
            for (int i = 0; i < deptdetails.Length; i++)
            {
                //删除已录入的成本
                strDel.AppendFormat("DELETE FROM cbhs.XYHS_DEPT_TO_HISDEPT WHERE HIS_DEPT_CODE='{0}'", deptdetails[i]["DEPT_CODE"]);

                List listDel = new List();
                listDel.StrSql = strDel;
                listDel.Parameters = new OleDbParameter[] { };
                listtable.Add(listDel);
                //
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.XYHS_DEPT_TO_HISDEPT 
                                          (HIS_DEPT_CODE,XYHS_DEPT_CODE)
                                        VALUES(?,?)", DataUser.CBHS);
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("HIS_DEPT_CODE",deptdetails[i]["DEPT_CODE"]),
											  new OleDbParameter("XYHS_DEPT_CODE",deptcode)
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 保存科室成本
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="date_time"></param>
        /// <param name="operators"></param>
        /// <param name="gettype"></param>
        public void SaveDeptCosts(Dictionary<string, string>[] costdetails, string date_time, string operators, string gettype)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();

            //删除已录入的成本
            strDel.AppendFormat("DELETE FROM {0}.XYHS_DEPT_COSTS_INPUT WHERE date_time=date'{2}' AND  GET_TYPE='{1}'", DataUser.CBHS, gettype, date_time);

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { };
            listtable.Add(listDel);
            for (int i = 0; i < costdetails.Length; i++)
            {
                if (costdetails[i]["COSTS"] == null || costdetails[i]["COSTS"].Equals("") || costdetails[i]["ITEM_CODE"].Equals("") || costdetails[i]["DEPT_CODE"].Equals(""))
                {
                    continue;
                }

                else
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendFormat(@"INSERT INTO {0}.XYHS_DEPT_COSTS_INPUT 
                                          (ITEM_CODE,DATE_TIME,COSTS,DEPT_CODE,
                                           OPUSER,MEMO,GET_TYPE)
                                        VALUES(?,?,?,?,?,?,?)", DataUser.CBHS);
                    double costs = Convert.ToDouble(costdetails[i]["COSTS"]);
                    OleDbParameter[] parameteradd = {
											  new OleDbParameter("item_code",costdetails[i]["ITEM_CODE"]),
											  new OleDbParameter("accounting_date",Convert.ToDateTime(date_time)),
											  new OleDbParameter("costs",costs),
											  new OleDbParameter("dept_code",costdetails[i]["DEPT_CODE"]),
											  new OleDbParameter("OPERATOR",operators),
		
											  new OleDbParameter("memo",costdetails[i]["MEMO"]),
											  new OleDbParameter("GET_TYPE","0")
										  };
                    List listAdd = new List();
                    listAdd.StrSql = strSql;
                    listAdd.Parameters = parameteradd;
                    listtable.Add(listAdd);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 效益核算科室类别
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetDeptType()
        {
            string strSql = string.Format("SELECT   ID, XYHS_DEPT_TYPE FROM {0}.XYHS_DEPT_TYPE_DICT where 1=1 order by id", DataUser.CBHS);// id!=0
            return OracleOledbBase.ExecuteDataSet(strSql);
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
        /// 获取科室成本分摊后明细数据
        /// </summary>
        /// <param name="dete_time">日期（201012）</param>
        /// <param name="dept_code">科室编码</param>
        /// <returns>DataSet</returns>
        public DataSet GetCostsDealDetail(string date_time, string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT ITEM_CODE,ITEM_NAME,SUM (COSTS+COSTS_ARMYFREE) COSTS,COST_TYPE
                                  FROM (SELECT A.ITEM_CODE,B.ITEM_NAME,A.COSTS,COSTS_ARMYFREE,
                                               CASE WHEN GET_TYPE ='0' THEN '直接成本'
                                                    WHEN GET_TYPE = '1' THEN '间接成本'
                                               END COST_TYPE
                                          FROM {0}.XYHS_DEPT_COSTS_DETAIL A,CBHS.XYHS_COST_ITEM_DICT B,cbhs.XYHS_DEPT_DICT c
                                         WHERE TO_CHAR(DATE_TIME,'yyyymm')='{1}' and a.dept_code=c.dept_code AND A.ITEM_CODE=B.ITEM_CODE", DataUser.CBHS, date_time);
            if (dept_code != null && dept_code != "")
            {
                strSql.AppendFormat(" AND c.ACCOUNT_DEPT_CODE = '{0}'", dept_code);
            }

            strSql.AppendFormat(@")
                                GROUP BY ITEM_CODE,ITEM_NAME,COST_TYPE ORDER BY COST_TYPE DESC,ITEM_CODE");
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
        /// 获取科室成本项目构成比数据
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="dept_code">科室编码</param>
        /// <returns>DataSet</returns>
        public DataSet GetChartDataByItem(string date_time, string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   c.ITEM_NAME, round(SUM (A.COSTS),2) COSTS
                                FROM   {0}.XYHS_DEPT_COSTS_DETAIL A,CBHS.XYHS_DEPT_DICT B,cbhs.XYHS_COST_ITEM_DICT c
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
        /// <summary>
        /// 对照his科室
        /// </summary>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataSet Select_dept_to_hisdept(string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   A.HIS_DEPT_CODE DEPT_CODE, b.dept_name 
                                FROM   CBHS.XYHS_DEPT_TO_HISDEPT A,comm.sys_dept_dict B
                               WHERE   A.XYHS_DEPT_CODE='{0}' and a.his_dept_code=b.dept_code", dept_code);
           
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// 查询分解方案外科室
        /// </summary>
        /// <param name="fjfa"></param>
        /// <returns></returns>
        public DataSet GetNoCheckDeptByProg(string fjfa, string depttype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.dept_code,a.dept_name,0 RATIO FROM {0}.XYHS_DEPT_DICT a where a.SHOW_FLAG='0'  and a.dept_code  not in (select dept_code from {0}.XYHS_COST_APPOR_DEPT_DETAIL b
  WHERE b.prog_code='{1}')", DataUser.CBHS,fjfa);
            if (!depttype.Equals(string.Empty))
            {
                str.AppendFormat(" and a.dept_type ='{0}'", depttype);
            }
            str.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询分解方案科室
        /// </summary>
        /// <param name="fjfa"></param>
        /// <returns></returns>
        public DataSet GetDeptByProg(string fjfa)
        {
            string str = string.Format(@"SELECT a.dept_code,a.dept_name,c.RATIO FROM {0}.XYHS_DEPT_DICT a,{0}.XYHS_COST_APPOR_PROG_DICT b,{0}.XYHS_COST_APPOR_DEPT_DETAIL c
  WHERE a.show_flag='0'  and a.dept_code=c.DEPT_CODE and b.PROG_CODE=c.PROG_CODE AND  b.prog_code='{1}' order by a.dept_code", DataUser.CBHS, fjfa);
            return OracleOledbBase.ExecuteDataSet(str);
        }

        public DataSet GetDeptList(string deptCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT A.DEPT_CODE,
                                         A.DEPT_NAME,
                                         A.DEPT_TYPE,
                                         A.ATTR,
                                         A.INPUT_CODE,
                                         A.ACCOUNT_DEPT_CODE,
                                         A.ACCOUNT_DEPT_NAME,
                                         A.SORT_NO,
                                         NVL(A.SHOW_FLAG,0) SHOW_FLAG,
                                         NVL(A.PATIENT_DEPT_FLAGS,0) PATIENT_DEPT_FLAGS,
                                         NVL(A.COM_DEPT_FLAGS,0) COM_DEPT_FLAGS
                                  FROM   {0}.XYHS_DEPT_DICT A", DataUser.CBHS);
            if (deptCode != null && deptCode != "")
            {
                strSql.AppendFormat(" where A.DEPT_CODE ='{0}'", deptCode);
            }
            strSql.Append(" ORDER BY A.SORT_NO,A.DEPT_CODE");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        public DataSet GetDeptList(string deptType, string showFlag, string filter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT A.DEPT_CODE,
                                         A.DEPT_NAME,
                                        (SELECT XYHS_DEPT_TYPE FROM CBHS.XYHS_DEPT_TYPE_DICT C WHERE C.ID=A.DEPT_TYPE ) DEPT_TYPE,
                                         A.ATTR,a.PATIENT_DEPT_FLAGS,a.COM_DEPT_FLAGS,
                                         A.INPUT_CODE,
                                         A.ACCOUNT_DEPT_CODE,
                                         A.ACCOUNT_DEPT_NAME,
                                         NVL(A.SORT_NO,0) SORT_NO,
                                         A.SHOW_FLAG
                                  FROM   {0}.XYHS_DEPT_DICT A
                                 WHERE   0 = 0", DataUser.CBHS);
            if (deptType != null && deptType != "")
            {
                strSql.AppendFormat("  AND A.DEPT_TYPE={0}", deptType);
            }
            if (showFlag != null && showFlag != "")
            {
                strSql.AppendFormat(" AND NVL (A.SHOW_FLAG, 0)={0}", showFlag);
            }
            if (filter != null && filter != "")
            {
                strSql.AppendFormat(@" AND (A.DEPT_CODE LIKE '{0}%' 
                                         OR A.DEPT_NAME LIKE '{0}%' 
                                         OR A.INPUT_CODE LIKE '{0}%')", filter);
            }
            strSql.Append(" ORDER BY A.SORT_NO,A.DEPT_CODE");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获得分解方案字典表
        /// </summary>
        /// <returns></returns>
        public DataTable GetProgDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.*,b.PROG_NAME GUIDE_NAME from CBHS.XYHS_COST_APPOR_PROG_DICT a,cbhs.XYHS_COST_APPOR_PROG_SQL b where a.PROG_EXPRESS=b.PROG_ITEM", DataUser.CBHS);
            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }
        public DataTable GetProgDict(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select prog_code,prog_name,input_code,PROG_EXPRESS,flags,APPLY_DEPT
             from {0}.xyHS_COST_APPOR_PROG_DICT a where id=" + id + "", DataUser.CBHS);
            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得分解方案类别
        /// </summary>
        /// <returns></returns>
        public DataTable GetApporDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select * from {0}.XYHS_COST_APPOR_DICT ", DataUser.CBHS);
            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获得指标
        /// </summary>
        /// <returns></returns>
        public DataTable GetGuidesqlDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PROG_ITEM,PROG_NAME from {0}.XYHS_COST_APPOR_PROG_SQL ", DataUser.CBHS);
            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 增加一个分解方案
        /// </summary>
        /// <param name="progcode"></param>
        /// <param name="progname"></param>
        /// <param name="inputcode"></param>
        /// <param name="type"></param>
        public void InsertProgDict(string progcode, string progname, string inputcode, string type,string guidecode)
        {
            string sql = string.Format(@" insert into {0}.xyHS_COST_APPOR_PROG_DICT (ID,PROG_CODE,PROG_NAME,INPUT_CODE,FLAGS,PROG_EXPRESS) 
             select NVL(max(ID),0)+1,'" + progcode + "','" + progname + "','" + inputcode + "','" + type + "','{1}' from {0}.xyHS_COST_APPOR_PROG_DICT", DataUser.CBHS,guidecode);
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新分解方案
        /// </summary>
        public void UpateProgDict(string id, string progcode, string progname, string inputcode, string type,string guidecode)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] cmdParms = new OleDbParameter[] { };
            //更新分解方案比率的代码
            string sqlratio = string.Format(@"update {0}.xyHS_COST_APPOR_RATIO set PROG_CODE='" + progcode + "' where PROG_CODE=(select PROG_CODE from {0}.xyHS_COST_APPOR_PROG_DICT where ID=" + id + ")", DataUser.CBHS);
            List listratio = new List();
            listratio.StrSql = sqlratio.ToString();
            listratio.Parameters = cmdParms;
            listttrans.Add(listratio);
            ////更新分解方案值的代码
            //string sqlvalue = string.Format(@"update {0}.CBHS_COST_APPOR_PLUS set PROG_CODE='" + progcode + "' where PROG_CODE=(select PROG_CODE from {0}.CBHS_COST_APPOR_PROG_DICT where ID=" + id + ")", DataUser.CBHS);
            //List listvalue = new List();
            //listvalue.StrSql = sqlvalue.ToString();
            //listvalue.Parameters = cmdParms;
            //listttrans.Add(listvalue);
            //更新分解方案代码
            string sqlProg = string.Format(@" update {0}.xyHS_COST_APPOR_PROG_DICT set PROG_CODE='" + progcode + "',PROG_NAME='" + progname + "',INPUT_CODE='" + inputcode + "',FLAGS='" + type + "',PROG_EXPRESS='{1}' where ID=" + id + "", DataUser.CBHS,guidecode);
            List listpro = new List();
            listpro.StrSql = sqlProg.ToString();
            listpro.Parameters = cmdParms;
            listttrans.Add(listpro);

            OracleOledbBase.ExecuteTranslist(listttrans);
        }
        /// <summary>
        /// 删除分解方案
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProgDict(string id)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] cmdParms = new OleDbParameter[] { };
            //删除分解方案比率的代码
            string sqlratio = string.Format(@"delete from  {0}.xyHS_COST_APPOR_RATIO  where PROG_CODE=(select PROG_CODE from {0}.CBHS_COST_APPOR_PROG_DICT where ID=" + id + ")", DataUser.CBHS);
            List listratio = new List();
            listratio.StrSql = sqlratio.ToString();
            listratio.Parameters = cmdParms;
            listttrans.Add(listratio);
            ////删除分解方案值的代码
            //string sqlvalue = string.Format(@"delete from {0}.CBHS_COST_APPOR_PLUS where PROG_CODE=(select PROG_CODE from {0}.CBHS_COST_APPOR_PROG_DICT where ID=" + id + ")", DataUser.CBHS);
            //List listvalue = new List();
            //listvalue.StrSql = sqlvalue.ToString();
            //listvalue.Parameters = cmdParms;
            //listttrans.Add(listvalue);
            //删除分解方案代码
            string sqlProg = string.Format(@"delete from  {0}.xyHS_COST_APPOR_PROG_DICT where ID=" + id + "", DataUser.CBHS);
            List listpro = new List();
            listpro.StrSql = sqlProg.ToString();
            listpro.Parameters = cmdParms;
            listttrans.Add(listpro);

            OracleOledbBase.ExecuteTranslist(listttrans);
        }
        /// <summary>
        /// 从试图查询科室成本
        /// </summary>
        /// <param name="date_itme">日期（201012）</param>
        /// <returns>DataSet</returns>
        public DataSet GetTotalCostsFromViews(string date_itme)
        {
            string strSql = string.Format(@"SELECT   DATE_TIME,ITEM_CODE, DEPT_CODE, COSTS
                                              FROM  {0}.V_XYHS_DEPT_COSTS_EXT
                                             WHERE  DATE_TIME='{1}'", DataUser.CBHS, date_itme);
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        /// <summary>
        /// 保存科级成本
        /// </summary>
        /// <param name="date_time"></param>
        public void SaveEXPORT_COSTS(string date_time)
        {
            MyLists listtable = new MyLists();
            string strDelSql = string.Format(@"DELETE FROM {0}.XYHS_DEPT_COSTS_EXT WHERE TO_CHAR(DATE_TIME,'yyyymm')='{1}'", DataUser.CBHS, date_time);
            List listDel = new List();
            OleDbParameter[] parameterdel = new OleDbParameter[] { };
            listDel.StrSql = strDelSql;
            listDel.Parameters = parameterdel;
            listtable.Add(listDel);

            StringBuilder strAddSql = new StringBuilder();
            strAddSql.AppendFormat(@"insert into {0}.XYHS_DEPT_COSTS_EXT(DEPT_CODE, ITEM_CODE, DATE_TIME, COSTS,COST_TYPE) 
SELECT E.DEPT_CODE,E.ITEM_CODE, DATE_TIME,E.COSTS,0 as COST_TYPE  FROM {0}.V_XYHS_DEPT_COSTS_EXT E WHERE TO_CHAR(DATE_TIME,'yyyymm')='{1}'", DataUser.CBHS, date_time);
            OleDbParameter[] parameteradd = new OleDbParameter[] { };
            List listAdd = new List();
            listAdd.StrSql = strAddSql;
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);

            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 科室成本归集
        /// </summary>
        /// <param name="accountdate"></param>
        /// <returns></returns>
        public string DeptCost_Union(string accountdate)
        {

            string ProcName = DataUser.CBHS + ".XYHS_DEPT_COST_UNION";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", accountdate),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }

    }
}
