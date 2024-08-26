using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
using System.Data.SqlClient;
using System.Data.OleDb;
namespace Goldnet.Dal
{
    public class AccountDeptType
    {
        public bool SaveContrastDeptInfo(Dictionary<string, string>[] rows)
        {

            string delsql = "delete from  CBHS.CBHS_DEPT_CONTRAST  ";
            MyLists listtable = new MyLists();
            //删除科室类别
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < rows.Length; i++)
            {
                string OTHER_COST_DEPTCODE = "";
                string OTHER_COST_DEPTNAME = "";

                if (rows[i]["OTHER_COST_DEPTCODE"] == null)
                {
                    OTHER_COST_DEPTCODE = "";
                }
                else
                {
                    OTHER_COST_DEPTCODE = rows[i]["OTHER_COST_DEPTCODE"].ToString();
                }
                if (rows[i]["OTHER_COST_DEPTNAME"] == null)
                {
                    OTHER_COST_DEPTNAME = "";
                }
                else
                {
                    OTHER_COST_DEPTNAME = rows[i]["OTHER_COST_DEPTNAME"].ToString();
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"insert into CBHS.CBHS_DEPT_CONTRAST (HIS_COST_DEPTCODE,HIS_COST_DEPTNAME,OTHER_COST_DEPTCODE,OTHER_COST_DEPTNAME) values ('" + rows[i]["DEPT_CODE"].ToString() + "','" + rows[i]["DEPT_NAME"].ToString() + "','" + OTHER_COST_DEPTCODE.ToString() + "','" + OTHER_COST_DEPTNAME.ToString() + "')", DataUser.CBHS);
                OleDbParameter[] parameteradd = { };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            //for (int i = 0; i < rows.Length; i++)
            //{
            //    string OTHER_COST_DEPTCODE = "";
            //    string OTHER_COST_DEPTNAME = "";
            //    StringBuilder isql = new StringBuilder();
            //    isql.Append(" insert into CBHS.CBHS_DEPT_CONTRAST (HIS_COST_DEPTCODE,HIS_COST_DEPTNAME,OTHER_COST_DEPTCODE,OTHER_COST_DEPTNAME) values (");
            //    isql.Append("'" + rows[i]["DEPT_CODE"].ToString() + "'");
            //    isql.Append(",");
            //    isql.Append("'" + rows[i]["DEPT_NAME"].ToString() + "'");
            //    isql.Append(",");

            //    if (rows[i]["OTHER_COST_DEPTCODE"] == null)
            //    {
            //        OTHER_COST_DEPTCODE = "";
            //    }
            //    else
            //    {
            //        OTHER_COST_DEPTCODE = rows[i]["OTHER_COST_DEPTCODE"].ToString();
            //    }
            //    if (rows[i]["OTHER_COST_DEPTNAME"] == null)
            //    {
            //        OTHER_COST_DEPTNAME = "";
            //    }
            //    else
            //    {
            //        OTHER_COST_DEPTNAME = rows[i]["OTHER_COST_DEPTNAME"].ToString();
            //    }
            //    isql.Append("" + rows[i]["OTHER_COST_DEPTCODE"].ToString() + "");
            //    isql.Append(",");
            //    isql.Append("" + rows[i]["OTHER_COST_DEPTNAME"].ToString() + "");
            //    isql.Append(") ");

            //    //添加科室类别
            //    List listcenterdetail = new List();
            //    listcenterdetail.StrSql = isql.ToString();
            //    listcenterdetail.Parameters = new OleDbParameter[] { };
            //    listtable.Add(listcenterdetail);
            //}


            try
            {
                OracleOledbBase.ExecuteTranslist(listtable);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// his对照
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns></returns>
        public DataTable getContrastDept()
        {

            //本月是否有已经设置的科室类别
            string sqlDeptType = " select   B.DEPT_CODE,B.DEPT_name, OTHER_COST_DEPTCODE,OTHER_COST_DEPTNAME    ";
            sqlDeptType += "from  ";
            sqlDeptType += "(select * from CBHS.CBHS_DEPT_CONTRAST ) a , ";
            sqlDeptType += "(select * from COMM.SYS_DEPT_DICT ) b ";
            sqlDeptType += "where A.HIS_COST_DEPTCODE(+)=B.DEPT_CODE  order by b.SORT_NO,nvl(P_DEPT_CODE,b.DEPT_CODE) || b.DEPT_CODE";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlDeptType.ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];

            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 查找科室类别信息，如果有设置好的科室的类别则显示出来，
        /// 如果没有的话，则取科室字典来构成科室类别设置
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns></returns>
        public DataTable getDeptInfo(string year, string month)
        {
            string st_date = BuildDate(year, month);
            //本月是否有已经设置的科室类别
            string sqlDeptType = " select  b.dept_code AS DEPT_CODE,b.dept_name AS DEPT_NAME, a.DEPT_PERSON_COUNT,a.DEPT_AREA    ";
            sqlDeptType += "from  ";
            sqlDeptType += "(select * from CBHS.CBHS_DEPT_INFO where DEPT_SNAP_DATE=to_date('" + st_date + "','yyyymmdd')) a , ";
            sqlDeptType += "(select * from COMM.SYS_DEPT_INFO b where DEPT_SNAP_DATE=to_date('" + st_date + "','yyyymmdd') and ATTR='是') b ";
            sqlDeptType += "where A.DEPT_CODE(+)=B.DEPT_CODE  order by b.SORT_NO,nvl(P_DEPT_CODE,b.DEPT_CODE) || b.DEPT_CODE";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlDeptType.ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];

            }
            else
            {
                return null;
            }
        }

        public bool SaveDeptInfo(Dictionary<string, string>[] rows, string year, string month)
        {
            if (year == "" || month == "")
            {
                return false;
            }
            string date = BuildDate(year, month);
            string delsql = "delete from  CBHS.CBHS_DEPT_INFO  where  DEPT_SNAP_DATE =TO_DATE ('" + date + "', 'yyyymmdd') ";
            MyLists listtable = new MyLists();
            //删除科室类别
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);


            for (int i = 0; i < rows.Length; i++)
            {
                decimal DEPT_PERSON_COUNT = 0;
                decimal DEPT_AREA = 0;
                StringBuilder isql = new StringBuilder();
                isql.Append(" insert into CBHS.CBHS_DEPT_INFO (DEPT_CODE,DEPT_NAME,DEPT_SNAP_DATE,DEPT_PERSON_COUNT,DEPT_AREA) values (");
                isql.Append("'" + rows[i]["DEPT_CODE"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["DEPT_NAME"].ToString() + "'");
                isql.Append(",");
                isql.Append("to_date('" + date + "','yyyymmdd')");
                isql.Append(",");
                if (rows[i]["DEPT_PERSON_COUNT"] == null)
                {
                    DEPT_PERSON_COUNT = 0;
                }
                else
                {
                    DEPT_PERSON_COUNT = decimal.Parse(rows[i]["DEPT_PERSON_COUNT"].ToString());
                }
                if (rows[i]["DEPT_AREA"] == null)
                {
                    DEPT_AREA = 0;
                }
                else
                {
                    DEPT_AREA = decimal.Parse(rows[i]["DEPT_AREA"].ToString());
                }
                isql.Append("" + DEPT_PERSON_COUNT + "");
                isql.Append(",");
                isql.Append("" + DEPT_AREA + "");
                isql.Append(") ");

                //添加科室类别
                List listcenterdetail = new List();
                listcenterdetail.StrSql = isql.ToString();
                listcenterdetail.Parameters = new OleDbParameter[] { };
                listtable.Add(listcenterdetail);
            }


            try
            {
                OracleOledbBase.ExecuteTranslist(listtable);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public DataTable getDeptType(string year, string month)
        {

            if (year == "" || month == "")
            {
                return BuildDeptType();
            }
            string st_date = BuildDate(year, month);
            //本月是否有已经设置的科室类别
            string sqlDeptType = " select b.dept_code as deptcode,b.dept_name as deptname,nvl(A.DEPT_TYPE,'10001') as type,nvl(A.DEPT_TYPE,'10001') as typeSelect ";
            sqlDeptType += "from  ";
            sqlDeptType += "(select * from PERFORMANCE.SET_ACCOUNTDEPTTYPE where st_date=to_date('" + st_date + "','yyyymmdd')) a , ";
            sqlDeptType += "(select * from COMM.SYS_DEPT_INFO b where DEPT_SNAP_DATE=to_date('" + st_date + "','yyyymmdd') and ATTR='是') b ";
            sqlDeptType += "where A.DEPT_CODE(+)=B.DEPT_CODE  order by b.SORT_NO,nvl(P_DEPT_CODE,b.DEPT_CODE) || b.DEPT_CODE";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlDeptType.ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];

            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 对已经查询出来的科室类别添加科室字典表有但是科室类别没有的科室
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void AddNewDept(DataSet ds, string date, bool lastMonth)
        {
            string sqlAddDept = "SELECT b.dept_code AS deptcode, b.dept_name AS deptname,'10001'  as type,'10001' as typeSelect ";
            if (lastMonth)
            {
                sqlAddDept += "  FROM (select * from PERFORMANCE.set_accountdepttype where st_date =  add_months(TO_DATE ('" + date + "', 'yyyymmdd'),-1)) a,  ";
            }
            else
            {
                sqlAddDept += "  FROM (select * from PERFORMANCE.set_accountdepttype where st_date =TO_DATE ('" + date + "', 'yyyymmdd')) a,  ";
            }
            sqlAddDept += "       (select * from comm.sys_dept_dict where attr='是') b ";
            sqlAddDept += " WHERE a.dept_code(+) = b.dept_code and a.dept_code is null ";
            DataSet dsAddDept = OracleOledbBase.ExecuteDataSet(sqlAddDept.ToString());
            if (dsAddDept != null && dsAddDept.Tables.Count > 0)
            {
                for (int i = 0; i < dsAddDept.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["deptcode"] = dsAddDept.Tables[0].Rows[i]["deptcode"];
                    dr["deptname"] = dsAddDept.Tables[0].Rows[i]["deptname"];
                    dr["type"] = dsAddDept.Tables[0].Rows[i]["type"];
                    dr["typeSelect"] = dsAddDept.Tables[0].Rows[i]["typeSelect"];
                    ds.Tables[0].Rows.Add(dr);
                }
            }
        }
        /// <summary>
        /// 组合科室核算类别设置
        /// </summary>
        /// <returns></returns>
        public DataTable BuildDeptType()
        {
            DataTable dt = new DataTable();
            DataColumn dcDEPTCODE = new DataColumn("DEPTCODE");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            DataColumn dcTYPE = new DataColumn("TYPE");
            DataColumn dcTYPESELECT = new DataColumn("TYPESELECT");
            dt.Columns.AddRange(new DataColumn[] { dcDEPTCODE, dcDEPTNAME, dcTYPE, dcTYPESELECT });
            return dt;
        }
        /// <summary>
        /// 组合生成类别
        /// </summary>
        /// <returns></returns>
        private DataTable BuildType()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID");
            DataColumn dcNAME = new DataColumn("NAME");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcNAME });
            return dt;
        }
        private string BuildDate(string year, string month)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            return year + "" + month + "" + "01";
        }
        /// <summary>
        /// 获得核算科室类别
        /// </summary>
        /// <returns></returns>
        public DataTable getType()
        {
            string sql = "select TYPE_CODE id,TYPE_NAME name from PERFORMANCE.SET_ACCOUNTTYPE order by SORT";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildType();
            }
        }
        /// <summary>
        /// 删除选中时间段内的核算科室类别以及添加核算科室类别
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool SaveDeptType(Dictionary<string, string>[] rows, string year, string month)
        {
            if (year == "" || month == "")
            {
                return false;
            }
            string date = BuildDate(year, month);
            string delsql = "delete from  PERFORMANCE.SET_ACCOUNTDEPTTYPE  where  st_date =TO_DATE ('" + date + "', 'yyyymmdd') ";
            MyLists listtable = new MyLists();
            //删除科室类别
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);


            for (int i = 0; i < rows.Length; i++)
            {
                StringBuilder isql = new StringBuilder();
                isql.Append(" insert into PERFORMANCE.SET_ACCOUNTDEPTTYPE (DEPT_CODE,DEPT_TYPE,ST_DATE) values (");
                isql.Append("'" + rows[i]["DEPTCODE"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["TYPESELECT"].ToString() + "'");
                isql.Append(",");
                isql.Append("to_date('" + date + "','yyyymmdd')");
                isql.Append(") ");

                //添加科室类别
                List listcenterdetail = new List();
                listcenterdetail.StrSql = isql.ToString();
                listcenterdetail.Parameters = new OleDbParameter[] { };
                listtable.Add(listcenterdetail);
            }


            try
            {
                OracleOledbBase.ExecuteTranslist(listtable);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
