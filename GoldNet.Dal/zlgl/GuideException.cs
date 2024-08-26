using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoldNet.Comm;

namespace Goldnet.Dal
{
   public class GuideException : GlobalException
    {
        /// <summary>
        /// 初始化异常
        /// </summary>
        /// <param name="title">异常标题</param>
        /// <param name="content">异常内容</param>
        public GuideException(string title, string content) : base(title, content) { }

        /// <summary>
        /// 初始化异常
        /// </summary>
        /// <param name="title">异常标题</param>
        /// <param name="content">异常内容</param>
        /// <param name="innerException">子异常</param>
        public GuideException(string title, string content, Exception innerException) : base(title, content, innerException) { }
    }
   /// <summary>
   /// 保存数据时异常
   /// </summary>
   public class SaveRecordDataException : GuideException
   {
       public SaveRecordDataException(string title, string Message) : base(title, "详细信息为：" + Message) { }
   }

   
}
