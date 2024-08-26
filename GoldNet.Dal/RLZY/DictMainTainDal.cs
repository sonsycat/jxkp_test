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
    public class DictMainTainDal
    {
        public DictMainTainDal()
		{}

        /// <summary>
        /// 查询字典数据
        /// </summary>
        /// <param name="TableName">数据库表名</param>
        /// <param name="fieldId">ID字段名</param>
        /// <param name="filedName">NAME字段名</param>
        /// <param name="isExitflg">true:包括删除标记,false:不包括标记</param>
        /// <param name="filedflg">标记字段名</param>
        /// <returns>字典集合</returns>
        public DataSet getDictInfo(string TableName, string fieldId, string filedName, bool isExitflg, string filedflg) 
        {
            StringBuilder str = new StringBuilder();
            str.Append("SELECT ");
            str.Append(fieldId);
            str.Append(" ID ,");
            str.Append(filedName);
            str.Append(" NAME ");
            str.Append(" FROM ");
            str.AppendFormat("{0}.{1}", DataUser.RLZY, TableName);
            if (isExitflg) 
            {
                str.Append(" WHERE " + filedflg + " = 0");
            }
            str.Append(" ORDER BY ");
            str.Append(fieldId);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 更新字典项目
        /// </summary>
        /// <param name="TableName">数据库表名</param>
        /// <param name="fieldId">ID字段名</param>
        /// <param name="filedName">NAME字段名</param>
        /// <param name="isExitflg">true:包括删除标记,false:不包括标记</param>
        /// <param name="filedflg">标记字段名</param>
        /// <param name="id">选择项目ID</param>
        /// <param name="name">选择名称</param>
        public void UpdatedictInfo(string TableName, string fieldId, string filedName, bool isExitflg, string filedflg,string id,string name) 
        {
            //UPDATE  LEVEL_DICT SET LEVEL_NAME =? WHERE SERIAL_NO=?
            StringBuilder str = new StringBuilder();
            str.Append("UPDATE ");
            str.AppendFormat("{0}.{1}", DataUser.RLZY, TableName);
            str.Append(" SET ");
            str.Append(filedName);
            str.Append(" = '");
            str.Append(name);
            str.AppendFormat("' WHERE ");
            str.Append(fieldId);
            str.Append(" = ");
            str.Append(id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 删除字典项目
        /// </summary>
        /// <param name="TableName">数据库表名</param>
        /// <param name="fieldId">ID字段名</param>
        /// <param name="filedName">NAME字段名</param>
        /// <param name="isExitflg">true:包括删除标记,false:不包括标记</param>
        /// <param name="filedflg">标记字段名</param>
        /// <param name="id">选择项目ID</param>
        public void DelDictInfo(string TableName, string fieldId, bool isExitflg, string filedflg, string id) 
        {
            //UPDATE  PUBLICATION_GRADE_DICT SET IS_DEL=1 WHERE ID=?
            //DELETE FROM  GUARD_GROUP_DICT WHERE SERIAL_NO=?
            StringBuilder str = new StringBuilder();
            if (isExitflg)
            {
                str.Append("UPDATE ");
                str.AppendFormat("{0}.{1}", DataUser.RLZY, TableName);
                str.Append(" SET ");
                str.Append(filedflg);
                str.Append(" = 1");
                str.AppendFormat(" WHERE ");
                str.Append(fieldId);
                str.Append(" = ");
                str.Append(id);
            }
            else 
            {
                str.Append("DELETE ");
                str.Append(" FROM ");
                str.AppendFormat("{0}.{1}", DataUser.RLZY, TableName);
                str.Append(" WHERE ");
                str.Append(fieldId);
                str.Append(" = ");
                str.Append(id);
            }
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 插入字典项目
        /// </summary>
        /// <param name="TableName">数据库表名</param>
        /// <param name="fieldId">ID字段名</param>
        /// <param name="filedName">NAME字段名</param>
        /// <param name="id">选择项目ID</param>
        /// <param name="name">选择名称</param>
        public void InsertDictInfo(string TableName, string fieldId, string filedName,string id,string name) 
        {
            StringBuilder str = new StringBuilder();
            str.Append("INSERT INTO ");
            str.AppendFormat("{0}.{1}", DataUser.RLZY, TableName);
            str.Append(" ( ");
            str.Append(fieldId);
            str.Append(" , ");
            str.Append(filedName);
            str.Append(" ) VALUES (" + OracleOledbBase.GetMaxID(fieldId, DataUser.RLZY + "." + TableName) + ",'" + name + "')");
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        #region 培训类别字典

        /// <summary>
        /// 查询培训类别字典
        /// </summary>
        /// <returns></returns>
        public DataSet ViewTrainingListDict() 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT ID,TRAINING_CODE TYPE,LIST_NAME NAME FROM {0}.TRAINING_LIST_DICT ORDER BY ID", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 删除培训类别字典
        /// </summary>
        /// <param name="id">培训类别ID</param>
        public void DelTrainListDict(string id) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM {0}.TRAINING_LIST_DICT WHERE ID={1}", DataUser.RLZY,id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 更新类别字典
        /// </summary>
        /// <param name="id">培训类别ID</param>
        /// <param name="name">培训类别名称</param>
        /// <param name="type">培训类别类别</param>
        public void UpdataTrainListDict(string id,string name,string type)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE {0}.TRAINING_LIST_DICT SET TRAINING_CODE='{2}', LIST_NAME = '{3}' WHERE ID={1}", DataUser.RLZY, id,type,name);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 添加类别字典
        /// </summary>
        /// <param name="name">培训类别名称</param>
        /// <param name="type">培训类别类别</param>
        public void InsertTrainListDict(string name, string type) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO {0}.TRAINING_LIST_DICT (ID,LIST_NAME,TRAINING_CODE) VALUES ({1},'{2}','{3}')", DataUser.RLZY, OracleOledbBase.GetMaxID("ID", DataUser.RLZY + ".TRAINING_LIST_DICT"), name, type);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        #endregion

        #region 考核类别字典

        /// <summary>
        /// 查询考核类别字典
        /// </summary>
        /// <returns></returns>
        public DataSet ViewAssessmentListDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT ID,ASSESSMENT_CODE TYPE,LIST_NAME NAME FROM {0}.ASSESSMENT_LIST_DICT ORDER BY ID", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 删除考核类别字典
        /// </summary>
        /// <param name="id">考核类别ID</param>
        public void DelAssessmentListDict(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM {0}.ASSESSMENT_LIST_DICT WHERE ID={1}", DataUser.RLZY, id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 更新类别字典
        /// </summary>
        /// <param name="id">考核类别ID</param>
        /// <param name="name">考核类别名称</param>
        /// <param name="type">考核类别类别</param>
        public void UpdataAssessmentListDict(string id, string name, string type)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE {0}.ASSESSMENT_LIST_DICT SET ASSESSMENT_CODE='{2}', LIST_NAME = '{3}' WHERE ID={1}", DataUser.RLZY, id, type, name);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 添加类别字典
        /// </summary>
        /// <param name="name">考核类别名称</param>
        /// <param name="type">考核类别类别</param>
        public void InsertAssessmentListDict(string name, string type)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO {0}.ASSESSMENT_LIST_DICT (ID,LIST_NAME,ASSESSMENT_CODE) VALUES ({1},'{2}','{3}')", DataUser.RLZY, OracleOledbBase.GetMaxID("ID", DataUser.RLZY + ".ASSESSMENT_LIST_DICT"), name, type);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }


        #endregion

        #region 考试类别字典
        /// <summary>
        /// 查询考试类别字典
        /// </summary>
        /// <returns></returns>
        public DataSet ViewExamScoreListDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT ID,EXAM_SCORE_CODE TYPE,LIST_NAME NAME FROM {0}.EXAM_SCORE_LIST_DICT ORDER BY ID", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 删除考试类别字典
        /// </summary>
        /// <param name="id">考试类别ID</param>
        public void DelExamScoreListDict(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM {0}.EXAM_SCORE_LIST_DICT WHERE ID={1}", DataUser.RLZY, id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 更新类别字典
        /// </summary>
        /// <param name="id">考试类别ID</param>
        /// <param name="name">考试类别名称</param>
        /// <param name="type">考试类别类别</param>
        public void UpdataExamScoreListDict(string id, string name, string type)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE {0}.EXAM_SCORE_LIST_DICT SET EXAM_SCORE_CODE='{2}', LIST_NAME = '{3}' WHERE ID={1}", DataUser.RLZY, id, type, name);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 添加类别字典
        /// </summary>
        /// <param name="name">考试类别名称</param>
        /// <param name="type">考试类别类别</param>
        public void InsertExamScoreListDict(string name, string type)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO {0}.EXAM_SCORE_LIST_DICT (ID,LIST_NAME,EXAM_SCORE_CODE) VALUES ({1},'{2}','{3}')", DataUser.RLZY, OracleOledbBase.GetMaxID("ID", DataUser.RLZY + ".EXAM_SCORE_LIST_DICT"), name, type);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        #endregion

        #region 特殊症疗项目字典

        /// <summary>
        /// 查询特殊症疗项目字典
        /// </summary>
        /// <returns></returns>
        public DataSet ViewEspeDiagItemDictDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT SERIAL_NO,DIAG_CODE,DIAG_NAME FROM {0}.ESPE_DIAG_ITEM_DICT ORDER BY SERIAL_NO", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取职务字典数据
        /// </summary>
        /// <returns></returns>
        public DataSet getDutyDict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select ID,DUTY,SUBSIDY,SORT_NO from RLZY.DUTY_DICT", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public DataSet getGangGan()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT BS, COSTS, ZCBL, CCBL FROM HISDATA.GANGGAN");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public DataSet getNot_MingDan()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT (SELECT USER_NAME FROM HISDATA.USERS WHERE DB_USER = A.USER_ID) USER_NAME,USER_ID  FROM HISDATA.NOT_JX A");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public DataSet getHS_GZL()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select ITEM_CODE,ITEM_NAME FROM HISDATA.HUSHI_GZL");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public DataSet getGL_XL()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT DEPT_CODE,
                                   (SELECT DEPT_NAME
                                      FROM HISDATA.DEPT_DICT
                                     WHERE DEPT_CODE = A.DEPT_CODE) DEPT_NAME,
                                   ITEM_CODE,
                                   (SELECT ITEM_NAME
                                      FROM HISDATA.PRICE_LIST
                                     WHERE ITEM_CODE = A.ITEM_CODE) ITEM_NAME,
                                   BL
                              FROM HISDATA.GULI_XM A");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public DataSet getINP_ZCF()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT DEPT_CODE, DEPT_NAME, USER_ID, USER_NAME FROM HISDATA.INP_ZCF");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 删除特殊症疗项目字典
        /// </summary>
        /// <param name="id">特殊症疗项目ID</param>
        public void DelEspeDiagItemDictDict(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM {0}.ESPE_DIAG_ITEM_DICT WHERE SERIAL_NO={1}", DataUser.RLZY, id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 更新特殊症疗项目字典
        /// </summary>
        /// <param name="id">特殊症疗项目ID</param>
        /// <param name="name">特殊症疗项目名称</param>
        /// <param name="code">特殊症疗项目CODE</param>
        public void UpdataEspeDiagItemDictDict(string id, string name, string code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE {0}.ESPE_DIAG_ITEM_DICT SET DIAG_CODE='{2}', DIAG_NAME = '{3}' WHERE SERIAL_NO={1}", DataUser.RLZY, id, code, name);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        /// <summary>
        /// 添加特殊症疗项目字典
        /// </summary>
        /// <param name="name">特殊症疗项目名称</param>
        /// <param name="code">特殊症疗项目CODE</param>
        public void InsertEspeDiagItemDictDict(string name, string code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO {0}.ESPE_DIAG_ITEM_DICT (SERIAL_NO,DIAG_NAME,DIAG_CODE) VALUES ({1},'{2}','{3}')", DataUser.RLZY, OracleOledbBase.GetMaxID("SERIAL_NO", DataUser.RLZY + ".ESPE_DIAG_ITEM_DICT"), name, code);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }


        public void InsertDutyDict(string duty, string subsidy,string sortno)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO RLZY.DUTY_DICT (ID,DUTY,SUBSIDY,SORT_NO) VALUES ({1},'{2}',{3},{4})", DataUser.RLZY, OracleOledbBase.GetMaxID("ID", DataUser.RLZY + ".DUTY_DICT"), duty, subsidy, sortno);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void InsertGangGan(string DictCode, string duty, string subsidy, string sortno)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO HISDATA.GANGGAN (BS,COSTS,ZCBL,CCBL) VALUES ('{0}','{1}','{2}','{3}')", DictCode, duty, subsidy, sortno);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void InsertNot_MingDan(string DictCode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO HISDATA.NOT_JX (USER_ID) VALUES ('{0}')", DictCode);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void InsertHS_GZL(string DictCode, string DictName)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO HISDATA.HUSHI_GZL (ITEM_CODE,ITEM_NAME) VALUES('{0}','{1}')", DictCode, DictName);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void InserGL_XM(string DictCode, string DictName,string BL)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO HISDATA.GULI_XM(dept_code,ITEM_CODE,BL) VALUES ('{0}','{1}','{2}')",DictName, DictCode, BL);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void InsertINP_ZCF(string DictCode, string DictName)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO HISDATA.INP_ZCF(DEPT_CODE, DEPT_NAME, USER_ID, USER_NAME) VALUES 
                                ((SELECT user_dept FROM hisdata.users WHERE db_user='{0}'),
                                (SELECT dept_name FROM hisdata.dept_dict WHERE dept_code=(SELECT user_dept FROM hisdata.users WHERE db_user='{0}')),
                                '{0}','{1}')", DictCode, DictName);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void UpdataDutyDict(string id, string duty, string subsidy, string sortno)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE {0}.DUTY_DICT SET DUTY='{2}', SUBSIDY = '{3}',SORT_NO={4} WHERE ID={1}", DataUser.RLZY, id, duty, subsidy, sortno);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void UpdataGangGan(string id, string duty, string subsidy, string sortno)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE  HISDATA.GANGGAN SET COSTS='{1}',ZCBL='{2}', CCBL='{3}' WHERE BS='{0}'", id,duty, subsidy, sortno);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void UpdataNot_MingDan(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE  HISDATA.NOT_JX SET USER_ID='{0}'", id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public DataSet GetAccoutCLASS_CODE()
        {
            StringBuilder strDel = new StringBuilder();

            string str = string.Format(@"SELECT CLASS_CODE,CLASS_NAME FROM HISDATA.RECK_ITEM_CLASS_DICT1");
            return OracleOledbBase.ExecuteDataSet(str);

        }
        public DataSet getDutyItem_Dict()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT ID,
                                      ITEM_CODE,
                                      ITEM_NAME,
                                      (SELECT CLASS_NAME
                                         FROM HISDATA.RECK_ITEM_CLASS_DICT1
                                        WHERE CLASS_CODE = A.CLASS_CODE) CLASS_NAME,
                                       A.CLASS_CODE
                                 FROM HISDATA.ITEM_DICT A");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public void InsertItem_Dict(string ITEM_CODE, string DEPT_NAME, string ITEM_NAME, string CLASS_CODE)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO HISDATA.ITEM_DICT
                                  (ID, ITEM_CODE, ITEM_NAME, CLASS_CODE)
                                VALUES
                                  (NVL((SELECT MAX(ID) FROM HISDATA.ITEM_DICT), 0) + 1,
                                   '{0}',
                                   '{1}',
                                   '{2}')", ITEM_CODE, ITEM_NAME, CLASS_CODE);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void UpdataIItem_Dict(string ITEM_CODE, string DEPT_NAME, string ITEM_NAME, string CLASS_CODE)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE HISDATA.ITEM_DICT SET CLASS_CODE='{1}' WHERE ITEM_CODE='{0}'", ITEM_CODE, CLASS_CODE);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void DelDutyItem_Dict(string ITEM_CODE)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM HISDATA.ITEM_DICT WHERE ITEM_CODE='{0}'", ITEM_CODE);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public string ChaXunItem_Dict(string ITEM_CODE)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT ITEM_NAME FROM HISDATA.PRICE_LIST WHERE ITEM_CODE='{0}'", ITEM_CODE);
            return OracleOledbBase.ExecuteNonQuery(str.ToString()).ToString();
        }
        public DataSet getDutyItem_Ohter()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT SERIAL_NO, CLASS_CODE, CLASS_NAME, INPUT_CODE FROM HISDATA.RECK_ITEM_CLASS_DICT1");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        public void InsertItem_Ohter(string CLASS_CODE, string DEPT_NAME, string CLASS_NAME, string INPUT_CODE)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO HISDATA.RECK_ITEM_CLASS_DICT1
                              (SERIAL_NO, CLASS_CODE, CLASS_NAME, INPUT_CODE, GROUP_NO, GROUP_NAME)
                            VALUES
                              ((SELECT MAX(SERIAL_NO) FROM HISDATA.RECK_ITEM_CLASS_DICT) + 1,
                               '{0}',
                               '{1}',
                               UPPER('{2}'),
                               '{1}',
                               '{2}')", CLASS_CODE, CLASS_NAME, INPUT_CODE);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void UpdataItem_Ohter(string CLASS_CODE, string DEPT_NAME, string CLASS_NAME, string INPUT_CODE)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE HISDATA.RECK_ITEM_CLASS_DICT1 SET CLASS_NAME='{1}',GROUP_NAME='{1}',INPUT_CODE= UPPER('{2}') WHERE CLASS_CODE='{0}'", CLASS_CODE, CLASS_NAME, INPUT_CODE);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void DelDutyItem_Ohter(string CLASS_CODE)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM HISDATA.RECK_ITEM_CLASS_DICT1 WHERE CLASS_CODE='{0}'", CLASS_CODE);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void DelDutyDict(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM {0}.DUTY_DICT WHERE ID={1}", DataUser.RLZY, id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void DelGangGan(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM HISDATA.GANGGAN WHERE BS='{0}'", id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void DelNot_MingDan(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM HISDATA.NOT_JX WHERE USER_ID='{0}'", id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public string Cha_HS_GZL(string DictCode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT ITEM_NAME FROM HISDATA.PRICE_LIST WHERE ITEM_CODE = '{0}'", DictCode);
            return OracleOledbBase.ExecuteScalar(str.ToString()).ToString();
            
        }
        public string Cha_GL_XM(string DictName)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT DEPT_NAME FROM HISDATA.DEPT_DICT WHERE DEPT_CODE = '{0}'", DictName);
            return OracleOledbBase.ExecuteScalar(str.ToString()).ToString();

        }
        public string Cha_INP_ZCF(string DictCode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT USER_NAME FROM HISDATA.USERS WHERE DB_USER = '{0}'", DictCode);
            return OracleOledbBase.ExecuteScalar(str.ToString()).ToString();
            //return OracleOledbBase.ExecuteNonQuery(str.ToString()).ToString();
        }
        public void DelHS_GZL(string DictCode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM HISDATA.HUSHI_GZL WHERE ITEM_CODE={0}", DictCode);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void DelGL_XM(string DictCode, string DictName)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM HISDATA.GULI_XM WHERE dept_code='{0}' AND item_code='{1}'", DictName,DictCode);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        public void DelINP_ZCF(string DictCode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM HISDATA.INP_ZCF WHERE USER_ID={0}", DictCode);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        #endregion

        #region 岗位字典
        public DataSet getStationTypeDictInfo() 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT ID,ATTRIBUE NAME FROM {0}.SYS_DEPT_ATTR_DICT ORDER BY SORTNO", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询岗位名称
        /// </summary>
        /// <param name="type">类别</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public DataSet getStationInfo(string type)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT ID,STATION_NAME NAME FROM {0}.SYS_STATION_MAINTENANCE_DICT WHERE STATION_TYPE_CODE = '{1}' ORDER BY STATION_NAME", DataUser.COMM,type);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        public DataSet getStationNameInfo(string deptCode) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT DISTINCT T1.ID, T1.STATION_NAME,T1.SEQUENCE
                                  FROM {0}.SYS_STATION_MAINTENANCE_DICT T1,
                                       {0}.SYS_DEPT_DICT T2
                                 WHERE T1.STATION_TYPE_CODE = T2.DEPT_TYPE
                                   AND T2.DEPT_CODE = '{1}' ORDER BY TO_NUMBER(T1.SEQUENCE)", DataUser.COMM, deptCode, DateTime.Now.Year.ToString());
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        #endregion

        #region 专科中心字典
        /// <summary>
        /// 查询专科中心字典
        /// </summary>
        /// <returns></returns>
        public DataSet getSpceCenterListDictInfo()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT ID,CENTER_CODE,CENTER_NAME FROM {0}.SPEC_CENTER_DICT", DataUser.RLZY);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 删除专科中心字典
        /// </summary>
        /// <param name="id">ID</param>
        public void DelSpceCenterDict(string id) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("DELETE FROM {0}.SPEC_CENTER_DICT WHERE ID={1}", DataUser.RLZY, id);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        /// <summary>
        /// 添加专科中心字典信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">代码</param>
        public void InsertSpceCenterDict(string name,string code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("INSERT INTO {0}.SPEC_CENTER_DICT (ID,CENTER_NAME,CENTER_CODE) VALUES ({1},'{2}','{3}')", DataUser.RLZY, OracleOledbBase.GetMaxID("ID", DataUser.RLZY + ".SPEC_CENTER_DICT"), name, code);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }
        /// <summary>
        /// 更新专科中心字典信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="code">代码</param>
        public void UpdateSpceCenterDict(string id,string name, string code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("UPDATE {0}.SPEC_CENTER_DICT SET CENTER_CODE='{2}', CENTER_NAME = '{3}' WHERE ID={1}", DataUser.RLZY, id, code, name);
            OracleOledbBase.ExecuteNonQuery(str.ToString());
        }

        #endregion

    }
}
