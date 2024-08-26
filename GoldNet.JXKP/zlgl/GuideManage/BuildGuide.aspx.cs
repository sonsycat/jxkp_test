using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;
using System.Threading;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace GoldNet.JXKP.zlgl.GuideSys
{
    public partial class BuildGuide : PageBase
    {
       public int year, month, days;
       public DateTime startDate, endDate;

        private List<WorkItem> TaskThread = new List<WorkItem>();
        //private bool Cancel;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                int curYear = 0;
                System.Globalization.Calendar cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
                curYear=year = cal.GetYear(DateTime.Now);
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear - 1), Convert.ToString(curYear - 1)));
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear), Convert.ToString(curYear)));
                this.cbbBeginYear.Items.Add(new Goldnet.Ext.Web.ListItem(Convert.ToString(curYear + 1), Convert.ToString(curYear + 1)));
                this.cbbBeginYear.SelectedItem.Value = curYear.ToString();
                for (int i = 1; i <= 12; i++)
                {
                    this.cbbBeginMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                }
                month = cal.GetMonth(DateTime.Now);
                this.cbbBeginMonth.SelectedItem.Value= month.ToString();
            }
        }
    

        /// <summary>
        /// 【生成】按钮按下，生成质量
        /// Cancel按钮变为【取消】状态
        /// 启动可取消的线程池，增加任务【BatchInitTargetGuide】
        /// 并将任务添加至TaskThread列表中
        /// 启动客户端的进度条刷新任务
        /// 设置Session["LongActionProgress"]任务执行状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_New_Add(object sender, EventArgs e)
        {
            string date = this.cbbBeginYear.SelectedItem.Value + "年" + this.cbbBeginMonth.SelectedItem.Value + "月";
            Ext.Msg.Confirm("系统提示", "您确定要生成-"+date+"-的质量数据吗？<br>如果这个月的数据存在会覆盖的，这个得确定好！", new MessageBox.ButtonsConfig
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
            Buttondel.Disabled = true;
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
            Buttondel.Disabled = false;
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
                    guidedal.DelCheckCollect(year+"-"+month,myTrans);
                    myTrans.Commit();
                    this.ShowMessage("系统提示","删除成功！");
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
                    this.ShowMessage("系统提示",msg);
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
                progressTip.Text = "质量统计完成";
                ScriptManager1.AddScript(" setCancelHidd()");
            }
        }
        protected void Buttonclose(object sender, AjaxEventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.BuideWin.hide();");
            
        }

        private void buidedate(object state)
        {
            year = int.Parse(this.cbbBeginYear.SelectedItem.Value);
            month = int.Parse(this.cbbBeginMonth.SelectedItem.Value);
            days = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(year, month);
            startDate = new DateTime(year, month, 1);
            endDate = new DateTime(year, month, days);
            using (OleDbConnection conn = new OleDbConnection(OracleOledbBase.ConnString))
            {
                conn.Open();
                using (OleDbTransaction myTrans = conn.BeginTransaction())
                {
                    OleDbCommand cmd = new OleDbCommand();
                    try
                    {
                        // 做质量汇总数据验证
                        this.Session["LongActionProgress"] = "1:正在数据验证(1/2)";
                        GoldNet.JXKP.BLL.Guide.privateTab1.verifyGuideData(year, month, startDate, endDate);
                        this.Session["LongActionProgress"] = "2:正在生成数据(2/2)";
                        GoldNet.JXKP.BLL.Guide.privateTab1.prepareData(year, month, startDate, endDate, myTrans);
                        myTrans.Commit();
                        this.Session.Remove("LongActionProgress");
                        this.Session.Remove("LastStep");
                        this.Session.Remove("NowCounter");
                    }
                    catch (GlobalException ex)
                    {
                        myTrans.Rollback();
                        BtnCancel.Disabled = true;
                        BtnClose.Disabled = false;
                        Buttondel.Disabled = false;
                        Progress1.UpdateProgress(1, "0%");
                        progressTip.Text = "进度";
                        this.Session["LongActionProgress"] ="-1:" +ex.Content;

                    }
                }
            }

        }
    }
}
