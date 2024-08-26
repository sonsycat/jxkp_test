using System;
using System.Data;
using Goldnet.Dal;
using GoldNet.Comm;
using Goldnet.Ext.Web;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class income_create : PageBase
    {
        private static DataTable dtTaskData = new DataTable();
        private static List<WorkItem> TaskThread;
        public int year, month, days;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    //               Response.End();
                }

                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
                Cbhs_dict dal = new Cbhs_dict();
                DataTable dt = dal.GetAccount_Signs().Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.IncomeItem.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ACCOUNT_TYPE"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
                this.IncomeItem.SelectedItem.Value = GetConfig.GetConfigString("accounttype");
                //BindData();

                int curYear = 0;
                System.Globalization.Calendar cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
                curYear = year = cal.GetYear(DateTime.Now);
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear - 1), Convert.ToString(curYear - 1)));
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear), Convert.ToString(curYear)));
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear + 1), Convert.ToString(curYear + 1)));
                this.cbbBeginYear.SelectedItem.Value = curYear.ToString();
                for (int i = 1; i <= 12; i++)
                {
                    string months = i.ToString();
                    if (months.Length == 1)
                        months = "0" + months;
                    this.cbbBeginMonth.Items.Add(new Goldnet.Ext.Web.ListItem(months, months));
                }

            }
        }

        /// <summary>
        /// 查询并绑定收入生成过程的操作记录
        /// </summary>
        /// <param name="date_time"></param>
        private void SetTaskData(string date_time)
        {
            Income_create dal = new Income_create();
            dtTaskData = dal.GetTask(date_time).Tables[0];
            this.Store1.DataSource = dtTaskData;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 查询绑定分解前收入
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="balance_tag"></param>
        private void SetIncome(string date_time, string balance_tag)
        {
            Income_create dal = new Income_create();
            DataTable dt = new DataTable();
            if (balance_tag == "0")
            {
                //发生
                dt = dal.GetIncomes(date_time).Tables[0];
            }
            else
            {
                //结算
                dt = dal.GetSettleIncomes(date_time).Tables[0];
            }
            this.Store2.DataSource = dt;
            this.Store2.DataBind();
        }

        /// <summary>
        /// 查询绑定分解后数据
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="balance_tag"></param>
        private void SetIncomeCreated(string date_time, string balance_tag)
        {
            Income_create dal = new Income_create();
            DataTable dt = dal.GetIncomesCreated(date_time, balance_tag).Tables[0];
            this.Store3.DataSource = dt;
            this.Store3.DataBind();
        }

        /// <summary>
        /// 查询绑定分解后数据
        /// </summary>
        private void BindData()
        {
            //查询并绑定收入生成操作记录
            SetTaskData(this.years.SelectedItem.Value + this.months.SelectedItem.Value);
            //获取分解前收入
            SetIncome(this.years.SelectedItem.Value + this.months.SelectedItem.Value, this.IncomeItem.SelectedItem.Value);
            //获取分解后收入
            SetIncomeCreated(this.years.SelectedItem.Value + this.months.SelectedItem.Value, this.IncomeItem.SelectedItem.Value);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("clare();");
            //显示图表数据
            string series1 = GetDate(this.years.SelectedItem.Value + this.months.SelectedItem.Value, this.IncomeItem.SelectedItem.Value, "chart1");
            if (series1 != "")
            {
                scManager.AddScript(" refreshCharts1(" + series1 + ");");
            }
            string series2 = GetDate(this.years.SelectedItem.Value + this.months.SelectedItem.Value, this.IncomeItem.SelectedItem.Value, "chart2");
            if (series2 != "")
            {
                scManager.AddScript(" refreshCharts2(" + series2 + ");");
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void IncomeItem_SelectOnChange(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_create_click(object sender, AjaxEventArgs e)
        {
            data("1");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_create_click_inp(object sender, AjaxEventArgs e)
        {
            data("0");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_create_click_dept(object sender, AjaxEventArgs e)
        {
            data("2");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="costtype"></param>
        private void data(string costtype)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;

            Cbhs_dict dal_dict = new Cbhs_dict();
            AccountingData dal_acc = new AccountingData();
            if (dal_dict.IsBonusSave(date_time))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "该月奖金已经生成、不可以改变收入数据!",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            else
            {
                //验证核算数据是否生成
                if (dal_acc.IsAccount(date_time))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "该月奖金核算已经完成、不可以改变收入数据!",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }

            Income_create dal = new Income_create();
            string rtmsg = "";
            //先清空时间和状态列
            for (int i = 0; i < dtTaskData.Rows.Count; i++)
            {
                dtTaskData.Rows[i]["TASK_TIME"] = "";
                dtTaskData.Rows[i]["TASK_STATE"] = "";
                dtTaskData.Rows[0]["TASK_ERR"] = "";
            }
            //验证奖金是否已经生成
            Cbhs_dict cbhsDict = new Cbhs_dict();
            try
            {
                if (cbhsDict.IsBonusSave(date_time))
                {
                    dtTaskData.Rows[0]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[0]["TASK_STATE"] = "未通过";
                    dtTaskData.Rows[0]["TASK_ERR"] = "奖金已经生成";
                    for (int i = 1; i < dtTaskData.Rows.Count; i++)
                    {
                        dtTaskData.Rows[i]["TASK_TIME"] = System.DateTime.Now.ToString();
                        dtTaskData.Rows[i]["TASK_STATE"] = "未执行";
                    }

                    this.ShowDataError("生成失败、原因奖金已经生成", Request.Path, "Button_create_click");

                    dal.SaveTask(dtTaskData, this.years.SelectedItem.Value + this.months.SelectedItem.Value);
                    this.Store1.DataSource = dtTaskData;
                    this.Store1.DataBind();
                    return;
                }
                else
                {
                    dtTaskData.Rows[0]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[0]["TASK_STATE"] = "通过";
                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                return;
            }
            //门诊收入分解
            try
            {
                if (costtype == "1")
                {
                    rtmsg = dal.Exec_Sp_Income_Auto_Acc(date_time);
                }
                else
                {
                    rtmsg = "";
                }
                if (rtmsg == "")
                {
                    dtTaskData.Rows[1]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[1]["TASK_STATE"] = "通过";
                    dtTaskData.Rows[2]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[2]["TASK_STATE"] = "通过";
                }
                else
                {
                    dtTaskData.Rows[1]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[1]["TASK_STATE"] = "未通过";
                    dtTaskData.Rows[1]["TASK_ERR"] = rtmsg;
                    dtTaskData.Rows[2]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[2]["TASK_STATE"] = "未通过";
                    dtTaskData.Rows[2]["TASK_ERR"] = rtmsg;
                    for (int i = 3; i < dtTaskData.Rows.Count; i++)
                    {
                        dtTaskData.Rows[i]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dtTaskData.Rows[i]["TASK_STATE"] = "未执行";
                    }
                    this.ShowDataError("生成失败，原因：" + rtmsg, Request.Path, "Button_create_click");
                    dal.SaveTask(dtTaskData, this.years.SelectedItem.Value + this.months.SelectedItem.Value);
                    this.Store1.DataSource = dtTaskData;
                    this.Store1.DataBind();
                    return;
                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                return;
            }
            //住院收入分解
            try
            {
                if (costtype == "0")
                {
                    rtmsg = dal.Exec_Sp_Income_Auto_Hos_Acc(date_time);
                }
                else
                {
                    rtmsg = "";
                }
                if (rtmsg == "")
                {
                    dtTaskData.Rows[3]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[3]["TASK_STATE"] = "通过";
                    dtTaskData.Rows[4]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[4]["TASK_STATE"] = "通过";
                }
                else
                {
                    dtTaskData.Rows[3]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[3]["TASK_STATE"] = "未通过";
                    dtTaskData.Rows[3]["TASK_ERR"] = rtmsg;
                    dtTaskData.Rows[4]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[4]["TASK_STATE"] = "未通过";
                    dtTaskData.Rows[4]["TASK_ERR"] = rtmsg;
                    for (int i = 5; i < dtTaskData.Rows.Count; i++)
                    {
                        dtTaskData.Rows[i]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dtTaskData.Rows[i]["TASK_STATE"] = "未执行";
                    }

                    this.ShowDataError("生成失败，原因：" + rtmsg, Request.Path, "Button_create_click");

                    dal.SaveTask(dtTaskData, this.years.SelectedItem.Value + this.months.SelectedItem.Value);
                    this.Store1.DataSource = dtTaskData;
                    this.Store1.DataBind();
                    return;
                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                return;
            }
            //折算成本
            try
            {
                if (costtype == "2")
                {
                    rtmsg = dal.Exec_Sp_Income_To_Cost(date_time);
                }
                else
                {
                    rtmsg = "";
                }
                if (rtmsg == "")
                {
                    dtTaskData.Rows[5]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[5]["TASK_STATE"] = "通过";
                    dtTaskData.Rows[6]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[6]["TASK_STATE"] = "通过";
                    dtTaskData.Rows[7]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[7]["TASK_STATE"] = "通过";
                }
                else
                {
                    dtTaskData.Rows[5]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[5]["TASK_STATE"] = "未通过";
                    dtTaskData.Rows[5]["TASK_ERR"] = rtmsg;
                    dtTaskData.Rows[6]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[6]["TASK_STATE"] = "未通过";
                    dtTaskData.Rows[6]["TASK_ERR"] = rtmsg;
                    dtTaskData.Rows[7]["TASK_TIME"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dtTaskData.Rows[7]["TASK_STATE"] = "未执行";
                    this.ShowDataError("生成失败，原因：" + rtmsg, Request.Path, "Button_create_click");

                    dal.SaveTask(dtTaskData, this.years.SelectedItem.Value + this.months.SelectedItem.Value);
                    this.Store1.DataSource = dtTaskData;
                    this.Store1.DataBind();
                    return;
                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                return;
            }
            string titlename = costtype == "1" ? "门诊收入" : "住院收入";
            if (costtype == "2") titlename = "科室核算";
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = "信息提示",
                Message = titlename + "生成成功",
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
            this.Store1.DataSource = dtTaskData;
            this.Store1.DataBind();
            Cost_detail prodal = new Cost_detail();
            prodal.Exec_Sp_Prog_Ratio(date_time);

            //保存操作记录
            dal.SaveTask(dtTaskData, this.years.SelectedItem.Value + this.months.SelectedItem.Value);
            BindData();
        }

        /// <summary>
        /// 显示图表数据
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="balance_tag"></param>
        /// <param name="chart"></param>
        /// <returns></returns>
        public static string GetDate(string date_time, string balance_tag, string chart)
        {
            Income_create dal = new Income_create();
            DataTable dt = new DataTable();
            if (chart == "chart1")
            {
                if (balance_tag == "0")
                {
                    //发生
                    dt = dal.GetIncomes(date_time).Tables[0];
                }
                else
                {
                    //结算
                    dt = dal.GetSettleIncomes(date_time).Tables[0];
                }
            }
            else
            {
                dt = dal.GetIncomesCreated(date_time, balance_tag).Tables[0];
            }
            StringBuilder Series = new StringBuilder();

            Series.Append("[{ type:'pie',name:'饼型分析图', data: [");
            bool bl = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["COSTS"].ToString() != "0" && dt.Rows[i]["ITEM_NAME"].ToString() != "合计")
                {
                    Series.Append("{name:'");
                    Series.Append(dt.Rows[i]["ITEM_NAME"].ToString());
                    Series.Append("',y:");
                    Series.Append(dt.Rows[i]["COSTS"].ToString());
                    Series.Append("},");
                    bl = true;
                }
            }
            if (!bl)
            {
                return "";
            }
            Series.Remove(Series.Length - 1, 1).Append("]}]");
            return Series.ToString();
        }

        /// <summary>
        /// 收入生成处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buide_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("incomes_add.aspx");

            showCenterSet(this.BuideWin, loadcfg);
        }

        /// <summary>
        /// 抓取收入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void getdata_Click(object sender, AjaxEventArgs e)
        {
            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "";
            this.Btn_BatCancel.Text = "退出";
            this.Win_BatchInit.SetTitle("收入数据提取");
            this.Win_BatchInit.Show("Btn_BatInit");
            this.cbbBeginYear.Focus();
        }

        /// <summary>
        /// 抓取收入数据开始处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_BatStart_Click(object sender, AjaxEventArgs e)
        {
            Cbhs_dict dal_dict = new Cbhs_dict();

            string date_time = this.cbbBeginYear.SelectedItem.Value + this.cbbBeginMonth.SelectedItem.Value;
            if (this.cbbBeginMonth.SelectedItem.Text == "" || this.cbbBeginYear.SelectedItem.Text == "")
            {
                this.ShowMessage("系统提示", "选择时间！");
            }
            else if (dal_dict.IsBonusSave(date_time))
            {
                this.ShowMessage("系统提示", "该月奖金已经生成，不能重新生成数据！");
            }
            else
            {
                string date = this.cbbBeginYear.SelectedItem.Value + "年" + this.cbbBeginMonth.SelectedItem.Value + "月";
                Ext.Msg.Confirm("系统提示", "您确定要提取-" + date + "-的收入数据吗？<br>如果这个月的数据存在将会被覆盖！", new MessageBox.ButtonsConfig
                {
                    Yes = new MessageBox.ButtonConfig
                    {
                        Handler = "CompanyX.buide()",
                        Text = "确定"

                    },
                    No = new MessageBox.ButtonConfig
                    {
                        Text = "取消"
                    }
                }).Show();
            }
        }

        /// <summary>
        /// 开始执行处理
        /// </summary>
        [AjaxMethod]
        public void buide()
        {
            TaskThread = new List<WorkItem>();
            this.Btn_BatCancel.Text = "取消";
            this.Session["LongActionProgress"] = "0:";
            TaskThread.Add(AbortableThreadPool.QueueUserWorkItem(new WaitCallback(BatchInitTargetGuide)));
            ScriptManager1.AddScript("{0}.startTask('longactionprogress');", TaskManager1.ClientID);
            this.Win_BatchInit.StyleSpec = "cursor: wait;";
        }

        /// <summary>
        /// 抓取收入数据停止
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
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void BatchInitTargetGuide(object state)
        {
            //提取年月
            string year = this.cbbBeginYear.SelectedItem.Value;
            string month = this.cbbBeginMonth.SelectedItem.Value;

            DateTime sdate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            DateTime edate = sdate.AddMonths(1).AddDays(-1);
            string startdate = sdate.ToString("yyyyMMdd");
            string enddate = edate.ToString("yyyyMMdd");

            Income_create dal = new Income_create();
            string mess = "";
            try
            {
                this.Session["LongActionProgress"] = "1:正在提取军卫收入(1/3)";
                mess = dal.Exec_Sp_Extrac_Yxt_data_Pre(startdate, enddate);

                this.Session["LongActionProgress"] = "2:正在初始化收入数据(2/3)";
                mess = dal.Exec_Sp_Extrac_Yxt_data(startdate, enddate);

                this.Session["LongActionProgress"] = "3:正在收入预处理(3/3)";
                mess = dal.Exec_Sp_Income_Calc_Per(startdate, enddate);

                this.Session.Remove("LongActionProgress");
                this.Session.Remove("LastStep");
                this.Session.Remove("NowCounter");
            }
            catch (Exception e)
            {
                this.Session["LongActionProgress"] = "-1:数据生成出错，请联系系统管理员!" + e.ToString();
            }
        }

        /// <summary>
        /// 进度条刷新事件，时间间隔为1秒Interval="1000"
        /// 根据Session["LongActionProgress"]设置进度条提示文本
        /// 线程任务结束运行后，该客户端刷新任务TaskManager1同时也停止
        /// 进度条文本提示任务结束,并且移除Session任务执行状态
        /// 并且将【取消】按钮变为【退出】按钮
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
                msg = progress.ToString();

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
                    this.ShowMessage("系统提示", msg);
                    ScriptManager1.AddScript(" setCancelHidd()");
                }
            }
            else
            {
                this.Session.Remove("LongActionProgress");
                this.Session.Remove("LastStep");
                this.Session.Remove("NowCounter");
                ScriptManager1.AddScript("{0}.stopTask('longactionprogress');", TaskManager1.ClientID);
                Progress1.UpdateProgress(1, "100%");
                progressTip.Text = "提取完成";
                ScriptManager1.AddScript(" setCancelHidd()");
            }
        }

    }
}
