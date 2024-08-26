using System;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class Assess_KH : PageBase
    {
        Goldnet.Dal.Assess dal = new Goldnet.Dal.Assess();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                SetInitState();
                for (int i = 0; i < this.GridPanel_List.ColumnModel.Columns.Count; i++)
                {
                    if (this.GridPanel_List.ColumnModel.Columns[i].ColumnID == "GUIDE_F_VALUE_01")
                        this.GridPanel_List.ColumnModel.Columns[i].Header = GetConfig.GetConfigString("BSC01");
                    if (this.GridPanel_List.ColumnModel.Columns[i].ColumnID == "GUIDE_F_VALUE_02")
                        this.GridPanel_List.ColumnModel.Columns[i].Header = GetConfig.GetConfigString("BSC02");
                    if (this.GridPanel_List.ColumnModel.Columns[i].ColumnID == "GUIDE_F_VALUE_03")
                        this.GridPanel_List.ColumnModel.Columns[i].Header = GetConfig.GetConfigString("BSC03");
                    if (this.GridPanel_List.ColumnModel.Columns[i].ColumnID == "GUIDE_F_VALUE_04")
                        this.GridPanel_List.ColumnModel.Columns[i].Header = GetConfig.GetConfigString("BSC04");
                }
            }
        }

        //设置页面控件初始化状态
        protected void SetInitState()
        {
            //年份、月份下拉框
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                this.Comb_EndYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                this.Comb_EndMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_EndMonth.SelectedIndex = DateTime.Now.Month - 1;

            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            string flg = dal.GetSavedAssessCnt(incount);
            this.Btn_View.Disabled = flg.Equals("0") ? true : false;

        }

        //查看按钮单击事件
        protected void Btn_View_Click(object sender, AjaxEventArgs e)
        {
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;
            string endyear = this.Comb_EndYear.SelectedItem.Value;
            string endmonth = this.Comb_EndMonth.SelectedItem.Value;

            string rtn = "";
            rtn = CheckMonthDate(startyear, startmonth, endyear, endmonth);
            if (!rtn.Equals(""))
            {
                showMsg(rtn);
                return;
            }
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            startyear = startyear.PadLeft(4, '0');
            DataTable dt = dal.GetAssessSavedTemp(incount, startyear).Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.Btn_Excel.Disabled = false;
                string flg = dal.GetAssessArchFlg(incount);
                this.Btn_Arch.Disabled = flg.Equals("0") ? false : true;
            }
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            string filename = "绩效考核结果临时归档文件";
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            ExportData ex = new ExportData();
            string stationyear = this.Comb_StartYear.SelectedItem.Value;
            stationyear = stationyear.PadLeft(4, '0');
            DataTable dt = dal.GetAssessSavedTemp(incount, stationyear).Tables[0];
            ex.ExportToLocal(dt, this.Page, "xls", filename);
        }

        /// <summary>
        /// 临时保存处理
        /// </summary>
        /// <param name="ASSESS_NAME"></param>
        /// <returns></returns>
        [AjaxMethod]
        public string Btn_Save_Click(string ASSESS_NAME)
        {
            string save_id = ((User)(Session["CURRENTSTAFF"])).UserId;
            string counting = ((User)(Session["CURRENTSTAFF"])).UserId;
            string ASSESS_APPRAISER = ((User)(Session["CURRENTSTAFF"])).UserId;
            string START_DATE = this.Comb_StartYear.SelectedItem.Value.PadLeft(4, '0') + this.Comb_StartMonth.SelectedItem.Value.PadLeft(2, '0');
            string END_DATE = this.Comb_EndYear.SelectedItem.Value.PadLeft(4, '0') + this.Comb_EndMonth.SelectedItem.Value.PadLeft(2, '0');
            string station_year = this.Comb_StartYear.SelectedItem.Value.PadLeft(4, '0');
            string rtn = "";
            if (ASSESS_NAME.Length > 50)
            {
                ASSESS_NAME = ASSESS_NAME.Substring(0, 50);
            }
            rtn = dal.Assess_informationAndresultSave(ASSESS_NAME, START_DATE, END_DATE, ASSESS_APPRAISER, station_year, counting, save_id);
            return rtn;
        }

        /// <summary>
        /// 归档保存
        /// </summary>
        /// <param name="ASSESS_NAME"></param>
        /// <returns></returns>
        [AjaxMethod]
        public string Btn_Arch_Click(string ASSESS_NAME)
        {
            string save_id = ((User)(Session["CURRENTSTAFF"])).UserId;

            string rtn = "";
            if (ASSESS_NAME.Length > 50)
            {
                ASSESS_NAME = ASSESS_NAME.Substring(0, 50);
            }
            rtn = dal.AddAssess_informationAndresult(ASSESS_NAME, save_id);
            return rtn;
        }

        //评价按钮，取得并检查评价的各项参数
        protected void Btn_Create_Click(object sender, AjaxEventArgs e)
        {
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;
            string endyear = this.Comb_EndYear.SelectedItem.Value;
            string endmonth = this.Comb_EndMonth.SelectedItem.Value;
            string rtn = "";
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;

            rtn = CheckMonthDate(startyear, startmonth, endyear, endmonth);
            if (!rtn.Equals(""))
            {
                showMsg(rtn);
                return;
            }
            startyear = startyear.PadLeft(4, '0');
            endyear = endyear.PadLeft(4, '0');
            startmonth = startmonth.PadLeft(2, '0');
            endmonth = endmonth.PadLeft(2, '0');
            string startdate = startyear + startmonth + "01";
            string enddate = endyear + endmonth + DateTime.DaysInMonth(Convert.ToInt32(endyear), Convert.ToInt32(endmonth)).ToString();

            rtn = dal.GetAssess_COMPUTE(startdate, enddate, startyear, incount);
            if (rtn.Equals(""))
            {
                DataTable dt = dal.GetAssessResultTemp(incount, startyear).Tables[0];
                this.Store1.DataSource = dt;
                this.Store1.DataBind();
                this.Btn_View.Disabled = true;
                if (dt.Rows.Count > 0)
                {
                    this.Btn_Excel.Disabled = false;
                    this.Btn_Save.Disabled = false;
                }
                //showDetailWin(getLoadConfig("Eval_Result_Detail.aspx?organ=K"), "科室评价结果一览", "900");
            }
            else
            {
                showMsg(rtn);
            }
        }

        //检查开始结束月份
        public string CheckMonthDate(string year1, string month1, string year2, string month2)
        {
            month1 = month1.PadLeft(2, '0');
            month2 = month2.PadLeft(2, '0');
            string rtn = "";
            if (year1.Equals("") || month1.Equals("") || year2.Equals("") || month2.Equals(""))
            {
                rtn = "请输入开始月份和结束月份!";
                return rtn;
            }
            int dateoffset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["dateoffset"].ToString());
            DateTime year1_set = Convert.ToDateTime(year1 + "-" + month1 + "-01").AddMonths(-dateoffset);
            DateTime year2_set = Convert.ToDateTime(year2 + "-" + month2 + "-01").AddMonths(-dateoffset);
            if (!year1_set.ToString("yyyy").Equals(year2_set.ToString("yyyy")))
            {
                rtn = "绩效考核不能跨年度!";
                return rtn;
            }
            year1 = year1.PadLeft(4, '0');
            year2 = year2.PadLeft(4, '0');
            month1 = month1.PadLeft(2, '0');
            month2 = month2.PadLeft(2, '0');

            string startime = year1 + "-" + month1 + "-01";
            string endtime = year2 + "-" + month2 + "-01";
            DateTime dd1 = default(DateTime);
            DateTime dd2 = default(DateTime);
            if (!DateTime.TryParse(startime, out dd1))
            {
                rtn = "请选择有效的考核开始时间!";
                return rtn;
            }
            if (!DateTime.TryParse(endtime, out dd2))
            {
                rtn = "请选择有效的考核结束时间!";
                return rtn;
            }
            if (dd1 > dd2)
            {
                rtn = "考核开始时间不能晚于结束结束，请检查!";
                return rtn;
            }
            if ((dd2.Year - dd1.Year) * 12 + (dd2.Month - dd1.Month) > 12)
            {
                rtn = "考核区间建议不超过1年，请重新设置!";
                return rtn;
            }
            return rtn;
        }


        ////显示详细窗口
        //private void showDetailWin(LoadConfig loadcfg, string title, string width)
        //{
        //    DetailWin.ClearContent();
        //    if (!title.Trim().Equals(""))
        //    {
        //        DetailWin.SetTitle(title);
        //    }
        //    if (!width.Trim().Equals(""))
        //    {
        //        DetailWin.Width = Unit.Pixel(Convert.ToInt16(width));
        //    }
        //    DetailWin.Center();
        //    DetailWin.Show();
        //    DetailWin.LoadContent(loadcfg);
        //}

        ////载入参数设置
        //private LoadConfig getLoadConfig(string url)
        //{
        //    LoadConfig loadcfg = new LoadConfig();
        //    loadcfg.Url = url;
        //    loadcfg.Mode = LoadMode.IFrame;
        //    loadcfg.MaskMsg = "载入中...";
        //    loadcfg.ShowMask = true;
        //    loadcfg.NoCache = true;
        //    return loadcfg;
        //}

        //显示提示信息
        public void showMsg(string msg)
        {
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }

        //Coll事件
        [AjaxMethod]
        public void GetInfo(string command, string colIndex, string staff_id)
        {
            LoadConfig loadcfg = new LoadConfig();
            switch (command)
            {
                case "NameInfo":
                    //显示人员详细信息界面
                    loadcfg = getLoadConfig("StaffInfo.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("staff_id", staff_id));
                    StaffInfoWin.ClearContent();
                    StaffInfoWin.Show();
                    StaffInfoWin.LoadContent(loadcfg);
                    break;
                case "ResultInfo":
                    //指标数值详细信息
                    loadcfg = getLoadConfig("Assess_ResultInfo.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("counting", ((User)(Session["CURRENTSTAFF"])).UserId));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("person_id", staff_id));
                    switch (colIndex)
                    {
                        case "GUIDE_F_VALUE_01":
                            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("bsc_class", "01"));
                            break;
                        case "GUIDE_F_VALUE_02":
                            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("bsc_class", "02"));
                            break;
                        case "GUIDE_F_VALUE_03":
                            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("bsc_class", "03"));
                            break;
                        case "GUIDE_F_VALUE_04":
                            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("bsc_class", "04"));
                            break;
                        case "ALL_VALUE":
                            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("bsc_class", ""));
                            break;
                    }
                    ResultInfoWin.ClearContent();
                    ResultInfoWin.Show();
                    ResultInfoWin.LoadContent(loadcfg);
                    break;
            }

        }

    }
}
