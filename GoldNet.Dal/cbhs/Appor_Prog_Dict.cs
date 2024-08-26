using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
using System.Collections;

namespace Goldnet.Dal
{
    //1、解决方案字典设置
    //2、自定义SQL解决方案
    //3、设置方案的默认的科室
    public class Appor_Prog_Dict
    {
        /*--------------------------------------------------------成本分摊方案中心---------------------------------------------------------------------------*/
        /// <summary>
        /// 获得分解方案字典表
        /// </summary>
        /// <returns></returns>
        public DataTable GetProgDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select * from CBHS.V_CBHS_COST_APPOR_DICT", DataUser.CBHS);
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetProgDict(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select prog_code,prog_name,input_code,PROG_EXPRESS,flags,APPLY_DEPT
             from {0}.CBHS_COST_APPOR_PROG_DICT a where id=" +id+"", DataUser.CBHS);
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
            str.AppendFormat(@" select * from {0}.CBHS_COST_APPOR_DICT ", DataUser.CBHS);
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
        public void InsertProgDict(string progcode, string progname, string inputcode, string type)
        {
            string sql = string.Format(@" insert into {0}.CBHS_COST_APPOR_PROG_DICT (ID,PROG_CODE,PROG_NAME,INPUT_CODE,FLAGS) 
             select nvl(max(ID),0)+1,'" + progcode + "','" + progname + "','" + inputcode + "','" + type + "' from {0}.CBHS_COST_APPOR_PROG_DICT", DataUser.CBHS);
             OracleOledbBase.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 更新分解方案
        /// </summary>
        public void UpateProgDict(string id, string progcode, string progname, string inputcode, string type)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] cmdParms = new OleDbParameter[] { };
            //更新分解方案比率的代码
            string sqlratio = string.Format(@"update {0}.CBHS_COST_APPOR_RATIO set PROG_CODE='" + progcode + "' where PROG_CODE=(select PROG_CODE from {0}.CBHS_COST_APPOR_PROG_DICT where ID="+id+")", DataUser.CBHS);
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
            string sqlProg = string.Format(@" update {0}.CBHS_COST_APPOR_PROG_DICT set PROG_CODE='" + progcode + "',PROG_NAME='" + progname + "',INPUT_CODE='" + inputcode + "',FLAGS='" + type + "' where ID=" + id + "", DataUser.CBHS);
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
            string sqlratio = string.Format(@"delete from  {0}.CBHS_COST_APPOR_RATIO  where PROG_CODE=(select PROG_CODE from {0}.CBHS_COST_APPOR_PROG_DICT where ID=" + id + ")", DataUser.CBHS);
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
            string sqlProg = string.Format(@"delete from  {0}.CBHS_COST_APPOR_PROG_DICT where ID=" + id + "", DataUser.CBHS);
            List listpro = new List();
            listpro.StrSql = sqlProg.ToString();
            listpro.Parameters = cmdParms;
            listttrans.Add(listpro);

            OracleOledbBase.ExecuteTranslist(listttrans);
        }
        public void UpdateDefaultDept(string id, string dept)
        {
            string sqlProg = string.Format(@" update {0}.CBHS_COST_APPOR_PROG_DICT set APPLY_DEPT='" + dept + "' where ID=" + id + "", DataUser.CBHS);
            OracleOledbBase.ExecuteNonQuery(sqlProg);
        }
        /// <summary>
        /// 指标SQL检查
        /// </summary>
        /// <param name="guidecode"></param>
        /// <param name="guidesql"></param>
        /// <param name="colcnt"></param>
        /// <returns></returns>
        public string CheckGuideSql(string guidecode, string guidesql, int colcnt)
        {
            string rtn = "";
            if (guidesql.Equals(""))
            {
                rtn = "SQL语句不可以为空";
                return rtn;
            }
            if (!guidesql.Contains("'" + guidecode + "'"))
            {
                rtn = "指标SQL中未包含指标代码'" + guidecode + "',请检查";
                return rtn;
            }
            DataTable dt = new DataTable();
            try
            {
                dt = OracleOledbBase.ExecuteDataSet(guidesql).Tables[0];
            }
            catch
            {
                rtn = "指标SQL语法有误，请检查！";
                return rtn;
            }
            if (dt.Columns.Count != colcnt)
            {
                rtn = "指标SQL返回结果集列数错误,<br/>月份指标为5列,区间指标为6列,请检查！";
                return rtn;
            }
            return rtn;
        }
        public string CheckSqlexpress(string sql)
        {
            string rtn = "";
            if (sql.Equals(""))
            {
                rtn = "SQL语句不可以为空";
                return rtn;
            }
            DataTable dt = new DataTable();
            try
            {
               dt= OracleOledbBase.ExecuteDataSet(sql).Tables[0];
            }
            catch
            {
                rtn = "SQL语法有误，请检查！";
                return rtn;
            }
            if (dt.Columns[0].ColumnName.ToUpper() != "DEPT_CODE" ||
                dt.Columns[1].ColumnName.ToUpper() != "RECK_ITEM" ||
                dt.Columns[2].ColumnName.ToUpper() != "INCOMES" ||
                dt.Columns[3].ColumnName.ToUpper() != "INCOMES_CHARGES" ||
                dt.Columns[4].ColumnName.ToUpper() != "BALANCE_TAG" ||
                dt.Columns[5].ColumnName.ToUpper() != "COSTS_TAG" ||
                dt.Columns[6].ColumnName.ToUpper() != "DATE_TIME" ||
                dt.Columns[7].ColumnName.ToUpper() != "DEPT_TYPE_FLAG" ||
                dt.Columns[8].ColumnName.ToUpper() != "APPEND_FLAG" ||
                dt.Columns[9].ColumnName.ToUpper() != "DEPT_CODE_OP" ||
                dt.Columns[10].ColumnName.ToUpper() != "DEPT_CODE_EX"
                )
            {
                rtn = "SQL字段名不对，请检查！";
            }
            
                return rtn;
        }
        /// <summary>
        /// 数字讲评部门过滤初始化树结构
        /// </summary>
        /// <param name="DeptType">组织类别</param>
        /// <param name="queryString">部门类别</param>
        /// <returns></returns>
        public DataSet getDeptForGuideLook()
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("    SELECT name,id,pid  ");
            str.AppendFormat("     FROM {0}.sys_dept_treeview  ", DataUser.COMM);
            str.AppendFormat("     where attr = '是'  ");

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }
        /*------------------------------------------------成本分摊比例字典-----------------------------------------------------------------------*/
        /// <summary>
        /// 成本核算分解字典查询
        /// </summary>
        /// <returns></returns>
       
        public DataTable GetProgSql()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PROG_ITEM,PROG_NAME,PROG_SQL,b.APPORTION_NAME APPORTION_CODE,PROG_MEMO  
                                from {0}.CBHS_COST_APPOR_PROG_SQL a
                                left join {0}.CBHS_COST_APPOR_DICT b
                                on nvl(a.APPORTION_CODE,1)=b.APPORTION_CODE
                                where nvl(FLAGS,0)=0  order by PROG_ITEM", DataUser.CBHS);
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
        public DataTable GetOthersqlexpress(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select id,SQL_NAME,SQLEXPRESS,FLAGS,DECODE(FLAGS,'0','启用','停用') FLAGSS,MEMO from {0}.CBHS_OTHER_SQLEXPRESS ", DataUser.CBHS);
            if (id != "")
            {
                str.AppendFormat(" where id={0}",id);
            }
           return  OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        public DataTable GetProgSqlList()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PROG_ITEM,PROG_NAME,0 as PROG_VALUES  
                                from {0}.CBHS_COST_APPOR_PROG_SQL 
                                where nvl(FLAGS,0)=0  order by PROG_ITEM", DataUser.CBHS);
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
        public DataTable GetProgSqlList(string faid)
        {
            if (faid == "3") faid = "1";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PROG_ITEM,PROG_NAME,0 as PROG_VALUES  
                                from {0}.CBHS_COST_APPOR_PROG_SQL 
                                where nvl(FLAGS,0)=0 and APPORTION_CODE='"+faid+"'  order by PROG_ITEM", DataUser.CBHS);
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
        public void UpdateSQL(string id, string expressSql)
        {
            string sql = " update cbhs.CBHS_COST_APPOR_PROG_DICT set PROG_EXPRESS='" + expressSql + "' where ID='"+id+"'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }
        public DataTable GetProgSql(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PROG_ITEM,PROG_NAME,PROG_SQL,APPORTION_CODE,PROG_MEMO
             from {0}.CBHS_COST_APPOR_PROG_SQL a where PROG_ITEM='" + id + "' and nvl(FLAGS,0)=0", DataUser.CBHS);
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

        public void InsertProgSql(string progcode, string progname, string prosql, string apportioncode, string progmeno)
        {
            string sql = string.Format(@" insert into {0}.CBHS_COST_APPOR_PROG_SQL (PROG_ITEM,PROG_NAME,PROG_SQL,APPORTION_CODE,PROG_MEMO,FLAGS) 
             values ('" + progcode + "','"+progname+"','"+prosql+"','"+apportioncode+"','"+progmeno+"',0)", DataUser.CBHS);
            OracleOledbBase.ExecuteNonQuery(sql);
        }
        public void InsertOthercbhssql(string sqlname, string sqlexpress, string flags, string memo)
        {
            string str = string.Format(" insert into {0}.CBHS_OTHER_SQLEXPRESS(id,sql_name,sqlexpress,flags,memo) values (?,?,?,?,?)",DataUser.CBHS);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("ID",OracleOledbBase.GetMaxID("id","CBHS.CBHS_OTHER_SQLEXPRESS")),
															   new OleDbParameter("SQL_NAME",sqlname),
				                                                new OleDbParameter("SQLEXPRESS",sqlexpress),
                                                                new OleDbParameter("FLAGS",flags),
                                                                new OleDbParameter("MEMO",memo)
														   };
            OracleOledbBase.ExecuteNonQuery(str,cmdPara);
        }
        public void UpdateOthercbhssql(string id, string sqlname, string sqlexpress, string flags, string memo)
        {
            string str = string.Format(" update {0}.CBHS_OTHER_SQLEXPRESS set SQL_NAME=?,SQLEXPRESS=?,FLAGS=?,MEMO=? where id={1}",DataUser.CBHS,id);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("SQL_NAME",sqlname),
				                                                new OleDbParameter("SQLEXPRESS",sqlexpress),
                                                                new OleDbParameter("FLAGS",flags),
                                                                new OleDbParameter("MEMO",memo)
														   };
            OracleOledbBase.ExecuteNonQuery(str,cmdPara);
        }

        /// <summary>
        /// 更新分解方案
        /// </summary>
        public void UpateProgSql(string progcode, string progname, string prosql, string apportioncode, string progmeno)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] cmdParms = new OleDbParameter[] { };
          
            //更新分解方案代码
            string sqlProg = string.Format(@" update {0}.CBHS_COST_APPOR_PROG_SQL set PROG_ITEM='" + progcode + "',PROG_NAME='" + progname + "',PROG_SQL='" + prosql + "',APPORTION_CODE='" + apportioncode + "',PROG_MEMO='" + progmeno + "' where PROG_ITEM='" + progcode + "'", DataUser.CBHS);
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
        public void DeleteProgSql(string id)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] cmdParms = new OleDbParameter[] { };
            
            string sqlratio = string.Format(@"update {0}.CBHS_COST_APPOR_PROG_SQL  set FLAGS=1 where PROG_ITEM='"+id+"'", DataUser.CBHS);
            List listratio = new List();
            listratio.StrSql = sqlratio.ToString();
            listratio.Parameters = cmdParms;
            listttrans.Add(listratio);
          

            OracleOledbBase.ExecuteTranslist(listttrans);
        }
        public void DeleteOthersql(string id)
        {
            string str = string.Format("delete {0}.CBHS_OTHER_SQLEXPRESS where id={1}",DataUser.CBHS,id);
            OracleOledbBase.ExecuteNonQuery(str);
        }
        public void SaveCosttoDeptDecom(List<GoldNet.Model.PageModels.deptselected> deptlist, string deptcode,string itemcode,string stardate,string enddate,string gettype,string username)
        {
            Cost_detail jddal = new Cost_detail();
            string date_time = DateTime.Parse(stardate).ToString("yyyMM");
            MyLists listttrans = new MyLists();
            OleDbParameter[] Parmsdel = new OleDbParameter[] { };
            //
            string strdel = string.Format(@"delete {0}.CBHS_DEPT_COST_DETAIL a where a.dept_code='{1}'  and a.GET_TYPE='{2}' and a.ACCOUNTING_DATE>=date'{3}' and a.ACCOUNTING_DATE<add_months(date'{4}',1) and a.ITEM_CODE='{5}'", DataUser.CBHS, deptcode, gettype, stardate, enddate,itemcode);

            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = Parmsdel;
            listttrans.Add(listdel);
            foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
            {
                double jdprog = jddal.GetDeptJdProgInfo(date_time, dept.DEPT_CODE, itemcode);
                double costs = Convert.ToDouble(dept.RATIO) * jdprog;
                double costs_armyfree = Convert.ToDouble(dept.RATIO) - costs;

                string str = string.Format("insert into {0}.CBHS_DEPT_COST_DETAIL (DEPT_CODE,ITEM_CODE,ACCOUNTING_DATE,COSTS,COSTS_ARMYFREE,OPERATOR,OPERATOR_DATE,GET_TYPE) values (?,?,date'{1}',?,?,?,SYSDATE,?)", DataUser.CBHS, stardate);
                OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("DEPT_CODE",dept.DEPT_CODE),
															   new OleDbParameter("ITEM_CODE",itemcode),
                                                               new OleDbParameter("COSTS",costs),
                                                                new OleDbParameter("COSTS_ARMYFREE",costs_armyfree),
				                                                new OleDbParameter("OPERATOR",username),
                                                                new OleDbParameter("GET_TYPE",gettype)
														   };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = cmdPara;
                listttrans.Add(listadd);
            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }
        /// <summary>
        /// 保存科室归集科室设置
        /// </summary>
        /// <param name="deptlist"></param>
        /// <param name="progcode"></param>
        public void SaveCosttoDept(List<GoldNet.Model.PageModels.deptselected> deptlist, string indexid)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] Parmsdel = new OleDbParameter[] { };
            //
            string strdel = string.Format(@"delete {0}.CBHS_COST_TO_DEPT_DETAIL a where a.index_id='{1}'", DataUser.CBHS, indexid);

            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = Parmsdel;
            listttrans.Add(listdel);
            foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
            {
                string str = string.Format("insert into {0}.CBHS_COST_TO_DEPT_DETAIL (INDEX_ID,DEPT_CODE,RATIO) values (?,?,?)", DataUser.CBHS);
                OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("INDEX_ID",indexid),
															   new OleDbParameter("DEPT_CODE",dept.DEPT_CODE),
				                                                new OleDbParameter("RATIO",Convert.ToDouble(dept.RATIO))
														   };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = cmdPara;
                listttrans.Add(listadd);
            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }
        /// <summary>
        /// 保存分解方案
        /// </summary>
        /// <param name="deptlist"></param>
        /// <param name="progcode"></param>
        public void SaveProgDept(List<GoldNet.Model.PageModels.deptselected> deptlist, string progcode)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] Parmsdel = new OleDbParameter[] { };
            //
            string strdel = string.Format(@"delete {0}.CBHS_COST_APPOR_DEPT_DETAIL a where a.prog_code='{1}'",DataUser.CBHS,progcode);

            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = Parmsdel;
            listttrans.Add(listdel);
            foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
            {
                string str = string.Format("insert into {0}.CBHS_COST_APPOR_DEPT_DETAIL (PROG_CODE,DEPT_CODE,RATIO) values (?,?,?)",DataUser.CBHS);
                OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("PROG_CODE",progcode),
															   new OleDbParameter("DEPT_CODE",dept.DEPT_CODE),
				                                                new OleDbParameter("RATIO",Convert.ToDouble(dept.RATIO))
														   };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = cmdPara;
                listttrans.Add(listadd);
            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }
        public DataTable  GetdeptPercent(string itemcode, string id)
        {
            StringBuilder str = new StringBuilder();
            if (id == "0")
            {
                str.AppendFormat("select round(ORDER_DEPT_DISTRIBUT+PERFORM_DEPT_DISTRIBUT+NURSING_PERCEN,4) in_numbers,round(OUT_OPDEPT_PERCEN+OUT_EXDEPT_PERCEN+OUT_NURSING_PERCEN,4) out_numbers from cbhs.CBHS_DISTRIBUTION_CALC_SCHM where item_class='{0}'", itemcode);
            }
            else
            {
                str.AppendFormat("select round(ORDER_DEPT_DISTRIBUT+PERFORM_DEPT_DISTRIBUT+NURSING_PERCEN,4) in_numbers,round(OUT_OPDEPT_PERCEN+OUT_EXDEPT_PERCEN+OUT_NURSING_PERCEN,4) out_numbers from cbhs.CBHS_DEPT_INCOM_ITEM_DICT where id={0}", id);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        public void SaveItemotherDept(List<GoldNet.Model.PageModels.deptselected> deptlist, string itemcode,string incomtype,string otherflags,string id)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] Parmsdel = new OleDbParameter[] { };
            //
            string strdel = string.Format(@"delete {0}.CBHS_OTHER_INCOM_ITEM a where a.item_code='{1}' and a.INCOM_TYPE='{2}' and a.OTHER_FLAGS='{3}' and a.OTHER_ID={4}", DataUser.CBHS, itemcode, incomtype, otherflags,id);

            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = Parmsdel;
            listttrans.Add(listdel);
            foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
            {
                string str = string.Format("insert into {0}.CBHS_OTHER_INCOM_ITEM (ITEM_CODE,DEPT_CODE,DEPT_PERCEN,INCOM_TYPE,OTHER_FLAGS,OTHER_ID) values (?,?,?,?,?,?)", DataUser.CBHS);
                OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("ITEM_CODE",itemcode),
															   new OleDbParameter("DEPT_CODE",dept.DEPT_CODE),
				                                                new OleDbParameter("DEPT_PERCEN",Convert.ToDouble(dept.RATIO)/100),
                                                                new OleDbParameter("INCOM_TYPE",incomtype),
                                                                new OleDbParameter("OTHER_FLAGS",otherflags),
                                                                new OleDbParameter("OTHER_ID",id)

														   };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = cmdPara;
                listttrans.Add(listadd);
            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }
        /// <summary>
        /// 科室归集项目
        /// </summary>
        /// <param name="fjfa"></param>
        /// <returns></returns>
        public DataSet GetCostItembyindex(string indexid)
        {
            string str = string.Format("select a.item_code,a.item_name from {0}.CBHS_COST_ITEM_DICT a,{0}.CBHS_COST_TO_DEPT_ITEM b where a.item_code=b.ITEM_CODE and b.INDEX_ID={1}", DataUser.CBHS, indexid);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 查询分解方案的成本项目
        /// </summary>
        /// <param name="fjfa"></param>
        /// <returns></returns>
        public DataSet GetCostItembyProg(string fjfa)
        {
            string str = string.Format("select a.item_code,a.item_name from {0}.CBHS_COST_ITEM_DICT a,{0}.CBHS_COST_APPOR_COST_DETAIL b where a.item_code=b.ITEM_CLASS and b.PROG_CODE='{1}'",DataUser.CBHS,fjfa);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        public DataSet GetDeptByotherdeptitem(string itemcode,string incomtype,string otherflags,string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT  a.dept_code, b.dept_name, a.dept_percen*100 ratio
  FROM cbhs.cbhs_other_incom_item a, comm.sys_dept_dict b
 WHERE a.dept_code = b.dept_code and a.item_code='{0}'
 and a.incom_type='{1}' and a.other_flags='{2}' and a.other_id={3}",itemcode,incomtype,otherflags,id);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public DataSet GetNoCheckDeptByitem(string itemcode, string depttype,string incomtype,string otherflags,string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.dept_code,a.dept_name,0 RATIO FROM {0}.sys_dept_dict a where a.SHOW_FLAG='0' and attr='是' and a.dept_code  not in (select dept_code from {1}.CBHS_OTHER_INCOM_ITEM b
  WHERE b.item_code='{2}' and b.INCOM_TYPE='{3}' and b.OTHER_FLAGS='{4}' and b.other_id={5})", DataUser.COMM, DataUser.CBHS, itemcode,incomtype,otherflags,id);
            if (!depttype.Equals(string.Empty))
            {
                str.AppendFormat(" and a.dept_type ='{0}'", depttype);
            }
            str.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 查询分解方案外科室
        /// </summary>
        /// <param name="fjfa"></param>
        /// <returns></returns>
        public DataSet GetNoCheckDeptByProg(string fjfa,string depttype)
        {
            StringBuilder str = new StringBuilder();
//            str.AppendFormat(@"SELECT a.dept_code,a.dept_name FROM {0}.sys_dept_dict a,{1}.CBHS_COST_APPOR_PROG_DICT b 
//  WHERE b.prog_code='{2}' and a.attr='是' and  INSTR (REPLACE (','||b.APPLY_DEPT || ' ', ' ', ','), ','||dept_code || ',' ) = 0", DataUser.COMM, DataUser.CBHS, fjfa);
            str.AppendFormat(@"SELECT a.dept_code,a.dept_name,0 RATIO FROM {0}.sys_dept_dict a where a.SHOW_FLAG='0' and attr='是' and a.dept_code  not in (select dept_code from {1}.CBHS_COST_APPOR_DEPT_DETAIL b
  WHERE b.prog_code='{2}')", DataUser.COMM, DataUser.CBHS, fjfa);
            if (!depttype.Equals(string.Empty))
            {
                str.AppendFormat(" and a.dept_type ='{0}'",depttype);
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
//            string str = string.Format(@"SELECT a.dept_code,a.dept_name FROM {0}.sys_dept_dict a,{1}.CBHS_COST_APPOR_PROG_DICT b 
//  WHERE b.prog_code='{2}' and  INSTR (REPLACE (','||b.APPLY_DEPT || ' ', ' ', ','), ','||dept_code || ',' ) > 0", DataUser.COMM, DataUser.CBHS, fjfa);
            string str = string.Format(@"SELECT a.dept_code,a.dept_name,c.RATIO FROM {0}.sys_dept_dict a,{1}.CBHS_COST_APPOR_PROG_DICT b,{1}.CBHS_COST_APPOR_DEPT_DETAIL c
  WHERE a.show_flag='0' and a.attr='是' and a.dept_code=c.DEPT_CODE and b.PROG_CODE=c.PROG_CODE AND  b.prog_code='{2}' order by a.dept_code", DataUser.COMM, DataUser.CBHS, fjfa);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 查询科室归集科室
        /// </summary>
        /// <param name="indexid"></param>
        /// <returns></returns>
        public DataSet GetCostTodept(string indexid)
        {
            string str = string.Format(@"select a.dept_code,b.dept_name,a.ratio from cbhs.CBHS_COST_TO_DEPT_DETAIL a,comm.sys_dept_dict b where a.dept_code=b.dept_code and A.INDEX_ID={0}",indexid);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 成本未归集科室
        /// </summary>
        /// <param name="fjfa"></param>
        /// <param name="depttype"></param>
        /// <returns></returns>
        public DataSet GetNoCheckDeptByindex(string indexid, string depttype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.dept_code,a.dept_name,0 RATIO FROM {0}.sys_dept_dict a where a.SHOW_FLAG='0' and a.attr='是'  and a.dept_code  not in (select dept_code from {1}.CBHS_COST_TO_DEPT_DETAIL b
  WHERE b.index_id={2})", DataUser.COMM, DataUser.CBHS, indexid);
            if (!depttype.Equals(string.Empty))
            {
                str.AppendFormat(" and a.dept_type ='{0}'", depttype);
            }
            str.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public DataSet GetCheckDeptBydepttype(string depttype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.dept_code,a.dept_name,0 RATIO FROM {0}.sys_dept_dict a where a.SHOW_FLAG='0'", DataUser.COMM);
            if (!depttype.Equals(string.Empty))
            {
                str.AppendFormat(" and a.dept_type ='{0}'", depttype);
            }
            str.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 没选择的项目
        /// </summary>
        /// <param name="fjfa"></param>
        /// <returns></returns>
        public DataSet GetNoCheckItem(string fjfa)
        {
            string str = string.Format("select a.item_code,a.item_name from {0}.CBHS_COST_ITEM_DICT a where a.item_code not in (select b.item_class from {0}.CBHS_COST_APPOR_COST_DETAIL b where b.PROG_CODE='{1}')",DataUser.CBHS,fjfa);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 科室归集没选择的项目
        /// </summary>
        /// <param name="indexid"></param>
        /// <returns></returns>
        public DataSet GetNoCheckItembyindex(string indexid)
        {
            string str = string.Format("select a.item_code,a.item_name from {0}.CBHS_COST_ITEM_DICT a where a.item_code not in (select b.item_code from {0}.CBHS_COST_TO_DEPT_ITEM b where b.index_id={1})", DataUser.CBHS, indexid);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 分解方案
        /// </summary>
        /// <returns></returns>
        public DataSet GetFJFA()
        {
            string str = string.Format("select PROG_CODE,PROG_NAME from {0}.CBHS_COST_APPOR_PROG_DICT",DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        public DataSet GetIncomitem()
        {
            string str = string.Format("select item_class,item_name from {0}.CBHS_DISTRIBUTION_CALC_SCHM order by item_class",DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 保存分摊方案的成本项目
        /// </summary>
        /// <param name="costlist"></param>
        /// <param name="fjfa"></param>
        public void SavefjfaCostitem(List<GoldNet.Model.PageModels.costselected> costlists, string fjfa)
        {
            MyLists listtable = new MyLists();
            string strdel = string.Format("delete {0}.CBHS_COST_APPOR_COST_DETAIL where PROG_CODE=?", DataUser.CBHS);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", fjfa) };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.costselected costlist in costlists)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.CBHS_COST_APPOR_COST_DETAIL(", DataUser.CBHS);
                strSql.Append("PROG_CODE,ITEM_CLASS)");
                strSql.Append(" values (");
                strSql.Append("?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("PROG_CODE",fjfa),
											 
                                              new OleDbParameter("ITEM_CLASS", costlist.ITEM_CODE) 
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 保存科室归集项目
        /// </summary>
        /// <param name="costlists"></param>
        /// <param name="indexid"></param>
        public void SavecosttodeptCostitem(List<GoldNet.Model.PageModels.costselected> costlists, string indexid)
        {
            MyLists listtable = new MyLists();
            string strdel = string.Format("delete {0}.CBHS_COST_TO_DEPT_ITEM where INDEX_ID=?", DataUser.CBHS);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", indexid) };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.costselected costlist in costlists)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.CBHS_COST_TO_DEPT_ITEM(", DataUser.CBHS);
                strSql.Append("INDEX_ID,ITEM_CODE)");
                strSql.Append(" values (");
                strSql.Append("?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("INDEX_ID",indexid),
											 
                                              new OleDbParameter("ITEM_CODE", costlist.ITEM_CODE) 
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }


        /// <summary>
        /// 通过收入编码查询项目编码以及名称
        /// </summary>
        /// <param name="itemcode">前台带出来的项目编码</param>
        /// <returns></returns>
        public DataSet GetNoCheckDeptByitemleibie(string itemcode, string TYPE_CODE)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.item_code,a.item_name,a.price,a.item_spec,0 RATIO FROM {0}.leibie a where grade='{1}' and grade_class='{2}'", DataUser.HISDATA, itemcode, TYPE_CODE);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询所有项目编码以及名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetNoCheckDeptByitemallleibie(string TYPE_CODE)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"select ITEM_CODE,ITEM_NAME from HISDATA.LEIBIE_OTHER WHERE TYPE_CLASS='{0}' and memo=0", TYPE_CODE);


            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// 获取工作量类别编码和名字
        /// </summary>
        /// <returns></returns>
        public DataSet GetIncomitemliebie()
        {
            string str = string.Format("select item_code,item_name from {0}.leibie ", DataUser.HISDATA);
            return OracleOledbBase.ExecuteDataSet(str);
        }

        /// <summary>
        /// 删除工作量分类
        /// </summary>
        /// <param name="leibielist"></param>
        /// <param name="itemcode"></param>
        public void deleteleibie(List<GoldNet.Model.PageModels.liebieselected> leibielist, string itemcode)
        {
            MyLists listttrans = new MyLists();

            string str = string.Format("update hisdata.leibie set grade='' where grade='{0}' ", itemcode);
            List listadd = new List();
            listadd.StrSql = str;
            listttrans.Add(listadd);
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 修改与添加工作量类别
        /// </summary>
        /// <param name="deptlist"></param>
        /// <param name="itemcode"></param>
        /// <param name="incomtype"></param>
        /// <param name="otherflags"></param>
        /// <param name="id"></param>
        public void addleibie(List<GoldNet.Model.PageModels.liebieselected> leibielist, string itemcode)
        {
            MyLists listttrans = new MyLists();
            foreach (GoldNet.Model.PageModels.liebieselected dept in leibielist)
            {


                if (dept.ITEM_SPEC == "")
                {
                    string str = string.Format("update hisdata.leibie a set grade='{0}' where a.item_code='{1}' and item_name='{2}' and price='{3}' ", itemcode, dept.ITEM_CODE, dept.ITEM_NAME, dept.PRICE);

                    List listadd = new List();
                    listadd.StrSql = str;
                    listttrans.Add(listadd);
                }
                else
                {
                    string str = string.Format("update hisdata.leibie a set grade='{0}' where a.item_code='{1}' and item_name='{2}' and price='{3}' and a.item_spec='{4}'", itemcode, dept.ITEM_CODE, dept.ITEM_NAME, dept.PRICE, dept.ITEM_SPEC);

                    List listadd = new List();
                    listadd.StrSql = str;
                    listttrans.Add(listadd);
                }
            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 查询没有修改的工作量编码
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public DataSet GetNoCheckDeptByitemnotleibie(string itemcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.item_code,a.item_name,a.price,a.item_spec,0 RATIO FROM {0}.leibie a where class_on_reckoning='{1}'", DataUser.HISDATA, itemcode);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        public void insertleibie(List<GoldNet.Model.PageModels.liebieselected> leibielist, string itemcode, string TYPE_CODE)
        {
            MyLists listttrans = new MyLists();

            string str = string.Format("insert into HISDATA.LEIBIE_OTHER (ITEM_CODE,ITEM_NAME,TYPE_CLASS,MEMO) select item_code,item_name,'{0}',0 from hisdata.leibie where grade='{1}' and grade_class='{0}'", TYPE_CODE, itemcode);
            List listadd = new List();
            listadd.StrSql = str;
            listttrans.Add(listadd);
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 删除工作量分类
        /// </summary>
        /// <param name="leibielist"></param>
        /// <param name="itemcode"></param>
        public void deleteleibie(List<GoldNet.Model.PageModels.liebieselected> leibielist, string itemcode, string TYPE_CODE)
        {
            MyLists listttrans = new MyLists();
            //string str = string.Format("update hisdata.leibie set grade='' where grade='{0}' and grade_class='{1}'", itemcode, TYPE_CODE);
            string str = string.Format("delete hisdata.leibie  where grade='{0}' and grade_class='{1}'", itemcode, TYPE_CODE);
            List listadd = new List();
            listadd.StrSql = str;
            listttrans.Add(listadd);
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 修改与添加工作量类别
        /// </summary>
        /// <param name="deptlist"></param>
        /// <param name="itemcode"></param>
        /// <param name="incomtype"></param>
        /// <param name="otherflags"></param>
        /// <param name="id"></param>
        public void addleibie(List<GoldNet.Model.PageModels.liebieselected> leibielist, string itemcode, string TYPE_CODE)
        {
            MyLists listttrans = new MyLists();
            foreach (GoldNet.Model.PageModels.liebieselected dept in leibielist)
            {



                //string str = string.Format("update hisdata.leibie a set grade='{0}' where a.item_code='{1}' and item_name='{2}' and price='{3}' and grade_class='{4}'", itemcode, dept.ITEM_CODE, dept.ITEM_NAME, dept.PRICE, TYPE_CODE);
                string str = string.Format("insert into hisdata.leibie(ITEM_CODE,ITEM_NAME,GRADE,GRADE_CLASS) values (?,?,?,?)");
                OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("ITEM_CODE",dept.ITEM_CODE),
															   new OleDbParameter("ITEM_NAME",dept.ITEM_NAME),
				                                               new OleDbParameter("GRADE",itemcode),
                                                               new OleDbParameter("GRADE_CLASS",TYPE_CODE)
														   };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = cmdPara;
                listttrans.Add(listadd);

            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        public void deleteleibie_other(List<GoldNet.Model.PageModels.liebieselected> leibielist, string itemcode, string TYPE_CODE)
        {
            MyLists listttrans = new MyLists();
            //string str = string.Format("update hisdata.leibie set grade='' where grade='{0}' and grade_class='{1}'", itemcode, TYPE_CODE);
            string str = string.Format("delete from  hisdata.leibie_other  where item_code in (select item_code from hisdata.leibie where grade='{0}' and grade_class='{1}') and TYPE_CLASS='{1}'", itemcode, TYPE_CODE);
            List listadd = new List();
            listadd.StrSql = str;
            listttrans.Add(listadd);
            OracleOledbBase.ExecuteTranslist(listttrans);
        }


    }
}
