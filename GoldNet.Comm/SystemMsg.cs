using System;
using System.Collections.Generic;
using System.Text;

namespace GoldNet.Comm
{
    /// <summary>
    /// 系统提示信息公共类
    /// </summary>
    public class SystemMsg
    {
        public static string msgtitle1 = "提示信息";
        public static string msgtitle2 = "系统错误";
        public static string msgtitle3 = "友情提示";
        public static string msgtitle4 = "操作提示";
        public static string msgop0 = "尝试登录";
        public static string msgop1 = "登录系统";

        public static string msgtipnull = "不能为空!";
        public static string msgtip0 = "页面转向中...";
        public static string msgtip1 = "请输入用户名和登录密码!";
        public static string msgtip2 = "用户登录认证失败!";
        public static string msgtip3 = "用户登录认证发生异常,请联系管理员!";
        public static string msgtip4 = "用户登录信息更新失败!";
        public static string msgtip5 = "用户登录信息更新发生异常,请联系管理员!";
        public static string msgtip6 = "用户菜单权限信息错误!";
        public static string msgtip7 = "获取用户权限菜单发生异常,请联系管理员!";
        public static string msgtip8 = "用户登录系统成功!";

        public static string msgdatatitle = "系统提示";
        public static string msgdatacontent = "数据库操作错误！";



        #region 诊断中心
        /// <summary>
        /// 不能在二级菜单下再创建菜单
        /// </summary>
        public static string msg_Guide1 = "不能在二级菜单下再创建菜单！";

        /// <summary>
        /// 类别名称不能为空
        /// </summary>
        public static string msg_Guide2 = "类别名称不能为空！";

        /// <summary>
        /// 添加成功
        /// </summary>
        public static string msg_Guide3 = "添加成功！";

        /// <summary>
        /// 修改成功
        /// </summary>
        public static string msg_Guide4 = "修改成功！";

        /// <summary>
        /// 删除成功
        /// </summary>
        public static string msg_Guide5 = "删除成功！";

        /// <summary>
        /// 你选择的报表类别有下级报表类别，请删除下级报表类别后在删除此结点
        /// </summary>
        public static string msg_Guide6 = "你选择的报表类别有下级报表类别，请删除下级报表类别后在删除此结点。！";

        #endregion


    }
}
