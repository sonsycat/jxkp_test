using System;
using System.Collections;

namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// CollectionBase : ���ϻ���
	/// </summary>
    [Serializable()]
	public abstract class CollectionBase : System.Collections.IEnumerable, System.Collections.ICollection
	{
		/// <summary>
		/// ����һ�������б�
		/// </summary>
		protected ArrayList List = new ArrayList();

		#region ICollection ��Ա
		/// <summary>
		/// ��ȡһ��ֵ����ֵָʾ�Ƿ�ͬ���� Array �ķ���
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return List.IsSynchronized;
			}
		}

		/// <summary>
		/// ���ؼ����ж���ĸ���
		/// </summary>
		public int Count
		{
			get
			{
				return List.Count;
			}
		}

		/// <summary>
		/// �� ArrayList ������һ���ָ��Ƶ�һά������
		/// </summary>
		/// <param name="array">Ҫ���Ƶ���ArrayList����</param>
		/// <param name="index">Ҫ���Ƶ���ʼλ��</param>
		public void CopyTo(Array array, int index)
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// ��ȡ������ͬ���� ArrayList ���ʵĶ���
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return List.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable ��Ա

		/// <summary>
		/// ���ؿ�ѭ������ ArrayList ��ö����
		/// </summary>
		/// <returns>�������� ArrayList ��һ��ö����</returns>
		public IEnumerator GetEnumerator()
		{
			return List.GetEnumerator();
		}

		#endregion



	}
}
