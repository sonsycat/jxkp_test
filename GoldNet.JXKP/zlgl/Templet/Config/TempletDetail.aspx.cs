using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using GoldNet.JXKP.Templet.BLL;

namespace GoldNet.JXKP.zlgl.Templet.Config
{
    public partial class TempletDetail :PageBase
    {
        int _templetID;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lnkbtnEdit.Click += new EventHandler(this.lnkbtnEdit_Click);
            this.lnkbtnAddNewField.Click += new EventHandler(this.lnkbtnAddNewField_Click);
            //
            // 获取模板编号
            _templetID = int.Parse(Request["templetid"].ToString());

            // 创建模板
            TempletBO templet = new TempletBO(_templetID);


            // 初始化页面
            // 基本信息区
            this.labName.Text = templet.Name;
            this.labTitle.Text = templet.Title;
            this.labCommon.Text = "<PRE>" + templet.Common + "</PRE>";

            // 字段区
            if (templet.GetFields(_templetID).Count == 0)
            {
                // 如果模板包含的字段为0,显示一个提示,并隐藏显示字段列表的表格
                this.labNoFieldInTemplet.Visible = true;
                this.Page.FindControl("tabFieldList").Visible = false;
            }
            else
            {
                // 获取显示字段的表格,并填充此表格exitdisplayfild
                HtmlTable fieldTab = (HtmlTable)this.Page.FindControl("tabFieldList");

                foreach (Field field in templet.GetFields(_templetID))
                {
                    HtmlTableCell cell1 = new HtmlTableCell();
                    HtmlTableCell cell2 = new HtmlTableCell();
                    HtmlTableCell cell3 = new HtmlTableCell();
                    HtmlTableRow row = new HtmlTableRow();
                    HyperLink link = new HyperLink();
                    link.Text = field.FieldName;
                    link.NavigateUrl = "Edit_Field.aspx?templetid="
                        + templet.ID.ToString() + "&fieldid="
                        + field.ID.ToString();
                    cell1.Controls.Add(link);
                    cell2.Controls.Add(new LiteralControl(field.FieldTypeName));
                    //					if (templet.DefaultView.DisplayFields.Contains(field))
                    //						cell3.Controls.Add(new LiteralControl("<IMG SRC='/images/check.gif' BORDER='0' ALT=''>"));
                    if (templet.exitdisplayfild(field))
                        cell3.Controls.Add(new LiteralControl("<IMG SRC='../images/check.gif' BORDER='0' ALT=''>"));

                    cell1.Align = "left";
                    cell2.Align = "center";
                    cell3.Align = "center";
                    row.Cells.Add(cell1);
                    row.Cells.Add(cell2);
                    row.Cells.Add(cell3);
                    
                    fieldTab.Rows.Add(row);
                }
            }

            // 视图区
            linkEditView.Text = templet.DefaultView.Name;
            linkEditView.NavigateUrl = "Edit_View.aspx?templetid="
                + _templetID.ToString()
                + "&viewid="
                + templet.DefaultViewID.ToString();
        }
        private void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            // 更改常规属性
            Server.Transfer("EditTemplet_Info.aspx?templetid=" + _templetID.ToString());
        }
        private void lnkbtnAddNewField_Click(object sender, EventArgs e)
        {
            // 添加新字段
            Server.Transfer("Add_NewField.aspx?templetid=" + _templetID.ToString());
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.templetinfo.hide();");
        }
    }
}
