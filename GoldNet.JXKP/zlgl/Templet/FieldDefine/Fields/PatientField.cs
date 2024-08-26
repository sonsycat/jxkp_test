using System;
using System.Data.OleDb;
using System.Data;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using GoldNet.JXKP.PowerManager;
using GoldNet.JXKP.Templet.BLL;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using Goldnet.Ext.Web;


namespace GoldNet.JXKP.Templet.BLL.Fields
{
	/// <summary>
	/// 病人类型字段. 
	/// </summary>
	[Serializable()]
	public class PatientField : IFieldType
	{
		#region --常量-- 

		internal string CONNECT_STRING = TempletBO.CONNECT_STRING;

		#endregion

		#region --私有变量-- 
		private Field _field;
		private PatientFieldDefineInfo _PatientInfo;
		#endregion

		#region --构造函数-- 
		public PatientField(Field field)
		{
			_field = field;
		}
		#endregion

		#region --内部实体-- 
		[Serializable()]
			private class PatientFieldDefineInfo
		{
			public int ID;
			public bool ValueRequired;
			public string Common;
		}
		#endregion

		#region IFieldType 成员
		public string ListDisplayDataName
		{
			get
			{
				return "Patient_" + _field.ID.ToString();
			}
		}

		public string[] CollectModes
		{
			get
			{
				return new string[]{};
			}
		}

		public string FieldTypeName
		{
			get
			{
				return "病人";
			}
		}

		public string ListDataFormatString
		{
			get
			{
				return string.Empty;
			}
		}

		public string[] FilterOperators
		{
			get
			{
				return new string[]{"等于", "包含"};
			}
		}
		
		public void ShowSpecialProperty(System.Web.UI.Control curCtrl)
		{
			curCtrl.Controls.Add(new LiteralControl("<table width='100%' border='0' cellpadding='0' cellspacing='0'>"));

			// 显示一个新录入页面：
			// 1、说明
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'> 说明:<BR>&nbsp;"));
			TextBox textCommon = new TextBox();
			textCommon.ID = "textFieldCommon";
			textCommon.Rows = 3;
			textCommon.Width = new Unit("280px");
			textCommon.TextMode = TextBoxMode.MultiLine;
			curCtrl.Controls.Add(textCommon);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 2、必填
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>是否为必填字段:<BR>&nbsp;"));
			CheckBox checkValueRequired = new CheckBox();
			checkValueRequired.ID = "checkValueRequired";
			checkValueRequired.Text = "是";
			curCtrl.Controls.Add(checkValueRequired);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			curCtrl.Controls.Add(new LiteralControl("</table>"));

			if (_field != null)
			{
				// 初始化控件
				textCommon.Text = this.PatientDefineInfo.Common;
				checkValueRequired.Checked = this.PatientDefineInfo.ValueRequired;
			}
		}

		public int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			PatientFieldDefineInfo PatientInfo = this.getPatientFieldInfoByPorpertyPage(curPage);

			int fieldDefineID = insertNew(PatientInfo);

			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ ADD (Patient_" + this._field.ID.ToString() 
				+ " VARCHAR2(100) NULL, "
				+ "  Patient_ID_" + this._field.ID.ToString() 
				+ " VARCHAR2(50) NULL )";

			return fieldDefineID;
		}
		public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			PatientFieldDefineInfo PatientInfo = this.getPatientFieldInfoByPorpertyPage(curPage);

			// 更新定义属性
			updateField(PatientInfo, this._field.FieldDefineID);

			// 定义SQL定义语句.
			fieldDefineSql = string.Empty;
		}

		public void ShowInputControl(System.Web.UI.Control curCtrl,bool pass)
		{
            Store store = new Store();

            HttpProxy proxy = new HttpProxy();
            proxy.Method = HttpMethod.POST;
            proxy.Url = "WebService/PatientLists.ashx";

            JsonReader reader = new JsonReader();
            reader.ReaderID = "PATIENT_ID";
            reader.Root = "list";
            reader.TotalProperty = "totalCount";

            reader.Fields.Add(new RecordField("PATIENT_ID"));
            reader.Fields.Add(new RecordField("PATIENT_NAME"));
            reader.Fields.Add(new RecordField("DEPT_NAME"));

            store.Proxy.Add(proxy);
            store.Reader.Add(reader);
            store.AutoLoad = false;

            curCtrl.Controls.Add(store);


            //
            PatientFieldDefineInfo patientInfo = this.PatientDefineInfo;
            Goldnet.Ext.Web.ComboBox combobox = new Goldnet.Ext.Web.ComboBox();
            combobox.ID = "Patient_INPUT_" + this._field.ID.ToString();
            combobox.StoreID = store.ClientID;
            combobox.DisplayField = "PATIENT_NAME";
            combobox.ValueField = "PATIENT_ID";
            combobox.TypeAhead = false;
            combobox.LoadingText = "Searching...";
            combobox.Width = Unit.Pixel(150);
            combobox.ListWidth = Unit.Pixel(300);
            combobox.PageSize = 10;
            combobox.HideTrigger = true;
            combobox.ItemSelector = "div.search-item";
            combobox.MinChars = 1;
            combobox.HideTrigger = false;
            combobox.Template.Text = @"
               <tpl for=""."">
                  <div class=""search-item"">
                     <h3><span>{DEPT_NAME}</span>{PATIENT_NAME}</h3>
                  </div>
               </tpl>";
            curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
            curCtrl.Controls.Add(combobox);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            if (this.PatientDefineInfo.ValueRequired)
            {
                Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
                lb.Text = "*";
                lb.ForeColor = System.Drawing.Color.Red;
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(lb);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
            }
            curCtrl.Controls.Add(new LiteralControl("</tr></table>"));
            if (this.PatientDefineInfo.Common != "")
                curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(this.PatientDefineInfo.Common, 200).Replace("\n", "<br>") + "</span>"));
            curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>* 输入科室拼音,提取科室病人</span>"));
			
		}

		public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id)
		{
            PatientFieldDefineInfo patientInfo = this.PatientDefineInfo;
			string sql = string.Format("UPDATE {0}." + tabName + " SET Patient_" + this._field.ID.ToString() + " = ?, Patient_ID_" + this._field.ID.ToString() + " = ? WHERE (ID = ?)",DataUser.ZLGL);

            Goldnet.Ext.Web.ComboBox combox = (Goldnet.Ext.Web.ComboBox)curPage.FindControl("Patient_INPUT_" + _field.ID.ToString());
            if (patientInfo.ValueRequired && combox.SelectedItem.Value == string.Empty)
            {
                throw new SaveRecordDataIsNullException(_field.FieldName);
            }
            else
            {
                OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,sql,
                    new OleDbParameter("newValue", combox.SelectedItem.Text),
                    new OleDbParameter("Patientid", combox.SelectedItem.Value),
                    new OleDbParameter("id", recId));
                string str = string.Format("update {0}.QUALITY_ERROR_LIST set DUTY_USER_NAME='" + combox.SelectedItem.Text + "',DUTY_USER_ID='" + combox.SelectedItem.Value + "' where id=" + id, DataUser.ZLGL);
                OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, str, new OleDbParameter[0] { });
            }

          
		}

		public void ShowViewData(string tabName, int recID, System.Web.UI.Control curCtrl)
		{
			string sql = string.Format("SELECT Patient_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter("recid", recID)).Tables[0];
			if (reader.Rows.Count>0)
			{
				string val1 = string.Empty;

				if (reader.Rows[0][0].ToString() != "")
					val1 = reader.Rows[0][0].ToString();

				curCtrl.Controls.Add(new LiteralControl(val1));
			}
			
		}

        public void ShowEditControl(string tabName, int recID, System.Web.UI.Control curCtrl, bool pass)
		{
            string sql = string.Format("SELECT Patient_" + this._field.ID.ToString() + ", Patient_ID_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)", DataUser.ZLGL);

            DataTable table = OracleOledbBase.ExecuteDataSet(sql, new OleDbParameter("recid", recID)).Tables[0];
            //
            Store store = new Store();

            HttpProxy proxy = new HttpProxy();
            proxy.Method = HttpMethod.POST;
            string patientid = "";
            if (table.Rows.Count > 0)
            {
                patientid = table.Rows[0][1].ToString();
            }
            //proxy.Url = "WebService/StaffLists.ashx?staffid=" + staffid;
            proxy.Url = "WebService/PatientLists.ashx?patientid=" + patientid + "&tablename=" + tabName+"&fieldid="+this._field.ID.ToString();

            JsonReader reader = new JsonReader();
            reader.ReaderID = "PATIENT_ID";
            reader.Root = "list";
            reader.TotalProperty = "totalCount";

            reader.Fields.Add(new RecordField("PATIENT_ID"));
            reader.Fields.Add(new RecordField("PATIENT_NAME"));
            reader.Fields.Add(new RecordField("DEPT_NAME"));

            store.Proxy.Add(proxy);
            store.Reader.Add(reader);
            store.AutoLoad = true;

            curCtrl.Controls.Add(store);


            //
            PatientFieldDefineInfo patientInfo = this.PatientDefineInfo;
            Goldnet.Ext.Web.ComboBox combobox = new Goldnet.Ext.Web.ComboBox();
            combobox.ID = "Patient_INPUT_" + this._field.ID.ToString();
            combobox.StoreID = store.ClientID;
            combobox.DisplayField = "PATIENT_NAME";
            combobox.ValueField = "PATIENT_ID";
            combobox.TypeAhead = false;
            combobox.LoadingText = "Searching...";
            combobox.Width = Unit.Pixel(150);
            combobox.ListWidth = Unit.Pixel(300);
            combobox.PageSize = 10;
            combobox.HideTrigger = true;
            combobox.ItemSelector = "div.search-item";
            combobox.MinChars = 1;
            combobox.HideTrigger = false;
            combobox.Template.Text = @"
               <tpl for=""."">
                  <div class=""search-item"">
                     <h3><span>{DEPT_NAME}</span>{PATIENT_NAME}</h3>
                  </div>
               </tpl>";
            if (pass)
            {
                combobox.Disabled = true;
            }
            curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
            curCtrl.Controls.Add(combobox);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            if (this.PatientDefineInfo.ValueRequired)
            {
                Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
                lb.Text = "*";
                lb.ForeColor = System.Drawing.Color.Red;
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(lb);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
            }
            curCtrl.Controls.Add(new LiteralControl("</tr></table>"));

            if (this.PatientDefineInfo.Common != "")
                curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(this.PatientDefineInfo.Common, 200).Replace("\n", "<br>") + "</span>"));
            curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>* 输入科室拼音,提取科室病人</span>"));


            if (table.Rows.Count > 0)
            {
                combobox.SelectedItem.Value = table.Rows[0][1].ToString();
            }
		}

		public string GetFilter(string compOperator, string values)
		{
			string result;
			switch(compOperator)       
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
			return string.Empty;
		}

		#endregion

		#region --私有属性-- 
		private PatientFieldDefineInfo PatientDefineInfo
		{
			get
			{
				if (this._PatientInfo == null)
					this._PatientInfo = getPatientFieldInfoByID(_field.FieldDefineID);
				return _PatientInfo;
			}
		}
		#endregion

		#region --私有方法-- 
		private int insertNew(PatientFieldDefineInfo PatientInfo)
		{
			int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_PatientFieldTypeDefineDict",DataUser.ZLGL));
			string sql = string.Format("INSERT INTO {0}.T_PatientFieldTypeDefineDict (id,ValueRequired, Common) VALUES (?, ?,?)",DataUser.ZLGL);
			Convert.ToInt32(OracleOledbBase.ExecuteNonQuery( sql, 
				new OleDbParameter("id", id),
				new OleDbParameter("valueRequired", PatientInfo.ValueRequired==true?0:-1),
                new OleDbParameter("common", CleanString.InputText(PatientInfo.Common,100))
				)) ;
			return id;
		}

		private void updateField(PatientFieldDefineInfo PatientInfo, int fieldDefineID)
		{
			string sql = string.Format("UPDATE {0}.T_PatientFieldTypeDefineDict SET ValueRequired=?, Common=? WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery( sql, 
				new OleDbParameter("valueRequired", PatientInfo.ValueRequired==true?0:-1),
                new OleDbParameter("common", CleanString.InputText(PatientInfo.Common,100)),
				new OleDbParameter("fieldDefineID", fieldDefineID)
				);
		}

		private PatientFieldDefineInfo getPatientFieldInfoByID(int fieldDefineID)
		{
			string sql = string.Format("SELECT ID, ValueRequired, Common FROM {0}.T_PatientFieldTypeDefineDict WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, 
				new OleDbParameter("fieldDefineID", fieldDefineID)
				).Tables[0];

			if (reader.Rows.Count>0)
			{
				PatientFieldDefineInfo PatientInfo = new PatientFieldDefineInfo();
				PatientInfo.ID = Convert.ToInt32(reader.Rows[0][0].ToString());
				PatientInfo.ValueRequired = reader.Rows[0][1].ToString()=="-1"?false:true;
				PatientInfo.Common = reader.Rows[0][2].ToString();
				return PatientInfo;
			}
			
			return null;
		}

		private PatientFieldDefineInfo getPatientFieldInfoByPorpertyPage(System.Web.UI.Page curPage)
		{
			// 生成一个实体类
			PatientFieldDefineInfo PatientInfo = new PatientFieldDefineInfo();
			PatientInfo.ValueRequired = ((CheckBox)curPage.FindControl("checkValueRequired")).Checked;
			PatientInfo.Common = ((TextBox)curPage.FindControl("textFieldCommon")).Text;

			return PatientInfo;
		}
		
		#endregion
	}
	
}
