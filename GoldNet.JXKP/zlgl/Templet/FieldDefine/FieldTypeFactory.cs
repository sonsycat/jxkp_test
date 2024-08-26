using System;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using GoldNet.JXKP.PowerManager;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm;


using GoldNet.JXKP.Templet.BLL.Fields;

namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// �ֶ����Ͷ���Ĺ����� 
	/// </summary>
	public class FieldTypeFactory
	{
		#region --�ڲ�ʵ��VO-- 
		/// <summary>
		/// �ֶ����Ͷ���Ļ�����ϢVO
		/// </summary>
		public class FieldTypeInfo
		{
			private int _id;
			private string _name;
			public string ClassName;
			public string Assembly;
			public string NameSpace;

			public FieldTypeInfo (int id, string name, string className, string assembly, string nameSpace)
			{
				this._id = id;
				this._name = name;
				this.ClassName = className;
				this.Assembly = assembly;
				this.NameSpace = nameSpace;
			}

			public int ID
			{
				get
				{
					return _id;
				}
			}

			public string Name
			{
				get
				{
					return _name;
				}
			}
		}

		/// <summary>
		/// �ֶ����Ͷ���Ļ�����Ϣ�ļ���
		/// </summary>
		public class FieldTypeCollect : CollectionBase
		{
			/// <summary>
			/// ��Ӷ��󵽼�����
			/// </summary>
			/// <param name="fieldTypeInfo">Ҫ��Ӽ��ϵĶ���</param>
			public void Add(FieldTypeInfo fieldTypeInfo)
			{
				List.Add(fieldTypeInfo);
			}

			/// <summary>
			/// ���������ŷ��ؼ����еĶ���
			/// </summary>
			public FieldTypeInfo this[int index]
			{
				get
				{
					return (FieldTypeInfo) List[index];
				}
			}
		}
		#endregion

		#region --���з���-- 
		/// <summary>
		/// ����һ���ֶζ����ʵ��
		/// </summary>
		/// <returns>�ֶ�ʵ��</returns>
		public static IFieldType CreateFieldTypeObj(int fieldTypeID, Field field)
		{
			string sql;
			FieldTypeInfo typeInfo=null;

			// ��ȡ�ֶ����Ͷ���Ļ�����Ϣ��
			sql = string.Format("SELECT ID, Name, ClassName, Assembly, NameSpace FROM {0}.T_FieldTypeDict WHERE (ID = ?)",DataUser.ZLGL);
			System.Data.DataTable reader = OracleOledbBase.ExecuteDataSet(sql, new OleDbParameter("id", fieldTypeID)).Tables[0];

			for (int i=0;i<reader.Rows.Count;i++)
			{
				typeInfo = new FieldTypeInfo(
					Convert.ToInt32(reader.Rows[i]["ID"].ToString()),
					reader.Rows[i]["Name"].ToString(),
					reader.Rows[i]["ClassName"].ToString(),
					reader.Rows[i]["Assembly"].ToString(),
					reader.Rows[i]["NameSpace"].ToString());
			}	

			// ���ش���ʵ��
			return (IFieldType)Assembly.Load(typeInfo.Assembly).CreateInstance(
				typeInfo.NameSpace + "." + typeInfo.ClassName,
				false,
				BindingFlags.Public | BindingFlags.Instance,
				null,
				new object[]{field},
				null,
				null
				);
           
		}

		/// <summary>
		/// �õ������ֶ����Ͷ�����Ϣ����
		/// </summary>
		/// <returns>�ֶ�������Ϣ����</returns>
		public static FieldTypeCollect GetAllFieldTypeInfo()
		{
			string sql;
			FieldTypeInfo typeInfo;
			FieldTypeCollect result;

			// ��ȡ�����ֶ�������Ϣ
			sql = string.Format("SELECT ID, Name, ClassName, Assembly, NameSpace FROM {0}.T_FieldTypeDict ORDER BY ID",DataUser.ZLGL);
			DataTable reader = OracleOledbBase.ExecuteDataSet(sql).Tables[0];

			result = new FieldTypeCollect();

			for(int i=0;i<reader.Rows.Count;i++)
			{
				typeInfo = new FieldTypeInfo(
					Convert.ToInt32(reader.Rows[i][0].ToString()),
					reader.Rows[i][1].ToString(),
					reader.Rows[i][2].ToString(),
					reader.Rows[i][3].ToString(),
					reader.Rows[i][4].ToString());

				result.Add(typeInfo);
			}
			
			
			return result;
		}
		#endregion
	}
}
