using System;
using System.Collections;

namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// CollectionBase : 集合基类
	/// </summary>
    [Serializable()]
	public abstract class CollectionBase : System.Collections.IEnumerable, System.Collections.ICollection
	{
		/// <summary>
		/// 定义一个数组列表
		/// </summary>
		protected ArrayList List = new ArrayList();

		#region ICollection 成员
		/// <summary>
		/// 获取一个值，该值指示是否同步对 Array 的访问
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return List.IsSynchronized;
			}
		}

		/// <summary>
		/// 返回集合中对象的个数
		/// </summary>
		public int Count
		{
			get
			{
				return List.Count;
			}
		}

		/// <summary>
		/// 将 ArrayList 或它的一部分复制到一维数组中
		/// </summary>
		/// <param name="array">要复制到的ArrayList对象</param>
		/// <param name="index">要复制的起始位置</param>
		public void CopyTo(Array array, int index)
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// 获取可用于同步对 ArrayList 访问的对象
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return List.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable 成员

		/// <summary>
		/// 返回可循环访问 ArrayList 的枚举数
		/// </summary>
		/// <returns>返回整个 ArrayList 的一个枚举数</returns>
		public IEnumerator GetEnumerator()
		{
			return List.GetEnumerator();
		}

		#endregion



	}
}
