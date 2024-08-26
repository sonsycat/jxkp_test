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
    public class XyhsDict
    {
        public XyhsDict()
        {

        }
        #region 科室设置
        /// <summary>
        /// 效益核算科室类别
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetDeptType()
        {
            string strSql = string.Format("SELECT   ID, XYHS_DEPT_TYPE FROM {0}.XYHS_DEPT_TYPE_DICT where 1=1 order by id", DataUser.CBHS);// id!=0
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        public DataSet GetDiag()
        {
            string str = "select DIAGNOSIS_CODE,DIAGNOSIS_NAME  from cbhs.XYHS_DIAGNOSIS_DICT";
            return OracleOledbBase.ExecuteDataSet(str);
        }
        /// <summary>
        /// 查询获取科室列表
        /// </summary>
        /// <param name="deptType">科室列表编码</param>
        /// <param name="showFlag">停用标识</param>
        /// <returns>DataSet</returns>
        public DataSet GetDeptList(string deptType, string showFlag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT DEPT_CODE,
                                                      DEPT_NAME,
                                                    (SELECT XYHS_DEPT_TYPE FROM CBHS.XYHS_DEPT_TYPE_DICT C WHERE C.ID=A.DEPT_TYPE ) DEPT_TYPE,
                                                     A.ATTR,
                                                     INPUT_CODE,
                                                     A.ACCOUNT_DEPT_CODE,
                                                     A.ACCOUNT_DEPT_NAME,
                                                     A.SORT_NO,
                                                     A.SHOW_FLAG
                                              FROM   {0}.XYHS_DEPT_DICT A 
                                             WHERE   A.SHOW_FLAG = 0", DataUser.CBHS);
            if (deptType != null && deptType != "")
            {
                strSql.AppendFormat("  AND A.DEPT_TYPE={0}", deptType);
            }
            if (showFlag != null && showFlag != "")
            {
                strSql.AppendFormat(" AND NVL (A.SHOW_FLAG, 0)={0}", showFlag);
            }
            strSql.Append(" ORDER BY A.SORT_NO,A.DEPT_CODE");

            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        public DataSet GetDeptList(string deptType, string showFlag, string filter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT NVL (A.DEPT_CODE, B.DEPT_CODE) DEPT_CODE,
                                         NVL (A.DEPT_NAME, B.DEPT_NAME) DEPT_NAME,
                                        (SELECT XYHS_DEPT_TYPE FROM CBHS.XYHS_DEPT_TYPE_DICT C WHERE C.ID=A.DEPT_TYPE ) DEPT_TYPE,
                                         A.ATTR,a.PATIENT_DEPT_FLAGS,a.COM_DEPT_FLAGS,
                                         NVL (A.INPUT_CODE, B.INPUT_CODE) INPUT_CODE,
                                         A.ACCOUNT_DEPT_CODE,
                                         A.ACCOUNT_DEPT_NAME,
                                         NVL(A.SORT_NO,0) SORT_NO,
                                         A.SHOW_FLAG
                                  FROM   {0}.XYHS_DEPT_DICT A, {1}.SYS_DEPT_DICT B
                                 WHERE   A.DEPT_CODE(+) = B.DEPT_CODE AND B.SHOW_FLAG = 0", DataUser.CBHS, DataUser.COMM);
            if (deptType != null && deptType != "")
            {
                strSql.AppendFormat("  AND A.DEPT_TYPE={0}", deptType);
            }
            if (showFlag != null && showFlag != "")
            {
                strSql.AppendFormat(" AND NVL (A.SHOW_FLAG, 0)={0}", showFlag);
            }
            if (filter != null && filter != "")
            {
                strSql.AppendFormat(@" AND (NVL (A.DEPT_CODE, B.DEPT_CODE) LIKE '{0}%' 
                                         OR NVL (A.DEPT_NAME, B.DEPT_NAME) LIKE '{0}%' 
                                         OR NVL (A.INPUT_CODE, B.INPUT_CODE) LIKE '{0}%')",filter);
            }
            strSql.Append(" ORDER BY NVL(A.SORT_NO,B.SORT_NO),NVL (A.DEPT_CODE, B.DEPT_CODE)");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        public DataSet GetDeptList(string deptCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT NVL (A.DEPT_CODE, B.DEPT_CODE) DEPT_CODE,
                                         NVL (A.DEPT_NAME, B.DEPT_NAME) DEPT_NAME,
                                         A.DEPT_TYPE,
                                         A.ATTR,
                                         NVL (A.INPUT_CODE, B.INPUT_CODE) INPUT_CODE,
                                         A.ACCOUNT_DEPT_CODE,
                                         A.ACCOUNT_DEPT_NAME,
                                         NVL(A.SORT_NO,B.SORT_NO) SORT_NO,
                                         NVL(A.SHOW_FLAG,0) SHOW_FLAG,
                                         NVL(A.PATIENT_DEPT_FLAGS,0) PATIENT_DEPT_FLAGS,
                                         NVL(A.COM_DEPT_FLAGS,0) COM_DEPT_FLAGS
                                  FROM   {0}.XYHS_DEPT_DICT A, {1}.SYS_DEPT_DICT B
                                 WHERE   A.DEPT_CODE(+) = B.DEPT_CODE AND B.SHOW_FLAG = 0", DataUser.CBHS, DataUser.COMM);
            if (deptCode != null && deptCode != "")
            {
                strSql.AppendFormat(" AND NVL (A.DEPT_CODE, B.DEPT_CODE) ='{0}'", deptCode);
            }
            strSql.Append(" ORDER BY NVL(A.SORT_NO,B.SORT_NO),NVL (A.DEPT_CODE, B.DEPT_CODE)");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 保存科室设置
        /// </summary>
        /// <param name="dept_code">科室编码</param>
        /// <param name="dept_name">科室名称</param>
        /// <param name="dept_type">科室类别</param>
        /// <param name="attr">是否核算</param>
        /// <param name="input_code">输入码</param>
        /// <param name="account_dept_code">核算科室代码</param>
        /// <param name="account_dept_name">核算科室名称</param>
        /// <param name="sort_no">排列序号</param>
        /// <param name="show_flag">是否停用</param>
        public void SaveDept(string dept_code, string dept_name, string dept_type,
                             string attr, string input_code, string account_dept_code,
                             string account_dept_name, string sort_no, string show_flag)
        {
            string sql = string.Format("SELECT * FROM {0}.XYHS_DEPT_DICT WHERE DEPT_CODE='{1}'", DataUser.CBHS, dept_code);
            DataTable dt = OracleOledbBase.ExecuteDataSet(sql).Tables[0];
            StringBuilder strSql = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                //更新数据
                strSql.AppendFormat(@"UPDATE {0}.XYHS_DEPT_DICT
                                        SET DEPT_NAME = ?,
                                            DEPT_TYPE = ?,
                                            ATTR = ?,
                                            INPUT_CODE = ?,
                                            ACCOUNT_DEPT_CODE = ?,
                                            ACCOUNT_DEPT_NAME = ?,
                                            SORT_NO = ?,
                                            SHOW_FLAG = ?
                                        WHERE DEPT_CODE = ?", DataUser.CBHS);
                OleDbParameter[] parameter = {
											  new OleDbParameter("dept_name",dept_name),
											  new OleDbParameter("dept_type",dept_type),
											  new OleDbParameter("attr",attr),
											  new OleDbParameter("input_code",input_code),
											  new OleDbParameter("account_dept_code",account_dept_code),
											  new OleDbParameter("account_dept_name",account_dept_name),
											  new OleDbParameter("sort_no",sort_no),
											  new OleDbParameter("show_flag",show_flag),
											  new OleDbParameter("dept_code",dept_code)
                                              };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);

            }
            else
            {
                //添加数据
                strSql.AppendFormat(@"INSERT INTO {0}.XYHS_DEPT_DICT(ID,DEPT_CODE,DEPT_NAME,DEPT_TYPE,ATTR,INPUT_CODE,ACCOUNT_DEPT_CODE,
                                                                     ACCOUNT_DEPT_NAME,SORT_NO,SHOW_FLAG)
                                                    VALUES(?,?,?,?,?,?,?,?,?,?)", DataUser.CBHS);
                string maxIdSql = string.Format("SELECT NVL(MAX(ID),0)+1 id FROM {0}.XYHS_DEPT_DICT ", DataUser.CBHS);
                string id = OracleOledbBase.ExecuteDataSet(maxIdSql).Tables[0].Rows[0]["id"].ToString();
                OleDbParameter[] parameter = {
											  new OleDbParameter("id",id),
											  new OleDbParameter("dept_code",dept_code),
											  new OleDbParameter("dept_name",dept_name),
											  new OleDbParameter("dept_type",dept_type),
											  new OleDbParameter("attr",attr),
											  new OleDbParameter("input_code",input_code),
											  new OleDbParameter("account_dept_code",account_dept_code),
											  new OleDbParameter("account_dept_name",account_dept_name),
											  new OleDbParameter("sort_no",sort_no),
											  new OleDbParameter("show_flag",show_flag)
                                              };
                OracleOledbBase.ExecuteNonQuery(strSql.ToString(), parameter);
            }
        }
        /// <summary>
        /// 删除科室
        /// </summary>
        /// <param name="dept_code">科室代码</param>
        public void DelDept(string dept_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"DELETE FROM  {0}.XYHS_DEPT_DICT WHERE DEPT_CODE='{1}'", DataUser.CBHS, dept_code);
            OracleOledbBase.ExecuteNonQuery(strSql.ToString());

        }

        public DataTable GetItemTypeSfyl()
        {
            string sql = @"    SELECT   ITEM_CODE,
             ITEM_NAME,
             CONNECT_BY_ROOT ITEM_NAME ROOT,
             RPAD (' ', 4 * (LEVEL - 1), '-') || ITEM_NAME DEPNAME,
             SFYL
      FROM   CBHS.XYHS_COSTS_DICT
START WITH   CLASSPID = '0'
CONNECT BY   PRIOR ITEM_CODE = CLASSPID";

            return OracleOledbBase.ExecuteDataSet(sql).Tables[0];
        }

        public void Saveitemtypesfyl(Dictionary<string, string>[] selectRow)
        {
            MyLists listtable = new MyLists();

            List listcenterdict = new List();
            for (int i = 0; i < selectRow.Length; i++)
            {
                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"update CBHS.XYHS_COSTS_DICT set SFYL='{0}' where ITEM_CODE='{1}' ", selectRow[i]["SFYL"].ToString(), selectRow[i]["ITEM_CODE"].ToString());

                List listcenterdetail = new List();
                listcenterdetail.StrSql = str.ToString();
                listcenterdetail.Parameters = new OleDbParameter[] { };
                listtable.Add(listcenterdetail);

            }

            OracleOledbBase.ExecuteTranslist(listtable);

        }


        #endregion 科室设置

        #region 科室信息
        /// <summary>
        /// 获取分解方案
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetProgDict()
        {
            string strSql = string.Format(@"SELECT PROG_CODE, PROG_NAME 
                                             FROM {0}.CBHS_COST_APPOR_PROG_DICT WHERE FLAGS = '3' 
                                          ORDER BY PROG_CODE", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql);
        }
        /// <summary>
        /// 全成本字典
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public DataTable GetAllcost(string itemcode)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select * from (
SELECT   a.PROG_TYPE,b.item_code, b.item_name,  d.prog_code, d.prog_name
    FROM {0}.xyhs_cost_prog_dict a,
         (
            SELECT   NVL (b.item_code, A.ITEM_CODE) AS item_code,
                     NVL (b.item_name, A.ITEM_NAME) AS ITEM_NAME,
                     b.ITEM_TYPE
              FROM      {0}.CBHS_COST_ITEM_DICT a
                     FULL JOIN
                        {0}.XYHS_COST_ITEM_DICT b
                     ON a.item_code = B.ITEM_CODE
         ) b,
         {0}.xyhs_cost_appor_prog_dict d
   WHERE a.item_code = b.item_code
     AND a.prog_code = d.prog_code
     and b.item_code='{1}')m
     full join {0}.xyhs_dept_type_dict n
     on PROG_TYPE=N.ID where n.flags=0
     order by n.id", DataUser.CBHS, itemcode);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }
        public void SaveAllcostsdict(Dictionary<string, string>[] selectRow, string itemcode)
        {
            bool flags = true;
            string delsql = string.Format("delete from  {0}.XYHS_COST_PROG_DICT where item_code='{1}'", DataUser.CBHS, itemcode);

            MyLists listtable = new MyLists();
            //删除
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);
            for (int i = 0; i < selectRow.Length; i++)
            {
                string strselect = string.Format("SELECT prog_code FROM {0}.XYHS_COST_APPOR_PROG_DICT where prog_name='{1}'", DataUser.CBHS, selectRow[i]["PROG_NAME"].ToString());
                string progname = OracleOledbBase.ExecuteScalar(strselect.ToString()).ToString();

                StringBuilder str = new StringBuilder();
                str.AppendFormat("insert into {0}.XYHS_COST_PROG_DICT( ", DataUser.CBHS);
                str.Append(" ITEM_CODE,PROG_TYPE,PROG_CODE ) values (");
                str.AppendFormat("'{0}','{1}','{2}')", itemcode, selectRow[i]["ID"].ToString(), progname);
                List listcenterdetail = new List();
                listcenterdetail.StrSql = str.ToString();
                listcenterdetail.Parameters = new OleDbParameter[] { };
                listtable.Add(listcenterdetail);
                if (selectRow[i]["PROG_NAME"].ToString() == "")
                {
                    flags = false;
                }
            }
            if (flags == true)
            {
                OracleOledbBase.ExecuteTranslist(listtable);
            }
        }
        /// <summary>
        /// 查询科室基本信息
        /// </summary>
        /// <param name="date_time">日期（201012）</param>
        /// <returns>DataSet</returns>
        public DataSet GetDeptInfo(string date_time)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT NVL (A.DEPT_CODE, B.DEPT_CODE) DEPT_CODE,
                                         NVL (A.DEPT_NAME, B.DEPT_NAME) DEPT_NAME,
                                         NVL (A.DEPT_TYPE, B.DEPT_TYPE) DEPT_TYPE,
                                         NVL (A.INPUT_CODE, B.INPUT_CODE) INPUT_CODE,
                                         NVL (A.SORT_NO, B.SORT_NO) SORT_NO,
                                         A.DEPT_PERSON_COUNT,
                                         A.DEPT_AREA,
                                         A.DEPT_EQUIPMENT_COUNT,
                                         A.DEC_SCHEME
                                  FROM   {0}.XYHS_DEPT_INFO A, {0}.XYHS_DEPT_DICT B
                                 WHERE   A.DEPT_CODE(+) = B.DEPT_CODE AND B.ATTR = '是'
                                    and to_char(a.date_time(+),'yyyymm')='{1}'
                                    order by NVL (A.SORT_NO, B.SORT_NO),NVL (A.DEPT_CODE, B.DEPT_CODE)", DataUser.CBHS, date_time);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 保存科室信息
        /// </summary>
        /// <param name="deptinfos">科室信息model</param>
        /// <param name="date_time">日期（201012）</param>
        public void SaveDeptInfo(List<GoldNet.Model.YxhsDeptInfo> deptinfos, string date_time)
        {
            MyLists listtable = new MyLists();
            List listDel = new List();
            string sql = string.Format("DELETE FROM {0}.XYHS_DEPT_INFO WHERE TO_CHAR(DATE_TIME,'yyyymm')='{1}'", DataUser.CBHS, date_time);
            OleDbParameter[] parameterdel = new OleDbParameter[] { };
            listDel.StrSql = sql;
            listDel.Parameters = parameterdel;
            listtable.Add(listDel);
            int id = 0;
            foreach (GoldNet.Model.YxhsDeptInfo deptinfo in deptinfos)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"INSERT INTO {0}.XYHS_DEPT_INFO(ID,DEPT_CODE,DEPT_NAME,DEPT_TYPE,INPUT_CODE,SORT_NO,
                                                                     DEPT_PERSON_COUNT,DEPT_AREA,DEPT_EQUIPMENT_COUNT,DEC_SCHEME,DATE_TIME)
                                                    VALUES(?,?,?,?,?,?,?,?,?,?,TO_DATE(?,'yyyymm'))", DataUser.CBHS);
                OleDbParameter[] parameteradd = new OleDbParameter[] {
											  new OleDbParameter("id",id), 
											  new OleDbParameter("dept_code",deptinfo.Dept_code),
											  new OleDbParameter("dept_name",deptinfo.Dept_name),
											  new OleDbParameter("dept_type",deptinfo.Dept_type),
											  new OleDbParameter("input_code",deptinfo.Input_code),
											  new OleDbParameter("sort_no",deptinfo.Sort_no),
											  new OleDbParameter("dept_person_count",deptinfo.Dept_person_count),
											  new OleDbParameter("dept_area",deptinfo.Dept_area),
											  new OleDbParameter("dept_equipment_count",deptinfo.Dept_equipment_count),
											  new OleDbParameter("dec_scheme",deptinfo.Dec_scheme),
											  new OleDbParameter("date_time",date_time)};
                List listAdd = new List();
                listAdd.StrSql = strSql;
                listAdd.Parameters = parameteradd;
                listtable.Add(listAdd);
                id++;
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }
        #endregion 科室信息

        #region 成本项目设置(新添加-Sunyc)


        /// <summary>
        /// 分析报表类别初始化树结构
        /// </summary>
        /// <returns></returns>
        public DataSet GetCostType()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"    SELECT   '0' ITEM_CODE,
         '项目类别' AS ITEM_NAME,
         NULL AS CLASSPID,
         1 LEV,
         1 ISLEAF
  FROM   DUAL
UNION ALL
    SELECT   ITEM_CODE,
             ITEM_NAME,
             CLASSPID,
             LEVEL LEV,
             CONNECT_BY_ISLEAF ISLEAF
      FROM   {0}.XYHS_COSTS_DICT
START WITH   CLASSPID = '0'
CONNECT BY   PRIOR ITEM_CODE = CLASSPID", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());


        }

        public DataSet GetCosttype()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
    SELECT   ITEM_CODE,
             ITEM_NAME,
             CLASSPID,
             LEVEL LEV,
             CONNECT_BY_ISLEAF ISLEAF
      FROM   {0}.XYHS_COSTS_DICT
START WITH   CLASSPID = '0'
CONNECT BY   PRIOR ITEM_CODE = CLASSPID", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
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
                l_listDelSql.StrSql = string.Format(" DELETE FROM {0}.XYHS_COSTS_DICT ", DataUser.CBHS);
                list.Add(l_listDelSql);

                ConvertPY convert = new ConvertPY();
                StringBuilder sqlstr = new StringBuilder();
                string sql = "";
                string classtxt = "";
                for (int i = 0; i < treeNodes.Length; i++)
                {
                    classtxt = treeNodes[i]["Text"].Length > 30 ? treeNodes[i]["Text"].Substring(1, 30) : treeNodes[i]["Text"];
                    sqlstr.AppendFormat(" SELECT  '{0}' AS ITEM_CODE, '{1}' AS ITEM_NAME ,{2} AS CLASSPID FROM DUAL UNION ALL "
                        , treeNodes[i]["Id"]
                        , classtxt.Replace("'", "''")
                        , "'" + treeNodes[i]["Pid"] + "'"
                         );
                }
                sql = sqlstr.ToString() + ")";
                sql = sql.Replace("UNION ALL )", "");
                sql = string.Format(" INSERT INTO {0}.XYHS_COSTS_DICT  (ITEM_CODE, ITEM_NAME, CLASSPID) ", DataUser.CBHS) + sql;

                List l_listUpdate = new List();
                l_listUpdate.Parameters = new OleDbParameter[] { new OleDbParameter() };
                l_listUpdate.StrSql = sql;
                list.Add(l_listUpdate);
                OracleOledbBase.ExecuteTranslist(list);
            }
        }

        /// <summary>
        /// 删除报表类别
        /// </summary>class
        /// <param name="id">报表类别ID</param>
        public void DelCostType(string id)
        {
            string sql = string.Format(@"delete {1}.XYHS_COSTS_DICT where ITEM_CODE = '{0}'", id, DataUser.CBHS);

            OracleOledbBase.ExecuteNonQuery(sql);
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
            string sql = string.Format("select count(*) from {1}.XYHS_COST_ITEM_DICT where ITEM_TYPE = '{0}'", id, DataUser.CBHS);
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
            string sql = string.Format("select count(*) from {1}.XYHS_COSTS_DICT where CLASSPID = '{0}'", id, DataUser.CBHS);
            num = Convert.ToInt32(OracleOledbBase.GetSingle(sql, pram));
            return num > 0 ? false : true;
        }

        /// <summary>
        /// 获取成本项目列表
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetCostItem()
        {
            string sql = string.Format(@"SELECT   b.ITEM_CODE,
         b.ITEM_NAME,
         (SELECT   a.ITEM_CODE || ':' || a.ITEM_NAME
            FROM   {0}.XYHS_COSTS_DICT a
           WHERE   a.ITEM_CODE = b.ITEM_TYPE)
            AS ITEM_TYPE,
         b.COST_TYPE,
         b.INPUT_CODE
  FROM   {0}.XYHS_COST_ITEM_DICT b", DataUser.CBHS);

            return OracleOledbBase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取分解方案字典
        /// </summary>
        /// <returns></returns>
        public DataSet GetProgdict(string flags)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT ID, prog_code, prog_name, input_code, flags,APPLY_DEPT FROM {0}.XYHS_COST_APPOR_PROG_DICT", DataUser.CBHS);
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());
        }

        #endregion 成本项目设置

    }
}
