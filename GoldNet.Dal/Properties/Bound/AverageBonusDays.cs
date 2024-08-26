using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class AverageBonusDays
    {
        /// <summary>
        /// 获得已经录入的平均奖的信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAverageBonusList()
        {
            string sql = " SELECT DISTINCT years, months, years || '-' || months AS year_month, ";
            sql += " years || '年' || months || '月' AS yearmonth, ";
            sql += " TO_CHAR (input_date, 'yyyy-MM-dd') AS inputdate, inputer";
            sql += " FROM performance.set_averagebonusdays ";
            sql += " order by to_number(years) desc,to_number(months) desc";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildAverageBonusList();
            }
        }

        /// <summary>
        /// 获得所在月份平均奖科室人的信息
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetAverageBounusDaysList(string years, string months, string staffname, string deptid)
        {
            string sql = "SELECT a.years, a.months, a.DEPT_ID DEPTID,a.dept_name deptname, a.staff_name staffname, a.isbonus, a.days,a.bonusmodulus,a.OBLIGATION,emp_no,STAFF_ID,NVL(qua_val,0) qua_val";
            sql += " FROM PERFORMANCE.set_averagebonusdays a   WHERE  years = '" + years + "' AND months = '" + months + "'";
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            if (!staffname.Equals(string.Empty))
            {
                sql += " and a.staff_name=? ";
            }
            if (deptid != "" && deptid != "00000")
            {
                sql += " and dept_id='" + deptid + "' ";
            }
            sql += "  and DEPT_ID in (select DEPT_CODE from PERFORMANCE.SET_ACCOUNTDEPTTYPE where st_date=to_date('" + years + months + "01" + "','yyyymmdd') and DEPT_TYPE in ('20001')) order by DEPTID,emp_no";

            DataSet ds = new DataSet();
            if (!staffname.Equals(string.Empty))
            {
                OleDbParameter[] parameters = { new OleDbParameter("staff_name", staffname) };
                ds = OracleOledbBase.ExecuteDataSet(sql.ToString(), parameters);
            }
            else
            {
                ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            }
            //
            //DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildAverageBonusDaysList();
            }
        }

        /// <summary>
        /// 获得人力资源中平均奖的科室人员
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetAverageBounusPerson(string years, string months, string deptid)
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
            string sql = "select '" + years + "' as years,'" + months + "' as months ,n.account_dept_code deptid,n.account_dept_name deptname,m.NAME as staffname,1 as ISBONUS,0 as DAYS,0 as BONUSMODULUS,0 as OBLIGATION from rlzy.new_staff_info m,comm.sys_dept_dict n  where 1=1 ";
            if (deptid != "" && deptid != "00000")
            {
                sql += " and n.account_dept_code ='" + deptid + "'";
            }
            sql += "  and M.DEPT_CODE=n.dept_code and n.account_DEPT_CODE in  (select a.DEPT_CODE from PERFORMANCE.SET_ACCOUNTDEPTTYPE a where st_date=to_date('" + years + month + "01" + "','yyyymmdd')  and a.DEPT_TYPE in ('20001') ) and ADD_MARK=1 order by   n.account_dept_code";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildAverageBonusDaysList();
            }
        }

        /// <summary>
        /// 获得所在月份没有录入进来但是人力资源中有的平均奖科室的人
        /// </summary>
        /// <param name="yeas"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetRLZYAveragePersonList(string years, string months, string staffname)
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
            //                                  FROM (SELECT A.* FROM RLZY.ATTENDANCE_NAME_DICT a WHERE IS_JJ <>2 order by ATTENDANCE_CODE)  ORDER BY KK","");
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
            //            sql +=  "b.account_DEPT_CODE deptid,b.account_DEPT_NAME deptname,a.NAME as staffname,1 as ISBONUS,";

            //            for (int i = 0; i < table.Rows.Count; i++)
            //            {
            //                if (table.Rows[i]["ATTENDANCE_NAME"].ToString() == "实际出勤")
            //                {
            //                    sql += " "+table.Rows[i]["ATTENDANCE_CODE"].ToString()+" AS DAYS,";
            //                }
            //            }

            //            sql += " 0 as BONUSMODULUS,";
            //            sql += " 0 AS OBLIGATION,";
            //            sql += " a.emp_no,";
            //            sql += " a.staff_id,";
            //            sql += " 0 AS QUA_VAL ";
            //            sql += " FROM rlzy.new_staff_info a,comm.sys_dept_dict b,( SELECT   year_month,dept_code,staff_id AS emp_no,";

            //            for (int i = 0; i < table.Rows.Count; i++)
            //            {
            //                if (table.Rows[i]["ATTENDANCE_NAME"].ToString() != "实际出勤")
            //                {
            //                    sql+= " SUM(CASE WHEN attendance_code = '"+table.Rows[i]["ATTENDANCE_CODE"].ToString()+"' THEN ATTENDANCE_VALUE ELSE 0 END) "+table.Rows[i]["ATTENDANCE_CODE"].ToString()+",";
            //                }
            //            }

            //            sql += "  staff_id AS STAFF_ID FROM   RLZY.QU_ATTENDANCE_DEPT GROUP BY   year_month, dept_code, staff_id) c ";
            //            sql += " WHERE c.dept_code = b.dept_code AND a.staff_id = c.staff_id AND b.account_dept_code in (SELECT dept_code from PERFORMANCE.SET_ACCOUNTDEPTTYPE where st_date=to_date('" + years + month + "01" + "','yyyymmdd') and DEPT_TYPE in ('20001')) AND TRUNC (c.year_month, 'mm') = TO_DATE ('" + years + month + "01', 'yyyymmdd')  ";

            //            if (!staffname.Equals(string.Empty))
            //            {
            //                sql += " and NAME=? ";
            //            }

            //            sql += " minus ";
            //            sql += " SELECT years,months,a.DEPT_ID as deptid,a.dept_name deptname, a.staff_name staffname,1 as ISBONUS,DAYS,0 as BONUSMODULUS,0 AS OBLIGATION,emp_no,staff_id,0 AS QUA_VAL ";
            //            sql += "   FROM PERFORMANCE.set_averagebonusdays a   ";
            //            sql += "  WHERE  years = '" + years + "' AND months = '" + months + "' ";
            //            if (!staffname.Equals(string.Empty))
            //            {
            //                sql += " and a.staff_name=? ";
            //            }
            //            sql += "  and DEPT_ID in (select DEPT_CODE from PERFORMANCE.SET_ACCOUNTDEPTTYPE where st_date=to_date('" + years + month + "01" + "','yyyymmdd') and DEPT_TYPE in ('20001'))";

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

            DataSet ds = new DataSet();

            string month = months;

            if (month.Length < 2)
            {
                month = "0" + month;
            }

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT   '{0}' AS years,
         '{1}' AS months,
         B.ACCOUNT_DEPT_CODE deptid,
         B.ACCOUNT_DEPT_NAME deptname,
         NAME AS staffname,
         A.STAFF_ID AS STAFF_ID,
         1 ISBONUS
  FROM   RLZY.NEW_STAFF_INFO A,
         (SELECT   dept_code, ACCOUNT_DEPT_NAME, ACCOUNT_DEPT_CODE
            FROM   comm.sys_dept_info
           WHERE   TO_CHAR (DEPT_SNAP_DATE, 'yyyymm') = '{2}'
                   AND account_dept_code IN
                            (SELECT   dept_code
                               FROM   PERFORMANCE.SET_ACCOUNTDEPTTYPE
                              WHERE   TO_CHAR (st_date, 'yyyymm') = '{2}'
                                      AND DEPT_TYPE = '20001')) b
 WHERE   a.DEPT_CODE = b.DEPT_CODE
MINUS
SELECT   years,
         months,
         a.DEPT_ID AS deptid,
         a.dept_name deptname,
         a.staff_name staffname,
         A.STAFF_ID AS STAFF_ID,
         ISBONUS
  FROM   PERFORMANCE.set_averagebonusdays a
 WHERE   years = '{0}' AND months = '{1}'
         AND DEPT_ID IN
                  (SELECT   dept_code
                     FROM   PERFORMANCE.SET_ACCOUNTDEPTTYPE
                    WHERE   TO_CHAR (st_date, 'yyyymm') = '201607'
                            AND DEPT_TYPE = '20001')
", years, months, years + month);

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
        /// 获得新建当月的设置的平均奖科室
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetAverageDeptAdd(string years, string months, bool all)
        {

            string sql = "";
            if (all)
            {
                sql += " select '00000' as DEPTID,'全部' DEPTNAME from dual union all  ";
            }
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            sql += "  select a.DEPT_CODE as DEPTID,b.DEPT_NAME as DEPTNAME from PERFORMANCE.SET_ACCOUNTDEPTTYPE a,comm.SYS_DEPT_DICT b where a.DEPT_CODE=b.DEPT_CODE and st_date=to_date('" + years + months + "01" + "','yyyymmdd') and a.DEPT_TYPE in ('20001')";
            sql += " group by  a.DEPT_CODE,b.DEPT_NAME ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildAverageDept();
            }
        }

        /// <summary>
        /// 获得所在月份平均奖科室的信息
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetAverageDept(string years, string months, bool all)
        {
            string sql = "";
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            if (all)
            {
                sql += " select '00000' as DEPTID,'全部' DEPTNAME from dual union all  ";
            }

            sql += "  SELECT b.dept_code deptid,b.dept_name deptname FROM PERFORMANCE.set_accountdepttype a,comm.sys_dept_dict b where a.st_date=to_date('" + years + months + "01" + "','yyyymmdd') and a.dept_code=b.dept_code and a.DEPT_TYPE in ('20001')";
            sql += " order by deptid ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildAverageDept();
            }
        }

        /// <summary>
        /// 平均奖科室信息表结构
        /// </summary>
        /// <returns></returns>
        private DataTable BuildAverageBonusList()
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable BuildAverageBonusDaysList()
        {
            DataTable dt = new DataTable();
            DataColumn dcYEARS = new DataColumn("YEARS");
            DataColumn dcMONTHS = new DataColumn("MONTHS");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            DataColumn dcSTAFFNAME = new DataColumn("STAFFNAME");
            DataColumn dcISBONUS = new DataColumn("ISBONUS");
            DataColumn dcDAYS = new DataColumn("DAYS");
            DataColumn dcBONUSMODULUS = new DataColumn("BONUSMODULUS");
            DataColumn dcOBLIGATION = new DataColumn("OBLIGATION");
            dt.Columns.AddRange(new DataColumn[] { dcYEARS, dcMONTHS, dcDEPTNAME, dcSTAFFNAME, dcISBONUS, dcDAYS, dcOBLIGATION });
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable BuildAverageDept()
        {
            DataTable dt = new DataTable();
            DataColumn dcDeptId = new DataColumn("DEPTID");
            DataColumn dcDeptName = new DataColumn("DEPTNAME");
            dt.Columns.AddRange(new DataColumn[] { dcDeptId, dcDeptName });
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 删除选中时间段内的平均奖科室的数据以及再次添加平均奖科室人数数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void SaveAverageBonusDays(Dictionary<string, string>[] rows, string year, string month, User user, string deptid)
        {
            string delsql = "delete from  performance.SET_AVERAGEBONUSDAYS  where  YEARS='" + year + "' and MONTHS='" + month + "'";
            if (deptid != "" && deptid != "00000")
                delsql += " and dept_id='" + deptid + "'";
            MyLists listtable = new MyLists();
            //删除科室类别
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < rows.Length; i++)
            {
                StringBuilder isql = new StringBuilder();
                isql.Append(" insert into performance.SET_AVERAGEBONUSDAYS (YEARS,MONTHS,DEPT_ID,DEPT_NAME,STAFF_NAME,ISBONUS,DAYS,INPUT_DATE,INPUTER_ID,INPUTER,BONUSMODULUS,OBLIGATION,STAFF_ID) values (");
                isql.Append("'" + year + "'");
                isql.Append(",");
                isql.Append("'" + month + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["DEPTID"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["DEPTNAME"].ToString() + "'");
                isql.Append(",");
                //isql.Append("'" + rows[i]["STAFFID"].ToString() + "'");
                //isql.Append(",");
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
                isql.Append("'" + user.StaffId + "'");
                isql.Append(",");
                isql.Append("'" + user.UserName + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["BONUSMODULUS"].ToString() + "'");
                string tempob = "";
                isql.Append(",");
                tempob = rows[i]["OBLIGATION"] == null ? "0" : rows[i]["OBLIGATION"].ToString();
                isql.Append("'" + tempob + "'");
                //isql.Append(",");
                //isql.Append("'" + rows[i]["EMP_NO"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["STAFF_ID"].ToString() + "'");
                //isql.Append(",");
                //string quaval = "";
                //quaval = rows[i]["QUA_VAL"] == null ? "0" : rows[i]["QUA_VAL"].ToString();
                //isql.Append("'" + quaval + "'");
                isql.Append(") ");
                //添加平均奖人数
                List listcenterdetail = new List();
                listcenterdetail.StrSql = isql.ToString();
                listcenterdetail.Parameters = new OleDbParameter[] { };
                listtable.Add(listcenterdetail);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public void EditAverageBonusDays(string year, string months)
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

            StringBuilder sql = new StringBuilder();
//            sql.AppendFormat(@"UPDATE   performance.set_averagebonusdays
//                                   SET   DAYS =
//                                            (SELECT   MAX (a01 - b01 - b02 - b03 - c01 - c02 - c03)
//                                               FROM   HISDATA.QU_ATTENDANCE_DEPT_VIEW a
//                                              WHERE   TRUNC (a.year_month, 'mm') = TO_DATE ('{0}{2}01', 'yyyymmdd')
//                                                      AND a.emp_no = performance.set_averagebonusdays.emp_no)
//                                 WHERE   years = '{0}' AND months = '{1}'", year, months, month);

            sql.AppendFormat(@"UPDATE performance.set_averagebonusdays set BONUSMODULUS=(SELECT  max(BONUSMODULUS)
                                               FROM   performance.set_averagebonusdays a
                                             WHERE   to_date(a.years||lpad(months,2,'0')||'01','yyyymmdd') = add_months(TO_DATE ('{0}{2}01', 'yyyymmdd'),-1)
                                                      
                                                      and a.emp_no=performance.set_averagebonusdays.emp_no),
                                             OBLIGATION=(SELECT  max(OBLIGATION)
                                               FROM   performance.set_averagebonusdays a
                                              WHERE   to_date(a.years||lpad(months,2,'0')||'01','yyyymmdd') = add_months(TO_DATE ('{0}{2}01', 'yyyymmdd'),-1)
                                                      
                                                      and a.emp_no=performance.set_averagebonusdays.emp_no),
                                             QUA_VAL=(SELECT  max(QUA_VAL)
                                               FROM   performance.set_averagebonusdays a
                                              WHERE   to_date(a.years||lpad(months,2,'0')||'01','yyyymmdd') = add_months(TO_DATE ('{0}{2}01', 'yyyymmdd'),-1)
                                                      
                                                      and a.emp_no=performance.set_averagebonusdays.emp_no)
                                                      
                                 WHERE   years = '{0}' AND months = '{1}'", year, months, month);

            OracleOledbBase.ExecuteNonQuery(sql.ToString());
        }

        /// <summary>
        /// 删除选中时间段内的平均奖科室的数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void DeleteAverageBonusDays(Dictionary<string, string>[] rows)
        {
            MyLists listtable = new MyLists();
            for (int i = 0; i < rows.Length; i++)
            {
                StringBuilder isql = new StringBuilder();
                isql.Append(" delete from  performance.SET_AVERAGEBONUSDAYS  where  YEARS='" + rows[i]["YEARS"].ToString() + "' and MONTHS='" + rows[i]["MONTHS"].ToString() + "' and");
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
            string sql = " select count(*) as cnt from performance.SET_AVERAGEBONUSDAYS where YEARS='" + year + "' and MONTHS='" + months + "' and DEPT_ID='" + deptid + "' and DEPT_NAME='" + deptname + "' and STAFF_NAME='" + staffname + "' ";
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
        public void AddOnePerson(string year, string month, string deptid, string deptname, string staffname, int isbonus, int days, double bonusmodulus, User user)
        {
            month = int.Parse(month).ToString();
            string sql = "insert into performance.SET_AVERAGEBONUSDAYS ";
            sql += "(YEARS,MONTHS,DEPT_ID,DEPT_NAME,STAFF_NAME,ISBONUS,DAYS,INPUT_DATE,INPUTER_ID,INPUTER,BONUSMODULUS) values (";
            sql += "'" + year + "',";
            sql += "'" + month + "',";
            sql += "'" + deptid + "',";
            sql += "'" + deptname + "',";
            sql += "'" + staffname + "',";
            sql += "'" + isbonus + "',";
            sql += "" + days + ",";
            sql += "to_date('" + System.DateTime.Now.ToString("yyyyMMdd") + "','yyyymmdd'),";
            sql += "'" + user.StaffId + "',";
            sql += "'" + user.UserName + "',";
            sql += "" + bonusmodulus + ")";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除平均奖科室
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        public void DeleteAvergeBonusDays(string years, string months)
        {
            string sql = "delete  performance.SET_AVERAGEBONUSDAYS where YEARS='" + years + "' and MONTHS='" + months + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

    }
}
