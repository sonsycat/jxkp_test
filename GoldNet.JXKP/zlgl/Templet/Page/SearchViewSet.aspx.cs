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

namespace GoldNet.JXKP.zlgl.Templet.Page
{
    public partial class SearchViewSet : PageBase
    {
        int _templetID;
        GoldNet.JXKP.Templet.BLL.ListView _view;
        GoldNet.JXKP.Templet.BLL.ListView _oldView = null;
        TempletBO _templet;
        protected void Page_Load(object sender, EventArgs e)
        {
            _templetID = int.Parse(Request["templetid"].ToString());
            _templet = new TempletBO(_templetID);
            if (Session["CURRENT_VIEW"] != null)
            {
                _oldView = (GoldNet.JXKP.Templet.BLL.ListView)Session["CURRENT_VIEW"];
                if (_oldView.TempletID != this._templetID)
                {
                    Session["CURRENT_VIEW"] = null;
                    _oldView = _templet.DefaultView;
                }
            }
            else
            {
                _oldView = _templet.DefaultView;
            }

            // 初始化页面值
            if (!this.IsPostBack)
            {
                textPageCount.Text = _oldView.PageCount.ToString();
            }

            // 显示字段
            HtmlTableCell td1, td2, td3, td4;
            td1 = (HtmlTableCell)this.Page.FindControl("td1");
            td2 = (HtmlTableCell)this.Page.FindControl("td2");
            td3 = (HtmlTableCell)this.Page.FindControl("tdFilter");
            td4 = (HtmlTableCell)this.Page.FindControl("tdCollectField");
            td1.RowSpan = td2.RowSpan = td3.RowSpan = td4.RowSpan = _templet.Fields.Count + 1;

            // 初始化无过滤条件选项
            listFristSortField.Items.Add(new System.Web.UI.WebControls.ListItem("无", ""));
            listSecondSortField.Items.Add(new System.Web.UI.WebControls.ListItem("无", ""));
            int fieldscount = _templet.GetFields(_templetID).Count;

            foreach (GoldNet.JXKP.Templet.BLL.Field field in _templet.GetFields(_templetID))
            {
                // 初始化列表显示字段
                if (_templet.exitdisplayfild(field))
                {
                    HtmlTableCell cell1, cell2, cell3;
                    cell1 = new HtmlTableCell();
                    cell2 = new HtmlTableCell();
                    cell3 = new HtmlTableCell();
                    HtmlTableRow row = new HtmlTableRow();
                    CheckBox chkboxFieldSel = new CheckBox();
                    chkboxFieldSel.ID = "display_field_" + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString());
                    if (!this.IsPostBack)
                    {
                        chkboxFieldSel.Checked = _oldView.DisplayFields.Contains(field);
                    }
                    cell1.Controls.Add(chkboxFieldSel);
                    cell2.Controls.Add(new LiteralControl("<label for='" + chkboxFieldSel.ClientID + "'>" + field.FieldName + "</label>"));
                    ListBox listSortNum = new ListBox();
                    listSortNum.ID = "field_sort_" + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString());
                    listSortNum.Rows = 1;
                    for (int i = 1; i <= _templet.GetFields(_templetID).Count; i++)
                        listSortNum.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                    if (!this.IsPostBack)
                    {
                        listSortNum.SelectedValue = (field.SortNum > fieldscount) ? fieldscount.ToString() : field.SortNum.ToString();
                    }
                    cell3.Controls.Add(listSortNum);
                    cell3.Align = "right";
                    row.Cells.Add(cell1);
                    row.Cells.Add(cell2);
                    row.Cells.Add(cell3);
                    ((HtmlTable)this.Page.FindControl("tabFieldList")).Rows.Add(row);

                    // 初始化排序字段的下拉列表
                    listFristSortField.Items.Add(new System.Web.UI.WebControls.ListItem(field.FieldName, Encrypt.EncryptMyStr("iloveyou", field.ID.ToString())));
                    listSecondSortField.Items.Add(new System.Web.UI.WebControls.ListItem(field.FieldName, Encrypt.EncryptMyStr("iloveyou", field.ID.ToString())));


                    // 过滤字段
                    HtmlTableCell filterCell1 = new HtmlTableCell(), filterCell2 = new HtmlTableCell(), filterCell3 = new HtmlTableCell();
                    HtmlTableRow filterRow = new HtmlTableRow();
                    ListBox listFilterOperator = new ListBox();
                    listFilterOperator.Rows = 1;
                    listFilterOperator.ID = "filterOperator_field_" + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString());
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
                        textFilterValue.ID = "filterValues_field_" + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString());
                        textFilterValue.CssClass = "gs-input-text";
                        textFilterValue.MaxLength = 200;
                        filterCell3.Controls.Add(textFilterValue);
                        filterCell3.Align = "center";
                        filterRow.Cells.Add(filterCell3);

                        FilterBO filter = _oldView.FieldFilters.Contains(field);
                        if (filter != null)
                        {
                            listFilterOperator.SelectedValue = filter.ComparisonOperator;
                            textFilterValue.Text = filter.ComparisonValues;
                        }
                    }

                    else
                    {
                        TextField textFilterValue = new TextField();
                        textFilterValue.ID = "filterValues_field_" + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString());
                        textFilterValue.CssClass = "gs-input-text";
                        textFilterValue.MaxLength = 200;
                        filterCell3.Controls.Add(textFilterValue);
                        filterCell3.Align = "center";
                        filterRow.Cells.Add(filterCell3);

                        FilterBO filter = _oldView.FieldFilters.Contains(field);
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
                    listCollectMode.ID = "collectMode_field_" + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString());
                    listCollectMode.Items.Add(new System.Web.UI.WebControls.ListItem("无", ""));
                    for (int j = 0; j < field.CollectModes.Length; j++)
                        listCollectMode.Items.Add(new System.Web.UI.WebControls.ListItem(field.CollectModes[j], field.CollectModes[j]));
                    collectCell1.Controls.Add(new LiteralControl("<label for='" + listCollectMode.ClientID + "'>" + field.FieldName + "</label>"));
                    collectCell2.Controls.Add(listCollectMode);
                    collectRow.Cells.Add(collectCell1);
                    collectRow.Cells.Add(collectCell2);
                    ((HtmlTable)this.Page.FindControl("tabCollect")).Rows.Add(collectRow);
                    // 初始化值
                    if (!this.IsPostBack)
                    {
                        CollectBO collect = _oldView.CollectFields.Contains(field);
                        if (collect != null)
                        {
                            listCollectMode.SelectedValue = collect.CollectMode;
                        }
                    }
                }
            }

            if (!this.IsPostBack)
            {
                // 显示排序
                if (_oldView.SortFields.FristSortField == null)
                {
                    listFristSortField.SelectedValue = "";
                    listSecondSortField.SelectedValue = "";
                }
                else
                {
                    listFristSortField.SelectedValue = Encrypt.EncryptMyStr("iloveyou", _oldView.SortFields.FristSortField.ID.ToString());
                    if (_oldView.SortFields.FristSortDesc)
                    {
                        rbtnFristSortAsc.Checked = false;
                        rbtnFristSortDesc.Checked = true;
                    }

                    if (_oldView.SortFields.SecondSortField == null)
                    {
                        listSecondSortField.SelectedValue = "";
                    }
                    else
                    {
                        listSecondSortField.SelectedValue = Encrypt.EncryptMyStr("iloveyou", _oldView.SortFields.SecondSortField.ID.ToString());
                        if (_oldView.SortFields.SecondSortDesc)
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
            // 保存
            // 创建一个临时视图，收集页面配置，保存到Session["CURRENT_VIEW"]中
            this._view = new GoldNet.JXKP.Templet.BLL.ListView(_templet);

            // 1、保存基本信息。
            this._view.PageCount = Convert.ToInt32(textPageCount.Text);

            // 保存排序字段信息
            int fristSortFieldID = 0, secondSortFieldID = 0;
            bool fristSortDesc = false, secondSortFieldDesc = false;
            if (listFristSortField.SelectedValue != "")
            {
                fristSortFieldID = Convert.ToInt32(Encrypt.UnEncryptMyStr("iloveyou", listFristSortField.SelectedValue));
                fristSortDesc = rbtnFristSortDesc.Checked;
                secondSortFieldID = 0;
                secondSortFieldDesc = false;
                if (listSecondSortField.SelectedValue != "")
                {
                    secondSortFieldID = Convert.ToInt32(Encrypt.UnEncryptMyStr("iloveyou", listSecondSortField.SelectedValue));
                    secondSortFieldDesc = rbtnSecondSortDesc.Checked;
                }
            }
            SortFieldsBO sortFieldBO = new SortFieldsBO(fristSortFieldID, fristSortDesc, secondSortFieldID, secondSortFieldDesc);
            _view.SortFields = sortFieldBO;

            // 保存显示字段、过滤、汇总集合
            FieldCollection displayFields = new FieldCollection();
            FilterCollection filterFields = new FilterCollection();
            CollectCollection collectFields = new CollectCollection();

            foreach (GoldNet.JXKP.Templet.BLL.Field field in _templet.GetFields(_templetID))
            {
                // 保存显示字段信息。
                CheckBox chkboxFieldSel = (CheckBox)this.Page.FindControl("display_field_" + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString()));
                if (chkboxFieldSel != null)
                {
                    if (chkboxFieldSel.Checked)
                    {
                        displayFields.Add(field);
                    }
                }

                // 保存过滤信息
                ListBox listFilterOperator = (ListBox)this.Page.FindControl("filterOperator_field_"
                    + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString()));
                if (listFilterOperator != null)
                {
                    if (listFilterOperator.SelectedValue != "")
                    {
                        if (field.FieldTypeName == "数字")
                        {
                            NumberField textFilterValue = (NumberField)this.Page.FindControl("filterValues_field_"
                                + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString()));

                            // 如果填写的备选值为空,不保存此条件
                            if (textFilterValue.Text != string.Empty)
                            {
                                FilterBO filter = new FilterBO(field.ID, listFilterOperator.SelectedValue, textFilterValue.Text);
                                filterFields.Add(filter);
                            }
                        }
                        else
                        {
                            TextField textFilterValue = (TextField)this.Page.FindControl("filterValues_field_"
                                + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString()));

                            // 如果填写的备选值为空,不保存此条件
                            if (textFilterValue.Text != string.Empty)
                            {
                                FilterBO filter = new FilterBO(field.ID, listFilterOperator.SelectedValue, textFilterValue.Text);
                                filterFields.Add(filter);
                            }
                        }
                    }
                }

                // 保存汇总信息
                ListBox listCollectMode = (ListBox)this.Page.FindControl("collectMode_field_"
                    + Encrypt.EncryptMyStr("iloveyou", field.ID.ToString()));
                if (listCollectMode != null)
                {
                    if (listCollectMode.SelectedValue != "")
                    {
                        CollectBO collect = new CollectBO(field.ID, listCollectMode.SelectedValue);

                        collectFields.Add(collect);
                    }
                }
            }
            _view.DisplayFields = displayFields;
            _view.FieldFilters = filterFields;
            _view.CollectFields = collectFields;

            // 保存视图到 Session["CURRENT_VIEW"] 
            Session["CURRENT_VIEW"] = _view;

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.searchset.hide();");
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            // 取消
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.searchset.hide();");
        }
    }
}
