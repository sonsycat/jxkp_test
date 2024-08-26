using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;
using GoldNet.Comm.ExportData;
using System.Web;

namespace GoldNet.JXKP.GuideLook
{
    public partial class ViewStaffReport : PageBase
    {
        public static Goldnet.Ext.Web.TreeNode root = null;

        private static string m_incount = null;

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

                setTimeContrl(); 

                m_incount = ((User)Session["CURRENTSTAFF"]).StaffId;

                if (m_incount == "" || m_incount == null) m_incount = "NotUserid";

                root = new Goldnet.Ext.Web.TreeNode();

                root.NodeID = "root";

                root.Text = "人员自由报表";

                root.Expanded = true;

                this.TreeCtrl.Root.Add(root);

                TreeBuild();
            }
        }

        /// <summary>
        /// 树结构初始化
        /// </summary>
        private void TreeBuild()
        {

            TreeDalDict dal = new TreeDalDict();

            ReportDalDict rdal = new ReportDalDict();

            DataTable l_TreeDt = dal.getReportInfoTreeBuilder("R", this.DeptFilter(""), ((User)Session["CURRENTSTAFF"]).StaffId, ((User)Session["CURRENTSTAFF"]).UserId).Tables[0];

            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.NodeID = l_TreeDt.Rows[i]["ID"].ToString();

                node.Text = l_TreeDt.Rows[i]["CLASSNAME"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                string pid = l_TreeDt.Rows[i]["CLASSPID"].ToString();
                node.SingleClickExpand = true;
                nodekey = pid;
                if (rootnode.Contains(nodekey))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[nodekey];
                    pnode.Nodes.Add(node);
                }
                else
                {
                    root.Nodes.Add(node);
                }
                rootnode.Add(node.NodeID, node);
            }

        }

        /// <summary>
        /// 时间转化
        /// </summary>
        /// <param name="time">字符串</param>
        /// <param name="split">时间分割符</param>
        /// <returns></returns>
        private string ConvertStringToTime(string time, string split)
        {
            string y = time.Substring(0, 4);

            string m = time.Substring(4,2);

            string d = time.Substring(6);
            if (HideStartDate.Text == "2")
            {
                time = y + split + m;
            }
            else 
            {
                time = y + split + m + split + d;
            }
            return time;
        }

        /// <summary>
        /// 点击人员指标明细数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StaffGuideDetail(object sender, AjaxEventArgs e)
        {
            string id = e.ExtraParams["NodeId"].ToString().Replace("\"", "");
            string url = this.getPageResponseUrl("ReportDetailGuide.aspx",id);
            if (url != "")
            {
                this.PageResponse(url, "人员指标明细数据");
            }
        }

        /// <summary>
        /// 点击按月分析指标值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StaffByMonthGuide(object sender, AjaxEventArgs e)
        {
            string id = e.ExtraParams["NodeId"].ToString().Replace("\"", "");

            string DeptCode = this.HiddenPath.Value.ToString();
            string DeptName = DeptCode.Split('&')[2];
            string url = this.getPageResponseUrl("ViewSatffByMonth.aspx",id);
            if (url != "")
            {
                this.PageResponse(url, "按月分析指标值----" + DeptName.Replace("Name=", ""));
            }

        }

        /// <summary>
        /// 点击查看相关性信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SatffGuideRelation(object sender, AjaxEventArgs e)
        {
            string id = e.ExtraParams["NodeId"].ToString().Replace("\"", "");
            string url = this.getPageResponseUrl("ViewStaffGuideRelation.aspx",id);
            if (url != "")
            {
                this.PageResponse(url, "查看相关性信息");
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            string id = e.ExtraParams["NodeId"].ToString().Replace("\"", "");
            string name = e.ExtraParams["NodeName"].ToString().Replace("\"", "");
            if (id != "" && id.IndexOf("CLASS") != 0)
            {
                DataTable kdept = new DataTable();

                string dd1 = DateTimeConvert(this.Comb_StartYear.SelectedItem.Value, this.Comb_StartMonth.SelectedItem.Value,this.Comb_StartDate,2);
                string dd2 = DateTimeConvert(this.Comb_EndYear.SelectedItem.Value, this.Comb_EndMonth.SelectedItem.Value, this.Comb_EndDate,1);

                DateTime time1 = default(DateTime);
                DateTime time2 = default(DateTime);
                if ((!DateTime.TryParse(dd1, out time1)) || (!DateTime.TryParse(dd2, out time2)))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "日期不正确,请重新选择条件",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                if (dd1.CompareTo(dd2) > 0) 
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "日期范围不正确,请重新选择条件",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }

                string terms = "";
                string station = "";
                ReportDalDict dal = new ReportDalDict();

                DataTable l_dt = dal.getReportTerms(id).Tables[0];

                if (l_dt.Rows.Count > 0)
                {
                    terms = l_dt.Rows[0]["RPT_TERMS"].ToString();
                    station = l_dt.Rows[0]["PRT_SECOND_TERMS"].ToString();
                }

                if (terms != "")
                {
                    string StaffCodeSql = AnalyzeStringToSql(terms, id, station);

                    dal.UpdateReportInfoTran(id, StaffCodeSql);

                    DataTable Rptdt = dal.getReportDetailInfo(dd1.Replace("-", ""), dd2.Replace("-", ""), "R", m_incount, id, kdept, this.DeptFilter(""), ((User)Session["CURRENTSTAFF"]).StaffId).Tables[0];
                    Session["RptStaffTable"] = Rptdt;
                    GuideDalDict GuideDal = new GuideDalDict();

                    DataTable l_dtName = GuideDal.GuideName().Tables[0];

                    DataTable l_dtIshighGuide = GuideDal.getGuideIshighGuideByReportid(id).Tables[0];

                    CreateIsHighGuide(l_dtIshighGuide);

                    CreateReportPanel(Rptdt, l_dtName);

                    this.GridPanel_Show.Title = "报表信息----" + name + "统计区间：" + ConvertStringToTime(dd1.Replace("-",""), ".") + "-" + ConvertStringToTime(dd2.Replace("-",""), ".");
                
                }
            }
        }


        private void CreateIsHighGuide(DataTable l_dt) 
        {
            string isHigh = "";

            for (int i = 0; i < l_dt.Rows.Count;i++ )
            {
                isHigh += l_dt.Rows[i][1].ToString() + ",";
            }
            isHighGuide.Text = isHigh.TrimEnd(new char[] { ',' });
        }

        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["RptStaffTable"] == null)
            {
                return;
            }
            GuideDalDict GuideDal = new GuideDalDict();

            DataTable l_dtName = GuideDal.GuideName().Tables[0];

            ExportData ex = new ExportData();
            DataTable ExcelTable = (DataTable)Session["RptStaffTable"];
            DataRow dr = ExcelTable.NewRow();
            //dr[0]
            dr[0] = "-1";
            dr[1] = "合计";
            for (int i = 2; i < ExcelTable.Columns.Count; i++)
            {
                double sum = 0;
                string name = "";
                for (int j = 0; j < ExcelTable.Rows.Count; j++)
                {
                    sum = sum + Convert.ToDouble(ExcelTable.Rows[j][i].ToString());
                }
                dr[i] = sum.ToString();
                DataRow[] l_dr = l_dtName.Select("GUIDE_CODE =" + ExcelTable.Columns[i].ColumnName.TrimStart(new char[] { 'A' }));
                if (l_dr.Length > 0)
                {
                    name = l_dr[0][1].ToString();
                }
                ExcelTable.Columns[i].ColumnName = name;
            }

            ExcelTable.Columns[1].ColumnName = "人员";
            ExcelTable.Rows.InsertAt(dr, ExcelTable.Rows.Count);
            ExcelTable.Columns.RemoveAt(0);

            outexcel(ExcelTable, "自由报表");
        }



        /// <summary>
        /// 分析条件字符转化SQL语句
        /// </summary>
        /// <param name="terms">条件字符</param>
        /// <param name="rptid">报表ID</param>
        /// <returns>SQL语句</returns>
        private string AnalyzeStringToSql(string terms, string rptid,string stationCode)
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
                if(stationCode != "*") 
                {
                    where = where + " and station_code = '" + stationCode + "'";
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
            if (item == "为空" || item=="")
            {
                item = " is null ";
            }
            else
            {
                item = " = '" + item + "'";
            }

            return item;
        }

        /// <summary>
        /// 自定义报表
        /// </summary>
        /// <param name="ReportInfo">报表信息</param>
        /// <param name="ReportColName">指标名称</param>
        private void CreateReportPanel(DataTable ReportInfo, DataTable ReportColName)
        {

            this.Store1.RemoveFields();

            this.GridPanel_Show.Reconfigure();

            RecordField field = null;
            for (int i = 0; i < ReportInfo.Columns.Count; i++)
            {
                if (ReportInfo.Columns[i].ColumnName.Equals("STAFF_ID") || ReportInfo.Columns[i].ColumnName.Equals("NAME"))
                {
                    field = new RecordField(ReportInfo.Columns[i].ColumnName, RecordFieldType.String);
                }
                else
                {
                    field = new RecordField(ReportInfo.Columns[i].ColumnName, RecordFieldType.Float);
                }

                this.Store1.AddField(field, i);

                Column col = new Column();

                if (ReportInfo.Columns[i].ColumnName.Equals("STAFF_ID"))
                {
                    col.Hidden = true;
                    col.Width = col.Header.Length * 50;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;
                    col.MenuDisabled = true;

                }
                if (ReportInfo.Columns[i].ColumnName.Equals("NAME"))
                {
                    col.Header = "姓名";
                    col.Width = col.Header.Length * 50;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;
                    col.MenuDisabled = true;
                }
                if (ReportInfo.Columns[i].ColumnName.Substring(0, 1).Equals("A"))
                {
                    DataRow[] l_dr = ReportColName.Select("GUIDE_CODE = " + ReportInfo.Columns[i].ColumnName.Substring(1, ReportInfo.Columns[i].ColumnName.Length - 1));

                    DataRow[] l_dr1 = ReportInfo.Select("STAFF_ID='-1'");

                    string header = "";
                    
                    if (l_dr.Length > 0) 
                    {
                        header = l_dr[0]["GUIDE_NAME"].ToString().Replace("（人）", "").Replace("(人)", "").Replace("（人)", "").Replace("(人）", "");
                        col.Header = l_dr[0]["GUIDE_NAME"].ToString().Replace("（人）", "").Replace("(人)", "").Replace("（人)", "").Replace("(人）", "") + "<br>" + "合计:" + l_dr1[0][i] + ""; ;
                    }
                    col.Width = header.Length * 25;
                    col.Sortable = true;
                    col.DataIndex = ReportInfo.Columns[i].ColumnName;
                    col.Align = Alignment.Right;
                    col.Renderer.Fn = "rmbMoney";
                    col.MenuDisabled = true;
                }
                this.GridPanel_Show.AddColumn(col);
            }
            DataRow[] l_dr2 = ReportInfo.Select("STAFF_ID='-1'");
            ReportInfo.Rows.Remove(l_dr2[0]);
            this.Store1.DataSource = ReportInfo;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 跳转页面
        /// </summary>
        /// <param name="url">跳转地址以及参数</param>
        /// <param name="title">窗体名称</param>
        private void PageResponse(string url, string title)
        {

            LoadConfig loadcfg = new LoadConfig();

            loadcfg.Url = url;

            loadcfg.Mode = LoadMode.IFrame;

            loadcfg.MaskMsg = "载入中...";

            loadcfg.ShowMask = true;

            loadcfg.NoCache = true;

            this.arcEditWindow.Title = title;

            arcEditWindow.ClearContent();

            arcEditWindow.Show();

            arcEditWindow.LoadContent(loadcfg);

        }

        /// <summary>
        /// 取得地址参数
        /// </summary>
        /// <param name="PageUrl">跳转地址</param>
        /// <returns>跳转地址以及参数</returns>
        private string getPageResponseUrl(string PageUrl,string NodeId)
        {
            string id = NodeId;
            string DeptCode = this.HiddenPath.Value.ToString();

            string Dept = DeptCode.Split('&')[0];

            string GuideCode = DeptCode.Split('&')[1];

            string dd1 = DateTimeConvert(this.Comb_StartYear.SelectedItem.Value, this.Comb_StartMonth.SelectedItem.Value, this.Comb_StartDate,2);

            string dd2 = DateTimeConvert(this.Comb_EndYear.SelectedItem.Value, this.Comb_EndMonth.SelectedItem.Value, this.Comb_EndDate,1);

            DateTime time1 = default(DateTime);
            DateTime time2 = default(DateTime);
            if ((!DateTime.TryParse(dd1, out time1)) || (!DateTime.TryParse(dd2, out time2)))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "日期不正确,请重新选择条件",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return "";
            }
            if (dd1.CompareTo(dd2) > 0)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "日期范围不正确,请重新选择条件",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return "";
            }

            string ResponseUrl = "" + PageUrl + "?ReportCode=" + id + "&" + Dept + "&FromDate=" + dd1.Replace("-","") + "&ToDate=" + dd2.Replace("-","") + "&" + GuideCode + "";

            return ResponseUrl;
        }

        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public string MenuItemJuge(string GuideCode, string Staffid)
        {
            //1:不可用,0:可用
            string Correlation = "1";
            string GuideExpressions = "1";

            ReportDalDict dal = new ReportDalDict();
            DataTable dt = dal.GetSatffCorrelation("", "", m_incount, GuideCode, Staffid).Tables[0];
            if(dt.Rows.Count > 0) 
            {
                Correlation = "0";
            }
            GuideDalDict gdal = new GuideDalDict();
            DataTable l_dt = gdal.getGuideExpressions(GuideCode).Tables[0];
            if (l_dt.Rows.Count > 0) 
            {
                if (l_dt.Rows[0]["GUIDE_SQL_DETAIL"] != null && l_dt.Rows[0]["GUIDE_SQL_DETAIL"].ToString() != "") 
                {
                    GuideExpressions = "0";
                }
            }
            string Juge = Correlation +"*"+ GuideExpressions;
            return Juge;
        }


        /// <summary>
        /// 初始化时间空件
        /// </summary>
        private void setTimeContrl()
        {
            //开始时间
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_StartMonth.SelectedIndex = 0;

            for (int i = 1; i <= 31; i++)
            {
                this.Comb_StartDate.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_StartDate.SelectedIndex = 0;


            //结束时间
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_EndYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_EndMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_EndMonth.SelectedIndex = DateTime.Now.Month - 1;


            for (int i = 1; i <= 31; i++)
            {
                this.Comb_EndDate.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_EndDate.SelectedIndex = DateTime.Now.Day - 1;
        }

        /// <summary>
        /// 时间转化格式
        /// </summary>
        /// <param name="year">yyyy</param>
        /// <param name="month">m or mm</param>
        /// <param name="date">日期选项</param>
        /// <param name="i">2:开始日1:结束日</param>
        /// <returns>yyyyMMdd</returns>
        private string DateTimeConvert(string year, string month, ComboBox cbo, int i)
        {
            string l_month = month;
            string l_date = cbo.SelectedItem.Value;
            if (Convert.ToInt32(l_month) < 10)
            {
                l_month = "0" + l_month;
            }
            else if (Convert.ToInt32(l_month) > 12)
            {
                l_month = "12";
            }
            l_date = l_date.Length == 1 ? "0" + l_date : l_date;
            switch (i)
            {
                case 1:
                    if (HiddenEndDate.Text == "2")
                    {
                        l_date = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)).ToString();
                    }
                    break;
                case 2:
                    if (HideStartDate.Text == "2")
                    {
                        l_date = "01";
                    }
                    break;
            }
            return year + "-" + l_month + "-" + l_date;
        }
    }
}
