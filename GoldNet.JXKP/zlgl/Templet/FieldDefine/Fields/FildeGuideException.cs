using System;

using GoldNet.Comm;

namespace GoldNet.JXKP.Templet.BLL.Fields
{
    public class FildeGuideException : GlobalException
    {
        /// <summary>
		/// 初始化异常 
		/// </summary>
		/// <param name="title">异常标题</param>
		/// <param name="content">异常内容</param>
		public FildeGuideException(string title,string content) : base(title,content){}

		/// <summary>
		/// 初始化异常
		/// </summary>
		/// <param name="title">异常标题</param>
		/// <param name="content">异常内容</param>
		/// <param name="innerException">子异常</param>
        public FildeGuideException(string title, string content, Exception innerException) : base(title, content, innerException) { }
    }
    /// <summary>
	/// 保存数据时异常
	/// </summary>
    public class SaveRecordDataException : FildeGuideException
	{
		public SaveRecordDataException(string title,string Message) : base(title,"详细信息为：" + Message){}
	}

	/// <summary>
	/// 指标名称不存在异常
	/// </summary>
    public class GuideNameNotExistedException : FildeGuideException
	{
		private const string NAME_NOT_EXISTED = "指定的指标名称不存在，请联系管理员解决！输入的名称：";
		public GuideNameNotExistedException(string name) : base("指标名称不存在", NAME_NOT_EXISTED + name){}
	}
    /// <summary>
    /// 最大值检查
    /// </summary>
    public class NumberFildMaxException : FildeGuideException
    {
        public NumberFildMaxException(string filename,string max) : base("系统提示", filename + "：不能大于最大值,最大值是："+max) { }
    }
    /// <summary>
    /// 最小值检查
    /// </summary>
    public class NumberFildMinException : FildeGuideException
    {
        public NumberFildMinException(string filename, string min) : base("系统提示", filename + "：不能小于最小值,最小值是：" + min) { }
    }
}
