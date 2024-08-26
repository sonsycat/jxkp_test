using System;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI;
using System.Data.OleDb;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;


namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// 模板的字段对象
	/// </summary>
	[Serializable()]
	public class Field
	{
		private FieldInfo _fieldInfo;
		private IFieldType _fieldtype;

		#region --内部实体类VO-- 
		/// <summary>
		/// 字段对象的实体类
		/// </summary>
		[Serializable()]
		internal class FieldInfo
		{
			/// <summary>
			/// 编号
			/// </summary>
			public int ID;
			/// <summary>
			/// 字段名称
			/// </summary>
			public string FieldName;
			/// <summary>
			/// 模板编号
			/// </summary>
			public int TempletID;
			/// <summary>
			/// 字段类型编号
			/// </summary>
			public int FieldTypeID;
			/// <summary>
			/// 字段在所在类型中的编号
			/// </summary>
			public int FieldDefineID;
			/// <summary>
			/// 字段排序号
			/// </summary>
			public int SortNum;

			public FieldInfo(int id, string fieldName, int templetID, int fieldTypeID, int fieldDefineID, int sortNum)
			{
				this.ID = id;
				this.FieldName = fieldName;
				this.TempletID = templetID;
				this.FieldTypeID = fieldTypeID;
				this.FieldDefineID = fieldDefineID;
				this.SortNum = sortNum;
			}
		}
		#endregion

		#region --构造函数-- 
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="fieldID">字段编号(不能为0)</param>
		public Field(int fieldID)
		{
			//			// 如果参数为0，建立一个空的指标对象。
			//			if (fieldID == 0)
			//				this._fieldInfo = new FieldInfo(0, "", 0, 0, 0, 0);
			//			else
			this._fieldInfo = this.getInfoByID(fieldID);
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="fieldInfo">字段信息的实体(不能为null)</param>
		internal Field(FieldInfo fieldInfo)
		{
			if (fieldInfo != null)
				this._fieldInfo = fieldInfo;
			else
				throw new FieldIDNotExistedException(0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="fieldName"></param>
		/// <param name="templetID"></param>
		/// <param name="fieldTypeID"></param>
		/// <param name="fieldDefineID"></param>
		/// <param name="sortNum"></param>
		internal Field(int id, string fieldName, int templetID, int fieldTypeID, int fieldDefineID, int sortNum)
		{
			this._fieldInfo = new FieldInfo(id, fieldName, templetID, fieldTypeID, fieldDefineID, sortNum);
		}
		#endregion

		#region --属性-- 
		/// <summary>
		/// 编号
		/// </summary>
		public int ID
		{
			get
			{
				return this._fieldInfo.ID;
			}
		}
		/// <summary>
		/// 字段名称
		/// </summary>
		public string FieldName
		{
			get
			{
				return this._fieldInfo.FieldName;
			}
		}
		/// <summary>
		/// 模板编号
		/// </summary>
		public int TempletID
		{
			get
			{
				return this._fieldInfo.TempletID;
			}
		}
		/// <summary>
		/// 字段类型编号
		/// </summary>
		public int FieldTypeID
		{
			get
			{
				return this._fieldInfo.FieldTypeID;
			}
		}
		/// <summary>
		/// 字段在所在类型中的编号
		/// </summary>
		public int FieldDefineID
		{
			get
			{
				return this._fieldInfo.FieldDefineID;
			}
		}
		/// <summary>
		/// 字段排序号
		/// </summary>
		public int SortNum
		{
			get
			{
				return this._fieldInfo.SortNum;
			}
		}


		/// <summary>
		/// 返回字段类型名称
		/// </summary>
		public string FieldTypeName
		{
			get
			{
				return FieldTypeObj.FieldTypeName;
			}
		}

		/// <summary>
		/// 得到字段类型对象。
		/// </summary>
		internal IFieldType FieldTypeObj
		{
			
			get
			{
				if (_fieldtype == null)
				{
					// FieldTypeObjsPool pool = FieldTypeObjsPool.GetPool();
					// _fieldtype = pool.GetFieldobjByID(this._fieldInfo.FieldTypeID);

					_fieldtype = FieldTypeFactory.CreateFieldTypeObj(this.FieldTypeID, this);
				}

				return _fieldtype;
			}
		}

		/// <summary>
		/// 返回列表显示时,要绑定的数据列的名称.
		/// </summary>
		public string ListDisplayDataName
		{
			get
			{
				return this.FieldTypeObj.ListDisplayDataName;
			}
		}
		/// <summary>
		/// 返回列表显示时，要绑定的数据列的格式化字符串。
		/// </summary>
		public string ListDataFormatString
		{
			get
			{
				return this.FieldTypeObj.ListDataFormatString;
			}
		}
		/// <summary>
		/// 返回过滤的可选方法的字符串
		/// </summary>
		public string[] FilterOperators
		{
			get
			{
				return this.FieldTypeObj.FilterOperators;
			}
		}
		/// <summary>
		/// 返回汇总的可选方法的字符串
		/// </summary>
		public string[] CollectModes
		{
			get
			{
				return this.FieldTypeObj.CollectModes;
			}
		}
		#endregion

		#region --公有方法-- 
		/// <summary>
		/// 更新排序号
		/// </summary>
		/// <param name="sortNum">新排序号</param>
		public void UpdataFieldSortNum(int sortNum)
		{
			string sql = string.Format("UPDATE {0}.T_TempletFieldDict SET SortNum = ? WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(sql, 
				new OleDbParameter("sortNum", sortNum),
				new OleDbParameter("fieldID", this.ID));
		}

		/// <summary>
		/// 更新字段定义的特殊属性定义信息
		/// 这里也可以添加一个新的字段定义，但好像比较麻烦。
		/// </summary>
		/// <param name="curPage">显示的页面</param>
		/// <param name="fieldDefineSql">用于定义数据字段的SQL语句</param>
		public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			this.FieldTypeObj.UpdateSpecialPropertyFormPage(curPage, ref fieldDefineSql);
		}

		/// <summary>
		/// 在指定控件中显示一个录入控件。
		/// </summary>
		/// <param name="curCtrl">指定的页面控件</param>
		public void ShowInputControl(Control curCtrl,bool pass)
		{
			string str1=curCtrl.ID;
			string str2=this.FieldTypeObj.FieldTypeName;
			this.FieldTypeObj.ShowInputControl(curCtrl, pass);
		}

		/// <summary>
		/// 在指定控件中显示一个字段属性定义的页面
		/// </summary>
		/// <param name="curCtrl">要显示的控件</param>
		public void ShowSpecialProperty(Control curCtrl)
		{
			this.FieldTypeObj.ShowSpecialProperty(curCtrl);
		}

		/// <summary>
		/// 更新一条记录
		/// </summary>
		/// <param name="tabName">事实数据表</param>
		/// <param name="recId">记录号</param>
		/// <param name="curPage">包含数据的页面</param>
		public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id)
		{
			this.FieldTypeObj.UpdateRecord(myTrans,tabName, recId, curPage,id);
		}

		/// <summary>
		/// 显示查看数据
		/// </summary>
		/// <param name="tabName">事实数据表</param>
		/// <param name="recID">记录号</param>
		/// <param name="curCtrl">指定的页面控件</param>
		public void ShowViewData(string tabName, int recID, Control curCtrl)
		{
			this.FieldTypeObj.ShowViewData(tabName, recID, curCtrl);
		}

		/// <summary>
		/// 显示修改控件
		/// </summary>
		/// <param name="tabName">事实数据表</param>
		/// <param name="recID">记录号</param>
		/// <param name="curCtrl">指定的页面控件</param>
		public void ShowEditControl(string tabName, int recID, Control curCtrl,bool pass)
		{
			this.FieldTypeObj.ShowEditControl(tabName, recID, curCtrl,pass);
		}

		/// <summary>
		/// 得到过滤信息
		/// </summary>
		/// <param name="compOperator">比较符</param>
		/// <param name="values">值</param>
		/// <returns>过滤字符串</returns>
		public string GetFilter(string compOperator, string values)
		{
			return this.FieldTypeObj.GetFilter(compOperator, values);
		}
		/// <summary>
		/// 得到汇总计算方法字符串
		/// 类似:"count(name)"-统计name列的记录个数
		/// </summary>
		/// <param name="collectMode">统计方法字符串</param>
		/// <returns>计算方法字符串</returns>
		public string GetCollectComputerString(string collectMode)
		{
			return this.FieldTypeObj.ListCollectComputerString(collectMode);
		}
		#endregion

		#region --私有方法-- 
		/// <summary>
		/// 得到Info
		/// </summary>
		/// <param name="fieldID"></param>
		/// <returns></returns>
		private FieldInfo getInfoByID(int fieldID)
		{
			string sql;
			FieldInfo info = null;

			sql = string.Format("SELECT ID, FieldName, TempletID, FieldTypeID, FieldDefineID, SortNum FROM {0}.T_TempletFieldDict WHERE (ID = ?)",DataUser.ZLGL);

			System.Data.DataTable reader = OracleOledbBase.ExecuteDataSet(sql, 
				new OleDbParameter("fieldID", fieldID)
				).Tables[0];

			for (int i=0;i<reader.Rows.Count;i++)
			{
				info = new FieldInfo(
					Convert.ToInt32(reader.Rows[i]["ID"].ToString()), 
					reader.Rows[i]["FieldName"].ToString(),
					Convert.ToInt32(reader.Rows[i]["TempletID"].ToString()), 
					Convert.ToInt32(reader.Rows[i]["FieldTypeID"].ToString()), 
					Convert.ToInt32(reader.Rows[i]["FieldDefineID"].ToString()), 
					Convert.ToInt32(reader.Rows[i]["SortNum"].ToString()));
			}

			return info;
		}

		#endregion
	}

	/// <summary>
	/// 字段的集合类
	/// </summary>
	[Serializable()]
	public class FieldCollection : CollectionBase
	{
		/// <summary>
		/// 添加对象到集合中
		/// </summary>
		/// <param name="field">要添加集合的对象</param>
		public void Add(Field field)
		{
			List.Add(field);
		}

		/// <summary>
		///根据索引号返回集合中的对象
		/// </summary>
		public Field this[int index]
		{
			get
			{
				return (Field) List[index];
			}
		}

		/// <summary>
		/// 判断字段是否已经在集合中 
		/// </summary>
		/// <param name="field">字段对象</param>
		/// <returns>一个bool值，是否已经包含此字段对象</returns>
		public bool Contains(Field field)
		{
			foreach (Field fieldin in List)
				if (fieldin.ID == field.ID) return true;
			return false;
		}

		/// <summary>
		/// 返回包含的字段个数
		/// </summary>
		public new int Count
		{
			get
			{
				return List.Count;
			}
		}
	}

	/// <summary>
	/// 字段的集合表类
	/// </summary>
	public class FieldCollectionTable : CollectionTable
	{
		public override object this[int i]
		{
			get
			{
				return new Field(
					Convert.ToInt32(this.Rows[i][0]),
					Convert.ToString(this.Rows[i][1]),
					Convert.ToInt32(this.Rows[i][2]),
					Convert.ToInt32(this.Rows[i][3]),
					Convert.ToInt32(this.Rows[i][4]),
					Convert.ToInt32(this.Rows[i][5]));
			}
		}

		public int Count
		{
			get
			{
				return this.Rows.Count;
			}
		}

		public bool Contains(Field field)
		{
			foreach(Field _field in this)
			{
				if (_field.ID == field.ID)
					return true;
			}
			return false;
		}
	}
}
