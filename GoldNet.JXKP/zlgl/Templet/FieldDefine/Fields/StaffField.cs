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
using GoldNet.Model;


namespace GoldNet.JXKP.Templet.BLL.Fields
{
	/// <summary>
	/// 人员类型字段. 
	/// </summary>
	[Serializable()]
	public class StaffField : IFieldType
	{
		#region --常量-- 
		internal string CONNECT_STRING = TempletBO.CONNECT_STRING;

		#endregion

		#region --私有变量-- 
		private Field _field;
		private StaffFieldDefineInfo _staffInfo;
		#endregion

		#region --构造函数-- 
		public StaffField(Field field)
		{
			_field = field;
		}
		#endregion

		#region --内部实体-- 
		[Serializable()]
		private class StaffFieldDefineInfo
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
				return "STAFF_" + _field.ID.ToString();
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
				return "人员";
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
				textCommon.Text = this.staffDefineInfo.Common;
				checkValueRequired.Checked = this.staffDefineInfo.ValueRequired;
			}
		}

		public int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			StaffFieldDefineInfo staffInfo = this.getStaffFieldInfoByPorpertyPage(curPage);

			int fieldDefineID = insertNew(staffInfo);

			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ ADD (STAFF_" + this._field.ID.ToString() 
				+ " VARCHAR2(100) NULL, "
				+ "  STAFF_ID_" + this._field.ID.ToString() 
				+ " VARCHAR2(50) NULL )";

			return fieldDefineID;
		}
		public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			StaffFieldDefineInfo staffInfo = this.getStaffFieldInfoByPorpertyPage(curPage);

			// 更新定义属性
			updateField(staffInfo, this._field.FieldDefineID);

			// 定义SQL定义语句.
			fieldDefineSql = string.Empty;
		}

		public void ShowInputControl(System.Web.UI.Control curCtrl,bool pass)
		{
            Store store = new Store();

            HttpProxy proxy = new HttpProxy();
            proxy.Method = HttpMethod.POST;
            proxy.Url = "WebService/StaffLists.ashx";

            JsonReader reader = new JsonReader();
            reader.ReaderID = "STAFF_ID";
            reader.Root = "list";
            reader.TotalProperty = "totalCount";

            reader.Fields.Add(new RecordField("STAFF_ID"));
            reader.Fields.Add(new RecordField("NAME"));
            reader.Fields.Add(new RecordField("DEPT_NAME"));

            store.Proxy.Add(proxy);
            store.Reader.Add(reader);
            store.AutoLoad = false;

            curCtrl.Controls.Add(store);


            //
            StaffFieldDefineInfo staffInfo = this.staffDefineInfo;
            Goldnet.Ext.Web.ComboBox combobox = new Goldnet.Ext.Web.ComboBox();
            combobox.ID = "STAFF_INPUT_" +this._field.ID.ToString();
            combobox.StoreID = store.ClientID;
            combobox.DisplayField = "NAME";
            combobox.ValueField = "STAFF_ID";
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
                     <h3><span>{DEPT_NAME}</span>{NAME}</h3>
                  </div>
               </tpl>";
          
            curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
            curCtrl.Controls.Add(combobox);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            if (this.staffDefineInfo.ValueRequired)
            {
                Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
                lb.Text = "*";
                lb.ForeColor = System.Drawing.Color.Red;
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(lb);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
            }
            curCtrl.Controls.Add(new LiteralControl("</tr></table>"));
			if (this.staffDefineInfo.Common != "")
				curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(this.staffDefineInfo.Common, 200).Replace("\n", "<br>") + "</span>"));
			curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>* 直接输入人名的首字母,再从列表中选择。如:张三(请输入 zs)</span>"));
		}
        //更新人员
		public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id)
		{
			string sql = string.Format("UPDATE {0}." + tabName + " SET STAFF_" + this._field.ID.ToString() + " = ?, STAFF_ID_" + this._field.ID.ToString() + " = ? WHERE (ID = ?)",DataUser.ZLGL);

            Goldnet.Ext.Web.ComboBox combox = (Goldnet.Ext.Web.ComboBox)curPage.FindControl("STAFF_INPUT_" + _field.ID.ToString());
            if (this.staffDefineInfo.ValueRequired && combox.SelectedItem.Value == string.Empty)
            {
                throw new SaveRecordDataIsNullException(_field.FieldName);
            }
            else
            {
                OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,sql,
                    new OleDbParameter("newValue", combox.SelectedItem.Text),
                    new OleDbParameter("staffid", combox.SelectedItem.Value),
                    new OleDbParameter("id", recId));
                string str = string.Format("update {0}.QUALITY_ERROR_LIST set DUTY_USER_NAME='" + combox.SelectedItem.Text + "',DUTY_USER_ID='" + combox.SelectedItem.Value + "' where  table_name='{1}' and table_id='{2}'", DataUser.ZLGL, tabName, recId);
                OracleOledbBase.ExecuteNonQuery(myTrans,CommandType.Text,str,new OleDbParameter[0]{});
            }
		}

		public void ShowViewData(string tabName, int recID, System.Web.UI.Control curCtrl)
		{
			string sql = string.Format("SELECT STAFF_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter("recid", recID)).Tables[0];
			if (reader.Rows.Count>0)
			{
				string val1 = string.Empty;

				if (reader.Rows[0][0].ToString() != "")
					val1 = reader.Rows[0][0].ToString();

				curCtrl.Controls.Add(new LiteralControl(val1));
			}
			
		}

		public void ShowEditControl(string tabName, int recID, System.Web.UI.Control curCtrl,bool pass)
		{
            string sql = string.Format("SELECT STAFF_" + this._field.ID.ToString() + ", STAFF_ID_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)", DataUser.ZLGL);

            DataTable table = OracleOledbBase.ExecuteDataSet(sql, new OleDbParameter("recid", recID)).Tables[0];
            //
            Store store = new Store();

            HttpProxy proxy = new HttpProxy();
            proxy.Method = HttpMethod.POST;
            string staffid = "";
            if (table.Rows.Count > 0)
            {
                staffid = table.Rows[0][1].ToString();
            }
            proxy.Url = "WebService/StaffLists.ashx?staffid=" + staffid ;

            JsonReader reader = new JsonReader();
            reader.ReaderID = "STAFF_ID";
            reader.Root = "list";
            reader.TotalProperty = "totalCount";

            reader.Fields.Add(new RecordField("STAFF_ID"));
            reader.Fields.Add(new RecordField("NAME"));
            reader.Fields.Add(new RecordField("DEPT_NAME"));

            store.Proxy.Add(proxy);
            store.Reader.Add(reader);
            store.AutoLoad = true;

            curCtrl.Controls.Add(store);


            //
            StaffFieldDefineInfo staffInfo = this.staffDefineInfo;
            Goldnet.Ext.Web.ComboBox combobox = new Goldnet.Ext.Web.ComboBox();
            combobox.ID = "STAFF_INPUT_" + this._field.ID.ToString();
            combobox.StoreID = store.ClientID;
            combobox.DisplayField = "NAME";
            combobox.ValueField = "STAFF_ID";
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
                     <h3><span>{DEPT_NAME}</span>{NAME}</h3>
                  </div>
               </tpl>";
            if (pass)
            {
                combobox.Disabled = true;
            }
            curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
            curCtrl.Controls.Add(combobox);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            if (this.staffDefineInfo.ValueRequired)
            {
                Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
                lb.Text = "*";
                lb.ForeColor = System.Drawing.Color.Red;
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(lb);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
            }
            curCtrl.Controls.Add(new LiteralControl("</tr></table>"));

            if (this.staffDefineInfo.Common != "")
                curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(this.staffDefineInfo.Common, 200).Replace("\n", "<br>") + "</span>"));
            curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>* 直接输入人名的首字母,再从列表中选择。如:张三(请输入 zs)</span>"));


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
		private StaffFieldDefineInfo staffDefineInfo
		{
			get
			{
				if (this._staffInfo == null)
					this._staffInfo = getStaffFieldInfoByID(_field.FieldDefineID);
				return _staffInfo;
			}
		}
		#endregion

		#region --私有方法-- 
		private int insertNew(StaffFieldDefineInfo staffInfo)
		{
			int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_StaffFieldTypeDefineDict",DataUser.ZLGL));
			string sql = string.Format("INSERT INTO {0}.T_StaffFieldTypeDefineDict (id,ValueRequired, Common) VALUES (?, ?,?)",DataUser.ZLGL);
			 Convert.ToInt32(OracleOledbBase.ExecuteNonQuery( sql, 
				new OleDbParameter("id", id),
				 new OleDbParameter("valueRequired", staffInfo.ValueRequired==true?0:-1),
				new OleDbParameter("common", staffInfo.Common)
				)) ;
			return id;
		}

		private void updateField(StaffFieldDefineInfo staffInfo, int fieldDefineID)
		{
			string sql = string.Format("UPDATE {0}.T_StaffFieldTypeDefineDict SET ValueRequired=?, Common=? WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(sql, 
				new OleDbParameter("valueRequired", staffInfo.ValueRequired==true?0:-1),
				new OleDbParameter("common", staffInfo.Common),
				new OleDbParameter("fieldDefineID", fieldDefineID)
				);
		}

		private StaffFieldDefineInfo getStaffFieldInfoByID(int fieldDefineID)
		{
			string sql = string.Format("SELECT ID, ValueRequired, Common FROM {0}.T_StaffFieldTypeDefineDict WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, 
				new OleDbParameter("fieldDefineID", fieldDefineID)
				).Tables[0];

			if (reader.Rows.Count>0)
			{
				StaffFieldDefineInfo staffInfo = new StaffFieldDefineInfo();
				staffInfo.ID = Convert.ToInt32(reader.Rows[0][0].ToString());
				staffInfo.ValueRequired = reader.Rows[0][1].ToString()=="-1"?false:true;
				staffInfo.Common = reader.Rows[0][2].ToString();
				return staffInfo;
			}
			
			return null;
		}

		private StaffFieldDefineInfo getStaffFieldInfoByPorpertyPage(System.Web.UI.Page curPage)
		{
			// 生成一个实体类
			StaffFieldDefineInfo staffInfo = new StaffFieldDefineInfo();
			staffInfo.ValueRequired = ((CheckBox)curPage.FindControl("checkValueRequired")).Checked;
			staffInfo.Common = ((TextBox)curPage.FindControl("textFieldCommon")).Text;

			return staffInfo;
		}
        public class Staff
        {
            public static IStaffTargetInfo GetStaff()
            {
                Object staff = HttpContext.Current.Session["CURRENTSTAFF"];
                User user = (User)(staff);
                GoldNet.JXKP.BLL.Organise.Staff curuser = new GoldNet.JXKP.BLL.Organise.Staff(user.UserId);
                return (IStaffTargetInfo)curuser;
            }
        }
		#endregion
	}
}
