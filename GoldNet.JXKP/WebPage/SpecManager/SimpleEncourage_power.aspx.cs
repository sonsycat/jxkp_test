using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.WebPage.SpecManager
{
    public partial class SimpleEncourage_power : PageBase
    {
        static string item_code = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SE_ROLE dal_se = new SE_ROLE();
                DataTable pertyperow = dal_se.GetSimpleEncourage();
                foreach (DataRow rw in pertyperow.Rows)
                {
                    this.ComboBox_Pertype.Items.Add(new Goldnet.Ext.Web.ListItem(rw["ITEMNAME"].ToString(), rw["ID"].ToString()));
                }
                this.ComboBox_Pertype.SelectedItem.Value = Request["ID"].ToString();
                SetDict();
            }
        }
        /// <summary>
        /// 初始
        /// </summary>
        public void SetDict()
        {
            SE_ROLE dal_se = new SE_ROLE();
            DataTable roletable = dal_se.GetRoleListspe(this.ComboBox_Pertype.SelectedItem.Value).Tables[0];
            this.Store1.DataSource = roletable;
            this.Store1.DataBind();
            this.Store2.DataSource = dal_se.GetSpeRole(this.ComboBox_Pertype.SelectedItem.Value);
            this.Store2.DataBind();
        }
        protected void SelectedPertype(object sender, AjaxEventArgs e)
        {
            SetDict();

        }

        //添加权限人员
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            List<PageModels.roleselected> rolelist = e.Object<PageModels.roleselected>();
            SE_ROLE dal_se = new SE_ROLE();
            try
            {

                dal_se.Saverolelist(rolelist, this.ComboBox_Pertype.SelectedItem.Value);
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.PowerWin.hide();");
                scManager.AddScript("parent.RefreshData();");
            }
            catch (Exception ex)
            {
                ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");
            }
        }


        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.PowerWin.hide();");
        }
    }
}
