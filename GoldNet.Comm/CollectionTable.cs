using System.Collections;
using System.Data;

namespace GoldNet.Comm
{
	/// <summary>
	/// 集合类
	/// 继承DataTable并能实现枚举的基类
	/// </summary>
	public abstract class CollectionTable : DataTable, IEnumerable, IEnumerator
	{
		/// <summary>
		/// 存储当前的位置信息。
		/// </summary>
		protected int _position = -1;

		#region IEnumerator 成员

		/// <summary>
		/// 接口方法：重置
		/// </summary>
		public new void Reset()
		{
			_position = -1;
		}

		/// <summary>
		/// 接口方法：当前值
		/// </summary>
		public object Current
		{
			get { return this[_position]; }
		}

		/// <summary>
		/// 接口方法：移动到下一位置
		/// </summary>
		/// <returns></returns>
		public bool MoveNext()
		{
			if (_position < this.Rows.Count - 1)
			{
				_position++;
				return true;
			}
			else
				return false;
		}

		#endregion

		#region IEnumerable 成员

		/// <summary>
		/// 返回一个枚举对象
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		#endregion

		/// <summary>
		/// 要实现的索引器
		/// </summary>
		public abstract object this[int i] { get; }
	}
}
