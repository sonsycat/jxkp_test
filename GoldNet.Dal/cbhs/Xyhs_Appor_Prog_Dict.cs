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

namespace Goldnet.Dal.cbhs
{
    /// <summary>
    /// 1、解决方案字典设置
    /// 2、自定义SQL解决方案
    /// 3、设置方案的默认的科室
    /// </summary>
    public class Xyhs_Appor_Prog_Dict
    {
        /// <summary>
        /// 成本核算分解字典查询
        /// </summary>
        /// <returns></returns>
        public DataTable GetProgSql()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PROG_ITEM,PROG_NAME,PROG_SQL,b.APPORTION_NAME APPORTION_CODE,PROG_MEMO  
                                from {0}.XYHS_COST_APPOR_PROG_SQL a
                                left join {0}.XYHS_COST_APPOR_DICT b
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

        public DataTable Getpatient()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PATIENT_ID,NAME,SEX,IDENTITY,CHARGE_TYPE,to_char(STAR_DATE,'yyyy-mm-dd') STAR_DATE,b.DIAGNOSIS_NAME,a.VISIT_ID,a.OUT_OR_IN    
                                from {0}.XYHS_PATIENT_DICT a,{0}.XYHS_DIAGNOSIS_DICT b where STAR_DATE>add_months(Sysdate,-24) and a.DIAGNOSIS_CODE=b.DIAGNOSIS_CODE(+) order by STAR_DATE desc", DataUser.CBHS);
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
        /// 获取病种和ICD对对照表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiagnosisIcd()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select * from {0}.XYHS_DIAGNOSIS_ICD ", DataUser.CBHS);
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
        /// <param name="patientid"></param>
        /// <param name="visitid"></param>
        /// <param name="outin"></param>
        public void deletepatientdict(string patientid, string visitid, string outin)
        {
            StringBuilder str = new StringBuilder();
            if (outin == "0")
            {
                str.AppendFormat("delete cbhs.XYHS_PATIENT_DICT where patient_id='{0}' and VISIT_ID={1}", patientid, visitid);
            }
            else
            {
                str.AppendFormat("delete cbhs.XYHS_PATIENT_DICT where patient_id='{0}'", patientid);
            }
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 删除病种ICD对照
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDiagnosisIcd(string id)
        {
            string sql = string.Format(@"delete from CBHS.XYHS_DIAGNOSIS_ICD  where id={0}",id);
            OracleOledbBase.ExecuteNonQuery(sql);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientid"></param>
        /// <param name="visitid"></param>
        /// <param name="outin"></param>
        public void deletepatientdict(string patientid, string visitid, string outin, string inputdate)
        {
            StringBuilder str = new StringBuilder();
            if (outin == "0")
            {
                str.AppendFormat("delete cbhs.XYHS_PATIENT_DICT where patient_id='{0}' and VISIT_ID={1}", patientid, visitid);
            }
            else
            {
                str.AppendFormat("delete cbhs.XYHS_PATIENT_DICT where patient_id='{0}' and STAR_DATE=to_date('{1}','yyyy-mm-dd')", patientid, inputdate);
            }
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        public string getpatientdate(string patientid, string visitid,string outin)
        {
            if (outin == "0")
            {
                string str = string.Format("select max(to_char(DIAGNOSIS_DATE,'yyyy-MM-dd')) from hisdata.DIAGNOSIS where PATIENT_ID='{0}' and VISIT_ID={1}", patientid, visitid);
                object ob = OracleOledbBase.ExecuteScalar(str);
                if (ob.ToString() != "")
                {
                    return ob.ToString();
                }
                else
                    return System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                string str = string.Format("select min(to_char(VISIT_DATE,'yyyy-MM-dd')) from hisdata.OUTP_MR where PATIENT_ID='{0}'", patientid);
                object ob = OracleOledbBase.ExecuteScalar(str);
                if (ob.ToString() != "")
                {
                    return ob.ToString();
                }
                else
                    return System.DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientid"></param>
        /// <param name="date"></param>
        /// <param name="DIAGNOSIS_CODE"></param>
        /// <param name="visitid"></param>
        /// <param name="outin"></param>
        public void insertpatientdict(string patientid, string date, string DIAGNOSIS_CODE,string visitid,string outin)
        {
             string strdate="";
             if (date == "")
             {
                 strdate = getpatientdate(patientid, visitid,outin);
             }
             else
             {
                 strdate = date;
             }

             deletepatientdict(patientid, visitid, outin, date);
            StringBuilder str = new StringBuilder();
            if (outin == "0")
            {
                str.AppendFormat(@"insert into cbhs.XYHS_PATIENT_DICT (patient_id,name,sex,IDENTITY,CHARGE_TYPE,STAR_DATE,DIAGNOSIS_CODE,VISIT_ID,OUT_OR_IN) 
            select distinct patient_id,name,sex,IDENTITY,CHARGE_TYPE,to_date('{1}','yyyy-mm-dd'),'{2}',{3},{4} from HISDATA.V_PAT_MASTER_INDEX where patient_id='{0}'", patientid, strdate, DIAGNOSIS_CODE, visitid,outin);
            }
            else
            {
                str.AppendFormat(@"insert into cbhs.XYHS_PATIENT_DICT (patient_id,name,sex,IDENTITY,CHARGE_TYPE,STAR_DATE,DIAGNOSIS_CODE,VISIT_ID,OUT_OR_IN) 
            select distinct patient_id,name,sex,IDENTITY,CHARGE_TYPE,to_date('{1}','yyyy-mm-dd'),'{2}',0,{3} from HISDATA.CLINIC_MASTER where patient_id='{0}'", patientid, date, DIAGNOSIS_CODE, outin);
            }
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 添加病种ICD对照
        /// </summary>
        /// <param name="diagnosis_code"></param>
        /// <param name="diagnosis_name"></param>
        /// <param name="icd_code"></param>
        /// <param name="icd_name"></param>
        public void InsertDiagnosisIcd(string diagnosis_code, string diagnosis_name, string icd_code, string icd_name)
        {
            string sql = string.Format(@"insert into cbhs.XYHS_DIAGNOSIS_ICD(DIAGNOSIS_CODE,DIAGNOSIS_NAME,ICD_CODE,ICD_NAME,ID)
                                        values ('{0}','{1}','{2}','{3}',{4})",diagnosis_code,  diagnosis_name,  icd_code,  icd_name,OracleOledbBase.GetMaxID("ID","cbhs.XYHS_DIAGNOSIS_ICD"));
            OracleOledbBase.ExecuteNonQuery(sql);
        }

     
        public void UpdateDiagnosisIcd(string diagnosis_code, string diagnosis_name, string icd_code, string icd_name,string id)
        {
            string sql = string.Format(@"update cbhs.XYHS_DIAGNOSIS_ICD 
                                        set DIAGNOSIS_CODE='{0}',
                                            DIAGNOSIS_NAME='{1}',
                                            ICD_CODE='{2}',
                                            ICD_NAME='{3}'
                                        where ID={4}", diagnosis_code, diagnosis_name, icd_code, icd_name, id);
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 通过ID获取病种ICD对照
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetDiagnosisIcdById(string id)
        {
            string sql = string.Format(@"select * from cbhs.XYHS_DIAGNOSIS_ICD where id={0}",id);
            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];

        }
        /// <summary>
        /// 删除分解方案
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProgSql(string id)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] cmdParms = new OleDbParameter[] { };

            string sqlratio = string.Format(@"update {0}.XYHS_COST_APPOR_PROG_SQL  set FLAGS=1 where PROG_ITEM='" + id + "'", DataUser.CBHS);
            List listratio = new List();
            listratio.StrSql = sqlratio.ToString();
            listratio.Parameters = cmdParms;
            listttrans.Add(listratio);


            OracleOledbBase.ExecuteTranslist(listttrans);
        }


        public DataTable GetProgSql(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PROG_ITEM,PROG_NAME,PROG_SQL,APPORTION_CODE,PROG_MEMO
             from {0}.XYHS_COST_APPOR_PROG_SQL a where PROG_ITEM='" + id + "' and nvl(FLAGS,0)=0", DataUser.CBHS);
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
        public DataTable GetdiagnsosiDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select * from {0}.XYHS_DIAGNOSIS_DICT ", DataUser.CBHS);
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

        /// <summary>
        /// 添加分解方案
        /// </summary>
        /// <param name="progcode"></param>
        /// <param name="progname"></param>
        /// <param name="prosql"></param>
        /// <param name="apportioncode"></param>
        /// <param name="progmeno"></param>
        public void InsertProgSql(string progcode, string progname, string prosql, string apportioncode, string progmeno)
        {
            string sql = string.Format(@" insert into {0}.XYHS_COST_APPOR_PROG_SQL (PROG_ITEM,PROG_NAME,PROG_SQL,APPORTION_CODE,PROG_MEMO,FLAGS) 
             values ('" + progcode + "','" + progname + "','" + prosql + "','" + apportioncode + "','" + progmeno + "',0)", DataUser.CBHS);
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新分解方案
        /// </summary>
        public void UpateProgSql(string progcode, string progname, string prosql, string apportioncode, string progmeno)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] cmdParms = new OleDbParameter[] { };

            //更新分解方案代码
            string sqlProg = string.Format(@" update {0}.XYHS_COST_APPOR_PROG_SQL set PROG_ITEM='" + progcode + "',PROG_NAME='" + progname + "',PROG_SQL='" + prosql + "',APPORTION_CODE='" + apportioncode + "',PROG_MEMO='" + progmeno + "' where PROG_ITEM='" + progcode + "'", DataUser.CBHS);
            List listpro = new List();
            listpro.StrSql = sqlProg.ToString();
            listpro.Parameters = cmdParms;
            listttrans.Add(listpro);

            OracleOledbBase.ExecuteTranslist(listttrans);
        }



        //----------------------------------------------------------------------------------------------

        /// <summary>
        /// 获得分解方案字典表
        /// </summary>
        /// <returns></returns>
        public DataTable GetProgDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select * from CBHS.V_xyHS_COST_APPOR_DICT", DataUser.CBHS);
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


        public DataTable GetProgSqlList()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PROG_ITEM,PROG_NAME,0 as PROG_VALUES  
                                from {0}.xyHS_COST_APPOR_PROG_SQL 
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

        public DataTable GetProgSqlList(string faid)
        {
            if (faid == "3") faid = "1";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" select PROG_ITEM,PROG_NAME,0 as PROG_VALUES  
                                from {0}.xyHS_COST_APPOR_PROG_SQL 
                                where nvl(FLAGS,0)=0 and APPORTION_CODE='" + faid + "'  order by PROG_ITEM", DataUser.CBHS);
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
            string sql = " update cbhs.xyHS_COST_APPOR_PROG_DICT set PROG_EXPRESS='" + expressSql + "' where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
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

        public void UpdateDefaultDept(string id, string dept)
        {
            string sqlProg = string.Format(@" update {0}.xyHS_COST_APPOR_PROG_DICT set APPLY_DEPT='" + dept + "' where ID=" + id + "", DataUser.CBHS);
            OracleOledbBase.ExecuteNonQuery(sqlProg);
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// 增加一个分解方案
        /// </summary>
        /// <param name="progcode"></param>
        /// <param name="progname"></param>
        /// <param name="inputcode"></param>
        /// <param name="type"></param>
        public void InsertProgDict(string progcode, string progname, string inputcode, string type)
        {
            string sql = string.Format(@" insert into {0}.xyHS_COST_APPOR_PROG_DICT (ID,PROG_CODE,PROG_NAME,INPUT_CODE,FLAGS) 
             select NVL(max(ID),0)+1,'" + progcode + "','" + progname + "','" + inputcode + "','" + type + "' from {0}.xyHS_COST_APPOR_PROG_DICT", DataUser.CBHS);
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
            string sqlProg = string.Format(@" update {0}.xyHS_COST_APPOR_PROG_DICT set PROG_CODE='" + progcode + "',PROG_NAME='" + progname + "',INPUT_CODE='" + inputcode + "',FLAGS='" + type + "' where ID=" + id + "", DataUser.CBHS);
            List listpro = new List();
            listpro.StrSql = sqlProg.ToString();
            listpro.Parameters = cmdParms;
            listttrans.Add(listpro);

            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        //----------------------------------------------------------------------

        /// <summary>
        /// 分解方案
        /// </summary>
        /// <returns></returns>
        public DataSet GetFJFA()
        {
            string str = string.Format("select PROG_CODE,PROG_NAME from {0}.XYHS_COST_APPOR_PROG_DICT", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(str);
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
            string strdel = string.Format(@"delete {0}.XYHS_COST_APPOR_DEPT_DETAIL a where a.prog_code='{1}'", DataUser.CBHS, progcode);

            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = Parmsdel;
            listttrans.Add(listdel);
            foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
            {
                string str = string.Format("insert into {0}.XYHS_COST_APPOR_DEPT_DETAIL (PROG_CODE,DEPT_CODE,RATIO) values (?,?,?)", DataUser.CBHS);
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


        /// <summary>
        /// 查询分解方案外科室
        /// </summary>
        /// <param name="fjfa"></param>
        /// <returns></returns>
        public DataSet GetNoCheckDeptByProg(string fjfa, string depttype)
        {
            StringBuilder str = new StringBuilder();
            //            str.AppendFormat(@"SELECT a.dept_code,a.dept_name FROM {0}.sys_dept_dict a,{1}.CBHS_COST_APPOR_PROG_DICT b 
            //  WHERE b.prog_code='{2}' and a.attr='是' and  INSTR (REPLACE (','||b.APPLY_DEPT || ' ', ' ', ','), ','||dept_code || ',' ) = 0", DataUser.COMM, DataUser.CBHS, fjfa);
            str.AppendFormat(@"SELECT a.dept_code,a.dept_name,0 RATIO FROM {0}.sys_dept_dict a where a.SHOW_FLAG='0' and attr='是' and a.dept_code  not in (select dept_code from {1}.CBHS_COST_APPOR_DEPT_DETAIL b
  WHERE b.prog_code='{2}')", DataUser.COMM, DataUser.CBHS, fjfa);
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
            //            string str = string.Format(@"SELECT a.dept_code,a.dept_name FROM {0}.sys_dept_dict a,{1}.CBHS_COST_APPOR_PROG_DICT b 
            //  WHERE b.prog_code='{2}' and  INSTR (REPLACE (','||b.APPLY_DEPT || ' ', ' ', ','), ','||dept_code || ',' ) > 0", DataUser.COMM, DataUser.CBHS, fjfa);
            string str = string.Format(@"SELECT a.dept_code,a.dept_name,c.RATIO FROM {0}.sys_dept_dict a,{1}.XYHS_COST_APPOR_PROG_DICT b,{1}.XYHS_COST_APPOR_DEPT_DETAIL c
  WHERE a.show_flag='0' and a.attr='是' and a.dept_code=c.DEPT_CODE and b.PROG_CODE=c.PROG_CODE AND  b.prog_code='{2}' order by a.dept_code", DataUser.COMM, DataUser.CBHS, fjfa);
            return OracleOledbBase.ExecuteDataSet(str);
        }


        //-------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// 查询分解方案的成本项目
        /// </summary>
        /// <param name="fjfa"></param>
        /// <returns></returns>
        public DataSet GetCostItembyProg(string fjfa)
        {
            string str = string.Format("select a.item_code,a.item_name from {0}.xyHS_COST_ITEM_DICT a,{0}.xyHS_COST_APPOR_COST_DETAIL b where a.item_code=b.ITEM_CLASS and b.PROG_CODE='{1}'", DataUser.CBHS, fjfa);
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
            string strdel = string.Format("delete {0}.xyHS_COST_APPOR_COST_DETAIL where PROG_CODE=?", DataUser.CBHS);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", fjfa) };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.costselected costlist in costlists)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.xyHS_COST_APPOR_COST_DETAIL(", DataUser.CBHS);
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
        /// 没选择的项目
        /// </summary>
        /// <param name="fjfa"></param>
        /// <returns></returns>
        public DataSet GetNoCheckItem(string fjfa)
        {
            string str = string.Format("select a.item_code,a.item_name from {0}.XYHS_COST_ITEM_DICT a where a.item_code not in (select b.item_class from {0}.xyHS_COST_APPOR_COST_DETAIL b where b.PROG_CODE='{1}')", DataUser.CBHS, fjfa);
            return OracleOledbBase.ExecuteDataSet(str);
        }







    }
}
