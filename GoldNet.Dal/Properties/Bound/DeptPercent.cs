using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Data.OleDb;
using System;

namespace Goldnet.Dal
{
    public class DeptPercent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable GetDeptPercent(string year, string month)
        {
            if (year == "" || month == "")
            {
                return BuildDeptPercent();
            }
            string date = BuildDate(year, month);

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT a.dept_code deptcode, b.dept_name deptname,
                                       NVL (c.dept_percent, 0) PERCENT, 
                                       NVL (c.director_percent, 0) director,
                                       NVL (c.dept_found, 0) fundpercent,
                                       NVL (c.remit_percent, 0) remitpercent,
                                       NVL (sec_dept_found, 0) secdeptfound ,
                                        nvl(c.JC_BZ,0) JC_BZ,
                                        nvl(c.JC_BZ_XY,0) JC_BZ_XY,
                                        nvl(c.gzbz,0) gzbz,nvl(c.JMCB,0) JMCB--,
                                       --nvl(c.jx,0)jx
                                  FROM (SELECT A.dept_code
                                          FROM PERFORMANCE.set_accountdepttype A,PERFORMANCE.SET_ACCOUNTTYPEGROUP B
                                         WHERE A.DEPT_TYPE=B.TYPE_CODE
                                           AND A.st_date = TO_DATE ('{0}', 'yyyymmdd')
                                           AND B.GROUP_CODE = '01'
                                       ) a,
                                       (SELECT *
                                          FROM comm.sys_dept_info
                                         WHERE dept_snap_date = TO_DATE ('{0}', 'yyyymmdd')) b,
                                       (SELECT *
                                          FROM PERFORMANCE.set_deptpercent
                                         WHERE st_date = TO_DATE ('{0}', 'yyyymmdd')) c
                                 WHERE a.dept_code = b.dept_code AND a.dept_code = c.dept_code(+) order by deptcode", date);

            DataSet dsPercent = OracleOledbBase.ExecuteDataSet(str.ToString());
            if (dsPercent != null && dsPercent.Tables.Count > 0)
            {
                if (dsPercent.Tables[0].Rows.Count == 0)
                {
                    BuildDeptPercent();
                }
                else
                {
                    return dsPercent.Tables[0];
                }
            }
            return BuildDeptPercent();
        }

        /// <summary>
        /// 组合核算科室提成比表结构
        /// </summary>
        /// <returns></returns>
        public DataTable BuildDeptPercent()
        {
            DataTable dt = new DataTable();
            DataColumn dcDEPTCODE = new DataColumn("DEPTCODE");
            DataColumn dcDEPTNAME = new DataColumn("DEPT_CODE");
            DataColumn dcDEPTPERCENT = new DataColumn("DEPTPERCENT");
            DataColumn dcDIRECTORPERCENT = new DataColumn("DIRECTORPERCENT");
            DataColumn dcDEPTFOUND = new DataColumn("DEPTFOUND");
            DataColumn dcREMITPERCENT = new DataColumn("REMITPERCENT");
            DataColumn dcSECDEPTFOUND = new DataColumn("SECDEPTFOUND");
            dt.Columns.AddRange(new DataColumn[] { dcDEPTCODE, dcDEPTNAME, dcDEPTPERCENT, dcDIRECTORPERCENT, dcDEPTFOUND, dcREMITPERCENT, dcSECDEPTFOUND });
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private string BuildDate(string year, string month)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            return year + "" + month + "" + "01";
        }

        /// <summary>
        /// 删除1个月核算科室提成比，添加1个月的核算科室提成比
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool SaveDeptPercent(Dictionary<string, string>[] rows, string year, string month)
        {
            if (year == "" || month == "")
            {
                return false;
            }
            string date = BuildDate(year, month);
            string delsql = "delete from  PERFORMANCE.SET_DEPTPERCENT  where  st_date =TO_DATE ('" + date + "', 'yyyymmdd') ";
            MyLists listtable = new MyLists();
            //删除科室类别
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);
            
            for (int i = 0; i < rows.Length; i++)
            {
                StringBuilder isql = new StringBuilder();
                isql.Append(" insert into PERFORMANCE.SET_DEPTPERCENT (ST_DATE,DEPT_CODE,DEPT_PERCENT,DIRECTOR_PERCENT,DEPT_FOUND,REMIT_PERCENT,SEC_DEPT_FOUND,JC_BZ,JC_BZ_XY,GZBZ,JMCB) values (");
                isql.Append("to_date('" + date + "','yyyymmdd')");
                isql.Append(",");
                isql.Append("'" + rows[i]["DEPTCODE"].ToString() + "'");
                isql.Append(",");
                isql.Append("" + rows[i]["PERCENT"].ToString() + "");
                isql.Append(",");
                isql.Append("" + rows[i]["DIRECTOR"].ToString() + "");
                isql.Append(",");
                isql.Append("" + rows[i]["FUNDPERCENT"].ToString() + "");
                isql.Append(",");
                isql.Append("" + rows[i]["REMITPERCENT"].ToString() + "");
                isql.Append(",");
                isql.Append("" + rows[i]["SECDEPTFOUND"].ToString() + "");
                isql.Append(",");
                isql.Append("" + rows[i]["JC_BZ"].ToString() + "");
                isql.Append(",");
                isql.Append("" + rows[i]["JC_BZ_XY"].ToString() + "");
                isql.Append(",");
                isql.Append("" + rows[i]["GZBZ"].ToString() + "");
                isql.Append(",");
                isql.Append("" + rows[i]["JMCB"].ToString() + "");
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

        /// <summary>
        /// 保存奖金相关系数
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool SaveBonusPercent(Dictionary<string, string>[] rows, string year, string month)
        {
            if (year == "" || month == "")
            {
                return false;
            }
            MyLists listtable = new MyLists();
            string strdict = string.Format("select * from {0}.SET_BONUSGUIDE_DICT order by BONUSGUIDE_ID", DataUser.PERFORMANCE);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];

            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < tabledict.Rows.Count; j++)
                {
                    //删除科室相关系数
                    string date = BuildDate(year, month);
                    string delsql = string.Format("delete from  {0}.SET_DEPT_BONUSPERCENT  where  st_date =TO_DATE ('" + date + "', 'yyyymmdd') and DEPT_CODE='{1}' and BONUSGUIDE_ID={2} ", DataUser.PERFORMANCE, rows[i]["DEPT_CODE"].ToString(), tabledict.Rows[j]["BONUSGUIDE_ID"].ToString());
                    List listcenterdict = new List();
                    listcenterdict.StrSql = delsql;
                    listcenterdict.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdict);

                    StringBuilder isql = new StringBuilder();
                    isql.AppendFormat(" insert into {0}.SET_DEPT_BONUSPERCENT (ST_DATE,DEPT_CODE,BONUSGUIDE_ID,GUIDEPERCENT) values (", DataUser.PERFORMANCE);
                    isql.Append("to_date('" + date + "','yyyymmdd')");
                    isql.Append(",");
                    isql.Append("'" + rows[i]["DEPT_CODE"].ToString() + "'");
                    isql.Append(",");
                    isql.Append("" + int.Parse(tabledict.Rows[j]["BONUSGUIDE_ID"].ToString()) + "");
                    isql.Append(",");
                    isql.Append("" + rows[i][tabledict.Rows[j]["BONUSGUIDE_NAME"].ToString()].ToString() + "");
                    isql.Append(")");
                  

                    //添加科室类别
                    List listcenterdetail = new List();
                    listcenterdetail.StrSql = isql.ToString();
                    listcenterdetail.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail);
                }
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

        /// <summary>
        /// 保存修改后导入奖金
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool SaveBonus_Jjdr(Dictionary<string, string>[] rows, string year, string month)
        {
            if (year == "" || month == "")
            {
                return false;
            }
            MyLists listtable = new MyLists();

            for (int i = 0; i < rows.Length; i++)
            {
                
                    //删除
                    string date = BuildDate(year, month);
                    string delsql = string.Format("delete from  {0}.JJ_DR  where  st_date = " + date + " and USER_ID='{1}'  ", DataUser.PERFORMANCE, rows[i]["USER_ID"].ToString());
                    List listcenterdict = new List();
                    listcenterdict.StrSql = delsql;
                    listcenterdict.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdict);

                    //增加修改后的
                    StringBuilder isql = new StringBuilder();
                    isql.AppendFormat(" insert into {0}.JJ_DR (ST_DATE,COSTS,DEPT_CODE,USER_ID) values (", DataUser.PERFORMANCE);
                    isql.Append("'" + date + "'");
                    isql.Append(",");
                    isql.Append("" + double.Parse(rows[i]["COSTS"].ToString()) + "");
                    isql.Append(",");
                    isql.Append("'" + rows[i]["DEPT_CODE"].ToString() + "'");
                    isql.Append(",");
                    isql.Append("'" + rows[i]["USER_ID"].ToString() + "'");
                    isql.Append(")");

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

        /// <summary>
        /// 科室奖金系数设置
        /// </summary>
        /// <returns></returns>
        public DataTable BonusGuidelist(string year,string month,string deptfilter)
        {
            string date = BuildDate(year, month);
            string strdict = string.Format("select * from {0}.SET_BONUSGUIDE_DICT order by BONUSGUIDE_ID", DataUser.PERFORMANCE);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT b.DEPT_SNAP_DATE, b.account_dept_code dept_code,b.account_dept_name \"科室名称\"");
            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                str.AppendFormat(" ,sum(DECODE (bonusguide_id, {0}, guidepercent,0)) \"{1}\"", tabledict.Rows[i]["BONUSGUIDE_ID"].ToString(), tabledict.Rows[i]["BONUSGUIDE_NAME"].ToString());
            }
            str.AppendFormat(" FROM PERFORMANCE.set_dept_bonuspercent a right join comm.sys_dept_info b on a.dept_code=b.dept_code and to_char(a.st_date,'yyyymm')=to_char(b.DEPT_SNAP_DATE,'yyyymm')");
            str.AppendFormat(" where b.attr='是' and to_char(b.DEPT_SNAP_DATE,'yyyymmdd')='{0}' and b.dept_code not in (SELECT dept_code  FROM PERFORMANCE.SET_ACCOUNTDEPTTYPE WHERE TO_CHAR(ST_DATE, 'yyyymmdd') = '{0}'   AND DEPT_TYPE = '10001')", date);
            if (deptfilter != "")
            {
                str.AppendFormat(" and (b.dept_name like '{0}' or b.dept_code like '{0}' or b.INPUT_CODE like '{0}')",deptfilter+"%");
            }
            str.Append(" group by  b.account_dept_code,b.account_dept_name,b.DEPT_SNAP_DATE,b.sort_no order by b.sort_no");

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 科室奖金系数设置
        /// </summary>
        /// <returns></returns>
        public DataTable BonusGuidelist_NEWUPDATE(string selectDate,string deptfilter)
        {
            string strdict = string.Format("select * from {0}.SET_BONUSGUIDE_DICT order by BONUSGUIDE_ID", DataUser.PERFORMANCE);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT b.DEPT_SNAP_DATE, b.account_dept_code dept_code,b.account_dept_name \"科室名称\"");
            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                str.AppendFormat(" ,sum(DECODE (bonusguide_id, {0}, guidepercent,0)) \"{1}\"", tabledict.Rows[i]["BONUSGUIDE_ID"].ToString(), tabledict.Rows[i]["BONUSGUIDE_NAME"].ToString());
            }
            str.AppendFormat(" FROM PERFORMANCE.set_dept_bonuspercent a right join comm.sys_dept_info b on a.dept_code=b.dept_code and to_char(a.st_date,'yyyymm')=to_char(b.DEPT_SNAP_DATE,'yyyymm')");
            str.AppendFormat(" where b.attr='是' and to_char(b.DEPT_SNAP_DATE,'yyyymmdd')='{0}' and b.dept_code not in (SELECT dept_code  FROM PERFORMANCE.SET_ACCOUNTDEPTTYPE WHERE TO_CHAR(ST_DATE, 'yyyymmdd') = '{0}'   AND DEPT_TYPE = '10001')", selectDate);
            if (deptfilter != "")
            {
                str.AppendFormat(" and (b.dept_name like '{0}' or b.dept_code like '{0}' or b.INPUT_CODE like '{0}')", deptfilter + "%");
            }
            str.Append(" group by  b.account_dept_code,b.account_dept_name,b.DEPT_SNAP_DATE,b.sort_no order by b.sort_no");

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 奖金导入
        /// </summary>
        /// <returns></returns>
        public DataTable Bonus_Jjdr(string year, string month, string deptcode)
        {
            string date = BuildDate(year, month);
            
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT   --a.ROWID,
                                         a.dept_code,
                                         (select dept_name from comm.sys_dept_dict b where b.dept_code=a.dept_code) dept_name,
                                         a.user_id，
                                        (select user_name from hisdata.users u where u.user_id=a.user_id) user_name,
                                        a.costs
                                  FROM   performance.jj_dr a
                                 WHERE    a.st_date='{1}' ", DataUser.PERFORMANCE, date);

            if (deptcode != "")
            {
                str.AppendFormat(" and a.dept_code in ({0})", deptcode);
            }


            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        //项目类别查询
        public DataTable Item_Leibie_Select(string itemcode)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT
	                                ITEM_CODE,
	                                ITEM_NAME,
	                                CLASS_NAME,
	                                ITEM_UNIT,
	                                ITEM_PRICE,
	                                PANDU,
	                                nvl(ZHIXING,0) ZHIXING,
	                                nvl(HULI,0) HULI
                                FROM
	                                HISDATA.LEIBIE_HZ where 1=1 ", DataUser.HISDATA);

            if (!string.IsNullOrEmpty(itemcode))
            {
                str.AppendFormat(" and item_code in ('{0}')", itemcode);
            }


            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        //刷新分页
        public DataTable Item_Leibie_SelectPaged(string deptcode, string itemcode, int start, int limit)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"
        SELECT * FROM (
            SELECT 
                DEPT_CODE,
                DEPT_NAME,
                ITEM_CODE,
                ITEM_NAME,
                CLASS_NAME,
                ITEM_UNIT,
                ITEM_PRICE,
                PANDU,
                ZHIXING,
                HULI,
                FENZUMC,
                ROW_NUMBER() OVER (ORDER BY ITEM_CODE) AS Row_Num
            FROM
                HISDATA.LEIBIE_HZ
            WHERE 1=1");

            if (!string.IsNullOrEmpty(deptcode))
            {
                str.AppendFormat(" AND DEPT_CODE='{0}' ", deptcode);
            }

            if (!string.IsNullOrEmpty(itemcode))
            {
                str.AppendFormat(" AND ITEM_CODE='{0}'", itemcode);
            }

            str.AppendFormat(@"
        )
        WHERE Row_Num > {0} AND Row_Num <= {1}", start, start + limit);

            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        //获取数据总数
        public int GetTotalCount(string deptcode, string itemcode)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"
        SELECT COUNT(*)
        FROM
            HISDATA.LEIBIE_HZ
        WHERE 1=1");

            if (!string.IsNullOrEmpty(deptcode))
            {
                str.AppendFormat(" AND DEPT_CODE IN ({0})", deptcode);
            }

            if (!string.IsNullOrEmpty(itemcode))
            {
                str.AppendFormat(" AND ITEM_CODE IN ({0})", itemcode);
            }

            return Convert.ToInt32(OracleOledbBase.ExecuteScalar(str.ToString()));
        }

        //保存修改项目类别
        public bool Item_Leibie_Save(Dictionary<string, string>[] changes)
        {
            bool success = true;
            foreach (var change in changes)
            {
                string itemCode = change["ITEM_CODE"];
                string pandu = change["PANDU"] != "" && change["PANDU"] != null ? change["PANDU"] : "0";
                string zhixing = change["ZHIXING"] != "" && change["ZHIXING"] != null ? change["ZHIXING"] : "0";
                string huli = change["HULI"] !="" && change["HULI"] !=null ? change["HULI"] : "0" ;
                // 继续获取其他字段的修改值

                // 构建更新语句
                string updateSql = string.Format(@"UPDATE HISDATA.LEIBIE_HZ 
                                      SET PANDU = {0} ,ZHIXING={1},HULI={2}
                                      WHERE ITEM_CODE = '{3}'",pandu,zhixing,huli,itemCode);

                // 执行更新语句，假设 ExecuteNonQuery 方法执行 SQL 并返回是否成功的布尔值
                bool updateSuccess = OracleOledbBase.ExecuteSql(updateSql); // 需要根据实际情况实现该方法

                if (!updateSuccess)
                {
                    success = false; // 更新失败时设置标志
                    break; // 如果有更新失败，则跳出循环
                }
            }

            return success;
        }
    }
}
