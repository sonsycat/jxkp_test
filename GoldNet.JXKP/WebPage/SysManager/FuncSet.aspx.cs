using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Collections.Generic;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class FuncSet : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                Edit(Request["app_id"].ToString(), Request["power_type"].ToString(), Request["role_id"].ToString());
            }
        }
        protected void SelectedFuncType(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table = dal.GetApplicationList(this.Combo_Functype.SelectedItem.Value).Tables[0];
            if (table.Rows.Count > 0)
            {
                this.Edit(table.Rows[0]["app_id"].ToString(), table.Rows[0]["power_type"].ToString(), table.Rows[0]["role_id"].ToString());
                this.Combo_Powertype.SelectedItem.Text = table.Rows[0]["power_type"].ToString();
                this.Combo_Role.SelectedItem.Value = table.Rows[0]["role_id"].ToString();
            }
        }
        /// <summary>
        ///设置下拉框
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table = dal.GetApplicationList("").Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.Combo_Functype.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["app_name"].ToString(), table.Rows[i]["app_id"].ToString()));
                }

            }
            DataTable roletable = dal.GetList("").Tables[0];
            if (roletable.Rows.Count > 0)
            {
                for (int i = 0; i < roletable.Rows.Count; i++)
                {
                    this.Combo_Role.Items.Add(new Goldnet.Ext.Web.ListItem(roletable.Rows[i]["role_name"].ToString(), roletable.Rows[i]["role_id"].ToString()));
                }
            }
            this.Combo_Powertype.Items.Add(new Goldnet.Ext.Web.ListItem("否", "0"));
            this.Combo_Powertype.Items.Add(new Goldnet.Ext.Web.ListItem("是", "1"));
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void Edit(string appid, string powertype, string roleid)
        {
            this.Combo_Functype.SelectedItem.Value = appid;
            this.Combo_Powertype.SelectedItem.Value = powertype;
            this.Combo_Role.SelectedItem.Value = roleid;
        }
        
        /// <summary>
        ///  保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            if (this.Combo_Functype.SelectedItem.Value == string.Empty)
            {
                this.ShowMessage("提示", "必填项不能为空");
            }
            else
            {
                if (this.Combo_Powertype.SelectedItem.Value == "0")
                    this.Combo_Role.SelectedItem.Value = string.Empty;
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                try
                {
                    dal.UpdateApplication(this.Combo_Functype.SelectedItem.Value, this.Combo_Powertype.SelectedItem.Value, this.Combo_Role.SelectedItem.Value);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.Func_Set.hide();");
                    
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

                }

            }

        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Func_Set.hide();");
        }
    }
}