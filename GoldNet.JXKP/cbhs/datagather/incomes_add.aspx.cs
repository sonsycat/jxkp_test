using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;
using System.Threading;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class incomes_add : PageBase
    {
        public int year, month, days;
        public DateTime startDate, endDate;

        private List<WorkItem> TaskThread = new List<WorkItem>();
        //private bool Cancel;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                int curYear = 0;
                System.Globalization.Calendar cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
                curYear = year = cal.GetYear(DateTime.Now);
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear - 5), Convert.ToString(curYear - 5)));
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear - 4), Convert.ToString(curYear - 4)));
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear - 3), Convert.ToString(curYear - 3)));
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear - 2), Convert.ToString(curYear - 2)));
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear - 1), Convert.ToString(curYear - 1)));
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear), Convert.ToString(curYear)));
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_New_Add(object sender, EventArgs e)
        {
            string date_time = this.cbbBeginYear.SelectedItem.Value + this.cbbBeginMonth.SelectedItem.Value;
            Cbhs_dict dal_dict = new Cbhs_dict();
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
                Ext.Msg.Confirm("系统提示", "您确定要生成-" + date + "-的收入数据吗？<br>如果这个月的数据存在将会被覆盖！", new MessageBox.ButtonsConfig
                {
                    Yes = new MessageBox.ButtonConfig
                    {
                        Handler = "Goldnet.buide()",
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
        /// 
        /// </summary>
        [AjaxMethod]
        public void buide()
        {
            CalculateBonus cal_dal = new CalculateBonus();

            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "";

            this.Session["LongActionProgress"] = "0:";
            TaskThread.Add(AbortableThreadPool.QueueUserWorkItem(new WaitCallback(buidedate)));
            ScriptManager1.AddScript("{0}.startTask('longactionprogress');", TaskManager1.ClientID);
            Session["TaskThread"] = TaskThread;

            BtnCancel.Disabled = false;
            BtnClose.Disabled = true;

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("setHidden();");
        }

        /// <summary>
        /// 窗口关闭事件
        /// 如果线程池中存在正在执行的任务，则取消任务
        /// 并且移除Session任务执行状态
        /// 该事件是前台窗品点击[X]或【取消/退出】按钮时触发，
        /// 在hide关闭该窗口之前，如果有任务正在运行，在客户端对用户进行 confirm 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Cancel(object sender, AjaxEventArgs e)
        {
            TaskManager1.StopAll();
            Random rand = new Random();
            if (Session["TaskThread"] != null)
            {
                TaskThread = (List<WorkItem>)Session["TaskThread"];
            }
            while (TaskThread.Count > 0)
            {
                int i = rand.Next(TaskThread.Count);
                WorkItem item = TaskThread[i];
                TaskThread.RemoveAt(i);
                AbortableThreadPool.Cancel(item, true).ToString();
            }
            this.Session.Remove("LongActionProgress");
            this.Session.Remove("LastStep");
            this.Session.Remove("NowCounter");
            this.Session.Remove("TaskThread");
            //Cancel = true;
            BtnSave.Enabled = true;
            BtnCancel.Disabled = true;
            BtnClose.Disabled = false;
            Progress1.UpdateProgress(1, "0%");
            progressTip.Text = "进度";
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("setCancelHidd();");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttondeldata(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
            using (OleDbConnection conn = new OleDbConnection(OracleOledbBase.ConnString))
            {
                conn.Open();
                using (OleDbTransaction myTrans = conn.BeginTransaction())
                {
                    string year = this.cbbBeginYear.SelectedItem.Value;
                    string month = this.cbbBeginMonth.SelectedItem.Value;
                    guidedal.DelCheckCollect(year + "-" + month, myTrans);
                    myTrans.Commit();
                    this.ShowMessage("系统提示", "删除成功！");
                }
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
                //if (progress.ToString().Split(':').Length > 1)
                //{
                //    msg = progress.ToString().Split(':')[1].ToString();
                //}
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
                        float p = ((int)(i - 1)) / 2f;
                        Progress1.UpdateProgress(p, (p).ToString("p0"));
                    }
                    else
                    {
                        this.Session["NowCounter"] = (Convert.ToInt16(this.Session["NowCounter"].ToString()) + 2).ToString();
                        float p = (((int)(i - 1)) / 2f + (Convert.ToInt16(this.Session["NowCounter"].ToString()) > 45 ? 0.45f : Convert.ToInt16(this.Session["NowCounter"].ToString()) / 100f));
                        Progress1.UpdateProgress(p, (p).ToString("p0"));
                    }
                    if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 2 == 0)
                    {
                        progressTip.Text = msg + "，请稍候";
                    }
                    else if (Convert.ToInt16(this.Session["NowCounter"].ToString()) % 2 == 1)
                    {
                        progressTip.Text = msg + "，请稍候.";
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
                progressTip.Text = "统计完成";
                ScriptManager1.AddScript(" setCancelHidd()");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonclose(object sender, AjaxEventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            //scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.BuideWin.hide();");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void buidedate(object state)
        {
            //
            DataTable dtTaskData = new DataTable();
            string date_time = this.cbbBeginYear.SelectedItem.Value + this.cbbBeginMonth.SelectedItem.Value;

            Cbhs_dict dal_dict = new Cbhs_dict();
            AccountingData dal_acc = new AccountingData();

            Income_create dal = new Income_create();
            dtTaskData = dal.GetTask(date_time).Tables[0];
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

            //try
            {
                // 做质量汇总数据验证
                this.Session["LongActionProgress"] = "1:正在生成收入数据(1/2)";
                rtmsg = dal.Exec_Sp_Income_Auto_Acc(date_time);
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

                }
                rtmsg = dal.Exec_Sp_Income_Auto_Hos_Acc(date_time);
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


                    return;
                }

                this.Session["LongActionProgress"] = "2:正在核算数据(2/2)";
                rtmsg = dal.Exec_Sp_Income_To_Cost(date_time);
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


                }
                dal.SaveTask(dtTaskData, this.cbbBeginYear.SelectedItem.Value + this.cbbBeginMonth.SelectedItem.Value);
                this.Session.Remove("LongActionProgress");
                this.Session.Remove("LastStep");
                this.Session.Remove("NowCounter");
            }
            //catch (GlobalException ex)
            //{

            //    BtnCancel.Disabled = true;
            //    BtnClose.Disabled = false;

            //    Progress1.UpdateProgress(1, "0%");
            //    progressTip.Text = "进度";
            //    this.Session["LongActionProgress"] = "-1:" + ex.Content;

            //}

        }
    }
}
