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
using GoldNet.Model;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class RoleEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                if (Request["roleid"].ToString() != "")
                {
                    GoldNet.Model.SYS_ROLE_DICT model = new GoldNet.Model.SYS_ROLE_DICT(int.Parse(Request["roleid"].ToString()));
                    Edit(model);
                    
                }
                
            }

        }
        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            if (this.Combo_RoleType.SelectedItem.Value == string.Empty)
            {
                this.ShowMessage("系统提示", "角色类别不能为空！");
            }
            else if (this.Text_RoleName.Value.ToString().Trim() == string.Empty)
            {
                this.ShowMessage("系统提示", "角色名称不能为空！");
            }
            else
            {
                GoldNet.Model.SYS_ROLE_DICT model = new GoldNet.Model.SYS_ROLE_DICT();
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                model.REMARK = this.Text_Remark.Text;
                model.ROLE_NAME = this.Text_RoleName.Text;
                model.ROLE_TYPE = this.Combo_RoleType.SelectedItem.Value;
                model.ROLE_ID = 0;
                model.ROLE_APP = this.Combo_App.Visible == true ? this.Combo_App.SelectedItem.Value : "-1";

                if (Request["roleid"].ToString() != "")
                {
                    model.ROLE_ID = int.Parse(Session["roleid"].ToString());
                }
                try
                {
                     User user = (User)Session["CURRENTSTAFF"];
                    dal.AddRole(model,user.UserId);
                    Session["roleid"] = null;
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RoleEdit.hide();");
                    scManager.AddScript("parent.RefreshData();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

                }
            }

        }
        /// <summary>
        ///设置下拉框
        /// </summary>
        public void SetDict()
        {
            this.Combo_App.Visible = false;
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table = dal.GetRoleType().Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.Combo_RoleType.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["role_type"].ToString(), table.Rows[i]["id"].ToString()));
                }

            }
            User user = (User)Session["CURRENTSTAFF"];
            DataTable apptable = user.GetUserApp();

            if (apptable.Rows.Count > 0)
            {
                Combo_App.Visible = true;
                for (int i = 0; i < apptable.Rows.Count; i++)
                {
                    this.Combo_App.Items.Add(new Goldnet.Ext.Web.ListItem(apptable.Rows[i]["app_name"].ToString(), apptable.Rows[i]["app_id"].ToString()));
                }

            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void Edit(GoldNet.Model.SYS_ROLE_DICT model)
        {
            this.Combo_App.SelectedItem.Value = model.ROLE_APP;
            this.Combo_RoleType.SelectedItem.Value = model.ROLE_TYPE;
            this.Combo_RoleType.SelectedItem.Text = model.ROLE_NAME;
            this.Text_RoleName.Value = model.ROLE_NAME;
            this.Text_Remark.Value = model.REMARK;
            Session["roleid"] = model.ROLE_ID.ToString();

        }
        
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.RoleEdit.hide();");
            
        }
    }
}