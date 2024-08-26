using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;
using System.Collections;
namespace Goldnet.Dal
{
	/// <summary>
	/// SYS_DEPT_DICT。
	/// </summary>
	public class Guide_Group
	{
        public Guide_Group()
		{}


		#region  成员方法


        /// <summary>
        /// 查询指标集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetGuideGroup(string where)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                SELECT GUIDE_GATHER_CODE ,GUIDE_GATHER_NAME ,GUIDE_ATTR,GUIDE_GROUP_TYPE_NAME AS GUIDE_ATTR_NAME,ORGAN_CLASS,ORGAN_CLASS_NAME,EVALUATION_YEAR
                    ,(SELECT COUNT(*) FROM {0}.GUIDE_GATHERS  BB WHERE BB.GUIDE_GATHER_CODE = {0}.GUIDE_GATHER_CLASS.GUIDE_GATHER_CODE )CNT
                  FROM {0}.GUIDE_GATHER_CLASS 
                 LEFT JOIN {0}.JXGL_GUIDE_ORGAN_CLASS_DICT ON {0}.GUIDE_GATHER_CLASS.ORGAN_CLASS = {0}.JXGL_GUIDE_ORGAN_CLASS_DICT.ORGAN_CLASS_CODE
                 LEFT JOIN {0}.GUIDE_GROUP_TYPE ON  {0}.GUIDE_GROUP_TYPE.GUIDE_GROUP_TYPE= GUIDE_ATTR", DataUser.HOSPITALSYS);
            if (where != "")
            {
                str.Append(" WHERE "+where);
            }
            str.Append(" ORDER BY GUIDE_GATHER_CODE ");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public DataSet GetGuideGroupByid(string id)
        //{
        //    string str = string.Format("select * from HOSPITALSYS.GUIDE_GATHER_CLASS where GUIDE_GATHER_CODE='{0}'", id);
        //    return OracleOledbBase.ExecuteDataSet(str);
        //}

        public DataSet GetOrganClass()
        {
            string str = string.Format("SELECT * FROM {0}.JXGL_GUIDE_ORGAN_CLASS_DICT ORDER BY ORGAN_CLASS_CODE", DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str);
 
        }
      

       /// <summary>
       /// 指标集类别字典信息列表
       /// </summary>
       /// <returns></returns>
        public DataSet GetGuideGroupTypeDictList()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                SELECT B.P_TYPE,B.P_TYPENAME,A.GUIDE_GROUP_TYPE,A.GUIDE_GROUP_TYPE_NAME,DECODE(A.ALLUSE,1,'通用','') ALLUSE
                  FROM {0}.GUIDE_GROUP_TYPE A,
                       ( SELECT GUIDE_GROUP_TYPE P_TYPE,GUIDE_GROUP_TYPE_NAME P_TYPENAME 
                           FROM {0}.GUIDE_GROUP_TYPE WHERE LENGTH(GUIDE_GROUP_TYPE)=2 ) B
                 WHERE LENGTH(A.GUIDE_GROUP_TYPE)>2
                   AND SUBSTR(GUIDE_GROUP_TYPE,1,2) = B.P_TYPE
                 ORDER BY B.P_TYPE,A.SORTNO",DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 指标集类别字典删除
        /// </summary>
        /// <param name="evacode"></param>
        /// <returns></returns>
        public string GuideGroupTypeDel(string typecode)
        {
            string resultstr = "";
            string sqlstr = string.Format("SELECT COUNT(*) CNT FROM {0}.GUIDE_GATHER_CLASS WHERE GUIDE_ATTR LIKE '{1}%'", DataUser.HOSPITALSYS, typecode.Replace("'", "''"));
            if (OracleOledbBase.ExecuteScalar(sqlstr).ToString().Equals("0"))
            {
                sqlstr = string.Format(" DELETE FROM {0}.GUIDE_GROUP_TYPE WHERE GUIDE_GROUP_TYPE='{1}'", DataUser.HOSPITALSYS, typecode.Replace("'", "''"));
                OracleOledbBase.ExecuteNonQuery(sqlstr);
            }
            else
            {
                resultstr = "该类别下已经定义了指标集，请删除指标集信息后再该除该类别！";
            }
            return resultstr;
        }
        /// <summary>
        /// 指标集类别更新
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="useall"></param>
        /// <returns></returns>
        public string GuideGroupTypeUpdate(string code, string name, string type, string useall)
        {
            string sqlstr = "";
            if (code.Equals("(新增项)"))
            {
                sqlstr = string.Format(@"
                    INSERT INTO {0}.GUIDE_GROUP_TYPE(GUIDE_GROUP_TYPE,GUIDE_GROUP_TYPE_NAME,SORTNO,ALLUSE)
                    SELECT '{1}'|| DECODE (MAX(GUIDE_GROUP_TYPE), NULL,'01', LPAD(TO_NUMBER(SUBSTR(MAX(GUIDE_GROUP_TYPE),3))+1 ,2,'0')) AS GUIDE_GROUP_TYPE,
                           '{2}' AS GUIDE_GROUP_TYPE_NAME,0 AS SORTNO ,'{3}' AS ALLUSE
                      FROM hospitalsys.GUIDE_GROUP_TYPE WHERE GUIDE_GROUP_TYPE LIKE   '{1}%' AND LENGTH(GUIDE_GROUP_TYPE) > 2",
                    DataUser.HOSPITALSYS,type,name,useall);
                OracleOledbBase.ExecuteNonQuery(sqlstr);
            }
            else
            {
                //指标集类型分类如果没有改变,直接更新.否则获取新的分类下的编号,顺带更新GUIDE_GATHER_CLASS表的GUIDE_ATTR字段
                if (code.Substring(0,2).Equals(type))
                {
                    sqlstr = string.Format(@" UPDATE {0}.GUIDE_GROUP_TYPE SET GUIDE_GROUP_TYPE_NAME='{1}',ALLUSE='{3}' WHERE GUIDE_GROUP_TYPE='{2}' ", DataUser.HOSPITALSYS, name, code, useall);
                    OracleOledbBase.ExecuteNonQuery(sqlstr);
                }
                else
                {
                    sqlstr = string.Format(@"
                    SELECT '{1}'|| DECODE (MAX(GUIDE_GROUP_TYPE), NULL,'01', LPAD(TO_NUMBER(SUBSTR(MAX(GUIDE_GROUP_TYPE),3))+1 ,2,'0')) AS GUIDE_GROUP_TYPE
                      FROM hospitalsys.GUIDE_GROUP_TYPE WHERE GUIDE_GROUP_TYPE LIKE   '{1}%' AND LENGTH(GUIDE_GROUP_TYPE) > 2", DataUser.HOSPITALSYS, type);
                    string newcode = OracleOledbBase.ExecuteScalar(sqlstr).ToString();
                    ArrayList SQLStringList = new ArrayList();
                    sqlstr = string.Format(@" UPDATE {0}.GUIDE_GROUP_TYPE SET GUIDE_GROUP_TYPE_NAME='{1}',ALLUSE='{3}',GUIDE_GROUP_TYPE='{4}' WHERE GUIDE_GROUP_TYPE='{2}' ", DataUser.HOSPITALSYS, name, code, useall, newcode);
                    SQLStringList.Add(sqlstr);
                    sqlstr = string.Format(@" UPDATE {0}.GUIDE_GATHER_CLASS SET GUIDE_ATTR='{1}' WHERE GUIDE_ATTR='{2}' ", DataUser.HOSPITALSYS, newcode, code);
                    SQLStringList.Add(sqlstr);
                    OracleBase.ExecuteSqlTran(SQLStringList);
                }

            }
            return "";
        }
        /// <summary>
        /// 获取指标集类别大分类
        /// </summary>
        /// <returns></returns>
        public DataSet GetGuideGroupTypeClass()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                    SELECT GUIDE_GROUP_TYPE P_TYPE,GUIDE_GROUP_TYPE_NAME P_TYPENAME 
                      FROM {0}.GUIDE_GROUP_TYPE WHERE LENGTH(GUIDE_GROUP_TYPE)=2 
                     ORDER BY SORTNO,GUIDE_GROUP_TYPE", DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 获取指标集类别小分类
        /// </summary>
        /// <param name="guidegrouptype"></param>
        /// <returns></returns>
        public DataSet GetGuideGroupType(string guidegrouptype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT GUIDE_GROUP_TYPE ,GUIDE_GROUP_TYPE_NAME FROM {0}.GUIDE_GROUP_TYPE WHERE LENGTH(GUIDE_GROUP_TYPE)> 2 ", DataUser.HOSPITALSYS);
            if (!guidegrouptype.Equals(""))
            {
                str.AppendFormat(" AND GUIDE_GROUP_TYPE LIKE '{0}%'", guidegrouptype);
            }
            str.Append(" ORDER BY SORTNO,GUIDE_GROUP_TYPE");            
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        /// <summary>
        /// 查询评价归档
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetEvaluateTypeList(string where)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select a.EVALUATE_CLASS_CODE ,a.EVALUATE_CLASS_NAME ,b.STATION_TYPE_CODE,b.STATION_TYPE_NAME  from {0}.EVALUATE_TYPE_DICT a, {1}.SYS_STATION_TYPE_DICT b where a.STATION_TYPE=b.STATION_TYPE_CODE ", DataUser.HOSPITALSYS, DataUser.COMM);
            if (where != "")
            {
                str.Append(where);
            }
            str.Append(" order by a.EVALUATE_CLASS_CODE");

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 评价归档类别的评价对象类别(院、科、人等)
        /// </summary>
        /// <returns></returns>
        public DataSet GetEvaluateTypeTarget()
        {
            string sqlstr = string.Format("SELECT STATION_TYPE_CODE ,STATION_TYPE_NAME  FROM {0}.SYS_STATION_TYPE_DICT", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(sqlstr);
        }
        /// <summary>
        /// 评价归档类别字典删除
        /// </summary>
        /// <param name="evacode"></param>
        /// <returns></returns>
        public string EvaluateTypeDel(string evacode)
        {
            string resultstr = "";
            string sqlstr = string.Format("SELECT COUNT(*) CNT FROM {0}.EVALUATE_INFORMATION WHERE EVALUATE_CLASS_CODE='{1}'", DataUser.HOSPITALSYS, evacode.Replace("'", "''"));
            if  (OracleOledbBase.ExecuteScalar(sqlstr).ToString().Equals("0"))
            {
                sqlstr = string.Format(" DELETE FROM {0}.EVALUATE_TYPE_DICT WHERE EVALUATE_CLASS_CODE='{1}'", DataUser.HOSPITALSYS, evacode.Replace("'", "''"));
                OracleOledbBase.ExecuteDataSet(sqlstr);
            }
            else
            {
                resultstr = "该类别已经在评价中使用，请先删除评价信息后再该除该类别！";
            }
            return resultstr;
        }
        /// <summary>
        /// 评价归档类别字典更新
        /// </summary>
        /// <param name="dataRows"></param>
        /// <returns></returns>
        public string EvaluateTypeUpdate(Dictionary<string, string>[] dataRows)
        {
            string resultstr = "";
            ArrayList SQLStringList = new ArrayList();
            string sqlstr = string.Format(@" DELETE FROM {0}.EVALUATE_TYPE_DICT ", DataUser.HOSPITALSYS);
            SQLStringList.Add(sqlstr);

            if (dataRows.Length > 0)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@" INSERT INTO {0}.EVALUATE_TYPE_DICT (EVALUATE_CLASS_CODE,EVALUATE_CLASS_NAME,STATION_TYPE ) ", DataUser.HOSPITALSYS);
                string classcode = "";
                string classname = "";
                string typecode = "";

                for (int i = 0; i < dataRows.Length; i++)
                {
                    classcode = dataRows[i]["EVALUATE_CLASS_CODE"].Trim();
                    classname = dataRows[i]["EVALUATE_CLASS_NAME"].Trim();
                    typecode = dataRows[i]["STATION_TYPE_CODE"].Trim();
                    if (classcode.Equals("") | classname.Equals("") | typecode.Equals(""))
                    {
                        resultstr = "请输入完整的评价归档类别信息后再保存！";
                        return resultstr;
                    }
                    else
                    {
                        sql.AppendFormat(@" SELECT  '{0}' AS EVALUATE_CLASS_CODE,'{1}' AS EVALUATE_CLASS_NAME, {2} AS STATION_TYPE FROM DUAL UNION ALL ", classcode, classname, typecode);
                    }
                }
                sql.AppendFormat("END");
                SQLStringList.Add(sql.ToString().Replace("UNION ALL END", ""));
            }
            OracleBase.ExecuteSqlTran(SQLStringList);
            return resultstr;
        }
            



        /// <summary>
        /// 删除指标集
        /// </summary>
        /// <param name="id"></param>
        public string DelGuideGroupByid(string id)
        {
            string strupdate1 = string.Format(" UPDATE {1}.GUIDE_NAME_DICT SET GUIDE_GATHER_CODE = NULL WHERE GUIDE_GATHER_CODE='{0}'", id, DataUser.HOSPITALSYS);
            string strupdate2 = string.Format(@" DELETE FROM {1}.STATION_GUIDE_INFORMATION 
                                                  WHERE STATION_CODE IN (
                                                       SELECT DISTINCT STATION_CODE  FROM {2}.SYS_STATION_BASIC_INFORMATION WHERE GUIDE_GATHER_CODE = '{0}')
                                                    AND GUIDE_CODE IN (SELECT GUIDE_CODE FROM {1}.GUIDE_GATHERS WHERE GUIDE_GATHER_CODE='{0}') ",
                                                id, DataUser.HOSPITALSYS,DataUser.COMM);
            string strupdate3 = string.Format(" UPDATE {1}.SYS_STATION_MAINTENANCE_DICT SET GUIDE_GATHER_CODE = NULL WHERE GUIDE_GATHER_CODE = '{0}'", id, DataUser.COMM);
            string strupdate4 = string.Format(" UPDATE {1}.SYS_STATION_BASIC_INFORMATION SET GUIDE_GATHER_CODE = NULL WHERE GUIDE_GATHER_CODE = '{0}'", id, DataUser.COMM);
            string strdel = string.Format(" DELETE FROM {1}.GUIDE_GATHER_CLASS WHERE GUIDE_GATHER_CODE='{0}'", id,DataUser.HOSPITALSYS);
            string strdelguide = string.Format(" DELETE FROM {1}.GUIDE_GATHERS WHERE GUIDE_GATHER_CODE='{0}'", id, DataUser.HOSPITALSYS);
            OracleOledbBase.ExecuteNonQuery(strupdate1);
            OracleOledbBase.ExecuteNonQuery(strupdate2);
            OracleOledbBase.ExecuteNonQuery(strupdate3);
            OracleOledbBase.ExecuteNonQuery(strupdate4);
            OracleOledbBase.ExecuteNonQuery(strdel);
            OracleOledbBase.ExecuteNonQuery(strdelguide);
            return "";
        }
        /// <summary>
        /// 更新指标集
        /// </summary>
        /// <param name="selectedid"></param>
        /// <param name="guidegroupname"></param>
        /// <param name="guidegroupclass"></param>
        /// <param name="organclass"></param>
        /// <param name="evaluationyear"></param>
        /// <returns></returns>
        public string UpdateGuideGroup(string selectedid, string guidegroupname, string guidegroupclass, string organclass, string evaluationyear)
        {
            string sqlstr = "";
            if (selectedid.Equals(""))
            {
                sqlstr = string.Format(@"
                    INSERT INTO {0}.GUIDE_GATHER_CLASS(GUIDE_GATHER_CODE,GUIDE_GATHER_NAME,GUIDE_ATTR,ORGAN_CLASS,EVALUATION_YEAR)
                    SELECT (SELECT NVL(MAX(GUIDE_GATHER_CODE),0) + 1 FROM {0}.GUIDE_GATHER_CLASS ),'{1}','{2}','{3}','{4}' FROM DUAL"
               , DataUser.HOSPITALSYS,  guidegroupname, guidegroupclass, organclass, evaluationyear);
            }
            else
            {
                sqlstr = string.Format(@" UPDATE {0}.GUIDE_GATHER_CLASS SET GUIDE_GATHER_NAME = '{2}' , GUIDE_ATTR = '{3}', 
                                            ORGAN_CLASS ='{4}',EVALUATION_YEAR = '{5}' WHERE GUIDE_GATHER_CODE = '{1}'"
                    ,DataUser.HOSPITALSYS,selectedid,guidegroupname,guidegroupclass,organclass,evaluationyear);
            }
            OracleOledbBase.ExecuteNonQuery(sqlstr);
            return "";
        }
        /// <summary>
        /// 复制指标集
        /// </summary>
        /// <param name="copyid">源指标集编码</param>
        /// <param name="newname">新指标集名称</param>
        public string  CopyGuideGroup(string copyid, string newname)
        {
            string newid = OracleOledbBase.GetSingle("SELECT NVL(MAX(GUIDE_GATHER_CODE),0) + 1 FROM " + DataUser.HOSPITALSYS + ".GUIDE_GATHER_CLASS").ToString();
            MyLists listtable = new MyLists();
            
            string sqlstr = string.Format(@"
                INSERT INTO {0}.GUIDE_GATHERS (GUIDE_GATHER_CODE,GUIDE_CODE,QZ)
                SELECT {1},GUIDE_CODE,QZ FROM {0}.GUIDE_GATHERS 
                 WHERE GUIDE_GATHER_CODE='{2}'", DataUser.HOSPITALSYS,newid,copyid);
            List listi = new List();
            listi.StrSql = sqlstr;
            listi.Parameters = new OleDbParameter[] { };
            listtable.Add(listi);
            string sqlstri = string.Format(@"
                    INSERT INTO {0}.GUIDE_GATHER_CLASS(GUIDE_GATHER_CODE,GUIDE_GATHER_NAME,GUIDE_ATTR,ORGAN_CLASS,EVALUATION_YEAR)
                    SELECT '{1}','{2}',GUIDE_ATTR,ORGAN_CLASS,EVALUATION_YEAR FROM {0}.GUIDE_GATHER_CLASS WHERE GUIDE_GATHER_CODE='{3}'"
                , DataUser.HOSPITALSYS, newid, newname, copyid);

            List listadd = new List();
            listadd.StrSql = sqlstri.ToString();
            listadd.Parameters = new OleDbParameter[] { };
            listtable.Add(listadd);
            OracleOledbBase.ExecuteTranslist(listtable);
            return "";
        }

        /// <summary>
        /// 非岗位指标集
        /// </summary>
        /// <param name="guidegrouptype"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public DataSet GetGuideGroupNotStation(string guidegrouptype, int length)
        {
            string sql = string.Format(@"SELECT   a.ID, a.guide_group_type, a.guide_group_type_name
    FROM {2}.guide_group_type a
   WHERE a.guide_group_type not LIKE '{0}%' AND LENGTH (a.guide_group_type) = {1}
ORDER BY a.ID", guidegrouptype, length,DataUser.HOSPITALSYS);

            return OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter());
        }
        /// <summary>
        /// 获取不具备该指标集权限的角色
        /// </summary>
        /// <param name="guidegathercode">指标集代码</param>
        /// <returns>DataSet</returns>
        public DataSet GetNoSelectRole(string guidegathercode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT A.ROLE_ID, A.ROLE_NAME
                                    FROM {0}.SYS_ROLE_DICT A
                                   WHERE NOT EXISTS (SELECT *
                                                       FROM {1}.GUIDE_GATHER_ROLE B
                                                      WHERE A.ROLE_ID = B.ROLE_ID
                                                        AND B.GUIDE_GATHER_CODE={2})
                                ORDER BY A.ROLE_ID", DataUser.COMM, DataUser.HOSPITALSYS, guidegathercode);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取具备该指标集权限的角色
        /// </summary>
        /// <param name="guidegathercode">指标集代码</param>
        /// <returns>DataSet</returns>
        public DataSet GetSelectRole(string guidegathercode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT A.ROLE_ID, A.ROLE_NAME
                                    FROM {0}.SYS_ROLE_DICT A
                                   WHERE EXISTS (SELECT *
                                                       FROM {1}.GUIDE_GATHER_ROLE B
                                                      WHERE A.ROLE_ID = B.ROLE_ID
                                                        AND B.GUIDE_GATHER_CODE={2})
                                ORDER BY A.ROLE_ID", DataUser.COMM, DataUser.HOSPITALSYS, guidegathercode);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 保存设置权限
        /// </summary>
        /// <param name="rolelist"></param>
        /// <param name="guidegathercode"></param>
        public void SaveGatherRole(List<GoldNet.Model.PageModels.roleselected> rolelist, string guidegathercode)
        {
            MyLists listtable = new MyLists();
            string strdel = string.Format("DELETE {0}.GUIDE_GATHER_ROLE where GUIDE_GATHER_CODE=? ", DataUser.HOSPITALSYS);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", guidegathercode) };
            listtable.Add(listdel);
            string strMaxId = "SELECT NVL(MAX(ID),0)+1 ID FROM " + DataUser.HOSPITALSYS + ".GUIDE_GATHER_ROLE";
            string id = OracleOledbBase.ExecuteDataSet(strMaxId).Tables[0].Rows[0]["id"].ToString();
            foreach (GoldNet.Model.PageModels.roleselected role in rolelist)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("INSERT INTO {0}.GUIDE_GATHER_ROLE(", DataUser.HOSPITALSYS);
                strSql.Append("ID,GUIDE_GATHER_CODE,ROLE_ID)");
                strSql.Append(" VALUES (");
                strSql.Append("?,?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("id",id),
											  new OleDbParameter("guide_gather_code",guidegathercode),
	                                          new OleDbParameter("role_id",role.ROLE_ID)
										  };
                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

		#endregion  成员方法
	}
}

