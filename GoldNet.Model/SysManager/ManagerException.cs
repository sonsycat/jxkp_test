using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoldNet.Comm;

namespace GoldNet.Model
{
    public class ManagerException : GlobalException
    {
         /// <summary>
        /// 初始化异常
        /// </summary>
        /// <param name="title">异常标题</param>
        /// <param name="content">异常内容</param>
        public ManagerException(string title, string content) : base(title, content) { }

        /// <summary>
        /// 初始化异常
        /// </summary>
        /// <param name="title">异常标题</param>
        /// <param name="content">异常内容</param>
        /// <param name="innerException">子异常</param>
        public ManagerException(string title, string content, Exception innerException) : base(title, content, innerException) { }
    }
    /// <summary>
    /// 登录数据异常
    /// </summary>
    public class LoginManagerException : ManagerException
    {
        public LoginManagerException(string title, string Message) : base(title, "登录数据异常：" + Message) { }
    }
    /// <summary>
    /// 密码错误
    /// </summary>
    public class LoginPWErrorException : ManagerException
    {
        private const string Mess = "密码错误，请重新输入！";
        public LoginPWErrorException() : base("系统提示", Mess) { }
    }
}
