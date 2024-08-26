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
using GoldNet.JXKP.Templet.BLL;
using Goldnet.Ext.Web;
using GoldNet.Comm;
namespace GoldNet.JXKP.zlgl.Templet.Config
{
    public partial class Add_NewField : PageBase
    {
        int _templetID;
        protected void Page_Load(object sender, EventArgs e)
        {
            // 获取模板编号
            _templetID = int.Parse(Request["templetid"].ToString());
            this.radiobtnlistFieldTypes.SelectedIndexChanged+=new EventHandler(radiobtnlistFieldTypes_SelectedIndexChanged);

            // 初始化页面
            if (!this.IsPostBack)
            {
                // 获取并绑定显示字段类型的 RadioButtonList
                radiobtnlistFieldTypes.DataSource = FieldTypeFactory.GetAllFieldTypeInfo();
                radiobtnlistFieldTypes.DataTextField = "Name";
                radiobtnlistFieldTypes.DataValueField = "ID";
                radiobtnlistFieldTypes.SelectedIndex = 0;

                radiobtnlistFieldTypes.DataBind();
            }
            // 显示特殊属性页面
            displaySpecialProperty();
        }
        private void displaySpecialProperty()
        {
            // 调用字段类型对象的显示可选属性页面方法
            Control ctrl = this.FindControl("tdSpecialProperty");
            ctrl.Controls.Clear();

            // IFieldType fieldTypeObj = FieldTypeObjsPool.GetPool().GetFieldobjByID(Convert.ToInt32(radiobtnlistFieldTypes.SelectedValue));
            IFieldType fieldTypeObj = FieldTypeFactory.CreateFieldTypeObj(Convert.ToInt32(radiobtnlistFieldTypes.SelectedValue), null);

            fieldTypeObj.ShowSpecialProperty(ctrl);
        }
        private void radiobtnlistFieldTypes_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // 变更字段类型选择；重新显示特殊属性页面。
            displaySpecialProperty();
        }

        protected void btnSave_Click(object sender, AjaxEventArgs e)
        {
            // 保存字段
            if (textFieldName.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "字段名称不能为空！");
            }
            else
            {
                string fieldname = CleanString.InputText(textFieldName.Text, 25);
                TempletBO templet = new TempletBO(_templetID);
                try
                {
                    templet.AddNewField(fieldname, Convert.ToInt32(radiobtnlistFieldTypes.SelectedValue), this.Page, chkboxAddToDefaultView.Checked);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "btnSave_Click");
                }

                Response.Redirect("TempletDetail.aspx?templetid=" + _templetID.ToString());
            }
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            // 取消
            Response.Redirect("TempletDetail.aspx?templetid=" + _templetID.ToString());
        }
    }
}
