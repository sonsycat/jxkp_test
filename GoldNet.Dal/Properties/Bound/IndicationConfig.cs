using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class IndicationConfigDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetIndicationConfig()
        {
            string sql = "select ID,NAME,REMARK,PROPERTYVALUE from PERFORMANCE.SET_INDICATIONCONFIG where tag=0";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildIndicationConfig();
            }
        }

        /// <summary>
        /// 组合指标配置维护
        /// </summary>
        /// <returns></returns>
        private DataTable BuildIndicationConfig()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID");
            DataColumn dcNAME = new DataColumn("NAME");
            DataColumn dcREMARK = new DataColumn("REMARK");
            DataColumn dcPROPERTYVALUE = new DataColumn("PROPERTYVALUE");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcNAME, dcREMARK, dcPROPERTYVALUE });
            return dt;
        }

        /// <summary>
        ///更新修改过的指标配置的值
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool SaveDeptPercent(Dictionary<string, string>[] rows)
        {
          
            MyLists listtable = new MyLists();
          
            for (int i = 0; i < rows.Length; i++)
            {
                StringBuilder isql = new StringBuilder();
                isql.Append(" update PERFORMANCE.SET_INDICATIONCONFIG set ");
                isql.Append("PROPERTYVALUE=" + rows[i]["PROPERTYVALUE"].ToString() + "");
                isql.Append(" where id=");
                isql.Append("'" + rows[i]["ID"].ToString() + "'");    
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

    }
}
