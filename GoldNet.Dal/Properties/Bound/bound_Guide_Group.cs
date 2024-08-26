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

namespace Goldnet.Dal.Properties.Bound
{
    public class bound_Guide_Group
    {
        public bound_Guide_Group() { }

        /// <summary>
        /// 查询指标集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetGuideGroup(string where)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                SELECT  GUIDE_GATHER_CODE,GUIDE_GATHER_NAME from {0}.SET_GUIDE_GATHER_CLASS ", DataUser.PERFORMANCE);
            if (where != "")
            {
                str.Append(" WHERE " + where);
            }
            str.Append(" ORDER BY GUIDE_GATHER_CODE ");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 删除指标集
        /// </summary>
        /// <param name="id"></param>
        public string DelGuideGroupByid(string id)
        {

            string strdel = string.Format(" DELETE FROM {1}.SET_GUIDE_GATHER_CLASS WHERE GUIDE_GATHER_CODE='{0}'", id, DataUser.PERFORMANCE);
            string strdelguide = string.Format(" DELETE FROM {1}.SET_GUIDE_GATHERS WHERE GUIDE_GATHER_CODE='{0}'", id, DataUser.PERFORMANCE);

            OracleOledbBase.ExecuteNonQuery(strdel);
            OracleOledbBase.ExecuteNonQuery(strdelguide);
            return "";
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
        /// 更新指标集
        /// </summary>
        /// <param name="selectedid"></param>
        /// <param name="guidegroupname"></param>
        /// <param name="guidegroupclass"></param>
        /// <param name="organclass"></param>
        /// <param name="evaluationyear"></param>
        /// <returns></returns>
        public string UpdateGuideGroup(string selectedid, string guidegroupname)
        {
            string sqlstr = "";
            if (selectedid.Equals(""))
            {
                sqlstr = string.Format(@"
                    INSERT INTO {0}.SET_GUIDE_GATHER_CLASS(GUIDE_GATHER_CODE,GUIDE_GATHER_NAME)
                    SELECT (SELECT NVL(MAX(GUIDE_GATHER_CODE),0) + 1 FROM {0}.SET_GUIDE_GATHER_CLASS ),'{1}' FROM DUAL"
               , DataUser.PERFORMANCE , guidegroupname);
            }
            else
            {
                sqlstr = string.Format(@" UPDATE {0}.SET_GUIDE_GATHER_CLASS SET GUIDE_GATHER_NAME = '{2}'  WHERE GUIDE_GATHER_CODE = '{1}'"
                    , DataUser.PERFORMANCE,selectedid, guidegroupname);
            }
            OracleOledbBase.ExecuteNonQuery(sqlstr);
            return "";
        }

        /// <summary>
        /// 获取奖金二次分配待选人员指标（根据指标集ID）
        /// </summary>
        /// <param name="guide_gather_code"></param>
        /// <returns></returns>
        public DataSet GetGuideDictListByGatherCode(string  guide_gather_code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                SELECT A.GUIDE_CODE,  A.GUIDE_NAME, A.GUIDE_NAME  GUIDE_NAME_QZ
                  FROM {0}.GUIDE_NAME_DICT A
                 WHERE GUIDE_TYPE='奖金' and ORGAN in( '03','04') and a.guide_code not in
    (select guide_code from performance.SET_GUIDE_GATHERS where GUIDE_GATHER_CODE='{1}') ", DataUser.HOSPITALSYS, guide_gather_code);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 根据指标集查找指标
        /// </summary>
        /// <param name="guide_gather_code"></param>
        /// <returns></returns>
        public DataSet GetGuideGroupListByGatherCode(string guide_gather_code)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                  SELECT   A.GUIDE_CODE,
           B.GUIDE_NAME,
           B.GUIDE_NAME || '(' || A.QZ || ')' GUIDE_NAME_QZ
    FROM   {0}.SET_GUIDE_GATHERS A, {1}.GUIDE_NAME_DICT B
   WHERE   A.GUIDE_CODE = B.GUIDE_CODE AND A.GUIDE_GATHER_CODE = '{2}'
ORDER BY   A.GUIDE_CODE ", DataUser.PERFORMANCE, DataUser.HOSPITALSYS, guide_gather_code);
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
            ArrayList SQLStringList = new ArrayList();
            string sqlstr = string.Format(" DELETE FROM {0}.SET_GUIDE_GATHERS WHERE GUIDE_GATHER_CODE = '{1}'", DataUser.PERFORMANCE, guidegatherid);
            SQLStringList.Add(sqlstr);
            if (selectedRow.Length > 0)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@" INSERT INTO {0}.SET_GUIDE_GATHERS (GUIDE_GATHER_CODE,GUIDE_CODE,QZ ) ", DataUser.PERFORMANCE);
                string guide_code = "";
                string qz = "";
                string guide_name = "";
                for (int i = 0; i < selectedRow.Length; i++)
                {
                    guide_code = selectedRow[i]["GUIDE_CODE"].Trim();
                    guide_name = selectedRow[i]["GUIDE_NAME"].Trim();
                    qz = selectedRow[i]["GUIDE_NAME_QZ"].Trim().Replace(guide_name, "").Replace("(", "").Replace(")", "");
                    qz = qz.Equals("") ? "1" : qz;
                    sql.AppendFormat(@" SELECT  '{0}' AS GUIDE_GATHER_CODE,'{1}' AS GUIDE_CODE, {2} AS QZ FROM DUAL UNION ALL ", guidegatherid, guide_code, qz);
                }
                sql.AppendFormat("END");
                SQLStringList.Add(sql.ToString().Replace("UNION ALL END", ""));
            }
            OracleBase.ExecuteSqlTran(SQLStringList);
            return resultstr;
        }

        /// <summary>
        /// 获取科室指标集
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable GetGuideDept(string year, string month)
        {

            string date = BuildDate(year, month);

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   a.dept_code deptcode,
         b.dept_name deptname,
         c.GUIDE_GROUP_CODE GUIDE_GROUP_CODE
  FROM   (SELECT   A.dept_code
            FROM   PERFORMANCE.set_accountdepttype A,
                   PERFORMANCE.SET_ACCOUNTTYPEGROUP B
           WHERE       A.DEPT_TYPE = B.TYPE_CODE
                   AND A.st_date = TO_DATE ('{0}', 'yyyymmdd')
                ) a,
         (SELECT   *
            FROM   comm.sys_dept_info
           WHERE   dept_snap_date = TO_DATE ('{0}', 'yyyymmdd')) b,
         (SELECT   *
            FROM   PERFORMANCE.SET_GUIDE_DEPT
           WHERE   st_date = TO_DATE ('{0}', 'yyyymmdd')) c
 WHERE   a.dept_code = b.dept_code AND a.dept_code = c.dept_code(+)", date);

            DataSet dsPercent = OracleOledbBase.ExecuteDataSet(str.ToString());

            return dsPercent.Tables[0];
        }

        private string BuildDate(string year, string month)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            return year + "" + month + "" + "01";
        }


        /// <summary>
        /// 删除1个月核算科室提成比，添加1个月的核算科室提成比
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool SaveGuideDept(Dictionary<string, string>[] rows, string year, string month)
        {
            if (year == "" || month == "")
            {
                return false;
            }
            string date = BuildDate(year, month);
            string delsql = "delete from  PERFORMANCE.SET_GUIDE_DEPT  where  st_date =TO_DATE ('" + date + "', 'yyyymmdd') ";
            MyLists listtable = new MyLists();
            //删除科室类别
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);


            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i]["GUIDE_GROUP_CODE"] == null | rows[i]["GUIDE_GROUP_CODE"]=="")
                {
                    continue;
                }
                StringBuilder isql = new StringBuilder();
                isql.Append(" insert into PERFORMANCE.SET_GUIDE_DEPT (ST_DATE,DEPT_CODE,GUIDE_GROUP_CODE) values (");
                isql.Append("to_date('" + date + "','yyyymmdd')");
                isql.Append(",");
                isql.Append("'" + rows[i]["DEPTCODE"].ToString() + "'");
                isql.Append(",");
                isql.Append("'" + rows[i]["GUIDE_GROUP_CODE"].ToString() + "'");
                isql.Append(") ");

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
