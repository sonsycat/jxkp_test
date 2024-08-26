using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm.ExportData;
using GoldNet.Model;
using GoldNet.JXKP.BLL.Guide;

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class Quality_Submit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                this.stardate.Value = System.DateTime.Now.ToString("yyyy-MM") + "-01";
                this.GridPanel1.ColumnModel.RegisterCommandStyleRules();
                GetPageData();
            }

        }

        protected void GetQueryPortalet(object sender, EventArgs e)
        {
            GetPageData();
        }

        private void GetPageData()
        {
            User user = (User)Session["CURRENTSTAFF"];
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            DataTable dt = dal.GetQualitySubmit(user.UserId, Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        protected void SubmitQuality(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                User user = (User)Session["CURRENTSTAFF"];
                Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
                dal.SubmitQuality(sm.SelectedRow.RecordID,Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"), user.UserName);
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("RefreshData();");
            }
        }

        //删除模版
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

        }
        [AjaxMethod]
        public void del()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            int temp_id = int.Parse(sm.SelectedRow.RecordID);

            dal.DelQualitySubmit(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"),temp_id);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");
            
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }
    }
}
