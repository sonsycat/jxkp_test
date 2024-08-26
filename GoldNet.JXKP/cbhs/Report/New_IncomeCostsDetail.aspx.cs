using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.cbhs.Report
{
    public partial class New_IncomeCostsDetail : PageBase
    {
        Goldnet.Dal.Report dal_report = new Goldnet.Dal.Report();

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
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();

                cbbYear.Value = year;
                cbbmonth.Value = month;

                ccbYearTo.Value = year;
                ccbMonthTo.Value = month;

                DataTable dt = dal_report.GetAccountType();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.cbbType.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["TYPE_NAME"].ToString(), dt.Rows[i]["TYPE_CODE"].ToString()));
                }
                if (dt.Rows.Count > 0)
                {
                    this.cbbType.SelectedItem.Value = dt.Rows[0]["TYPE_CODE"].ToString();
                }

                cbbType.Value = 0;
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string balance = cbbType.SelectedItem.Value;

            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = dal_report.GetNew_IncomeCostsDetail(GetBeginDate(), GetEndDate(),cbbType.SelectedItem.Value);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("New_IncomeCostsDetail");
                Session["New_IncomeCostsDetail"] = dt;
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
        /// 数据导出处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["New_IncomeCostsDetail"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["New_IncomeCostsDetail"];


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "核算单位";
                    }
                    else if (dt.Columns[i].ColumnName == "Z_INCOME")
                    {
                        dt.Columns[i].ColumnName = "总收入";
                    }
                    else if (dt.Columns[i].ColumnName == "JJ_INCOME")
                    {
                        dt.Columns[i].ColumnName = "计价收入";
                    }
                    else if (dt.Columns[i].ColumnName == "SJ_INCOME")
                    {
                        dt.Columns[i].ColumnName = "实际收入";
                    }
                    else if (dt.Columns[i].ColumnName == "Z_COSTS")
                    {
                        dt.Columns[i].ColumnName = "总成本";
                    }
                    else if (dt.Columns[i].ColumnName == "Z_LIRUN")
                    {
                        dt.Columns[i].ColumnName = "总利润";
                    }
                    else if (dt.Columns[i].ColumnName == "J_LIRUN")
                    {
                        dt.Columns[i].ColumnName = "净利润";
                    }
                    else if (dt.Columns[i].ColumnName == "TQ_INCOME")
                    {
                        dt.Columns[i].ColumnName = "同期总收入";
                    }
                    else if (dt.Columns[i].ColumnName == "TQ_JJ_INCOME")
                    {
                        dt.Columns[i].ColumnName = "同期计价收入";
                    }
                    else if (dt.Columns[i].ColumnName == "TQ_SJ_INCOME")
                    {
                        dt.Columns[i].ColumnName = "同期实际收入";
                    }

                    else if (dt.Columns[i].ColumnName == "TQ_Z_COSTS")
                    {
                        dt.Columns[i].ColumnName = "同期总成本";
                    }
                    else if (dt.Columns[i].ColumnName == "TQ_Z_LIRUN")
                    {
                        dt.Columns[i].ColumnName = "同期总利润";
                    }
                    else if (dt.Columns[i].ColumnName == "TQ_J_LIRUN")
                    {
                        dt.Columns[i].ColumnName = "同期净利润";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZL_Z_INCOME")
                    {
                        dt.Columns[i].ColumnName = "同比增量总收入";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZL_JJ_INCOME")
                    {
                        dt.Columns[i].ColumnName = "同比增量计价收入";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZL_SJ_INCOME")
                    {
                        dt.Columns[i].ColumnName = "同比增量实际收入";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZL_Z_COSTS")
                    {
                        dt.Columns[i].ColumnName = "同比增量总成本";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZL_Z_LIRUN")
                    {
                        dt.Columns[i].ColumnName = "同比增量总利润";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZL_J_LIRUN")
                    {
                        dt.Columns[i].ColumnName = "同比增量净利润";
                    }

                    else if (dt.Columns[i].ColumnName == "TBZF_Z_INCOME")
                    {
                        dt.Columns[i].ColumnName = "同比增幅总收入";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZF_JJ_INCOME")
                    {
                        dt.Columns[i].ColumnName = "同比增幅计价收入";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZF_SS_INCOME")
                    {
                        dt.Columns[i].ColumnName = "同比增幅实际收入";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZF_Z_COSTS")
                    {
                        dt.Columns[i].ColumnName = "同比增幅总成本";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZF_Z_LIRUN")
                    {
                        dt.Columns[i].ColumnName = "同比增幅总利润";
                    }
                    else if (dt.Columns[i].ColumnName == "TBZF_J_LIRUN")
                    {
                        dt.Columns[i].ColumnName = "同比增幅净利润";
                    }

                }
                string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月";
                ex.ExportToLocal(dt, this.Page, "xls", "医院收入结构明细表(" + dates + ")");
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
            string benginDate = year + month + "01";
            return benginDate;
        }


        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        private string GetEndDate()
        {
            string year = ccbYearTo.SelectedItem.Value.ToString();
            int year1 = Convert.ToInt32(year);
            string month = ccbMonthTo.SelectedItem.Value.ToString();
            int month1 = Convert.ToInt32(month);

            if (month1 == 12)
            {
                month1 = 1;
                year1 = year1 + 1;
                year = year1.ToString();
                month = month1.ToString();
            }
            else
            {
                month1 = month1 + 1;
                year1 = year1;
                year = year1.ToString();
                month = month1.ToString();
            }

            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string endDate = year + month + "01";
            return endDate;
        }


    }
}
