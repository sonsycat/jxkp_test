using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldNet.Comm
{
    public sealed class Constant
    {
        /// <summary>
        /// 质量项目编号
        /// </summary>
        public static string ZLGL_FUN_TYPE = "3";
        /// <summary>
        /// 专业质量类别
        /// </summary>
        public static string SPEGUIDETYPE = "2";

        /// <summary>
        /// 医院名称
        /// </summary>
        public static string HospitalName = GoldNet.Comm.GetConfig.GetConfigString("HospitalName");

        /// <summary>
        /// 特殊权限类别设置（人员类别）
        /// </summary>
        public static string PER_SPEPOWER = "1";
        /// <summary>
        /// 特殊权限类别设置（成本类别）
        /// </summary>
        public static string COST_SPEPOWER = "2";
        public static string Stastaffid
        {
            get;set;
        }

        /// <summary>
        /// 特殊权限类别设置（全成本类别）
        /// </summary>
        public static string XYHS_COST_SPEPOWER = "4";

    }
}
