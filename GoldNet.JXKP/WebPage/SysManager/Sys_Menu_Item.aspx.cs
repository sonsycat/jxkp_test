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
using System.Collections.Generic;
using GoldNet.Comm;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Menu_Item : PageBase
    {
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
        protected void Bindlist()
        {
            SE_ROLE sd = new SE_ROLE();

            DataTable dt = sd.GetMenuItem(Request["APP_MENU_ID"].ToString(), Request["MENU_GUIDE_ID"].ToString());
            DataRow rw = dt.NewRow();
            if (dt.Rows.Count > 0)
            {
                rw["ITEM_ID"] = int.Parse(dt.Rows[dt.Rows.Count - 1]["ITEM_ID"].ToString()) + 1;
            }
            else
                rw["ITEM_ID"] = "1";
            dt.Rows.Add(rw);
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
        //保存
        protected void Button_Save_click(object sender, AjaxEventArgs e)
        {
            string appid = Request["APP_MENU_ID"].ToString();
            string menuid = Request["MENU_GUIDE_ID"].ToString();
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    SE_ROLE sd = new SE_ROLE();
                    sd.SaveMenuItem(selectRow,appid,menuid);
                    Bindlist();
                }
                catch (Exception ex)
                {
                    this.ShowMessage("系统提示", "数据填写不完整！");
                }
            }
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        
      
       
    }
}
