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

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class Cost_02_1 : PageBase
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
                ccbYearTo.Value = year;
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ccbMonthTo.Value = month;

                //costtype.Value = 1;
                // SetDict();
                if(!Page.IsPostBack)
                 data(DateTime.Now.ToString("yyyy-MM") + "-01", DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01");
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>

        private void data(string stdate, string enddate)
        {
            string deptcode = this.DeptFilter("");
            XyhsDetail report_dal = new XyhsDetail();
            //string wheres = " and cbhs.xyhs_costs_dict.item_code in ('J','K')";
            //string wheres2 = " and cbhs.xyhs_costs_dict.item_code in ('L','M')";
             
            DataTable table = report_dal.GetCost02_1("0", stdate, enddate); //report_dal.GetCost02("0", stdate, enddate);
            Session.Remove("Cost_02_1");
            Session["Cost_02_1"] = table;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                int clleng = table.Columns[i].ColumnName.Length;
                record = new RecordField(table.Columns[i].ColumnName.ToString(), RecordFieldType.String);
                this.SReport.AddField(record);
                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName.Substring(1, clleng - 1);
                if (cl.Header.Equals("科室名称"))
                {
                    cl.Align = Alignment.Left;
                }
                else
                {
                    cl.Align = Alignment.Right;
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
            DataTable costtype = report_dal.GetXyhsCostdictSFYL1();
            ExtRows ers = new ExtRows();

            

            ExtRow er0 = new ExtRow();
            er0.Header = "";
            er0.ColSpan = 1;
            er0.Align = Alignment.Center;
            ers.Rows.Add(er0);

            ExtRow er2 = new ExtRow();
            er2.Header = "医疗成本合计";
            er2.ColSpan = 3;
            er2.Align = Alignment.Center;
            ers.Rows.Add(er2);

            for (int i = 0; i < costtype.Rows.Count; i++)
            {
                ExtRow er = new ExtRow();
                er.Header = costtype.Rows[i]["ITEM_NAME"].ToString();
                er.ColSpan = 3;
                er.Align = Alignment.Center;
                ers.Rows.Add(er);
            }
            ExtRow er3 = new ExtRow();
            er3.Header = "医疗全成本合计";
            er3.ColSpan = 3;
            er3.Align = Alignment.Center;
            ers.Rows.Add(er3);

            costtype = report_dal.GetXyhsCostdictSFYL2();

            for (int i = 0; i < costtype.Rows.Count; i++)
            {
                ExtRow er = new ExtRow();
                er.Header = costtype.Rows[i]["ITEM_NAME"].ToString();
                er.ColSpan = 3;
                er.Align = Alignment.Center;
                ers.Rows.Add(er);
            }

            ExtRow er1 = new ExtRow();
            er1.Header = "医院全成本合计";
            er1.ColSpan = 3;
            er1.Align = Alignment.Center;
            ers.Rows.Add(er1);
            this.GridPanel_Show.ExtColumnModel.HeadRows.Add(ers);
            this.SReport.DataSource = table;
            this.SReport.DataBind();
        }
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), GetEndDate());
        }
        protected void OutExcel(object sender, EventArgs e)
        {

            if (Session["Cost_02_1"] == null)
                return;
            XyhsDetail report_dal = new XyhsDetail();
            int ii = 0;//循环次数
            DataTable d0 = report_dal.GetXyhsCostdictSFYL1();          
            DataTable d1 = report_dal.GetXyhsCostdictSFYL2();
            ii = 3 * 3;
            ii = ii + d0.Rows.Count * 3;
            ii = ii + d1.Rows.Count * 3;
            int cc = 0;

            TableCell[] header = new TableCell[30];

            header[cc] = new TableHeaderCell();
            header[cc].ColumnSpan = 22;
            header[cc].RowSpan = 1;
            header[cc].VerticalAlign = VerticalAlign.Middle;
            header[cc].Text = cbbYear.SelectedItem.Value.ToString() + "年" + cbbmonth.SelectedItem.Value.ToString() + "月份至" + ccbYearTo.SelectedItem.Value.ToString() + "年" + ccbMonthTo.SelectedItem.Value.ToString() + "月份" + "医院临床服务类科室全成本表（医疗全成本和医院全成本）</th></tr><tr>";
            cc++;
            header[cc] = new TableHeaderCell();
            header[cc].Text = "科室名称\\成本项目";
            header[cc].ColumnSpan = 1;
            header[cc].RowSpan = 2;
            cc++;
            header[cc] = new TableHeaderCell();
            header[cc].Text = "医疗成本合计";
            header[cc].ColumnSpan = 3;
            cc++;
            for (int i = 0; i < d0.Rows.Count; i++)
            {
                header[cc + i] = new TableHeaderCell();
                header[cc + i].Text = d0.Rows[i]["ITEM_NAME"].ToString(); ;
                header[cc + i].ColumnSpan = 3;
                cc++;
            }                       
            header[cc] = new TableHeaderCell();
            header[cc].Text = "医疗全成本合计";
            header[cc].ColumnSpan = 3;
            cc++;
            for (int i = 0; i < d1.Rows.Count; i++)
            {
                header[cc + i] = new TableHeaderCell();
                header[cc + i].Text = d1.Rows[i]["ITEM_NAME"].ToString(); ;
                header[cc + i].ColumnSpan = 3;
                cc++;
            }
            header[cc] = new TableHeaderCell();
            header[cc].Text = "医院全成本合计</th></tr><tr>";
            header[cc].ColumnSpan = 3;
            cc++;
            for (int i = 0; i < ii; i++)
            {
                if (i % 3 == 1)
                {
                    header[cc  + i] = new TableHeaderCell();
                    header[cc  + i].Text = "直接成本";
                }
                else if (i % 3 == 0)
                {
                    header[cc + i] = new TableHeaderCell();
                    header[cc + i].Text = "间接成本";
                }
                else
                {
                    header[cc  + i] = new TableHeaderCell();
                    header[cc  + i].Text = "合计";
                }
            }

            MHeaderTabletoExcel((DataTable)Session["Cost_02_1"], header, "医成本02_1表" + DateTime.Now, null, 0);
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
            string benginDate = year + "-" + month + "-01";
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
            string endDate = year + "-" + month + "-01";
            return endDate;
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
    }
}
