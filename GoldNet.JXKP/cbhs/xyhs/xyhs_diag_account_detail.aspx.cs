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
    public partial class xyhs_diag_account_detail : PageBase
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
                string diag_code = Request.QueryString["diag_code"].ToString();
                string date_time = Request.QueryString["date_time"].ToString();
                string diag_name = Request.QueryString["diag_name"].ToString();
                this.dd1Name.Text = date_time + "-" + diag_name;
                Bindlist(diag_code, date_time);
            }

        }
        private void Bindlist(string diag_code, string date_time)
        {
            XyhsDetail dal = new XyhsDetail();
            DataTable dt = dal.GetXyhsdiagdetail(diag_code, date_time).Tables[0];
            DataRow dr = dt.NewRow();
            dr["dept_name"] = "合计";
            dr["incomes"] = dt.Compute("Sum(incomes)", "");
            dr["COSTS"] = dt.Compute("Sum(COSTS)", "");

            dt.Rows.Add(dr);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
    }
}
