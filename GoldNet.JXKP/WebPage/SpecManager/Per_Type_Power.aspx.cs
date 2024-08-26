using System;
using System.Data;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.WebPage.SpecManager
{
    public partial class Per_Type_Power : PageBase
    {
        static string item_code = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                DataRow[] pertyperow = dal.GetPersType("").Select();
                foreach (DataRow rw in pertyperow)
                {
                    this.ComboBox_Pertype.Items.Add(new Goldnet.Ext.Web.ListItem(rw["PERS_SORT_NAME"].ToString(), rw["ID"].ToString()));
                }
                this.ComboBox_Pertype.SelectedItem.Value = Request["id"].ToString();
                SetDict();
            }
        }

        /// <summary>
        /// 初始
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable roletable = dal.GetRoleListspe(this.ComboBox_Pertype.SelectedItem.Value, Constant.PER_SPEPOWER).Tables[0];
            this.Store1.DataSource = roletable;
            this.Store1.DataBind();
            this.Store2.DataSource = dal.GetSpeRole(this.ComboBox_Pertype.SelectedItem.Value, Constant.PER_SPEPOWER);
            this.Store2.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedPertype(object sender, AjaxEventArgs e)
        {
            SetDict();

        }

        /// <summary>
        /// 添加权限人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            List<PageModels.roleselected> rolelist = e.Object<PageModels.roleselected>();
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            try
            {

                dal.Saverolelist(rolelist, this.ComboBox_Pertype.SelectedItem.Value, Constant.PER_SPEPOWER);
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.PersTypeEdit.hide();");
                scManager.AddScript("parent.RefreshData();");
            }
            catch (Exception ex)
            {
                ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.PersTypeEdit.hide();");
        }
    }
}
