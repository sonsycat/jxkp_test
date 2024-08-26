using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm;
using System.Threading;
using GoldNet.Comm.DAL.Oracle;
using System.Data.OleDb;

namespace GoldNet.JXKP
{
    public partial class BonusNewAdd : PageBase
    {
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
                Progress1.UpdateProgress(1, "0%");
                BoundComm boundcomm = new BoundComm();
                CheckPersons checkpersons = new CheckPersons();
                Store1.DataSource = boundcomm.getYears();
                Store1.DataBind();
                Store2.DataSource = boundcomm.getMonth();
                Store2.DataBind();
                string Mode = Request["Mode"];
                if (Mode == "NewRebuild")
                {
                    string bonusName = Request["BonusName"];
                    string years = Request["Years"];
                    string months = Request["Months"];
                    cbbBeginYear.SetValue(years);
                    cbbBeginMonth.SetValue(months);
                    cbbBeginYear.SelectedItem.Text = years;
                    cbbBeginYear.SelectedItem.Value = years;
                    cbbBeginMonth.SelectedItem.Text = months;
                    cbbBeginMonth.SelectedItem.Value = months;
                    tfBName.Text = bonusName;


                    tfBName.Disabled = true;
                    cbbBeginYear.Disabled = true;
                    cbbBeginMonth.Disabled = true;
                }
                else
                {
                    cbbBeginYear.SetValue(DateTime.Now.Year);
                    cbbBeginMonth.SetValue(DateTime.Now.Month);
                    cbbBeginYear.SelectedItem.Text = DateTime.Now.Year.ToString();
                    cbbBeginYear.SelectedItem.Value = DateTime.Now.Year.ToString();
                    cbbBeginMonth.SelectedItem.Text = DateTime.Now.Month.ToString();
                    cbbBeginMonth.SelectedItem.Value = DateTime.Now.Month.ToString();


                }
                BtnCancel.Disabled = true;
            }
        }
        #region ///===============生成奖金=======================///

        /// <summary>
        /// 【生成】按钮按下，生成奖金
        /// Cancel按钮变为【取消】状态
        /// 启动可取消的线程池，增加任务【BatchInitTargetGuide】
        /// 并将任务添加至TaskThread列表中
        /// 启动客户端的进度条刷新任务
        /// 设置Session["LongActionProgress"]任务执行状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_New_Add(object sender, AjaxEventArgs e)
        {
            CalculateBonus cal_dal = new CalculateBonus();
            if (!cal_dal.CheckGuideBase())
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "奖金必备信息错误",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                return;
            }
            string Mode = Request["Mode"];
            if (Mode == "NewRebuild")
            {
                string bonusid = Request["BonusID"];
                cal_dal.DeleteBonusById(bonusid);
            }
            else
            {
                string years = cbbBeginYear.SelectedItem.Text;
                string months = cbbBeginMonth.SelectedItem.Text;
                string msg = cal_dal.CheckDeptTypeSet(years, months);
                if (msg != "")
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = years + "年" + months + "月的" + msg,
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                if (cal_dal.CheckBonusIsExist(years, months))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = years + "年" + months + "月的奖金已经存在，不可以再新建",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }
            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "";

            this.Session["LongActionProgress"] = "1:";
            TaskThread.Add(AbortableThreadPool.QueueUserWorkItem(new WaitCallback(BatchInitTargetGuide)));
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
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = msg,
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });

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
                progressTip.Text = "奖金计算完成";
                ScriptManager1.AddScript(" setCancelHidd()");
            }
        }

        /// <summary>
        /// 根据参照年度，批量初始化年度指标目标值
        /// 由于该函数由系统线程池托管，故不能弹出任何Ext Alert提示
        /// 只能将出错信息放至Session中，由进度条刷新函数RefreshProgress进行处理
        /// </summary>
        /// <param name="state"></param>
        private void BatchInitTargetGuide(object state)
        {
            CalculateBonus calculatebonus = new CalculateBonus();
            string bonusName = tfBName.Text;
            int beginYear = Convert.ToInt32(cbbBeginYear.SelectedItem.Text);
            int beginMonth = Convert.ToInt32(cbbBeginMonth.SelectedItem.Text);
            int bonusid = OracleOledbBase.GetMaxID("id", "Performance.Bonus_Index");

            //try
            //{
                //using (OleDbConnection conn = new OleDbConnection(OracleOledbBase.ConnString))
                //{
                //    conn.Open();
                //    using (OleDbTransaction myTrans = conn.BeginTransaction())
                //    {
                //        OleDbCommand cmd = new OleDbCommand();
                      
                //        {
                            this.Session["LongActionProgress"] = "1:正在计算奖金(1/2)";
                            //执行计算奖金的指标存储过程
                            //calculatebonus.RunGuide(beginYear, beginMonth, myTrans);
                            calculatebonus.RunGuide_oracle(beginYear, beginMonth);

                            this.Session["LongActionProgress"] = "2:正在记录超劳补贴(2/2)";
                            //添加奖金主表
                            //calculatebonus.AddMainBonus(bonusName, beginYear, beginMonth, bonusid, myTrans);
                            calculatebonus.AddMainBonus_oracle(bonusName, beginYear, beginMonth, bonusid);

                            //this.Session["LongActionProgress"] = "3:添加奖金指标(3/6)";
                            //添加奖金指标
                            //calculatebonus.AddBonusGuide(bonusid, myTrans);
                            calculatebonus.AddBonusGuide_oracle(bonusid);

                            //this.Session["LongActionProgress"] = "4:添加奖金指标值(4/6)";
                            //添加奖金指标值
                            //calculatebonus.AddBonusValue(beginYear, beginMonth, bonusid, myTrans);
                            calculatebonus.AddBonusValue_oracle(beginYear, beginMonth, bonusid);

                            //this.Session["LongActionProgress"] = "5:奖金科室(5/6)";
                            //奖金科室
                            //calculatebonus.AddBonusDept(bonusid, myTrans);
                            calculatebonus.AddBonusDept_oracle(bonusid);

                            //this.Session["LongActionProgress"] = "6:科室人员(6/6)";
                            //科室人员
                            //calculatebonus.AddBonusPerson(bonusid, myTrans);
                            calculatebonus.AddBonusPerson_oracle(bonusid);
                            //科室留存处理
                            //calculatebonus.AddBonusBalance(beginYear, beginMonth);
                            //myTrans.Commit();
                            this.Session.Remove("LongActionProgress");
                            this.Session.Remove("LastStep");
                            this.Session.Remove("NowCounter");
                //        }
                       
                //    }
                //}


            //}
            //catch (Exception e)
            //{
            //     this.Session["LongActionProgress"] = "-1:数据生成出错，请联系系统管理员!" + e.ToString();
            //    BtnCancel.Disabled = true;
            //    BtnClose.Disabled = false;
            //    Progress1.UpdateProgress(1, "0%");
            //    progressTip.Text = "进度";
            //}

        }

        #endregion
    }
}
