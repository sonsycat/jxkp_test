/*
 * 查询条件类
 * 高山 04/27
 * 
 * */

/* 例子： *************************
 * 
 * 建立一个条件
 * 			Condition c1 = new Condition();
 * 			c1.Add("IsDeleted", ConditionType.Equal, new object[]{"0"}); //
 * 			c1.Add(true, "IsDeleted", ConditionType.Equal, 1);  // 加 or 连接到上一个条件。
 * 			c1.Add("StaffName", ConditionType.Between , new object[]{"高山"});
 * 			c1.Add("StaffName", ConditionType.Between , "高山", "高山");
 * 			c1.Add("StaffName", ConditionType.GreatThan , new object[]{"高山", "高山"});
 * 			c1.Add("StaffName", ConditionType.In , new object[]{"高山", "高山"});
 * 建立一个子条件
 *			Condition c2 = new Condition();
 *			c2.Add("StaffName", ConditionType.Equal , "高山");
 * 联合两个条件
 *			c1.Add(true, c2); // 把c1 和c2 分别加括号，中间用or连接后，作为条件。
 * 使用此条件: 
 *			sql = "SELECT * FROM tab1 ";
 *			sql += "WHERE " 
 *			sql += c1.GetWhereString;	//添加条件语句模板
 *			DALBase.ExecuteDataSet(sql, c1.GetParams);	 // 开始查询
 * 
 * 已知的bug： *************************
 * 
 * 1:在条件联合时，因为C#中默认的参数传递都使用引用传递，多次联合一个条件，将导致一个重复引用异常，如下面的例子：
 * 			Condition c1 = new Condition();
 * 			c1.Add ......
 *			Condition c2 = new Condition();
 *			c2.Add ......
 *			c1.Add(c2); // and 联合
 *			c1.Add(true, c2);  // 再作一次 or 联合，将重复引用C2中的fields，导致重名异常。
 *			// 正确的做法是再构造一个一模一样的条件，并联合到c1
 * 2:在条件联合后，再次向已联合的条件中添加条件，将有可能出现重命名异常，或其它异常及错误的结果。
 *			c1.Add(c2); // and 联合
 *			c2.Add(......) // 再次添加条件将可能引发重命名异常、其它异常或错误的结果。
 *			// 正确的做法，应该是添加好条件后，再联合。
 * 
 * 以后要更改： *************************
 * 
 * 1: 增加一个是否被锁定的属性(IsLocked)，如果 c1.Add(c2); 则c2被锁定，不能再添加条件或再被联合到另一个条件。
 * 
 * */
using System;
using System.Text;
using System.Collections;
using System.Data.SqlClient;

namespace GoldNet.Comm.DAL.SqlServer
{
	/// <summary>
	/// 一个查询条件的类型
	/// </summary>
	public enum ConditionType : int
	{
		/// <summary>
		/// 相等
		/// </summary>
		Equal = 1,
		/// <summary>
		/// 大于等于
		/// </summary>
		GreatThan = 2,
		/// <summary>
		/// 小于等于
		/// </summary>
		LessThan = 3,
		/// <summary>
		/// 在两者之间
		/// </summary>
		Between = 4,
		/// <summary>
		/// 包含，SQL中的 '%XXX%'
		/// </summary>
		Include = 5,
		/// <summary>
		/// 枚举，SQL中的 IN
		/// </summary>
		In = 6
	}

	/// <summary>
	/// 查询条件集类，此类负责依据添加的条件生成一个多个条件的查询语句。
	/// 此语句由一个WHERE语句模板和相应一个SqlParameter[]数组
	/// 可以传递此WHERE语句模板和SqlParameter[]数组到<see cref="GoldNet.Comm.DAL.SqlServer.DALBase">DALBase</see>对象
	/// <seealso cref="GoldNet.Comm.DAL.SqlServer.DALBase">DALBase</seealso>
	/// </summary>
	public class Condition
	{
		#region "--私有变量--"
		private ArrayList _fieldNames = new ArrayList();
		private string _whereString = String.Empty;
		private ArrayList _params = new ArrayList();
		private int _subCondCount = 0;
		#endregion

		#region "--方法--"
		/// <summary>
		/// 添加一个条件
		/// </summary>
		/// <example>
		/// 以下方法返回满足"SELECT * FROM Tab WHERE (IsDeleted = 0)"的记录集
		/// <code>
		/// // 建立一个条件对象 
		/// Condition cond = new Condition();
		/// 
		/// // 添加一个 IsDeleted = 0 的条件
		/// cond.Add("IsDeleted", ConditionType.Equal, 0);	
		/// sql = "SELECT * FROM Tab ";
		/// sql += "WHERE ";
		/// 
		/// //添加条件语句模板
		/// sql += cond.GetWhereString;	
		/// 
		/// // 开始查询
		/// DALBase.ExecuteDataSet(sql, c1.GetParams);	 
		/// </code>
		/// </example>
		/// <param name="fieldName">要查询的字段名称</param>
		/// <param name="type">条件类型</param>
		/// <param name="values">备选值</param>
		public void Add(string fieldName, ConditionType type, params object[] values)
		{
			this.Add(false, fieldName, type, values);
		}


		/// <summary>
		/// 添加一个条件
		/// </summary>
		/// <example>
		/// <code>
		/// // 建立一个条件对象
		/// Condition cond = new Condition();
		/// 
		/// // 添加一个 IsDeleted = 0 的条件
		/// cond.Add("IsDeleted", ConditionType.Equal, 0);	
		/// cond.Add
		/// sql = "SELECT * FROM tab1 ";
		/// sql += "WHERE ";
		/// 
		/// //添加条件语句模板
		/// sql += cond.GetWhereString;	
		/// 
		/// // 开始查询
		/// DALBase.ExecuteDataSet(sql, c1.GetParams);	 
		/// </code>
		/// </example>
		/// <param name="isor">是否使用OR连接</param>
		/// <param name="fieldName">要查询的字段名称</param>
		/// <param name="type">条件类型</param>
		/// <param name="values">备选值</param>
		public void Add(bool isor, string fieldName, ConditionType type, params object[] values)
		{
			if (values == null)
				values = new object[]{DBNull.Value};
			//	throw new NullReferenceException("使用Condition类初始化函数时，values参数为空！");
			if (values.Length == 0)
				values = new object[]{DBNull.Value};
			//	throw new NullReferenceException("使用Condition类初始化函数时，values参数为空！");

			// 当前字段的序号值（第几个）
			int countNum;
			// 参数名
			string paramsName, andStr;
			// 参数值
			object paramsValue;

			_fieldNames.Add(fieldName);

			countNum = GetItemCount(_fieldNames, fieldName);

			if (_whereString == String.Empty)
				andStr = "";
			else if (isor)
				andStr = " OR ";
			else 
				andStr = " AND ";

			_whereString += andStr;
			
			string fieldParName = _filter(fieldName);

			switch (type) 
			{		
				default:
				case ConditionType.Equal:
					//等于
					if (values[0]!=DBNull.Value)
					{
						paramsName = "@" + fieldParName + countNum.ToString();
						paramsValue = values[0];
						_params.Add (new SqlParameter(paramsName, paramsValue));
						_whereString += " (" + fieldName + "=" + paramsName + ") ";
					}
					else
						_whereString += " (" + fieldName + " IS NULL ) ";
					break;
				case ConditionType.GreatThan : 
					//大于或等于
					paramsName = "@" + fieldParName + countNum.ToString();
					paramsValue = ((values[0])==null?DBNull.Value:values[0]);

					_params.Add (new SqlParameter(paramsName, paramsValue));
					_whereString += " (" + fieldName + ">=" + paramsName + ") ";
					break;
				case ConditionType.LessThan : 
					//小于或等于
					paramsName = "@" + fieldParName + countNum.ToString();
					paramsValue = ((values[0])==null?DBNull.Value:values[0]);

					_params.Add (new SqlParameter(paramsName, paramsValue));
					_whereString += " (" + fieldName + "<=" + paramsName + ") ";
					break;
				case ConditionType.Include : 
					//Include
					paramsName = "@" + fieldParName + countNum.ToString();
					paramsValue = ((values[0])==null?DBNull.Value:values[0]);

					_params.Add (new SqlParameter(paramsName, paramsValue));
					_whereString += " (" + fieldName + " LIKE " + paramsName + ") ";
					break;
				case ConditionType.Between : 
					//两者之间
					// 定义第一个参数：后面加a ，值取[0]
					paramsName = "@" + fieldParName + countNum.ToString() + "a";
					paramsValue = ((values[0])==null?DBNull.Value:values[0]);

					_params.Add (new SqlParameter(paramsName, paramsValue));
					_whereString += " (" + fieldName + " BETWEEN " + paramsName;

					// 定义第二个参数：后面加b，如果有第二个值取[1]，否则还是上面取的[0]
					paramsName = "@" + fieldParName + countNum.ToString() + "b";
					if (values.Length >= 2)
						paramsValue = ((values[1])==null?DBNull.Value:values[1]);
					// 如果只有一个参数,那么paramsValue就是上面的值,不用改变。
                    
					_params.Add (new SqlParameter(paramsName, paramsValue));
					_whereString += " AND " + paramsName + ") ";
					break;
				case ConditionType.In : 
					//IN
					_whereString += " (" + fieldName + " IN (";
					for(int i=0; i<values.Length; i++)
					{
						paramsName = "@" + fieldParName + countNum.ToString() + i.ToString();
						paramsValue = ((values[i])==null?DBNull.Value:values[i]);

						_params.Add (new SqlParameter(paramsName, paramsValue));
						_whereString += paramsName + ",";
					}
					_whereString = _whereString.TrimEnd(',') + ")) ";
					break;
			} //switch

		} //void Add()


		/// <summary>
		/// 添加一个集合条件,默认使用 And 关系
		/// </summary>
		/// <param name="subCond">子条件集合</param>
		public void Add(Condition subCond)
		{
			this.Add(false, subCond);
		}


		/// <summary>
		/// 添加一个集合条件
		/// </summary>
		/// <param name="isor">是否使用OR连接</param>
		/// <param name="subCond">子条件集合</param>
		public void Add(bool isor, Condition subCond)
		{
			string andStr;
			_subCondCount += 1;

			subCond.SetTagAdd(_subCondCount.ToString());

			if (_whereString == String.Empty)
				andStr = "";
			else if (isor)
				andStr = " OR ";
			else 
				andStr = " AND ";

			_whereString = (_whereString==string.Empty? "" : " (" + _whereString + ") ");
			_whereString += andStr;
			_whereString +=	" (" + subCond._whereString + ") ";

			_params.AddRange(subCond._params);
		}


		/// <summary>
		/// 为当前条件添加统一标志
		/// </summary>
		/// <param name="tag">标志字符</param>
		private void SetTagAdd(string tag)
		{
			_whereString = _whereString.Replace("@", "@" + tag);
			for (int i=0; i<_params.Count; i++)
				((SqlParameter)_params[i]).ParameterName = ((SqlParameter)_params[i]).ParameterName.Replace("@", "@" + tag);
		}


		/// <summary>
		/// 得到ArrayList中某个值的个数.
		/// </summary>
		/// <param name="li">一个ArrayList</param>
		/// <param name="obj">要查找的object</param>
		/// <returns>此object在ArrayList中的个数</returns>
		private int GetItemCount(ArrayList li, object obj)
		{
			int _index;
			int _count = 0;

			_index = li.IndexOf(obj);
			while(_index >=0)
			{
				_count += 1;
				_index = li.IndexOf(obj, _index + 1);
			}
			return _count;
		}



		private string _filter(string filedName)
		{
			return filedName.Replace(".", "");
		}
		#endregion

		#region "--属性--"
		/// <summary>
		/// 返回查询语句的WHERE模板
		/// </summary>
		public string GetWhereString
		{
			get
			{
				if (_whereString == String.Empty)
					return " 1=1 ";
				else
					return _whereString;
			}
		}


		/// <summary>
		/// 返回此条件使用的 SqlParameter[] 数组 
		/// </summary>
		public SqlParameter[] GetParams
		{
			get
			{
				SqlParameter[] _sp = new SqlParameter[_params.Count];
				for (int i=0; i<_params.Count; i++)
					_sp[i] = (SqlParameter)_params[i];
				return _sp;
			}
		}
		#endregion
	}
}
