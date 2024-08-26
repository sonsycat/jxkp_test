/*
 * ��ѯ������
 * ��ɽ 04/27
 * 
 * */

/* ���ӣ� *************************
 * 
 * ����һ������
 * 			Condition c1 = new Condition();
 * 			c1.Add("IsDeleted", ConditionType.Equal, new object[]{"0"}); //
 * 			c1.Add(true, "IsDeleted", ConditionType.Equal, 1);  // �� or ���ӵ���һ��������
 * 			c1.Add("StaffName", ConditionType.Between , new object[]{"��ɽ"});
 * 			c1.Add("StaffName", ConditionType.Between , "��ɽ", "��ɽ");
 * 			c1.Add("StaffName", ConditionType.GreatThan , new object[]{"��ɽ", "��ɽ"});
 * 			c1.Add("StaffName", ConditionType.In , new object[]{"��ɽ", "��ɽ"});
 * ����һ��������
 *			Condition c2 = new Condition();
 *			c2.Add("StaffName", ConditionType.Equal , "��ɽ");
 * ������������
 *			c1.Add(true, c2); // ��c1 ��c2 �ֱ�����ţ��м���or���Ӻ���Ϊ������
 * ʹ�ô�����: 
 *			sql = "SELECT * FROM tab1 ";
 *			sql += "WHERE " 
 *			sql += c1.GetWhereString;	//����������ģ��
 *			DALBase.ExecuteDataSet(sql, c1.GetParams);	 // ��ʼ��ѯ
 * 
 * ��֪��bug�� *************************
 * 
 * 1:����������ʱ����ΪC#��Ĭ�ϵĲ������ݶ�ʹ�����ô��ݣ��������һ��������������һ���ظ������쳣������������ӣ�
 * 			Condition c1 = new Condition();
 * 			c1.Add ......
 *			Condition c2 = new Condition();
 *			c2.Add ......
 *			c1.Add(c2); // and ����
 *			c1.Add(true, c2);  // ����һ�� or ���ϣ����ظ�����C2�е�fields�����������쳣��
 *			// ��ȷ���������ٹ���һ��һģһ���������������ϵ�c1
 * 2:���������Ϻ��ٴ��������ϵ�������������������п��ܳ����������쳣���������쳣������Ľ����
 *			c1.Add(c2); // and ����
 *			c2.Add(......) // �ٴ�������������������������쳣�������쳣�����Ľ����
 *			// ��ȷ��������Ӧ������Ӻ������������ϡ�
 * 
 * �Ժ�Ҫ���ģ� *************************
 * 
 * 1: ����һ���Ƿ�����������(IsLocked)����� c1.Add(c2); ��c2������������������������ٱ����ϵ���һ��������
 * 
 * */
using System;
using System.Text;
using System.Collections;
using System.Data.SqlClient;

namespace GoldNet.Comm.DAL.SqlServer
{
	/// <summary>
	/// һ����ѯ����������
	/// </summary>
	public enum ConditionType : int
	{
		/// <summary>
		/// ���
		/// </summary>
		Equal = 1,
		/// <summary>
		/// ���ڵ���
		/// </summary>
		GreatThan = 2,
		/// <summary>
		/// С�ڵ���
		/// </summary>
		LessThan = 3,
		/// <summary>
		/// ������֮��
		/// </summary>
		Between = 4,
		/// <summary>
		/// ������SQL�е� '%XXX%'
		/// </summary>
		Include = 5,
		/// <summary>
		/// ö�٣�SQL�е� IN
		/// </summary>
		In = 6
	}

	/// <summary>
	/// ��ѯ�������࣬���ฺ��������ӵ���������һ����������Ĳ�ѯ��䡣
	/// �������һ��WHERE���ģ�����Ӧһ��SqlParameter[]����
	/// ���Դ��ݴ�WHERE���ģ���SqlParameter[]���鵽<see cref="GoldNet.Comm.DAL.SqlServer.DALBase">DALBase</see>����
	/// <seealso cref="GoldNet.Comm.DAL.SqlServer.DALBase">DALBase</seealso>
	/// </summary>
	public class Condition
	{
		#region "--˽�б���--"
		private ArrayList _fieldNames = new ArrayList();
		private string _whereString = String.Empty;
		private ArrayList _params = new ArrayList();
		private int _subCondCount = 0;
		#endregion

		#region "--����--"
		/// <summary>
		/// ���һ������
		/// </summary>
		/// <example>
		/// ���·�����������"SELECT * FROM Tab WHERE (IsDeleted = 0)"�ļ�¼��
		/// <code>
		/// // ����һ���������� 
		/// Condition cond = new Condition();
		/// 
		/// // ���һ�� IsDeleted = 0 ������
		/// cond.Add("IsDeleted", ConditionType.Equal, 0);	
		/// sql = "SELECT * FROM Tab ";
		/// sql += "WHERE ";
		/// 
		/// //����������ģ��
		/// sql += cond.GetWhereString;	
		/// 
		/// // ��ʼ��ѯ
		/// DALBase.ExecuteDataSet(sql, c1.GetParams);	 
		/// </code>
		/// </example>
		/// <param name="fieldName">Ҫ��ѯ���ֶ�����</param>
		/// <param name="type">��������</param>
		/// <param name="values">��ѡֵ</param>
		public void Add(string fieldName, ConditionType type, params object[] values)
		{
			this.Add(false, fieldName, type, values);
		}


		/// <summary>
		/// ���һ������
		/// </summary>
		/// <example>
		/// <code>
		/// // ����һ����������
		/// Condition cond = new Condition();
		/// 
		/// // ���һ�� IsDeleted = 0 ������
		/// cond.Add("IsDeleted", ConditionType.Equal, 0);	
		/// cond.Add
		/// sql = "SELECT * FROM tab1 ";
		/// sql += "WHERE ";
		/// 
		/// //����������ģ��
		/// sql += cond.GetWhereString;	
		/// 
		/// // ��ʼ��ѯ
		/// DALBase.ExecuteDataSet(sql, c1.GetParams);	 
		/// </code>
		/// </example>
		/// <param name="isor">�Ƿ�ʹ��OR����</param>
		/// <param name="fieldName">Ҫ��ѯ���ֶ�����</param>
		/// <param name="type">��������</param>
		/// <param name="values">��ѡֵ</param>
		public void Add(bool isor, string fieldName, ConditionType type, params object[] values)
		{
			if (values == null)
				values = new object[]{DBNull.Value};
			//	throw new NullReferenceException("ʹ��Condition���ʼ������ʱ��values����Ϊ�գ�");
			if (values.Length == 0)
				values = new object[]{DBNull.Value};
			//	throw new NullReferenceException("ʹ��Condition���ʼ������ʱ��values����Ϊ�գ�");

			// ��ǰ�ֶε����ֵ���ڼ�����
			int countNum;
			// ������
			string paramsName, andStr;
			// ����ֵ
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
					//����
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
					//���ڻ����
					paramsName = "@" + fieldParName + countNum.ToString();
					paramsValue = ((values[0])==null?DBNull.Value:values[0]);

					_params.Add (new SqlParameter(paramsName, paramsValue));
					_whereString += " (" + fieldName + ">=" + paramsName + ") ";
					break;
				case ConditionType.LessThan : 
					//С�ڻ����
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
					//����֮��
					// �����һ�������������a ��ֵȡ[0]
					paramsName = "@" + fieldParName + countNum.ToString() + "a";
					paramsValue = ((values[0])==null?DBNull.Value:values[0]);

					_params.Add (new SqlParameter(paramsName, paramsValue));
					_whereString += " (" + fieldName + " BETWEEN " + paramsName;

					// ����ڶ��������������b������еڶ���ֵȡ[1]������������ȡ��[0]
					paramsName = "@" + fieldParName + countNum.ToString() + "b";
					if (values.Length >= 2)
						paramsValue = ((values[1])==null?DBNull.Value:values[1]);
					// ���ֻ��һ������,��ôparamsValue���������ֵ,���øı䡣
                    
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
		/// ���һ����������,Ĭ��ʹ�� And ��ϵ
		/// </summary>
		/// <param name="subCond">����������</param>
		public void Add(Condition subCond)
		{
			this.Add(false, subCond);
		}


		/// <summary>
		/// ���һ����������
		/// </summary>
		/// <param name="isor">�Ƿ�ʹ��OR����</param>
		/// <param name="subCond">����������</param>
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
		/// Ϊ��ǰ�������ͳһ��־
		/// </summary>
		/// <param name="tag">��־�ַ�</param>
		private void SetTagAdd(string tag)
		{
			_whereString = _whereString.Replace("@", "@" + tag);
			for (int i=0; i<_params.Count; i++)
				((SqlParameter)_params[i]).ParameterName = ((SqlParameter)_params[i]).ParameterName.Replace("@", "@" + tag);
		}


		/// <summary>
		/// �õ�ArrayList��ĳ��ֵ�ĸ���.
		/// </summary>
		/// <param name="li">һ��ArrayList</param>
		/// <param name="obj">Ҫ���ҵ�object</param>
		/// <returns>��object��ArrayList�еĸ���</returns>
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

		#region "--����--"
		/// <summary>
		/// ���ز�ѯ����WHEREģ��
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
		/// ���ش�����ʹ�õ� SqlParameter[] ���� 
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
