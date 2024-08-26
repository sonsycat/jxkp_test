using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Text;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_cost_item_set : PageBase
    {
        private Cost_Item_Type dal_type = new Cost_Item_Type();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    //               Response.End();
                }
                Bindlist();
            }

        }
        
        //查询绑定数据
        protected void Bindlist()
        {
            this.GridPanel1.Dispose();
            this.Store1.Dispose();
            XyhsDetail dal = new XyhsDetail();
            DataTable dt = dal.GetXyhsCostItem().Tables[0];
           
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //刷新store
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }
        
        //修改
        protected void Button_edit_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Ext.Msg.Alert("提示", "请选择一条记录！").Show();
            }
            else
            {
                string item_code = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("xyhs_item_edit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", item_code));
                this.showCenterSet(this.TypeWin, loadcfg);
            }
        }

        //添加
        protected void Button_add_click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("xyhs_item_edit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", "add"));
            this.showCenterSet(this.TypeWin, loadcfg);
        }

        protected void Button_allcostbset_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Ext.Msg.Alert("提示", "请选择一条记录！").Show();
            }
            else
            {
                string item_code = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("allcost_set.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("item_code", item_code));
                this.showCenterSet(this.AllcostWin, loadcfg);
            }
        }
       
        //刷新
        protected void Button_refresh_click(object sender, EventArgs e)
        {
            Bindlist();
        }
       
       
    }
}
