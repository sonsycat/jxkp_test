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
using System.Text.RegularExpressions;
using Goldnet.Dal.cbhs;

namespace GoldNet.JXKP.cbhs.xyhs.Report
{
    public partial class TechnologyAnalyse : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                BoundComm boundcomm = new BoundComm();
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;

                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ccbmonthTo.Value = month;

                data(DateTime.Now.ToString("yyyyMM") + "01", DateTime.Now.AddMonths(1).ToString("yyyyMM") + "01", DateTime.Now.ToString("yyyyMM") + "01");
            }
        }

        private void data(string stdate, string enddate, string sdate)
        {
            XyhsReport report_dal = new XyhsReport();
            DataTable table = report_dal.GetTechnologyAnalyse(sdate, stdate, enddate);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                int clleng = table.Columns[i].ColumnName.Length;
                record = new RecordField(table.Columns[i].ColumnName.ToString(), RecordFieldType.String);
                this.SReport.AddField(record);
                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName.Substring(5, clleng - 5);
                if (cl.Header.Equals("科室"))
                {
                    cl.Align = Alignment.Left;
                    cl.Width = Unit.Point(260);
                }
                else
                {
                    cl.Align = Alignment.Right;
                    cl.Width = Unit.Point(100);
                }
                cl.Sortable = false;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName.ToString();

                TextField fils = new TextField();
                fils.ReadOnly = true;
                fils.ID = i.ToString();
                fils.SelectOnFocus = false;
                cl.Editor.Add(fils);
                this.GridPanel_Show.ExtColumnModel.Columns.Add(cl);
            }

            ExtRows ers2 = new ExtRows();

            ExtRow er21 = new ExtRow();
            er21.Header = "";
            er21.ColSpan = 1;
            er21.Align = Alignment.Center;
            ers2.Rows.Add(er21);

            ExtRow er22 = new ExtRow();
            er22.Header = "收入";
            er22.ColSpan = 2;
            er22.Align = Alignment.Center;
            ers2.Rows.Add(er22);

            ExtRow er23 = new ExtRow();
            er23.Header = "成本";
            er23.ColSpan = 2;
            er23.Align = Alignment.Center;
            ers2.Rows.Add(er23);

            ExtRow er24 = new ExtRow();
            er24.Header = "收益";
            er24.ColSpan = 2;
            er24.Align = Alignment.Center;
            ers2.Rows.Add(er24);

            this.GridPanel_Show.ExtColumnModel.HeadRows.Add(ers2);


            this.SReport.DataSource = table;
            this.SReport.DataBind();
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), GetEndDate(), GetSDate());
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            TableCell[] header = new TableCell[10];


            header[0] = new TableHeaderCell();
            header[0].Text = "科室";
            header[0].RowSpan = 2;

            header[1] = new TableHeaderCell();
            header[1].ColumnSpan = 2;
            header[1].Text = "收入";

            header[2] = new TableHeaderCell();
            header[2].ColumnSpan = 2;
            header[2].Text = "成本";

            header[3] = new TableHeaderCell();
            header[3].ColumnSpan = 2;
            header[3].Text = "收益</th></tr><tr>";

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    header[3 + i + j + 1] = new TableHeaderCell();
                    if (j == 0)
                        header[3 + i + j + 1].Text = "本期";
                    if (j == 1)
                        header[3 + i + j + 1].Text = "累计";
                    if (i == 4 && j == 1)
                        header[3 + i + j + 1].Text = "累计</th>";
                }
            }

            XyhsReport report_dal = new XyhsReport();

            DataTable dt = report_dal.GetTechnologyAnalyse(GetSDate(), GetBeginDate(), GetEndDate());

            Dictionary<int, int> mergeCellNums = new Dictionary<int, int>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                mergeCellNums.Add(i, 2);
            }
            MHeaderTabletoExcel(dt, header, "医技科室收入成本收益明细表", mergeCellNums, 0); 
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
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        private string GetEndDate()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = ccbmonthTo.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string endDate = year + "" + month + "01";
            return endDate;
        }

        private string GetSDate()
        {
            string year = cbbYear.SelectedItem.Value.ToString();

            return year + "0101";
        }
    }
}
