using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;

namespace Goldnet.Dal
{
	/// <summary>
	/// 权限维护
	/// </summary>
	public class SYS_ROLE_DICT
	{
		public SYS_ROLE_DICT()
		{}
		#region  成员方法

		/// <summary>
		/// 获得权限列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("with mm as  (select a.ROLE_ID,a.ROLE_NAME,a.REMARK,b.ROLE_TYPE,a.role_app");
            strSql.AppendFormat(" FROM {0}.SYS_ROLE_DICT a,{0}.SYS_ROLE_TYPE b where a.role_type=b.id", DataUser.COMM);
			if(strWhere.Trim()!="")
			{
				strSql.Append(" and "+strWhere);
			}
            strSql.Append(" order by a.ROLE_ID) select * from mm");
            return OracleBase.QuerySet(strSql.ToString());
               
		}
        //没被特殊权限选择的角色
        public DataSet GetRoleListspe(string id,string type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.ROLE_ID,a.ROLE_NAME");
            strSql.AppendFormat(" FROM {0}.SYS_ROLE_DICT a where a.ROLE_APP='-1' and a.role_id not in (select role_id from SYS_SPECPOWER_ROLE where id='{1}' and type={2})", DataUser.COMM,id,type);
            strSql.Append(" order by a.role_id");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());

        }
        /// <summary>
        /// 选中的特殊角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet GetSpeRole(string id, string type)
        {
            string str = string.Format("select a.role_id,a.role_name from {0}.sys_role_dict a,{1}.SYS_SPECPOWER_ROLE b where a.role_id=b.role_id and b.id='{2}' and b.type={3}",DataUser.COMM,DataUser.COMM,id,type);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 保存特殊角色
        /// </summary>
        /// <param name="rolelist"></param>
        /// <param name="id"></param>
        public void Saverolelist(List<GoldNet.Model.PageModels.roleselected> rolelist, string id,string type)
        {
            MyLists listtable = new MyLists();
            string strdel = string.Format("delete {0}.SYS_SPECPOWER_ROLE where id=? and type={1}", DataUser.COMM,type);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", id) };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.roleselected role in rolelist)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.SYS_SPECPOWER_ROLE(", DataUser.COMM);
                strSql.Append("id,role_id,type)");
                strSql.Append(" values (");
                strSql.Append("?,?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("id",id),
											  new OleDbParameter("role_id", role.ROLE_ID),
	                                          new OleDbParameter("type",int.Parse(type))
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 添加修改角色
        /// </summary>
        /// <param name="role"></param>
        public void AddRole(GoldNet.Model.SYS_ROLE_DICT role,string userid)
        {
            StringBuilder strSql = new StringBuilder();
            if (role.ROLE_ID != 0)
            {
                strSql.AppendFormat("update {0}.SYS_ROLE_DICT set ",DataUser.COMM);
                strSql.Append("ROLE_NAME=?,");
                strSql.Append("REMARK=?,");
                strSql.Append("ROLE_TYPE=?,");
                strSql.Append("ROLE_APP=?,");
                strSql.Append("USER_ID=?");
                strSql.Append(" where ROLE_ID=?");
                OleDbParameter[] parameters = {
											  new OleDbParameter("ROLE_NAME", role.ROLE_NAME),
											  new OleDbParameter("REMARK", role.REMARK),
											  new OleDbParameter("ROLE_TYPE", role.ROLE_TYPE) ,
                                              new OleDbParameter("ROLE_APP",role.ROLE_APP),
                                              new OleDbParameter("USER_ID",userid),
                                              new OleDbParameter("ROLE_ID",role.ROLE_ID)
										  };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
            }
            else
            {
                role.ROLE_ID = OracleOledbBase.GetMaxID("role_id", DataUser.COMM + ".SYS_ROLE_DICT");
                strSql.AppendFormat("insert into {0}.SYS_ROLE_DICT(", DataUser.COMM);
                strSql.Append("ROLE_ID,ROLE_NAME,REMARK,ROLE_TYPE,role_app,user_id)");
                strSql.Append(" values (");
                strSql.Append("?,?,?,?,?,?)");
                OleDbParameter[] parameters = {
											  new OleDbParameter("ROLE_ID",role.ROLE_ID),
											  new OleDbParameter("ROLE_NAME", role.ROLE_NAME),
											  new OleDbParameter("REMARK", role.REMARK),
											  new OleDbParameter("ROLE_TYPE", role.ROLE_TYPE),
                                              new OleDbParameter("ROLE_APP",role.ROLE_APP),
                                              new OleDbParameter("USER_ID",userid)
										  };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
            }
           
            
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleid"></param>
        public void RemoveRole(int roleid)
        {
            RemoveRolebyRoleid(roleid);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role"></param>
        public void RemoveRole(GoldNet.Model.SYS_ROLE_DICT role)
        {
            RemoveRolebyRoleid(role.ROLE_ID);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        private void RemoveRolebyRoleid(int roleid)
        {
            MyLists listtable = new MyLists();
            //删除角色
            string delroledict = string.Format("DELETE {0}.SYS_ROLE_DICT WHERE ROLE_ID=?", DataUser.COMM);
            List listdelroledict = new List();
            listdelroledict.StrSql = delroledict;
            listdelroledict.Parameters = new OleDbParameter[] { new OleDbParameter("", roleid) };
            listtable.Add(listdelroledict);
            //删除角色对应的功能
            string delrolefun = string.Format("DELETE {0}.SYS_ROLE_FUNCTION WHERE ROLE_ID=?", DataUser.COMM);
            List listdelrolefun = new List();
            listdelrolefun.StrSql = delrolefun;
            listdelrolefun.Parameters = new OleDbParameter[] { new OleDbParameter("", roleid) };
            listtable.Add(listdelrolefun);
            //删除角色里的人员
            string delroleper = string.Format("DELETE {0}.SYS_POWER_DETAIL WHERE POWER_ID=?", DataUser.COMM);
            List listdelroleper = new List();
            listdelroleper.StrSql = delroleper;
            listdelroleper.Parameters = new OleDbParameter[] { new OleDbParameter("", roleid) };
            listtable.Add(listdelroleper);
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        /// <summary>
        /// 角色类别
        /// </summary>
        public DataSet GetRoleType()
        {
            string strSql = string.Format("select * from {0}.sys_role_type",DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(strSql);

        }
        /// <summary>
        /// 项目列表
        /// </summary>
        public DataSet GetApplicationList(string appid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select app_id,app_name,case when power_type=0 then '否' else '是' end as power_type,role_id from {0}.SYS_APPLICATION_SUBSYS where del_f='0'", DataUser.COMM);
            if (!appid.Equals(string.Empty))
            {
                strSql.AppendFormat(" and app_id='{0}'",appid);
            }
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());

        }

        //public DataSet GetAppListByuserid(string userid)
        //{
        //    StringBuilder str = new StringBuilder();
        //    str.AppendFormat("select * from {0}.SYS_APPLICATION_SUBSYS where ");
        //}
        /// <summary>
        /// 修改项目设置
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="powertype"></param>
        /// <param name="roleid"></param>
        public void UpdateApplication(string appid,string powertype,string roleid)
        {
            string str = string.Format("update {0}.SYS_APPLICATION_SUBSYS set power_type='{1}',role_id='{2}' where app_id='{3}'",DataUser.COMM,powertype,roleid,appid);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        /// <summary>
        /// 功能表
        /// </summary>
        /// <param name="functiontype"></param>
        /// <returns></returns>
        public DataSet GetFunction(string functiontype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from {0}.SYS_FUNCTION_DICT ", DataUser.COMM);
            if (!functiontype.Equals(string.Empty))
            {
                strSql.AppendFormat(" where function_type='{0}'",functiontype);
            }
            strSql.Append(" order by function_name,function_id");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());

        }

        public DataSet GetFunction(string functiontype,string roleid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from {0}.SYS_FUNCTION_DICT a where function_type='{1}' and DEL_F='0'", DataUser.COMM, functiontype);
            if (!roleid.Equals(string.Empty))
            {
                strSql.AppendFormat(" and a.function_id not in (select function_id from {0}.SYS_ROLE_FUNCTION where role_id='{1}')",DataUser.COMM,roleid);
            }
            strSql.Append(" order by a.function_name,a.function_id");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());

        }

        public DataSet GetFunctionType()
        {
            string strSql = string.Format("select * from {0}.SYS_APPLICATION_SUBSYS where DEL_F='0'", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        /// <summary>
        /// 角色权限
        /// </summary>
        /// <param name="functiontype"></param>
        /// <returns></returns>
        public DataSet GetFunctionByRole(string roleid)
        {
            string strSql = string.Format("select b.function_id,b.function_name,b.function_type,a.edit,a.pass from {0}.SYS_ROLE_FUNCTION a, {0}.SYS_FUNCTION_DICT b where a.role_id=? and a.function_id=b.function_id order by FUNCTION_TYPE desc,FUNCTION_ID", DataUser.COMM);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", roleid)};
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);

        }
        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="functons"></param>
        public void SaveRoleFunction(List<GoldNet.Model.PageModels.functionselected> functions, int roleid)
        {
            MyLists listtable = new MyLists();
            string strdel = string.Format("delete {0}.SYS_ROLE_FUNCTION where role_id=?",DataUser.COMM);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", roleid) };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.functionselected function in functions)
            {
                if (function.EDIT == true.ToString() || function.EDIT == "1")
                {
                    function.EDIT = "1";
                }
                else
                {
                    function.EDIT = "0";
                }
                if (function.PASS == true.ToString() || function.PASS == "1")
                {
                    function.PASS = "1";
                }
                else
                {
                    function.PASS = "0";
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.SYS_ROLE_FUNCTION(", DataUser.COMM);
                strSql.Append("ROLE_ID,FUNCTION_ID,EDIT,PASS)");
                strSql.Append(" values (");
                strSql.Append("?,?,?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("ROLE_ID",roleid),
											  new OleDbParameter("FUNCTION_ID", function.FUNCTION_ID),
											  new OleDbParameter("EDIT", function.EDIT),
                                              new OleDbParameter("PASS", function.PASS) 
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// his用户
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetHisUsers()
        {
            string strSql = string.Format("select a.account,a.user_id,a.db_user,a.user_name,b.dept_name,b.dept_code from {0}.users a,{0}.DEPT_DICT b where b.dept_code=a.user_dept order by b.dept_code", DataUser.HISFACT);
            return OracleOledbBase.ExecuteDataSet(strSql);

        }
        /// <summary>
        /// 查询角色外人员
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public DataSet GetHisUsersbyrole(string roleid)
        {
            string strSql = string.Format("select a.account,a.user_id,a.db_user,a.user_name,b.dept_name,b.dept_code from {0}.users a,{0}.DEPT_DICT b where b.dept_code=a.user_dept and (a.user_id not in (select TARGET_ID from {1}.SYS_POWER_DETAIL where POWER_ID='{2}')) order by b.dept_code", DataUser.HISFACT,DataUser.COMM,roleid);
            return OracleOledbBase.ExecuteDataSet(strSql);

        }
        /// <summary>
        /// 查询医生
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetHisdoctorbydept(string deptcode)
        {
            string str = string.Format("select a.USER_ID,a.USER_NAME,a.DEPT_CODE,a.INPUT_CODE from {0}.DEPT_DOCTORS a where a.dept_code='{1}' and a.user_id not in (select user_id from {2}.SYS_DEPT_PERSONS)", DataUser.HISFACT, deptcode,DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str);

        }
        /// <summary>
        /// 查询人力资源人员
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetRlzyPersons(string deptcode)
        {
            string str = string.Format("select a.staff_id,a.NAME user_name,a.dept_code,a.dept_name,a.BANK_CODE from  {0}.NEW_STAFF_INFO a where a.dept_code in (select dept_code from {1}.sys_dept_dict b where b.account_dept_code='{2}')", DataUser.RLZY, DataUser.COMM, deptcode);
            return OracleOledbBase.ExecuteDataSet(str);

        }
       /// <summary>
       /// 按条件查询his用户
       /// </summary>
       /// <returns></returns>
        public DataSet GetHisUsers(string deptfilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.account,a.user_id,a.db_user,a.user_name,b.dept_name,b.dept_code from {0}.users a,{0}.DEPT_DICT b where b.dept_code=a.user_dept ", DataUser.HISFACT);
            if (deptfilter != "")
            {
                strSql.AppendFormat(" and (a.user_id like '{0}%' or a.db_user like '{0}%' or a.user_name like '{0}%' or b.dept_name like '{0}%' or b.dept_code like '{0}%')",deptfilter.ToUpper());
            }
            strSql.Append(" order by b.dept_code");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());

        }
        /// <summary>
        /// 查询医生
        /// </summary>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public DataSet GetHisdoctor(string deptfilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.user_id,a.user_name,a.dept_code from {0}.dept_doctors a ", DataUser.HISFACT);
           
            strSql.AppendFormat(" where (a.user_id = '{0}' or a.user_name ='{0}')", deptfilter.ToUpper());
            
            strSql.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        public DataSet GetDeptdoctor(string deptfilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.user_id,a.user_name,a.dept_code,b.dept_name from {0}.SYS_DEPT_PERSONS a,{0}.sys_dept_dict b where a.dept_code=b.dept_code ", DataUser.COMM);

            strSql.AppendFormat(" and (a.user_id = '{0}' or a.user_name ='{0}')", deptfilter.ToUpper());

            strSql.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// his用户
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetHisUsers(string inputcode,string deptfilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.user_id,a.db_user,a.user_name,b.dept_name,b.dept_code from {0}.users a,{0}.DEPT_DICT b where b.dept_code=a.user_dept and Upper(a.INPUT_CODE) like ? ", DataUser.HISFACT);

            if (!deptfilter.Equals(""))
            {
                strSql.AppendFormat(" and {0}", deptfilter);
            }
            strSql.Append(" order by b.dept_code");
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(),cmdPara);

        }
        /// <summary>
        /// staff用户
        /// </summary>
        /// <param name="inputcode"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public DataSet GetHisStaff(string inputcode, string deptfilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.emp_no,a.name,a.dept_code from {0}.staff_dict a where  Upper(a.input_code) like ? ", DataUser.HISFACT);

            if (!deptfilter.Equals(""))
            {
                strSql.AppendFormat(" and {0}", deptfilter);
            }
            strSql.Append(" order by a.dept_code");
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(), cmdPara);

        }
        /// <summary>
        /// 人力资源人员
        /// </summary>
        /// <param name="inputcode"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public DataSet GetStaffUsers(string inputcode, string staffid)
        {
            StringBuilder strSql = new StringBuilder();
            if (staffid != "")
            {
                strSql.AppendFormat("select a.staff_id,a.name,a.dept_name from {0}.new_staff_info a where  to_char(a.staff_id)={1}", DataUser.RLZY, staffid);
                strSql.Append(" union all ");
                strSql.AppendFormat(" select a.staff_id,a.name,a.dept_name from {0}.new_staff_info a where  (Upper(a.input_code) like ?  and to_char(a.staff_id)!={1} and a.add_mark='1')", DataUser.RLZY, staffid);
            }
            else
            {
                strSql.AppendFormat(" select a.staff_id,a.name,a.dept_name from {0}.new_staff_info a where a.add_mark='1' and  Upper(a.input_code) like ?", DataUser.RLZY);
            }
           
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(), cmdPara);

        }
        /// <summary>
        /// his科室
        /// </summary>
        /// <param name="inputcode">输入码</param>
        /// <returns></returns>
        public DataSet GetHisDept(string inputcode,string deptfilter)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.AppendFormat(@"select nvl(a.dept_code,b.dept_code) dept_code, nvl(a.dept_name,b.dept_name) dept_name, nvl(a.input_code,b.input_code) input_code from {0}.DEPT_DICT a full join {1}.sys_dept_dict b
 on a.dept_code = b.dept_code where Upper(nvl(a.input_code,b.input_code)) like ? ", DataUser.HISFACT,DataUser.COMM);
            if (!deptfilter.Equals(""))
            {
               // strSql.AppendFormat(" and a.{0}",deptfilter);
            }
            strSql.Append(" order by a.dept_code");
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(),cmdPara);
        }
        public DataSet GetHisDept(string inputcode)
        {
            string strSql = string.Format("select a.dept_code, a.dept_name,a.input_code from {0}.DEPT_DICT a where Upper(a.input_code) like ? order by a.dept_code", DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql,cmdPara);
        }
        /// <summary>
        /// 病人
        /// </summary>
        /// <param name="inputcode"></param>
        /// <returns></returns>
        public DataSet GetPatient(string inputcode,string patientid,string tablename,string fieldid)
        {
           
            StringBuilder str = new StringBuilder();
            if (patientid.Equals(string.Empty))
            {
                str.AppendFormat(@"SELECT a.patient_id , (SELECT b.NAME
                                  FROM {0}.pat_master_index b
                                 WHERE b.patient_id = a.patient_id) AS patient_NAME,
       b.dept_name AS dept_name, b.input_code AS input_code
  FROM  {0}.pats_in_hospital a, {1}.sys_dept_dict b
 WHERE a.dept_code = b.dept_code
   AND b.input_code LIKE '{2}%'
   AND a.settled_indicator(+) = 0
   AND (ROWNUM <= 100)", DataUser.HISFACT, DataUser.COMM, inputcode);
            }
            else
            {
                str.AppendFormat(@"SELECT distinct {2} as patient_id,{4} as patient_name,'' dept_name,'' input_code  from {0}.{1} where {2}='{3}'", DataUser.ZLGL, tablename, "patient_id_" + fieldid, patientid, "patient_" + fieldid);
                str.AppendFormat(@" union all  SELECT a.patient_id , (SELECT b.NAME
                                  FROM {0}.pat_master_index b
                                 WHERE b.patient_id = a.patient_id) AS patient_NAME,
       b.dept_name AS dept_name, b.input_code AS input_code
  FROM  {0}.pats_in_hospital a, {1}.sys_dept_dict b
 WHERE a.dept_code = b.dept_code and a.patient_id!='{3}'
   AND b.input_code LIKE '{2}%'
   AND a.settled_indicator(+) = 0
   AND (ROWNUM <= 100)", DataUser.HISFACT, DataUser.COMM, inputcode,patientid);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString(), new OleDbParameter("", inputcode.ToUpper()));
        }

        /// <summary>
        /// 提取角色中的人员
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public DataSet GetRoleUser(string roleid)
        {
            string strSql = string.Format(@"select b.account,b.user_id,b.db_user,b.user_name,c.dept_name, c.dept_code 
                                              from {0}.SYS_POWER_DETAIL a,{1}.users b,{2}.dept_dict c 
                                             where a.target_id=b.user_id(+) 
                                                   and b.user_dept=c.dept_code(+) 
                                                   and a.power_id=? 
                                             order by c.dept_code", DataUser.COMM,DataUser.HISFACT,DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", roleid) };
            return OracleOledbBase.ExecuteDataSet(strSql,cmdPara);
        }
        
        /// <summary>
        /// 人员赋权
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roleid"></param>
        public void SaveRoleUsers(List<GoldNet.Model.PageModels.userselected> users, int roleid)
        {
            MyLists listtable = new MyLists();
            string strpowuser = string.Format("select * from {0}.SYS_POWER_DETAIL where POWER_ID='{1}'", DataUser.COMM, roleid);
            DataTable tableuser = OracleOledbBase.ExecuteDataSet(strpowuser).Tables[0];
            for (int i = 0; i < tableuser.Rows.Count; i++)
            {
                PageModels.userselected puser = new PageModels.userselected();
                puser.USER_ID = tableuser.Rows[i]["TARGET_ID"].ToString();
                if (users.Exists(delegate(PageModels.userselected usersp) { if (usersp.USER_ID == puser.USER_ID) return true; else return false; }) == false)
                {
                    string ppower = string.Format(@"SELECT b.ROLE_ID
  FROM {0}.sys_application_subsys a, {0}.sys_role_dict b
 WHERE a.role_id = '{1}' AND a.app_id = b.role_app AND a.power_type = '1' and b.USER_ID='{2}'",DataUser.COMM,roleid.ToString(),puser.USER_ID);
                    DataTable pptable = OracleOledbBase.ExecuteDataSet(ppower).Tables[0];
                    for (int j = 0; j < pptable.Rows.Count; j++)
                    {
                        //删除角色
                        string delroledict = string.Format("DELETE {0}.SYS_ROLE_DICT WHERE ROLE_ID=?", DataUser.COMM);
                        List listdelroledict = new List();
                        listdelroledict.StrSql = delroledict;
                        listdelroledict.Parameters = new OleDbParameter[] { new OleDbParameter("", int.Parse(pptable.Rows[j]["ROLE_ID"].ToString())) };
                        listtable.Add(listdelroledict);
                        //删除角色对应的功能
                        string delrolefun = string.Format("DELETE {0}.SYS_ROLE_FUNCTION WHERE ROLE_ID=?", DataUser.COMM);
                        List listdelrolefun = new List();
                        listdelrolefun.StrSql = delrolefun;
                        listdelrolefun.Parameters = new OleDbParameter[] { new OleDbParameter("", int.Parse(pptable.Rows[j]["ROLE_ID"].ToString())) };
                        listtable.Add(listdelrolefun);
                        //删除角色里的人员
                        string delroleper = string.Format("DELETE {0}.SYS_POWER_DETAIL WHERE POWER_ID=?", DataUser.COMM);
                        List listdelroleper = new List();
                        listdelroleper.StrSql = delroleper;
                        listdelroleper.Parameters = new OleDbParameter[] { new OleDbParameter("", int.Parse(pptable.Rows[j]["ROLE_ID"].ToString())) };
                        listtable.Add(listdelroleper);
                    }
                }

            }
            string strdel = string.Format("delete {0}.SYS_POWER_DETAIL where POWER_ID=?", DataUser.COMM);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", roleid) };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.userselected user in users)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.SYS_POWER_DETAIL(", DataUser.COMM);
                strSql.Append("POWER_ID,TARGET_ID)");
                strSql.Append(" values (");
                strSql.Append("?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("POWER_ID",roleid),
											  new OleDbParameter("TARGET_ID", user.USER_ID) 
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 保存医生
        /// </summary>
        /// <param name="users"></param>
        /// <param name="deptcode"></param>
        public void SaveDeptDoctor(List<GoldNet.Model.PageModels.doctorselected> users, string deptcode)
        {
            MyLists listtable = new MyLists();
            string del = string.Format("DELETE {0}.SYS_DEPT_PERSONS WHERE DEPT_CODE=?", DataUser.COMM);
            List listdel = new List();
            listdel.StrSql = del;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", deptcode) };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.doctorselected doctor in users)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.SYS_DEPT_PERSONS(", DataUser.COMM);
                strSql.Append("USER_ID,USER_NAME,DEPT_CODE)");
                strSql.Append(" values (");
                strSql.Append("?,?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("USER_ID",doctor.USER_ID),
											  new OleDbParameter("USER_NAME", doctor.USER_NAME),
                                              new OleDbParameter("DEPT_CODE", deptcode) 
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 查询人员权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet GetUserPower(string userid)
        {
            string strSql = string.Format("SELECT D.ROLE_NAME,A.FUNCTION_NAME FROM {0}.sys_function_dict a,{0}.sys_power_detail b,{0}.sys_role_function c, {0}.SYS_ROLE_DICT d WHERE a.function_id = c.function_id AND b.power_id = c.role_id and D.ROLE_ID=B.POWER_ID and B.TARGET_ID=? order by d.role_id", DataUser.COMM);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", userid) };
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
        }
        /// <summary>
        /// 查询功能人员
        /// </summary>
        /// <param name="functionid"></param>
        /// <returns></returns>
        public DataSet GetUserbyFunction(string functionid)
        {
            string strSql = string.Format("SELECT C.USER_NAME,B.ROLE_NAME FROM {0}.sys_role_function a,{0}.sys_role_dict b,{1}.users c,{0}.sys_power_detail d WHERE a.role_id = D.POWER_ID and B.ROLE_ID=A.ROLE_ID and C.USER_ID=D.TARGET_ID and A.FUNCTION_ID=?", DataUser.COMM,DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", functionid) };
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
        }
      
		#endregion  成员方法

        #region
        /// <summary>
        /// 分析报表类别初始化树结构
        /// </summary>
        /// <returns></returns>
        public DataSet GetCostType()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                  SELECT COST_TYPE_CODE CLASSID,COST_TYPE_CODE,COST_TYPE_NAME CLASSNAME,nvl(COST_TYPE_CODE_P,0) CLASSPID,
                    LEVEL LEV, CONNECT_BY_ISLEAF ISLEAF
                   FROM {0}.CBHS_COST_TYPE_DICT
                   START WITH COST_TYPE_CODE_P= '0'
                CONNECT BY PRIOR COST_TYPE_CODE = COST_TYPE_CODE_P   ", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 判断下级是否有数据
        /// </summary>list
        /// <param name="id">id</param>
        /// <returns>true:不存在数据 false:存在数据</returns>
        public bool EstimateData(string id)
        {
            OleDbParameter[] pram = new OleDbParameter[] { };
            int num = -1;
            string sql = string.Format("select count(*) from {1}.CBHS_COST_ITEM_DICT where ITEM_TYPE = '{0}'", id, DataUser.CBHS);
            num = Convert.ToInt32(OracleOledbBase.GetSingle(sql, pram));
            return num > 0 ? false : true;
        }

        /// <summary>
        /// 判断是否有下级节点
        /// </summary>list
        /// <param name="id">id</param>
        /// <returns>true:不存在数据 false:存在数据</returns>
        public bool EstimatePData(string id)
        {
            OleDbParameter[] pram = new OleDbParameter[] { };
            int num = -1;
            string sql = string.Format("select count(*) from {1}.CBHS_COST_TYPE_DICT where COST_TYPE_CODE_P = '{0}'", id, DataUser.CBHS);
            num = Convert.ToInt32(OracleOledbBase.GetSingle(sql, pram));
            return num > 0 ? false : true;
        }
        /// <summary>
        /// 获取成本项目列表
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetCostItem(string itemcode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT item_code, item_name, cost_property, input_code,
                               (SELECT prog_name
                                  FROM {0}.CBHS_COST_APPOR_PROG_DICT
                                 WHERE prog_code = ALLOT_FOR_JD) AS ALLOT_FOR_JD,
                               (SELECT prog_name
                                  FROM {0}.CBHS_COST_APPOR_PROG_DICT
                                 WHERE prog_code = ALLOT_FOR_JC) AS ALLOT_FOR_JC,
                               (SELECT prog_name
                                  FROM {0}.CBHS_COST_APPOR_PROG_DICT
                                 WHERE prog_code = ALLOT_FOR_RY) AS ALLOT_FOR_RY, item_type ||':'||item_class as item_type,
                               gettype,
                                (select a.ACCOUNT_TYPE from  cbhs.CBHS_ACCOUNT_TYPE a where b.ACCOUNT_TYPE=to_char(a.ID)) ACCOUNT_TYPE
                                
                          FROM {0}.cbhs_cost_item_dict b where del_flag='0'", DataUser.CBHS);
            if (itemcode != "")
            {
                strSql.AppendFormat(" and b.item_code='{0}' ",itemcode);
            }
            strSql.Append(" order by item_code");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
       
       
        /// <summary>
        /// 添加成本项目所属用户
        /// </summary>
        public void AddCostItemPower(string item_code, string roleid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update  {0}.cbhs_cost_item_dict set item_power='{1}' where item_code='{2}'", DataUser.CBHS, roleid, item_code);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());

        }
        /// <summary>
        /// 人员类别（人力资源）
        /// </summary>
        /// <returns></returns>
        public DataTable GetPersType(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.SERIAL_NO id,a.PERS_SORT_NAME from {0}.PERS_SORT_DICT a", DataUser.RLZY);
            if (id != "")
            {
                strSql.AppendFormat(" and a.SERIAL_NO={0}",int.Parse(id));
            }
            strSql.Append(" order by a.SERIAL_NO");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 添加人员类别权限
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="roleid"></param>
        public void AddPersTypePower(string id, string roleid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update  {0}.PERS_SORT_DICT set role_id='{1}' where SERIAL_NO={2}", DataUser.RLZY, roleid, int.Parse(id));
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());

        }
       
        #endregion
    }
   
}

