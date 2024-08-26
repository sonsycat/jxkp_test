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
    public class BonusGuideDict
    { 
        /// <summary>
        /// 指标列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetGuideDictList(string guidename, string ispage, string bscone, string bsctwo)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"
                SELECT A.GUIDE_CODE, B.BSC_CLASS_NAME  BSC_CLASS_NAME1, E.BSC_CLASS_NAME  BSC_CLASS_NAME2, A.GUIDE_NAME, A.GUIDE_EXPRESS,
                       CASE WHEN A.ISEXPRESS = '0' THEN '否' ELSE '是' END  ISEXPRESS,
                       CASE WHEN A.ISPAGE = '0' THEN '停用' WHEN A.ISPAGE = '1' THEN '启用' ELSE '不显示' END  ISPAGE,
                       CASE WHEN A.ISHIGHGUIDE = '0' THEN '否' ELSE '是' END   ISHIGHGUIDE, 
                       CASE WHEN A.ISSEL = '0' THEN '否' ELSE '是' END   ISSEL, 
                       CASE WHEN A.ISABS = '0' THEN '否' ELSE '是' END  ISABS, 
                       CASE WHEN (A.ISEXPRESS = '1' AND  A.GUIDE_EXPRESS IS NULL)  THEN '无'
                            WHEN (A.ISEXPRESS = '1' AND  LENGTH(A.GUIDE_EXPRESS)>0 )  THEN '有'
                            WHEN A.GUIDE_SQL = '0' THEN '无' ELSE '有' END  GUIDE_SQL,
                       DECODE(A.DEPT,'00','公共部分', C.DEPT_CLASS_NAME) DEPT_CLASS_NAME, F.ORGAN_CLASS_NAME ,A.BSC,A.DEPT,A.ORGAN,A.SERIAL_NO,A.GUIDE_GATHER_CODE,A.ISSAME,A.SHOWNUM,A.EXPLAIN,
                       CASE WHEN A.INDEXTYPE = '0' THEN '通用' WHEN A.INDEXTYPE = '1' THEN '临床' WHEN A.INDEXTYPE = '2' THEN '医技' END  INDEXTYPE
                  FROM HOSPITALSYS.GUIDE_NAME_DICT A,
                       HOSPITALSYS.JXGL_GUIDE_BSC_CLASS_DICT B,
                       HOSPITALSYS.JXGL_GUIDE_BSC_CLASS_DICT E,
                       HOSPITALSYS.JXGL_GUIDE_DEPT_CLASS_DICT C,
                       HOSPITALSYS.JXGL_GUIDE_ORGAN_CLASS_DICT F
                 WHERE SUBSTR (A.BSC, 0, 2) = B.BSC_CLASS_CODE
                   AND A.BSC = E.BSC_CLASS_CODE
                   AND A.DEPT = C.DEPT_CLASS_CODE(+)
                   AND A.ORGAN = F.ORGAN_CLASS_CODE ");
            if (ispage != "")
            {
                str.Append(" and ispage='" + ispage + "' ");
            }
            if (guidename != "")
            {
                str.Append(" and (guide_name like '%" + guidename + "%' or guide_code like '%" + guidename + "%')");
            }
            if (bscone != "")
            {
                str.AppendFormat(" and substr(b.bsc_class_code,0,2) = '{0}'", bscone);
            }
            if (bsctwo != "")
            {
                str.AppendFormat(" and e.bsc_class_code = '{0}'", bsctwo);
            }
            str.Append("and a.GUIDE_TYPE='奖金' order by a.GUIDE_CODE");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 根据BSC树形分类和组织、科室 查询指标列表
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="org"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        public DataSet GetGuideDictListByBscOrgDept()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                SELECT A.GUIDE_CODE,  A.GUIDE_NAME, A.GUIDE_NAME  GUIDE_NAME_QZ
                  FROM {0}.GUIDE_NAME_DICT A
                 WHERE GUIDE_TYPE='奖金' and a.guide_code not in
    (select guide_code from performance.SET_BONUSGUIDE_GATHER) ", DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeid"></param>
        /// <returns></returns>
        public DataSet GetGuideDictListByBscOrgDept(string codeid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                SELECT A.GUIDE_CODE,  A.GUIDE_NAME, A.GUIDE_NAME  GUIDE_NAME_QZ,A.SHOWNUM  SHOW_WIDTH
                  FROM {0}.GUIDE_NAME_DICT A
                 WHERE GUIDE_TYPE='奖金' and ISSAME='{1}'  and a.guide_code not in
                 (select guide_code from performance.SET_BONUSGUIDE_GATHER where bonus_type ='{1}') ORDER BY A.GUIDE_NAME", DataUser.HOSPITALSYS, codeid);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 更新指标集包含的指标项目
        /// </summary>
        /// <param name="guidegatherid"></param>
        /// <param name="selectedRow"></param>
        /// <returns></returns>
        public string UpdateGuideGatherDetail(Dictionary<string, string>[] selectedRow, string codeid)
        {
            string resultstr = "";
            MyLists listtable = new MyLists();
            string sqlstr = string.Format(" DELETE FROM {0}.SET_BONUSGUIDE_GATHER where BONUS_TYPE='{1}'", DataUser.PERFORMANCE, codeid);
            List listdel = new List();
            listdel.StrSql = sqlstr;
            listdel.Parameters = new OleDbParameter[] { };
            listtable.Add(listdel);
            if (selectedRow.Length > 0)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@" INSERT INTO {0}.SET_BONUSGUIDE_GATHER (GUIDE_CODE,GUIDE_NAME,BONUS_TYPE,SORT,SHOW_WIDTH ) ", DataUser.PERFORMANCE);
                string guide_code = "";
                // string qz = "";
                string guide_name = "";
                string shownum = "";
                for (int i = 0; i < selectedRow.Length; i++)
                {
                    guide_code = selectedRow[i]["GUIDE_CODE"].Trim();
                    guide_name = selectedRow[i]["GUIDE_NAME"].Trim();
                    shownum = Convert.IsDBNull(selectedRow[i]["SHOW_WIDTH"]) ? selectedRow[i]["SHOW_WIDTH"].Trim() : "100";
                    sql.AppendFormat(@" SELECT  '{0}' AS GUIDE_CODE,'{1}' AS GUIDE_NAME, '{3}' AS BONUS_TYPE,{2} as SORT,{4} AS SHOW_WIDTH from DUAL UNION ALL ", guide_code, guide_name, i + 1, codeid, shownum);
                }
                sql.AppendFormat("END");
                List listadd = new List();
                listadd.StrSql = sql.ToString().Replace("UNION ALL END", "");
                OleDbParameter[] parameteradd = new OleDbParameter[] { };
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
            return resultstr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedRow"></param>
        /// <param name="codeid"></param>
        /// <returns></returns>
        public string UpdateGuideGatherDetail1(Dictionary<string, string>[] selectedRow, string codeid)
        {
            string resultstr = "";
            ArrayList SQLStringList = new ArrayList();
            string sqlstr = string.Format(" DELETE FROM {0}.SET_BONUSGUIDE_GATHER where BONUS_TYPE='{1}'", DataUser.PERFORMANCE, codeid);
            SQLStringList.Add(sqlstr);
            // OracleOledbBase.ExecuteNonQuery(sqlstr);
            if (selectedRow.Length > 0)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@" INSERT INTO {0}.SET_BONUSGUIDE_GATHER (GUIDE_CODE,GUIDE_NAME,BONUS_TYPE,SORT,SHOW_WIDTH ) ", DataUser.PERFORMANCE);
                string guide_code = "";
                // string qz = "";
                string guide_name = "";
                string shownum = "";
                for (int i = 0; i < selectedRow.Length; i++)
                {
                    guide_code = selectedRow[i]["GUIDE_CODE"].Trim();
                    guide_name = selectedRow[i]["GUIDE_NAME"].Trim();
                    shownum = selectedRow[i]["SHOW_WIDTH"].Trim();
                    sql.AppendFormat(@" SELECT  '{0}' AS GUIDE_CODE,'{1}' AS GUIDE_NAME, '{3}' AS BONUS_TYPE,{2} as SORT,{4} AS SHOW_WIDTH from DUAL UNION ALL ", guide_code, guide_name, i + 1, codeid, shownum);
                }
                sql.AppendFormat("END");
                SQLStringList.Add(sql.ToString().Replace("UNION ALL END", ""));
            }
            OracleBase.ExecuteSqlTran(SQLStringList);
            return resultstr;
        }

        /// <summary>
        /// 检查4项关键指标是否都设置进来
        /// </summary>
        /// <returns></returns>
        public string CheckGuidePrimary(Dictionary<string, string>[] selectedRow)
        {
            string sql = " select guide_code,GUIDE_NAME from performance.SET_BONUSGUIDE_BASE";
            DataTable dt = OracleOledbBase.ExecuteDataSet(sql).Tables[0];
            Hashtable hs = new Hashtable();
            for (int i = 0; i < selectedRow.Length; i++)
            {
                hs.Add(selectedRow[i]["GUIDE_CODE"].Trim(), selectedRow[i]["GUIDE_NAME"].Trim());
            }
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (!hs.Contains(dt.Rows[j]["GUIDE_CODE"].ToString()))
                {
                    return dt.Rows[j]["GUIDE_NAME"].ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guide_code"></param>
        /// <returns></returns>
        public string CheckGuidePrimary(string guide_code)
        {
            string sql = " select guide_code,GUIDE_NAME from performance.SET_BONUSGUIDE_BASE where guide_code='" + guide_code + "'";
            DataTable dt = OracleOledbBase.ExecuteDataSet(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return guide_code + "为奖金基础指标不可以删除";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 检查4项关键指标是否都设置进来
        /// </summary>
        /// <returns></returns>
        public string CheckGuidePrimary(Dictionary<string, string>[] selectedRow, string Typeid)
        {
            string code = "";
            string sql = " select guide_code,GUIDE_NAME from performance.SET_BONUSGUIDE_BASE where BONUS_TYPE=" + Typeid + "";
            DataTable dt = OracleOledbBase.ExecuteDataSet(sql).Tables[0];
            Hashtable hs = new Hashtable();
            for (int i = 0; i < selectedRow.Length; i++)
            {
                hs.Add(selectedRow[i]["GUIDE_CODE"].Trim(), selectedRow[i]["GUIDE_NAME"].Trim());
            }
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (!hs.Contains(dt.Rows[j]["GUIDE_CODE"].ToString()))
                {
                    code = dt.Rows[j]["GUIDE_NAME"].ToString();
                }
            }
            return code;
        }

        /// <summary>
        /// 指标集详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetGuideGroupList()
        {
            string str = string.Format(@"select  GUIDE_CODE, GUIDE_NAME,GUIDE_NAME  GUIDE_NAME_QZ from {0}.SET_BONUSGUIDE_GATHER order by SORT", DataUser.PERFORMANCE);
            return OracleOledbBase.ExecuteDataSet(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetGuideGroupList(string id)
        {
            string str = string.Format(@"select  GUIDE_CODE, GUIDE_NAME,GUIDE_NAME  GUIDE_NAME_QZ,SHOW_WIDTH from {0}.SET_BONUSGUIDE_GATHER where bonus_type='{1}' order by sort", DataUser.PERFORMANCE, id);
            return OracleOledbBase.ExecuteDataSet(str);
        }

        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptType()
        {
            return OracleOledbBase.ExecuteDataSet(string.Format("SELECT '00' AS DEPT_CLASS_CODE, '公共部分' AS DEPT_CLASS_NAME FROM DUAL  UNION SELECT DEPT_CLASS_CODE,DEPT_CLASS_NAME FROM {0}.JXGL_GUIDE_DEPT_CLASS_DICT", DataUser.HOSPITALSYS));
        }

        /// <summary>
        /// 组织
        /// </summary>
        /// <returns></returns>
        public DataSet Getorg()
        {
            return OracleOledbBase.ExecuteDataSet(string.Format("SELECT * FROM {0}.JXGL_GUIDE_ORGAN_CLASS_DICT", DataUser.HOSPITALSYS));
        }

        /// <summary>
        /// 删除指标
        /// </summary>
        /// <param name="guidecode"></param>
        /// <returns></returns>
        public string DelGuide(string guidecode)
        {
            ArrayList SQLStringList = new ArrayList();
            string sqlstr0 = string.Format(" DELETE FROM  {0}.GUIDE_NAME_DICT WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr0);
            string sqlstr1 = string.Format(" DELETE FROM  {0}.GUIDE_EXPRESSIONS WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr1);
            string sqlstr2 = string.Format(" DELETE FROM  {0}.GUIDE_GATHERS WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr2);
            string sqlstr3 = string.Format(" DELETE FROM  {0}.GUIDE_MONTH_CAUSE WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr3);
            string sqlstr4 = string.Format(" DELETE FROM  {0}.GUIDE_VALUE WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr4);
            string sqlstr5 = string.Format(" DELETE FROM  {0}.GUIDE_VALUE_SUM WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr5);
            string sqlstr6 = string.Format(" DELETE FROM  {0}.REPORTINFO WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr6);
            string sqlstr7 = string.Format(" DELETE FROM  {0}.STATION_GUIDE_INFORMATION WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr7);
            string sqlstr8 = string.Format(" UPDATE  {0}.GUIDE_NAME_DICT SET GUIDE_EXPRESS =''  WHERE GUIDE_EXPRESS LIKE '%{1}%' ", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr8);
            string sqlstr9 = string.Format(@" UPDATE {0}.GUIDE_HOSPITAL_DEPT_MEMBER SET 
                    HOSPITAL_GUIDE_CODE =  DECODE(HOSPITAL_GUIDE_CODE,'{1}',NULL,HOSPITAL_GUIDE_CODE),
                    HOSPITAL_GUIDE_NAME =  DECODE(HOSPITAL_GUIDE_CODE,'{1}',NULL,HOSPITAL_GUIDE_NAME),
                    DEPT_GUIDE_CODE =    DECODE(DEPT_GUIDE_CODE,'{1}',NULL,DEPT_GUIDE_CODE),
                    DEPT_GUIDE_NAME =    DECODE(DEPT_GUIDE_CODE,'{1}',NULL,DEPT_GUIDE_NAME),
                    MEMBER_GUIDE_CODE =  DECODE(MEMBER_GUIDE_CODE,'{1}',NULL,MEMBER_GUIDE_CODE),
                    MEMBER_GUIDE_NAME =  DECODE(DEPT_GUIDE_CODE,'{1}',NULL,MEMBER_GUIDE_NAME)
                WHERE HOSPITAL_GUIDE_CODE||','||DEPT_GUIDE_CODE||','||MEMBER_GUIDE_CODE LIKE '%{1}%'", DataUser.HOSPITALSYS, guidecode);
            SQLStringList.Add(sqlstr9);

            OracleBase.ExecuteSqlTran(SQLStringList);
            return "";
        }

        /// <summary>
        /// BSCone
        /// </summary>
        /// <returns></returns>
        public DataSet GetBSCOne()
        {
            string sqlstr = string.Format("SELECT BSC_CLASS_CODE,BSC_CLASS_NAME FROM {0}.JXGL_GUIDE_BSC_CLASS_DICT WHERE LENGTH(BSC_CLASS_CODE)=2", DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(sqlstr);
        }

        /// <summary>
        /// BSCtwo
        /// </summary>
        /// <returns></returns>
        public DataSet GetBSTwo(string code)
        {
            string sqlstr = string.Format("SELECT BSC_CLASS_CODE,BSC_CLASS_NAME FROM {0}.JXGL_GUIDE_BSC_CLASS_DICT WHERE SUBSTR(BSC_CLASS_CODE,1,2)='{1}'  AND LENGTH(BSC_CLASS_CODE)>2", DataUser.HOSPITALSYS, code);
            return OracleOledbBase.ExecuteDataSet(sqlstr);
        }

        /// <summary>
        /// 绑定通用指标集
        /// </summary>
        /// <returns></returns>
        public DataSet GetGuideGather(string guide_type, string organ, string evalyear)
        {
            string sql = string.Format(@"SELECT GUIDE_GATHER_CODE,GUIDE_GATHER_NAME 
                                          FROM {0}.GUIDE_GATHER_CLASS , {0}.GUIDE_GROUP_TYPE
                                         WHERE GUIDE_ATTR = GUIDE_GROUP_TYPE(+)
                                           AND (GUIDE_ATTR LIKE '{1}%'  OR  ALLUSE = '1')
                                           AND  ORGAN_CLASS = '{2}'
                                           AND (EVALUATION_YEAR ='{3}' OR EVALUATION_YEAR ='all') ORDER BY GUIDE_GATHER_CODE "
                                , DataUser.HOSPITALSYS, guide_type, organ, evalyear);
            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 绑定指标大类别
        /// </summary>
        /// <returns></returns>
        public DataSet GetGuidetype()
        {
            string sql = string.Format(@"SELECT GUIDE_GROUP_TYPE,GUIDE_GROUP_TYPE_NAME, SORTNO 
                                          FROM {0}.GUIDE_GROUP_TYPE  WHERE LENGTH(GUIDE_GROUP_TYPE) = 2 
                              UNION ALL SELECT '00','通用类指标集' ,999 FROM DUAL ORDER BY SORTNO,GUIDE_GROUP_TYPE", DataUser.HOSPITALSYS);

            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取指标集类别小分类(含通用)
        /// </summary>
        /// <param name="guidegrouptype"></param>
        /// <returns></returns>
        public DataSet GetGuideGroupType(string guidegrouptype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                    SELECT GUIDE_GROUP_TYPE ,GUIDE_GROUP_TYPE_NAME 
                      FROM {0}.GUIDE_GROUP_TYPE 
                     WHERE LENGTH(GUIDE_GROUP_TYPE)> 2 
                       AND (   (('{1}' = '00')  AND (ALLUSE = 1) )
                            OR (('{1}'<> '00')  AND (GUIDE_GROUP_TYPE LIKE '{1}%' ) ) 
                           ) ORDER BY SORTNO,GUIDE_GROUP_TYPE", DataUser.HOSPITALSYS, guidegrouptype);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取指标集类别信息，供前台页面初始显示各类别下拉框使用
        /// </summary>
        /// <param name="guidegathercode"></param>
        /// <returns></returns>
        public DataSet GetGuideGatherInfo(string guidegathercode)
        {
            string sqlstr = string.Format("SELECT GUIDE_GATHER_CODE,GUIDE_GATHER_NAME,GUIDE_ATTR,ORGAN_CLASS,EVALUATION_YEAR FROM {0}.GUIDE_GATHER_CLASS WHERE GUIDE_GATHER_CODE ='{1}'", DataUser.HOSPITALSYS, guidegathercode);
            return OracleOledbBase.ExecuteDataSet(sqlstr);
        }

        /// <summary>
        /// 下一个guidecode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetNextGuideCode(string OrganBSC2Code)
        {
            string str = "SELECT DECODE(MAX(GUIDE_CODE) ,NULL ," + OrganBSC2Code + "001" + ",MAX(GUIDE_CODE)+1 )  GUIDE_CODE  FROM " + DataUser.HOSPITALSYS + ".GUIDE_NAME_DICT A WHERE A.GUIDE_CODE LIKE '" + OrganBSC2Code + "%'";
            return OracleOledbBase.GetSingle(str).ToString();
        }

        /// <summary>
        /// 指标基本信息更新
        /// </summary>
        /// <param name="FormValues"></param>
        /// <returns></returns>
        public string UpdateGuideInfo(Dictionary<string, string> FormValues)
        {
            //FormValues传进来的值，其中CorrelationCHK是单选框，如果未选中时，则没有该项
            //{[BSC2Comb_Value, 0101]}
            //{[BSC2Comb, 门急诊工作量]}
            //{[BSC2Comb_SelIndex, 0]}
            //{[DeptTypeComb_Value, 00]}
            //{[DeptTypeComb, 公共部分]}
            //{[DeptTypeComb_SelIndex, 0]}
            //{[OrganComb_Value, 01]}
            //{[OrganComb, 院]}
            //{[OrganComb_SelIndex, 0]}
            //{[GuideCodeTxt, 10101010]}
            //{[GuideCodeOriginal, ]}
            //{[GuideNameTxt, sdfsdfsdf]}
            //{[IsExpressComb_Value, 0]}
            //{[IsExpressComb, 否]}
            //{[IsExpressComb_SelIndex, 1]}
            //{[GuideExpressTxt, ]}
            //{[IsPageComb_Value, 1]}
            //{[IsPageComb, 启用]}
            //{[IsPageComb_SelIndex, 0]}
            //{[IsHighComb_Value, 1]}
            //{[IsHighComb, 是]}
            //{[IsHighComb_SelIndex, 0]}
            //{[IsZhpjComb_Value, 1]}
            //{[IsZhpjComb, 是]}
            //{[IsZhpjComb_SelIndex, 0]}
            //{[IsABSComb_Value, 1]}
            //{[IsABSComb, 绝对值指标]}
            //{[IsABSComb_SelIndex, 0]}
            //{[CorrelationCHK, CorrelationCHK]}
            //{[GuideGatherCombo_Value, ]}
            //{[GuideGatherCombo, 请选择指标集]}
            //{[GuideGatherCombo_SelIndex, ]}

            StringBuilder strSql = new StringBuilder();
            //增加操作
            if (FormValues["GuideCodeOriginal"].Equals(""))
            {
                string SERIAL_NO = OracleOledbBase.GetSingle("SELECT NVL(MAX(SERIAL_NO),0) + 1 FROM " + DataUser.HOSPITALSYS + ".GUIDE_NAME_DICT").ToString();
                strSql = new StringBuilder();
                strSql.AppendFormat(" INSERT INTO {0}.GUIDE_NAME_DICT(", DataUser.HOSPITALSYS);
                strSql.Append(" SERIAL_NO,GUIDE_CODE,GUIDE_TYPE,GUIDE_NAME,GUIDE_EXPRESS,ISEXPRESS,ISPAGE,ISHIGHGUIDE,ISSEL,ISABS,GUIDE_SQL,BSC,DEPT,ORGAN,GUIDE_GATHER_CODE,ISSAME,SHOWNUM,EXPLAIN,INDEXTYPE )");
                strSql.Append(" VALUES (");
                strSql.Append("?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)");
                OleDbParameter[] parameters = {
                    new OleDbParameter("SERIAL_NO",    SERIAL_NO),
					new OleDbParameter("GUIDE_CODE",   FormValues["GuideCodeTxt"]),
					new OleDbParameter("GUIDE_TYPE",   FormValues["BSC2Comb"]),
					new OleDbParameter("GUIDE_NAME",   FormValues["GuideNameTxt"].Trim()),
					new OleDbParameter("GUIDE_EXPRESS",FormValues["GuideExpressTxt"]),
					new OleDbParameter("ISEXPRESS",    FormValues["IsExpressComb_Value"]),
					new OleDbParameter("ISPAGE",       FormValues["IsPageComb_Value"]),
					new OleDbParameter("ISHIGHGUIDE",  FormValues["IsHighComb_Value"]),
					new OleDbParameter("ISSEL",        FormValues["IsZhpjComb_Value"]),
					new OleDbParameter("ISABS",        FormValues["IsABSComb_Value"]),
					new OleDbParameter("GUIDE_SQL",    "0"),
					new OleDbParameter("BSC",          FormValues["BSC2Comb_Value"]),
					new OleDbParameter("DEPT",         FormValues["DeptTypeComb_Value"]),
					new OleDbParameter("ORGAN",        FormValues["OrganComb_Value"]),
                    new OleDbParameter("GUIDE_GATHER_CODE",(FormValues.ContainsKey("CorrelationCHK")?FormValues["GuideGatherCombo_Value"]:"")),
                    new OleDbParameter("ISSAME", FormValues["cbbType_Value"]),
                      new OleDbParameter("SHOWNUM", FormValues["TextField2"]),
                      new OleDbParameter("EXPLAIN", FormValues["TextAreaexplain"]),
                       new OleDbParameter("INDEXTYPE", FormValues["zbType_Value"])
                };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
            }
            //更新操作
            else
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("UPDATE {0}.GUIDE_NAME_DICT SET ", DataUser.HOSPITALSYS);
                strSql.Append(" GUIDE_CODE=?,");
                strSql.Append(" GUIDE_TYPE=?,");
                strSql.Append(" GUIDE_NAME=?,");
                strSql.Append(" GUIDE_EXPRESS=?,");
                strSql.Append(" ISEXPRESS=?,");
                strSql.Append(" ISPAGE=?,");
                strSql.Append(" ISHIGHGUIDE=?,");
                strSql.Append(" ISSEL=?,");
                strSql.Append(" ISABS=?,");
                strSql.Append(" BSC=?,");
                strSql.Append(" DEPT=?,");
                strSql.Append(" ORGAN=?,");
                strSql.Append(" GUIDE_GATHER_CODE = ?,");
                strSql.Append(" ISSAME = ?,");
                strSql.Append(" SHOWNUM = ?,");
                strSql.Append(" EXPLAIN = ?,");
                strSql.Append(" INDEXTYPE = ? ");
                strSql.AppendFormat(" WHERE GUIDE_CODE='{0}' ", FormValues["GuideCodeOriginal"]);
                OleDbParameter[] parameters = {
					new OleDbParameter("GUIDE_CODE",    FormValues["GuideCodeTxt"]),
					new OleDbParameter("GUIDE_TYPE",    FormValues["BSC2Comb"]),
					new OleDbParameter("GUIDE_NAME",    FormValues["GuideNameTxt"].Trim()),
					new OleDbParameter("GUIDE_EXPRESS", FormValues["GuideExpressTxt"]),
					new OleDbParameter("ISEXPRESS",     FormValues["IsExpressComb_Value"]),
					new OleDbParameter("ISPAGE",        FormValues["IsPageComb_Value"]),
					new OleDbParameter("ISHIGHGUIDE",   FormValues["IsHighComb_Value"]),
					new OleDbParameter("ISSEL",         FormValues["IsZhpjComb_Value"]),
					new OleDbParameter("ISABS",         FormValues["IsABSComb_Value"]),
					new OleDbParameter("BSC",           FormValues["BSC2Comb_Value"]),
					new OleDbParameter("DEPT",          FormValues["DeptTypeComb_Value"]),
					new OleDbParameter("ORGAN",         FormValues["OrganComb_Value"]),
                    new OleDbParameter("GUIDE_GATHER_CODE",(FormValues.ContainsKey("CorrelationCHK")?FormValues["GuideGatherCombo_Value"]:"")),
                    new OleDbParameter("ISSAME", FormValues["cbbType_Value"]),
                      new OleDbParameter("SHOWNUM", FormValues["TextField2"]),
                      new OleDbParameter("EXPLAIN", FormValues["TextAreaexplain"]),
                      new OleDbParameter("INDEXTYPE", FormValues["zbType_Value"])
                                              };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);

                string guidename = FormValues["GuideNameTxt"].Trim().Replace("'", "");
                string guidecode = FormValues["GuideCodeOriginal"];
                string guidecode_new = FormValues["GuideCodeTxt"];
                //指标代码变更后，更新关联的指标
                if (!guidecode.Equals(guidecode_new))
                {
                    ArrayList SQLStringList = new ArrayList();
                    string sqlstr1 = string.Format(" UPDATE {0}.GUIDE_EXPRESSIONS SET GUIDE_CODE = '{2}', GUIDE_SQL = REPLACE(GUIDE_SQL,'{1}','{2}') , GUIDE_SQL_SUM = REPLACE(GUIDE_SQL_SUM,'{1}','{2}') WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new);
                    SQLStringList.Add(sqlstr1);
                    string sqlstr2 = string.Format(" UPDATE {0}.GUIDE_GATHERS SET GUIDE_CODE = '{2}' WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new);
                    SQLStringList.Add(sqlstr2);
                    string sqlstr3 = string.Format(" UPDATE {0}.GUIDE_MONTH_CAUSE SET GUIDE_CODE = '{2}' WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new);
                    SQLStringList.Add(sqlstr3);
                    string sqlstr4 = string.Format(" UPDATE {0}.GUIDE_VALUE SET GUIDE_CODE = '{2}' WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new);
                    SQLStringList.Add(sqlstr4);
                    string sqlstr5 = string.Format(" UPDATE {0}.GUIDE_VALUE_SUM SET GUIDE_CODE = '{2}' WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new);
                    SQLStringList.Add(sqlstr5);
                    string sqlstr6 = string.Format(" UPDATE {0}.REPORTINFO SET GUIDE_CODE = '{2}' WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new);
                    SQLStringList.Add(sqlstr6);
                    string sqlstr7 = string.Format(" UPDATE {0}.STATION_GUIDE_INFORMATION SET GUIDE_CODE = '{2}' WHERE GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new);
                    SQLStringList.Add(sqlstr7);
                    string sqlstr8 = string.Format(" UPDATE {0}.GUIDE_NAME_DICT SET GUIDE_EXPRESS =REPLACE(GUIDE_EXPRESS,'{1}','{2}')   WHERE GUIDE_EXPRESS LIKE '%{1}%' ", DataUser.HOSPITALSYS, guidecode, guidecode_new);
                    SQLStringList.Add(sqlstr8);
                    string sqlstr9 = string.Format(" UPDATE {0}.GUIDE_HOSPITAL_DEPT_MEMBER SET HOSPITAL_GUIDE_CODE = '{2}',HOSPITAL_GUIDE_NAME='{3}' WHERE HOSPITAL_GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new, guidename);
                    SQLStringList.Add(sqlstr9);
                    string sqlstr10 = string.Format(" UPDATE {0}.GUIDE_HOSPITAL_DEPT_MEMBER SET DEPT_GUIDE_CODE = '{2}',DEPT_GUIDE_NAME='{3}' WHERE HOSPITAL_GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new, guidename);
                    SQLStringList.Add(sqlstr10);
                    string sqlstr11 = string.Format(" UPDATE {0}.GUIDE_HOSPITAL_DEPT_MEMBER SET MEMBER_GUIDE_CODE = '{2}',MEMBER_GUIDE_NAME='{3}' WHERE HOSPITAL_GUIDE_CODE='{1}' ", DataUser.HOSPITALSYS, guidecode, guidecode_new, guidename);
                    SQLStringList.Add(sqlstr11);
                    OracleBase.ExecuteSqlTran(SQLStringList);
                }
                else
                {
                    string sqlstr12 = string.Format(@" UPDATE {0}.GUIDE_HOSPITAL_DEPT_MEMBER SET 
                        HOSPITAL_GUIDE_NAME =  DECODE(HOSPITAL_GUIDE_CODE,'{1}','{2}',HOSPITAL_GUIDE_NAME),
                        DEPT_GUIDE_NAME =    DECODE(DEPT_GUIDE_CODE,'{1}','{2}',DEPT_GUIDE_NAME),
                        MEMBER_GUIDE_NAME =  DECODE(DEPT_GUIDE_CODE,'{1}','{2}',MEMBER_GUIDE_NAME)
                        WHERE HOSPITAL_GUIDE_CODE||','||DEPT_GUIDE_CODE||','||MEMBER_GUIDE_CODE LIKE '%{1}%'", DataUser.HOSPITALSYS, guidecode, guidename);
                    OracleBase.ExecuteSql(sqlstr12);
                }
            }
            return "";
        }

        /// <summary>
        /// 查询指标sql
        /// </summary>
        /// <param name="guidecode"></param>
        /// <returns></returns>
        public DataSet GetGuideExpressByGuideCode(string guidecode)
        {
            string str = string.Format("SELECT GUIDE_SQL,GUIDE_SQL_SUM,GUIDE_SQL_DETAIL FROM {0}.GUIDE_EXPRESSIONS WHERE GUIDE_CODE='{1}'", DataUser.HOSPITALSYS, guidecode);
            return OracleOledbBase.ExecuteDataSet(str);
        }

        /// <summary>
        /// 保存指标SQL算法
        /// </summary>
        /// <param name="guidecode"></param>
        /// <param name="guidesql"></param>
        /// <param name="guidesumsql"></param>
        /// <param name="guidedetailsql"></param>
        /// <returns></returns>
        public string SaveGuideSqls(string guidecode, string guidesql, string guidesumsql, string guidedetailsql)
        {
            string rtn = "";
            if (guidesql.Equals("") && guidesumsql.Equals("") && guidedetailsql.Equals(""))
            {
                OracleBase.ExecuteSql(string.Format("DELETE FROM {0}.GUIDE_EXPRESSIONS WHERE GUIDE_CODE ='{1}'", DataUser.HOSPITALSYS, guidecode));
                OracleBase.ExecuteSql(string.Format("UPDATE {0}.GUIDE_NAME_DICT SET GUIDE_SQL='0' WHERE GUIDE_CODE ='{1}'", DataUser.HOSPITALSYS, guidecode));
                return rtn;
            }
            rtn = CheckGuideSql(guidecode, guidesql, 5);
            if (!rtn.Equals(""))
            {
                return "月份" + rtn;
            }
            rtn = CheckGuideSql(guidecode, guidesumsql, 6);
            if (!rtn.Equals(""))
            {
                return "区间" + rtn;
            }
            OracleBase.ExecuteSql(string.Format("DELETE FROM {0}.GUIDE_EXPRESSIONS WHERE GUIDE_CODE ='{1}'", DataUser.HOSPITALSYS, guidecode));
            string SERIAL_NO = OracleOledbBase.GetSingle("SELECT NVL(MAX(SERIAL_NO),0) + 1 FROM " + DataUser.HOSPITALSYS + ".GUIDE_EXPRESSIONS").ToString();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(" INSERT INTO {0}.GUIDE_EXPRESSIONS(", DataUser.HOSPITALSYS);
                strSql.Append(" SERIAL_NO,GUIDE_CODE,GUIDE_SQL,GUIDE_SQL_SUM,GUIDE_SQL_DETAIL )");
                strSql.Append(" VALUES (");
                strSql.Append("?,?,?,?,?)");
                OleDbParameter[] parameters = {
                    new OleDbParameter("SERIAL_NO",       SERIAL_NO),
					new OleDbParameter("GUIDE_CODE",      guidecode),
					new OleDbParameter("GUIDE_SQL",       guidesql),
					new OleDbParameter("GUIDE_SQL_SUM",   guidesumsql),
					new OleDbParameter("GUIDE_SQL_DETAIL",guidedetailsql)
                };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
                OracleBase.ExecuteSql(string.Format("UPDATE {0}.GUIDE_NAME_DICT SET GUIDE_SQL='1' WHERE GUIDE_CODE ='{1}'", DataUser.HOSPITALSYS, guidecode));
                return rtn;
            }
            catch
            {
                rtn = "指标保存时发生数据库错误，保存失败！";
                return rtn;
            }
        }

        /// <summary>
        /// 指标SQL检查
        /// </summary>
        /// <param name="guidecode"></param>
        /// <param name="guidesql"></param>
        /// <param name="colcnt"></param>
        /// <returns></returns>
        public string CheckGuideSql(string guidecode, string guidesql, int colcnt)
        {
            string rtn = "";
            if (guidesql.Equals(""))
            {
                return rtn;
            }
            if (!guidesql.Contains("'" + guidecode + "'"))
            {
                rtn = "指标SQL中未包含指标代码'" + guidecode + "',请检查";
                return rtn;
            }
            if (!((guidecode.Substring(0, 1).Equals("1") && guidesql.Contains("'Y'")) || (guidecode.Substring(0, 1).Equals("2") && guidesql.Contains("'K'")) || (guidecode.Substring(0, 1).Equals("4") && guidesql.Contains("'Z'")) || (guidecode.Substring(0, 1).Equals("3") && guidesql.Contains("'R'"))))
            {
                rtn = "指标SQL中类别代码有误！('Y','K','R','Z'请用大写)";
                return rtn;
            }
            DataTable dt = new DataTable();
            try
            {
                dt = OracleOledbBase.ExecuteDataSet(guidesql).Tables[0];
            }
            catch
            {
                rtn = "指标SQL语法有误，请检查！";
                return rtn;
            }
            if (dt.Columns.Count != colcnt)
            {
                rtn = "指标SQL返回结果集列数错误,<br/>月份指标为5列,区间指标为6列,请检查！";
                return rtn;
            }
            return rtn;
        }

        /// <summary>
        /// 获得奖金指标关键指标
        /// </summary>
        /// <returns></returns>
        public DataTable GetPrimaryGuide()
        {
            string sql = " select GUIDE_CODE,GUIDE_NAME,BONUS_TYPE,BONUS_NAME from performance.SET_BONUSGUIDE_BASE order by GUIDE_CODE";
            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectRow"></param>
        /// <returns></returns>
        public string SetPrimaryGuide(Dictionary<string, string>[] selectRow)
        {
            for (int i = 0; i < selectRow.Length; i++)
            {
                string sqlName = " select * from hospitalsys.GUIDE_NAME_DICT where GUIDE_CODE='" + selectRow[i]["GUIDE_CODE"] + "'";
                if (OracleOledbBase.ExecuteDataSet(sqlName).Tables[0].Rows.Count == 0)
                {
                    return selectRow[i]["GUIDE_CODE"] + "指标代码不在指标设置中";
                }
            }

            MyLists SQLStringList = new MyLists();
            List sqlstr = new List();
            sqlstr.StrSql = string.Format(" DELETE FROM {0}.SET_BONUSGUIDE_BASE", DataUser.PERFORMANCE);
            SQLStringList.Add(sqlstr);

            List sqlstr2 = new List();
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@" INSERT INTO {0}.SET_BONUSGUIDE_BASE (GUIDE_CODE,GUIDE_NAME,BONUS_TYPE,BONUS_NAME ) ", DataUser.PERFORMANCE);
            string guide_code = "";
            string guide_name = "";
            for (int i = 0; i < selectRow.Length; i++)
            {
                guide_code = selectRow[i]["GUIDE_CODE"].Trim();
                guide_name = selectRow[i]["GUIDE_NAME"].Trim();
                sql.AppendFormat(@" SELECT  '{0}' AS GUIDE_CODE,'{1}' AS GUIDE_NAME, 
                                        '{2}' as BONUS_TYPE,'{3}' as BONUS_NAME from DUAL UNION ALL ", guide_code, guide_name, selectRow[i]["BONUS_TYPE"].Trim(), selectRow[i]["BONUS_NAME"].Trim());
            }
            sql.AppendFormat("END");
            sqlstr2.StrSql = sql.ToString().Replace("UNION ALL END", "");
            SQLStringList.Add(sqlstr2);

            OracleOledbBase.ExecuteTranslist(SQLStringList);
            return "奖金基础指标设置成功";
        }

    }
}
