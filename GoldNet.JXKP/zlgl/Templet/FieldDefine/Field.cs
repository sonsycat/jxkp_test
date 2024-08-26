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
	/// ģ����ֶζ���
	/// </summary>
	[Serializable()]
	public class Field
	{
		private FieldInfo _fieldInfo;
		private IFieldType _fieldtype;

		#region --�ڲ�ʵ����VO-- 
		/// <summary>
		/// �ֶζ����ʵ����
		/// </summary>
		[Serializable()]
		internal class FieldInfo
		{
			/// <summary>
			/// ���
			/// </summary>
			public int ID;
			/// <summary>
			/// �ֶ�����
			/// </summary>
			public string FieldName;
			/// <summary>
			/// ģ����
			/// </summary>
			public int TempletID;
			/// <summary>
			/// �ֶ����ͱ��
			/// </summary>
			public int FieldTypeID;
			/// <summary>
			/// �ֶ������������еı��
			/// </summary>
			public int FieldDefineID;
			/// <summary>
			/// �ֶ������
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

		#region --���캯��-- 
		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="fieldID">�ֶα��(����Ϊ0)</param>
		public Field(int fieldID)
		{
			//			// �������Ϊ0������һ���յ�ָ�����
			//			if (fieldID == 0)
			//				this._fieldInfo = new FieldInfo(0, "", 0, 0, 0, 0);
			//			else
			this._fieldInfo = this.getInfoByID(fieldID);
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="fieldInfo">�ֶ���Ϣ��ʵ��(����Ϊnull)</param>
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

		#region --����-- 
		/// <summary>
		/// ���
		/// </summary>
		public int ID
		{
			get
			{
				return this._fieldInfo.ID;
			}
		}
		/// <summary>
		/// �ֶ�����
		/// </summary>
		public string FieldName
		{
			get
			{
				return this._fieldInfo.FieldName;
			}
		}
		/// <summary>
		/// ģ����
		/// </summary>
		public int TempletID
		{
			get
			{
				return this._fieldInfo.TempletID;
			}
		}
		/// <summary>
		/// �ֶ����ͱ��
		/// </summary>
		public int FieldTypeID
		{
			get
			{
				return this._fieldInfo.FieldTypeID;
			}
		}
		/// <summary>
		/// �ֶ������������еı��
		/// </summary>
		public int FieldDefineID
		{
			get
			{
				return this._fieldInfo.FieldDefineID;
			}
		}
		/// <summary>
		/// �ֶ������
		/// </summary>
		public int SortNum
		{
			get
			{
				return this._fieldInfo.SortNum;
			}
		}


		/// <summary>
		/// �����ֶ���������
		/// </summary>
		public string FieldTypeName
		{
			get
			{
				return FieldTypeObj.FieldTypeName;
			}
		}

		/// <summary>
		/// �õ��ֶ����Ͷ���
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
		/// �����б���ʾʱ,Ҫ�󶨵������е�����.
		/// </summary>
		public string ListDisplayDataName
		{
			get
			{
				return this.FieldTypeObj.ListDisplayDataName;
			}
		}
		/// <summary>
		/// �����б���ʾʱ��Ҫ�󶨵������еĸ�ʽ���ַ�����
		/// </summary>
		public string ListDataFormatString
		{
			get
			{
				return this.FieldTypeObj.ListDataFormatString;
			}
		}
		/// <summary>
		/// ���ع��˵Ŀ�ѡ�������ַ���
		/// </summary>
		public string[] FilterOperators
		{
			get
			{
				return this.FieldTypeObj.FilterOperators;
			}
		}
		/// <summary>
		/// ���ػ��ܵĿ�ѡ�������ַ���
		/// </summary>
		public string[] CollectModes
		{
			get
			{
				return this.FieldTypeObj.CollectModes;
			}
		}
		#endregion

		#region --���з���-- 
		/// <summary>
		/// ���������
		/// </summary>
		/// <param name="sortNum">�������</param>
		public void UpdataFieldSortNum(int sortNum)
		{
			string sql = string.Format("UPDATE {0}.T_TempletFieldDict SET SortNum = ? WHERE (ID = ?)",DataUser.ZLGL);

			OracleOledbBase.ExecuteNonQuery(sql, 
				new OleDbParameter("sortNum", sortNum),
				new OleDbParameter("fieldID", this.ID));
		}

		/// <summary>
		/// �����ֶζ�����������Զ�����Ϣ
		/// ����Ҳ�������һ���µ��ֶζ��壬������Ƚ��鷳��
		/// </summary>
		/// <param name="curPage">��ʾ��ҳ��</param>
		/// <param name="fieldDefineSql">���ڶ��������ֶε�SQL���</param>
		public void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql)
		{
			this.FieldTypeObj.UpdateSpecialPropertyFormPage(curPage, ref fieldDefineSql);
		}

		/// <summary>
		/// ��ָ���ؼ�����ʾһ��¼��ؼ���
		/// </summary>
		/// <param name="curCtrl">ָ����ҳ��ؼ�</param>
		public void ShowInputControl(Control curCtrl,bool pass)
		{
			string str1=curCtrl.ID;
			string str2=this.FieldTypeObj.FieldTypeName;
			this.FieldTypeObj.ShowInputControl(curCtrl, pass);
		}

		/// <summary>
		/// ��ָ���ؼ�����ʾһ���ֶ����Զ����ҳ��
		/// </summary>
		/// <param name="curCtrl">Ҫ��ʾ�Ŀؼ�</param>
		public void ShowSpecialProperty(Control curCtrl)
		{
			this.FieldTypeObj.ShowSpecialProperty(curCtrl);
		}

		/// <summary>
		/// ����һ����¼
		/// </summary>
		/// <param name="tabName">��ʵ���ݱ�</param>
		/// <param name="recId">��¼��</param>
		/// <param name="curPage">�������ݵ�ҳ��</param>
		public void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id)
		{
			this.FieldTypeObj.UpdateRecord(myTrans,tabName, recId, curPage,id);
		}

		/// <summary>
		/// ��ʾ�鿴����
		/// </summary>
		/// <param name="tabName">��ʵ���ݱ�</param>
		/// <param name="recID">��¼��</param>
		/// <param name="curCtrl">ָ����ҳ��ؼ�</param>
		public void ShowViewData(string tabName, int recID, Control curCtrl)
		{
			this.FieldTypeObj.ShowViewData(tabName, recID, curCtrl);
		}

		/// <summary>
		/// ��ʾ�޸Ŀؼ�
		/// </summary>
		/// <param name="tabName">��ʵ���ݱ�</param>
		/// <param name="recID">��¼��</param>
		/// <param name="curCtrl">ָ����ҳ��ؼ�</param>
		public void ShowEditControl(string tabName, int recID, Control curCtrl,bool pass)
		{
			this.FieldTypeObj.ShowEditControl(tabName, recID, curCtrl,pass);
		}

		/// <summary>
		/// �õ�������Ϣ
		/// </summary>
		/// <param name="compOperator">�ȽϷ�</param>
		/// <param name="values">ֵ</param>
		/// <returns>�����ַ���</returns>
		public string GetFilter(string compOperator, string values)
		{
			return this.FieldTypeObj.GetFilter(compOperator, values);
		}
		/// <summary>
		/// �õ����ܼ��㷽���ַ���
		/// ����:"count(name)"-ͳ��name�еļ�¼����
		/// </summary>
		/// <param name="collectMode">ͳ�Ʒ����ַ���</param>
		/// <returns>���㷽���ַ���</returns>
		public string GetCollectComputerString(string collectMode)
		{
			return this.FieldTypeObj.ListCollectComputerString(collectMode);
		}
		#endregion

		#region --˽�з���-- 
		/// <summary>
		/// �õ�Info
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
	/// �ֶεļ�����
	/// </summary>
	[Serializable()]
	public class FieldCollection : CollectionBase
	{
		/// <summary>
		/// ��Ӷ��󵽼�����
		/// </summary>
		/// <param name="field">Ҫ��Ӽ��ϵĶ���</param>
		public void Add(Field field)
		{
			List.Add(field);
		}

		/// <summary>
		///���������ŷ��ؼ����еĶ���
		/// </summary>
		public Field this[int index]
		{
			get
			{
				return (Field) List[index];
			}
		}

		/// <summary>
		/// �ж��ֶ��Ƿ��Ѿ��ڼ����� 
		/// </summary>
		/// <param name="field">�ֶζ���</param>
		/// <returns>һ��boolֵ���Ƿ��Ѿ��������ֶζ���</returns>
		public bool Contains(Field field)
		{
			foreach (Field fieldin in List)
				if (fieldin.ID == field.ID) return true;
			return false;
		}

		/// <summary>
		/// ���ذ������ֶθ���
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
	/// �ֶεļ��ϱ���
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
