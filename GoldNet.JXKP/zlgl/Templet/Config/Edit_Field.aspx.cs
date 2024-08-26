using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GoldNet.JXKP.Templet.BLL;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.zlgl.Templet.Config
{
    public partial class Edit_Field : PageBase
    {
        int _templetID, _fieldID;
        TempletBO _templet;
        GoldNet.JXKP.Templet.BLL.Field _field;
        protected void Page_Load(object sender, EventArgs e)
        {
            // 获取模板和字段编号
            _templetID = int.Parse(Request["templetid"].ToString());
            _fieldID = int.Parse(Request["fieldid"].ToString());

            _field = new GoldNet.JXKP.Templet.BLL.Field(_fieldID);
            _templet = new TempletBO(_templetID);

            if (!this.IsPostBack)
            {
                // 初始化页面
                textFieldName.Text = _field.FieldName;
                labFieldType.Text = _field.FieldTypeName;
                //chkboxAddToDefaultView.Checked = _templet.DefaultView.DisplayFields.Contains(_field);
                chkboxAddToDefaultView.Checked = _templet.exitdisplayfild(_field);
            }

            displaySpecialProperty(_field);
        }
        private void displaySpecialProperty(GoldNet.JXKP.Templet.BLL.Field field)
        {
            // 调用字段类型对象的显示可选属性页面方法
            Control ctrl = this.FindControl("tdSpecialProperty");
            ctrl.Controls.Clear();

            field.ShowSpecialProperty(ctrl);
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            // 取消
            Response.Redirect("TempletDetail.aspx?templetid=" + _templetID.ToString());
        }

        protected void btnSave_Click(object sender, AjaxEventArgs e)
        {
            // 保存
            if (textFieldName.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "字段名称不能为空！");
            }
            else
            {
                try
                {
                    _templet.UpdateField(_fieldID, textFieldName.Text, this.Page, chkboxAddToDefaultView.Checked);
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
