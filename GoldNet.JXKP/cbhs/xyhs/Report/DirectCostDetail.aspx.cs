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
    public partial class DirectCostDetail : PageBase
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
        private void data(string stdate, string enddate,string sdate)
        {
            XyhsReport report_dal = new XyhsReport();
            DataTable table = report_dal.GetDirectCostDetail(sdate, stdate, enddate);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                int clleng = table.Columns[i].ColumnName.Length;
                record = new RecordField(table.Columns[i].ColumnName.ToString(), RecordFieldType.String);
                this.SReport.AddField(record);
                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName.Substring(7, clleng - 7);
                if (cl.Header.Equals("科室"))
                {
                    cl.Align = Alignment.Left;
                    cl.Width = Unit.Point(200);
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


            


            ExtRows ers0 = new ExtRows();

            ExtRow er1 = new ExtRow();
            er1.Header = "";
            er1.ColSpan = 1;
            er1.Align = Alignment.Center;
            ers0.Rows.Add(er1);

            ExtRow er2 = new ExtRow();
            er2.Header = "合计";
            er2.ColSpan = 6;
            er2.Align = Alignment.Center;
            ers0.Rows.Add(er2);

            ExtRow er3 = new ExtRow();
            er3.Header = "药品";
            er3.ColSpan = 6;
            er3.Align = Alignment.Center;
            ers0.Rows.Add(er3);

            ExtRow er4 = new ExtRow();
            er4.Header = "非药品";
            er4.ColSpan = 6;
            er4.Align = Alignment.Center;
            ers0.Rows.Add(er4);

            this.GridPanel_Show.ExtColumnModel.HeadRows.Add(ers0);



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


            ExtRow er25 = new ExtRow();
            er25.Header = "收入";
            er25.ColSpan = 2;
            er25.Align = Alignment.Center;
            ers2.Rows.Add(er25);

            ExtRow er26 = new ExtRow();
            er26.Header = "成本";
            er26.ColSpan = 2;
            er26.Align = Alignment.Center;
            ers2.Rows.Add(er26);

            ExtRow er27 = new ExtRow();
            er27.Header = "收益";
            er27.ColSpan = 2;
            er27.Align = Alignment.Center;
            ers2.Rows.Add(er27);


            ExtRow er28 = new ExtRow();
            er28.Header = "收入";
            er28.ColSpan = 2;
            er28.Align = Alignment.Center;
            ers2.Rows.Add(er28);

            ExtRow er29 = new ExtRow();
            er29.Header = "成本";
            er29.ColSpan = 2;
            er29.Align = Alignment.Center;
            ers2.Rows.Add(er29);

            ExtRow er210 = new ExtRow();
            er210.Header = "收益";
            er210.ColSpan = 2;
            er210.Align = Alignment.Center;
            ers2.Rows.Add(er210);

            this.GridPanel_Show.ExtColumnModel.HeadRows.Add(ers2);


            this.SReport.DataSource = table;
            this.SReport.DataBind();
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), GetEndDate(),GetSDate());
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            TableCell[] header = new TableCell[31];


            header[0] = new TableHeaderCell();
            header[0].Text = "科室";
            header[0].RowSpan = 3;

            header[1] = new TableHeaderCell();
            header[1].ColumnSpan = 6;
            header[1].Text = "合计";

            header[2] = new TableHeaderCell();
            header[2].ColumnSpan = 6;
            header[2].Text = "药品";

            header[3] = new TableHeaderCell();
            header[3].ColumnSpan = 6;
            header[3].Text = "非药品</th></tr><tr>";



            header[4] = new TableHeaderCell();
            header[4].ColumnSpan = 2;
            header[4].Text = "收入";

            header[5] = new TableHeaderCell();
            header[5].ColumnSpan = 2;
            header[5].Text = "成本";

            header[6] = new TableHeaderCell();
            header[6].ColumnSpan = 2;
            header[6].Text = "收益";


            header[7] = new TableHeaderCell();
            header[7].ColumnSpan = 2;
            header[7].Text = "收入";

            header[8] = new TableHeaderCell();
            header[8].ColumnSpan = 2;
            header[8].Text = "成本";

            header[9] = new TableHeaderCell();
            header[9].ColumnSpan = 2;
            header[9].Text = "收益";


            header[10] = new TableHeaderCell();
            header[10].ColumnSpan = 2;
            header[10].Text = "收入";

            header[11] = new TableHeaderCell();
            header[11].ColumnSpan = 2;
            header[11].Text = "成本";

            header[12] = new TableHeaderCell();
            header[12].ColumnSpan = 2;
            header[12].Text = "收益</th></tr><tr>";

            for (int i = 0; i < 17; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    header[12 + i  + j + 1] = new TableHeaderCell();
                    if (j == 0)
                        header[12 + i  + j + 1].Text = "本期";
                    if (j == 1)
                        header[12 + i  + j + 1].Text = "累计";
                    if (i == 16 && j == 1)
                        header[i].Text = "累计</th>";
                }
            }

            XyhsReport report_dal = new XyhsReport();

            DataTable dt = report_dal.GetDirectCostDetail( GetSDate(),GetBeginDate(), GetEndDate());

            Dictionary<int, int> mergeCellNums = new Dictionary<int, int>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                mergeCellNums.Add(i, 2);
            }
            MHeaderTabletoExcel(dt, header, "直接医疗科室收入成本收益明细表", mergeCellNums, 0); 




            
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
