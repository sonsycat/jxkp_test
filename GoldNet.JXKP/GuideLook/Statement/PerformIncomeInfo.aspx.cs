using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook.Statement
{
    public partial class PerformIncomeInfo : System.Web.UI.Page
    {
        private StatementDal dal = new StatementDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }

            if (!Ext.IsAjaxRequest)
            {
                string deptCode = Request.QueryString["DeptCode"].ToString();
                string fromDate = Request.QueryString["FromDate"].ToString();
                string ToDate = Request.QueryString["ToDate"].ToString();
                string Type = Request.QueryString["Type"].ToString();
                string ClassCode = Request.QueryString["ClassCode"].ToString();
                this.Store1.DataSource = dal.getPerformIncomeInfo(fromDate, ToDate, deptCode, ClassCode, Type).Tables[0];
                this.Store1.DataBind();
            }
        }
    }
}
