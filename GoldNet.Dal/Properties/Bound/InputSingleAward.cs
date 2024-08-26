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
    public class InputSingleAward
    {
        /// <summary>
        /// 查询效益调整数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSingleAwardList(User user)
        {
            SE_ROLE dal_se = new SE_ROLE();
            string where = " and TYPE_ID" + dal_se.GetSEID(user.UserId);
            string sql = "select ID,to_char(AWARD_DATE,'yyyy-mm-dd') as AWARDDATE,DEPT_NAME as DEPTNAME,TYPE_NAME as TYPENAME,round(MONEY,2) MONEY from PERFORMANCE.INPUT_SINGLEAWARD where DEL_FLAG=0 " + where + " order by AWARD_DATE desc";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSingleAward();
            }

        }
        public DataTable GetSingleAwardList(string condition, User user, string datetime)
        {
            StringBuilder sql = new StringBuilder();
            SE_ROLE dal_se = new SE_ROLE();
            string where = " and TYPE_ID " + dal_se.GetSEID(user.UserId);
            sql.AppendFormat("select ID,to_char(AWARD_DATE,'yyyy-mm-dd') as AWARDDATE,DEPT_NAME as DEPTNAME,TYPE_NAME as TYPENAME,round(MONEY,2) MONEY from PERFORMANCE.INPUT_SINGLEAWARD where DEL_FLAG=0  " + where + " ");
            if (condition != "")
            {
                sql.AppendFormat(" and {0}", condition);
            }
            if (datetime != "")
            {
                sql.AppendFormat(" and to_char(AWARD_DATE,'yyyyMM')='{0}'", datetime);
            }
            sql.Append(" order by INPUTE_DATE desc");
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSingleAward();
            }

        }
        public DataTable GetKejijin(string datetime)
        {
            StringBuilder sql = new StringBuilder();
            SE_ROLE dal_se = new SE_ROLE();

            sql.AppendFormat("select ID,to_char(AWARD_DATE,'yyyy-mm-dd') as AWARDDATE,DEPT_NAME as DEPTNAME,TYPE_NAME as TYPENAME,round(MONEY,2) MONEY,round(SY_MONEY,2) SY_MONEY from PERFORMANCE.KEJIJIN where DEL_FLAG=0  ");

            if (datetime != "")
            {
                sql.AppendFormat(" and to_char(AWARD_DATE,'yyyyMM')='{0}'", datetime);
            }
            sql.Append(" order by INPUTE_DATE desc");
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSingleAward();
            }

        }

        public bool GetIsNullDrjj(DateTime datetime, string dept_code)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"select count(*) from performance.jj_dr where substr(st_date,0,6)=to_char(to_date('{0}', 'yyyy-mm-dd hh24:mi:ss'),'yyyymm') AND dept_code='{1}' ", datetime, dept_code);

            return int.Parse(OracleOledbBase.ExecuteScalar(sql.ToString()).ToString()) < 1;

        }

        public DataTable GetSimpleType(User user)
        {
            SE_ROLE dal_se = new SE_ROLE();
            string where = " where ID " + dal_se.GetSEID(user.UserId);
            string sql = "select ID TYPEID,ITEMNAME TYPENAME from PERFORMANCE.SET_SINGLEAWARDDICT " + where;
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleType();
            }
        }
        public DataTable GetSimpleType(string id)
        {
            string sql = "select ID TYPEID,ITEMNAME TYPENAME,CHECKSTAN,REMARK from PERFORMANCE.SET_SINGLEAWARDDICT where ID='" + id + "'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleType();
            }
        }
        public DataTable GetSimpleType_kjj(string id)
        {
            string sql = "select ID TYPEID,ITEMNAME TYPENAME,CHECKSTAN,REMARK from PERFORMANCE.KEJIJIN where ID='" + id + "'";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleType();
            }
        }
        /// <summary>
        /// 根据ID查询效益调整数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSingleAwardSimple(string id)
        {
            string sql = "select ID,DEPT_CODE,DEPT_NAME,TYPE_ID,TYPE_NAME,CHECKSTAN,REMARK,round(MONEY,2) MONEY,AWARD_DATE,INPUTER,INPUTE_DATE,MODIFIER,MODIFY_DATE from PERFORMANCE.INPUT_SINGLEAWARD where ID=" + id + "";
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
        public DataTable GetSingKejijin(string id)
        {
            string sql = "select ID,DEPT_CODE,DEPT_NAME,TYPE_ID,TYPE_NAME,CHECKSTAN,REMARK,round(MONEY,2) MONEY,round(SY_MONEY,2) SY_MONEY,AWARD_DATE,INPUTER,INPUTE_DATE,MODIFIER,MODIFY_DATE from PERFORMANCE.KEJIJIN where ID=" + id + "";
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
        private DataTable BuildSingleAward()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID");
            DataColumn dcDEPTNAME = new DataColumn("DEPTNAME");
            DataColumn dcADJUSTDATE = new DataColumn("AWARDDATE");
            DataColumn dcTYPE = new DataColumn("TYPENAME");
            DataColumn dcMONEY = new DataColumn("MONEY");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcDEPTNAME, dcADJUSTDATE, dcTYPE, dcMONEY });
            return dt;
        }
        private DataTable BuildSimpleType()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID");
            DataColumn dcITEMNAME = new DataColumn("ITEMNAME");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcITEMNAME });
            return dt;
        }
        public void UpdateSingleAward(string id, string deptname, string deptid, string typeid, string typename, string checkstan, string remark, double money, DateTime awarddate, string modifier)
        {
            string sql = " update PERFORMANCE.INPUT_SINGLEAWARD set DEPT_CODE='" + deptid + "',DEPT_NAME='" + deptname + "',TYPE_ID='" + typeid + "',TYPE_NAME='" + typename + "',";
            sql += " CHECKSTAN='" + checkstan + "',REMARK='" + remark + "',MONEY=" + money + ",AWARD_DATE=to_date('" + awarddate.ToString("yyyyMMdd") + "','yyyymmdd'),MODIFIER='" + modifier + "',";
            sql += " MODIFY_DATE=sysdate  where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        public void UpdateKjj(string id, string deptname, string deptid, string typeid, string typename, string checkstan, string remark, double money, DateTime awarddate, string modifier)
        {
            string sql = " update PERFORMANCE.KEJIJIN set DEPT_CODE='" + deptid + "',DEPT_NAME='" + deptname + "',TYPE_ID='" + typeid + "',TYPE_NAME='" + typename + "',";
            sql += " CHECKSTAN='" + checkstan + "',REMARK='" + remark + "',MONEY=" + money + ",AWARD_DATE=to_date('" + awarddate.ToString("yyyyMMdd") + "','yyyymmdd'),MODIFIER='" + modifier + "',";
            sql += "sy_money=nvl((SELECT nvl(sy_money,0) FROM PERFORMANCE.KEJIJIN a WHERE id=(SELECT MAX(ID) FROM PERFORMANCE.KEJIJIN  WHERE dept_code =" + deptid + ") and a.ID<>'" + id + "'),0)+" + money + " ,";
            sql += " MODIFY_DATE=sysdate  where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        public void InsertSingleAward(string deptname, string deptid, string typeid, string typename, string checkstan, string remark, double money, DateTime awarddate, string inputer)
        {
            string sql = " insert into PERFORMANCE.INPUT_SINGLEAWARD (ID,DEPT_CODE,DEPT_NAME,TYPE_ID,TYPE_NAME,CHECKSTAN,REMARK,MONEY,AWARD_DATE,INPUTER,INPUTE_DATE,DEL_FLAG) ";
            sql += " select nvl(MAX (ID),0) + 1,'" + deptid + "','" + deptname + "','" + typeid + "','" + typename + "','" + checkstan + "','" + remark + "'," + money + ", ";
            sql += " to_date('" + awarddate.ToString("yyyyMMdd") + "','yyyymmdd'),'" + inputer + "',sysdate,0";
            sql += " from PERFORMANCE.INPUT_SINGLEAWARD ";
            OracleOledbBase.ExecuteNonQuery(sql);
        }
        public void InsertSingleAward_KJJ(string deptname, string deptid, string typeid, string typename, string checkstan, string remark, double money, DateTime awarddate, string inputer)
        {
            string sql = " insert into PERFORMANCE.KEJIJIN (ID,DEPT_CODE,DEPT_NAME,TYPE_ID,TYPE_NAME,CHECKSTAN,REMARK,MONEY,AWARD_DATE,INPUTER,INPUTE_DATE,DEL_FLAG,sy_money) ";
            sql += " select NVL((SELECT MAX(ID) FROM PERFORMANCE.KEJIJIN), 0) + 1,'" + deptid + "','" + deptname + "','" + typeid + "','" + typename + "','" + checkstan + "','" + remark + "'," + money + ", ";
            sql += " to_date('" + awarddate.ToString("yyyyMMdd") + "','yyyymmdd'),'" + inputer + "',sysdate,0";
            sql += ",nvl((SELECT nvl(sy_money,0) FROM PERFORMANCE.KEJIJIN WHERE id=(SELECT MAX(ID) FROM PERFORMANCE.KEJIJIN WHERE dept_code =" + deptid + ")),0)+" + money + "";
            sql += " from dual";
            OracleOledbBase.ExecuteNonQuery(sql);
        }
        public void DeleteSingleAward(string id)
        {
            string sql = " update PERFORMANCE.INPUT_SINGLEAWARD set DEL_FLAG=1 where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }


        public int ResultDept(DateTime date, string dept_code)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT count(dept_code) from {0}.KEJIJIN
                                 WHERE  to_char(AWARD_DATE,'yyyyMM')=to_char(to_date('{1}', 'YYYY-MM-DD HH24:MI:SS'),
               'yyyyMM') and dept_code='{2}' ", DataUser.PERFORMANCE, date, dept_code);

            return int.Parse(OracleOledbBase.ExecuteScalar(str.ToString()).ToString());
        }
    }
}
