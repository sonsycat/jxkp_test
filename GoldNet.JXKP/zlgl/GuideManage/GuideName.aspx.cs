using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GoldNet.JXKP.PowerManager;
using GoldNet.Comm;
using GoldNet.JXKP.Templet.BLL;
using Goldnet.Ext.Web;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class GuideName : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                GetPageData();
            }
        }
        //数据提取
        private void GetPageData()
        {
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            //填充公共部分指标名称信息
            DataTable datatable = dal.CommonGuide_View().Tables[0];
            this.Store1.DataSource = datatable;
            this.Store1.DataBind();
            //填充专业部分指标名称信息
            DataTable spdatatable = dal.SpecialtyGuide_Viewd().Tables[0];
            this.Store2.DataSource = spdatatable;
            this.Store2.DataBind();
        }

        protected void BtnDelComm_Click(object sender, EventArgs e)
        {
            //删除公共部分指标名称数据
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
                        Handler = "Goldnet.commguidedel()",
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
        public void commguidedel()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            string id = sm.SelectedRow.RecordID;
            dal.CommonGuide_Del(int.Parse(id));
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");
        }

        protected void BtnDelSpec_Click(object sender,EventArgs e)
        {
            //删除专业部分指标名称数据
            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
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
                        Handler = "Goldnet.speguidedel()",
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
        public void speguidedel()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            string id = sm.SelectedRow.RecordID;
            dal.SpecialtyGuide_Del(int.Parse(id));
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");
        }
        protected void BtnEditComm_Click(object sender, EventArgs e)
        {
            //修改公共部分指标名称数据
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                string id = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("GuideNameEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("straction", "edit"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("strsigntype", "0"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
                showCenterSet(this.guideinfo, loadcfg);
            }
        }

        protected void BtnEditSpec_Click(object sender, EventArgs e)
        {
            //修改专业部分指标名称数据
            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                string id = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("SpecGuideNameEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("straction", "edit"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("strsigntype", "1"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
                showCenterSet(this.guideinfo, loadcfg);
            }

        }

        protected void BtnAddComm_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("GuideNameEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("straction", "add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("strsigntype", "0"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", "0"));
            showCenterSet(this.guideinfo, loadcfg);
        }

        protected void BtnAddSpec_Click(object sender,EventArgs e)
        {
            //Response.Redirect("SpecGuideNameEdit.aspx?straction=add&strsigntype=1&id=0");

            LoadConfig loadcfg = getLoadConfig("SpecGuideNameEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("straction", "add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("strsigntype", "1"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", "0"));
            showCenterSet(this.guideinfo, loadcfg);
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("GuideViewDetail.aspx");
        }
    }
}
