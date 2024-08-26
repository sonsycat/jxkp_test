using System;
using GoldNet.Comm;
using GoldNet.JXKP.PowerManager;
namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// ģ�����ʹ�õ�ͨ���쳣
	/// </summary>
	public class TempletException : GlobalException
	{
		/// <summary>
		/// ��ʼ���쳣�� 
		/// </summary>
		/// <param name="title">�쳣��Ϣ�ı���</param>
		/// <param name="content">�쳣��Ϣ������</param>
		public TempletException(string title, string content):base(title,content){}
		/// <summary>
		///  ��ʼ���쳣��
		/// </summary>
		/// <param name="title">�쳣��Ϣ�ı���</param>
		/// <param name="content">�쳣��Ϣ������</param>
		/// <param name="innerException">�����쳣�����쳣</param>
		public TempletException(string title, string content, Exception innerException):base(title,content,innerException){}
	}

	/// <summary>
	/// ģ�������ѱ�ʹ���쳣��
	/// </summary>
	public class TempletNameInUseException : TempletException
	{
		private const string NAME_IN_USE = "��д��ģ�������ѱ�ʹ�ã���ʹ���������ơ���д�����ƣ�";
		public TempletNameInUseException(string name) : base("ģ�����Ʊ�ʹ��", NAME_IN_USE + name){}
	}

	/// <summary>
	/// �ֶ������ѱ�ʹ���쳣��
	/// </summary>
	public class FieldNameInUseException : TempletException
	{
		private const string NAME_IN_USE = "��д���ֶ������ѱ�ʹ�ã���ʹ���������ơ���д�����ƣ�";
		public FieldNameInUseException(string name) : base("�ֶ����Ʊ�ʹ��", NAME_IN_USE + name){}
	}

	/// <summary>
	/// ָ����ģ���Ų������쳣
	/// </summary>
	public class TempletIDNotExistedException : TempletException
	{
		private const string ID_NOT_EXISTED = "ָ����ģ�岻���ڣ�����ϵ����Ա��������⡣ָ����ģ���ţ�";
		public TempletIDNotExistedException(int templetID) : base("ģ�岻����", ID_NOT_EXISTED + templetID.ToString()){}
	}

	/// <summary>
	/// ָ������ͼ��Ų������쳣
	/// </summary>
	public class ViewIDNotExistedException : TempletException
	{
		private const string ID_NOT_EXISTED = "ָ������ͼ�����ڣ�����ϵ����Ա��������⡣ָ������ͼ��ţ�";
		public ViewIDNotExistedException(int viewID) : base("��ͼ������", ID_NOT_EXISTED + viewID.ToString()){}
	}

	/// <summary>
	/// ָ�����ֶ����Ͷ����Ų������쳣
	/// </summary>
	public class FieldTypeIDNotExistedException : TempletException
	{
		private const string ID_NOT_EXISTED = "�ֶ����Ͳ����ڣ�����ϵ����Ա��������⡣�ֶ����ͱ�ţ�";
		public FieldTypeIDNotExistedException(int fieldTypeID) : base("�ֶ����Ͷ����Ų�����", ID_NOT_EXISTED + fieldTypeID.ToString()){}
	}

	/// <summary>
	/// ָ�����ֶζ����Ų������쳣
	/// </summary>
	public class FieldIDNotExistedException : TempletException
	{
		private const string ID_NOT_EXISTED = "�ֶβ����ڣ�����ϵ����Ա��������⡣�ֶα�ţ�";
		public FieldIDNotExistedException(int fieldID) : base("�ֶζ����Ų�����", ID_NOT_EXISTED + fieldID.ToString()){}
	}

	/// <summary>
	/// �����¼����ʱ�����Ĵ���
	/// </summary>
	public class SaveRecordDataException : TempletException
	{
		private const string SAVE_DATA_ERROR = "�ڱ����¼����ʱ������������ϵ����Ա��������⡣�������ϸ��Ϣ��";
		public SaveRecordDataException(Exception ex) : base("�����¼����ʱ��������", SAVE_DATA_ERROR + ex.Message, ex){}
	}
    /// <summary>
    /// ��������
    /// </summary>
    public class SaveRecordDataIsNullException : TempletException
    {
        public SaveRecordDataIsNullException(string filename) : base("ϵͳ��ʾ", filename + "������Ϊ�գ�") { }
    }

    public class SaveRecordDataIsNo : TempletException
    {
        public SaveRecordDataIsNo(string filename) : base("ϵͳ��ʾ", filename + "�������Ѿ����ɣ������ٱ༭�ˣ�") { }
    }
    public class SaveRecordDataIsNoWeek : TempletException
    {
        public SaveRecordDataIsNoWeek(string filename) : base("ϵͳ��ʾ", filename + "�����������Ѿ���ˣ������ٱ༭�ˣ�") { }
    }

    public class DateTimeException : TempletException
    {
        public DateTimeException(string filename) : base("ϵͳ��ʾ", filename + "��(DateTime����),�ڸ������ݿ�ʱ����Format�쳣��") { }
    }
}
