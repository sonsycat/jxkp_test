using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Menu_Edit : PageBase
    {
        private SE_ROLE dal_prog = new SE_ROLE();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Request["EditMode"] != null)
                {
                    DataTable appdb = dal_prog.GetFiledtype();
                    Store1.DataSource = appdb;
                    Store1.DataBind();
                    DataTable Adb = dal_prog.GetFiledaccount();
                    Store2.DataSource = Adb;
                    Store2.DataBind();
                    DataTable attrdb = dal_prog.GetFiledattr();
                    Store3.DataSource = attrdb;
                    Store3.DataBind();
                    string mode = Request["EditMode"].ToString();
                    if (mode == "Edit")
                    {
                        string appid = Request["APP_MENU_ID"].ToString();
                        string menuid = Request["MENU_GUIDE_ID"].ToString();
                        DataTable dt = dal_prog.GetSysMenu(appid,menuid);
                        if (dt.Rows.Count > 0)
                        {
                            MENU_GUIDE_NAME.Text = dt.Rows[0]["MENU_GUIDE_NAME"].ToString();
                            this.apptype.SelectedItem.Value = dt.Rows[0]["FIELD_TYPE"].ToString();
                            this.accounttype.SelectedItem.Value = dt.Rows[0]["ACCOUNT_FLAGS"].ToString();
                            this.menuattr.SelectedItem.Value = dt.Rows[0]["MENU_ATTR"].ToString();
                            this.SHOW_WIDTH.Text = dt.Rows[0]["SHOW_WIDTH"].ToString();
                            this.guidecode.Text = dt.Rows[0]["LINK_GUIDE_CODE"].ToString();
                        }
                    }
                    else if (mode == "Add")
                    {
                        this.apptype.SelectedIndex = 0;
                        this.accounttype.SelectedIndex = 0;
                        this.menuattr.SelectedIndex = 0;
                        MENU_GUIDE_NAME.Text = "";
                        SHOW_WIDTH.Text = "100";
                    }
                    
                }
            }
        }
        protected void SaveProg_onClick(object sender, AjaxEventArgs e)
        {
            if (Request["EditMode"] != null)
            {

                string mode = Request["EditMode"].ToString();

                string menuname = MENU_GUIDE_NAME.Text.Trim().Replace("'", "''");
                DataTable db = dal_prog.GetSystemMenuByName(Request["APP_MENU_ID"].ToString(), menuname);

                if (mode == "Add")
                {
                    if (db.Rows.Count > 0)
                    {
                        this.ShowMessage("提示", "这个字段名称已经存在！");
                    }
                    else
                    {
                        try
                        {
                            string widths = this.SHOW_WIDTH.Value.ToString();
                            string apptypes = this.apptype.SelectedItem.Value;
                            string accounttypes = this.accounttype.SelectedItem.Value;
                            string menuattr = this.menuattr.SelectedItem.Value;
                            string guidecode = this.guidecode.Text.Trim();
                            dal_prog.SaveSysMenu(Request["APP_MENU_ID"].ToString(), menuname, widths, apptypes, accounttypes, menuattr, guidecode);
                            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                            scManager.AddScript("parent.RefreshData('添加成功');");
                            scManager.AddScript("parent.DetailWin.hide();");
                            scManager.AddScript("parent.DetailWin.clearContent();");
                        }
                        catch (Exception ex)
                        {
                            ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveProgInsert");
                        }
                    }
                }
                else if (mode == "Edit")
                {
                    string menuid = Request["MENU_GUIDE_ID"].ToString();
                    bool flags = false;
                    if (db.Rows.Count > 0)
                    {
                        if (db.Rows[0]["MENU_GUIDE_ID"].ToString() == menuid)
                        {
                            flags = true;
                        }
                    }
                    else
                    {
                        flags = true;
                    }
                    if (flags)
                    {
                        try
                        {
                            string appid = Request["APP_MENU_ID"].ToString();
                            string widths = this.SHOW_WIDTH.Value.ToString();
                            string apptypes = this.apptype.SelectedItem.Value;
                            string accounttypes = this.accounttype.SelectedItem.Value;
                            string menuattr = this.menuattr.SelectedItem.Value;
                            string guidecode = this.guidecode.Text.Trim();
                            dal_prog.UpdateSysMenu(menuid, menuname, widths, apptypes, accounttypes, menuattr, appid,guidecode);
                            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                            scManager.AddScript("parent.RefreshData('修改成功');");
                            scManager.AddScript("parent.DetailWin.hide();");
                            scManager.AddScript("parent.DetailWin.clearContent();");
                        }
                        catch (Exception ex)
                        {
                            ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveProgEidt");
                        }
                    }
                    else
                        this.ShowMessage("提示", "这个字段名称已经存在！");

                }
            }
        }
    }
}
