using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_incomes_dict : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Bindlist();
            }
        }
        //刷新store
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }
        //查询、绑定数据
        public void Bindlist()
        {
            this.GridPanel1.Dispose();
            this.Store1.Dispose();
            XyhsDetail sd = new XyhsDetail();
            DataTable dt = sd.GetxyhsincomesList("").Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //刷新
        protected void Button_refresh_click(object sender, EventArgs e)
        {
            Bindlist();
        }
        //设置
        protected void Button_set_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "请选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                string itemcode = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("xyhs_incomes_dict_edit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", itemcode));
                this.showCenterSet(this.DetailWin,loadcfg);
                
            }
        }
     
       
    }
}
