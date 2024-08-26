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
    public class WebServiceDept
    {
        /// <summary>
        /// 人员部门CODE
        /// </summary>
        /// <param name="inputcode">部门输入编码</param>
        /// <returns>部门信息</returns>
        public DataSet GetAccountDept(string inputcode)
        {
            try
            {
                
                if (inputcode == "")
                {
                    string strSql = string.Format("select DEPT_CODE, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a WHERE a.ATTR='是' and show_flag=0   order by a.DEPT_CODE", DataUser.COMM);
                    return OracleOledbBase.ExecuteDataSet(strSql);
                }
                else
                {
                    string strSql = string.Format("select DEPT_CODE, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a WHERE a.ATTR='是' and show_flag=0 and a.input_code like ? order by a.DEPT_CODE", DataUser.COMM);
                    OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
                    return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
                }
            }
            catch
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtDept());
                return ds;
            }
        }
        public DataTable dtDept()
        {
            DataTable dt = new DataTable();
            DataColumn dcDept_Code = new DataColumn();
            dcDept_Code.ColumnName = "DEPT_CODE";
            dt.Columns.Add(dcDept_Code);
            DataColumn dcDept_Name = new DataColumn();
            dcDept_Name.ColumnName = "DEPT_NAME";
            dt.Columns.Add(dcDept_Name);
            DataColumn dcInput_Code = new DataColumn();
            dcInput_Code.ColumnName = "INPUT_CODE";
            dt.Columns.Add(dcInput_Code);
            return dt;

        }
    }
}
