using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;
using System.Collections;
using System.Threading;


namespace GoldNet.JXKP.jxkh
{
    public partial class StationManager : System.Web.UI.Page
    {
        private Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
        private static List<WorkItem> TaskThread;

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                //年度下拉框内容
                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.Combo_StationYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString() + "年", years.ToString()));
                    this.Combo_TargetYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString() + "年", years.ToString()));
                }
                this.Combo_StationYear.Value = System.DateTime.Now.Year.ToString();
                this.Combo_StationYear.Focus();
                this.Combo_TargetYear.Value = System.DateTime.Now.Year.ToString();
                Combo_StationYear.SetSize(100, 25);

                BuildDeptTree();
                this.Progress1.UpdateProgress(0, "0%");

                DataTable dt = dal.GetStationGatherDiff(System.DateTime.Now.Year.ToString()).Tables[0];
                this.Store2.DataSource = dt;
                this.Store2.DataBind();
            }
        }

        /// <summary>
        /// 设置字典
        /// </summary>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string deptcode = this.DeptCodeHidden.Value.ToString();
            if (!deptcode.Equals(""))
            {
                DataTable table = dal.GetStationListByDeptCode(deptcode, this.Combo_StationYear.SelectedItem.Value).Tables[0];
                this.Store1.DataSource = table;
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store2_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string station_year = this.Combo_StationYear.SelectedItem.Value;
            DataTable dt = dal.GetStationGatherDiff(station_year).Tables[0];
            this.Store2.RemoveAll();
            this.Store2.DataSource = dt;
            this.Store2.DataBind();
        }

        /// <summary>
        /// 岗位删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_DelStation_Click(object sender, AjaxEventArgs e)
        {
            string ids = "";
            string stationYear = this.Combo_StationYear.SelectedItem.Value;
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            foreach (SelectedRow row in sm.SelectedRows)
            {
                ids = ids + "'" + row.RecordID + "',";
            }
            if (!ids.Equals(""))
            {
                ids = ids.TrimEnd(',');
                dal.DelStationByCodeAndYear(ids, stationYear);
            }
            GridPanel1.DeleteSelected();

        }

        /// <summary>
        /// 岗位建立按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_CreateStation_Click(object sender, AjaxEventArgs e)
        {
            string station_year = this.Combo_StationYear.SelectedItem.Value;
            dal.CreateStation(station_year);
            Ext.Msg.Alert("系统提示", "已经建立并更新" + station_year + "年度岗位!").Show();
        }

        /// <summary>
        /// 年度岗位人员进岗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_PersonStation_Click(object sender, AjaxEventArgs e)
        {
            string station_year = this.Combo_StationYear.SelectedItem.Value;
            dal.PersonInStation(station_year);
            Ext.Msg.Alert("系统提示", "完成" + station_year + "年度岗位人员进岗!").Show();
        }

        #region ///===============批量指标量化=======================///

        /// <summary>
        /// 批量指标量化小窗口显示，初始化进度条，参照年度，窗口标题
        /// Cancel按钮初始状态为【退出】,参照年度获得光标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_BatInit_Click(object sender, AjaxEventArgs e)
        {
            string station_year = this.Combo_StationYear.SelectedItem.Value;
            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "";
            this.Btn_BatCancel.Text = "退出";
            this.Win_BatchInit.SetTitle(station_year + "年度批量指标量化");
            this.Win_BatchInit.Show("Btn_BatInit");
            this.Combo_TargetYear.Focus();
        }

        /// <summary>
        /// 【开始】按钮按下，批量量化初始指标目标值
        /// Cancel按钮变为【取消】状态
        /// 启动可取消的线程池，增加任务【BatchInitTargetGuide】
        /// 并将任务添加至TaskThread列表中
        /// 启动客户端的进度条刷新任务
        /// 设置Session["LongActionProgress"]任务执行状态
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
        /// 窗口关闭事件
        /// 如果线程池中存在正在执行的任务，则取消任务
        /// 并且移除Session任务执行状态
        /// 该事件是前台窗品点击[X]或【取消/退出】按钮时触发，
        /// 在hide关闭该窗口之前，如果有任务正在运行，在客户端对用户进行 confirm 确认。详见aspx 113 - 125行代码。
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
        /// 进度条刷新事件，时间间隔为1秒Interval="1000",在aspx 83行中设置
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
                    this.Btn_BatCancel.Text = "退出";
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
                this.Btn_BatCancel.Text = "退出";
                this.Win_BatchInit.StyleSpec = "cursor: auto;";
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
            //岗位年度
            string stationYear = this.Combo_StationYear.SelectedItem.Value;

            //目标参照年度
            string targetYear = this.Combo_TargetYear.SelectedItem.Value;
            string startdate = targetYear + "01";
            string enddate = targetYear + "12";
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            string mess = "";
            try
            {
                this.Session["LongActionProgress"] = "1:正在生成参照年度指标值(1/3)";
                mess = dal.GetYearTarget(startdate + "01", enddate + "31", targetYear, incount);

                this.Session["LongActionProgress"] = "2:正在初始化岗位指标信息(2/3)";
                dal.Initial(targetYear, stationYear);

                this.Session["LongActionProgress"] = "3:正在更新岗位指标目标值(3/3)";
                dal.UpdateGuideValue(stationYear);

                this.Session.Remove("LongActionProgress");
                this.Session.Remove("LastStep");
                this.Session.Remove("NowCounter");
            }
            catch (Exception e)
            {
                this.Session["LongActionProgress"] = "-1:数据生成出错，请联系系统管理员!" + e.ToString();
            }
        }

        #endregion

        /// <summary>
        /// 岗位年度刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Combo_StationYear_Selected(object sender, AjaxEventArgs e)
        {
            string station_year = this.Combo_StationYear.SelectedItem.Value;
            string deptcode = this.DeptCodeHidden.Value.ToString();
            if (!deptcode.Equals(""))
            {
                GridPanelRefresh(deptcode);
            }

            DataTable dt = dal.GetStationGatherDiff(station_year).Tables[0];
            this.Store2.RemoveAll();
            this.Store2.DataSource = dt;
            this.Store2.DataBind();
        }

        /// <summary>
        /// Ajax方法，刷新树节点对应的信息
        /// </summary>
        /// <param name="DeptCode"></param>
        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public void GridPanelRefresh(string DeptCode)
        {
            DataTable table = dal.GetStationListByDeptCode(DeptCode, this.Combo_StationYear.SelectedItem.Value).Tables[0];
            this.Store1.RemoveAll();
            this.Store1.DataSource = table;
            this.Store1.DataBind();
            if (table.Rows.Count == 0)
            {
                this.Btn_DelStation.Disabled = true;
                //RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            }
        }

        ///<summary>
        ///构造科室组织树
        ///</summary>
        protected void BuildDeptTree()
        {
            DataTable dt = dal.GetDeptTree().Tables[0];
            Hashtable rootnode = new Hashtable();

            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "绩效考评";
            root.Expanded = true;
            root.SingleClickExpand = true;
            this.TreeCtrl.Root.Clear();
            this.TreeCtrl.Root.Add(root);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string nodeid = dt.Rows[i]["ID"].ToString();
                string nodetext = dt.Rows[i]["NAME"].ToString();
                string attr = dt.Rows[i]["ATTR"].ToString();
                string pid = dt.Rows[i]["PID"].ToString();
                string isleaf = dt.Rows[i]["ISLEAF"].ToString();
                Goldnet.Ext.Web.TreeNode node1 = new Goldnet.Ext.Web.TreeNode();
                node1.NodeID = nodeid;
                node1.Text = nodetext;
                node1.SingleClickExpand = true;
                //改用客户端加上统一的js方法refreshGrid() ,刷新表格数据
                //node1.Listeners.Click.Handler = "e.stopEvent();GridPanel1.setTitle('');";
                if (nodetext.IndexOf("临床") == 0)
                {
                    node1.Expanded = true;
                }
                if (isleaf.Equals("1"))
                {
                    node1.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                }
                if (rootnode.Contains(pid))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[pid];
                    pnode.Nodes.Add(node1);
                }
                else
                {
                    root.Nodes.Add(node1);
                }
                if (isleaf.Equals("0"))
                {
                    rootnode.Add(nodeid, node1);
                }
            }
        }

        # region  ///===============表格中的四个功能按钮【编辑】【量化】【下属】【评测】============================///

        /// <summary>
        /// Ajax方法，刷新树节点对应的信息
        /// </summary>
        /// <param name="DeptCode"></param>
        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public void ShowDetailWindow(string command, string stationcode, string guidegathercode, string stationname, string deptcode, string deptname)
        {
            //command="CmdBJGW","编辑岗位信息"
            //command="CmdZBLH","岗位指标量化"
            //command="CmdXSRY","岗位下属人员"
            //command="CmdJXPC","绩效评测"


            string stationYear = this.Combo_StationYear.SelectedItem.Value;
            if (command == "CmdBJGW")
            {
                showDetailWin(getLoadConfig("StationManagerInfo.aspx?id=" + stationcode + "&sy=" + stationYear), "岗位详细信息--" + stationYear + "年度", "560");
            }
            else if (command == "CmdZBLH")
            {
                showDetailWin(getLoadConfig("StationGuideInformation.aspx?id=" + stationcode + "&sy=" + stationYear + "&gd=" + guidegathercode), "岗位指标量化--" + stationname + "--" + stationYear + "年度", "980");
            }
            else if (command == "CmdXSRY")
            {
                showDetailWin(getLoadConfig("StationPersonnel.aspx?id=" + stationcode + "&sy=" + stationYear + "&dc=" + deptcode), deptname + "==>" + stationname + " 岗位下属人员--" + stationYear + "年度", "560");
            }
            else if (command == "CmdJXPC")
            {
                string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
                dal.GetStationTestKH(stationYear + "0101", stationYear + "1231", stationYear, incount, stationcode);
                showDetailWin(getLoadConfig("StationEvaluation.aspx?id=" + stationcode + "&sy=" + stationYear), "岗位绩效评测--" + stationname + "--" + stationYear + "年度", "800");
            }
        }

        //显示详细窗口
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
            loadcfg.NoCache = true;
            return loadcfg;
        }

        //反序列化得到客户端提交的gridpanel数据行
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0)
            {
                return null;
            }
            else
            {
                return selectRow;
            }
        }

        #endregion
    }
}
