using System;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class BoundComm
    {
        /// <summary>
        /// 生成年数据，当前年份上下5年
        /// </summary>
        /// <returns></returns>
        public DataTable getYears()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();
            dc.ColumnName = "YEAR";
            dt.Columns.Add(dc);
            int year = DateTime.Now.Year;
            for (int i = year - 10; i <= year + 5; i++)
            {
                DataRow dr = dt.NewRow();
                dr["YEAR"] = i;
                dt.Rows.Add(dr);

            }
            return dt;
        }
        /// <summary>
        /// 生成1-12月份
        /// </summary>
        /// <returns></returns>
        public DataTable getMonth()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();
            dc.ColumnName = "MONTH";
            dt.Columns.Add(dc);
            for (int i = 1; i <= 12; i++)
            {
                DataRow dr = dt.NewRow();
                dr["MONTH"] = i;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 条件查询下拉列表框数据
        /// </summary>
        /// <returns></returns>
        public DataTable dtDate()
        {
            DataTable dt = new DataTable();
            DataColumn dcKey = new DataColumn();
            dcKey.ColumnName = "Key";
            dt.Columns.Add(dcKey);
            DataColumn dcValue = new DataColumn();
            dcValue.ColumnName = "Value";
            dt.Columns.Add(dcValue);
            DataRow dr1 = dt.NewRow();
            dr1["Key"] = "TiaoJian";
            dr1["Value"] = "条件";
            DataRow dr2 = dt.NewRow();
            dr2["Key"] = "DaYu";
            dr2["Value"] = "大于";
            DataRow dr3 = dt.NewRow();
            dr3["Key"] = "DaYuDengYu";
            dr3["Value"] = "大于等于";
            DataRow dr4 = dt.NewRow();
            dr4["Key"] = "XiaoYu";
            dr4["Value"] = "小于";
            DataRow dr5 = dt.NewRow();
            dr5["Key"] = "XiaoYuDengYu";
            dr5["Value"] = "小于等于";
            DataRow dr6 = dt.NewRow();
            dr6["Key"] = "BuDengYu";
            dr6["Value"] = "不等于";
            DataRow dr7 = dt.NewRow();
            dr7["Key"] = "DengYu";
            dr7["Value"] = "等于";

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            dt.Rows.Add(dr3);
            dt.Rows.Add(dr4);
            dt.Rows.Add(dr5);
            dt.Rows.Add(dr6);
            dt.Rows.Add(dr7);

            return dt;
        }

        /// <summary>
        /// 奖金查询中条件查询下拉列表框数据
        /// </summary>
        /// <returns></returns>
        public DataTable dtBonusAmount()
        {
            DataTable dt = new DataTable();
            DataColumn dcKey = new DataColumn();
            dcKey.ColumnName = "Key";
            dt.Columns.Add(dcKey);
            DataColumn dcValue = new DataColumn();
            dcValue.ColumnName = "Value";
            dt.Columns.Add(dcValue);
            DataRow dr2 = dt.NewRow();
            dr2["Key"] = ">";
            dr2["Value"] = "大于";
            DataRow dr3 = dt.NewRow();
            dr3["Key"] = ">=";
            dr3["Value"] = "大于等于";
            DataRow dr4 = dt.NewRow();
            dr4["Key"] = "<";
            dr4["Value"] = "小于";
            DataRow dr5 = dt.NewRow();
            dr5["Key"] = "=<";
            dr5["Value"] = "小于等于";
            DataRow dr6 = dt.NewRow();
            dr6["Key"] = "<>";
            dr6["Value"] = "不等于";
            DataRow dr7 = dt.NewRow();
            dr7["Key"] = "=";
            dr7["Value"] = "等于";

            dt.Rows.Add(dr2);
            dt.Rows.Add(dr3);
            dt.Rows.Add(dr4);
            dt.Rows.Add(dr5);
            dt.Rows.Add(dr6);
            dt.Rows.Add(dr7);

            return dt;
        }

        public DataTable dtCondition()
        {
            DataTable dt = new DataTable();
            DataColumn dcKey = new DataColumn();
            dcKey.ColumnName = "Key";
            dt.Columns.Add(dcKey);
            DataColumn dcValue = new DataColumn();
            dcValue.ColumnName = "Value";
            dt.Columns.Add(dcValue);
            DataRow dr1 = dt.NewRow();
            dr1["Key"] = "TiaoJian";
            dr1["Value"] = "条件";
            DataRow dr2 = dt.NewRow();
            dr2["Key"] = "DengYu";
            dr2["Value"] = "等于";
            DataRow dr3 = dt.NewRow();
            dr3["Key"] = "BaoHan";
            dr3["Value"] = "包含";

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            dt.Rows.Add(dr3);

            return dt;
        }
        public DataTable dtRelation()
        {
            DataTable dt = new DataTable();
            DataColumn dcKey = new DataColumn();
            dcKey.ColumnName = "Key";
            dt.Columns.Add(dcKey);
            DataColumn dcValue = new DataColumn();
            dcValue.ColumnName = "Value";
            dt.Columns.Add(dcValue);
            DataRow dr1 = dt.NewRow();
            dr1["Key"] = "BingQie";
            dr1["Value"] = "并且";
            DataRow dr2 = dt.NewRow();
            dr2["Key"] = "HuoZhe";
            dr2["Value"] = "或者";

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);

            return dt;
        }
        /// <summary>
        /// 人员部门CODE
        /// </summary>
        /// <param name="inputcode">部门输入编码</param>
        /// <returns>部门信息</returns>
        public DataSet GetAccountDept(string inputcode, string dept_date)
        {
            try
            {
                DateTime dtDept = Convert.ToDateTime(dept_date);
                if (inputcode == "")
                {
                    string strSql = string.Format("select DEPT_CODE, a.dept_name,a.input_code from {0}.sys_dept_info a WHERE a.ATTR='是' and to_char(DEPT_SNAP_DATE,'yyyymm')=to_char(to_date('" + dtDept.ToShortDateString() + "','yyyy-mm-dd'),'yyyymm') order by a.DEPT_CODE", DataUser.COMM);
                    return OracleOledbBase.ExecuteDataSet(strSql);
                }
                else
                {
                    string strSql = string.Format("select DEPT_CODE, a.dept_name,a.input_code from {0}.sys_dept_info a WHERE a.ATTR='是'and to_char(DEPT_SNAP_DATE,'yyyymm')=to_char(to_date('" + dtDept.ToShortDateString() + "','yyyy-mm-dd'),'yyyymm') AND a.input_code like ? order by a.DEPT_CODE", DataUser.COMM);
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
        /// <summary>
        /// 检查当前年月
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool GetAccountType(string year, string month, string accountType)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string sql = " select * from performance.SET_ACCOUNTDEPTTYPE where ST_DATE=to_date('" + year + month + "01" + "','yyyymmdd') and DEPT_TYPE not in ('" + accountType + "')";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql);
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
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public bool GetAccountTypeByGroup(string year, string month, string groupcode)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT A.*
                                  FROM PERFORMANCE.set_accountdepttype A,PERFORMANCE.SET_ACCOUNTTYPEGROUP B
                                 WHERE A.DEPT_TYPE=B.TYPE_CODE 
                                   AND A.st_date = TO_DATE ('{0}'||LPAD('{1}',2,0)||'01', 'yyyymmdd')
                                   AND B.GROUP_CODE = '{2}'", year, month,groupcode);

            DataSet ds = OracleOledbBase.ExecuteDataSet(str.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
