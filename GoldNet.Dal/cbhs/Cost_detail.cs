using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm.DAL.SqlServer;
using GoldNet.Comm;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class Cost_detail
    {
        public Cost_detail()
        {

        }

        #region 单项成本
        /// <summary>
        /// 获取单项成本录入列表
        /// </summary>
        /// <param name="balance_tag">结算标识</param>
        /// <param name="item_code">成本代码</param>
        /// <param name="date_time">时间（201012）</param>
        /// <returns>DataSet</returns>
        public DataSet GetSingleCost(string item_code, string date_time, string gettype, string progcode, string cbbtype, string deptcode)
        {
            string dept = "";
            if (deptcode != "")
            {
                dept = deptcode;
            }

            //成本分解前的成本数据，来自单项成本录入
            string tb = "CBHS_DEPT_COST_DETAIL_COMMIT";

            if (cbbtype == "1")
            {
                //成本分解后数据表
                tb = "CBHS_DEPT_COST_DETAIL";
            }
            progcode = progcode == "全部" ? "" : progcode;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DEPT_CODE, DECODE (DEPT_CODE,'', ' 合计',MIN (DEPT_NAME)) DEPT_NAME, SUM (TOTAL_COSTS) TOTAL_COSTS,
                                         SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,
                                         DECODE (DEPT_CODE, '', '', MIN (MEMO)) MEMO,CASE WHEN DEPT_CODE='{5}' THEN '1' ELSE '0' END FLAG
                                    FROM (SELECT   B.DEPT_CODE DEPT_CODE, B.DEPT_NAME dept_name,
                                                   SUM (COSTS + COSTS_ARMYFREE) AS TOTAL_COSTS,
                                                   SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,MEMO,B.SORT_NO
                                              FROM {0}.{4} A, {1}.SYS_DEPT_DICT B
                                             WHERE A.DEPT_CODE(+) = B.DEPT_CODE
                                               and b.account_dept_code is not null
                                               AND GET_TYPE(+)='{2}'
                                               AND ITEM_CODE(+) = '{3}' and b.SHOW_FLAG='0'", DataUser.CBHS, DataUser.COMM, gettype, item_code, tb, dept);
            if (!progcode.Equals(string.Empty))
            {
                strSql.AppendFormat(" and a.prog_code(+)='{0}'", progcode);
            }
            strSql.AppendFormat(@" AND ACCOUNTING_DATE(+) = date'{0}'
                                          GROUP BY B.DEPT_CODE, B.DEPT_NAME, MEMO,B.SORT_NO
                                          ORDER BY B.SORT_NO,B.DEPT_CODE)
                                GROUP BY CUBE (DEPT_CODE)
                                ORDER BY DEPT_NAME", date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        /// <param name="gettype"></param>
        /// <param name="progcode"></param>
        /// <param name="cbbtype"></param>
        /// <returns></returns>
        public DataSet GetSingleCost(string item_code, string date_time, string gettype, string progcode, string cbbtype)
        {

            //成本分解前的成本数据，来自单项成本录入
            string tb = "CBHS_DEPT_COST_DETAIL_COMMIT";

            if (cbbtype == "1")
            {
                //成本分解后数据表
                tb = "CBHS_DEPT_COST_DETAIL";
            }
            progcode = progcode == "全部" ? "" : progcode;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   A.ROWID,B.DEPT_CODE DEPT_CODE, 
                                                   B.DEPT_NAME dept_name,
                                                   COSTS + COSTS_ARMYFREE AS TOTAL_COSTS,
                                                   COSTS COSTS, 
                                                   COSTS_ARMYFREE COSTS_ARMYFREE,
                                                   MEMO,
                                                   B.SORT_NO,
                                                   A.GET_TYPE,
                                                   DECODE(GET_TYPE,'0','录入','1','提取','分解') GET_NAME
                                              FROM {0}.{4} A, {1}.SYS_DEPT_DICT B
                                             WHERE A.DEPT_CODE = B.DEPT_CODE
                                               AND B.account_dept_code is not null
                                               AND A.GET_TYPE in ('0','1','3')
                                               AND A.ITEM_CODE = '{3}' 
                                               AND B.SHOW_FLAG='0'", DataUser.CBHS, DataUser.COMM, gettype, item_code, tb);
            if (!progcode.Equals(string.Empty))
            {
                strSql.AppendFormat(" and a.prog_code(+)='{0}'", progcode);
            }
            strSql.AppendFormat(@" AND ACCOUNTING_DATE(+) = date'{0}'
                                          ORDER BY B.DEPT_NAME ", date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        /// <param name="gettype"></param>
        /// <param name="progcode"></param>
        /// <param name="cbbtype"></param>
        /// <returns></returns>
        public DataSet GetSingleCostdecompose(string item_code, string date_time, string gettype, string progcode, string cbbtype)
        {
            string tb = "CBHS_DEPT_COST_DETAIL_COMMIT";
            if (cbbtype == "1")
            {
                tb = "CBHS_DEPT_COST_DETAIL";
            }
            progcode = progcode == "全部" ? "" : progcode;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DEPT_CODE, DECODE (DEPT_CODE,'', '合计',MIN (DEPT_NAME)) DEPT_NAME, SUM (TOTAL_COSTS) TOTAL_COSTS,
                                         SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,
                                         DECODE (DEPT_CODE, '', '', MIN (MEMO)) MEMO
                                    FROM (SELECT   B.DEPT_CODE DEPT_CODE, B.DEPT_NAME dept_name,
                                                   SUM (COSTS + COSTS_ARMYFREE) AS TOTAL_COSTS,
                                                   SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,MEMO,B.SORT_NO
                                              FROM {0}.{4} A, {1}.SYS_DEPT_DICT B
                                             WHERE A.DEPT_CODE = B.DEPT_CODE
                                               and b.account_dept_code is not null
                                               AND GET_TYPE='{2}'
                                               AND ITEM_CODE = '{3}' ", DataUser.CBHS, DataUser.COMM, gettype, item_code, tb);
            if (!progcode.Equals(string.Empty))
            {
                strSql.AppendFormat(" and a.prog_code='{0}'", progcode);
            }
            strSql.AppendFormat(@" AND ACCOUNTING_DATE = date'{0}'
                                          GROUP BY B.DEPT_CODE, B.DEPT_NAME, MEMO,B.SORT_NO
                                          ORDER BY B.SORT_NO,B.DEPT_CODE)
                                GROUP BY CUBE (DEPT_CODE)
                                ORDER BY DECODE(DEPT_CODE,NULL,-1,MIN(SORT_NO)),DEPT_CODE", date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 根据成本项目查询分摊方案
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public DataSet GetProgByCostItem(string itemcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select a.PROG_CODE, '默认分解方案' PROG_NAME from {0}.CBHS_COST_APPOR_PROG_DICT a,{0}.CBHS_COST_ITEM_DICT b where a.PROG_CODE=b.ALLOT_FOR_JC and b.item_code='{1}'", DataUser.CBHS, itemcode);
            str.AppendFormat(" union all select a.PROG_CODE,a.PROG_NAME from {0}.CBHS_COST_APPOR_PROG_DICT a,{0}.CBHS_COST_APPOR_COST_DETAIL b where a.PROG_CODE=b.PROG_CODE and b.ITEM_CLASS='{1}'", DataUser.CBHS, itemcode);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 保存界面成本到数据库
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        /// <param name="operators"></param>
        /// <param name="gettype">成本来源：0录入1提取3分解</param>
        /// <param name="fjfa"></param>
        public void SaveSingleCosts(Dictionary<string, string>[] costdetails, string item_code, string date_time, string operators, string gettype, string fjfa)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();

            //删除已录入的成本
            strDel.AppendFormat("DELETE FROM {0}.CBHS_DEPT_COST_DETAIL_COMMIT WHERE ACCOUNTING_DATE=date'{2}' AND ITEM_CODE=? AND GET_TYPE='{1}'", DataUser.CBHS, gettype, date_time);
            if (!fjfa.Equals(string.Empty))
            {
                strDel.AppendFormat(" and PROG_CODE='{0}'", fjfa);
            }
            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { new OleDbParameter("item_code", item_code) };
            listtable.Add(listDel);

            for (int i = 0; i < costdetails.Length; i++)
            {
                if (costdetails[i]["TOTAL_COSTS"] == null || costdetails[i]["TOTAL_COSTS"].Equals("") || costdetails[i]["DEPT_CODE"].Equals(""))
                {
                    continue;
                }

                //if (costdetails[i]["GET_TYPE"] == null )
                //{
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendFormat(@"INSERT INTO {0}.CBHS_DEPT_COST_DETAIL_COMMIT 
                                          (DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,COSTS_ARMYFREE,
                                           OPERATOR,OPERATOR_DATE,GET_TYPE,COST_FLAG,BALANCE_TAG,DEPT_TYPE_FLAG,MEMO,PROG_CODE)
                                        VALUES(?,?,?,?,?,?,SYSDATE,?,?,?,?,?,?)", DataUser.CBHS, date_time);
                    //double jdprog = GetDeptJdProgInfo(date_time, costdetail.Dept_code, item_code);
                    double jdprog = 1;
                    double costs = Convert.ToDouble(costdetails[i]["TOTAL_COSTS"]) * jdprog;
                    double costs_armyfree = Convert.ToDouble(costdetails[i]["TOTAL_COSTS"]) - costs;
                    OleDbParameter[] parameteradd = {
											  new OleDbParameter("dept_code",costdetails[i]["DEPT_CODE"] ),
											  new OleDbParameter("item_code",item_code),
											  new OleDbParameter("accounting_date",Convert.ToDateTime(date_time)),
											  new OleDbParameter("costs",costs),
											  new OleDbParameter("costs_armyfree",costs_armyfree),
											  new OleDbParameter("OPERATOR",operators),
											  new OleDbParameter("get_type",gettype),
											  new OleDbParameter("cost_flag",""),
											  new OleDbParameter("balance_tag",""),
											  new OleDbParameter("dept_type_flag",""),
											  new OleDbParameter("memo",costdetails[i]["MEMO"]),
											  new OleDbParameter("PROG_CODE",fjfa)
										  };
                    List listAdd = new List();
                    listAdd.StrSql = strSql;
                    listAdd.Parameters = parameteradd;
                    listtable.Add(listAdd);
                //}
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        /// <param name="operators"></param>
        /// <param name="gettype"></param>
        /// <param name="fjfa"></param>
        public void SaveSingleCosts(List<GoldNet.Model.CostDetail> costdetails, string item_code, string date_time, string operators, string gettype, string fjfa)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.CBHS_DEPT_COST_DETAIL_COMMIT WHERE ACCOUNTING_DATE=date'{2}' AND ITEM_CODE=? AND GET_TYPE='{1}'", DataUser.CBHS, gettype, date_time);
            if (!fjfa.Equals(string.Empty))
            {
                strDel.AppendFormat(" and PROG_CODE='{0}'", fjfa);
            }
            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { new OleDbParameter("item_code", item_code) };
            listtable.Add(listDel);

            foreach (GoldNet.Model.CostDetail costdetail in costdetails)
            {
                if (costdetail.Total_costs == null || costdetail.Total_costs.Equals("") || costdetail.Dept_code.Equals(""))
                {
                    continue;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.CBHS_DEPT_COST_DETAIL_COMMIT 
                                          (DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,COSTS_ARMYFREE,
                                           OPERATOR,OPERATOR_DATE,GET_TYPE,COST_FLAG,BALANCE_TAG,DEPT_TYPE_FLAG,MEMO,PROG_CODE)
                                        VALUES(?,?,?,?,?,?,SYSDATE,?,?,?,?,?,?)", DataUser.CBHS, date_time);
                //double jdprog = GetDeptJdProgInfo(date_time, costdetail.Dept_code, item_code);
                double jdprog = 1;
                double costs = Convert.ToDouble(costdetail.Total_costs) * jdprog;
                double costs_armyfree = Convert.ToDouble(costdetail.Total_costs) - costs;
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("dept_code",costdetail.Dept_code),
											  new OleDbParameter("item_code",item_code),
											  new OleDbParameter("accounting_date",Convert.ToDateTime(date_time)),
											  new OleDbParameter("costs",costs),
											  new OleDbParameter("costs_armyfree",costs_armyfree),
											  new OleDbParameter("OPERATOR",operators),
											  new OleDbParameter("get_type",gettype),//0为录入
											  new OleDbParameter("cost_flag",""),
											  new OleDbParameter("balance_tag",""),
											  new OleDbParameter("dept_type_flag",""),
											  new OleDbParameter("memo",costdetail.Memo),
											  new OleDbParameter("PROG_CODE",fjfa)
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        /// <param name="operators"></param>
        /// <param name="gettype"></param>
        /// <param name="fjfa"></param>
        public void SaveSingleCostsdecompose(List<GoldNet.Model.CostDetail> costdetails, string item_code, string date_time, string operators, string gettype, string fjfa)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.CBHS_DEPT_COST_DETAIL_COMMIT WHERE ACCOUNTING_DATE=date'{2}' AND ITEM_CODE=? AND GET_TYPE='{1}'", DataUser.CBHS, gettype, date_time);
            if (!fjfa.Equals(string.Empty))
            {
                strDel.AppendFormat(" and PROG_CODE='{0}'", fjfa);
            }
            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { new OleDbParameter("item_code", item_code) };
            listtable.Add(listDel);

            foreach (GoldNet.Model.CostDetail costdetail in costdetails)
            {
                if (costdetail.Total_costs == null || costdetail.Total_costs.Equals("") || costdetail.Dept_code.Equals(""))
                {
                    continue;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.CBHS_DEPT_COST_DETAIL_COMMIT
                                          (DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,COSTS_ARMYFREE,
                                           OPERATOR,OPERATOR_DATE,GET_TYPE,COST_FLAG,BALANCE_TAG,DEPT_TYPE_FLAG,MEMO,PROG_CODE)
                                        VALUES(?,?,?,?,?,?,SYSDATE,?,?,?,?,?,?)", DataUser.CBHS, date_time);
                //double jdprog = GetDeptJdProgInfo(date_time, costdetail.Dept_code, item_code);
                double jdprog = 1;
                double costs = Convert.ToDouble(costdetail.Total_costs) * jdprog;
                double costs_armyfree = Convert.ToDouble(costdetail.Total_costs) - costs;
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("dept_code",costdetail.Dept_code),
											  new OleDbParameter("item_code",item_code),
											  new OleDbParameter("accounting_date",Convert.ToDateTime(date_time)),
											  new OleDbParameter("costs",costdetail.Costs),
											  new OleDbParameter("costs_armyfree",costdetail.Costs_armyfree),
											  new OleDbParameter("OPERATOR",operators),
											  new OleDbParameter("get_type",gettype),//0为录入
											  new OleDbParameter("cost_flag",""),
											  new OleDbParameter("balance_tag",""),
											  new OleDbParameter("dept_type_flag",""),
											  new OleDbParameter("memo",costdetail.Memo),
											  new OleDbParameter("PROG_CODE",fjfa)
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 单项目分解
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        /// <param name="operators"></param>
        /// <param name="gettype"></param>
        public void SaveDecomposeCosts(List<GoldNet.Model.CostDetail> costdetails, string item_code, string date_time, string operators, string gettype, string deptcode)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.CBHS_DEPT_COST_DETAIL WHERE ACCOUNTING_DATE=date'{2}' AND ITEM_CODE=? AND GET_TYPE='{1}' and dept_code='{3}'", DataUser.CBHS, gettype, date_time, deptcode);

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { new OleDbParameter("item_code", item_code) };
            listtable.Add(listDel);

            foreach (GoldNet.Model.CostDetail costdetail in costdetails)
            {
                if (costdetail.Total_costs == null || costdetail.Total_costs.Equals("") || costdetail.Dept_code.Equals(""))
                {
                    continue;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.CBHS_DEPT_COST_DETAIL 
                                          (DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,COSTS_ARMYFREE,
                                           OPERATOR,OPERATOR_DATE,GET_TYPE,COST_FLAG,BALANCE_TAG,DEPT_TYPE_FLAG,MEMO)
                                        VALUES(?,?,?,?,?,?,SYSDATE,?,?,?,?,?)", DataUser.CBHS, date_time);
                double jdprog = GetDeptJdProgInfo(date_time, costdetail.Dept_code, item_code);
                double costs = Convert.ToDouble(costdetail.Total_costs) * jdprog;
                double costs_armyfree = Convert.ToDouble(costdetail.Total_costs) - costs;
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("dept_code",costdetail.Dept_code),
											  new OleDbParameter("item_code",item_code),
											  new OleDbParameter("accounting_date",Convert.ToDateTime(date_time)),
											  new OleDbParameter("costs",costs),
											  new OleDbParameter("costs_armyfree",costs_armyfree),
											  new OleDbParameter("OPERATOR",operators),
											  new OleDbParameter("get_type",gettype),
											  new OleDbParameter("cost_flag",""),
											  new OleDbParameter("balance_tag",""),
											  new OleDbParameter("dept_type_flag",""),
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
        /// 获取科室成本军地分解方案
        /// </summary>
        /// <param name="date_time">时间（201012）</param>
        /// <param name="dept_code">科室代码</param>
        /// <param name="item_code">项目代码</param>
        /// <returns>地方成本比例</returns>
        public double GetDeptJdProgInfo(string date_time, string dept_code, string item_code)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendFormat(@"SELECT ITEM_CODE, UNIT_CODE, PROG_CODE, RATIO INCOMES_NUMBER, 1-RATIO AS CHANGERS_NUMBER
                                    FROM {0}.CBHS_COST_ITEM_DICT A, {0}.CBHS_COST_APPOR_RATIO B
                                 WHERE A.ALLOT_FOR_JD = B.PROG_CODE
                                   AND TO_CHAR(DATE_TIME,'yyyy-mm-dd')='{1}' AND UNIT_CODE ='{2}' AND ITEM_CODE='{3}' ", DataUser.CBHS, date_time, dept_code, item_code);
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDouble(dt.Rows[0]["INCOMES_NUMBER"]);
            }
            else
            {
                return GetHosJdProgInfo(date_time, item_code);
            }
        }
        /// <summary>
        /// 获取院级军地分解方案
        /// </summary>
        /// <param name="date_time">时间（201012）</param>
        /// <param name="item_code">成本项目代码</param>
        /// <returns>地方成本比例</returns>
        protected double GetHosJdProgInfo(string date_time, string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT ITEM_CODE, UNIT_CODE, PROG_CODE, RATIO INCOMES_NUMBER, 1-RATIO AS CHANGERS_NUMBER
                                    FROM {0}.CBHS_COST_ITEM_DICT A, {0}.CBHS_COST_APPOR_RATIO B
                                 WHERE A.ALLOT_FOR_JD = B.PROG_CODE
                                   AND TO_CHAR(DATE_TIME,'yyyymm')='{1}' AND UNIT_CODE ='00' AND ITEM_CODE='{2}' ", DataUser.CBHS, date_time, item_code);
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDouble(dt.Rows[0]["INCOMES_NUMBER"]);
            }
            else
            {
                return 1.0;
            }
        }

        /// <summary>
        /// 删除单行录入的成本项目
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="item_code"></param>
        /// <param name="dept_code"></param>
        /// <param name="gettype">成本来源：0录入1提取3分解</param>
        /// <param name="progcode"></param>
        public void DelSingleCosts(string date_time, string item_code, string dept_code, string gettype, string progcode)
        {
            StringBuilder strSql = new StringBuilder();
//            strSql.AppendFormat(@"DELETE FROM {0}.CBHS_DEPT_COST_DETAIL_COMMIT
//                                  WHERE ACCOUNTING_DATE = date'{1}'
//                                    AND ITEM_CODE = '{2}' and get_type='{3}'", DataUser.CBHS, date_time, item_code, gettype);
            strSql.AppendFormat(@"DELETE FROM {0}.CBHS_DEPT_COST_DETAIL_COMMIT
                                  WHERE ACCOUNTING_DATE = date'{1}'
                                    AND ITEM_CODE = '{2}' ", DataUser.CBHS, date_time, item_code);
            if (dept_code != null && !dept_code.Equals(""))
            {
                strSql.AppendFormat(" AND DEPT_CODE = '{0}'", dept_code);
            }
            if (progcode != "")
            {
                strSql.AppendFormat(" and PROG_CODE='{0}'", progcode);
            }
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }

        /// <summary>
        /// 删除单行录入的成本项目
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="item_code"></param>
        /// <param name="dept_code"></param>
        /// <param name="progcode"></param>
        public void Del_Jjdr(string date_time, string user_id, string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            
            strSql.AppendFormat(@"DELETE FROM performance.JJ_DR
                                  WHERE st_date = '{0}'
                                    AND user_id = '{1}' ", date_time,user_id);
            if (dept_code != null && !dept_code.Equals(""))
            {
                strSql.AppendFormat(" AND DEPT_CODE = '{0}'", dept_code);
            }
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }

        /// <summary>
        /// 删除单行录入的成本项目
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="item_code"></param>
        /// <param name="dept_code"></param>
        /// <param name="gettype">成本来源：0录入1提取3分解</param>
        /// <param name="progcode"></param>
        public void DelSingleCosts_hou(string date_time, string item_code, string dept_code, string gettype, string progcode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"DELETE FROM {0}.CBHS_DEPT_COST_DETAIL
                                  WHERE ACCOUNTING_DATE = date'{1}'
                                    AND ITEM_CODE = '{2}' ", DataUser.CBHS, date_time, item_code);
            if (dept_code != null && !dept_code.Equals(""))
            {
                strSql.AppendFormat(" AND DEPT_CODE = '{0}'", dept_code);
            }
            if (progcode != "")
            {
                strSql.AppendFormat(" and PROG_CODE='{0}'", progcode);
            }
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="item_code"></param>
        /// <param name="dept_code"></param>
        /// <param name="gettype"></param>
        /// <param name="progcode"></param>
        public void DelSingleCostsp(string date_time, string item_code, string dept_code, string gettype, string progcode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"DELETE FROM {0}.CBHS_DEPT_COST_DETAIL_COMMIT
                                  WHERE ACCOUNTING_DATE = date'{1}'
                                    AND ITEM_CODE = '{2}' and get_type='{3}'", DataUser.CBHS, date_time, item_code, gettype);
            if (dept_code != null && !dept_code.Equals(""))
            {
                strSql.AppendFormat(" AND DEPT_CODE = '{0}'", dept_code);
            }
            if (progcode != "")
            {
                strSql.AppendFormat(" and PROG_CODE='{0}'", progcode);
            }
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }

        /// <summary>
        /// 执行结算分解方案基础比例过程
        /// </summary>
        /// <param name="date_time">时间（201012）</param>
        /// <returns>执行结果</returns>
        public void Exec_Sp_Prog_Ratio(string tjny)
        {
            string ProcName = DataUser.CBHS + ".SP_PROG_RATIO";
            OleDbParameter[] parameteradd = { new OleDbParameter("tjny", tjny) };
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
        }

        /// <summary>
        /// 执行成本分摊过程
        /// </summary>
        /// <param name="accountdate">日期（201012）</param>
        /// <param name="costitem">成本项目代码</param>
        /// <param name="costvalue">成本总额</param>
        /// <returns>执行结果</returns>
        public string Exec_Sp_Extract_Cost_Input(string progcode, string accountdate, string costitem, double costvalue)
        {
            progcode = progcode == "全部" ? "" : progcode;
            string ProcName = DataUser.CBHS + ".SP_EXTRACT_COST_INPUT";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("progcode", progcode),
                                              new OleDbParameter("accountdate", accountdate),
                                              new OleDbParameter("costitem",costitem),
                                              new OleDbParameter("costvalue",costvalue),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountdate"></param>
        /// <returns></returns>
        public string Exec_sp_extract_hospital_cost(string accountdate)
        {
            string ProcName = DataUser.CBHS + ".sp_extract_hospital_cost";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { 
                                              new OleDbParameter("accountdate", accountdate),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }

        /// <summary>
        /// 查询单项成本录入临时表
        /// </summary>
        /// <returns></returns>
        public DataSet GetSingleCostTmp()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DEPT_CODE, DECODE (DEPT_CODE,'', '合计',MIN (DEPT_NAME)) DEPT_NAME, SUM (TOTAL_COSTS) TOTAL_COSTS,
                                         SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,
                                         DECODE (DEPT_CODE, '', '', MIN (MEMO)) MEMO
                                    FROM (SELECT  B.ACCOUNT_DEPT_CODE DEPT_CODE, B.ACCOUNT_DEPT_NAME DEPT_NAME,
                                                   SUM (COSTS + COSTS_ARMYFREE) TOTAL_COSTS, SUM (COSTS) COSTS,
                                                   SUM (COSTS_ARMYFREE) COSTS_ARMYFREE, MEMO
                                              FROM {0}.CBHS_DEPT_COST_DETAIL_TMP A, {1}.SYS_DEPT_DICT B
                                             WHERE A.DEPT_CODE(+) = B.DEPT_CODE and B.ACCOUNT_DEPT_CODE is not null 
                                          GROUP BY B.ACCOUNT_DEPT_CODE, B.ACCOUNT_DEPT_NAME, MEMO
                                          ORDER BY B.ACCOUNT_DEPT_CODE)
                                GROUP BY CUBE (DEPT_CODE)", DataUser.CBHS, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetDecomposeDetail()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DEPT_CODE, DECODE (DEPT_CODE,'', '合计',MIN (DEPT_NAME)) DEPT_NAME, SUM (TOTAL_COSTS) TOTAL_COSTS,
                                         SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,
                                         DECODE (DEPT_CODE, '', '', MIN (MEMO)) MEMO
                                    FROM (SELECT  B.ACCOUNT_DEPT_CODE DEPT_CODE, B.ACCOUNT_DEPT_NAME DEPT_NAME,
                                                   SUM (COSTS + COSTS_ARMYFREE) TOTAL_COSTS, SUM (COSTS) COSTS,
                                                   SUM (COSTS_ARMYFREE) COSTS_ARMYFREE, MEMO
                                              FROM {0}.CBHS_DEPT_COST_DETAIL_TMP A, {1}.SYS_DEPT_DICT B
                                             WHERE A.DEPT_CODE = B.DEPT_CODE and B.ACCOUNT_DEPT_CODE is not null 
                                          GROUP BY B.ACCOUNT_DEPT_CODE, B.ACCOUNT_DEPT_NAME, MEMO
                                          ORDER BY B.ACCOUNT_DEPT_CODE)
                                GROUP BY CUBE (DEPT_CODE)", DataUser.CBHS, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        #endregion 单项成本
        #region 成本提取
        /// <summary>
        /// 获取提取的成本
        /// </summary>
        /// <param name="dept_code">科室代码（为空时查询全院）</param>
        /// <param name="date_time">日期（201012）</param>
        public DataSet GetDeptCosts(string dept_code, string date_time, string userid)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            sqlWhere.Append(" AND 1=1");
            if (dept_code != null && !dept_code.Equals(""))
            {
                sqlWhere.AppendFormat(" AND ACCOUNT_DEPT_CODE='{0}'", dept_code);
            }

            strSql.AppendFormat(@"SELECT ITEM_CODE, DECODE (ITEM_CODE,'', '合计',MIN (ITEM_NAME)) ITEM_NAME, SUM (TOTAL_COSTS) TOTAL_COSTS,
                                         SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,
                                         DECODE (ITEM_CODE, '', '', MIN (MEMO)) MEMO
                                   FROM (SELECT   M2.ITEM_CODE, M2.ITEM_NAME,
                                                 SUM (M1.COSTS + M1.COSTS_ARMYFREE) TOTAL_COSTS, SUM (M1.COSTS) COSTS,
                                                 SUM (M1.COSTS_ARMYFREE) COSTS_ARMYFREE, MEMO
                                            FROM (SELECT A.ITEM_CODE, A.COSTS, A.COSTS_ARMYFREE, A.MEMO, A.GET_TYPE,
                                                         A.ACCOUNTING_DATE
                                                    FROM {0}.CBHS_DEPT_COST_DETAIL_COMMIT A, {1}.SYS_DEPT_DICT B
                                                   WHERE A.DEPT_CODE = B.DEPT_CODE
                                                     AND TO_CHAR (ACCOUNTING_DATE(+),'yyyymm') = '{2}'
                                                     AND GET_TYPE in('1','6')  {3}) M1,
                                                 {0}.CBHS_COST_ITEM_DICT M2
                                           WHERE M1.ITEM_CODE(+) = M2.ITEM_CODE and M2.ITEM_CODE IN (SELECT  a.item_code
                                                       FROM {0}.cbhs_cost_item_dict a,
                                                            {1}.sys_power_detail b,
                                                            {1}.sys_specpower_role c
                                                      WHERE a.item_code = c.ID
                                                        AND c.TYPE = 2
                                                        AND c.role_id = b.power_id
                                                        AND UPPER (b.target_id) = '{4}')
                                        GROUP BY M2.ITEM_CODE, M2.ITEM_NAME, MEMO
                                        ORDER BY M2.ITEM_CODE)
                                  GROUP BY CUBE (ITEM_CODE)", DataUser.CBHS, DataUser.COMM, date_time, sqlWhere.ToString(), userid);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 科室归集成本
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="gettype"></param>
        /// <returns></returns>
        public DataTable GetManualCostList(string datetime, string accounttype)
        {
            int numbers = accounttype == "1" ? -1 : 1;
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT b.account_dept_code dept_code,  DECODE (b.account_dept_code,'', '合计',MIN (b.account_dept_name)) dept_name, SUM ({0}*costs) costs, SUM ({0}*costs_armyfree) costs_armyfree
  FROM cbhs.cbhs_dept_cost_detail a, comm.sys_dept_dict b
 WHERE TO_CHAR(a.ACCOUNTING_DATE,'yyyymm')='{1}'  and  a.dept_code = b.dept_code and a.get_type=5 and a.ACCOUNT_FLAGS={2}", numbers, datetime, accounttype);
            str.AppendFormat(@" GROUP BY CUBE (b.account_dept_code)
 order by b.account_dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        /// <summary>
        /// 科室归集分解
        /// </summary>
        /// <param name="date_time"></param>
        public void Exec_Sp_Manual_Cost_Pre(string date_time)
        {
            string ProcName = DataUser.CBHS + ".SP_DEPTCOST_TO_DEPT";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);

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
                    AND UPPER (b.target_id) = UPPER('{3}')
               ORDER BY a.item_code) a left join 
               {0}.CBHS_COSTS_SUBMIT b on a.item_code=b.ITEM_CODE and b.MONTHS='{4}' order by item_code", DataUser.CBHS, DataUser.COMM, Constant.COST_SPEPOWER, userid, months);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
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
            strDel.AppendFormat("DELETE FROM {0}.CBHS_COSTS_SUBMIT WHERE MONTHS=? AND ITEM_CODE=? ", DataUser.CBHS);

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { new OleDbParameter("", months), new OleDbParameter("", itemcode) };
            listtable.Add(listDel);
            //
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"insert into {0}.CBHS_COSTS_SUBMIT(MONTHS,ITEM_CODE,FLAGS,SUBMIT_PERSONS,SUBMIT_DATE) values (?,?,?,?,SYSDATE)", DataUser.CBHS);
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
            str.AppendFormat(@"update {0}.CBHS_COSTS_SUBMIT set CHECK_FLAGS=?,CHECK_NAME=? where months=? and item_code=?", DataUser.CBHS);
            OleDbParameter[] parameteradd = {
                                              new OleDbParameter("CHECK_FLAGS",'1'),
											  new OleDbParameter("CHECK_NAME",opertor),
											  new OleDbParameter("MONTHS",months),
											  new OleDbParameter("ITEM_CODE",itemcode)  
										  };

            OracleOledbBase.ExecuteNonQuery(str.ToString(), parameteradd);

        }
        /// <summary>
        /// 复合成本
        /// </summary>
        /// <param name="months"></param>
        /// <param name="itemcode"></param>
        /// <param name="opertor"></param>
        public void Compcost(string months, string itemcode, string opertor)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"update {0}.CBHS_COSTS_SUBMIT set COMP_FLAGS=?,COMP_NAME=? where months=? and item_code=?", DataUser.CBHS);
            OleDbParameter[] parameteradd = {
                                              new OleDbParameter("COMP_FLAGS",'1'),
											  new OleDbParameter("COMP_NAME",opertor),
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
            str.AppendFormat(@"DELETE FROM {0}.CBHS_COSTS_SUBMIT WHERE MONTHS=? AND ITEM_CODE=? ", DataUser.CBHS);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("MONTHS",months),
											  new OleDbParameter("ITEM_CODE",itemcode)  
										  };

            OracleOledbBase.ExecuteNonQuery(str.ToString(), parameteradd);

        }
        /// <summary>
        /// 成本是否提交
        /// </summary>
        /// <param name="months"></param>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public bool GetIsSubmit(string months, string itemcode)
        {
            string str = string.Format("select * from {0}.CBHS_COSTS_SUBMIT where MONTHS=? AND ITEM_CODE=? ", DataUser.CBHS);
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
        /// 
        /// </summary>
        /// <param name="months"></param>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public bool GetIsSave(string months, string itemcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select * from cbhs.CBHS_DEPT_COST_DETAIL_COMMIT where item_code='{0}' and accounting_date = to_date('{1}','yyyymmdd') ", itemcode, months + "01");
            OleDbParameter[] parameteradd = {

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
        /// 删除提取成本
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="item_code">成本项目代码</param>
        public void DelDeptCosts(string dept_code, string date_time, string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"DELETE FROM {0}.CBHS_DEPT_COST_DETAIL_COMMIT 
                                   WHERE TO_CHAR (ACCOUNTING_DATE,'yyyymm') = '{1}'
                                     AND GET_TYPE='1' ", DataUser.CBHS, date_time);
            if (dept_code != null && !dept_code.Equals(""))
            {
                strSql.AppendFormat(" AND DEPT_CODE IN (SELECT DEPT_CODE FROM COMM.SYS_DEPT_DICT WHERE ACCOUNT_DEPT_CODE = '{0}')", dept_code);
            }
            if (item_code != null && !item_code.Equals(""))
            {
                strSql.AppendFormat(" AND ITEM_CODE='{0}'", item_code);
            }
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }

        /// <summary>
        /// 成本预提
        /// </summary>
        /// <param name="accountdate">日期(201012)</param>
        /// <param name="opuser">操作员</param>
        /// <returns>执行结果</returns>
        public string Exec_Sp_Extract_Cost_Pre(string date_time, string opuser, string userid)
        {
            string ProcName = DataUser.CBHS + ".SP_EXTRACT_COST_PRE";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter rtnno = new OleDbParameter("rtnno", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnno.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time),
                                              new OleDbParameter("opuser",opuser),
                                              new OleDbParameter("userid",userid),
                                              rtnno,
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }

        /// <summary>
        /// 执行成本确定提取过程
        /// </summary>
        /// <param name="accountdate">日期（201012）</param>
        /// <param name="opuser">操作员</param>
        /// <param name="opno">预提次数</param>
        /// <returns>执行结果</returns>
        public string Exec_Sp_Extract_Cost(string date_time, string opuser, string userid, string opno)
        {
            string ProcName = DataUser.CBHS + ".SP_EXTRACT_COST";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time),
                                              new OleDbParameter("opuser",userid),
                                              new OleDbParameter("userid",userid),
                                              new OleDbParameter("opno",opno),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }

        /// <summary>
        /// 单项成本分解
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="opuser"></param>
        /// <param name="userid"></param>
        /// <param name="opno"></param>
        /// <returns></returns>
        public string Exec_Sp_Extract_Cost_commit(string date_time, string opuser, string userid, string opno)
        {
            string ProcName = DataUser.CBHS + ".SP_EXTRACT_COST_COMMIT";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time),
                                              new OleDbParameter("opuser",userid),
                                              new OleDbParameter("userid",userid),
                                              new OleDbParameter("opno",opno),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
        /// <summary>
        /// 查询当月提取次数
        /// </summary>
        /// <param name="date_time">日期（201010）</param>
        /// <returns></returns>
        public DataSet GetOpno(string date_time, string username)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DISTINCT OPNUM AS OPNUM 
                                    FROM {0}.CBHS_COST_EXTRACT_LOG
                                   WHERE TO_CHAR(ST_DATE,'yyyymm')='{1}' and OPUSER='{2}' ORDER BY OPNUM ", DataUser.CBHS, date_time, username);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 查询预提结果数据
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <param name="dept_code">科室代码</param>
        /// <param name="opno">预提次数</param>
        /// <returns></returns>
        public DataSet GetDeptCostsPer(string date_time, string dept_code, string opno, string username, string userid)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            sqlWhere.Append(" AND 1=1");
            if (dept_code != null && !dept_code.Equals(""))
            {
                sqlWhere.AppendFormat(" AND ACCOUNT_DEPT_CODE='{0}'", dept_code);
            }
            strSql.AppendFormat(@"SELECT ITEM_CODE, DECODE (ITEM_CODE,'', '合计',MIN (ITEM_NAME)) ITEM_NAME, SUM (TOTAL_COSTS) TOTAL_COSTS,
                                         SUM (COSTS) COSTS, SUM (COSTS_ARMYFREE) COSTS_ARMYFREE,
                                         DECODE (ITEM_CODE, '', '', MIN (MEMO))
                                    FROM (SELECT   M2.ITEM_CODE, M2.ITEM_NAME,
                                                 SUM (M1.COSTS) TOTAL_COSTS,  NULL COSTS, NULL COSTS_ARMYFREE, NULL AS MEMO
                                            FROM (SELECT A.ITEM_CODE, A.COSTS
                                                    FROM {0}.CBHS_COST_EXTRACT_LOG A, {1}.SYS_DEPT_DICT B
                                                   WHERE A.DEPT_CODE = B.DEPT_CODE  and a.OPUSER='{5}'
                                                     AND TO_CHAR (ST_DATE(+),'yyyymm') = '{2}'
                                                     AND OPNUM(+)='{3}' {4}) M1,
                                                 {0}.CBHS_COST_ITEM_DICT M2
                                           WHERE M1.ITEM_CODE(+) = M2.ITEM_CODE and M2.ITEM_CODE IN (SELECT  a.item_code
                                                       FROM {0}.cbhs_cost_item_dict a,
                                                            {1}.sys_power_detail b,
                                                            {1}.sys_specpower_role c
                                                      WHERE a.item_code = c.ID
                                                        AND c.TYPE = 2
                                                        AND c.role_id = b.power_id
                                                        AND UPPER (b.target_id) = '{6}')
                                        GROUP BY M2.ITEM_CODE, M2.ITEM_NAME
                                        ORDER BY M2.ITEM_CODE)
                                  GROUP BY CUBE (ITEM_CODE)", DataUser.CBHS, DataUser.COMM, date_time, opno, sqlWhere.ToString(), username, userid);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        public void UpdateDepteCosts(string date_time, List<GoldNet.Model.CostDetail> costdetails)
        {
            MyLists listtable = new MyLists();
            foreach (GoldNet.Model.CostDetail costdetail in costdetails)
            {
                if (costdetail.Memo == null || costdetail.Memo.Equals("") || costdetail.Item_code.Equals(""))
                {
                    continue;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"UPDATE {0}.CBHS_DEPT_COST_DETAIL 
                                          SET MEMO=?
                                       WHERE ITEM_CODE=?
                                         AND TO_CHAR(ACCOUNTING_DATE,'yyyymm')=?", DataUser.CBHS);
                OleDbParameter[] parameteradd = {
                                              new OleDbParameter("memo",costdetail.Memo),
											  new OleDbParameter("item_code",costdetail.Item_code),
											  new OleDbParameter("accounting_date",date_time)
											 										  
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 长安医院成本提取(核算科室)
        /// </summary>
        /// <param name="date"></param>
        public void getcacost(string stdate)
        {
            string yycostsconn = System.Configuration.ConfigurationSettings.AppSettings["costsql"];
            //
            string stracccosts = System.Configuration.ConfigurationSettings.AppSettings["yyacccosts"];
            string notstraccosts = System.Configuration.ConfigurationSettings.AppSettings["notyyacccosts"];
            string enddate = Convert.ToDateTime(stdate).AddMonths(1).ToString("yyyy-MM-dd");
            ///////////////
            string str = string.Format(@"SELECT   b.ccode, b.ccode_name, c.cdepname, a.cdept_id, SUM (a.md) md
                                            FROM gl_accvouch a, code b, department c
                                           WHERE a.cdept_id = c.cdepcode
                                             AND a.ccode = b.ccode
                                             AND a.dbill_date >= '{0}'
                                             AND a.dbill_date < '{1}'
                                             AND a.cdept_id IS NOT NULL
                                             AND b.cclass = '损益'
                                             AND b.bproperty = '1'
                                             and substring(b.ccode,1,4) in ({2})
                                             and b.ccode not in ( {3})
                                        GROUP BY b.ccode, b.ccode_name, a.cdept_id, c.cdepname", stdate, enddate, stracccosts, notstraccosts);
            System.Data.SqlClient.SqlParameter[] cmdParmsyy = new System.Data.SqlClient.SqlParameter[0] { };
            DataTable table = DALBase.ExecuteDataSet(yycostsconn, str, cmdParmsyy).Tables[0];

            //提取物业软件成本
            string connWebsites = System.Configuration.ConfigurationSettings.AppSettings["ConnWebsites"];
            string WebsitesSql = string.Format(@"SELECT   'XDF' AS ccode, '洗涤费' AS ccode_name, emp_unit AS cdepname,
                                                 department_serial AS cdept_id, SUM (laundry_price) AS md
                                            FROM v_goldnet_laundry
                                           WHERE department_serial IS NOT NULL
                                             AND laundry_date >= '{0}'
                                             AND laundry_date < '{1}'
                                        GROUP BY emp_unit, department_serial", stdate, enddate);
            System.Data.SqlClient.SqlParameter[] cmdParms = new System.Data.SqlClient.SqlParameter[0] { };
            DataTable Websites_dt = DALBase.ExecuteDataSet(connWebsites, WebsitesSql, cmdParms).Tables[0];
            //DataTable Websites_dt = DALBase.ExecuteDataSet( ).Tables[0];

            table.Merge(Websites_dt);

            //string str = "SELECT   '64010117' ccode,  '0000' ccode_name, '99999' cdepname, '20130101' cdept_id, 100 md from dual";
            //DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            /////////////////////
            string strdel = string.Format("delete {1}.CBHS_DEPT_COST_DETAIL_COMMIT where TO_CHAR (ACCOUNTING_DATE,'yyyy-mm-dd')='{0}' and GET_TYPE='1'", stdate, DataUser.CBHS);
            OracleOledbBase.ExecuteNonQuery(strdel);


            MyLists listttrans = new MyLists();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string strcost = string.Format("select cost_item from {1}.CBHS_COSTS_VS_YYCOSTS where ccode='{0}'", table.Rows[i]["ccode"].ToString(), DataUser.CBHS);
                string itemcode = "4011";
                object item = OracleOledbBase.GetSingle(strcost);
                if (item != null)
                    itemcode = item.ToString();
                if (itemcode == "4011")
                {

                    continue;
                }
                string strdept = string.Format("select ACCOUNT_DEPT_CODE from {0}.SYS_DEPT_dict where dept_code='{1}'", DataUser.COMM, table.Rows[i]["cdept_id"].ToString());
                string deptcode = "0";
                object dept = OracleOledbBase.GetSingle(strdept);
                if (dept != null)

                    deptcode = dept.ToString();

                StringBuilder strSql = new StringBuilder();

                strSql.AppendFormat("insert into {0}.CBHS_DEPT_COST_DETAIL_COMMIT(", DataUser.CBHS);
                strSql.Append("DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,OPERATOR,OPERATOR_DATE,GET_TYPE,END_FLAG)");
                strSql.Append(" values (");
                strSql.AppendFormat("?,?,to_date('{0}','yyyy-mm-dd'),?,?,sysdate,?,?)", stdate);
                OleDbParameter[] parameters = {
					
					new OleDbParameter("DEPT_CODE", deptcode),
					new OleDbParameter("ITEM_CODE", itemcode),
					
					
					new OleDbParameter("COSTS", Convert.ToDouble( table.Rows[i]["md"].ToString())),
					new OleDbParameter("OPERATOR", ""),
					
					new OleDbParameter("GET_TYPE", "1"),
                   
                   
                    new OleDbParameter("END_FLAG",  1)
            };
                List listindex = new List();
                listindex.StrSql = strSql.ToString();
                listindex.Parameters = parameters;
                listttrans.Add(listindex);

            }
            OracleOledbBase.ExecuteTranslist(listttrans);
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
            string strdel = string.Format("delete {1}.CBHS_DEPT_COST_DETAIL where TO_CHAR (ACCOUNTING_DATE,'yyyymm')='{0}' and GET_TYPE='6'", stdate, DataUser.CBHS);
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
                string strcost = string.Format("select cost_item from {1}.CBHS_COSTS_VS_YYCOSTS where ccode='{0}'", table.Rows[i]["itemcode"].ToString().Trim(), DataUser.CBHS);
                string itemcode = table.Rows[i]["itemcode"].ToString().Trim();
                object item = OracleOledbBase.GetSingle(strcost);
                if (item != null)
                    itemcode = item.ToString();

                StringBuilder strSql = new StringBuilder();

                strSql.AppendFormat("insert into {0}.CBHS_DEPT_COST_DETAIL(", DataUser.CBHS);
                strSql.Append("DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,OPERATOR,OPERATOR_DATE,GET_TYPE,END_FLAG,yy_dept_code)");
                strSql.Append(" values (");
                strSql.AppendFormat("?,?,to_date('{0}','yyyy-mm-dd'),?,?,sysdate,?,?,?)", stdate + "01");
                OleDbParameter[] parameters = {
					
					new OleDbParameter("DEPT_CODE", deptcode),
					new OleDbParameter("ITEM_CODE", itemcode),
					
					
					new OleDbParameter("COSTS", Convert.ToDouble( table.Rows[i]["costs"].ToString().Trim())),
					new OleDbParameter("OPERATOR",username),
					
					new OleDbParameter("GET_TYPE", "6"),
                   
                   
                    new OleDbParameter("END_FLAG",  1),
                    new OleDbParameter("YY_DEPT_CODE",table.Rows[i]["deptcode"].ToString().Trim())
            };
                List listindex = new List();
                listindex.StrSql = strSql.ToString();
                listindex.Parameters = parameters;
                listttrans.Add(listindex);

            }
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        #endregion 成本提取


    }
}
