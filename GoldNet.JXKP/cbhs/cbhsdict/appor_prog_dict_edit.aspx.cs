using System;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;

namespace GoldNet.JXKP
{
    public partial class appor_prog_dict_edit : PageBase
    {
        private Appor_Prog_Dict dal_prog = new Appor_Prog_Dict();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Request["ApproProgEditMode"] != null)
                {
                    string mode = Request["ApproProgEditMode"].ToString();
                    if (mode == "Edit" && Request["ApporProgID"] != null)
                    {
                        string id = Request["ApporProgID"].ToString();
                        DataTable dt = dal_prog.GetProgDict(id);
                        if (dt.Rows.Count > 0)
                        {
                            tfPROGCODE.Text = dt.Rows[0]["PROG_CODE"].ToString();
                            tfPROGNAME.Text = dt.Rows[0]["PROG_NAME"].ToString();
                            tfINPUTCODE.Text = dt.Rows[0]["INPUT_CODE"].ToString();
                            ccbtype.SetValue(dt.Rows[0]["FLAGS"].ToString());
                            ccbtype.SelectedItem.Value = dt.Rows[0]["FLAGS"].ToString();

                        }
                    }
                    else if (mode == "Add")
                    {
                        tfPROGCODE.Text = "";
                        tfPROGNAME.Text = "";
                        tfINPUTCODE.Text = "";
                        ccbtype.SetValue("");
                    }
                    Store2.DataSource = dal_prog.GetApporDict();
                    Store2.DataBind();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveProg_onClick(object sender, AjaxEventArgs e)
        {
            if (Request["ApproProgEditMode"] != null)
            {
                string mode = Request["ApproProgEditMode"].ToString();
                string progcode = tfPROGCODE.Text.Trim().Replace("'", "''");
                string progname = tfPROGNAME.Text.Trim().Replace("'", "''");
                string inputcode = tfINPUTCODE.Text.Trim().Replace("'", "''");
                string type = ccbtype.SelectedItem.Value;
                if (mode == "Add")
                {
                    try
                    {
                        dal_prog.InsertProgDict(progcode, progname, inputcode, type);
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData('成本分摊方案添加成功');");
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
                    string id = Request["ApporProgID"].ToString();
                    try
                    {
                        dal_prog.UpateProgDict(id, progcode, progname, inputcode, type);
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData('成本分摊方案修改成功');");
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
