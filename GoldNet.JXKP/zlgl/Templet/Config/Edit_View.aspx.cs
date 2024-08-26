using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GoldNet.Comm;
using GoldNet.JXKP.Templet.BLL;
using Goldnet.Ext.Web;


namespace GoldNet.JXKP.zlgl.Templet.Config
{
    public partial class Edit_View : PageBase
    {
        int _templetID, _viewID;
        GoldNet.JXKP.Templet.BLL.ListView _view;
        TempletBO _templet;
        protected void Page_Load(object sender, EventArgs e)
        {
            // 获取模板编号
            _templetID = int.Parse(Request["templetid"].ToString());
            _viewID = int.Parse(Request["viewid"].ToString());

            _templet = new TempletBO(_templetID);
            _view = new GoldNet.JXKP.Templet.BLL.ListView(_templet, _viewID);
            int templetcount = _templet.GetFields(_templetID).Count;

            // 初始化页面值
            if (!this.IsPostBack)
            {
                textViewName.Text = _view.Name;
                textPageCount.Text = _view.PageCount.ToString();
            }

            // 显示字段
            HtmlTableCell td1, td2, td3, td4;
            td1 = (HtmlTableCell)this.Page.FindControl("td1");
            td2 = (HtmlTableCell)this.Page.FindControl("td2");
            td3 = (HtmlTableCell)this.Page.FindControl("tdFilter");
            td4 = (HtmlTableCell)this.Page.FindControl("tdCollectField");
            td1.RowSpan = td2.RowSpan = td3.RowSpan = td4.RowSpan = _templet.Fields.Count + 1;

            // 初始化无过滤条件选项
            //listFristSortField.Items.Clear();
            //listSecondSortField.Items.Clear();
            listFristSortField.Items.Add(new System.Web.UI.WebControls.ListItem("无", ""));
            listSecondSortField.Items.Add(new System.Web.UI.WebControls.ListItem("无", ""));

            foreach (GoldNet.JXKP.Templet.BLL.Field field in _templet.GetFields(_templetID))
            {
                // 初始化列表显示字段
                HtmlTableCell cell1, cell2, cell3;
                cell1 = new HtmlTableCell();
                cell2 = new HtmlTableCell();
                cell3 = new HtmlTableCell();
                HtmlTableRow row = new HtmlTableRow();
                CheckBox chkboxFieldSel = new CheckBox();
                chkboxFieldSel.ID = "display_field_" + field.ID.ToString();
                if (!this.IsPostBack)
                {
                    if (_templet.exitdisplayfild(field))
                        chkboxFieldSel.Checked = _view.DisplayFields.Contains(field);
                }
                cell1.Controls.Add(chkboxFieldSel);
                cell2.Controls.Add(new LiteralControl("<label for='" + chkboxFieldSel.ClientID + "'>" + field.FieldName + "</label>"));
                ListBox listSortNum = new ListBox();
                listSortNum.ID = "field_sort_" + field.ID.ToString();
                listSortNum.Rows = 1;
                for (int i = 1; i <= templetcount; i++)
                    listSortNum.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                if (!this.IsPostBack)
                {
                    listSortNum.SelectedValue = (field.SortNum > templetcount) ? templetcount.ToString() : field.SortNum.ToString();
                }
                cell3.Controls.Add(listSortNum);
                cell1.Align = "center";
                cell2.Align = "center";
                cell3.Align = "center";
                row.Cells.Add(cell1);
                row.Cells.Add(cell2);
                row.Cells.Add(cell3);
                ((HtmlTable)this.Page.FindControl("tabFieldList")).Rows.Add(row);

                // 初始化排序字段的下拉列表
                listFristSortField.Items.Add(new System.Web.UI.WebControls.ListItem(field.FieldName, field.ID.ToString()));
                listSecondSortField.Items.Add(new System.Web.UI.WebControls.ListItem(field.FieldName, field.ID.ToString()));


                // 过滤字段
                HtmlTableCell filterCell1 = new HtmlTableCell(), filterCell2 = new HtmlTableCell(), filterCell3 = new HtmlTableCell();
                HtmlTableRow filterRow = new HtmlTableRow();
                ListBox listFilterOperator = new ListBox();
                listFilterOperator.Rows = 1;
                listFilterOperator.ID = "filterOperator_field_" + field.ID.ToString();
                listFilterOperator.Items.Add(new System.Web.UI.WebControls.ListItem("无", ""));
                for (int j = 0; j < field.FilterOperators.Length; j++)
                    listFilterOperator.Items.Add(new System.Web.UI.WebControls.ListItem(field.FilterOperators[j]));
                filterCell1.Controls.Add(new LiteralControl("<label for='" + listFilterOperator.ClientID + "'>" + field.FieldName + "</label>"));
                filterCell2.Controls.Add(listFilterOperator);
                filterCell1.Align = "left";
                filterCell2.Align = "left";
                filterRow.Cells.Add(filterCell1);
                filterRow.Cells.Add(filterCell2);
                if (field.FieldTypeName == "数字")
                {
                    NumberField textFilterValue = new NumberField();
                    textFilterValue.ID = "filterValues_field_" + field.ID.ToString();
                    textFilterValue.CssClass = "gs-input-text";
                    textFilterValue.MaxLength = 200;
                    filterCell3.Controls.Add(textFilterValue);
                    filterCell3.Align = "center";
                    filterRow.Cells.Add(filterCell3);

                    FilterBO filter = _view.FieldFilters.Contains(field);
                    if (filter != null)
                    {
                        listFilterOperator.SelectedValue = filter.ComparisonOperator;
                        textFilterValue.Text = filter.ComparisonValues;
                    }
                }
               
                else
                {
                    TextField textFilterValue = new TextField();
                    textFilterValue.ID = "filterValues_field_" + field.ID.ToString();
                    textFilterValue.CssClass = "gs-input-text";
                    textFilterValue.MaxLength = 200;
                    filterCell3.Controls.Add(textFilterValue);
                    filterCell3.Align = "center";
                    filterRow.Cells.Add(filterCell3);

                    FilterBO filter = _view.FieldFilters.Contains(field);
                    if (filter != null)
                    {
                        listFilterOperator.SelectedValue = filter.ComparisonOperator;
                        textFilterValue.Text = filter.ComparisonValues;
                    }
                }
               
                ((HtmlTable)this.Page.FindControl("tabFilter")).Rows.Add(filterRow);
               

                // 汇总字段
                HtmlTableCell collectCell1 = new HtmlTableCell(), collectCell2 = new HtmlTableCell();
                HtmlTableRow collectRow = new HtmlTableRow();
                ListBox listCollectMode = new ListBox();
                listCollectMode.Rows = 1;
                listCollectMode.ID = "collectMode_field_" + field.ID.ToString();
                listCollectMode.Items.Add(new System.Web.UI.WebControls.ListItem("无", ""));
                for (int j = 0; j < field.CollectModes.Length; j++)
                    listCollectMode.Items.Add(new System.Web.UI.WebControls.ListItem(field.CollectModes[j], field.CollectModes[j]));
                collectCell1.Controls.Add(new LiteralControl("<label for='" + listCollectMode.ClientID + "'>" + field.FieldName + "</label>"));
                collectCell2.Controls.Add(listCollectMode);
                collectCell1.Align = "left";
                collectCell2.Align = "left";
                collectRow.Cells.Add(collectCell1);
                collectRow.Cells.Add(collectCell2);
                ((HtmlTable)this.Page.FindControl("tabCollect")).Rows.Add(collectRow);
                // 初始化值
                if (!this.IsPostBack)
                {
                    CollectBO collect = _view.CollectFields.Contains(field);
                    if (collect != null)
                    {
                        listCollectMode.SelectedValue = collect.CollectMode;
                    }
                }
            }

            if (!this.IsPostBack)
            {
                // 显示排序
                if (_view.SortFields.FristSortField == null)
                {
                    listFristSortField.SelectedValue = "";
                    listSecondSortField.SelectedValue = "";
                }
                else
                {
                    listFristSortField.SelectedValue = _view.SortFields.FristSortField.ID.ToString();
                    if (!_view.SortFields.FristSortDesc)
                    {
                        rbtnFristSortAsc.Checked = false;
                        rbtnFristSortDesc.Checked = true;
                    }

                    if (_view.SortFields.SecondSortField == null)
                    {
                        listSecondSortField.SelectedValue = "";
                    }
                    else
                    {
                        listSecondSortField.SelectedValue = _view.SortFields.SecondSortField.ID.ToString();
                        if (!_view.SortFields.SecondSortDesc)
                        {
                            rbtnSecondSortAsc.Checked = false;
                            rbtnSecondSortDesc.Checked = true;
                        }
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, AjaxEventArgs e)
        {
            if (textViewName.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","视图名称不能为空！");
            }
            else if (textPageCount.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "每页显示的记录数不能为空！");
            }
            else
            {
                OleDbConnection myConnection = new OleDbConnection(TempletBO.CONNECT_STRING);
                myConnection.Open();
                OleDbTransaction myTrans = myConnection.BeginTransaction();
                try
                {
                    // 1、保存基本信息。
                    _view.UpdateViewInfo(myTrans, textViewName.Text, Convert.ToInt32(textPageCount.Text));

                    // 保存排序字段信息
                   _view.DelSortField(myTrans);
                    if (listFristSortField.SelectedValue != "")
                    {
                        int fristSortFieldID = Convert.ToInt32(listFristSortField.SelectedValue);
                        bool fristSortDesc = rbtnFristSortDesc.Checked;
                        int secondSortFieldID = 0;
                        bool secondSortFieldDesc = false;
                        if (listSecondSortField.SelectedValue != "")
                        {
                            secondSortFieldID = Convert.ToInt32(listSecondSortField.SelectedValue);
                            secondSortFieldDesc = rbtnSecondSortDesc.Checked;
                        }

                        _view.UpdateSortField(myTrans, fristSortFieldID, fristSortDesc, secondSortFieldID, secondSortFieldDesc);
                    }

                    _view.DelAllFilter(myTrans);
                    _view.DelAllCollect(myTrans);

                    foreach (GoldNet.JXKP.Templet.BLL.Field field in _templet.GetFields(_templetID))
                    {
                        // 保存显示字段信息。
                        CheckBox chkboxFieldSel = (CheckBox)this.Page.FindControl("display_field_" + field.ID.ToString());
                        if (chkboxFieldSel.Checked)
                            _view.AddDisplayField(myTrans, field.ID);
                        else
                            _view.DeleteDisplayField(myTrans, field.ID);

                        // 保存排序号
                        ListBox listSortNum = (ListBox)this.Page.FindControl("field_sort_" + field.ID.ToString());
                        field.UpdataFieldSortNum(Convert.ToInt32(listSortNum.SelectedValue));

                        // 保存过滤信息
                        ListBox listFilterOperator = (ListBox)this.Page.FindControl("filterOperator_field_"
                            + field.ID.ToString());
                        if (listFilterOperator.SelectedValue != "")
                        {
                            if (field.FieldTypeName == "数字")
                            {
                                NumberField textFilterValue = (NumberField)this.Page.FindControl("filterValues_field_"
                                    + field.ID.ToString());
                                if (textFilterValue.Text != string.Empty)
                                {
                                    _view.AddFilter(myTrans, field.ID, listFilterOperator.SelectedValue, textFilterValue.Text);
                                }
                            }
                            else
                            {
                                TextField textFilterValue = (TextField)this.Page.FindControl("filterValues_field_"
                                    + field.ID.ToString());
                                if (textFilterValue.Text != string.Empty)
                                {
                                    _view.AddFilter(myTrans, field.ID, listFilterOperator.SelectedValue, textFilterValue.Text);
                                }
                            }
                        }

                        // 保存汇总信息
                        ListBox listCollectMode = (ListBox)this.Page.FindControl("collectMode_field_"
                            + field.ID.ToString());
                        if (listCollectMode.SelectedValue != "")
                        {
                            _view.AddCollect(myTrans, field.ID, listCollectMode.SelectedValue);
                        }
                    }
                    myTrans.Commit();
                    myConnection.Close();
                    myConnection.Dispose();
                    this.SaveSucceed();
                    // Response.Redirect("TempletDetail.aspx?templetid=" + _templetID.ToString());
                }
                catch (Exception ex)
                {
                    myTrans.Rollback();
                    myConnection.Close();
                    ShowDataError(ex, Request.Url.LocalPath, "btnSave_Click");
                }
            }
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("TempletDetail.aspx?templetid=" + _templetID.ToString());
        }
    }
}
