using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
namespace Goldnet.Dal
{
	/// <summary>
	/// SYS_DEPT_DICT。
	/// </summary>
	public class SYS_DEPT_DICT
	{
		public SYS_DEPT_DICT()
		{}
		#region  成员方法


        public void adddept(string deptcode, string deptname)
        {
            int id = OracleOledbBase.GetMaxID("id", DataUser.COMM + ".SYS_DEPT_DICT");
            string str = string.Format("insert into {0}.sys_dept_dict (id,dept_code,dept_name,input_code) values (?,?,?,?)",DataUser.COMM);
            OleDbParameter[] parameters = {
					new OleDbParameter("ID", id),
					new OleDbParameter("DEPT_CODE", deptcode),
					new OleDbParameter("DEPT_NAME", deptname),
					new OleDbParameter("INPUT_CODE", pinyin.GetChineseSpell(deptname))
					};

            OracleOledbBase.ExecuteNonQuery(str, parameters);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(GoldNet.Model.SYS_DEPT_DICT model)
		{
            model.ID = OracleOledbBase.GetMaxID("id", DataUser.COMM + ".SYS_DEPT_DICT").ToString();
			StringBuilder strSql=new StringBuilder();
			strSql.AppendFormat("insert into {0}.SYS_DEPT_DICT(",DataUser.COMM);
            strSql.Append("ID,DEPT_CODE,DEPT_NAME,P_DEPT_CODE,DEPT_TYPE,DEPT_LCATTR,DEPT_DATE,SORT_NO,SHOW_FLAG,ATTR,INPUT_CODE,ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME,DEPT_CODE_SECOND,DEPT_NAME_SECOND,P_DEPT_NAME,OUT_OR_IN,DEPT_GROUP)");
			strSql.Append(" values (");
            strSql.Append("?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)");
            OleDbParameter[] parameters = {
					new OleDbParameter("ID", int.Parse(model.ID)),
					new OleDbParameter("DEPT_CODE", model.DEPT_CODE),
					new OleDbParameter("DEPT_NAME", model.DEPT_NAME),
					new OleDbParameter("P_DEPT_CODE", model.P_DEPT_CODE),
					new OleDbParameter("DEPT_TYPE", model.DEPT_TYPE),
					new OleDbParameter("DEPT_LCATTR", model.DEPT_LCATTR),
					new OleDbParameter("DEPT_DATE", System.DateTime.Now.ToString("yyyy-MM-dd")),
					new OleDbParameter("SORT_NO", model.SORT_NO),
					new OleDbParameter("SHOW_FLAG", model.SHOW_FLAG),
					new OleDbParameter("ATTR", model.ATTR),
					new OleDbParameter("INPUT_CODE", model.INPUT_CODE),
					new OleDbParameter("ACCOUNT_DEPT_CODE", model.ACCOUNT_DEPT_CODE),
					new OleDbParameter("ACCOUNT_DEPT_NAME", model.ACCOUNT_DEPT_NAME),
					new OleDbParameter("DEPT_CODE_SECOND", model.DEPT_CODE_SECOND),
					new OleDbParameter("DEPT_NAME_SECOND", model.DEPT_NAME_SECOND),
					new OleDbParameter("P_DEPT_NAME", model.P_DEPT_Name),
                    new OleDbParameter("OUT_OR_IN", model.OUT_OR_IN),
                    new OleDbParameter("OUT_OR_IN", model.DEPT_GROUP)
                                          
                                          };

            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(GoldNet.Model.SYS_DEPT_DICT model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update {0}.SYS_DEPT_DICT set ",DataUser.COMM);
            strSql.Append("DEPT_NAME=?,");
            strSql.Append("P_DEPT_CODE=?,");
            strSql.Append("DEPT_TYPE=?,");
            strSql.Append("DEPT_LCATTR=?,");
            strSql.Append("DEPT_DATE=?,");
            strSql.Append("SORT_NO=?,");
            strSql.Append("SHOW_FLAG=?,");
            strSql.Append("ATTR=?,");
            strSql.Append("INPUT_CODE=?,");
            strSql.Append("ACCOUNT_DEPT_CODE=?,");
            strSql.Append("ACCOUNT_DEPT_NAME=?,");
            strSql.Append("DEPT_CODE_SECOND=?,");
            strSql.Append("DEPT_NAME_SECOND=?,");
            strSql.Append("P_DEPT_NAME=?,");
            strSql.Append("OUT_OR_IN=?,");
            strSql.Append("DEPT_GROUP=?");
            strSql.Append(" where DEPT_CODE=?");
            OleDbParameter[] parameters = {
					new OleDbParameter("DEPT_NAME", model.DEPT_NAME),
					new OleDbParameter("P_DEPT_CODE", model.P_DEPT_CODE),
					new OleDbParameter("DEPT_TYPE", model.DEPT_TYPE),
					new OleDbParameter("DEPT_LCATTR", model.DEPT_LCATTR),
					new OleDbParameter("DEPT_DATE", System.DateTime.Now.ToString("yyyy-MM-dd")),
					new OleDbParameter("SORT_NO", model.SORT_NO),
					new OleDbParameter("SHOW_FLAG", model.SHOW_FLAG),
					new OleDbParameter("ATTR", model.ATTR),
					new OleDbParameter("INPUT_CODE", model.INPUT_CODE),
					new OleDbParameter("ACCOUNT_DEPT_CODE", model.ACCOUNT_DEPT_CODE),
					new OleDbParameter("ACCOUNT_DEPT_NAME", model.ACCOUNT_DEPT_NAME),
					new OleDbParameter("DEPT_CODE_SECOND", model.DEPT_CODE_SECOND),
					new OleDbParameter("DEPT_NAME_SECOND", model.DEPT_NAME_SECOND),
					new OleDbParameter("P_DEPT_NAME", model.P_DEPT_Name),
                    new OleDbParameter("OUT_OR_IN", model.OUT_OR_IN),
                    new OleDbParameter("OUT_OR_IN", model.DEPT_GROUP),
                    new OleDbParameter("DEPT_CODE", model.DEPT_CODE)};

            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
		}
        /// <summary>
        /// 模糊查询科室
        /// </summary>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public DataSet GetDeptByFilter(string deptfilter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT b.ID,b.dept_code, b.dept_name, b.input_code, b.p_dept_code,
       b.dept_type, b.dept_lcattr, b.dept_date, b.sort_no, b.show_flag,
       b.attr, b.account_dept_code, b.account_dept_name, b.dept_code_second,
       b.dept_name_second,b.p_dept_name
  FROM {0}.sys_dept_dict b
  where b.show_flag='0' ", DataUser.COMM);
            if (!deptfilter.Equals(string.Empty))
            {
                str.AppendFormat(" and (b.dept_code like '{0}%' or b.dept_name like '{0}%' or b.input_code like '{0}%')",deptfilter.ToUpper());
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 查询科室
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptSet(string depttype, string showflag,string deptfilter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT b.ID, nvl(a.dept_code,b.dept_code) dept_code, nvl(a.dept_name,b.dept_name) dept_name,a.dept_code his_dept_code,b.dept_code jx_dept_code, 
a.dept_name his_dept_name,b.dept_name jx_dept_name,nvl(a.input_code,b.input_code) input_code, b.p_dept_code,
       b.dept_type, b.dept_lcattr, b.dept_date, b.sort_no, b.show_flag,
       b.attr, b.account_dept_code, b.account_dept_name, b.dept_code_second,
       b.dept_name_second,b.p_dept_name
  FROM {0}.dept_dict a full join {1}.sys_dept_dict b
 on a.dept_code = b.dept_code where 1=1 ", DataUser.HISFACT, DataUser.COMM);
            if (!depttype.Equals(string.Empty))
            {
                str.AppendFormat(" and b.dept_type='{0}'", depttype);
            }
            if (!deptfilter.Equals(string.Empty))
            {
                str.AppendFormat(" and (nvl(a.dept_code,b.dept_code) like '{0}%' or nvl(a.dept_name,b.dept_name) like '{0}%' or nvl(a.input_code,b.input_code) like '{0}%')", deptfilter.ToUpper());
            }
            if (showflag.Equals("1"))
            {
                str.AppendFormat(" and b.show_flag='{0}'", showflag);
            }
            else
            {
                str.AppendFormat(" and (b.show_flag='{0}' or b.show_flag is null)", 0);
            }
            str.Append(" order by b.account_dept_code, b.sort_no, a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 查找人员姓名
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserNameByUserid(string userid)
        {
            string str = string.Format(@"select user_name from {0}.users where user_id='{1}'",DataUser.HISFACT,userid);
            return OracleOledbBase.ExecuteScalar(str).ToString();
        }
        /// <summary>
        /// 查询科室代码
        /// </summary>
        /// <param name="deptname"></param>
        /// <returns></returns>
        public string GetDeptCodeByDeptname(string deptname)
        {
            string str = string.Format("select dept_code from {0}.sys_dept_dict where dept_name='{1}'",DataUser.COMM,deptname);
            return OracleOledbBase.ExecuteScalar(str).ToString();
        }
        public string GetIncomeCodeByIncomename(string incomname)
        {
            string str = string.Format("select ITEM_CLASS from {0}.CBHS_DISTRIBUTION_CALC_SCHM where ITEM_NAME='{1}'", DataUser.CBHS, incomname);
            return OracleOledbBase.ExecuteScalar(str).ToString();
        }
        public string GetdiagnosisCodeByname(string diagnosisname)
        {
            string str = string.Format("select DIAGNOSIS_CODE from hisdata.DIAGNOSIS_DICT where DIAGNOSIS_NAME='{0}'", diagnosisname);
            return OracleOledbBase.ExecuteScalar(str).ToString();
        }
        /// <summary>
        /// 查询科室名称
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetDeptNameByDeptcode(string deptcode)
        {
            string str = string.Format("select dept_name from {0}.sys_dept_dict where dept_code='{1}'", DataUser.COMM, deptcode);
            return OracleOledbBase.ExecuteScalar(str).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetPersonByid(int id)
        {
            string str = string.Format(@"select a.ID, a.user_id, a.dept_code, b.dept_name, c.user_name,
       TO_CHAR (a.st_date, 'yyyy-MM-dd') st_date,
       TO_CHAR (a.end_date, 'yyyy-MM-dd') end_date, a.operator_userid,
       TO_CHAR (a.operate_date, 'yyyy-MM-dd') operate_date from {0}.SYS_PERSONS_CHANGE a,{0}.sys_dept_dict b,{1}.users c where a.DEPT_CODE=b.dept_code and a.user_id=c.user_id and a.id={2}", DataUser.COMM, DataUser.HISFACT, id);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 人组对照是否存在
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool ExitPersons(string userid,string username)
        {
            string str = string.Format("select user_id from {0}.SYS_DEPT_PERSONS where (user_id='{1}' or USER_NAME='{2}')", DataUser.COMM, userid, username);
            DataTable table=OracleOledbBase.ExecuteDataSet(str).Tables[0];
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }
        /// <summary>
        /// 添加人员转科
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="deptcode"></param>
        /// <param name="stdate"></param>
        /// <param name="enddate"></param>
        /// <param name="opuser"></param>
        /// <param name="opdate"></param>
        public void AddPersonschange(string userid, string deptcode, string stdate, string enddate, string opuser, string opdate)
        {
            string id = OracleOledbBase.GetMaxID("id", DataUser.COMM + ".SYS_PERSONS_CHANGE").ToString();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("insert into {0}.SYS_PERSONS_CHANGE(", DataUser.COMM);
            strSql.Append("id,USER_ID,DEPT_CODE,ST_DATE,END_DATE,OPERATOR_USERID,OPERATE_DATE)");
            strSql.Append(" values (");
            strSql.Append("?,?,?,?,?,?,?)");
            OleDbParameter[] parameteradd = {
                                              new OleDbParameter("ID",int.Parse(id)),
											  new OleDbParameter("USER_ID",userid),
											  new OleDbParameter("DEPT_CODE", deptcode),
	                                          new OleDbParameter("ST_DATE", Convert.ToDateTime(stdate)),
                                              new OleDbParameter("END_DATE",  Convert.ToDateTime(enddate)<Convert.ToDateTime("1900-01-01")?DateTime.MaxValue:Convert.ToDateTime(enddate)),
                                              new OleDbParameter("OPERATOR_USERID", opuser),
                                              new OleDbParameter("OPERATE_DATE", Convert.ToDateTime(opdate))
                                            };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameteradd);
        }
        /// <summary>
        /// 修改转科
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="deptcode"></param>
        /// <param name="stdate"></param>
        /// <param name="enddate"></param>
        /// <param name="opuser"></param>
        /// <param name="opdate"></param>
        public void ModifyPersonschange(string id, string deptcode, string stdate, string enddate, string opuser, string opdate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update {0}.SYS_PERSONS_CHANGE set ", DataUser.COMM);
            strSql.Append("DEPT_CODE=?,");
            strSql.Append("ST_DATE=?,");
            strSql.Append("END_DATE=?,");
            strSql.Append("OPERATOR_USERID=?,");
            strSql.Append("OPERATE_DATE=?");
            strSql.Append(" where id=?");
            OleDbParameter[] parameters = {
					new OleDbParameter("DEPT_CODE", deptcode),
					new OleDbParameter("ST_DATE", Convert.ToDateTime(stdate)),
					new OleDbParameter("END_DATE", Convert.ToDateTime(enddate)<Convert.ToDateTime("1900-01-01")?DateTime.MaxValue:Convert.ToDateTime(enddate)),
					new OleDbParameter("OPERATOR_USERID", opuser),
					new OleDbParameter("OPERATE_DATE", Convert.ToDateTime(opdate)),
                    new OleDbParameter("id", id)
					};

            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 保存人组对照
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="userid"></param>
        public void AddPersons(string deptcode, string userid,string name)
        {
            //string id = OracleOledbBase.GetMaxID("id", DataUser.COMM + ".SYS_DEPT_PERSONS").ToString();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("insert into {0}.SYS_DEPT_PERSONS(", DataUser.COMM);
            strSql.Append("DEPT_CODE,USER_ID,USER_NAME)");
            strSql.Append(" values (");
            strSql.Append("?,?,?)");
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("DEPT_CODE",deptcode),
											  new OleDbParameter("USER_ID", userid),
	                                          new OleDbParameter("USER_NAME", name)
                                            };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameteradd);
        }
        /// <summary>
        /// 人组对照
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptPersons(string deptcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT NVL (b.dept_code, a.dept_code) dept_code,a.user_id,m.dept_name,a.user_name,k.dept_name dept_first_name
              FROM {0}.sys_dept_persons a, {0}.sys_persons_change b,{0}.sys_dept_dict m,{1}.DEPT_DOCTORS n,{0}.sys_dept_dict k
             WHERE a.user_id = b.user_id(+) and (b.st_date=(select max(st_date) from {0}.sys_persons_change c where c.user_id=b.user_id) or b.id is null)
             and NVL (b.dept_code, a.dept_code)=m.DEPT_CODE and a.user_id=n.user_id and a.DEPT_CODE=k.dept_code  ", DataUser.COMM, DataUser.HISFACT);
            if (!deptcode.Equals(string.Empty))
            {
                str.AppendFormat(" and NVL (b.dept_code, a.dept_code)='{0}'",deptcode);
            }
            str.AppendFormat(" order by NVL (b.dept_code, a.dept_code)");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 模糊查找人员对照
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataSet GetDeptPersonsByfilter(string filter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT NVL (b.dept_code, a.dept_code) dept_code,a.user_id,m.dept_name,n.user_name,k.dept_name dept_first_name
  FROM {0}.sys_dept_persons a, {0}.sys_persons_change b,{0}.sys_dept_dict m,{1}.DEPT_DOCTORS n,{0}.sys_dept_dict k
 WHERE a.user_id = b.user_id(+) and (b.st_date=(select max(st_date) from {0}.sys_persons_change c where c.user_id=b.user_id) or b.id is null)
 and NVL (b.dept_code, a.dept_code)=m.DEPT_CODE and a.user_id=n.USER_ID and a.DEPT_CODE=k.dept_code  ", DataUser.COMM, DataUser.HISFACT);
            if (!filter.Equals(string.Empty))
            {
                str.AppendFormat(" and (a.user_id like '{0}%' or a.user_name like '{0}%')", filter);
            }
            str.AppendFormat(" order by NVL (b.dept_code, a.dept_code)");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 转科记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet GetPersonsChange(string userid)
        {
            string str = string.Format(@"select a.ID, a.user_id, a.dept_code, b.dept_name, c.user_name,
       TO_CHAR (a.st_date, 'yyyy-MM-dd') st_date,
       TO_CHAR (a.end_date, 'yyyy-MM-dd') end_date, a.operator_userid,
       TO_CHAR (a.operate_date, 'yyyy-MM-dd') operate_date from {0}.SYS_PERSONS_CHANGE a,{0}.sys_dept_dict b,{1}.users c where a.DEPT_CODE=b.dept_code and a.user_id=c.user_id and a.user_id={2} order by a.st_date", DataUser.COMM, DataUser.HISFACT, userid);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 删除转科
        /// </summary>
        /// <param name="id"></param>
        public void DelPersonsChange(int id)
        {
            string str = string.Format("delete {0}.SYS_PERSONS_CHANGE where id={1}", DataUser.COMM, id);
            OracleOledbBase.ExecuteNonQuery(str);
        }
        /// <summary>
        /// 科室类别
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptType()
        {
            string str = string.Format("select * from {0}.SYS_DEPT_ATTR_DICT order by SORTNO", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        //组
        public DataSet GetDeptgroupType()
        {
            string str = string.Format("select * from {0}.SYS_DEPT_GROUP", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        public DataSet GetXYHSDeptType()
        {
            string str = string.Format("select * from {0}.XYHS_DEPT_TYPE_DICT WHERE ID!=0 order by ID", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 临床属性
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptLcattr()
        {
            string str = string.Format("select * from {0}.SYS_LCDEPT_ATTR_DICT", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 科室代码是否存在
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHisDeptcodeExit(string deptcode)
        {
            string str = string.Format("SELECT nvl(a.dept_name,b.dept_name) DEPT_NAME FROM {0}.DEPT_DICT a full join {1}.sys_dept_dict b on a.dept_code=b.dept_code WHERE nvl(a.DEPT_CODE,b.dept_code) =?", DataUser.HISFACT,DataUser.COMM);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", deptcode) };
            return OracleOledbBase.ExecuteScalar(str, cmdPara).ToString();
        }
        /// <summary>
        /// 科室名称是否存在
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHisDeptnameExit(string deptname)
        {
            string str = string.Format("SELECT nvl(a.dept_name,b.dept_name) DEPT_NAME FROM {0}.DEPT_DICT a full join {1}.sys_dept_dict b on a.dept_code=b.dept_code WHERE nvl(a.DEPT_NAME,b.dept_name) =?", DataUser.HISFACT, DataUser.COMM);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", deptname) };
            return OracleOledbBase.ExecuteScalar(str, cmdPara).ToString();
        }
        /// <summary>
        /// 核算科室
        /// </summary>
        /// <param name="inputcode">输入码</param>
        /// <returns></returns>
        public DataSet GetAccountDept(string inputcode, string deptfilter,string deptcode)
        {
            StringBuilder strSql = new StringBuilder();
            if (deptcode != "")
            {
                strSql.AppendFormat("select a.dept_code, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a where a.attr='是' and a.SHOW_FLAG='0' and ACCOUNT_DEPT_CODE  is not  null AND a.dept_code = '{1}' ", DataUser.COMM, deptcode);
                strSql.Append(" union all ");
                strSql.AppendFormat("select a.dept_code, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a where a.attr='是' and a.SHOW_FLAG='0' and ACCOUNT_DEPT_CODE  is not  null and  a.input_code like ? and a.dept_code!='{1}'", DataUser.COMM, deptcode);
                if (!deptfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", deptfilter);
                }
                
            }
            else
            {
                strSql.AppendFormat("select a.dept_code, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a where a.attr='是' and a.SHOW_FLAG='0' and ACCOUNT_DEPT_CODE  is not  null and  a.input_code like ? ", DataUser.COMM);
                if (!deptfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", deptfilter);
                }
                strSql.Append(" order by a.SORT_NO");
            }
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(),cmdPara);
        }
        /// <summary>
        /// 所有科室
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
                strSql.AppendFormat("select a.dept_code DEPT_CODE,a.dept_name DEPT_NAME,a.input_code from {0}.SYS_DEPT_DICT a where  a.dept_code = '{1}' and a.SHOW_FLAG='0' ", DataUser.COMM, deptcode);
                strSql.Append(" union all ");
                strSql.AppendFormat("select a.dept_code DEPT_CODE,a.dept_name DEPT_NAME,a.input_code from {0}.SYS_DEPT_DICT a where   a.input_code like ? and a.dept_code!='{1}' and a.SHOW_FLAG='0'", DataUser.COMM, deptcode);
                if (!deptfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", deptfilter);
                }

            }
            else
            {
                strSql.AppendFormat("select a.dept_code DEPT_CODE,a.dept_name DEPT_NAME,a.input_code from {0}.SYS_DEPT_DICT a where   a.SHOW_FLAG='0' and a.input_code like ? ", DataUser.COMM);
                if (!deptfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", deptfilter);
                }
                strSql.Append(" order by a.SORT_NO");
            }
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(), cmdPara);
        }
        public DataSet GetAccountDept(string inputcode)
        {
            string strSql = string.Format("select a.dept_code, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a where a.attr='是' and a.SHOW_FLAG='0' and ACCOUNT_DEPT_CODE  is not  null and  a.input_code like ? order by a.SORT_NO", DataUser.COMM);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql,cmdPara);
        }

        /// <summary>
        /// 所有类别
        /// </summary>
        /// <param name="inputcode"></param>
        /// <param name="deptfilter"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataSet GetAllLeibie(string inputcode,string deptfilter, string itemcode)
        {
            StringBuilder strSql = new StringBuilder();
            if (itemcode != "")
            {
                strSql.AppendFormat("SELECT ITEM_CODE,ITEM_NAME FROM {0}.LEIBIE_HZ where  item_code = '{1}' group by item_code,item_name   ", DataUser.HISDATA, itemcode);
                strSql.Append(" union all ");
                strSql.AppendFormat("SELECT ITEM_CODE,ITEM_NAME FROM {0}.LEIBIE_HZ where  input_code like ? and item_code!='{1}' group by item_code,item_name ", DataUser.HISDATA, itemcode);
                if (!deptfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", deptfilter);
                }

            }
            else
            {
                strSql.AppendFormat("SELECT ITEM_CODE,ITEM_NAME,input_code FROM {0}.LEIBIE_HZ  where input_code like ?  group by item_code,item_name,input_code", DataUser.HISDATA);
                //strSql.Append(" union all ");
                //strSql.AppendFormat("SELECT ITEM_CODE,ITEM_NAME FROM {0}.LEIBIE_TEST where   like '%{1}%' and item_name not like '%{1}%' group by item_code,item_name ", DataUser.HISDATA, itemname);
                if (!deptfilter.Equals(""))
                {
                    strSql.AppendFormat(" and {0}", deptfilter);
                }
                strSql.Append(" order by item_code");
            }
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", "%" + inputcode + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql.ToString(), cmdPara);
        }
        /// <summary>
        /// 根据科室列表查询科室
        /// </summary>
        /// <param name="depttype"></param>
        /// <returns></returns>
        public DataSet GetDeptBytype(string depttype)
        {
            string strSql = string.Format("select a.dept_code, a.dept_name from {0}.SYS_DEPT_DICT a where a.dept_type='{1}' and a.SHOW_FLAG='0' order by a.SORT_NO", DataUser.COMM, depttype);
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        /// <summary>
        /// 专科中心科室
        /// </summary>
        /// <param name="depttype"></param>
        /// <returns></returns>
        public DataSet GetDeptBytypeCenter(string depttype,string centerid)
        {
            string strSql = string.Format("select a.dept_code, a.dept_name from {0}.SYS_DEPT_DICT a where a.dept_type='{1}' and a.SHOW_FLAG='0' and a.dept_code not in (select dept_code from {0}.SYS_CENTER_DETAIL where to_char(CENTER_ID)='{2}') order by a.SORT_NO", DataUser.COMM, depttype,centerid);
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        /// <summary>
        /// 根据科室列表查询科室
        /// </summary>
        /// <param name="depttype"></param>
        /// <returns></returns>
        public DataSet GetSpeDeptBytype(string depttype, string tempid)
        {
            StringBuilder strSql = new StringBuilder();
            string deptlist = "";
            //if (GetConfig.Score.Equals("0"))
            {
                if (tempid.Equals(string.Empty))
                {
                    deptlist = "'-1'";
                }
                else
                {
                    string dept = OracleOledbBase.ExecuteScalar(string.Format("select CHECKDEPT from {0}.G_SPECIALTYGUIDE where TEMPLETID={1}", DataUser.ZLGL, tempid)).ToString();
                    if (dept != string.Empty)
                    {
                        
                       // deptlist = dept.Substring(0, dept.Length - 1);
                        string[] dept_list = dept.Split(',');

                        for (int i = 0; i < dept_list.Length; i++)
                        {
                            if (!dept_list[i].ToString().Equals(string.Empty))
                                deptlist = deptlist + "'" + dept_list[i].ToString() + "',";
                        }
                        deptlist = deptlist.Substring(0, deptlist.Length - 1);
                    }
                    else
                    {
                        deptlist = "'-1'";
                    }
                }
            }
            //else
            //{
            //    DataTable table = OracleOledbBase.ExecuteDataSet(string.Format("select CHECKDEPT from {0}.G_SPECIALTYGUIDE ", DataUser.ZLGL)).Tables[0];
            //    StringBuilder deptcodes = new StringBuilder();
            //    for (int i = 0; i < table.Rows.Count; i++)
            //    {
            //        if (table.Rows[i]["CHECKDEPT"].ToString().Trim() != "")
            //        {
            //            deptcodes.Append(table.Rows[i]["CHECKDEPT"].ToString());
            //        }
            //    }
            //    if (deptcodes.ToString() != "")
            //    {
            //        //deptlist = deptcodes.ToString().Substring(0, deptcodes.ToString().Length - 1);

            //        string[] dept_list = deptcodes.ToString().Split(',');

            //        for (int i = 0; i < dept_list.Length; i++)
            //        {
            //            if (!dept_list[i].ToString().Equals(string.Empty))
            //                deptlist = deptlist + "'" + dept_list[i].ToString() + "',";
            //        }
            //        deptlist = deptlist.Substring(0, deptlist.Length - 1);
            //    }
            //    else
            //    {
            //        deptlist = "'-1'";
            //    }
            //}
            strSql.AppendFormat("select a.dept_code, a.dept_name from {0}.SYS_DEPT_DICT a where a.dept_type='{1}' and a.SHOW_FLAG='0' and a.dept_code not in ({2}) order by a.SORT_NO", DataUser.COMM, depttype, deptlist);

            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 专科中心科室
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public DataSet GetDeptByCenter(string centerid)
        {
            string strSql = string.Format("select a.dept_code, a.dept_name from {0}.SYS_DEPT_DICT a,{0}.SYS_CENTER_DETAIL b where a.dept_code=b.dept_code and b.center_id='{1}' order by a.dept_code", DataUser.COMM, centerid);
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        /// <summary>
        /// 保存专科中心的科室
        /// </summary>
        /// <param name="deptlist"></param>
        /// <param name="centerid"></param>
        public void SaveCenterDept(List<GoldNet.Model.PageModels.deptselected> deptlist, int centerid)
        {
            MyLists listtable = new MyLists();
            string strdel = string.Format("delete {0}.SYS_CENTER_DETAIL where center_id=?", DataUser.COMM);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", centerid) };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.SYS_CENTER_DETAIL(", DataUser.COMM);
                strSql.Append("CENTER_ID,DEPT_CODE)");
                strSql.Append(" values (");
                strSql.Append("?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("CENTER_ID",centerid),
											  new OleDbParameter("DEPT_CODE", dept.DEPT_CODE)	  
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 专科中心
        /// </summary>
        /// <returns></returns>
        public DataSet GetCenterList(int centerid)
        {
            string strSql="";
            if (centerid != 0)
            {
                strSql = string.Format("select * from {0}.SYS_DEPT_CENTER where id={1} ORDER BY ID", DataUser.COMM,centerid);
            }
            else
            {
                strSql = string.Format("select * from {0}.SYS_DEPT_CENTER ORDER BY ID", DataUser.COMM);
            }
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        /// <summary>
        /// 添加专科中心
        /// </summary>
        /// <param name="centername"></param>
        /// <param name="director"></param>
        public void EditCenter(int centerid,string centername, string directorid,string directorname,string roleid)
        {
            StringBuilder strSql = new StringBuilder();
            if (centerid != 0)
            {
                strSql.AppendFormat("update {0}.SYS_DEPT_CENTER set ", DataUser.COMM);
                strSql.Append("CENTER_NAME=?,");
                strSql.Append("CENTER_DIRECTOR_ID=?,");
                strSql.Append("CENTER_DIRECTOR_NAME=?,");
                strSql.Append("ROLE_ID=?");
                strSql.Append(" where ID=?");
                OleDbParameter[] parameters = {
											  new OleDbParameter("CENTER_NAME", centername),
											  new OleDbParameter("CENTER_DIRECTOR_ID", directorid),
											  new OleDbParameter("CENTER_DIRECTOR_NAME", directorname),
                                              new OleDbParameter("ROLE_ID", roleid),
                                              new OleDbParameter("ID",centerid)
										  };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
            }
            else
            {
                centerid = OracleOledbBase.GetMaxID("ID", DataUser.COMM + ".SYS_DEPT_CENTER");
                strSql.AppendFormat("insert into {0}.SYS_DEPT_CENTER(", DataUser.COMM);
                strSql.Append("ID,CENTER_NAME,CENTER_DIRECTOR_ID,CENTER_DIRECTOR_NAME,ROLE_ID)");
                strSql.Append(" values (");
                strSql.Append("?,?,?,?,?)");
                OleDbParameter[] parameters = {
											  new OleDbParameter("ID",centerid),
											  new OleDbParameter("CENTER_NAME", centername),
											  new OleDbParameter("CENTER_DIRECTOR_ID", directorid),
											  new OleDbParameter("CENTER_DIRECTOR_NAME", directorname),
                                               new OleDbParameter("ROLE_ID", roleid)
										  };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
            }
        }
        /// <summary>
        /// 删除科室
        /// </summary>
        /// <param name="id"></param>
        public void DelDept(string deptcode)
        {
            string str = string.Format("delete {0}.sys_dept_dict where dept_code='{1}'",DataUser.COMM,deptcode);
            OracleOledbBase.ExecuteNonQuery(str);
        }
        /// <summary>
        /// 删除人组对照
        /// </summary>
        /// <param name="userid"></param>
        public void deldeptpersons(string userid)
        {
            MyLists listtable = new MyLists();
            string deldeptperson = string.Format("delete {0}.SYS_DEPT_PERSONS where user_id=?", DataUser.COMM);
            List listdeptperson = new List();
            listdeptperson.StrSql = deldeptperson;
            listdeptperson.Parameters = new OleDbParameter[] { new OleDbParameter("", userid) };
            listtable.Add(listdeptperson);

            string delpersonchange = string.Format("DELETE {0}.SYS_PERSONS_CHANGE WHERE user_id=?", DataUser.COMM);
            List listpersonchange = new List();
            listpersonchange.StrSql = delpersonchange;
            listpersonchange.Parameters = new OleDbParameter[] { new OleDbParameter("", userid) };
            listtable.Add(listpersonchange);
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 删除专科中心
        /// </summary>
        /// <param name="id"></param>
        public void DelCenter(int id)
        {
            MyLists listtable = new MyLists();
            //删除专科中心
            string delcenterdict = string.Format("delete {0}.SYS_DEPT_CENTER where id=?", DataUser.COMM, id);
            List listcenterdict = new List();
            listcenterdict.StrSql = delcenterdict;
            listcenterdict.Parameters = new OleDbParameter[] { new OleDbParameter("", id) };
            listtable.Add(listcenterdict);
            //删除专科中心对应的科室
            string delcenterdetail = string.Format("DELETE {0}.SYS_CENTER_DETAIL WHERE CENTER_ID=?", DataUser.COMM);
            List listcenterdetail = new List();
            listcenterdetail.StrSql = delcenterdetail;
            listcenterdetail.Parameters = new OleDbParameter[] { new OleDbParameter("", id) };
            listtable.Add(listcenterdetail);
            //删除专科中心的角色
            //string delrole = string.Format("DELETE {0}.SYS_CENTER_ROLE WHERE CENTER_ID=?", DataUser.COMM);
            //List listdelrole = new List();
            //listdelrole.StrSql = delrole;
            //listdelrole.Parameters = new OleDbParameter[] { new OleDbParameter("",id) };
            //listtable.Add(listdelrole);
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 提取科室
        /// </summary>
        /// <param name="deptcodes"></param>
        /// <returns></returns>
        public DataTable GetDeptByDeptCodes(string deptcodes)
        {
            string str = string.Format("select dept_code,dept_name from {0}.SYS_DEPT_DICT where SHOW_FLAG='0' and  dept_code in ({1}) order by SORT_NO", DataUser.COMM, deptcodes);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }
        /// <summary>
        /// 所有核算科室
        /// </summary>
        /// <param name="inputcode"></param>
        /// <returns></returns>
        public DataSet GetAAccountDept(string deptcode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.dept_code, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a where a.ATTR ='是' and a.SHOW_FLAG='0' and ACCOUNT_DEPT_CODE  is not  null ", DataUser.COMM);
            if (deptcode != "")
            {
                strSql.AppendFormat(" and a.dept_code ='{0}' ",deptcode);
            }
            strSql.Append(" order by a.SORT_NO");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 过滤科室
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public DataSet GetAAccountDeptfilter(string deptfilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.dept_code, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a where a.ATTR ='是' and a.SHOW_FLAG='0' and ACCOUNT_DEPT_CODE  is not  null ", DataUser.COMM);
            if (deptfilter != "")
            {
                strSql.AppendFormat(" and {0} ", deptfilter);
            }
            strSql.Append(" order by a.SORT_NO");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 返回指定核算科室的科室
        /// </summary>
        /// <param name="accountdeptcode"></param>
        /// <returns></returns>
        public string  GetDeptbyaccountdept(string accountdeptcode)
        {
            string str = string.Format("select dept_code,dept_name from {0}.SYS_DEPT_DICT where ACCOUNT_DEPT_CODE = '{1}' and SHOW_FLAG='0' order by SORT_NO", DataUser.COMM, accountdeptcode);
            DataTable dept = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            if (dept.Rows.Count > 0)
            {
                accountdeptcode = "(";
                for (int i = 0; i < dept.Rows.Count; i++)
                {
                    accountdeptcode += "'" + dept.Rows[i]["dept_code"].ToString() + "',";
                }
                accountdeptcode = accountdeptcode.Substring(0, accountdeptcode.Length - 1);
                accountdeptcode += ")";
            }
            return accountdeptcode;
        }
        /// <summary>
        /// 科室查询
        /// </summary>
        /// <param name="accountdeptcode"></param>
        /// <returns></returns>
        public DataTable  GetDeptbydept(string accountdeptcode)
        {
            string str = string.Format("select dept_code,dept_name from {0}.SYS_DEPT_DICT where SHOW_FLAG='0' and ACCOUNT_DEPT_CODE = '{1}' order by SORT_NO", DataUser.COMM, accountdeptcode);
            return  OracleOledbBase.ExecuteDataSet(str).Tables[0];
            
        }

        //数据提取日志
        public DataTable Get_WORK_LOG(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select to_char(DATE_TIME,'yyyyMMdd') BEGIN_DATE_TIME,TASK_NAME from {0}.work_err_log where DATE_TIME>=date'{1}' and DATE_TIME <add_months(date'{1}',1) order by DATE_TIME desc", DataUser.HISDATA, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable Get_EXEC_LOG(string date)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.task_name,a.table_name,a.error_message,a.DATE_TIME from {0}.work_err_log a where DATE_TIME>=date'{1}' and DATE_TIME <add_months(date'{1}',1)
                                order by DATE_TIME desc", DataUser.HISDATA, date);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        public DataSet GetXYHSDeptType2()
        {
            string str = string.Format("select ID,XYHS_DEPT_TYPE as ATTRIBUE,id as sortno from CBHS.XYHS_DEPT_TYPE_DICT order by id", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        public DataSet Getleibie_dict()
        {
            string str = string.Format("SELECT '住院人数' item_name  FROM DUAL UNION ALL SELECT '挂号费' FROM DUAL UNION ALL SELECT '实际占床日' FROM DUAL");
            return OracleOledbBase.ExecuteDataSet(str);
        }
        public DataSet Getyiji_dict(string user_id)
        {
            StringBuilder strDel = new StringBuilder();
            string num1 = strDel.AppendFormat("select a.role_id from comm.SYS_ROLE_DICT a where a.role_id=(select min(b.power_id) from comm.SYS_POWER_DETAIL b where b.target_id=upper('{0}'))", user_id).ToString();
            string num = OracleOledbBase.ExecuteScalar(strDel.ToString()).ToString();
            if (num.Equals("1"))
            {
                string str = string.Format(@"SELECT DISTINCT DEPT_CODE, DEPT_NAME FROM HISDATA.JIEJIRI_OUP");
                return OracleOledbBase.ExecuteDataSet(str);
            }
            else
            {
                string str = string.Format("SELECT DISTINCT DEPT_CODE, DEPT_NAME FROM HISDATA.JIEJIRI_OUP where dept_code=(select user_dept from hisdata.users where db_user='{0}')", user_id);
                return OracleOledbBase.ExecuteDataSet(str);
            }

        }
        public DataSet GetHL_ZYJS()
        {
            string str = string.Format(@"SELECT DEPT_CODE, DEPT_NAME FROM COMM.SYS_DEPT_DICT WHERE DEPT_CODE LIKE '99%' ORDER BY DEPT_CODE");
             return OracleOledbBase.ExecuteDataSet(str);
           
        }
		#endregion  成员方法

        /// <summary>
        /// 通过收入编码查询
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptTypeleibie()
        {
            string str = string.Format("select item_class,item_name from {0}.leibie_dict  order by item_class ", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        public DataSet GetAccoutDeptCode111(string user_id)
        {
            string str = string.Format(@"SELECT ACCOUNT_DEPT_CODE DEPT_CODE, ACCOUNT_DEPT_NAME DEPT_NAME
  FROM COMM.SYS_DEPT_DICT
 WHERE DEPT_CODE IN (SELECT DISTINCT DEPT_CODE FROM HISDATA.WULIU_KC)
 AND ATTR = '是'
 order by account_dept_code");

            return OracleOledbBase.ExecuteDataSet(str);  
          
        }
        
       
	}
}

