using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm;
using System.Data.OleDb;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OracleClient;
using System.Collections;

namespace Goldnet.Dal
{
    public class Cost_Item_Type
    {
        /// <summary>
        /// 分析报表类别初始化树结构
        /// </summary>
        /// <returns></returns>
        public DataSet GetCostType()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                  SELECT COST_TYPE_CODE CLASSID,COST_TYPE_CODE,COST_TYPE_NAME CLASSNAME,nvl(COST_TYPE_CODE_P,0) CLASSPID,
                    LEVEL LEV, CONNECT_BY_ISLEAF ISLEAF
                   FROM {0}.CBHS_COST_TYPE_DICT
                   START WITH COST_TYPE_CODE_P= '0'
                CONNECT BY PRIOR COST_TYPE_CODE = COST_TYPE_CODE_P   ", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 删除报表类别
        /// </summary>class
        /// <param name="id">报表类别ID</param>
        public void DelCostType(string id)
        {
            string sql = string.Format(@"delete {1}.CBHS_COST_TYPE_DICT where COST_TYPE_CODE = '{0}'", id, DataUser.CBHS);

            OracleOledbBase.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改报表类别名称
        /// </summary>class
        /// <param name="id">id</param>
        /// <param name="name">报表类别名称</param>
        public void UpdateCostType(Dictionary<string, string>[] treeNodes)
        {
            if (treeNodes.Length > 0)
            {
                for (int i = 0; i < treeNodes.Length; i++)
                {
                
                }
                MyLists list = new MyLists();

                List l_listDelSql = new List();
                l_listDelSql.Parameters = new OleDbParameter[] { new OleDbParameter() };
                l_listDelSql.StrSql = string.Format(" DELETE FROM {0}.CBHS_COST_TYPE_DICT ", DataUser.CBHS);
                list.Add(l_listDelSql);

                ConvertPY convert = new ConvertPY();
                StringBuilder sqlstr = new StringBuilder();
                string sql = "";
                string classtxt = "";
                for (int i = 0; i < treeNodes.Length; i++)
                {
                    classtxt = treeNodes[i]["Text"].Length > 30 ? treeNodes[i]["Text"].Substring(1, 30) : treeNodes[i]["Text"];
                    sqlstr.AppendFormat(" SELECT '{0}' AS ID, '{1}' AS COST_TYPE_CODE, '{2}' AS COST_TYPE_NAME ,{3} AS COST_TYPE_CODE_P,{4} AS INPUT_CODE FROM DUAL UNION ALL "
                        , i+1
                        , treeNodes[i]["Id"]
                        , classtxt.Replace("'", "''")
                        , "'" + treeNodes[i]["Pid"] + "'"
                        , "'" + convert.GetPYString(classtxt) + "'"
                         );
                }
                sql = sqlstr.ToString() + ")";
                sql = sql.Replace("UNION ALL )", "");
                sql = string.Format(" INSERT INTO {0}.CBHS_COST_TYPE_DICT  (ID, COST_TYPE_CODE, COST_TYPE_NAME, COST_TYPE_CODE_P,INPUT_CODE) ", DataUser.CBHS) + sql;

                List l_listUpdate = new List();
                l_listUpdate.Parameters = new OleDbParameter[] { new OleDbParameter() };
                l_listUpdate.StrSql = sql;
                list.Add(l_listUpdate);
                OracleOledbBase.ExecuteTranslist(list);
            }
        }

        /// <summary>
        /// 判断下级是否有数据
        /// </summary>list
        /// <param name="id">id</param>
        /// <returns>true:不存在数据 false:存在数据</returns>
        public bool EstimateData(string id)
        {
            OleDbParameter[] pram = new OleDbParameter[] { };
            int num = -1;
            string sql = string.Format("select count(*) from {1}.CBHS_COST_ITEM_DICT where ITEM_TYPE = '{0}'", id, DataUser.CBHS);
            num = Convert.ToInt32(OracleOledbBase.GetSingle(sql, pram));
            return num > 0 ? false : true;
        }

        /// <summary>
        /// 判断是否有下级节点
        /// </summary>list
        /// <param name="id">id</param>
        /// <returns>true:不存在数据 false:存在数据</returns>
        public bool EstimatePData(string id)
        {
            OleDbParameter[] pram = new OleDbParameter[] { };
            int num = -1;
            string sql = string.Format("select count(*) from {1}.CBHS_COST_TYPE_DICT where COST_TYPE_CODE_P = '{0}'", id, DataUser.CBHS);
            num = Convert.ToInt32(OracleOledbBase.GetSingle(sql, pram));
            return num > 0 ? false : true;
        }
    }
}
