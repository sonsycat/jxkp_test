using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm;
using System.Data.OleDb;
using GoldNet.Comm.DAL.Oracle;

namespace Goldnet.Dal
{
    public class GuideDalDict
    {

        public GuideDalDict()
		{ }

        #region 成员方法

        /// <summary>
        /// 人员部门CODE
        /// </summary>
        /// <param name="inputcode">部门输入编码</param>
        /// <returns>部门信息</returns>
        public DataSet GetStaffDept(string inputcode)
        {
            string strSql = string.Format("select DISTINCT A.DEPT_CODE, a.dept_name,a.input_code from {0}.SYS_DEPT_DICT a,{1}.NEW_STAFF_INFO B WHERE A.DEPT_CODE = B.DEPT_CODE AND B.ADD_MARK = '1' AND a.input_code like ? and show_flag = '0' order by a.DEPT_CODE", DataUser.COMM, DataUser.RLZY);
            OleDbParameter[] cmdPara = new OleDbParameter[] { new OleDbParameter("", inputcode.ToUpper() + "%") };
            return OracleOledbBase.ExecuteDataSet(strSql, cmdPara);
        }

        /// <summary>
        /// 根据科室条件获取人力资源科室代码、名称
        /// </summary>
        /// <param name="inputcode">输入码</param>
        /// <param name="deptfilter">科室过滤条件</param>
        /// <returns>DataSet</returns>
        public DataSet GetStaffDept(string inputcode, string deptfilter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select DISTINCT A.DEPT_CODE, a.dept_name,a.input_code 
                                              from {0}.SYS_DEPT_DICT a,{1}.NEW_STAFF_INFO B 
                                             WHERE A.DEPT_CODE = B.DEPT_CODE 
                                               AND B.ADD_MARK = '1' AND a.input_code like '{2}%' and show_flag = '0'", DataUser.COMM, DataUser.RLZY, inputcode.ToUpper());
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


        /// <summary>
        /// 删除报表指标
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <returns>删除报表指标相关SQL</returns>
        public string DeleteReportGuide(string rptid)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("delete from {0}.REPORTINFO where REPORT_CODE = ",DataUser.HOSPITALSYS);

            str.Append(rptid);

            return str.ToString();
        }

        /// <summary>
        /// 删除分析报表指标
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <returns>删除报表指标相关SQL</returns>
        public string DeleteAnaylseReportGuide(string rptid)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("delete from {0}.ANALYSISREPORTINFO where REPORT_CODE = ", DataUser.HOSPITALSYS);

            str.Append(rptid);

            return str.ToString();
        }

        /// <summary>
        /// 添加分析报表指标信息
        /// </summary>
        /// <param name="dr">指标集合</param>
        /// <param name="reportid">报表ID</param>
        /// <returns>添加报表指标信息SQL</returns>
        public string UpdateReportGuide(DataRow dr, string reportid)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(" INSERT INTO {3}.REPORTINFO(REPORT_CODE,GUIDE_CODE,RANKING) VALUES('{0}','{1}',{2})", reportid, dr["VALUE"].ToString(), dr["INDEX"].ToString(), DataUser.HOSPITALSYS);

            return str.ToString();
        }

        /// <summary>
        /// 添加报表指标信息
        /// </summary>
        /// <param name="dr">指标集合</param>
        /// <param name="reportid">报表ID</param>
        /// <returns>添加报表指标信息SQL</returns>
        public string UpdateAnalyseReportGuide(DataRow dr, string reportid)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(" INSERT INTO {3}.ANALYSISREPORTINFO(REPORT_CODE,GUIDE_CODE,RANKING) VALUES('{0}','{1}',{2})", reportid, dr["VALUE"].ToString(), dr["INDEX"].ToString(), DataUser.HOSPITALSYS);

            return str.ToString();
        }

        /// <summary>
        /// 更新报表指标信息
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <param name="l_dt">指标集合</param>
        public void UpdateReportGuideInfo(string rptid, DataTable l_dt)
        {
            MyLists list = new MyLists();

            List l_listDel = new List();

            l_listDel.Parameters = new OleDbParameter[] { new OleDbParameter() };

            l_listDel.StrSql = DeleteReportGuide(rptid);

            list.Add(l_listDel);

            for (int i = 0; i < l_dt.Rows.Count; i++)
            {
                List insert = new List();

                insert.Parameters = new OleDbParameter[] { new OleDbParameter() };

                insert.StrSql = UpdateReportGuide(l_dt.Rows[i], rptid);
                list.Add(insert);
            }

            OracleOledbBase.ExecuteTranslist(list);

        }

        /// <summary>
        /// 更新报表指标信息
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <param name="l_dt">指标集合</param>
        public void UpdateAnalyseReportGuideInfo(string rptid, DataTable l_dt) 
        {
            MyLists list = new MyLists();

            List l_listDel = new List();

            l_listDel.Parameters = new OleDbParameter[] { new OleDbParameter() };

            l_listDel.StrSql = DeleteAnaylseReportGuide(rptid);

            list.Add(l_listDel);

            for (int i = 0; i < l_dt.Rows.Count;i++ )
            {
                List insert = new List();

                insert.Parameters = new OleDbParameter[] { new OleDbParameter() };

                insert.StrSql = UpdateAnalyseReportGuide(l_dt.Rows[i], rptid);
                list.Add(insert);
            }

            OracleOledbBase.ExecuteTranslist(list);
            
        }

        /// <summary>
        /// 指标名称
        /// </summary>
        /// <returns>指标集合</returns>
        public DataSet GuideName()
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("select guide_code,guide_name from {0}.guide_name_dict",DataUser.HOSPITALSYS);

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 通过报表ID得到指标CODE
        /// </summary>
        /// <returns>指标集合</returns>
        public DataSet getGuideCodeByReportid(string report_code) 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("select GUIDE_CODE from {1}.REPORTINFO where REPORT_CODE='{0}' order by ranking", report_code,DataUser.HOSPITALSYS);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report_code"></param>
        /// <returns></returns>
        public DataSet getGuideIshighGuideByReportid(string report_code) 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT T1.GUIDE_CODE,T2.ISHIGHGUIDE
                                    FROM (SELECT GUIDE_CODE, RANKING
                                            FROM {1}.REPORTINFO
                                           WHERE REPORT_CODE = '{0}') T1,
                                         {1}.GUIDE_NAME_DICT T2
                                   WHERE T1.GUIDE_CODE = T2.GUIDE_CODE
                                ORDER BY T1.RANKING", report_code, DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        
        /// <summary>
        /// 查询指标明细SQL
        /// </summary>
        /// <param name="GuideCode">指标</param>
        /// <returns></returns>
        public DataSet getGuideExpressions(string GuideCode) 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("SELECT GUIDE_CODE,GUIDE_SQL_DETAIL from {1}.GUIDE_EXPRESSIONS where GUIDE_CODE='{0}'", GuideCode, DataUser.HOSPITALSYS);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 查询指标明细内容
        /// </summary>
        /// <param name="DetailSql">指标明细SQL</param>
        /// <param name="GuideCode">指标</param>
        /// <param name="unit_code">CODE</param>
        /// <param name="FromData">开始时间</param>
        /// <param name="ToData">结束时间</param>
        /// <returns></returns>
        public DataSet getGuideExpressionsDetail(string DetailSql,string GuideCode,string unit_code,string FromData,string ToData) 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(DetailSql, GuideCode, unit_code, FromData, ToData);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 设置报表已选指标
        /// </summary>
        /// <returns></returns>
        public DataSet getReportGuideExCol(string rptid)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT b.guide_code as VALUE, b.guide_name as TEXT ");
            sql.Append("FROM hospitalsys.reportinfo a, hospitalsys.guide_name_dict b ");
            sql.Append("WHERE a.guide_code = b.guide_code and  report_code=");
            sql.Append(rptid);
            sql.Append(" order by a.RANKING");
            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }

        /// <summary>
        /// 通过分析报表ID得到指标CODE
        /// </summary>
        /// <returns>指标集合</returns>
        public DataSet getAnalyseReportGuideCodeByReportid(string report_code)
        {
            StringBuilder str = new StringBuilder();
            str.Append("SELECT b.guide_code as VALUE, b.guide_name as TEXT ");
            str.AppendFormat("FROM {0}.ANALYSISREPORTINFO a, {0}.guide_name_dict b ",DataUser.HOSPITALSYS);
            str.Append("WHERE a.guide_code = b.guide_code and  report_code=");
            str.Append(report_code);
            str.Append(" order by a.RANKING");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 科室关系SQL
        /// </summary>
        /// <returns></returns>
        public DataSet getReportDeptGuideType()
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("select DEPT_CLASS_CODE ID ,DEPT_CLASS_NAME as TEXT from HOSPITALSYS.JXGL_GUIDE_DEPT_CLASS_DICT order by DEPT_CLASS_CODE");

            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }

        /// <summary>
        /// 根据科室查询指标
        /// </summary>
        /// <param name="organ">组织类别</param>
        /// <param name="dept">部门</param>
        /// <param name="bsc">父部门</param>
        /// <param name="Search">查询条件</param>
        /// <returns></returns>
        public DataSet getReportSearchGuide(string organ, string dept, string bsc, string Search)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("select guide_name as TEXT , guide_code as VALUE ,bsc,dept,organ ");
            sql.Append("from hospitalsys.guide_name_dict ");
            sql.Append("WHERE organ = (select ORGAN_CLASS_CODE from HOSPITALSYS.JXGL_GUIDE_ORGAN_CLASS_DICT where ORGAN_CLASS_KEY = '");
            sql.Append(organ);
            sql.Append("')");
            sql.Append(" AND ( dept =");
            sql.Append(dept);
            sql.Append(" or dept = '00')");
            if (Search.Equals(""))
            {
                if (bsc != "") 
                {
                    sql.Append(" AND bsc =");
                    sql.Append(bsc);
                }
            }
            else
            {
                sql.Append(" AND guide_name like '%");
                sql.Append(Search);
                sql.Append("%'");
            }
            sql.Append(" AND ispage = '1' ORDER BY GUIDE_NAME");
            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }

        /// <summary>
        /// 根据BSC树形分类和组织、科室 查询指标列表
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="org"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        public DataSet GetAnalyseGuideDictListByBscOrgDept(string bsc, string org, string dept, string tag)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                SELECT A.GUIDE_CODE VALUE,  A.GUIDE_NAME TEXT
                  FROM {0}.GUIDE_NAME_DICT A
                 WHERE A.ISPAGE = '1'
                   AND DECODE('{1}',NULL,'1',A.ORGAN) = NVL('{1}','1')
                   AND DECODE('{2}',NULL,'1',DECODE(A.DEPT,'00' , '{2}' ,A.DEPT)) = NVL('{2}','1') ", DataUser.HOSPITALSYS, org, dept);
            if (!bsc.Equals(""))
            {
                str.AppendFormat(" AND A.BSC = '{0}' ", bsc);
            }
            if (!tag.Equals(""))
            {
                str.Append(" AND (A.GUIDE_NAME LIKE '%" + tag + "%' OR A.GUIDE_CODE LIKE '%" + tag + "%')");
            }
            str.Append(" ORDER BY SERIAL_NO DESC");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 指标转换
        /// </summary>
        /// <param name="code">指标CODE</param>
        /// <param name="organ">01院，02科</param>
        /// <returns></returns>
        public DataSet getGuideByHosDeptGuideCode(string guidecode, string organ) 
        {
            StringBuilder str = new StringBuilder();
            switch (organ) 
            {
                case "01":
                    str.AppendFormat("SELECT DEPT_GUIDE_CODE FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER T1 WHERE T1.HOSPITAL_GUIDE_CODE='{1}'", DataUser.HOSPITALSYS, guidecode);
                    break;
                case "02":
                    str.AppendFormat("SELECT MEMBER_GUIDE_CODE FROM {0}.GUIDE_HOSPITAL_DEPT_MEMBER T1 WHERE T1.DEPT_GUIDE_CODE='{1}'", DataUser.HOSPITALSYS, guidecode);
                    break;
            }
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
                             WHERE A.ISPAGE = '1' ", DataUser.HOSPITALSYS);
            if (!depttype.Equals(""))
            {
                str.AppendFormat(@" AND  DECODE(A.DEPT,'00' , '{0}' ,A.DEPT) = '{0}' ", depttype);
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
        /// 查询科室岗位
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataSet getStationMain(string deptCode,string year) 
        {
            deptCode = deptCode == "AND DEPT_CODE IN ('-1')" ? "" : "AND DEPT_CODE IN (" + deptCode + ")";
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" SELECT DISTINCT  A.ID VALUE,A.STATION_NAME TEXT
                                   FROM {0}.SYS_STATION_MAINTENANCE_DICT A,
                                        (SELECT STATION_DICT_CODE
                                           FROM {0}.SYS_STATION_BASIC_INFORMATION
                                          WHERE STATION_YEAR = '{1}' {2} ) B
                                  WHERE A.ID = B.STATION_DICT_CODE ", DataUser.COMM, DateTime.Now.Year.ToString(),deptCode);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 评价指标按BSC分类树
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
        /// 评价指标按科室分类树
        /// </summary>
        /// <returns></returns>
        public DataSet getDeptGuideTree()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" 
                 SELECT 'D'||DEPT_CLASS_CODE AS DEPT_CLASS_CODE ,DEPT_CLASS_NAME FROM {0}.JXGL_GUIDE_DEPT_CLASS_DICT UNION ALL
                 SELECT 'D00' ,'公共部分' FROM DUAL ORDER BY DEPT_CLASS_CODE", DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 根据BSC树形分类和组织、科室 查询指标列表
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
                   AND DECODE('{1}',NULL,'1',A.ORGAN) = NVL('{1}','1')
                   AND DECODE('{2}',NULL,'1',DECODE(A.DEPT,'00' , '{2}' ,A.DEPT)) = NVL('{2}','1') ", DataUser.HOSPITALSYS, org, dept);
            if (!bsc.Equals(""))
            {
                str.AppendFormat(" AND A.BSC = '{0}' ", bsc);
            }
            str.Append(" ORDER BY SERIAL_NO DESC");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

       
        #endregion

    }
}
