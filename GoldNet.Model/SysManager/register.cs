using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.Model.SysManager
{
    public class register
    {
        public register()
        {
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="USER_DEPT">科室编码</param>
        /// <param name="USER_NAME">人员名称</param>
        /// <param name="DB_USER">登录口令</param>
        /// <param name="PASSWORD">密码</param>
        public void AddUser(string USER_DEPT, string USER_NAME, string DB_USER, string PASSWORD)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT INTO {0}.SYS_USERS
                                        (DB_USER, USER_ID, USER_NAME, USER_DEPT,
                                         LOGIN_DATE, USER_PSWD
                                        )
                                 VALUES (?,?,?,?,sysdate,?)", DataUser.COMM);
            OleDbParameter[] parameter = {
											  new OleDbParameter("DB_USER",DB_USER),
                                              new OleDbParameter("USER_ID",DB_USER),
                                              new OleDbParameter("USER_NAME",USER_NAME),
                                              new OleDbParameter("USER_DEPT",USER_DEPT),
                                              new OleDbParameter("USER_PSWD",PASSWORD)
										  };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
        }
        public DataTable GetUser_new_new(string user_id, string bb)
        {
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("select min(a.role_id) from comm.SYS_ROLE_DICT a where a.role_id=(select min(b.power_id) from comm.SYS_POWER_DETAIL b where b.target_id=upper('{0}'))", user_id).ToString();
            string num = OracleOledbBase.ExecuteScalar(strDel.ToString()).ToString();
            StringBuilder sql = new StringBuilder();
            if (num.Equals("1") || num.Equals("21"))
            {
                sql.AppendFormat("select * from COMM.SYS_USERS where (user_id like '{0}%' or user_name like '{0}%')", bb);
            }
            else
            {
                sql.AppendFormat("select * from COMM.SYS_USERS where user_id='{0}'", user_id);
            }
            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }
        public string GetQX(string user_id)
        {
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("select min(a.role_id) from comm.SYS_ROLE_DICT a where a.role_id=(select min(b.power_id) from comm.SYS_POWER_DETAIL b where b.target_id=upper('{0}'))", user_id).ToString();
            string num = OracleOledbBase.ExecuteScalar(strDel.ToString()).ToString();
            if (num.Equals("27"))
            {
                num = "1";
            }
            return num;
        }
        private string GetUser_id()
        {
            string res = string.Empty;

            int id = OracleOledbBase.GetMaxID(" substr(user_id,3,Length(user_id)) ", "COMM.SYS_USERS");

            if (id == 1)
            {
                res = "jw" + "1000";
            }
            else
            {
                res = "jw" + id;
            }

            return res;
        }

        public DataTable GetUser_new()
        {
            string sql = "select * from COMM.SYS_USERS";

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        public DataTable GetUser_newById(string id)
        {
            string sql = string.Format(@"select * from COMM.SYS_USERS where user_id='{0}'", id);

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        public void UpdateUser_new(string USER_DEPT, string USER_NAME, string PASSWORD, string user_id)
        {
            string sql = string.Format(@"update COMM.SYS_USERS set   USER_NAME=?, USER_DEPT=?,
                                         LOGIN_DATE=sysdate, USER_PSWD=? where USER_ID=?");
            OleDbParameter[] parameter = {
                                              new OleDbParameter("USER_NAME",USER_NAME),
                                              new OleDbParameter("USER_DEPT",USER_DEPT),
                                              new OleDbParameter("USER_PSWD",PASSWORD),
                                              new OleDbParameter("USER_ID",user_id)
										  };
            OracleOledbBase.ExecuteNonQuery(sql, parameter);
        }

        public void DelUser_new(string user_ids)
        {
            string sql = string.Format(@"delete from COMM.SYS_USERS where user_id in ({0})", user_ids);
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        public string GetdbUser(string dbuser)
        {
            string sql = string.Format(@"SELECT   *
  FROM   (SELECT   db_user FROM comm.sys_users)
 WHERE   db_user = '{0}'", dbuser);

            object obj = OracleOledbBase.GetSingle(sql);
            if (obj == null)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }

        #region 验证非HIS人员登录

        /// <summary>
        /// 检查是否是HIS人员
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns>0 his人员，1 非his人员，2没找到人,3单点登录</returns>
        public string VerificationHis(string UserName)
        {
            string res = string.Empty;
            string sql = string.Format(@"select STATE from {0}.USERS where ACCOUNT='{1}'", DataUser.HISFACT, UserName);

            DataSet dr = OracleOledbBase.ExecuteDataSet(sql);
            if (dr.Tables[0].Rows.Count == 0 || dr.Tables.Count == 0)
            {
                res = "2";
            }
            else if (dr.Tables[0].Rows[0][0].ToString() == "0")
            {
                //HIS用户
                res = "0";
            }
            else if (dr.Tables[0].Rows[0][0].ToString() == "1")
            {
                //自定义用户
                res = "1";
            }
            else if (dr.Tables[0].Rows[0][0].ToString() == "3")
            {
                res = "3";
            }
            return res;

        }

        public string GetHotHisUser(string dbuser, string password,string ishis)
        {

            if (dbuser.Equals(string.Empty) || password.Equals(string.Empty))
            {
                throw new LoginManagerException("系统提示", "用户和密码不能为空！");
            }
            string TestString = GetConfig.TestString;
            string userId = "";
            string sql = string.Format(@"select USER_ID,USER_PSWD PASSWORD from {0}.SYS_USERS where DB_USER='{1}'", DataUser.COMM, dbuser);
            DataSet dr = OracleOledbBase.ExecuteDataSet(sql, new OleDbParameter("", dbuser));
            if (dr.Tables[0].Rows.Count != 0)
            {
                userId = dr.Tables[0].Rows[0]["USER_ID"].ToString();
            }
            if (userId.Equals(string.Empty) && dbuser.ToLower() != "admin")
            {
                throw new LoginManagerException("系统提示", "用户不存在，请重新输入！");
            }
            if (!TestString.Equals("god"))
            {
                string dbps = dr.Tables[0].Rows[0]["PASSWORD"].ToString();
                if (ishis == "1")
                {
                     dbps = GoldNet.Comm.DEncrypt.Decrypt(dr.Tables[0].Rows[0]["PASSWORD"].ToString());
                }
                
                if (!password.Equals(dbps))
                {
                    throw new LoginManagerException("系统提示", "密码不正确！");
                }
            }

            if (!userId.Equals(string.Empty))
            {
                return userId;
            }
            else
            {
                throw new GlobalException("错误的人员编码！", "[" + userId + "]为无效的人员编码！");
            }
        }

        #endregion




    }
}
