using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace Goldnet.Dal
{
    public class FlowChart
    {
        public FlowChart()
        {
        }
        /// <summary>
        /// 获取流程状态数据
        /// </summary>
        /// <param name="date_time">日期</param>
        /// <returns>DataSet</returns>
        public DataSet GetFlowData(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT A.ID, A.P_ID, A.CLASS NAME, A.STEP, A.STATE, A.DATAVALUE, A.OPERATE,
                                               TO_CHAR(A.OPERATEDATE,'yyyy-mm-dd') OPERATEDATE, A.MEMO,B.PAGEURL URL 
                                    FROM {0}.CBHS_FLOW_CHART A,COMM.SYS_APPLICATION_MENU B
                                   WHERE A.URL=B.MENUID(+)
                                     AND B.MODID(+)='1'
                                     AND TO_CHAR(A.DATETIME,'yyyymm')='{1}'
                                     AND A.ID=A.P_ID
                                   ORDER BY A.ID", DataUser.CBHS,date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取流程状态数据（重载）
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="p_id"></param>
        /// <returns></returns>
        public DataSet getFlowData(string date_time, string p_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT A.ID, A.P_ID, A.NAME, A.STEP, A.STATE, A.DATAVALUE, A.OPERATE,
                                               TO_CHAR(A.OPERATEDATE,'yyyy-mm-dd') OPERATEDATE, A.MEMO ,B.PAGEURL URL
                                    FROM {0}.CBHS_FLOW_CHART A,COMM.SYS_APPLICATION_MENU B
                                   WHERE A.URL=B.MENUID(+)
                                     AND B.MODID(+)='1'
                                     AND TO_CHAR(A.DATETIME,'yyyymm')='{1}'
                                     AND A.P_ID='{2}' AND A.ID<>A.P_ID
                                   ORDER BY A.ID", DataUser.CBHS, date_time, p_id);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 将数据库查询结果转化为JSON格式字符串
        /// </summary>
        /// <param name="date_time">日期</param>
        /// <returns>JSON格式字符串</returns>
        public string GetJsonString(string date_time)
        {
            StringBuilder json = new StringBuilder();
            DataTable dt = new DataTable();
            dt = GetFlowData(date_time).Tables[0];
            string rt_json = "";
            if (dt.Rows.Count > 0)
            {
                rt_json = AppJsonString(dt, date_time, json,"open").ToString();
            }
            return rt_json;
        }
        public StringBuilder AppJsonString(DataTable dt, string date_time, StringBuilder json, string state)
        {
            if (dt.Rows.Count > 0)
            {
                json.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    json.Append("{");
	                json.Append("'name':'"+dt.Rows[i]["name"]+"',");
	                json.Append("'step':'"+dt.Rows[i]["step"]+"',");
                    json.Append("'stat':'" + dt.Rows[i]["state"] + "',");
	                json.Append("'datavalue':'"+dt.Rows[i]["datavalue"]+"',");
	                json.Append("'operate':'"+dt.Rows[i]["operate"]+"',");
	                json.Append("'operatedate':'"+dt.Rows[i]["operatedate"]+"',");
                    json.Append("'memo':'" + dt.Rows[i]["memo"] + "',");
                    json.Append("'url':'" + dt.Rows[i]["url"] + "'");
                    

                    DataTable dt_ch=getFlowData(date_time,dt.Rows[i]["id"].ToString()).Tables[0];
                    if (state != null)
                    {
                        json.Append(",'state':'" + state + "'");
                    }
                    else if (dt_ch.Rows.Count > 0)
                    {
                        json.Append(",'state':'closed'");
                    }
                    if (dt_ch.Rows.Count > 0)
                    {
                        json.Append(",");
                        json.Append("'children':");
                        json=AppJsonString(dt_ch, date_time,json,null);
                    }

                    json.Append("}");
                    if (i < dt.Rows.Count-1)
                    {
                        json.Append(",");
                    }
                }
                json.Append("]");
            }
            return json;
        }
        /// <summary>
        /// 执行状态检查过程
        /// </summary>
        /// <param name="date_time"></param>
        /// <returns></returns>
        public string Exec_Sp_Flow_Chart(string date_time)
        {
            string ProcName = DataUser.CBHS + ".SP_FLOW_CHART";
            OleDbParameter rtnmsg = new OleDbParameter("rtnmsg", System.Data.OleDb.OleDbType.VarChar, 200);
            rtnmsg.Direction = ParameterDirection.Output;
            OleDbParameter[] parameteradd = { new OleDbParameter("accountdate", date_time),
                                              rtnmsg};
            OracleOledbBase.RunProcedure(ProcName, parameteradd);
            return rtnmsg.Value.ToString();
        }
    }
}
