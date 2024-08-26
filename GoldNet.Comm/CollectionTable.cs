using System.Collections;
using System.Data;

namespace GoldNet.Comm
{
	/// <summary>
	/// ������
	/// �̳�DataTable����ʵ��ö�ٵĻ���
	/// </summary>
	public abstract class CollectionTable : DataTable, IEnumerable, IEnumerator
	{
		/// <summary>
		/// �洢��ǰ��λ����Ϣ��
		/// </summary>
		protected int _position = -1;

		#region IEnumerator ��Ա

		/// <summary>
		/// �ӿڷ���������
		/// </summary>
		public new void Reset()
		{
			_position = -1;
		}

		/// <summary>
		/// �ӿڷ�������ǰֵ
		/// </summary>
		public object Current
		{
			get { return this[_position]; }
		}

		/// <summary>
		/// �ӿڷ������ƶ�����һλ��
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

		#region IEnumerable ��Ա

		/// <summary>
		/// ����һ��ö�ٶ���
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		#endregion

		/// <summary>
		/// Ҫʵ�ֵ�������
		/// </summary>
		public abstract object this[int i] { get; }
	}
}
