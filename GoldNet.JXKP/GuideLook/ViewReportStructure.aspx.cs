using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Text;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.GuideLook
{
    public partial class ViewReportStructure : PageBase
    {

        #region --页面初始化--

        #endregion

        #region --页面事件--
        protected void Page_Load(object sender, EventArgs e)
        {

            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }

            if (!Ext.IsAjaxRequest)
            {
                DataTable kdept = new DataTable();
                string incount = ((User)Session["CURRENTSTAFF"]).StaffId;
                if (incount == "" || incount == null) incount = "NotUserid";
                ReportDalDict dal = new ReportDalDict();
                string dd1 = DateTime.Now.AddMonths(-1).ToString("yyyyMMdd");
                string dd2 = DateTime.Now.ToString("yyyyMMdd");
                string rptid = Request.QueryString["Reportid"].ToString();
                string OrganType = Request.QueryString["OrganType"].ToString();
                DataTable l_dt = dal.getReportTerms(rptid).Tables[0];
                string l_terms = "";
                GuideDalDict GuideDal = new GuideDalDict();
                DataTable l_GuideName = GuideDal.GuideName().Tables[0];
                //取得报表指标
                DataTable l_GuideTable = GuideDal.getGuideCodeByReportid(rptid).Tables[0];
                DataTable l_Table = new DataTable();

                if (OrganType == "R")
                {
                    //StaffDalDict StaffDal = new StaffDalDict();
                    //l_Table = StaffDal.AnalyzeTermsToStaffId(l_terms).Tables[0];
                    //DataTable StaffReportInfo = dal.CreateStaffReportStruct(l_GuideTable, l_Table).Tables[0];
                    l_terms = l_dt.Rows[0]["RPT_TERMS"].ToString();
                    string station = l_dt.Rows[0]["PRT_SECOND_TERMS"].ToString();
                    if (l_terms !="") 
                    {
                        string StaffCodeSql = AnalyzeStringToStaffSql(l_terms, rptid, station);
                        dal.UpdateReportInfoTran(rptid, StaffCodeSql);
                        DataTable StaffReportInfo = dal.getReportDetailInfo(dd1, dd2, "R", incount, rptid, kdept, "","").Tables[0];
                        CreateStaffReportPanel(StaffReportInfo, l_GuideName);
                    }
 
                }
                if (OrganType == "K")
                {
                    string DeptTreeType = Request.QueryString["DeptTreeType"].ToString();
                    string DeptCodeSql = "";
                    if (DeptTreeType == "0")
                    {
                        l_terms = l_dt.Rows[0]["RPT_TERMS"].ToString();
                        DeptCodeSql = AnalyzeStringToSql(l_terms, rptid);
                    }
                    else 
                    {
                        l_terms = l_dt.Rows[0]["PRT_SECOND_TERMS"].ToString();
                        DeptCodeSql = AnalyzeStringToDeptSecondSql(l_terms, rptid);
                    }
                    
                    dal.UpdateReportInfoTran(rptid, DeptCodeSql);
                    //运行报表和相关性分析的存储过程
                    DataTable DeptReportInfo = dal.getReportDetailInfo(dd1, dd2, "K", incount, rptid, kdept, "","").Tables[0];
                    //DeptDalDict DeptDal = new DeptDalDict();
                    //l_Table = DeptDal.AnalyzeTermsToDeptCode(l_terms).Tables[0];
                    //DataTable DeptReportInfo = dal.CreateDeptReportStruct(l_GuideTable, l_Table).Tables[0];
                    CreateDeptReportPanel(DeptReportInfo, l_GuideName);
                }

                //}
            }
        }
        #endregion

        #region --页面逻辑--
        /// <summary>
        /// 自定义课时报表结构集合
        /// </summary>
        /// <param name="ReportInfo">报表指标</param>
        /// <param name="ReportColName">指标名称</param>
        private void CreateDeptReportPanel(DataTable ReportInfo, DataTable ReportColName)
        {

            //ReportInfo.Columns.Remove("tlevel");

            ReportInfo.Columns.Remove("sortno");

            this.Store1.RemoveFields();
            this.GridPanel_Show.Reconfigure();
            for (int i = 0; i < ReportInfo.Columns.Count; i++)
            {
                RecordField field = new RecordField(ReportInfo.Columns[i].ColumnName, RecordFieldType.String);

                this.Store1.AddField(field, i);

                Column col = new Column();
                //不显示ID
                if (ReportInfo.Columns[i].ColumnName.Equals("DEPT_CODE"))
                {
                    continue;
                }
                if (ReportInfo.Columns[i].ColumnName.Equals("DEPT_NAME"))
                {

                    col.Header = "科室";
                    col.Width = col.Header.Length * 50;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;

                }
                if (ReportInfo.Columns[i].ColumnName.Substring(0, 1).Equals("A"))
                {
                    //提取指标名称
                    DataRow[] l_dr = ReportColName.Select("GUIDE_CODE = " + ReportInfo.Columns[i].ColumnName.Substring(1, ReportInfo.Columns[i].ColumnName.Length - 1));
                    if (l_dr.Length > 0) 
                    {
                        col.Header = l_dr[0]["GUIDE_NAME"].ToString().Replace("（科）", "").Replace("(科)", "").Replace("（科)", "").Replace("(科）", "");
                    }
                    col.Width = col.Header.Length * 15;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;

                }

                this.GridPanel_Show.AddColumn(col);

            }

            this.Store1.DataSource = ReportInfo;
            this.Store1.DataBind();


        }

        /// <summary>
        /// 自定义人员指标集合
        /// </summary>
        /// <param name="ReportInfo">报表指标</param>
        /// <param name="ReportColName">指标名称</param>
        private void CreateStaffReportPanel(DataTable ReportInfo, DataTable ReportColName)
        {
            this.Store1.RemoveFields();

            this.GridPanel_Show.Reconfigure();


            for (int i = 0; i < ReportInfo.Columns.Count; i++)
            {
                RecordField field = new RecordField(ReportInfo.Columns[i].ColumnName, RecordFieldType.String);

                this.Store1.AddField(field, i);

                Column col = new Column();

                if (ReportInfo.Columns[i].ColumnName.Equals("STAFF_ID"))
                {
                    continue;
                }
                if (ReportInfo.Columns[i].ColumnName.Equals("NAME"))
                {
                    col.Header = "姓名";
                    col.Width = col.Header.Length * 50;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;
                }
                if (ReportInfo.Columns[i].ColumnName.Substring(0, 1).Equals("A"))
                {
                    DataRow[] l_dr = ReportColName.Select("GUIDE_CODE = " + ReportInfo.Columns[i].ColumnName.Substring(1, ReportInfo.Columns[i].ColumnName.Length - 1));
                    if(l_dr.Length > 0) 
                    {
                        col.Header = l_dr[0]["GUIDE_NAME"].ToString().Replace("（人）", "").Replace("(人)", "").Replace("（人)", "").Replace("(人）", "");
                    }
                    col.Width = col.Header.Length * 15;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;
                }
                this.GridPanel_Show.AddColumn(col);
            }

            this.Store1.DataSource = ReportInfo;
            this.Store1.DataBind();
        }

        private string AnalyzeStringToDeptSecondSql(string terms, string rptid) 
        {
            string[] temp = terms.Split(';');

            string Sort = temp[temp.Length - 1];
            //string deptCode = RightItems.TrimEnd(new char[] { ',' }).Replace(",", "','").Insert(0, "'") + "'";
            //string deptCode = "*";
            //if (RightItems != "*")
            //{
            //    deptCode = RightItems.TrimEnd(new char[] { ',' }).Replace(",", "','").Insert(0, "'") + "'";
            //}
            StringBuilder str = new StringBuilder();

            if (Sort == "1")
            {
                str.AppendFormat(@"SELECT '{1}' REPORT_CODE,DEPT_CODE 
                                FROM {0}.SYS_DEPT_DICT 
                                WHERE DEPT_CODE_SECOND IN ({2}) AND DEPT_CODE NOT IN ({3})", DataUser.COMM, rptid, (temp[0].TrimEnd(new char[] { ',' }).Replace(",", "','").Insert(0, "'") + "'"), temp[1] == "*" ? "'-1'" : (temp[1].TrimEnd(new char[] { ',' }).Replace(",", "','").Insert(0, "'") + "'"));

            }
            else 
            {
                str.AppendFormat(@"SELECT '{1}' REPORT_CODE,DEPT_CODE 
                                FROM {0}.SYS_DEPT_DICT 
                                WHERE DEPT_CODE IN ({2})", DataUser.COMM, rptid, (temp[1].TrimEnd(new char[] { ',' }).Replace(",", "','").Insert(0, "'") + "'"));
            }
            return str.ToString();
        }

        /// <summary>
        /// 通过条件字符转化SQL语句
        /// </summary>
        /// <param name="terms">条件字符</param>
        /// <param name="rptid">报表ID</param>
        /// <returns>SQL语句</returns>
        private string AnalyzeStringToSql(string terms, string rptid)
        {
            string[] AnalyzeString = terms.Split(';');

            StringBuilder str = new StringBuilder();

            str.Append("SELECT '");

            str.Append(rptid);

            str.Append("' report_code ,");

            str.Append(" DEPT_CODE from comm.SYS_DEPT_DICT where attr='是' ");

            if (AnalyzeString[AnalyzeString.Length - 1] == "0")
            {
                str.Append(" and DEPT_CODE in ( ");

                str.Append(AnalyzeString[AnalyzeString.Length - 2].ToString().TrimEnd(new char[] { ',' }).Replace(",", "','").Insert(0, "'") + "'");

                str.Append(")");

            }
            else
            {
                if (AnalyzeString[0] != "*" || AnalyzeString[1] != "*")
                {
                    str.Append(" and (");

                }

                if (AnalyzeString[0] != "*")
                {

                    str.Append("( ");

                    str.Append(" dept_type = 0");

                    str.Append(" and DEPT_LCATTR in  ");

                    str.Append("( ");

                    string[] lcdcode = AnalyzeString[0].Split(',');

                    for (int x = 0; x < lcdcode.Length; x++)
                    {
                        if (x == lcdcode.Length - 1)
                        {
                            str.Append(lcdcode[x]);
                        }
                        else
                        {
                            str.Append(lcdcode[x]);
                            str.Append(",");
                        }
                    }
                    str.Append(" ) ) ");
                }

                if (AnalyzeString[0] != "*" && AnalyzeString[1] != "*")
                {
                    str.Append(" or  ");
                }

                if (AnalyzeString[1] != "*")
                {
                    str.Append(" dept_type in ( ");

                    string[] deptType = AnalyzeString[1].Split(',');

                    for (int y = 0; y < deptType.Length; y++)
                    {
                        if (y == deptType.Length - 1)
                        {
                            str.Append(deptType[y]);
                        }
                        else
                        {
                            str.Append(deptType[y]);
                            str.Append(",");
                        }
                    }
                    str.Append("  )");
                }

                if (AnalyzeString[0] != "*" || AnalyzeString[1] != "*")
                {
                    str.Append(" ) ");
                }

                if (AnalyzeString[2] != "*")
                {
                    str.Append(" and DEPT_CODE not in ( ");

                    string[] deptCodeNotIn = AnalyzeString[2].Split(',');

                    for (int z = 0; z < deptCodeNotIn.Length; z++)
                    {
                        if (z == deptCodeNotIn.Length - 1)
                        {
                            if (z == 0)
                            {
                                str.Append("'");
                            }
                            str.Append(deptCodeNotIn[z]);
                            str.Append("'");
                        }
                        else
                        {
                            if(z == 0) 
                            {
                                str.Append("'");
                            }
                            str.Append(deptCodeNotIn[z]);
                            str.Append("','");
                        }
                    }
                    str.Append("  )");
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 分析条件字符转化SQL语句
        /// </summary>
        /// <param name="terms">条件字符</param>
        /// <param name="rptid">报表ID</param>
        /// <returns>SQL语句</returns>
        private string AnalyzeStringToStaffSql(string terms, string rptid,string station)
        {

            string[] StaffCodes = terms.Split(';');

            string[] ComboxInfos = StaffCodes[0].Split(',');

            string where = "select '" + rptid + "' report_code, staff_id DEPT_CODE  from rlzy.NEW_STAFF_INFO where add_mark='1'";

            if (StaffCodes[StaffCodes.Length - 1].Equals("1"))
            {
                if (ComboxInfos[0].ToString() != "*")
                {
                    where = where + "and staffsort " + this.Stringfilter(ComboxInfos[0].ToString().Trim());
                }

                if (ComboxInfos[1].ToString() != "*")
                {
                    where = where + "  and techincclass " + this.Stringfilter(ComboxInfos[1].ToString());
                }

                if (ComboxInfos[2].ToString() != "*")
                {
                    where = where + "  and title_list " + this.Stringfilter(ComboxInfos[2].ToString());
                }

                if (ComboxInfos[3].ToString() != "*")
                {
                    where = where + " and TechnicClassDate is not null and ";

                    where = where + " substr(nvl(TechnicClassDate,'') ,1,4)|| lpad(replace(replace(replace(substr(nvl(TechnicClassDate,'') ,6,2),'.',''),'-',''),'/',''),2,'0')||'01' ";

                    where = where + ComboxInfos[3].Split('$')[0].ToString() + ComboxInfos[3].Split('$')[1] + " ";
                }

                if (ComboxInfos[4].ToString() != "*")
                {
                    where = where + "  and edu1 " + this.Stringfilter(ComboxInfos[4].ToString());
                }
                if (ComboxInfos[5].ToString() != "*")
                {
                    where = where + "  and CivilServiceClass " + this.Stringfilter(ComboxInfos[5].ToString());
                }
                if (ComboxInfos[6].ToString() != "*")
                {
                    where = where + "  and dept_code ='" + ComboxInfos[6].ToString() + "'";
                }

                if (StaffCodes[StaffCodes.Length - 2] != "*")
                {
                    where = where + " and staff_id not in (" + StaffCodes[StaffCodes.Length - 2] + ")";
                }
                if (station != "*") 
                {
                    where = where + " and station_code = '" + station + "'";
                }
            }
            else
            {
                where = where + " and staff_id in (" + StaffCodes[StaffCodes.Length - 2] + ")";

            }


            return where;

        }

        /// <summary>
        /// 过滤提取信息
        /// </summary>
        /// <param name="item">字符</param>
        /// <returns></returns>
        private string Stringfilter(string item)
        {
            if (item == "为空" || item == "")
            {
                item = " is null ";
            }
            else
            {
                item = " = '" + item + "'";
            }

            return item;
        }



        #endregion

    }
}
