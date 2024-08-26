using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using Goldnet.Dal;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class OtherAward_Type : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                data();
            }
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            data();
        }
        protected void data()
        {
            InputOtherAward dal = new InputOtherAward();
            DataTable table = new DataTable();
            try
            {
                table = dal.GetOtherAwardType("");
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "OtherAward_Type");
            }
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
        protected void OtherAward_add(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("OtherAward_Type_Edit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", "0"));
            showCenterSet(this.add_edit, loadcfg);
        }
        protected void OtherAward_edit(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                string id = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("OtherAward_Type_Edit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
                showCenterSet(this.add_edit, loadcfg);
            }
        }
        protected void OtherAward_del(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {

                Ext.Msg.Confirm("系统提示", "您确定要删除选中记录吗？", new MessageBox.ButtonsConfig
                {
                    Yes = new MessageBox.ButtonConfig
                    {
                        Handler = "Goldnet.del()",
                        Text = "确定"

                    },
                    No = new MessageBox.ButtonConfig
                    {
                        Text = "取消"
                    }
                }).Show();
            }
        }
        [AjaxMethod]
        public void del()
        {
            InputOtherAward dal = new InputOtherAward();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            string id = sm.SelectedRow.RecordID;
            dal.DeleteOtherAwardtype(id);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");

        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.edittype.hide();");
        }
    }
}
