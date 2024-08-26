using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;

namespace GoldNet.Model
{
	/// <summary>
	/// 角色实体类SYS_ROLE_DICT
	/// </summary>
	[Serializable]
	public class SYS_ROLE_DICT
	{
		public SYS_ROLE_DICT()
		{
            _roleinfo = new roleinfo();
        }
		#region Model
        [Serializable()]
        public class roleinfo
        {
            public int _role_id;
            public string _role_name;
            public string _remark;
            public string _role_type;
            public string _role_app;

            public roleinfo(int roleid, string rolename, string remark,string roletype,string roleapp)
            {
                _role_id = roleid;
                _role_name = rolename;
                _remark = remark;
                _role_type = roletype;
                _role_app = roleapp;
            }
            public roleinfo()
            {}
        }
        private roleinfo _roleinfo;

        public SYS_ROLE_DICT(int roleid)
        {
            _roleinfo = GetRoleById(roleid);
        }

        /// <summary>
        /// 角色id
        /// </summary>
        public int ROLE_ID
        {
            set { _roleinfo._role_id = value; }
            get { return _roleinfo._role_id; }
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string ROLE_NAME
        {
            set { _roleinfo._role_name = value; }
            get { return _roleinfo._role_name; }
        }
        /// <summary>
        /// 角色说明
        /// </summary>
        public string REMARK
        {
            set { _roleinfo._remark = value; }
            get { return _roleinfo._remark; }
        }
        /// <summary>
        /// 角色类别代码
        /// </summary>
        public string ROLE_TYPE
        {
            set { _roleinfo._role_type = value; }
            get { return _roleinfo._role_type; }
        }
        /// <summary>
        /// 项目
        /// </summary>
        public string ROLE_APP
        {
            set { _roleinfo._role_app = value; }
            get { return _roleinfo._role_app; }
        }
        /// <summary>
        /// 角色类别名称
        /// </summary>
        public string ROLE_TYPE_NAME
        {
            get { return GetRoleTypeName(int.Parse(_roleinfo._role_type)); }
        }

        private string GetRoleTypeName(int roletypeid)
        {
            string str = string.Format("select role_type from {0}.sys_role_type where id=?",DataUser.COMM);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", roletypeid) };
            return OracleOledbBase.ExecuteScalar(str,cmdPara).ToString();
        }
        /// <summary>
        /// 角色对象
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        private roleinfo GetRoleById(int roleid)
        {
            string str = string.Format("SELECT ROLE_ID,ROLE_NAME,REMARK,ROLE_TYPE,ROLE_APP FROM {0}.SYS_ROLE_DICT WHERE (ROLE_ID =?)", DataUser.COMM);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", roleid) };
            DataTable table = OracleOledbBase.ExecuteDataSet(str, cmdPara).Tables[0];
            if (table.Rows.Count > 0)
            {
                roleinfo role = new roleinfo(roleid, table.Rows[0]["ROLE_NAME"].ToString(), table.Rows[0]["REMARK"].ToString(), table.Rows[0]["ROLE_TYPE"].ToString(),table.Rows[0]["role_app"].ToString());
                return role;
            }
            else
            {
                return null ;
            }
        }

       
		#endregion Model
        
      
	}
}

