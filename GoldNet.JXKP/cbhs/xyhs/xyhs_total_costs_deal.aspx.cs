using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using Goldnet.Dal;
using GoldNet.Model;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_total_costs_deal : PageBase
    {
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
                string date_time = System.DateTime.Now.ToString("yyyyMM");
                SetDict(date_time);
                string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
                Bindlist(item_code, date_time);
                if (IsEdit())
                {
                    this.Button_del.Disabled = false;
                    this.Button_save.Disabled = false;
                    this.Button_break.Disabled = false;
                }
                else
                {
                    this.Button_del.Disabled = true;
                    this.Button_save.Disabled = true;
                    this.Button_break.Disabled = true;
                }
            }

        }
        //查询绑定数据
        protected void Bindlist(string item_code, string date_time)
        {
            XyhsDetail dal = new XyhsDetail();
            string deptFilter = DeptFilter("");
            DataTable dt = dal.GetDeptClass1Costs(date_time, item_code, deptFilter).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //界面菜单初始化
        protected void SetDict(string date_time)
        {
            this.COST_ITEM.Value = string.Empty;
            this.AllCosts.Clear();
            XyhsDetail dal = new XyhsDetail();
            DataTable dt =  dal.GetTotalCostsItems(date_time).Tables[0];
            this.Store2.DataSource = dt;
            this.Store2.DataBind();
            if (dt.Rows.Count > 0)
            {
                this.COST_ITEM.SelectedItem.Value = dt.Rows[0]["ITEM_CODE"].ToString();
                this.AllCosts.ReadOnly = false;
                this.AllCosts.Value = dal.GetItemAllCosts(date_time, dt.Rows[0]["ITEM_CODE"].ToString());
                this.AllCosts.ReadOnly = true;
            }
        }
        //查询
        protected void Button_look_click(object sender, EventArgs e)
        {
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            Bindlist(item_code, date_time);
        }
        //删除
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (selectRow == null || selectRow.Length < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请至少选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                XyhsDetail dal = new XyhsDetail();
                try
                {
                    dal.DelDeptCosts(date_time, item_code, selectRow);

                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("Store1.reload();");
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
                }
            }
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        //查询条件改变
        protected void Item_SelectOnChange(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            XyhsDetail dal = new XyhsDetail();
            this.AllCosts.Value = dal.GetItemAllCosts(date_time, item_code);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
        }
        //日期改变
        protected void Date_SelectOnChange(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            SetDict(date_time);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            Bindlist(item_code, date_time);

        }
        //提交保存
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            List<CostDetail> costdetail = e.Object<CostDetail>();
            XyhsDetail dal = new XyhsDetail();
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string item_name = this.COST_ITEM.SelectedItem.Text;
            User user = (User)Session["CURRENTSTAFF"];
            string operators = user.UserName;
            try
            {
                dal.SaveDeptClass1Costs(costdetail, date_time, item_code, item_name, operators);
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("Store1.reload();");
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "保存成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
            }
        }
        //刷新
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            Bindlist(item_code, date_time);
        }
        //分摊
        protected void Button_break_click(object sender, EventArgs e)
        {
            XyhsDetail dal = new XyhsDetail();
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            string costitem = this.COST_ITEM.SelectedItem.Value.ToString();
            double costvalue;
            string rtnmsg = "";
            try
            {
                costvalue = dal.GetItemAllCosts(date_time, costitem);
                try
                {
                    Cost_detail _dal = new Cost_detail();
                    _dal.Exec_Sp_Prog_Ratio(date_time);
                    rtnmsg = dal.Exec_Sp_Hos_Costs_Deal(date_time, costitem, costvalue);
                    //查询绑定临时数据
                    DataTable dt = _dal.GetSingleCostTmp().Tables[0];
                    this.Store1.DataSource = dt;
                    this.Store1.DataBind();
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "分解完成，注意保存！",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_break_click");
            }
        }
    }
}
