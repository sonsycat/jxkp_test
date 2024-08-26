using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;

namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// ģ����б���ͼ���� 
	/// </summary>
	[Serializable()]
	public class ListView
	{
		#region --˽�б���-- 
		private TempletBO _templet;
		private ListViewInfo _view;
		private FieldCollection _displayFields;
		private SortFieldsBO _sortFields;
		private FilterCollection _fieldFilters; 
		private CollectCollection _collectFields;

		private string _rowFilter;
		private string _sortStr;
		#endregion

		#region --�ڲ�ʵ����VO-- 
		/// <summary>
		/// ��ͼ����VO
		/// </summary>
		[Serializable()]
		internal class ListViewInfo
		{
			/// <summary>
			/// ��ͼ���
			/// </summary>
			public int ID;
			/// <summary>
			/// ��ͼ����
			/// </summary>
			public string Name;
			/// <summary>
			/// ģ����
			/// </summary>
			public int TempletID;
			/// <summary>
			/// ��ͼ���ԣ�ҳ����ʾ�ļ�¼��
			/// </summary>
			public int PageCount;
		}
		#endregion
		
		#region --���캯��-- 
		/// <summary>
		/// ���캯��(����һ����ʱģ��)
		/// </summary>
		/// <param name="templet">������ͼ��ģ�����</param>
		/// <param name="viewId">��ͼ���</param>
		public ListView(TempletBO templet, int viewId)
		{
			// ����ģ����Ϣ
			this._templet = templet;

			#region --���ݱ�Ź�����ͼ��ϢVO����������ڣ��׳��쳣--
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
		/// ���캯��
		/// </summary>
		/// <param name="templet">������ͼ��ģ�����</param>
		public ListView(TempletBO templet)
		{
			this._templet = templet;

			this._view = new ListViewInfo();
			this._view.ID = 0;
			this._view.Name = "��ʱ��ͼ";
			this._view.PageCount = 0;
			this._view.TempletID = this._templet.ID;
		}
		#endregion

		#region --����-- 
		/// <summary>
		/// ��ͼ���
		/// </summary>
		public int ID
		{
			get
			{
				return _view.ID;
			}
		}

		/// <summary>
		/// ��ͼ����
		/// </summary>
		public string Name
		{
			get
			{
				return _view.Name;
			}
		}

		/// <summary>
		/// ģ����
		/// </summary>
		public int TempletID
		{
			get
			{
				return _view.TempletID;
			}
		}
		/// <summary>
		/// ��ͼ���ԣ�ҳ����ʾ�ļ�¼��
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
		/// ������ͼ����ʾ�ֶμ���
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
		/// �����������
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
		/// ����ɸѡ�ֶμ��϶���
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
		/// ���ػ����ֶμ��϶���
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
		/// ������ͼ�Ĺ����ַ���
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
		/// ������ͼ�������ַ���
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

		#region --��ͼά������-- 
		/// <summary>
		/// ������ͼ�Ļ�����Ϣ
		/// </summary>
		/// <param name="viewName">��ͼ����</param>
		/// <param name="pageCount">ҳ����ʾ��</param>
		public void UpdateViewInfo(OleDbTransaction trans,string viewName, int pageCount)
		{
			string sql = string.Format("UPDATE {0}.T_ViewDict  SET Name = ?, PageCount = ?  WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, 
				new OleDbParameter("viewName", viewName),
				new OleDbParameter("pageCount", pageCount),
				new OleDbParameter("viewID", this.ID));
		}
		/// <summary>
		/// ���һ���ֶε���ʾ�ֶμ���
		/// </summary>
		/// <param name="fieldID">�ֶα��</param>
		public void AddDisplayField(OleDbTransaction trans,int fieldID)
		{
			// ���ж��ֶ��Ƿ��Ѿ�����ͼ�У����ڵĻ���ӣ�ͬʱ��������š�
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
            // ���ж��ֶ��Ƿ��Ѿ�����ͼ�У����ڵĻ���ӣ�ͬʱ��������š�
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
		/// ����ʾ�ֶμ���ɾ��һ���ֶ�
		/// </summary>
		/// <param name="fieldID">�ֶα��</param>
        public void DeleteDisplayField(OleDbTransaction trans, int fieldID)
		{
			// ���ж��ֶ��Ƿ��Ѿ�����ͼ�У����ڵĻ���ӣ�ͬʱ��������š�
			string sql;

			sql = string.Format("DELETE FROM {0}.T_ViewDisplayFiledsDict WHERE (ViewID = ?) AND (FieldID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, 
				new OleDbParameter("viewID", this.ID),
				new OleDbParameter("fieldID", fieldID)
				);
		}

		/// <summary>
		/// ���������ֶ�
		/// </summary>
		/// <param name="fristSortFieldID">��Ҫ�����ֶα��</param>
		/// <param name="fristIsDesc">��Ҫ�����ֶη���</param>
		/// <param name="secondSortFieldID">��Ҫ�����ֶα��</param>
		/// <param name="seondIsDesc">��Ҫ�����ֶη���</param>
        public void UpdateSortField(OleDbTransaction trans, int fristSortFieldID, bool fristIsDesc, int secondSortFieldID, bool seondIsDesc)
		{
			// ���ж��ֶ��Ƿ��Ѿ�����ͼ�У����ڵĻ���ӣ�ͬʱ��������š�
			string sql;
			//bool existed;

            //sql = string.Format("SELECT COUNT(ViewID) AS Have FROM {0}.T_ViewSortDict WHERE (ViewID = ?)",DataUser.ZLGL);
            //existed = Convert.ToBoolean(OracleOledbBase.ExecuteScalar(sql, 
            //    new OleDbParameter("viewID", this.ID)
            //    ));

            //if (existed)
            //{
            //    // SQL:���������ֶ�
            //    sql = string.Format("UPDATE {0}.T_ViewSortDict SET FristFieldID =?, FristSortDesc =?, SecondFieldID =?, SecondSortDesc =?  WHERE (ViewID = ?)",DataUser.ZLGL);
            //}
            //else
			{
				// SQL:���������ֶ�
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
		/// ɾ�������ֶ�
		/// </summary>
		public void DelSortField(OleDbTransaction trans)
		{
			string sql = string.Format("DELETE FROM {0}.T_ViewSortDict WHERE (ViewID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(trans,CommandType.Text, sql, 
				new OleDbParameter("viewID", this.ID));
		}

		/// <summary>
		/// ���һ����������
		/// </summary>
		/// <param name="fieldID">�ֶα��</param>
		/// <param name="compOperator">�ȽϷ�</param>
		/// <param name="valus">�Ƚ�ֵ</param>
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
		/// ɾ�����еĹ����ֶ�
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
		/// ɾ�����л����ֶ�
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
		#region --˽�б���-- 
		private ListView _view;
		private SortFieldInfo _sortFieldInfo;
		private Field _fristField;
		private Field _secondField;
		#endregion

		#region --�ڲ�ʵ����-- 
		/// <summary>
		/// �б���������
		/// </summary>
		[Serializable()]
		public class SortFieldInfo
		{
			/// <summary>
			/// ��Ҫ�����ֶ�
			/// </summary>
			public int FristSortField;
			/// <summary>
			/// ��Ҫ������
			/// </summary>
			public bool FristSortDesc;
			/// <summary>
			/// ��Ҫ�����ֶ�
			/// </summary>
			public int SecondSortField;
			/// <summary>
			/// ��Ҫ������
			/// </summary>
			public bool SecondSortDesc;
		}
		#endregion

		#region --���캯��-- 
		/// <summary>
		/// ����������ͼ����һ���������
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
		/// ���ݲ�������һ����ʱ��ͼ���������
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

		#region --����-- 
		/// <summary>
		/// ��Ҫ�����ֶΣ����Ϊnull����������������
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
		/// ��Ҫ�������������Ϊnull�����޴�Ҫ��������
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
		/// �õ������ֶΡ�
		/// </summary>
		/// <returns>�����ַ���</returns>
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
		#region --˽�б���-- 
		// private int _filterFieldID;
		private string _comparisonOperator;
		private string _comparisonValues;
		private Field _filterField;
		#endregion

		#region --���캯��-- 
		public FilterBO(int filterFieldID, string comparisonOperator, string comparisonValues)
		{
			// _filterFieldID = filterFieldID;
			_comparisonOperator = comparisonOperator;
			_comparisonValues = comparisonValues;

			_filterField = new Field(filterFieldID);

		}
		#endregion

		#region --����-- 
		/// <summary>
		/// �����ֶ�
		/// </summary>
		public Field FilterField
		{
			get
			{
				return _filterField;
			}
		}

		/// <summary>
		/// �ȽϷ�
		/// </summary>
		public string ComparisonOperator
		{
			get
			{
				return _comparisonOperator;
			}
		}

		/// <summary>
		/// ֵ
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
	/// ������������
	/// </summary>
	[Serializable()]
	public class FilterCollection : System.Collections.CollectionBase
	{
		#region --˽�б���-- 
		private ListView _view;
		#endregion

		#region --���캯��-- 
		/// <summary>
		/// ���캯��
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
		/// ����һ���չ��˼���,���ڲ�����ʱ��ͼ�Ĺ�������
		/// </summary>
		public FilterCollection(){}
		#endregion

		#region --���Ϸ���-- 
		/// <summary>
		/// ʵ�Ӷ��󵽼�����
		/// </summary>
		/// <param name="filter">Ҫ��Ӽ��ϵĶ���</param>
		public void Add(FilterBO filter)
		{
			List.Add(filter);
		}

		/// <summary>
		/// ���������ŷ��ؼ����еĶ���
		/// </summary>
		public FilterBO this[int index]
		{
			get
			{
				return (FilterBO) List[index];
			}
		}

		/// <summary>
		/// �ж��ֶ��Ƿ��ڲ�ѯ�ֶμ�����
		/// </summary>
		/// <param name="field">�ֶζ���</param>
		/// <returns>һ��boolֵ���Ƿ��Ѿ��������ֶεĹ�����������</returns>
		public FilterBO Contains(Field field)
		{
			foreach (FilterBO filterBO in List)
				if (filterBO.FilterField.ID == field.ID) return filterBO;
			return null;
		}
		#endregion

		#region --����-- 
		/// <summary>
		/// ���������ֶεĸ�����
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
		#region --˽�б���-- 
		// private int _collectFieldID;
		private string _collectMode;
		private Field _collectField;
		#endregion

		#region --���캯��-- 
		public CollectBO(int collectFieldID, string collectMode)
		{
			// _collectFieldID = collectFieldID;
			_collectMode = collectMode;

			_collectField = new Field(collectFieldID);
		}
		#endregion

		#region --����-- 
		/// <summary>
		/// �����ֶ�
		/// </summary>
		public Field CollectField
		{
			get
			{
				return _collectField;
			}
		}

		/// <summary>
		/// ����ģʽ
		/// </summary>
		public string CollectMode
		{
			get
			{
				return _collectMode;
			}
		}

		/// <summary>
		/// ���ؼ�����ַ���
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
	/// ������������
	/// </summary>
	[Serializable()]
	public class CollectCollection : System.Collections.CollectionBase
	{
		#region --˽�б���-- 
		private ListView _view;
		#endregion

		#region --���캯��-- 
		/// <summary>
		/// ���캯��
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
		/// ����һ���ջ��ܼ���,���ڲ�����ʱ��ͼ�Ļ�������
		/// </summary>
		public CollectCollection(){}
		#endregion

		#region --���Ϸ���-- 
		/// <summary>
		/// ʵ�Ӷ��󵽼�����
		/// </summary>
		/// <param name="collect">Ҫ��Ӽ��ϵĶ���</param>
		public void Add(CollectBO collect)
		{
			List.Add(collect);
		}

		/// <summary>
		/// ���������ŷ��ؼ����еĶ���
		/// </summary>
		public CollectBO this[int index]
		{
			get
			{
				return (CollectBO) List[index];
			}
		}



		/// <summary>
		/// �ж��ֶ��Ƿ��ڲ�ѯ�ֶμ�����
		/// </summary>
		/// <param name="field">�ֶζ���</param>
		/// <returns>һ��boolֵ���Ƿ��Ѿ��������ֶεĹ�����������</returns>
		public CollectBO Contains(Field field)
		{
			foreach (CollectBO collect in List)
				if (collect.CollectField.ID == field.ID) return collect;
			return null;
		}
		#endregion

		#region --����-- 
		/// <summary>
		/// ���������ֶεĸ�����
		/// </summary>
		public new int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		/// <summary>
		/// ������ͼ
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
