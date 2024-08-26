using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Web.UI.WebControls;
using System.Threading;

namespace GoldNet.JXKP.jxkh
{
    public partial class Guide_Dict : System.Web.UI.Page
    {
        //
        Goldnet.Dal.Guide_Dict dal = new Goldnet.Dal.Guide_Dict();
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
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                this.start_date.Value = System.DateTime.Now.ToString("yyyy-MM") + "-01";
                this.end_date.Value = System.DateTime.Now.ToString("yyyy-MM") + "-01";
                Store_RefreshData(null, null);
            }
        }

        /// <summary>
        /// 获取指标列表数据
        /// </summary>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable table = dal.GetGuideDictList("", "", "", "").Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 删除指标操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            string codeStr = e.ExtraParams["Values"];
            string rtn = dal.DelGuide(codeStr);
            if (rtn.Equals(""))
            {
                Store_RefreshData(null, null);
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = rtn,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }

        /// <summary>
        /// 增加指标操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            Session.Remove("SelectedGuide");
            showDetailWin(getLoadConfig("Guide_Edit.aspx"), "添加指标", "543");
        }

        /// <summary>
        /// 编辑指标操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            string values = e.ExtraParams["Values"];
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(values);
            Session.Remove("SelectedGuide");
            Session.Add("SelectedGuide", selectedRow[0]);
            showDetailWin(getLoadConfig("Guide_Edit.aspx"), "编辑指标", "543");
        }

        /// <summary>
        /// 编辑指标语句
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Sql_Click(object sender, AjaxEventArgs e)
        {
            string values = e.ExtraParams["Values"];
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(values);
            Session.Remove("SelectedGuide");
            Session.Add("SelectedGuide", selectedRow[0]);
            showDetailWin(getLoadConfig("Guide_SQLExpress.aspx"), "指标算法信息", "543");
        }

        /// <summary>
        /// 全部指标生成处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Create_Click(object sender, AjaxEventArgs e)
        {
            //DateTime start_time = Convert.ToDateTime(Convert.ToDateTime(this.start_date.Value).ToString("yyyy-MM"));
            //DateTime end_time = Convert.ToDateTime(Convert.ToDateTime(this.end_date.Value).ToString("yyyy-MM"));
            //string rtn = "";
            //string tjny = "";

            TaskThread = new List<WorkItem>();
            this.cancel.Text = "取消";
            this.Session["LongActionProgress"] = "0:";
            TaskThread.Add(AbortableThreadPool.QueueUserWorkItem(new WaitCallback(BatchInitTargetGuide)));
            ScriptManager1.AddScript("{0}.startTask('longactionprogress');", TaskManager1.ClientID);
            this.Win_BatchInit.StyleSpec = "cursor: wait;";

            //string yearstr = DateTime.Now.Year.ToString();
            //for (int i = 1; i <= 12; i++)
            //{
            //    tjny = tjny + yearstr + i.ToString().PadLeft(2, '0') + ",";
            //}

            //for (DateTime i = start_time; i <= end_time; i = i.AddMonths(1))
            //{
            //    tjny = tjny + i.ToString("yyyyMM") + ",";
            //}

            //tjny = tjny.TrimEnd(',');
            //try
            //{
            //    OleDbParameter[] parameters = { new OleDbParameter("TJNY", tjny) };
            //    OracleOledbBase.RunProcedure(DataUser.HOSPITALSYS + ".ADD_GUIDE_VALUE", parameters);
            //    rtn = "全部指标数据生成成功！";
            //}
            //catch (Exception ee)
            //{
            //    rtn = "数据生成失败！<br/>原因：" + ee.Message;
            //}
            //Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            //{
            //    Title = SystemMsg.msgtitle4,
            //    Message = rtn,
            //    Buttons = MessageBox.Button.OK,
            //    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            //});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void BatchInitTargetGuide(object state)
        {
            DateTime start_time = Convert.ToDateTime(Convert.ToDateTime(this.start_date.Value).ToString("yyyy-MM"));
            DateTime end_time = Convert.ToDateTime(Convert.ToDateTime(this.end_date.Value).ToString("yyyy-MM"));
            string tjny = "";

            string yearstr = DateTime.Now.Year.ToString();
            //for (int i = 1; i <= 12; i++)
            //{
            //    tjny = tjny + yearstr + i.ToString().PadLeft(2, '0') + ",";
            //}

            for (DateTime i = start_time; i <= end_time; i = i.AddMonths(1))
            {
                tjny = tjny + i.ToString("yyyyMM") + ",";
            }
            tjny = tjny.TrimEnd(',');

            string mess = "";
            try
            {
                this.Session["LongActionProgress"] = "1:正在生成年度指标值";
                mess = dal.CreateStationGuide(tjny);

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
        /// 显示详细窗口
        /// </summary>
        /// <param name="loadcfg"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        private void showDetailWin(LoadConfig loadcfg, string title, string width)
        {
            DetailWin.ClearContent();
            if (!title.Trim().Equals(""))
            {
                DetailWin.SetTitle(title);
            }
            if (!width.Trim().Equals(""))
            {
                DetailWin.Width = Unit.Pixel(Convert.ToInt16(width));
            }
            DetailWin.Center();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        //载入参数设置
        private LoadConfig getLoadConfig(string url)
        {
            LoadConfig loadcfg = new LoadConfig();
            loadcfg.Url = url;
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            //loadcfg.NoCache = true;
            return loadcfg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateStationGuide(object sender, AjaxEventArgs e)
        {
            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "";
            this.cancel.Text = "退出";

            this.Win_BatchInit.Show("Win_BatchInit");
            this.cancel.Focus();

        }

        /// <summary>
        /// 
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
                    this.cancel.Text = "退出";
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
                progressTip.Text = "批量量化初始指标已完成!";
                this.cancel.Text = "退出";
                this.Win_BatchInit.StyleSpec = "cursor: auto;";
            }
        }

        /// <summary>
        /// 
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

    }
}
