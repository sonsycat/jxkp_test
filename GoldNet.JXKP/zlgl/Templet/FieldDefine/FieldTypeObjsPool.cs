using System;
using System.Collections;

namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// 字段对象类型的缓冲池 
	/// </summary>
	public class FieldTypeObjsPool_bak_no_use
	{
		#region --构造函数-- 
		/// <summary>
		/// 私有构造函数，来实现单例
		/// </summary>
		// private FieldTypeObjsPool(){}

		#endregion

		#region --私有变量-- 
		/// <summary>
		/// 静态对象实例
		/// </summary>
		// private static FieldTypeObjsPool instance;

		/// <summary>
		/// 字段对象的hashtable
		/// </summary>
		private static Hashtable _fieldTypeObjsObj = new Hashtable();
		#endregion
	}
}
