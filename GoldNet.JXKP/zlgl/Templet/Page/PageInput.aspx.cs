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
    public partial class PageInput : PageBase
    {
        private int _templetID;
        private TempletBO _templet;
        protected void Page_Load(object sender, EventArgs e)
        {
  //         if (!Ext.IsAjaxRequest)
            {
                _templetID = int.Parse(Request["templetid"].ToString());
                _templet = new TempletBO(_templetID);
                bool pass = Request["pass"].ToUpper() == "TRUE" ? true : false;
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
                    if (field.FieldTypeID == TempletBO.NUMBERFIELDTYPEID)
                    {
                        rowInput.Attributes["onpaste"] = "return false";
                    }
                    TextField tf = new TextField();
                    Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label(field.FieldName);
                    cellFieldName.Controls.Add(lb);
                    cellFieldName.VAlign = "top";
                    cellFieldName.Width = "14%";
                    rowInput.Cells.Add(cellFieldName);
                    rowInput.Cells.Add(cellFieldInput);
                    ((HtmlTable)this.Page.FindControl("tabInput")).Rows.Add(rowInput);
                    field.ShowInputControl(cellFieldInput,pass);
                }
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            try
            {
                _templet.InsertNewRecord(this.Page, Staff.GetStaff());
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.RefreshData();");
                scManager.AddScript("parent.ListDetail.hide();");
            }
            catch (Exception ex)
            {
                ShowDataError(ex, Request.Url.LocalPath, "BtnSave_Click");

            }
        }
       
    }
    public class MyHtmlTableCell : HtmlTableCell
    {
        public MyHtmlTableCell()
        {
        }
        private string _addattr;
        public string addattr
        {
            set
            {
                _addattr = value;
            }
            get
            {
                return _addattr;
            }
        }
    }
}
