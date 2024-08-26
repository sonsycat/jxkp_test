using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;
using GoldNet.Model;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP.jxkh
{
    public partial class Dept_Assess_KH : PageBase
    {
        Goldnet.Dal.Assess dal = new Goldnet.Dal.Assess();
        private static List<WorkItem> TaskThread;
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
                //设置页面控件初始化状态
                SetInitState();

                //根据web配置更换列表表头名称
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

                //清空已归档标志
                this.Session.Remove("saveflag");
            }
        }

        /// <summary>
        /// 设置页面控件初始化状态
        /// </summary>
        protected void SetInitState()
        {
            //年份、月份下拉框
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }

            //设置默认年月
            this.Comb_StartYear.Value = Convert.ToString(DateTime.Now.Year);
            this.Comb_StartMonth.SelectedIndex = DateTime.Now.Month - 1;
            string year = Convert.ToString(DateTime.Now.Year);
            string month = Convert.ToString(DateTime.Now.Month - 1);
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + "" + month;

            //string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            //string flg = dal.GetDeptSavedAssessCnt(incount,benginDate);

            //设置查询按钮
            this.Btn_View.Disabled = false;
            //设置临时保存按钮
            this.Btn_Save.Disabled = true;
            //设置归档保存按钮
            this.Btn_Arch.Disabled = true;
            //设置导出按钮
            this.Btn_Excel.Disabled = true;
        }

        /// <summary>
        /// 数据刷新处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //选择日期
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            string benginDate = GetBeginDate();
            DataTable dt;
            this.Session["benginDate"] = benginDate;

            if (this.Session["saveflag"] == null)
            {
                //未做临时保存处理
                dt = dal.GetDeptAssessResultTemp(incount, startyear, benginDate).Tables[0];
                this.Store1.DataSource = dt;
                this.Store1.DataBind();
            }
            else
            {
                //已做临时保存处理
                dt = dal.GetDeptAssessResultSave(incount, startyear, benginDate).Tables[0];
                this.Store1.DataSource = dt;
                this.Store1.DataBind();
            }

            //string flg = dal.GetDeptSavedAssessCnt(incount, benginDate);
            //设置查询按钮
            //this.Btn_View.Disabled = flg.Equals("0") ? true : false;

            //已经临时保存数据，屏蔽保存按钮
            if (dt.Rows.Count > 0)
            {
                //EXCEL导出按钮设置
                this.Btn_Excel.Disabled = false;
                //临时保存按钮设置
                this.Btn_Save.Disabled = false;

                //做过临时保存
                if (this.Session["saveflag"] == "0")
                {
                    //string flg = dal.GetDeptAssessArchFlg(incount, benginDate);
                    //归档按钮设置
                    this.Btn_Arch.Disabled = false;// flg.Equals("0") ? false : true;
                }
            }
        }

        /// <summary>
        /// 查询事件处理，查询临时保存的考核结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_View_Click(object sender, AjaxEventArgs e)
        {
            //选择年月
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;

            //当前登录者
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            string rtn = "";

            startyear = startyear.PadLeft(4, '0');
            //选择年月
            string benginDate = GetBeginDate();

            //获取临时保存的绩效考核数据
            DataTable dt = dal.GetDeptAssessSavedTemp(incount, startyear, benginDate).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //设置导出按钮
                this.Btn_Excel.Disabled = false;
                //设置临时保存按钮
                this.Btn_Save.Disabled = false;
                //检查是否已经临时保存
                //string flg = dal.GetDeptAssessArchFlg(incount, benginDate);
                //设置归档按钮
                this.Btn_Arch.Disabled = false;// flg.Equals("0") ? false : true;

                this.Session["saveflag"] = "0";
                this.Session["benginDate"] = benginDate;
            }
            else
            {
                //设置导出按钮
                this.Btn_Excel.Disabled = true;
                //设置临时保存
                this.Btn_Save.Disabled = true;
                //设置归档按钮
                this.Btn_Arch.Disabled = true;

                this.Session.Remove("saveflag");
                this.Session.Remove("benginDate");
            }
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 科室绩效考核生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_BatInit_Click(object sender, AjaxEventArgs e)
        {
            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "";
            this.Btn_BatCancel.Text = "关闭";
            this.Win_BatchInit.SetTitle("科室考评");
            //弹出生成窗体
            this.Win_BatchInit.Show("Btn_BatInit");
        }

        /// <summary>
        /// 开始生成事件处理，生成科室绩效考核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_BatStart_Click(object sender, AjaxEventArgs e)
        {
            TaskThread = new List<WorkItem>();
            this.Btn_BatCancel.Text = "取消";
            this.Session["LongActionProgress"] = "0:";
            TaskThread.Add(AbortableThreadPool.QueueUserWorkItem(new WaitCallback(BatchInitTargetGuide)));
            ScriptManager1.AddScript("{0}.startTask('longactionprogress');", TaskManager1.ClientID);
            this.Win_BatchInit.StyleSpec = "cursor: wait;";
        }

        /// <summary>
        /// 生成科室绩效考核方法
        /// </summary>
        /// <param name="state"></param>
        private void BatchInitTargetGuide(object state)
        {
            //当前年月
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;
            string endyear = this.Comb_StartYear.SelectedItem.Value;
            string endmonth = this.Comb_StartMonth.SelectedItem.Value;
            string rtn = "";
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            //检查日期合法性
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

            try
            {
                //调用考核计算过程（DEPT_ASSESS_COMPUTE）
                this.Session["LongActionProgress"] = "1:正在生成科室考评";
                rtn = dal.GetdeptAssess_COMPUTE(startdate, enddate, startyear, incount);
                if (rtn.Equals(""))
                {
                    //获取新计算，未做临时保存的结果数据
                    DataTable dt = dal.GetDeptAssessResultTemp(incount, startyear, startyear + startmonth).Tables[0];
                    this.Store1.DataSource = dt;
                    this.Store1.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        //设置导出按钮
                        this.Btn_Excel.Disabled = false;
                        //设置临时按钮
                        this.Btn_Save.Disabled = false;
                        //设置归档按钮
                        this.Btn_Arch.Disabled = false;
                    }
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    //刷新数据
                    scManager.AddScript("Store1.reload();");
                }
                this.Session.Remove("LongActionProgress");
                this.Session.Remove("LastStep");
                this.Session.Remove("NowCounter");
                this.Session.Remove("saveflag");
            }
            catch (Exception e)
            {
                this.Session["LongActionProgress"] = "-1:数据生成出错，请联系系统管理员!" + rtn;
            }
        }

        /// <summary>
        /// 绩效考核生成过程中取消处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CloseBatInit(object sender, AjaxEventArgs e)
        {
            if (TaskThread != null)
            {
                Random rand = new Random();
                while (TaskThread.Count > 0)
                {
                    int i = rand.Next(TaskThread.Count);
                    WorkItem item = TaskThread[i];
                    TaskThread.RemoveAt(i);
                    AbortableThreadPool.Cancel(item, true).ToString();
                }
            }
            this.Session.Remove("LongActionProgress");
            this.Session.Remove("LastStep");
            this.Session.Remove("NowCounter");
        }

        /// <summary>
        /// 进度条刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RefreshProgress(object sender, AjaxEventArgs e)
        {
            object progress = this.Session["LongActionProgress"];
            if (progress != null)
            {
                int i = Convert.ToInt16(progress.ToString().Split(':')[0]);
                string msg = "";
                if (progress.ToString().Split(':').Length > 1)
                {
                    msg = progress.ToString().Split(':')[1].ToString();
                }
                if (i >= 0)
                {
                    if (this.Session["LastStep"] == null)
                    {
                        this.Session["LastStep"] = "0";
                    }
                    if (this.Session["NowCounter"] == null)
                    {
                        this.Session["NowCounter"] = "0";
                    }
                    if (!(i.ToString().Equals(this.Session["LastStep"].ToString())))
                    {
                        this.Session["LastStep"] = i.ToString();
                        this.Session["NowCounter"] = "0";
                        float p = ((int)(i - 1)) / 3f;
                        Progress1.UpdateProgress(p, (p).ToString("p0"));
                    }
                    else
                    {
                        this.Session["NowCounter"] = (Convert.ToInt16(this.Session["NowCounter"].ToString()) + 1).ToString();
                        float p = (((int)(i - 1)) / 3f + (Convert.ToInt16(this.Session["NowCounter"].ToString()) > 30 ? 0.3f : Convert.ToInt16(this.Session["NowCounter"].ToString()) / 100f));
                        Progress1.UpdateProgress(p, (p).ToString("p0"));
                    }
                    if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 4 == 0)
                    {
                        progressTip.Text = msg + "，请稍候";
                    }
                    else if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 4 == 1)
                    {
                        progressTip.Text = msg + "，请稍候.";
                    }
                    else if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 4 == 2)
                    {
                        progressTip.Text = msg + "，请稍候..";
                    }
                    else if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 4 == 3)
                    {
                        progressTip.Text = msg + "，请稍候...";
                    }
                }
                else
                {
                    //出错时处理
                    this.Session.Remove("LongActionProgress");
                    this.Session.Remove("LastStep");
                    this.Session.Remove("NowCounter");
                    ScriptManager1.AddScript("{0}.stopTask('longactionprogress');", TaskManager1.ClientID);
                    this.Btn_BatCancel.Text = "关闭";
                    Ext.Msg.Alert("系统提示", msg).Show();
                    this.Win_BatchInit.StyleSpec = "cursor: auto;";
                }
            }
            else
            {
                this.Session.Remove("LongActionProgress");
                this.Session.Remove("LastStep");
                this.Session.Remove("NowCounter");
                ScriptManager1.AddScript("{0}.stopTask('longactionprogress');", TaskManager1.ClientID);
                Progress1.UpdateProgress(1, "100%");
                progressTip.Text = "科室考评已完成!";
                this.Btn_BatCancel.Text = "关闭";
                this.Win_BatchInit.StyleSpec = "cursor: auto;";
            }
        }

        /// <summary>
        /// EXCEL导出处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            //string filename = "绩效考核结果临时归档文件";
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            string benginDate = GetBeginDate();
            //ExportData ex = new ExportData();
            string stationyear = this.Comb_StartYear.SelectedItem.Value;
            stationyear = stationyear.PadLeft(4, '0');
            DataTable dt = dal.GetDeptAssessSavedTemp(incount, stationyear, benginDate).Tables[0];
            //ex.ExportToLocal(dt, this.Page, "xls", filename);

            string year = Comb_StartYear.SelectedItem.Value.ToString();
            string month = Comb_StartMonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string s_date = year + "-" + month;

            // 创建工作簿
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("绩效考核结果临时归档文件");

            // 创建表头
            IRow headerRow = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            // 创建样式
            ICellStyle numberStyle = workbook.CreateCellStyle();
            numberStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");

            ICellStyle textStyle = workbook.CreateCellStyle();
            textStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");

            ICellStyle currencyStyle = workbook.CreateCellStyle();
            currencyStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00"); // 金钱格式

            // 填充数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    string cellValue = dt.Rows[i][j].ToString();
                    double number;

                    if (double.TryParse(cellValue, out number))
                    {
                        cell.SetCellValue(number);

                        // 如果是金钱列，设置为金钱格式；否则设置为数值格式
                        //if (j == /* 你的金钱列索引，例如第5列 */)
                        //{
                        //    cell.CellStyle = numberStyle;
                        //}
                        //else
                        //{
                        //    cell.CellStyle = currencyStyle;
                        //}

                        cell.CellStyle = numberStyle; // 数值格式
                    }
                    else
                    {
                        cell.SetCellValue(cellValue);
                        cell.CellStyle = textStyle; // 文本格式
                    }
                }
            }

            // 自动调整列宽
            int maxColumnWidth = 30 * 256; // 最大宽度设置为30字符宽
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
                int currentWidth = sheet.GetColumnWidth(i);
                if (currentWidth > maxColumnWidth)
                {
                    sheet.SetColumnWidth(i, maxColumnWidth);
                }
            }

            // 导出为Excel文件
            using (MemoryStream exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "绩效考核结果临时归档文件.xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(exportData.ToArray());
                Response.End();
            }
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
            string END_DATE = this.Comb_StartYear.SelectedItem.Value.PadLeft(4, '0') + this.Comb_StartMonth.SelectedItem.Value.PadLeft(2, '0');
            string station_year = this.Comb_StartYear.SelectedItem.Value.PadLeft(4, '0');
            string rtn = "";
            if (ASSESS_NAME.Length > 50)
            {
                ASSESS_NAME = ASSESS_NAME.Substring(0, 50);
            }
            rtn = dal.deptAssess_informationAndresultSave(ASSESS_NAME, START_DATE, END_DATE, ASSESS_APPRAISER, station_year, counting, save_id);

            //更新临时保存标识
            this.Session["saveflag"] = "0";

            return rtn;
        }

        /// <summary>
        /// 归档保存处理
        /// </summary>
        /// <param name="ASSESS_NAME"></param>
        /// <returns></returns>
        [AjaxMethod]
        public string Btn_Arch_Click(string ASSESS_NAME)
        {
            string save_id = ((User)(Session["CURRENTSTAFF"])).UserId;
            string START_DATE = this.Comb_StartYear.SelectedItem.Value.PadLeft(4, '0') + this.Comb_StartMonth.SelectedItem.Value.PadLeft(2, '0');
            string rtn = "";
            if (ASSESS_NAME.Length > 50)
            {
                ASSESS_NAME = ASSESS_NAME.Substring(0, 50);
            }
            rtn = dal.deptAddAssess_informationAndresult(ASSESS_NAME, save_id, START_DATE);

            //更新归档标识
            this.Session["saveflag"] = "1";
            return rtn;
        }

        /// <summary>
        /// 关闭处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonclose(object sender, AjaxEventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");
            scManager.AddScript("Win_BatchInit.hide();");
        }

        /// <summary>
        /// 评价按钮，取得并检查评价的各项参数（未使用）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Create_Click(object sender, AjaxEventArgs e)
        {
            string startyear = this.Comb_StartYear.SelectedItem.Value;
            string startmonth = this.Comb_StartMonth.SelectedItem.Value;
            string endyear = this.Comb_StartYear.SelectedItem.Value;
            string endmonth = this.Comb_StartMonth.SelectedItem.Value;
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

            rtn = dal.GetdeptAssess_COMPUTE(startdate, enddate, startyear, incount);
            if (rtn.Equals(""))
            {
                DataTable dt = dal.GetDeptAssessResultTemp(incount, startyear, startyear + startmonth).Tables[0];
                this.Store1.DataSource = dt;
                this.Store1.DataBind();
                //this.Btn_View.Disabled = true;
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

        /// <summary>
        /// 检查开始结束月份
        /// </summary>
        /// <param name="year1"></param>
        /// <param name="month1"></param>
        /// <param name="year2"></param>
        /// <param name="month2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="msg"></param>
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

        //查看考核明细
        [AjaxMethod]
        public void GetInfo(string command, string colIndex, string deptcode)
        {
            LoadConfig loadcfg = new LoadConfig();
            switch (command)
            {
                case "NameInfo":
                    //显示人员详细信息界面
                    loadcfg = getLoadConfig("StaffInfo.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("staff_id", deptcode));
                    //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("benginDate", this.Session["benginDate"].ToString()));
                    StaffInfoWin.ClearContent();
                    StaffInfoWin.Show();
                    StaffInfoWin.LoadContent(loadcfg);
                    break;
                case "ResultInfo":
                    //指标数值详细信息
                    loadcfg = getLoadConfig("dept_Assess_ResultInfo.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("counting", ((User)(Session["CURRENTSTAFF"])).UserId));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", deptcode));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("benginDate", this.Session["benginDate"].ToString()));
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

        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = Comb_StartYear.SelectedItem.Value.ToString();
            string month = Comb_StartMonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + "" + month;
            return benginDate;
        }
    }
}
