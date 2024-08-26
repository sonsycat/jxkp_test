using System;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

using System.Web.UI;
using System.Web.UI.WebControls;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;


namespace GoldNet.JXKP.Templet.BLL.Fields
{
	/// <summary>
	/// 日期字段
	/// </summary>
	[Serializable()]
	internal class DateField : IFieldType
	{
		#region --私有变量-- 
		private Field _field;
		private DateFieldDefineInfo _dateInfo;
		#endregion

		#region --构造函数-- 
		public DateField(Field field)
		{
			_field = field;
		}

		#endregion

		#region --内部实体类-- 
		[Serializable()]
		private class DateFieldDefineInfo
		{
			public int ID;
			public bool ValueRequired;
			public string DefaultValue;
			public string Common;
		}
		#endregion

		#region IFieldType 成员

		public string FieldTypeName
		{
			get
			{
				return "日期";
			}
		}

		public string[] FilterOperators
		{
			get
			{
				return new string[]{"等于", "大于", "小于"};
			}
		}

		public string[] CollectModes
		{
			get
			{
				return new string[]{};
			}
		}

		public string ListDisplayDataName
		{
			get
			{
				return "DATE_" + _field.ID.ToString();
			}
		}

		public string ListDataFormatString
		{
			get
			{
				// 格式化成短日期格式
				return "{0:d}";
			}
		}

		public void ShowSpecialProperty(Control curCtrl)
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
			// textCommon.Text = "请按 年-月-日 格式输入日期。";
			curCtrl.Controls.Add(textCommon);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 2、必填
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>是否为必填字段:<BR>&nbsp;"));
			CheckBox checkValueRequested = new CheckBox();
			checkValueRequested.ID = "checkValueRequested";
			checkValueRequested.Text = "是";
			curCtrl.Controls.Add(checkValueRequested);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			// 3、默认值
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>默认值:"));
			RadioButtonList radioDefaultValue = new RadioButtonList();
			radioDefaultValue.ID = "radioDefaultValue";
			radioDefaultValue.Items.Add(new ListItem("无", ""));
			radioDefaultValue.Items.Add(new ListItem("当前日期", "TODAY"));
			radioDefaultValue.Items.Add(new ListItem("指定日期：", "CUSTOM_DATE"));
			radioDefaultValue.SelectedIndex = 0;
			curCtrl.Controls.Add(radioDefaultValue);

			curCtrl.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));

			TextBox customDateTextBox = new TextBox();
			customDateTextBox.ID = "customDateTextBox";
			//customDateTextBox.Attributes["onfocus"] = "javascript:document.all.checkDefaultValue_2.checked=true;";
			customDateTextBox.Width = new Unit("80px");
			customDateTextBox.MaxLength = 12;
			curCtrl.Controls.Add(customDateTextBox);

			CompareValidator dateValidator = new CompareValidator();
			dateValidator.Type = ValidationDataType.Date;
			dateValidator.Operator = ValidationCompareOperator.DataTypeCheck;
			dateValidator.ControlToValidate = customDateTextBox.ID;
			dateValidator.ErrorMessage = "请按 年-月-日 格式指定日期！";
			dateValidator.Text = "*";
			curCtrl.Controls.Add(dateValidator);

			curCtrl.Controls.Add(new LiteralControl("(按 年-月-日 格式输入)</td></tr>"));
			curCtrl.Controls.Add(new LiteralControl("</table>"));

			if (_field != null)
			{
				DateFieldDefineInfo dateInfo = this.dateDefineInfo;

				// 初始化控件
				textCommon.Text = dateInfo.Common;
				checkValueRequested.Checked = dateInfo.ValueRequired;
				if (dateInfo.DefaultValue == "")
				{
					radioDefaultValue.SelectedValue = "";
				}
				else if(dateInfo.DefaultValue == "TODAY")
				{
					radioDefaultValue.SelectedValue = "TODAY";
				}
				else
				{
					radioDefaultValue.SelectedValue = "CUSTOM_DATE";
					customDateTextBox.Text = dateInfo.DefaultValue;
				}
			}
		}

		public int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			DateFieldDefineInfo dateInfo = this.getDateFieldInfoByPorpertyPage(curPage);

			int fieldDefineID = insertNew(dateInfo);

			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ ADD DATE_" + this._field.ID.ToString() 
				+ " DATE ";

			return fieldDefineID;
		}

		public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			DateFieldDefineInfo dateInfo = this.getDateFieldInfoByPorpertyPage(curPage);

			// 更新定义属性
			updateField(dateInfo, this._field.FieldDefineID);

			// 定义SQL定义语句.
			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ modify DATE_" + this._field.ID.ToString() 
				+ " DATE ";
		}

		public void ShowInputControl(Control curCtrl,bool pass)
		{
			DateFieldDefineInfo dateInfo = this.dateDefineInfo;
            Goldnet.Ext.Web.DateField txtInput = new Goldnet.Ext.Web.DateField();
            txtInput.Width = new Unit(150);
            txtInput.ID = "DATEINPUT_"+this._field.ID.ToString();
            //txtInput.Format = "yyyy-m-dd";
            txtInput.ReadOnly = true;
            if (pass)
            {
                txtInput.Disabled = true;
            }
            if (dateInfo.DefaultValue == "")
            {
                //txtInput.Value = "";
                txtInput.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (dateInfo.DefaultValue == "TODAY")
            {
                txtInput.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                txtInput.Value = dateInfo.DefaultValue;
            }
            Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
            lb.Text = "*";
            lb.ForeColor = System.Drawing.Color.Red;
            curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
            curCtrl.Controls.Add(txtInput);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            curCtrl.Controls.Add(new LiteralControl("<td>"));
            curCtrl.Controls.Add(lb);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            curCtrl.Controls.Add(new LiteralControl("</tr></table>"));
            curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>* 请按 年-月-日 格式指定日期。</span>"));
		}
        //更新时间
		public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id)
		{
            DateFieldDefineInfo dateInfo = this.dateDefineInfo;
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
            string newValue = ((Goldnet.Ext.Web.DateField)curPage.FindControl("DATEINPUT_" +  _field.ID.ToString())).Value.ToString();
            if (dateInfo.ValueRequired && newValue == string.Empty)
            {
                throw new SaveRecordDataIsNullException(_field.FieldName);
            }
            else if (newValue == string.Empty)
            {
                newValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (guidedal.IsExistCheckCollectWeek(newValue) == true)
            {
                throw new SaveRecordDataIsNoWeek(ConvertDate.GetWeekRange(newValue));
            }
            else if (guidedal.IsExistCheckCollect(Convert.ToDateTime(newValue).Year.ToString() + "-" + Convert.ToDateTime(newValue).Month.ToString()) == true)
            {
                throw new SaveRecordDataIsNo(Convert.ToDateTime(newValue).Year.ToString() + "-" + Convert.ToDateTime(newValue).Month.ToString());
            }
            else
            {
                // 如果用户不输入值或值为空
                if (newValue != string.Empty)
                {
                    DateTime timeVal;
                    try
                    {
                        timeVal = Convert.ToDateTime(newValue);
                        newValue = timeVal.ToString("yyyy-MM-dd");
                        if (timeVal.Year < 1900)
                        {
                            throw new DateTimeException(this._field.FieldName);
                        }
                        else
                        {
                            string sql = string.Format("UPDATE {0}." + tabName + " SET DATE_" + this._field.ID.ToString() + " = to_date('" + newValue + "','YYYY-MM-DD') WHERE (ID = " + recId + ")", DataUser.ZLGL);
                            OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql, new OleDbParameter[0] {});
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                  
                }
                else
                {
                    newValue = System.DateTime.Now.ToString("yyyy-MM-dd");
                    string sql = string.Format("UPDATE {0}." + tabName + " SET DATE_" + this._field.ID.ToString() + " = to_date('" + newValue + "','YYYY-MM-DD') WHERE (ID = " + recId + ")", DataUser.ZLGL);
                    OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, sql, new OleDbParameter[0] { });
                }
                string strup = string.Format("update {0}.QUALITY_ERROR_LIST set DATE_TIME='{1}' where table_name='{2}' and table_id='{3}'", DataUser.ZLGL, newValue, tabName, recId);
                OracleOledbBase.ExecuteNonQuery(myTrans, CommandType.Text, strup, new OleDbParameter[0] { });
            }
		}

		public void ShowViewData(string tabName, int recID, Control curCtrl)
		{
			string sql = string.Format("SELECT DATE_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			string val = Convert.ToString(OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("recid", recID)));

			if (val == null || val == "")
			{
				curCtrl.Controls.Add(new LiteralControl(string.Empty));
			}
			else
			{
				curCtrl.Controls.Add(new LiteralControl(Convert.ToDateTime(val).ToString("yyyy-MM-dd")));
			}
		}

        public void ShowEditControl(string tabName, int recID, Control curCtrl, bool pass)
		{
			string sql = string.Format("SELECT DATE_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			string val = Convert.ToString(OracleOledbBase.ExecuteScalar( sql, 
				new OleDbParameter("recid", recID)));

			string dateVal = string.Empty;
			if (val != null && val != "")
			{
				dateVal = Convert.ToDateTime(val).ToShortDateString();
			}

            DateFieldDefineInfo dateInfo = this.dateDefineInfo;
            Goldnet.Ext.Web.DateField txtInput = new Goldnet.Ext.Web.DateField();
            txtInput.Width = new Unit(150);
            txtInput.ID = "DATEINPUT_" + this._field.ID.ToString();
            //txtInput.Format = "yyyy-m-dd";
            txtInput.ReadOnly = true;
            if (pass)
            {
                txtInput.Disabled = true;
            }
            
            txtInput.Value = dateVal;
            Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
            lb.Text = "*";
            lb.ForeColor = System.Drawing.Color.Red;
            curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
            curCtrl.Controls.Add(txtInput);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            curCtrl.Controls.Add(new LiteralControl("<td>"));
            curCtrl.Controls.Add(lb);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            curCtrl.Controls.Add(new LiteralControl("</tr></table>"));

            curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>* 请按 年-月-日 格式指定日期。</span>"));
		}

		public string GetFilter(string compOperator, string values)
		{
			int year, month;
			DateTime mm;
			switch(values)
			{
				case "上一月":
					year = DateTime.Now.Year;
					month = DateTime.Now.Month;

					if (month == 1)
					{
						month = 12;
						year -= 1;
					}
					else
					{
						month -= 1;
					}

					mm = new DateTime(year, month, 1);
					values = mm.ToShortDateString();
					break;
				case "上两月":
					year = DateTime.Now.Year;
					month = DateTime.Now.Month;

					if (month == 1)
					{
						month = 11;
						year -= 1;
					}
					else if (month == 2)
					{
						month = 12;
						year -= 1;
					}
					else
					{
						month -= 2;
					}

					mm = new DateTime(year, month, 1);
					values = mm.ToShortDateString();
					break;
				default:
					break;
			}

			// "等于", "大于", "小于"
			string result;
			
			switch(compOperator)       
			{
				case "等于":
					if (values.Split('-').Length == 2)
					{
						int _year = Convert.ToInt32(values.Split('-')[0]);
						int _month = Convert.ToInt32(values.Split('-')[1]);
						int _days = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(_year, _month);
						System.DateTime datefirst=Convert.ToDateTime(_year.ToString()+"-"+ _month.ToString() + "-01");
						System.DateTime datesecond=Convert.ToDateTime(_year.ToString()+"-"+ _month.ToString() + "-"+_days.ToString());
						
						result = this.ListDisplayDataName + ">='" +datefirst+"' AND " + this.ListDisplayDataName + "<='" +datesecond+"'";
					}
					else
					{
						result = this.ListDisplayDataName + "= '" + values.Replace("'", "") + "'";
					}
					break;
				case "大于":
					if (values.Split('-').Length == 2)
					{
						int _year = Convert.ToInt32(values.Split('-')[0]);
						int _month = Convert.ToInt32(values.Split('-')[1]);
						int _days = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(_year, _month);
						System.DateTime datefirst=Convert.ToDateTime(_year.ToString()+"-"+ _month.ToString() + "-01");
						System.DateTime datesecond=Convert.ToDateTime(_year.ToString()+"-"+ _month.ToString() + "-"+_days.ToString());
						
						result = this.ListDisplayDataName + ">='" +datefirst+"'";
					}
					else
					{
						result = this.ListDisplayDataName + " >= '" + values.Replace("'", "") + "'";
					}
					break;
				case "小于":
					if (values.Split('-').Length == 2)
					{
						int _year = Convert.ToInt32(values.Split('-')[0]);
						int _month = Convert.ToInt32(values.Split('-')[1]);
						int _days = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(_year, _month);
						System.DateTime datefirst=Convert.ToDateTime(_year.ToString()+"-"+ _month.ToString() + "-01");
						System.DateTime datesecond=Convert.ToDateTime(_year.ToString()+"-"+ _month.ToString() + "-"+_days.ToString());
						
						result = this.ListDisplayDataName + "<='" +datefirst+"'";
					}
					else
					{
						result = this.ListDisplayDataName + " <= '" + values.Replace("'", "") + "'";
					}
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
			switch(collectMode)       
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
		private DateFieldDefineInfo dateDefineInfo
		{
			get
			{
				if (this._dateInfo == null)
					this._dateInfo = getDateFieldInfoByID(_field.FieldDefineID);
				return _dateInfo;
			}
		}
		#endregion

		#region --私有方法-- 
		private int insertNew(DateFieldDefineInfo dateInfo)
		{
			string sql; 
			sql = string.Format("INSERT INTO {0}.T_DateFieldTypeDefineDict (ID,ValueRequired, DefaultValue, Common) VALUES (?,?,?,?)",DataUser.ZLGL);
			int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_DateFieldTypeDefineDict",DataUser.ZLGL));

			OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("ID", id),
				new OleDbParameter("valueRequesed", dateInfo.ValueRequired==true?0:-1),
				new OleDbParameter("defaultValue", dateInfo.DefaultValue),
                new OleDbParameter("common", CleanString.InputText(dateInfo.Common,100))
				) ;
			return id;
		}

		private void updateField(DateFieldDefineInfo dateInfo, int fieldDefineID)
		{
			string sql; 
			sql = string.Format("UPDATE {0}.T_DateFieldTypeDefineDict SET ValueRequired = ?, DefaultValue = ?, Common = ? WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(sql, 
				new OleDbParameter("valueRequesed", dateInfo.ValueRequired==true?0:-1),
				new OleDbParameter("defaultValue", dateInfo.DefaultValue),
                new OleDbParameter("common", CleanString.InputText(dateInfo.Common,100)),
				new OleDbParameter("fieldDefineID", fieldDefineID)
				);
		}

		private DateFieldDefineInfo getDateFieldInfoByID(int fieldDefineID)
		{
			string sql; 
			sql = string.Format("SELECT ID, ValueRequired, DefaultValue, Common FROM {0}.T_DateFieldTypeDefineDict WHERE (ID = ?)",DataUser.ZLGL);

			DateFieldDefineInfo dateInfo = null;

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, 
				new OleDbParameter("fieldDefineID", fieldDefineID)
				).Tables[0];

			for (int i=0;i<reader.Rows.Count;i++)
			{
				dateInfo = new DateFieldDefineInfo();
				dateInfo.ID = Convert.ToInt32(reader.Rows[i]["ID"].ToString());
				dateInfo.ValueRequired = reader.Rows[i]["ValueRequired"].ToString()=="-1"?false:true;
				dateInfo.DefaultValue = reader.Rows[i]["DefaultValue"].ToString();
				dateInfo.Common = reader.Rows[i]["Common"].ToString();
			}

			//reader.Close();
			return dateInfo;
		}

		private DateFieldDefineInfo getDateFieldInfoByPorpertyPage(System.Web.UI.Page curPage)
		{
			// 生成一个实体类
			DateFieldDefineInfo dateInfo = new DateFieldDefineInfo();
			dateInfo.ValueRequired = ((CheckBox)curPage.FindControl("checkValueRequested")).Checked;
			dateInfo.Common = ((TextBox)curPage.FindControl("textFieldCommon")).Text;

			string defaultValue = "";
			switch (((RadioButtonList)curPage.FindControl("radioDefaultValue")).SelectedValue)
			{
				case "TODAY":
					defaultValue = "TODAY";
					break;
				case "CUSTOM_DATE":
					defaultValue = ((TextBox)curPage.FindControl("customDateTextBox")).Text;
					break;
				default:
					defaultValue = "";
					break;
			}
			dateInfo.DefaultValue = defaultValue;
			return dateInfo;
		}
		#endregion
	}
}
