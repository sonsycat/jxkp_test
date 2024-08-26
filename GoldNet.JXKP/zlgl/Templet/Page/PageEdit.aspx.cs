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
using GoldNet.Comm;
using GoldNet.JXKP.Templet.BLL;
using Goldnet.Ext.Web;
using GoldNet.Model;
using GoldNet.JXKP.PowerManager;

namespace GoldNet.JXKP.zlgl.Templet.Page
{
    public partial class PageEdit : PageBase
    {
        private int _templetID, _recID;
        private TempletBO _templet;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Ext.IsAjaxRequest)

            _templetID = int.Parse(Request["templetid"].ToString());
            _recID = int.Parse(Request["recid"].ToString());
            _templet = new TempletBO(_templetID);
            bool pass = Request["pass"].ToUpper() == "TRUE" ? true : false;
            if (pass == true)
            {
                this.quality.Enabled = false;
                this.qualityedit.Enabled = false;
            }
            DataTable tabletype = _templet.GetQuality();
            DropDownList list = ((DropDownList)this.Page.FindControl("DropDownList1"));
            list.DataSource = tabletype;
            list.DataTextField = tabletype.Columns["quality_type_name"].ToString();
            list.DataValueField = tabletype.Columns["function_id"].ToString();
            list.DataBind();
            this.Page.Title = _templet.Title;
            HtmlTableCell cellFieldName, cellFieldInput;
            HtmlTableRow rowInput;
            int str1 = _templet.Fields.Count;
            string deptcode = _templet.GetDeptFields().Rows[0][0].ToString();
            foreach (GoldNet.JXKP.Templet.BLL.Field field in _templet.GetFields(_templetID))
            {
                cellFieldName = new MyHtmlTableCell();
                cellFieldInput = new MyHtmlTableCell();
                rowInput = new HtmlTableRow();
                TextField tf = new TextField();
                if (field.FieldTypeID == TempletBO.NUMBERFIELDTYPEID)
                {
                    rowInput.Attributes["onpaste"] = "return false";
                }
                Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label(field.FieldName);
                cellFieldName.Controls.Add(lb);
                cellFieldName.VAlign = "top";
                cellFieldName.Width = "14%";
                rowInput.Cells.Add(cellFieldName);
                rowInput.Cells.Add(cellFieldInput);
                ((HtmlTable)this.Page.FindControl("tabInput")).Rows.Add(rowInput);
                field.ShowEditControl(_templet.TabName, _recID, cellFieldInput, pass);
            }

            if (!IsPostBack)
            {
                Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
                DataTable table = dal.GetQulityCheck(_templet.TabName, _recID.ToString());
                if (table.Rows.Count > 0)
                {
                    this.quality.Checked = table.Rows[0]["IS_CHECKLOOK"].ToString() == "1" ? true : false;
                    if (this.quality.Checked == false)
                    {
                        this.qualityedit.Visible = false;
                        this.qualityedit.Checked = false;
                    }
                    else
                    {
                        this.qualityedit.Visible = true;
                        this.qualityedit.Checked = table.Rows[0]["FLAGS"].ToString() == "1" ? true : false;
                    }
                        
                }
            }
        }
        //保存
        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            try
            {
                _templet.checkrecorddel(_recID);
                _templet.UpdataRecord(this.Page, Staff.GetStaff(), _recID, 0);
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.RefreshData();");
                scManager.AddScript("parent.ListDetail.hide();");
            }
            catch (Exception ex)
            {
                ShowDataError(ex, Request.Url.LocalPath, "btnSave_Click");

            }
        }

    }

}
