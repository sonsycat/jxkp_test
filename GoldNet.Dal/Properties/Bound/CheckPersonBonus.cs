using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class CheckPersonBonus
    {
        /// <summary>
        /// 获得所在月份科室人的信息
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetCheckPersonBounusDaysList(string years, string months,string deptid,string staffname)
        {
            string sql = "SELECT a.years, a.months, a.DEPT_ID DEPTID,a.dept_name deptname, a.staff_name staffname, a.isbonus, a.days,a.bonusmodulus,PERSONSMODULUS,SUBSIDYMODULUS,(a.bonusmodulus+PERSONSMODULUS+SUBSIDYMODULUS) SUMMODULUS,a.STAFF_ID AS STAFFID";
            sql += " FROM PERFORMANCE.SET_CHECKBONUSDAYS a   WHERE  years = '" + years + "' AND months = '" + months + "'";
            if (!deptid.Equals(string.Empty))
            {
                sql += " and DEPT_ID='" + deptid + "'";
            }
            if (!staffname.Equals(string.Empty))
            {
                sql += " and a.staff_name=? ";
            }
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            sql += "  and DEPT_ID in (select DEPT_CODE from PERFORMANCE.SET_ACCOUNTDEPTTYPE where st_date=to_date('" + years + months + "01" + "','yyyymmdd') and DEPT_TYPE not  in ('10001','20001')) order by DEPTID";
            DataSet ds=new DataSet();
            if (!staffname.Equals(string.Empty))
            {
                OleDbParameter[] parameters = { new OleDbParameter("staff_name", staffname) };
                 ds = OracleOledbBase.ExecuteDataSet(sql.ToString(), parameters);
            }
            else
            {
                 ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            }
           
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildCheckPersonBounusDaysList();
            }
        }

        /// <summary>
        /// 获得所在月份没有录入进来但是人力资源中有的科室的人
        /// </summary>
        /// <param name="yeas"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetRLZYCheckPersonBounusList(string years, string months,string deptid,string staffname)
        {
            //            //考勤类型
            //            StringBuilder str1 = new StringBuilder();
            //            str1.AppendFormat(@"SELECT ATTENDANCE_CODE, 
            //                                       ATTENDANCE_NAME,
            //                                       ATTENDANCE_CODE kk 
            //                                  FROM RLZY.ATTENDANCE_NAME_DICT a 
            //                                UNION ALL
            //                                SELECT REPLACE(to_char(wmsys.wm_concat(case when ATTENDANCE_CODE='A01' THEN ATTENDANCE_CODE ELSE CASE WHEN IS_JJ='1' THEN '+'||ATTENDANCE_CODE ELSE '-'||ATTENDANCE_CODE END END)),',','') ATTENDANCE_CODE,
            //                                       '实际出勤' ATTENDANCE_NAME,
            //                                       'A0111' KK
            //                                  FROM (SELECT A.* FROM RLZY.ATTENDANCE_NAME_DICT a WHERE IS_JJ <>2 order by ATTENDANCE_CODE)  ORDER BY KK", "");
            //            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];

            //            string month = "";
            //            if (months.Length == 1)
            //            {
            //                month = "0" + months;
            //            }
            //            else
            //            {
            //                month = months;
            //            }
            //            string sql = "select '" + years + "' as years,'" + months + "' as months ,";
            //            sql += " B.ACCOUNT_DEPT_CODE deptid, B.ACCOUNT_DEPT_NAME deptname,NAME as staffname,1 as ISBONUS,";

            //            for (int i = 0; i < table.Rows.Count; i++)
            //            {
            //                if (table.Rows[i]["ATTENDANCE_NAME"].ToString() == "实际出勤")
            //                {
            //                    sql += " " + table.Rows[i]["ATTENDANCE_CODE"].ToString() + " AS DAYS,";
            //                }
            //            }

            //            sql += " 0 as BONUSMODULUS,0 as PERSONSMODULUS,0 as SUBSIDYMODULUS,A.STAFF_ID as STAFFID from RLZY.NEW_STAFF_INFO A";
            //            sql += " ,(SELECT dept_code,ACCOUNT_DEPT_NAME,ACCOUNT_DEPT_CODE  ";
            //            sql += " FROM comm.sys_dept_info  ";
            //            sql += " WHERE DEPT_SNAP_DATE= to_date('" + years + "'||lpad(" + months + ",2,0)||'01','yyyymmdd')   ";

            //            if (!deptid.Equals(string.Empty))
            //            {
            //                sql += " and account_dept_code = '" + deptid + "'";
            //            }
            //            sql += " ) b, ( SELECT   year_month,dept_code,staff_id AS emp_no, ";

            //            for (int i = 0; i < table.Rows.Count; i++)
            //            {
            //                if (table.Rows[i]["ATTENDANCE_NAME"].ToString() != "实际出勤")
            //                {
            //                    sql += " SUM(CASE WHEN attendance_code = '" + table.Rows[i]["ATTENDANCE_CODE"].ToString() + "' THEN ATTENDANCE_VALUE ELSE 0 END) " + table.Rows[i]["ATTENDANCE_CODE"].ToString() + ",";
            //                }
            //            }

            //            sql += " staff_id AS STAFF_ID FROM   RLZY.QU_ATTENDANCE_DEPT GROUP BY   year_month, dept_code, staff_id) c  ";

            //            sql += " WHERE c.dept_code = b.dept_code AND a.staff_id = c.staff_id  ";
            //            sql += " AND a.add_mark = 1  AND TRUNC (c.year_month, 'mm') = TO_DATE ('" + years + month + "01" + "', 'yyyymmdd')";
            //            if (!staffname.Equals(string.Empty))
            //            {
            //                sql += " and name=? ";
            //            }
            //            sql += " minus ";
            //            sql += " SELECT years,months,a.DEPT_ID as deptid,a.dept_name deptname, a.staff_name staffname,1 as ISBONUS,0 as DAYS,0 as BONUSMODULUS,0 as PERSONSMODULUS, 0 as SUBSIDYMODULUS,A.STAFF_ID as STAFFID";
            //            sql += "   FROM PERFORMANCE.SET_CHECKBONUSDAYS a ";
            //            sql += "  WHERE  years = '" + years + "' AND months = '" + months + "' ";
            //            if (!deptid.Equals(string.Empty))
            //            {
            //                sql += " and DEPT_ID='" + deptid + "'";
            //            }

            //            DataSet ds = new DataSet();
            //            if (!staffname.Equals(string.Empty))
            //            {
            //                OleDbParameter[] parameters = { new OleDbParameter("name", staffname), new OleDbParameter("staff_name", staffname) };
            //                ds = OracleOledbBase.ExecuteDataSet(sql.ToString(), parameters);
            //            }
            //            else
            //            {
            //                ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            //            }
            //            //
            //            //DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            DataSet ds = new DataSet();

            string month = months;

            if (month.Length < 2)
            {
                month = "0" + month;
            }

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT   '{0}' AS years,
         '{4}' AS months,
         B.ACCOUNT_DEPT_CODE deptid,
         B.ACCOUNT_DEPT_NAME deptname,
         NAME AS staffname,
         A.STAFF_ID AS STAFFID,1 ISBONUS
  FROM   RLZY.NEW_STAFF_INFO A,
         (SELECT   dept_code, ACCOUNT_DEPT_NAME, ACCOUNT_DEPT_CODE
            FROM   comm.sys_dept_info
           WHERE   TO_CHAR (DEPT_SNAP_DATE, 'yyyymm') = '{2}'
                   AND account_dept_code = '{3}') b
 WHERE   a.DEPT_CODE = b.DEPT_CODE
 MINUS
 SELECT   years,
         months,
         a.DEPT_ID AS deptid,
         a.dept_name deptname,
         a.staff_name staffname,
         A.STAFF_ID AS STAFFID,ISBONUS
  FROM   PERFORMANCE.SET_CHECKBONUSDAYS a
 WHERE   years = '{0}' AND months = '{4}' AND DEPT_ID = '{3}'
", years, month, years + month, deptid, months);

            ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildRLZY();
            }
        }

        /// <summary>
        /// 科室信息表结构
        /// </summary>
        /// <returns></returns>
        private DataTable BuildCheckPersonBonusList()
        {
            DataTable dt = new DataTable();
            DataColumn dcYEARS = new DataColumn("YEARS");
            DataColumn dcMONTHS = new DataColumn("MONTHS");
            DataColumn dcYEAR_MONTH = new DataColumn("YEAR_MONTH");
            DataColumn dcYEARMONTH = new DataColumn("YEARMONTH");
            DataColumn dcINPUUDATE = new DataColumn("INPUTDATE");
            DataColumn dcINPUTER = new DataColumn("INPUTER");
            dt.Columns.AddRange(new DataColumn[] { dcYEARS, dcMONTHS, dcYEAR_MONTH, dcYEARMONTH, dcINPUUDATE, dcINPUTER });
            return dt;
        }
        public DataTable BuildCheckPersonBounusDaysList()
        {
            DataTable dt = new DataTable();
            DataColumn dcYEARS = new DataColumn("YEARS");
            DataColumn dcMONTHS = new DataColumn("MONTHS");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            DataColumn dcSTAFFNAME = new DataColumn("STAFFNAME");
            DataColumn dcISBONUS = new DataColumn("ISBONUS");
            DataColumn dcDAYS = new DataColumn("DAYS");
            DataColumn dcBONUSMODULUS = new DataColumn("BONUSMODULUS");
            DataColumn dcPERSONSMODULUS = new DataColumn("PERSONSMODULUS");
            DataColumn dcBSUBSIDYMODULUS = new DataColumn("SUBSIDYMODULUS");
            dt.Columns.AddRange(new DataColumn[] { dcYEARS, dcMONTHS, dcDEPTNAME, dcSTAFFNAME, dcISBONUS, dcDAYS, dcBONUSMODULUS, dcPERSONSMODULUS, dcBSUBSIDYMODULUS });
            return dt;
        }
        public DataTable BuildCheckDept()
        {
            DataTable dt = new DataTable();
            DataColumn dcDeptId = new DataColumn("DEPTID");
            DataColumn dcDeptName = new DataColumn("DEPTNAME");
            dt.Columns.AddRange(new DataColumn[] { dcDeptId, dcDeptName });
            return dt;
        }
        public DataTable BuildRLZY()
        {
            DataTable dt = new DataTable();
            DataColumn dcSTAFFID = new DataColumn("STAFFID");
            DataColumn dcSTAFFNAME = new DataColumn("STAFFNAME");
            DataColumn dcDEPTCODE = new DataColumn("DEPTCODE");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            dt.Columns.AddRange(new DataColumn[] { dcSTAFFID, dcSTAFFNAME, dcDEPTCODE, dcDEPTNAME });
            return dt;
        }

        /// <summary>
        /// 删除选中时间段内的科室的数据以及再次添加平均奖科室人数数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void SaveCheckPersonBonusDays(Dictionary<string, string>[] rows, string year, string month, User user,string deptid,string deptname)
        {
            StringBuilder delsql = new StringBuilder();
            delsql.Append("delete from  performance.SET_CHECKBONUSDAYS  where  YEARS='" + year + "' and MONTHS='" + month + "' ");
            if (!deptid.Equals(""))
            {
                delsql.Append(" and DEPT_ID='" + deptid + "'");
            }
            //string delsql = "delete from  performance.SET_CHECKBONUSDAYS  where  YEARS='" + year + "' and MONTHS='" + month + "'";
            MyLists listtable = new MyLists();
            //删除科室类别
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < rows.Length; i++)
            {
                string deptnames = deptname == "" ? rows[i]["DEPTNAME"].ToString() : deptname;
                StringBuilder isql = new StringBuilder();
                isql.Append(" insert into performance.SET_CHECKBONUSDAYS (YEARS,MONTHS,DEPT_ID,DEPT_NAME,STAFF_NAME,ISBONUS,DAYS,INPUT_DATE,INPUTER,BONUSMODULUS,PERSONSMODULUS,SUBSIDYMODULUS,STAFF_ID) values (");
                isql.Append("'" + year + "'");
                isql.Append(",");
                isql.Append("'" + month + "'");
                isql.Append(",");
                isql.Append("'" + deptid + "'");
                isql.Append(",");
                isql.Append("'" + deptnames + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["STAFFNAME"].ToString() + "'");
                isql.Append(",");
                if (rows[i]["ISBONUS"].ToString() == "True")
                {
                    isql.Append("1");
                }
                else if (rows[i]["ISBONUS"].ToString() == "False")
                {
                    isql.Append("0");
                }
                else
                {
                    isql.Append("" + Convert.ToInt32(rows[i]["ISBONUS"].ToString()) + "");
                }
                isql.Append(",");
                isql.Append("'" + rows[i]["DAYS"].ToString() + "'");
                isql.Append(",");
                isql.Append("to_date('" + System.DateTime.Now.ToString("yyyyMMdd") + "','yyyymmdd')");
                isql.Append(",");
                isql.Append("'" + user.UserName + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["BONUSMODULUS"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["PERSONSMODULUS"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["SUBSIDYMODULUS"].ToString() + "'");
                isql.Append(",");
                isql.Append("" + rows[i]["STAFFID"].ToString() + "");
                isql.Append(") ");
                //添加人数
                List listcenterdetail = new List();
                listcenterdetail.StrSql = isql.ToString();
                listcenterdetail.Parameters = new OleDbParameter[] { };
                listtable.Add(listcenterdetail);
            }
            //string usql = " update performance.SET_CHECKPERSONS set PERSONS=" + rows.Length + " where YEARS=" + year + " and MONTHS=" + month + " and DEPT_ID='" + deptid + "'";
            //List listusql = new List();
            //listusql.StrSql = usql.ToString();
            //listusql.Parameters = new OleDbParameter[] { };
            //listtable.Add(listusql);
            OracleOledbBase.ExecuteTranslist(listtable);

        }

        /// <summary>
        /// 删除选中时间段内的科室的数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void DeleteCheckPersonBonusDays(Dictionary<string, string>[] rows)
        {
            MyLists listtable = new MyLists();
            for (int i = 0; i < rows.Length; i++)
            {
                StringBuilder isql = new StringBuilder();
                isql.Append(" delete from  performance.SET_CHECKBONUSDAYS  where  YEARS='" + rows[i]["YEARS"].ToString() + "' and MONTHS='" + rows[i]["MONTHS"].ToString() + "' and");
                isql.Append("  DEPT_ID='" + rows[i]["DEPTID"].ToString() + "' and STAFF_NAME='" + rows[i]["STAFFNAME"].ToString() + "'");
                //添加平均奖人数
                List listcenterdetail = new List();
                listcenterdetail.StrSql = isql.ToString();
                listcenterdetail.Parameters = new OleDbParameter[] { };
                listtable.Add(listcenterdetail);
            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        /// <summary>
        /// 检查科室的人是否重名
        /// </summary>
        /// <param name="year"></param>
        /// <param name="months"></param>
        /// <param name="deptid"></param>
        /// <param name="deptname"></param>
        /// <param name="staffname"></param>
        /// <returns></returns>
        public bool CheckPersonByName(string year, string months, string deptid, string deptname, string staffname)
        {
            string sql = " select count(*) as cnt from performance.SET_CHECKBONUSDAYS where YEARS='" + year + "' and MONTHS='" + months + "' and DEPT_ID='" + deptid + "' and DEPT_NAME='" + deptname + "' and STAFF_NAME='" + staffname + "' ";
            string cnt = OracleOledbBase.ExecuteScalar(sql.ToString()).ToString();
            if (!cnt.Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 单独添加一个人
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptname"></param>
        /// <param name="staffname"></param>
        /// <param name="isbonus"></param>
        /// <param name="days"></param>
        /// <param name="bonusmodulus"></param>
        public void AddOnePerson(string year, string month, string deptid, string deptname, string staffname, int isbonus, int days, double bonusmodulus, User user, double PERSONSMODULUS,double SUBSIDYMODULUS)
        {
            string sql = "insert into performance.SET_CHECKBONUSDAYS ";
            sql += "(YEARS,MONTHS,DEPT_ID,DEPT_NAME,STAFF_NAME,ISBONUS,DAYS,INPUT_DATE,INPUTER,BONUSMODULUS,PERSONSMODULUS,SUBSIDYMODULUS) values (";
            sql += "'" + year + "',";
            sql += "'" + month + "',";
            sql += "'" + deptid + "',";
            sql += "'" + deptname + "',";
            sql += "'" + staffname + "',";
            sql += "'" + isbonus + "',";
            sql += "" + days + ",";
            sql += "to_date('" + System.DateTime.Now.ToString("yyyyMMdd") + "','yyyymmdd'),";
            sql += "'" + user.UserName + "',";

            sql += "" + bonusmodulus + ",";
            sql += "" + PERSONSMODULUS + ",";

            sql += "" + SUBSIDYMODULUS + ")";
            OracleOledbBase.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 获得人力资源中科室人员
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetCheckPersonAdd(string years, string months,string deptid)
        {
            string month = "";
            if (months.Length == 1)
            {
                month = "0" + months;
            }
            else
            {
                month = months;
            }
            string sql = "select '" + years + "' as years,'" + months + "' as months ,DEPT_CODE deptid,DEPT_NAME deptname,NAME as staffname,1 as ISBONUS,0 as DAYS,0 as BONUSMODULUS,0 as PERSONSMODULUS,0 as SUBSIDYMODULUS from RLZY.NEW_STAFF_INFO ";
            sql += " where DEPT_CODE in  (select DEPT_CODE from PERFORMANCE.SET_ACCOUNTDEPTTYPE where st_date=to_date('" + years + month + "01" + "','yyyymmdd') and DEPT_TYPE not in ('10001','20001')) and ADD_MARK=1 and DEPT_CODE='" + deptid + "' order by   DEPT_CODE";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildCheckPersonBounusDaysList();
            }
        }
    }
}
