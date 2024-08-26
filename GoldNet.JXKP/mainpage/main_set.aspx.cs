using System;
using System.Collections;
using System.Data;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using GoldNet.Comm;
using Goldnet.Dal.home;
using GoldNet.Model;
using System.Threading;

namespace GoldNet.JXKP.mainpage
{
    public partial class main_set : PageBase
    {
        HomeDal dal = new HomeDal();
        private static List<WorkItem> TaskThread;

        /// <summary>
        /// 初始化
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
                // 岗位代码
                string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();
                // 人员ID
                string personid = Session["curpersonid"] == null ? ((User)Session["CURRENTSTAFF"]).StaffId : ((User)Session["CURRENTSTAFF"]).GetStaffid(Session["curpersonid"].ToString());
                // 是否有编辑权限
                bool isEdit = IsEdit();

                if (isEdit)
                {
                    creatstationguide.Visible = true;
                }
                else
                {
                    creatstationguide.Visible = false;
                }

                ////获取岗位下的指标
                //DataTable table = dal.getBSCGuideTreeSelect(stationcode, "00").Tables[0];
                //Store2.DataSource = table;
                //Store2.DataBind();

                //初始化年度下拉列表
                DataTable yeartable = dal.getStationYear().Tables[0];
                SYear.DataSource = yeartable;
                SYear.DataBind();

                //获取区域代码,并设置下拉列表
                SetDictList();
            }
        }

        /// <summary>
        /// 填充字典信息
        /// </summary>
        /// <param name="organ"></param>
        protected void SetDictList()
        {
            this.Combo_DeptType.Items.Insert(0, new Goldnet.Ext.Web.ListItem("岗位指标监控", "00"));
            this.Combo_DeptType.Items.Insert(1, new Goldnet.Ext.Web.ListItem("门诊量图表", "01"));
            this.Combo_DeptType.Items.Insert(2, new Goldnet.Ext.Web.ListItem("医疗费用图表", "02"));
            this.Combo_DeptType.Items.Insert(3, new Goldnet.Ext.Web.ListItem("住院量图表", "03"));
            this.Combo_DeptType.Items.Insert(4, new Goldnet.Ext.Web.ListItem("手术量图表", "04"));
            this.Combo_DeptType.SelectedIndex = 0;
        }

        /// <summary>
        /// 在组织、科室下拉选择改变时，刷新树 , 获取Json
        /// </summary>
        /// <returns></returns>
        [AjaxMethod]
        public string RefreshTree()
        {
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "指标体系";
            root.Expanded = true;
            string depttype = this.Combo_DeptType.SelectedItem.Value.ToString();
            // 岗位代码
            string stationcode = this.ComboBox1.SelectedItem.Value.ToString(); // Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();

            // 获取当前岗位已选择的指标
            DataTable tablebsc = dal.getBSCGuideTree(stationcode).Tables[0];
            Hashtable rootnode = new Hashtable();
            for (int i = 0; i < tablebsc.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                node.NodeID = tablebsc.Rows[i]["id"].ToString();
                node.Text = tablebsc.Rows[i]["name"].ToString();
                node.Icon = Icon.Folder;
                string pid = tablebsc.Rows[i]["pid"].ToString();
                nodekey = pid;
                if (rootnode.Contains(nodekey))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[nodekey];
                    pnode.Nodes.Add(node);
                }
                else
                {
                    root.Nodes.Add(node);
                    rootnode.Add(node.NodeID, node);
                }
            }
            Goldnet.Ext.Web.TreeNodeCollection tnodec = new Goldnet.Ext.Web.TreeNodeCollection();
            tnodec.Add(root);
            return tnodec.ToJson();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="selectedid"></param>
        [AjaxMethod]
        public void TreeSelectedGuide(string bsc, string selectedid)
        {
            // 岗位代码
            //string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();

            string stationcode = this.ComboBox1.SelectedItem.Value.ToString();

            BindLeftSelector(bsc, stationcode, selectedid);
        }

        /// <summary>
        /// 绑字左侧选择列表
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="organtype"></param>
        /// <param name="depttype"></param>
        /// <param name="tag"></param>
        /// <param name="selectedid"></param>
        protected void BindLeftSelector(string bsc, string stationcode, string selectedid)
        {
            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(selectedid);
            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }
            string years = this.cbbYear.SelectedItem.Value.ToString();
            DataTable dt = dal.getBSCGuideTreeAll(bsc, stationcode, years).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (array.Contains(dt.Rows[i]["ID"].ToString()))
                {
                    dt.Rows.RemoveAt(i);
                    i--;
                }
            }
            Store1.DataSource = dt;
            Store1.DataBind();
        }

        /// <summary>
        /// 保存指标集定义中包含的指标及每项指标的权重
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveGuide(object sender, AjaxEventArgs e)
        {
            // 已选择的指标
            string selectedid = e.ExtraParams["multi2"];
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(selectedid);
            // 岗位代码
            string stationcode = this.ComboBox1.SelectedItem.Value.ToString(); //Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();
            // 区域代码
            string depttype = this.Combo_DeptType.SelectedItem.Value.ToString();
            // 标题
            string showTitle = this.txtTagName.Text;
            // 是否表示
            string showFlag = "1";
            if (this.Radio1.Checked)
            {
                // 显示
                showFlag = "1";
            }
            else if (this.Radio2.Checked)
            {
                // 不显示
                showFlag = "0";
            }

            //更新保存
            string rtn = dal.updateBSCGuide(stationcode, depttype, showTitle, showFlag, selectedRow);
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = (rtn.Equals("") ? "所选指标保存成功！" : rtn),
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }

        /// <summary>
        /// 选择年度获取岗位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search_Select(object sender, EventArgs e)
        {
            string year = this.cbbYear.SelectedItem.Value.ToString();
            bool isEdit = IsEdit();
            string stationdept = "";
            string StaffId = ((User)Session["CURRENTSTAFF"]).StaffId;
            string StationCode = ((User)Session["CURRENTSTAFF"]).GetStationCode(StaffId, DateTime.Now.Year.ToString());

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript(ComboBox1.ClientID + ".store.removeAll();");
            scManager.AddScript(ComboBox1.ClientID + ".clearValue();");

            if (!isEdit)
            {
                stationdept = "and station_code='" + StationCode + "'";
            }

            DataTable yeartable = dal.getStationByYear(year, stationdept).Tables[0];
            if (yeartable.Rows.Count > 0)
            {
                for (int i = 0; i < yeartable.Rows.Count; i++)
                {
                    this.ComboBox1.AddItem(yeartable.Rows[i]["stationname"].ToString(), yeartable.Rows[i]["stationcode"].ToString());
                }
            }
            this.ComboBox1.SelectedIndex = 0;

            this.txtTagName.Text = "";
            Store1.RemoveAll();
            Store1.DataBind();
        }

        /// <summary>
        /// 岗位选择处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void station_Select(object sender, EventArgs e)
        {
            string year = this.cbbYear.SelectedItem.Value.ToString();
            bool isEdit = IsEdit();
            string StationCode = this.ComboBox1.SelectedItem.Value.ToString();

            //获取岗位下的指标
            DataTable tables = dal.getBSCGuideTreeSelect(StationCode, "00").Tables[0];

            if (!Convert.IsDBNull(tables))
            {
                DataTable table = tables;
                Store2.DataSource = table;
                Store2.DataBind();
                if (table.Rows.Count > 0)
                {
                    // 标题设置
                    if (!Convert.IsDBNull(table.Rows[0]["SHOW_TITLE"]))
                    {
                        this.txtTagName.Text = table.Rows[0]["SHOW_TITLE"].ToString();
                    }
                    else
                    {
                        this.txtTagName.Text = "";
                    }

                    // 是否显示设置
                    if ("1".Equals(table.Rows[0]["SHOW_FLAG"].ToString()))
                    {
                        this.Radio1.Checked = true;
                        this.Radio2.Checked = false;
                    }
                    else
                    {
                        this.Radio1.Checked = false;
                        this.Radio2.Checked = true;
                    }
                }
            }
            else
            {
                this.txtTagName.Text = "";
                Store2.RemoveAll();
                Store2.DataBind();
            }

            //this.txtTagName.Text = "";
            Store1.RemoveAll();
            Store1.DataBind();
        }

        /// <summary>
        /// 区域选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void depttype_Select(object sender, EventArgs e)
        {
            string year = this.cbbYear.SelectedItem.Value.ToString();
            bool isEdit = IsEdit();
            string StationCode = this.ComboBox1.SelectedItem.Value.ToString();
            string depttype = this.Combo_DeptType.SelectedItem.Value.ToString();

            //获取岗位下的指标
            DataTable tables = dal.getBSCGuideTreeSelect(StationCode, depttype).Tables[0];

            if (!Convert.IsDBNull(tables))
            {
                DataTable table = tables;
                Store2.DataSource = table;
                Store2.DataBind();
                if (table.Rows.Count > 0)
                {
                    // 标题设置
                    if (!Convert.IsDBNull(table.Rows[0]["SHOW_TITLE"]))
                    {
                        this.txtTagName.Text = table.Rows[0]["SHOW_TITLE"].ToString();
                    }
                    else
                    {
                        this.txtTagName.Text = "";
                    }

                    // 是否显示设置
                    if ("1".Equals(table.Rows[0]["SHOW_FLAG"].ToString()))
                    {
                        this.Radio1.Checked = true;
                        this.Radio2.Checked = false;
                    }
                    else
                    {
                        this.Radio1.Checked = false;
                        this.Radio2.Checked = true;
                    }
                }
                else
                {
                    this.txtTagName.Text = "";
                    Store2.RemoveAll();
                    Store2.DataBind();
                }
            }

            //this.txtTagName.Text = "";
            Store1.RemoveAll();
            Store1.DataBind();
        }

        /// <summary>
        /// 生成岗位数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateStationGuide(object sender, AjaxEventArgs e)
        {
            // 是否有编辑权限
            bool isEdit = IsEdit();
            string station_year = this.cbbYear.SelectedItem.Value.ToString();
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;

            if (station_year == "")
            {
                showMsg("请输入要生成的岗位年度。");
                return;
            }

            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "";
            this.Btn_BatCancel.Text = "退出";
            this.Win_BatchInit.SetTitle(station_year + "年度岗位指标");
            this.Win_BatchInit.Show("Btn_BatInit");
            this.Btn_BatStart.Focus();

            //rtn = dal.CreateStationGuide(years, incount);
            //showMsg(rtn);
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void BatchInitTargetGuide(object state)
        {
            //目标参照年度
            string targetYear = this.cbbYear.SelectedItem.Value;

            string startdate = targetYear + "01" + "01";
            string enddate = targetYear + "12" + "31";

            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            string mess = "";
            try
            {
                this.Session["LongActionProgress"] = "1:正在生成年度指标值";
                mess = dal.CreateStationGuide(targetYear, incount, startdate, enddate);

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
                progressTip.Text = "岗位指标已完成!";
                this.Btn_BatCancel.Text = "退出";
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



        /// <summary>
        /// 信息提示
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

    }
}
