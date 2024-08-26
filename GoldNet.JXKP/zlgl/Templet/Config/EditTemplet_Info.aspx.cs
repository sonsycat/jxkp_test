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
    public partial class EditTemplet_Info :PageBase
    {
        int _templetID;
        protected void Page_Load(object sender, EventArgs e)
        {
            _templetID = int.Parse(Request["templetid"].ToString());

            if (!this.IsPostBack)
            {
                // 创建模板
                TempletBO templet = new TempletBO(_templetID);

                // 初始化页面
                textTempletName.Text = templet.Name;
                textTempletTitle.Text = templet.Title;
                textTempletCommon.Text = templet.Common;
                showorder.Text = templet.ShowOrder.ToString();
                iscount.SelectedItem.Value = templet.Mark;
            }
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            // 取消
            Response.Redirect("TempletDetail.aspx?templetid=" + _templetID.ToString());
        }

        protected void btnSave_Click(object sender, AjaxEventArgs e)
        {
            // 确定
            string templetName, templetTitle, templetCommon;
            int shower=0;
            if (textTempletName.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "模版名称不能为空！");
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
                    shower = int.Parse(this.showorder.Text);
                }
                catch
                {
                    this.ShowMessage("系统提示", "显示顺序请输入数字！");
                }

                try
                {
                    TempletBO.UpdateTempletInfo(_templetID, templetName, templetTitle, templetCommon, shower,iscount.SelectedItem.Value);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "btnSave_Click");
                }

                Response.Redirect("TempletDetail.aspx?templetid=" + _templetID.ToString());
            }
        }
    }
}
