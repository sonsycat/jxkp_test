using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class DeptCostAccount_detail_new : PageBase
    {
        Report dal_report = new Report();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string stardate = Request["stardate"].ToString();
                string enddate = Request["enddate"].ToString();
                string deptcode = Request["deptcode"].ToString();
                string balances = Request["balances"].ToString();

                data(stardate, enddate, deptcode, balances);
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <param name="deptcode"></param>
        /// <param name="balances"></param>
        /// <param name="gettype"></param>
        private void data(string stardate, string enddate, string deptcode, string balances)
        {
            DataTable dt = dal_report.GetHospitalCostBenefit_new(stardate, enddate, balances, deptcode);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("HospitalCostBenefit2");
                Session["HospitalCostBenefit2"] = dt;
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "未找到数据",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }

        /// <summary>
        /// 返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Cost_Detail.hide();");
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["HospitalCostBenefit2"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["HospitalCostBenefit2"];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "FACACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入";
                    }
                    else if (dt.Columns[i].ColumnName == "COSTS")
                    {
                        dt.Columns[i].ColumnName = "实际成本";
                    }
                    else if (dt.Columns[i].ColumnName == "PROFIT")
                    {
                        dt.Columns[i].ColumnName = "实际收益";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMYCOSTS")
                    {
                        dt.Columns[i].ColumnName = "计价成本";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_CODE")
                    {
                        dt.Columns[i].ColumnName = "科室代码";
                    }
                    else if (dt.Columns[i].ColumnName == "MEDINCOMES")
                    {
                        dt.Columns[i].ColumnName = "药品收益";
                    }
                }

                ex.ExportToLocal(dt, this.Page, "xls", "医疗收支,药品效益汇总表");
            }
        }


    }
}
