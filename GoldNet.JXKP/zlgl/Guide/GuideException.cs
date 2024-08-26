using System;

using GoldNet.Comm;

namespace GoldNet.JXKP.BLL.Guide
{
	/// <summary>
	/// 指标对象通用异常
	/// </summary>
	public class GuideException : GlobalException
	{
		/// <summary>
		/// 初始化异常
		/// </summary>
		/// <param name="title">异常标题</param>
		/// <param name="content">异常内容</param>
		public GuideException(string title,string content) : base(title,content){}

		/// <summary>
		/// 初始化异常
		/// </summary>
		/// <param name="title">异常标题</param>
		/// <param name="content">异常内容</param>
		/// <param name="innerException">子异常</param>
		public GuideException(string title,string content,Exception innerException) : base(title,content,innerException){}
	}
    public class QualityException : GlobalException
    {
        public QualityException(string content) : base("质量异常", content) { }
        protected QualityException(string title, string content) : base(title, content) { }
        protected QualityException(string title, string content, Exception ex) : base(title, content, ex) { }
    }


	/// <summary>
	/// 保存数据时异常
	/// </summary>
	public class SaveRecordDataException : GuideException
	{
		public SaveRecordDataException(string title,string Message) : base(title,"详细信息为：" + Message){}
	}

	/// <summary>
	/// 指标名称不存在异常
	/// </summary>
	public class GuideNameNotExistedException : GuideException
	{
		private const string NAME_NOT_EXISTED = "指定的指标名称不存在，请联系管理员解决！输入的名称：";
		public GuideNameNotExistedException(string name) : base("指标名称不存在", NAME_NOT_EXISTED + name){}
	}
    /// <summary>
    /// 质量生成的数据验证异常
    /// </summary>
    public class QualityVerifyDataException : QualityException
    {
        public QualityVerifyDataException(string content) : base("质量生成的数据验证异常", content) { }
    }

    /// <summary>
    /// 质量数据生成异常
    /// </summary>
    public class BonusPrepareDataException : QualityException
    {
        public BonusPrepareDataException(string content) : base("质量数据生成异常", content) { }
        public BonusPrepareDataException(string content, Exception ex) : base("质量数据生成异常", content, ex) { }
    }


}
