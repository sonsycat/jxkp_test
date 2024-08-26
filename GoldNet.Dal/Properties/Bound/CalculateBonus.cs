using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Data.OleDb;
using System.Data.OracleClient;


namespace Goldnet.Dal
{
    public class CalculateBonus
    {
        /// <summary>
        /// "公开状态", "StateOpened" "已公开":"未公开
        /// "审批状态", "StateChecked" "已审批":"未审批
        ///  "归档状态", "STATEARCHIVED" "已归档":"未归档
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public DataTable GetBonusList(string state)
        {
            string sql = "";
            if (state == "test")
            {
                sql = " SELECT a.id,a.BonusName,case when a.StateOpened=0 then '未公开' else '公开' end STATE,to_char(a.CreateDate,'yyyy-MM-dd') CREATEDATE,";
                sql += " to_char(a.BEGINYEAR) || '年' ||to_char(a.BEGINMONTH) || '月'  AS BONUSDATE ";
                sql += " FROM PERFORMANCE.bonus_index a WHERE (Deleted = 0)  AND (StateChecked = 0)  AND (STATEARCHIVED = 0) order by BonusName desc";
            }
            else if (state == "open")
            {
                sql = " SELECT a.id,a.BonusName,case when a.STATECHECKED=0 then '未提交审批' else '已提交审批' end STATE,to_char(a.CreateDate,'yyyy-MM-dd') CREATEDATE,";
                sql += " to_char(a.BEGINYEAR) || '年' ||to_char(a.BEGINMONTH) || '月' AS BONUSDATE ";
                sql += " FROM PERFORMANCE.bonus_index a WHERE (Deleted = 0)  AND (STATEOPENED = 1) AND (STATEARCHIVED = 0)  order by CREATEDATE desc";
            }
            else if (state == "check")
            {
                sql = " SELECT a.id,a.BonusName,case when a.STATEARCHIVED=0 then '未归档' else '已归档' end STATE,to_char(a.CreateDate,'yyyy-MM-dd') CREATEDATE,";
                sql += " to_char(a.BEGINYEAR) || '年' ||to_char(a.BEGINMONTH) || '月' AS BONUSDATE ";
                sql += " FROM PERFORMANCE.bonus_index a WHERE (Deleted = 0)  AND (STATECHECKED = 1) AND (STATEARCHIVED = 0) order by CREATEDATE desc";

            }
            else if (state == "archived")
            {
                sql = " SELECT a.id,a.BonusName,'审核通过,可以发放奖金' as STATE,to_char(a.CreateDate,'yyyy-MM-dd') CREATEDATE,";
                sql += " to_char(a.BEGINYEAR) || '年' ||to_char(a.BEGINMONTH) || '月' AS BONUSDATE ";
                sql += " FROM PERFORMANCE.bonus_index a WHERE (Deleted = 0)  AND  (STATEARCHIVED = 1) order by CREATEDATE desc";
            }
            else
            {
                return BuildBonusList();
            }
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return BuildBonusList();
        }

        /// <summary>
        /// 获取已归档奖金列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetIndex()
        {
            string str = string.Format("select * from {0}.BONUS_INDEX where STATEARCHIVED=1", DataUser.PERFORMANCE);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        public DataTable GetIndexAll()
        {
            string str = string.Format("select * from {0}.BONUS_INDEX where STATEARCHIVED=1 ORDER BY ID DESC ", DataUser.PERFORMANCE);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        public DataTable GetPersonsType()
        {
            string str = string.Format("select * from {0}.BONUS_PSERSONS_TYPE ", DataUser.PERFORMANCE);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }
        public bool CheckLinShi(string ITEM_CODE)
        {
            string sqlguide = " select count(*) amount from hisdata.price_list where item_code='" + ITEM_CODE + "'";
            string ds = OracleOledbBase.ExecuteScalar(sqlguide.ToString()).ToString();
            int aa = Convert.ToInt32(ds);
            if (aa > 0)
            {


                return true;

            }

            return false;

        }
        /// <summary>
        /// 提取历史奖金人员
        /// </summary>
        /// <param name="oldindex"></param>
        /// <param name="newindex"></param>
        /// <param name="deptcode"></param>
        public void SaveOldIndexPersons(string oldindex, string newindex, string deptcode)
        {
            MyLists listtable = new MyLists();
            //删除奖金
            List lindex = new List();
            lindex.StrSql = string.Format("DELETE FROM {0}.BONUS_DETAIL WHERE INDEX_ID={1} and UNIT_CODE='{2}'", DataUser.PERFORMANCE, newindex, deptcode);
            lindex.Parameters = new OleDbParameter[] { };
            listtable.Add(lindex);

            List pindex = new List();
            pindex.StrSql = string.Format("DELETE FROM {0}.BONUS_PERSONS_DETAIL WHERE INDEX_ID={1} and DEPT_CODE='{2}'", DataUser.PERFORMANCE, newindex, deptcode);
            pindex.Parameters = new OleDbParameter[] { };
            listtable.Add(pindex);
            OracleOledbBase.ExecuteTranslist(listtable);

            string str = string.Format("select * from {0}.BONUS_DETAIL where INDEX_ID={1} and UNIT_CODE='{2}'", DataUser.PERFORMANCE, oldindex, deptcode);
            DataTable table = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            int id = OracleOledbBase.GetMaxID("id", DataUser.PERFORMANCE + ".BONUS_DETAIL");
            MyLists listtadd = new MyLists();
            int d = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.BONUS_DETAIL(", DataUser.PERFORMANCE);
                strSql.Append("id,INDEX_ID,UNIT_NAME,USER_NAME,USER_BONUS,UNIT_CODE,BANK_CODE,PERSONS_TYPE)");
                strSql.Append(" values (");
                strSql.Append("?,?,?,?,0,?,?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("id",id+d),
											  new OleDbParameter("INDEX_ID", newindex), 
                                              new OleDbParameter("UNIT_NAME",table.Rows[i]["UNIT_NAME"].ToString()),
                                               new OleDbParameter("USER_NAME",table.Rows[i]["USER_NAME"].ToString()),
                                                 new OleDbParameter("UNIT_CODE",table.Rows[i]["UNIT_CODE"].ToString()),
                                                 new OleDbParameter("BANK_CODE",table.Rows[i]["BANK_CODE"].ToString()),
                                                 new OleDbParameter("PERSONS_TYPE",table.Rows[i]["PERSONS_TYPE"].ToString())
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtadd.Add(listadd);
                d++;
            }
            OracleOledbBase.ExecuteTranslist(listtadd);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexid"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public DataTable GetDeptBonusList(string indexid, string deptfilter)
        {
            string depts = deptfilter == "" ? "" : " and unit_code in (" + deptfilter + ")";
            StringBuilder str = new StringBuilder();
            //            str.AppendFormat(@"SELECT m.unit_code,m.unit_name,
            //          m.sec_unit_name
            //       || '   应发：'
            //       || (SELECT SUM (n.unit_bonus)||'   实发：'||sum(bonus_persons_value)
            //             FROM (SELECT   a.unit_name, a.sec_unit_name,
            //                            nvl(SUM (a.unit_bonus),0) unit_bonus,
            //                            nvl(SUM (b.bonus_persons_value),0) bonus_persons_value
            //                       FROM {0}.bonus_unit a,
            //                            {0}.bonus_persons_detail b
            //                      WHERE a.unit_code = b.dept_code(+)
            //                        AND a.index_id = b.index_id(+)
            //                        AND a.index_id = {1} {2}
            //                   GROUP BY a.unit_name, a.sec_unit_name) n
            //            WHERE m.sec_unit_name = n.sec_unit_name) sec_unit_name,
            //       m.unit_bonus, m.bonus_persons_value
            //  FROM (SELECT   a.unit_code,a.unit_name, a.sec_unit_name, nvl(SUM (a.unit_bonus),0) unit_bonus,
            //                 nvl(SUM (b.bonus_persons_value),0) bonus_persons_value
            //            FROM {0}.bonus_unit a, {0}.bonus_persons_detail b
            //           WHERE a.unit_code = b.dept_code(+) AND a.index_id = b.index_id(+)
            //                 AND a.index_id = {1} {2}
            //        GROUP BY a.unit_code,a.unit_name, a.sec_unit_name) m",DataUser.PERFORMANCE,indexid,depts);
            str.AppendFormat(@"select t.*,decode(g.flags,1,'已提交','') flags from (select * from (SELECT   decode(grouping(a.sec_unit_name),1,'',sec_unit_name) sec_unit_name,
         decode(grouping(a.sec_unit_name)+grouping(a.unit_name),1,' 合计:',unit_name) unit_name,
          round(nvl(SUM (a.guide_value),0),2) unit_bonus,
                 round(nvl(SUM (b.bonus_persons_value),0),2) bonus_persons_value
            FROM (SELECT n.*, m.guide_value
  FROM {0}.bonus_value m, {0}.bonus_unit n
 WHERE m.index_id = n.index_id AND m.guide_code in (select GUIDE_CODE from performance.SET_BONUSGUIDE_BASE where BONUS_TYPE in ('3','4'))
       AND m.index_id = {1} and m.UNIT_CODE=n.UNIT_CODE) a, (select dept_code,sum(BONUS_PERSONS_VALUE) BONUS_PERSONS_VALUE,index_id from  PERFORMANCE.bonus_persons_detail group by dept_code,index_id) b
           WHERE a.unit_code = b.dept_code(+) AND a.index_id = b.index_id(+)
                 AND a.index_id = {1} {2}
        GROUP BY ROLLUP(a.sec_unit_name,a.unit_name)) where sec_unit_name is not null) t,(select h.*,l.dept_name from {0}.BONUS_PERSONS_SUBMIT h,{3}.sys_dept_dict l
where h.index_id={1} and H.DEPT_CODE=l.dept_code) g where t.unit_name=g.dept_name(+) order by unit_name desc", DataUser.PERFORMANCE, indexid, depts, DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        public DataTable GetBonusPersonType()
        {
            string str = string.Format("select * from {0}.BONUS_PSERSONS_TYPE", DataUser.PERFORMANCE);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }
        /// <summary>
        /// 奖金公开
        /// </summary>
        /// <param name="bonusid"></param>
        /// <returns></returns>
        public bool SetBonusOpen(string bonusid)
        {
            string sql = " update PERFORMANCE.bonus_index set STATEOPENED=1 where ID=" + bonusid + "";
            int i = OracleOledbBase.ExecuteNonQuery(sql);
            if (i >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 奖金提交审核
        /// </summary>
        /// <param name="bonusid"></param>
        /// <returns></returns>
        public bool SetBonusCheck(string bonusid)
        {
            string sql = " update PERFORMANCE.bonus_index set STATECHECKED=1 where ID=" + bonusid + "";
            int i = OracleOledbBase.ExecuteNonQuery(sql);
            if (i >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 奖金归档
        /// </summary>
        /// <param name="bonusid"></param>
        /// <returns></returns>
        public bool SetBonusArchived(string bonusid)
        {
            string sql = " update PERFORMANCE.bonus_index set STATEARCHIVED=1 where ID=" + bonusid + "";
            int i = OracleOledbBase.ExecuteNonQuery(sql);
            if (i >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 奖金审批不同意
        /// </summary>
        /// <param name="bonusid"></param>
        /// <returns></returns>
        public bool SetBonusCheckBack(string bonusid)
        {
            string sql = " update PERFORMANCE.bonus_index set STATECHECKED=0,STATEOPENED=0 where ID=" + bonusid + "";
            int i = OracleOledbBase.ExecuteNonQuery(sql);
            if (i >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 检查奖金的状态
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="bonusState"></param>
        /// <returns></returns>
        public bool IsBonusOpen(string bonusid, string bonusState)
        {
            string sql = " select * from PERFORMANCE.bonus_index where ID=" + bonusid + "";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string state = ds.Tables[0].Rows[0][bonusState].ToString();
                if (state == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据Id获得奖金主表数据
        /// </summary>
        /// <param name="bonusid"></param>
        /// <returns></returns>
        public DataTable GetBonusById(string bonusid, string tag)
        {
            string str = "select * from performance.bonus_selects";
            DataTable tablesel = OracleOledbBase.ExecuteDataSet(str).Tables[0];

            string sql = " select '考核月份' ITEM_NAME,BEGINYEAR || ";
            sql += " '年' || BEGINMONTH || '月' as ITEM_VALUE ";
            sql += " from performance.BONUS_INDEX  ";
            sql += " where ID=" + bonusid + " ";
            sql += " union all ";

            sql += " select '开始时间' ITEM_NAME,to_char(STARTDATE,'yyyy-mm-dd') ITEM_VALUE ";
            sql += " from performance.BONUS_INDEX  ";
            sql += " where ID=" + bonusid + "";
            sql += " union all ";

            sql += " select '结束时间' ITEM_NAME,to_char(ENDDATE,'yyyy-mm-dd') ITEM_VALUE ";
            sql += " from performance.BONUS_INDEX  ";
            sql += " where ID=" + bonusid + "";
            sql += " union all ";

            sql += " select '核算科室奖金' ITEM_NAME,to_char(sum(GUIDE_VALUE)) ITEM_VALUE ";
            sql += " from performance.SET_BONUSGUIDE_BASE a, ";
            sql += " performance.BONUS_VALUE b,PERFORMANCE.SET_ACCOUNTTYPEGROUP c  ";
            sql += " where A.GUIDE_CODE=B.GUIDE_CODE  and b.UNIT_TYPE =c.TYPE_CODE and c.GROUP_CODE='01'";
            sql += " and index_id=" + bonusid + " and bonus_type=3 ";
            sql += " union all ";

            sql += " select '平均奖科室奖金' ITEM_NAME,to_char(sum(GUIDE_VALUE)) ITEM_VALUE ";
            sql += " from performance.SET_BONUSGUIDE_BASE a, ";
            sql += " performance.BONUS_VALUE b,PERFORMANCE.SET_ACCOUNTTYPEGROUP c  ";
            sql += " where A.GUIDE_CODE=B.GUIDE_CODE  and b.UNIT_TYPE =c.TYPE_CODE and c.GROUP_CODE='02'";
            sql += " and index_id=" + bonusid + " and bonus_type=4 ";

            for (int i = 0; i < tablesel.Rows.Count; i++)
            {
                sql += " union all ";
                sql += string.Format(" select '{0}' ITEM_NAME,to_char(sum(GUIDE_VALUE)) ITEM_VALUE ", tablesel.Rows[i]["GUIDE_NAME"].ToString());
                sql += " from performance.BONUS_VALUE a ";
                sql += " where index_id=" + bonusid + string.Format(" and GUIDE_CODE='{0}' ", tablesel.Rows[i]["GUIDE_CODE"].ToString());
            }

            sql += " union all ";

            sql += " select '总奖金' ITEM_NAME,to_char(sum(ITEM_VALUE)) ITEM_VALUE ";
            sql += " from   ";
            sql += " (select '核算科室奖金' ITEM_NAME,sum(GUIDE_VALUE) ITEM_VALUE ";
            sql += " from performance.SET_BONUSGUIDE_BASE a, ";
            sql += " performance.BONUS_VALUE b ,PERFORMANCE.SET_ACCOUNTTYPEGROUP c";
            sql += " where A.GUIDE_CODE=B.GUIDE_CODE  and b.UNIT_TYPE =c.TYPE_CODE and c.GROUP_CODE='01'";
            sql += " and index_id=" + bonusid + " and bonus_type=3 ";
            sql += " union all ";

            sql += " select '平均奖科室奖金' ITEM_NAME,sum(GUIDE_VALUE) ITEM_VALUE ";
            sql += " from performance.SET_BONUSGUIDE_BASE a, ";
            sql += " performance.BONUS_VALUE b ,PERFORMANCE.SET_ACCOUNTTYPEGROUP c ";
            sql += " where A.GUIDE_CODE=B.GUIDE_CODE  and b.UNIT_TYPE =c.TYPE_CODE and c.GROUP_CODE='02'";
            sql += " and index_id=" + bonusid + " and bonus_type=4 )";

            sql += " union all ";

            sql += " SELECT   DISTINCT '内科系统平均奖人均' ITEM_NAME,to_char(guide_value) ITEM_VALUE";
            sql += " FROM   hospitalsys.guide_value a, PERFORMANCE.BONUS_INDEX b,PERFORMANCE.SET_ACCOUNTDEPTTYPE c";
            sql += " WHERE       a.tjyf = TO_CHAR (b.startdate, 'yyyymm')";
            sql += " AND c.st_date=b.startdate";
            sql += " and a.unit_code=c.dept_code";
            sql += " and c.dept_type in ('30001')";
            sql += " AND guide_code = '20219079'";
            sql += " AND b.id =" + bonusid + "";

            sql += " union all";

            sql += " SELECT   DISTINCT '外科系统平均奖人均' ITEM_NAME,to_char(guide_value) ITEM_VALUE";
            sql += " FROM   hospitalsys.guide_value a, PERFORMANCE.BONUS_INDEX b,PERFORMANCE.SET_ACCOUNTDEPTTYPE c";
            sql += " WHERE       a.tjyf = TO_CHAR (b.startdate, 'yyyymm')";
            sql += " AND c.st_date=b.startdate";
            sql += " and a.unit_code=c.dept_code";
            sql += " and c.dept_type in ('40001')";
            sql += " AND guide_code = '20219079'";
            sql += " AND b.id =" + bonusid + "";

            sql += " union all";

            sql += " SELECT   DISTINCT '医技系统平均奖人均' ITEM_NAME,to_char(guide_value) ITEM_VALUE";
            sql += " FROM   hospitalsys.guide_value a, PERFORMANCE.BONUS_INDEX b,PERFORMANCE.SET_ACCOUNTDEPTTYPE c";
            sql += " WHERE       a.tjyf = TO_CHAR (b.startdate, 'yyyymm')";
            sql += " AND c.st_date=b.startdate";
            sql += " and a.unit_code=c.dept_code";
            sql += " and c.dept_type in ('50001')";
            sql += " AND guide_code = '20219079'";
            sql += " AND b.id = " + bonusid + "  ";

            sql += " union all";

            sql += " SELECT   DISTINCT '院平均奖人均' ITEM_NAME,to_char(nvl(guide_value,0)) ITEM_VALUE";
            sql += " FROM   hospitalsys.guide_value a, PERFORMANCE.BONUS_INDEX b";
            sql += " WHERE       a.tjyf = TO_CHAR (b.startdate, 'yyyymm')";
            sql += " AND guide_code = '20204015'";
            sql += " AND b.id =" + bonusid + "";

            sql += " union all ";

            sql += " SELECT   DISTINCT '核算科室岗位津贴' ITEM_NAME,to_char(nvl(sum(guide_value),0)) ITEM_VALUE";
            sql += " FROM   hospitalsys.guide_value a, PERFORMANCE.BONUS_INDEX b,PERFORMANCE.SET_ACCOUNTDEPTTYPE c";
            sql += " WHERE       a.tjyf = TO_CHAR (b.startdate, 'yyyymm')";
            sql += " AND c.st_date=b.startdate";
            sql += " AND a.unit_code=c.dept_code";
            sql += " AND c.dept_type in ('30001','40001','50001','60001','80001')";
            sql += " AND guide_code = '20219084'";
            sql += " AND b.id = " + bonusid + "";

            sql += " union all ";

            sql += " SELECT   DISTINCT '平均奖科室岗位津贴' ITEM_NAME,to_char(nvl(sum(guide_value),0)) ITEM_VALUE";
            sql += " FROM   hospitalsys.guide_value a, PERFORMANCE.BONUS_INDEX b,PERFORMANCE.SET_ACCOUNTDEPTTYPE c";
            sql += " WHERE       a.tjyf = TO_CHAR (b.startdate, 'yyyymm')";
            sql += " AND c.st_date=b.startdate";
            sql += " AND a.unit_code=c.dept_code";
            sql += " AND c.dept_type in ('20001')";
            sql += " AND guide_code = '20219078'";
            sql += " AND b.id = " + bonusid + "";

            sql += " union all ";

            sql += " SELECT   DISTINCT '总岗位津贴' ITEM_NAME,to_char(nvl(sum(guide_value),0)) ITEM_VALUE";
            sql += " FROM   hospitalsys.guide_value a, PERFORMANCE.BONUS_INDEX b,PERFORMANCE.SET_ACCOUNTDEPTTYPE c";
            sql += " WHERE       a.tjyf = TO_CHAR (b.startdate, 'yyyymm')";
            sql += " AND c.st_date=b.startdate";
            sql += " AND a.unit_code=c.dept_code";
            sql += " AND c.dept_type in ('20001','30001','40001','50001','60001','80001')";
            sql += " AND (guide_code = '20219078' or guide_code = '20219084')";
            sql += " AND b.id =" + bonusid + "";

            sql += " union all ";

            sql += " select '创建时间' ITEM_NAME,to_char(CREATEDATE,'yyyy-mm-dd') ITEM_VALUE ";
            sql += " from performance.BONUS_INDEX  ";
            sql += " where ID=" + bonusid + " ";

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得奖金名称
        /// </summary>
        /// <param name="bonusid"></param>
        /// <returns></returns>
        public string GetBonusCaption(string bonusid, string columnName)
        {
            string sql = " select " + columnName + " from  performance.BONUS_INDEX where ID=" + bonusid + "";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0].Rows[0][columnName].ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 查找科室的在奖金月份所属的科室类型（未设置，平均奖，核算科室）
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="year"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public string CheckDeptAccountType(string deptcode, string year, string months)
        {
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            string date = year + months + "01";
            string sql = " select * from performance.SET_ACCOUNTDEPTTYPE where DEPT_CODE='" + deptcode + "' and ST_DATE=to_date('" + date + "','yyyymmdd')";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count > 0 & ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["DEPT_TYPE"].ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 检查科室是否已经设置
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public string CheckDeptTypeSet(string year, string months)
        {
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            string date = year + months + "01";
            string sql = " select * from performance.SET_ACCOUNTDEPTTYPE where  ST_DATE=to_date('" + date + "','yyyymmdd') and DEPT_TYPE in ('20001') ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "平均奖科室未设置";
            }
            sql = " select * from performance.SET_ACCOUNTDEPTTYPE where  ST_DATE=to_date('" + date + "','yyyymmdd') and    DEPT_TYPE != '10001'";
            ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "核算科室未设置";
            }
            return "";
        }
        /// <summary>
        /// 删除奖金
        /// </summary>
        /// <param name="bonusid"></param>
        public void DeleteBonusById(string bonusid)
        {
            //奖金主表
            string delbonusindex = " delete from performance.BONUS_INDEX where ID=" + bonusid;
            //奖金科室
            string delbonusunit = " delete from performance.BONUS_UNIT where INDEX_ID=" + bonusid;
            //奖金指标
            string delbonusguide = " delete from performance.BONUS_GUIDE where INDEX_ID=" + bonusid;
            //奖金人
            string delbonusdetail = " delete from performance.BONUS_DETAIL where INDEX_ID=" + bonusid;
            //奖金值
            string delbonusvalue = " delete from performance.BONUS_VALUE where INDEX_ID=" + bonusid;
            //人员明细
            string delpersondetail = "delete from performance.BONUS_PERSONS_DETAIL where index_id=" + bonusid;
            //提交信息
            string delpersonsubmit = "delete from performance.BONUS_PERSONS_SUBMIT where index_id=" + bonusid;
            MyLists listtable = new MyLists();
            //删除奖金
            List lindex = new List();
            lindex.StrSql = delbonusindex;
            lindex.Parameters = new OleDbParameter[] { };
            listtable.Add(lindex);

            List lunit = new List();
            lunit.StrSql = delbonusunit;
            lunit.Parameters = new OleDbParameter[] { };
            listtable.Add(lunit);

            List lguide = new List();
            lguide.StrSql = delbonusguide;
            lguide.Parameters = new OleDbParameter[] { };
            listtable.Add(lguide);

            List ldetail = new List();
            ldetail.StrSql = delbonusdetail;
            ldetail.Parameters = new OleDbParameter[] { };
            listtable.Add(ldetail);

            List lvalue = new List();
            lvalue.StrSql = delbonusvalue;
            lvalue.Parameters = new OleDbParameter[] { };
            listtable.Add(lvalue);

            List lpersons = new List();
            lpersons.StrSql = delpersondetail;
            lpersons.Parameters = new OleDbParameter[] { };
            listtable.Add(lpersons);

            List lpersonssub = new List();
            lpersonssub.StrSql = delpersonsubmit;
            lpersonssub.Parameters = new OleDbParameter[] { };
            listtable.Add(lpersonssub);

            OracleOledbBase.ExecuteTranslist(listtable);
        }
        private DataTable BuildBonusList()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID");
            DataColumn dcBONUSNAME = new DataColumn("BONUSNAME");
            DataColumn dcSTATE = new DataColumn("STATE");
            DataColumn dcCREATEDATE = new DataColumn("CREATEDATE");
            DataColumn dcBONUSDATE = new DataColumn("BONUSDATE");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcBONUSNAME, dcSTATE, dcCREATEDATE, dcBONUSDATE });
            return dt;
        }

        #region 奖金生成

        /// <summary>
        /// 检查奖金必备的指标是否存在
        /// </summary>
        /// <returns></returns>
        public bool CheckGuideBase()
        {
            string sqlguide = " select a.guide_code from PERFORMANCE.SET_BONUSGUIDE_BASE a left join ";
            sqlguide += " HOSPITALSYS.GUIDE_NAME_DICT b on ";
            sqlguide += " a.guide_code=b.guide_code ";
            sqlguide += " where b.guide_code is null ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlguide.ToString(), new OleDbParameter[] { });
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 执行计算奖金的指标存储过程
        /// </summary>
        /// <param name="beginYear">开始年</param>
        /// <param name="beginMonth">开始月</param>
        /// <param name="totalMonth">差月</param>
        public void RunGuide(int beginYear, int beginMonth, OleDbTransaction trans)
        {
            string strcreatguide = @"select * from (
                                    SELECT   GUIDE_CODE, GUIDE_NAME, BONUS_TYPE
                                      FROM   performance.SET_BONUSGUIDE_GATHER
                                    UNION
                                    SELECT   GUIDE_CODE, GUIDE_NAME, BONUS_TYPE
                                      FROM   performance.SET_BONUS_BALANCE
                                    UNION
                                    SELECT   DISTINCT guide_code, '', ''
                                      FROM   performance.SET_GUIDE_GATHERS a,   
                                             performance.SET_GUIDE_DEPT b
                                     WHERE   A.GUIDE_GATHER_CODE = B.GUIDE_GROUP_CODE) order by GUIDE_CODE";

            //获取奖金指标
            DataTable tableguide = OracleOledbBase.ExecuteDataSet(strcreatguide, new OleDbParameter[] { }).Tables[0];

            string tjyf = Convert.ToDateTime(beginYear.ToString() + "-" + (beginMonth).ToString() + "-01").ToString("yyyyMM");

            OleDbParameter[] param = {
                     new OleDbParameter("begindate",tjyf)
                };
            OracleOledbBase.RunProcedure("PERFORMANCE.ps_get_BONUS_BUSINESS", param);

            //计算指标
            for (int i = 0; i < tableguide.Rows.Count; i++)
            {
                OleDbParameter[] parameters = {
                     new OleDbParameter("TJNY",tjyf), 
                     new OleDbParameter("guidecode", tableguide.Rows[i]["guide_code"].ToString())
                };
                OracleOledbBase.RunProcedure("HOSPITALSYS.guide_value_add", parameters);
            }

            //触发过程
            string procedure = GetConfig.GetConfigString("RunProcedure");
            if (!procedure.Equals(""))
            {
                OleDbParameter[] parameterspor = {
                     new OleDbParameter("StartDate", Convert.ToDateTime(beginYear.ToString() + "-" + (beginMonth).ToString() + "-01").ToString("yyyyMMdd")), 
                     new OleDbParameter("EndDate", Convert.ToDateTime(beginYear.ToString() + "-" + (beginMonth).ToString() + "-01").AddMonths(1).AddDays(-1).ToString("yyyyMMdd"))
                };
                OracleOledbBase.RunProcedure(procedure, parameterspor);
            }
        }

        /// <summary>
        /// 执行计算奖金的指标存储过程(oracle驱动)
        /// </summary>
        /// <param name="beginYear">开始年</param>
        /// <param name="beginMonth">开始月</param>
        /// <param name="totalMonth">差月</param>
        public void RunGuide_oracle(int beginYear, int beginMonth)
        {
            string strcreatguide = @"select * from (
                                    SELECT   GUIDE_CODE, GUIDE_NAME, BONUS_TYPE
                                      FROM   performance.SET_BONUSGUIDE_GATHER
                                    UNION
                                    SELECT   GUIDE_CODE, GUIDE_NAME, BONUS_TYPE
                                      FROM   performance.SET_BONUS_BALANCE
                                    UNION
                                    SELECT   DISTINCT guide_code, '', ''
                                      FROM   performance.SET_GUIDE_GATHERS a,   
                                             performance.SET_GUIDE_DEPT b
                                     WHERE   A.GUIDE_GATHER_CODE = B.GUIDE_GROUP_CODE) order by GUIDE_CODE";

            //获取奖金指标
            DataTable tableguide = OracleBase.Query(strcreatguide).Tables[0];

            string tjyf = Convert.ToDateTime(beginYear.ToString() + "-" + (beginMonth).ToString() + "-01").ToString("yyyyMM");

            OracleParameter[] param = {
                     new OracleParameter("begindate",tjyf)
                };
            //OracleOledbBase.RunProcedure("PERFORMANCE.ps_get_BONUS_BUSINESS", param);

            OracleBase.RunProcedure("PERFORMANCE.ps_get_BONUS_BUSINESS", param);

            //计算指标
            for (int i = 0; i < tableguide.Rows.Count; i++)
            {
                OracleParameter[] parameters = {
                     new OracleParameter("TJNY",tjyf), 
                     new OracleParameter("guidecode", tableguide.Rows[i]["guide_code"].ToString())
                };
                OracleBase.RunProcedure("HOSPITALSYS.guide_value_add", parameters);
            }

            ////触发过程
            //string procedure = GetConfig.GetConfigString("RunProcedure");
            //if (!procedure.Equals(""))
            //{
            //    OracleParameter[] parameterspor = {
            //         new OracleParameter("StartDate", Convert.ToDateTime(beginYear.ToString() + "-" + (beginMonth).ToString() + "-01").ToString("yyyyMMdd")), 
            //         new OracleParameter("EndDate", Convert.ToDateTime(beginYear.ToString() + "-" + (beginMonth).ToString() + "-01").AddMonths(1).AddDays(-1).ToString("yyyyMMdd"))
            //    };
            //    OracleBase.RunProcedure(procedure, parameterspor);
            //}
        }

        /// <summary>
        /// 添加奖金主表
        /// </summary>
        /// <param name="bonusName"></param>
        /// <param name="beginYear"></param>
        /// <param name="beginMonth"></param>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddMainBonus(string bonusName, int beginYear, int beginMonth, int bonusid, OleDbTransaction trans)
        {
            int days = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(beginYear, beginMonth);
            string sqladd = @"INSERT INTO performance.Bonus_Index
                            (id,BonusName, BEGINYEAR, BEGINMONTH, Months, StartDate, EndDate, CreateDate)
                            VALUES (?,?,?,?,?,?,?,?)";

            OracleOledbBase.ExecuteScalar(trans, CommandType.Text, sqladd,
                new OleDbParameter("id", bonusid),
                new OleDbParameter("bonusName", bonusName),
                new OleDbParameter("beginyear", beginYear),
                new OleDbParameter("beginmonth", beginMonth),
                new OleDbParameter("months", 1),
                new OleDbParameter("startDate", new DateTime(beginYear, beginMonth, 1)),
                new OleDbParameter("endDate", new DateTime(beginYear, beginMonth, days)),
                new OleDbParameter("createDate", System.DateTime.Now)
                );
        }

        /// <summary>
        /// 添加奖金主表(oracle驱动)
        /// </summary>
        /// <param name="bonusName"></param>
        /// <param name="beginYear"></param>
        /// <param name="beginMonth"></param>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddMainBonus_oracle(string bonusName, int beginYear, int beginMonth, int bonusid)
        {
            int days = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(beginYear, beginMonth);
            string sdata = new DateTime(beginYear, beginMonth, 1).ToString("yyyy-MM-dd");
            string edata = new DateTime(beginYear, beginMonth, days).ToString("yyyy-MM-dd");
            string sqladd = string.Format(@"INSERT INTO performance.Bonus_Index
                            (id,BonusName, BEGINYEAR, BEGINMONTH, Months, StartDate, EndDate, CreateDate)
                            VALUES ({0},'{1}',{2},{3},{4},to_date('{5}','yyyy-mm-dd'),to_date('{6}','yyyy-mm-dd'),sysdate)", bonusid, bonusName, beginYear, beginMonth, 1, sdata, edata);
            OracleBase.ExecuteSql(sqladd);
            //OracleBase.ExecuteSql(sqladd,
            //    new OracleParameter("id", bonusid),
            //    new OracleParameter("bonusName", bonusName),
            //    new OracleParameter("beginyear", beginYear),
            //    new OracleParameter("beginmonth", beginMonth),
            //    new OracleParameter("months", 1),
            //    new OracleParameter("startDate", new DateTime(beginYear, beginMonth, 1)),
            //    new OracleParameter("endDate", new DateTime(beginYear, beginMonth, days)),
            //    new OracleParameter("createDate", System.DateTime.Now));
        }

        /// <summary>
        /// 添加奖金指标
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddBonusGuide(int bonusid, OleDbTransaction trans)
        {
            string strguide = String.Format(@"INSERT INTO PERFORMANCE.bonus_guide
                            select '{0}' AS index_id,GUIDE_CODE,GUIDE_NAME,BONUS_TYPE,SHOW_WIDTH,SHOW_STYLE,SHOW_COL from performance.SET_BONUSGUIDE_GATHER", bonusid);
            OracleOledbBase.ExecuteScalar(trans, CommandType.Text, strguide, new OleDbParameter[] { });
        }

        /// <summary>
        /// 添加奖金指标(oracle驱动)
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddBonusGuide_oracle(int bonusid)
        {
            string strguide = String.Format(@"INSERT INTO PERFORMANCE.bonus_guide
                            select '{0}' AS index_id,GUIDE_CODE,GUIDE_NAME,BONUS_TYPE,SHOW_WIDTH,SHOW_STYLE,SHOW_COL from performance.SET_BONUSGUIDE_GATHER", bonusid);
            //OracleOledbBase.ExecuteScalar(trans, CommandType.Text, strguide, new OleDbParameter[] { });
            OracleBase.ExecuteSql(strguide);
        }

        /// <summary>
        /// 添加奖金指标值
        /// </summary>
        /// <param name="beginYear"></param>
        /// <param name="beginMonth"></param>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddBonusValue(int beginYear, int beginMonth, int bonusid, OleDbTransaction trans)
        {
            string begintjyf = Convert.ToDateTime(beginYear.ToString() + "-" + (beginMonth).ToString() + "-01").ToString("yyyyMM");

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT guide_code, guide_name, guide_value, unit_code, d.dept_type unit_type,
                                       guide_type, bonus_type
                                  FROM (SELECT tjyf st_date, a.guide_code, b.guide_name, a.guide_value,
                                               a.unit_code, b.bonus_type guide_type,
                                               NVL (c.bonus_type, 0) bonus_type
                                          FROM hospitalsys.guide_value a,
                                               PERFORMANCE.set_bonusguide_gather b,
                                               PERFORMANCE.set_bonusguide_base c
                                         WHERE a.guide_code = b.guide_code
                                           AND a.tjyf = '{0}'
                                           AND a.guide_code = c.guide_code(+)) a,
                                       PERFORMANCE.set_accountdepttype d
                                 WHERE a.unit_code = d.dept_code(+) AND a.st_date = TO_CHAR (d.st_date(+),'yyyymm')", begintjyf);
            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString(), new OleDbParameter[] { });
            int id = OracleOledbBase.GetMaxID("id", "performance.BONUS_VALUE");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Performance.BONUS_VALUE(");
                strSql.Append("id,INDEX_ID,GUIDE_CODE,GUIDE_NAME,GUIDE_VALUE,UNIT_CODE,UNIT_TYPE,GUIDE_TYPE)");
                strSql.Append(" values (");
                strSql.Append("?,?,?,?,?,?,?,?)");
                OleDbParameter[] parameters = {
					new OleDbParameter("id", id),
					new OleDbParameter("INDEX_ID", bonusid),
                    new OleDbParameter("GUIDE_CODE", ds.Tables[0].Rows[i]["GUIDE_CODE"].ToString()),
                    new OleDbParameter("GUIDE_NAME", ds.Tables[0].Rows[i]["GUIDE_NAME"].ToString()),
                    new OleDbParameter("GUIDE_VALUE", ds.Tables[0].Rows[i]["GUIDE_VALUE"].ToString()),
                    new OleDbParameter("UNIT_CODE", ds.Tables[0].Rows[i]["UNIT_CODE"].ToString()),
                    new OleDbParameter("UNIT_TYPE", ds.Tables[0].Rows[i]["UNIT_TYPE"].ToString()),
					new OleDbParameter("GUIDE_TYPE", ds.Tables[0].Rows[i]["GUIDE_TYPE"].ToString())};
                OracleOledbBase.ExecuteScalar(trans, CommandType.Text, strSql.ToString(), parameters);
                id++;
                if (ds.Tables[0].Rows[i]["bonus_type"].ToString() == "1")
                {
                    string upindexstr = "update Performance.Bonus_Index set AVGBONUS=? where id=?";
                    OleDbParameter[] upindexparameters = {
                    new OleDbParameter("AVGBONUS", ds.Tables[0].Rows[i]["GUIDE_VALUE"].ToString()),
					new OleDbParameter("id", bonusid)};
                    OracleOledbBase.ExecuteScalar(trans, CommandType.Text, upindexstr, upindexparameters);
                }
                if (ds.Tables[0].Rows[i]["bonus_type"].ToString() == "2")
                {
                    string upindexstr = "update Performance.Bonus_Index set TOTALBONUS=? where id=?";
                    OleDbParameter[] upindexparameters = {
                    new OleDbParameter("TOTALBONUS", ds.Tables[0].Rows[i]["GUIDE_VALUE"].ToString()),
					new OleDbParameter("id", bonusid)};
                    OracleOledbBase.ExecuteScalar(trans, CommandType.Text, upindexstr, upindexparameters);
                }
            }

            //添加人员指标
            StringBuilder strq = new StringBuilder();
            strq.AppendFormat(@"
INSERT INTO Performance.BONUS_VALUE (id,
                                     INDEX_ID,
                                     GUIDE_CODE,
                                     GUIDE_NAME,
                                     GUIDE_VALUE,
                                     UNIT_CODE,
                                     UNIT_TYPE,
                                     GUIDE_TYPE)
   SELECT   ({0} + ROWNUM) AS id,
            '{2}' AS INDEX_ID,
            aa.guide_code,
            aa.guide_name,
            aa.guide_value,
            aa.unit_code,
            '' AS unit_type,
            aa.issame AS guide_type
     FROM   (SELECT   a.tjyf,
                      unit_code,
                      a.guide_code,
                      B.GUIDE_NAME,
                      B.issame,
                      a.guide_value
               FROM   hospitalsys.guide_value a,
                      HOSPITALSYS.GUIDE_NAME_DICT b
              WHERE       a.guide_code = b.guide_code
                      AND a.guide_type IN ('R','Z')
                      AND a.tjyf = '{1}') aa,
            (SELECT   DISTINCT guide_code
               FROM   Performance.SET_GUIDE_DEPT a, Performance.SET_GUIDE_GATHERS b
              WHERE   a.GUIDE_GROUP_CODE = b.GUIDE_GATHER_CODE
                      AND TO_CHAR (ST_DATE, 'yyyymm') = '{1}') bb
    WHERE   aa.guide_code = bb.guide_code", id + 1, begintjyf, bonusid);
            OracleOledbBase.ExecuteScalar(trans, CommandType.Text, strq.ToString(), new OleDbParameter[] { });

        }

        /// <summary>
        /// 添加奖金指标值(oracle 驱动)
        /// </summary>
        /// <param name="beginYear"></param>
        /// <param name="beginMonth"></param>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddBonusValue_oracle(int beginYear, int beginMonth, int bonusid)
        {
            string begintjyf = Convert.ToDateTime(beginYear.ToString() + "-" + (beginMonth).ToString() + "-01").ToString("yyyyMM");

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT guide_code, guide_name, guide_value, unit_code, d.dept_type unit_type,
                                       guide_type, bonus_type
                                  FROM (SELECT tjyf st_date, a.guide_code, b.guide_name, a.guide_value,
                                               a.unit_code, b.bonus_type guide_type,
                                               NVL (c.bonus_type, 0) bonus_type
                                          FROM hospitalsys.guide_value a,
                                               PERFORMANCE.set_bonusguide_gather b,
                                               PERFORMANCE.set_bonusguide_base c
                                         WHERE a.guide_code = b.guide_code
                                           AND a.tjyf = '{0}'
                                           AND a.guide_code = c.guide_code(+)) a,
                                       PERFORMANCE.set_accountdepttype d
                                 WHERE a.unit_code = d.dept_code(+) AND a.st_date = TO_CHAR (d.st_date(+),'yyyymm')", begintjyf);
            //DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString(), new OleDbParameter[] { });
            DataSet ds = OracleBase.Query(str.ToString());
            int id = OracleBase.GetMaxID("id", "performance.BONUS_VALUE");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string gv = ds.Tables[0].Rows[i]["GUIDE_VALUE"] == System.DBNull.Value ? "0" : ds.Tables[0].Rows[i]["GUIDE_VALUE"].ToString();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Performance.BONUS_VALUE(");
                strSql.Append("id,INDEX_ID,GUIDE_CODE,GUIDE_NAME,GUIDE_VALUE,UNIT_CODE,UNIT_TYPE,GUIDE_TYPE)");
                strSql.Append(" values (");
                strSql.Append(string.Format("{0},{1},'{2}','{3}',{4},'{5}','{6}','{7}')",
                    id, bonusid, ds.Tables[0].Rows[i]["GUIDE_CODE"].ToString(), ds.Tables[0].Rows[i]["GUIDE_NAME"].ToString(),
                    gv, ds.Tables[0].Rows[i]["UNIT_CODE"].ToString(),
                    ds.Tables[0].Rows[i]["UNIT_TYPE"].ToString(), ds.Tables[0].Rows[i]["GUIDE_TYPE"].ToString()
                    ));

                //OracleParameter[] parameters = {
                //    new OracleParameter("id", id),
                //    new OracleParameter("INDEX_ID", bonusid),
                //    new OracleParameter("GUIDE_CODE", ds.Tables[0].Rows[i]["GUIDE_CODE"].ToString()),
                //    new OracleParameter("GUIDE_NAME", ds.Tables[0].Rows[i]["GUIDE_NAME"].ToString()),
                //    new OracleParameter("GUIDE_VALUE", ds.Tables[0].Rows[i]["GUIDE_VALUE"].ToString()),
                //    new OracleParameter("UNIT_CODE", ds.Tables[0].Rows[i]["UNIT_CODE"].ToString()),
                //    new OracleParameter("UNIT_TYPE", ds.Tables[0].Rows[i]["UNIT_TYPE"].ToString()),
                //    new OracleParameter("GUIDE_TYPE", ds.Tables[0].Rows[i]["GUIDE_TYPE"].ToString())};
                OracleBase.ExecuteSql(strSql.ToString());
                id++;
                if (ds.Tables[0].Rows[i]["bonus_type"].ToString() == "1")
                {
                    string upindexstr = string.Format("update Performance.Bonus_Index set AVGBONUS={0} where id={1}", ds.Tables[0].Rows[i]["GUIDE_VALUE"].ToString(), bonusid);
                    //OracleParameter[] upindexparameters = {
                    //new OracleParameter("AVGBONUS", ds.Tables[0].Rows[i]["GUIDE_VALUE"].ToString()),
                    //new OracleParameter("id", bonusid)};
                    OracleBase.ExecuteSql(upindexstr);
                }
                if (ds.Tables[0].Rows[i]["bonus_type"].ToString() == "2")
                {
                    string upindexstr = string.Format("update Performance.Bonus_Index set TOTALBONUS={0} where id={1}", ds.Tables[0].Rows[i]["GUIDE_VALUE"].ToString(), bonusid);
                    //OracleParameter[] upindexparameters = {
                    //new OracleParameter("TOTALBONUS", ds.Tables[0].Rows[i]["GUIDE_VALUE"].ToString()),
                    //new OracleParameter("id", bonusid)};
                    OracleBase.ExecuteSql(upindexstr);
                }
            }

            //添加人员指标
            StringBuilder strq = new StringBuilder();
            strq.AppendFormat(@"
INSERT INTO Performance.BONUS_VALUE (id,
                                     INDEX_ID,
                                     GUIDE_CODE,
                                     GUIDE_NAME,
                                     GUIDE_VALUE,
                                     UNIT_CODE,
                                     UNIT_TYPE,
                                     GUIDE_TYPE)
   SELECT   ({0} + ROWNUM) AS id,
            '{2}' AS INDEX_ID,
            aa.guide_code,
            aa.guide_name,
            aa.guide_value,
            aa.unit_code,
            '' AS unit_type,
            aa.issame AS guide_type
     FROM   (SELECT   a.tjyf,
                      unit_code,
                      a.guide_code,
                      B.GUIDE_NAME,
                      B.issame,
                      a.guide_value
               FROM   hospitalsys.guide_value a,
                      HOSPITALSYS.GUIDE_NAME_DICT b
              WHERE       a.guide_code = b.guide_code
                      AND a.guide_type IN ('R','Z')
                      AND a.tjyf = '{1}') aa,
            (SELECT   DISTINCT guide_code
               FROM   Performance.SET_GUIDE_DEPT a, Performance.SET_GUIDE_GATHERS b
              WHERE   a.GUIDE_GROUP_CODE = b.GUIDE_GATHER_CODE
                      AND TO_CHAR (ST_DATE, 'yyyymm') = '{1}') bb
    WHERE   aa.guide_code = bb.guide_code", id + 1, begintjyf, bonusid);
            OracleBase.ExecuteSql(strq.ToString());

        }

        /// <summary>
        /// 添加奖金科室
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddBonusDept(int bonusid, OleDbTransaction trans)
        {
            string strdept = " insert into PERFORMANCE.BONUS_UNIT ";
            strdept += " (ID,INDEX_ID,UNIT_CODE,UNIT_NAME,UNIT_DIRECTOR,SEC_UNIT_CODE,UNIT_FUND,SEC_UNIT_FUND,UNIT_TYPE,SEC_UNIT_NAME,UNIT_BONUS,UNIT_TYPE_NAME,SORT_NO) ";
            strdept += " select (select nvl(max(id),0) from PERFORMANCE.BONUS_UNIT)+rownum as id,'" + bonusid + "', ";
            strdept += " DEPT_CODE,DEPT_NAME,DIRECTOR,DEPT_CODE_SECOND, ";
            strdept += " DEPT_FOUNDBONUS,DEPT_SEC_FOUNDBONUS, ";
            strdept += " DEPT_TYPE,DEPT_NAME_SECOND,UNIT_BONUS,TYPE_NAME,SORT_NO ";
            strdept += "  from PERFORMANCE.BONUS_UNIT_INFO ";

            OracleOledbBase.ExecuteScalar(trans, CommandType.Text, strdept, new OleDbParameter[] { });
        }

        /// <summary>
        /// 添加奖金科室(oracle 驱动)
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddBonusDept_oracle(int bonusid)
        {
            string strdept = " insert into PERFORMANCE.BONUS_UNIT ";
            strdept += " (ID,INDEX_ID,UNIT_CODE,UNIT_NAME,UNIT_DIRECTOR,SEC_UNIT_CODE,UNIT_FUND,SEC_UNIT_FUND,UNIT_TYPE,SEC_UNIT_NAME,UNIT_BONUS,UNIT_TYPE_NAME,SORT_NO) ";
            strdept += " select (select nvl(max(id),0) from PERFORMANCE.BONUS_UNIT)+rownum as id,'" + bonusid + "', ";
            strdept += " DEPT_CODE,DEPT_NAME,DIRECTOR,DEPT_CODE_SECOND, ";
            strdept += " DEPT_FOUNDBONUS,DEPT_SEC_FOUNDBONUS, ";
            strdept += " DEPT_TYPE,DEPT_NAME_SECOND,UNIT_BONUS,TYPE_NAME,SORT_NO ";
            strdept += "  from PERFORMANCE.BONUS_UNIT_INFO ";

            OracleBase.ExecuteSql(strdept);
        }

        /// <summary>
        /// 添加科室的人员
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddBonusPerson(int bonusid, OleDbTransaction trans)
        {
            string sqlPerson = "insert into performance.BONUS_DETAIL ";
            sqlPerson += " (ID,INDEX_ID,UNIT_NAME,USER_NAME,USER_BONUS,UNIT_CODE,USER_SEC_BONUS,BONUSMODULUS,staff_id,BANK_CODE,PERSONS_TYPE) ";
            sqlPerson += " select ";
            sqlPerson += " (select nvl(max(ID),0) from performance.BONUS_DETAIL)+rownum, ";
            sqlPerson += " '" + bonusid + "',ACCOUNT_DEPT_NAME,STAFF_NAME,USER_BONUS,DEPT_ID,0,BONUSMODULUS,staff_id,BANK_CODE,PERSONS_TYPE ";
            sqlPerson += " from performance.BONUS_PERSON_DETAIL ";

            OracleOledbBase.ExecuteScalar(trans, CommandType.Text, sqlPerson, new OleDbParameter[] { });
        }

        /// <summary>
        /// 添加科室的人员(oracle 驱动)
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="trans"></param>
        public void AddBonusPerson_oracle(int bonusid)
        {
            string sqlPerson = "insert into performance.BONUS_DETAIL ";
            sqlPerson += " (ID,INDEX_ID,UNIT_NAME,USER_NAME,USER_BONUS,UNIT_CODE,USER_SEC_BONUS,BONUSMODULUS,staff_id,BANK_CODE,PERSONS_TYPE) ";
            sqlPerson += " select ";
            sqlPerson += " (select nvl(max(ID),0) from performance.BONUS_DETAIL)+rownum, ";
            sqlPerson += " '" + bonusid + "',ACCOUNT_DEPT_NAME,STAFF_NAME,USER_BONUS,DEPT_ID,0,BONUSMODULUS,staff_id,BANK_CODE,PERSONS_TYPE ";
            sqlPerson += " from performance.BONUS_PERSON_DETAIL ";

            OracleBase.ExecuteSql(sqlPerson);
        }

        /// <summary>
        /// 添加科室留存
        /// </summary>
        /// <param name="beginYear"></param>
        /// <param name="beginMonth"></param>
        public void AddBonusBalance(int beginYear, int beginMonth)
        {
            StringBuilder str = new StringBuilder();
            //删除留存数据
            string delsql = "DELETE PERFORMANCE.BONUS_BALANCE WHERE OCCURRENCE_TIME =TO_DATE('" + beginYear.ToString() + "'||LPAD('" + beginMonth.ToString() + "',2,'0')||'01','yyyymmdd')";
            OracleBase.ExecuteSql(delsql.ToString());
            //添加留存数据
            str.AppendFormat(@"INSERT INTO PERFORMANCE.BONUS_BALANCE( UNIT_CODE,UNIT_CASH,OCCURRENCE_TIME) SELECT A.UNIT_CODE,A.GUIDE_VALUE,TO_DATE(A.TJYF||'01','yyyymmdd') 
                                FROM HOSPITALSYS.GUIDE_VALUE A,PERFORMANCE.SET_BONUS_BALANCE B
                                WHERE A.GUIDE_CODE =B.GUIDE_CODE
                                AND A.TJYF='{0}'||LPAD('{1}',2,0)", beginYear.ToString(), beginMonth.ToString());
            OracleBase.ExecuteSql(str.ToString());
        }

        #endregion

        /// <summary>
        /// 检查奖金是否存在
        /// </summary>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public bool CheckBonusIsExist(string years, string months)
        {
            string sqlCheck = " select * from performance.BONUS_INDEX where BEGINYEAR=" + years + " and BEGINMONTH=" + months + "";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlCheck.ToString(), new OleDbParameter[] { });
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 奖金科室类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeptType()
        {
            string str = string.Format("select * from {0}.SET_ACCOUNTTYPE where TYPE_CODE not in ('10001','20001')", DataUser.PERFORMANCE);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        //        /// <summary>
        //        /// 奖金报表
        //        /// </summary>
        //        /// <param name="bonusid"></param>
        //        /// <returns></returns>
        //        public DataTable GetBonusValue(string bonusid, string bonusType, string deptcode, string depttype)
        //        {
        //            depttype = depttype == "全部" ? "" : depttype;
        //            StringBuilder str = new StringBuilder();
        //            string strguideList = "";
        //            string strsumcol = "";
        //            string strgroup = "UNIT_TYPE_NAME";

        //            if (bonusType == "1")
        //            {
        //                //核算科室奖金表指标
        //                strguideList = String.Format(@"SELECT a.guide_code, a.guide_name
        //                          FROM performance.bonus_guide a
        //                               where a.index_id='{0}'
        //                           and a.guide_type='5'", bonusid);

        //                //核算科室奖金表分组合计设置
        //                strsumcol = String.Format(@"SELECT * FROM PERFORMANCE.SET_BONUSSUM_COL 
        //                                           where BONUS_TYPE='1'");
        //            }
        //            else
        //            {
        //                //平均奖科室奖金表指标
        //                strguideList = String.Format(@"SELECT a.guide_code, a.guide_name
        //                          FROM performance.bonus_guide a
        //                               where a.index_id='{0}'
        //                           and a.guide_type='6'", bonusid);

        //                //平均奖科室奖金表分组合计设置
        //                strsumcol = String.Format(@"SELECT * FROM PERFORMANCE.SET_BONUSSUM_COL 
        //                                           where BONUS_TYPE='2'");
        //            }
        //            //奖金表指标
        //            DataTable table = OracleOledbBase.ExecuteDataSet(strguideList).Tables[0];
        //            //奖金表分组合计设置
        //            DataTable tablesumcol = OracleOledbBase.ExecuteDataSet(strsumcol).Tables[0];

        //            for (int i = 0; i < tablesumcol.Rows.Count; i++)
        //            {
        //                if (tablesumcol.Rows[i]["CLASS_ID"].ToString() == "1")
        //                {
        //                    strgroup = tablesumcol.Rows[i]["guide_expression"].ToString();
        //                }
        //            }

        //            if (bonusType == "0")
        //            {
        //                //平均奖科室报表处理
        //                str.Append(@"select x.* from (
        //select  decode(substr(m.unit_name,length(m.unit_name)-1,2),'小计',substr(m.unit_name,0,length(m.unit_name)-2),m.unit_name) dept_names,m.* from 
        //( SELECT unit_type_name,UNIT_CODE,UNIT_NAME ");
        //                //处理奖金表合计
        //                for (int j = 0; j < table.Rows.Count; j++)
        //                {
        //                    bool findflag = true;

        //                    for (int i = 0; i < tablesumcol.Rows.Count; i++)
        //                    {
        //                        if (table.Rows[j]["guide_code"].ToString() == tablesumcol.Rows[i]["guide_code"].ToString() && tablesumcol.Rows[i]["CLASS_ID"].ToString() == "2")
        //                        {
        //                            //效益奖人均的合计部分重新计算
        //                            str.Append(@" ,ROUND (CASE WHEN UNIT_CODE IS NULL THEN " + tablesumcol.Rows[i]["guide_expression"].ToString() + " ELSE a" + tablesumcol.Rows[i]["guide_code"].ToString() + " END, 2) AS a" + tablesumcol.Rows[i]["guide_code"].ToString() + " ");
        //                            findflag = false;
        //                        }
        //                    }

        //                    if (findflag)
        //                    {
        //                        str.Append(" ,round(a" + table.Rows[j]["guide_code"].ToString() + ",2) as a" + table.Rows[j]["guide_code"].ToString());
        //                    }
        //                }
        //                str.Append(" FROM (");

        //                //合计名称处理
        //                str.Append("SELECT unit_type_name,DECODE(c.unit_code,'','',MIN(c.unit_code)) unit_code,CASE WHEN c.unit_code IS NULL AND " + strgroup + " IS NOT NULL THEN MIN (" + strgroup + ")||'小计' WHEN c.unit_code IS NULL AND " + strgroup + " IS NULL THEN '总 计' ELSE min(c.unit_name) END unit_name ");

        //            }
        //            else
        //            {
        //                //核算科室奖处理
        //                str.Append(@"select x.* from (
        //select  decode(substr(m.unit_name,length(m.unit_name)-1,2),'小计',substr(m.unit_name,0,length(m.unit_name)-2),m.unit_name) dept_names,m.* from 
        //( SELECT unit_type_name,UNIT_CODE,UNIT_NAME ");

        //                //处理奖金表合计
        //                for (int j = 0; j < table.Rows.Count; j++)
        //                {
        //                    bool findflag = true;
        //                    for (int i = 0; i < tablesumcol.Rows.Count; i++)
        //                    {
        //                        if (table.Rows[j]["guide_code"].ToString() == tablesumcol.Rows[i]["guide_code"].ToString() && tablesumcol.Rows[i]["CLASS_ID"].ToString() == "2")
        //                        {
        //                            //效益奖人均的合计部分重新计算
        //                            str.Append(@" ,ROUND (CASE WHEN UNIT_CODE IS NULL THEN " + tablesumcol.Rows[i]["guide_expression"].ToString() + " ELSE a" + tablesumcol.Rows[i]["guide_code"].ToString() + " END, 2) AS a" + tablesumcol.Rows[i]["guide_code"].ToString() + " ");
        //                            findflag = false;
        //                        }
        //                    }

        //                    if (findflag)
        //                    {
        //                        str.Append(" ,round(a" + table.Rows[j]["guide_code"].ToString() + ",2) as a" + table.Rows[j]["guide_code"].ToString());
        //                    }
        //                }
        //                str.Append(" FROM (");

        //                //合计名称处理
        //                str.Append("SELECT unit_type_name,DECODE(c.unit_code,'','',MIN(c.unit_code)) unit_code,CASE WHEN c.unit_code IS NULL AND " + strgroup + " IS NOT NULL THEN MIN (" + strgroup + ")||'小计' WHEN c.unit_code IS NULL AND " + strgroup + " IS NULL THEN '总 计' ELSE min(c.unit_name) END unit_name ");
        //            }

        //            for (int i = 0; i < table.Rows.Count; i++)
        //            {
        //                str.AppendFormat(", sum((SELECT round(nvl(max(b.guide_value),0),6) FROM {0}.bonus_value b WHERE b.index_id = '" + bonusid + "' ", DataUser.PERFORMANCE);
        //                str.Append(" and b.unit_code=c.unit_code AND b.guide_code ='");
        //                str.Append(table.Rows[i]["guide_code"].ToString() + "')) as a" + table.Rows[i]["guide_code"].ToString());
        //            }
        //            str.Append(" FROM performance.bonus_unit c,PERFORMANCE.SET_ACCOUNTTYPEGROUP d WHERE C.UNIT_TYPE =D.TYPE_CODE and c.index_id ='" + bonusid + "'");

        //            if (bonusType == "0")
        //            {
        //                //平均奖科室
        //                str.Append(" and D.GROUP_CODE='02' ");
        //            }
        //            else
        //            {
        //                //核算奖科室
        //                str.Append(" and D.GROUP_CODE='01' ");
        //            }

        //            if (depttype != "")
        //            {
        //                //科室类别选择后查询
        //                str.AppendFormat(" and c.unit_type ='{0}'", depttype);
        //            }

        //            if (deptcode != "")
        //            {
        //                //科室代码选择查询
        //                str.Append(" and UNIT_CODE in (" + deptcode + ")");
        //            }

        //            //分组合计处理
        //            str.Append(" group by ROLLUP(" + strgroup + ",c.unit_code))) m) x,");
        //            str.AppendFormat(@" (select a.* from comm.sys_dept_info a,performance.BONUS_INDEX b 
        //          where b.id='{0}' and to_char(b.STARTDATE,'yyyyMM') =to_char(a.DEPT_SNAP_DATE,'yyyyMM')) y
        //          where x.dept_names=y.dept_name(+) 
        //           order by x.unit_type_name,y.SORT_NO,y.DEPT_CODE_SECOND,x.unit_code,x.dept_names", bonusid);

        //            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
        //            return ds.Tables[0];
        //        }

        /// <summary>
        /// 奖金报表
        /// </summary>
        /// <param name="bonusid"></param>
        /// <returns></returns>
        public DataTable GetBonusValue(string bonusid, string bonusType, string deptcode, string depttype)
        {
            depttype = depttype == "全部" ? "" : depttype;
            StringBuilder str = new StringBuilder();
            string strguideList = "";
            string strsumcol = "";
            string strgroup = "UNIT_TYPE_NAME";

            if (bonusType == "1")
            {
                //核算科室奖金表指标
                strguideList = String.Format(@"SELECT a.guide_code, a.guide_name
                          FROM performance.bonus_guide a
                               where a.index_id='{0}'
                           and a.guide_type='5'", bonusid);

                //核算科室奖金表分组合计设置
                strsumcol = String.Format(@"SELECT * FROM PERFORMANCE.SET_BONUSSUM_COL 
                                           where BONUS_TYPE='1'");
            }
            else
            {
                //平均奖科室奖金表指标
                strguideList = String.Format(@"SELECT a.guide_code, a.guide_name
                          FROM performance.bonus_guide a
                               where a.index_id='{0}'
                           and a.guide_type='6'", bonusid);

                //平均奖科室奖金表分组合计设置
                strsumcol = String.Format(@"SELECT * FROM PERFORMANCE.SET_BONUSSUM_COL 
                                           where BONUS_TYPE='2'");
            }
            //奖金表指标
            DataTable table = OracleOledbBase.ExecuteDataSet(strguideList).Tables[0];
            //奖金表分组合计设置
            DataTable tablesumcol = OracleOledbBase.ExecuteDataSet(strsumcol).Tables[0];

            for (int i = 0; i < tablesumcol.Rows.Count; i++)
            {
                if (tablesumcol.Rows[i]["CLASS_ID"].ToString() == "1")
                {
                    strgroup = tablesumcol.Rows[i]["guide_expression"].ToString();
                }
            }

            if (bonusType == "0")
            {
                //平均奖科室报表处理
                str.Append(" SELECT UNIT_CODE,UNIT_NAME ");
                //处理奖金表合计
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    bool findflag = true;

                    for (int i = 0; i < tablesumcol.Rows.Count; i++)
                    {
                        if (table.Rows[j]["guide_code"].ToString() == tablesumcol.Rows[i]["guide_code"].ToString() && tablesumcol.Rows[i]["CLASS_ID"].ToString() == "2")
                        {
                            string bits = "";
                            if (tablesumcol.Rows[i]["COUNTTYPE"].ToString().ToUpper() == "ROUND")
                                bits = "," + tablesumcol.Rows[i]["BIT"].ToString();
                            else if (tablesumcol.Rows[i]["COUNTTYPE"].ToString().ToUpper() == "FLOOR")
                                bits = "";
                            //效益奖人均的合计部分重新计算
                            str.AppendFormat(@" ,{2} (CASE WHEN UNIT_CODE IS NULL THEN " + tablesumcol.Rows[i]["guide_expression"].ToString() + " ELSE a" + tablesumcol.Rows[i]["guide_code"].ToString() + " END {1}){0} AS a" + tablesumcol.Rows[i]["guide_code"].ToString() + " ", tablesumcol.Rows[i]["FLAGS"].ToString(), bits, tablesumcol.Rows[i]["COUNTTYPE"].ToString());
                            findflag = false;
                        }
                    }

                    if (findflag)
                    {
                        str.Append(" ,round(a" + table.Rows[j]["guide_code"].ToString() + ",4) as a" + table.Rows[j]["guide_code"].ToString());
                    }
                }
                str.Append(" FROM (");

                //合计名称处理
                str.Append("SELECT DECODE(c.sort_no||c.unit_code,'','',MIN(c.unit_code)) unit_code,sum(C.SORT_NO) SORT_NO,CASE WHEN c.sort_no||c.unit_code IS NULL AND " + strgroup + " IS NOT NULL THEN MIN (" + strgroup + ")||'小计' WHEN c.sort_no||c.unit_code IS NULL AND " + strgroup + " IS NULL THEN '合计' ELSE min(c.unit_name) END unit_name,UNIT_TYPE_NAME ");
            }
            else
            {
                //核算科室奖处理
                str.Append(" SELECT UNIT_CODE,UNIT_NAME ");

                //处理奖金表合计
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    bool findflag = true;
                    for (int i = 0; i < tablesumcol.Rows.Count; i++)
                    {
                        if (table.Rows[j]["guide_code"].ToString() == tablesumcol.Rows[i]["guide_code"].ToString() && tablesumcol.Rows[i]["CLASS_ID"].ToString() == "2")
                        {
                            string bits = "";

                            if (tablesumcol.Rows[i]["COUNTTYPE"].ToString().ToUpper().Equals("ROUND"))
                                bits = "," + tablesumcol.Rows[i]["BIT"].ToString();
                            else if (tablesumcol.Rows[i]["COUNTTYPE"].ToString().ToUpper().Equals("FLOOR"))
                                bits = "";
                            //效益奖人均的合计部分重新计算
                            str.AppendFormat(@" ,{2} (CASE WHEN UNIT_CODE IS NULL THEN " + tablesumcol.Rows[i]["guide_expression"].ToString() + " ELSE a" + tablesumcol.Rows[i]["guide_code"].ToString() + " END {1}){0} AS a" + tablesumcol.Rows[i]["guide_code"].ToString() + " ", tablesumcol.Rows[i]["FLAGS"].ToString(), bits, tablesumcol.Rows[i]["COUNTTYPE"].ToString());
                            findflag = false;
                        }
                    }

                    if (findflag)
                    {
                        str.Append(" ,round(a" + table.Rows[j]["guide_code"].ToString() + ",4) as a" + table.Rows[j]["guide_code"].ToString());
                    }
                }
                str.Append(" FROM (");

                //合计名称处理
                str.Append("SELECT DECODE(c.sort_no||c.unit_code,'','',MIN(c.unit_code)) unit_code,sum(C.SORT_NO) SORT_NO,CASE WHEN c.sort_no||c.unit_code IS NULL AND " + strgroup + " IS NOT NULL THEN MIN (" + strgroup + ")||'小计' WHEN c.sort_no||c.unit_code IS NULL AND " + strgroup + " IS NULL THEN '合计' ELSE min(c.unit_name) END unit_name,UNIT_TYPE_NAME ");
            }

            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.AppendFormat(", sum((SELECT round(nvl(max(b.guide_value),0),6) FROM {0}.bonus_value b WHERE b.index_id = '" + bonusid + "' ", DataUser.PERFORMANCE);
                str.Append(" and b.unit_code=c.unit_code AND b.guide_code ='");
                str.Append(table.Rows[i]["guide_code"].ToString() + "')) as a" + table.Rows[i]["guide_code"].ToString());
            }
            str.Append(" FROM performance.bonus_unit c,PERFORMANCE.SET_ACCOUNTTYPEGROUP d WHERE C.UNIT_TYPE =D.TYPE_CODE and c.index_id ='" + bonusid + "'");

            if (bonusType == "0")
            {
                //平均奖科室
                str.Append(" and D.GROUP_CODE='02' ");
            }
            else
            {
                //核算奖科室
                str.Append(" and D.GROUP_CODE='01' ");
            }

            if (depttype != "")
            {
                //科室类别选择后查询
                str.AppendFormat(" and c.unit_type ='{0}'", depttype);
            }

            if (deptcode != "")
            {
                //科室代码选择查询
                str.Append(" and UNIT_CODE in (" + deptcode + ")");
            }

            //分组合计处理
            str.Append(" group by ROLLUP(" + strgroup + ",c.sort_no||c.unit_code))");
            str.Append(" ORDER  BY SORT_NO");
            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonusId"></param>
        /// <param name="bonusType"></param>
        /// <param name="dt"></param>
        public void SetBonusDeptListExcel(string bonusId, string bonusType, DataTable dt)
        {
            string strcode = "";
            if (bonusType == "1")//1表示核算科室
            {
                strcode = String.Format(@"SELECT 'A' || a.guide_code guide_code, a.guide_name
                                      FROM performance.bonus_guide a
                                           where a.index_id='{0}'
                                       and a.guide_type='5'", bonusId);
            }
            else
            {
                strcode = String.Format(@"SELECT 'A' || a.guide_code guide_code, a.guide_name
                                      FROM performance.bonus_guide a
                                           where a.index_id='{0}'
                                       and a.guide_type='6'", bonusId);
            }
            DataTable table = OracleOledbBase.ExecuteDataSet(strcode).Tables[0];
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].ColumnName == "UNIT_CODE")
                {
                    dt.Columns[j].ColumnName = "科室代码";
                }
                if (dt.Columns[j].ColumnName == "UNIT_NAME")
                {
                    dt.Columns[j].ColumnName = "科室";
                }
                if (dt.Columns[j].ColumnName == "SEC_UNIT_NAME")
                {
                    dt.Columns[j].ColumnName = "二级科";
                }
                else
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (dt.Columns[j].ColumnName == table.Rows[i]["guide_code"].ToString())
                        {
                            dt.Columns[j].ColumnName = table.Rows[i]["guide_name"].ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获得核算科室人列表
        /// </summary>
        /// <param name="bonusId"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataTable GetAccountPersonDetail(string bonusId, string deptCode)
        {
            string sqlPerson = " SELECT 0 ID, '科室基金' NAME, round(nvl(unit_fund,0),2) bonus,round(nvl(sec_unit_fund,0),2) SEC_BONUS,'' BANK_CODE,0 as isSave ";
            sqlPerson += "  FROM PERFORMANCE.bonus_unit ";
            sqlPerson += "  WHERE index_id='" + bonusId + "' and UNIT_CODE = '" + deptCode + "' ";
            sqlPerson += "  UNION ALL ";
            sqlPerson += "  select ID,USER_NAME NAME,NVL(USER_BONUS,0) bonus,";
            sqlPerson += "  nvl(USER_SEC_BONUS,0) SEC_BONUS,BANK_CODE,1 AS issave";
            sqlPerson += "    from PERFORMANCE.bonus_detail where INDEX_ID='" + bonusId + "' and UNIT_CODE='" + deptCode + "'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlPerson.ToString());
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取科室总奖金
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetDeptBonus(string bonusid, string deptcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select round(sum(nvl(guide_value,0)),2) from {2}.BONUS_VALUE a where A.INDEX_ID={0} and a.UNIT_CODE='{1}' 
                               and a.guide_code in (select guide_code from {2}.SET_BONUSGUIDE_BASE b where b.BONUS_TYPE in ('3','4')) ", bonusid, deptcode, DataUser.PERFORMANCE);
            object ob = OracleOledbBase.ExecuteScalar(str.ToString());
            if (ob.ToString() == string.Empty)
                ob = 0;
            return ob.ToString();
        }

        /// <summary>
        /// 获取科室总奖金(20240729改)
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetDeptBonus_New(string bonusid, string deptcode,string tag)
        {
            StringBuilder str = new StringBuilder();
            string guidecode = "";
            if (tag == "1")
            {
                //核算科室
                guidecode = "20219114";
            }
            else
            {
                //平均奖科室
                guidecode = "20219104";
            }
            str.AppendFormat(@"select round(sum(nvl(guide_value,0)),2) from {2}.BONUS_VALUE where unit_code='{1}' and guide_code='{3}' AND index_id='{0}' ", bonusid, deptcode, DataUser.PERFORMANCE,guidecode);
            object ob = OracleOledbBase.ExecuteScalar(str.ToString());
            if (ob.ToString() == string.Empty)
                ob = 0;
            return ob.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetDeptPersonsBonus(string bonusid, string deptcode, string tag)
        {
            StringBuilder str = new StringBuilder();
            string guidecode = "";
            if (tag == "1")
            {
                //核算科室
                guidecode = "30219003";
            }
            else
            {
                //平均奖科室
                guidecode = "30219001";
            }
            str.AppendFormat("select  round(sum(nvl(BONUS_PERSONS_VALUE,0)),2) from {2}.BONUS_PERSONS_DETAIL where INDEX_ID={0} and dept_code='{1}' and bonus_persons_id='{3}'", bonusid, deptcode, DataUser.PERFORMANCE, guidecode);
            object ob = OracleOledbBase.ExecuteScalar(str.ToString());
            if (ob.ToString() == string.Empty)
                ob = 0;
            return ob.ToString();
        }
        /// <summary>
        /// 导出绩效
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetBonusPersons_DC(string bonusid, string deptcode)
        {
            //获取奖金日期
            string strperson = string.Format(@"SELECT   a.BEGINYEAR
                                                 || CASE
                                                       WHEN LENGTH (a.BEGINMONTH) < 2 THEN '0' || a.BEGINMONTH
                                                       ELSE to_char(a.BEGINMONTH)
                                                    END as st_date
                                          FROM   {0}.BONUS_INDEX a
                                         WHERE   id = {1}", DataUser.PERFORMANCE, bonusid);
            DataTable table = OracleOledbBase.ExecuteDataSet(strperson).Tables[0];
            string st_date = table.Rows[0][0].ToString();

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
SELECT B.UNIT_CODE 科室编码,
       B.UNIT_NAME 科室名称,
       B.USER_NAME 姓名,
       B.STAFF_ID STAFF_ID,
       ROUND(SUM(BONUS_PERSONS_VALUE), 2) AS 实发
  FROM PERFORMANCE.BONUS_INDEX A
 INNER JOIN PERFORMANCE.BONUS_DETAIL B
    ON A.ID = B.INDEX_ID
 INNER JOIN PERFORMANCE.BONUS_PERSONS_DETAIL C
    ON A.ID = C.INDEX_ID
   AND B.UNIT_CODE = C.DEPT_CODE
   AND B.ID = C.BONUS_DETAIL_ID
 WHERE TO_CHAR(A.STARTDATE, 'yyyymm') = '{0}'
   AND B.UNIT_CODE = '{1}'
 GROUP BY B.UNIT_CODE, B.UNIT_NAME, B.USER_NAME, B.STAFF_ID
UNION ALL
SELECT '',
       '合计',
       '',
       '',
       ROUND(SUM(BONUS_PERSONS_VALUE), 2) AS USER_TOTAL_BONUS
  FROM PERFORMANCE.BONUS_INDEX A
 INNER JOIN PERFORMANCE.BONUS_DETAIL B
    ON A.ID = B.INDEX_ID
 INNER JOIN PERFORMANCE.BONUS_PERSONS_DETAIL C
    ON A.ID = C.INDEX_ID
   AND B.UNIT_CODE = C.DEPT_CODE
   AND B.ID = C.BONUS_DETAIL_ID
 WHERE TO_CHAR(A.STARTDATE, 'yyyymm') = '{0}'
   AND B.UNIT_CODE = '{1}'
 GROUP BY B.UNIT_CODE, B.UNIT_NAME
UNION ALL
SELECT '', '', '主任/护士长签字：', '', NULL FROM DUAL", st_date, deptcode);
           
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 人员奖金明细
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetBonusPersons(string bonusid, string deptcode)
        {
            //获取奖金日期
            string strperson = string.Format(@"SELECT   a.BEGINYEAR
                                                 || CASE
                                                       WHEN LENGTH (a.BEGINMONTH) < 2 THEN '0' || a.BEGINMONTH
                                                       ELSE to_char(a.BEGINMONTH)
                                                    END as st_date
                                          FROM   {0}.BONUS_INDEX a
                                         WHERE   id = {1}", DataUser.PERFORMANCE, bonusid);
            DataTable table = OracleOledbBase.ExecuteDataSet(strperson).Tables[0];
            string st_date = table.Rows[0][0].ToString();

            //查询人员指标，用于显示
            string strdict = string.Format(@"
                                            SELECT   a.dept_code,
                                                     b.guide_code AS BONUS_PERSONS_ID,
                                                     C.GUIDE_NAME AS BONUS_PERSONS_NAME
                                              FROM   PERFORMANCE.SET_GUIDE_DEPT a,
                                                     PERFORMANCE.SET_GUIDE_GATHERS b,
                                                     HOSPITALSYS.GUIDE_NAME_DICT c
                                             WHERE       A.GUIDE_GROUP_CODE = B.GUIDE_GATHER_CODE
                                                     AND TO_CHAR (st_date, 'yyyymm') = '{0}'
                                                     AND a.dept_code = '{1}'
                                                     AND b.guide_code = c.guide_code", st_date, deptcode);

            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];

            //获取二次分配的所有指标
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT b.id,b.unit_name \"科室名称\",b.user_name \"名称\" ");
            str.AppendFormat(",b.BANK_CODE \"银行账号\" ");
            str.AppendFormat(",SUM (NVL(b.BONUS_PERSONS_VALUE,a.GUIDE_VALUE)) \"合计\"");
            str.AppendFormat(",b.staff_id STAFFID");
            string aa = "";
            string bb = "";
            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                if (tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString() != "1" && tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString() != "2")
                {
                    str.AppendFormat(" ,sum(DECODE (NVL(BONUS_PERSONS_ID,GUIDE_CODE), {0}, NVL (b.BONUS_PERSONS_VALUE, a.GUIDE_VALUE),0)) \"{1}\"", tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString(), tabledict.Rows[i]["BONUS_PERSONS_NAME"].ToString());
                    aa = aa + "'" + tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString() + "',";
                }
                //aa = aa + ",";
            }

            if (aa.Length > 0)
            {
                bb = " and guide_code in (" + aa.Substring(0, aa.Length - 1) + ")";
            }
            str.AppendFormat(@" from
                                (
                                select * from performance.bonus_value where index_id='{0}'  {2}
                                ) a,
                                (
                                select a.*,b.BONUS_PERSONS_VALUE,b.BONUS_PERSONS_ID 
                                  from performance.bonus_detail a,performance.BONUS_PERSONS_DETAIL b 
                                 where a.id=b.BONUS_DETAIL_ID(+) 
                                       and a.staff_id=b.staff_id(+)
                                       and a.index_id='{0}' 
                                       and a.unit_code='{1}'
                                ) b
                                where a.unit_code(+)=b.staff_id
                                  
                                group by b.id,b.unit_name,b.user_name,B.bank_code,B.persons_type,b.staff_id", bonusid, deptcode, bb);

            //for (int i = 0; i < tabledict.Rows.Count; i++)
            //{
            //    if (tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString() != "1" && tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString() != "2")
            //    {
            //        str.AppendFormat(" ,sum(DECODE (b.BONUS_PERSONS_ID, {0}, NVL(B.BONUS_PERSONS_VALUE,C.GUIDE_VALUE),0)) \"{1}\"", tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString(), tabledict.Rows[i]["BONUS_PERSONS_NAME"].ToString());
            //    }
            //}

            //            str.AppendFormat(@" FROM performance.bonus_detail a left join performance.BONUS_PERSONS_DETAIL b on A.ID=B.BONUS_DETAIL_ID LEFT JOIN
            //              PERFORMANCE.BONUS_VALUE C
            //              ON A.STAFF_ID=C.UNIT_CODE
            //              AND A.INDEX_ID=C.INDEX_ID and b.BONUS_PERSONS_ID=c.guide_code");
            //            str.AppendFormat(" WHERE a.index_id = '{0}' AND a.unit_code = '{1}' group by a.id,a.user_name,a.unit_name,a.bank_code, a.persons_type order by a.id", bonusid, deptcode);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="bonusindex"></param>
        /// <param name="deptcode"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public DataTable GetBonusPersonsSearch(string tag, string bonusindex, string deptcode, string typename)
        {
            //获取列表字段名称
            string strdict = string.Format("SELECT * FROM {0}.BONUS_PSERSONS_DICT WHERE BONUS_PERSONS_TYPE='{1}' order by bonus_persons_id", DataUser.PERFORMANCE, tag);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];
            //
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT B.id,B.unit_code DEPT_CODE,B.user_name \"姓名\", B.unit_name \"科室名称\"");
            str.AppendFormat(" ,B.BANK_CODE \"银行账号\",B.PERSONS_TYPE \"人员类别\"");
            str.AppendFormat(",SUM(BONUS_PERSONS_VALUE) \"合计\"");

            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                str.AppendFormat(" ,SUM(DECODE (A.BONUS_PERSONS_ID, {0}, BONUS_PERSONS_VALUE,0)) \"{1}\"", tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString(), tabledict.Rows[i]["BONUS_PERSONS_NAME"].ToString());
            }

            str.AppendFormat(" FROM PERFORMANCE.BONUS_PERSONS_DETAIL A,PERFORMANCE.bonus_detail B,{0}", "");
            str.AppendFormat(" (SELECT * FROM PERFORMANCE.BONUS_PSERSONS_DICT WHERE BONUS_PERSONS_TYPE='{0}') C ", tag);
            str.AppendFormat(" WHERE A.BONUS_DETAIL_ID = B.ID AND A.BONUS_PERSONS_ID = C.BONUS_PERSONS_ID {0}", "");
            if (bonusindex != "")
            {
                str.AppendFormat(" AND B.INDEX_ID='{0}'", bonusindex);
            }

            if (deptcode != "")
            {
                str.AppendFormat(" AND B.unit_code ='{0}'", deptcode);
            }

            if (typename != "")
            {
                str.AppendFormat(" AND B.PERSONS_TYPE='{0}'", typename);
            }

            str.AppendFormat(" GROUP BY B.ID,B.user_name,B.unit_name,B.unit_code,B.bank_code,B.persons_type {0}", "");
            str.AppendFormat(" ORDER BY B.unit_code,B.ID {0}", "");

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 全院奖金
        /// </summary>
        /// <param name="starmonth"></param>
        /// <param name="endmonth"></param>
        /// <returns></returns>
        public DataTable GetHospitalsysBonus(string starmonth, string endmonth)
        {
            string strdict = string.Format("select * from {0}.BONUS_PSERSONS_DICT where BONUS_PERSONS_ID not in (1,2) order by BONUS_PERSONS_ID", DataUser.PERFORMANCE);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT  a.unit_name \"科室名称\"");
            str.AppendFormat(",sum(BONUS_PERSONS_VALUE) \"合计\"");
            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                str.AppendFormat(" ,sum(DECODE (b.BONUS_PERSONS_ID, {0}, BONUS_PERSONS_VALUE,0)) \"{1}\"", tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString(), tabledict.Rows[i]["BONUS_PERSONS_NAME"].ToString());

            }

            str.AppendFormat(" FROM performance.BONUS_UNIT a,performance.BONUS_PERSONS_DETAIL b,performance.BONUS_INDEX c where A.UNIT_CODE=B.DEPT_CODE and a.INDEX_ID=b.INDEX_ID and a.INDEX_ID=c.id");
            str.AppendFormat(" and to_date( c.beginyear||c.beginmonth,'yyyyMM') >= to_date('{0}','yyyyMM')  AND  to_date( c.beginyear||c.beginmonth,'yyyyMM') <= to_date('{1}','yyyyMM') group by a.UNIT_CODE,a.unit_name", starmonth, endmonth);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 各类人员奖金
        /// </summary>
        /// <param name="starmonth"></param>
        /// <param name="endmonth"></param>
        /// <returns></returns>
        public DataTable GetPersonTypeBonus(string starmonth, string endmonth, string persontype)
        {
            string strdict = string.Format("select * from {0}.BONUS_PSERSONS_DICT where BONUS_PERSONS_ID not in (1,2) order by BONUS_PERSONS_ID", DataUser.PERFORMANCE);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT a.unit_name \"科室名称\", a.USER_NAME \"姓名\"");
            str.AppendFormat(",sum(BONUS_PERSONS_VALUE) \"合计\"");
            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                str.AppendFormat(" ,sum(DECODE (b.BONUS_PERSONS_ID, {0}, BONUS_PERSONS_VALUE,0)) \"{1}\"", tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString(), tabledict.Rows[i]["BONUS_PERSONS_NAME"].ToString());

            }

            str.AppendFormat(" FROM performance.BONUS_DETAIL a,performance.BONUS_PERSONS_DETAIL b,performance.BONUS_INDEX c where A.id=B.BONUS_DETAIL_ID and a.INDEX_ID=b.INDEX_ID and a.INDEX_ID=c.id ");
            if (!persontype.Equals(""))
            {
                str.AppendFormat(" and a.PERSONS_TYPE='{0}' ", persontype);
            }
            str.AppendFormat(" and to_date( c.beginyear||c.beginmonth,'yyyyMM') >= to_date('{0}','yyyyMM')  AND  to_date( c.beginyear||c.beginmonth,'yyyyMM') <= to_date('{1}','yyyyMM') group by a.unit_name,UNIT_CODE,a.USER_NAME order by a.UNIT_CODE", starmonth, endmonth);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonusid"></param>
        /// <returns></returns>
        public DataTable GetBonusPersons(string bonusid)
        {
            string strdict = string.Format("select * from {0}.BONUS_PSERSONS_DICT order by BONUS_PERSONS_ID", DataUser.PERFORMANCE);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT a.id,a.user_name \"姓名\", a.unit_name \"科室名称\"");
            str.AppendFormat(" ,a.BANK_CODE \"银行账号\",a.PERSONS_TYPE \"人员类别\"");
            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                if (tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString() != "1" && tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString() != "2")
                {
                    str.AppendFormat(" ,to_char(sum(DECODE (b.BONUS_PERSONS_ID, {0}, BONUS_PERSONS_VALUE,0))) \"{1}\"", tabledict.Rows[i]["BONUS_PERSONS_ID"].ToString(), tabledict.Rows[i]["BONUS_PERSONS_NAME"].ToString());
                }
            }
            str.AppendFormat(" FROM performance.bonus_detail a left join performance.BONUS_PERSONS_DETAIL b on A.ID=B.BONUS_DETAIL_ID");
            str.AppendFormat(" WHERE a.index_id = '{0}' group by a.id,a.user_name,a.UNIT_CODE,a.unit_name,a.bank_code, a.persons_type order by a.UNIT_CODE,a.id", bonusid);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonusId"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataTable GetAccountPersonEmptyDetail(string bonusId, string deptCode)
        {
            string sqlPerson = " SELECT 0 ID, '科室基金' NAME, round(nvl(unit_fund,0),2) bonus,round(nvl(sec_unit_fund,0),2) SEC_BONUS,'' BANK_CODE,0 as isSave ";
            sqlPerson += "  FROM PERFORMANCE.bonus_unit ";
            sqlPerson += "  WHERE index_id='" + bonusId + "' and UNIT_CODE = '" + deptCode + "' ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlPerson.ToString());
            return ds.Tables[0];
        }

        /// <summary>
        /// 获得平均奖科室人列表
        /// </summary>
        /// <param name="bonusId"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataTable GetAvgPersonDetail(string bonusId, string deptCode)
        {
            //string sqlAvgPerson = "select aa.*,nvl(bb.USER_BONUS,(select GUIDE_VALUE from performance.bonus_hospital_base)*aa.BONUSMODULUS) BONUS,bb.BANK_CODE ";
            //sqlAvgPerson += " from ( ";
            //sqlAvgPerson += " select a.dept_id,a.staff_name,a.BONUSMODULUS from ";
            //sqlAvgPerson += " (select case when length(MONTHS)=1 then '0' || MONTHS else MONTHS end months, ";
            //sqlAvgPerson += " YEARS,DEPT_ID,STAFF_NAME,BONUSMODULUS from performance.SET_AVERAGEBONUSDAYS where ISBONUS=1) a, ";
            //sqlAvgPerson += " (SELECT BEGINYEAR,case when length(BEGINMONTH)=1 then '0' || to_char(BEGINMONTH) else to_char(BEGINMONTH) end BEGINMONTH ";
            //sqlAvgPerson += " FROM performance.bonus_index WHERE ID = '" + bonusId + "') b, ";
            //sqlAvgPerson += " (select * from performance.SET_ACCOUNTDEPTTYPE where DEPT_TYPE='20001') c ";
            //sqlAvgPerson += " where a.YEARS=b.beginyear and a.MONTHS=b.BEGINMONTH ";
            //sqlAvgPerson += " and a.years=to_char(c.ST_DATE,'yyyy') and  ";
            //sqlAvgPerson += " a.months=to_char(c.ST_DATE,'mm') ";
            //sqlAvgPerson += " and a.dept_id=c.dept_code ";
            //sqlAvgPerson += " ) aa, ";
            //sqlAvgPerson += " (select * from performance.BONUS_DETAIL where INDEX_ID='" + bonusId + "') bb ";
            //sqlAvgPerson += " where aa.dept_id=bb.UNIT_CODE(+) and aa.staff_name=bb.USER_NAME(+)";
            //sqlAvgPerson += " and aa.dept_id='" + deptCode + "' ";
            string sqlAvgPerson = " select ID,USER_NAME NAME, BONUSMODULUS,round(nvl(USER_BONUS,0),2) bonus,bank_code ";
            sqlAvgPerson += " from PERFORMANCE.BONUS_DETAIL ";
            sqlAvgPerson += "  where INDEX_ID='" + bonusId + "' and UNIT_CODE='" + deptCode + "'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlAvgPerson.ToString());
            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptCode"></param>
        public void DelAllAccountPerson(string bonusid, string deptCode)
        {
            string delStr = " delete  from performance.BONUS_DETAIL where INDEX_ID='" + bonusid + "' and UNIT_CODE='" + deptCode + "'";
            OracleOledbBase.ExecuteScalar(delStr);
        }

        /// <summary>
        /// 删除指定的人
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptCode"></param>
        /// <param name="deptName"></param>
        /// <param name="PersonBouns"></param>
        public void DelPersonBonus(string personid)
        {

            string delStr = " delete  from performance.BONUS_DETAIL where ID='" + personid + "'";
            OracleOledbBase.ExecuteScalar(delStr);
            #region
            //MyLists listttrans = new MyLists();
            //List listdel = new List();
            //listdel.StrSql = delStr;
            //listdel.Parameters = new OleDbParameter[] { };
            //listttrans.Add(listdel);
            //for (int i = 0; i < PersonBouns.Length; i++)
            //{
            //    if (PersonBouns[i].Count > 0 && PersonBouns[i]["ISSAVE"].ToString()=="1")
            //    {
            //        StringBuilder str = new StringBuilder();
            //        if (PersonBouns[i]["BANK_CODE"] == null)
            //        {
            //            PersonBouns[i]["BANK_CODE"] = "";
            //        }
            //        StringBuilder strdetail = new StringBuilder();
            //        strdetail.Append("insert into Performance.BONUS_DETAIL(");
            //        strdetail.Append("id,index_id,unit_code,unit_name,user_name,user_bonus,BANK_CODE,USER_SEC_BONUS)");
            //        strdetail.Append(" values (");
            //        strdetail.Append("?,?,?,?,?,?,?,?)");
            //        OleDbParameter[] parameterdetail = {
            //        new OleDbParameter("id", PersonBouns[i]["ID"].ToString()),
            //        new OleDbParameter("INDEX_ID", bonusid),                    
            //        new OleDbParameter("unit_code", deptCode),
            //        new OleDbParameter("unit_name", deptName),
            //        new OleDbParameter("user_name", PersonBouns[i]["NAME"].ToString()),
            //        new OleDbParameter("user_bonus", double.Parse(PersonBouns[i]["BONUS"].ToString())),                      
            //            new OleDbParameter("BANK_CODE", PersonBouns[i]["BANK_CODE"]) ,
            //            new OleDbParameter("USER_SEC_BONUS", double.Parse(PersonBouns[i]["SEC_BONUS"])),  
            //            };
            //        List listdetail = new List();
            //        listdetail.StrSql = strdetail.ToString();
            //        listdetail.Parameters = parameterdetail;
            //        listttrans.Add(listdetail);

            //    }
            //}
            //OracleOledbBase.ExecuteTranslist(listttrans);
            #endregion
        }

        /// <summary>
        /// 获取本科室奖金发放人员
        /// </summary>
        /// <param name="index"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public bool GetDeptPersonsIsSubmit(string index, string deptcode)
        {
            string str = string.Format("select * from {0}.BONUS_PERSONS_SUBMIT where INDEX_ID=? AND DEPT_CODE=? ", DataUser.PERFORMANCE);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("INDEX_ID",index),
											  new OleDbParameter("DEPT_CODE",deptcode)  
										    };
            DataTable tb = OracleOledbBase.ExecuteDataSet(str.ToString(), parameteradd).Tables[0];
            if (tb.Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 判断此科室核算或平均奖是否提交
        /// </summary>
        /// <param name="index"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public bool GetBonusIsSubmit(string index, string deptcode,string tag)
        {
            string str = string.Format("select nvl(iscommit,0) from {0}.BONUS_SUBMIT where INDEX_ID=? AND DEPT_CODE=? and FLAGS=?", DataUser.PERFORMANCE);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("INDEX_ID",index),
											  new OleDbParameter("DEPT_CODE",deptcode),
											  new OleDbParameter("FLAGS",tag)  
										    };
            var result =OracleOledbBase.ExecuteScalar(str.ToString(), parameteradd).ToString();
            int res=2;
            if (!string.IsNullOrEmpty(result))
            { 
                res = int.Parse(result);
            }
            if (res == 1)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 获取本科室奖金发放人员
        /// </summary>
        /// <param name="index"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public bool GetDeptPersonsIsCommit(string index, string deptcode)
        {
            string str = string.Format("select * from {0}.BONUS_SUBMIT where INDEX_ID=? AND DEPT_CODE=? ", DataUser.PERFORMANCE);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("INDEX_ID",index),
											  new OleDbParameter("DEPT_CODE",deptcode)  
										    };
            DataTable tb = OracleOledbBase.ExecuteDataSet(str.ToString(), parameteradd).Tables[0];
            if (tb.Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

         /// <summary>
        /// 校验奖金数对不对
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptCode"></param>
        /// <param name="deptname"></param>
        /// <param name="PersonBouns"></param>
        public bool SaveAccountPersonBouns_JY(string bonusid, string deptCode, string deptname, Dictionary<string, string>[] PersonBouns)
        {
            //获取奖金日期
            string strperson = string.Format(@"SELECT   a.BEGINYEAR
                     || CASE
                           WHEN LENGTH (a.BEGINMONTH) < 2 THEN '0' || a.BEGINMONTH
                           ELSE to_char(a.BEGINMONTH)
                        END as st_date
              FROM   {0}.BONUS_INDEX a
             WHERE   id = {1}", DataUser.PERFORMANCE, bonusid);
            DataTable table = OracleOledbBase.ExecuteDataSet(strperson).Tables[0];
            string st_date = table.Rows[0][0].ToString();

            //删除科室类别
            MyLists listtable = new MyLists();
            List listcenterdict = new List();
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            string strdict = string.Format(@"
                                            SELECT   a.dept_code,
                                                     b.guide_code AS BONUS_PERSONS_ID,
                                                     C.GUIDE_NAME AS BONUS_PERSONS_NAME
                                              FROM   PERFORMANCE.SET_GUIDE_DEPT a,
                                                     PERFORMANCE.SET_GUIDE_GATHERS b,
                                                     HOSPITALSYS.GUIDE_NAME_DICT c
                                             WHERE       A.GUIDE_GROUP_CODE = B.GUIDE_GATHER_CODE
                                                     AND TO_CHAR (st_date, 'yyyymm') = '{0}'
                                                     AND a.dept_code = '{1}'
                                                     AND b.guide_code = c.guide_code 
                                            UNION ALL
                                            SELECT   '1' dept_code,
                                                     '1' BONUS_PERSONS_ID,
                                                     '银行账号' BONUS_PERSONS_NAME
                                              FROM   DUAL 
                                            ", st_date, deptCode);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];
            decimal aa = 0;
            for (int i = 0; i < PersonBouns.Length - 1; i++)
            {
                for (int j = 0; j < tabledict.Rows.Count; j++)
                {
                    if (tabledict.Rows[j]["BONUS_PERSONS_NAME"].ToString() != "银行账号")
                    {

                        string bonuspersonsname = PersonBouns[i][tabledict.Rows[j]["BONUS_PERSONS_NAME"].ToString()].ToString() == "" ? "" : PersonBouns[i][tabledict.Rows[j]["BONUS_PERSONS_NAME"].ToString()].ToString();
                        if (bonuspersonsname.Trim().Equals(""))
                        {
                            bonuspersonsname = "0";
                        }
                        aa = aa + Convert.ToDecimal(bonuspersonsname);
                    }
                }




            }

            string strdict1 = string.Format(@"SELECT  SUM(GUIDE_VALUE) GUIDE_VALUE
  FROM (SELECT A.UNIT_CODE, A.GUIDE_VALUE
          FROM HOSPITALSYS.GUIDE_VALUE A
         WHERE A.TJYF = '{0}'
           AND A.GUIDE_CODE = '20219114'
        AND A.UNIT_CODE IN (SELECT DEPT_CODE
                                 FROM PERFORMANCE.SET_ACCOUNTDEPTTYPE B
                                WHERE TO_CHAR(B.ST_DATE, 'yyyymm') =  '{0}'
                                  AND B.DEPT_TYPE <> '20001')
        UNION ALL
        SELECT A.UNIT_CODE, A.GUIDE_VALUE
          FROM HOSPITALSYS.GUIDE_VALUE A
         WHERE A.TJYF = '{0}'
           AND A.GUIDE_CODE = '20219104'
 AND A.UNIT_CODE IN (SELECT DEPT_CODE
                                 FROM PERFORMANCE.SET_ACCOUNTDEPTTYPE B
                                WHERE TO_CHAR(B.ST_DATE, 'yyyymm') =  '{0}'
                                  AND B.DEPT_TYPE = '20001'))
 WHERE UNIT_CODE = '{1}'", st_date, deptCode);
            DataTable tabledict1 = OracleOledbBase.ExecuteDataSet(strdict1).Tables[0];
            string num = tabledict1.Rows[0][0].ToString();
            decimal num1 = Convert.ToDecimal(num);
            decimal aa1 = Convert.ToDecimal(aa);
            if (num1 == aa1)
            {
                return false;
            }
            else
            {
                return true;
            }
//            string strdict1 = string.Format(@"SELECT  SUM(GUIDE_VALUE) GUIDE_VALUE
//  FROM (SELECT A.UNIT_CODE, A.GUIDE_VALUE
//          FROM HOSPITALSYS.GUIDE_VALUE A
//         WHERE A.TJYF = '{0}'
//           AND A.GUIDE_CODE = '20219051')  ---更改对应的奖金编码
// WHERE UNIT_CODE = '{1}'", st_date, deptCode);
//            DataTable tabledict1 = OracleOledbBase.ExecuteDataSet(strdict1).Tables[0];
//            string num = tabledict1.Rows[0][0].ToString();

//            string k_jj = string.Format(@"SELECT NVL((SELECT MONEY
//             FROM PERFORMANCE.KEJIJIN
//            WHERE TO_CHAR(AWARD_DATE, 'yyyymm') = '{0}'
//              AND DEPT_CODE = '{1}'),
//           0)
//  FROM DUAL", st_date, deptCode);
//            DataTable tabledict2 = OracleOledbBase.ExecuteDataSet(k_jj).Tables[0];
//            string num_j = tabledict2.Rows[0][0].ToString();


//            decimal num1 = Convert.ToDecimal(num);
//            decimal aa1 = Convert.ToDecimal(aa);
//            decimal k_jj2 = Convert.ToDecimal(num_j);
//            if (num1 - k_jj2 == aa1)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }
        }
        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptcode"></param>
        public void GetBonusP(string bonusid, string deptcode)
        {
            //获取奖金日期
            string strperson = string.Format(@"SELECT   a.BEGINYEAR
         || CASE
               WHEN LENGTH (a.BEGINMONTH) < 2 THEN '0' || a.BEGINMONTH
               ELSE to_char(a.BEGINMONTH)
            END as st_date
  FROM   {0}.BONUS_INDEX a
 WHERE   id = 1", DataUser.PERFORMANCE, bonusid);
            DataTable table = OracleOledbBase.ExecuteDataSet(strperson).Tables[0];
            string st_date = table.Rows[0][0].ToString();

            string delsql = string.Format(@"delete from PERFORMANCE.BONUS_PERSONS_DETAIL where INDEX_ID={0} and DEPT_CODE='{1}'", bonusid, deptcode);
            OracleOledbBase.ExecuteNonQuery(delsql);

            //插入人员指标值
            string sql = string.Format(@"
INSERT INTO {0}.BONUS_PERSONS_DETAIL (BONUS_DETAIL_ID,
                                  BONUS_PERSONS_ID,
                                  BONUS_PERSONS_VALUE,
                                  INDEX_ID,
                                  DEPT_CODE)
   SELECT   b.id AS BONUS_DETAIL_ID,
            b.guide_code AS BONUS_PERSONS_ID,
            b.guide_value AS BONUS_PERSONS_VALUE,
            {1} AS INDEX_ID,
            b.dept_code
     FROM   (SELECT   a.dept_code, b.guide_code
               FROM   {0}.SET_GUIDE_DEPT a, {0}.SET_GUIDE_GATHERS b
              WHERE       A.GUIDE_GROUP_CODE = B.GUIDE_GATHER_CODE
                      AND TO_CHAR (st_date, 'yyyymm') = '{2}'
                      AND a.dept_code = '{3}') a,
            (SELECT   b.id,
                      a.guide_code,
                      a.guide_value,
                      b.unit_code dept_code
               FROM   {0}.bonus_value a, {0}.bonus_detail b
              WHERE       A.UNIT_CODE = B.STAFF_ID
                      AND B.UNIT_CODE = '{3}'
                      AND A.INDEX_ID = B.INDEX_ID
                      AND a.INDEX_ID = {1}) b
    WHERE   a.dept_code = b.dept_code AND a.guide_code = b.guide_code", DataUser.PERFORMANCE, bonusid, st_date, deptcode);
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 提交人员奖金
        /// </summary>
        /// <param name="month"></param>
        /// <param name="itemcode"></param>
        public void SubmitBonusPersons(string index, string deptcode, string opertor)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.BONUS_PERSONS_SUBMIT WHERE INDEX_ID=? AND DEPT_CODE=? ", DataUser.PERFORMANCE);

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { new OleDbParameter("", index), new OleDbParameter("", deptcode) };
            listtable.Add(listDel);
            //
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"insert into {0}.BONUS_PERSONS_SUBMIT(INDEX_ID,DEPT_CODE,FLAGS,SUBMIT_PERSONS,SUBMIT_DATE) values (?,?,?,?,SYSDATE)", DataUser.PERFORMANCE);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("INDEX_ID",index),
											  new OleDbParameter("DEPT_CODE",deptcode),
											  new OleDbParameter("FLAGS",'1'),
											  new OleDbParameter("SUBMIT_PERSONS",opertor)
											  
										  };
            List listadd = new List();
            listadd.StrSql = str;
            listadd.Parameters = parameteradd;
            listtable.Add(listadd);
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 提交人员奖金
        /// </summary>
        /// <param name="month"></param>
        /// <param name="itemcode"></param>
        public void CommitBonusPersons(string index, string deptcode, string opertor,string tag)
        {
            MyLists listtable = new MyLists();
            StringBuilder strDel = new StringBuilder();
            strDel.AppendFormat("DELETE FROM {0}.BONUS_SUBMIT WHERE INDEX_ID=? AND DEPT_CODE=? and flags=? ", DataUser.PERFORMANCE);

            List listDel = new List();
            listDel.StrSql = strDel;
            listDel.Parameters = new OleDbParameter[] { new OleDbParameter("", index), new OleDbParameter("", deptcode), new OleDbParameter("", tag) };
            listtable.Add(listDel);
            //
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"insert into {0}.BONUS_SUBMIT(INDEX_ID,DEPT_CODE,FLAGS,SUBMIT_PERSONS,SUBMIT_DATE,ISCOMMIT) values (?,?,?,?,SYSDATE,?)", DataUser.PERFORMANCE);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("INDEX_ID",index),
											  new OleDbParameter("DEPT_CODE",deptcode),
											  new OleDbParameter("FLAGS",tag),
											  new OleDbParameter("SUBMIT_PERSONS",opertor),
											  new OleDbParameter("ISCOMMIT",'1')
											  
										  };
            List listadd = new List();
            listadd.StrSql = str;
            listadd.Parameters = parameteradd;
            listtable.Add(listadd);
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 科室奖金取消
        /// </summary>
        /// <param name="months"></param>
        /// <param name="itemcode"></param>
        /// <param name="opertor"></param>
        public void CancleBonusPersons(string index, string deptcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM {0}.BONUS_PERSONS_SUBMIT WHERE INDEX_ID=? AND DEPT_CODE=? ", DataUser.PERFORMANCE);
            OleDbParameter[] parameteradd = {
											  new OleDbParameter("INDEX_ID",index),
											  new OleDbParameter("DEPT_CODE",deptcode)  
										  };

            OracleOledbBase.ExecuteNonQuery(str.ToString(), parameteradd);
        }

        /// <summary>
        /// 奖金人员提交信息
        /// </summary>
        /// <param name="months"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetSubmitBonusPersons(string index, string deptfilter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.dept_code, a.dept_name,decode(b.FLAGS,'','','已提交') flags,b.SUBMIT_PERSONS from (
         SELECT  a.UNIT_CODE dept_code, a.UNIT_NAME dept_name,a.index_id
           FROM {0}.BONUS_UNIT a                
          WHERE a.index_id = {1}", DataUser.PERFORMANCE, index);
            if (deptfilter != "")
            {
                str.AppendFormat(" and a.UNIT_CODE in ({0})", deptfilter);
            }
            str.AppendFormat(@" ORDER BY a.UNIT_CODE) a left join 
       {0}.BONUS_PERSONS_SUBMIT b on a.index_id=b.index_id and a.dept_code=b.dept_code", DataUser.PERFORMANCE);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        //奖金二次分配是否提交查询
        public DataTable GetSubmitBonus(string stdate)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT {1} st_date,a.index_id,
         a.dept_code,
         b.dept_name,
         a.flags,
         a.iscommit
    FROM performance.bonus_submit a, comm.sys_dept_dict b
   WHERE     a.dept_code = b.dept_code(+)
         AND index_id = (SELECT id stdate
                           FROM {0}.bonus_index
                          WHERE TO_CHAR (startdate, 'yyyymm') = '{1}')
ORDER BY b.sort_no", DataUser.PERFORMANCE, stdate);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        //奖金二次分配是否提交查询导出
        public DataTable GetSubmitBonus(string stdate,int index)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT {1} st_date,a.index_id,
         a.dept_code,
         b.dept_name,
         case when a.flags='1' then '核算' else '平均' end flags ,
         case when a.iscommit='1' then '是' else '否' end iscommit
    FROM performance.bonus_submit a, comm.sys_dept_dict b
   WHERE     a.dept_code = b.dept_code(+)
         AND index_id = (SELECT id stdate
                           FROM {0}.bonus_index
                          WHERE TO_CHAR (startdate, 'yyyymm') = '{1}')
ORDER BY b.sort_no", DataUser.PERFORMANCE, stdate);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        //删除已提交记录
        public bool DeleteGetSubmitBonus(string stdate,string index_id,string dept_code,string flags)
        {
            StringBuilder sql = new StringBuilder();
            sql = sql.AppendFormat(@"delete performance.bonus_submit where index_id='{0}' AND dept_code='{1}' and flags={2} ",index_id,dept_code,flags);
            return (OracleOledbBase.ExecuteSql(sql.ToString()));
        }

        /// <summary>
        /// 保存核算科室中人的奖金
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptCode"></param>
        /// <param name="PersonBouns"></param>
        public void SaveAccountPersonBouns(string bonusid, string deptCode, string deptname, Dictionary<string, string>[] PersonBouns)
        {
            //获取奖金日期
            string strperson = string.Format(@"SELECT   a.BEGINYEAR
                     || CASE
                           WHEN LENGTH (a.BEGINMONTH) < 2 THEN '0' || a.BEGINMONTH
                           ELSE to_char(a.BEGINMONTH)
                        END as st_date
              FROM   {0}.BONUS_INDEX a
             WHERE   id = {1}", DataUser.PERFORMANCE, bonusid);
            DataTable table = OracleOledbBase.ExecuteDataSet(strperson).Tables[0];
            string st_date = table.Rows[0][0].ToString();

            //删除科室类别
            string delsql = string.Format("delete from {0}.BONUS_PERSONS_DETAIL  where  index_id={1} and dept_code='{2}'", DataUser.PERFORMANCE, bonusid, deptCode);
            MyLists listtable = new MyLists();
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            //获取指标人员指标
            //            string strdict = string.Format(@"
            //                                            SELECT   a.dept_code,
            //                                                     b.guide_code AS BONUS_PERSONS_ID,
            //                                                     C.GUIDE_NAME AS BONUS_PERSONS_NAME
            //                                              FROM   PERFORMANCE.SET_GUIDE_DEPT a,
            //                                                     PERFORMANCE.SET_GUIDE_GATHERS b,
            //                                                     HOSPITALSYS.GUIDE_NAME_DICT c
            //                                             WHERE       A.GUIDE_GROUP_CODE = B.GUIDE_GATHER_CODE
            //                                                     AND TO_CHAR (st_date, 'yyyymm') = '{0}'
            //                                                     AND a.dept_code = '{1}'
            //                                                     AND b.guide_code = c.guide_code 
            //                                            UNION ALL
            //                                            SELECT   '1' dept_code,
            //                                                     '1' BONUS_PERSONS_ID,
            //                                                     '银行账号' BONUS_PERSONS_NAME
            //                                              FROM   DUAL 
            //                                            UNION ALL
            //                                            SELECT   '2' dept_code,
            //                                                     '2' BONUS_PERSONS_ID,
            //                                                     '人员类别' BONUS_PERSONS_NAME
            //                                              FROM   DUAL 
            //                                            ", st_date, deptCode);

            string strdict = string.Format(@"
                                            SELECT   a.dept_code,
                                                     b.guide_code AS BONUS_PERSONS_ID,
                                                     C.GUIDE_NAME AS BONUS_PERSONS_NAME
                                              FROM   PERFORMANCE.SET_GUIDE_DEPT a,
                                                     PERFORMANCE.SET_GUIDE_GATHERS b,
                                                     HOSPITALSYS.GUIDE_NAME_DICT c
                                             WHERE       A.GUIDE_GROUP_CODE = B.GUIDE_GATHER_CODE
                                                     AND TO_CHAR (st_date, 'yyyymm') = '{0}'
                                                     AND a.dept_code = '{1}'
                                                     AND b.guide_code = c.guide_code 
                                            UNION ALL
                                            SELECT   '1' dept_code,
                                                     '1' BONUS_PERSONS_ID,
                                                     '银行账号' BONUS_PERSONS_NAME
                                              FROM   DUAL 
                                            ", st_date, deptCode);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];

            for (int i = 0; i < PersonBouns.Length - 1; i++)
            {
                if (tabledict.Rows.Count > 0)
                {
                    for (int j = 0; j < tabledict.Rows.Count; j++)
                    {
                        string bonuspersonsname = PersonBouns[i][tabledict.Rows[j]["BONUS_PERSONS_NAME"].ToString()].ToString() == "" ? "" : PersonBouns[i][tabledict.Rows[j]["BONUS_PERSONS_NAME"].ToString()].ToString();
                        StringBuilder isql = new StringBuilder();
                        string colname = tabledict.Rows[j]["BONUS_PERSONS_ID"].ToString();
                        if (colname == "1" || colname == "2")
                        {
                            string colnames = "";
                            if (colname == "1")
                            {
                                colnames = "BANK_CODE";
                            }
                            else if (colname == "2")
                            {
                                colnames = "PERSONS_TYPE";
                            }

                            //string colnames = colname == "1" ? "BANK_CODE" : "PERSONS_TYPE";
                            isql.AppendFormat("update  {0}.BONUS_DETAIL set {1}='{2}' where id={3}", DataUser.PERFORMANCE, colnames, bonuspersonsname, PersonBouns[i]["ID"].ToString());
                        }
                        else
                        {
                            isql.AppendFormat(" insert into {0}.BONUS_PERSONS_DETAIL (INDEX_ID,DEPT_CODE,BONUS_DETAIL_ID,BONUS_PERSONS_ID,BONUS_PERSONS_VALUE,STAFF_ID) values (", DataUser.PERFORMANCE);
                            isql.Append("'" + bonusid + "'");
                            isql.Append(",");
                            isql.Append("'" + deptCode + "'");
                            isql.Append(",");
                            isql.Append("" + PersonBouns[i]["ID"].ToString() + "");
                            isql.Append(",");
                            isql.Append("" + int.Parse(tabledict.Rows[j]["BONUS_PERSONS_ID"].ToString()) + "");
                            isql.Append(",");
                            string aa = bonuspersonsname == "" ? "0" : bonuspersonsname;
                            isql.Append("'" + aa + "'");
                            isql.Append(",");
                            isql.Append("'" + PersonBouns[i]["STAFFID"].ToString() + "'");
                            isql.Append(")");
                        }


                        //添加科室类别
                        List listcenterdetail = new List();
                        listcenterdetail.StrSql = isql.ToString();
                        listcenterdetail.Parameters = new OleDbParameter[] { };
                        listtable.Add(listcenterdetail);
                    }
                }
                else
                {
                    StringBuilder isql = new StringBuilder();
                    StringBuilder isql2 = new StringBuilder();
                    //string colname = tabledict.Rows[j]["BONUS_PERSONS_ID"].ToString();
                    //if (colname == "1" || colname == "2")
                    //{
                    //string colnames = colname == "1" ? "BANK_CODE" : "PERSONS_TYPE";
                    isql2.AppendFormat("update  {0}.BONUS_DETAIL set BANK_CODE='{2}',PERSONS_TYPE='{4}' where id={3}", DataUser.PERFORMANCE, "", PersonBouns[i]["银行账号"].ToString(), PersonBouns[i]["ID"].ToString(), PersonBouns[i]["人员类别"].ToString());
                    List listcenterdetail2 = new List();
                    listcenterdetail2.StrSql = isql2.ToString();
                    listcenterdetail2.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail2);
                    //}
                    //else
                    //{
                    isql.AppendFormat(" insert into {0}.BONUS_PERSONS_DETAIL (INDEX_ID,DEPT_CODE,BONUS_DETAIL_ID,BONUS_PERSONS_ID,BONUS_PERSONS_VALUE,STAFF_ID) values (", DataUser.PERFORMANCE);
                    isql.Append("'" + bonusid + "'");
                    isql.Append(",");
                    isql.Append("'" + deptCode + "'");
                    isql.Append(",");
                    isql.Append("" + PersonBouns[i]["ID"].ToString() + "");
                    isql.Append(",");
                    isql.Append("''");
                    isql.Append(",");
                    isql.Append("" + PersonBouns[i]["合计"].ToString() == "" ? "0" : PersonBouns[i]["合计"].ToString() + "");
                    isql.Append(",");
                    isql.Append("'" + PersonBouns[i]["STAFFID"].ToString() + "'");
                    isql.Append(")");
                    //}


                    //添加科室类别
                    List listcenterdetail = new List();
                    listcenterdetail.StrSql = isql.ToString();
                    listcenterdetail.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 保存平均奖科室人的奖金
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptCode"></param>
        /// <param name="deptName"></param>
        /// <param name="PersonBouns"></param>
        public void SaveAvgPersonBonu(string bonusid, Dictionary<string, string>[] PersonBouns)
        {
            MyLists listttrans = new MyLists();
            int id = OracleOledbBase.GetMaxID("id", "performance.BONUS_DETAIL");
            for (int i = 0; i < PersonBouns.Length; i++)
            {
                if (PersonBouns[i].Count > 0)
                {
                    StringBuilder str = new StringBuilder();

                    if (PersonBouns[i]["BANK_CODE"] == null)
                    {
                        PersonBouns[i]["BANK_CODE"] = "";
                    }

                    StringBuilder strdetail = new StringBuilder();
                    strdetail.Append("update Performance.BONUS_DETAIL set ");
                    strdetail.Append("user_bonus=?");
                    strdetail.Append(" where id=?");
                    OleDbParameter[] parameterdetail = {
                    new OleDbParameter("user_bonus", double.Parse(PersonBouns[i]["BONUS"].ToString())),
					new OleDbParameter("id", int.Parse(PersonBouns[i]["ID"].ToString()))};
                    List listdetail = new List();
                    listdetail.StrSql = strdetail.ToString();
                    listdetail.Parameters = parameterdetail;
                    listttrans.Add(listdetail);

                }
            }
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 检查人员奖金的合计是否与科室的奖金一致
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptCode"></param>
        /// <param name="deptType"></param>
        /// <param name="PersonBouns"></param>
        /// <returns></returns>
        public bool ChcckPersonBonus(string bonusid, string deptCode, string deptType, Dictionary<string, string>[] PersonBouns)
        {
            string bonus_type = "";
            if (deptType == "0")
            {
                bonus_type = "4";
            }
            else
            {
                bonus_type = "3";
            }
            string sqlGuide = "select GUIDE_VALUE from performance.SET_BONUSGUIDE_BASE a, ";
            sqlGuide += " performance.BONUS_VALUE b ";
            sqlGuide += " where A.GUIDE_CODE=B.GUIDE_CODE";
            sqlGuide += " and index_id=" + bonusid + " and bonus_type='" + bonus_type + "' and unit_code='" + deptCode + "'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlGuide.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                double deptbonus = Convert.ToDouble(ds.Tables[0].Rows[0]["GUIDE_VALUE"]);
                double totalBonus = 0.0;
                for (int i = 0; i < PersonBouns.Length; i++)
                {
                    totalBonus += Convert.ToDouble(PersonBouns[i]["BONUS"]);
                }
                if (deptbonus != totalBonus)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public DataTable SearchBonusBSC(string benginDate)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT '结余' 名称,
       ROUND((SELECT INCOME_COST
               FROM HISDATA.QUAN_ZHONG
              WHERE ST_DATE = '{0}'),
             0) 金额,
       '比例：13.3%' 实发VS比例,
       ROUND((SELECT GFJX FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0) 实发金额
  FROM DUAL
UNION ALL
SELECT '绩效总额',
       ROUND((SELECT GFJX FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       NULL,
       NULL
  FROM DUAL
UNION ALL
SELECT '一.医生绩效总额',
       ROUND((SELECT YSZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') +
             (SELECT QT
                FROM PERFORMANCE.BONUS_DATA
               WHERE ST_DATE = '{0}'
                 AND DEPT_CODE = '020014'),
             0),
       '医生实发',
       ROUND((SELECT SUM((XYJX + YJJX) * KPBL / 100 + ZL - YF * YZB)
                FROM PERFORMANCE.BONUS_DATA
               WHERE ST_DATE = '{0}'
                 AND DEPT_CODE IN (SELECT DEPT_CODE
                                     FROM COMM.SYS_DEPT_DICT A
                                    WHERE A.DEPT_TYPE = 0)) +
             (SELECT QT
                FROM PERFORMANCE.BONUS_DATA
               WHERE ST_DATE = '{0}'
                 AND DEPT_CODE = '020014'),
             0)
  FROM DUAL
UNION ALL
SELECT '医生效益绩效',
       ROUND(((SELECT YSZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') +
             (SELECT QT
                 FROM PERFORMANCE.BONUS_DATA
                WHERE ST_DATE = '{0}'
                  AND DEPT_CODE = '020014')) *
             (SELECT YSXYBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       '比例：60%',
       ROUND(((SELECT SUM((XYJX + YJJX) * KPBL / 100 + ZL - YF * YZB)
                 FROM PERFORMANCE.BONUS_DATA
                WHERE ST_DATE = '{0}'
                  AND DEPT_CODE IN (SELECT DEPT_CODE
                                      FROM COMM.SYS_DEPT_DICT A
                                     WHERE A.DEPT_TYPE = 0)) +
             (SELECT QT
                 FROM PERFORMANCE.BONUS_DATA
                WHERE ST_DATE = '{0}'
                  AND DEPT_CODE = '020014')) *
             (SELECT YSXYBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0)
  FROM DUAL
UNION ALL
SELECT '医生业绩绩效',
       ROUND(((SELECT YSZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') +
             (SELECT QT
                 FROM PERFORMANCE.BONUS_DATA
                WHERE ST_DATE = '{0}'
                  AND DEPT_CODE = '020014')) *
             (SELECT YSYJBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       '比例：40%',
       ROUND(((SELECT SUM((XYJX + YJJX) * KPBL / 100 + ZL - YF * YZB)
                 FROM PERFORMANCE.BONUS_DATA
                WHERE ST_DATE = '{0}'
                  AND DEPT_CODE IN (SELECT DEPT_CODE
                                      FROM COMM.SYS_DEPT_DICT A
                                     WHERE A.DEPT_TYPE = 0)) +
             (SELECT QT
                 FROM PERFORMANCE.BONUS_DATA
                WHERE ST_DATE = '{0}'
                  AND DEPT_CODE = '020014')) *
             (SELECT YSYJBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0)
  FROM DUAL
UNION ALL
SELECT '二.医技绩效总额',
       ROUND(((SELECT YJZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') +
             (SELECT SUM(QT + QT1 + QT2)
                 FROM PERFORMANCE.BONUS_DATA
                WHERE ST_DATE = '{0}'
                  AND DEPT_CODE IN ('0502', '0503', '0505'))),
             0),
       '实发',
       ROUND((SELECT SUM(SF)
               FROM PERFORMANCE.BONUS_DATA
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE IN ('0502', '0503', '0505')),
             0)
  FROM DUAL
UNION ALL
SELECT '医技效益绩效',
       ROUND(((SELECT YJZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') +
             (SELECT SUM(QT + QT1 + QT2)
                 FROM PERFORMANCE.BONUS_DATA
                WHERE ST_DATE = '{0}'
                  AND DEPT_CODE IN ('0502', '0503', '0505'))) *
             (SELECT YJXYBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       '比例：60%',
       ROUND((SELECT SUM(SF)
                FROM PERFORMANCE.BONUS_DATA
               WHERE ST_DATE = '{0}'
                 AND DEPT_CODE IN ('0502', '0503', '0505')) *
             (SELECT YJXYBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0)
  FROM DUAL
UNION ALL
SELECT '医技业绩绩效',
       ROUND(((SELECT YJZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') +
             (SELECT SUM(QT + QT1 + QT2)
                 FROM PERFORMANCE.BONUS_DATA
                WHERE ST_DATE = '{0}'
                  AND DEPT_CODE IN ('0502', '0503', '0505'))) *
             (SELECT YJYJBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       '比例：40%',
       ROUND((SELECT SUM(SF)
                FROM PERFORMANCE.BONUS_DATA
               WHERE ST_DATE = '{0}'
                 AND DEPT_CODE IN ('0502', '0503', '0505')) *
             (SELECT YJYJBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0)
  FROM DUAL
UNION ALL
SELECT '三.护士绩效总额',
       ROUND((SELECT HSZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       '实发',
       ROUND((SELECT ROUND(SUM(SF), 2)
               FROM PERFORMANCE.BONUS_DATA
              WHERE ST_DATE = '{0}'
                AND (DEPT_CODE LIKE '99%' OR DEPT_CODE = '060001')),
             0)
  FROM DUAL
UNION ALL
SELECT '护士效益绩效',
       ROUND((SELECT HSZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') *
             (SELECT HSXYBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       '比例：40%',
       ROUND((SELECT ROUND(SUM(SF), 2)
                FROM PERFORMANCE.BONUS_DATA
               WHERE ST_DATE = '{0}'
                 AND (DEPT_CODE LIKE '99%' OR DEPT_CODE = '060001')) *
             (SELECT HSXYBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0)
  FROM DUAL
UNION ALL
SELECT '护士岗位绩效',
       ROUND((SELECT HSZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') *
             (SELECT HSZCBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       '比例：40%',
       ROUND((SELECT ROUND(SUM(SF), 2)
                FROM PERFORMANCE.BONUS_DATA
               WHERE ST_DATE = '{0}'
                 AND (DEPT_CODE LIKE '99%' OR DEPT_CODE = '060001')) *
             (SELECT HSZCBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0)
  FROM DUAL
UNION ALL
SELECT '护士工作量绩效',
       ROUND((SELECT HSZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') *
             (SELECT HSYJBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       '比例：20%',
       ROUND((SELECT ROUND(SUM(SF), 2)
                FROM PERFORMANCE.BONUS_DATA
               WHERE ST_DATE = '{0}'
                 AND (DEPT_CODE LIKE '99%' OR DEPT_CODE = '060001')) *
             (SELECT HSYJBL FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0)
  FROM DUAL
UNION ALL
SELECT '四.药局绩效总额',
       ROUND((SELECT YAOJUZE
               FROM HISDATA.QUAN_ZHONG
              WHERE ST_DATE = '{0}'),
             0),
       '实发',
       ROUND((SELECT SUM((PJXJX) * ZLKP / 100)
               FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE IN ('080002', '080003')),
             0)
  FROM DUAL
UNION ALL
SELECT '五.窗口绩效总额',
       ROUND((SELECT CKZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}'),
             0),
       '实发',
       ROUND((SELECT SUM(SF)
               FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE IN ('070001', '070003', '070004')),
             0)
  FROM DUAL
UNION ALL
SELECT '六.行政后勤绩效总额',
       ROUND((SELECT XZZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}') +
             (SELECT SUM(QT1 + QT2 + QT3)
                FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
               WHERE ST_DATE = '{0}'
                 AND DEPT_CODE NOT IN ('070001',
                                       '070004',
                                       '080002',
                                       '080003',
                                       '01',
                                       '070003',
                                       '040001')),
             0),
       '实发',
       ROUND((SELECT SUM((PJXJX + QT) * ZLKP / 100)
                FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
               WHERE ST_DATE = '{0}'
                 AND DEPT_CODE NOT IN
                     ('070001', '070004', '080002', '080003', '01', '070003')) +
             (SELECT SUM(QT1 + QT2 + QT3)
                FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
               WHERE ST_DATE = '{0}'
                 AND DEPT_CODE NOT IN ('070001',
                                       '070004',
                                       '080002',
                                       '080003',
                                       '01',
                                       '070003',
                                       '040001')),
             0)
  FROM DUAL
UNION ALL
SELECT '七.护理员绩效总额',
       ROUND((SELECT QT2
               FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE = '040001'),
             0),
       NULL,
       NULL
  FROM DUAL
UNION ALL
SELECT '八.急诊医生绩效总额',
       ROUND((SELECT SUM(YF)
               FROM PERFORMANCE.BONUS_DATA
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE = '020024'),
             0),
       '实发',
       ROUND((SELECT SUM(YF * KPBL / 100 + ZL)
               FROM PERFORMANCE.BONUS_DATA
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE = '020024'),
             0)
  FROM DUAL
UNION ALL
SELECT '九.体检绩效',
       ROUND((SELECT NVL(SUM(MONEY), 0)
               FROM PERFORMANCE.INPUT_OTHERAWARD
              WHERE DEL_FLAG = 0
                AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
                AND OTHER_DICT_ID = '2'),
             0),
       NULL,
       NULL
  FROM DUAL
UNION ALL
SELECT '十.门诊诊查费总金额',
       ROUND((SELECT SUM(MZZCF + HZF + MZJR)
               FROM PERFORMANCE.BONUS_DATA
              WHERE ST_DATE = '{0}'),
             0),
       NULL,
       NULL
  FROM DUAL
UNION ALL
SELECT '十一.住院诊查费总金额',
       ROUND((SELECT SUM(ZYZCF)
               FROM PERFORMANCE.BONUS_DATA
              WHERE ST_DATE = '{0}'),
             0),
       '实发',
       ROUND((SELECT SUM(ZYZCF * KPBL / 100)
               FROM PERFORMANCE.BONUS_DATA
              WHERE ST_DATE = '{0}'),
             0)
  FROM DUAL
UNION ALL
SELECT '十二.中药局处方绩效',
       ROUND((SELECT SUM(QT)
               FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE = '080003'),
             0),
       '实发',
       ROUND((SELECT SUM(QT * ZLKP / 100)
               FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE = '080003'),
             0)
  FROM DUAL
UNION ALL
SELECT '十三.中药局熬药费',
       ROUND((SELECT SUM(QT1)
               FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE = '080003'),
             0),
       NULL,
       NULL
  FROM DUAL
UNION ALL
SELECT '十四.院务绩效',
       ROUND((SELECT SUM(PJXJX+QT1)
               FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
              WHERE ST_DATE = '{0}'
                AND DEPT_CODE = '01'),
             0),
       '实发',
       ROUND((SELECT NVL(SUM(MONEY), 0)
               FROM PERFORMANCE.INPUT_OTHERAWARD
              WHERE DEL_FLAG = 0
                AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
                AND OTHER_DICT_ID = '7'),
             0)
  FROM DUAL
UNION ALL
SELECT '十五.专家手术',
       ROUND((SELECT NVL(SUM(MONEY), 0)
               FROM PERFORMANCE.INPUT_OTHERAWARD
              WHERE DEL_FLAG = 0
                AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
                AND OTHER_DICT_ID = '3'),
             0),
       NULL,
       NULL
  FROM DUAL
UNION ALL
SELECT '十六.其他绩效',
       ROUND((SELECT NVL(SUM(MONEY), 0)
               FROM PERFORMANCE.INPUT_OTHERAWARD
              WHERE DEL_FLAG = 0
                AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
                AND OTHER_DICT_ID = '4'),
             0),
       NULL,
       NULL
  FROM DUAL
UNION ALL
SELECT '十七.妇科补差',
       ROUND((SELECT NVL(SUM(MONEY), 0)
               FROM PERFORMANCE.INPUT_OTHERAWARD
              WHERE DEL_FLAG = 0
                AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
                AND OTHER_DICT_ID = '8'),
             0),
       NULL,
       NULL
  FROM DUAL
UNION ALL
SELECT '十八.新技术项目',
       ROUND((SELECT SUM(ZYJS)
               FROM PERFORMANCE.BONUS_DATA
              WHERE ST_DATE = '{0}'),
             0),
       NULL,
       NULL
  FROM DUAL
UNION ALL
SELECT '十九.上年度鼓励奖金',
       ROUND((SELECT NVL(SUM(MONEY), 0)
               FROM PERFORMANCE.INPUT_OTHERAWARD
              WHERE DEL_FLAG = 0
                AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
                AND OTHER_DICT_ID = '6'),
             0),
       NULL,
       NULL
  FROM DUAL
  UNION ALL
        SELECT '二十.本月绩效工资总额',
               ROUND(((SELECT YSZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}')+(SELECT QT
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE ='020014')+(SELECT YJZE FROM hisdata.quan_zhong WHERE st_date='{0}')+
       (SELECT SUM(QT+QT1+QT2)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN ('0502', '0503', '0505'))+ ROUND((SELECT HSZE FROM hisdata.quan_zhong WHERE st_date='{0}'),0)
           +(SELECT YAOJUZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}')+(SELECT CKZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}')+
            (SELECT XZZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}')+
       (SELECT  SUM(QT1+QT2+QT3)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE NOT IN
               ('070001', '070004', '080002', '080003', '01','070003','040001'))+(SELECT QT2
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '040001')+(SELECT SUM(YF)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '020024')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '2')+(SELECT SUM(MZZCF + HZF + MZJR)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}')+(SELECT SUM(ZYZCF) FROM PERFORMANCE.BONUS_DATA WHERE ST_DATE = '{0}')+(SELECT SUM(QT)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '080003')+(SELECT SUM(QT1)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '080003')+(SELECT SUM(PJXJX+QT1)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '01')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '3')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '4')+(SELECT SUM(ZYJS) FROM PERFORMANCE.BONUS_DATA WHERE ST_DATE = '{0}')-(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '5')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '6'))+ROUND((SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '8'),0),0),               
               '实发', 
               ROUND(((SELECT SUM((XYJX + YJJX) * KPBL/100+ ZL-YF * YZB)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN (SELECT DEPT_CODE
                               FROM COMM.SYS_DEPT_DICT A
                              WHERE A.DEPT_TYPE = 0))+(SELECT QT
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE ='020014')+(SELECT SUM(SF)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN ('0502', '0503', '0505'))+(SELECT round(SUM(SF),2)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND (DEPT_CODE LIKE '99%'
            OR DEPT_CODE = '060001'))+(SELECT SUM((PJXJX) * ZLKP/100)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN ('080002', '080003'))+(SELECT SUM(SF)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN ('070001', '070003', '070004'))+(SELECT  SUM((PJXJX + QT) * ZLKP/100)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE NOT IN
               ('070001', '070004', '080002', '080003', '01','070003'))+(SELECT  SUM(QT1+QT2+QT3)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE NOT IN
               ('070001', '070004', '080002', '080003', '01','070003','040001'))+(SELECT QT2
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '040001')+(SELECT SUM(YF * KPBL/100 + ZL)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '020024')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '2')+(SELECT SUM(MZZCF + HZF + MZJR)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}')+(SELECT SUM(ZYZCF * KPBL/100)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}')+(SELECT SUM(QT * ZLKP/100)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '080003')+(SELECT SUM(QT1)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '080003')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '7')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '3')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '4')+(SELECT SUM(ZYJS) FROM PERFORMANCE.BONUS_DATA WHERE ST_DATE = '{0}')-(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '5')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '6'))+ROUND((SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '8'),0),0)+ ROUND((SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '10'),0)              
          FROM DUAL
          UNION ALL
          SELECT '二十一.质量扣款',ROUND((SELECT SUM(ZL) FROM PERFORMANCE.BONUS_DATA A WHERE ST_DATE = '{0}'),0)
          ,'二十二.考评扣款',
          -ROUND(((SELECT YSZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}')+(SELECT QT
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE ='020014')+(SELECT YJZE FROM hisdata.quan_zhong WHERE st_date='{0}')+
       (SELECT SUM(QT+QT1+QT2)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN ('0502', '0503', '0505'))+ ROUND((SELECT HSZE FROM hisdata.quan_zhong WHERE st_date='{0}'),0)
           +(SELECT YAOJUZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}')+(SELECT CKZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}')+
            (SELECT XZZE FROM HISDATA.QUAN_ZHONG WHERE ST_DATE = '{0}')+
       (SELECT  SUM(QT1+QT2+QT3)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE NOT IN
               ('070001', '070004', '080002', '080003', '01','070003','040001'))+(SELECT QT2
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '040001')+(SELECT SUM(YF)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '020024')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '2')+(SELECT SUM(MZZCF + HZF + MZJR)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}')+(SELECT SUM(ZYZCF) FROM PERFORMANCE.BONUS_DATA WHERE ST_DATE = '{0}')+(SELECT SUM(QT)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '080003')+(SELECT SUM(QT1)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '080003')+(SELECT SUM(PJXJX+QT1)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '01')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '3')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '4')+(SELECT SUM(ZYJS) FROM PERFORMANCE.BONUS_DATA WHERE ST_DATE = '{0}')-(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '5')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '6'))+ROUND((SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '8'),0),0)+ROUND(((SELECT SUM((XYJX + YJJX) * KPBL/100+ ZL-YF * YZB)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN (SELECT DEPT_CODE
                               FROM COMM.SYS_DEPT_DICT A
                              WHERE A.DEPT_TYPE = 0))+(SELECT QT
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE ='020014')+(SELECT SUM(SF)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN ('0502', '0503', '0505'))+(SELECT round(SUM(SF),2)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND (DEPT_CODE LIKE '99%'
            OR DEPT_CODE = '060001'))+(SELECT SUM((PJXJX) * ZLKP/100)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN ('080002', '080003'))+(SELECT SUM(SF)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE IN ('070001', '070003', '070004'))+(SELECT  SUM((PJXJX + QT) * ZLKP/100)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE NOT IN
               ('070001', '070004', '080002', '080003', '01','070003'))+(SELECT  SUM(QT1+QT2+QT3)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE NOT IN
               ('070001', '070004', '080002', '080003', '01','070003','040001'))+(SELECT QT2
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '040001')+(SELECT SUM(YF * KPBL/100 + ZL)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '020024')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '2')+(SELECT SUM(MZZCF + HZF + MZJR)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}')+(SELECT SUM(ZYZCF * KPBL/100)
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{0}')+(SELECT SUM(QT * ZLKP/100)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '080003')+(SELECT SUM(QT1)
          FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
         WHERE ST_DATE = '{0}'
           AND DEPT_CODE = '080003')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '7')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '3')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '4')+(SELECT SUM(ZYJS) FROM PERFORMANCE.BONUS_DATA WHERE ST_DATE = '{0}')-(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '5')+(SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '6'))+ROUND((SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '8'),0),0)-ROUND((SELECT SUM(ZL) FROM PERFORMANCE.BONUS_DATA A WHERE ST_DATE = '{0}'),0)
                   FROM dual
                   UNION ALL
                   SELECT '二十三.绩效扣款',
        ROUND((SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '10'),0),
        '二十四.农合扣款',
       ROUND((SELECT NVL(SUM(MONEY), 0)
          FROM PERFORMANCE.INPUT_OTHERAWARD
         WHERE DEL_FLAG = 0
           AND TO_CHAR(INPUT_DATE, 'yyyymm') = '{0}'
           AND OTHER_DICT_ID = '5'),0)
  FROM DUAL ", benginDate);
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return ds.Tables[0];
        }
        public DataTable SearchBonus_ChaZhi(string years, string months, string benginDate)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT UNIT_CODE,
       UNIT_NAME,
       SUM(USER_TOTAL_BONUS) USER_TOTAL_BONUS,
       SUM(SF) SF,
       SUM(USER_TOTAL_BONUS)-SUM(SF) CHAZHI
  FROM (SELECT CASE
                 WHEN B.DEPT_CODE LIKE '99%' THEN
                  '99'
                 ELSE
                  (SELECT DEPT_CODE
                     FROM COMM.SYS_DEPT_DICT
                    WHERE DEPT_CODE = B.DEPT_CODE)
               END UNIT_CODE,
               CASE
                 WHEN B.DEPT_CODE LIKE '99%' THEN
                  '护理合计'
                 ELSE
                  (SELECT DEPT_NAME
                     FROM COMM.SYS_DEPT_DICT
                    WHERE DEPT_CODE = B.DEPT_CODE)
               END UNIT_NAME,
               SUM(USER_TOTAL_BONUS) USER_TOTAL_BONUS,
               SUM(SF) SF,
               SUM(USER_TOTAL_BONUS) - SUM(SF) CHAZHI
          FROM (SELECT B.UNIT_CODE,
                       ROUND(SUM(BONUS_PERSONS_VALUE), 2) AS USER_TOTAL_BONUS
                  FROM PERFORMANCE.BONUS_INDEX A
                 INNER JOIN PERFORMANCE.BONUS_DETAIL B
                    ON A.ID = B.INDEX_ID
                 INNER JOIN PERFORMANCE.BONUS_PERSONS_DETAIL C
                    ON A.ID = C.INDEX_ID
                   AND B.UNIT_CODE = C.DEPT_CODE
                   AND B.ID = C.BONUS_DETAIL_ID
                 WHERE A.BEGINYEAR = '{0}'
                   AND A.BEGINMONTH = '{1}'
                 GROUP BY B.UNIT_CODE) A,
               (SELECT DEPT_CODE, SF
                  FROM PERFORMANCE.BONUS_DATA
                 WHERE ST_DATE = '{2}'
                UNION ALL
                SELECT DEPT_CODE, SF
                  FROM PERFORMANCE.BONUS_BUSINESS_AVERAGE
                 WHERE ST_DATE = '{2}') B
         WHERE B.DEPT_CODE = A.UNIT_CODE(+)
         GROUP BY B.DEPT_CODE)
 GROUP BY UNIT_CODE, UNIT_NAME
 ORDER BY UNIT_CODE", years, months, benginDate);
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return ds.Tables[0];
        }
        public DataTable SearchHuLi_ChaZhi(string years, string months, string benginDate)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT (SELECT DEPT_CODE
          FROM COMM.SYS_DEPT_DICT
         WHERE DEPT_CODE = B.DEPT_CODE) UNIT_CODE,
       (SELECT DEPT_NAME
          FROM COMM.SYS_DEPT_DICT
         WHERE DEPT_CODE = B.DEPT_CODE) UNIT_NAME,
       SUM(USER_TOTAL_BONUS) USER_TOTAL_BONUS,
       SUM(SF) SF,
       SUM(USER_TOTAL_BONUS) - SUM(SF) CHAZHI
  FROM (SELECT B.UNIT_CODE,
               ROUND(SUM(BONUS_PERSONS_VALUE), 2) AS USER_TOTAL_BONUS
          FROM PERFORMANCE.BONUS_INDEX A
         INNER JOIN PERFORMANCE.BONUS_DETAIL B
            ON A.ID = B.INDEX_ID
         INNER JOIN PERFORMANCE.BONUS_PERSONS_DETAIL C
            ON A.ID = C.INDEX_ID
           AND B.UNIT_CODE = C.DEPT_CODE
           AND B.ID = C.BONUS_DETAIL_ID
         WHERE A.BEGINYEAR = '{0}'
           AND A.BEGINMONTH = '{1}'
         GROUP BY B.UNIT_CODE) A,
       (SELECT DEPT_CODE, SF
          FROM PERFORMANCE.BONUS_DATA
         WHERE ST_DATE = '{2}') B
 WHERE B.DEPT_CODE = A.UNIT_CODE(+)
   AND (B.DEPT_CODE LIKE '99%' OR B.DEPT_CODE = '060001')
 GROUP BY B.DEPT_CODE
 ORDER BY B.DEPT_CODE", years, months, benginDate);
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 对奖金人员表进行查询
        /// </summary>
        /// <param name="years">奖金年</param>
        /// <param name="months">奖金月</param>
        /// <param name="deptName">科室</param>
        /// <param name="staffName">人员</param>
        /// <param name="bankCode">账号</param>
        /// <param name="bonus">奖金数</param>
        /// <returns></returns>
        public DataTable SearchBonus(string years, string months, string deptcode)
        {
            string condition = "";
            if (deptcode != "")
            {
                condition += " and UNIT_CODE in (" + deptcode + ")";
            }

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"SELECT   b.unit_code,b.unit_name, b.user_name,B.STAFF_ID STAFF_ID,
                                        (select BANK_CODE  from rlzy.new_staff_info where staff_id=b.staff_id) BANK_CODE,
                                        (select staffsort  from rlzy.new_staff_info where staff_id=b.staff_id) STAFFSORT,
                                         ROUND (sum(bonus_persons_value),2) AS USER_TOTAL_BONUS
                                    FROM PERFORMANCE.bonus_index a INNER JOIN PERFORMANCE.bonus_detail b
                                         ON a.ID = b.index_id
                                         inner join PERFORMANCE.BONUS_PERSONS_DETAIL c
                                         on a.ID = c.index_id and B.UNIT_CODE = c.dept_code and b.id= c.bonus_detail_id
                                   WHERE a.beginyear = {0}
                                     AND a.beginmonth = {1} {2}
                                group BY b.unit_code,b.unit_name, b.user_name,B.STAFF_ID
                                order by b.unit_code", years, months, condition);

            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            return ds.Tables[0];
        }
    }
}
