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
using System.Text;
using GoldNet.Model;
namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_diag_detail : PageBase
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

                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");

                XyhsDict dal = new XyhsDict();
                DataTable dt = dal.GetDiag().Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        this.Combo_diag.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["DIAGNOSIS_NAME"].ToString(), dt.Rows[i]["DIAGNOSIS_CODE"].ToString()));
                    }
                }
            }
        }
        //查询绑定数据
        private void Bindlist()
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            string dept_type = this.Combo_DeptType.SelectedItem.Value.ToString();
            XyhsDetail dal = new XyhsDetail();
            //科室过滤条件
            DataTable dt = dal.GetXyhsDiagaccountdetail(date_time,"","","").Tables[0];
            DataRow dr = dt.NewRow();
            dr["patient_name"] = "合计";
            dr["incomes"] = dt.Compute("Sum(incomes)", "");
            dr["COSTS"] = dt.Compute("Sum(COSTS)", "");
            dr["INCOMES_CHARGES"] = dt.Compute("Sum(INCOMES_CHARGES)", "");
            dr["COST_COSTS_CHARGES"] = dt.Compute("Sum(COST_COSTS_CHARGES)", "");
            //dr["COUNT_INCOME"] = dt.Compute("Sum(COUNT_INCOME)", "");
            dr["INCOMES_ALL"] = dt.Compute("Sum(INCOMES_ALL)", "");
            dr["COSTS_ALL"] = dt.Compute("Sum(COSTS_ALL)", "");
            dt.Rows.Add(dr);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
            Session["patientdeptdetail"] = dt;
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["patientdeptdetail"] == null)
                return;

            TableCell[] header = new TableCell[10];

            header[0] = new TableHeaderCell();
            header[0].Text = "病人id";

            header[1] = new TableHeaderCell();
            header[1].Text = "病人名称";
            header[2] = new TableHeaderCell();
            header[2].Text = "科室名称";

            header[3] = new TableHeaderCell();
            header[3].Text = "开单医生";
            header[4] = new TableHeaderCell();
            header[4].Text = "地方收入";
            header[5] = new TableHeaderCell();
            header[5].Text = "军免收入";
            header[6] = new TableHeaderCell();
            header[6].Text = "收入合计";
            header[7] = new TableHeaderCell();
            header[7].Text = "地方成本";
            header[8] = new TableHeaderCell();
            header[8].Text = "军免成本";
            header[9] = new TableHeaderCell();
            header[9].Text = "成本合计";
            MHeaderTabletoExcel((DataTable)Session["patientdeptdetail"], header, "病种核算报表" + DateTime.Now, null, 0);
        }
        //界面查询事件
        protected void Button_find_click(object sender, EventArgs e)
        {
            Bindlist();
        }
        protected void select_dept(object sender, EventArgs e)
        {
            Bindlist();
        }
        //日期选择改变事件
        protected void Data_SelectOnChange(object sender, EventArgs e)
        {
            // Bindlist();
        }

        //明细、构成比显示事件
        protected void Btn_Command_Click(object sender, AjaxEventArgs e)
        {

        }

    }
}
