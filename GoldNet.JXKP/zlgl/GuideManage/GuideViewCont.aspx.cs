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

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class GuideViewCont : PageBase
    {
        public int intID;
        public string strGuideName;
        public int GuideTypeID;
        protected void Page_Load(object sender, EventArgs e)
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            intID = Convert.ToInt32(Request.QueryString["ID"].ToString());
            GuideTypeID = Convert.ToInt32(Request.QueryString["GuideTypeID"].ToString());
            this.guidename.Text = dal.GuideNameByguide(intID.ToString(), GuideTypeID.ToString());
            if (GuideTypeID == 2)
            {
                this.Contentype.Visible = true;
            }

            if (!Ext.IsAjaxRequest)
            {
                GetPageData();
            }
        }
        protected void Buttonadd_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("GuideInput.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("GuideTypeID", GuideTypeID.ToString()));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("GuideNameID", intID.ToString()));
            showCenterSet(this.guidecontedit, loadcfg);
        }
        protected void Buttoneidttype_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("ContenList.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("GuideTypeID", intID.ToString()));
            showCenterSet(this.guideconttype, loadcfg);
        }
        protected void Buttonedit_Click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                
                string id = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("GuideEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("GuideNameID", intID.ToString()));
               // loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("GuideName", strGuideName));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("GuideTypeID", GuideTypeID.ToString()));
                showCenterSet(this.guidecontedit, loadcfg);
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
        }
        [AjaxMethod]
        public void del()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            string id = sm.SelectedRow.RecordID;
            dal.GuideCheckContent_Del(int.Parse(id));
            GetPageData();
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
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            DataTable table = dal.Guide_Cont(intID, GuideTypeID).Tables[0];

            int n = table.Rows.Count;
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("GuideViewDetail.aspx");
        }
    }
}
