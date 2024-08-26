using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using GoldNet.Model;
using Goldnet.Ext.Web;
using GoldNet.Comm;

namespace GoldNet.JXKP
{
    public class UserControlBase : System.Web.UI.UserControl
    {
        /// <summary>
        /// 是否可编辑
        /// </summary>
        /// <returns></returns>
        protected bool IsEdit()
        {

            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');

            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            GoldNet.Model.User curuser = new User(user.UserId);
            return curuser.SetControlEdit(menuid, modid);
        }
        /// <summary>
        /// 审核（上报）也可以用在页面的第二个功能
        /// </summary>
        /// <returns></returns>
        protected bool IsPass()
        {

            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');

            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            GoldNet.Model.User curuser = new User(user.UserId);
            return curuser.SetControlPass(menuid, modid);
        }
        /// <summary>
        /// 科室权限
        /// </summary>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        protected string DeptFilter(string deptfilter)
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');

            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            GoldNet.Model.User curuser = new User(user.UserId);
            return curuser.GetUserDeptFilter(deptfilter, menuid, modid);
        }
        
        /// <summary>
        /// 人员权限
        /// </summary>
        /// <param name="userfilter"></param>
        /// <returns></returns>
        protected string UserFilter(string userfilter)
        {
            User user = (User)Session["CURRENTSTAFF"];
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');

            string menuid = pageids[0].ToString();
            string modid = pageids[1].ToString();
            GoldNet.Model.User curuser = new User(user.UserId);
            return curuser.GetUserFilter(userfilter, menuid, modid);
        }
        /// <summary>
        /// 保存成功
        /// </summary>
        protected void SaveSucceed()
        {
            ShowMessage("系统提示", "保存成功！");
        }
        /// <summary>
        /// 选择一条记录
        /// </summary>
        protected void SelectRecord()
        {
            ShowMessage("系统提示", "请选择一条记录！");
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected void ShowMessage(string title, string message)
        {
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = title,
                Message = message,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }
        
    }
}
