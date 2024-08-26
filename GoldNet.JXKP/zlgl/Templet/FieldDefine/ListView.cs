using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;

namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// 模板的列表视图对象 
	/// </summary>
	[Serializable()]
	public class ListView
	{
		#region --私有变量-- 
		private TempletBO _templet;
		private ListViewInfo _view;
		private FieldCollection _displayFields;
		private SortFieldsBO _sortFields;
		private FilterCollection _fieldFilters; 
		private CollectCollection _collectFields;

		private string _rowFilter;
		private string _sortStr;
		#endregion

		#region --内部实体类VO-- 
		/// <summary>
		/// 视图对象VO
		/// </summary>
		[Serializable()]
		internal class ListViewInfo
		{
			/// <summary>
			/// 视图编号
			/// </summary>
			public int ID;
			/// <summary>
			/// 视图名称
			/// </summary>
			public string Name;
			/// <summary>
			/// 模板编号
			/// </summary>
			public int TempletID;
			/// <summary>
			/// 视图属性：页面显示的记录数
			/// </summary>
			public int PageCount;
		}
		#endregion
		
		#region --构造函数-- 
		/// <summary>
		/// 构造函数(构造一个临时模板)
		/// </summary>
		/// <param name="templet">创建视图的模板对象</param>
		/// <param name="viewId">视图编号</param>
		public ListView(TempletBO templet, int viewId)
		{
			// 保存模板信息
			this._templet = templet;

			#region --依据编号构建视图信息VO，如果不存在，抛出异常--
			string sql; 

			sql = string.Format("SELECT ID, Name, TempletID, PageCount FROM {0}.T_ViewDict WHERE (ID = ?)",DataUser.ZLGL);
			System.Data.DataTable reader = OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter("id", viewId)).Tables[0];
			for (int i=0;i<reader.Rows.Count;i++)
			{
				_view = new ListViewInfo();

				_view.ID = Convert.ToInt32(reader.Rows[i]["ID"].ToString());
				_view.Name = reader.Rows[i]["Name"].ToString();
				_view.TempletID = Convert.ToInt32(reader.Rows[i]["TempletID"].ToString());
				_view.PageCount = Convert.ToInt32(reader.Rows[i]["PageCount"].ToString());
			}
			#endregion
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="templet">创建视图的模板对象</param>
		public ListView(TempletBO templet)
		{
			this._templet = templet;

			this._view = new ListViewInfo();
			this._view.ID = 0;
			this._view.Name = "临时视图";
			this._view.PageCount = 0;
			this._view.TempletID = this._templet.ID;
		}
		#endregion

		#region --属性-- 
		/// <summary>
		/// 视图编号
		/// </summary>
		public int ID
		{
			get
			{
				return _view.ID;
			}
		}

		/// <summary>
		/// 视图名称
		/// </summary>
		public string Name
		{
			get
			{
				return _view.Name;
			}
		}

		/// <summary>
		/// 模板编号
		/// </summary>
		public int TempletID
		{
			get
			{
				return _view.TempletID;
			}
		}
		/// <summary>
		/// 视图属性：页面显示的记录数
		/// </summary>
		public int PageCount
		{
			get
			{
				return _view.PageCount;
			}
			set
			{
				this._view.PageCount = value;
			}
		}


		/// <summary>
		/// 返回视图的显示字段集合
		/// </summary>
		public FieldCollection DisplayFields
		{
			get
			{
				if (_displayFields == null) _displayFields = _templet.GetFields(this.ID);
				return _displayFields;
			}
			set
			{
				_displayFields = value;
			}
		}
		/// <summary>
		/// 返回排序对象
		/// </summary>
		public SortFieldsBO SortFields
		{
			get
			{
				if (_sortFields == null) _sortFields = new SortFieldsBO(this);
				return _sortFields;
			}
			set
			{
				_sortFields = value;
			}
		}

		/// <summary>
		/// 返回筛选字段集合对象
		/// </summary>
		public FilterCollection FieldFilters
		{
			get
			{
				if (_fieldFilters == null) _fieldFilters = new FilterCollection(this);
				return _fieldFilters;
			}
			set
			{
				_fieldFilters = value;
			}
		}
		/// <summary>
		/// 返回汇总字段集合对象
		/// </summary>
		public CollectCollection CollectFields
		{
			get
			{
				if (_collectFields == null)
					_collectFields = new CollectCollection(this);
				return _collectFields;
			}
			set
			{
				_collectFields = value;
			}
		}


		/// <summary>
		/// 返回视图的过滤字符串
		/// </summary>
		public string RowFilter
		{
			get
			{
				if (this._rowFilter == null || this._rowFilter == "" || this._rowFilter == string.Empty)
				{
					string[] filterStrs = new string[this.FieldFilters.Count];
					for (int i=0; i<this.FieldFilters.Count; i++)
					{
						filterStrs[i] = FieldFilters[i].FilterField.GetFilter(
							FieldFilters[i].ComparisonOperator,
							FieldFilters[i].ComparisonValues
							);
					}
					this._rowFilter = string.Join(" AND ", filterStrs);
				}
				return this._rowFilter;
			}
		}

		/// <summary>
		/// 返回视图的排序字符串
		/// </summary>
		public string SortStr
		{
			get
			{
				if (this._sortStr == null)
				{
					this._sortStr = this.SortFields.GetSortStr();
				}
				return this._sortStr;
			}
		}
		#endregion

		#region --视图维护方法-- 
		/// <summary>
		/// 更新视图的基本信息
		/// </summary>
		/// <param name="viewName">视图名称</param>
		/// <param name="pageCount">页面显示数</param>
		public void UpdateViewInfo(OleDbTransaction trans,string viewName, int pageCount)
		{
			string sql = string.Format("UPDATE {0}.T_ViewDict  SET Name = ?, PageCount = ?  WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, 
				new OleDbParameter("viewName", viewName),
				new OleDbParameter("pageCount", pageCount),
				new OleDbParameter("viewID", this.ID));
		}
		/// <summary>
		/// 添加一个字段到显示字段集合
		/// </summary>
		/// <param name="fieldID">字段编号</param>
		public void AddDisplayField(OleDbTransaction trans,int fieldID)
		{
			// 先判断字段是否已经在视图中，不在的话添加，同时更新排序号。
			string sql;
			bool existed;

			sql = string.Format("SELECT COUNT(ViewID) AS countnum FROM {0}.T_ViewDisplayFiledsDict WHERE (ViewID = ?) AND (FieldID = ?)",DataUser.ZLGL);
			existed = Convert.ToBoolean(OracleOledbBase.ExecuteScalar(sql, 
				new OleDbParameter("viewID", this.ID),
				new OleDbParameter("fieldID", fieldID)
				));

			if (!existed)
			{
				sql = string.Format("INSERT INTO {0}.T_ViewDisplayFiledsDict (ViewID, FieldID) VALUES (?,?)",DataUser.ZLGL);

                OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, 
					new OleDbParameter("viewID", this.ID),
					new OleDbParameter("fieldID", fieldID)
					);
			}

			_displayFields = null;
		}
        public void AddDisplayField(int fieldID)
        {
            // 先判断字段是否已经在视图中，不在的话添加，同时更新排序号。
            string sql;
            bool existed;

            sql = string.Format("SELECT COUNT(ViewID) AS countnum FROM {0}.T_ViewDisplayFiledsDict WHERE (ViewID = ?) AND (FieldID = ?)", DataUser.ZLGL);
            existed = Convert.ToBoolean(OracleOledbBase.ExecuteScalar(sql,
                new OleDbParameter("viewID", this.ID),
                new OleDbParameter("fieldID", fieldID)
                ));

            if (!existed)
            {
                sql = string.Format("INSERT INTO {0}.T_ViewDisplayFiledsDict (ViewID, FieldID) VALUES (?,?)", DataUser.ZLGL);

                OracleOledbBase.ExecuteNonQuery(TempletBO.CONNECT_STRING, sql,
                    new OleDbParameter("viewID", this.ID),
                    new OleDbParameter("fieldID", fieldID)
                    );
            }

            _displayFields = null;
        }

		/// <summary>
		/// 从显示字段集合删除一个字段
		/// </summary>
		/// <param name="fieldID">字段编号</param>
        public void DeleteDisplayField(OleDbTransaction trans, int fieldID)
		{
			// 先判断字段是否已经在视图中，不在的话添加，同时更新排序号。
			string sql;

			sql = string.Format("DELETE FROM {0}.T_ViewDisplayFiledsDict WHERE (ViewID = ?) AND (FieldID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, 
				new OleDbParameter("viewID", this.ID),
				new OleDbParameter("fieldID", fieldID)
				);
		}

		/// <summary>
		/// 更新排序字段
		/// </summary>
		/// <param name="fristSortFieldID">主要排序字段编号</param>
		/// <param name="fristIsDesc">主要排序字段方向</param>
		/// <param name="secondSortFieldID">次要排序字段编号</param>
		/// <param name="seondIsDesc">次要排序字段方向</param>
        public void UpdateSortField(OleDbTransaction trans, int fristSortFieldID, bool fristIsDesc, int secondSortFieldID, bool seondIsDesc)
		{
			// 先判断字段是否已经在视图中，不在的话添加，同时更新排序号。
			string sql;
			//bool existed;

            //sql = string.Format("SELECT COUNT(ViewID) AS Have FROM {0}.T_ViewSortDict WHERE (ViewID = ?)",DataUser.ZLGL);
            //existed = Convert.ToBoolean(OracleOledbBase.ExecuteScalar(sql, 
            //    new OleDbParameter("viewID", this.ID)
            //    ));

            //if (existed)
            //{
            //    // SQL:更新排序字段
            //    sql = string.Format("UPDATE {0}.T_ViewSortDict SET FristFieldID =?, FristSortDesc =?, SecondFieldID =?, SecondSortDesc =?  WHERE (ViewID = ?)",DataUser.ZLGL);
            //}
            //else
			{
				// SQL:插入排序字段
				sql = string.Format("INSERT INTO {0}.T_ViewSortDict (ViewID, FristFieldID, FristSortDesc, SecondFieldID, SecondSortDesc) VALUES (?, ?,?, ?,?)",DataUser.ZLGL);
			}

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, 
				new OleDbParameter("viewID", this.ID),
				new OleDbParameter("fristFieldID", fristSortFieldID),
				new OleDbParameter("fristSortDesc", fristIsDesc),
				new OleDbParameter("secondFieldID", secondSortFieldID),
				new OleDbParameter("secondSortDesc", seondIsDesc));

			_sortFields = null;
		}

		/// <summary>
		/// 删除排序字段
		/// </summary>
		public void DelSortField(OleDbTransaction trans)
		{
			string sql = string.Format("DELETE FROM {0}.T_ViewSortDict WHERE (ViewID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, 
				new OleDbParameter("viewID", this.ID));
		}

		/// <summary>
		/// 添加一个过滤条件
		/// </summary>
		/// <param name="fieldID">字段编号</param>
		/// <param name="compOperator">比较符</param>
		/// <param name="valus">比较值</param>
		public void AddFilter(OleDbTransaction trans,int fieldID, string compOperator, string valus)
		{
			string sql = string.Format("INSERT INTO {0}.T_ViewFilterDict (ViewID, FieldID, ComparisonOperator, ComparisonValues) VALUES (?, ?,?,?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, 
				new OleDbParameter("viewID", this.ID),
				new OleDbParameter("fieldID", fieldID),
				new OleDbParameter("compOperator", compOperator),
				new OleDbParameter("values", valus));

			_fieldFilters = null;
		}

		/// <summary>
		/// 删除所有的过滤字段
		/// </summary>
		public void DelAllFilter(OleDbTransaction trans)
		{
			string sql = string.Format("DELETE FROM {0}.T_ViewFilterDict WHERE (ViewID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, new OleDbParameter("viewID", this.ID));

			_fieldFilters = null;
		}


        public void AddCollect(OleDbTransaction trans, int fieldID, string collectMode)
		{
			string sql = string.Format("INSERT INTO {0}.T_ViewCollectDict (ViewID, FieldID, CollectMode) VALUES (?,?,?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text,sql, 
				new OleDbParameter("viewID", this.ID),
				new OleDbParameter("fieldID", fieldID),
				new OleDbParameter("collectMode", collectMode));

			_collectFields = null;
		}

		/// <summary>
		/// 删除所有汇总字段
		/// </summary>
		public void DelAllCollect(OleDbTransaction trans)
		{
			string sql = string.Format("DELETE FROM {0}.T_ViewCollectDict WHERE (ViewID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, new OleDbParameter("viewID", this.ID));

			_collectFields = null;
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
	public class SortFieldsBO
	{
		#region --私有变量-- 
		private ListView _view;
		private SortFieldInfo _sortFieldInfo;
		private Field _fristField;
		private Field _secondField;
		#endregion

		#region --内部实体类-- 
		/// <summary>
		/// 列表排序属性
		/// </summary>
		[Serializable()]
		public class SortFieldInfo
		{
			/// <summary>
			/// 主要排序字段
			/// </summary>
			public int FristSortField;
			/// <summary>
			/// 主要排序方向
			/// </summary>
			public bool FristSortDesc;
			/// <summary>
			/// 次要排序字段
			/// </summary>
			public int SecondSortField;
			/// <summary>
			/// 次要排序方向
			/// </summary>
			public bool SecondSortDesc;
		}
		#endregion

		#region --构造函数-- 
		/// <summary>
		/// 依据已有视图构造一个排序对象
		/// </summary>
		/// <param name="view"></param>
		public SortFieldsBO(ListView view)
		{
			_view = view;

			string sql = string.Format("SELECT FristFieldID, FristSortDesc, SecondFieldID, SecondSortDesc FROM {0}.T_ViewSortDict WHERE (ViewID = ?)",DataUser.ZLGL);

			System.Data.DataTable reader = OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter("viewID", view.ID)).Tables[0];

			_sortFieldInfo = new SortFieldInfo();

			for (int i=0;i<reader.Rows.Count;i++)
			{
				_sortFieldInfo.FristSortField = Convert.ToInt32(reader.Rows[i]["FristFieldID"].ToString());
				_sortFieldInfo.FristSortDesc = reader.Rows[i]["FristSortDesc"].ToString()=="-1"?false:true;
				_sortFieldInfo.SecondSortField = Convert.ToInt32(reader.Rows[i]["SecondFieldID"].ToString());
				_sortFieldInfo.SecondSortDesc = reader.Rows[i]["SecondSortDesc"].ToString()=="0"?true:false;
				_fristField = new Field(_sortFieldInfo.FristSortField);
				if (_sortFieldInfo.SecondSortField != 0)
					_secondField = new Field(_sortFieldInfo.SecondSortField);
			}
			
		}

		/// <summary>
		/// 依据参数构造一个临时视图的排序对象
		/// </summary>
		/// <param name="fristSortFieldID"></param>
		/// <param name="fristDesc"></param>
		/// <param name="secondSortFieldID"></param>
		/// <param name="secondDesc"></param>
		public SortFieldsBO(int fristSortFieldID, bool fristDesc, int secondSortFieldID, bool secondDesc)
		{
			_sortFieldInfo = new SortFieldInfo();

			_sortFieldInfo.FristSortField = fristSortFieldID;
			_sortFieldInfo.FristSortDesc = fristDesc;
			_sortFieldInfo.SecondSortField = secondSortFieldID;
			_sortFieldInfo.SecondSortDesc = secondDesc;

			if (fristSortFieldID != 0)
			{
				_fristField = new Field(_sortFieldInfo.FristSortField);
				if (_sortFieldInfo.SecondSortField != 0)
					_secondField = new Field(_sortFieldInfo.SecondSortField);
			}
		}
		#endregion

		#region --属性-- 
		/// <summary>
		/// 主要排序字段，如果为null，则无排序条件。
		/// </summary>
		public Field FristSortField
		{
			get
			{
				return _fristField;
			}
		}

		public Field SecondSortField
		{
			get
			{
				return _secondField;
			}
		}

		/// <summary>
		/// 次要排序条件，如果为null，则无次要排序条件
		/// </summary>
		public bool FristSortDesc
		{
			get
			{
				return _sortFieldInfo.FristSortDesc;
			}
		}

		public bool SecondSortDesc
		{
			get
			{
				return _sortFieldInfo.SecondSortDesc;
			}
		}

		public ListView view
		{
			get
			{
				return _view;
			}
		}

		/// <summary>
		/// 得到排序字段。
		/// </summary>
		/// <returns>排序字符串</returns>
		public string GetSortStr()
		{
			if (this.FristSortField != null)
			{
				string result = this.FristSortField.ListDisplayDataName;
				if (this.FristSortDesc)	result += " Desc";

				if (this.SecondSortField != null)
				{
					result += ", " + this.SecondSortField.ListDisplayDataName;
					if (this.SecondSortDesc) result += " Desc";
				}

				return result;
			}
			else
			{
				return "";
			}
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
	public class FilterBO
	{
		#region --私有变量-- 
		// private int _filterFieldID;
		private string _comparisonOperator;
		private string _comparisonValues;
		private Field _filterField;
		#endregion

		#region --构造函数-- 
		public FilterBO(int filterFieldID, string comparisonOperator, string comparisonValues)
		{
			// _filterFieldID = filterFieldID;
			_comparisonOperator = comparisonOperator;
			_comparisonValues = comparisonValues;

			_filterField = new Field(filterFieldID);

		}
		#endregion

		#region --属性-- 
		/// <summary>
		/// 过滤字段
		/// </summary>
		public Field FilterField
		{
			get
			{
				return _filterField;
			}
		}

		/// <summary>
		/// 比较符
		/// </summary>
		public string ComparisonOperator
		{
			get
			{
				return _comparisonOperator;
			}
		}

		/// <summary>
		/// 值
		/// </summary>
		public string ComparisonValues
		{
			get
			{
				return _comparisonValues;
			}
		}
		#endregion
	}

	/// <summary>
	/// 过滤条件集合
	/// </summary>
	[Serializable()]
	public class FilterCollection : System.Collections.CollectionBase
	{
		#region --私有变量-- 
		private ListView _view;
		#endregion

		#region --构造函数-- 
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="view"></param>
		public FilterCollection(ListView view)
		{
			_view = view;

			string sql = string.Format("SELECT FieldID, ComparisonOperator, ComparisonValues FROM {0}.T_ViewFilterDict WHERE (ViewID = ?)",DataUser.ZLGL);

			System.Data.DataTable reader = OracleOledbBase.ExecuteDataSet( sql, new OleDbParameter("viewID", view.ID)).Tables[0];

			for (int i=0;i<reader.Rows.Count;i++)
			{
				FilterBO filter = new FilterBO(Convert.ToInt32(reader.Rows[i]["FieldID"].ToString()), reader.Rows[i]["ComparisonOperator"].ToString(), reader.Rows[i]["ComparisonValues"].ToString());

				this.Add(filter);
			}
		}

		/// <summary>
		/// 构造一个空过滤集合,用于产生临时视图的过滤条件
		/// </summary>
		public FilterCollection(){}
		#endregion

		#region --集合方法-- 
		/// <summary>
		/// 实加对象到集合中
		/// </summary>
		/// <param name="filter">要添加集合的对象</param>
		public void Add(FilterBO filter)
		{
			List.Add(filter);
		}

		/// <summary>
		/// 根据索引号返回集合中的对象
		/// </summary>
		public FilterBO this[int index]
		{
			get
			{
				return (FilterBO) List[index];
			}
		}

		/// <summary>
		/// 判断字段是否在查询字段集合中
		/// </summary>
		/// <param name="field">字段对象</param>
		/// <returns>一个bool值，是否已经包含此字段的过滤条件对象</returns>
		public FilterBO Contains(Field field)
		{
			foreach (FilterBO filterBO in List)
				if (filterBO.FilterField.ID == field.ID) return filterBO;
			return null;
		}
		#endregion

		#region --属性-- 
		/// <summary>
		/// 返回排序字段的个数。
		/// </summary>
		public new int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		public ListView view
		{
			get
			{
				return _view;
			}
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
	public class CollectBO
	{
		#region --私有变量-- 
		// private int _collectFieldID;
		private string _collectMode;
		private Field _collectField;
		#endregion

		#region --构造函数-- 
		public CollectBO(int collectFieldID, string collectMode)
		{
			// _collectFieldID = collectFieldID;
			_collectMode = collectMode;

			_collectField = new Field(collectFieldID);
		}
		#endregion

		#region --属性-- 
		/// <summary>
		/// 汇总字段
		/// </summary>
		public Field CollectField
		{
			get
			{
				return _collectField;
			}
		}

		/// <summary>
		/// 汇总模式
		/// </summary>
		public string CollectMode
		{
			get
			{
				return _collectMode;
			}
		}

		/// <summary>
		/// 返回计算的字符串
		/// </summary>
		public string CollectComputerString
		{
			get
			{
				return this._collectField.GetCollectComputerString(this.CollectMode);
			}
		}
		#endregion
	}

	/// <summary>
	/// 汇总条件集合
	/// </summary>
	[Serializable()]
	public class CollectCollection : System.Collections.CollectionBase
	{
		#region --私有变量-- 
		private ListView _view;
		#endregion

		#region --构造函数-- 
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="view"></param>
		public CollectCollection(ListView view)
		{
			_view = view;

			string sql = string.Format("SELECT FieldID, CollectMode FROM {0}.T_ViewCollectDict WHERE (ViewID = ?)",DataUser.ZLGL);

			System.Data.DataTable reader = OracleOledbBase.ExecuteDataSet(sql, new OleDbParameter("viewID", view.ID)).Tables[0];

			for (int i=0;i<reader.Rows.Count;i++)
			{
				CollectBO collect = new CollectBO(Convert.ToInt32(reader.Rows[i]["FieldID"].ToString()), reader.Rows[i]["CollectMode"].ToString());

				this.Add(collect);
			}
			//reader.Close();
		}

		/// <summary>
		/// 构造一个空汇总集合,用于产生临时视图的汇总条件
		/// </summary>
		public CollectCollection(){}
		#endregion

		#region --集合方法-- 
		/// <summary>
		/// 实加对象到集合中
		/// </summary>
		/// <param name="collect">要添加集合的对象</param>
		public void Add(CollectBO collect)
		{
			List.Add(collect);
		}

		/// <summary>
		/// 根据索引号返回集合中的对象
		/// </summary>
		public CollectBO this[int index]
		{
			get
			{
				return (CollectBO) List[index];
			}
		}



		/// <summary>
		/// 判断字段是否在查询字段集合中
		/// </summary>
		/// <param name="field">字段对象</param>
		/// <returns>一个bool值，是否已经包含此字段的过滤条件对象</returns>
		public CollectBO Contains(Field field)
		{
			foreach (CollectBO collect in List)
				if (collect.CollectField.ID == field.ID) return collect;
			return null;
		}
		#endregion

		#region --属性-- 
		/// <summary>
		/// 返回排序字段的个数。
		/// </summary>
		public new int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		/// <summary>
		/// 返回视图
		/// </summary>
		public ListView view
		{
			get
			{
				return _view;
			}
		}

		#endregion
	}
}
