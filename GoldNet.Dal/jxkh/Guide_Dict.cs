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
	public class Guide_Dict
	{
        public Guide_Dict()
		{}
		#region  成员方法



        /// <summary>
        /// 指标列表
        /// </summary>
        /// <returns></returns>
        public  DataSet GetGuideDictList(string guidename, string ispage, string bscone, string bsctwo)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"
                SELECT A.GUIDE_CODE, B.BSC_CLASS_NAME  BSC_CLASS_NAME1, E.BSC_CLASS_NAME  BSC_CLASS_NAME2, A.GUIDE_NAME, A.GUIDE_EXPRESS,
                       CASE WHEN A.ISEXPRESS = '0' THEN '否' ELSE '是' END  ISEXPRESS,
                       CASE WHEN A.ISPAGE = '0' THEN '停用' WHEN A.ISPAGE = '1' THEN '启用' ELSE '不显示' END  ISPAGE,
                       CASE WHEN A.ISHIGHGUIDE = '0' THEN '否' WHEN A.ISHIGHGUIDE = '1' THEN  '是' ELSE '一般' END   ISHIGHGUIDE, 
                       CASE WHEN A.ISSEL = '0' THEN '否' ELSE '是' END   ISSEL, 
                       CASE WHEN A.FIXNUM = '0' THEN '否' ELSE '是' END   FIXNUM,
                       CASE WHEN A.ISABS = '0' THEN '否' ELSE '是' END  ISABS, 
                       CASE WHEN (A.ISEXPRESS = '1' AND  A.GUIDE_EXPRESS IS NULL)  THEN '无'
                            WHEN (A.ISEXPRESS = '1' AND  LENGTH(A.GUIDE_EXPRESS)>0 )  THEN '有'
                            WHEN A.GUIDE_SQL = '0' THEN '无' ELSE '有' END  GUIDE_SQL,
                       DECODE(A.DEPT,'00','公共部分', C.DEPT_CLASS_NAME) DEPT_CLASS_NAME, F.ORGAN_CLASS_NAME ,A.BSC,A.DEPT,A.ORGAN,A.SERIAL_NO,A.GUIDE_GATHER_CODE,A.THRESHOLD_RATIO,a.EXPLAIN,nvl(a.SORT_NO,0) SORT_NO
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
            str.Append(" order by SERIAL_NO desc");
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }
        /// <summary>
        /// 根据BSC树形分类和组织、科室 查询指标列表
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="org"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        public DataSet GetGuideDictListByBscOrgDept(string bsc, string org, string dept,string tag)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                SELECT A.GUIDE_CODE,  A.GUIDE_NAME, A.GUIDE_NAME  GUIDE_NAME_QZ
                  FROM {0}.GUIDE_NAME_DICT A
                 WHERE A.ISPAGE = '1'
                   AND DECODE('{1}',NULL,'1',A.ORGAN) = NVL('{1}','1')", DataUser.HOSPITALSYS,org);
            if (!bsc.Equals(""))
            {
                str.AppendFormat(" AND A.BSC = '{0}' ", bsc);
            }
            if (tag.Equals(""))
            {
                str.AppendFormat("AND DECODE('{0}',NULL,'1',DECODE(A.DEPT,'00' , '{0}' ,A.DEPT)) = NVL('{0}','1') ", dept);
            }
            else
            {
                str.Append(" AND (A.GUIDE_NAME LIKE '%" + tag + "%' OR A.GUIDE_CODE LIKE '%" + tag + "%')");
            }
            str.Append(" ORDER BY SERIAL_NO DESC");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询指标
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetGuideList(string where)
        {
            StringBuilder str = new StringBuilder();
            str.Append("select guide_code,guide_name from guide_name_dict where ispage='1'");
            if (where != "")
            {
                str.Append(" and " + where);
            }
            str.Append(" order by guide_name");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询指标BSC分类，构造树形结构，仅显示bsc2级分类下存在指标的树形结构
        /// </summary>
        /// <param name="depttype"></param>
        /// <param name="OrganType"></param>
        /// <returns></returns>
        public DataSet getBSCGuideTree(string depttype, string OrganType)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" 
                SELECT AA.NAME,AA.ID,AA.PID FROM(
                       SELECT  C.NAME,C.ID,LEVEL GLEVEL,CONNECT_BY_ISLEAF LEAF ,C.PID
                         FROM ( 
                            SELECT BSC_CLASS_NAME NAME, BSC_CLASS_CODE ID ,  
                                   DECODE( LENGTH(B.BSC_CLASS_CODE) ,2 ,'0', SUBSTR(B.BSC_CLASS_CODE,1,LENGTH(B.BSC_CLASS_CODE)- 2)   ) AS PID,
                                   '' DEPT,'' ORGAN
                              FROM {0}.JXGL_GUIDE_BSC_CLASS_DICT   B UNION ALL
                            SELECT GUIDE_NAME NAME , GUIDE_CODE CODE ,BSC PID , DEPT,ORGAN
                              FROM {0}.GUIDE_NAME_DICT A
                             WHERE A.ISPAGE = '1' ",DataUser.HOSPITALSYS);
            if (!depttype.Equals(""))
            {
                str.AppendFormat(@" AND  DECODE(A.DEPT,'00' , '{0}' ,A.DEPT) = '{0}' ",depttype);
            }
            str.AppendFormat(@"
                               AND  A.ORGAN  = '{0}'
                           ) C  
                      START WITH PID= '0'  CONNECT BY PRIOR ID = PID 
                      )AA
                 WHERE AA.GLEVEL =1 OR (AA.GLEVEL = 2 AND  AA.LEAF = 0)   ", OrganType);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 更新指标集包含的指标项目
        /// </summary>
        /// <param name="guidegatherid"></param>
        /// <param name="selectedRow"></param>
        /// <returns></returns>
        public string UpdateGuideGatherDetail(string guidegatherid, Dictionary<string, string>[] selectedRow)
        {
            string resultstr = "";
            
            MyLists list = new MyLists();

            List l_listDelSql = new List();
            l_listDelSql.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDelSql.StrSql = string.Format(" DELETE FROM {0}.GUIDE_GATHERS WHERE GUIDE_GATHER_CODE = '{1}'", DataUser.HOSPITALSYS, guidegatherid);
            list.Add(l_listDelSql);
            for (int i = 0; i < selectedRow.Length; i++)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@" INSERT INTO {0}.GUIDE_GATHERS (GUIDE_GATHER_CODE,GUIDE_CODE,QZ ) values(?,?,?) ", DataUser.HOSPITALSYS);
               
                string qz = "";
                qz = selectedRow[i]["GUIDE_NAME_QZ"].Trim().Replace(selectedRow[i]["GUIDE_NAME"].Trim(), "").Replace("(", "").Replace(")", "");
                qz = qz.Equals("") ? "1" : qz;
                OleDbParameter[] parameter = {
											  new OleDbParameter("GUIDE_GATHER_CODE",guidegatherid),
                                              new OleDbParameter("GUIDE_CODE",selectedRow[i]["GUIDE_CODE"].Trim()),
                                              new OleDbParameter("QZ",qz)
										  };
                List insertsql = new List();
                insertsql.Parameters = parameter;
                insertsql.StrSql = sql.ToString();
                list.Add(insertsql);
            }
            OracleOledbBase.ExecuteTranslist(list);
            return resultstr;

        }


        /// <summary>
        /// 指标集详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetGuideGroupList(string id)
        {
            string str = string.Format(@"SELECT A.GUIDE_CODE, B.GUIDE_NAME,B.GUIDE_NAME ||'('|| A.QZ ||')' GUIDE_NAME_QZ
                                          FROM {0}.GUIDE_GATHERS A, {0}.GUIDE_NAME_DICT B
                                         WHERE A.GUIDE_CODE = B.GUIDE_CODE
                                           AND A.GUIDE_GATHER_CODE = '{1}' ORDER BY A.GUIDE_CODE", DataUser.HOSPITALSYS, id);
            return OracleOledbBase.ExecuteDataSet(str);


        }
        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        public  DataSet GetDeptType()
        {
            return OracleOledbBase.ExecuteDataSet(string.Format("SELECT '00' AS DEPT_CLASS_CODE, '公共部分' AS DEPT_CLASS_NAME FROM DUAL  UNION SELECT DEPT_CLASS_CODE,DEPT_CLASS_NAME FROM {0}.JXGL_GUIDE_DEPT_CLASS_DICT", DataUser.HOSPITALSYS));
        }
        /// <summary>
        /// 组织
        /// </summary>
        /// <returns></returns>
        public  DataSet Getorg()
        {
            return OracleOledbBase.ExecuteDataSet(string.Format("SELECT * FROM {0}.JXGL_GUIDE_ORGAN_CLASS_DICT",DataUser.HOSPITALSYS));
        }

        /// <summary>
        /// 指标关联列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetGuideHospitalDeptMemberList(string where)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT SERIAL_NO ,HOSPITAL_GUIDE_CODE ,HOSPITAL_GUIDE_NAME ,DEPT_GUIDE_CODE ,DEPT_GUIDE_NAME ,MEMBER_GUIDE_CODE ,MEMBER_GUIDE_NAME  FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER ",DataUser.HOSPITALSYS);
            if (where != "")
            {
                str.Append(where);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 根据组织结构查询指标
        /// </summary>
        /// <param name="organ"></param>
        /// <returns></returns>
        public DataSet GetGuideByOrgan(string organ)
        {
            string str = string.Format(@"
            SELECT GUIDE_CODE,GUIDE_NAME FROM {0}.GUIDE_NAME_DICT WHERE ORGAN='{1}' 
               AND GUIDE_CODE NOT IN 
               (SELECT HOSPITAL_GUIDE_CODE  FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER WHERE  HOSPITAL_GUIDE_CODE IS NOT NULL UNION ALL
                SELECT DEPT_GUIDE_CODE      FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER WHERE  DEPT_GUIDE_CODE IS NOT NULL  UNION ALL
                SELECT MEMBER_GUIDE_CODE    FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER WHERE  MEMBER_GUIDE_CODE IS NOT NULL) ORDER BY GUIDE_NAME",DataUser.HOSPITALSYS, organ);
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 删除指标关联
        /// </summary>
        /// <param name="id"></param>
        public string DelGuideHospitalDeptMember(string id)
        {
            string str = string.Format("DELETE FROM  {1}.GUIDE_HOSPITAL_DEPT_MEMBER where SERIAL_NO='{0}'", id, DataUser.HOSPITALSYS);
            OracleOledbBase.ExecuteNonQuery(str);
            return "";
        }

        /// <summary>
        /// 保存指标关联
        /// </summary>
        /// <param name="model"></param>
        public void SaveGuideHospitalDeptMember(string serialno, string guidey, string guidek, string guider)
        {
            string strSql = "";
            ArrayList SQLStringList = new ArrayList();
            strSql = string.Format("DELETE FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER WHERE SERIAL_NO='{1}'", DataUser.HOSPITALSYS, serialno);
            SQLStringList.Add(strSql);
            strSql = string.Format(@"
                INSERT INTO {0}.GUIDE_HOSPITAL_DEPT_MEMBER(HOSPITAL_GUIDE_CODE,HOSPITAL_GUIDE_NAME,DEPT_GUIDE_CODE,DEPT_GUIDE_NAME,MEMBER_GUIDE_CODE,MEMBER_GUIDE_NAME,SERIAL_NO)
                SELECT 	'{1}',
	                (SELECT MAX(GUIDE_NAME) FROM {0}.GUIDE_NAME_DICT WHERE GUIDE_CODE = '{1}') ,
	                '{2}',
	                (SELECT MAX(GUIDE_NAME) FROM {0}.GUIDE_NAME_DICT WHERE GUIDE_CODE = '{2}') ,
	                '{3}',
	                (SELECT MAX(GUIDE_NAME) FROM {0}.GUIDE_NAME_DICT WHERE GUIDE_CODE = '{3}') ,
	                NVL('{4}',(SELECT NVL(MAX(SERIAL_NO),0) + 1 FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER) )
                FROM DUAL", DataUser.HOSPITALSYS, guidey, guidek, guider,serialno);
            SQLStringList.Add(strSql);
            OracleBase.ExecuteSqlTran(SQLStringList);
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
            string sqlstr = string.Format("SELECT BSC_CLASS_CODE,BSC_CLASS_NAME FROM {0}.JXGL_GUIDE_BSC_CLASS_DICT WHERE SUBSTR(BSC_CLASS_CODE,1,2)='{1}'  AND LENGTH(BSC_CLASS_CODE)>2",DataUser.HOSPITALSYS,code);
            return OracleOledbBase.ExecuteDataSet(sqlstr);
        }

        /// <summary>
        /// 绑定通用指标集
        /// </summary>
        /// <returns></returns>
        public DataSet GetGuideGather(string guide_type,string organ,string evalyear)
        {
            string sql = string.Format(@"SELECT GUIDE_GATHER_CODE,GUIDE_GATHER_NAME 
                                          FROM {0}.GUIDE_GATHER_CLASS , {0}.GUIDE_GROUP_TYPE
                                         WHERE GUIDE_ATTR = GUIDE_GROUP_TYPE(+)
                                           AND (GUIDE_ATTR LIKE '{1}%'  OR  ALLUSE = '1')
                                           AND  ORGAN_CLASS = '{2}'
                                           AND (EVALUATION_YEAR ='{3}' OR EVALUATION_YEAR ='all') ORDER BY GUIDE_GATHER_CODE "
                                ,DataUser.HOSPITALSYS,guide_type,organ,evalyear);
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
                           ) ORDER BY SORTNO,GUIDE_GROUP_TYPE", DataUser.HOSPITALSYS,guidegrouptype);
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
            string str = "SELECT DECODE(MAX(GUIDE_CODE) ,NULL ," + OrganBSC2Code + "001" + ",MAX(GUIDE_CODE)+1 )  GUIDE_CODE  FROM "+DataUser.HOSPITALSYS+".GUIDE_NAME_DICT A WHERE A.GUIDE_CODE LIKE '" + OrganBSC2Code + "%'";
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
                strSql.Append(" SERIAL_NO,GUIDE_CODE,GUIDE_TYPE,GUIDE_NAME,GUIDE_EXPRESS,ISEXPRESS,ISPAGE,ISHIGHGUIDE,ISSEL,ISABS,GUIDE_SQL,BSC,DEPT,ORGAN,GUIDE_GATHER_CODE,THRESHOLD_RATIO,EXPLAIN,SORT_NO )");
                strSql.Append(" VALUES (");
                strSql.Append("?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)");
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
                    new OleDbParameter("THRESHOLD_RATIO",        FormValues["THRESHOLD_RATIO"]),
                    new OleDbParameter("EXPLAIN", FormValues["TextAreaexplain"]),
                    new OleDbParameter("SORT_NO", FormValues["NumberField1"])
                };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameters);
            }
            //更新操作
            else
            {
                strSql = new StringBuilder();
                strSql.AppendFormat("UPDATE {0}.GUIDE_NAME_DICT SET ",DataUser.HOSPITALSYS);
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
                strSql.Append(" THRESHOLD_RATIO=?,");
                strSql.Append(" FIXNUM=?,");
                strSql.Append(" ORGAN=?,");
                strSql.Append(" GUIDE_GATHER_CODE = ?,");
                strSql.Append(" EXPLAIN = ?,");
                strSql.Append(" SORT_NO = ?");
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
                    new OleDbParameter("THRESHOLD_RATIO",FormValues["THRESHOLD_RATIO"]),
                    new OleDbParameter("FIXNUM",FormValues["fixnum_Value"]),
					new OleDbParameter("ORGAN",         FormValues["OrganComb_Value"]),
                    new OleDbParameter("GUIDE_GATHER_CODE",(FormValues.ContainsKey("CorrelationCHK")?FormValues["GuideGatherCombo_Value"]:"")),
                    new OleDbParameter("EXPLAIN", FormValues["TextAreaexplain"]),
                    new OleDbParameter("SORT_NO", FormValues["NumberField1"])
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
                    //OracleBase.ExecuteSqlTran(SQLStringList);
                }
                else
                {
                    string sqlstr12 = string.Format(@" UPDATE {0}.GUIDE_HOSPITAL_DEPT_MEMBER SET 
                        HOSPITAL_GUIDE_NAME =  DECODE(HOSPITAL_GUIDE_CODE,'{1}','{2}',HOSPITAL_GUIDE_NAME),
                        DEPT_GUIDE_NAME =    DECODE(DEPT_GUIDE_CODE,'{1}','{2}',DEPT_GUIDE_NAME),
                        MEMBER_GUIDE_NAME =  DECODE(DEPT_GUIDE_CODE,'{1}','{2}',MEMBER_GUIDE_NAME)
                        WHERE HOSPITAL_GUIDE_CODE||','||DEPT_GUIDE_CODE||','||MEMBER_GUIDE_CODE LIKE '%{1}%'", DataUser.HOSPITALSYS, guidecode, guidename);
                   // OracleBase.ExecuteSql(sqlstr12);
                    OracleOledbBase.ExecuteNonQuery(sqlstr12);
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
            string str = string.Format("SELECT GUIDE_SQL,GUIDE_SQL_SUM,GUIDE_SQL_DETAIL FROM {0}.GUIDE_EXPRESSIONS WHERE GUIDE_CODE='{1}'", DataUser.HOSPITALSYS ,guidecode);
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
        public string SaveGuideSqls(string guidecode,string guidesql,string guidesumsql,string guidedetailsql)
        {
            string rtn = "";
            if (guidesql.Equals("") && guidesumsql.Equals("") && guidedetailsql.Equals(""))
            {
                OracleOledbBase.ExecuteNonQuery(string.Format("DELETE FROM {0}.GUIDE_EXPRESSIONS WHERE GUIDE_CODE ='{1}'", DataUser.HOSPITALSYS, guidecode));
                OracleOledbBase.ExecuteNonQuery(string.Format("UPDATE {0}.GUIDE_NAME_DICT SET GUIDE_SQL='0' WHERE GUIDE_CODE ='{1}'", DataUser.HOSPITALSYS, guidecode));
                return rtn;
            }
            rtn = CheckGuideSql(guidecode, guidesql, 5);
            if (!rtn.Equals(""))
            {
                return "月份"+rtn;
            }
            rtn = CheckGuideSql(guidecode, guidesumsql,6);
            if (!rtn.Equals(""))
            {
                return "区间" + rtn;
            }
            OracleOledbBase.ExecuteNonQuery(string.Format("DELETE FROM {0}.GUIDE_EXPRESSIONS WHERE GUIDE_CODE ='{1}'", DataUser.HOSPITALSYS, guidecode));
            string SERIAL_NO = OracleOledbBase.GetSingle("SELECT NVL(MAX(SERIAL_NO),0) + 1 FROM " + DataUser.HOSPITALSYS + ".GUIDE_EXPRESSIONS").ToString();
            try {
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
                OracleOledbBase.ExecuteNonQuery(string.Format("UPDATE {0}.GUIDE_NAME_DICT SET GUIDE_SQL='1' WHERE GUIDE_CODE ='{1}'", DataUser.HOSPITALSYS, guidecode));
                return rtn;
            }
            catch {
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
            if (!guidesql.Contains("'"+guidecode+"'"))
            {
                rtn = "指标SQL中未包含指标代码'" + guidecode  + "',请检查";
                return rtn;
            }
            if (colcnt.Equals(6))
            {
                if (!guidesql.ToLower().Contains("hospitalsys.guide_day_dict_sum"))
                {
                    rtn = "统计SQL中必须包含hospitalsys.guide_day_dict_sum时间字典表',请检查";
                    return rtn; 
                }
            }
            if (!((guidecode.Substring(0, 1).Equals("1") && guidesql.Contains("'Y'")) || (guidecode.Substring(0, 1).Equals("2") && guidesql.Contains("'K'")) || (guidecode.Substring(0, 1).Equals("3") && guidesql.Contains("'R'")) || (guidecode.Substring(0, 1).Equals("4") && guidesql.Contains("'Z'"))))
            {
                rtn = "指标SQL中类别代码有误！('Y','K','R','Z'请用大写)";
                return rtn;
            }

            DataTable dt = new DataTable();
            try {
                dt = OracleOledbBase.ExecuteDataSet(guidesql).Tables[0];
            }
            catch {
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


		#endregion  成员方法

        public string CreateStationGuide(string tjny)
        {
            string rtn = "全部指标数据生成成功！";

            //生成评价数据
            try
            {
                OleDbParameter[] parameters = { new OleDbParameter("TJNY", tjny) };
                OracleOledbBase.RunProcedure(DataUser.HOSPITALSYS + ".ADD_GUIDE_VALUE", parameters);
            }
            catch (Exception ee)
            {
                rtn = "数据生成失败！<br/>原因：" + ee.Message;
            }

            return rtn;
        }
	}
}

