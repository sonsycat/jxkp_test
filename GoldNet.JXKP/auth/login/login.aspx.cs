using System;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;

namespace Goldnet.JXKP.auth.login
{
    public partial class login : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.Web.Ext.IsAjaxRequest)
            {
                //bool loginflag = true;
                //string dates = System.Configuration.ConfigurationManager.AppSettings["Dates"].ToString();
                //string datess = System.Configuration.ConfigurationManager.AppSettings["Datess"].ToString();
                //string testdates = "2014-10-01";
                //string MacPsw = DEncrypt.Encrypt(testdates);
                //string DMacPsw = DEncrypt.Decrypt(dates);
                //string TestString = GetConfig.TestString;
                //if (TestString != "god")
                //{
                //    try
                //    {
                //        string Macstr = DEncrypt.GetMacAddress() + DMacPsw;
                //        //Macstr = "005056816C672014-10-01";
                //        string MacPsws = DEncrypt.Encrypt(Macstr, Macstr);
                //        string DMacPsws = DEncrypt.Decrypt(TestString, Macstr);


                //        if (Macstr != DMacPsws)
                //        {
                //            yz.InnerText = string.Format("系统没有注册，请与管理员联系！");
                //            loginflag = false;
                //            this.UNAME.ReadOnly = true;
                //            this.PASSWORD.ReadOnly = true;
                //            this.Button2.Disabled = true;
                //            return;
                //        }
                //    }
                //    catch
                //    {
                //        yz.InnerText = string.Format("系统没有注册，请与管理员联系！");
                //        loginflag = false;
                //        this.UNAME.ReadOnly = true;
                //        this.PASSWORD.ReadOnly = true;
                //        this.Button2.Disabled = true;
                //        return;
                //    }
                //}
                //try
                //{
                //    DateTime t1 = DateTime.Parse(DMacPsw);
                //    DateTime t2 = DateTime.Now.Date;
                //    System.TimeSpan ts = t1 - t2;
                //    if (t2 < t1)
                //    {
                //        if (ts.Days > 0 && ts.Days < int.Parse(datess))
                //        {
                //            yz.InnerText = string.Format("程序在试用期，还有{0}天到期！", ts.Days);

                //        }
                //    }

                //    else
                //    {
                //        yz.InnerText = string.Format("程序试用期已过，请与管理员联系！");
                //        loginflag = false;
                //        this.UNAME.ReadOnly = true;
                //        this.PASSWORD.ReadOnly = true;
                //        this.Button2.Disabled = true;
                //        return;
                //    }
                //}
                //catch
                //{
                //    yz.InnerText = string.Format("程序试用期已过，请与管理员联系！");
                //    loginflag = false;
                //    this.UNAME.ReadOnly = true;
                //    this.PASSWORD.ReadOnly = true;
                //    this.Button2.Disabled = true;
                //    return;
                //}

                //

                bq.InnerText = string.Format("2014-{0} 心医科级", System.DateTime.Now.Year);
                Session.RemoveAll();
                Session.Clear();
                //this.UNAME.Focus();
                //this.hosname.InnerHtml = System.Configuration.ConfigurationManager.AppSettings["HospitalName"].ToString();
                ////GetLogin();
                //if (Request["u"] != null && Request["p"] != null && loginflag == true)
                //{
                //    user_login(Request["u"].ToString().Trim().Replace("'", "").Replace("--", "").ToUpper(), Request["p"].ToString().Trim().Replace("'", "").Replace("--", ""));
                //}
                //if (Request["u"] != null && Request["p"] == null && loginflag == true)
                //{
                //    this.UNAME.Text = Request["u"].ToString().Trim().Replace("'", "").Replace("--", "");
                //    this.PASSWORD.Focus();
                //}
            }
        }

        /// <summary>
        /// 登陆按钮事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, AjaxEventArgs e)
        {
            string username = this.UNAME.Text.ToUpper().Trim().Replace("'", "").Replace("--", "");
            string psw = this.PASSWORD.Text.Trim().Replace("'", "").Replace("--", "");
            user_login(username, psw);
        }

        /// <summary>
        /// 用户登陆处理
        /// </summary>
        /// <param name="username"></param>
        /// <param name="psw"></param>
        protected void user_login(string username, string psw)
        {
            string DBUSER = username;
            //非his人员登录
            GoldNet.Model.SysManager.register reg = new GoldNet.Model.SysManager.register();

            //检查是否是HIS人员
            string ishis = reg.VerificationHis(DBUSER);
            if (ishis == "1" || ishis == "3")
            {
                //非HIS用户验证
                string strUserName = username;
                string strPassword = psw;
                NotHisLogin(strUserName, strPassword, ishis);
            }
            else if (ishis == "0" || ishis == "2")
            {
                //HIS用户验证
                User UserInstance = null;
                try
                {
                    string strUserName = username;
                    string strPassword = psw;

                    UserInstance = new User(strUserName, strPassword);

                    //将用户实例置入session会话状态中保存
                    Session.Add("CURRENTSTAFF", UserInstance);

                    //页面迁移跳转至相应的首页
                    Mask.Config msgconfig = new Mask.Config();
                    msgconfig.Msg = SystemMsg.msgtip0;
                    msgconfig.MsgCls = "x-mask-loading";
                    Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
                    if (UserInstance.GetUserPower.Select("modid='8'").Length > 0)
                    {
                        Goldnet.Ext.Web.Ext.Redirect("/home/default_01.aspx");
                    }
                    else
                    {
                        Goldnet.Ext.Web.Ext.Redirect("/home/default.aspx");
                    }
                }
                catch (Exception ex)
                {
                    //ShowDataError(ex, Request.Url.LocalPath, "Button1_Click");

                    if (ex is GlobalException)
                    {
                        ShowMessage(((GlobalException)ex).Title, ((GlobalException)ex).Content);
                    }
                    else
                    {
                        ShowMessage(SystemMsg.msgdatatitle, SystemMsg.msgdatacontent);
                    }
                }
            }

        }
        protected void ShowMessage(string title, string message)
        {
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = title,
                Message = message + "   ",
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO"),
                Modal = true
            });

        }

        /// <summary>
        /// 非HIS用户验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="ishis"></param>
        private void NotHisLogin(string username, string password, string ishis)
        {
            User UserInstance = null;
            try
            {
                string strUserName = username;
                string strPassword = password;
                GoldNet.Model.SysManager.register reg = new GoldNet.Model.SysManager.register();

                UserInstance = new User(reg.GetHotHisUser(strUserName.ToUpper(), strPassword, ishis));

                //将用户实例置入session会话状态中保存
                Session.Add("CURRENTSTAFF", UserInstance);


                //页面迁移跳转至相应的首页
                Mask.Config msgconfig = new Mask.Config();
                msgconfig.Msg = SystemMsg.msgtip0;
                msgconfig.MsgCls = "x-mask-loading";
                Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
                if (UserInstance.GetUserPower.Select("modid='8'").Length > 0)
                {
                    Goldnet.Ext.Web.Ext.Redirect("/home/default_02.aspx");
                }
                else
                {
                    Goldnet.Ext.Web.Ext.Redirect("/home/default.aspx");
                }
            }
            catch (Exception ex)
            {
                //ShowDataError(ex, Request.Url.LocalPath, "Button1_Click");

                if (ex is GlobalException)
                {
                    ShowMessage(((GlobalException)ex).Title, ((GlobalException)ex).Content);
                }
                else
                {
                    ShowMessage(SystemMsg.msgdatatitle, SystemMsg.msgdatacontent);
                }
            }
        }


        private void GetLogin()
        {
            string DBUSER = string.Empty;
            string strUserName = string.Empty;
            string strPassword = string.Empty;

            if (Request.QueryString["usnm"] != null && Request.QueryString["usnm"] != "" && Request.QueryString["pswd"] != null && Request.QueryString["pswd"] != "")
            {
                DBUSER = Request.QueryString["usnm"].ToString().ToUpper().Trim().Replace("'", "").Replace("--", "");
                strUserName = Request.QueryString["usnm"].ToString().ToUpper().Trim().Replace("'", "").Replace("--", "");
                strPassword = Request.QueryString["pswd"].ToString().ToUpper().Trim().Replace("'", "").Replace("--", "");



                //非his人员登录
                GoldNet.Model.SysManager.register reg = new GoldNet.Model.SysManager.register();
                string ishis = reg.VerificationHis(DBUSER);
                if (ishis == "1" || ishis == "3")
                {
                    NotHisLogin(strUserName, strPassword, ishis);
                }
                else if (ishis == "0")
                {
                    User UserInstance = null;

                    try
                    {



                        UserInstance = new User(strUserName, strPassword.ToUpper());

                        //将用户实例置入session会话状态中保存
                        Session.Add("CURRENTSTAFF", UserInstance);


                        //页面迁移跳转至相应的首页
                        Mask.Config msgconfig = new Mask.Config();
                        msgconfig.Msg = SystemMsg.msgtip0;
                        msgconfig.MsgCls = "x-mask-loading";
                        Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
                        if (UserInstance.GetUserPower.Select("modid='8'").Length > 0)
                        {
                            Goldnet.Ext.Web.Ext.Redirect("/home/default_02.aspx");
                        }
                        else
                        {
                            Goldnet.Ext.Web.Ext.Redirect("/home/default.aspx");
                        }
                    }
                    catch (Exception ex)
                    {
                        //ShowDataError(ex, Request.Url.LocalPath, "Button1_Click");

                        if (ex is GlobalException)
                        {
                            ShowMessage(((GlobalException)ex).Title, ((GlobalException)ex).Content);
                        }
                        else
                        {
                            ShowMessage(SystemMsg.msgdatatitle, SystemMsg.msgdatacontent);
                        }
                    }
                }
                else if (ishis == "2")
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "用户名错误",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
            }
        }


    }
}
