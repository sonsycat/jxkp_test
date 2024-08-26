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
	/// �����෽��
	/// </summary>
	public class Appraisal
	{
        public Appraisal()
		{}


		#region  ��Ա����


        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetAppraisalList(string where, string deptFilter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT  A.EVALUATE_CODE, B.EVALUATE_CLASS_NAME, A.EVALUATE_NAME, A.START_DATE,
                                       A.END_DATE, A.EVALUATE_TIME, D.USER_NAME AS EVALUATE_APPRAISER,
                                       A.EVALUATE_DESCRIPTION, A.ORG_TYPE, A.ARCHIVE_TAGS, A.IS_EVLUATE_BONUS
                                  FROM {0}.EVALUATE_INFORMATION A,
                                       {0}.EVALUATE_TYPE_DICT B,
                                       {1}.USERS D
                                 WHERE A.EVALUATE_CLASS_CODE = B.EVALUATE_CLASS_CODE
                                   AND A.EVALUATE_APPRAISER = D.USER_ID ", DataUser.HOSPITALSYS, DataUser.HISFACT);

            if (where != "")
            {
                str.Append(where);
            }
            if (deptFilter != "")
            {
                str.AppendFormat(@" AND EXISTS (
                          SELECT *
                            FROM {0}.EVALUATE_RESULT_LIST E
                           WHERE A.EVALUATE_CODE = E.EVALUATE_CODE
                             AND EVALUATE_DEPT_CODE IN ({1}))", DataUser.HOSPITALSYS, deptFilter);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// ��ѯ������ϸ
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataSet GetEvaluateDetail(string code, string deptFilter)
        {
            StringBuilder str = new StringBuilder();
            string strcode = string.Format( "SELECT DISTINCT A.GUIDE_CODE,B.GUIDE_NAME,A.ORG_TYPE FROM {0}.EVALUATE_GUIDE_VALUE A,{0}.GUIDE_NAME_DICT B WHERE A.GUIDE_CODE=B.GUIDE_CODE AND  EVALUATE_CODE='{1}' ORDER BY A.GUIDE_CODE",DataUser.HOSPITALSYS,code);
            DataTable table = OracleOledbBase.ExecuteDataSet(strcode).Tables[0];
            str.Append("SELECT A.EVALUATE_DEPT_CODE, A.EVALUATE_PERSON_NAME STAFF_ID,A.ORG_TYPE,d.DEPT_NAME \"��������\", C.NAME \"����\", A.EVALUATE_VALUE \"�÷�\",A.ONGUARD_DAYS \"�ڸ�����\"");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                str.Append(", (SELECT B.GUIDE_VALUE FROM "+DataUser.HOSPITALSYS+".EVALUATE_GUIDE_VALUE B WHERE B.EVALUATE_CODE = '" + code + "' ");
                if (table.Rows[i]["ORG_TYPE"].ToString() == "K")
                {
                    str.Append(" AND A.EVALUATE_DEPT_CODE = B.UNIT_CODE AND B.GUIDE_CODE ='");
                }
                else
                {
                    str.Append(" AND A.EVALUATE_PERSON_NAME = B.UNIT_CODE AND B.GUIDE_CODE ='");
                }
                str.Append(table.Rows[i]["GUIDE_CODE"].ToString() + "') AS \"" + table.Rows[i]["GUIDE_NAME"].ToString().Replace("��","(").Replace("��", ")") + "\" ");
            }
            str.Append(" FROM " + DataUser.HOSPITALSYS + ".EVALUATE_RESULT_LIST A," + DataUser.RLZY + ".NEW_STAFF_INFO C," + DataUser.COMM + ".sys_dept_dict d WHERE A.EVALUATE_CODE ='" + code + "' AND A.EVALUATE_PERSON_NAME=C.STAFF_ID(+) and c.dept_code=d.dept_code");

            if (deptFilter != null && deptFilter != "")
            {
                str.Append(" and A.EVALUATE_DEPT_CODE in(" + deptFilter + ")");
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// �鵵����
        /// </summary>
        /// <param name="id"></param>
        public void UpdateApprailsalist(string id,string desc,string is_bonus)
        {
            string str = String.Format("UPDATE {0}.EVALUATE_INFORMATION SET ARCHIVE_TAGS='1',ARCHIVE_DATE=TO_CHAR(SYSDATE,'YYYY-MM-DD'),EVALUATE_DESCRIPTION='{2}',IS_EVLUATE_BONUS='{3}' WHERE EVALUATE_CODE='{1}'", DataUser.HOSPITALSYS, id, desc, is_bonus);
            OracleOledbBase.ExecuteNonQuery(str);
        }


        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="id"></param>
        public void DelApprailsaList(string id)
        {
            ArrayList SQLStringList = new ArrayList();

            string sqlstr = string.Format(" DELETE FROM  {0}.EVALUATE_INFORMATION WHERE EVALUATE_CODE='{1}' ",DataUser.HOSPITALSYS,id);
            SQLStringList.Add(sqlstr);

            sqlstr = string.Format(" DELETE FROM  {0}.EVALUATE_RESULT_LIST WHERE EVALUATE_CODE='{1}' ", DataUser.HOSPITALSYS, id);
            SQLStringList.Add(sqlstr);

            sqlstr = string.Format(" DELETE FROM  {0}.EVALUATE_GUIDE_VALUE WHERE EVALUATE_CODE='{1}' ", DataUser.HOSPITALSYS, id);
            SQLStringList.Add(sqlstr);

            OracleBase.ExecuteSqlTran(SQLStringList);
        }



        /// <summary>
        /// ���۷���
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetEvalutetype(string where)
        {
            //EVALUATE_CLASS_CODE
            //EVALUATE_CLASS_NAME
            //STATION_TYPE
            StringBuilder str = new StringBuilder();
            str.AppendFormat ("SELECT  * FROM  {0}.EVALUATE_TYPE_DICT ",DataUser.HOSPITALSYS);
            if (where != "")
            {
                str.Append(where);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        public DataSet GetEvaluteBonustype()
        {
             StringBuilder str = new StringBuilder();
             str.AppendFormat("SELECT  * FROM  {0}.EVALUATE_BONUS_TYPE_DICT ", DataUser.HOSPITALSYS );
             return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// ѡ�����ۿ���ʱ�����ҵ�������νṹ
        /// </summary>
        /// <returns></returns>
        public DataSet GetDeptClassTree()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                    SELECT T1.PID AS PID, T1.ID AS ID,T1.ATTRIBUE AS NAME 
                     FROM (
                        SELECT '0' AS PID,('L' || A.ID) AS ID,A.ATTRIBUE,0 SORTNO FROM {0}.SYS_LCDEPT_ATTR_DICT A 
                        UNION ALL 
                        SELECT '-1' AS PID,('' || C.ID) AS ID,C.ATTRIBUE,C.SORTNO FROM {0}.SYS_DEPT_ATTR_DICT C
                     ) T1
                    START WITH PID= '-1'  CONNECT BY PRIOR ID = PID  ORDER SIBLINGS BY SORTNO", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// ���ݿ�������ٴ����õ������б�
        /// </summary>
        /// <param name="classlist"></param>
        /// <returns></returns>
        public DataSet GetDeptLeftSelector(string classlist, string deptFilter)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                    SELECT DEPT_CODE ,DEPT_NAME FROM {0}.SYS_DEPT_DICT WHERE ATTR='��' 
                     AND ( ( DEPT_LCATTR IS NULL AND DEPT_TYPE IN ({1}) )  OR ( 'L'||DEPT_LCATTR  IN ({1}) ) ) ",DataUser.COMM,classlist);
            if (deptFilter != null & deptFilter != "")
            {
                str.Append(" AND " + deptFilter);
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// ����ָ�꼯���
        /// </summary>
        /// <returns></returns>
        public DataSet GetEvalGuideGroupTypeClass()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                    SELECT '0000' AS GUIDE_GROUP_TYPE ,'ͨ��ָ�꼯' AS GUIDE_GROUP_TYPE_NAME,9999 AS SORTNO FROM DUAL UNION ALL
                    SELECT GUIDE_GROUP_TYPE,GUIDE_GROUP_TYPE_NAME,SORTNO FROM {0}.GUIDE_GROUP_TYPE WHERE GUIDE_GROUP_TYPE LIKE '02__'
                    ORDER BY SORTNO,GUIDE_GROUP_TYPE", DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// �����û���ȡ����Ȩ��ָ�꼯��ָ�꼯������أ�
        /// </summary>
        /// <param name="user_id">�û�ID</param>
        /// <returns>DataSet</returns>
        public DataSet GetEvalGuideGroupTypeClass(string user_id,string organ)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.*
                                  FROM (SELECT   '0000' AS GUIDE_GROUP_TYPE,
                                                 'ͨ��ָ�꼯' AS GUIDE_GROUP_TYPE_NAME, 9999 AS SORTNO
                                            FROM DUAL
                                        UNION ALL
                                        SELECT   GUIDE_GROUP_TYPE, GUIDE_GROUP_TYPE_NAME, SORTNO
                                            FROM {0}.GUIDE_GROUP_TYPE
                                           WHERE GUIDE_GROUP_TYPE LIKE '02__'
                                        ORDER BY SORTNO, GUIDE_GROUP_TYPE) A,
                                       (SELECT DISTINCT C.GUIDE_ATTR, TARGET_ID
                                          FROM {1}.SYS_POWER_DETAIL A,
                                               {0}.GUIDE_GATHER_ROLE B,
                                               {0}.GUIDE_GATHER_CLASS C
                                         WHERE POWER_ID = B.ROLE_ID
                                           AND C.ORGAN_CLASS = '{3}'
                                           AND B.GUIDE_GATHER_CODE = C.GUIDE_GATHER_CODE
                                           AND (EVALUATION_YEAR = 'all' OR EVALUATION_YEAR = TO_CHAR(SYSDATE,'YYYY') )
                                           AND A.TARGET_ID='{2}') B
                                 WHERE A.GUIDE_GROUP_TYPE = B.GUIDE_ATTR", DataUser.HOSPITALSYS, DataUser.COMM, user_id, organ);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// ������ָ�꼯����ͨ����ָ�꼯
        /// </summary>
        /// <param name="typestr"></param>
        /// <param name="organ"></param>
        /// <returns></returns>
        public DataSet GetEvalGuideGather(string typestr, string organ)
        { 
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                    SELECT GUIDE_GATHER_CODE,GUIDE_GATHER_NAME  FROM {0}.GUIDE_GATHER_CLASS A, HOSPITALSYS.GUIDE_GROUP_TYPE B 
                     WHERE A.GUIDE_ATTR = B.GUIDE_GROUP_TYPE(+) 
                       AND ( ( '{1}' = '0000' AND B.ALLUSE = 1) OR ( '{1}' <> '0000' AND A.GUIDE_ATTR = '{1}') )
                       AND A.ORGAN_CLASS = '{2}'
                       AND (EVALUATION_YEAR = 'all' OR EVALUATION_YEAR = TO_CHAR(SYSDATE,'YYYY') )
                     ORDER BY GUIDE_GATHER_CODE", DataUser.HOSPITALSYS, typestr, organ);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// ��ȡ�û��߱�Ȩ�޵�ָ�꼯(����)
        /// </summary>
        /// <param name="typestr">ָ�꼯���</param>
        /// <param name="organ">Ժ�����ʶ</param>
        /// <param name="user_id">�û�ID</param>
        /// <returns>DataSet</returns>
        public DataSet GetEvalGuideGather(string typestr, string organ,string user_id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT A.GUIDE_GATHER_CODE, GUIDE_GATHER_NAME
                                FROM {0}.GUIDE_GATHER_CLASS A,
                                     {0}.GUIDE_GROUP_TYPE B,
                                     (SELECT GUIDE_GATHER_CODE
                                        FROM {1}.SYS_POWER_DETAIL C, {0}.GUIDE_GATHER_ROLE D
                                       WHERE C.POWER_ID = D.ROLE_ID AND TARGET_ID = '{4}') E
                               WHERE A.GUIDE_ATTR = B.GUIDE_GROUP_TYPE(+)
                                 AND A.GUIDE_GATHER_CODE = E.GUIDE_GATHER_CODE
                                 AND (('{2}' = '0000' AND B.ALLUSE = 1)OR ('{2}' <> '0000' AND A.GUIDE_ATTR = '{2}'))
                                 AND A.ORGAN_CLASS = '{3}'
                                 AND (EVALUATION_YEAR = 'all' OR EVALUATION_YEAR = TO_CHAR (SYSDATE, 'YYYY') )
                            ORDER BY GUIDE_GATHER_CODE", DataUser.HOSPITALSYS, DataUser.COMM,typestr, organ,user_id);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// ָ�꼯��ϸ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetGuideGroupList(string id)
        {
            string str = string.Format(@"
                    SELECT A.GUIDE_CODE, B.GUIDE_NAME,B.GUIDE_NAME ||'('|| A.QZ ||')' GUIDE_NAME_QZ
                      FROM {0}.GUIDE_GATHERS A, {0}.GUIDE_NAME_DICT B
                     WHERE A.GUIDE_CODE = B.GUIDE_CODE
                       AND B.ISHIGHGUIDE<>'2'
                       AND A.GUIDE_GATHER_CODE = '{1}' ORDER BY A.GUIDE_CODE", DataUser.HOSPITALSYS, id);
            return OracleOledbBase.ExecuteDataSet(str);
        }



        /// <summary>
        /// ����BSC���η������֯������ ��ѯָ���б�
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="org"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        public DataSet GetGuideDictListByBscOrgDept(string bsc, string dept, string org)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                SELECT A.GUIDE_CODE,  A.GUIDE_NAME, A.GUIDE_NAME  GUIDE_NAME_QZ
                  FROM {0}.GUIDE_NAME_DICT A
                 WHERE A.ISPAGE = '1' AND A.ISSEL='1' 
                   AND A.ISHIGHGUIDE<>'2'
                   AND DECODE('{1}',NULL,'1',A.ORGAN) = NVL('{1}','1')
                   AND DECODE('{2}',NULL,'1',DECODE(A.DEPT,'00' , '{2}' ,A.DEPT)) = NVL('{2}','1') ", DataUser.HOSPITALSYS, org, dept);
            if (!bsc.Equals(""))
            {
                str.AppendFormat(" AND A.BSC = '{0}' ", bsc);
            }
            str.Append(" ORDER BY SERIAL_NO DESC");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        /// <summary>
        /// ����ָ�갴BSC������
        /// </summary>
        /// <param name="depttype"></param>
        /// <param name="OrganType"></param>
        /// <returns></returns>
        public DataSet getBSCGuideTree(string OrganType)
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
                             WHERE A.ISPAGE = '1' AND A.ISSEL='1' ", DataUser.HOSPITALSYS);
            str.AppendFormat(@"
                               AND  A.ORGAN  = '{0}'
                           ) C  
                      START WITH PID= '0'  CONNECT BY PRIOR ID = PID 
                      )AA
                 WHERE AA.GLEVEL =1 OR (AA.GLEVEL = 2 AND  AA.LEAF = 0)   ", OrganType);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// ����ָ�갴���ҷ�����
        /// </summary>
        /// <returns></returns>
        public DataSet getDeptGuideTree()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" 
                 SELECT 'D'||DEPT_CLASS_CODE AS DEPT_CLASS_CODE ,DEPT_CLASS_NAME FROM {0}.JXGL_GUIDE_DEPT_CLASS_DICT UNION ALL
                 SELECT 'D00' ,'��������' FROM DUAL ORDER BY DEPT_CLASS_CODE", DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        /// <summary>
        /// �õ�����ָ���Ȩ���Լ���Ӧ�ĸߵ��ž���ֵ�ӷֶ�
        /// </summary>
        /// <param name="guidestrs"></param>
        /// <param name="qzstrs"></param>
        /// <returns></returns>
        public DataSet GetEvalGuideQZHIGHT(string guidestrs,string qzstrs)
        {
            //guidestrs ="20106018,20106019,20106020,20106021,20106022,20106023,20106024,20106025,20106026";
            //qzstrs = "5,2.5,-12,25,3.5,5.1,8,4,3.3";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                    SELECT C.I,C.GUIDE_CODE,D.GUIDE_NAME,C.QZ,DECODE(D.ISHIGHGUIDE,'1',1,DECODE(D.ISABS,'1',-11,-10)) HIGHT 
                      FROM ( SELECT A.GUIDE_CODE,B.QZ,A.I FROM 
                               (SELECT rownum i, REGEXP_SUBSTR( '{1}' ,'[0-9]+',1,ROWNUM ) GUIDE_CODE  FROM DUAL  CONNECT BY ROWNUM <999) A,
                               (SELECT rownum i, REGEXP_SUBSTR( '{2}' ,'[^,]+',1,ROWNUM ) qz  FROM DUAL  CONNECT BY ROWNUM < 999) B
                              WHERE A.I=B.I AND A.GUIDE_CODE IS NOT NULL
                            )C,{0}.GUIDE_NAME_DICT D
                    WHERE C.GUIDE_CODE = D.GUIDE_CODE(+)
                    ORDER BY C.I ", DataUser.HOSPITALSYS,guidestrs,qzstrs);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// ����ָ��õ�����ֵ
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="guide"></param>
        /// <param name="depttype"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public DataTable GetAppraisalByGuide( Dictionary<string, string>[] Units, Dictionary<string, string>[] Guides, string organ, string incount)
        {
            StringBuilder str = new StringBuilder();
            string virtaltables = "";
            for (int i = 0; i < Units.Length; i++)
            {
                virtaltables = virtaltables + "\n SELECT " + i.ToString() + " AS I, '" + Units[i]["value"] + "' AS UNIT_CODE, '" + Units[i]["text"] + "' AS UNIT_NAME FROM DUAL UNION ALL ";
            }
            virtaltables = virtaltables + "END";
            virtaltables = virtaltables.Replace("UNION ALL END", "");
            str.AppendFormat(@" SELECT B.UNIT_CODE ,B.UNIT_NAME,C.* FROM (
            {0}
            ) B, ", virtaltables);
            str.Append( organ.Equals("R")?" (SELECT A.PERSON_ID AS CODE ":" (SELECT A.DEPT_CODE AS CODE ");
            for (int i = 0; i < Guides.Length; i++)
            {
                str.Append(@" ,SUM(DECODE(A.GUIDE_CODE, '"+Guides[i]["value"]+"',A.GUIDE_VALUE, 0)  ) AS \""+Guides[i]["text"].Substring(0, Guides[i]["text"].LastIndexOf('(')) +"\"  \n");
            }
            str.AppendFormat(" FROM {0}.EVALUATE_TEMP A  WHERE  COUNTING='{1}' ", DataUser.HOSPITALSYS, incount);
            str.Append(organ.Equals("R") ? "  GROUP BY A.PERSON_ID) C " : "  GROUP BY A.DEPT_CODE) C ");
            str.Append(@"
             WHERE B.UNIT_CODE = C.CODE(+) 
             ORDER BY B.I");
            DataTable dt = OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
            dt.Columns[0].ColumnName = organ.Equals("R") ? "��Ա���" : "���Ҵ���";
            dt.Columns[1].ColumnName = organ.Equals("R") ? "��Ա����" : "��������";
            dt.Columns[2].ColumnName = "�÷�";

            for (int i = 0; i < Guides.Length; i++)
            {
                dt.Columns[i + 3].Caption = Guides[i]["value"];
            }

            return dt;
        }


        /// <summary>
        /// ��ѯ��Ա
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns></returns>
        public DataSet GetStaff(string where)
        {
            StringBuilder str = new StringBuilder();

//            str.AppendFormat(@"select staff_id as PERSON_CODE,case when
//                                  (select count(NAME) from RLZY.NEW_STAFF_INFO t2 where T2.NAME = t1.name and t2.add_mark='1') = 2 
//                                  THEN t1.name ||'('|| DEPT_NAME ||')'
//                                  ELSE t1.name 
//                                  END as PERSON_NAME FROM {0}.NEW_STAFF_INFO t1 where add_mark='1' ", DataUser.RLZY);
            str.AppendFormat(@"SELECT STAFF_ID AS PERSON_CODE,T1.NAME ||'('|| DEPT_NAME ||')'AS PERSON_NAME FROM {0}.NEW_STAFF_INFO t1 WHERE ADD_MARK='1' ", DataUser.RLZY);
            if (where != "")
            {
                str.Append(" " + where);
            }
            str.Append(" ORDER BY STAFF_ID");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// ���漨Ч����
        /// </summary>
        /// <param name="model"></param>
        /// <param name="table"></param>
        public string SaveEvaluate(DataTable table, string startdate, string enddate, string incount, string evalapp, string evaname, string evatype, string evamemo, string unittype, string bonusclass)
        {
            string rtn = "";
            //ArrayList SQLStringList = new ArrayList();
            MyLists SQLStringList = new MyLists();
            string evaluatecode =  OracleOledbBase.GetMaxID( "EVALUATE_CODE", DataUser.HOSPITALSYS+".EVALUATE_INFORMATION").ToString();

            //�������ۻ�����
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO " + DataUser.HOSPITALSYS + ".EVALUATE_INFORMATION(");
            strSql.Append(" EVALUATE_CODE,EVALUATE_NAME,EVALUATE_CLASS_CODE,EVALUATE_ARITHMETIC,EVALUATE_DESCRIPTION,START_DATE,END_DATE,EVALUATE_APPRAISER,EVALUATE_TIME,ARCHIVE_TAGS,ORG_TYPE)");
            strSql.AppendFormat(@"
                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',to_char(sysdate,'yyyy-mm-dd'),0,'{8}')"
                     , evaluatecode , evaname.Replace("'","''") , evatype , evalapp , evamemo.Replace("'","''") , startdate.Substring(0,6) , enddate.Substring(0,6) , incount , unittype  );
            List sqlList = new List();
            sqlList.StrSql = strSql.ToString();
            SQLStringList.Add(sqlList);

            //�������������
            int detailid = OracleOledbBase.GetMaxID( "SERIAL_NO", DataUser.HOSPITALSYS+".EVALUATE_RESULT_LIST");
            StringBuilder strdetail = new StringBuilder();
            strdetail.Append("INSERT INTO " + DataUser.HOSPITALSYS + ". EVALUATE_RESULT_LIST(");
            strdetail.Append("SERIAL_NO,EVALUATE_CODE,EVALUATE_DEPT_CODE,EVALUATE_PERSON_NAME,GUIDE_VALUE,EVALUATE_DEPT_NAME,EVALUATE_VALUE,ONGUARD_DAYS,ORG_TYPE)");
            StringBuilder strsub = new StringBuilder();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (unittype.Equals("R"))
                {
                    strsub.AppendFormat(@"
                    SELECT {0} AS SERIAL_NO, {1} AS EVALUATE_CODE,  0 AS GUIDE_VALUE , '{2}'AS EVALUATE_PERSON_NAME, {3} AS EVALUATE_VALUE, '{4}' AS ORG_TYPE FROM DUAL UNION ALL "
                    , detailid, evaluatecode, table.Rows[i][0].ToString(), table.Rows[i][2].ToString(), unittype);
                }
                else
                {
                    strsub.AppendFormat(@"
                    SELECT {0} AS SERIAL_NO, {1} AS EVALUATE_CODE, '{2}' AS EVALUATE_DEPT_CODE, ''  AS EVALUATE_PERSON_NAME, 0 AS GUIDE_VALUE , '{3}' AS EVALUATE_DEPT_NAME, '{4}' EVALUATE_VALUE, 0 AS ONGUARD_DAYS , '{5}' AS ORG_TYPE FROM DUAL UNION ALL "
                    , detailid, evaluatecode, table.Rows[i][0].ToString(), table.Rows[i][1].ToString(), table.Rows[i][2].ToString(), unittype);
                }
                detailid += 1;
            }
            if (unittype.Equals("R"))
            {
                strdetail.Append(@"
                    SELECT A.SERIAL_NO,A.EVALUATE_CODE, B.DEPT_CODE AS EVALUATE_DEPT_CODE, A.EVALUATE_PERSON_NAME, A.GUIDE_VALUE,
                    B.DEPT_NAME AS EVALUATE_DEPT_NAME,A.EVALUATE_VALUE,NVL(B.ONGUARD_DAYS,0) AS ONGUARD_DAYS ,A.ORG_TYPE FROM (");
                strdetail.Append((strsub.ToString()+"END").Replace("UNION ALL END",""));
                strdetail.AppendFormat(@" ) A LEFT JOIN
                    ( SELECT STAFF_ID, MAX(DEPT_CODE) DEPT_CODE,MAX(DEPT_NAME) DEPT_NAME ,SUM(ONGUARD_DAYS) ONGUARD_DAYS 
                      FROM {0}.ONGUARD_DAYS WHERE ONGUARD_DATE BETWEEN '{1}' AND '{2}' GROUP BY STAFF_ID) B
                      ON A. EVALUATE_PERSON_NAME = B.STAFF_ID", DataUser.RLZY, startdate.Substring(0, 6), enddate.Substring(0, 6));
            }
            else
            {
                strdetail.Append((strsub.ToString() + "END").Replace("UNION ALL END", ""));
            }
            List sqlList2 = new List();
            sqlList2.StrSql = strdetail.ToString();
            SQLStringList.Add(sqlList2);
            //SQLStringList.Add(strdetail.ToString());

            //��������ָ������
            int guideid = OracleOledbBase.GetMaxID("ID", DataUser.HOSPITALSYS + ".EVALUATE_GUIDE_VALUE");
            StringBuilder strguide = new StringBuilder();
            strguide.AppendFormat(" INSERT INTO {0}.EVALUATE_GUIDE_VALUE(ID,EVALUATE_CODE,GUIDE_CODE,GUIDE_VALUE,UNIT_CODE,ORG_TYPE) ", DataUser.HOSPITALSYS);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 3; j < table.Columns.Count; j++)
                {
                    strguide.AppendFormat(@"
                                SELECT {0} AS ID,'{1}' AS EVALUATE_CODE ,'{2}' AS GUIDE_CODE, '{3}' AS GUIDE_VALUE ,'{4}' AS UNIT_CODE ,'{5}'AS ORG_TYPE FROM DUAL UNION ALL "
                        , guideid
                        , evaluatecode
                        , table.Columns[j].Caption
                        , table.Rows[i][j].ToString().Equals("") ? "0" : table.Rows[i][j].ToString()
                        , table.Rows[i][0].ToString()
                        , unittype);
                    guideid += 1;
                }
            }
            strguide.Append("END");
            List sqlList3 = new List();
            sqlList3.StrSql = strguide.ToString().Replace("UNION ALL END", "");
            SQLStringList.Add(sqlList3);
            //SQLStringList.Add(strguide.ToString().Replace("UNION ALL END", ""));

            //ִ������
            try
            {
                //OracleBase.ExecuteSqlTran(SQLStringList);
                OracleOledbBase.ExecuteTranslist(SQLStringList);
            }
            catch (Exception ee)
            {
                rtn = "�������ݱ���ʧ�ܣ�<br/>ԭ��" + ee.Message;
                return rtn;
            }
            return rtn;
        }
        /// <summary>
        /// ���ݿ���������ȡ������Դ���Ҵ��롢����
        /// </summary>
        /// <param name="inputcode">������</param>
        /// <param name="deptfilter">���ҹ�������</param>
        /// <returns>DataSet</returns>
        public DataSet GetStaffDept(string inputcode, string deptfilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select DISTINCT A.DEPT_CODE, a.dept_name,a.input_code 
                                              from {0}.SYS_DEPT_DICT a,{1}.NEW_STAFF_INFO B 
                                             WHERE A.DEPT_CODE = B.DEPT_CODE 
                                               AND B.ADD_MARK = '1' AND a.input_code like '{2}%' ", DataUser.COMM, DataUser.RLZY, inputcode.ToUpper());
            if (deptfilter != "")
            {
                strSql.Append(" and a." + deptfilter + "  order by a.DEPT_CODE");
            }
            else
            {
                strSql.Append("  order by a.DEPT_CODE");
            }
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
		#endregion  ��Ա����
	}
}

