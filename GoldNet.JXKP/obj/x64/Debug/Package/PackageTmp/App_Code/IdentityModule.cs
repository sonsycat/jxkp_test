using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Goldnet.Ext.Web;
using Goldnet.JXKP;


/// <summary>
/// IdentityModule 的摘要说明
/// </summary>
public class IdentityModule : IHttpModule
{
    public IdentityModule()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    #region IHttpModule 成员

    public void Dispose()
    {
    }

    public void Init(HttpApplication context)
    {
        context.AcquireRequestState += new EventHandler(OnMyRequest);
    }

    public static void OnMyRequest(object sender, EventArgs e)
    {
        HttpApplication context = (HttpApplication)sender;

       // IUser user = ManagementFactory.CreateUser(context.Session);
       // string currentUser = user.GetCurrentUserId();
       // if (currentUser == null)
       // {
       //     string currentPage = context.Request.Url.AbsolutePath.Substring(context.Request.Url.AbsolutePath.LastIndexOf("/") + 1);

       ////     if (currentPage.ToLower() != "login.aspx")
       // //    {
       //  //       context.Response.Redirect(@"login.aspx");
       //   //  }

       // }

    }

    #endregion
}
