using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoldNet.JXKP.Templet.BLL;
using GoldNet.JXKP.PowerManager;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.Templet.BLL.Fields
{
	/// <summary>
	/// 指标类型对象.
	/// </summary>
	[Serializable()]
	public class GuideField : IFieldType
	{
		/// <summary>
		/// Connection string
		/// </summary>

		internal string CONNECT_STRING = TempletBO.CONNECT_STRING;

		#region --私有变量-- 
		private Field _field;
		private GuideFieldDefineInfo _guideInfo;
		#endregion

		#region --构造函数-- 
		public GuideField(Field field)
		{
			_field = field;
		}
		#endregion

		#region --内部实体-- 
		[Serializable()]
		private class GuideFieldDefineInfo
		{
			public int ID;
			public string GuideID;
			public string Common;
		}
		#endregion

		#region IFieldType 成员

		public string ListDisplayDataName
		{
			get
			{
				return "GUIDE_" + _field.ID.ToString();
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
				return "指标";
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
            Goldnet.Dal.ZLGL_Guide_Dict guidedal = new Goldnet.Dal.ZLGL_Guide_Dict();
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

			// 2、指标编号
			curCtrl.Controls.Add(new LiteralControl("<tr><td class='gs-input-section'>质量考核指标:<BR>&nbsp;"));
			DropDownList ddlGuideID = new DropDownList();
			ddlGuideID.ID = "ddlGuideID";
            ddlGuideID.DataSource = guidedal.GetAllGuide();
			ddlGuideID.DataTextField = "GuideName";
			ddlGuideID.DataValueField = "GuideName";
			ddlGuideID.DataBind();

			curCtrl.Controls.Add(ddlGuideID);
			curCtrl.Controls.Add(new LiteralControl("</td></tr>"));

			curCtrl.Controls.Add(new LiteralControl("</table>"));

			if (_field != null)
			{
				// 初始化控件
				textCommon.Text = this.guideDefineInfo.Common;
				if (ddlGuideID.Items.FindByValue(this.guideDefineInfo.GuideID.ToString()) != null)
					ddlGuideID.SelectedValue = this.guideDefineInfo.GuideID.ToString();
			}
		}

		public int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			GuideFieldDefineInfo guideInfo = this.getGuideFieldInfoByPorpertyPage(curPage);

			int fieldDefineID = insertNew(guideInfo);

			fieldDefineSql = " ALTER TABLE $$TABLE_NAME$$ ADD (GUIDE_" + this._field.ID.ToString() 
				+ " VARCHAR2(1000) "
				+ ", GUIDE_STANDARD_" + this._field.ID.ToString() 
				+ " VARCHAR2(2000))";

			return fieldDefineID;
		}
		public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			GuideFieldDefineInfo guideInfo = this.getGuideFieldInfoByPorpertyPage(curPage);

			// 更新定义属性
			updateField(guideInfo, this._field.FieldDefineID);

			// 定义SQL定义语句.
			fieldDefineSql = string.Empty;
		}

		public void ShowInputControl(System.Web.UI.Control curCtrl,bool pass)
		{
            Goldnet.Dal.Guide_Manager guidedal = new Goldnet.Dal.Guide_Manager();
            Store store = new Store();
            store.ID = "storeguide";
            JsonReader reader = new JsonReader();
            reader.Fields.Add(new RecordField("ID"));
            reader.Fields.Add(new RecordField("CHECKCONT"));

            store.Reader.Add(reader);
            store.AutoLoad = false;

            curCtrl.Controls.Add(store);
            //
			//专业质量分类
            DataTable contentypetable = guidedal.Conten_Type_all(this.guideDefineInfo.GuideID).Tables[0];
            Goldnet.Ext.Web.ComboBox combobox_contentype = new Goldnet.Ext.Web.ComboBox();
            combobox_contentype.Editable = false;
            combobox_contentype.AutoPostBack = false;
            combobox_contentype.AutoShow = false;
            

            combobox_contentype.Width = new Unit(120);
            combobox_contentype.ID = "GUIDE_TYPE_" + _field.ID.ToString();
            for (int i = 0; i < contentypetable.Rows.Count; i++)
            {
                combobox_contentype.Items.Insert(i, new Goldnet.Ext.Web.ListItem(contentypetable.Rows[i]["CONTENT_TYPE"].ToString(), contentypetable.Rows[i]["ID"].ToString()));
            }
            combobox_contentype.AjaxEvents.Select.Event += Comcontype_Select_Event;

            combobox_contentype.AjaxEvents.Select.EventMask.ShowMask = true;
            
            //考核内容
            Goldnet.Ext.Web.ComboBox combobox_ddlguide = new Goldnet.Ext.Web.ComboBox();
            combobox_ddlguide.Editable = false;
           
            //combobox_ddlguide.Width = new Unit(350);
            combobox_ddlguide.ID = "GUIDE_INPUT_"+_field.ID.ToString();
            combobox_ddlguide.StoreID = store.ClientID;
            combobox_ddlguide.DisplayField = "CHECKCONT";
            combobox_ddlguide.ValueField = "ID";
            combobox_ddlguide.AjaxEvents.Select.Event += Comguide_Select_Event;

            combobox_ddlguide.AjaxEvents.Select.EventMask.ShowMask = true;
            //考核标准
            FildeGuide guide = new FildeGuide(this.guideDefineInfo.GuideID); 
            Goldnet.Ext.Web.TextArea textGuideStanard = new Goldnet.Ext.Web.TextArea();
            textGuideStanard.ID = "GUIDE_STANARD_"+_field.ID.ToString();
			textGuideStanard.Width = new Unit(470);
			textGuideStanard.BorderWidth = new Unit("0px");
            textGuideStanard.ReadOnly = true;

            Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
            lb.Text = "*";
            lb.ForeColor = System.Drawing.Color.Red;

            if (guide.GuideTypeID != 2 && guide.GuideTypeID != 3)
            {
                combobox_ddlguide.Width = new Unit(470);
                Goldnet.Dal.TempList tempdal = new Goldnet.Dal.TempList();
                DataSet sourceDS = GetGuideContent(this.guideDefineInfo.GuideID);
                Goldnet.Ext.Web.Store storeguide = ((Goldnet.Ext.Web.Store)curCtrl.Page.FindControl("storeguide"));
                storeguide.DataSource = sourceDS;
                storeguide.DataBind();
                curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
                curCtrl.Controls.Add(combobox_ddlguide);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(lb);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("</tr></table>"));
            }
            else
			{
                combobox_ddlguide.Width = new Unit(350);
                curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
                curCtrl.Controls.Add(combobox_contentype);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(combobox_ddlguide);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(lb);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("</tr></table>"));
			}
           
            curCtrl.Controls.Add(new LiteralControl("<br><fieldset style='width:95%' class='gs-input-desc'><legend><span class='gs-input-desc'>考评标准：</span></legend>"));
            curCtrl.Controls.Add(textGuideStanard);
            curCtrl.Controls.Add(new LiteralControl("</fieldset>"));
          
		}

        void Comcontype_Select_Event(object sender, AjaxEventArgs e)
        {
           Goldnet.Dal.TempList tempdal = new Goldnet.Dal.TempList();
           Goldnet.Ext.Web.ComboBox com= (Goldnet.Ext.Web.ComboBox)(sender);
           Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(com.Page);
           Goldnet.Ext.Web.ComboBox ddlGuide = ((Goldnet.Ext.Web.ComboBox)com.Page.FindControl("GUIDE_INPUT_"+_field.ID.ToString()));
           Goldnet.Ext.Web.TextArea txt = ((Goldnet.Ext.Web.TextArea)com.FindControl("GUIDE_STANARD_"+_field.ID.ToString()));
           txt.Text = "";
           scManager.AddScript(ddlGuide.ClientID + ".store.removeAll();");
           scManager.AddScript(ddlGuide.ClientID + ".clearValue();");
           DataSet sourceDS = tempdal.GetGuideContent(com.SelectedItem.Value);
           com.SelectedItem.Value = com.SelectedItem.Value;
           //com.Page.Session["CONTENTYPE"] = com.SelectedItem.Value;
           Goldnet.Ext.Web.Store storeguide = ((Goldnet.Ext.Web.Store)com.Page.FindControl("storeguide"));
           ddlGuide.StoreID = storeguide.ID;
           storeguide.DataSource = sourceDS;
           storeguide.DataBind();

           


        }
        void Comguide_Select_Event(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.TempList tempdal = new Goldnet.Dal.TempList();
            Goldnet.Ext.Web.ComboBox com = (Goldnet.Ext.Web.ComboBox)(sender);
            Goldnet.Ext.Web.TextArea txt = ((Goldnet.Ext.Web.TextArea)com.Page.FindControl("GUIDE_STANARD_"+_field.ID.ToString()));
            txt.Text = tempdal.GetGuideContentdetail(int.Parse(com.SelectedItem.Value));
            com.SelectedItem.Value = com.SelectedItem.Value;
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(com.Page);
            scManager.AddScript(com.ClientID + ".list.hide();");
            
        }
       
        //更新指标
		public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id)
		{
			string newValue="";
            FildeGuide guide = new FildeGuide(this.guideDefineInfo.GuideID);

            Goldnet.Ext.Web.ComboBox ddlGuide = ((Goldnet.Ext.Web.ComboBox)curPage.FindControl("GUIDE_INPUT_" + _field.ID.ToString()));
            newValue = ddlGuide.SelectedItem.Text;
            if (newValue == string.Empty)
            {
                throw new SaveRecordDataIsNullException(_field.FieldName);
            }
            else
            {
                GoldNet.Comm.DAL.Oracle.MyLists listtable = new GoldNet.Comm.DAL.Oracle.MyLists();
                List listdeldetail = new List();
                if (newValue.Length > 500) newValue = newValue.Substring(0, 500);
                string guideStanard = ((Goldnet.Ext.Web.TextArea)curPage.FindControl("GUIDE_STANARD_" + _field.ID.ToString())).Text;
                if (guideStanard.Length > 500) guideStanard = guideStanard.Substring(0, 500);
                string sql = string.Format("UPDATE {0}." + tabName + " SET GUIDE_" + this._field.ID.ToString() + " = ?, GUIDE_STANDARD_" + this._field.ID.ToString() + " = ?,CHECKCONT_ID=?  WHERE (ID = ?)", DataUser.ZLGL);

                OleDbParameter[] cmdParms = new OleDbParameter[]{
																new OleDbParameter("newValue", newValue),
																new OleDbParameter("guideStanard", guideStanard),
                                                                new OleDbParameter("CHECKCONT_ID", int.Parse(ddlGuide.SelectedItem.Value)),
																new OleDbParameter("id", recId)												
															};

                listdeldetail.StrSql = sql;
                listdeldetail.Parameters = cmdParms;
                listtable.Add(listdeldetail);
                string str = string.Format("update {0}.QUALITY_ERROR_LIST set GUIDE_CONTENT='" + newValue + "',CHECKCONT_ID=" + int.Parse(ddlGuide.SelectedItem.Value) + " where id=" + id, DataUser.ZLGL);
                List listdelerr = new List();
                listdelerr.StrSql = str;
                listdelerr.Parameters = new OleDbParameter[0] { };
                listtable.Add(listdelerr);
                OracleOledbBase.ExecuteTranslist(myTrans,listtable);
            }
		}

		
        /// <summary>
        /// 获取某一指标名称的考评内容
        /// </summary>
        /// <param name="guideName">指标名称</param>
        /// <returns>DataSet</returns>
        public static DataSet GetGuideContent(string guideName)
        {
            FildeGuide guide = new FildeGuide(guideName);                        //初始化指标对象

            int guideNameID = guide.ID;                                             //返回指标名称ID
            int guideTypeID = guide.GuideTypeID;                                    //返回指标类别ID

            string SQL_GetGuideCont = String.Format("SELECT ID,CheckCont FROM {2}.G_GuideCheckContent WHERE GuideNameID = {0} AND GuideTypeID = {1}", guideNameID, guideTypeID,DataUser.ZLGL);

            System.Data.OracleClient.OracleParameter[] cmdPara = new System.Data.OracleClient.OracleParameter[] { };
            DataSet ds = OracleOledbBase.ExecuteDataSet(SQL_GetGuideCont);

            return ds;
        }


		public void ShowViewData(string tabName, int recID, System.Web.UI.Control curCtrl)
		{
			string sql = string.Format("SELECT GUIDE_" + this._field.ID.ToString() + " , GUIDE_STANDARD_" + this._field.ID.ToString() + " FROM {0}." + tabName + " WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet(sql, new OleDbParameter("recid", recID)).Tables[0];
			for (int i=0;i<reader.Rows.Count;i++)
			{
				string val1 = string.Empty;
				string val2 = string.Empty;

				if (reader.Rows[i][0].ToString() != "")
					val1 = reader.Rows[i][0].ToString();
				if (reader.Rows[i][1].ToString() != "")
					val2 = reader.Rows[i][1].ToString();
				curCtrl.Controls.Add(new LiteralControl(val1 + "<br><fieldset style='width:80%' class='gs-input-desc'><legend><span class='gs-input-desc'>考评标准：</span></legend>" + val2.Replace("\r\n", "<br>") + "</fieldset>"));

			}
		}

        public void ShowEditControl(string tabName, int recID, System.Web.UI.Control curCtrl, bool pass)
		{
            Store store = new Store();
            store.ID = "storeguide";
            JsonReader reader = new JsonReader();
            reader.Fields.Add(new RecordField("ID"));
            reader.Fields.Add(new RecordField("CHECKCONT"));

            store.Reader.Add(reader);
            

            curCtrl.Controls.Add(store);
            //
            //专业质量分类
            Goldnet.Dal.Guide_Manager guidedal = new Goldnet.Dal.Guide_Manager();
            DataTable contentypetable = guidedal.Conten_Type_all(this.guideDefineInfo.GuideID).Tables[0];
            Goldnet.Ext.Web.ComboBox combobox_contentype = new Goldnet.Ext.Web.ComboBox();
            combobox_contentype.Editable = false;
            combobox_contentype.AutoPostBack = false;
            
            combobox_contentype.Width = new Unit(120);
            combobox_contentype.ID = "GUIDE_TYPE_" + _field.ID.ToString();
            for (int i = 0; i < contentypetable.Rows.Count; i++)
            {
                combobox_contentype.Items.Insert(i, new Goldnet.Ext.Web.ListItem(contentypetable.Rows[i]["CONTENT_TYPE"].ToString(), contentypetable.Rows[i]["ID"].ToString()));
            }
            combobox_contentype.AjaxEvents.Select.Event += Comcontype_Select_Event;

            combobox_contentype.AjaxEvents.Select.EventMask.ShowMask = true;
            combobox_contentype.Disabled = true;
          


            //考核内容
            Goldnet.Ext.Web.ComboBox combobox_ddlguide = new Goldnet.Ext.Web.ComboBox();
            combobox_ddlguide.Editable = false;
            combobox_ddlguide.Width = new Unit(200);
            combobox_ddlguide.ID = "GUIDE_INPUT_" + _field.ID.ToString();
            combobox_ddlguide.StoreID = store.ClientID;
            combobox_ddlguide.DisplayField = "CHECKCONT";
            combobox_ddlguide.ValueField = "ID";
            combobox_ddlguide.AjaxEvents.Select.Event += Comguide_Select_Event;
            combobox_ddlguide.Disabled = true;

            combobox_ddlguide.AjaxEvents.Select.EventMask.ShowMask = true;
            //考核标准
            FildeGuide guide = new FildeGuide(this.guideDefineInfo.GuideID);
            Goldnet.Ext.Web.TextArea textGuideStanard = new Goldnet.Ext.Web.TextArea();
            textGuideStanard.ID = "GUIDE_STANARD_" + _field.ID.ToString();
            textGuideStanard.Width = new Unit(470);
            textGuideStanard.BorderWidth = new Unit("0px");
            textGuideStanard.ReadOnly = true;

            Goldnet.Ext.Web.Label lb = new Goldnet.Ext.Web.Label();
            lb.Text = "*";
            lb.ForeColor = System.Drawing.Color.Red;
            if (guide.GuideTypeID != 2 && guide.GuideTypeID != 3)
            {
                Goldnet.Dal.TempList tempdal = new Goldnet.Dal.TempList();
                DataSet sourceDS = GetGuideContent(this.guideDefineInfo.GuideID);
                Goldnet.Ext.Web.Store storeguide = ((Goldnet.Ext.Web.Store)curCtrl.Page.FindControl("storeguide"));
                storeguide.DataSource = sourceDS;
                storeguide.DataBind();
                curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
                curCtrl.Controls.Add(combobox_ddlguide);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(lb);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("</tr></table>"));
            }
            else
            {
                store.AutoLoad = true;
                curCtrl.Controls.Add(new LiteralControl("<table><tr><td>"));
                curCtrl.Controls.Add(combobox_contentype);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(combobox_ddlguide);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("<td>"));
                curCtrl.Controls.Add(lb);
                curCtrl.Controls.Add(new LiteralControl("</td>"));
                curCtrl.Controls.Add(new LiteralControl("</tr></table>"));
            }
            curCtrl.Controls.Add(new LiteralControl("<br><fieldset style='width:95%' class='gs-input-desc'><legend><span class='gs-input-desc'>考评标准：</span></legend>"));
            curCtrl.Controls.Add(textGuideStanard);
            curCtrl.Controls.Add(new LiteralControl("</fieldset>"));

            //
            string sql = string.Format("SELECT b.id, to_char(b.CHECKCONT) CHECKCONT, a.GUIDE_STANDARD_" + this._field.ID.ToString() + ",b.CONTENTYPE,c.CONTENT_TYPE FROM {0}." + tabName + " a,{0}.G_GUIDECHECKCONTENT b,{0}.G_CONTENT_TYPE c WHERE c.id(+)=b.CONTENTYPE and   (a.GUIDE_" + this._field.ID.ToString() + "=to_char(b.checkcont) OR a.CHECKCONT_ID = TO_CHAR (b.ID)) and (a.ID = ?)", DataUser.ZLGL);
			DataTable table = OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter("recid", recID)).Tables[0];
            if (guide.GuideTypeID == 2)
            {
                if (table.Rows.Count > 0)
                {
                    combobox_contentype.SelectedItem.Value = table.Rows[0]["CONTENTYPE"].ToString();
                    //
                    Goldnet.Dal.TempList tempdal = new Goldnet.Dal.TempList();
                    DataSet sourceDS = tempdal.GetGuideContent(combobox_contentype.SelectedItem.Value);
                    store.DataSource = sourceDS;
                    store.DataBind();
                    combobox_ddlguide.SelectedItem.Value = table.Rows[0]["ID"].ToString();
                    textGuideStanard.Text = table.Rows[0]["GUIDE_STANDARD_" + this._field.ID.ToString()].ToString();

                }
            }
            else
            {
                if (table.Rows.Count > 0)
                {
                    combobox_ddlguide.SelectedItem.Value = table.Rows[0]["ID"].ToString();
                    textGuideStanard.Text = table.Rows[0]["GUIDE_STANDARD_" + this._field.ID.ToString()].ToString();
                }
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
		private GuideFieldDefineInfo guideDefineInfo
		{
			get
			{
				if (this._guideInfo == null)
					this._guideInfo = getGuideFieldInfoByID(_field.FieldDefineID);
				return _guideInfo;
			}
		}
		#endregion

		#region --私有方法-- 
		private int insertNew(GuideFieldDefineInfo guideInfo)
		{
			int id=OracleOledbBase.GetMaxID("ID",string.Format("{0}.T_GuideFieldTypeDefineDict",DataUser.ZLGL));
			string sql = string.Format("INSERT INTO {0}.T_GuideFieldTypeDefineDict(id,GuideID, Common) VALUES (?,?, ?)",DataUser.ZLGL);
			OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("id", id),
				new OleDbParameter("guideID", guideInfo.GuideID),
                new OleDbParameter("common", CleanString.InputText(guideInfo.Common,100))
				) ;
			return id;
		}

		private void updateField(GuideFieldDefineInfo guideInfo, int fieldDefineID)
		{
			string sql = string.Format("UPDATE {0}.T_GuideFieldTypeDefineDict SET GuideID = ?, Common=? WHERE (ID = ?)",DataUser.ZLGL);
			OracleOledbBase.ExecuteNonQuery( sql, 
				new OleDbParameter("guideID", guideInfo.GuideID),
                new OleDbParameter("common", CleanString.InputText(guideInfo.Common,100)),
				new OleDbParameter("fieldDefineID", fieldDefineID)
				);
		}

		private GuideFieldDefineInfo getGuideFieldInfoByID(int fieldDefineID)
		{
			string sql = string.Format("SELECT ID, GuideID, Common FROM {0}.T_GuideFieldTypeDefineDict WHERE (ID = ?)",DataUser.ZLGL);

			DataTable reader = OracleOledbBase.ExecuteDataSet( sql, 
				new OleDbParameter("fieldDefineID", fieldDefineID)
				).Tables[0];

			if (reader.Rows.Count>0)
			{
				GuideFieldDefineInfo guideInfo = new GuideFieldDefineInfo();
				guideInfo.ID = Convert.ToInt32(reader.Rows[0]["ID"].ToString());
				guideInfo.GuideID = reader.Rows[0]["GuideID"].ToString();
				guideInfo.Common = reader.Rows[0]["Common"].ToString();

				//reader.Close();
				return guideInfo;
			}
			//reader.Close();
			return null;
		}

		private GuideFieldDefineInfo getGuideFieldInfoByPorpertyPage(System.Web.UI.Page curPage)
		{
			// 生成一个实体类
			GuideFieldDefineInfo guideInfo = new GuideFieldDefineInfo();
			// guideInfo.GuideID = Convert.ToInt32(((TextBox)curPage.FindControl("textGuideID")).Text);
			guideInfo.GuideID = ((DropDownList)curPage.FindControl("ddlGuideID")).SelectedValue;
			guideInfo.Common = ((TextBox)curPage.FindControl("textFieldCommon")).Text;

			return guideInfo;
		}
		#endregion

		private void ContenTypeList_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
	}
	public class MyLists:DropDownList
	{
		/// <summary>
		/// 
		/// </summary>
		public MyLists()
		{
		}

		private DataTable objectlist;
		public DataTable ObjectList
		{
			get
			{
				return this.objectlist;
			}
			set
			{
				this.objectlist = value;
			}
		}
	}
}
