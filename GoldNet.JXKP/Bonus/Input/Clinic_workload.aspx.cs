using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
namespace GoldNet.JXKP.Bonus.Input
{
    public partial class Clinic_workload : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string year = DateTime.Now.AddMonths(-1).Year.ToString();
                string month = DateTime.Now.AddMonths(-1).Month.ToString();

                BoundComm boundcomm = new BoundComm();
                //初始化年度列表
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                //初始化月份下拉列表
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            OutRestNonusDal dal = new OutRestNonusDal();

            DataTable dt = dal.GetClinicWorkload(GetBeginDate(), "");
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("Clinic_workload");
                Session["Clinic_workload"] = dt;
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
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["Clinic_workload"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Clinic_workload"];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "VISIT_DATE")
                    {
                        dt.Columns[i].ColumnName = "日期";
                    }
                    else if (dt.Columns[i].ColumnName == "VISIT_DEPT")
                    {
                        dt.Columns[i].ColumnName = "科室代码";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "CLINIC_LABEL")
                    {
                        dt.Columns[i].ColumnName = "号别";
                    }
                    else if (dt.Columns[i].ColumnName == "CLINIC_TYPE")
                    {
                        dt.Columns[i].ColumnName = "号类";
                    }
                    else if (dt.Columns[i].ColumnName == "CON")
                    {
                        dt.Columns[i].ColumnName = "数量";
                    }

                }
                string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月";
                ex.ExportToLocal(dt, this.Page, "xls", "门诊专家（不含双休日）工作量统计(" + dates + ")");
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + "" + month + "01";
            return benginDate;
        }
    }
}
