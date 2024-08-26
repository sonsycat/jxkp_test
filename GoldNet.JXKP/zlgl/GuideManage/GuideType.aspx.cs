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
    public partial class GuideType : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                GetPageData();
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonadd_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("GuideTypeEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", "0"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("straction", "add"));
            showCenterSet(this.guidetypeedit, loadcfg);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                LoadConfig loadcfg = getLoadConfig("GuideTypeEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("straction", "edit"));
                showCenterSet(this.guidetypeedit, loadcfg);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttondel_Click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.SelectRecord();
            }
            else
            {
                if (sm.SelectedRow.RecordID == Constant.SPEGUIDETYPE)
                {
                    this.ShowMessage("系统提示","专业质量分类不能删除！");
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
        }
        [AjaxMethod]
        public void del()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            string id = sm.SelectedRow.RecordID;
            dal.GuideType_Del(int.Parse(id));
            GetPageData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            DataTable table = dal.GetGuideType().Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("GuideViewDetail.aspx");
        }
    }
}
