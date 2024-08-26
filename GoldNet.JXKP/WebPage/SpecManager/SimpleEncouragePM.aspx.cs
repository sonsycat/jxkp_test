using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Text;

namespace GoldNet.JXKP.WebPage.SpecManager
{
    public partial class SimpleEncouragePM : PageBase
    {
        private SE_ROLE dal_se = new SE_ROLE();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    //               Response.End();
                }
              
                Bindlist();
            }

        }        
        //查询绑定数据
        protected void Bindlist()
        {
            DataTable dt = dal_se.GetSimpleEncourage();
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //刷新store
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }


        //刷新
        protected void Button_refresh_click(object sender, EventArgs e)
        {
            Bindlist();
        }

        //显示弹出权限窗口
        private void showPowerWin(LoadConfig loadcfg)
        {
            PowerWin.ClearContent();
            PowerWin.Show();
            PowerWin.LoadContent(loadcfg);
        }


        //设置权限
        protected void SetPower(object sender, AjaxEventArgs e)
        {
            string item_code = e.ExtraParams["Values"].ToString();
            LoadConfig loadcfg = getLoadConfig("SimpleEncourage_power.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("ID", item_code));
            showPowerWin(loadcfg);
        }
    }
}
