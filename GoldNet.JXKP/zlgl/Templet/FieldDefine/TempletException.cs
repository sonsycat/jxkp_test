using System;
using GoldNet.Comm;
using GoldNet.JXKP.PowerManager;
namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// 模板对象使用的通用异常
	/// </summary>
	public class TempletException : GlobalException
	{
		/// <summary>
		/// 初始化异常类 
		/// </summary>
		/// <param name="title">异常信息的标题</param>
		/// <param name="content">异常信息的内容</param>
		public TempletException(string title, string content):base(title,content){}
		/// <summary>
		///  初始化异常类
		/// </summary>
		/// <param name="title">异常信息的标题</param>
		/// <param name="content">异常信息的内容</param>
		/// <param name="innerException">引发异常的子异常</param>
		public TempletException(string title, string content, Exception innerException):base(title,content,innerException){}
	}

	/// <summary>
	/// 模板名称已被使用异常。
	/// </summary>
	public class TempletNameInUseException : TempletException
	{
		private const string NAME_IN_USE = "填写的模板名称已被使用，请使用其他名称。填写的名称：";
		public TempletNameInUseException(string name) : base("模板名称被使用", NAME_IN_USE + name){}
	}

	/// <summary>
	/// 字段名称已被使用异常。
	/// </summary>
	public class FieldNameInUseException : TempletException
	{
		private const string NAME_IN_USE = "填写的字段名称已被使用，请使用其他名称。填写的名称：";
		public FieldNameInUseException(string name) : base("字段名称被使用", NAME_IN_USE + name){}
	}

	/// <summary>
	/// 指定的模板编号不存在异常
	/// </summary>
	public class TempletIDNotExistedException : TempletException
	{
		private const string ID_NOT_EXISTED = "指定的模板不存在，请联系管理员解决此问题。指定的模板编号：";
		public TempletIDNotExistedException(int templetID) : base("模板不存在", ID_NOT_EXISTED + templetID.ToString()){}
	}

	/// <summary>
	/// 指定的视图编号不存在异常
	/// </summary>
	public class ViewIDNotExistedException : TempletException
	{
		private const string ID_NOT_EXISTED = "指定的视图不存在，请联系管理员解决此问题。指定的视图编号：";
		public ViewIDNotExistedException(int viewID) : base("视图不存在", ID_NOT_EXISTED + viewID.ToString()){}
	}

	/// <summary>
	/// 指定的字段类型对象编号不存在异常
	/// </summary>
	public class FieldTypeIDNotExistedException : TempletException
	{
		private const string ID_NOT_EXISTED = "字段类型不存在，请联系管理员解决此问题。字段类型编号：";
		public FieldTypeIDNotExistedException(int fieldTypeID) : base("字段类型对象编号不存在", ID_NOT_EXISTED + fieldTypeID.ToString()){}
	}

	/// <summary>
	/// 指定的字段对象编号不存在异常
	/// </summary>
	public class FieldIDNotExistedException : TempletException
	{
		private const string ID_NOT_EXISTED = "字段不存在，请联系管理员解决此问题。字段编号：";
		public FieldIDNotExistedException(int fieldID) : base("字段对象编号不存在", ID_NOT_EXISTED + fieldID.ToString()){}
	}

	/// <summary>
	/// 保存记录数据时发生的错误
	/// </summary>
	public class SaveRecordDataException : TempletException
	{
		private const string SAVE_DATA_ERROR = "在保存记录数据时发生错误，请联系管理员解决此问题。错误的详细信息：";
		public SaveRecordDataException(Exception ex) : base("保存记录数据时发生错误", SAVE_DATA_ERROR + ex.Message, ex){}
	}
    /// <summary>
    /// 必填项检查
    /// </summary>
    public class SaveRecordDataIsNullException : TempletException
    {
        public SaveRecordDataIsNullException(string filename) : base("系统提示", filename + "：不能为空！") { }
    }

    public class SaveRecordDataIsNo : TempletException
    {
        public SaveRecordDataIsNo(string filename) : base("系统提示", filename + "：数据已经生成，不能再编辑了！") { }
    }
    public class SaveRecordDataIsNoWeek : TempletException
    {
        public SaveRecordDataIsNoWeek(string filename) : base("系统提示", filename + "：这周数据已经审核，不能再编辑了！") { }
    }

    public class DateTimeException : TempletException
    {
        public DateTimeException(string filename) : base("系统提示", filename + "：(DateTime类型),在更新数据库时发生Format异常！") { }
    }
}
