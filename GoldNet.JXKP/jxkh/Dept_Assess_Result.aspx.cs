using System;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class Dept_Assess_Result : PageBase
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

                this.Session.Remove("saveflag");
            }
            if (!this.IsEdit())
            {
                this.Btn_Del.Hidden = true;
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
            //暂时不考虑是否是考核操作员
            //string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            string stationyear = DateTime.Now.Year.ToString();
            DataTable dt = dal.GetDeptArchAssess(stationyear, "").Tables[0];
            if (dt.Rows.Count.Equals(0))
            {
                this.Btn_View.Disabled = true;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.Comb_AssessName.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ASSESS_NAME"].ToString(), dt.Rows[i]["ASSESS_CODE"].ToString()));
                }
                this.Comb_AssessName.SelectedIndex = 0;
            }
            Goldnet.Dal.SYS_DEPT_DICT daldept = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = daldept.GetDeptType().Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.ComboBoxdepttype.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["ATTRIBUE"].ToString(), table.Rows[i]["id"].ToString()));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Comb_Year_Selected(object sender, AjaxEventArgs e)
        {
            string stationyear = this.Comb_StartYear.SelectedItem.Value;
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            DataTable dt = dal.GetDeptArchAssess(stationyear, incount).Tables[0];

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript(Comb_AssessName.ClientID + ".store.removeAll();");
            scManager.AddScript(Comb_AssessName.ClientID + ".clearValue();");

            if (dt.Rows.Count.Equals(0))
            {
                this.Btn_View.Disabled = true;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.Comb_AssessName.AddItem(dt.Rows[i]["ASSESS_NAME"].ToString(), dt.Rows[i]["ASSESS_CODE"].ToString());
                }
                this.Comb_AssessName.SelectedIndex = 0;
                this.Btn_View.Disabled = false;
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_View_Clicked(object sender, AjaxEventArgs e)
        {
            string stationyear = this.Comb_StartYear.SelectedItem.Value;
            string assesscode = this.Comb_AssessName.SelectedItem.Value;
            stationyear = stationyear.PadLeft(4, '0');
            string deptFilter = DeptFilter("");
            DataTable dt = dal.GetDeptAssessSavedArch(assesscode, stationyear, deptFilter, this.ComboBoxdepttype.SelectedItem.Value).Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (IsEdit())
                {
                    this.Btn_Del.Disabled = false;
                }
                else
                {
                    this.Btn_Del.Disabled = true;
                }
            }
            else
            {
                this.Btn_Excel.Disabled = true;
            }
            this.Btn_Del.Disabled = false;
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 删除处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Clicked(object sender, AjaxEventArgs e)
        {
            string assesscode = this.Comb_AssessName.SelectedItem.Value;
            string rtn = "";

            rtn = dal.DelDeptAssessSavedArch(assesscode);
            if (rtn.Equals(""))
            {
                showMsg("删除成功！");
                this.Store1.RemoveAll();
                this.Comb_AssessName.Value = "";
                this.Comb_AssessName.RemoveByValue(assesscode);
                this.Btn_View.Disabled = true;
                this.Btn_Excel.Disabled = true;
                this.Btn_Del.Disabled = true;
            }
            else
            {
                showMsg(rtn);
            }
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            string filename = this.Comb_AssessName.SelectedItem.Text;
            ExportData ex = new ExportData();
            string stationyear = this.Comb_StartYear.SelectedItem.Value;
            string assesscode = this.Comb_AssessName.SelectedItem.Value;
            stationyear = stationyear.PadLeft(4, '0');
            string deptFilter = DeptFilter("");
            DataTable dt = dal.GetDeptAssessSavedArch(assesscode, stationyear, deptFilter, this.ComboBoxdepttype.SelectedItem.Value).Tables[0];
            ex.ExportToLocal(dt, this.Page, "xls", filename);
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
        public void GetInfo(string command, string colIndex, string deptcode)
        {
            LoadConfig loadcfg = new LoadConfig();

            switch (command)
            {
                case "NameInfo":

                    loadcfg = getLoadConfig("StaffInfo.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("staff_id", deptcode));
                    StaffInfoWin.ClearContent();
                    StaffInfoWin.Show();
                    StaffInfoWin.LoadContent(loadcfg);
                    break;
                case "ResultInfo":
                    //指标数值详细信息
                    loadcfg = getLoadConfig("dept_Assess_ResultInfo.aspx");
                    //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("counting", ((User)(Session["CURRENTSTAFF"])).UserId));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("assess_code", this.Comb_AssessName.SelectedItem.Value));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", deptcode));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("benginDate", ""));
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
