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
    /// ��ѡ�ֶ� 
    /// </summary>
    [Serializable()]
    internal class SelectField : IFieldType
    {
        #region --˽�б���--
        private Field _field;
        private SelectFieldDefineInfo _selectInfo;
        #endregion

        #region --���캯��--
        public SelectField(Field field)
        {
            _field = field;
        }
        #endregion

        #region --�ڲ�ʵ����--
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

        #region IFieldType ��Ա

        public string FieldTypeName
        {
            get
            {
                return "ѡ��(���б���ѡ��)";
            }
        }

        public string[] FilterOperators
        {
            get
            {
                return new string[] { "����", "����" };
            }
        }

        public string[] CollectModes
        {
            get
            {
                return new string[] { "����" };
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

            // 1��˵�� Common
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'> ˵��:<BR>&nbsp;"));
            TextBox textCommon = new TextBox();
            textCommon.ID = "textFieldCommon";
            textCommon.Rows = 3;
            textCommon.Width = new Unit("280px");
            textCommon.TextMode = TextBoxMode.MultiLine;
            curCtrl.Controls.Add(textCommon);
            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

            // 2������ ValueRequired
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>�Ƿ�Ϊ�����ֶ�:<BR>&nbsp;"));
            CheckBox checkValueRequested = new CheckBox();
            checkValueRequested.ID = "checkValueRequested";
            checkValueRequested.Text = "��";
            curCtrl.Controls.Add(checkValueRequested);
            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

            // 3����ѡֵ OptionalValues
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>���м���ÿ��ѡ��:<BR>&nbsp;"));
            TextBox textOptionalValues = new TextBox();
            textOptionalValues.ID = "textOptionalValues";
            textOptionalValues.Rows = 5;
            textOptionalValues.Width = new Unit("280px");
            textOptionalValues.TextMode = TextBoxMode.MultiLine;
            curCtrl.Controls.Add(textOptionalValues);
            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));



            // 4��ѡ����ʽ SelectStyle
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>��ʾѡ��ʹ��:"));
            RadioButtonList radioSelectStyle = new RadioButtonList();
            radioSelectStyle.ID = "radioSelectStyle";
            radioSelectStyle.RepeatDirection = RepeatDirection.Vertical;
            radioSelectStyle.Items.Add(new System.Web.UI.WebControls.ListItem("�����˵�", "0"));
            radioSelectStyle.Items.Add(new System.Web.UI.WebControls.ListItem("��ѡ��ť", "1"));
            radioSelectStyle.Items.Add(new System.Web.UI.WebControls.ListItem("��ѡ��(�������ѡ��)", "2"));
            radioSelectStyle.SelectedIndex = 0;
            radioSelectStyle.CellPadding = 0;
            radioSelectStyle.CellSpacing = 2;
            curCtrl.Controls.Add(radioSelectStyle);
            curCtrl.Controls.Add(new LiteralControl("<table border=0 cellpadding='0' cellspacing='0'><tr><td><FONT COLOR='#808080'>&nbsp;&nbsp;ע��Ŀǰ��ѡ���޷�ʵ�ֱ�����֤��</FONT></td></tr></table>"));
            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));




            // 5���Ƿ���������ֵ CanAddnew
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>������䡱ѡ��:<BR>&nbsp;"));
            CheckBox checkCanAddnew = new CheckBox();
            checkCanAddnew.ID = "checkCanAddnew";
            checkCanAddnew.Text = "��";
            curCtrl.Controls.Add(checkCanAddnew);

            curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

            // 6��Ĭ��ֵ DefaultValue
            curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>Ĭ��ֵ:<BR>&nbsp;"));
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
                // ��ʼ���ؼ�
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

            // ����SQL�ֶζ������.
            fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ ADD SELECT_" + this._field.ID.ToString()
                + " VARCHAR(300) NULL ";

            return fieldDefineID;
        }

        public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
        {
            SelectFieldDefineInfo selectInfo = this.getSelectFieldInfoByPorpertyPage(curPage);

            // �������Զ���
            this.updateField(selectInfo, this._field.FieldDefineID);

            // ����SQL�������
            fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ modify ( SELECT_" + this._field.ID.ToString()
                + " VARCHAR2(300) )";
        }

        public void ShowInputControl(Control curCtrl,bool pass)
        {
            // ��������
            RadioButton radioValueInTextBoxCtrl = null, radioValueInSelectCtrl = null;
            ListControl selectControl = null;
            TextBox textNewValue = null;
            RequiredFieldValidator requiredForText = null, requiredForSel = null;

            // ¼��ؼ���ʾ
            bool canAddnew = this.selectDefineInfo.CanAddnew;

            // ������������ֵ������һ����ֵ
            if (canAddnew)
            {
                // ��ʾһ��radiobutton : ���б���ѡ��
                radioValueInSelectCtrl = new RadioButton();
                radioValueInSelectCtrl.ID = "SEL_VAL_IN_SEL_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                radioValueInSelectCtrl.Checked = true;
                radioValueInSelectCtrl.GroupName = "valueIn";
                radioValueInSelectCtrl.Text = "�ӿ�ѡ����ѡ��: &nbsp;";
                curCtrl.Controls.Add(radioValueInSelectCtrl);
            }

            // ��ʾѡ��ؼ�
            switch (this.selectDefineInfo.SelectStyle)
            {
                case 1:
                    // ��ѡ
                    selectControl = new RadioButtonList();
                    ((RadioButtonList)selectControl).RepeatDirection = RepeatDirection.Horizontal;
                    ((RadioButtonList)selectControl).RepeatColumns = 4;
                    ((RadioButtonList)selectControl).CellSpacing = 3;
                    break;
                case 2:
                    // ��ѡ��
                    selectControl = new CheckBoxList();
                    ((CheckBoxList)selectControl).RepeatDirection = RepeatDirection.Horizontal;
                    ((CheckBoxList)selectControl).RepeatColumns = 4;
                    ((CheckBoxList)selectControl).CellSpacing = 3;
                    break;
                default:
                    // �����б�
                    selectControl = new DropDownList();
                    break;
            }

            selectControl.ID = "SELECT_SELINPUT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());

            // ������б����κ�ѡ��ʱ���б�CheckBox��ѡ�С�
            if (canAddnew)
                selectControl.Attributes["onclick"] = "javascript:document.all['" + radioValueInSelectCtrl.ClientID + "'].checked=true;";

            // ������ѡ�б���
            string[] values = this.selectDefineInfo.OptionalValues.Split('\n');
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != "")
                    selectControl.Items.Add(new System.Web.UI.WebControls.ListItem(values[i].Replace("\r", ""), values[i].Replace("\r", "")));
            }

            // ����б�ؼ�

            if (this.selectDefineInfo.SelectStyle != 1)
                curCtrl.Controls.Add(selectControl);

            // �����֤�ؼ�: �����Ҫ��֤�Ƿ�Ϊ���Ҳ��Ǹ�ѡ��
            if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
            {
                requiredForSel = new RequiredFieldValidator();
                requiredForSel.ControlToValidate = selectControl.ID;
                requiredForSel.Display = ValidatorDisplay.Dynamic;
                requiredForSel.ID = "requiredForSel" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                requiredForSel.ErrorMessage = "\"" + this._field.FieldName + "\"Ϊ�����ֶΣ�����Ϊ�գ�������ָ��һ��ѡ�";
                requiredForSel.Text = "*";
                curCtrl.Controls.Add(requiredForSel);
            }

            //** ���У��ǵĻ�����ӣ�
            if (this.selectDefineInfo.SelectStyle == 1) curCtrl.Controls.Add(selectControl);

            if (this.selectDefineInfo.SelectStyle == 0) curCtrl.Controls.Add(new LiteralControl("<br>"));

            // ������������ֵ���ٶ���һ��RadioButton
            if (canAddnew)
            {
                // ��ʾһ��radiobutton : ���б���ѡ��
                radioValueInTextBoxCtrl = new RadioButton();
                radioValueInTextBoxCtrl.ID = "SEL_VAL_IN_TEXT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                radioValueInTextBoxCtrl.GroupName = "valueIn";
                radioValueInTextBoxCtrl.Text = "��дһ����ֵ: ";
                curCtrl.Controls.Add(radioValueInTextBoxCtrl);
                curCtrl.Controls.Add(new LiteralControl("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));

                textNewValue = new TextBox();
                textNewValue.ID = "SELECT_TEXTINPUT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                textNewValue.MaxLength = 50;
                textNewValue.Width = new Unit("240px");
                // ���ı����ý���ʱ����ѡ��ť��ѡ��
                textNewValue.Attributes["onfocus"] = "javascript:document.all['" + radioValueInTextBoxCtrl.ClientID + "'].checked=true;";
                curCtrl.Controls.Add(textNewValue);

                // �����֤�ؼ�: �����Ҫ��֤�Ƿ�Ϊ���Ҳ��Ǹ�ѡ��
                if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
                {
                    requiredForText = new RequiredFieldValidator();
                    requiredForText.ControlToValidate = textNewValue.ID;
                    requiredForText.ID = "requiredForText" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                    requiredForText.ErrorMessage = "\"" + this._field.FieldName + "\"Ϊ�����ֶΣ�����Ϊ�գ�����д���ݣ�";
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

            // ����п������ֵѡ������ж��û�ѡ�������ֵ���Ǵ��б���ѡȡ��
            if (this.selectDefineInfo.CanAddnew)
            {
                RadioButton radioValueInTextBoxCtrl = (RadioButton)curPage.FindControl("SEL_VAL_IN_TEXT_" + Encrypt.EncryptMyStr("iloveyou", _field.ID.ToString()));
                if (radioValueInTextBoxCtrl.Checked)
                {

                    // �û�ѡ��¼����ֵ����TextBox�л�ȡֵ
                    TextBox textNewValue
                        = (TextBox)curPage.FindControl("SELECT_TEXTINPUT_" + Encrypt.EncryptMyStr("iloveyou", _field.ID.ToString()));
                    valStr = textNewValue.Text;
                }
                else
                {
                    // �û�ѡ���˴��б���ѡȡ�����ж��Ƿ��ѡ
                    ListControl selectControl
                        = (ListControl)curPage.FindControl("SELECT_SELINPUT_" + Encrypt.EncryptMyStr("iloveyou", _field.ID.ToString()));

                    if (this.selectDefineInfo.SelectStyle == 2)
                    {
                        // ��ѡ
                        foreach (System.Web.UI.WebControls.ListItem item in selectControl.Items)
                        {
                            if (item.Selected) valStr += item.Value + ";";
                        }
                    }
                    else
                    {
                        // ��ѡ
                        valStr = selectControl.SelectedValue;
                    }
                }
            }
            else
            {
                // �������ֵѡ����б��л�ȡֵ
                ListControl selectControl
                    = (ListControl)curPage.FindControl("SELECT_SELINPUT_" + Encrypt.EncryptMyStr("iloveyou", _field.ID.ToString()));

                if (this.selectDefineInfo.SelectStyle == 2)
                {
                    // ��ѡ
                    foreach (System.Web.UI.WebControls.ListItem item in selectControl.Items)
                    {
                        if (item.Selected) valStr += item.Value + ";";
                    }
                }
                else
                {
                    // ��ѡ
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

                // ����ַ�����������
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
            // ��������
            RadioButton radioValueInTextBoxCtrl = null, radioValueInSelectCtrl = null;
            ListControl selectControl = null;
            TextBox textNewValue = null;
            RequiredFieldValidator requiredForText = null, requiredForSel = null;

            

            string sql = string.Format("SELECT SELECT_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)", DataUser.ZLGL);

            string val = Convert.ToString(OracleOledbBase.ExecuteScalar(sql,
                new OleDbParameter("recid", recID)));

            // ¼��ؼ���ʾ
            bool canAddnew = this.selectDefineInfo.CanAddnew;

            // ������������ֵ������һ����ֵ
            if (canAddnew)
            {
                // ��ʾһ��radiobutton : ���б���ѡ��
                radioValueInSelectCtrl = new RadioButton();
                radioValueInSelectCtrl.ID = "SEL_VAL_IN_SEL_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                radioValueInSelectCtrl.Checked = true;
                radioValueInSelectCtrl.GroupName = "valueIn";
                radioValueInSelectCtrl.Text = "�ӿ�ѡ����ѡ��: &nbsp;";
                curCtrl.Controls.Add(radioValueInSelectCtrl);
                if (pass)
                {
                    radioValueInSelectCtrl.Enabled = false;
                }
            }

            // ��ʾѡ��ؼ�
            switch (this.selectDefineInfo.SelectStyle)
            {
                case 1:
                    // ��ѡ
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
                    // ��ѡ��
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
                    // �����б�
                    selectControl = new DropDownList();
                    if (pass)
                    {
                        selectControl.Enabled = false;
                    }
                    break;
            }

            selectControl.ID = "SELECT_SELINPUT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());

            // ������б����κ�ѡ��ʱ���б�CheckBox��ѡ�С�
            if (canAddnew)
                selectControl.Attributes["onclick"] = "javascript:document.all['" + radioValueInSelectCtrl.ClientID + "'].checked=true;";

            // ������ѡ�б���
            string[] values = this.selectDefineInfo.OptionalValues.Split('\n');
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != "")
                    selectControl.Items.Add(new System.Web.UI.WebControls.ListItem(values[i].Replace("\r", ""), values[i].Replace("\r", "")));
            }

            // ����б�ؼ�

            if (this.selectDefineInfo.SelectStyle != 1)
                curCtrl.Controls.Add(selectControl);

            // �����֤�ؼ�: �����Ҫ��֤�Ƿ�Ϊ���Ҳ��Ǹ�ѡ��
            if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
            {
                requiredForSel = new RequiredFieldValidator();
                requiredForSel.ControlToValidate = selectControl.ID;
                requiredForSel.Display = ValidatorDisplay.Dynamic;
                requiredForSel.ID = "requiredForSel" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                requiredForSel.ErrorMessage = "\"" + this._field.FieldName + "\"Ϊ�����ֶΣ�����Ϊ�գ�������ָ��һ��ѡ�";
                requiredForSel.Text = "*";
                curCtrl.Controls.Add(requiredForSel);
            }


            if (this.selectDefineInfo.SelectStyle == 1) curCtrl.Controls.Add(selectControl);

            if (this.selectDefineInfo.SelectStyle == 0) curCtrl.Controls.Add(new LiteralControl("<br>"));

            // ������������ֵ���ٶ���һ��RadioButton
            if (canAddnew)
            {
                // ��ʾһ��radiobutton : ���б���ѡ��
                radioValueInTextBoxCtrl = new RadioButton();
                radioValueInTextBoxCtrl.ID = "SEL_VAL_IN_TEXT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                radioValueInTextBoxCtrl.GroupName = "valueIn";
                radioValueInTextBoxCtrl.Text = "��дһ����ֵ: ";
                curCtrl.Controls.Add(radioValueInTextBoxCtrl);
                curCtrl.Controls.Add(new LiteralControl("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));

                textNewValue = new TextBox();
                textNewValue.ID = "SELECT_TEXTINPUT_" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                textNewValue.MaxLength = 50;
                textNewValue.Width = new Unit("240px");
                // ���ı����ý���ʱ����ѡ��ť��ѡ��
                textNewValue.Attributes["onfocus"] = "javascript:document.all['" + radioValueInTextBoxCtrl.ClientID + "'].checked=true;";
                curCtrl.Controls.Add(textNewValue);

                // �����֤�ؼ�: �����Ҫ��֤�Ƿ�Ϊ���Ҳ��Ǹ�ѡ��
                if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
                {
                    requiredForText = new RequiredFieldValidator();
                    requiredForText.ControlToValidate = textNewValue.ID;
                    requiredForText.ID = "requiredForText" + Encrypt.EncryptMyStr("iloveyou", this._field.ID.ToString());
                    requiredForText.ErrorMessage = "\"" + this._field.FieldName + "\"Ϊ�����ֶΣ�����Ϊ�գ�����д���ݣ�";
                    requiredForText.Text = "*";
                    curCtrl.Controls.Add(requiredForText);

                    radioValueInTextBoxCtrl.Attributes["onclick"] = @"javascript:ValidatorEnable(document.all['" + requiredForSel.ID + "'], false);ValidatorEnable(document.all['" + requiredForText.ID + "'], true);";

                    radioValueInSelectCtrl.Attributes["onclick"] = @"javascript:ValidatorEnable(document.all['" + requiredForText.ID + "'], false);ValidatorEnable(document.all['" + requiredForSel.ID + "'], true);";
                }
            }

            // ��ʼ��ֵ
            bool isSel = false; // ֵ�Ƿ��Ǵӱ�ѡ��ѡ���
            if (this.selectDefineInfo.SelectStyle == 2)
            {
                // ��ѡ
                foreach (System.Web.UI.WebControls.ListItem item in selectControl.Items)
                {
                    if (val.IndexOf(item.Value) >= 0)
                    {
                        // ����κ�һ����ѡֵ��ֵ�б��У���isSel = true;
                        item.Selected = true;
                        isSel = true;
                        if (canAddnew)
                            radioValueInSelectCtrl.Checked = true;
                    }
                }
                if (!isSel && canAddnew)
                {
                    // ������Ǵӱ�ѡ��ѡ���ֵ��д��TextBox��
                    textNewValue.Text = val;
                    radioValueInTextBoxCtrl.Checked = true;

                    if (this.selectDefineInfo.ValueRequired && this.selectDefineInfo.SelectStyle != 2)
                    {
                        requiredForSel.Enabled = false; // �б���֤�ؼ�
                        requiredForText.Enabled = true; // �ı���֤�ؼ�
                    }
                }
            }
            else
            {
                // ��ѡ
                if (selectControl.Items.Contains(new System.Web.UI.WebControls.ListItem(val, val)))
                {
                    // ���ѡ���б��а���ֵ
                    selectControl.SelectedValue = val;
                    if (canAddnew) radioValueInSelectCtrl.Checked = true;

                    if (this.selectDefineInfo.ValueRequired)
                    {
                        requiredForSel.Enabled = true; // �б���֤�ؼ�

                        if (canAddnew)
                            requiredForText.Enabled = false; // �ı���֤�ؼ�
                    }
                }
                else if (canAddnew)
                {
                    // ����,��TextBox����ʾֵ
                    textNewValue.Text = val;
                    radioValueInTextBoxCtrl.Checked = true;

                    if (this.selectDefineInfo.ValueRequired)
                    {
                        requiredForSel.Enabled = false; // �б���֤�ؼ�
                        requiredForText.Enabled = true; // �ı���֤�ؼ�
                    }
                }
            }
            
        }

        public string GetFilter(string compOperator, string values)
        {
            // FilterOperators "����", "����"
            string result;
            switch (compOperator)
            {
                case "����":
                    result = this.ListDisplayDataName + "= '" + values.Replace("'", "") + "'";
                    break;
                case "����":
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
            // collectMode : "����"
            string result;
            switch (collectMode)
            {
                case "����":
                    result = "count(" + this.ListDisplayDataName + ")";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        #endregion

        #region --˽������--
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

        #region --˽�з���--
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
            // ����һ��ʵ����
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
