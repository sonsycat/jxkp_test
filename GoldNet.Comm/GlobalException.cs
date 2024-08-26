using System;

namespace GoldNet.Comm
{
	/// <summary>
	/// ȫ��ͨ���쳣��
	/// �����������ʹ�������
	/// 
	/// ��չ��
	///   Ӧ���Ǳ��������쳣���������ļ����ȡ�Ŀǰ���԰Ѵ���Ϣ������content�С�
	/// </summary>
	public class GlobalException : Exception
	{
		private string _title;
		private string _content;
		private Exception _innerException;

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="title">�쳣�ı���</param>
		/// <param name="content">�쳣����ϸ����</param>
		public GlobalException(string title, string content)
		{
			_title = title;
			_content = content;
			_innerException = null;
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="title">�쳣����</param>
		/// <param name="content">�쳣����ϸ����</param>
		/// <param name="innerException">�����쳣�����쳣</param>
		public GlobalException(string title, string content, Exception innerException)
		{
			_title = title;
			_content = content;
			_innerException = innerException;
		}

		/// <summary>
		/// �����쳣�ı�����Ϣ
		/// </summary>
		public string Title
		{
			get
			{
				return _title;
			}
		}

		/// <summary>
		/// �����쳣����ϸ����
		/// </summary>
		public string Content
		{
			get
			{
				return _content;
			}
		}

		/// <summary>
		/// ��д��Exception��Message���ԡ�
		/// </summary>
		public new string Message
		{
			get
			{
				return "Title:" + _title + ";Content:" + _content + ";Base Message:" + base.Message;
			}
		}

		/// <summary>
		/// ���������쳣�����쳣
		/// </summary>
		public new Exception InnerException
		{
			get
			{
				if (_innerException != null)
					return _innerException;
				else
					return null;
			}
		}
	}
}
