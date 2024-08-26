using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook.Statement
{
    public partial class PerBenefit : System.Web.UI.Page
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
                string fromdate = Request.QueryString["FromDate"].ToString();
                string todate = Request.QueryString["ToDate"].ToString();
                string type = Request.QueryString["Type"].ToString();
                string staffid = Request.QueryString["StaffId"].ToString();
                string deptCode = Request.QueryString["DeptCode"].ToString();
                string itemClass = Request.QueryString["itemClass"].ToString();
                this.Store1.DataSource = dal.getPerBenefitInfo(fromdate, todate, deptCode, staffid, type,itemClass).Tables[0];
                this.Store1.DataBind();
            }
        }
    }
}
