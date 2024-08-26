using System;

using GoldNet.Comm;

namespace GoldNet.JXKP.BLL.Guide
{
	/// <summary>
	/// ָ�����ͨ���쳣
	/// </summary>
	public class GuideException : GlobalException
	{
		/// <summary>
		/// ��ʼ���쳣
		/// </summary>
		/// <param name="title">�쳣����</param>
		/// <param name="content">�쳣����</param>
		public GuideException(string title,string content) : base(title,content){}

		/// <summary>
		/// ��ʼ���쳣
		/// </summary>
		/// <param name="title">�쳣����</param>
		/// <param name="content">�쳣����</param>
		/// <param name="innerException">���쳣</param>
		public GuideException(string title,string content,Exception innerException) : base(title,content,innerException){}
	}
    public class QualityException : GlobalException
    {
        public QualityException(string content) : base("�����쳣", content) { }
        protected QualityException(string title, string content) : base(title, content) { }
        protected QualityException(string title, string content, Exception ex) : base(title, content, ex) { }
    }


	/// <summary>
	/// ��������ʱ�쳣
	/// </summary>
	public class SaveRecordDataException : GuideException
	{
		public SaveRecordDataException(string title,string Message) : base(title,"��ϸ��ϢΪ��" + Message){}
	}

	/// <summary>
	/// ָ�����Ʋ������쳣
	/// </summary>
	public class GuideNameNotExistedException : GuideException
	{
		private const string NAME_NOT_EXISTED = "ָ����ָ�����Ʋ����ڣ�����ϵ����Ա�������������ƣ�";
		public GuideNameNotExistedException(string name) : base("ָ�����Ʋ�����", NAME_NOT_EXISTED + name){}
	}
    /// <summary>
    /// �������ɵ�������֤�쳣
    /// </summary>
    public class QualityVerifyDataException : QualityException
    {
        public QualityVerifyDataException(string content) : base("�������ɵ�������֤�쳣", content) { }
    }

    /// <summary>
    /// �������������쳣
    /// </summary>
    public class BonusPrepareDataException : QualityException
    {
        public BonusPrepareDataException(string content) : base("�������������쳣", content) { }
        public BonusPrepareDataException(string content, Exception ex) : base("�������������쳣", content, ex) { }
    }


}
