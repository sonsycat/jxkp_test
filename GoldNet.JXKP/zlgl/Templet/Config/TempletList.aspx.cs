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
using GoldNet.JXKP.Templet.BLL;
using GoldNet.JXKP.PowerManager;

namespace GoldNet.JXKP.zlgl.Templet.Config
{
    public partial class TempletList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                GetPageData();
            }
        }
        //添加模版
        protected void Buttonadd_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("AddTemplet.aspx");
            showCenterSet(this.templetname, loadcfg);
        }
        //编辑模版
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
                LoadConfig loadcfg = getLoadConfig("TempletDetail.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("templetid", id));
                showCenterSet(this.templetinfo, loadcfg);
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
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            int templetID = int.Parse(sm.SelectedRow.RecordID);
            TempletBO templet = new TempletBO(templetID);
            dal.DeleteFunc(templet.FuncKey, templet.ID.ToString());
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("RefreshData();");
            scManager.AddScript("templetname.hide();");
        }
       //选择模版
        protected void RowSelect(object sender, AjaxEventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count == 1)
            {
                this.Buttondel.Disabled = false;
                this.Buttonedit.Disabled = false;
            }
        }
        //数据提取
        private void GetPageData()
        {
            DataTable datatable = TempletBO.GetAllTempletList();
            this.Store1.DataSource = datatable;
            this.Store1.DataBind();
        }
        //刷新
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }
    }
}
