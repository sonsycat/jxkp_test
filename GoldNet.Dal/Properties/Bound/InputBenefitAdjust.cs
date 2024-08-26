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
    public class InputBenefitAdjust
    {
        /// <summary>
        /// 查询效益调整数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetBenefitAdjustList()
        {
            string sql = "select ID,DEPT_NAME as DEPTNAME,to_char(ADJUST_DATE,'yyyy-mm-dd') as ADJUSTDATE,TYPE,round(MONEY,2) MONEY,DIRECTION,INPUTER from PERFORMANCE.INPUT_BENEFITADJUST where DEL_FLAG=0 order by ADJUST_DATE desc";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildBenefitAdjust();
            }

        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable GetBenefitAdjustListt(string filter)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select ID,DEPT_NAME as DEPTNAME,to_char(ADJUST_DATE,'yyyy-mm-dd') as ADJUSTDATE,TYPE,round(MONEY,2) MONEY,DIRECTION,INPUTER from PERFORMANCE.INPUT_BENEFITADJUST where DEL_FLAG=0 ");
            if (!filter.Equals(""))
            {
                sql.AppendFormat(" and (dept_code like '{0}%' or dept_name like '{0}%' or to_char(ADJUST_DATE,'yyyy-mm-dd') like '{0}%')", filter.ToUpper());
            }
            sql.Append(" order by ADJUST_DATE desc");
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildBenefitAdjust();
            }

        }
        public DataTable GetBenefitAdjustList(string condition,string datetime)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select ID,DEPT_NAME as DEPTNAME,to_char(ADJUST_DATE,'yyyy-mm-dd') as ADJUSTDATE,TYPE,round(MONEY,2) MONEY,DIRECTION,INPUTER from PERFORMANCE.INPUT_BENEFITADJUST where DEL_FLAG=0  ");
            if (condition != "")
            {
                sql.AppendFormat(" and {0}",condition);
            }
            if (datetime != "")
            {
                sql.AppendFormat(" and to_char(ADJUST_DATE,'yyyyMM')='{0}'",datetime);
            }
            sql.Append(" order by ADJUST_DATE desc");
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildBenefitAdjust();
            }

        }
        public DataTable GetDept()
        {
            string sql = "select DEPT_CODE DEPTID,DEPT_NAME DEPTNAME from SYS_DEPT_DICT where ATTR='是'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildDept();
            }
        }
        /// <summary>
        /// 根据ID查询效益调整数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetBenefitAdjustSimple(string id)
        {
            string sql = "select ID,DEPT_CODE,DEPT_NAME,TYPE,DIRECTION,round(MONEY,2) MONEY,MEMO,ADJUST_DATE,INPUTER,INPUTE_DATE,MODIFIER,MODIFY_DATE,INPUTER,INPUTE_DATE from PERFORMANCE.INPUT_BENEFITADJUST where ID=" + id + "";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 设置表结构
        /// </summary>
        /// <returns></returns>
        private DataTable BuildBenefitAdjust()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            DataColumn dcADJUSTDATE = new DataColumn("ADJUSTDATE");
            DataColumn dcTYPE = new DataColumn("TYPE");
            DataColumn dcMONEY = new DataColumn("MONEY");
            DataColumn dcDIRECTION = new DataColumn("DIRECTION");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcDEPTNAME, dcADJUSTDATE, dcTYPE, dcMONEY, dcDIRECTION });
            return dt;
        }
        private DataTable BuildDept()
         {
            DataTable dt = new DataTable();
            DataColumn dcDEPTCODE = new DataColumn("DEPTCODE");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            dt.Columns.AddRange(new DataColumn[] { dcDEPTCODE, dcDEPTNAME });
            return dt;
        }
        public void UpdateBenefitAdjust(string id, string deptname, string deptid, string type, string direction, double money, string memo, DateTime adjustdate, string modifier)
        {
            string sql = " update PERFORMANCE.INPUT_BENEFITADJUST set DEPT_CODE='" + deptid + "',DEPT_NAME='" + deptname + "',TYPE='" + type + "',DIRECTION='" + direction + "',";
            sql += " MONEY=" + money + ",MEMO='" + memo + "',ADJUST_DATE=to_date('" + adjustdate.ToString("yyyyMMdd") + "','yyyymmdd'),MODIFIER='" + modifier + "',";
            sql += " MODIFY_DATE=sysdate  where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        public void InsertBenefitAdjust(string deptname, string deptid, string type, string direction, double money, string memo, DateTime adjustdate, string inputer)
        {
            string sql = " insert into PERFORMANCE.INPUT_BENEFITADJUST (ID,DEPT_CODE,DEPT_NAME,TYPE,DIRECTION,MONEY,MEMO,ADJUST_DATE,INPUTER,INPUTE_DATE,DEL_FLAG) ";
            sql += " select nvl(MAX (ID),0) + 1,'" + deptid + "','" + deptname + "','" + type + "','" + direction + "'," + money + ",'" + memo + "', ";
            sql += " to_date('" + adjustdate.ToString("yyyyMMdd") + "','yyyymmdd'),'" + inputer + "',sysdate,0";
            sql += " from PERFORMANCE.INPUT_BENEFITADJUST ";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        public void DeleteBenefitAdjust(string id)
        {
            string sql = " update PERFORMANCE.INPUT_BENEFITADJUST set DEL_FLAG=1 where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }
    }
}
