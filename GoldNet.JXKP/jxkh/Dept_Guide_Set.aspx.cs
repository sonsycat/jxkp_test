using System;
using System.Data;
using Goldnet.Dal;
using GoldNet.Comm;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using GoldNet.Model;
using System.Threading;

namespace GoldNet.JXKP.jxkh
{
    public partial class Dept_Guide_Set : PageBase
    {
        private BoundComm boundcomm = new BoundComm();
        private Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
        private static List<WorkItem> TaskThread;
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //年度下拉列表初始化
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.Combo_TargetYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString() + "年", years.ToString()));
                }
                this.Combo_TargetYear.Value = System.DateTime.Now.Year.ToString();
                //科室类型下拉列表初始化
                Goldnet.Dal.SYS_DEPT_DICT daldept = new Goldnet.Dal.SYS_DEPT_DICT();
                DataTable table = daldept.GetDeptType().Tables[0];
                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        this.ComboBoxdepttype.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["ATTRIBUE"].ToString(), table.Rows[i]["id"].ToString()));

                    }
                }

                //查询科室信息表Dept_info
                Bindlist(DateTime.Now.Year.ToString(), "");

                //根据WEB配置更换列表表头
                for (int i = 0; i < this.GridPanel1.ColumnModel.Columns.Count; i++)
                {
                    if (this.GridPanel1.ColumnModel.Columns[i].ColumnID == "STATION_BSC_CLASS_01")
                        this.GridPanel1.ColumnModel.Columns[i].Header = GetConfig.GetConfigString("BSC01");
                    if (this.GridPanel1.ColumnModel.Columns[i].ColumnID == "STATION_BSC_CLASS_02")
                        this.GridPanel1.ColumnModel.Columns[i].Header = GetConfig.GetConfigString("BSC02");
                    if (this.GridPanel1.ColumnModel.Columns[i].ColumnID == "STATION_BSC_CLASS_03")
                        this.GridPanel1.ColumnModel.Columns[i].Header = GetConfig.GetConfigString("BSC03");
                    if (this.GridPanel1.ColumnModel.Columns[i].ColumnID == "STATION_BSC_CLASS_04")
                        this.GridPanel1.ColumnModel.Columns[i].Header = GetConfig.GetConfigString("BSC04");
                }

                SetStoreProxy();
            }
        }

        /// <summary>
        /// 初始化科室下拉列表
        /// </summary>
        private void SetStoreProxy()
        {
            HttpProxy pro2 = new HttpProxy();
            pro2.Method = HttpMethod.POST;
            pro2.Url = "../WebService/Depts.ashx";
            this.SDept.Proxy.Add(pro2);
        }

        /// <summary>
        /// 查询科室信息表Dept_info
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptcode"></param>
        protected void Bindlist(string year, string deptcode)
        {
            Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
            DataTable dt = dal.GetDeptGuide(year, deptcode, this.ComboBoxdepttype.SelectedItem.Value).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            string year = cbbYear.SelectedItem.Value;
            string deptcode = cbbdept.SelectedItem.Value;
            Bindlist(year, deptcode);
        }

        /// <summary>
        /// 数据集刷新处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string year = cbbYear.SelectedItem.Value;
            string deptcode = cbbdept.SelectedItem.Value;
            Bindlist(year, deptcode);
        }

        /// <summary>
        /// 编辑处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_edit_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string deptcode = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("Dept_Guide_Edit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("years", cbbYear.SelectedItem.Value));
                showCenterSet(this.DeptGuide_Edit, loadcfg);
            }

        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 科室指标设置处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Set_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string deptcode = selectRow[0]["DEPT_CODE"];
                string gathercode = selectRow[0]["GATHER_CODE"];
                LoadConfig loadcfg = getLoadConfig("DeptGuideInformation.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("gathercode", gathercode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("years", cbbYear.SelectedItem.Value));
                showCenterSet(this.DeptGuideSet, loadcfg);
            }
        }

        /// <summary>
        /// 科室指标集关联解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_unlock_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string deptcode = selectRow[0]["DEPT_CODE"];
                string gathercode = selectRow[0]["GATHER_CODE"];
                string selyear = cbbYear.SelectedItem.Value;

                Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
                dal.DelDeptGathers(selyear, deptcode, gathercode);
                this.SaveSucceed();
                Bindlist(selyear, "");
            }
        }

        /// <summary>
        /// 批量指标量化事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_BatInit_Click(object sender, AjaxEventArgs e)
        {
            string station_year = this.cbbYear.SelectedItem.Value;
            Progress1.UpdateProgress(0, " ");
            Progress1.Text = "";
            this.Btn_BatCancel.Text = "退出";
            this.Win_BatchInit.SetTitle(station_year + "年度批量指标量化");
            this.Win_BatchInit.Show("Btn_BatInit");
            this.Combo_TargetYear.Focus();

        }

        /// <summary>
        /// 批量指标量化“开始生成”按钮处理
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
        /// 生成过程取消事件处理
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
        /// 进度条更新处理
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
        /// 批量指标量化处理过程
        /// </summary>
        /// <param name="state"></param>
        private void BatchInitTargetGuide(object state)
        {
            //岗位年度
            string stationYear = this.cbbYear.SelectedItem.Value;

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

                this.Session["LongActionProgress"] = "3:正在更新科室绩效设置(2/3)";
                dal.UpdateDpetGathers(stationYear, targetYear, incount, System.DateTime.Now.ToString());

                this.Session["LongActionProgress"] = "3:正在更新科室指标设置(3/3)";
                dal.deptUpdate(stationYear, targetYear);

                //this.Session["LongActionProgress"] = "2:正在初始化科室指标信息(3/4)";
                //dal.deptInitial(targetYear, stationYear);

                //this.Session["LongActionProgress"] = "3:正在更新科室指标目标值(4/4)";
                //dal.UpdateDeptGuideValue(stationYear);

                

                this.Session.Remove("LongActionProgress");
                this.Session.Remove("LastStep");
                this.Session.Remove("NowCounter");
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("RefreshData();");
            }
            catch (Exception e)
            {
                this.Session["LongActionProgress"] = "-1:数据生成出错，请联系系统管理员!" + e.ToString();
            }
        }
    }
}
