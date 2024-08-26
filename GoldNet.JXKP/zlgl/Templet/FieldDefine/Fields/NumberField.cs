using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.JXKP.PowerManager;

namespace GoldNet.JXKP.Templet.BLL.Fields
{
	/// <summary>
	/// �����ֶ�
	/// </summary>
	[Serializable()]
	internal class NumberField : IFieldType
	{
		private Decimal DEFAULT_VALUE = 5211M;
		#region --˽�б���-- 
		private Field _field;
		private NumberFieldDefineInfo _numberInfo;
		#endregion

		#region --���캯��-- 
		public NumberField(Field field)
		{
			_field = field;
		}

		#endregion

		#region --�ڲ�ʵ����-- 
		[Serializable()]
		private class NumberFieldDefineInfo
		{
			public int ID;
			public bool ValueRequired;
			public Decimal MaxValue;
			public Decimal MinValue;
			public int DecimalDigits;
			public Decimal DefaultValue;
			public bool PercentStyle;
			public string Common;
		}
		#endregion

		#region IFieldType ��Ա
		public string FieldTypeName
		{
			get
			{
				return "����";
			}
		}

		public string[] FilterOperators
		{
			get
			{
				return new string[]{"����", "������", "����", "С��", "���ڵ���", "С�ڵ���"};
			}
		}

		public string[] CollectModes
		{
			get
			{
				return new string[]{"����", "���", "ƽ��ֵ", "���ֵ", "��Сֵ", "ͳ�Ʒ���"};
			}
		}

		public string ListDisplayDataName
		{
			get
			{
				return "NUMBER_" + _field.ID.ToString();
			}
		}

		public string ListDataFormatString
		{
			get
			{
				if (this.numberDefineInfo.PercentStyle)
					return "{0:P" + this.numberDefineInfo.DecimalDigits.ToString() + "}";
				else
					return "{0:N" + this.numberDefineInfo.DecimalDigits.ToString() + "}";
					// return "{0:D" + this.numberDefineInfo.DecimalDigits.ToString() + "}";
			}
		}

		public void ShowSpecialProperty(Control curCtrl)
		{
			curCtrl.Controls.Add(new LiteralControl("<table width='100%' border='0' cellpadding='0' cellspacing='0'>"));

			// ��ʾһ����¼��ҳ�棺
			// 1��˵��
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'> ˵��:<BR>&nbsp;"));
			TextBox textCommon = new TextBox();
			textCommon.ID = "textFieldCommon";
			textCommon.Rows = 3;
			textCommon.Width = new Unit("280px");
			textCommon.TextMode = TextBoxMode.MultiLine;
			curCtrl.Controls.Add(textCommon);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 2������
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>�Ƿ�Ϊ�����ֶ�:<BR>&nbsp;"));
			CheckBox checkValueRequested = new CheckBox();
			checkValueRequested.ID = "checkValueRequested";
			checkValueRequested.Text = "��";
			curCtrl.Controls.Add(checkValueRequested);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));
		
			// 3�����ֵ
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>���ֵ:<BR>&nbsp;"));
			TextBox textMaxValue = new TextBox();
			textMaxValue.ID = "textMaxValue";
			textMaxValue.TextMode = TextBoxMode.SingleLine;
			textMaxValue.MaxLength = 8;
			textMaxValue.Width = new Unit("100px");
			curCtrl.Controls.Add(textMaxValue);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 4����Сֵ
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>��Сֵ:<BR>&nbsp;"));
			TextBox textMinValue = new TextBox();
			textMinValue.ID = "textMinValue";
			textMinValue.TextMode = TextBoxMode.SingleLine;
			textMinValue.MaxLength = 8;
			textMinValue.Width = new Unit("100px");
			curCtrl.Controls.Add(textMinValue);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 5��С��λ
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>С��λ��:<BR>&nbsp;"));
			ListBox listDecimalDigits = new ListBox();
			listDecimalDigits.Rows = 1;
			listDecimalDigits.ID = "listDecimalDigits";
			listDecimalDigits.Items.Add(new ListItem("0", "0"));
			listDecimalDigits.Items.Add(new ListItem("1", "1"));
			listDecimalDigits.Items.Add(new ListItem("2", "2"));
			listDecimalDigits.Items.Add(new ListItem("3", "3"));
			listDecimalDigits.Items.Add(new ListItem("4", "4"));
			listDecimalDigits.Items.Add(new ListItem("5", "5"));
			listDecimalDigits.SelectedIndex = 0;
			listDecimalDigits.Width = new Unit("100px");
			curCtrl.Controls.Add(listDecimalDigits);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 6��Ĭ��ֵ
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>Ĭ��ֵ:<BR>&nbsp;"));
			TextBox textDefaultValue = new TextBox();
			textDefaultValue.ID = "textDefaultValue";
			textDefaultValue.TextMode = TextBoxMode.SingleLine;
			textDefaultValue.MaxLength = 50;
			textDefaultValue.Width = new Unit("100px");
			curCtrl.Controls.Add(textDefaultValue);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 7���ٷֱ���ʽ
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>�Ƿ���ʾΪ�ٷֱ�(%)��ʽ:<BR>&nbsp;"));
			CheckBox checkPercentStyle = new CheckBox();
			checkPercentStyle.ID = "checkPercentStyle";
			checkPercentStyle.Text = "�԰ٷֱ���ʾ(����: 50%)";
			curCtrl.Controls.Add(checkPercentStyle);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			curCtrl.Controls.Add(new LiteralControl("</table>"));

			if (_field != null)
			{
				NumberFieldDefineInfo numberInfo = getNumberFieldInfoByID(_field.FieldDefineID);

				// ��ʼ���ؼ�
				textCommon.Text = numberInfo.Common;
				checkValueRequested.Checked = numberInfo.ValueRequired;

				if (numberInfo.MaxValue != DEFAULT_VALUE)
					textMaxValue.Text = numberInfo.MaxValue.ToString();
				else
					textMaxValue.Text = "";

				if (numberInfo.MinValue != DEFAULT_VALUE)
					textMinValue.Text = numberInfo.MinValue.ToString();
				else
					textMinValue.Text = "";

				listDecimalDigits.SelectedValue = numberInfo.DecimalDigits.ToString();

				if (numberInfo.DefaultValue != DEFAULT_VALUE)
					textDefaultValue.Text = numberInfo.DefaultValue.ToString();
				else
					textDefaultValue.Text = "";

				checkPercentStyle.Checked = numberInfo.PercentStyle;
			}
		}

		public int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			NumberFieldDefineInfo numberInfo = this.getNumberFieldInfoByPorpertyPage(curPage);

			int fieldDefineID = insertNew(numberInfo);

			// ���Ϊ�ٷֱȣ���Ӧ��С��λ��2����������ʱ��100
			int decimalDigits = numberInfo.DecimalDigits;
			if (numberInfo.PercentStyle)
				decimalDigits += 2;

			// ����SQL�������.
			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ ADD NUMBER_" + this._field.ID.ToString() 
				+ " decimal(18, " + decimalDigits.ToString() + ") NULL ";

			return fieldDefineID;
		}

		public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			NumberFieldDefineInfo numberInfo = this.getNumberFieldInfoByPorpertyPage(curPage);

			// ���¶�������
			updateField(numberInfo, this._field.FieldDefineID);

			if (numberInfo.DecimalDigits == -1) numberInfo.DecimalDigits = 4;

			// ���Ϊ�ٷֱȣ���Ӧ��С��λ��2����������ʱ��100
			int decimalDigits = numberInfo.DecimalDigits;
			if (numberInfo.PercentStyle)
				decimalDigits += 2;

			// ����SQL�������
			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ modify NUMBER_" + this._field.ID.ToString() 
				+ " decimal(20, " + decimalDigits.ToString() + ") ";
		}

		public void ShowInputControl(Control curCtrl,bool pass)
		{
            Goldnet.Ext.Web.NumberField textInput = new Goldnet.Ext.Web.NumberField();
            textInput.DecimalPrecision = this.numberDefineInfo.DecimalDigits;
            textInput.ID = "NUMBERINPUT_" + this._field.ID.ToString();
            textInput.MaxLength = 12;
            textInput.Width = new Unit(150);
            if (this.numberDefineInfo.DefaultValue != DEFAULT_VALUE) textInput.Text = this.numberDefineInfo.DefaultValue.ToString();
            curCtrl.Controls.Add(textInput);
            if (this.numberDefineInfo.ValueRequired)
            {
                Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
                lb.Text = "*";
                lb.ForeColor = System.Drawing.Color.Red;
                curCtrl.Controls.Add(lb);
            }
			if (this.numberDefineInfo.PercentStyle) curCtrl.Controls.Add(new LiteralControl("%"));
			if (this.numberDefineInfo.Common != "")
				curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(this.numberDefineInfo.Common, 200).Replace("\n", "<br>") + "</span>"));
		}
        //���·���
		public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id)
		{
            NumberFieldDefineInfo numberInfo = this.numberDefineInfo;
			string sql = string.Format("UPDATE {0}." + tabName + " SET NUMBER_" + this._field.ID.ToString() + " = ? WHERE (ID = ?)",DataUser.ZLGL);
            string newValue = ((Goldnet.Ext.Web.NumberField)curPage.FindControl("NUMBERINPUT_" + _field.ID.ToString())).Text;
            if (numberInfo.ValueRequired && newValue == string.Empty)
            {
                throw new SaveRecordDataIsNullException(this._field.FieldName);
            }
            else if (newValue == string.Empty)
            {
                newValue = "0";
            }
            else if (decimal.Parse(newValue) > numberInfo.MaxValue)
            {
                throw new NumberFildMaxException(this._field.FieldName, numberInfo.MaxValue.ToString());
            }
            else if (decimal.Parse(newValue) < numberInfo.MinValue)
            {
                throw new NumberFildMinException(this._field.FieldName, numberInfo.MinValue.ToString());
            }
            else
            {
                Object newVal;
                // ���Ϊ�ٷֱȣ�����100����
                if (this.numberDefineInfo.PercentStyle)
                    newVal = Convert.ToDecimal(newValue) / 100M;
                else
                    newVal = Convert.ToDecimal(newValue);
                OleDbParameter[] cmdParms = new OleDbParameter[]{
																new OleDbParameter("newValue",newVal),
																new OleDbParameter("id",recId)												
															};

                OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,sql, cmdParms);
                string str;
                if (this._field.FieldName == "���ֵ")
                {
                    str = string.Format("update {0}.QUALITY_ERROR_LIST set NUMBERS_1=" + newVal + " where table_name='{1}' and table_id='{2}'", DataUser.ZLGL, tabName, recId);
                }
                else
                {
                    str = string.Format("update {0}.QUALITY_ERROR_LIST set NUMBERS=" + newVal + " where table_name='{1}' and table_id='{2}'", DataUser.ZLGL, tabName, recId);
                }
                OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, str, new OleDbParameter[0] { });
            }
		}

		public void ShowViewData(string tabName, int recID, Control curCtrl)
		{
			string sql = string.Format("SELECT NUMBER_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			string val = Convert.ToString(OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("recid", recID)));

			if (val == null || val == "")
			{
				curCtrl.Controls.Add(new LiteralControl(val));
			}
			else
			{
				try
				{
					if (this.numberDefineInfo.PercentStyle)
					{
						Decimal vald = Convert.ToDecimal(val);
						curCtrl.Controls.Add(new LiteralControl(vald.ToString("P" + this.numberDefineInfo.DecimalDigits.ToString())));
					}
					else
					{
						curCtrl.Controls.Add(new LiteralControl(val));
					}
				}
				catch(Exception ex)
				{
					throw new Exception("����ת��String��һ����ֵ���͵��ֶ�ʱ���������ֶ�����Ϊ��"
						+ this._field.FieldName
						+ "����ϸ�Ĵ�����ϢΪ��" + ex.Message, ex);
				}
			}
		}
        public void ShowEditControl(string tabName, int recID, Control curCtrl, bool pass)
		{
			string sql = string.Format("SELECT NUMBER_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			object obj = OracleOledbBase.ExecuteScalar(sql, new OleDbParameter("recid", recID));

			string val = Convert.ToString(obj);
            Goldnet.Ext.Web.NumberField textInput = new Goldnet.Ext.Web.NumberField();
            textInput.ID = "NUMBERINPUT_" + this._field.ID.ToString();
            textInput.MaxLength = 12;
            textInput.Width = new Unit(150);
            if (this.numberDefineInfo.DefaultValue != DEFAULT_VALUE) textInput.Text = this.numberDefineInfo.DefaultValue.ToString();
            if (val != "")
            {
                if (this.numberDefineInfo.PercentStyle)
                    textInput.Text = Convert.ToString((Convert.ToDecimal(val) * 100M));
                else
                    textInput.Text = val;
            }
            if (pass)
            {
                textInput.ReadOnly = true;
            }
            curCtrl.Controls.Add(textInput);
            if (this.numberDefineInfo.ValueRequired)
            {
                Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
                lb.Text = "*";
                lb.ForeColor = System.Drawing.Color.Red;
                curCtrl.Controls.Add(lb);
            }
            if (this.numberDefineInfo.PercentStyle) curCtrl.Controls.Add(new LiteralControl("%"));
            if (this.numberDefineInfo.Common != "")
                curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(this.numberDefineInfo.Common, 200).Replace("\n", "<br>") + "</span>"));
		}
		public string GetFilter(string compOperator, string values)
		{
			string result;
			// "����", "������", "����", "С��", "���ڵ���", "С�ڵ���", "��...��"
			switch(compOperator)       
			{
				case "����":
					result = this.ListDisplayDataName + "= '" + values.Replace("'", "") + "'";
					break;
				case "������":
					result = this.ListDisplayDataName + "<> '" + values.Replace("'", "") + "'";
					break;
				case "����":
					result = this.ListDisplayDataName + "> '" + values.Replace("'", "") + "'";
					break;
				case "С��":
					result = this.ListDisplayDataName + "< '" + values.Replace("'", "") + "'";
					break;
				case "���ڵ���":
					result = this.ListDisplayDataName + ">= '" + values.Replace("'", "") + "'";
					break;
				case "С�ڵ���":
					result = this.ListDisplayDataName + "<= '" + values.Replace("'", "") + "'";
					break;
				default:
					result = "";
					break;
			}

			return result;
		}

		public string ListCollectComputerString(string collectMode)
		{
			// collectMode : "����", "���", "ƽ��ֵ", "���ֵ", "��Сֵ", "ͳ�Ʒ���"
			string result;
			switch(collectMode)       
			{
				case "����":
					result = "Count(" + this.ListDisplayDataName + ")";
					break;
				case "���":
					result = "Sum(" + this.ListDisplayDataName + ")";
					break;
				case "ƽ��ֵ":
					result = "Avg(" + this.ListDisplayDataName + ")";
					break;
				case "���ֵ":
					result = "Max(" + this.ListDisplayDataName + ")";
					break;
				case "��Сֵ":
					result = "Min(" + this.ListDisplayDataName + ")";
					break;
				case "ͳ�Ʒ���":
					result = "Var(" + this.ListDisplayDataName + ")";
					break;
				default:
					result = "";
					break;
			}
			return result;
		}
		#endregion

		#region --˽������-- 
		private NumberFieldDefineInfo numberDefineInfo
		{
			get
			{
				if (this._numberInfo == null)
					this._numberInfo = this.getNumberFieldInfoByID(_field.FieldDefineID);
				return this._numberInfo;
			}
		}
		#endregion

		#region --˽�з���-- 
		private int insertNew(NumberFieldDefineInfo numberInfo)
		{
			string sql = string.Format("INSERT INTO {0}.T_NumberFieldTypeDefineDict (id,ValueRequired, MinValue, MaxValue, DecimalDigits, DefaultValue, PercentStyle,   Common) VALUES (?,?,?,?,?,?,?,?)",DataUser.ZLGL);
			int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_NumberFieldTypeDefineDict",DataUser.ZLGL));

			OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("id", id),
				new OleDbParameter("valueRequesed", numberInfo.ValueRequired==true?0:-1),
				new OleDbParameter("minValue", numberInfo.MinValue),
				new OleDbParameter("maxValue", numberInfo.MaxValue),
				new OleDbParameter("decimalDigits", numberInfo.DecimalDigits),
				new OleDbParameter("defaultValue", numberInfo.DefaultValue),
				new OleDbParameter("percentStyle", numberInfo.PercentStyle==true?0:-1),
                new OleDbParameter("common", CleanString.InputText(numberInfo.Common,100))
				);
			return id;
		}

		private void updateField(NumberFieldDefineInfo numberInfo, int fieldDefineID)
		{
			string str=string.Format("select DecimalDigits from {0}.T_NumberFieldTypeDefineDict where id="+fieldDefineID,DataUser.ZLGL);
			int digits=Convert.ToInt32(OracleOledbBase.ExecuteScalar(str).ToString());
			if(numberInfo.DecimalDigits<digits)
				numberInfo.DecimalDigits=digits;
			string sql; 
			sql = string.Format("UPDATE {0}.T_NumberFieldTypeDefineDict SET ValueRequired = ?, MinValue = ?, MaxValue = ?, DecimalDigits = ?, DefaultValue = ?, PercentStyle =?, Common = ?  WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery( sql, 
				new OleDbParameter("valueRequesed", numberInfo.ValueRequired==true?0:-1),
				new OleDbParameter("minValue", numberInfo.MinValue),
				new OleDbParameter("maxValue", numberInfo.MaxValue),
				new OleDbParameter("decimalDigits", numberInfo.DecimalDigits),
				new OleDbParameter("defaultValue", numberInfo.DefaultValue),
				new OleDbParameter("percentStyle", numberInfo.PercentStyle==true?0:-1),
                new OleDbParameter("common", CleanString.InputText(numberInfo.Common,100)),
				new OleDbParameter("fieldDefineID", fieldDefineID)
				);
		}

		private NumberFieldDefineInfo getNumberFieldInfoByID(int fieldDefineID)
		{
			string sql = string.Format("SELECT ID, ValueRequired, MinValue, MaxValue, DecimalDigits, DefaultValue, PercentStyle, Common FROM {0}.T_NumberFieldTypeDefineDict WHERE (ID = ?)",DataUser.ZLGL);

			NumberFieldDefineInfo numberFieldInfo = null;

			System.Data.DataTable reader = OracleOledbBase.ExecuteDataSet(sql, 
				new OleDbParameter("fieldDefineID", fieldDefineID)
				).Tables[0];

			for (int i=0;i<reader.Rows.Count;i++)
			{
				numberFieldInfo = new NumberFieldDefineInfo();
				numberFieldInfo.ID = Convert.ToInt32(reader.Rows[i]["ID"].ToString());
				numberFieldInfo.ValueRequired = reader.Rows[i]["ValueRequired"].ToString()=="-1"?false:true;
				numberFieldInfo.MinValue = Convert.ToDecimal(reader.Rows[i]["MinValue"].ToString());
				numberFieldInfo.MaxValue = Convert.ToDecimal(reader.Rows[i]["MaxValue"].ToString());
				numberFieldInfo.DecimalDigits = Convert.ToInt32(reader.Rows[i]["DecimalDigits"].ToString());
				numberFieldInfo.DefaultValue = Convert.ToDecimal(reader.Rows[i]["DefaultValue"].ToString());
				numberFieldInfo.PercentStyle = reader.Rows[i]["PercentStyle"].ToString()=="0"?true:false;
				numberFieldInfo.Common = reader.Rows[i]["Common"].ToString();
			}

//			reader.Close();
			return numberFieldInfo;
		}

		private NumberFieldDefineInfo getNumberFieldInfoByPorpertyPage(System.Web.UI.Page curPage)
		{
			// ����һ��ʵ����
			NumberFieldDefineInfo numberInfo = new NumberFieldDefineInfo();

			// �õ�����ѡ��
			numberInfo.ValueRequired = ((CheckBox)curPage.FindControl("checkValueRequested")).Checked;

			// �õ��Ƿ񰴰ٷֱ���ʾ
			numberInfo.PercentStyle = ((CheckBox)curPage.FindControl("checkPercentStyle")).Checked;

			// �õ����ֵ
			string maxValue = ((TextBox)curPage.FindControl("textMaxValue")).Text;
            if (maxValue != "")
			{
				try
				{
					numberInfo.MaxValue = Convert.ToDecimal(maxValue);
				}
				catch(Exception)
				{
					numberInfo.MaxValue = 1000;
				}
			}
			else
				numberInfo.MaxValue = 1000;

			// �õ���Сֵ
			string minValue = ((TextBox)curPage.FindControl("textMinValue")).Text;
			if (minValue != "")
			{
				try
				{
					numberInfo.MinValue = Convert.ToDecimal(minValue);
				}
				catch(Exception)
				{
					numberInfo.MinValue = -1000;
				}
			}
			else
				numberInfo.MinValue = -1000;

			// �õ�Ĭ��ֵ
			string defaultValue = ((TextBox)curPage.FindControl("textDefaultValue")).Text;
			if (defaultValue != "")
			{
				try
				{
					numberInfo.DefaultValue = Convert.ToDecimal(defaultValue);
				}
				catch(Exception)
				{
					numberInfo.DefaultValue = 0;
				}
			}
			else
				numberInfo.DefaultValue = 0;

			// �õ�С��λ
			try
			{
				numberInfo.DecimalDigits = Convert.ToInt32(((ListBox)curPage.FindControl("listDecimalDigits")).SelectedValue);
			}
			catch(Exception)
			{
				numberInfo.DecimalDigits = 0;
			}

			// �õ���ע
			numberInfo.Common = CleanString.InputText(((TextBox)curPage.FindControl("textFieldCommon")).Text, 200);

			return numberInfo;
		}
		#endregion

	}
}
