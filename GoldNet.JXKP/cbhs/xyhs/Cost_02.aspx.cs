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

namespace GoldNet.JXKP
{
    public partial class Cost_02 :PageBase
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
                {
                    data(DateTime.Now.ToString("yyyy-MM") + "-01", DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01"); 
                }               
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
     
        private void data(string stdate, string enddate)
        {
            string deptcode = this.DeptFilter("");
            XyhsDetail report_dal = new XyhsDetail();
            //string wheres =  " and cbhs.xyhs_costs_dict.item_code not in ('J','K','L','M')";
            DataTable table = report_dal.GetCost02("0", stdate, enddate); //report_dal.GetCost02("0", stdate, enddate);
            
            Session.Remove("Cost_02");
            Session["Cost_02"] = table;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                int clleng=table.Columns[i].ColumnName.Length;
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
            DataTable costtype = report_dal.GetXyhsCostdictSFYL();
            ExtRows ers = new ExtRows();

            ExtRow er0 = new ExtRow();
            er0.Header = "";
            er0.ColSpan = 1;
            er0.Align = Alignment.Center;
            ers.Rows.Add(er0);
            for (int i = 0; i < costtype.Rows.Count; i++)
            {
                ExtRow er = new ExtRow();
                er.Header = costtype.Rows[i]["ITEM_NAME"].ToString();
                er.ColSpan = 3;
                er.Align = Alignment.Center;
                ers.Rows.Add(er);
            }
            ExtRow er1 = new ExtRow();
            er1.Header = "合计";
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
            if (Session["Cost_02"] == null)
                return;
            XyhsDetail report_dal = new XyhsDetail(); 
            //string wheres = " and cbhs.xyhs_costs_dict.item_code not in ('J','K','L','M')";
            DataTable costtype = report_dal.GetXyhsCostdictSFYL();//表头

            int length = costtype.Rows.Count;
            int all_length = length + 3 + (length + 1) * 3;//合并2列(length + 1) * 2
            TableCell[] header = new TableCell[all_length];

            header[0] = new TableHeaderCell();
            header[0].ColumnSpan = 1+(length+1)*3;
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
            MHeaderTabletoExcel((DataTable)Session["Cost_02"], header, "成本医02表"+DateTime.Now, null, 0);       
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
