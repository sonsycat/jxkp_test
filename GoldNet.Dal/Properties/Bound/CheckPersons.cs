using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class CheckPersons
    {
        public DataTable GetDeptType(string year, string month)
        {
            string sql = "  select TYPE_CODE as TYPECODE,TYPE_NAME as TYPENAME from  performance.SET_ACCOUNTTYPE  where  TYPE_CODE not in ('10001','20001') order by sort ";
            return OracleOledbBase.ExecuteDataSet(sql.ToString()).Tables[0];
        }
        /// <summary>
        /// 获得已经录入的平均奖的信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetCheckPersonsList()
        {
            StringBuilder str = new StringBuilder();

            str.Append(@"SELECT DISTINCT a.years, a.months,
                                a.years || '年' || a.months || '月' AS yearmonth,
                                TO_CHAR (MIN (input_date), 'yyyy-MM-dd') AS inputdate,
                                MIN (inputer) inputer, SUM (persons) totalpersons
                           FROM PERFORMANCE.set_checkpersons a,
                                (SELECT TO_CHAR (st_date, 'yyyy') years,
                                        TO_CHAR (st_date, 'mm') months, 
                                        dept_code
                                   FROM PERFORMANCE.set_accountdepttype A,PERFORMANCE.SET_ACCOUNTTYPEGROUP B
                                  WHERE A.DEPT_TYPE=B.TYPE_CODE 
                                    AND B.GROUP_CODE = '01'            
                                ) b
                          WHERE a.years = b.years
                            AND (CASE
                                    WHEN LENGTH (a.months) = 1
                                       THEN '0' || a.months
                                    ELSE a.months
                                 END
                                ) = b.months
                            AND a.dept_id = b.dept_code
                       GROUP BY a.years, a.months
                       ORDER BY TO_NUMBER (a.years) DESC, TO_NUMBER (a.months) DESC");

            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
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
        /// 单个保存奖金人员
        /// </summary>
        /// <param name="index"></param>
        /// <param name="username"></param>
        /// <param name="deptcode"></param>
        /// <param name="deptname"></param>
        public void savesingepersons(int index, string username, string deptcode, string deptname)
        {
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.BONUS_DETAIL", DataUser.PERFORMANCE));
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("insert into {0}.BONUS_DETAIL(", DataUser.PERFORMANCE);
            strSql.Append("id,INDEX_ID,UNIT_NAME,USER_NAME,USER_BONUS,UNIT_CODE)");
            strSql.Append(" values (");
            strSql.Append("?,?,?,?,0,?)");
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("id",id),
											  new OleDbParameter("INDEX_ID", index), 
                                              new OleDbParameter("UNIT_NAME",deptname),
                                               new OleDbParameter("USER_NAME",username),
                                                 new OleDbParameter("UNIT_CODE",deptcode)
										  };
            OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameteradd);
        }
        /// <summary>
        /// 保存奖金人员
        /// </summary>
        /// <param name="users"></param>
        /// <param name="deptcode"></param>
        /// <param name="deptname"></param>
        public void SaverlzyUsers(List<GoldNet.Model.PageModels.rlzylected> users, int index, string deptcode, string deptname)
        {
            MyLists listtable = new MyLists();
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.BONUS_DETAIL", DataUser.PERFORMANCE));
            int i = 0;
            foreach (GoldNet.Model.PageModels.rlzylected user in users)
            {
                string b_code = user.BANK_CODE;
                if (b_code == null)
                    b_code = "";


                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.BONUS_DETAIL(", DataUser.PERFORMANCE);
                strSql.Append("id,INDEX_ID,UNIT_NAME,USER_NAME,USER_BONUS,UNIT_CODE,STAFF_ID,BANK_CODE)");
                strSql.Append(" values (");
                strSql.Append("?,?,?,?,0,?,?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("id",id+i),
											  new OleDbParameter("INDEX_ID", index), 
                                              new OleDbParameter("UNIT_NAME",deptname),
                                              new OleDbParameter("USER_NAME",user.USER_NAME),
                                              new OleDbParameter("UNIT_CODE",deptcode),
                                              new OleDbParameter("STAFF_ID",user.STAFF_ID),
                                              new OleDbParameter("BANK_CODE",b_code)
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
                i++;
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        /// <summary>
        /// 获得所在月份核算科室人数
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetCheckPersons(string years, string months,string deptcode)
        {
            StringBuilder str= new StringBuilder();
            str.AppendFormat(@"SELECT '{0}' years, '{1}' months, b.dept_code deptid, b.dept_name deptname,
                                   nvl(a.persons,0) persons, 
                                   NVL (a.doctor_numbers, 0) doctor_numbers,
                                   NVL (a.nurse_numbers, 0) nurse_numbers,
                                   NVL (a.other_numbers, 0) other_numbers,b.st_date,b.dept_code,b.dept_type
                                   
                        FROM (SELECT a.years, a.months, a.dept_id deptid, a.dept_name deptname,
                                   a.persons persons, NVL (a.doctor_numbers, 0) doctor_numbers,
                                   NVL (a.nurse_numbers, 0) nurse_numbers,
                                   NVL (a.other_numbers, 0) other_numbers
                              FROM PERFORMANCE.set_checkpersons a
                             WHERE years = '{0}'
                               AND months = '{1}'
                               AND dept_id IN (
                                      SELECT A.dept_code
                                        FROM PERFORMANCE.set_accountdepttype A,PERFORMANCE.SET_ACCOUNTTYPEGROUP B
                                       WHERE A.DEPT_TYPE=B.TYPE_CODE 
                                         AND A.st_date = TO_DATE ('{0}'||LPAD('{1}',2,0)||'01', 'yyyymmdd')
                                         AND B.GROUP_CODE = '01')
                           ) a
                           right JOIN
                           (
                            SELECT a.*,c.dept_name,c.sort_no
                              FROM PERFORMANCE.set_accountdepttype a,
                                   PERFORMANCE.set_accounttypegroup b,
                                   comm.sys_dept_info c
                             WHERE a.dept_type = b.type_code
                               and A.DEPT_CODE = c.dept_code
                               AND a.st_date = TO_DATE ('{0}' || LPAD ('{1}', 2, 0) || '01', 'yyyymmdd')
                               and C.DEPT_SNAP_DATE =TO_DATE ('{0}' || LPAD ('{1}', 2, 0) || '01', 'yyyymmdd')
                               AND b.group_code = '01'
                           ) b
                           ON a.deptid = b.dept_code
                          AND b.st_date = TO_DATE (a.years||LPAD(a.months,2,0)|| '01','yyyymmdd')", years, months);
            if (deptcode != "")
                str.AppendFormat(" where b.dept_code in ({0})",deptcode);
            str.AppendFormat(" order by b.SORT_NO");


            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildCheckPersons();
            }
        }
        /// <summary>
        /// 获得所在月份平均奖科室的信息
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetCheckDept(string years, string months, bool all)
        {
            string sql = "";
            if (all)
            {
                sql += " select '00000' as DEPTID,'全部' DEPTNAME from dual union all  ";
            }
            sql += " select DEPT_ID DEPTID,DEPT_NAME DEPTNAME from performance.SET_CHECKPERSONS  where YEARS='" + years + "' and MONTHS='" + months + "'";
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            sql += "  and DEPT_ID in (select DEPT_CODE from PERFORMANCE.SET_ACCOUNTDEPTTYPE where st_date=to_date('" + years + months + "01" + "','yyyymmdd') and DEPT_TYPE not in ('10001','20001'))";
            sql += " group by  DEPT_ID,DEPT_NAME ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildCheckDept();
            }
        }
        /// <summary>
        /// 新建部门的列表
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetNewCheckPerson(string years, string months)
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
            string sql = " select years,months,deptid,deptname,ROUND(persons,2) persons, round(DOCTOR_NUMBERS,2) DOCTOR_NUMBERS, round(NURSE_NUMBERS,2) NURSE_NUMBERS, round(OTHER_NUMBERS,2) OTHER_NUMBERS ";
            sql += " from ";
            sql += " ( ";
            sql += " SELECT   '" + years + "' AS years, '" + months + "' AS months, dept_code deptid, dept_name deptname, ";
            sql += "  0 AS persons, 0 AS doctor_numbers, 0 AS nurse_numbers, 0 AS other_numbers ";
            sql += "   FROM comm.sys_dept_dict ";
            sql += "   WHERE dept_code IN ( ";
            sql += "   SELECT dept_code ";
            sql += "   FROM PERFORMANCE.set_accountdepttype ";
            sql += "   WHERE st_date = TO_DATE ('" + years + month + "01" + "', 'yyyymmdd') ";
            sql += "   AND dept_type not in ('10001','20001')) ";
            sql += " and dept_code not in ( ";
            sql += " select DEPT_ID from ";
            sql += " PERFORMANCE.SET_CHECKPERSONS  ";
            sql += " where years='" + years + "' and months='" + months + "') ";
            sql += " ) ";
            sql += " union ";
            sql += " select years,months,DEPT_ID deptid,dept_name,PERSONS,nvl(DOCTOR_NUMBERS,0) DOCTOR_NUMBERS,nvl(NURSE_NUMBERS,0) NURSE_NUMBERS,nvl(OTHER_NUMBERS,0) OTHER_NUMBERS from ";
            sql += " PERFORMANCE.SET_CHECKPERSONS  ";
            sql += " where years='" + years + "' and months='" + months + "' ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildCheckPersons();
            }
        }
        public bool GetcheckPerson(string year, string month, string deptid)
        {
            string sql = " select DEPT_ID from PERFORMANCE.SET_CHECKPERSONS where YEARS=" + year + " and MONTHS=" + month + " and DEPT_ID='" + deptid + "'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获得新建当月的设置的科室
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetNewCheckDept(string years, string months, bool all)
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
            sql += "  select a.DEPT_CODE as DEPTID,b.DEPT_NAME as DEPTNAME from PERFORMANCE.SET_ACCOUNTDEPTTYPE a,comm.SYS_DEPT_DICT b where a.DEPT_CODE=b.DEPT_CODE and st_date=to_date('" + years + months + "01" + "','yyyymmdd') and a.DEPT_TYPE not in ('10001','20001')";
            sql += " group by  a.DEPT_CODE,b.DEPT_NAME ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildCheckDept();
            }
        }
        /// <summary>
        /// 删除选中时间段内的核算科室的数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void DeleteCheckPerson(Dictionary<string, string>[] rows)
        {
            MyLists listtable = new MyLists();
            for (int i = 0; i < rows.Length; i++)
            {
                StringBuilder isql = new StringBuilder();
                isql.Append(" delete from  performance.SET_CHECKPERSONS  where  YEARS='" + rows[i]["YEARS"].ToString() + "' and MONTHS='" + rows[i]["MONTHS"].ToString() + "'");
                //添加平均奖人数
                List listcenterdetail = new List();
                listcenterdetail.StrSql = isql.ToString();
                listcenterdetail.Parameters = new OleDbParameter[] { };
                listtable.Add(listcenterdetail);
            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }
        /// <summary>
        /// 核算科室的人数列表表结构
        /// </summary>
        /// <returns></returns>
        private DataTable BuildAverageBonusList()
        {
            DataTable dt = new DataTable();
            DataColumn dcYEARS = new DataColumn("YEARS");
            DataColumn dcMONTHS = new DataColumn("MONTHS");
            DataColumn dcYEAR_MONTH = new DataColumn("YEARMONTH");
            DataColumn dcTOTALNUMBER = new DataColumn("TOTALPERSONS");
            DataColumn dcINPUUDATE = new DataColumn("INPUTDATE");
            DataColumn dcINPUTER = new DataColumn("INPUTER");
            dt.Columns.AddRange(new DataColumn[] { dcYEARS, dcMONTHS, dcYEAR_MONTH, dcTOTALNUMBER, dcINPUUDATE, dcINPUTER });
            return dt;
        }
        /// <summary>
        /// 核算科室的人数表结构
        /// </summary>
        /// <returns></returns>
        public DataTable BuildCheckPersons()
        {
            DataTable dt = new DataTable();
            DataColumn dcYEARS = new DataColumn("YEARS");
            DataColumn dcMONTHS = new DataColumn("MONTHS");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            DataColumn dcPERSONS = new DataColumn("PERSONS");
            DataColumn dcDOCTOR_NUMBERS = new DataColumn("DOCTOR_NUMBERS");
            DataColumn dcNURSE_NUMBERS = new DataColumn("NURSE_NUMBERS");
            DataColumn dcOTHER_NUMBERS = new DataColumn("OTHER_NUMBERS");
            dt.Columns.AddRange(new DataColumn[] { dcYEARS, dcMONTHS, dcDEPTNAME, dcPERSONS, dcDOCTOR_NUMBERS, dcNURSE_NUMBERS, dcOTHER_NUMBERS });
            return dt;
        }
        private DataTable BuildCheckDept()
        {
            DataTable dt = new DataTable();
            DataColumn dcDeptId = new DataColumn("DEPTID");
            DataColumn dcDeptName = new DataColumn("DEPTNAME");
            dt.Columns.AddRange(new DataColumn[] { dcDeptId, dcDeptName });
            return dt;
        }
        /// <summary>
        /// 删除选中时间段内的核算科室的数据以及再次添加核算科室人数数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void SaveCheckPersons(Dictionary<string, string>[] rows, string year, string month, User user)
        {
            string delsql = "delete from  performance.SET_CHECKPERSONS  where  YEARS='" + year + "' and MONTHS='" + month + "'";
            MyLists listtable = new MyLists();
            //删除科室类别
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < rows.Length; i++)
            {
                double numbers = double.Parse(rows[i]["DOCTOR_NUMBERS"].ToString() == "" ? "0" : rows[i]["DOCTOR_NUMBERS"].ToString()) + double.Parse(rows[i]["NURSE_NUMBERS"].ToString() == "" ? "0" : rows[i]["NURSE_NUMBERS"].ToString()) + double.Parse(rows[i]["OTHER_NUMBERS"].ToString() == "" ? "0" : rows[i]["OTHER_NUMBERS"].ToString());
                StringBuilder isql = new StringBuilder();
                isql.Append(" insert into performance.SET_CHECKPERSONS (YEARS,MONTHS,DEPT_ID,DEPT_NAME,PERSONS,INPUT_DATE,INPUTER_ID,INPUTER,DOCTOR_NUMBERS,NURSE_NUMBERS,OTHER_NUMBERS) values (");
                isql.Append("'" + year + "'");
                isql.Append(",");
                isql.Append("'" + month + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["DEPTID"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["DEPTNAME"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + numbers.ToString() + "'");
                isql.Append(",");
                isql.Append("to_date('" + System.DateTime.Now.ToString("yyyyMMdd") + "','yyyymmdd')");
                isql.Append(",");
                isql.Append("'" + user.StaffId + "'");
                isql.Append(",");
                isql.Append("'" + user.UserName + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["DOCTOR_NUMBERS"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["NURSE_NUMBERS"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["OTHER_NUMBERS"].ToString() + "'");
                isql.Append(")");

                //添加平均奖人数
                List listcenterdetail = new List();
                listcenterdetail.StrSql = isql.ToString();
                listcenterdetail.Parameters = new OleDbParameter[] { };
                listtable.Add(listcenterdetail);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        public DataTable GetDeptBonussetList(string years, string months,string deptcode,string deptfilter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select  dept_id,dept_name,staff_name,nvl(days,0) days,nvl(BONUSMODULUS,0) BONUSMODULUS,nvl(PERSONSMODULUS,0) PERSONSMODULUS,(nvl(BONUSMODULUS,0)+nvl(PERSONSMODULUS,0)) summodulus from performance.SET_CHECKBONUSDAYS where years='{0}' and months='{1}'", years, months);
            if (deptcode != "")
            {
                str.AppendFormat(" and dept_id='{0}'",deptcode);
            }
            if (deptfilter != "")
            {
                str.AppendFormat(" and dept_id in ({0})", deptfilter);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0]; 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public DataTable GetBonusBalance(string years, string months)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT UNIT_CODE DEPTCODE,
                                      DEPT_NAME DEPTNAME,
                                      ROUND(DECODE(SIGN(SYLC-BYLC),1,SYLC-BYLC),2) BYFH,
                                      ROUND(DECODE(SIGN(SYLC-BYLC),-1,BYLC-SYLC),2) BYFD,
                                      ROUND(SYLC,2) SYLC
                                        FROM
                                        (
                                        SELECT A.UNIT_CODE,
                                               C.DEPT_NAME,
                                               SUM(CASE WHEN OCCURRENCE_TIME=TO_DATE('{0}'||LPAD('{1}',2,0)||'01','yyyymmdd') THEN NVL(UNIT_CASH,0) ELSE 0 END) BYLC,
                                               SUM(CASE WHEN OCCURRENCE_TIME=ADD_MONTHS(TO_DATE('{0}'||LPAD('{1}',2,0)||'01','yyyymmdd'),-1) THEN NVL(UNIT_CASH,0) ELSE 0 END) SYLC
                                        FROM PERFORMANCE.BONUS_BALANCE A,
                                             PERFORMANCE.SET_ACCOUNTDEPTTYPE B,
                                             COMM.SYS_DEPT_DICT C
                                        WHERE A.UNIT_CODE(+) = B.DEPT_CODE
                                          AND A.OCCURRENCE_TIME(+)=B.ST_DATE
                                          AND C.DEPT_CODE(+)=B.DEPT_CODE
                                          AND (B.ST_DATE = TO_DATE('{0}'||LPAD('{1}',2,0)||'01','yyyymmdd') OR B.ST_DATE = ADD_MONTHS(TO_DATE('{0}'||LPAD('{1}',2,0)||'01','yyyymmdd'),-1))
                                          AND B.DEPT_TYPE IN (40001,50001,60001,80001,30001)
                                          GROUP BY A.UNIT_CODE,C.DEPT_NAME
                                          ORDER BY A.UNIT_CODE
                                          )", years, months);
            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildBonusBalance();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable BuildBonusBalance()
        {
            DataTable dt = new DataTable();
            DataColumn dcDEPTCODE = new DataColumn("DEPTCODE");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            DataColumn dcBYFH = new DataColumn("BYFH");
            DataColumn dcBYFD = new DataColumn("BYFD");
            DataColumn dcSYLC = new DataColumn("SYLC");
            dt.Columns.AddRange(new DataColumn[] { dcDEPTCODE, dcDEPTNAME, dcBYFH, dcBYFD, dcSYLC });
            return dt;
        }

    }
}
