using System;
using Goldnet.Ext.Web;
using System.Data;
using Goldnet.Dal.cbhs;

namespace GoldNet.JXKP.cbhs.xyhs.xyhsdict
{
    public partial class appor_prog_sqlexpress_edit : PageBase
    {
        private Xyhs_Appor_Prog_Dict dal_prog = new Xyhs_Appor_Prog_Dict();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Request["ApproProgSQLEditMode"] != null)
                {
                    string mode = Request["ApproProgSQLEditMode"].ToString();
                    if (mode == "Edit" && Request["ApporProgSQLID"] != null)
                    {
                        string id = Request["ApporProgSQLID"].ToString();
                        DataTable dt = dal_prog.GetProgSql(id);
                        if (dt.Rows.Count > 0)
                        {
                            tfPROGCODE.Text = dt.Rows[0]["PROG_ITEM"].ToString();
                            tfPROGNAME.Text = dt.Rows[0]["PROG_NAME"].ToString();
                            ccbtype.SetValue(dt.Rows[0]["APPORTION_CODE"].ToString());
                            ccbtype.SelectedItem.Value = dt.Rows[0]["APPORTION_CODE"].ToString();
                            taSQL.Text = dt.Rows[0]["PROG_SQL"].ToString();
                            taRemark.Text = dt.Rows[0]["PROG_MEMO"].ToString();
                        }
                    }
                    else if (mode == "Add")
                    {
                        tfPROGCODE.Text = "";
                        tfPROGNAME.Text = "";
                        taSQL.Text = "";
                        ccbtype.SetValue("");
                        taRemark.Text = "";
                    }
                    Store2.DataSource = dal_prog.GetApporDict();
                    Store2.DataBind();
                }
            }
        }


        protected void SaveProg_onClick(object sender, AjaxEventArgs e)
        {
            if (Request["ApproProgSQLEditMode"] != null)
            {

                string mode = Request["ApproProgSQLEditMode"].ToString();

                string progcode = tfPROGCODE.Text.Trim().Replace("'", "''");
                string progname = tfPROGNAME.Text.Trim().Replace("'", "''");
                string type = ccbtype.SelectedItem.Value;
                string sql = taSQL.Text;
                string meno = taRemark.Text.Trim().Replace("'", "''"); ;
                string msg = dal_prog.CheckGuideSql(progcode, sql, 4);
                if (msg != "")
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = msg,
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                if (mode == "Add")
                {
                    try
                    {
                        dal_prog.InsertProgSql(progcode, progname, sql.Trim().Replace("'", "''"), type, meno);
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData('成本分摊比例字典添加成功');");
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
                    string id = Request["ApporProgSQLID"].ToString();
                    try
                    {
                        dal_prog.UpateProgSql(progcode, progname, sql.Trim().Replace("'", "''"), type, meno);
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData('成本分摊比例字典修改成功');");
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
