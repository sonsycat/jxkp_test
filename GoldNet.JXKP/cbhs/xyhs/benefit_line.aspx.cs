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
    public partial class benefit_line : PageBase
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
                string date_time = Request.QueryString["date_time"].ToString();
                string dept_code = Request.QueryString["dept_code"].ToString();
                string dept_name = Request.QueryString["dept_name"].ToString();
                BindData(date_time, dept_code, dept_name);
            }
        }
        private void BindData(string date_time, string dept_code,string dept_name)
        {
            XyhsDetail dal = new XyhsDetail();
            double charges = Convert.ToDouble(dal.GetLineChartCharges(date_time, dept_code).Tables[0].Rows[0][0]);
            DataTable dt = dal.GetLineChartCosts(date_time, dept_code).Tables[0];
            double costs = Convert.ToDouble(dt.Rows[0]["COSTS"]);
            double gd_costs = Convert.ToDouble(dt.Rows[0]["gd_costs"]);
            double x,y;
            try
            {
                x = gd_costs / (charges / 100 - (costs - gd_costs) / 100);
                y = x * charges / 100;
                CreateChart(dept_name,charges, costs, gd_costs, Math.Round(x, 2), Math.Round(y, 2));
            }
            catch(Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "BindData");
            }
        }
        private void CreateChart(string dept_name,double charges,double costs,double gd_costs,double x,double y)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("clare();");
            scManager.AddScript(" refreshCharts1('" + dept_name + "'," + charges + "," + costs + "," + gd_costs + "," + x + "," + y + ");");

        }
    }
}
