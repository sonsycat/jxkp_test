using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.JXKP.PowerManager;

namespace GoldNet.JXKP.Templet.BLL.Fields
{
	/// <summary>
	/// �ı��ֶ� 
	/// </summary>
	[Serializable()]
	internal class CharField : IFieldType
	{
		#region --˽�б���-- 
		private Field _field;
		private CharFieldDefineInfo _charInfo;
		#endregion

		#region --���캯��-- 
		public CharField(Field field)
		{
			_field = field;
		}

		#endregion

		#region --�ڲ�ʵ����-- 
		[Serializable()]
		private class CharFieldDefineInfo
		{
			public int ID;
			public bool ValueRequired;
			public bool MultiLine;
			public int DisplayLine;
			public int MaxLen;
			public string Common;
			public string DefaultValue;
		}
		#endregion

		#region IFieldType ��Ա
		public string FieldTypeName
		{
			get
			{
				return "�ı�";
			}
		}

		public string[] FilterOperators
		{
			get
			{
				return new string[]{"����", "����"};
			}
		}

		public string[] CollectModes
		{
			get
			{
				return new string[]{"����"};
			}
		}

		public string ListDisplayDataName
		{
			get
			{
				return "CHAR_" + _field.ID.ToString();
			}
		}

		public string ListDataFormatString
		{
			get
			{
				return string.Empty;
			}
		}

		public void ShowSpecialProperty(System.Web.UI.Control curCtrl)
		{
			curCtrl.Controls.Add(new LiteralControl("<table width='100%' border='0' cellpadding='0' cellspacing='0'>"));

			// ���������Ϊ0����ʾһ����¼��ҳ�棺
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

			// 3���Ƿ�Ϊ�����ı�
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>�Ƿ�Ϊ�����ı�:<BR>&nbsp;"));
			CheckBox checkMultiLine = new CheckBox();
			checkMultiLine.ID = "checkMultiLine";
			checkMultiLine.Text = "��";
			curCtrl.Controls.Add(checkMultiLine);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 4���ı��������
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>�ı�������:<BR>&nbsp;"));
			TextBox textLineNum = new TextBox();
			textLineNum.ID = "textLineNum";
			textLineNum.TextMode = TextBoxMode.SingleLine;
			textLineNum.MaxLength = 2;
			textLineNum.Width = new Unit("100px");
			curCtrl.Controls.Add(textLineNum);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 5����󳤶�
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>�ı���󳤶�:<BR>&nbsp;"));
			TextBox textMaxlen = new TextBox();
			textMaxlen.ID = "textMaxlen";
			textMaxlen.TextMode = TextBoxMode.SingleLine;
			textMaxlen.MaxLength = 4;
			textMaxlen.Width = new Unit("100px");
			curCtrl.Controls.Add(textMaxlen);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 6��Ĭ��ֵ
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>Ĭ��ֵ:<BR>&nbsp;"));
			TextBox textDefaultValue = new TextBox();
			textDefaultValue.ID = "textDefaultValue";
			textDefaultValue.TextMode = TextBoxMode.SingleLine;
			textDefaultValue.MaxLength = 50;
			textDefaultValue.Width = new Unit("280px");
			curCtrl.Controls.Add(textDefaultValue);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			curCtrl.Controls.Add(new LiteralControl("</table>"));

			if (_field != null)
			{
				CharFieldDefineInfo charInfo = this.charDefineInfo;

				// ��ʼ���ؼ�
				textCommon.Text = charInfo.Common;
				checkValueRequested.Checked = charInfo.ValueRequired;
				checkMultiLine.Checked = charInfo.MultiLine;
				textLineNum.Text = charInfo.DisplayLine.ToString();
				textMaxlen.Text = charInfo.MaxLen.ToString();
				textDefaultValue.Text = charInfo.DefaultValue;
			}
		}

		public int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			CharFieldDefineInfo charInfo = this.getCharFieldInfoByPorpertyPage(curPage);

			int fieldDefineID = insertNew(charInfo);

			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ ADD CHAR_" + this._field.ID.ToString() 
				+ " VARCHAR2(" + charInfo.MaxLen + ") ";

			return fieldDefineID;
		}
		
		public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			CharFieldDefineInfo charInfo = this.getCharFieldInfoByPorpertyPage(curPage);

			// ���¶�������
			updateField(charInfo, this._field.FieldDefineID);
			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ modify  CHAR_" + this._field.ID.ToString() 
				+ " VARCHAR2(" + charInfo.MaxLen + ") ";
		}

		public void ShowInputControl(System.Web.UI.Control curCtrl,bool pass)
		{
			CharFieldDefineInfo charInfo = this.charDefineInfo;

			TextBox textInput = new TextBox();
			textInput.ID = "CHARINPUT_" + this._field.ID.ToString();
			textInput.MaxLength = charInfo.MaxLen;
			textInput.Text = charInfo.DefaultValue;
			textInput.Width = new Unit("280px");
            if (pass)
            {
                textInput.ReadOnly = true;
            }
			if (charInfo.MultiLine)
			{
				textInput.TextMode = TextBoxMode.MultiLine;
				textInput.Rows = charInfo.DisplayLine;
			}
			else
				textInput.TextMode = TextBoxMode.SingleLine;

			curCtrl.Controls.Add(textInput);
            if (charInfo.ValueRequired)
            {
                Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
                lb.Text="*";
                lb.ForeColor = System.Drawing.Color.Red;
                curCtrl.Controls.Add(lb);
            }
			if (charInfo.Common != "")
				curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(charInfo.Common, 500).Replace("\n", "<br>") + "</span>"));
		}
        //��������
		public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id)
		{
            CharFieldDefineInfo charInfo = this.charDefineInfo;
			string sql = string.Format("UPDATE {0}." + tabName + " SET CHAR_" + this._field.ID.ToString() + " = ? WHERE (ID = ?)",DataUser.ZLGL);

			string newValue = ((TextBox)curPage.FindControl("CHARINPUT_" + _field.ID.ToString())).Text;
            if (charInfo.ValueRequired && newValue == string.Empty)
            {
                throw new SaveRecordDataIsNullException(_field.FieldName);
            }
            else
            {
                if (newValue.Length > this.charDefineInfo.MaxLen / 2) newValue = newValue.Substring(0, this.charDefineInfo.MaxLen / 2);

                OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,sql,
                    new OleDbParameter("newValue", newValue),
                    new OleDbParameter("id", recId));
                string str = string.Format("update {0}.QUALITY_ERROR_LIST set MEMO='" + newValue  + "' where table_name='{1}' and table_id='{2}'", DataUser.ZLGL, tabName, recId);
                OleDbParameter[] cmdParms = new OleDbParameter[] { };
                OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text, str.ToString(), cmdParms);
            }
		}
		public void ShowViewData(string tabName, int recID, System.Web.UI.Control curCtrl)
		{
			string sql = string.Format("SELECT CHAR_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			string val = Convert.ToString(OracleOledbBase.ExecuteScalar( sql, 
				new OleDbParameter("recid", recID)));

			curCtrl.Controls.Add(new LiteralControl(CleanString.InputText(val, 1000).Replace("\n", "<br>")));
		}
        public void ShowEditControl(string tabName, int recID, System.Web.UI.Control curCtrl, bool pass)
		{
            CharFieldDefineInfo charInfo = this.charDefineInfo;

            TextBox textInput = new TextBox();
            textInput.ID = "CHARINPUT_" + this._field.ID.ToString();
            textInput.MaxLength = charInfo.MaxLen;
            textInput.Text = charInfo.DefaultValue;
            textInput.Width = new Unit("280px");
            if (pass)
            {
                textInput.ReadOnly = true;
            }
            if (charInfo.MultiLine)
            {
                textInput.TextMode = TextBoxMode.MultiLine;
                textInput.Rows = charInfo.DisplayLine;
            }
            else
                textInput.TextMode = TextBoxMode.SingleLine;

            curCtrl.Controls.Add(textInput);
            if (charInfo.ValueRequired)
            {
                Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
                lb.Text = "*";
                lb.ForeColor = System.Drawing.Color.Red;
                curCtrl.Controls.Add(lb);
            }
            if (charInfo.Common != "")
                curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(charInfo.Common, 500).Replace("\n", "<br>") + "</span>"));
            //
			string sql = string.Format("SELECT CHAR_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			string val = Convert.ToString(OracleOledbBase.ExecuteScalar( sql, 
				new OleDbParameter("recid", recID)));

			textInput.Text = val;
			
		}
		public string GetFilter(string compOperator, string values)
		{
			string result;
			switch(compOperator)       
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
			switch(collectMode)       
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
		private CharFieldDefineInfo charDefineInfo
		{
			get
			{
				if (this._charInfo == null)
					this._charInfo = getCharFieldInfoByID(_field.FieldDefineID);
				return _charInfo;
			}
		}
		#endregion

		#region --˽�з���-- 
		private int insertNew(CharFieldDefineInfo charInfo)
		{
			string sql; 
			sql = string.Format("INSERT INTO {0}.T_CharFieldTypeDefineDict (id,ValueRequired, MultiLine, DisplayLine, MaxLen, Common, DefaultValue) VALUES (?, ?,?,?,?,?,?)",DataUser.ZLGL);
			int id=OracleOledbBase.GetMaxID("ID",DataUser.ZLGL+".T_CharFieldTypeDefineDict");

			OracleOledbBase.ExecuteScalar( sql, 
				new OleDbParameter("id", id),
				new OleDbParameter("valueRequesed", charInfo.ValueRequired==true?0:-1),
				new OleDbParameter("multiLine", charInfo.MultiLine==true?0:-1),
				new OleDbParameter("displayLine", charInfo.DisplayLine),
				new OleDbParameter("maxlen", charInfo.MaxLen),
                new OleDbParameter("common", CleanString.InputText(charInfo.Common,100)),
                new OleDbParameter("defaultValue", CleanString.InputText(charInfo.DefaultValue,25))
				);
			return id;
		}

		private void updateField(CharFieldDefineInfo charInfo, int fieldDefineID)
		{
			string str=string.Format("select maxlen from {0}.T_CharFieldTypeDefineDict where id="+fieldDefineID,DataUser.ZLGL);
			int maxlen=Convert.ToInt32(OracleOledbBase.ExecuteScalar(str).ToString());
            if(charInfo.MaxLen<maxlen)
				charInfo.MaxLen=maxlen;
			string sql; 
			sql = string.Format("UPDATE {0}.T_CharFieldTypeDefineDict SET ValueRequired =?, MultiLine = ?, DisplayLine =?, MaxLen =?, Common =?, DefaultValue = ? WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery( sql, 
				new OleDbParameter("valueRequesed", charInfo.ValueRequired==false? -1:0),
				new OleDbParameter("multiLine", charInfo.MultiLine==false?-1:0),
				new OleDbParameter("displayLine", charInfo.DisplayLine),
				new OleDbParameter("maxlen", charInfo.MaxLen),
                new OleDbParameter("common", CleanString.InputText(charInfo.Common,100)),
                new OleDbParameter("defaultValue", CleanString.InputText(charInfo.DefaultValue,25)),
				new OleDbParameter("fieldDefineID", fieldDefineID)
				);
		}
		private CharFieldDefineInfo getCharFieldInfoByID(int fieldDefineID)
		{
			string sql; 
			sql = string.Format("SELECT ID, ValueRequired, MultiLine, DisplayLine, MaxLen, Common, DefaultValue FROM {0}.T_CharFieldTypeDefineDict WHERE (ID = ?)",DataUser.ZLGL);

			CharFieldDefineInfo charInfo = null;

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, 
				new OleDbParameter("fieldDefineID", fieldDefineID)
				).Tables[0];

			for (int i=0;i<reader.Rows.Count;i++)
			{
				charInfo = new CharFieldDefineInfo();
				charInfo.ID = Convert.ToInt32(reader.Rows[i]["ID"].ToString());
				charInfo.ValueRequired = reader.Rows[i]["ValueRequired"].ToString()=="0"?true:false;
				charInfo.MultiLine = reader.Rows[i]["MultiLine"].ToString()=="-1"?false:true;
				charInfo.DisplayLine = Convert.ToInt32(reader.Rows[i]["DisplayLine"].ToString());
				charInfo.MaxLen = Convert.ToInt32(reader.Rows[i]["MaxLen"].ToString());
				charInfo.Common = reader.Rows[i]["Common"].ToString();
				charInfo.DefaultValue = reader.Rows[i]["DefaultValue"].ToString();
			}

			//reader.Close();
			return charInfo;
		}

		private CharFieldDefineInfo getCharFieldInfoByPorpertyPage(System.Web.UI.Page curPage)
		{
			// ����һ��ʵ����
			CharFieldDefineInfo charInfo = new CharFieldDefineInfo();
			charInfo.ValueRequired = ((CheckBox)curPage.FindControl("checkValueRequested")).Checked;
			charInfo.MultiLine = ((CheckBox)curPage.FindControl("checkMultiLine")).Checked;

			if (charInfo.MultiLine)
			{
				try
				{
					charInfo.DisplayLine = Convert.ToInt32(((TextBox)curPage.FindControl("textLineNum")).Text);
				}
				catch(Exception)
				{
					charInfo.DisplayLine = 1;
				}
			}
			else
				charInfo.DisplayLine = 1;

			try
			{
				charInfo.MaxLen = Convert.ToInt32(((TextBox)curPage.FindControl("textMaxlen")).Text);
			}
			catch(Exception)
			{
				charInfo.MaxLen = 50;
			}

			charInfo.DefaultValue = ((TextBox)curPage.FindControl("textDefaultValue")).Text;
			charInfo.Common = ((TextBox)curPage.FindControl("textFieldCommon")).Text;

			return charInfo;
		}
		#endregion
	}
}
