using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class Cbhs_dict
    {
        public Cbhs_dict()
        {

        }
        #region 公用字典
        //获取收入类别
        public DataSet GetIncome_Type()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select ID,IMCOM_TYPE from {0}.CBHS_INCOM_TYPE", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        //获取结算标志
        public DataSet GetAccount_Signs()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT ID, ACCOUNT_TYPE  FROM {0}.cbhs_account_signs", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取核算科室
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet getAccount_Dept()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT dept_code, dept_name, dept_type, account_dept_code, account_dept_name FROM {0}.sys_dept_dict WHERE attr = '是' ", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        public DataSet getAccountType()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT dept_code, dept_name, dept_type, account_dept_code, account_dept_name FROM {0}.sys_dept_dict WHERE attr = '是' and DEPT_TYPE in ('0','1','2')", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 判断奖金是否生成
        /// </summary>
        /// <param name="date_time">201006</param>
        /// <returns>生成true，为生成false</returns>
        public bool IsBonusSave(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT *
                                    FROM {0}.BONUS_INDEX
                                   WHERE TO_CHAR (BEGINYEAR) || LPAD (TO_CHAR (BEGINMONTH), 2, '0') ='{1}'", DataUser.PERFORMANCE, date_time);
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion 公用字典

        #region 收入

        /// <summary>
        /// 获取收入项目
        /// </summary>
        /// <param name="input_code">输入码</param>
        /// <returns>DataSet</returns>
        public DataSet GetReck_Items(string input_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT class_code, class_name, input_code FROM {1}.reck_item_class_dict where input_code like '{0}%'  order by class_code", input_code, DataUser.HISFACT);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input_code"></param>
        /// <returns></returns>
        public DataSet Getdiagnosis(string input_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT DIAGNOSIS_CODE, DIAGNOSIS_NAME FROM HISDATA.DIAGNOSIS_DICT where input_code like '{0}%'  order by DIAGNOSIS_CODE", input_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取成本项目
        /// </summary>
        /// <param name="input_code">输入码</param>
        /// <returns>DataSet</returns>
        public DataSet GetCbhs_Items(string input_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT item_code, item_name, input_code FROM {0}.cbhs_cost_item_dict where input_code like '{1}%'   order by item_code", DataUser.CBHS, input_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取核算类型
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetAccount_Type()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT ID, account_type FROM {0}.cbhs_account_type ", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 收入类别
        /// </summary>
        /// <returns></returns>
        public DataTable GetItem_Type()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT * FROM {0}.CBHS_INCOM_TYPE_DICT ", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取收入比例列表
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetIncomeItemList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   (SELECT cc.class_code || ':' || cc.class_name
                                        FROM {1}.reck_item_class_dict cc
                                       WHERE cc.class_code = SUBSTR (c.class_code, 0, 1)) class_code,
                                     c.class_code AS item_class, c.class_name item_name, NVL(a.input_code,c.input_code) input_code,
                                     order_dept_distribut * 100 order_dept_distribut,
                                     perform_dept_distribut * 100 perform_dept_distribut,
                                     nursing_percen * 100 nursing_percen,
                                     out_opdept_percen * 100 out_opdept_percen,
                                     out_exdept_percen * 100 out_exdept_percen,
                                     out_nursing_percen * 100 out_nursing_percen,
                                     cooperant_percen * 100 cooperant_percen,
                                     (SELECT account_type
                                        FROM cbhs.cbhs_account_type aa
                                       WHERE a.calculation_type = TO_CHAR (aa.ID)) calculation_type,
                                     fixed_percen * 100 fixed_percen,
                                     NVL (profit_rate, 1) * 100 profit_rate, b.item_name AS cost_code,
                                     (SELECT dd.dept_name
                                        FROM comm.sys_dept_dict dd
                                       WHERE dd.dept_code = a.perfro_dept) AS perfro_dept,
                                     NVL (zjcbbl, 1) * 100 AS zjcbbl, NVL (jjcbbl, 1) * 100 AS jjcbbl,
                                     NVL (dccb, 0) AS dccb,a.class_type,a.class_name
                                FROM {0}.cbhs_distribution_calc_schm a,
                                     {0}.cbhs_cost_item_dict b,
                                     {1}.reck_item_class_dict c
                               WHERE a.cost_code = b.item_code(+)
                                 AND c.class_code = a.item_class(+)
                                 
                            ORDER BY class_code, item_class", DataUser.CBHS, DataUser.HISFACT);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 根据收入项目代码获取收入项目
        /// </summary>
        /// <param name="item_class">收入项目代码</param>
        /// <returns>DataSet</returns>
        public DataSet GetIncomeItem(string item_class)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   a.rowid as row_id,c.class_code AS item_class, c.class_name item_name, nvl(a.input_code,c.input_code) input_code,
                                         order_dept_distribut * 100 order_dept_distribut,
                                         perform_dept_distribut * 100 perform_dept_distribut,
                                         nursing_percen * 100 nursing_percen,
                                         out_opdept_percen * 100 out_opdept_percen,
                                         out_exdept_percen * 100 out_exdept_percen,
                                         out_nursing_percen * 100 out_nursing_percen,
                                         cooperant_percen * 100 cooperant_percen, 
                                          calculation_type,
                                         fixed_percen * 100 fixed_percen,nvl(PROFIT_RATE,1)*100 PROFIT_RATE ,cost_code,PERFRO_DEPT,OTHER_DEPT,
                                         nvl(OTHER_PERCEN,1)*100 OTHER_PERCEN,OUT_OTHER_DEPT,
                                         nvl(OUT_OTHER_PERCEN,1)*100 OUT_OTHER_PERCEN,
                                         nvl(ZJCBBL,1)*100 as ZJCBBL, nvl(JJCBBL,1)*100 as JJCBBL, nvl(DCCB,0) as DCCB,a.CLASS_TYPE,a.CLASS_NAME
                                    FROM {0}.cbhs_distribution_calc_schm a,
                                         {2}.reck_item_class_dict c
                                   WHERE c.class_code = a.item_class(+) 
                                     
                                     AND c.class_code = '{1}'
                                ORDER BY item_class", DataUser.CBHS, item_class, DataUser.HISFACT);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 设置收入项目比例（添加）
        /// </summary>
        /// <param name="model">收入项目model</param>
        public void SetIncomeItem(GoldNet.Model.IncomeItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT INTO {0}.cbhs_distribution_calc_schm
                                        (item_class, item_name, input_code, order_dept_distribut,
                                         perform_dept_distribut, nursing_percen, out_opdept_percen,
                                         out_exdept_percen, out_nursing_percen, cooperant_percen,
                                         calculation_type, fixed_percen, cost_code,PROFIT_RATE,
                                         PERFRO_DEPT,ZJCBBL,JJCBBL,DCCB,CLASS_TYPE,CLASS_NAME   
                                        )
                                 VALUES (?, ?, upper(trim(?)), ?,?, ?,?, ?, ?,?, ?, ?,?, ?, ?,?,?,?,?,?)", DataUser.CBHS);
            OleDbParameter[] parameter = {
											  new OleDbParameter("item_class",model.Item_class),
                                              new OleDbParameter("item_name",model.Item_name),
                                              new OleDbParameter("input_code",model.Input_code),
                                              new OleDbParameter("order_dept_distribut",model.Order_dept_distribut/100),
                                              new OleDbParameter("perform_dept_distribut",model.Perform_dept_distribut/100),
                                              new OleDbParameter("nursing_percen",model.Nursing_percen/100),
                                              new OleDbParameter("out_opdept_percen",model.Out_opdept_percen/100),
                                              new OleDbParameter("out_exdept_percen",model.Out_exdept_percen/100),
                                              new OleDbParameter("out_nursing_percen",model.Out_nursing_percen/100),
                                              new OleDbParameter("cooperant_percen",model.Cooperant_prercen/100),
                                              new OleDbParameter("calculation_type",model.Calculation_type),
                                              new OleDbParameter("fixed_percen",model.Fixed_percen/100),
                                              new OleDbParameter("cost_code",model.Cost_code),
                                              new OleDbParameter("PROFIT_RATE",model.Profit_rate/100),
                                              new OleDbParameter("PERFRO_DEPT",model.PERFRO_DEPT),
                                             
                                              new OleDbParameter("ZJCBBL",model.ZJCBBL/100),
                                              new OleDbParameter("JJCBBL",model.JJCBBL/100),
                                              new OleDbParameter("DCCB",model.DCCB),
                                              new OleDbParameter("CLASS_TYPE",model.CLASSTYPE),
                                              new OleDbParameter("CLASS_NAME",model.CLASSNAME)
										  };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
        }
        /// <summary>
        /// 更新收入项目比例
        /// </summary>
        /// <param name="model">收入项目model</param>
        public void UpdateIncomeItem(GoldNet.Model.IncomeItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"UPDATE {0}.cbhs_distribution_calc_schm
                                   SET input_code = trim(?),
                                       order_dept_distribut = ?,
                                       perform_dept_distribut = ?,
                                       nursing_percen = ?,
                                       out_opdept_percen = ?,
                                       out_exdept_percen = ?,
                                       out_nursing_percen = ?,
                                       cooperant_percen = ?,
                                       calculation_type = ?,
                                       fixed_percen = ?,
                                       cost_code = ?,
                                       PROFIT_RATE=?,
                                       PERFRO_DEPT = ?,
                                       
                                       ZJCBBL = ?,
                                       JJCBBL = ?,
                                       DCCB = ?,
                                       class_type=?,
                                       class_name=?
                                 WHERE item_class = ? ", DataUser.CBHS);
            OleDbParameter[] parameter = {    new OleDbParameter("input_code",model.Input_code),
                                              new OleDbParameter("order_dept_distribut",model.Order_dept_distribut/100),
                                              new OleDbParameter("perform_dept_distribut",model.Perform_dept_distribut/100),
                                              new OleDbParameter("nursing_percen",model.Nursing_percen/100),
                                              new OleDbParameter("out_opdept_percen",model.Out_opdept_percen/100),
                                              new OleDbParameter("out_exdept_percen",model.Out_exdept_percen/100),
                                              new OleDbParameter("out_nursing_percen",model.Out_nursing_percen/100),
                                              new OleDbParameter("cooperant_percen",model.Cooperant_prercen/100),
                                              new OleDbParameter("calculation_type",model.Calculation_type),
                                              new OleDbParameter("fixed_percen",model.Fixed_percen/100),
                                              new OleDbParameter("cost_code",model.Cost_code),
                                              new OleDbParameter("PROFIT_RATE",model.Profit_rate/100),
                                              new OleDbParameter("PERFRO_DEPT",model.PERFRO_DEPT),
                                              
                                              new OleDbParameter("ZJCBBL",model.ZJCBBL/100),
                                              new OleDbParameter("JJCBBL",model.JJCBBL/100),
                                              new OleDbParameter("DCCB",model.DCCB),
                                              new OleDbParameter("CLASS_TYPE",model.CLASSTYPE),
                                              new OleDbParameter("CLASS_NAME",model.CLASSNAME),
                                              new OleDbParameter("item_class",model.Item_class)
                                         };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
        }
        public string GetIncomeItemname(string itemcode)
        {
            string str = string.Format("select item_name from {0}.CBHS_DISTRIBUTION_CALC_SCHM where item_class='{1}'", DataUser.CBHS, itemcode);
            return OracleOledbBase.ExecuteScalar(str).ToString();
        }
        /// <summary>
        /// 科室成本分配设置
        /// </summary>
        /// <returns></returns>
        public DataTable GetCost_toDeptset()
        {
            string str = string.Format(@"SELECT   aa.ID,
           aa.DEPT_CODE,
           aa.DEPT_NAME,
           aa.PROG_CODE,
           bb.PROG_NAME
    FROM   (SELECT   A.ID,
                     A.DEPT_CODE,
                     B.DEPT_NAME,
                     A.PROG_CODE
              FROM   cbhs.CBHS_COST_TO_DEPT_INDEX a, comm.sys_dept_dict b
             WHERE   A.DEPT_CODE = b.dept_code) aa,
           cbhs.CBHS_COST_APPOR_PROG_DICT bb
   WHERE   Aa.PROG_CODE = bb.PROG_CODE(+)
ORDER BY   aa.id");
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }
        /// <summary>
        /// 获取科室明细收入比例
        /// </summary>
        /// <param name="item_class">收入项目代码</param>
        /// <returns>DataSet</returns>
        public DataSet GetDeptIncomeItem()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   a.id,a.ITEM_CLASS,(SELECT dept_name
            FROM {0}.sys_dept_dict m
           WHERE m.dept_code = a.dept_code) order_dept,
         (SELECT dept_name
            FROM {0}.sys_dept_dict m
           WHERE m.dept_code = a.perform_dept_code) perform_dept,
         (select m.item_name from {1}.CBHS_DISTRIBUTION_CALC_SCHM m where m.item_class=a.item_class ) classname,
         a.order_dept_distribut * 100 AS order_dept_distribut,
         a.perform_dept_distribut * 100 AS perform_dept_distribut,
         a.nursing_percen * 100 AS nursing_percen,
         a.out_opdept_percen * 100 AS out_opdept_percen,
         a.out_exdept_percen * 100 AS out_exdept_percen,
         a.out_nursing_percen * 100 AS out_nursing_percen,
         a.cooperant_percen * 100 AS cooperant_percen,
         a.fixed_percen * 100 AS fixed_percen,
         NVL (profit_rate, 1) * 100 profit_rate,(select  m.item_name from {1}.cbhs_cost_item_dict m where m.item_code=a.cost_code) AS cost_code,
        
         (SELECT dept_name
            FROM {0}.sys_dept_dict m
           WHERE m.dept_code = a.perfro_dept) perfro_dept
    FROM {1}.cbhs_dept_incom_item_dict a
ORDER BY a.id", DataUser.COMM, DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());

        }

        /// <summary>
        /// 保存科室归集设置
        /// </summary>
        /// <param name="selectRow"></param>
        public void SaveCostToDept(Dictionary<string, string>[] selectRow)
        {
            Goldnet.Dal.SYS_DEPT_DICT deptdal = new SYS_DEPT_DICT();
            string delsql = "delete from  cbhs.CBHS_COST_TO_DEPT_INDEX ";
            MyLists listtable = new MyLists();
            //删除
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < selectRow.Length; i++)
            {
                if (selectRow[i]["DEPT_NAME"].ToString() != "")
                {
                    StringBuilder isql = new StringBuilder();
                    isql.Append(" insert into cbhs.CBHS_COST_TO_DEPT_INDEX ( ");
                    isql.Append("id,DEPT_CODE,PROG_CODE ");
                    isql.Append(") values (");
                    isql.Append("" + Convert.ToInt32(selectRow[i]["ID"].ToString()) + ",");
                    isql.Append("'" + deptdal.GetDeptCodeByDeptname(selectRow[i]["DEPT_NAME"].ToString()) + "',");
                    isql.Append("'" + GetProgName(selectRow[i]["PROG_NAME"].ToString()) + "'");
                    isql.Append(") ");
                    List listcenterdetail = new List();
                    listcenterdetail.StrSql = isql.ToString();
                    listcenterdetail.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        public void SaveCostIncome(Dictionary<string, string>[] selectRow)
        {
            Goldnet.Dal.SYS_DEPT_DICT deptdal = new SYS_DEPT_DICT();
            string delsql = "delete from  cbhs.XYHS_DEPT_ITEM_CODE ";
            MyLists listtable = new MyLists();
            //删除
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < selectRow.Length; i++)
            {
                string selectdept = selectRow[i]["DEPT_NAME"] == null ? "" : selectRow[i]["DEPT_NAME"].ToString();
                string selectincome = selectRow[i]["INCOME_NAME"] == null ? "" : selectRow[i]["INCOME_NAME"].ToString();
                if (selectdept != "" && selectincome != "")
                {
                    StringBuilder isql = new StringBuilder();
                    isql.Append(" insert into cbhs.XYHS_DEPT_ITEM_CODE ( ");
                    isql.Append("DEPT_CODE,ITEM_CODE ");
                    isql.Append(") values (");
                    isql.Append("'" + deptdal.GetDeptCodeByDeptname(selectRow[i]["DEPT_NAME"].ToString()) + "',");
                    isql.Append("'" + deptdal.GetIncomeCodeByIncomename(selectRow[i]["INCOME_NAME"].ToString()) + "'");

                    isql.Append(") ");
                    //
                    List listcenterdetail = new List();
                    listcenterdetail.StrSql = isql.ToString();
                    listcenterdetail.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        public void SaveCost_Income(Dictionary<string, string>[] selectRow)
        {
            Goldnet.Dal.SYS_DEPT_DICT deptdal = new SYS_DEPT_DICT();
            string delsql = "delete from  cbhs.XYHS_ITEM_COST ";
            MyLists listtable = new MyLists();
            //删除
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < selectRow.Length; i++)
            {
                string selectincome = selectRow[i]["INCOME_NAME"] == null ? "" : selectRow[i]["INCOME_NAME"].ToString();
                if (selectincome != "")
                {
                    StringBuilder isql = new StringBuilder();
                    isql.Append(" insert into cbhs.XYHS_ITEM_COST ( ");
                    isql.Append("COST_CODE,INCOME_CODE ");
                    isql.Append(") values (");
                    isql.Append("'" + selectRow[i]["COST_CODE"].ToString() + "',");
                    isql.Append("'" + deptdal.GetIncomeCodeByIncomename(selectRow[i]["INCOME_NAME"].ToString()) + "'");

                    isql.Append(") ");
                    //
                    List listcenterdetail = new List();
                    listcenterdetail.StrSql = isql.ToString();
                    listcenterdetail.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        public void SaveDiagnosis(Dictionary<string, string>[] selectRow)
        {
            Goldnet.Dal.SYS_DEPT_DICT deptdal = new SYS_DEPT_DICT();
            int id = OracleOledbBase.GetMaxID("DIAGNOSIS_CODE", "cbhs.XYHS_DIAGNOSIS_DICT");
            string delsql = "delete from  cbhs.XYHS_DIAGNOSIS_DICT ";
            MyLists listtable = new MyLists();
            //删除
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < selectRow.Length; i++)
            {
                string diagnosiscode = selectRow[i]["DIAGNOSIS_CODE"] == null ? "" : selectRow[i]["DIAGNOSIS_CODE"].ToString();
                string diagnosisname = selectRow[i]["DIAGNOSIS_NAME"] == null ? "" : selectRow[i]["DIAGNOSIS_NAME"].ToString();
                if (diagnosisname != "")
                {
                    StringBuilder isql = new StringBuilder();
                    isql.Append(" insert into cbhs.XYHS_DIAGNOSIS_DICT ( ");
                    isql.Append("DIAGNOSIS_CODE,DIAGNOSIS_NAME ");
                    isql.Append(") values (");
                    isql.Append("" + diagnosiscode == "" ? id.ToString() : diagnosiscode + "");
                    isql.Append(",'" + selectRow[i]["DIAGNOSIS_NAME"].ToString() + "'");

                    isql.Append(") ");
                    //
                    List listcenterdetail = new List();
                    listcenterdetail.StrSql = isql.ToString();
                    listcenterdetail.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        public void SaveDeptIncome(Dictionary<string, string>[] selectRow)
        {
            Goldnet.Dal.SYS_DEPT_DICT deptdal = new SYS_DEPT_DICT();
            string delsql = "delete from  cbhs.cbhs_dept_incom_item_dict ";
            MyLists listtable = new MyLists();
            //删除科室类别
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);


            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT item_code, item_name FROM {0}.cbhs_cost_item_dict  order by item_code", DataUser.CBHS);
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];

            for (int i = 0; i < selectRow.Length; i++)
            {
                if (selectRow[i]["CLASSNAME"].ToString() != "" && (selectRow[i]["ORDER_DEPT"].ToString() != "" || selectRow[i]["PERFORM_DEPT"].ToString() != ""))
                {
                    StringBuilder isql = new StringBuilder();
                    isql.Append(" insert into cbhs.cbhs_dept_incom_item_dict ( ");
                    isql.Append("id,DEPT_CODE,ITEM_CLASS,ORDER_DEPT_DISTRIBUT,");
                    isql.Append("PERFORM_DEPT_DISTRIBUT,NURSING_PERCEN,OUT_OPDEPT_PERCEN,");
                    isql.Append("OUT_EXDEPT_PERCEN,OUT_NURSING_PERCEN,COOPERANT_PERCEN,FIXED_PERCEN,");
                    isql.Append("COST_CODE,TAG,PROFIT_RATE,PERFRO_DEPT,PERFORM_DEPT_CODE");
                    isql.Append(") values (");
                    isql.Append("" + Convert.ToInt32(selectRow[i]["ID"].ToString()) + ",");
                    isql.Append("'" + deptdal.GetDeptCodeByDeptname(selectRow[i]["ORDER_DEPT"].ToString()) + "',");
                    isql.Append("'" + GetIncomItem(selectRow[i]["CLASSNAME"].ToString()) + "',");
                    isql.Append("" + Convert.ToDouble(selectRow[i]["ORDER_DEPT_DISTRIBUT"]) / 100 + ",");
                    isql.Append("" + Convert.ToDouble(selectRow[i]["PERFORM_DEPT_DISTRIBUT"]) / 100 + ",");
                    isql.Append("" + Convert.ToDouble(selectRow[i]["NURSING_PERCEN"]) / 100 + ",");
                    isql.Append("" + Convert.ToDouble(selectRow[i]["OUT_OPDEPT_PERCEN"]) / 100 + ",");
                    isql.Append("" + Convert.ToDouble(selectRow[i]["OUT_EXDEPT_PERCEN"]) / 100 + ",");
                    isql.Append("" + Convert.ToDouble(selectRow[i]["OUT_NURSING_PERCEN"]) / 100 + ",");
                    isql.Append("" + Convert.ToDouble(selectRow[i]["COOPERANT_PERCEN"]) / 100 + ",");
                    isql.Append("" + Convert.ToDouble(selectRow[i]["FIXED_PERCEN"]) / 100 + ",");
                    object costcode = GetCostItem(selectRow[i]["COST_CODE"].ToString(), dt);
                    if (costcode == null)
                    {
                        isql.Append("null,");
                    }
                    else
                    {
                        isql.Append("'" + costcode + "',");
                    }

                    isql.Append("1,");
                    isql.Append("1,");

                    isql.Append("'" + deptdal.GetDeptCodeByDeptname(selectRow[i]["PERFRO_DEPT"].ToString()) + "',");
                    isql.Append("'" + deptdal.GetDeptCodeByDeptname(selectRow[i]["PERFORM_DEPT"].ToString()) + "'");
                    isql.Append(") ");
                    //
                    List listcenterdetail = new List();
                    listcenterdetail.StrSql = isql.ToString();
                    listcenterdetail.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        public string GetProgName(string progname)
        {
            string str = string.Format("select prog_code from {0}.CBHS_COST_APPOR_PROG_DICT where prog_name='{1}'", DataUser.CBHS, progname);
            return OracleOledbBase.ExecuteScalar(str).ToString();
        }
        public string GetIncomItem(string itemname)
        {
            string str = string.Format("select item_class from {0}.CBHS_DISTRIBUTION_CALC_SCHM where item_name='{1}'", DataUser.CBHS, itemname);
            return OracleOledbBase.ExecuteScalar(str).ToString();
        }
        private object GetCostItem(string costName, DataTable dt)
        {
            if (costName == "")
            {
                return null;
            }
            DataRow[] dr = dt.Select("item_name='" + costName + "'");
            if (dr.Length > 0)
            {
                return dr[0]["item_code"];
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 获取科室明细收入比例
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <param name="item_class">收入项目代码</param>
        /// <returns>DataSet</returns>
        public DataSet GetDeptIncomeItem(string dept_code, string item_class)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT a.dept_code, b.dept_name, a.item_class, a.item_name,
                                       a.order_dept_distribut * 100 as order_dept_distribut, 
                                       a.perform_dept_distribut * 100 as perform_dept_distribut, 
                                       a.nursing_percen * 100 as nursing_percen,
                                       a.out_opdept_percen * 100 as out_opdept_percen, 
                                       a.out_exdept_percen * 100 as out_exdept_percen,
                                       a.out_nursing_percen * 100 as out_nursing_percen,
                                       a.cooperant_percen * 100 as cooperant_percen, 
                                       a.fixed_percen * 100 as fixed_percen,nvl(PROFIT_RATE,1)*100 PROFIT_RATE, a.cost_code
                                  FROM {0}.cbhs_dept_incom_item_dict a,
                                       (select * from {1}.sys_dept_dict where DEPT_TYPE in ('0','1','2')) b
                                 WHERE a.dept_code = b.dept_code
                                   and a.item_class='{2}' and a.dept_code='{3}'  ", DataUser.CBHS, DataUser.COMM, item_class, dept_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());

        }
        /// <summary>
        /// 设置科室收入项目比例
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <param name="model">收入项目model</param>
        /// <param name="tag">内外层设置标识</param>
        public void SetDeptIncomeItem(string dept_code, GoldNet.Model.IncomeItem model, string tag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"insert into  {0}.cbhs_dept_incom_item_dict
                                        (dept_code, item_class, item_name, 
                                         order_dept_distribut, perform_dept_distribut, nursing_percen, 
                                         out_opdept_percen, out_exdept_percen, out_nursing_percen, 
                                         cooperant_percen, fixed_percen, cost_code,tag,PROFIT_RATE,PERFRO_DEPT,OTHER_DEPT,OTHER_PERCEN,OUT_OTHER_DEPT,OUT_OTHER_PERCEN)
                                 VALUES (?, ?, upper(?), ?,?, ?, ?,?, ?, ?,?, ?,?,?,?,?,?,?,?)", DataUser.CBHS);
            OleDbParameter[] parameter = {    new OleDbParameter("dept_code",dept_code),
											  new OleDbParameter("item_class",model.Item_class),
                                              new OleDbParameter("item_name",model.Item_name),
                                              new OleDbParameter("order_dept_distribut",model.Order_dept_distribut/100),
                                              new OleDbParameter("perform_dept_distribut",model.Perform_dept_distribut/100),
                                              new OleDbParameter("nursing_percen",model.Nursing_percen/100),
                                              new OleDbParameter("out_opdept_percen",model.Out_opdept_percen/100),
                                              new OleDbParameter("out_exdept_percen",model.Out_exdept_percen/100),
                                              new OleDbParameter("out_nursing_percen",model.Out_nursing_percen/100),
                                              new OleDbParameter("cooperant_percen",model.Cooperant_prercen/100),
                                              new OleDbParameter("fixed_percen",model.Fixed_percen/100),
                                              new OleDbParameter("cost_code",model.Cost_code),
                                              new OleDbParameter("tag",tag),
                                              new OleDbParameter("PROFIT_RATE",model.Profit_rate/100),
                                              new OleDbParameter("PERFRO_DEPT",model.PERFRO_DEPT),
                                              new OleDbParameter("OTHER_DEPT",model.OTHER_DEPT),
                                              new OleDbParameter("OTHER_PERCEN",model.OTHER_PERCEN/100),
                                               new OleDbParameter("OUT_OTHER_DEPT",model.OUT_OTHER_DEPT),
                                              new OleDbParameter("OUT_OTHER_PERCEN",model.OUT_OTHER_PERCEN/100)
										  };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
        }
        public DataSet GetProgList(string deptfilter, string flags)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.prog_code, a.prog_name,a.input_code from {0}.CBHS_COST_APPOR_PROG_DICT a where flags='{1}' ", DataUser.CBHS, flags);

            if (!deptfilter.Equals(""))
            {
                strSql.AppendFormat(" and FLAGS= '{0}'", deptfilter);
            }
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 更新科室收入项目比例
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        /// <param name="model">收入项目model</param>
        /// <param name="tag">内外层设置标识</param>
        public void UpdateDeptIncomeItem(string dept_code, GoldNet.Model.IncomeItem model, string tag)
        {
            StringBuilder strSql = new StringBuilder();
            if (tag.Equals("0"))
            {
                strSql.AppendFormat(@"UPDATE {0}.cbhs_dept_incom_item_dict
                                   SET order_dept_distribut = ?,
                                       perform_dept_distribut = ?,
                                       nursing_percen = ?,
                                       out_opdept_percen = ?,
                                       out_exdept_percen = ?,
                                       out_nursing_percen = ?,
                                       cooperant_percen = ?,
                                       fixed_percen = ?,
                                       cost_code = ?,
                                       PROFIT_RATE=?,
                                       PERFRO_DEPT=?,
                                       OTHER_DEPT=?,
                                       OTHER_PERCEN=?,
                                       OUT_OTHER_DEPT=?,
                                       OUT_OTHER_PERCEN=?,
                                       tag='{1}'
                                 WHERE item_class = ? and dept_code=?", DataUser.CBHS, tag);
            }
            else
            {
                strSql.AppendFormat(@"UPDATE {0}.cbhs_dept_incom_item_dict
                                   SET order_dept_distribut = ?,
                                       perform_dept_distribut = ?,
                                       nursing_percen = ?,
                                       out_opdept_percen = ?,
                                       out_exdept_percen = ?,
                                       out_nursing_percen = ?,
                                       cooperant_percen = ?,
                                       fixed_percen = ?,
                                       cost_code = ?,
                                       PROFIT_RATE=?,
                                       PERFRO_DEPT=?,
                                       OTHER_DEPT=?,
                                       OTHER_PERCEN=?,
                                       OUT_OTHER_DEPT=?,
                                       OUT_OTHER_PERCEN=?
                                 WHERE item_class = ? and dept_code=? and tag='{1}'", DataUser.CBHS, tag);
            }
            OleDbParameter[] parameter = {    new OleDbParameter("order_dept_distribut",model.Order_dept_distribut/100),
                                              new OleDbParameter("perform_dept_distribut",model.Perform_dept_distribut/100),
                                              new OleDbParameter("nursing_percen",model.Nursing_percen/100),
                                              new OleDbParameter("out_opdept_percen",model.Out_opdept_percen/100),
                                              new OleDbParameter("out_exdept_percen",model.Out_exdept_percen/100),
                                              new OleDbParameter("out_nursing_percen",model.Out_nursing_percen/100),
                                              new OleDbParameter("cooperant_percen",model.Cooperant_prercen/100),
                                              new OleDbParameter("fixed_percen",model.Fixed_percen/100),
                                              new OleDbParameter("cost_code",model.Cost_code),
                                               new OleDbParameter("PROFIT_RATE",model.Profit_rate/100),
                                                new OleDbParameter("PERFRO_DEPT",model.PERFRO_DEPT),
                                              new OleDbParameter("OTHER_DEPT",model.OTHER_DEPT),
                                              new OleDbParameter("OTHER_PERCEN",model.OTHER_PERCEN/100),
                                               new OleDbParameter("OUT_OTHER_DEPT",model.OUT_OTHER_DEPT),
                                              new OleDbParameter("OUT_OTHER_PERCEN",model.OUT_OTHER_PERCEN/100),
                                              new OleDbParameter("item_class",model.Item_class),
                                              new OleDbParameter("dept_code",dept_code)
                                              
                                         };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);

        }
        /// <summary>
        /// 删除科室收入项目比例
        /// </summary>
        /// <param name="item_code">收入项目代码</param>
        public void DelDeptIncomeItem(string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0}.cbhs_dept_incom_item_dict where item_class={1}", DataUser.CBHS, item_code);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }





        #endregion 收入

        #region 成本
        /// <summary>
        /// 获取成本项目列表
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetCostItem()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT item_code, item_name, 
                               ( select COST_PROPERTY from cbhs.CBHS_COST_PROPERTY aa where to_char(aa.ID)=b.COST_PROPERTY) cost_property,
                                input_code,
                               (SELECT prog_name
                                  FROM {0}.CBHS_COST_APPOR_PROG_DICT
                                 WHERE prog_code = ALLOT_FOR_JD) AS ALLOT_FOR_JD,
                               (SELECT prog_name
                                  FROM {0}.CBHS_COST_APPOR_PROG_DICT
                                 WHERE prog_code = ALLOT_FOR_JC) AS ALLOT_FOR_JC,
                               (SELECT prog_name
                                  FROM {0}.CBHS_COST_APPOR_PROG_DICT
                                 WHERE prog_code = ALLOT_FOR_RY) AS ALLOT_FOR_RY, item_type ||':'||item_class as item_type,
                                   (select GETTYPE from cbhs.CBHS_COST_ITEM_GETTYPE aa where to_char(aa.ID)=b.GETTYPE) gettype,
                                '' as item_power,
                                (select ACCOUNT_TYPE from cbhs.CBHS_ACCOUNT_TYPE aa where to_char(aa.ID)=b.ACCOUNT_TYPE) ACCOUNT_TYPE,
                               ( select COST_DIRECT from cbhs.CBHS_COST_DIRECT aa where to_char(aa.ID)=b.COST_DIRECT)  COST_DIRECT,
                                nvl(COST_PUBLICSHARE,1)*100 COMPUTE_PER
                          FROM {0}.cbhs_cost_item_dict b where del_flag='0' order by item_code", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 根据成本项目代码获取成本项目
        /// </summary>
        /// <param name="item_code">成本项目代码</param>
        /// <returns>DataSet</returns>
        public DataSet GetCostItemByCode(string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT   rowid as row_id,item_type, item_class, item_code, item_name, cost_property,
                                             input_code, ALLOT_FOR_JD, ALLOT_FOR_JC, ALLOT_FOR_RY, gettype,ACCOUNT_TYPE,nvl(COST_PUBLICSHARE,1)*100 COMPUTE_PER,COST_DIRECT
                                        FROM {0}.cbhs_cost_item_dict
                                     where item_code ='{1}' and del_flag='0' ORDER BY item_code", DataUser.CBHS, item_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取成本项目所属用户--权限
        /// </summary>
        /// <param name="item_code">成本代码</param>
        /// <returns>DataSet</returns>
        public DataSet GetCostItemPower(string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select item_code,user_id,user_name from {0}.cbhs_cost_item_power where item_code ='{1}'", DataUser.CBHS, item_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());

        }
        /// <summary>
        /// 添加成本项目所属用户
        /// </summary>
        /// <param name="item_code">成本项目代码</param>
        /// <param name="user_id">人员id</param>
        /// <param name="user_name">人员名称</param>
        public void AddCostItemPower(string item_code, string user_id, string user_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("insert into {0}.cbhs_cost_item_power (item_code,user_id,user_name) values('{1}','{2}','{3}') ", DataUser.CBHS, item_code, user_id, user_name);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());

        }
        /// <summary>
        /// 判断添加的权限用户是否已经存在
        /// </summary>
        /// <param name="item_code">成本项目代码</param>
        /// <param name="user_id">人员id</param>
        /// <returns>bool</returns>
        public bool PowerIsExist(string item_code, string user_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from {0}.cbhs_cost_item_power where item_code='{1}' and user_id='{2}'", DataUser.CBHS, item_code, user_id);
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 删除成本项目用户权限
        /// </summary>
        /// <param name="item_code">成本项目代码</param>
        /// <param name="user_id">人员id</param>
        public void DelCostItemPower(string item_code, string user_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0}.cbhs_cost_item_power where item_code='{1}' and user_id='{2}'", DataUser.CBHS, item_code, user_id);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());

        }
        /// <summary>
        /// 获取成本类别
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetCostItemType()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT ID, cost_type_code, cost_type_name, input_code FROM {0}.cbhs_cost_type_dict  ", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取成本属性
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetCostItemProperty()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select  id, cost_property from {0}.cbhs_cost_property  order by id asc", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取分解方案字典
        /// </summary>
        /// <returns></returns>
        public DataSet GetProgdict(string flags)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT ID, prog_code, prog_name, input_code, flags,APPLY_DEPT FROM {0}.CBHS_COST_APPOR_PROG_DICT where flags='{1}'", DataUser.CBHS, flags);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取分解方案字典
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllProgdict(string flags)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT ID, prog_code, prog_name, input_code, flags,APPLY_DEPT FROM {0}.XYHS_COST_APPOR_PROG_DICT where flags='{1}'", DataUser.CBHS, flags);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取成本获取方式字典
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetGetType()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select id, gettype from {0}.cbhs_cost_item_gettype", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取直接/间接成本
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetCOSTDIRECT()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select id, COST_DIRECT from {0}.CBHS_COST_DIRECT", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 判断成本项目是否存在
        /// </summary>
        /// <param name="item_name">项目名称</param>
        /// <returns>bool</returns>
        public bool CostsIsExistByName(string item_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from {0}.cbhs_cost_item_dict where item_name = trim('{1}')", DataUser.CBHS, item_name);
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取新加成本的项目代码序号
        /// </summary>
        /// <param name="item_type"></param>
        /// <returns></returns>
        public string GetCostItemCountByType(string item_type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select max(to_number(substr(ITEM_CODE,length(item_type)+1,2)))+1 as num FROM {0}.cbhs_cost_item_dict  WHERE item_type = '{1}'", DataUser.CBHS, item_type);
            DataTable dt = OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
            string num = string.Empty;
            if (dt.Rows.Count > 0)
            {
                num = dt.Rows[0]["num"].ToString();
                if (num.Length < 2)
                {
                    num = "0" + num;
                }
                return item_type + num;
            }
            else
            {
                return item_type + "01";
            }
        }
        /// <summary>
        /// 添加成本项目
        /// </summary>
        /// <param name="model">成本项目model</param>
        public void AddCostItem(GoldNet.Model.CostItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"insert into cbhs.cbhs_cost_item_dict
                                        (item_type, item_class, item_code, item_name, cost_property, input_code,
                                           ALLOT_FOR_JC, ALLOT_FOR_JD, ALLOT_FOR_RY, GETTYPE, del_flag,ACCOUNT_TYPE,COST_PUBLICSHARE,COST_DIRECT)
                                         values(?,?,?,?,?,upper(?),?,?,?,?,'0',?,?,?)");
            OleDbParameter[] parameter = {   new OleDbParameter("item_type",model.Item_type),
                                             new OleDbParameter("item_class",model.Item_class),
                                             new OleDbParameter("item_code",model.Item_code),
                                             new OleDbParameter("item_name",model.Item_name),
                                             new OleDbParameter("cost_property",model.Cost_property),
                                             new OleDbParameter("input_code",model.Input_code),
                                             new OleDbParameter("ALLOT_FOR_JC",model.Allot_for_jc),
                                             new OleDbParameter("ALLOT_FOR_JD",model.Allot_for_jd),
                                             new OleDbParameter("ALLOT_FOR_RY",model.Allot_for_ry),
                                             new OleDbParameter("GETTYPE",model.Gettype),
                                              new OleDbParameter("ACCOUNT_TYPE",model.Account_type),
                                             new OleDbParameter("COST_PUBLICSHARE",model.Compute_per/100),
                                             new OleDbParameter("COST_DIRECT",model.Cost_direct),
                                         };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
        }
        /// <summary>
        /// 更新成本项目
        /// </summary>
        /// <param name="model">成本项目model</param>
        /// <param name="row_id">row_id</param>
        public void UpdateCostItem(GoldNet.Model.CostItem model, string row_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"UPDATE {0}.cbhs_cost_item_dict
                                   SET item_type = ?,
                                       item_class = ?,
                                       item_code = ?,
                                       item_name = ?,
                                       cost_property = ?,
                                       input_code = upper(?),
                                       ALLOT_FOR_JC = ?,
                                       ALLOT_FOR_JD = ?,
                                       ALLOT_FOR_RY = ?,
                                       ACCOUNT_TYPE=?,
                                       COST_PUBLICSHARE=?,
                                       COST_DIRECT=?,
                                       GETTYPE=? 
                                   where rowid='{1}'", DataUser.CBHS, row_id);
            OleDbParameter[] parameter = {    new OleDbParameter("item_type",model.Item_type),
                                             new OleDbParameter("item_class",model.Item_class),
                                             new OleDbParameter("item_code",model.Item_code),
                                             new OleDbParameter("item_name",model.Item_name),
                                             new OleDbParameter("cost_property",model.Cost_property),
                                             new OleDbParameter("input_code",model.Input_code),
                                             new OleDbParameter("ALLOT_FOR_JC",model.Allot_for_jc),
                                             new OleDbParameter("ALLOT_FOR_JD",model.Allot_for_jd),
                                             new OleDbParameter("ALLOT_FOR_RY",model.Allot_for_ry),
                                             new OleDbParameter("ACCOUNT_TYPE",model.Account_type),
                                             new OleDbParameter("COST_PUBLICSHARE",model.Compute_per/100),
                                              new OleDbParameter("COST_DIRECT",model.Cost_direct),
                                              new OleDbParameter("GETTYPE",model.Gettype)
                                         };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
        }
        public void DelCostItem(string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0}.cbhs_cost_item_dict where item_code='{1}'", DataUser.CBHS, item_code);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        #endregion 成本

        #region 科室组织结构
        /// <summary>
        /// 获取科室组织结构列表
        /// </summary>
        /// <param name="datatime">时间条件</param>
        /// <returns>DataSet</returns>
        public DataSet GetDeptInfo(string year, string month)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from {0}.SYS_DEPT_INFO where to_char(DEPT_SNAP_DATE,'yyyymm')=" + year + month + "  order by nvl(P_DEPT_CODE,DEPT_CODE) || DEPT_CODE", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 保存科室组织结构
        /// </summary>
        /// <param name="deptinfos">科室组织结构集合</param>
        /// <param name="datetime">时间</param>
        public void SaveDeptInfo(List<GoldNet.Model.DeptInfo> deptinfos, string year, string month)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            MyLists listtable = new MyLists();
            foreach (GoldNet.Model.DeptInfo deptinfo in deptinfos)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(string.Format(" update {0}.SYS_DEPT_INFO set DEPT_PERSON_COUNT='" + deptinfo.Dept_person_count + "',", DataUser.COMM));
                strSql.Append(" DEPT_AREA='" + deptinfo.Dept_area + "',DEPT_EQUIPMENT_COUNT='" + deptinfo.Dept_equipment_count + "',");
                strSql.Append(" DEPT_COOPERATION_INCOMS='" + deptinfo.Dept_cooperation_incoms + "',DEPT_COOPERATION_BENEFITS='" + deptinfo.Dept_cooperation_benefits + "'");
                strSql.Append(" where DEPT_CODE='" + deptinfo.Dept_code + "' and to_char(DEPT_SNAP_DATE,'yyyymm')=" + year + month + "");
                OleDbParameter[] parameteradd = new OleDbParameter[] { };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        #endregion 科室组织结构

        #region 科室人员
        public DataTable GetStaffInfo(string filter)
        {
            if (filter != "")
            {
                string sql = string.Format(" select STAFF_ID,NAME from {0}.NEW_STAFF_INFO where INPUT_CODE like '" + filter + "%'", DataUser.RLZY);
                return OracleOledbBase.ExecuteDataSet(sql).Tables[0];

            }
            else
            {
                string sql = string.Format(" select STAFF_ID,NAME from {0}.NEW_STAFF_INFO ", DataUser.RLZY);
                return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
            }

        }
        #endregion

        public void InsertIncomeAdd( string s_date, string d_date, string to_date_tiem)
        {
            MyLists listtable = new MyLists();
            
            int t_id = OracleOledbBase.GetMaxID("id", "HISFACT.INCOME_UPDATE_DATE");
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"insert into HISFACT.INCOME_UPDATE_DATE(id,S_DATE,D_DATE,DATE_TIME,TO_DATE_TIME)
                                  values({0},'{1}','{2}',sysdate,'{3}')", t_id,  s_date, d_date, to_date_tiem);

            //OleDbParameter[] parameter = new OleDbParameter[] { };
            //List list = new List();
            //list.StrSql = strSql.ToString();
            //list.Parameters = parameter;
            //listtable.Add(list);

            //OracleOledbBase.ExecuteTranslist(listtable);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        public void InsertIncomePRAdd(string s_date, string d_date, string to_date_tiem)
        {
            MyLists listtable = new MyLists();

            int t_id = OracleOledbBase.GetMaxID("id", "HISFACT.INCOME_UPDATE_PINGRIDATE");
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"insert into HISFACT.INCOME_UPDATE_PINGRIDATE(id,S_DATE,D_DATE,DATE_TIME,TO_DATE_TIME)
                                  values({0},'{1}','{2}',sysdate,'{3}')", t_id, s_date, d_date, to_date_tiem);

            //OleDbParameter[] parameter = new OleDbParameter[] { };
            //List list = new List();
            //list.StrSql = strSql.ToString();
            //list.Parameters = parameter;
            //listtable.Add(list);

            //OracleOledbBase.ExecuteTranslist(listtable);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }

        public DataSet GetUpdateData(string date)
        {
            string sql = string.Format(@"select ID,      
       S_DATE,
       D_DATE,
       to_char(DATE_TIME,'yyyy-mm-dd hh24:mi:ss') DATE_TIME,
       TO_DATE_TIME from HISFACT.INCOME_UPDATE_DATE where TO_CHAR (to_date(TO_DATE_TIME,'yyyy-mm-dd'), 'yyyy-MM')='{0}'", date);

            return OracleOledbBase.ExecuteDataSet(sql);
        }
        public DataSet GetUpdatePRData(string date)
        {
            string sql = string.Format(@"select ID,      
       S_DATE,
       D_DATE,
       to_char(DATE_TIME,'yyyy-mm-dd hh24:mi:ss') DATE_TIME,
       TO_DATE_TIME from HISFACT.INCOME_UPDATE_PINGRIDATE where TO_CHAR (to_date(TO_DATE_TIME,'yyyy-mm-dd'), 'yyyy-MM')='{0}'", date);

            return OracleOledbBase.ExecuteDataSet(sql);
        }
        public void Delete_date(string ID)
        {
            string sql = string.Format(@"DELETE FROM HISFACT.INCOME_UPDATE_DATE  WHERE ID='{0}'", ID);
            OracleOledbBase.ExecuteDataSet(sql);
        }
        public void Delete_PRdate(string ID)
        {
            string sql = string.Format(@"DELETE FROM  HISFACT.INCOME_UPDATE_PINGRIDATE   WHERE ID='{0}'", ID);
            OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 工作量界面绑定数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetleibieItemList(string type_code)
        {
            if (type_code == "")
            {
                type_code = "3";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select a.item_class,
                                   a.item_name,
                                   a.input_code,
                                   INP_GRADE  INP_GRADE,
                                   OUP_GRADE  OUP_GRADE,
                                   a.class_type,
                                   a.class_name,
                                   TYPE_CODE,
           RECK_NAME||':'||RECK_CLASS as CLASS_CODE
                                   from hisdata.leibie_dict a
                                   where TYPE_CODE='{0}'
                                   order by item_class", type_code);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 删除工作量类别
        /// </summary>
        /// <param name="item_code"></param>
        public void DelleibieItem(string item_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0}.leibie_dict where item_class='{1}'", DataUser.HISDATA, item_code);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        /// <summary>
        /// 添加工作量类别
        /// </summary>
        /// <param name="model"></param>
        public void InsertleibieItem(GoldNet.Model.IncomeItem model)
        {
            StringBuilder strSql = new StringBuilder();
            MyLists listtable = new MyLists();
            strSql.AppendFormat(@"insert into hisdata.leibie_dict
                                  (INP_GRADE,
                                   OUP_GRADE,
                                   TYPE_CODE,
                                   input_code,
                                   item_class,
                                   item_name)
                                   values('{0}','{1}','{2}',upper(trim('{3}')),'{4}','{5}')", model.INP_GRADE,
                                                                                                                model.OUP_GRADE,
                                                                                                                model.TYPE_CODE,
                                                                                                                model.Input_code,
                                                                                                                model.Item_class,
                                                                                                               model.Item_name);
            List listcenterdict = new List();
            listcenterdict.StrSql = strSql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            StringBuilder strSql1 = new StringBuilder();
            strSql1.AppendFormat(@"update hisdata.leibie_dict a
                                       set class_type =
                                           (select class_type
                                              from hisdata.leibie_dict
                                             where item_class = substr(a.item_class, 1, 1)),
                                           class_name =
                                           (select class_name
                                              from hisdata.leibie_dict
                                             where item_class = substr(a.item_class, 1, 1))
                                     where a.class_type is null");
            List listcenterdetail = new List();
            listcenterdetail.StrSql = strSql1.ToString();
            listcenterdetail.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdetail);
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 重设工作量分配比例
        /// </summary>
        /// <param name="model"></param>
        public void UpdateleibieItem(GoldNet.Model.IncomeItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"update hisdata.leibie_dict a
                                     set a.INP_GRADE   = ?,
                                         a.OUP_GRADE = ?
                                         WHERE  a.TYPE_CODE  = ?
                                            and a.ITEM_CLASS = ?");
            OleDbParameter[] parameter = {    
                                              new OleDbParameter("INP_GRADE",model.INP_GRADE),
                                              new OleDbParameter("OUP_GRADE",model.OUP_GRADE),
                                              new OleDbParameter("TYPE_CODE",model.TYPE_CODE.ToString()),
                                              new OleDbParameter("ITEM_CLASS",model.Item_class)
                                         };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
        }

        /// <summary>
        /// 通过收费编码找到工作量信息
        /// </summary>
        /// <param name="item_class"></param>
        /// <returns></returns>
        public DataSet GetLeibieIncomeItem(string item_class, string TYPE_CODE)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT c.item_class AS item_class,
                                   c.item_name item_name,
                                   c.input_code input_code,
                                   INP_GRADE INP_GRADE,
                                   OUP_GRADE OUP_GRADE,
                                   TYPE_CODE TYPE_CODE,
                                   c.CLASS_TYPE,
                                   c.CLASS_NAME,
                                    TYPE_CODE
                              FROM hisdata.leibie_dict c
                             WHERE c.item_class = '{0}'
                               and TYPE_CODE='{1}' 
                             ORDER BY item_class", item_class, TYPE_CODE);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
    }
}
