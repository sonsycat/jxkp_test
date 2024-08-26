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
    /// <summary>
    /// 附加收入CBHS_DEPT_APPENDED_INCOME。
    /// </summary>
    public class Appended_income
    {
        public Appended_income()
        {

        }
        #region 成员方法
        //获取附加收入列表
        public DataSet GetList(string datetime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT A.ROWID AS ROW_ID,b.item_name as reck_item, a.incomes, a.incomes_charges,
             (SELECT c.dept_name
                FROM {2}.sys_dept_dict c
               WHERE a.ordered_by = c.dept_code) ordered_by,
             (SELECT c.dept_name
                FROM {2}.sys_dept_dict c
               WHERE a.performed_by = c.dept_code) performed_by, a.order_doctor,
             (SELECT c.dept_name
                FROM {2}.sys_dept_dict c
               WHERE a.ward_code = c.dept_code) ward_code, 
             (select c.imcom_type
                from {1}.cbhs_incom_type c
               where c.id=a.incom_type) incom_type,
             (select c.account_type
                from {1}.cbhs_account_signs c
               where c.id=a.account_type) account_type, 
             to_char(accounting_date,'yyyy-mm-dd') as accounting_date , remarks
        FROM {1}.cbhs_dept_appended_income a, {1}.cbhs_distribution_calc_schm b
       WHERE a.reck_item = b.item_class
         and to_char(a.accounting_date,'yyyy-mm')='{0}' order by accounting_date,operator_date desc", datetime, DataUser.CBHS, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        //获取指定rowid的数据
        public DataSet Get_Income(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT A.ROWID AS ROW_ID,b.item_class as reck_item, a.incomes, a.incomes_charges,ordered_by,
                                    (select c.dept_name from  comm.sys_dept_dict c where a.ordered_by=c.dept_code)  ordered_by_name,performed_by,
                                    (select c.dept_name from  comm.sys_dept_dict c where a.performed_by=c.dept_code) performed_by_name,
                                    a.order_doctor,ward_code,
                                    (select c.dept_name from  comm.sys_dept_dict c where a.ward_code=c.dept_code) ward_code_name,
                                    incom_type, account_type, 
                                    to_char(accounting_date,'yyyy-mm-dd') as accounting_date , remarks
                             FROM {0}.cbhs_dept_appended_income a, {0}.cbhs_distribution_calc_schm b
                             WHERE a.reck_item = b.item_class and a.rowid='{1}'", DataUser.CBHS, id, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        //添加附加收入
        public void Add_Income(GoldNet.Model.AppendIncome appendincome)
        {
            StringBuilder strSql = new StringBuilder();
            if (appendincome != null)
            {
                strSql.AppendFormat(@"insert into {0}.cbhs_dept_appended_income 
                    (reck_item,incomes,incomes_charges,ordered_by,performed_by,order_doctor,ward_code,incom_type,account_type,accounting_date,remarks,operator_date)
                    values(?,?,?,?,?,?,?,?,?,to_date(?,'yyyy-mm-dd'),?,sysdate)", DataUser.CBHS);
                OleDbParameter[] parameter = {
											  new OleDbParameter("reck_item",appendincome.Reck_item),
											  new OleDbParameter("incomes", appendincome.Incomes),
											  new OleDbParameter("incomes_charges", appendincome.Incomes_charges),
                                              new OleDbParameter("ordered_by", appendincome.Ordered_by) ,
                                              new OleDbParameter("performed_by",appendincome.Performed_by),
                                              new OleDbParameter("order_doctor",appendincome.Order_doctor),
                                              new OleDbParameter("ward_code",appendincome.Ward_code),
                                              new OleDbParameter("incom_type",appendincome.Incom_type),
                                              new OleDbParameter("account_type",appendincome.Account_type),
                                              new OleDbParameter("accounting_date",appendincome.Accounting_date),
                                              new OleDbParameter("remarks",appendincome.Remarks)
										  };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
            }
        }
        //更新附加收入
        public void Update_Income(GoldNet.Model.AppendIncome appendincome)
        {
            StringBuilder strSql = new StringBuilder();
            if (appendincome != null)
            {
                strSql.AppendFormat(@"update {0}.cbhs_dept_appended_income
                      set reck_item=?,
                          incomes=?,
                          incomes_charges=?,
                          ordered_by=?,
                          performed_by=?,
                          order_doctor=?,
                          ward_code=?,
                          incom_type=?,
                          account_type=?,
                          accounting_date=to_date(?,'yyyy-mm-dd'),
                          remarks=?,
                          operator_date = sysdate
                      where rowid=?", DataUser.CBHS);
                OleDbParameter[] parameter = {
											  new OleDbParameter("reck_item",appendincome.Reck_item),
											  new OleDbParameter("incomes", appendincome.Incomes),
											  new OleDbParameter("incomes_charges", appendincome.Incomes_charges),
                                              new OleDbParameter("ordered_by", appendincome.Ordered_by) ,
                                              new OleDbParameter("performed_by",appendincome.Performed_by),
                                              new OleDbParameter("order_doctor",appendincome.Order_doctor),
                                              new OleDbParameter("ward_code",appendincome.Ward_code),
                                              new OleDbParameter("incom_type",appendincome.Incom_type),
                                              new OleDbParameter("account_type",appendincome.Account_type),
                                              new OleDbParameter("accounting_date",appendincome.Accounting_date),
                                              new OleDbParameter("remarks",appendincome.Remarks),
                                              new OleDbParameter("rowid",appendincome.Row_id) };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
            }
        }
        //删除附加收入
        public void Del_Income(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0}.cbhs_dept_appended_income where rowid='{1}'", DataUser.CBHS, id);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        //删除整月附加收入
        public void Del_Incomes(string datetime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0}.cbhs_dept_appended_income where to_char(accounting_date,'yyyy-mm')='{1}'", DataUser.CBHS, datetime);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }
        /// <summary>
        /// 院成本
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DataSet GetHospitalcostsList(string datetime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT ID, dept_code, (SELECT b.dept_name
                         FROM {0}.sys_dept_dict b
                        WHERE a.dept_code = b.dept_code) dept_name,
       cost_name,account_date,costs,to_cost_code,
       (select c.item_name from {1}.CBHS_COST_ITEM_DICT c where c.item_code=a.to_cost_code) to_cost_name,
       (select d.prog_name from {1}.CBHS_COST_APPOR_PROG_DICT d where d.prog_code=a.hos_prog_code) hos_prog_name,hos_prog_code,
       (select d.prog_name from {1}.CBHS_COST_APPOR_PROG_DICT d where d.prog_code=a.dept_prog_code) dept_prog_name,dept_prog_code,
       operator_date,operator,memo
  FROM {1}.cbhs_hospital_costs a where to_char(a.account_date,'yyyy-mm')='{2}'", DataUser.COMM, DataUser.CBHS, datetime);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetHospitalcostsdetail(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT ID, dept_code, (SELECT b.dept_name
                         FROM {0}.sys_dept_dict b
                        WHERE a.dept_code = b.dept_code) dept_name,
       cost_name,account_date,costs,to_cost_code,
       (select c.item_name from {1}.CBHS_COST_ITEM_DICT c where c.item_code=a.to_cost_code) to_cost_name,
       (select d.prog_name from {1}.CBHS_COST_APPOR_PROG_DICT d where d.prog_code=a.hos_prog_code) hos_prog_name,hos_prog_code,
       (select d.prog_name from {1}.CBHS_COST_APPOR_PROG_DICT d where d.prog_code=a.dept_prog_code) dept_prog_name,dept_prog_code,
       operator_date,operator,memo
  FROM {1}.cbhs_hospital_costs a where a.id={2}", DataUser.COMM, DataUser.CBHS, id);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 添加院分解成本
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="costname"></param>
        /// <param name="accountdate"></param>
        /// <param name="costs"></param>
        /// <param name="tocostcode"></param>
        /// <param name="progcode"></param>
        /// <param name="operators"></param>
        /// <param name="memo"></param>
        public void AddHospitalcost(string deptcode, string costname, string accountdate, double costs, string tocostcode, string hosprogcode, string deptprogcode, string operators, string memo)
        {
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.cbhs_hospital_costs", DataUser.CBHS));
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"insert into {0}.cbhs_hospital_costs(id,DEPT_CODE,COST_NAME,ACCOUNT_DATE,COSTS,TO_COST_CODE,HOS_PROG_CODE,DEPT_PROG_CODE,OPERATOR_DATE,OPERATOR,MEMO) values(?,?,?,to_date(?,'yyyy-mm-dd'),?,?,?,?,to_date(?,'yyyy-mm-dd'),?,?)", DataUser.CBHS);
            OleDbParameter[] parameter = {
											  new OleDbParameter("id",id),
											  new OleDbParameter("DEPT_CODE", deptcode),
											  new OleDbParameter("COST_NAME", costname),
                                              new OleDbParameter("ACCOUNT_DATE", accountdate) ,
                                              new OleDbParameter("COSTS",costs),
                                              new OleDbParameter("TO_COST_CODE",tocostcode),
                                              new OleDbParameter("HOS_PROG_CODE",hosprogcode),
                                              new OleDbParameter("DEPT_PROG_CODE",deptprogcode),
                                              new OleDbParameter("OPERATOR_DATE",System.DateTime.Now.ToString("yyyy-MM-dd")),
                                              new OleDbParameter("OPERATOR",operators),
                                              new OleDbParameter("MEMO",memo)
										  };
            OracleOledbBase.ExecuteNonQuery(str.ToString(), parameter);
        }
        /// <summary>
        /// 修改院分解成本
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deptcode"></param>
        /// <param name="costname"></param>
        /// <param name="accountdate"></param>
        /// <param name="costs"></param>
        /// <param name="tocostcode"></param>
        /// <param name="progcode"></param>
        /// <param name="operators"></param>
        /// <param name="memo"></param>
        public void updateHospitalcost(int id, string deptcode, string costname, string accountdate, double costs, string tocostcode, string hosprogcode, string deptprogcode, string operators, string memo)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"update {0}.cbhs_hospital_costs set DEPT_CODE=?,COST_NAME=?,ACCOUNT_DATE=to_date(?,'yyyy-mm-dd'),COSTS=?,TO_COST_CODE=?,HOS_PROG_CODE=?,DEPT_PROG_CODE=?,OPERATOR_DATE=to_date(?,'yyyy-mm-dd'),OPERATOR=?,MEMO=? where id=?", DataUser.CBHS);
            OleDbParameter[] parameter = {
											  new OleDbParameter("DEPT_CODE", deptcode),
											  new OleDbParameter("COST_NAME", costname),
                                              new OleDbParameter("ACCOUNT_DATE", accountdate) ,
                                              new OleDbParameter("COSTS",costs),
                                              new OleDbParameter("TO_COST_CODE",tocostcode),
                                              new OleDbParameter("HOS_PROG_CODE",hosprogcode),
                                              new OleDbParameter("DEPT_PROG_CODE",deptprogcode),
                                              new OleDbParameter("OPERATOR_DATE",System.DateTime.Now.ToString("yyyy-MM-dd")),
                                              new OleDbParameter("OPERATOR",operators),
                                              new OleDbParameter("MEMO",memo),
                                              new OleDbParameter("id",id),
										  };
            OracleOledbBase.ExecuteNonQuery(str.ToString(), parameter);
        }
        //删除院分解成本
        public void Del_Hospitalcost(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0}.cbhs_hospital_costs where id={1}", DataUser.CBHS, id);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());
        }

        //成本导入
        public void saveAssets(DataTable dt, string st_date)
        {
            int r_id = OracleOledbBase.GetMaxID("ROWS_ID", "cbhs.INPUT_DATA_REAL") + 1;

            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            StringBuilder ITEM_CODE = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < dt.Rows.Count - 1)
                {
                    ITEM_CODE.AppendFormat(@"'{0}',", dt.Rows[i]["ITEM_CODE"].ToString());
                }
                else
                {
                    ITEM_CODE.AppendFormat(@"'{0}'", dt.Rows[i]["ITEM_CODE"].ToString());
                }
            }
            strDel.AppendFormat("DELETE FROM cbhs.INPUT_DATA_REAL WHERE st_date='{0}'and  ITEM_CODE in ({1})", st_date, ITEM_CODE);

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { };
            listtable.Add(listDel);
            //
            for (int i = 0; i < dt.Rows.Count; i++)
            {


                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"insert into cbhs.INPUT_DATA_REAL(  ST_DATE ,
                                                                                ITEM_CODE ,
                                                                                DEPT_CODE,
                                                                                COSTS,
                                                                                ROWS_ID)
                                                  values('{0}','{1}','{2}','{3}','{4}')"
                                                            , dt.Rows[i]["ST_DATE"].ToString()
                                                            , dt.Rows[i]["ITEM_CODE"].ToString()
                                                            , dt.Rows[i]["DEPT_CODE"].ToString()
                                                            , dt.Rows[i]["COSTS"].ToString()
                                                            , Convert.ToString(r_id + i));

                OleDbParameter[] parameteradd = { };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        //当量积分导入
        public bool saveDljf_Dr(DataTable dt)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            StringBuilder ITEM_CODE = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < dt.Rows.Count - 1)
                {
                    ITEM_CODE.AppendFormat(@"'{0}',", dt.Rows[i]["ITEM_CODE"].ToString());
                }
                else
                {
                    ITEM_CODE.AppendFormat(@"'{0}'", dt.Rows[i]["ITEM_CODE"].ToString());
                }
            }
            strDel.AppendFormat("DELETE FROM HISDATA.LEIBIE_HZ where item_code in ({0}) ", ITEM_CODE);

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { };
            listtable.Add(listDel);

            for (int i = 0; i < dt.Rows.Count; i++)
            {


                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"insert into HISDATA.LEIBIE_HZ(
                                                                                ITEM_CODE ,
                                                                                ITEM_NAME,CLASS_NAME,ITEM_UNIT,ITEM_PRICE,ZHB,PANDU,ZHIXING,HULI,INPUT_CODE)
                                                  values('{0}','{1}','{2}','{3}','{4}','{5}'，'{6}','{7}','{8}',comm.f_pinyin('{9}') )"
                                                            , dt.Rows[i]["ITEM_CODE"].ToString()
                                                            , dt.Rows[i]["ITEM_NAME"].ToString()
                                                            , dt.Rows[i]["CLASS_NAME"].ToString()
                                                            , dt.Rows[i]["ITEM_UNIT"].ToString()
                                                            , dt.Rows[i]["ITEM_PRICE"].ToString()
                                                            , dt.Rows[i]["ZHB"].ToString()
                                                            , dt.Rows[i]["PANDU"].ToString()
                                                            , dt.Rows[i]["ZHIXING"].ToString()
                                                            , dt.Rows[i]["HULI"].ToString()
                                                            , dt.Rows[i]["ITEM_NAME"].ToString() );

                OleDbParameter[] parameteradd = { };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            return OracleOledbBase.ExecuteTranslist_Bool(listtable);
        }

        //奖金导入
        public void saveJjdr(DataTable dt, string st_date)
        {
            string date = st_date + "01";

            MyLists listtable = new MyLists();
            //StringBuilder strDel = new StringBuilder();

            //strDel.AppendFormat("DELETE FROM performance.JJ_DR WHERE st_date='{0}' and dept_code='{1}' and user_id='{2}' ", date);

            //List listDel = new List();
            //listDel.StrSql = strDel;
            //listDel.Parameters = new OleDbParameter[] { };
            //listtable.Add(listDel);

            //
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StringBuilder strDel = new StringBuilder();

                strDel.AppendFormat("DELETE FROM performance.JJ_DR WHERE st_date='{0}' and dept_code='{1}' and user_id='{2}' ", date, dt.Rows[i]["DEPT_CODE"].ToString(), dt.Rows[i]["USER_ID"].ToString());

                List listDel = new List();
                listDel.StrSql = strDel;
                listDel.Parameters = new OleDbParameter[] { };
                listtable.Add(listDel);


                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"insert into performance.JJ_DR(  ST_DATE ,
                                                                   COSTS ,
                                                                   USER_ID,DEPT_CODE)
                                                  values('{0}','{1}','{2}','{3}')"
                                                            , date
                                                            ,  Convert.ToDouble(dt.Rows[i]["COSTS"].ToString())
                                                            , dt.Rows[i]["USER_ID"].ToString()
                                                            , dt.Rows[i]["DEPT_CODE"].ToString()
                                                            );

                OleDbParameter[] parameteradd = { };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        public void savegongzuoliang(DataTable dt, string st_date)
        {


            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            StringBuilder ITEM_CODE = new StringBuilder();

            strDel.AppendFormat("DELETE FROM HISDATA.LEIBIE");

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { };
            listtable.Add(listDel);
            //
            for (int i = 0; i < dt.Rows.Count; i++)
            {


                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"insert into HISDATA.LEIBIE(  DEPT_CODE ,
                                                                                DEPT_NAME ,
                                                                                CLASS_NAME,
                                                                                ITEM_CODE,
                                                                                ITEM_NAME,ITEM_UNIT,ITEM_PRICE,PANDU,ZHIXING)
                                                  values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')"
                                                            , dt.Rows[i]["DEPT_CODE"].ToString()
                                                            , dt.Rows[i]["DEPT_NAME"].ToString()
                                                            , dt.Rows[i]["CLASS_NAME"].ToString()
                                                            , dt.Rows[i]["ITEM_NAME"].ToString()
                                                            , dt.Rows[i]["ITEM_UNIT"].ToString()
                                                            , dt.Rows[i]["ITEM_PRICE"].ToString()
                                                            , dt.Rows[i]["PANDU"].ToString()
                                                            , dt.Rows[i]["ZHIXING"].ToString());

                OleDbParameter[] parameteradd = { };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        #endregion 成员方法
        public DataSet GetAssetsByDate(string s_date)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT   ST_DATE,
         ITEM_CODE,
         DEPT_CODE,
         COSTS,
         ROWS_ID
  FROM   CBHS.INPUT_DATA_REAL");

            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }
        public void AssetsDelById(string id)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"delete from cbhs.CBHS_ASSETS where id={0}", id);

            OracleOledbBase.ExecuteNonQuery(sql.ToString());
        }

    }
}
