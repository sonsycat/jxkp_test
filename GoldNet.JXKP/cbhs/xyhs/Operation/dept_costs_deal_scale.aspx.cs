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
using System.Text;
using Goldnet.Dal;
using Goldnet.Ext.Web;
namespace GoldNet.JXKP.cbhs.xyhs.Operation
{
    public partial class dept_costs_deal_scale :PageBase
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
                string op_type = Request.QueryString["op_type"].ToString();
                if (op_type.Equals("收入"))
                {
                    //收入构成比
                    this.func.Hidden = true;
                    this.Combo_type.Hidden = true;
                }
                else
                {
                    //成本构成比
                    this.func.Hidden = false;
                    this.Combo_type.Hidden = false;
                }
                this.dd1Name.Text = date_time + "-" + dept_name;
                this.Combo_type.SelectedItem.Value = "1";
                BindData(date_time, dept_code, "1");
            }

        }

        private void BindData(string date_time, string dept_code, string dis_type)
        {
            string op_type = Request.QueryString["op_type"].ToString();
            XyhsOperation dal = new XyhsOperation();
            DataTable dt = new DataTable();
            if (op_type != null && op_type.Equals("收入"))
            {
                dt = dal.GetChartSrData(date_time, dept_code).Tables[0];
            }
            if (op_type != null && op_type.Equals("成本"))
            {
                if (dis_type == "1")
                {
                    dt = dal.GetChartDataByItem(date_time, dept_code).Tables[0];
                }
                else
                {
                    dt = dal.GetChartDataByType(date_time, dept_code).Tables[0];
                }
            }
            CreateChart(dt);

        }
        private void CreateChart(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                StringBuilder Series = new StringBuilder();
                Series.Append("[{ type:'pie',name:'饼型分析图', data: [");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["COSTS"].ToString() != "0")
                    {
                        Series.Append("{name:'");
                        Series.Append(dt.Rows[i]["ITEM_NAME"].ToString());
                        Series.Append("',y:");
                        Series.Append(dt.Rows[i]["COSTS"].ToString());
                        Series.Append("},");
                    }
                }
                Series.Remove(Series.Length - 1, 1).Append("]}]");

                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("clare();");
                if (Series.ToString() != "")
                {
                    scManager.AddScript(" refreshCharts1(" + Series.ToString() + ");");
                }
            }
        }
        //显示类别改变
        protected void Type_Selected(object sender, EventArgs e)
        {
            string date_time = Request.QueryString["date_time"].ToString();
            string dept_code = Request.QueryString["dept_code"].ToString();
            string dis_type = this.Combo_type.SelectedItem.Value.ToString();
            BindData(date_time, dept_code, dis_type);
        }
    }
}
