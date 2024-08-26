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
    public class Income_Difference_Dal
    {
        //获取临时表结构
        public DataTable GetTableColumn(string tableName)
        {
            string sql = "select * from all_COL_COMMENTS m where M.TABLE_NAME=UPPER('" + tableName + "_TMP')";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        //获取对比收临时表数据
        public DataTable GetTableData(string tableName)
        {
            string sql = string.Format("select rowid as row_id,a.* from {0}." + tableName + "_TMP a", DataUser.HISDATA);
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        //执行SP_INCOMESOURCE_UPDATE过程
        public string Exec_Sp_Incomesource_Minus(string comparemonth, string sourcetype)
        {
            string ProcName = DataUser.CBHS + ".SP_INCOMESOURCE_MINUS";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("comparemonth", comparemonth),
                                              new OleDbParameter("sourcetype",sourcetype),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
        //执行SP_INCOMESOURCE_UPDATE过程
        public string Exec_Sp_Incomesource_Update(string sourcetype, string row_id, string comparemonth)
        {
            string ProcName = DataUser.CBHS + ".SP_INCOMESOURCE_UPDATE";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("sourcetype", sourcetype),
                                              new OleDbParameter("rowids",row_id),
                                              new OleDbParameter("comparemonth",comparemonth),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
        //从新提取收入过程
        public string Exec_Extrac_Income_Data(string date_time,string table_names)
        {
            string ProcName = DataUser.CBHS + ".EXTRAC_INCOME_DATA";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("incomedate", date_time),
                                              new OleDbParameter("tablenames",table_names),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }


    }
}
