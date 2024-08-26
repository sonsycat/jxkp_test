using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.Templet.BLL.Fields
{
    /// <summary>
    /// 多选字段 
    /// </summary>
    [Serializable()]
    internal class SelectField : IFieldType
    {
        #region --私有变量--
        private Field _field;
        private SelectFieldDefineInfo _selectInfo;
        #endregion

        #region --构造函数--
        public SelectField(Field field)
        {
            _field = field;
        }
        #endregion

        #region --内部实体类--
        [Serializable()]
        private class SelectFieldDefineInfo
        {
            public int ID;
            public bool ValueRequired;
            public string OptionalValues;
            public int SelectStyle;
            public bool CanAddnew;
            public string DefaultValue;
            public string Common;
        }
        #endregion

        #region IFieldType 成员

        public string FieldTypeName
        {
            get
            {
                return "选项(从列表中选择)";
            }
        }

        public string[] FilterOperators
        {
            get
            {
                return new string[] { "等于", "包含" };
            }
        }

        public string[] CollectModes
        {
            get
            {
                return new string[] { "计数" };
            }
        }

        public string ListDisplayDataName
        {
            get
            {
                return "SELECT_" + _field.ID.ToString();
            }
        }

        public string ListDataFormatString
        {
            get
            {
                return string.Empty;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="curCtrl"></param>
        public void ShowSpecialProperty(Control curCtrl)
        {
            curCtrl.Controls.Add(new LiteralControl("<table width='100%' border='0' cellpadding='0' cellspacing='0'>"));

            // 1、说明 Common
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'> 说明:<BR>&nbsp;"));
            TextBox textCommon = new TextBox();
            textCommon.ID = "textFieldCommon";
            textCommon.Rows = 3;
            textCommon.Width = new Unit("280px");
            textCommon.TextMode = TextBoxMode.MultiLine;
            curCtrl.Controls.Add(textCommon);
            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

            // 2、必填 ValueRequired
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>是否为必填字段:<BR>&nbsp;"));
            CheckBox checkValueRequested = new CheckBox();
            checkValueRequested.ID = "checkValueRequested";
            checkValueRequested.Text = "是";
            curCtrl.Controls.Add(checkValueRequested);
            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

            // 3、可选值 OptionalValues
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>分行键入每个选项:<BR>&nbsp;"));
            TextBox textOptionalValues = new TextBox();
            textOptionalValues.ID = "textOptionalValues";
            textOptionalValues.Rows = 5;
            textOptionalValues.Width = new Unit("280px");
            textOptionalValues.TextMode = TextBoxMode.MultiLine;
            curCtrl.Controls.Add(textOptionalValues);
            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));



            // 4、选择样式 SelectStyle
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>显示选项使用:"));
            RadioButtonList radioSelectStyle = new RadioButtonList();
            radioSelectStyle.ID = "radioSelectStyle";
            radioSelectStyle.RepeatDirection = RepeatDirection.Vertical;
            radioSelectStyle.Items.Add(new System.Web.UI.WebControls.ListItem("下拉菜单", "0"));
            radioSelectStyle.Items.Add(new System.Web.UI.WebControls.ListItem("单选按钮", "1"));
            radioSelectStyle.Items.Add(new System.Web.UI.WebControls.ListItem("复选框(允许多重选择)", "2"));
            radioSelectStyle.SelectedIndex = 0;
            radioSelectStyle.CellPadding = 0;
            radioSelectStyle.CellSpacing = 2;
            curCtrl.Controls.Add(radioSelectStyle);
            curCtrl.Controls.Add(new LiteralControl("<table border=0 cellpadding='0' cellspacing='0'><tr><td><FONT COLOR='#808080'>&nbsp;&nbsp;注：目前复选框无法实现必填验证。</FONT></td></tr></table>"));
            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));




            // 5、是否可以添加新值 CanAddnew
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>允许“填充”选项:<BR>&nbsp;"));
            CheckBox checkCanAddnew = new CheckBox();
            checkCanAddnew.ID = "checkCanAddnew";
            checkCanAddnew.Text = "是";
            curCtrl.Controls.Add(checkCanAddnew);

            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

            // 6、默认值 DefaultValue
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>默认值:<BR>&nbsp;"));
            TextBox textDefaultValue = new TextBox();
            textDefaultValue.ID = "textDefaultValue";
            textDefaultValue.TextMode = TextBoxMode.SingleLine;
            textDefaultValue.MaxLength = 50;
            textDefaultValue.Width = new Unit("200px");
            curCtrl.Controls.Add(textDefaultValue);
            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

            curCtrl.Controls.Add(new LiteralControl("</table>"));

            if (_field != null)
            {
                // 初始化控件
                textCommon.Text = this.selectDefineInfo.Common;
                checkValueRequested.Checked = this.selectDefineInfo.ValueRequired;
                textOptionalValues.Text = this.selectDefineInfo.OptionalValues;
                radioSelectStyle.SelectedValue = this.selectDefineInfo.SelectStyle.ToString();
                checkCanAddnew.Checked = this.selectDefineInfo.CanAddnew;
                textDefaultValue.Text = this.selectDefineInfo.DefaultValue;
            }
        }

        public int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
        {
            SelectFieldDefineInfo selectInfo = this.getSelectFieldInfoByPorpertyPage(curPage);

            int fieldDefineID = insertNew(selectInfo);

            // 定义SQL字段定义语句.
            fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ ADD SELECT_" + this._field.ID.ToString()
                + " VARCHAR(300) NULL ";

            return fieldDefineID;
        }

        public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
        {
            SelectFieldDefineInfo selectInfo = this.getSelectFieldInfoByPorpertyPage(curPage);

            // 更新属性定义
            this.updateField(selectInfo, this._field.FieldDefineID);

            // 定义SQL定义语句
            fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ modify ( SELECT_" + this._field.ID.ToString()
                + " VARCHAR2(300) )";
        }

        public void ShowInputControl(Control curCtrl,bool pass)
        {
            // 变量定义
            RadioButton radioValueInTextBoxCtrl = null, radioValueInSelectCtrl = null;
            ListControl selectControl = null;
            TextBox textNewValue = null;
            RequiredFieldValidator requiredForText = null, requiredForSel = null;

            // 录入控件显示
            bool canAddnew = this.selectDefineInfo.CanAddnew;

            // 如果可以添加新值，定义一个新值
            if (canAddnew)
            {
                // 显示一个radiobutton : 从列表中选择
                radioValueInSelectCtrl = new RadioButton();
                radioValueInSelectCtrl.ID = "SEL_VAL_IN_SEL_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                radioValueInSelectCtrl.Checked = true;
                radioValueInSelectCtrl.GroupName = "valueIn";
                radioValueInSelectCtrl.Text = "从可选项中选择: &nbsp;";
                curCtrl.Controls.Add(radioValueInSelectCtrl);
            }

            // 显示选择控件
            switch (this.selectDefineInfo.SelectStyle)
            {
                case 1:
                    // 单选
                    selectControl = new RadioButtonList();
                    ((RadioButtonList)selectControl).RepeatDirection = RepeatDirection.Horizontal;
                    ((RadioButtonList)selectControl).RepeatColumns = 4;
                    ((RadioButtonList)selectControl).CellSpacing = 3;
                    break;
                case 2:
                    // 复选框
                    selectControl = new CheckBoxList();
                    ((CheckBoxList)selectControl).RepeatDirection = RepeatDirection.Horizontal;
                    ((CheckBoxList)selectControl).RepeatColumns = 4;
                    ((CheckBoxList)selectControl).CellSpacing = 3;
                    break;
                default:
                    // 下拉列表
                    selectControl = new DropDownList();
                    break;
            }

            selectControl.ID = "SELECT_SELINPUT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());

            // 当点击列表中任何选项时，列表CheckBox被选中。
            if (canAddnew)
                selectControl.Attributes["onclick"] = "javascript:document.all['" + radioValueInSelectCtrl.ClientID + "'].checked=true;";

            // 创建可选列表项
            string[] values = this.selectDefineInfo.OptionalValues.Split('\n');
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != "")
                    selectControl.Items.Add(new System.Web.UI.WebControls.ListItem(values[i].Replace("\r", ""), values[i].Replace("\r", "")));
            }

            // 添加列表控件

            if (this.selectDefineInfo.SelectStyle != 1)
                curCtrl.Controls.Add(selectControl);

            // 添加验证控件: 如果需要验证是否为空且不是复选框
            if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
            {
                requiredForSel = new RequiredFieldValidator();
                requiredForSel.ControlToValidate = selectControl.ID;
                requiredForSel.Display = ValidatorDisplay.Dynamic;
                requiredForSel.ID = "requiredForSel" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                requiredForSel.ErrorMessage = "\"" + this._field.FieldName + "\"为必填字段，不能为空，请至少指定一个选项！";
                requiredForSel.Text = "*";
                curCtrl.Controls.Add(requiredForSel);
            }

            //** 土招：是的话后添加；
            if (this.selectDefineInfo.SelectStyle == 1) curCtrl.Controls.Add(selectControl);

            if (this.selectDefineInfo.SelectStyle == 0) curCtrl.Controls.Add(new LiteralControl("<br>"));

            // 如果可以添加新值，再定义一个RadioButton
            if (canAddnew)
            {
                // 显示一个radiobutton : 从列表中选择
                radioValueInTextBoxCtrl = new RadioButton();
                radioValueInTextBoxCtrl.ID = "SEL_VAL_IN_TEXT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                radioValueInTextBoxCtrl.GroupName = "valueIn";
                radioValueInTextBoxCtrl.Text = "填写一个新值: ";
                curCtrl.Controls.Add(radioValueInTextBoxCtrl);
                curCtrl.Controls.Add(new LiteralControl("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));

                textNewValue = new TextBox();
                textNewValue.ID = "SELECT_TEXTINPUT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                textNewValue.MaxLength = 50;
                textNewValue.Width = new Unit("240px");
                // 当文本框获得焦点时，其选择按钮被选中
                textNewValue.Attributes["onfocus"] = "javascript:document.all['" + radioValueInTextBoxCtrl.ClientID + "'].checked=true;";
                curCtrl.Controls.Add(textNewValue);

                // 添加验证控件: 如果需要验证是否为空且不是复选框
                if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
                {
                    requiredForText = new RequiredFieldValidator();
                    requiredForText.ControlToValidate = textNewValue.ID;
                    requiredForText.ID = "requiredForText" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                    requiredForText.ErrorMessage = "\"" + this._field.FieldName + "\"为必填字段，不能为空，请填写内容！";
                    requiredForText.Text = "*";
                    requiredForText.Enabled = false;
                    curCtrl.Controls.Add(requiredForText);

                    radioValueInTextBoxCtrl.Attributes["onclick"] = @"javascript:ValidatorEnable(document.all['" + requiredForSel.ID + "'], false);ValidatorEnable(document.all['" + requiredForText.ID + "'], true);";

                    radioValueInSelectCtrl.Attributes["onclick"] = @"javascript:ValidatorEnable(document.all['" + requiredForText.ID + "'], false);ValidatorEnable(document.all['" + requiredForSel.ID + "'], true);";
                }
            }
        }

        public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage, int id)
        {
            string valStr = "";

            // 如果有可添加新值选项，则先判断用户选择的是新值还是从列表中选取的
            if (this.selectDefineInfo.CanAddnew)
            {
                RadioButton radioValueInTextBoxCtrl = (RadioButton)curPage.FindControl("SEL_VAL_IN_TEXT_" + Encrypt.EncryptMyStr("iloveyou", _field.ID.ToString()));
                if (radioValueInTextBoxCtrl.Checked)
                {

                    // 用户选择录入新值，从TextBox中获取值
                    TextBox textNewValue
                        = (TextBox)curPage.FindControl("SELECT_TEXTINPUT_" + Encrypt.EncryptMyStr("iloveyou", _field.ID.ToString()));
                    valStr = textNewValue.Text;
                }
                else
                {
                    // 用户选择了从列表中选取，再判断是否多选
                    ListControl selectControl
                        = (ListControl)curPage.FindControl("SELECT_SELINPUT_" + Encrypt.EncryptMyStr("iloveyou", _field.ID.ToString()));

                    if (this.selectDefineInfo.SelectStyle == 2)
                    {
                        // 多选
                        foreach (System.Web.UI.WebControls.ListItem item in selectControl.Items)
                        {
                            if (item.Selected) valStr += item.Value + ";";
                        }
                    }
                    else
                    {
                        // 单选
                        valStr = selectControl.SelectedValue;
                    }
                }
            }
            else
            {
                // 无添加新值选项，从列表中获取值
                ListControl selectControl
                    = (ListControl)curPage.FindControl("SELECT_SELINPUT_" + Encrypt.EncryptMyStr("iloveyou", _field.ID.ToString()));

                if (this.selectDefineInfo.SelectStyle == 2)
                {
                    // 多选
                    foreach (System.Web.UI.WebControls.ListItem item in selectControl.Items)
                    {
                        if (item.Selected) valStr += item.Value + ";";
                    }
                }
                else
                {
                    // 单选
                    valStr = selectControl.SelectedValue;
                }
            }
            if (this.selectDefineInfo.ValueRequired && valStr == string.Empty)
            {
                throw new SaveRecordDataIsNullException(_field.FieldName);
            }
            else
            {
                string sql = string.Format("UPDATE {0}." + tabName + " SET SELECT_" + this._field.ID.ToString() + " = ? WHERE (ID = ?)", DataUser.ZLGL);

                // 最大字符串长度限制
                if (valStr.Length > 300) valStr = valStr.Substring(0, 300);

                OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,sql,
                    new OleDbParameter("newValue", valStr),
                    new OleDbParameter("id", recId));
            }

        }

        public void ShowViewData(string tabName, int recID, Control curCtrl)
        {
            string sql = string.Format("SELECT SELECT_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)", DataUser.ZLGL);

            string val = Convert.ToString(OracleOledbBase.ExecuteScalar(sql,
                new OleDbParameter("recid", recID)));

            curCtrl.Controls.Add(new LiteralControl(CleanString.InputText(val, 1000)));
        }

        public void ShowEditControl(string tabName, int recID, Control curCtrl, bool pass)
        {
            // 变量定义
            RadioButton radioValueInTextBoxCtrl = null, radioValueInSelectCtrl = null;
            ListControl selectControl = null;
            TextBox textNewValue = null;
            RequiredFieldValidator requiredForText = null, requiredForSel = null;

            

            string sql = string.Format("SELECT SELECT_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)", DataUser.ZLGL);

            string val = Convert.ToString(OracleOledbBase.ExecuteScalar(sql,
                new OleDbParameter("recid", recID)));

            // 录入控件显示
            bool canAddnew = this.selectDefineInfo.CanAddnew;

            // 如果可以添加新值，定义一个新值
            if (canAddnew)
            {
                // 显示一个radiobutton : 从列表中选择
                radioValueInSelectCtrl = new RadioButton();
                radioValueInSelectCtrl.ID = "SEL_VAL_IN_SEL_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                radioValueInSelectCtrl.Checked = true;
                radioValueInSelectCtrl.GroupName = "valueIn";
                radioValueInSelectCtrl.Text = "从可选项中选择: &nbsp;";
                curCtrl.Controls.Add(radioValueInSelectCtrl);
                if (pass)
                {
                    radioValueInSelectCtrl.Enabled = false;
                }
            }

            // 显示选择控件
            switch (this.selectDefineInfo.SelectStyle)
            {
                case 1:
                    // 单选
                    selectControl = new RadioButtonList();
                    ((RadioButtonList)selectControl).RepeatDirection = RepeatDirection.Horizontal;
                    ((RadioButtonList)selectControl).RepeatColumns = 4;
                    ((RadioButtonList)selectControl).CellSpacing = 3;
                    if (pass)
                    {
                        selectControl.Enabled = false;
                    }
                    break;
                case 2:
                    // 复选框
                    selectControl = new CheckBoxList();
                    ((CheckBoxList)selectControl).RepeatDirection = RepeatDirection.Horizontal;
                    ((CheckBoxList)selectControl).RepeatColumns = 4;
                    ((CheckBoxList)selectControl).CellSpacing = 3;
                    if (pass)
                    {
                        selectControl.Enabled = false;
                    }
                    break;
                default:
                    // 下拉列表
                    selectControl = new DropDownList();
                    if (pass)
                    {
                        selectControl.Enabled = false;
                    }
                    break;
            }

            selectControl.ID = "SELECT_SELINPUT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());

            // 当点击列表中任何选项时，列表CheckBox被选中。
            if (canAddnew)
                selectControl.Attributes["onclick"] = "javascript:document.all['" + radioValueInSelectCtrl.ClientID + "'].checked=true;";

            // 创建可选列表项
            string[] values = this.selectDefineInfo.OptionalValues.Split('\n');
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != "")
                    selectControl.Items.Add(new System.Web.UI.WebControls.ListItem(values[i].Replace("\r", ""), values[i].Replace("\r", "")));
            }

            // 添加列表控件

            if (this.selectDefineInfo.SelectStyle != 1)
                curCtrl.Controls.Add(selectControl);

            // 添加验证控件: 如果需要验证是否为空且不是复选框
            if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
            {
                requiredForSel = new RequiredFieldValidator();
                requiredForSel.ControlToValidate = selectControl.ID;
                requiredForSel.Display = ValidatorDisplay.Dynamic;
                requiredForSel.ID = "requiredForSel" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                requiredForSel.ErrorMessage = "\"" + this._field.FieldName + "\"为必填字段，不能为空，请至少指定一个选项！";
                requiredForSel.Text = "*";
                curCtrl.Controls.Add(requiredForSel);
            }


            if (this.selectDefineInfo.SelectStyle == 1) curCtrl.Controls.Add(selectControl);

            if (this.selectDefineInfo.SelectStyle == 0) curCtrl.Controls.Add(new LiteralControl("<br>"));

            // 如果可以添加新值，再定义一个RadioButton
            if (canAddnew)
            {
                // 显示一个radiobutton : 从列表中选择
                radioValueInTextBoxCtrl = new RadioButton();
                radioValueInTextBoxCtrl.ID = "SEL_VAL_IN_TEXT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                radioValueInTextBoxCtrl.GroupName = "valueIn";
                radioValueInTextBoxCtrl.Text = "填写一个新值: ";
                curCtrl.Controls.Add(radioValueInTextBoxCtrl);
                curCtrl.Controls.Add(new LiteralControl("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));

                textNewValue = new TextBox();
                textNewValue.ID = "SELECT_TEXTINPUT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                textNewValue.MaxLength = 50;
                textNewValue.Width = new Unit("240px");
                // 当文本框获得焦点时，其选择按钮被选中
                textNewValue.Attributes["onfocus"] = "javascript:document.all['" + radioValueInTextBoxCtrl.ClientID + "'].checked=true;";
                curCtrl.Controls.Add(textNewValue);

                // 添加验证控件: 如果需要验证是否为空且不是复选框
                if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
                {
                    requiredForText = new RequiredFieldValidator();
                    requiredForText.ControlToValidate = textNewValue.ID;
                    requiredForText.ID = "requiredForText" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                    requiredForText.ErrorMessage = "\"" + this._field.FieldName + "\"为必填字段，不能为空，请填写内容！";
                    requiredForText.Text = "*";
                    curCtrl.Controls.Add(requiredForText);

                    radioValueInTextBoxCtrl.Attributes["onclick"] = @"javascript:ValidatorEnable(document.all['" + requiredForSel.ID + "'], false);ValidatorEnable(document.all['" + requiredForText.ID + "'], true);";

                    radioValueInSelectCtrl.Attributes["onclick"] = @"javascript:ValidatorEnable(document.all['" + requiredForText.ID + "'], false);ValidatorEnable(document.all['" + requiredForSel.ID + "'], true);";
                }
            }

            // 初始化值
            bool isSel = false; // 值是否是从备选中选择的
            if (this.selectDefineInfo.SelectStyle == 2)
            {
                // 多选
                foreach (System.Web.UI.WebControls.ListItem item in selectControl.Items)
                {
                    if (val.IndexOf(item.Value) >= 0)
                    {
                        // 如果任何一个备选值在值列表中，则isSel = true;
                        item.Selected = true;
                        isSel = true;
                        if (canAddnew)
                            radioValueInSelectCtrl.Checked = true;
                    }
                }
                if (!isSel && canAddnew)
                {
                    // 如果不是从备选中选择的值，写在TextBox中
                    textNewValue.Text = val;
                    radioValueInTextBoxCtrl.Checked = true;

                    if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
                    {
                        requiredForSel.Enabled = false; // 列表验证控件
                        requiredForText.Enabled = true; // 文本验证控件
                    }
                }
            }
            else
            {
                // 单选
                if (selectControl.Items.Contains(new System.Web.UI.WebControls.ListItem(val, val)))
                {
                    // 如果选择列表中包含值
                    selectControl.SelectedValue = val;
                    if (canAddnew) radioValueInSelectCtrl.Checked = true;

                    if (this.selectDefineInfo.ValueRequired)
                    {
                        requiredForSel.Enabled = true; // 列表验证控件

                        if (canAddnew)
                            requiredForText.Enabled = false; // 文本验证控件
                    }
                }
                else if (canAddnew)
                {
                    // 否则,在TextBox中显示值
                    textNewValue.Text = val;
                    radioValueInTextBoxCtrl.Checked = true;

                    if (this.selectDefineInfo.ValueRequired)
                    {
                        requiredForSel.Enabled = false; // 列表验证控件
                        requiredForText.Enabled = true; // 文本验证控件
                    }
                }
            }
            
        }

        public string GetFilter(string compOperator, string values)
        {
            // FilterOperators "等于", "包含"
            string result;
            switch (compOperator)
            {
                case "等于":
                    result = this.ListDisplayDataName + "= '" + values.Replace("'", "") + "'";
                    break;
                case "包含":
                    result = this.ListDisplayDataName + " like '%" + values.Replace("'", "") + "%'";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        public string ListCollectComputerString(string collectMode)
        {
            // collectMode : "计数"
            string result;
            switch (collectMode)
            {
                case "计数":
                    result = "count(" + this.ListDisplayDataName + ")";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        #endregion

        #region --私有属性--
        private SelectFieldDefineInfo selectDefineInfo
        {
            get
            {
                if (this._selectInfo == null)
                    this._selectInfo = getSelectFieldInfoByID(_field.FieldDefineID);
                return _selectInfo;
            }
        }
        #endregion

        #region --私有方法--
        private int insertNew(SelectFieldDefineInfo selectInfo)
        {
            string sql = string.Format("INSERT INTO {0}.T_SelectFieldTypeDefineDict (id,ValueRequired, OptionalValues, SelectStyle, CanAddnew, DefaultValue, Common) VALUES (?, ?,?,?,?,?,?)", DataUser.ZLGL);
            int id = OracleOledbBase.GetMaxID("ID", string.Format("{0}.T_SelectFieldTypeDefineDict", DataUser.ZLGL));

            OracleOledbBase.ExecuteScalar(sql,
                new OleDbParameter("ID", id),
                new OleDbParameter("valueRequesed", selectInfo.ValueRequired == true ? 0 : -1),
                new OleDbParameter("optionalValues", selectInfo.OptionalValues),
                new OleDbParameter("selectStyle", selectInfo.SelectStyle),
                new OleDbParameter("canAddnew", selectInfo.CanAddnew == true ? 0 : -1),
                new OleDbParameter("defaultValue", CleanString.InputText(selectInfo.DefaultValue,50)),
                new OleDbParameter("common", selectInfo.Common)
                );
            return id;
        }

        private void updateField(SelectFieldDefineInfo selectInfo, int fieldDefineID)
        {
            string sql = string.Format("UPDATE {0}.T_SelectFieldTypeDefineDict SET ValueRequired=?, OptionalValues=?, SelectStyle=?, CanAddnew=?, DefaultValue=?, Common=? WHERE (ID=?)", DataUser.ZLGL);

            OracleOledbBase.ExecuteNonQuery(sql,
                new OleDbParameter("valueRequesed", selectInfo.ValueRequired == true ? 0 : -1),
                new OleDbParameter("optionalValues", selectInfo.OptionalValues),
                new OleDbParameter("selectStyle", selectInfo.SelectStyle),
                new OleDbParameter("canAddnew", selectInfo.CanAddnew == true ? 0 : -1),
                new OleDbParameter("defaultValue", selectInfo.DefaultValue),
                new OleDbParameter("common", selectInfo.Common),
                new OleDbParameter("fieldDefineID", fieldDefineID)
                );
        }

        private SelectFieldDefineInfo getSelectFieldInfoByID(int fieldDefineID)
        {
            string sql;
            sql = string.Format("SELECT ID, ValueRequired, OptionalValues, SelectStyle, CanAddnew, DefaultValue, Common FROM {0}.T_SelectFieldTypeDefineDict WHERE (ID = {1})", DataUser.ZLGL, fieldDefineID);

            SelectFieldDefineInfo selectInfo = null;

            DataTable reader = OracleOledbBase.ExecuteDataSet(sql).Tables[0];

            for (int i = 0; i < reader.Rows.Count; i++)
            {
                selectInfo = new SelectFieldDefineInfo();
                selectInfo.ID = Convert.ToInt32(reader.Rows[i]["ID"].ToString());
                selectInfo.ValueRequired = reader.Rows[i]["ValueRequired"].ToString() == "-1" ? false : true;
                selectInfo.OptionalValues = reader.Rows[i]["OptionalValues"].ToString();
                selectInfo.SelectStyle = Convert.ToInt32(reader.Rows[i]["SelectStyle"].ToString());
                selectInfo.CanAddnew = reader.Rows[i]["CanAddnew"].ToString() == "0" ? true : false;
                selectInfo.DefaultValue = reader.Rows[i]["DefaultValue"].ToString();
                selectInfo.Common = reader.Rows[i]["Common"].ToString();
            }

            //reader.Close();
            return selectInfo;
        }

        private SelectFieldDefineInfo getSelectFieldInfoByPorpertyPage(System.Web.UI.Page curPage)
        {
            // 生成一个实体类
            SelectFieldDefineInfo selectInfo = new SelectFieldDefineInfo();
            selectInfo.ValueRequired = ((CheckBox)curPage.FindControl("checkValueRequested")).Checked;
            selectInfo.CanAddnew = ((CheckBox)curPage.FindControl("checkCanAddnew")).Checked;
            selectInfo.OptionalValues = ((TextBox)curPage.FindControl("textOptionalValues")).Text;
            selectInfo.SelectStyle = Convert.ToInt32(((RadioButtonList)curPage.FindControl("radioSelectStyle")).SelectedValue);
            selectInfo.DefaultValue = ((TextBox)curPage.FindControl("textDefaultValue")).Text;
            selectInfo.Common = ((TextBox)curPage.FindControl("textFieldCommon")).Text;

            return selectInfo;
        }
        #endregion
    }
}
