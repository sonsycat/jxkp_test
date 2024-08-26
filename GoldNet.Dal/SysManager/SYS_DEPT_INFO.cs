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
    public class SYS_DEPT_INFO
    {
        /// <summary>
        /// 根据年月获得科室的快照
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetDeptInfo(string year, string month)
        {
            //取本月的科室快照
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string sqlNowMonths = " select b.ID, b.dept_code, b.dept_name, a.input_code, b.p_dept_code, ";
            sqlNowMonths += " b.dept_type, b.dept_lcattr, b.dept_date, b.sort_no, b.show_flag, ";
            sqlNowMonths += " b.attr, b.account_dept_code, b.account_dept_name, b.dept_code_second, ";
            sqlNowMonths += " b.dept_name_second,b.p_dept_name ";
            sqlNowMonths += " from comm.sys_dept_info b,comm.sys_dept_dict a ";
            sqlNowMonths += " where a.dept_code(+) = b.dept_code and to_char(DEPT_SNAP_DATE,'yyyymm')=to_char(to_date('" + year + month + "01" + "','yyyy-mm-dd'),'yyyymm') order by nvl( b.p_dept_code,a.dept_code) || a.dept_code";
            DataSet dsNowMonths = OracleOledbBase.ExecuteDataSet(sqlNowMonths.ToString());
            return dsNowMonths.Tables[0];
//            if (dsNowMonths.Tables.Count == 0 || dsNowMonths.Tables[0].Rows.Count == 0)
//            {
//                //去上个月的科室快照
//                string sqlPreMonths = " select b.ID, b.dept_code, b.dept_name, a.input_code, b.p_dept_code, ";
//                sqlPreMonths += " b.dept_type, b.dept_lcattr, b.dept_date, b.sort_no, b.show_flag, ";
//                sqlPreMonths += " b.attr, b.account_dept_code, b.account_dept_name, b.dept_code_second, ";
//                sqlPreMonths += " b.dept_name_second,b.p_dept_name ";
//                sqlPreMonths += " from comm.sys_dept_info b,hisdata.dept_dict a ";
//                sqlPreMonths += " where a.dept_code(+) = b.dept_code and to_char(DEPT_SNAP_DATE,'yyyymm')=to_char(add_months(to_date('" + year + month + "01" + "','yyyy-mm-dd'),-1),'yyyymm')  order by nvl( b.p_dept_code,a.dept_code) || a.dept_code";
//                DataSet dsPreMonths = OracleOledbBase.ExecuteDataSet(sqlPreMonths.ToString());
//                if (dsPreMonths.Tables.Count == 0 || dsPreMonths.Tables[0].Rows.Count == 0)
//                {
//                    StringBuilder sqlDept = new StringBuilder();
//                    sqlDept.AppendFormat(@"SELECT b.ID, b.dept_code, b.dept_name, a.input_code, b.p_dept_code,
//                               b.dept_type, b.dept_lcattr, b.dept_date, b.sort_no, b.show_flag,
//                               b.attr, b.account_dept_code, b.account_dept_name, b.dept_code_second,
//                               b.dept_name_second,b.p_dept_name
//                          FROM {0}.dept_dict a, {1}.sys_dept_dict b
//                         WHERE a.dept_code = b.dept_code(+)  order by nvl(b.p_dept_code,a.dept_code) || a.dept_code ", DataUser.HISDATA, DataUser.COMM);
//                    DataSet dsDept = OracleOledbBase.ExecuteDataSet(sqlDept.ToString());
//                    if (dsPreMonths.Tables.Count > 0)
//                    {
//                        return dsDept.Tables[0];
//                    }
//                    else
//                    {
//                        return null;
//                    }
//                }
//                else
//                {
//                    return dsPreMonths.Tables[0];
//                }
//            }
//            else
//            {
//                return dsNowMonths.Tables[0];
//            }
        }
        public void DeptCollateInfo(string year,string month)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }

            string sql = @" SELECT b.ID, b.dept_code, b.dept_name, b.input_code, b.p_dept_code,
                              b.dept_type, b.dept_lcattr, b.dept_date, b.sort_no, b.show_flag,
                              b.attr, b.account_dept_code, b.account_dept_name, b.dept_code_second,
                          b.dept_name_second,b.p_dept_name
                   FROM comm.sys_dept_dict b where SHOW_FLAG=0";
            DataTable dt = OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];

            string delsql = "delete from  comm.sys_dept_info  where  DEPT_SNAP_DATE =TO_DATE ('" + year + month + "01" + "', 'yyyymmdd') ";
            MyLists listtable = new MyLists();
            //删除科室快照
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            int ID = OracleOledbBase.GetMaxID("id", DataUser.COMM + ".sys_dept_info");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into {0}.sys_dept_info(", DataUser.COMM);
            sb.Append("ID,DEPT_CODE,DEPT_NAME,P_DEPT_CODE,DEPT_TYPE,DEPT_LCATTR,DEPT_DATE,SORT_NO,SHOW_FLAG,ATTR,INPUT_CODE,ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME,DEPT_CODE_SECOND,DEPT_NAME_SECOND,P_DEPT_NAME,DEPT_SNAP_DATE)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append(" select ");
                sb.Append((ID + i) + " as ID,");
                sb.Append("'" + dt.Rows[i]["DEPT_CODE"] + "' as DEPT_CODE,");
                sb.Append("'" + dt.Rows[i]["DEPT_NAME"] + "' as DEPT_NAME,");
                sb.Append("'" + dt.Rows[i]["P_DEPT_CODE"] + "' as P_DEPT_CODE,");
                sb.Append("'" + dt.Rows[i]["DEPT_TYPE"] + "' as DEPT_TYPE,");
                sb.Append("'" + dt.Rows[i]["DEPT_LCATTR"] + "' as DEPT_LCATTR,");
                sb.Append("'" + dt.Rows[i]["DEPT_DATE"] + "' as DEPT_DATE,");
                sb.Append("'" + dt.Rows[i]["SORT_NO"] + "' as SORT_NO,");
                sb.Append("'" + dt.Rows[i]["SHOW_FLAG"] + "' as SHOW_FLAG,");
                sb.Append("'" + dt.Rows[i]["ATTR"] + "' as ATTR,");
                sb.Append("'" + dt.Rows[i]["INPUT_CODE"] + "' as INPUT_CODE,");
                sb.Append("'" + dt.Rows[i]["ACCOUNT_DEPT_CODE"] + "' as ACCOUNT_DEPT_CODE,");
                sb.Append("'" + dt.Rows[i]["ACCOUNT_DEPT_NAME"] + "' as ACCOUNT_DEPT_NAME,");
                sb.Append("'" + dt.Rows[i]["DEPT_CODE_SECOND"] + "' as DEPT_CODE_SECOND,");
                sb.Append("'" + dt.Rows[i]["DEPT_NAME_SECOND"] + "' as DEPT_NAME_SECOND,");
                sb.Append("'" + dt.Rows[i]["P_DEPT_NAME"] + "' as P_DEPT_NAME,");
                sb.Append("to_date('" + year + month + "01" + "','yyyymmdd') as DEPT_SNAP_DATE");
                sb.Append(" from dual union all");
            }
            sb.Remove(sb.Length - 10, 10);
            //添加科室快照
            List listcenterdetail = new List();
            listcenterdetail.StrSql = sb.ToString();
            listcenterdetail.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdetail);
            //执行事务
            OracleOledbBase.ExecuteTranslist(listtable);
          

        }
        /// <summary>
        /// 保存科室快照
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public void SaveDeptInfo(Dictionary<string, string>[] rows, string year, string month)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string delsql = "delete from  comm.sys_dept_info  where  DEPT_SNAP_DATE =TO_DATE ('" + year + month + "01" + "', 'yyyymmdd') ";
            MyLists listtable = new MyLists();
            //删除科室快照
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            int ID = OracleOledbBase.GetMaxID("id", DataUser.COMM + ".sys_dept_info");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into {0}.sys_dept_info(", DataUser.COMM);
            sb.Append("ID,DEPT_CODE,DEPT_NAME,P_DEPT_CODE,DEPT_TYPE,DEPT_LCATTR,DEPT_DATE,SORT_NO,SHOW_FLAG,ATTR,INPUT_CODE,ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME,DEPT_CODE_SECOND,DEPT_NAME_SECOND,P_DEPT_NAME,DEPT_SNAP_DATE)");
            for (int i = 0; i < rows.Length; i++)
            {
                sb.Append(" select ");
                sb.Append((ID + i) + " as ID,");
                sb.Append("'"+rows[i]["DEPT_CODE"] + "' as DEPT_CODE,");
                sb.Append("'" + rows[i]["DEPT_NAME"] + "' as DEPT_NAME,");
                sb.Append("'" + rows[i]["P_DEPT_CODE"] + "' as P_DEPT_CODE,");
                sb.Append("'" + rows[i]["DEPT_TYPE"] + "' as DEPT_TYPE,");
                sb.Append("'" + rows[i]["DEPT_LCATTR"] + "' as DEPT_LCATTR,");
                sb.Append("'" + rows[i]["DEPT_DATE"] + "' as DEPT_DATE,");
                sb.Append("'" + rows[i]["SORT_NO"] + "' as SORT_NO,");
                sb.Append("'" + rows[i]["SHOW_FLAG"] + "' as SHOW_FLAG,");
                sb.Append("'" + rows[i]["ATTR"] + "' as ATTR,");
                sb.Append("'" + rows[i]["INPUT_CODE"] + "' as INPUT_CODE,");
                sb.Append("'" + rows[i]["ACCOUNT_DEPT_CODE"] + "' as ACCOUNT_DEPT_CODE,");
                sb.Append("'" + rows[i]["ACCOUNT_DEPT_NAME"] + "' as ACCOUNT_DEPT_NAME,");
                sb.Append("'" + rows[i]["DEPT_CODE_SECOND"] + "' as DEPT_CODE_SECOND,");
                sb.Append("'" + rows[i]["DEPT_NAME_SECOND"] + "' as DEPT_NAME_SECOND,");
                sb.Append("'" + rows[i]["P_DEPT_NAME"] + "' as P_DEPT_NAME,");
                sb.Append("to_date('"+year + month + "01" + "','yyyymmdd') as DEPT_SNAP_DATE");
                sb.Append(" from dual union all");
            }
            sb.Remove(sb.Length - 10, 10);
            //添加科室快照
            List listcenterdetail = new List();
            listcenterdetail.StrSql = sb.ToString();
            listcenterdetail.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdetail);
            //执行事务
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        /// <summary>
        /// 删除科室对照信息
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            string sql = "delete from comm.sys_dept_info where ID=" + id;
            OracleOledbBase.ExecuteNonQuery(sql);

        }
        /// <summary>
        /// 根据ID获得科室对照信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetDeptInfoById(string id)
        {
            string sql = " select ID,DEPT_CODE,DEPT_NAME,P_DEPT_CODE,DEPT_TYPE,DEPT_LCATTR,DEPT_DATE,SORT_NO,";
            sql += " SHOW_FLAG,ATTR,INPUT_CODE,ACCOUNT_DEPT_CODE,ACCOUNT_DEPT_NAME,DEPT_CODE_SECOND,DEPT_NAME_SECOND,P_DEPT_NAME,DEPT_SNAP_DATE ";
            sql += " from comm.sys_dept_info where ID="+id+"";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 更新一个科室对照
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sysDept"></param>
        public void Update(string id,GoldNet.Model.SYS_DEPT_DICT sysDept)
        {
            string sql = " update comm.sys_dept_info set DEPT_CODE='" + sysDept.DEPT_CODE + "'";
            sql += ",DEPT_NAME='" + sysDept.DEPT_NAME + "'";
            sql += ",P_DEPT_CODE='" + sysDept.P_DEPT_CODE + "'";
            sql += ",P_DEPT_NAME='" + sysDept.P_DEPT_Name + "'";
            sql += ",DEPT_TYPE='" + sysDept.DEPT_TYPE + "'";
            sql += ",ACCOUNT_DEPT_CODE='" + sysDept.ACCOUNT_DEPT_CODE + "'";
            sql += ",ACCOUNT_DEPT_NAME='" + sysDept.ACCOUNT_DEPT_NAME + "'";
            sql += ",DEPT_CODE_SECOND='" + sysDept.DEPT_CODE_SECOND + "'";
            sql += ",DEPT_NAME_SECOND='" + sysDept.DEPT_NAME_SECOND + "'";
            sql += ",DEPT_LCATTR='" + sysDept.DEPT_LCATTR+"'";
            sql += ",SORT_NO='" + sysDept.SORT_NO + "'";
            sql += ",SHOW_FLAG='" + sysDept.SHOW_FLAG + "'";
            sql += ",ATTR='" + sysDept.ATTR + "'";
            sql += " where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);

        }
        /// <summary>
        /// 新建一个科室对照
        /// </summary>
        /// <param name="sysDept"></param>
        public void Insert(GoldNet.Model.SYS_DEPT_DICT sysDept,string year,string month)
        {
            if(month.Length==1)
            {
                month="0"+month;
            }
            int id = OracleOledbBase.GetMaxID("ID", "comm.sys_dept_info");
            string sql = " insert into comm.sys_dept_info (";
            sql += " DEPT_CODE,DEPT_NAME,P_DEPT_CODE,P_DEPT_NAME,DEPT_TYPE,ACCOUNT_DEPT_CODE,";
            sql += " ACCOUNT_DEPT_NAME,DEPT_CODE_SECOND,DEPT_NAME_SECOND,DEPT_LCATTR,SORT_NO,SHOW_FLAG,ID,DEPT_SNAP_DATE)";
            sql += " values ";
            sql += " ('" + sysDept.DEPT_CODE + "','" + sysDept.DEPT_NAME + "','" + sysDept.P_DEPT_CODE + "','" + sysDept.P_DEPT_Name + "' ";
            sql += " ,'" + sysDept.DEPT_TYPE + "','" + sysDept.ACCOUNT_DEPT_CODE + "','" + sysDept.ACCOUNT_DEPT_NAME + "','" + sysDept.DEPT_CODE_SECOND + "' ";
            sql += " ,'" + sysDept.DEPT_NAME_SECOND + "','" + sysDept.DEPT_LCATTR + "','" + sysDept.SORT_NO + "','" + sysDept.SHOW_FLAG + "' ";
            sql += " ,'"+id+"',to_date('"+year+month+"01"+"','yyyymmdd')) ";
            OracleOledbBase.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 检查科室快照是否存在
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckDept(string deptCode,string year,string month)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string sql = " select * from comm.sys_dept_info where DEPT_CODE='" + deptCode + "' and DEPT_SNAP_DATE=to_date('" + year + month + "01" + "','yyyymmdd')";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
