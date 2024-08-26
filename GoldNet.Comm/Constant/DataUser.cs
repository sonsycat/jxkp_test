using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldNet.Comm
{
    public static class DataUser
    {
        /// <summary>
        /// HIS服务器
        /// </summary>
        public static string HIS = GoldNet.Comm.GetConfig.GetConfigString("HIS");
       /// <summary>
       /// 系统管理用户
       /// </summary>
        public static string COMM = GoldNet.Comm.GetConfig.GetConfigString("COMM");
       /// <summary>
       /// 成本核算用户
       /// </summary>
        public static string CBHS = GoldNet.Comm.GetConfig.GetConfigString("CBHS");
        /// <summary>
        /// hisdata原始表
        /// </summary>
        public static string HISDATA = GoldNet.Comm.GetConfig.GetConfigString("HISDATA");
        /// <summary>
        /// hisfact中间表
        /// </summary>
        public static string HISFACT = GoldNet.Comm.GetConfig.GetConfigString("HISFACT");
        /// <summary>
        /// 质量管理
        /// </summary>
        public static string ZLGL = GoldNet.Comm.GetConfig.GetConfigString("ZLGL");
        /// <summary>
        /// 绩效
        /// </summary>
        public static string HOSPITALSYS = GoldNet.Comm.GetConfig.GetConfigString("HOSPITALSYS");
        /// <summary>
        /// 人力资源
        /// </summary>
        public static string RLZY = GoldNet.Comm.GetConfig.GetConfigString("RLZY");
        /// <summary>
        /// 奖金
        /// </summary>
        public static string PERFORMANCE = GoldNet.Comm.GetConfig.GetConfigString("PERFORMANCE");

        public static object comm { get; set; }
    }
}
