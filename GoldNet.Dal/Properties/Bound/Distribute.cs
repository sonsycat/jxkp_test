using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace Goldnet.Dal
{
    public class Distribute
    {

        /// <summary>
        /// 根据选择的报表ID，找到报表内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetDistributeBonus(string id, string year, string months, string dept_code)
        {
            string codesql = "";
            if (dept_code != "")
            {
                codesql = " and b.DEPT_CODE in (" + dept_code + ")";
            }



            string eguidecode = "  select a.guide_code,B.GUIDE_NAME from hospitalsys.EVALUATE_GUIDE_VALUE a left join ";
            eguidecode += "HOSPITALSYS.GUIDE_NAME_DICT b on A.GUIDE_CODE=B.GUIDE_CODE where EVALUATE_CODE = '" + id + "' group by a.guide_code,B.GUIDE_NAME ";
            string eguidsql = " SELECT ";
            DataSet dseguid = OracleOledbBase.ExecuteDataSet(eguidecode.ToString());
            if (dseguid.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dseguid.Tables[0].Rows.Count; i++)
                {
                    eguidsql += " sum(case when GUIDE_CODE='" + dseguid.Tables[0].Rows[i]["GUIDE_CODE"].ToString() + "' then GUIDE_VALUE else 0 end) as \"" + dseguid.Tables[0].Rows[i]["GUIDE_NAME"].ToString() + "\", ";
                }
            }
            eguidsql += "  UNIT_CODE as dept_code FROM  hospitalsys.EVALUATE_GUIDE_VALUE  ";
            eguidsql += " WHERE evaluate_code = '" + id + "' ";
            eguidsql += " group BY UNIT_CODE  ";


            string sql = " select DEPT_CODE,dept_name ,score SCORE,dbonus BONUS from performance.DISTRIBUTE_INDEX a ";
            sql += " inner join performance.DISTRIBUTE_DETAIL b ";
            sql += " on a.ID=b.DINDEX ";
            sql += " where a.EID=" + id + " and a.YEAR='" + year + "' and a.MONTH='" + months + "'" + codesql + " order by b.dept_code";


            string totalsql = " select b.dept_name,b.SCORE,b.BONUS,a.* from (" + eguidsql + ") a inner join (" + sql + ") b on a.dept_code=b.dept_code";


            DataSet ds = OracleOledbBase.ExecuteDataSet(totalsql.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                string sqlE = "select evaluate_dept_name as dept_name,EVALUATE_DEPT_CODE as dept_code,evaluate_value as score,0 as bonus from ";
                sqlE += " hospitalsys.EVALUATE_INFORMATION a ";
                sqlE += " inner join ";
                sqlE += " hospitalsys.EVALUATE_RESULT_LIST b ";
                sqlE += " on a.EVALUATE_CODE=b.EVALUATE_CODE ";
                sqlE += " where a.EVALUATE_CODE='" + id + "'" + codesql + " order by b.EVALUATE_DEPT_CODE";

                totalsql = " select b.dept_name,b.SCORE,b.BONUS,a.* from (" + eguidsql + ") a inner join (" + sqlE + ") b on a.dept_code=b.dept_code order by a.dept_code";

                DataSet dsE = OracleOledbBase.ExecuteDataSet(totalsql.ToString());
                if (dsE.Tables.Count > 0 && dsE.Tables[0].Rows.Count > 0)
                {
                    return dsE.Tables[0];
                }
                else
                {
                    return GetDistributeBonus();
                }
            }

        }
        public DataTable GetDistributeBonus()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn();
            dc1.ColumnName = "DEPT_NAME";
            DataColumn dc2 = new DataColumn();
            dc2.ColumnName = "SCORE";
            DataColumn dc3 = new DataColumn();
            dc3.ColumnName = "BONUS";
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            return dt;
        }
        /// <summary>
        /// 获得评价列表
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public DataTable GetEvaluateName(string typeID)
        {
            string sql = " SELECT a.evaluate_code ID, a.evaluate_name NAME ";
            sql += " FROM hospitalsys.evaluate_information a ";
            sql += " WHERE  a.EVALUATE_CLASS_CODE = '" + typeID + "' ";
            sql += "   AND a.archive_tags = '1'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return GetEvaluateName();
            }
        }
        private DataTable GetEvaluateName()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn();
            dc1.ColumnName = "ID";
            DataColumn dc2 = new DataColumn();
            dc2.ColumnName = "NAME";
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            return dt;
        }
        /// <summary>
        /// 分配的奖金是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool CheckExist(string id, string year, string month)
        {
            string sql = " select * from performance.DISTRIBUTE_INDEX where EID='" + id + "' and YEAR='" + year + "' and MONTH='" + month + "'";
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
        /// <summary>
        /// 奖金分配
        /// </summary>
        /// <param name="id">报表ID</param>
        /// <param name="totalBonus">总奖金</param>
        /// <param name="name">报表名称</param>
        /// <param name="typeid">类型</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public void DistributeBonus(string id, double totalBonus, string name, string typeid, string year, string month)
        {
            string sqlE = "select EVALUATE_DEPT_CODE dept_code,evaluate_dept_name as dept_name,evaluate_value as score from ";
            sqlE += " hospitalsys.EVALUATE_INFORMATION a ";
            sqlE += " inner join ";
            sqlE += " hospitalsys.EVALUATE_RESULT_LIST b ";
            sqlE += " on a.EVALUATE_CODE=b.EVALUATE_CODE ";
            sqlE += " where a.EVALUATE_CODE='" + id + "' order by b.EVALUATE_DEPT_CODE";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sqlE.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {


                double totalScore = 0.0;
                string sqlT = " select nvl(round(sum(evaluate_value),2),0) totalscore from hospitalsys.EVALUATE_RESULT_LIST where EVALUATE_CODE='" + id + "'";
                DataSet dsT = OracleOledbBase.ExecuteDataSet(sqlT.ToString());
                if (dsT.Tables.Count > 0 && dsT.Tables[0].Rows.Count > 0)
                {
                    totalScore = Convert.ToDouble(dsT.Tables[0].Rows[0]["totalscore"]);

                }
                if (totalScore == 0)
                {
                    throw new Exception("总分为0,无法分配");
                }
                //添加主表
                int maxID = OracleOledbBase.GetMaxID("ID", "performance.DISTRIBUTE_INDEX");
                MyLists listtable = new MyLists();
                string sqlIndex = " insert into performance.DISTRIBUTE_INDEX (ID,ENAME,ETYPE ,YEAR ,MONTH,TOTALBONUS ,TOTALSCORE,EID)";
                sqlIndex += " values ('" + maxID + "','" + name + "','" + typeid + "','" + year + "','" + month + "'," + totalBonus + "," + totalScore + ",'" + id + "')";
                List lindex = new List();
                lindex.StrSql = sqlIndex;
                lindex.Parameters = new OleDbParameter[] { };
                listtable.Add(lindex);
                ///添加明细
                double ave = Math.Round(totalBonus / totalScore, 2);
                int maxDetailID = OracleOledbBase.GetMaxID("DETAILID", "performance.DISTRIBUTE_DETAIL");
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into performance.DISTRIBUTE_DETAIL (DETAILID,DEPT_CODE,DEPT_NAME,SCORE,DBONUS,DINDEX)");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sb.Append(" select " + (maxDetailID + i) + ",");
                    sb.Append(" '" + ds.Tables[0].Rows[i]["dept_code"] + "',");
                    sb.Append(" '" + ds.Tables[0].Rows[i]["dept_name"] + "',");
                    sb.Append(" '" + ds.Tables[0].Rows[i]["score"] + "',");
                    sb.Append(" '" + Convert.ToDouble(ds.Tables[0].Rows[i]["score"]) * (ave) + "',");
                    sb.Append(" '" + maxID + "'");
                    sb.Append("from dual union all ");
                }
                sb.Remove(sb.Length - 10, 10);
                List lDetail = new List();
                lDetail.StrSql = sb.ToString();
                lDetail.Parameters = new OleDbParameter[] { };
                listtable.Add(lDetail);

                OracleOledbBase.ExecuteTranslist(listtable);
            }
        }
        /// <summary>
        /// 删除分配的奖金
        /// </summary>
        /// <param name="id"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public void DeleteDistributeBonus(string id, string year, string month)
        {
            MyLists listtable = new MyLists();
            string sqlD = " delete from performance.DISTRIBUTE_DETAIL where DINDEX=(select max(ID) from performance.DISTRIBUTE_INDEX where EID='" + id + "' and YEAR='" + year + "' and MONTH='" + month + "')";
            List lDetail = new List();
            lDetail.StrSql = sqlD;
            lDetail.Parameters = new OleDbParameter[] { };
            listtable.Add(lDetail);

            string sqlI = " delete from performance.DISTRIBUTE_INDEX where EID='" + id + "' and YEAR='" + year + "' and MONTH='" + month + "'";
            List lindex = new List();
            lindex.StrSql = sqlI;
            lindex.Parameters = new OleDbParameter[] { };
            listtable.Add(lindex);

            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 查询奖金平衡计分卡奖惩结果
        /// </summary>
        /// <param name="st_date"></param>
        /// <returns></returns>
        public DataTable GetBonusBSC(string st_date)
        {
            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat(@"SELECT   DISTINCT guide_code, guide_name
  FROM   PERFORMANCE.BONUS_BSC_DATA
 WHERE   TO_CHAR (ST_DATE, 'yyyymm') = '{0}'
 order by guide_code", st_date);

            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT  a.dept_code, b.dept_name  \"科室名称\"");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string guide_name = table.Rows[i]["GUIDE_NAME"].ToString();
                string str_name = guide_name.Replace(".", "");
                str.AppendFormat("  ,SUM (CASE WHEN guide_code = '{0}' THEN guide_cause  END) \"{1}\"", table.Rows[i]["GUIDE_CODE"].ToString(), str_name + "目标值");
                str.AppendFormat("  ,SUM (CASE WHEN guide_code = '{0}' THEN guide_value  END) \"{1}\"", table.Rows[i]["GUIDE_CODE"].ToString(), str_name + "实际值");
                str.AppendFormat("  ,SUM (CASE WHEN guide_code = '{0}' THEN bonus_score  END) \"{1}\"", table.Rows[i]["GUIDE_CODE"].ToString(), str_name + "奖惩");
            }
            str.AppendFormat(@"FROM   PERFORMANCE.BONUS_BSC_DATA a, comm.sys_dept_dict b
   WHERE   a.dept_code = B.DEPT_CODE and TO_CHAR (ST_DATE, 'yyyymm')='{0}'", st_date);

            str.AppendFormat(" GROUP BY   b.dept_name, a.dept_code");
            str.AppendFormat(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 查询奖金平衡计分卡奖惩结果
        /// </summary>
        /// <param name="st_date"></param>
        /// <returns></returns>
        public DataTable GetBonusBSC(string st_date,string end_date, string dept_code)
        {

            StringBuilder str1 = new StringBuilder();
            str1.AppendFormat(@"SELECT   DISTINCT guide_code, guide_name
  FROM   PERFORMANCE.BONUS_BSC_DATA
 WHERE   TO_CHAR (ST_DATE, 'yyyymm') >= '{0}' and TO_CHAR (ST_DATE, 'yyyymm') <= '{1}'
 order by guide_code", st_date, end_date);

            DataTable table = OracleOledbBase.ExecuteDataSet(str1.ToString()).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT  a.dept_code, b.dept_name  \"科室名称\"");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string guide_name = table.Rows[i]["GUIDE_NAME"].ToString();
                string str_name = guide_name.Replace(".", "");
                str.AppendFormat("  ,SUM (CASE WHEN guide_code = '{0}' THEN guide_cause  END) \"{1}\"", table.Rows[i]["GUIDE_CODE"].ToString(), str_name + "目标值");
                str.AppendFormat("  ,SUM (CASE WHEN guide_code = '{0}' THEN guide_value  END) \"{1}\"", table.Rows[i]["GUIDE_CODE"].ToString(), str_name + "实际值");
                str.AppendFormat("  ,SUM (CASE WHEN guide_code = '{0}' THEN bonus_score  END) \"{1}\"", table.Rows[i]["GUIDE_CODE"].ToString(), str_name + "奖惩");
            }
            str.AppendFormat(@"FROM   PERFORMANCE.BONUS_BSC_DATA a, comm.sys_dept_dict b
   WHERE   a.dept_code = B.DEPT_CODE and TO_CHAR (ST_DATE, 'yyyymm') >= '{0}' and TO_CHAR (ST_DATE, 'yyyymm') <= '{1}' ", st_date, end_date);
            if (dept_code != "")
            {
                str.AppendFormat(" and b.dept_code in ({0})", dept_code);
            }
            str.AppendFormat(" GROUP BY   b.dept_name, a.dept_code");
            str.AppendFormat(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

    }
}
