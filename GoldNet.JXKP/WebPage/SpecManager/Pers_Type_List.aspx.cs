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

namespace GoldNet.JXKP.WebPage.SpecManager
{
    public partial class Pers_Type_List : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                GetPageData();
                string deptcode = this.DeptFilter("dept_code");
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
            GetPageData();
        }
        private void GetPageData()
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table = dal.GetPersType("");
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
        protected void SetPower(object sender, AjaxEventArgs e)
        {
            string id = e.ExtraParams["Values"].ToString();
           
            LoadConfig loadcfg = getLoadConfig("Per_Type_Power.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
            showCenterSet(this.PersTypeEdit, loadcfg);
        }
    }
}
