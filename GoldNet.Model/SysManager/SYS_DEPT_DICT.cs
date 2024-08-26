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
    /// 科室
    /// </summary>
    [Serializable]
    public class SYS_DEPT_DICT
    {
        public SYS_DEPT_DICT()
		{
            _deptinfo = new deptinfo();
        }
         [Serializable()]
        public class deptinfo
        {
            public string  _id;
            public string _dept_code;
            public string _dept_name;
            public string _p_dept_code;
            public string _p_dept_name;
            public string _dept_type;
            public string _dept_lcattr;
            public string _sort_no;
            public string _show_flag;
            public string _attr;
            public string _input_code;
            public string _account_dept_code;
            public string _account_dept_name;
            public string _dept_code_second;
            public string _dept_name_second;
            public string _out_or_in;
            public string _dept_group;

            public deptinfo(string id, string deptcode, string deptname, string pdeptcode,string pdeptname, string depttype, string deptlcattr, string sortno, string showflag, string attr, string inputcode, string accountdeptcode, string accountdeptname, string deptcodesecond, string deptnamesecond,string out_or_in,string deptgrooup)
            {
                _id=id;
                _dept_code=deptcode;
                _dept_name=deptname;
                _p_dept_name = pdeptname;
                _p_dept_code=pdeptcode;
                _dept_type=depttype;
                _dept_lcattr=deptlcattr;
                _sort_no=sortno;
                _show_flag=showflag;
                _attr=attr;
                _input_code=inputcode;
                _account_dept_code=accountdeptcode;
                _account_dept_name=accountdeptname;
                _dept_code_second=deptcodesecond;
                _dept_name_second = deptnamesecond;
                _out_or_in = out_or_in;
                _dept_group = deptgrooup;
            }
            public deptinfo()
            {}
        }
         private deptinfo _deptinfo;

         public SYS_DEPT_DICT(string deptcode)
         {
             _deptinfo = GetDeptBydeptcode(deptcode);
         }
        /// <summary>
        /// id
        /// </summary>
         public string  ID
         {
             set { _deptinfo._id = value; }
             get { return _deptinfo._id; }
         }
        /// <summary>
        /// 科室代码
        /// </summary>
         public string DEPT_CODE
         {
             set { _deptinfo._dept_code = value; }
             get { return _deptinfo._dept_code; }
         }
        /// <summary>
        /// 科室名称
        /// </summary>
         public string DEPT_NAME
         {
             set { _deptinfo._dept_name = value; }
             get { return _deptinfo._dept_name; }
         }
        /// <summary>
        /// 上级科室代码
        /// </summary>
         public string P_DEPT_CODE
         {
             set { _deptinfo._p_dept_code = value; }
             get { return _deptinfo._p_dept_code; }
         }
        /// <summary>
        /// 上级科室名称
        /// </summary>
         public string P_DEPT_Name
         {
             set { _deptinfo._p_dept_name = value; }
             get { return _deptinfo._p_dept_name; }
         }
        /// <summary>
        /// 科室类型
        /// </summary>
         public string DEPT_TYPE
         {
             set { _deptinfo._dept_type = value; }
             get { return _deptinfo._dept_type; }
         }
        /// <summary>
        /// 临床属性
        /// </summary>
         public string DEPT_LCATTR
         {
             set { _deptinfo._dept_lcattr = value; }
             get { return _deptinfo._dept_lcattr; }
         }
         public string DEPT_GROUP
         {
             set { _deptinfo._dept_group = value; }
             get { return _deptinfo._dept_group; }
         }
        /// <summary>
        /// 排序
        /// </summary>
         public string SORT_NO
         {
             set { _deptinfo._sort_no = value; }
             get { return _deptinfo._sort_no; }
         }
        /// <summary>
        /// 是否启用
        /// </summary>
         public string SHOW_FLAG
         {
             set { _deptinfo._show_flag = value; }
             get { return _deptinfo._show_flag; }
         }
        /// <summary>
        /// 是否核算
        /// </summary>
         public string ATTR
         {
             set { _deptinfo._attr = value; }
             get { return _deptinfo._attr; }
         }
        /// <summary>
        /// 输入码
        /// </summary>
         public string INPUT_CODE
         {
             set { _deptinfo._input_code = value; }
             get { return _deptinfo._input_code; }
         }
        /// <summary>
        /// 核算科室代码
        /// </summary>
         public string ACCOUNT_DEPT_CODE
         {
             set { _deptinfo._account_dept_code = value; }
             get { return _deptinfo._account_dept_code; }
         }
        /// <summary>
        /// 核算科室名称
        /// </summary>
         public string ACCOUNT_DEPT_NAME
         {
             set { _deptinfo._account_dept_name = value; }
             get { return _deptinfo._account_dept_name; }
         }
        /// <summary>
        /// 二级科室代码
        /// </summary>
         public string DEPT_CODE_SECOND
         {
             set { _deptinfo._dept_code_second = value; }
             get { return _deptinfo._dept_code_second; }
         }
        /// <summary>
        /// 二级科室名称
        /// </summary>
         public string DEPT_NAME_SECOND
         {
             set { _deptinfo._dept_name_second = value; }
             get { return _deptinfo._dept_name_second; }
         }

        /// <summary>
        /// 门诊或者住院
        /// </summary>
         public string OUT_OR_IN
         {
             get { return _deptinfo._out_or_in; }
             set {  _deptinfo._out_or_in = value; }
         }
         private deptinfo GetDeptBydeptcode(string deptcode)
         {
             StringBuilder str = new StringBuilder();
             str.AppendFormat(@"SELECT b.ID, nvl(a.dept_code,b.dept_code) dept_code, nvl(a.dept_name,b.dept_name) dept_name, nvl(a.input_code,b.input_code) input_code, b.p_dept_code,
       b.dept_type, b.dept_lcattr, b.dept_date, b.sort_no, b.show_flag,
       b.attr, b.account_dept_code, b.account_dept_name, b.dept_code_second,
       b.dept_name_second,b.p_dept_name,b.out_or_in,b.dept_group 
  FROM {0}.dept_dict a full join {1}.sys_dept_dict b
 on a.dept_code = b.dept_code ", DataUser.HISFACT, DataUser.COMM);
             str.AppendFormat(" where nvl(a.dept_code,b.dept_code)='{0}'", deptcode);
             DataTable table= OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
             if (table.Rows.Count > 0)
             {
                 deptinfo dept = new deptinfo(table.Rows[0]["id"].ToString(), table.Rows[0]["dept_code"].ToString(), table.Rows[0]["dept_name"].ToString(), table.Rows[0]["p_dept_code"].ToString(), table.Rows[0]["p_dept_name"].ToString(), table.Rows[0]["dept_type"].ToString(), table.Rows[0]["dept_lcattr"].ToString(), table.Rows[0]["sort_no"].ToString(), table.Rows[0]["show_flag"].ToString(), table.Rows[0]["attr"].ToString(), table.Rows[0]["input_code"].ToString(), table.Rows[0]["account_dept_code"].ToString(), table.Rows[0]["account_dept_name"].ToString(), table.Rows[0]["dept_code_second"].ToString(), table.Rows[0]["dept_name_second"].ToString(), table.Rows[0]["out_or_in"].ToString(), table.Rows[0]["dept_group"].ToString());
                 return dept;
             }
             else
             {
                 return null;
             }
         }
    }
}
