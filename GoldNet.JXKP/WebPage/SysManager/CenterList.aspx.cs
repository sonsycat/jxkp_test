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
    public partial class CenterList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                GetPageData();   
                string deptcode = this.DeptFilter("dept_code");
                DataTable table = this.PersTypeFilter();

                //
                User user = (User)Session["CURRENTSTAFF"];
                string aa = this.GetUserOrg();
                string bb = user.GetStationCode(user.StaffId,"2010");
            }
        }
        protected void Buttonadd_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("CenterEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("centerid", ""));
            showCenterSet(this.Center_Edit,loadcfg);
        }
        protected void Buttonedit_Click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string id = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("CenterEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("centerid", id));
                showCenterSet(this.Center_Edit,loadcfg);
            }


        }
        protected void Buttondel_Click(object sender, EventArgs e)
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
            //
            //string str1 = Request["pageid"].ToString();
            //Goldnet.Dal.SYS_DEPT_DICT bll = new Goldnet.Dal.SYS_DEPT_DICT();
            //RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            //if (sm.SelectedRows.Count < 1)
            //{
            //    this.ShowMessage("提示", "请选择一条记录！");
            //}
            //else
            //{
            //    string id = sm.SelectedRow.RecordID;
            //    bll.DelCenter(int.Parse(id));
            //    this.ShowMessage("提示", "删除成功！");
            //}
            //GetQueryPortalet(null, null);


        }
        [AjaxMethod]
        public void del()
        {
            Goldnet.Dal.SYS_DEPT_DICT bll = new Goldnet.Dal.SYS_DEPT_DICT();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            int id = int.Parse(sm.SelectedRow.RecordID);
            bll.DelCenter(id);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");

        }
        protected void RowSelect(object sender, AjaxEventArgs e)
        {
            string roleid = e.ExtraParams["ROLE_ID"];

            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;

            string str = sm.SelectedRow.RecordID;
        }
        protected void Buttonset_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("CenterDeptSet.aspx");
            showCenterSet(this.CenterDeptSet, loadcfg);

        }

        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            GetPageData();
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }
        private void GetPageData()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = dal.GetCenterList(0).Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
    }
}
