using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;
using GoldNet.Comm;
using System.Collections;

namespace Goldnet.Dal
{
    public class TreeDalDict
    {
        public TreeDalDict()
		{}

        /// <summary>
        /// 报表类别初始化树结构
        /// </summary>
        /// <returns></returns>
        public DataSet getReportTypeTreeBuilder()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                 SELECT CLASSID,CLASSNAME,CLASSPID,RANKING,LEVEL LEV, CONNECT_BY_ISLEAF ISLEAF
                   FROM {0}.REPORT_CLASS_DICT 
                  START WITH CLASSPID= '0' 
                CONNECT BY PRIOR CLASSID = CLASSPID   
                  ORDER SIBLINGS BY RANKING,TO_NUMBER(CLASSID)", DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 分析报表类别初始化树结构
        /// </summary>
        /// <returns></returns>
        public DataSet getAnalyseReportTypeTreeBuilder()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"
                 SELECT CLASSID,CLASSNAME,CLASSPID,RANKING,LEVEL LEV, CONNECT_BY_ISLEAF ISLEAF
                   FROM {0}.ANALYSISRPT_CLASS_DICT 
                  START WITH CLASSPID= '0' 
                CONNECT BY PRIOR CLASSID = CLASSPID   
                  ORDER SIBLINGS BY RANKING,TO_NUMBER(CLASSID)", DataUser.HOSPITALSYS);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 设置报表初始化树结构
        /// </summary>
        /// <returns></returns>
        public DataSet getSetReportTreeBuilder()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(" select CLASSID,CLASSNAME,CLASSPID from {0}.REPORT_CLASS_DICT ", DataUser.HOSPITALSYS);
            str.Append(" start with CLASSPID= '0' ");
            str.Append(" connect by prior CLASSID = CLASSPID   ");
            str.Append(" ORDER SIBLINGS BY ranking  ");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        /// <summary>
        /// 设置分析报表初始化树结构
        /// </summary>
        /// <returns></returns>
        public DataSet getSetAnalyseReportTreeBuilder()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(" select CLASSID,CLASSNAME,CLASSPID from {0}.ANALYSISRPT_CLASS_DICT ", DataUser.HOSPITALSYS);
            str.Append(" start with CLASSPID= '0' ");
            str.Append(" connect by prior CLASSID = CLASSPID   ");
            str.Append(" ORDER SIBLINGS BY ranking  ");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        /// <summary>
        /// 设置报表指标初始化树结构
        /// </summary>
        /// <param name="depttype">部门类别</param>
        /// <param name="OrganType">组织类别</param>
        /// <returns></returns>
        public DataSet getSetReportGuideTreeBuilder(string depttype, string OrganType)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" SELECT A.name,a.id,a.pid FROM 
                                    (
                                    select  c.NAME,C.ID,LEVEL glevel,connect_by_isleaf LEAF ,c.pid
                                    from ( select BSC_CLASS_NAME NAME, BSC_CLASS_CODE ID ,  
                                    case when length(B.BSC_CLASS_CODE) =2 then '0'  else substr(B.BSC_CLASS_CODE,1,length(B.BSC_CLASS_CODE)- 2)    end as pid,     '' dept,'' organ,'' isexpress,'' ispage,'' issame,'' ishighguide,'' issel,'' isabs,'' isszpj   from hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT   b   
                                    union all  
                                    select guide_name name , guide_code code ,bsc pid , dept,organ,isexpress,ispage,issame,ishighguide,issel,isabs,isszpj 
                                    from  hospitalsys.guide_name_dict a where a.ispage = '1' and case when a.DEPT = '00' THEN '{1}' ELSE a.DEPT END = '{1}' and a.organ = (select ORGAN_CLASS_CODE from HOSPITALSYS.JXGL_GUIDE_ORGAN_CLASS_DICT where ORGAN_CLASS_KEY = '{0}')) c    
                                    start with pid= '0'  connect by prior ID = pid 
                                     )
                                     A
                                     WHERE 
                                           A.glevel <> 3
                                          and  (A.glevel <> 2 or A.LEAF <> 1)   ", OrganType, depttype);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        public DataSet getSetReportDeptSecondTreeBuilder() 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" SELECT * FROM COMM.SYS_DEPT_SECOND ");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }




        /// <summary>
        /// 设置报表部门初始化树结构
        /// </summary>
        /// <returns></returns>
        public DataSet getSetReportDeptTreeBuilder()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" SELECT T1.PID AS PID, T1.ID AS ID,T1.ATTRIBUE AS NAME 
                     FROM (
                        SELECT '0' AS PID,('l' || A.ID) AS ID,A.ATTRIBUE,0 SORTNO FROM {0}.SYS_LCDEPT_ATTR_DICT A 
                        UNION ALL 
                        SELECT '-1' AS PID,('' || C.ID) AS ID,C.ATTRIBUE,C.SORTNO FROM {0}.SYS_DEPT_ATTR_DICT C
                     ) T1
                    START WITH PID= '-1'  CONNECT BY PRIOR ID = PID  ORDER SIBLINGS BY SORTNO ", DataUser.COMM);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 报表查询中心初始化树结构
        /// </summary>
        /// <param name="OrgType">组织类别</param>
        /// <param name="PowerCode">权限</param>
        /// <returns></returns>
        public DataSet getReportInfoTreeBuilder(string OrgType,string PowerCode,string staffpower,string incount)
        {
            StringBuilder str = new StringBuilder();

            switch (OrgType)
            {
                case "K":
                    if (PowerCode == "'-1'")
                    {
                        str.AppendFormat(@"select ID,RPT_NAME CLASSNAME,RPT_TYPE,'CLASS_'|| RPT_CLASSID 
                                            AS CLASSPID,RPT_RANKING RANKING 
                                            from hospitalsys.REPORT_LIST_DICT WHERE RPT_TYPE = 'K'");
                    }
                    else if (PowerCode == "")
                    {
                        str.AppendFormat(@"   SELECT T2.*
                                      FROM (SELECT     T1.*, LEVEL GLEVEL, CONNECT_BY_ISLEAF LEAF
                                                  FROM (SELECT 'CLASS_' || CLASSID AS ID, CLASSNAME,
                                                               '' AS RPT_TYPE, 'CLASS_' || CLASSPID CLASSPID,
                                                               RANKING
                                                          FROM HOSPITALSYS.REPORT_CLASS_DICT
                                                        UNION ALL
                                                        SELECT ID, RPT_NAME, RPT_TYPE,
                                                               'CLASS_' || RPT_CLASSID AS CLASSPID,
                                                               RPT_RANKING RANKING
                                                          FROM HOSPITALSYS.REPORT_LIST_DICT
                                                         WHERE RPT_TYPE = 'K') T1
                                            START WITH T1.CLASSPID = 'CLASS_0'
                                            CONNECT BY PRIOR T1.ID = T1.CLASSPID
                                              ORDER SIBLINGS BY RANKING) T2
                                     WHERE T2.GLEVEL <> 2 OR T2.LEAF <> 1");
                    }
                    else if (PowerCode != "" && PowerCode != "'-1'") 
                    {
//                        UpdataReportInfo(PowerCode, incount);
//                        str.AppendFormat(@"SELECT * FROM (
//                                                SELECT distinct AA.*, LEVEL GLEVEL, CONNECT_BY_ISLEAF LEAF
//                                                      FROM HOSPITALSYS.REPORT_TREE_TEMP AA
//                                                START WITH CLASSPID = 'CLASS_0'
//                                                CONNECT BY PRIOR ID = CLASSPID
//                                                ORDER SIBLINGS BY RANKING)
//                                                WHERE (GLEVEL <> 2 OR LEAF <> 1) and INCOUNT='{0}'", incount);
                        str.AppendFormat(@"   SELECT T2.*
                                      FROM (SELECT     T1.*, LEVEL GLEVEL, CONNECT_BY_ISLEAF LEAF
                                                  FROM (SELECT 'CLASS_' || CLASSID AS ID, CLASSNAME,
                                                               '' AS RPT_TYPE, 'CLASS_' || CLASSPID CLASSPID,
                                                               RANKING
                                                          FROM HOSPITALSYS.REPORT_CLASS_DICT
                                                        UNION ALL
                                                        SELECT ID, RPT_NAME, RPT_TYPE,
                                                               'CLASS_' || RPT_CLASSID AS CLASSPID,
                                                               RPT_RANKING RANKING
                                                          FROM HOSPITALSYS.REPORT_LIST_DICT
                                                         WHERE RPT_TYPE = 'K') T1
                                            START WITH T1.CLASSPID = 'CLASS_0'
                                            CONNECT BY PRIOR T1.ID = T1.CLASSPID
                                              ORDER SIBLINGS BY RANKING) T2
                                     WHERE T2.GLEVEL <> 2 OR T2.LEAF <> 1");
                    }
                    break;
                case "R":
                    if(PowerCode == "") 
                    {
                        str.AppendFormat(@"   SELECT T2.*
                                      FROM (SELECT     T1.*, LEVEL GLEVEL, CONNECT_BY_ISLEAF LEAF
                                                  FROM (SELECT 'CLASS_' || CLASSID AS ID, CLASSNAME,
                                                               '' AS RPT_TYPE, 'CLASS_' || CLASSPID CLASSPID,
                                                               RANKING
                                                          FROM HOSPITALSYS.REPORT_CLASS_DICT
                                                        UNION ALL
                                                        SELECT ID, RPT_NAME, RPT_TYPE,
                                                               'CLASS_' || RPT_CLASSID AS CLASSPID,
                                                               RPT_RANKING RANKING
                                                          FROM HOSPITALSYS.REPORT_LIST_DICT
                                                         WHERE RPT_TYPE = 'R') T1
                                            START WITH T1.CLASSPID = 'CLASS_0'
                                            CONNECT BY PRIOR T1.ID = T1.CLASSPID
                                              ORDER SIBLINGS BY RANKING) T2
                                     WHERE T2.GLEVEL <> 2 OR T2.LEAF <> 1");
                    }
                    else if (PowerCode == "'-1'") 
                    {
                        UpdataStaffReportInfo(InsertRptTreeStaffSelfTemp(incount, staffpower), incount);
                        str.AppendFormat(@"SELECT t2.*
                                          FROM (SELECT     t1.*, LEVEL glevel, CONNECT_BY_ISLEAF leaf
                                                      FROM hospitalsys.REPORT_TREE_STAFF_TEMP t1
                                                START WITH t1.classpid = 'CLASS_0'
                                                CONNECT BY PRIOR t1.ID = t1.classpid
                                                  ORDER SIBLINGS BY ranking) t2
                                         WHERE (t2.glevel <> 2 OR t2.leaf <> 1) and INCOUNT='{0}'",incount);
                    }
                    else if (PowerCode != "" && PowerCode != "'-1'") 
                    {
//                        UpdataStaffReportInfo(InsertRptTreeStaffTemp(PowerCode, incount), incount);

//                        str.AppendFormat(@"SELECT t2.*
//                                          FROM (SELECT     t1.*, LEVEL glevel, CONNECT_BY_ISLEAF leaf
//                                                      FROM hospitalsys.REPORT_TREE_STAFF_TEMP t1
//                                                START WITH t1.classpid = 'CLASS_0'
//                                                CONNECT BY PRIOR t1.ID = t1.classpid
//                                                  ORDER SIBLINGS BY ranking) t2
//                                         WHERE (t2.glevel <> 2 OR t2.leaf <> 1) and INCOUNT='{0}'",incount);
                        str.AppendFormat(@"   SELECT T2.*
                                      FROM (SELECT     T1.*, LEVEL GLEVEL, CONNECT_BY_ISLEAF LEAF
                                                  FROM (SELECT 'CLASS_' || CLASSID AS ID, CLASSNAME,
                                                               '' AS RPT_TYPE, 'CLASS_' || CLASSPID CLASSPID,
                                                               RANKING
                                                          FROM HOSPITALSYS.REPORT_CLASS_DICT
                                                        UNION ALL
                                                        SELECT ID, RPT_NAME, RPT_TYPE,
                                                               'CLASS_' || RPT_CLASSID AS CLASSPID,
                                                               RPT_RANKING RANKING
                                                          FROM HOSPITALSYS.REPORT_LIST_DICT
                                                         WHERE RPT_TYPE = 'R') T1
                                            START WITH T1.CLASSPID = 'CLASS_0'
                                            CONNECT BY PRIOR T1.ID = T1.CLASSPID
                                              ORDER SIBLINGS BY RANKING) T2
                                     WHERE T2.GLEVEL <> 2 OR T2.LEAF <> 1");
                    }

                    break;
            }
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        public DataSet getAnalyseReportViewTree() 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT T2.*
                                  FROM (SELECT T1.*, LEVEL GLEVEL, CONNECT_BY_ISLEAF LEAF
                                              FROM (SELECT 'CLASS_' || CLASSID AS ID, CLASSNAME,
                                                           'CLASS_' || CLASSPID CLASSPID,
                                                           RANKING
                                                      FROM {0}.ANALYSISRPT_CLASS_DICT
                                                    UNION ALL
                                                    SELECT ID, RPT_NAME,
                                                           'CLASS_' || RPT_CLASSID AS CLASSPID,
                                                           RPT_RANKING RANKING
                                                      FROM {0}.ANALYSISRPT_LIST_DICT) T1
                                        START WITH T1.CLASSPID = 'CLASS_0'
                                        CONNECT BY PRIOR T1.ID = T1.CLASSPID
                                          ORDER SIBLINGS BY RANKING) T2
                                 WHERE T2.GLEVEL <> 2 OR T2.LEAF <> 1", DataUser.HOSPITALSYS);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 病种分析初始化树结构
        /// </summary>
        /// <param name="type">DEPT_OPERATION_DICT:手术</param>
        /// <returns></returns>
        public DataSet getDiseaseAnalyseOperationTreeBuilder(string type) 
        {
            StringBuilder str = new StringBuilder();
            str.Append(" select * from ");
            str.Append("(            ");
            str.Append(" select   A.dept_code as id,   '0' as p_id ,B.dept_name as name");
            str.Append("  from ");
            str.AppendFormat("   (select distinct dept_code from {0}.{1}) A", DataUser.HISFACT,type);
            str.AppendFormat("   left join {0}.SYS_DEPT_DICT b on a.dept_code=b.dept_code", DataUser.COMM);
            str.Append("  union all ");
            str.AppendFormat("  select to_char(rownum)||'_N' as id, to_char(dept_code) as p_id, jbpm as name from {0}.{1} ",DataUser.HISFACT,type);
            str.Append(")c  ");
            str.Append("start with p_id = 0 ");
            str.Append("connect by  prior id =   p_id");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 数字讲评初始化树结构
        /// </summary>
        /// <param name="OrganType">组织类别</param>
        /// <param name="DeptType">部门类别</param>
        /// <returns></returns>
        public DataSet getGuideForGuideLook(string OrganType, string DeptType) 
        {
            StringBuilder str = new StringBuilder();

            str.Append("  select  name,code,pid,level,connect_by_isleaf from (   ");
            str.Append("  select BSC_CLASS_NAME name, BSC_CLASS_CODE code ,");
            str.Append("  case when length(B.BSC_CLASS_CODE) =2 then '0' ");
            str.Append(" else substr(B.BSC_CLASS_CODE,1,length(B.BSC_CLASS_CODE)- 2)  ");
            str.Append("  end as pid, ");
            str.Append("    '' dept,'' organ,'' isexpress,'' ispage,'' issame,'' ishighguide,'' issel,'' isabs,'' isszpj  ");
            str.Append(" from hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT   b   ");
            str.Append(" union all ");
            str.Append(" select guide_name name , guide_code code ,bsc pid ,dept,organ,isexpress,ispage,issame,ishighguide,issel,isabs,isszpj from ");
            str.AppendFormat(" hospitalsys.guide_name_dict a where  case when a.DEPT = '00' THEN '{0}' ELSE  a.DEPT END = '", DeptType);

            str.Append(DeptType);

            str.Append("' AND A.organ = '");
            str.Append(OrganType);
            str.Append("' and ispage = '1' and issel='1' ");
            str.Append(") c  ");
            str.Append(" where level <> 2 or connect_by_isleaf <> 1 ");
            str.Append(" start with pid= '0' ");
            str.Append(" connect by prior code = pid ");

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        
        }


        /// <summary>
        /// 数字讲评部门过滤初始化树结构
        /// </summary>
        /// <param name="DeptType">组织类别</param>
        /// <param name="queryString">部门类别</param>
        /// <returns></returns>
        public DataSet getDeptForGuideLook(string DeptType,string queryString)
        {
            StringBuilder str = new StringBuilder();
          
            str.AppendFormat("    SELECT name,id,pid  ");
            str.AppendFormat("     FROM {0}.sys_dept_treeview  ", DataUser.COMM);
            str.AppendFormat("     where attr = '是'  ");
            if (queryString != "") 
            {
                str.AppendFormat(" AND (ID LIKE 'CLASS%' OR (ID NOT LIKE 'CLASS%' AND NAME LIKE '%{0}%'))  ", queryString);
            }
           
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }


        /// <summary>
        /// 数字讲评人员过滤初始化树结构
        /// </summary>
        /// <param name="DeptType">部门类别</param>
        /// <param name="year">年度</param>
        /// <param name="queryString">查询条件</param>
        /// <returns></returns>
        public DataSet getSatffForGuideLook(string DeptType,string year,string queryString)
        {
            StringBuilder str = new StringBuilder();

            //str.AppendFormat(" SELECT ID,NAME,PID, LEVEL,CONNECT_BY_ISLEAF ISLEAF FROM ");
            //str.AppendFormat(" (SELECT ID, NAME, pid ");
            //str.AppendFormat("  FROM {0}.sys_dept_treeview ",DataUser.COMM);
            //str.AppendFormat("  WHERE attr = '是' ");
            //str.AppendFormat("  UNION ALL ");
            //str.AppendFormat("  select  T2.STATION_CODE ID,T2.STATION_NAME NAME ,T1.DEPT_CODE PID ");
            //str.AppendFormat("  from {0}.SYS_DEPT_DICT t1,{0}.SYS_STATION_BASIC_INFORMATION t2  ",DataUser.COMM);
            //str.AppendFormat("  where t1.DEPT_CODE = t2.DEPT_CODE and t2.STATION_YEAR = '{0}' ",year);
            //if (queryString != "") 
            //{
            //    str.AppendFormat("  AND T2.STATION_NAME like '%{0}%'  ",queryString);
            //}
            //str.AppendFormat("  ) ");
            //str.AppendFormat(" START WITH pid = '000000' ");
            //str.AppendFormat(" CONNECT BY PRIOR ID = pid ");

            str.AppendFormat(@"SELECT ID, NAME, PID,ISLEAF,TPATH,TLEVEL,SORTNO
                                FROM COMM.SYS_DEPT_TREEVIEW
                                WHERE ATTR = '是'");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 更新报表指标信息
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <param name="l_dt">指标集合</param>
        public void UpdataReportInfo(string power,string incount)
        {
            MyLists list = new MyLists();

            List l_listDel = new List();

            l_listDel.Parameters = new OleDbParameter[] { new OleDbParameter() };

            l_listDel.StrSql = delRptTreeTemp(incount);

            list.Add(l_listDel);

            List insert = new List();

            insert.Parameters = new OleDbParameter[] { new OleDbParameter() };

            insert.StrSql = InsertRptTreeTemp(power, incount);
            list.Add(insert);
            
            OracleOledbBase.ExecuteTranslist(list);
        }

        private string delRptTreeTemp(string incount) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM {0}.REPORT_TREE_TEMP WHERE INCOUNT = '{1}'",DataUser.HOSPITALSYS,incount);
            return str.ToString();
        }

        private string InsertRptTreeTemp(string power,string incount)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO {0}.REPORT_TREE_TEMP VALUE SELECT 'CLASS_' || CLASSID AS ID, CLASSNAME, '' RPT_TYPE,
                                     'CLASS_' || CLASSPID CLASSPID, RANKING,'{1}' INCONUT
                                FROM {0}.REPORT_CLASS_DICT
                              UNION ALL
                              SELECT DISTINCT ID, RPT_NAME, RPT_TYPE RPT_TYPE,
                                              ('CLASS_' || RPT_CLASSID) AS CLASSPID,
                                              RPT_RANKING RANKING,'{1}' INCONUT
                                         FROM {0}.REPORT_POWER_DEPT_VIEW t1
                                        WHERE t1.DEPT_CODE IN
                                                       ({2})", DataUser.HOSPITALSYS, incount,power);
            return str.ToString();
        }

        private string delRptTreeStaffTemp(string incount)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"DELETE FROM {0}.REPORT_TREE_STAFF_TEMP WHERE INCOUNT = '{1}'", DataUser.HOSPITALSYS, incount);
            return str.ToString();
        }

        private string InsertRptTreeStaffTemp(string power, string incount)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO {0}.REPORT_TREE_STAFF_TEMP VALUE SELECT 'CLASS_' || CLASSID AS ID, CLASSNAME, '' RPT_TYPE,
                                     'CLASS_' || CLASSPID CLASSPID, RANKING,'{1}' INCONUT
                                FROM {0}.REPORT_CLASS_DICT
                              UNION ALL
                              SELECT DISTINCT ID, rpt_name classname, rpt_type,
                                    'CLASS_' || rpt_classid AS classpid,
                                    rpt_ranking ranking,'{1}' INCONUT
                               FROM {0}.report_power_staff_view
                              WHERE {0}.report_power_staff_view.staff_id IN (
                                       SELECT t0.staff_id
                                         FROM rlzy.new_staff_info t0
                                        WHERE dept_code IN
                                                 ({2}))", DataUser.HOSPITALSYS, incount, power);
            return str.ToString();
        }

        private string InsertRptTreeStaffSelfTemp(string incount ,string staffid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"INSERT INTO {0}.REPORT_TREE_STAFF_TEMP VALUE SELECT 'CLASS_' || CLASSID AS ID, CLASSNAME, '' RPT_TYPE,
                                     'CLASS_' || CLASSPID CLASSPID, RANKING,'{1}' INCONUT
                                FROM {0}.REPORT_CLASS_DICT
                              UNION ALL
                              SELECT DISTINCT ID, rpt_name classname, rpt_type,
                                    'CLASS_' || rpt_classid AS classpid,
                                    rpt_ranking ranking,'{1}' INCONUT
                               FROM {0}.report_power_staff_view
                              WHERE {0}.report_power_staff_view.staff_id = '{1}'", DataUser.HOSPITALSYS, incount);
            return str.ToString();
        }


        /// <summary>
        /// 更新报表指标信息
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <param name="l_dt">指标集合</param>
        public void UpdataStaffReportInfo(string insrtSql,string incount)
        {
            MyLists list = new MyLists();

            List l_listDel = new List();

            l_listDel.Parameters = new OleDbParameter[] { new OleDbParameter() };

            l_listDel.StrSql = delRptTreeStaffTemp(incount);

            list.Add(l_listDel);

            List insert = new List();

            insert.Parameters = new OleDbParameter[] { new OleDbParameter() };

            insert.StrSql = insrtSql;
            list.Add(insert);

            OracleOledbBase.ExecuteTranslist(list);
        }


    }
}
