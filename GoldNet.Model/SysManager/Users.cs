using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;

namespace GoldNet.Model
{
    [Serializable()]
    public class User
    {
        #region 对象
        /// <summary>
        /// his用户
        /// </summary>
        [Serializable()]
        public class UserInfo
        {
            public string UserName;//用户姓名
            public string UserId;//用户id
            public string DbUser;//登录名称
            public string StrDate;//时间字符串（如：2010-01-01）

            public UserInfo(string db_user, string userid, string username, string date)
            {
                DbUser = db_user;
                UserName = username;
                UserId = userid;
                StrDate = date;
            }
        }
        #endregion

        //私有变量
        private UserInfo _userinfo;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userid"></param>
        public User(string userid)
        {
            _userinfo = GetUserById(userid);
        }

        /// <summary>
        /// his用户登陆验证
        /// </summary>
        /// <param name="dbuser"></param>
        /// <param name="password"></param>
        public User(string dbuser, string password)
        {
            if (dbuser.Equals(string.Empty))
            {
                throw new LoginManagerException("系统提示", "用户和密码不能为空！");
            }
            string TestString = GetConfig.TestString;

            string userId = "";
            string sql = string.Format("SELECT USER_ID FROM {0}.USERS WHERE ACCOUNT = ?", DataUser.HISFACT);
            DataSet dr = OracleOledbBase.ExecuteDataSet(sql, new OleDbParameter("", dbuser));
            if (dr.Tables[0].Rows.Count != 0)
            {
                userId = dr.Tables[0].Rows[0][0].ToString();
            }
            if (userId.Equals(string.Empty) && dbuser.ToLower() != "admin")
            {
                throw new LoginManagerException("系统提示", "用户不存在，请重新输入！");
            }
            if (!userId.Equals(string.Empty))
            {
                _userinfo = GetUserById(userId);
            }
            else
            {
                _userinfo = new UserInfo("ADMIN", "ADMIN", "系统管理员", DateTime.Now.ToString("yyyy-MM-dd"));
                return;
            }
            if (!TestString.Equals("god"))
            {
                if (GetConfig.GetConfigString("HIS").Equals("TJ"))
                {
                    try
                    {
                        if (!getpass(userId, password))
                        {
                            throw new LoginPWErrorException();
                        }
                    }
                    catch
                    {
                        throw new LoginPWErrorException();
                    }
                }
                else
                {
                    string CONN_SERVER = GetConfig.ORACLEServer;
                    try
                    {
                        string sqlps = "Provider='MSDAORA.1';User ID=" + dbuser + ";Data Source=" + CONN_SERVER + ";Password=" + password;
                        OleDbConnection conn = new OleDbConnection(sqlps);
                        conn.Open();
                        conn.Close();
                        conn.Dispose();
                    }
                    catch
                    {
                        throw new LoginPWErrorException();
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        bool getpass(string userId, string passWord)
        {
            StringBuilder strpass = new StringBuilder();
            strpass.AppendFormat("select CHR (ASCII (SUBSTR ('{0}', '{1}', 1)) + '{2}') ", passWord, 1, 7);

            for (int i = 2; i <= passWord.Length; i++)
            {
                if (i % 2 == 1)
                {
                    strpass.AppendFormat(" || CHR (ASCII (SUBSTR ('{0}', '{1}', 1)) + '{2}') ", passWord, i, 8 - i);
                }
                else
                {
                    strpass.AppendFormat(" || CHR (ASCII (SUBSTR ('{0}', '{1}', 1)) + '{2}')", passWord, i, -32 + i);
                }
            }
            strpass.Append(" from dual");

            string passw = OracleOledbBase.ExecuteScalar(strpass.ToString()).ToString();

            string str = string.Format("select password from {0}.users where USER_ID='{1}'", DataUser.HISFACT, userId);
            string passwhis = OracleOledbBase.ExecuteScalar(str.ToString()).ToString();

            if (passw == passwhis)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region 属性
        /// <summary>
        /// 人员id(his的user_id)
        /// </summary>
        public string UserId
        {
            get { return _userinfo.UserId; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string DbUser
        {
            get { return _userinfo.DbUser; }
        }
        /// <summary>
        /// 人力资源里的id(返回空表示在人力资源表里没找到这个人)
        /// </summary>
        public string StaffId
        {
            get { return GetStaffid(_userinfo.UserId); }
        }
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string UserName
        {
            get { return _userinfo.UserName; }
        }
        /// <summary>
        /// 时间(查询时间有变化，先设置用户时间)
        /// </summary>
        public string StrDate
        {
            get { return _userinfo.StrDate; }
            set { _userinfo.StrDate = value; }
        }
        /// <summary>
        /// 用户在his的科室代码
        /// </summary>
        public string HisDeptCode
        {
            get { return GetHisDeptCodeByUserid(_userinfo.UserId); }
        }
        /// <summary>
        /// 用户在his的科室名称
        /// </summary>
        public string HisDeptName
        {
            get { return GetHisDeptNameByUserid(_userinfo.UserId); }
        }
        /// <summary>
        /// 用户所在三级科室或核算科室代码
        /// </summary>
        public string AccountDeptCode
        {
            get { return GetAccountDeptCodeByUserid(_userinfo.UserId); }
        }
        /// <summary>
        /// 用户所在三级科室或核算科室名称
        /// </summary>
        public string AccountDeptName
        {
            get { return GetAccountDeptNameByUserid(_userinfo.UserId); }
        }
        /// <summary>
        /// 用户所在二级科室代码
        /// </summary>
        public string SecondDeptCode
        {
            get { return GetSecondDeptCodeByUserid(_userinfo.UserId); }
        }
        /// <summary>
        /// 用户所在二级科室名称
        /// </summary>
        public string SecondDeptName
        {
            get { return GetSecondDeptNameByUserid(_userinfo.UserId); }
        }
        /// <summary>
        ///科室属性
        /// </summary>
        public string DeptType
        {
            get { return GetDeptType(_userinfo.UserId); }
        }
        /// <summary>
        /// 人员菜单权限
        /// </summary>
        public DataTable GetUserPower
        {
            get { return PowerDetail(_userinfo.UserId, _userinfo.DbUser); }
        }


        #endregion
        #region 公共方法


        /// <summary>
        /// 得到岗位代码
        /// </summary>
        /// <param name="staffid"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public string GetStationCode(string staffid, string years)
        {
            string value = "";
            if (!staffid.Equals(string.Empty))
            {
                string str = string.Format("select STATION_CODE from {0}.SYS_STATION_PERSONNEL_INFO where STATION_YEAR={1} and PERSON_ID='{2}'", DataUser.COMM, years, staffid);
                object obj = OracleOledbBase.ExecuteScalar(str);
                if (obj != null)
                {
                    value = obj.ToString();
                }
            }
            return value;
        }

        /// <summary>
        /// 人的院科人权限
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="modid"></param>
        /// <returns></returns>
        public string GetUserOrg(string menuid, string modid)
        {
            string str = string.Format("select a.ROLE_TYPE from {0}.SYS_ROLE_DICT a,{0}.SYS_POWER_DETAIL b,{0}.SYS_ROLE_FUNCTION c,{0}.SYS_APPLICATION_MENU d where a.ROLE_ID=b.POWER_ID and c.role_id=b.power_id and c.FUNCTION_ID=d.FUNCTION_ID and d.MENUID='{1}' and d.MODID='{2}' and b.TARGET_ID='{3}' order by a.role_type", DataUser.COMM, menuid, modid, _userinfo.UserId);
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            if (table.Rows.Count > 0)
            {
                return table.Rows[0][0].ToString();
            }
            else
                return "";
        }

        /// <summary>
        /// 人的院科人权限
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="modid"></param>
        /// <returns></returns>
        public string GetUserOrg(string menuid, string modid, string userid)
        {
            string str = string.Format("select a.ROLE_TYPE from {0}.SYS_ROLE_DICT a,{0}.SYS_POWER_DETAIL b,{0}.SYS_ROLE_FUNCTION c,{0}.SYS_APPLICATION_MENU d where a.ROLE_ID=b.POWER_ID and c.role_id=b.power_id and c.FUNCTION_ID=d.FUNCTION_ID and d.MENUID='{1}' and d.MODID='{2}' and b.TARGET_ID='{3}' order by a.role_type", DataUser.COMM, menuid, modid, userid.ToLower());
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            if (table.Rows.Count > 0)
            {
                return table.Rows[0][0].ToString();
            }
            else
                return "";
        }

        /// <summary>
        /// 设置某按钮(控件)的可用状态
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public bool SetControlEdit(string menuid, string modid)
        {
            string str = "";
            if (modid == Constant.ZLGL_FUN_TYPE)
            {
                str = string.Format(@"SELECT b.edit FROM {0}.sys_power_detail a, {0}.sys_role_function b,{1}.T_TEMPLETDICT c
 WHERE a.power_id = b.role_id AND upper(a.target_id) = '{2}'
 and B.FUNCTION_ID=C.FUNCKEY and   c.id='{3}' order by edit desc", DataUser.COMM, DataUser.ZLGL, _userinfo.UserId.ToUpper(), int.Parse(menuid).ToString());

            }
            //
            else
            {
                str = string.Format(@"SELECT b.edit FROM {0}.sys_power_detail a, {0}.sys_role_function b,{0}.sys_application_menu c
 WHERE a.power_id = b.role_id AND upper(a.target_id) = '{1}'
 and B.FUNCTION_ID=C.FUNCTION_ID and c.MENUID='{2}' and c.modid='{3}' order by edit desc ", DataUser.COMM, _userinfo.UserId.ToUpper(), menuid, modid);
            }
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            if (table.Rows.Count > 0)
            {
                if (table.Rows[0][0].ToString().Equals("1"))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 审核或上报
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="modid"></param>
        /// <returns></returns>
        public bool SetControlPass(string menuid, string modid)
        {
            string str = "";
            if (modid == Constant.ZLGL_FUN_TYPE)
            {
                str = string.Format(@"SELECT b.pass FROM {0}.sys_power_detail a, {0}.sys_role_function b,{1}.T_TEMPLETDICT c
 WHERE a.power_id = b.role_id AND upper(a.target_id) = '{2}'
 and B.FUNCTION_ID=C.FUNCKEY and   c.id='{3}' order by pass desc", DataUser.COMM, DataUser.ZLGL, _userinfo.UserId.ToUpper(), int.Parse(menuid).ToString());

            }
            else
            {
                str = string.Format(@"SELECT b.pass FROM {0}.sys_power_detail a, {0}.sys_role_function b,{0}.sys_application_menu c
 WHERE a.power_id = b.role_id AND upper(a.target_id) = '{1}'
 and B.FUNCTION_ID=C.FUNCTION_ID and c.MENUID='{2}' and c.modid='{3}' order by pass desc ", DataUser.COMM, _userinfo.UserId.ToUpper(), menuid, modid);
            }
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            if (table.Rows.Count > 0)
            {
                if (table.Rows[0][0].ToString().Equals("1"))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 个人是否有权限
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="modid"></param>
        /// <returns></returns>
        public bool GetPersonsPower(string menuid, string modid)
        {
            string str = "";
            if (modid == Constant.ZLGL_FUN_TYPE)
            {
                str = string.Format(@"SELECT b.edit FROM {0}.sys_power_detail a, {0}.sys_role_function b,{1}.T_TEMPLETDICT c
 WHERE a.power_id = b.role_id AND upper(a.target_id) = '{2}'
 and B.FUNCTION_ID=C.FUNCKEY and   c.id='{3}' order by edit desc", DataUser.COMM, DataUser.ZLGL, _userinfo.UserId.ToUpper(), int.Parse(menuid).ToString());

            }
            //
            else
            {
                str = string.Format(@"SELECT b.edit FROM {0}.sys_power_detail a, {0}.sys_role_function b,{0}.sys_application_menu c
 WHERE a.power_id = b.role_id AND upper(a.target_id) = '{1}'
 and B.FUNCTION_ID=C.FUNCTION_ID and c.MENUID='{2}' and c.modid='{3}' order by edit desc ", DataUser.COMM, _userinfo.UserId.ToUpper(), menuid, modid);
            }
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            if (table.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 角色过滤
        /// </summary>
        /// <returns></returns>
        public string GetRoleFilter(string appfilter)
        {
            string rolefilter = appfilter + " in (";
            DataTable table = GetUserApp();
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    rolefilter += "'" + table.Rows[i]["app_id"].ToString() + "',";
                }
            }
            else
            {
                rolefilter += "'-1',";
            }
            rolefilter = rolefilter.Substring(0, rolefilter.Length - 1) + ")";
            return rolefilter;
        }

        /// <summary>
        /// 单独授权项目
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserApp()
        {
            string str = string.Format(@"SELECT a.app_id,a.app_name
  FROM {0}.sys_application_subsys a,
       {0}.sys_role_dict b,
       {0}.sys_power_detail c
 WHERE a.role_id = b.role_id AND a.role_id = c.power_id
 and upper(C.TARGET_ID)='{1}'", DataUser.COMM, _userinfo.UserId.ToUpper());
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        private class SelectDept
        {
            public string DEPT_CODE { get; set; }
        }

        /// <summary>
        /// 院，科，专科中心权限
        /// </summary>
        /// <param name="strdept"></param>
        /// <returns></returns>
        public string GetUserDeptFilter(string strfilter, string menuid, string modid)
        {
            string flags = "0";
            string deptfilter = string.Format("{0} in (", strfilter);
            if (strfilter.Trim() == "")
            {
                deptfilter = "";
            }
            List<SelectDept> deptlist = new List<SelectDept>();
            string str = "";
            int max = OracleOledbBase.GetMaxID("id", string.Format("{0}.t_templetdict", DataUser.ZLGL));
            if (modid == Constant.ZLGL_FUN_TYPE && int.Parse(menuid) <= max)
            {
                str = string.Format("select distinct B.ROLE_TYPE from {0}.SYS_POWER_DETAIL a,{0}.SYS_ROLE_DICT b,{0}.sys_role_function c,{3}.T_TEMPLETDICT d where A.POWER_ID=B.ROLE_ID and upper(a.target_id)='{1}' and  c.function_id = d.funckey and C.ROLE_ID=B.ROLE_ID AND d.id = '{2}'", DataUser.COMM, _userinfo.UserId.ToUpper(), int.Parse(menuid).ToString(), DataUser.ZLGL);
            }
            else
            {
                str = string.Format("select distinct B.ROLE_TYPE from {0}.SYS_POWER_DETAIL a,{0}.SYS_ROLE_DICT b,{0}.sys_role_function c,{0}.sys_application_menu d where A.POWER_ID=B.ROLE_ID and upper(a.target_id)='{1}' and  c.function_id = d.function_id and C.ROLE_ID=B.ROLE_ID AND d.menuid = '{2}' AND d.modid = '{3}'", DataUser.COMM, _userinfo.UserId.ToUpper(), menuid, modid);
            }
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];

            //获取相同二级科的所有科室代码
            string strsql = string.Format("select dept_code from {0}.sys_dept_dict where DEPT_CODE_SECOND='{1}'", DataUser.COMM, this.SecondDeptCode);
            DataTable depttable = OracleOledbBase.ExecuteDataSet(strsql).Tables[0];

            //获取相同核算科室的所有科室代码
            string strsqlaccount = string.Format("select dept_code from {0}.sys_dept_dict where ACCOUNT_DEPT_CODE='{1}'", DataUser.COMM, this.AccountDeptCode);
            DataTable accountdepttable = OracleOledbBase.ExecuteDataSet(strsqlaccount).Tables[0];

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["role_type"].ToString().Equals("1"))//院级权限
                    {
                        flags = "1";
                        deptfilter = "";
                        break;
                    }
                    else if (table.Rows[i]["role_type"].ToString().Equals("2"))//科级权限
                    {
                        if (depttable.Rows.Count < 1)
                        {
                            SelectDept dept = new SelectDept();
                            dept.DEPT_CODE = this.AccountDeptCode;
                            if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                            {
                                deptlist.Add(dept);
                            }
                        }
                        else
                        {
                            for (int a = 0; a < depttable.Rows.Count; a++)
                            {
                                SelectDept dept = new SelectDept();
                                dept.DEPT_CODE = depttable.Rows[a]["dept_code"].ToString();

                                if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                                {
                                    deptlist.Add(dept);
                                }
                            }
                        }
                    }
                    else if (table.Rows[i]["role_type"].ToString().Equals("3"))//组级权限
                    {
                        if (accountdepttable.Rows.Count < 1)
                        {
                            SelectDept dept = new SelectDept();
                            dept.DEPT_CODE = this.AccountDeptCode;
                            if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                            {
                                deptlist.Add(dept);
                            }
                        }
                        else
                        {
                            for (int a = 0; a < accountdepttable.Rows.Count; a++)
                            {
                                SelectDept dept = new SelectDept();
                                dept.DEPT_CODE = accountdepttable.Rows[a]["dept_code"].ToString();
                                if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                                {
                                    deptlist.Add(dept);
                                }
                            }
                        }
                    }
                    else if (table.Rows[i]["role_type"].ToString().Equals("4"))//人
                    {
                        SelectDept dept = new SelectDept();
                        dept.DEPT_CODE = this.AccountDeptCode;
                        if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                        {
                            deptlist.Add(dept);
                        }
                    }
                    else
                    {
                        flags = "1";
                        deptfilter = strfilter + "='-1'";
                        if (strfilter.Trim() == "")
                        {
                            deptfilter = "'-1'";
                        }
                    }
                }
                if (flags.Equals("0"))
                {
                    //专科中心
                    string strcenter = string.Format(@"select distinct a.dept_code from {0}.SYS_CENTER_DETAIL a,{0}.SYS_DEPT_CENTER b ,{0}.SYS_POWER_DETAIL c
                                                        where A.CENTER_ID=B.ID and B.ROLE_ID=C.POWER_ID   and upper(C.TARGET_ID)='{1}'", DataUser.COMM, _userinfo.UserId.ToUpper());
                    DataTable centertable = OracleOledbBase.ExecuteDataSet(strcenter).Tables[0];
                    for (int j = 0; j < centertable.Rows.Count; j++)
                    {
                        SelectDept dept = new SelectDept();
                        dept.DEPT_CODE = centertable.Rows[j]["dept_code"].ToString();
                        if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                        {
                            deptlist.Add(dept);
                        }
                    }

                    foreach (SelectDept depts in deptlist)
                    {
                        deptfilter += "'" + depts.DEPT_CODE + "',";
                    }
                    deptfilter = deptfilter.Substring(0, deptfilter.Length - 1);
                    if (strfilter != "")
                    {
                        deptfilter += ")";
                    }
                }
            }
            else
            {
                deptfilter = strfilter + "='-1'";
                if (strfilter.Trim() == "")
                {
                    deptfilter = "'-1'";
                }
            }
            return deptfilter;
        }

        /// <summary>
        /// 获取指定人员的权限
        /// </summary>
        /// <param name="strfilter"></param>
        /// <param name="menuid"></param>
        /// <param name="modid"></param>
        /// <returns></returns>
        public string GetUserDeptFilterByStaff(string strfilter, string menuid, string modid, string staffid)
        {
            string flags = "0";
            string deptfilter = string.Format("{0} in (", strfilter);
            if (strfilter.Trim() == "")
            {
                deptfilter = "";
            }
            List<SelectDept> deptlist = new List<SelectDept>();
            string str = "";
            int max = OracleOledbBase.GetMaxID("id", string.Format("{0}.t_templetdict", DataUser.ZLGL));
            if (modid == Constant.ZLGL_FUN_TYPE && int.Parse(menuid) <= max)
            {
                str = string.Format("select distinct B.ROLE_TYPE from {0}.SYS_POWER_DETAIL a,{0}.SYS_ROLE_DICT b,{0}.sys_role_function c,{3}.T_TEMPLETDICT d where A.POWER_ID=B.ROLE_ID and upper(a.target_id)='{1}' and  c.function_id = d.funckey and C.ROLE_ID=B.ROLE_ID AND d.id = '{2}'", DataUser.COMM, staffid.ToUpper(), int.Parse(menuid).ToString(), DataUser.ZLGL);
            }
            else
            {
                str = string.Format("select distinct B.ROLE_TYPE from {0}.SYS_POWER_DETAIL a,{0}.SYS_ROLE_DICT b,{0}.sys_role_function c,{0}.sys_application_menu d where A.POWER_ID=B.ROLE_ID and upper(a.target_id)='{1}' and  c.function_id = d.function_id and C.ROLE_ID=B.ROLE_ID AND d.menuid = '{2}' AND d.modid = '{3}'", DataUser.COMM, staffid.ToUpper(), menuid, modid);
            }
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];

            //获取相同二级科的所有科室代码
            string strsql = string.Format("select dept_code from {0}.sys_dept_dict where DEPT_CODE_SECOND='{1}'", DataUser.COMM, GetSecondDeptCodeByUserid(staffid.ToUpper()));
            DataTable depttable = OracleOledbBase.ExecuteDataSet(strsql).Tables[0];

            //获取相同核算科室的所有科室代码
            string strsqlaccount = string.Format("select dept_code from {0}.sys_dept_dict where ACCOUNT_DEPT_CODE='{1}'", DataUser.COMM, GetAccountDeptCodeByUserid(staffid.ToUpper()));
            DataTable accountdepttable = OracleOledbBase.ExecuteDataSet(strsqlaccount).Tables[0];

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["role_type"].ToString().Equals("1"))//院级权限
                    {
                        flags = "1";
                        deptfilter = "";
                        break;
                    }
                    else if (table.Rows[i]["role_type"].ToString().Equals("2"))//科级权限
                    {
                        if (depttable.Rows.Count < 1)
                        {
                            SelectDept dept = new SelectDept();
                            dept.DEPT_CODE = GetAccountDeptCodeByUserid(staffid.ToUpper());
                            if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                            {
                                deptlist.Add(dept);
                            }
                        }
                        else
                        {
                            for (int a = 0; a < depttable.Rows.Count; a++)
                            {
                                SelectDept dept = new SelectDept();
                                dept.DEPT_CODE = depttable.Rows[a]["dept_code"].ToString();

                                if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                                {
                                    deptlist.Add(dept);
                                }
                            }
                        }
                    }
                    else if (table.Rows[i]["role_type"].ToString().Equals("3"))//组级权限
                    {
                        if (accountdepttable.Rows.Count < 1)
                        {
                            SelectDept dept = new SelectDept();
                            dept.DEPT_CODE = GetAccountDeptCodeByUserid(staffid.ToUpper());
                            if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                            {
                                deptlist.Add(dept);
                            }
                        }
                        else
                        {
                            for (int a = 0; a < accountdepttable.Rows.Count; a++)
                            {
                                SelectDept dept = new SelectDept();
                                dept.DEPT_CODE = accountdepttable.Rows[a]["dept_code"].ToString();
                                if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                                {
                                    deptlist.Add(dept);
                                }
                            }
                        }
                    }
                    else if (table.Rows[i]["role_type"].ToString().Equals("4"))//人
                    {
                        SelectDept dept = new SelectDept();
                        dept.DEPT_CODE = GetAccountDeptCodeByUserid(staffid.ToUpper());
                        if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                        {
                            deptlist.Add(dept);
                        }
                    }
                    else
                    {
                        flags = "1";
                        deptfilter = strfilter + "='-1'";
                        if (strfilter.Trim() == "")
                        {
                            deptfilter = "'-1'";
                        }
                    }
                }
                if (flags.Equals("0"))
                {
                    //专科中心
                    string strcenter = string.Format(@"select distinct a.dept_code from {0}.SYS_CENTER_DETAIL a,{0}.SYS_DEPT_CENTER b ,{0}.SYS_POWER_DETAIL c
                                                        where A.CENTER_ID=B.ID and B.ROLE_ID=C.POWER_ID   and upper(C.TARGET_ID)='{1}'", DataUser.COMM, staffid.ToUpper());
                    DataTable centertable = OracleOledbBase.ExecuteDataSet(strcenter).Tables[0];
                    for (int j = 0; j < centertable.Rows.Count; j++)
                    {
                        SelectDept dept = new SelectDept();
                        dept.DEPT_CODE = centertable.Rows[j]["dept_code"].ToString();
                        if (deptlist.Exists(delegate(SelectDept depts) { if (depts.DEPT_CODE == dept.DEPT_CODE) return true; else return false; }) == false)
                        {
                            deptlist.Add(dept);
                        }
                    }

                    foreach (SelectDept depts in deptlist)
                    {
                        deptfilter += "'" + depts.DEPT_CODE + "',";
                    }
                    deptfilter = deptfilter.Substring(0, deptfilter.Length - 1);
                    if (strfilter != "")
                    {
                        deptfilter += ")";
                    }
                }
            }
            else
            {
                deptfilter = strfilter + "='-1'";
                if (strfilter.Trim() == "")
                {
                    deptfilter = "'-1'";
                }
            }
            return deptfilter;
        }

        /// <summary>
        /// 人的权限
        /// </summary>
        /// <param name="strfilter"></param>
        /// <param name="menuid"></param>
        /// <param name="modid"></param>
        /// <returns></returns>
        public string GetUserFilter(string strfilter, string menuid, string modid)
        {
            string deptfilter = string.Format("{0} in (", strfilter);
            string str = string.Format("select distinct B.ROLE_TYPE from {0}.SYS_POWER_DETAIL a,{0}.SYS_ROLE_DICT b,{0}.sys_role_function c,{0}.sys_application_menu d where A.POWER_ID=B.ROLE_ID and upper(a.target_id)='{1}' and  c.function_id = d.function_id and C.ROLE_ID=B.ROLE_ID AND d.menuid = '{2}' AND d.modid = '{3}' and b.role_type<>'3'", DataUser.COMM, _userinfo.UserId.ToUpper(), menuid, modid);
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];

            if (table.Rows.Count > 0)
            {
                deptfilter = "";
            }
            else
            {
                if (strfilter.Equals(string.Empty))
                {
                    deptfilter = string.Format("'{0}'", this.UserId);
                }
                else
                {
                    deptfilter = string.Format("{0} = '{1}'", strfilter, this.UserId);
                }
            }
            return deptfilter;
        }

        /// <summary>
        /// 成本项目权限
        /// </summary>
        /// <returns></returns>
        public DataTable GetCostFilter()
        {
            string str = string.Format("select distinct a.item_code,a.item_name from {0}.CBHS_COST_ITEM_DICT a,{1}.SYS_POWER_DETAIL b,{2}.SYS_SPECPOWER_ROLE c  where a.item_code=c.id and c.type={3} and c.role_id=b.POWER_ID and upper(b.TARGET_ID)='{4}' order by a.item_code", DataUser.CBHS, DataUser.COMM, DataUser.COMM, Constant.COST_SPEPOWER, _userinfo.UserId.ToUpper());
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 人员类别权限
        /// </summary>
        /// <returns></returns>
        public DataTable GetPersTypeFilter()
        {
            string str = string.Format("select distinct a.SERIAL_NO id,a.PERS_SORT_NAME name from {0}.PERS_SORT_DICT a,{1}.SYS_POWER_DETAIL b,{2}.SYS_SPECPOWER_ROLE c  where a.SERIAL_NO=c.id and c.type={3} and c.ROLE_ID=b.POWER_ID and upper(b.TARGET_ID)='{4}'", DataUser.RLZY, DataUser.COMM, DataUser.COMM, Constant.PER_SPEPOWER, _userinfo.UserId.ToUpper());
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 角色
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="modid"></param>
        /// <returns></returns>
        public DataTable GetPersRoleType(string menuid, string modid)
        {
            string str = string.Format("select '0'||ID id,role_type from {0}.sys_role_type where id>=(select min(a.ROLE_TYPE) from {0}.SYS_ROLE_DICT a,{0}.SYS_POWER_DETAIL b,{0}.SYS_APPLICATION_MENU c,{0}.SYS_ROLE_FUNCTION d where a.ROLE_ID=b.POWER_ID and c.FUNCTION_ID=d.FUNCTION_ID and b.POWER_ID=d.ROLE_ID and c.MENUID='{2}' and c.MODID='{3}' and upper(b.TARGET_ID)='{1}')", DataUser.COMM, _userinfo.UserId.ToUpper(), menuid, modid);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        //liu.shh  2012.12.18
        /// <summary>
        /// 科室编码
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="modid"></param>
        /// <returns></returns>
        public string GetPersonDept(string user_id)
        {
            string str = "";
            str = string.Format("SELECT USER_DEPT FROM {0}.USERS WHERE ROWNUM = 1 AND USER_ID = '{1}'", DataUser.HISFACT, user_id.ToLower());
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            string deptCode = "'" + table.Rows[0]["USER_DEPT"].ToString() + "'";
            return deptCode;
        }

        #endregion
        #region 私有方法
        /// <summary>
        /// 创建人员对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private UserInfo GetUserById(string userid)
        {
            string str = string.Format("SELECT UPPER(DB_USER) AS DB_USER,USER_ID  AS USER_ID,USER_NAME FROM {0}.USERS WHERE UPPER(USER_ID) =?", DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", userid.ToUpper()) };
            DataTable table = OracleOledbBase.ExecuteDataSet(str, cmdPara).Tables[0];
            string curdate = System.DateTime.Now.ToString("yyyy-MM") + "-01";
            if (table.Rows.Count > 0)
            {
                UserInfo user = new UserInfo(table.Rows[0]["DB_USER"].ToString(), table.Rows[0]["USER_ID"].ToString(), table.Rows[0]["user_name"].ToString(), curdate);
                return user;
            }
            else
            {
                throw new GlobalException("错误的人员编码！", "[" + userid + "]为无效的人员编码！");
            }
        }

        /// <summary>
        /// 得到人的staff_id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetStaffid(string userid)
        {
            string str = string.Format("select staff_id from {0}.new_staff_info where upper(user_id)='{1}'", DataUser.RLZY, userid.ToUpper());
            object obj = OracleOledbBase.ExecuteScalar(str);
            if (obj == null)
            {
                return "";
            }
            else
                return obj.ToString();
        }

        /// <summary>
        /// 用户在his的科室代码
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetHisDeptCodeByUserid(string userid)
        {
            string str = string.Format("SELECT USER_DEPT FROM {0}.USERS WHERE (UPPER(USER_ID) =?)", DataUser.HISFACT);
            //string str = string.Format("SELECT  max(b.dept_code) USER_DEPT FROM {0}.USERS a,{0}.staff_dict b WHERE upper(a.db_user)=upper(b.USER_NAME) and UPPER(a.USER_ID) =?", DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", userid.ToUpper()) };
            object obj = OracleOledbBase.ExecuteScalar(str, cmdPara);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                throw new GlobalException("错误！", "[" + userid + "]在his的科室为空！");
            }
        }

        /// <summary>
        /// 用户在his的科室名称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetHisDeptNameByUserid(string userid)
        {
            string str = string.Format("SELECT DEPT_NAME FROM {0}.DEPT_DICT WHERE (DEPT_CODE =?)", DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", GetHisDeptCodeByUserid(userid)) };
            return OracleOledbBase.ExecuteScalar(str, cmdPara).ToString();
        }

        /// <summary>
        /// 用户所在三级科室或核算科室代码
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetAccountDeptCodeByUserid(string userid)
        {
            string str = string.Format("SELECT a.account_dept_code FROM {0}.sys_dept_dict a, rlzy.new_staff_info b WHERE a.dept_code = b.dept_code AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            //string str = string.Format("SELECT max(a.account_dept_code) FROM {0}.sys_dept_dict a, {1}.users b,{1}.staff_dict c WHERE a.dept_code = c.dept_code and upper(b.db_user)=upper(c.USER_NAME) AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", userid.ToUpper()) };
            return OracleOledbBase.ExecuteScalar(str, cmdPara).ToString();
        }

        /// <summary>
        /// 用户所在三级科室或核算科室名称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetAccountDeptNameByUserid(string userid)
        {
            string str = string.Format("SELECT a.account_dept_name FROM {0}.sys_dept_dict a, rlzy.new_staff_info b WHERE a.dept_code = b.dept_code AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            // string str = string.Format("SELECT max(a.account_dept_name) FROM {0}.sys_dept_dict a, {1}.users b,{1}.staff_dict c WHERE a.dept_code = c.dept_code and upper(b.db_user)=upper(c.USER_NAME) AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", userid.ToUpper()) };
            return OracleOledbBase.ExecuteScalar(str, cmdPara).ToString();
        }

        /// <summary>
        /// 用户所在二级科室代码
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetSecondDeptCodeByUserid(string userid)
        {
            string str = string.Format("SELECT a.DEPT_CODE_SECOND FROM {0}.sys_dept_dict a, rlzy.new_staff_info b WHERE a.dept_code = b.dept_code AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            //string str = string.Format("SELECT max(a.DEPT_CODE_SECOND) FROM {0}.sys_dept_dict a, {1}.users b,{1}.staff_dict c  WHERE a.dept_code = c.dept_code and upper(b.db_user)=upper(c.USER_NAME) AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", userid.ToUpper()) };
            return OracleOledbBase.ExecuteScalar(str, cmdPara).ToString();
        }

        /// <summary>
        /// 用户所在二级科室名称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetSecondDeptNameByUserid(string userid)
        {
            string str = string.Format("SELECT a.DEPT_NAME_SECOND FROM {0}.sys_dept_dict a, rlzy.new_staff_info b WHERE a.dept_code = b.dept_code AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            //string str = string.Format("SELECT max(a.DEPT_NAME_SECOND) FROM {0}.sys_dept_dict a, {1}.users b,{1}.staff_dict c  WHERE a.dept_code = c.dept_code and upper(b.db_user)=upper(c.USER_NAME) AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", userid.ToUpper()) };
            return OracleOledbBase.ExecuteScalar(str, cmdPara).ToString();
        }

        /// <summary>
        /// 科室属性
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetDeptType(string userid)
        {
            string str = string.Format("SELECT a.DEPT_TYPE FROM {0}.sys_dept_dict a, rlzy.new_staff_info b WHERE a.dept_code = b.dept_code AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            //string str = string.Format("SELECT max(a.DEPT_TYPE) FROM {0}.sys_dept_dict a, {1}.users b,{1}.staff_dict c  WHERE a.dept_code = c.dept_code and upper(b.db_user)=upper(c.USER_NAME) AND UPPER(b.user_id)=?", DataUser.COMM, DataUser.HISFACT);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", userid.ToUpper()) };
            return OracleOledbBase.ExecuteScalar(str, cmdPara).ToString();
        }

        /// <summary>
        /// 人员菜单权限
        /// </summary>
        /// <param name="db_user"></param>
        /// <returns></returns>
        private DataTable PowerDetail(string userid, string dbuser)
        {
            StringBuilder sql = new StringBuilder();
            if (dbuser.ToLower() == "admin" && OracleOledbBase.ExecuteDataSet(string.Format("select * from {0}.SYS_POWER_DETAIL", DataUser.COMM)).Tables[0].Rows.Count < 1)
            {
                sql.AppendFormat(@"SELECT   b.menuid, b.menuname, b.modid, b.grouptext, b.pagename, b.pagetitle,
         b.pageurl, b.optionrule,
         DECODE (SUBSTR (b.menuid, 5, 2), '00', '0', b.sortno) sort,
         b.openstatus, b.openmode, b.ico,B.FUNCTION_ID
    FROM {0}.sys_application_menu b
   WHERE b.del_f = 0 AND b.visible > 0 and b.FUNCTION_ID in('9001','9002')", DataUser.COMM);
            }
            //            else if (GetConfig.TestString.Equals("god"))
            //            {
            //                sql.AppendFormat(@"SELECT distinct m.*
            //  FROM (SELECT LPAD (templetid, 6, '0') AS menuid, guidename AS menuname,
            //               '3' AS modid, guidetype AS grouptext, '1' AS pagename,
            //               guidename AS pagetitle,
            //               '/zlgl/Templet/Page/Pagelist.aspx' AS pageurl,
            //               '1' AS optionrule, '0' AS sort, 0 AS openstatus, 0 AS openmode,
            //               icon AS ico, funckey AS function_id,0 AS persons_flags
            //          FROM (SELECT   g_guidetype.ID AS guidetypeid,
            //                         g_guidetype.guidetype AS guidetype,
            //                         tableguide.ID AS guideid,
            //                         tableguide.guidename AS guidename,
            //                         tableguide.templetid AS templetid,
            //                         t_templetdict.NAME AS templetname_old,
            //                         t_templetdict.funckey AS funckey,
            //                         TO_CHAR (g_guidetype.guidetype) AS guidetypename,
            //                         (TO_CHAR (t_templetdict.NAME)) AS templetname, icon
            //                    FROM (SELECT ID AS ID, commguidename AS guidename,
            //                                 templetid AS templetid,
            //                                 commguidetypeid AS typeid
            //                            FROM {0}.g_commonguide
            //                          UNION
            //                          SELECT ID AS ID, specguidename AS guidename,
            //                                 templetid AS templetid,
            //                                 specguidetypeid AS typeid
            //                            FROM {0}.g_specialtyguide) tableguide,
            //                         {0}.t_templetdict,
            //                         {0}.g_guidetype
            //                   WHERE tableguide.typeid = g_guidetype.ID
            //                     AND tableguide.templetid = t_templetdict.ID
            //                ORDER BY t_templetdict.showorder)
            //        UNION ALL
            //        SELECT b.menuid, b.menuname, b.modid, b.grouptext, b.pagename,
            //               b.pagetitle, b.pageurl, b.optionrule,
            //               DECODE (SUBSTR (b.menuid, 5, 2), '00', '0', b.sortno) sort,
            //               b.openstatus, b.openmode, b.ico, b.function_id,b.persons_flags
            //          FROM {1}.sys_application_menu b,{1}.SYS_APPLICATION_SUBSYS c
            //        WHERE b.del_f = 0 AND b.visible > 0 and b.modid=c.app_id and c.del_f=0) m
            //  
            //   order by   to_number(modid),to_number(sort)", DataUser.ZLGL, DataUser.COMM);
            //            }
            else
            {
                sql.AppendFormat(@"SELECT distinct m.*
  FROM (SELECT LPAD (templetid, 6, '0') AS menuid, guidename AS menuname,
               '3' AS modid, guidetype AS grouptext, '1' AS pagename,
               guidename AS pagetitle,
               '/zlgl/Templet/Page/Pagelist.aspx' AS pageurl,
               '1' AS optionrule, to_char(sorts) AS sort, 0 AS openstatus, 0 AS openmode,
               icon AS ico, funckey AS function_id,0 AS persons_flags
          FROM (SELECT   g_guidetype.ID AS guidetypeid,
                         g_guidetype.guidetype AS guidetype,
                         tableguide.ID AS guideid,
                         tableguide.guidename AS guidename,
                         tableguide.templetid AS templetid,
                         t_templetdict.NAME AS templetname_old,
                         t_templetdict.funckey AS funckey,
                         t_templetdict.SHOWORDER as sorts,
                         TO_CHAR (g_guidetype.guidetype) AS guidetypename,
                         (TO_CHAR (t_templetdict.NAME)) AS templetname, icon
                    FROM (SELECT ID AS ID, commguidename AS guidename,
                                 templetid AS templetid,
                                 commguidetypeid AS typeid
                            FROM {0}.g_commonguide
                          UNION
                          SELECT ID AS ID, specguidename AS guidename,
                                 templetid AS templetid,
                                 specguidetypeid AS typeid
                            FROM {0}.g_specialtyguide) tableguide,
                         {0}.t_templetdict,
                         {0}.g_guidetype
                   WHERE tableguide.typeid = g_guidetype.ID
                     AND tableguide.templetid = t_templetdict.ID
                ORDER BY t_templetdict.showorder)
        UNION ALL
        SELECT b.menuid, b.menuname, b.modid, b.grouptext, b.pagename,
               b.pagetitle, b.pageurl, b.optionrule,
               DECODE (SUBSTR (b.menuid, 5, 2), '00', '0', b.sortno) sort,
               b.openstatus, b.openmode, b.ico, b.function_id,b.persons_flags
          FROM {1}.sys_application_menu b,{1}.SYS_APPLICATION_SUBSYS c
         WHERE b.del_f = 0 AND b.visible > 0 and b.modid=c.app_id and c.del_f=0 ) m,
       {1}.sys_power_detail n,
       {1}.sys_role_function h
 WHERE n.power_id = h.role_id
   AND upper(n.target_id) = '{2}'
   AND m.function_id = h.function_id order by   to_number(modid),to_number(sort)", DataUser.ZLGL, DataUser.COMM, userid.ToUpper());
            }
            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }

        /// <summary>
        /// 全成本项目权限
        /// </summary>
        /// <returns></returns>
        public DataTable GetxyhsCostFilter()
        {
            string str = string.Format("select distinct a.item_code,a.item_name from {0}.CBHS_COST_ITEM_DICT a,{1}.SYS_POWER_DETAIL b,{2}.SYS_SPECPOWER_ROLE c  where a.item_code=c.id and c.type={3} and c.role_id=b.POWER_ID and upper(b.TARGET_ID)='{4}' order by a.item_code", DataUser.CBHS, DataUser.COMM, DataUser.COMM, Constant.XYHS_COST_SPEPOWER, _userinfo.UserId.ToUpper());
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }
        #endregion

    }
}
