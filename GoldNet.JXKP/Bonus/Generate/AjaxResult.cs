﻿using System.Web.Script.Serialization;

namespace GoldNet.JXKP
{
    public class AjaxResult
    {
        private AjaxResult()
        {
        }

        private bool iserror = false;

        /// <summary>
        /// 是否产生错误
        /// </summary>
        public bool IsError { get { return iserror; } }

        /// <summary>
        /// 错误信息，或者成功信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 成功可能时返回的数据
        /// </summary>
        public object Data { get; set; }

        public static AjaxResult Error()
        {
            return new AjaxResult()
            {
                iserror = true
            };
        }

        public static AjaxResult Error(string message)
        {
            return new AjaxResult()
            {
                iserror = true,
                Message = message
            };
        }

        public static AjaxResult Success()
        {
            return new AjaxResult()
            {
                iserror = false
            };
        }
        public static AjaxResult Success(string message)
        {
            return new AjaxResult()
            {
                iserror = false,
                Message = message
            };
        }
        public static AjaxResult Success(object data)
        {
            return new AjaxResult()
            {
                iserror = false,
                Data = data
            };
        }
        public static AjaxResult Success(object data, string message)
        {
            return new AjaxResult()
            {
                iserror = false,
                Data = data,
                Message = message
            };
        }

        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
        }

    }
}