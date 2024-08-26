using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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
	/// 科室字段
	/// </summary>
	[Serializable()]
	public class DeptField : IFieldType
	{
		#region --常量-- 
		/// <summary>
		/// Connection string
		/// </summary>
		internal string CONNECT_STRING = TempletBO.CONNECT_STRING;

		#endregion

		#region --私有变量-- 
		private Field _field;
		private DeptFieldDefineInfo _deptInfo;
		#endregion

		#region --构造函数-- 
		public DeptField(Field field)
		{
			_field = field;
		}
		#endregion

		#region --内部实体-- 
		[Serializable()]
		private class DeptFieldDefineInfo
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
				return "DEPT_" + _field.ID.ToString();
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
				return "部门";
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
				textCommon.Text = this.deptDefineInfo.Common;
				checkValueRequired.Checked = this.deptDefineInfo.ValueRequired;
			}
		}

		public int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			DeptFieldDefineInfo deptInfo = this.getDeptFieldInfoByPorpertyPage(curPage);

			int fieldDefineID = insertNew(deptInfo);

			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ ADD (DEPT_" + this._field.ID.ToString() 
				+ " VARCHAR2(100) "
				+ " , DEPT_ID_" + this._field.ID.ToString() 
				+ " VARCHAR2(30))";

			return fieldDefineID;
		}
		public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			DeptFieldDefineInfo deptInfo = this.getDeptFieldInfoByPorpertyPage(curPage);

			// 更新定义属性
			updateField(deptInfo, this._field.FieldDefineID);

			// 定义SQL定义语句.
			fieldDefineSql = string.Empty;
		}

		public void ShowInputControl(System.Web.UI.Control curCtrl,bool pass)
		{
            Goldnet.Dal.TempList templist = new Goldnet.Dal.TempList();
            string deptlist = templist.GetSpecDeptList("DEPT_CODE", _field.TempletID);

            Store store = new Store();

            HttpProxy proxy = new HttpProxy();
            proxy.Method = HttpMethod.POST;
            proxy.Url = "WebService/DeptLists.ashx?deptfilter=" + deptlist;
            JsonReader reader = new JsonReader();
            reader.ReaderID = "DEPT_CODE";
            reader.Root = "deptlist";
            reader.TotalProperty = "totalCount";

            reader.Fields.Add(new RecordField("DEPT_CODE"));
            reader.Fields.Add(new RecordField("DEPT_NAME"));

            store.Proxy.Add(proxy);
            store.Reader.Add(reader);
            store.AutoLoad = false;

            curCtrl.Controls.Add(store);
            //
            Goldnet.Ext.Web.ComboBox combobox = new Goldnet.Ext.Web.ComboBox();
            combobox.ID = "DEPT_ID_" + this._field.ID.ToString();
            combobox.StoreID = store.ClientID;
            combobox.DisplayField = "DEPT_NAME";
            combobox.ValueField = "DEPT_CODE";
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
                     <h3><span>{DEPT_CODE}</span>{DEPT_NAME}</h3>
                  </div>
               </tpl>";
            if (pass)
            {
                combobox.Disabled = true;
            }
            Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
            lb.Text = "*";
            lb.ForeColor = System.Drawing.Color.Red;
            curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
            curCtrl.Controls.Add(combobox);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            curCtrl.Controls.Add(new LiteralControl("<td>"));
            curCtrl.Controls.Add(lb);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            curCtrl.Controls.Add(new LiteralControl("</tr></table>"));
			IStaffTargetInfo currentStaff = Staff.GetStaff();
			if (this.deptDefineInfo.Common != "")
				curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(this.deptDefineInfo.Common, 200).Replace("\n", "<br>") + "</span>"));
			curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>* 直接输入部门名的首字母,再从列表中选择。如:骨科(请输入 gk)</span>"));	
		}

       
        //更新科室
		public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id)
		{
			string sql = string.Format("UPDATE {0}." + tabName + " SET DEPT_" + this._field.ID.ToString() + " = ?, DEPT_ID_" + this._field.ID.ToString() + " = ?,dept_code=? WHERE (ID = ?)",DataUser.ZLGL);

            Goldnet.Ext.Web.ComboBox combox = (Goldnet.Ext.Web.ComboBox)curPage.FindControl("DEPT_ID_" + _field.ID.ToString());

            if (combox.SelectedItem.Value == string.Empty)
            {
                throw new SaveRecordDataIsNullException(_field.FieldName);
            }
            else
            {
               GoldNet.Comm.DAL.Oracle.MyLists listtable = new GoldNet.Comm.DAL.Oracle.MyLists();
                List listdeldetail = new List();
                OleDbParameter[] cmdParms = new OleDbParameter[]{
																new OleDbParameter("newValue", combox.SelectedItem.Text),
																new OleDbParameter("deptid", combox.SelectedItem.Value),
                                                                new OleDbParameter("deptcode", combox.SelectedItem.Value),
																new OleDbParameter("id", recId)											
															};
                listdeldetail.StrSql = sql;
                listdeldetail.Parameters = cmdParms;
                listtable.Add(listdeldetail);

                string str = string.Format("update {0}.QUALITY_ERROR_LIST set DUTY_DEPT_ID='" + combox.SelectedItem.Value + "',duty_dept_name='" + combox.SelectedItem.Text + "' where table_name='{1}' and table_id='{2}'", DataUser.ZLGL,tabName,recId);
                List listdelerr = new List();
                listdelerr.StrSql = str;
                listdelerr.Parameters = new OleDbParameter[0] { };
                listtable.Add(listdelerr);
                OracleOledbBase.ExecuteTranslist(myTrans, listtable);
            }
		}

		public void ShowViewData(string tabName, int recID, System.Web.UI.Control curCtrl)
		{
			string sql = string.Format("SELECT DEPT_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter("recid", recID)).Tables[0];
			for (int i=0;i<reader.Rows.Count;i++)
			{
				string val1 = string.Empty;

				if (reader.Rows[i][0].ToString() != "")
					val1 = reader.Rows[i][0].ToString();

				curCtrl.Controls.Add(new LiteralControl(val1));
			}
			//reader.Close();
		}

        public void ShowEditControl(string tabName, int recID, System.Web.UI.Control curCtrl, bool pass)
		{
            Goldnet.Dal.TempList templist = new Goldnet.Dal.TempList();
            string deptlist = templist.GetSpecDeptList("DEPT_CODE", _field.TempletID);

            string sqldept = string.Format("SELECT DEPT_" + this._field.ID.ToString() + ", DEPT_ID_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)", DataUser.ZLGL);

            DataTable tabledept = OracleOledbBase.ExecuteDataSet(sqldept, new OleDbParameter("recid", recID)).Tables[0];
             string deptcode = "";
             if (tabledept.Rows.Count > 0)
            {
                deptcode = tabledept.Rows[0][1].ToString();
            }

            Store store = new Store();

            HttpProxy proxy = new HttpProxy();
            proxy.Method = HttpMethod.POST;
            proxy.Url = "WebService/DeptLists.ashx?deptfilter=" + deptlist+"&deptcode="+deptcode;
            JsonReader reader = new JsonReader();
            reader.ReaderID = "DEPT_CODE";
            reader.Root = "deptlist";
            reader.TotalProperty = "totalCount";

            reader.Fields.Add(new RecordField("DEPT_CODE"));
            reader.Fields.Add(new RecordField("DEPT_NAME"));

            store.Proxy.Add(proxy);
            store.Reader.Add(reader);
            store.AutoLoad = true;

            curCtrl.Controls.Add(store);
            //
            Goldnet.Ext.Web.ComboBox combobox = new Goldnet.Ext.Web.ComboBox();
            combobox.ID = "DEPT_ID_" + this._field.ID.ToString();
            combobox.StoreID = store.ClientID;
            combobox.DisplayField = "DEPT_NAME";
            combobox.ValueField = "DEPT_CODE";
            combobox.TypeAhead = false;
            combobox.LoadingText = "Searching...";
            combobox.Width = Unit.Pixel(150);
            combobox.ListWidth = Unit.Pixel(220);
            combobox.PageSize = 10;
            combobox.HideTrigger = true;
            combobox.ItemSelector = "div.search-item";
            combobox.MinChars = 1;
            combobox.HideTrigger = false;
            combobox.Template.Text = @"
               <tpl for=""."">
                  <div class=""search-item"">
                     <h3><span>{DEPT_CODE}</span>{DEPT_NAME}</h3>
                  </div>
               </tpl>";
            if (pass)
            {
                combobox.Disabled = true;
            }
            Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
            lb.Text = "*";
            lb.ForeColor = System.Drawing.Color.Red;
            curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
            curCtrl.Controls.Add(combobox);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            curCtrl.Controls.Add(new LiteralControl("<td>"));
            curCtrl.Controls.Add(lb);
            curCtrl.Controls.Add(new LiteralControl("</td>"));
            curCtrl.Controls.Add(new LiteralControl("</tr></table>"));

            IStaffTargetInfo currentStaff = Staff.GetStaff();
            if (this.deptDefineInfo.Common != "")
                curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>" + CleanString.InputText(this.deptDefineInfo.Common, 200).Replace("\n", "<br>") + "</span>"));
            curCtrl.Controls.Add(new LiteralControl("<br><span class='gs-input-desc'>* 直接输入部门名的首字母,再从列表中选择。如:骨科(请输入 gk)</span>"));	

			string sql = string.Format("SELECT DEPT_" + this._field.ID.ToString() + ", DEPT_ID_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			DataTable table = OracleOledbBase.ExecuteDataSet(sql, new OleDbParameter("recid", recID)).Tables[0];
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
		private DeptFieldDefineInfo deptDefineInfo
		{
			get
			{
				if (this._deptInfo == null)
					this._deptInfo = getDeptFieldInfoByID(_field.FieldDefineID);
				return _deptInfo;
			}
		}
		#endregion

		#region --私有方法-- 
		private int insertNew(DeptFieldDefineInfo deptInfo)
		{
			string sql = string.Format("INSERT INTO {0}.T_DeptFieldTypeDefineDict (ID,ValueRequired, Common) VALUES (?,?,?)",DataUser.ZLGL);
			int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_DeptFieldTypeDefineDict",DataUser.ZLGL));

			OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("ID", id),
				new OleDbParameter("valueRequired", deptInfo.ValueRequired==true?0:-1),
                new OleDbParameter("common", CleanString.InputText(deptInfo.Common,100))
				) ;
			return id;
		}

		private void updateField(DeptFieldDefineInfo deptInfo, int fieldDefineID)
		{
			string sql = string.Format("UPDATE {0}.T_DeptFieldTypeDefineDict SET ValueRequired=?, Common=? WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(sql, 
				new OleDbParameter("valueRequired", deptInfo.ValueRequired==true?0:-1),
                new OleDbParameter("common", CleanString.InputText(deptInfo.Common,100)),
				new OleDbParameter("fieldDefineID", fieldDefineID)
				);
		}

		private DeptFieldDefineInfo getDeptFieldInfoByID(int fieldDefineID)
		{
			string sql = string.Format("SELECT ID, ValueRequired, Common FROM {0}.T_DeptFieldTypeDefineDict WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, 
				new OleDbParameter("fieldDefineID", fieldDefineID)
				).Tables[0];

			if( reader.Rows.Count >0)
			{
				DeptFieldDefineInfo deptInfo = new DeptFieldDefineInfo();
				deptInfo.ID = Convert.ToInt32(reader.Rows[0]["ID"].ToString());
				deptInfo.ValueRequired = reader.Rows[0]["ValueRequired"].ToString()=="-1"?false:true;
				deptInfo.Common = reader.Rows[0]["Common"].ToString();
				return deptInfo;
			}
			//reader.Close();
			return null;
		}

		private DeptFieldDefineInfo getDeptFieldInfoByPorpertyPage(System.Web.UI.Page curPage)
		{
			// 生成一个实体类
			DeptFieldDefineInfo deptInfo = new DeptFieldDefineInfo();
			deptInfo.ValueRequired = ((CheckBox)curPage.FindControl("checkValueRequired")).Checked;
			deptInfo.Common = ((TextBox)curPage.FindControl("textFieldCommon")).Text;

			return deptInfo;
		}
		#endregion

	}
    
}
