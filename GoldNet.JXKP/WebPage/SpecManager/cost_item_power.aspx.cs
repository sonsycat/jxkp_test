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
    public partial class cost_item_power : PageBase
    {
        static string item_code = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                DataRow[] pertyperow = dal.GetCostItem("").Tables[0].Select();
                foreach (DataRow rw in pertyperow)
                {
                    this.ComboBox_Pertype.Items.Add(new Goldnet.Ext.Web.ListItem(rw["ITEM_NAME"].ToString(), rw["ITEM_CODE"].ToString()));
                }
                this.ComboBox_Pertype.SelectedItem.Value = Request["ITEM_CODE"].ToString();
                SetDict();
            }
        }
        /// <summary>
        /// 初始
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable roletable = dal.GetRoleListspe(this.ComboBox_Pertype.SelectedItem.Value, Constant.COST_SPEPOWER).Tables[0];
            this.Store1.DataSource = roletable;
            this.Store1.DataBind();
            this.Store2.DataSource = dal.GetSpeRole(this.ComboBox_Pertype.SelectedItem.Value, Constant.COST_SPEPOWER);
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
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            try
            {

                dal.Saverolelist(rolelist, this.ComboBox_Pertype.SelectedItem.Value, Constant.COST_SPEPOWER);
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
