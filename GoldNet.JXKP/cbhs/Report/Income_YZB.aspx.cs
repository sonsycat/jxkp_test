using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.cbhs.Report
{
    public partial class Income_YZB : PageBase
    {
        private Goldnet.Dal.Report report_dal = new Goldnet.Dal.Report();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string year = DateTime.Now.AddMonths(-1).Year.ToString();
                string month = DateTime.Now.AddMonths(-1).Month.ToString();
                BoundComm boundcomm = new BoundComm();
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                ccbYearTo.Value = year;
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ccbMonthTo.Value = month;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {




            DataTable dt = report_dal.GetIncode_YZB(GetBeginDate(), GetEndDate(), cbbType.SelectedItem.Value);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("Income_YZB");
                Session["Income_YZB"] = dt;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["Income_YZB"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Income_YZB"];



                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "HJ")
                    {
                        dt.Columns[i].ColumnName = "合计";
                    }
                    else if (dt.Columns[i].ColumnName == "YP")
                    {
                        dt.Columns[i].ColumnName = "药品";
                    }
                    else if (dt.Columns[i].ColumnName == "JMYP")
                    {
                        dt.Columns[i].ColumnName = "减免药品";
                    }
                    else if (dt.Columns[i].ColumnName == "JY")
                    {
                        dt.Columns[i].ColumnName = "检验";
                    }
                    else if (dt.Columns[i].ColumnName == "JC")
                    {
                        dt.Columns[i].ColumnName = "检查";
                    }
                    else if (dt.Columns[i].ColumnName == "ZL")
                    {
                        dt.Columns[i].ColumnName = "治疗";
                    }
                    else if (dt.Columns[i].ColumnName == "FS")
                    {
                        dt.Columns[i].ColumnName = "放射";
                    }
                    else if (dt.Columns[i].ColumnName == "SS")
                    {
                        dt.Columns[i].ColumnName = "手术";
                    }
                    else if (dt.Columns[i].ColumnName == "SX")
                    {
                        dt.Columns[i].ColumnName = "输血";
                    }
                    else if (dt.Columns[i].ColumnName == "HL")
                    {
                        dt.Columns[i].ColumnName = "护理";
                    }
                    else if (dt.Columns[i].ColumnName == "GH")
                    {
                        dt.Columns[i].ColumnName = "挂号";
                    }
                    else if (dt.Columns[i].ColumnName == "CW")
                    {
                        dt.Columns[i].ColumnName = "床位";
                    }
                    else if (dt.Columns[i].ColumnName == "QT")
                    {
                        dt.Columns[i].ColumnName = "其它";
                    }
                    else if (dt.Columns[i].ColumnName == "ZLSR")
                    {
                        dt.Columns[i].ColumnName = "治疗收入";
                    }
                    else if (dt.Columns[i].ColumnName == "ZLF")
                    {
                        dt.Columns[i].ColumnName = "诊疗费";
                    }
                    else if (dt.Columns[i].ColumnName == "MZ")
                    {
                        dt.Columns[i].ColumnName = "麻醉";
                    }
                    else if (dt.Columns[i].ColumnName == "ZLFF")
                    {
                        dt.Columns[i].ColumnName = "材料";
                    }
                    else if (dt.Columns[i].ColumnName == "YZB")
                    {
                        dt.Columns[i].ColumnName = "药占比";
                    }

                }
                string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月-" + this.ccbYearTo.SelectedItem.Value + "年" + this.ccbMonthTo.SelectedItem.Value + "月";
                ex.ExportToLocal(dt, this.Page, "xls", "药占比汇总(" + dates + ")");
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
            string benginDate = year + month;
            return benginDate;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        private string GetEndDate()
        {
            string year = ccbYearTo.SelectedItem.Value.ToString();
            string month = ccbMonthTo.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string endDate = year + month;
            return endDate;
        }







    }
}