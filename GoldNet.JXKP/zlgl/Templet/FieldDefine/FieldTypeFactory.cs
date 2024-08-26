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
	/// 字段类型对象的工厂类 
	/// </summary>
	public class FieldTypeFactory
	{
		#region --内部实体VO-- 
		/// <summary>
		/// 字段类型对象的基本信息VO
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
		/// 字段类型对象的基本信息的集合
		/// </summary>
		public class FieldTypeCollect : CollectionBase
		{
			/// <summary>
			/// 添加对象到集合中
			/// </summary>
			/// <param name="fieldTypeInfo">要添加集合的对象</param>
			public void Add(FieldTypeInfo fieldTypeInfo)
			{
				List.Add(fieldTypeInfo);
			}

			/// <summary>
			/// 根据索引号返回集合中的对象
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

		#region --公有方法-- 
		/// <summary>
		/// 创建一个字段对象的实例
		/// </summary>
		/// <returns>字段实例</returns>
		public static IFieldType CreateFieldTypeObj(int fieldTypeID, Field field)
		{
			string sql;
			FieldTypeInfo typeInfo=null;

			// 获取字段类型对象的基本信息。
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

			// 返回创建实例
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
		/// 得到所有字段类型对象信息集合
		/// </summary>
		/// <returns>字段类型信息集合</returns>
		public static FieldTypeCollect GetAllFieldTypeInfo()
		{
			string sql;
			FieldTypeInfo typeInfo;
			FieldTypeCollect result;

			// 获取所有字段类型信息
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
