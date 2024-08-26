using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using System.Text;
using GoldNet.Comm;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook
{
    public partial class SetStation : System.Web.UI.Page
    {
        #region --页面初始化--
        /// <summary>
        /// 报表ID
        /// </summary>
        private static string m_rptid = null;

        /// <summary>
        /// 初始化人员属性选项
        /// </summary>
        private void InitComboxValue()
        {
            StaffDalDict dal = new StaffDalDict();

            DataTable l_Sortdt = dal.getPSort().Tables[0];

            this.cbx_Ptype.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_Sortdt.Rows.Count; i++)
            {
                this.cbx_Ptype.Items.Add(new Goldnet.Ext.Web.ListItem(l_Sortdt.Rows[i]["ID"].ToString(), l_Sortdt.Rows[i]["ID"].ToString()));

            }
            this.cbx_Ptype.Value = "全部";


            DataTable l_Techdt = dal.getTechnicclass().Tables[0];

            this.cbx_PTechType.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_Techdt.Rows.Count; i++)
            {
                this.cbx_PTechType.Items.Add(new Goldnet.Ext.Web.ListItem(l_Techdt.Rows[i]["techID"].ToString(), l_Techdt.Rows[i]["techID"].ToString()));

            }

            this.cbx_PTechType.Value = "全部";



            DataTable l_leveldt = dal.getCivilserviceclass().Tables[0];

            this.cbx_PLevel.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_leveldt.Rows.Count; i++)
            {
                this.cbx_PLevel.Items.Add(new Goldnet.Ext.Web.ListItem(l_leveldt.Rows[i]["civID"].ToString(), l_leveldt.Rows[i]["civID"].ToString()));

            }

            this.cbx_PLevel.Value = "全部";



            DataTable l_Degeedt = dal.getDegee().Tables[0];

            this.cbx_PCollage.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_Degeedt.Rows.Count; i++)
            {
                this.cbx_PCollage.Items.Add(new Goldnet.Ext.Web.ListItem(l_Degeedt.Rows[i]["eduID"].ToString(), l_Degeedt.Rows[i]["eduID"].ToString()));

            }

            this.cbx_PCollage.Value = "全部";



            DataTable l_Titlelistdt = dal.getTitlelist().Tables[0];

            this.cbx_PTech.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_Titlelistdt.Rows.Count; i++)
            {
                this.cbx_PTech.Items.Add(new Goldnet.Ext.Web.ListItem(l_Titlelistdt.Rows[i]["ID"].ToString(), l_Titlelistdt.Rows[i]["ID"].ToString()));

            }

            this.cbx_PTech.Value = "全部";

            this.cbx_TimeOrgan.Value = "全部";


            DataTable l_Stationdt = dal.getStationStore("").Tables[0];
            DataRow l_dr = l_Stationdt.NewRow();
            l_dr[0] = "全部";
            l_dr[1] = "全部";
            l_Stationdt.Rows.InsertAt(l_dr,0);
            this.StoreStation.DataSource = l_Stationdt;
            this.StoreStation.DataBind();
            this.cboStation.Value = "全部";
        }

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
                string rptid = Request.QueryString["rptid"].ToString();

                m_rptid = rptid;

                InitComboxValue();

                string terms = "";

                string stationCode = "";

                if (Session["ReportStaffCode"] != null)
                {

                    terms = Session["ReportStaffCode"].ToString();
                    stationCode = Session["ReportStation"].ToString();
                    Session.Remove("ReportStaffCode");
                    Session.Remove("ReportStation");

                }
                else
                {
                    ReportDalDict dal = new ReportDalDict();
                    DataTable l_dt = dal.getReportTerms(m_rptid).Tables[0];
                    if (l_dt.Rows.Count > 0)
                    {
                        terms = l_dt.Rows[0]["RPT_TERMS"].ToString();
                        stationCode = l_dt.Rows[0]["PRT_SECOND_TERMS"].ToString();
                    }
                }

                if (terms != "")
                {
                    //ToRightSelector
                    this.Store2.DataSource = AnalyzeTermsToRightSelector(terms);
                    this.Store2.DataBind();

                    //反向的情况下显示全部的条件
                    if (isSortMethod(terms))
                    {
                        DataTable l_dt =  AnalyzeTermsToLeft(terms, stationCode);
                        this.Store1.DataSource = l_dt;
                        this.Store1.DataBind();

                    }
                }
            }
        }

        protected void QueryStation(object sender, AjaxEventArgs e)
        {
            StaffDalDict dal = new StaffDalDict();
            DataTable l_Stationdt = dal.getStationStore(this.DeptCodeCombo.SelectedItem.Value).Tables[0];
            DataRow l_dr = l_Stationdt.NewRow();
            l_dr[0] = "全部";
            l_dr[1] = "全部";
            l_Stationdt.Rows.InsertAt(l_dr, 0);
            this.StoreStation.DataSource = l_Stationdt;
            this.StoreStation.DataBind();
            this.cboStation.SelectedItem.Value = "全部";
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Next_Click(object sender, AjaxEventArgs e)
        {
            string multi1 = e.ExtraParams["multi1"];

            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi1);

            string multi2 = e.ExtraParams["multi2"];

            Goldnet.Ext.Web.ListItem[] items2 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi2);

            //判断选没选中项目
            if (this.ckx_Sort.Checked)
            {
                if (items2.Length == 0)
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "没有符合条件的人员,请重新选择条件",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }
            else
            {
                if (items1.Length == 0)
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "请选择人员",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }


            string StaffReport = TermsStringCreate(items1);

            string TermsStation = this.cboStation.SelectedItem.Value.Replace("全部","*");

            ReportDalDict dal = new ReportDalDict();

            dal.UpdateStaffReportTerms(m_rptid, StaffReport,TermsStation);

            if (Session["SelectRightGuideItem"] != null)
            {
                DataTable l_dt = ((DataTable)Session["SelectRightGuideItem"]);

                GuideDalDict GuideDal = new GuideDalDict();

                GuideDal.UpdateReportGuideInfo(m_rptid, l_dt);

                Session.Remove("SelectRightGuideItem");
            }


            //关闭父窗体WINDOW
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.Ext.Msg.show({ title: '系统提示', msg:  '报表保存成功!',icon: 'ext-mb-info',  buttons: { ok: true }  });");
            scManager.AddScript("parent.window.arcEditWindow.hide()");
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Prev_Click(object sender, AjaxEventArgs e)
        {
            string multi1 = e.ExtraParams["multi1"];

            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi1);

            string StaffReport = TermsStringCreate(items1);

            Session["ReportStation"] = this.cboStation.SelectedItem.Value;

            Session["ReportStaffCode"] = StaffReport;

            Response.Redirect("ReportDetail.aspx?rptid=" + m_rptid + "");
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QueryStaff(object sender, AjaxEventArgs e)
        {
            #region 查询条件

            StringBuilder str = new StringBuilder();

            if (!this.cbx_Ptype.SelectedItem.Value.Equals("全部"))
            {
                str.Append(" and staffsort" + Stringfilter(this.cbx_Ptype.SelectedItem.Text.Trim()) + " ");
            }

            if (this.cbx_PTechType.SelectedItem.Value != "全部")
            {
                str.Append(" and techincclass" + Stringfilter(this.cbx_PTechType.SelectedItem.Value) + " ");
            }
            if (this.cbx_PLevel.SelectedItem.Value != "全部")
            {
                str.Append(" and CivilServiceClass" + Stringfilter(this.cbx_PLevel.SelectedItem.Value) + " ");
            }
            if (this.cbx_PTech.SelectedItem.Value != "全部")
            {
                str.Append(" and title_list" + Stringfilter(this.cbx_PTech.SelectedItem.Text.Trim()) + " ");
            }
            if (this.DeptCodeCombo.SelectedItem.Value != "")
            {
                str.Append(" and dept_code='" + this.DeptCodeCombo.SelectedItem.Value + "' ");
            }
            if (this.cbx_PCollage.SelectedItem.Value != "全部")
            {
                str.Append(" and edu1" + Stringfilter(this.cbx_PCollage.SelectedItem.Text.Trim()) + " ");
            }

            if (this.cbx_TimeOrgan.SelectedItem.Value == ">=")
            {
                str.Append(" and TechnicClassDate is not null and ");

                str.Append(" substr(nvl(TechnicClassDate,'') ,1,4)|| lpad(replace(replace(replace(substr(nvl(TechnicClassDate,'') ,6,2),'.',''),'-',''),'/',''),2,'0')||'01'  >= '");

                str.Append(this.timer.SelectedDate.ToString("yyyyMM") + "01" + "' ");
            }
            else if (this.cbx_TimeOrgan.SelectedItem.Value == "<=")
            {
                str.Append(" and TechnicClassDate is not null and ");

                str.Append(" substr(nvl(TechnicClassDate,'') ,1,4)|| lpad(replace(replace(replace(substr(nvl(TechnicClassDate,'') ,6,2),'.',''),'-',''),'/',''),2,'0')||'01'  <= '");

                str.Append(this.timer.SelectedDate.ToString("yyyyMM") + "01" + "' ");
            }
            else if (this.cbx_TimeOrgan.SelectedItem.Value == "=")
            {
                str.Append(" and TechnicClassDate is not null and ");

                str.Append(" substr(nvl(TechnicClassDate,'') ,1,4)|| lpad(replace(replace(replace(substr(nvl(TechnicClassDate,'') ,6,2),'.',''),'-',''),'/',''),2,'0')||'01'  = '");

                str.Append(this.timer.SelectedDate.ToString("yyyyMM") + "01" + "' ");
            }
            else if (this.cboStation.SelectedItem.Value != "全部") 
            {
                str.Append(" and station_code = '" + this.cboStation.SelectedItem.Value.Trim() + "' ");
            }

            #endregion

            this.Store1.RemoveAll();

            Goldnet.Dal.StaffDalDict dal = new Goldnet.Dal.StaffDalDict();

            DataTable l_dt = dal.GetStaff(str.ToString()).Tables[0];

            l_dt = LeftSelectorFilter(l_dt, e);

            if (l_dt.Rows.Count > 0)
            {
                this.Store1.DataSource = l_dt;
                this.Store1.DataBind();
            }
        }

        #endregion

        #region --页面逻辑--

        /// <summary>
        /// 判断选择方法
        /// </summary>
        /// <param name="terms">信息字符</param>
        /// <returns>true:反向,false:正向</returns>
        private bool isSortMethod(string terms)
        {
            string[] StaffCodes = terms.Split(';');

            string SortMethod = StaffCodes[StaffCodes.Length - 1].ToString();

            if (SortMethod == "0")
            {
                this.ckx_Sort.Checked = false;
            }
            else
            {
                this.ckx_Sort.Checked = true;
            }

            return this.ckx_Sort.Checked;
        }

        /// <summary>
        /// 解析字符提取左边SELECTOR数据
        /// </summary>
        /// <param name="terms">条件字符</param>
        /// <returns>数据集合</returns>
        private DataTable AnalyzeTermsToLeft(string terms,string station)
        {
            string[] StaffCodes = terms.Split(';');

            string[] ComboxInfos = StaffCodes[0].Split(',');

            string where = "";

            //还原人员属性条件
            this.cbx_Ptype.Value = AnalyzeTermsToComboxValue(ComboxInfos[0].ToString());
            this.cbx_PTechType.Value = AnalyzeTermsToComboxValue(ComboxInfos[1].ToString());
            this.cbx_PTech.Value = AnalyzeTermsToComboxValue(ComboxInfos[2].ToString());

            string timeValue = AnalyzeTermsToComboxValue(ComboxInfos[3].ToString());

            //还原字符条件SQL
            if (timeValue != "全部")
            {
                this.cbx_TimeOrgan.Value = timeValue.Split('$')[0].ToString();

                this.timer.Value = timeValue.Split('$')[1];
            }

            this.cbx_PCollage.Value = AnalyzeTermsToComboxValue(ComboxInfos[4].ToString());

            this.cbx_PLevel.Value = AnalyzeTermsToComboxValue(ComboxInfos[5].ToString());
            if (ComboxInfos[6].ToString() != "*")
            {
                this.DeptCodeCombo.Value = ComboxInfos[6].ToString();
            }
           

            StaffDalDict dalStaff = new StaffDalDict();
            DataTable l_Stationdt = dalStaff.getStationStore(ComboxInfos[6].ToString().Replace("*", "")).Tables[0];
            DataRow l_dr = l_Stationdt.NewRow();
            l_dr[0] = "全部";
            l_dr[1] = "全部";
            l_Stationdt.Rows.InsertAt(l_dr, 0);
            this.StoreStation.DataSource = l_Stationdt;
            this.StoreStation.DataBind();
            this.cboStation.Value = station.Replace("*", "全部");


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
                if (ComboxInfos[5].ToString() != "")
                {
                    where = where + "  and CivilServiceClass " + this.Stringfilter(ComboxInfos[5].ToString());
                }
                else
                {
                    where = where + "  and CivilServiceClass is null";
                }
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

            StaffDalDict dal = new StaffDalDict();

            DataTable l_dt = dal.GetStaff(where).Tables[0];

            return l_dt;
        }


        /// <summary>
        /// 过滤字符
        /// </summary>
        /// <param name="item">条件字符</param>
        /// <returns></returns>
        private string AnalyzeTermsToComboxValue(string item)
        {
            if (item == "*")
            {
                item = "全部";
            }
            if (item == "")
            {
                item = "为空";
            }
            return item;
        }


        /// <summary>
        /// 提取右侧项目
        /// </summary>
        /// <param name="terms">条件字符</param>
        /// <returns></returns>
        private DataTable AnalyzeTermsToRightSelector(string terms)
        {
            string[] StaffCodes = terms.Split(';');
            string StaffCode = StaffCodes[StaffCodes.Length - 2].ToString();
            string where = "";
            if (StaffCode != "*")
            {
                where = " and staff_id in (" + StaffCode + ")";
            }
            else
            {
                where = " and 1=2 ";
            }
            StaffDalDict dal = new StaffDalDict();
            DataTable l_dt = dal.GetStaff(where).Tables[0];
            return l_dt;
        }

        /// <summary>
        /// 创建条件字符
        /// </summary>
        /// <param name="items">选择项</param>
        /// <returns>条件字符</returns>
        private string TermsStringCreate(Goldnet.Ext.Web.ListItem[] items)
        {
            string TermsString = "";
            //条件字符
            if (!this.ckx_Sort.Checked)
            {
                TermsString = "*;";
            }
            else
            {
                TermsString = TermsStaffInfoStringCreate();
            }

            //选择项CODE
            for (int i = 0; i < items.Length; i++)
            {
                Goldnet.Ext.Web.ListItem item = items[i];
                if (i == items.Length - 1)
                {
                    TermsString = TermsString + item.Value + ";";
                }
                else
                {
                    TermsString = TermsString + item.Value + ",";
                }
            }
            if (items.Length == 0)
            {
                TermsString = TermsString + "*;";
            }
            if (this.ckx_Sort.Checked)
            {
                //反向
                TermsString = TermsString + "1";
            }
            else
            {
                //正向
                TermsString = TermsString + "0";
            }
            return TermsString;
        }

        /// <summary>
        /// 人员属性条件字符
        /// </summary>
        /// <returns>条件字符</returns>
        private string TermsStaffInfoStringCreate()
        {
            string PtypeString = this.cbx_Ptype.SelectedItem.Value;

            string PTechTypeString = this.cbx_PTechType.SelectedItem.Value;

            string PTechString = this.cbx_PTech.SelectedItem.Value;

            string TimeOrganString = this.cbx_TimeOrgan.SelectedItem.Value;

            string TimerString = this.timer.Value.ToString();

            string EduString = this.cbx_PCollage.SelectedItem.Value;

            string PLevelString = this.cbx_PLevel.SelectedItem.Value;

            string DeptCodeString = this.DeptCodeCombo.SelectedItem.Value;

            string terms = AnalyzeStrCreate(PtypeString);

            terms = terms + "," + AnalyzeStrCreate(PTechTypeString);

            terms = terms + "," + AnalyzeStrCreate(PTechString);

            terms = terms + "," + AnalyzeStrCreate(TimeOrganString);

            if (TimeOrganString != "全部")
            {
                terms = terms + "$" + this.timer.SelectedDate.ToString("yyyyMM") + "01";
            }

            terms = terms + "," + AnalyzeStrCreate(EduString);

            terms = terms + "," + AnalyzeStrCreate(PLevelString);

            terms = terms + "," + AnalyzeStrCreate(DeptCodeString) + ";";

            return terms;

        }

        /// <summary>
        /// 过滤字符
        /// </summary>
        /// <param name="item">条件字符</param>
        /// <returns>组成字符</returns>
        private string AnalyzeStrCreate(string item)
        {
            if (item == "全部" || item == "")
            {
                item = "*";
            }
            if (item == "为空")
            {
                item = "";
            }
            return item;
        }

        /// <summary>
        /// 组建条件字符
        /// </summary>
        /// <param name="item">元素</param>
        /// <returns></returns>
        private string Stringfilter(string item)
        {

            if (item == "为空")
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
        /// 过滤选择项
        /// </summary>
        /// <param name="table"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private DataTable LeftSelectorFilter(DataTable table, AjaxEventArgs e)
        {
            string multi1 = e.ExtraParams["multi1"];
            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi1);


            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }

            try
            {

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (array.Contains(table.Rows[i]["VALUE"].ToString()))
                    {
                        table.Rows.RemoveAt(i);
                        i--;
                    }

                }

            }
            catch
            {
                Ext.Msg.Alert("系统提示", "输入有误！").Show();
            }


            return table;

        }


        #endregion
    }
}
