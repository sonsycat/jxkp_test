using System;
using System.Data;
using System.Data.SqlClient; 
using System.Data.OleDb;
using System.Configuration;
using GoldNet.JXKP.PowerManager;
using GoldNet.Comm;
using GoldNet.JXKP.BLL.Organise;
using Microsoft.Win32;
using GoldNet.Comm.DAL.Oracle;
namespace GoldNet.JXKP.BLL.Organise
{
	/// <summary>
	/// 人员类
	/// </summary>
	[Serializable()]
	public class Staff : GoldNet.JXKP.PowerManager.IStaffTargetInfo
	{

		#region 内部VO

		[Serializable()]
			public class StaffInfo
		{
			public string StaffName;
			public string StaffKey;
			public string DeptCode;
			public string DeptName;
			public StaffInfo(string staffKey,string staffName,string deptcode,string deptname)
			{
				StaffName=staffName;
				StaffKey=staffKey;
				DeptCode=deptcode;
				DeptName=deptname;

			}

		}
		#endregion

		#region 私有变量

		private StaffInfo _StaffInfo;

		#endregion

		#region 构造函数
		public Staff(string key)
		{
			_StaffInfo=getStaffByKey(key);
		}
		#endregion
		
		#region 私有方法
		/// <summary>
		/// 获得人员对象通过人员KEY
		/// </summary>
		/// <param name="key">人员KEY</param>
		/// <returns>指定人员Key的人员对象</returns>
		private StaffInfo getStaffByKey(string key)
		{
			DataTable table=GetStaffInDeptCode(key);
			if ( table.Rows.Count!=0 ) 
			{
                StaffInfo staff = new StaffInfo(key, table.Rows[0]["USER_NAME"].ToString(), table.Rows[0]["ACCOUNT_DEPT_CODE"].ToString(), table.Rows[0]["ACCOUNT_DEPT_NAME"].ToString());
				return staff;
			}
			else
			{
				StaffInfo staff=new StaffInfo(key,key,"","");
				return staff;
			}

		}

		#endregion

		#region 公共方法

		/// <summary>
		/// 获得人员对象
		/// </summary>
		/// <param name="key">人员对象序号</param>
		/// <returns>返回人员对象</returns>
		public IStaffTargetInfo GetObjByKey(string key)
		{
		   Staff staff=new Staff(key);
		   return staff;
		}

		private DataTable GetStaffInDeptCode(string key)
		{
            string GetSTAFF_IN_DEPT_CODE = string.Format("SELECT a.ACCOUNT_DEPT_CODE,a.ACCOUNT_DEPT_NAME,b.user_name FROM {0}.sys_dept_dict a, {1}.users b WHERE a.dept_code = b.user_dept AND b.user_id=?", DataUser.COMM,DataUser.HISFACT);
			
			OleDbParameter[] cmdPara = new OleDbParameter[] {new OleDbParameter("", key)};
			DataTable table =OracleOledbBase.ExecuteDataSet(GetSTAFF_IN_DEPT_CODE,cmdPara).Tables[0];

			return table;
		}

		#endregion

		#region 属性
		#region ITargetInfo 成员

		/// <summary>
		/// 人员序号
		/// </summary>
		public string Key
		{
			get
			{
				return _StaffInfo.StaffKey;
			}
		}

		#endregion

		/// <summary>
		/// 人员姓名
		/// </summary>
		public string Name
		{
			get
			{
				return _StaffInfo.StaffName;
			}
		}
		public string Dept
		{
			get
			{
				return _StaffInfo.DeptCode;
			}
		}
		public string DeptName
		{
			get
			{
				return _StaffInfo.DeptName;
			}
		}


		#endregion

	}
}
