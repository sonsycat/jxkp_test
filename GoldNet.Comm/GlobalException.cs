using System;

namespace GoldNet.Comm
{
	/// <summary>
	/// 全局通用异常类
	/// 包含错误标题和错误内容
	/// 
	/// 扩展：
	///   应考虑保留引发异常的类名或文件名等。目前可以把此信息包含在content中。
	/// </summary>
	public class GlobalException : Exception
	{
		private string _title;
		private string _content;
		private Exception _innerException;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="title">异常的标题</param>
		/// <param name="content">异常的详细内容</param>
		public GlobalException(string title, string content)
		{
			_title = title;
			_content = content;
			_innerException = null;
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="title">异常标题</param>
		/// <param name="content">异常的详细内容</param>
		/// <param name="innerException">引发异常的子异常</param>
		public GlobalException(string title, string content, Exception innerException)
		{
			_title = title;
			_content = content;
			_innerException = innerException;
		}

		/// <summary>
		/// 返回异常的标题信息
		/// </summary>
		public string Title
		{
			get
			{
				return _title;
			}
		}

		/// <summary>
		/// 返回异常的详细内容
		/// </summary>
		public string Content
		{
			get
			{
				return _content;
			}
		}

		/// <summary>
		/// 重写了Exception的Message属性。
		/// </summary>
		public new string Message
		{
			get
			{
				return "Title:" + _title + ";Content:" + _content + ";Base Message:" + base.Message;
			}
		}

		/// <summary>
		/// 返回引发异常的子异常
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
