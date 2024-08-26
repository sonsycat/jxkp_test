using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;

using GoldNet.JXKP.Templet.BLL;

namespace GoldNet.JXKP.zlgl.Templet.Config
{
    public partial class AddTemplet : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                
            }
        }

        protected void save_Click(object sender, AjaxEventArgs e)
        {
            string templetName, templetTitle, templetCommon;
            int showorder=0;
            if (textTempletName.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","模版名称不能为空！");
            }
            else if (textTempletTitle.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "模版标题不能为空！");
            }
            else
            {
                templetName = CleanString.InputText(textTempletName.Text, 25);
                templetTitle = CleanString.InputText(textTempletTitle.Text, 25);
                templetCommon = CleanString.InputText(textTempletCommon.Text, 100);

                try
                {
                    showorder = int.Parse(this.showorder.Text);
                }
                catch
                {
                    this.ShowMessage("系统提示", "显示顺序请输入数字！");
                }

                try
                {
                    TempletBO.AddNewTemplet(templetName, templetTitle, templetCommon, showorder,this.iscount.SelectedItem.Value);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData();");
                    scManager.AddScript("parent.templetname.hide();");
                }

                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "save_Click");

                }
            }
           
            
        }
        
    }
}
