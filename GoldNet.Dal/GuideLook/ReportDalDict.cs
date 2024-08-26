using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GoldNet.Comm;
using System.Data.OleDb;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OracleClient;
using System.Collections;

namespace Goldnet.Dal
{
    public class ReportDalDict
    {
        public ReportDalDict()
		{ }

        #region 成员方法
        /// <summary>
        /// 得到报表信息字段
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <returns></returns>
        public DataSet getReportTerms(string rptid)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("SELECT ID,RPT_NAME,RPT_TERMS,PRT_SECOND_TERMS,RPT_DPETTREETYPE FROM {0}.REPORT_LIST_DICT WHERE ID = ", DataUser.HOSPITALSYS);

            str.Append(rptid);

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 更新报表信息
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <param name="Terms">报表条件</param>
        /// /// <param name="Terms">0:科室类别，1:2级科室</param>
        public void UpdateReportTerms(string rptid, string Terms,string DeptTreeType)
        {
            StringBuilder sql = new StringBuilder();

            string deptSecondTerms = "";
            string deptTypeTerms = "";
            if (DeptTreeType == "0")
            {
                deptTypeTerms = Terms;
            }
            else 
            {
                deptSecondTerms = Terms;
            }

            sql.AppendFormat("update {0}.REPORT_LIST_DICT set RPT_TERMS = '",DataUser.HOSPITALSYS);

            sql.Append(deptTypeTerms);

            sql.AppendFormat("',PRT_SECOND_TERMS = '");

            sql.Append(deptSecondTerms);

            sql.AppendFormat("',RPT_DPETTREETYPE = '");

            sql.Append(DeptTreeType);

            sql.Append("' where ID = ");

            sql.Append(rptid);

            OracleOledbBase.ExecuteNonQuery(sql.ToString());

        }


        /// <summary>
        /// 更新报表信息
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <param name="Terms">报表条件</param>
        
        public void UpdateReportTerms(string rptid, string Terms)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update {0}.REPORT_LIST_DICT set RPT_TERMS = '", DataUser.HOSPITALSYS);
            sql.Append(Terms);
            sql.Append("' where ID = ");
            sql.Append(rptid);
            OracleOledbBase.ExecuteNonQuery(sql.ToString());
        }


        /// <summary>
        /// 更新报表信息
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <param name="Terms">报表条件</param>
        public void UpdateStaffReportTerms(string rptid, string Terms,string TermsStation)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update {0}.REPORT_LIST_DICT set RPT_TERMS = '", DataUser.HOSPITALSYS);
            sql.Append(Terms);
            sql.AppendFormat("' ,PRT_SECOND_TERMS = '{0}' where ID = ", TermsStation);
            sql.Append(rptid);
            OracleOledbBase.ExecuteNonQuery(sql.ToString());
        }



        /// <summary>
        /// 删除报表信息
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <returns></returns>
        public string DeleteReportCodeInfo(string rptid)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("delete from {0}.REPORT_UNIT_CODE where report_code =", DataUser.HOSPITALSYS);

            str.Append(rptid);

            return str.ToString();

        }

        /// <summary>
        /// 添加报表信息
        /// </summary>
        /// <param name="terms">添加数据SQL</param>
        /// <returns></returns>
        public string UpDateReportInfo(string terms)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("insert into {0}.REPORT_UNIT_CODE(report_code,UNIT_CODE)  ", DataUser.HOSPITALSYS);

            str.Append(terms);

            return str.ToString();

        }

        /// <summary>
        /// 添加报表信息过程
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <param name="terms">添加数据SQL</param>
        public void UpdateReportInfoTran(string rptid, string terms) 
        {
            MyLists list = new MyLists();

            List l_listDel = new List();

            l_listDel.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDel.StrSql = DeleteReportCodeInfo(rptid);
            list.Add(l_listDel);


            List l_listUp = new List();
            l_listUp.Parameters = new OleDbParameter[] { new OleDbParameter() };

            l_listUp.StrSql=UpDateReportInfo(terms);

            list.Add(l_listUp);

            OracleOledbBase.ExecuteTranslist(list);
        
        }

        /// <summary>
        /// 生成报表信息
        /// </summary>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="unit_type">报表类别</param>
        /// <param name="incount">staffid</param>
        /// <param name="report_code">报表ID</param>
        /// <param name="kdept"></param>
        /// <returns></returns>
        public DataSet getReportDetailInfo(string startdate, string enddate, string unit_type, string incount, string report_code, DataTable kdept,string power,string staffPower)
        {
            GuideDalDict dal = new GuideDalDict();

            DataSet l_ds = new DataSet();

            #region 报表指标值存储过程
            //报表指标值存储过程
            // HOSPITALSYS.REPORT_COMPUTE(startdate VARCHAR2,enddate  VARCHAR2,incount VARCHAR2,reportcode varchar2)
            OleDbParameter[] parameters = {
            new OleDbParameter("startdate", startdate),
            new OleDbParameter("enddate", enddate),
            new OleDbParameter("incount",incount),
            new OleDbParameter("reportcode",report_code)
                                                        };
            OracleOledbBase.RunProcedure("HOSPITALSYS.REPORT_COMPUTE", parameters);

            //查询指标集合
            DataTable l_dt = dal.getGuideCodeByReportid(report_code).Tables[0];

            string Guide = "";

            for (int i = 0; i < l_dt.Rows.Count; i++)
            {
                Guide = Guide + l_dt.Rows[i][0].ToString() + ",";
            }

            //查看相关性信息
            OleDbParameter[] parameters1 = {
            new OleDbParameter("startdate", startdate),
            new OleDbParameter("enddate", enddate),
            new OleDbParameter("incount",incount),
            new OleDbParameter("guidecode",Guide.TrimEnd(new char[]{','}))
                                                        };
            OracleOledbBase.RunProcedure("HOSPITALSYS.CORRELATION", parameters1);

            #endregion

            kdept = dal.getGuideCodeByReportid(report_code).Tables[0];

            if (unit_type.Equals("R"))
            {
                l_ds = getReportStaff(incount, report_code, kdept, power,staffPower);
            }
            if (unit_type.Equals("K")) 
            {
                l_ds = getReportDept(incount, report_code, kdept, power);
            }
            return l_ds;
        }

        /// <summary>
        /// 取得人员报表
        /// </summary>
        /// <param name="incount">STAFFID</param>
        /// <param name="report_code">报表ID</param>
        /// <param name="kdept">指标集合</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet getReportStaff(string incount, string report_code, DataTable kdept, string power,string staffPower)
        {
            //DataSet a = new DataSet();
            StringBuilder str = new StringBuilder();

            StringBuilder str1 = new StringBuilder();

            StringBuilder str2 = new StringBuilder();

            for (int i = 0; i < kdept.Rows.Count; i++)
            {
                if (kdept.Rows[i]["GUIDE_CODE"].ToString() != "")
                {
                    str1.Append(",to_char(SUM (CASE WHEN a.guide_code='");
                    str1.Append(kdept.Rows[i]["GUIDE_CODE"].ToString());
                    str1.Append("' THEN a.guide_value ELSE 0 end )) AS ");
                    str1.Append("a" + kdept.Rows[i]["GUIDE_CODE"].ToString());
                }
            }


            str2.AppendFormat(@"SELECT f.STAFF_ID,CASE
                                    WHEN (SELECT COUNT (NAME)
                                            FROM {0}.NEW_STAFF_INFO T2
                                           WHERE T2.NAME = F.NAME AND T2.ADD_MARK = '1') = 2
                                       THEN F.NAME || '(' || F.DEPT_NAME || ')'
                                    ELSE F.NAME
                                 END AS NAME {1}",DataUser.RLZY,str1.ToString());
            //for (int i = 0; i < kdept.Rows.Count; i++)
            //{
            //    if (kdept.Rows[i]["GUIDE_CODE"].ToString() !="") 
            //    {
            //        str.Append(",to_char(SUM (CASE WHEN a.guide_code='");
            //        str.Append(kdept.Rows[i]["GUIDE_CODE"].ToString());
            //        str.Append("' THEN a.guide_value ELSE 0 end )) AS ");
            //        str.Append("a" + kdept.Rows[i]["GUIDE_CODE"].ToString());
            //    }
            //}
            str2.Append(" FROM hospitalsys.report_temp a, rlzy.new_staff_info f WHERE to_char(f.staff_id) = a.person_id(+)");
            str2.Append(" and a.COUNTING(+)='" + incount + "' and A.ORGAN(+) = 'R'");
            str2.Append(" AND f.staff_id IN (");
            /*251 没岗位不用岗位找人
           str.Append(string.Format(@"SELECT c.person_id
                             FROM report_dept_code a,
                                  station_basic_information b,
                                  station_personnel_information c
                            WHERE a.dept_code = b.station_dict_code
                              AND b.station_code = c.station_code
                              and B.STATION_YEAR = to_char(sysdate,'yyyy')
                              AND a.REPORT_CODE =  '{0}'", report_code));
             */
            str2.Append(string.Format(" SELECT info.unit_code FROM hospitalsys.REPORT_UNIT_CODE info WHERE info.REPORT_CODE = '{0}'", report_code));

            str2.Append(")");
            if (power == "'-1'")
            {
                str2.Append("AND F.STAFF_ID IN (" + staffPower + ")");
            }
            else if (power != "'-1'" && power != "") 
            {
                str2.Append("AND F.STAFF_ID IN (SELECT STAFF_ID FROM RLZY.NEW_STAFF_INFO T1 WHERE T1.DEPT_CODE IN (" + power + "))");
            }
            str2.Append(" GROUP BY f.staff_id, f.name,F.DEPT_NAME order by f.name");



            str.AppendFormat(@"  select ccc.* from ({0}) ccc
                                union all
                                select -1 staff_id ,'合计' name ", str2.ToString());

            for (int i = 0; i < kdept.Rows.Count; i++)
            {
                str.AppendFormat(", to_char(SUM (a" + kdept.Rows[i]["GUIDE_CODE"].ToString() + "),'999g999g990d00') ");
            }
            str.AppendFormat("from ({0}) ccc",str2.ToString());
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 取得科室报表
        /// </summary>
        /// <param name="incount">STAFFID</param>
        /// <param name="report_code">报表ID</param>
        /// <param name="kdept">指标集合</param>
        /// <param name="power">权限</param>
        /// <returns></returns>
        public DataSet getReportDept(string incount, string report_code, DataTable kdept,string power)
        {
            power = power == "" ? "" : "AND F.DEPT_CODE IN (" + power + ")";
            StringBuilder str = new StringBuilder();

            StringBuilder str1 = new StringBuilder();

            for (int i = 0; i < kdept.Rows.Count; i++)
            {
                str1.AppendFormat(", TO_CHAR(SUM ( DECODE (a.guide_code, '" + kdept.Rows[i]["GUIDE_CODE"].ToString() + "', a.guide_value,0))) AS a" + kdept.Rows[i]["GUIDE_CODE"].ToString() + " ");
            }

            StringBuilder str2 = new StringBuilder();

            str2.AppendFormat(" SELECT to_char(BB.SORT_NO) sortno, AA.*  FROM( ");
            str2.AppendFormat("SELECT   F.DEPT_CODE , max(F.DEPT_NAME) DEPT_NAME {0}", str1.ToString());
            str2.AppendFormat("    FROM {0}.report_temp a, {1}.SYS_DEPT_DICT f ", DataUser.HOSPITALSYS, DataUser.COMM);
            str2.AppendFormat("   WHERE F.DEPT_CODE = a.dept_code(+) and A.ORGAN(+) = 'K'");
            str2.AppendFormat("     AND a.counting(+) = '{0}'", incount);
            str2.AppendFormat("    AND F.DEPT_CODE IN (SELECT info.unit_code  FROM {0}.report_unit_code info  WHERE info.report_code = '{1}') {2}", DataUser.HOSPITALSYS, report_code, power);

            str2.AppendFormat("GROUP BY F.DEPT_CODE) AA , {0}.sys_dept_dict BB where AA.DEPT_CODE = BB.DEPT_CODE(+) ", DataUser.COMM);


            str.AppendFormat(@"  select ccc.* from ({0}) ccc
                                 union all
                                 select '-1' sortno,'-1' dept_code, '合计' dept_name",str2.ToString());

            for (int i = 0; i < kdept.Rows.Count; i++)
            {
                str.AppendFormat(", to_char(SUM (a" + kdept.Rows[i]["GUIDE_CODE"].ToString() + "),'999g999g990d00') ");
            }
            str.AppendFormat("from ({0}) ccc",str2.ToString());

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }



        /// <summary>
        /// 取得报表信息
        /// </summary>
        /// <param name="rpt_classid">报表ID</param>
        /// <param name="OrganType">组织类别</param>
        /// <returns></returns>
        public DataSet getDictBuild(string rpt_classid, string OrganType)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("SELECT ID,RPT_NAME,RPT_TYPE,RPT_CLASSID,(SELECT CLASSNAME FROM {0}.report_class_dict WHERE RPT_CLASSID = CLASSID) RPT_PNAME,RPT_RANKING from {0}.REPORT_LIST_DICT WHERE rpt_classid =  ",DataUser.HOSPITALSYS);

            str.Append(rpt_classid);

            str.Append(" AND RPT_TYPE = '");

            str.Append(OrganType);

            str.Append("'");

            str.Append(" ORDER BY RPT_RANKING");

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }



        /// <summary>
        /// 取得分析报表信息
        /// </summary>
        /// <param name="rpt_classid">报表ID</param>
        /// <param name="OrganType">组织类别</param>
        /// <returns></returns>
        public DataSet getAnalyseDictBuild(string rpt_classid)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("SELECT ID,RPT_NAME,RPT_CLASSID,(SELECT CLASSNAME FROM {0}.ANALYSISRPT_CLASS_DICT WHERE RPT_CLASSID = CLASSID) RPT_PNAME,RPT_RANKING from {0}.ANALYSISRPT_LIST_DICT WHERE RPT_CLASSID =  ", DataUser.HOSPITALSYS);

            str.Append(rpt_classid);

            str.Append(" ORDER BY RPT_RANKING");

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 根据父节点查询分析报表信息
        /// </summary>
        /// <param name="rpt_pid">父节点</param>
        /// <param name="OrganType">组织类别</param>
        /// <returns></returns>
        public DataSet getAnalyseReportByPid(string rpt_pid)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT ID,RPT_NAME, RPT_CLASSID,T2.CLASSNAME RPT_PNAME
                                 FROM {0}.ANALYSISRPT_LIST_DICT t1,{0}.ANALYSISRPT_CLASS_DICT t2
                                 WHERE rpt_classid IN (SELECT classid
                                                         FROM {0}.ANALYSISRPT_CLASS_DICT
                                WHERE classpid = '{1}') and T1.RPT_CLASSID = T2.CLASSID order by T2.RANKING", DataUser.HOSPITALSYS, rpt_pid);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }



        /// <summary>
        /// 根据父节点查询报表信息
        /// </summary>
        /// <param name="rpt_pid">父节点</param>
        /// <param name="OrganType">组织类别</param>
        /// <returns></returns>
        public DataSet getReportByPid(string rpt_pid, string OrganType) 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT ID,RPT_NAME, RPT_CLASSID,RPT_TYPE,T2.CLASSNAME RPT_PNAME
                                 FROM {0}.report_list_dict t1,{0}.report_class_dict t2
                                 WHERE rpt_classid IN (SELECT classid
                                                         FROM {0}.report_class_dict
                                WHERE classpid = '{1}') AND rpt_type = '{2}' and T1.RPT_CLASSID = T2.CLASSID order by T1.RPT_RANKING", DataUser.HOSPITALSYS, rpt_pid, OrganType);

            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="pid">父节点ID</param>
        /// <param name="name">报表名称</param>
        /// <param name="organType">报表类别</param>
        /// <param name="ranking">报表顺序</param>
        public void AddReportCenterDict(string pid, string name, string organType, string ranking,string userid)
        {


            string sql = @"insert into HOSPITALSYS.REPORT_LIST_DICT(ID,RPT_CLASSID,RPT_NAME,CREATER,RPT_TYPE,RPT_RANKING) values(?,?,?,?,?,?)";
            OleDbParameter[] prams = {
                new OleDbParameter("ID",OracleOledbBase.GetMaxID("ID","hospitalsys.REPORT_LIST_DICT").ToString()),
                new OleDbParameter("RPT_CLASSID",pid),
                new OleDbParameter("RPT_NAME",name),
                new OleDbParameter("CREATER",userid),
                new OleDbParameter("RPT_TYPE",organType),
                new OleDbParameter("RPT_RANKING",ranking)
            };

            OracleOledbBase.ExecuteNonQuery(sql, prams);
        }



        /// <summary>
        /// 更新分析报表数据库
        /// </summary>
        /// <param name="pid">父节点ID</param>
        /// <param name="name">报表名称</param>
        /// <param name="organType">报表类别</param>
        /// <param name="ranking">报表顺序</param>
        public void AddAnalyseReportCenterDict(string pid, string name,string ranking, string userid)
        {


            string sql = @"insert into HOSPITALSYS.ANALYSISRPT_LIST_DICT(ID,RPT_CLASSID,RPT_NAME,CREATER,RPT_RANKING) values(?,?,?,?,?)";
            OleDbParameter[] prams = {
                new OleDbParameter("ID",OracleOledbBase.GetMaxID("ID","hospitalsys.ANALYSISRPT_LIST_DICT").ToString()),
                new OleDbParameter("RPT_CLASSID",pid),
                new OleDbParameter("RPT_NAME",name),
                new OleDbParameter("CREATER",userid),
                new OleDbParameter("RPT_RANKING",ranking)
            };

            OracleOledbBase.ExecuteNonQuery(sql, prams);
        }




        /// <summary>
        /// 修改报表
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">报表名称</param>
        public void UpdateReportCenterDict(string id, string name, string Sort)
        {
            string sql = @"update hospitalsys.REPORT_LIST_DICT set RPT_NAME=?,RPT_RANKING=? where ID=?";
            OleDbParameter[] prams = 
                {
                    new OleDbParameter("RPT_NAME",name),
                    new OleDbParameter("RPT_RANKING",Sort),
                    new OleDbParameter("ID",id)
                };

            OracleOledbBase.ExecuteNonQuery(sql, prams);
        }

        /// <summary>
        /// 排序交换位置
        /// </summary>
        /// <param name="id">报表ID</param>
        /// <param name="Tempid">交换报表ID</param>
        /// <param name="Ranking">报表位置</param>
        /// <param name="TempRanking">报表交换位置</param>
        public void ReportRankingTran(string id,string Tempid,string Ranking,string TempRanking) 
        {

            MyLists list = new MyLists();

            List l_listReport = new List();

            l_listReport.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listReport.StrSql = "update hospitalsys.REPORT_LIST_DICT set RPT_RANKING=" + TempRanking + " where ID=" + id + "";
            list.Add(l_listReport);

            List l_listDelDeptStaffCode = new List();

            l_listDelDeptStaffCode.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDelDeptStaffCode.StrSql = "update hospitalsys.REPORT_LIST_DICT set RPT_RANKING=" + Ranking + " where ID=" + Tempid + "";
            list.Add(l_listDelDeptStaffCode);

            OracleOledbBase.ExecuteTranslist(list);
        }

        /// <summary>
        /// 分析排序交换位置
        /// </summary>
        /// <param name="id">报表ID</param>
        /// <param name="Tempid">交换报表ID</param>
        /// <param name="Ranking">报表位置</param>
        /// <param name="TempRanking">报表交换位置</param>
        public void ReportAnalyseRankingTran(string id, string Tempid, string Ranking, string TempRanking)
        {

            MyLists list = new MyLists();

            List l_listReport = new List();

            l_listReport.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listReport.StrSql = "update hospitalsys.ANALYSISRPT_LIST_DICT set RPT_RANKING=" + TempRanking + " where ID=" + id + "";
            list.Add(l_listReport);

            List l_listDelDeptStaffCode = new List();

            l_listDelDeptStaffCode.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDelDeptStaffCode.StrSql = "update hospitalsys.ANALYSISRPT_LIST_DICT set RPT_RANKING=" + Ranking + " where ID=" + Tempid + "";
            list.Add(l_listDelDeptStaffCode);

            OracleOledbBase.ExecuteTranslist(list);
        }

        /// <summary>
        /// 获得报表排序最大值
        /// </summary>
        /// <param name="type">1:自由报表,2:分析报表</param>
        /// <returns>排序值</returns>
        public int getReportMaxRanking(string type,string rpttype,string classid) 
        {
            StringBuilder str = new StringBuilder();
            switch(type) 
            {
                case "1":
                    str.AppendFormat(@" SELECT NVL(MAX(RPT_RANKING) +1,0) FROM  {0}.REPORT_LIST_DICT WHERE RPT_TYPE = '{1}' AND RPT_CLASSID = '{2}'",DataUser.HOSPITALSYS,rpttype,classid);
                    break;
                case "2":
                    str.AppendFormat(@" SELECT NVL(MAX(RPT_RANKING) + 1,0) FROM  {0}.ANALYSISRPT_LIST_DICT WHERE RPT_CLASSID = '{1}'", DataUser.HOSPITALSYS, classid);
                    break;
            }
            DataSet l_ds = OracleOledbBase.ExecuteDataSet(str.ToString());
            int i = Convert.ToInt32(l_ds.Tables[0].Rows[0][0].ToString());
            return i;
        }

        /// <summary>
        /// 修改分析报表
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">报表名称</param>
        public void UpdateAnalyseReportCenterDict(string id, string name, string Sort)
        {
            string sql = @"update hospitalsys.ANALYSISRPT_LIST_DICT set RPT_NAME=?,RPT_RANKING=? where ID=?";
            OleDbParameter[] prams = 
                {
                    new OleDbParameter("RPT_NAME",name),
                    new OleDbParameter("RPT_RANKING",Sort),
                    new OleDbParameter("ID",id)
                };

            OracleOledbBase.ExecuteNonQuery(sql, prams);
        }



        /// <summary>
        /// 删除报表
        /// </summary>
        /// <param name="id"></param>
        public string DelReportCenterDict(string id)
        {
            string sql = string.Format(@"delete hospitalsys.REPORT_LIST_DICT where id = '{0}'", id);
            return sql;
        }

        /// <summary>
        /// 删除分析报表
        /// </summary>
        /// <param name="id"></param>
        public string DelAnalyseReportCenterDict(string id)
        {
            string sql = string.Format(@"delete hospitalsys.ANALYSISRPT_LIST_DICT where id = '{0}'", id);
            return sql;
        }

        /// <summary>
        /// 删除报表信息
        /// </summary>
        /// <param name="id"></param>
        public void DelReportListDictTran(string id) 
        {
            GuideDalDict dal = new GuideDalDict();

            MyLists list = new MyLists();

            List l_listDelGuide = new List();

            l_listDelGuide.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDelGuide.StrSql = dal.DeleteReportGuide(id);
            list.Add(l_listDelGuide);

            List l_listDelDeptStaffCode = new List();

            l_listDelDeptStaffCode.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDelDeptStaffCode.StrSql = DelReportDeptStaffCode(id);
            list.Add(l_listDelDeptStaffCode);

            List l_listDelReportList = new List();

            l_listDelReportList.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDelReportList.StrSql = DelReportCenterDict(id);
            list.Add(l_listDelReportList);

            OracleOledbBase.ExecuteTranslist(list);
        
        }


        /// <summary>
        /// 删除分析报表信息
        /// </summary>
        /// <param name="id"></param>
        public void DelAnalyseReportListDictTran(string id)
        {
            GuideDalDict dal = new GuideDalDict();
            MyLists list = new MyLists();
            List l_listDelGuide = new List();
            l_listDelGuide.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDelGuide.StrSql = dal.DeleteAnaylseReportGuide(id);
            list.Add(l_listDelGuide);

            List l_listDelReportList = new List();
            l_listDelReportList.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDelReportList.StrSql = DelAnalyseReportCenterDict(id);
            list.Add(l_listDelReportList);
            OracleOledbBase.ExecuteTranslist(list);

        }



        /// <summary>
        /// 删除报表行结构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DelReportDeptStaffCode(string id) 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(" DELETE FROM {1}.REPORT_UNIT_CODE WHERE REPORT_CODE = '{0}'", id, DataUser.HOSPITALSYS);
   
            return str.ToString();
        }


        /// <summary>
        /// 取得报表类别信息
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <returns></returns>
        public DataSet getReportTypeInfo(string id)
        {
            StringBuilder str = new StringBuilder();

            str.Append(" SELECT CLASSID,CLASSNAME,CLASSPID,RANKING from HOSPITALSYS.REPORT_CLASS_DICT ");
            str.Append("WHERE  CLASSPID='");
            str.Append(id);
            str.Append("' ORDER BY RANKING");

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }


        /// <summary>
        /// 修改报表类别名称
        /// </summary>class
        /// <param name="id">id</param>
        /// <param name="name">报表类别名称</param>
        public void UpdateReportTypeCenterDict(Dictionary<string, string>[] treeNodes, string DataBaseName)
        {
            if (treeNodes.Length > 0)
            {
                MyLists list = new MyLists();

                List l_listDelSql = new List();
                l_listDelSql.Parameters = new OleDbParameter[] { new OleDbParameter() };
                l_listDelSql.StrSql = string.Format(" DELETE FROM {0}.{1} ", DataUser.HOSPITALSYS, DataBaseName);
                list.Add(l_listDelSql);

                StringBuilder sqlstr = new StringBuilder();
                string sql = "";
                string classtxt = "";
                for (int i = 0; i < treeNodes.Length; i++)
                {
                    classtxt = treeNodes[i]["Text"].Length > 30 ? treeNodes[i]["Text"].Substring(1, 30) : treeNodes[i]["Text"];
                    sqlstr.AppendFormat(" SELECT '{0}' AS CLASSID, '{1}' AS CLASSNAME, '{2}' AS CLASSPID ,{3} AS RANKING FROM DUAL UNION ALL "
                        , treeNodes[i]["Id"]
                        , classtxt.Replace("'","''")
                        , treeNodes[i]["Pid"]
                        , i);
                }
                sql = sqlstr.ToString() + ")";
                sql = sql.Replace("UNION ALL )", "");
                sql = string.Format(" INSERT INTO {0}.{1} (CLASSID, CLASSNAME, CLASSPID, RANKING) ", DataUser.HOSPITALSYS, DataBaseName) + sql;

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
        public void DelReportTypeCenterDict(string id,string DataBaseName)
        {
            string sql = string.Format(@"delete {1}.{2} where CLASSID = '{0}'", id, DataUser.HOSPITALSYS, DataBaseName);
            OracleOledbBase.ExecuteNonQuery(sql, new OleDbParameter[] { });
        }


        /// <summary>
        /// 判断下级是否有数据
        /// </summary>list
        /// <param name="id">id</param>
        /// <returns>true:不存在数据 false:存在数据</returns>
        public bool EstimateData(string id,string DataBaseName)
        {
            OleDbParameter[] pram = new OleDbParameter[] { };
            int num = -1;
            string sql = string.Format("select count(*) from {1}.{2} where rpt_classid = '{0}'", id, DataUser.HOSPITALSYS, DataBaseName);
            num = Convert.ToInt32(OracleOledbBase.GetSingle(sql, pram));
            return num > 0 ? false : true;
        }


        /// <summary>
        /// 查询科室报表结构
        /// </summary>
        /// <param name="GuideTable">指标集合</param>
        /// <param name="DeptTable">部门集合</param>
        /// <returns></returns>
        public DataSet CreateDeptReportStruct(DataTable GuideTable,DataTable DeptTable) 
        {
            StringBuilder str = new StringBuilder();

            str.Append(@"SELECT  f.dept_code,  f.dept_name ");

            for (int i = 0; i < GuideTable.Rows.Count; i++)
            {

                str.Append(",'' a" + GuideTable.Rows[i]["GUIDE_CODE"].ToString());
            }

            str.Append(" FROM COMM.SYS_DEPT_DICT f WHERE ");
            str.Append(" f.dept_code IN (");

            for (int i = 0; i < DeptTable.Rows.Count; i++)
            {
                if (i == DeptTable.Rows.Count - 1)
                {
                    str.Append(DeptTable.Rows[i]["DEPT_CODE"].ToString());

                }
                else 
                {
                    str.Append(DeptTable.Rows[i]["DEPT_CODE"].ToString());
                    str.Append(",");
                }
               
            }
            str.Append(" ) GROUP BY f.dept_code, f.dept_name order by f.dept_code");

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 查询人员报表结构
        /// </summary>
        /// <param name="GuideTable">指标集合</param>
        /// <param name="DeptTable">人员集合</param>
        /// <returns></returns>
        public DataSet CreateStaffReportStruct(DataTable GuideTable, DataTable StaffTable)
        {
            StringBuilder str = new StringBuilder();

            str.Append(@"SELECT  f.STAFF_ID,  f.name ");

            for (int i = 0; i < GuideTable.Rows.Count; i++)
            {

                str.Append(",'' a" + GuideTable.Rows[i]["GUIDE_CODE"].ToString());
            }

            str.Append(" FROM rlzy.NEW_STAFF_INFO f WHERE ");
            str.Append(" f.STAFF_ID IN (");

            for (int i = 0; i < StaffTable.Rows.Count; i++)
            {
                if (i == StaffTable.Rows.Count - 1)
                {
                    str.Append(StaffTable.Rows[i]["STAFF_ID"].ToString());

                }
                else
                {

                    str.Append(StaffTable.Rows[i]["STAFF_ID"].ToString());
                    str.Append(",");
                }

            }
            str.Append(" ) GROUP BY f.STAFF_ID, f.name order by f.name");

            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }

        /// <summary>
        /// 插入权限中间表
        /// </summary>
        /// <param name="code">权限CODE</param>
        /// <returns></returns>
        public string InsertTmpPowerId(string code) 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("insert into  {0}.tmp_power_dept values('", DataUser.HOSPITALSYS);

            str.Append(code);

            str.Append("')");

            return str.ToString();
        
        
        }


        /// <summary>
        /// 删除权限中间表数据
        /// </summary>
        /// <returns></returns>
        public string DelTemPowerId() 
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("delete from {0}.tmp_power_dept",DataUser.HOSPITALSYS);

            return str.ToString();
        }


        /// <summary>
        /// 添加报表权限信息
        /// </summary>
        /// <param name="rptid">报表ID</param>
        /// <param name="terms">添加数据SQL</param>
        public void UpdateReportPowerInfoTran(ArrayList id)
        {
            MyLists list = new MyLists();

            List l_listDel = new List();

            l_listDel.Parameters = new OleDbParameter[] { new OleDbParameter() };
            l_listDel.StrSql = DelTemPowerId();
            list.Add(l_listDel);

            for (int i = 0; i < id.Count;i++ ) 
            {
                List l_listUp = new List();
                l_listUp.Parameters = new OleDbParameter[] { new OleDbParameter() };

                l_listUp.StrSql = InsertTmpPowerId(id[i].ToString());

                list.Add(l_listUp);            
            }
            OracleOledbBase.ExecuteTranslist(list);
        }


        /// <summary>
        /// 取得部门按月查询信息
        /// </summary>
        /// <param name="dd1">开始时间</param>
        /// <param name="dd2">结束时间</param>
        /// <param name="Guide_Code">指标</param>
        /// <param name="unit_code">部门CODE</param>
        /// <param name="guide_type">指标种类</param>
        /// <returns></returns>
        public DataSet getDeptByMonthReport(string dd1, string dd2, string Guide_Code, string unit_code, string guide_type,string reportid) 
        {
            string dd1YYYYmm = dd1.Substring(0, 6);
            string dd2YYYYmm = dd2.Substring(0, 6);
            StringBuilder str = new StringBuilder();
            GuideDalDict dal = new GuideDalDict();
            DataTable GuideTable = dal.getGuideCodeByReportid(reportid).Tables[0];
            str.AppendFormat(@"  SELECT   F.DEPT_CODE UNIT_CODE,A.TJYF");
            for (int i = 0; i < GuideTable.Rows.Count; i++)
            {
            str.AppendFormat(", SUM ( DECODE (a.guide_code, '" + GuideTable.Rows[i]["GUIDE_CODE"].ToString() + "', a.guide_value,0)) AS a" + GuideTable.Rows[i]["GUIDE_CODE"].ToString() + " ");
            }
            str.AppendFormat(@"   FROM (SELECT   A.TJYF, A.GUIDE_CODE, SUM (GUIDE_VALUE) AS GUIDE_VALUE,
                                   B.ACCOUNT_DEPT_CODE AS UNIT_CODE
                              FROM {1}.GUIDE_VALUE A, {0}.SYS_DEPT_DICT B
                             WHERE A.UNIT_CODE = B.DEPT_CODE
                               AND A.GUIDE_TYPE = 'K'
                               AND A.TJYF >= '{2}'
                               AND A.TJYF <= '{3}'
                          GROUP BY A.GUIDE_CODE, A.TJYF, B.ACCOUNT_DEPT_CODE) A,
                         {0}.SYS_DEPT_DICT F
                    WHERE F.DEPT_CODE = A.UNIT_CODE AND F.DEPT_CODE = '{4}'
                    GROUP BY A.TJYF,F.DEPT_CODE
                    ORDER BY F.DEPT_CODE,A.TJYF", DataUser.COMM, DataUser.HOSPITALSYS, dd1YYYYmm, dd2YYYYmm, unit_code);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 取得人员按月查询信息
        /// </summary>
        /// <param name="dd1">开始时间</param>
        /// <param name="dd2">结束时间</param>
        /// <param name="Guide_Code">指标</param>
        /// <param name="unit_code">人员CODE</param>
        /// <param name="guide_type">指标种类</param>
        /// <returns></returns>
        public DataSet getStaffByMonthReport(string dd1, string dd2, string Guide_Code, string unit_code, string guide_type,string reportid)
        {
            string dd1YYYYmm = dd1.Substring(0, 6);
            string dd2YYYYmm = dd2.Substring(0, 6);
            StringBuilder str = new StringBuilder();
            GuideDalDict dal = new GuideDalDict();
            DataTable GuideTable = dal.getGuideCodeByReportid(reportid).Tables[0];
            str.AppendFormat("SELECT   MAX(A.UNIT_CODE) AS UNIT_CODE, A.TJYF ");
            for (int i = 0; i < GuideTable.Rows.Count; i++)
            {
                str.AppendFormat(", SUM ( DECODE (a.guide_code, '" + GuideTable.Rows[i]["GUIDE_CODE"].ToString() + "', a.guide_value,0)) AS a" + GuideTable.Rows[i]["GUIDE_CODE"].ToString() + " ");
            }
            str.AppendFormat(@"FROM {0}.GUIDE_VALUE A
                                   WHERE  UNIT_CODE = '{1}' 
                                         AND A.TJYF >= '{2}'
                                       AND A.TJYF <= '{3}' 
                                       AND GUIDE_TYPE = 'R' 
                                GROUP BY A.TJYF
                                ORDER BY A.TJYF", DataUser.HOSPITALSYS, unit_code, dd1YYYYmm, dd2YYYYmm);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取可行性分析报表
        /// </summary>
        /// <param name="StartDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="incount">staff_id</param>
        /// <param name="gudie_code">指标编码</param>
        /// <param name="person">人员编码</param>
        /// <returns></returns>
        public DataSet GetSatffCorrelation(string StartDate, string EndDate, string incount, string gudie_code,string person)
        {

            StringBuilder str = new StringBuilder();


            str.AppendFormat(@"SELECT   A.GUIDE_CODE , B.GUIDE_NAME , SUM (A.GUIDE_VALUE) AS GUIDE_VALUE
                            FROM HOSPITALSYS.CORRELATION_TEMP A, HOSPITALSYS.GUIDE_NAME_DICT B
                           WHERE A.GUIDE_CODE = B.GUIDE_CODE
                             AND A.PERSON_ID = '{0}'
                             AND A.COUNTING(+) = '{1}'
                             AND A.GUIDE_CODE IN (
                                             SELECT GUIDE_CODE
                                               FROM HOSPITALSYS.GUIDE_GATHERS
                                              WHERE GUIDE_GATHER_CODE =
                                                                      (SELECT GUIDE_GATHER_CODE
                                                                         FROM HOSPITALSYS.GUIDE_NAME_DICT
                                                                        WHERE GUIDE_CODE = '{2}'))
                        GROUP BY A.GUIDE_CODE, B.GUIDE_NAME", person, incount, gudie_code);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        /// <summary>
        /// 获取可行性分析报表
        /// </summary>
        /// <param name="StartDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="incount">staff_id</param>
        /// <param name="gudie_code">指标编码</param>
        /// <param name="unit_code_p">科室编码</param>
        /// <returns></returns>
        public DataSet GetDeptCorrelation(string StartDate, string EndDate, string incount, string gudie_code, string unit_code_p, string person)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT   A.GUIDE_CODE , B.GUIDE_NAME , SUM (A.GUIDE_VALUE) AS GUIDE_VALUE
                            FROM HOSPITALSYS.CORRELATION_TEMP A, HOSPITALSYS.GUIDE_NAME_DICT B
                           WHERE A.GUIDE_CODE = B.GUIDE_CODE
                             AND A.DEPT_CODE = '{0}'
                             AND A.COUNTING(+) = '{1}'
                             AND A.GUIDE_CODE IN (
                                             SELECT GUIDE_CODE
                                               FROM HOSPITALSYS.GUIDE_GATHERS
                                              WHERE GUIDE_GATHER_CODE =
                                                                      (SELECT GUIDE_GATHER_CODE
                                                                         FROM HOSPITALSYS.GUIDE_NAME_DICT
                                                                        WHERE GUIDE_CODE = '{2}'))
                        GROUP BY A.GUIDE_CODE, B.GUIDE_NAME", unit_code_p, incount, gudie_code);
            return OracleOledbBase.ExecuteDataSet(str.ToString());

        }


        /// <summary>
        /// 取得部门钻取人员报表信息SQL
        /// </summary>
        /// <param name="incount">STAFFID</param>
        /// <param name="dept_code">部门CODE</param>
        /// <param name="kdept">指标集合</param>
        /// <returns></returns>
        public string ReportDeptSql(string incount, string dept_code, DataTable kdept)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"SELECT f.STAFF_ID STAFF_ID,   f.name NAME ");

            for (int i = 0; i < kdept.Rows.Count; i++)
            {
                if (kdept.Rows[i]["GUIDE_CODE"].ToString() != "")
                {
                    str.Append(",SUM (CASE WHEN a.guide_code='");
                    str.Append(kdept.Rows[i]["GUIDE_CODE"].ToString());
                    str.Append("' THEN a.guide_value ELSE 0 end ) AS ");
                    str.Append("a" + kdept.Rows[i]["GUIDE_CODE"].ToString());
                }
            }
            str.Append(" FROM hospitalsys.report_temp a, rlzy.new_staff_info f WHERE f.staff_id = a.person_id(+)");
            str.Append(" and A.ORGAN(+) = 'R' and a.COUNTING(+)='" + incount + "'");
            str.Append(" AND f.staff_id IN (");
            str.Append(string.Format(" SELECT info.STAFF_ID FROM RLZY.NEW_STAFF_INFO info WHERE info.DEPT_CODE = '{0}'", dept_code));
            str.Append(") GROUP BY f.staff_id,f.name order by f.name");
            return str.ToString();
        }

        /// <summary>
        /// 取得部门钻取人员报表信息
        /// </summary>
        /// <param name="incount">STAFFID</param>
        /// <param name="dept_code">部门CODE</param>
        /// <param name="kdept">指标集合</param>
        /// <returns></returns>
        public DataSet getReportDept(string incount, string report_code, string dept_code)
        {
            DataTable l_dt = new DataTable();

            string sql = "select (SELECT MEMBER_GUIDE_CODE FROM HOSPITALSYS.GUIDE_HOSPITAL_DEPT_MEMBER T1 WHERE T1.DEPT_GUIDE_CODE=guide_code) AS  GUIDE_CODE from hospitalsys.REPORTINFO where REPORT_CODE='" + report_code + "'";

            DataTable l_kdept = OracleOledbBase.ExecuteDataSet(sql).Tables[0];

            return OracleOledbBase.ExecuteDataSet(ReportDeptSql(incount, dept_code, l_kdept));
        }


        /// <summary>
        /// 生成分析报表数据
        /// </summary>
        /// <returns></returns>
        public DataSet getAnalyseReport(string incount, string rpt_id) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT T3.GUIDE_NAME, T1.GUIDE_CODE, T1.UNIT_CODE, T1.GUIDE_VALUE,
                                       T1.GUIDE_VALUE_YEAR, T1.GUIDE_CAUSE, T1.GUIDE_FACT,
                                       T1.GUIDE_RATIO_TONG, T1.GUIDE_VALUE_TONG, T1.GUIDE_RATIO_HUAN,
                                       T1.GUIDE_VALUE_HUAN, T1.GUIDE_WCB_MONTH, T1.GUIDE_WCB_YEAR, T1.ORGAN
                                  FROM {0}.ANALYSISRPT_TEMP T1,
                                       {0}.ANALYSISREPORTINFO T2,
                                       {0}.GUIDE_NAME_DICT T3
                                 WHERE T1.GUIDE_CODE(+) = T2.GUIDE_CODE AND T2.GUIDE_CODE = T3.GUIDE_CODE AND T1.COUNTING(+) = '{1}' AND T2.REPORT_CODE = {2} ORDER BY T2.RANKING", DataUser.HOSPITALSYS, incount, rpt_id);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }


        public DataSet getAnalyseReportGuideByRptId(string rpt_id) 
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT GUIDE_CODE FROM  {0}.ANALYSISREPORTINFO WHERE REPORT_CODE = {1} ORDER BY RANKING", DataUser.HOSPITALSYS, rpt_id);
            return OracleOledbBase.ExecuteDataSet(str.ToString()); 
        }

        #endregion
    }
}
