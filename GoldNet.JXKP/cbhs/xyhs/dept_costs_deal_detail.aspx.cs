using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class dept_costs_deal_detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    Response.End();
                }
                string dept_code = Request.QueryString["dept_code"].ToString();
                string date_time = Request.QueryString["date_time"].ToString();
                string dept_name = Request.QueryString["dept_name"].ToString();
                this.dd1Name.Text = date_time + "-" + dept_name;
                Bindlist(date_time, dept_code);
            }

        }
        private void Bindlist(string date_time, string dept_code)
        {
            XyhsDetail dal = new XyhsDetail();
            DataTable dt =  dal.GetCostsDealDetail(date_time, dept_code).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
    }
}
