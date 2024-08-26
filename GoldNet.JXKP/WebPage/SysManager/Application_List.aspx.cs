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
    public partial class Application_List : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                GetAppList();
               
            }
        }
        protected void GetQueryFunc(object sender, AjaxEventArgs e)
        {

            GetAppList();
        }
        /// <summary>
        /// 项目设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonset_Click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string roleid = sm.SelectedRow.RecordID;
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                DataTable table = dal.GetApplicationList(roleid).Tables[0];
                if (table.Rows.Count > 0)
                {
                    LoadConfig loadcfg = getLoadConfig("FuncSet.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("app_id", table.Rows[0]["app_id"].ToString()));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("power_type", table.Rows[0]["power_type"].ToString() == "否" ? "0" : "1"));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("role_id", table.Rows[0]["role_id"].ToString()));
                    showCenterSet(this.Func_Set, loadcfg);
                }
            }

        }
       /// <summary>
       /// 双击设置
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string roleid = sm.SelectedRow.RecordID;
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                DataTable table = dal.GetApplicationList(roleid).Tables[0];
                if (table.Rows.Count > 0)
                {
                   
                    LoadConfig loadcfg = getLoadConfig("FuncSet.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("app_id", table.Rows[0]["app_id"].ToString()));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("power_type", table.Rows[0]["power_type"].ToString() == "否" ? "0" : "1"));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("role_id", table.Rows[0]["role_id"].ToString()));
                    showCenterSet(this.Func_Set, loadcfg);
                }
            }
        }
        protected void RowSelect(object sender, AjaxEventArgs e)
        {
            string roleid = e.ExtraParams["ROLE_ID"];

            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;

            string str = sm.SelectedRow.RecordID;

        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetAppList();
        }
        /// <summary>
        /// 项目列表
        /// </summary>
        public void GetAppList()
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table = dal.GetApplicationList("").Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();  
        }
    }
}
