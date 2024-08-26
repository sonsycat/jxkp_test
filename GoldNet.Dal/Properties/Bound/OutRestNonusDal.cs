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
    public class OutRestNonusDal
    {
        public DataTable GetOutRestNonus(string id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"  SELECT   b.dept_code,min(weeks) weeks,
                                       DECODE (b.DEPT_CODE, '', '合计', MIN (b.DEPT_NAME)) DEPT_NAME,
                                       SUM (a.FORMAL_AM) AS FORMAL_AM,
                                       SUM (a.FORMAL_PM) AS FORMAL_PM,
                                       SUM (a.TEMPORARY_AM) AS TEMPORARY_AM,
                                       SUM (a.TEMPORARY_PM) AS TEMPORARY_PM,
                                       sum(a.sumnub) as sumnub
                                FROM   (  SELECT   DEPT_CODE,
                                                   ROUND (SUM (FORMAL_AM), 2) FORMAL_AM,
                                                   ROUND (SUM (FORMAL_PM), 2) FORMAL_PM,
                                                   ROUND (SUM (TEMPORARY_AM), 2) TEMPORARY_AM,
                                                   ROUND (SUM (TEMPORARY_PM), 2) TEMPORARY_PM,
                                                   ROUND (SUM (FORMAL_AM+FORMAL_PM+TEMPORARY_AM+TEMPORARY_PM), 2) as sumnub
                                            FROM   PERFORMANCE.OUT_REST_BONUS
                                           WHERE   weeks_id={0}
                                        GROUP BY   DEPT_CODE) a, comm.SYS_DEPT_DICT B,performance.OUT_WEEKS_SET c
                               WHERE       a.dept_code(+) = b.dept_code and c.id={0}
                                       and b.OUT_OR_IN='0'
                                       AND ACCOUNT_DEPT_CODE IS NOT NULL
                            GROUP BY   CUBE (b.DEPT_CODE)", id);
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 批量保存其它奖惩
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="input_date"></param>
        /// <param name="other_dict_id"></param>
        /// <param name="inputer"></param>
        /// <param name="other_dict_name"></param>
        public void SaveOutRestNonus(Dictionary<string, string>[] rows, string id)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.OUT_REST_BONUS WHERE weeks_id={1}", DataUser.PERFORMANCE, id);

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { };
            listtable.Add(listDel);

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i]["DEPT_CODE"].Equals(""))
                {
                    continue;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.OUT_REST_BONUS 
                                          (DEPT_CODE     ,
                                          WEEKS_ID       ,
                                          FORMAL_AM     ,
                                          FORMAL_PM     ,
                                          TEMPORARY_AM  ,
                                          TEMPORARY_PM)
                                        VALUES(?,?,?,?,?,?)", DataUser.PERFORMANCE);

                OleDbParameter[] parameteradd = {
											 new OleDbParameter("DEPT_CODE",rows[i]["DEPT_CODE"].ToString()),
											  new OleDbParameter("WEEKS_ID",id),
											  new OleDbParameter("FORMAL_AM",Convert.ToDouble( rows[i]["FORMAL_AM"])),
											  new OleDbParameter("FORMAL_PM",Convert.ToDouble( rows[i]["FORMAL_PM"])),
											  new OleDbParameter("TEMPORARY_AM",Convert.ToDouble( rows[i]["TEMPORARY_AM"])),
											  new OleDbParameter("TEMPORARY_PM", Convert.ToDouble( rows[i]["TEMPORARY_PM"]))
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }


            OracleOledbBase.ExecuteTranslist(listtable);
        }


        public void DelOutRestNonus(Dictionary<string, string>[] rows, string id)
        {
            MyLists listtable = new MyLists();
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i]["DEPT_CODE"].Equals(""))
                {
                    continue;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"delete from {0}.OUT_REST_BONUS  where weeks_id={1} and DEPT_CODE='{2}'"
                    , DataUser.PERFORMANCE, id, rows[i]["DEPT_CODE"].ToString());


                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = new OleDbParameter[] { };
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        public DataTable GetWeeks()
        {
            string str = string.Format("select * from performance.OUT_WEEKS_SET");
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="weeksid"></param>
        public void SaveWeeks(string date, string weeksid)
        {
            MyLists listtable = new MyLists();
            string delstr = string.Format(@"delete from performance.OUT_REST_BONUS_LIST where weeks_id={0} and to_char(st_date,'yyyy-mm-dd')='{1}'", weeksid, date);
            List listdel = new List();
            listdel.StrSql = delstr;
            listdel.Parameters = new OleDbParameter[] { };
            listtable.Add(listdel);

            string stradd = string.Format(@"insert into performance.OUT_REST_BONUS_LIST (st_date,weeks_id) values (to_date('{0}','yyyy-mm-dd'),?)", date);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("weeks_id", weeksid)
										  };
            List listAdd = new List();
            listAdd.StrSql = stradd;
            listAdd.Parameters = parameteradd;
            listtable.Add(listAdd);
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetOutRestNonusList()
        {
            string sql = string.Format(@"select to_char(st_date,'yyyy-mm-dd') as st_date,a.weeks_id,b.weeks from PERFORMANCE.OUT_REST_BONUS_LIST a,performance.OUT_WEEKS_SET b where a.weeks_id=b.id order by a.st_date desc");
            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        public DataTable GetOutRestNonusList(string datetime)
        {
            string sql = string.Format(@"   SELECT  TO_CHAR(A.RIQI,'yyyymmdd') RIQI,
                                                    NVL(B.WEEKS,CASE WHEN A.XINGQI='7' THEN '星期六'
                                                         WHEN A.XINGQI='1' THEN '星期日'
                                                         WHEN A.XINGQI='2' THEN '星期一'
                                                         WHEN A.XINGQI='3' THEN '星期二'
                                                         WHEN A.XINGQI='4' THEN '星期三'
                                                         WHEN A.XINGQI='5' THEN '星期四'
                                                         WHEN A.XINGQI='6' THEN '星期五'
                                                    END) WEEKS,
                                                    NVL(B.HOLIDAY_FLAG,CASE WHEN A.XINGQI='7' THEN '双休日'
                                                                            WHEN A.XINGQI='1' THEN '双休日'
                                                                            ELSE '工作日'
                                                                        END) HOLIDAY_FLAG,
                                                    NVL(B.WEEKS_ID,A.XINGQI) WEEKS_ID
                                                    

                                             FROM   (    SELECT  TO_CHAR (TO_DATE ('{0}', 'yyyymmdd') + ROWNUM - 1,'D') AS XINGQI,
                                                                 TO_DATE ('{0}', 'yyyymmdd') + ROWNUM - 1 AS RIQI
                                                          FROM   DUAL
                                                    CONNECT BY   ROWNUM <=ADD_MONTHS (TO_DATE ('{0}', 'yyyymmdd'),1) - TO_DATE ('{0}', 'yyyymmdd')
                                                    ) A,
                                                    (   SELECT   *
                                                          FROM   PERFORMANCE.OUT_REST_BONUS_LIST
                                                         WHERE   ST_DATE >= TO_DATE ('{0}', 'yyyymmdd')
                                                                 AND ST_DATE < ADD_MONTHS (TO_DATE ('{0}', 'yyyymmdd'), 1)
                                                    ) B
                                            WHERE  A.RIQI=B.ST_DATE(+)
                                            ORDER BY A.RIQI", datetime);

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 删除工作日、双休日
        /// </summary>
        /// <param name="input_date"></param>
        public void DelOutRestNonusList(string input_date)
        {
            MyLists listtable = new MyLists();

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"delete from {0}.OUT_REST_BONUS_list  where to_char(ST_DATE,'yyyy-mm-dd')='{1}'"
                , DataUser.PERFORMANCE, input_date);

            List listAdd = new List();
            listAdd.StrSql = strSql;
            listAdd.Parameters = new OleDbParameter[] { };
            listtable.Add(listAdd);

            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 保存工作日、双休日
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="datetime"></param>
        /// <param name="deptcode"></param>
        /// <param name="inputuser"></param>
        public void SaveOutRestNonusList(Dictionary<string, string>[] costdetails)
        {
            MyLists listtable = new MyLists();

            for (int i = 0; i < costdetails.Length; i++)
            {
                if (costdetails[i]["RIQI"] == null || costdetails[i]["WEEKS"].Equals("") || costdetails[i]["HOLIDAY_FLAG"].Equals(""))
                {
                    continue;
                }

                StringBuilder strDel = new StringBuilder();
                strDel.AppendFormat("DELETE PERFORMANCE.OUT_REST_BONUS_list WHERE st_date= to_date('{0}','yyyymmdd')", costdetails[i]["RIQI"]);

                List listDel = new List();
                listDel.StrSql = strDel;
                listtable.Add(listDel);

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO PERFORMANCE.OUT_REST_BONUS_list
                                          (ST_DATE,WEEKS_ID,WEEKS,HOLIDAY_FLAG)
                                        VALUES(to_date(?,'yyyymmdd'),?,?,?)", "");

                OleDbParameter[] parameteradd = {
											  new OleDbParameter("ST_DATE",costdetails[i]["RIQI"] ),
											  new OleDbParameter("WEEKS_ID",Convert.IsDBNull(costdetails[i]["WEEKS_ID"]) ? 0:Convert.ToDouble(costdetails[i]["WEEKS_ID"])),
											  new OleDbParameter("WEEKS",costdetails[i]["WEEKS"]),
											  new OleDbParameter("HOLIDAY_FLAG",costdetails[i]["HOLIDAY_FLAG"])
										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 获取日期类型（双休日、工作日）
        /// </summary>
        /// <returns></returns>
        public DataSet GetHolidays()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT '双休日' HOLIDAY_FLAG,'双休日' HOLIDAY_NAME FROM DUAL
                               UNION ALL
                               SELECT '工作日' HOLIDAY_FLAG,'工作日' HOLIDAY_NAME FROM DUAL
                               UNION ALL
                               SELECT '假日' HOLIDAY_FLAG,'假日' HOLIDAY_NAME FROM DUAL
                               ", "");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdate"></param>
        /// <param name="ddate"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetOutRestNonusAccount(string sdate, string ddate, string dept_code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                            SELECT   to_char(ST_DATE,'yyyy-mm-dd') as ST_DATE,
                                     b.dept_code,
                                     DEPT_NAME,
                                     a.FORMAL_AM AS FORMAL_AM,
                                     a.FORMAL_PM AS FORMAL_PM,
                                     a.TEMPORARY_AM AS TEMPORARY_AM,
                                     a.TEMPORARY_PM AS TEMPORARY_PM,
                                     a.sumnub AS sumnub
                              FROM   (SELECT   ST_DATE,
                                               DEPT_CODE,
                                               FORMAL_AM,
                                               FORMAL_PM,
                                               TEMPORARY_AM,
                                               TEMPORARY_PM,
                                               FORMAL_AM + FORMAL_PM + TEMPORARY_AM + TEMPORARY_PM
                                                  AS sumnub
                                        FROM   PERFORMANCE.OUT_REST_BONUS a,performance.OUT_REST_BONUS_LIST b
                                       WHERE  A.WEEKS_ID=B.WEEKS_ID and ST_DATE >= TO_DATE ('{0}', 'yyyymmdd')
                                               AND ST_DATE < add_months(TO_DATE ('{1}', 'yyyymmdd'),1)) a,
                                     comm.SYS_DEPT_DICT B
                             WHERE       a.dept_code = b.dept_code
                                     --AND B.ATTR = '是'
                                     AND b.OUT_OR_IN = '0'
                                     AND ACCOUNT_DEPT_CODE IS NOT NULL", sdate, ddate);

            if (!dept_code.Equals(""))
            {
                str.Append(dept_code);
            }

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 查询双休日门诊固定岗位补贴
        /// </summary>
        /// <param name="sdate"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public DataTable GetOutRestNonusAccount(string sdate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                        SELECT B.DEPT_CODE,
                               B.DEPT_NAME,
                               '{0}' DATE_TIME,
                               A1_1,
                               A1_2,
                               A1_3,
                               A1_4,
                               A2_1,
                               A2_2,
                               A2_3,
                               A2_4,
                               A3_1,
                               A3_2,
                               A3_3,
                               A3_4,
                               A4_1,
                               A4_2,
                               A4_3,
                               A4_4,
                               A5_1,
                               A5_2,
                               A5_3,
                               A5_4,
                               A6_1,
                               A6_2,
                               A6_3,
                               A6_4,
                               A7_1,
                               A7_2,
                               A7_3,
                               A7_4,
                               A8_1,
                               A8_2,
                               A8_3,
                               A8_4,
                               A9_1,
                               A9_2,
                               A9_3,
                               A9_4,
                               A10_1,
                               A10_2,
                               A10_3,
                               A10_4,
                               A11_1,
                               A11_2,
                               A11_3,
                               A11_4,
                               A12_1,
                               A12_2,
                               A12_3,
                               A12_4,
                               A13_1,
                               A13_2,
                               A13_3,
                               A13_4,
                               A14_1,
                               A14_2,
                               A14_3,
                               A14_4,
                               NVL(A1_1,0)*50+NVL(A1_2,0)*50+NVL(A1_3,0)*40+NVL(A1_4,0)*40+NVL(A2_1,0)*50+NVL(A2_2,0)*50+NVL(A2_3,0)*40+NVL(A2_4,0)*40+NVL(A3_1,0)*50+NVL(A3_2,0)*50+NVL(A3_3,0)*40+NVL(A3_4,0)*40+NVL(A4_1,0)*50+NVL(A4_2,0)*50+NVL(A4_3,0)*40+NVL(A4_4,0)*40+NVL(A5_1,0)*50+NVL(A5_2,0)*50+NVL(A5_3,0)*40+NVL(A5_4,0)*40+NVL(A6_1,0)*50+NVL(A6_2,0)*50+NVL(A6_3,0)*40+NVL(A6_4,0)*40+NVL(A7_1,0)*50+NVL(A7_2,0)*50+NVL(A7_3,0)*40+NVL(A7_4,0)*40+NVL(A8_1,0)*50+NVL(A8_2,0)*50+NVL(A8_3,0)*40+NVL(A8_4,0)*40+NVL(A9_1,0)*50+NVL(A9_2,0)*50+NVL(A9_3,0)*40+NVL(A9_4,0)*40+NVL(A10_1,0)*50+NVL(A10_2,0)*50+NVL(A10_3,0)*40+NVL(A10_4,0)*40+NVL(A11_1,0)*50+NVL(A11_2,0)*50+NVL(A11_3,0)*40+NVL(A11_4,0)*40+NVL(A12_1,0)*50+NVL(A12_2,0)*50+NVL(A12_3,0)*40+NVL(A12_4,0)*40+NVL(A13_1,0)*50+NVL(A13_2,0)*50+NVL(A13_3,0)*40+NVL(A13_4,0)*40+NVL(A14_1,0)*50+NVL(A14_2,0)*50+NVL(A14_3,0)*40+NVL(A14_4,0)*40 OUT_TOTAL
                          FROM (SELECT * FROM PERFORMANCE.OUT_BONUS_REST_INPUT WHERE DATA_TIME='{0}') A,COMM.SYS_DEPT_DICT B 
                         WHERE A.DEPT_CODE(+) = B.DEPT_CODE
                           AND B.OUT_OR_IN='0'
                           ORDER BY B.DEPT_NAME", sdate);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 保存双休日门诊固定岗位补贴
        /// </summary>
        /// <param name="costdetails"></param>
        /// <param name="date_time"></param>
        public void SaveOutRestNonusAccount(Dictionary<string, string>[] costdetails, string date_time)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM PERFORMANCE.OUT_BONUS_REST_INPUT WHERE DATA_TIME='{0}' ", date_time);

            List listDel = new List();
            listDel.StrSql = strDel;
            listtable.Add(listDel);

            for (int i = 0; i < costdetails.Length; i++)
            {
                if (costdetails[i]["DEPT_CODE"] == null || costdetails[i]["DEPT_CODE"].Equals(""))
                {
                    continue;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO PERFORMANCE.OUT_BONUS_REST_INPUT 
                                          (DEPT_CODE,DATA_TIME,A1_1,A1_2,A1_3,A1_4,A2_1,A2_2,A2_3,A2_4,A3_1,A3_2,A3_3,A3_4,A4_1,A4_2,A4_3,A4_4,A5_1,A5_2,A5_3,A5_4,A6_1,A6_2,A6_3,A6_4,
                                                               A7_1,A7_2,A7_3,A7_4,A8_1,A8_2,A8_3,A8_4,A9_1,A9_2,A9_3,A9_4,A10_1,A10_2,A10_3,A10_4,A11_1,A11_2,A11_3,A11_4,A12_1,A12_2,A12_3,A12_4,A13_1,A13_2,A13_3,A13_4,A14_1,A14_2,A14_3,A14_4)
                                        VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DataUser.CBHS, date_time);

                OleDbParameter[] parameteradd = {
											  new OleDbParameter("DEPT_CODE",costdetails[i]["DEPT_CODE"] ),
											  new OleDbParameter("DATA_TIME",costdetails[i]["DATE_TIME"]),
											  new OleDbParameter("A1_1",Convert.IsDBNull(costdetails[i]["A1_1"]) || costdetails[i]["A1_1"]=="" ? 0:Convert.ToDouble(costdetails[i]["A1_1"])),
											  new OleDbParameter("A1_2",Convert.IsDBNull(costdetails[i]["A1_2"]) || costdetails[i]["A1_2"]=="" ? 0:Convert.ToDouble(costdetails[i]["A1_2"])),
											  new OleDbParameter("A1_3",Convert.IsDBNull(costdetails[i]["A1_3"]) ? 0:Convert.ToDouble(costdetails[i]["A1_3"])),
											  new OleDbParameter("A1_4",Convert.IsDBNull(costdetails[i]["A1_4"]) ? 0:Convert.ToDouble(costdetails[i]["A1_4"])),
											  new OleDbParameter("A2_1",Convert.IsDBNull(costdetails[i]["A2_1"]) ? 0:Convert.ToDouble(costdetails[i]["A2_1"])),
											  new OleDbParameter("A2_2",Convert.IsDBNull(costdetails[i]["A2_2"]) ? 0:Convert.ToDouble(costdetails[i]["A2_2"])),
											  new OleDbParameter("A2_3",Convert.IsDBNull(costdetails[i]["A2_3"]) ? 0:Convert.ToDouble(costdetails[i]["A2_3"])),
											  new OleDbParameter("A2_4",Convert.IsDBNull(costdetails[i]["A2_4"]) ? 0:Convert.ToDouble(costdetails[i]["A2_4"])),
											  new OleDbParameter("A3_1",Convert.IsDBNull(costdetails[i]["A3_1"]) ? 0:Convert.ToDouble(costdetails[i]["A3_1"])),
											  new OleDbParameter("A3_2",Convert.IsDBNull(costdetails[i]["A3_2"]) ? 0:Convert.ToDouble(costdetails[i]["A3_2"])),
                                              new OleDbParameter("A3_3",Convert.IsDBNull(costdetails[i]["A3_3"]) ? 0:Convert.ToDouble(costdetails[i]["A3_3"])),
                                              new OleDbParameter("A3_4",Convert.IsDBNull(costdetails[i]["A3_4"]) ? 0:Convert.ToDouble(costdetails[i]["A3_4"])),
                                              new OleDbParameter("A4_1",Convert.IsDBNull(costdetails[i]["A4_1"]) ? 0:Convert.ToDouble(costdetails[i]["A4_1"])),
                                              new OleDbParameter("A4_2",Convert.IsDBNull(costdetails[i]["A4_2"]) ? 0:Convert.ToDouble(costdetails[i]["A4_2"])),
                                              new OleDbParameter("A4_3",Convert.IsDBNull(costdetails[i]["A4_3"]) ? 0:Convert.ToDouble(costdetails[i]["A4_3"])),
                                              new OleDbParameter("A4_4",Convert.IsDBNull(costdetails[i]["A4_4"]) ? 0:Convert.ToDouble(costdetails[i]["A4_4"])),
                                              new OleDbParameter("A5_1",Convert.IsDBNull(costdetails[i]["A5_1"]) ? 0:Convert.ToDouble(costdetails[i]["A5_1"])),
                                              new OleDbParameter("A5_2",Convert.IsDBNull(costdetails[i]["A5_2"]) ? 0:Convert.ToDouble(costdetails[i]["A5_2"])),
                                              new OleDbParameter("A5_3",Convert.IsDBNull(costdetails[i]["A5_3"]) ? 0:Convert.ToDouble(costdetails[i]["A5_3"])),
                                              new OleDbParameter("A5_4",Convert.IsDBNull(costdetails[i]["A5_4"]) ? 0:Convert.ToDouble(costdetails[i]["A5_4"])),
                                              new OleDbParameter("A6_1",Convert.IsDBNull(costdetails[i]["A6_1"]) ? 0:Convert.ToDouble(costdetails[i]["A6_1"])),
                                              new OleDbParameter("A6_2",Convert.IsDBNull(costdetails[i]["A6_2"]) ? 0:Convert.ToDouble(costdetails[i]["A6_2"])),
                                              new OleDbParameter("A6_3",Convert.IsDBNull(costdetails[i]["A6_3"]) ? 0:Convert.ToDouble(costdetails[i]["A6_3"])),
                                              new OleDbParameter("A6_4",Convert.IsDBNull(costdetails[i]["A6_4"]) ? 0:Convert.ToDouble(costdetails[i]["A6_4"])),
                                              new OleDbParameter("A7_1",Convert.IsDBNull(costdetails[i]["A7_1"]) ? 0:Convert.ToDouble(costdetails[i]["A7_1"])),
                                              new OleDbParameter("A7_2",Convert.IsDBNull(costdetails[i]["A7_2"]) ? 0:Convert.ToDouble(costdetails[i]["A7_2"])),
                                              new OleDbParameter("A7_3",Convert.IsDBNull(costdetails[i]["A7_3"]) ? 0:Convert.ToDouble(costdetails[i]["A7_3"])),
                                              new OleDbParameter("A7_4",Convert.IsDBNull(costdetails[i]["A7_4"]) ? 0:Convert.ToDouble(costdetails[i]["A7_4"])),
                                              new OleDbParameter("A8_1",Convert.IsDBNull(costdetails[i]["A8_1"]) ? 0:Convert.ToDouble(costdetails[i]["A8_1"])),
                                              new OleDbParameter("A8_2",Convert.IsDBNull(costdetails[i]["A8_2"]) ? 0:Convert.ToDouble(costdetails[i]["A8_2"])),
                                              new OleDbParameter("A8_3",Convert.IsDBNull(costdetails[i]["A8_3"]) ? 0:Convert.ToDouble(costdetails[i]["A8_3"])),
                                              new OleDbParameter("A8_4",Convert.IsDBNull(costdetails[i]["A8_4"]) ? 0:Convert.ToDouble(costdetails[i]["A8_4"])),
                                              new OleDbParameter("A9_1",Convert.IsDBNull(costdetails[i]["A9_1"]) ? 0:Convert.ToDouble(costdetails[i]["A9_1"])),
                                              new OleDbParameter("A9_2",Convert.IsDBNull(costdetails[i]["A9_2"]) ? 0:Convert.ToDouble(costdetails[i]["A9_2"])),
                                              new OleDbParameter("A9_3",Convert.IsDBNull(costdetails[i]["A9_3"]) ? 0:Convert.ToDouble(costdetails[i]["A9_3"])),
                                              new OleDbParameter("A9_4",Convert.IsDBNull(costdetails[i]["A9_4"]) ? 0:Convert.ToDouble(costdetails[i]["A9_4"])),
                                              new OleDbParameter("A10_1",Convert.IsDBNull(costdetails[i]["A10_1"]) ? 0:Convert.ToDouble(costdetails[i]["A10_1"])),
                                              new OleDbParameter("A10_2",Convert.IsDBNull(costdetails[i]["A10_2"]) ? 0:Convert.ToDouble(costdetails[i]["A10_2"])),
                                              new OleDbParameter("A10_3",Convert.IsDBNull(costdetails[i]["A10_3"]) ? 0:Convert.ToDouble(costdetails[i]["A10_3"])),
                                              new OleDbParameter("A10_4",Convert.IsDBNull(costdetails[i]["A10_4"]) ? 0:Convert.ToDouble(costdetails[i]["A10_4"])),

                                              new OleDbParameter("A11_1",Convert.IsDBNull(costdetails[i]["A11_1"]) ? 0:Convert.ToDouble(costdetails[i]["A11_1"])),
                                              new OleDbParameter("A11_2",Convert.IsDBNull(costdetails[i]["A11_2"]) ? 0:Convert.ToDouble(costdetails[i]["A11_2"])),
                                              new OleDbParameter("A11_3",Convert.IsDBNull(costdetails[i]["A11_3"]) ? 0:Convert.ToDouble(costdetails[i]["A11_3"])),
                                              new OleDbParameter("A11_4",Convert.IsDBNull(costdetails[i]["A11_4"]) ? 0:Convert.ToDouble(costdetails[i]["A11_4"])),

                                              new OleDbParameter("A12_1",Convert.IsDBNull(costdetails[i]["A12_1"]) ? 0:Convert.ToDouble(costdetails[i]["A12_1"])),
                                              new OleDbParameter("A12_2",Convert.IsDBNull(costdetails[i]["A12_2"]) ? 0:Convert.ToDouble(costdetails[i]["A12_2"])),
                                              new OleDbParameter("A12_3",Convert.IsDBNull(costdetails[i]["A12_3"]) ? 0:Convert.ToDouble(costdetails[i]["A12_3"])),
                                              new OleDbParameter("A12_4",Convert.IsDBNull(costdetails[i]["A12_4"]) ? 0:Convert.ToDouble(costdetails[i]["A12_4"])),

                                              new OleDbParameter("A13_1",Convert.IsDBNull(costdetails[i]["A13_1"]) ? 0:Convert.ToDouble(costdetails[i]["A13_1"])),
                                              new OleDbParameter("A13_2",Convert.IsDBNull(costdetails[i]["A13_2"]) ? 0:Convert.ToDouble(costdetails[i]["A13_2"])),
                                              new OleDbParameter("A13_3",Convert.IsDBNull(costdetails[i]["A13_3"]) ? 0:Convert.ToDouble(costdetails[i]["A13_3"])),
                                              new OleDbParameter("A13_4",Convert.IsDBNull(costdetails[i]["A13_4"]) ? 0:Convert.ToDouble(costdetails[i]["A13_4"])),

                                              new OleDbParameter("A14_1",Convert.IsDBNull(costdetails[i]["A14_1"]) ? 0:Convert.ToDouble(costdetails[i]["A14_1"])),
                                              new OleDbParameter("A14_2",Convert.IsDBNull(costdetails[i]["A14_2"]) ? 0:Convert.ToDouble(costdetails[i]["A14_2"])),
                                              new OleDbParameter("A14_3",Convert.IsDBNull(costdetails[i]["A14_3"]) ? 0:Convert.ToDouble(costdetails[i]["A14_3"])),
                                              new OleDbParameter("A14_4",Convert.IsDBNull(costdetails[i]["A14_4"]) ? 0:Convert.ToDouble(costdetails[i]["A14_4"]))

										  };
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);

            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 获取当月的双休日
        /// </summary>
        /// <param name="sdate"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
//        public DataTable GetOutRestWeeks(string sdate)
//        {
//            StringBuilder str = new StringBuilder();
//            str.AppendFormat(@"SELECT CASE WHEN A.XINGQI='7' THEN '六'
//                                            WHEN A.XINGQI='1' THEN '日'
//                                        END WEEKS,
//                                        A.RIQI,
//                                       CASE WHEN A.XINGQI='1' AND MOD(ROWNUM,2)=0 THEN 'A'||TO_CHAR(ROWNUM) 
//                                            WHEN A.XINGQI='1' AND MOD(ROWNUM,2)=1 THEN 'A'||TO_CHAR(ROWNUM+1)
//                                            WHEN A.XINGQI='7' AND MOD(ROWNUM,2)=1 THEN 'A'||TO_CHAR(ROWNUM)
//                                            WHEN A.XINGQI='7' AND MOD(ROWNUM,2)=0 THEN 'A'||TO_CHAR(ROWNUM+1)
//                                        END ZIDUAN
//                                FROM
//                                (
//                                SELECT TO_CHAR(to_date('{0}','yyyymmdd') + ROWNUM - 1, 'D') AS XINGQI,
//                                       TO_CHAR(to_date('{0}','yyyymmdd') + ROWNUM - 1,'MM-DD') AS RIQI
//                                       
//                                  FROM DUAL
//                                CONNECT BY ROWNUM <= ADD_MONTHS(TO_DATE('{0}','yyyymmdd'),1)-TO_DATE('{0}','yyyymmdd')
//                                ) A
//                                WHERE XINGQI IN (7,1)", sdate);

//            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
//        }


        public DataTable GetOutRestWeeks(string sdate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   substr(weeks,5) WEEKS,to_char(ST_DATE,'MM-DD') RIQI ,'A'||TO_CHAR(ROWNUM) ZIDUAN 
                                          FROM   PERFORMANCE.OUT_REST_BONUS_LIST
                                         WHERE   ST_DATE >= TO_DATE ('{0}', 'yyyymmdd')
                                                 AND ST_DATE < ADD_MONTHS (TO_DATE ('{0}', 'yyyymmdd'), 1)
                                                 and holiday_flag in ('假日','双休日')", sdate);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 职务岗位津贴统计表
        /// </summary>
        /// <param name="sdate"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public DataTable Getquattendance(string sdate, string deptfilter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                        select k.*,l.dept_name from (
                        select m.dept_code,m.name,m.job,m.zwts-nvl(n.zwts,0) zwts,m.subsidy,round(m.moneys-nvl(n.moneys,0),0) moneys,m.datetime from 
                        (SELECT    a.dept_code,b.NAME, b.job,
                                   a01
                                 - (a02 + a03 + a04 + a05 + a06 + a07 + a09 + b01 + b02 + b03 + c01
                                    + c02 + c03
                                   ) AS zwts,
                                 c.subsidy,
                                 ROUND (  (  a01
                                           - (  a02
                                              + a03
                                              + a04
                                              + a05
                                              + a06
                                              + a07
                                              + a09
                                              + b01
                                              + b02
                                              + b03
                                              + c01
                                              + c02
                                              + c03
                                             )
                                          )
                                        * c.subsidy
                                        / TO_NUMBER (TO_CHAR (LAST_DAY (date'{0}'), 'DD')),
                                        2
                                       ) moneys,
                                 to_char(date'{0}','yyyy-MM-dd') datetime
                            FROM hisdata.v_qu_attendance_dept_view a, hisdata.v_staff_dict b, hisdata.v_headship_dict c
                           WHERE b.job = c.job
                             AND SUBSTR (b.attr, 1, 1) <> '0'
                             AND a.emp_no = b.emp_no
                             AND a.year_month > date'{0}'
                             AND a.year_month < add_months(date'{0}',1)) m,

                        (SELECT   a.dept_code,a.memo job, a.attendance_value zwts, c.subsidy,
                                 ROUND (  a.attendance_value
                                        * c.subsidy
                                        / TO_NUMBER (TO_CHAR (LAST_DAY (date'{0}'), 'DD')),
                                        2
                                       ) moneys,
                                 to_char(date'{0}','yyyy-MM-dd') datetime
                            FROM hisdata.v_qu_attendance_dept a, hisdata.v_staff_dict b, hisdata.v_headship_dict c
                           WHERE a.memo = c.job
                             AND a.emp_no = b.emp_no
                             AND a.attendance_code = 'A07'
                             AND a.year_month >= date'{0}'
                             AND a.year_month < add_months(date'{0}',1)) n
                             where m.dept_code=n.dept_code(+) and m.job=n.job(+)
                           union all
                        SELECT   a.dept_code, b.NAME, '代'||a.memo job, a.attendance_value zwts, c.subsidy,
                                 ROUND (  a.attendance_value
                                        * c.subsidy
                                        / TO_NUMBER (TO_CHAR (LAST_DAY (date'{0}'), 'DD')),
                                        0
                                       ) moneys,
                                 to_char(date'{0}','yyyy-MM-dd') datetime
                            FROM hisdata.v_qu_attendance_dept a, hisdata.v_staff_dict b, hisdata.v_headship_dict c
                           WHERE a.memo = c.job
                             AND a.emp_no = b.emp_no
                             AND a.attendance_code = 'A07'
                             AND a.year_month >= date'{0}'
                             AND a.year_month < add_months(date'{0}',1)) k,comm.sys_dept_dict l
                             where k.dept_code=l.dept_code
                        ", sdate);

            if (!deptfilter.Equals(""))
            {
                str.AppendFormat(" {0}", deptfilter);
            }

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];

        }

        /// <summary>
        /// 获取门诊双休日工作量
        /// </summary>
        /// <param name="sdate"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public DataTable GetClinicWorkloadHoliday(string sdate, string deptfilter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   TRUNC (A.VISIT_DATE, 'MM') VISIT_DATE,
                                       A.VISIT_DEPT,
                                       B.DEPT_NAME,
                                       A.CLINIC_LABEL,
                                       A.CLINIC_TYPE,
                                       COUNT (CLINIC_TYPE) CON
                                FROM   HISDATA.CLINIC_MASTER A,
                                       HISDATA.DEPT_DICT B
                               WHERE   A.VISIT_DEPT=B.DEPT_CODE
                                       AND A.VISIT_DATE IN
                                             (SELECT   ST_DATE
                                                FROM   PERFORMANCE.OUT_REST_BONUS_LIST A
                                               WHERE   A.HOLIDAY_FLAG IN ('双休日', '假日') 
                                                       AND ST_DATE >=  TO_DATE('{0}','yyyymmdd')
                                                       AND ST_DATE <   ADD_MONTHS (TO_DATE ('{0}', 'yyyymmdd'),1)
                                             )
                                       AND A.CLINIC_TYPE IN ( '普通号','副主任号','正主任号','知名专家','特需专家','开药号')
                                       AND A.VISIT_TIME_DESC <>'昼夜'
                                       AND RETURNED_DATE IS NULL
                                       AND A.VISIT_DATE >= TO_DATE('{0}','yyyymmdd')
                                       AND A.VISIT_DATE <  ADD_MONTHS (TO_DATE ('{0}', 'yyyymmdd'),1)
                            GROUP BY   TRUNC (A.VISIT_DATE, 'MM'),
                                       A.VISIT_DEPT,
                                       B.DEPT_NAME,
                                       A.CLINIC_LABEL,
                                       A.CLINIC_TYPE
                            ORDER BY B.DEPT_NAME,A.CLINIC_LABEL,A.CLINIC_TYPE ", sdate);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];

        }

        /// <summary>
        /// 获取门诊专家（不含双休日）工作量
        /// </summary>
        /// <param name="sdate"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public DataTable GetClinicWorkload(string sdate, string deptfilter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"  SELECT   TRUNC (A.VISIT_DATE, 'MM') VISIT_DATE,
                                       A.VISIT_DEPT,
                                       B.DEPT_NAME,
                                       A.CLINIC_LABEL,
                                       A.CLINIC_TYPE,
                                       COUNT (CLINIC_TYPE) CON
                                FROM   HISDATA.CLINIC_MASTER A,
                                       HISDATA.DEPT_DICT B
                               WHERE   A.VISIT_DEPT=B.DEPT_CODE
                                       AND A.VISIT_DATE IN
                                             (SELECT   ST_DATE
                                                FROM   PERFORMANCE.OUT_REST_BONUS_LIST A
                                               WHERE   A.HOLIDAY_FLAG IN ('工作日') 
                                                       AND ST_DATE >=  TO_DATE('{0}','yyyymmdd')
                                                       AND ST_DATE <   ADD_MONTHS (TO_DATE ('{0}', 'yyyymmdd'),1)
                                             )
                                       AND A.CLINIC_TYPE IN ('副主任号','正主任号','知名专家','特需专家','特约专家')
                                       AND RETURNED_DATE IS NULL
                                       AND A.VISIT_DATE >= TO_DATE('{0}','yyyymmdd')
                                       AND A.VISIT_DATE <  ADD_MONTHS (TO_DATE ('{0}', 'yyyymmdd'),1)
                            GROUP BY   TRUNC (A.VISIT_DATE, 'MM'),
                                       A.VISIT_DEPT,
                                       B.DEPT_NAME,
                                       A.CLINIC_LABEL,
                                       A.CLINIC_TYPE
                            ORDER BY B.DEPT_NAME,A.CLINIC_LABEL,A.CLINIC_TYPE", sdate);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];

        }


    }
}
