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
    public class SimpleEncourage
    {
        /// <summary>
        /// 查询单项奖惩类别设置表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSimpleEncourage()
        {
           
            string sql = "select ID,ITEMNAME,CHECKSTAN,REMARK from PERFORMANCE.SET_SINGLEAWARDDICT ";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleEncourage();
            }

        }
        /// <summary>
        /// 查询单项奖惩类别设置表数据根据ID
        /// </summary>
        /// <returns></returns>
        public DataTable GetSimpleEncourage(string id)
        {
            string sql = "select ID,ITEMNAME,CHECKSTAN,REMARK from PERFORMANCE.SET_SINGLEAWARDDICT where ID=" + id + "";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleEncourage();
            }

        }
        /// <summary>
        /// 组合单项奖惩类别设置表结构
        /// </summary>
        /// <returns></returns>
        private DataTable BuildSimpleEncourage()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID");
            DataColumn dcITEMNAME = new DataColumn("ITEMNAME");
            DataColumn dcCHECKSTAN = new DataColumn("CHECKSTAN");
            DataColumn dcREMARK = new DataColumn("REMARK");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcITEMNAME, dcCHECKSTAN, dcREMARK });
            return dt;
        }

        public void UpdateSimpleEncourage(string id,string itemname,string checkstan,string remark)
        {
            string sql = " update PERFORMANCE.SET_SINGLEAWARDDICT set ITEMNAME='" + itemname + "',CHECKSTAN='" + checkstan + "',REMARK='"+remark+"' where ID='"+id+"'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        public void InsertSimpleEncourage(string itemname, string checkstan, string remark)
        {
            string sql = " insert into PERFORMANCE.SET_SINGLEAWARDDICT (ID,ITEMNAME,CHECKSTAN,REMARK) ";
                sql += " select max(ID)+1,'" + itemname + "','" + checkstan + "','" + remark + "' from PERFORMANCE.SET_SINGLEAWARDDICT ";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

        public void DeleteSimpleEncourage(string id)
        {
            string sql = " delete PERFORMANCE.SET_SINGLEAWARDDICT  where ID='" + id + "'";
            OracleOledbBase.ExecuteNonQuery(sql);
        }

    }
}
