using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;

namespace GoldNet.JXKP
{
    public partial class Other_dept_cbhs_sqlexpress_edit : PageBase
    {
        private Appor_Prog_Dict dal_prog = new Appor_Prog_Dict();
        protected void Page_Load(object sender, EventArgs e)
        {
           if (!Ext.IsAjaxRequest)
            {
                if (Request["Mode"] != null)
                {
                    string mode = Request["Mode"].ToString();
                    if (mode == "Edit" && Request["ID"] != null)
                    {
                        string id = Request["ID"].ToString();
                        DataTable dt = dal_prog.GetOthersqlexpress(id);
                        if (dt.Rows.Count > 0)
                        {
                            this.SQL_NAME.Text = dt.Rows[0]["SQL_NAME"].ToString();
                            this.flags.SelectedItem.Value = dt.Rows[0]["FLAGS"].ToString();
                            this.SQLEXPRESS.Text = dt.Rows[0]["SQLEXPRESS"].ToString();

                            this.MEMO.Text = dt.Rows[0]["MEMO"].ToString();
                           
                        }
                    }
                    else if (mode == "Add")
                    {
                        SQL_NAME.Text = "";
                        SQLEXPRESS.Text = "";
                        MEMO.Text = "";
                        flags.SelectedIndex = 0;
                    }
                    
                }
            }
            
        }
        protected void SaveExpress_onClick(object sender, AjaxEventArgs e)
        {
            if (Request["Mode"] != null)
            {

                string mode = Request["Mode"].ToString();

                string sqlname = this.SQL_NAME.Text.Trim();
                string sqlexpress = this.SQLEXPRESS.Text.Trim();
                string memo = this.MEMO.Text;
                string flags = this.flags.SelectedItem.Value;

                string msg = dal_prog.CheckSqlexpress(sqlexpress);
                if (msg != "")
                {
                    this.ShowMessage("系统提示",msg);
                    return;
                }
                if (mode == "Add")
                {
                    try
                    {
                        dal_prog.InsertOthercbhssql(sqlname,sqlexpress,flags,memo);
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
                else if (mode == "Edit")
                {
                    string id = Request["id"].ToString();
                    try
                    {
                        dal_prog.UpdateOthercbhssql(id, sqlname, sqlexpress, flags, memo);
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
            }
        }
    }
}
