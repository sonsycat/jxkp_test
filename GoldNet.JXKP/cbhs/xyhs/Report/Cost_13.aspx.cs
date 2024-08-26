using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.cbhs.xyhs.Report
{
    public partial class Cost_13 : PageBase
    {
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

            XyhsDetail report_dal = new XyhsDetail();
            DataTable table = report_dal.GetCost13(GetBeginDate(), GetEndDate());


            if (table != null)
            {
                SReport.DataSource = table;
                SReport.DataBind();
                Session.Remove("Account_Subject_Income");
                Session["Account_Subject_Income"] = table;
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
            string month = ccbMonthTo.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string endDate = year + month + "01";
            return endDate;
        }


        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["Cost_02"] == null)
                return;
            XyhsDetail report_dal = new XyhsDetail();
            //string wheres = " and cbhs.xyhs_costs_dict.item_code not in ('J','K','L','M')";
            DataTable costtype = report_dal.GetXyhsCostdictSFYL();//表头

            int length = costtype.Rows.Count;
            int all_length = length + 3 + (length + 1) * 3;//合并2列(length + 1) * 2
            TableCell[] header = new TableCell[all_length];

            header[0] = new TableHeaderCell();
            header[0].ColumnSpan = 1 + (length + 1) * 3;
            header[0].RowSpan = 1;
            header[0].VerticalAlign = VerticalAlign.Middle;
            header[0].Text = cbbYear.SelectedItem.Value.ToString() + "年" + cbbmonth.SelectedItem.Value.ToString() + "月份至" + ccbYearTo.SelectedItem.Value.ToString() + "年" + ccbMonthTo.SelectedItem.Value.ToString() + "月份" + "医院临床服务类科室全成本表（医疗成本）</th></tr><tr>";


            header[1] = new TableHeaderCell();
            header[1].Text = "科室名称\\成本项目";
            header[1].ColumnSpan = 1;
            header[1].RowSpan = 2;

            for (int i = 0; i < length; i++)
            {
                header[i + 2] = new TableHeaderCell();
                header[i + 2].ColumnSpan = 3;//合并列
                header[i + 2].Text = costtype.Rows[i]["ITEM_NAME"].ToString();//表头字段
            }
            header[length + 2] = new TableHeaderCell();
            header[length + 2].ColumnSpan = 3;//合并列
            header[length + 2].Text = "小计</th></tr><tr>";//表头合计

            for (int i = 0; i < all_length - (length + 3); i++)
            {
                if (i % 3 == 1)
                {
                    header[length + 3 + i] = new TableHeaderCell();
                    header[length + 3 + i].Text = "直接成本";
                }
                else if (i % 3 == 0)
                {
                    header[length + 3 + i] = new TableHeaderCell();
                    header[length + 3 + i].Text = "间接成本";
                }
                else
                {
                    header[length + 3 + i] = new TableHeaderCell();
                    header[length + 3 + i].Text = "小计";
                }
            }
            MHeaderTabletoExcel((DataTable)Session["Cost_02"], header, "成本医02表" + DateTime.Now, null, 0);
        }







    }
}
