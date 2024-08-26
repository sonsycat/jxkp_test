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

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Persons_Change_List : PageBase
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
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = new DataTable();
            try
            {
                table = dal.GetPersonsChange(Request["userid"].ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Persons_Change_List");
            }
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
        protected void persons_add(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("Persons_change_edit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("userid", Request["userid"].ToString()));
            showCenterSet(this.add_persons, loadcfg);
        }
        protected void persons_edit(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                string id = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("Persons_change_edit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
                showCenterSet(this.add_persons, loadcfg);
            }
        }
        protected void persons_del(object sender, EventArgs e)
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
            Goldnet.Dal.SYS_DEPT_DICT bll = new Goldnet.Dal.SYS_DEPT_DICT();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            string id = sm.SelectedRow.RecordID;
            bll.DelPersonsChange(int.Parse(id));
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");

        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.persons_change.hide();");
        }
    }
}
