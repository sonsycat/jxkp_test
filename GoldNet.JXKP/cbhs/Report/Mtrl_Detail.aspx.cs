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
    public partial class Mtrl_Detail :PageBase
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
               
                data(DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01", DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01");
            }
        }
        
        private void data(string stdate, string enddate)
        {
            Report report_dal = new Report();
            DataTable table = report_dal.GetMtrlDetail(stdate, enddate,"1");
            DataRow dr = table.NewRow();
            dr["MTRL_NAME"] = "合计";
            dr["CHAJIA"] = table.Compute("Sum(CHAJIA)","");
            table.Rows.Add(dr);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(record);
                Column cl = new Column();
                if (table.Columns[i].ColumnName == "MTRL_NAME")
                {
                    cl.Header = "物资名称";
                }
                else if (table.Columns[i].ColumnName == "MTRL_CODE")
                {
                    cl.Header = "物资代码";
                }
                else if (table.Columns[i].ColumnName == "MTRL_SPEC")
                {
                    cl.Header = "物资规格";
                }
                else if (table.Columns[i].ColumnName == "UNITS")
                {
                    cl.Header = "单位";
                }
                else if (table.Columns[i].ColumnName == "QUANTITY")
                {
                    cl.Header = "数量";
                }
                else if (table.Columns[i].ColumnName == "PURCHASE_PRICE")
                {
                    cl.Header = "进货价";
                }
                else if (table.Columns[i].ColumnName == "RETAIL_PRICE")
                {
                    cl.Header = "招标价";
                }
                else if (table.Columns[i].ColumnName == "ACCOUNT_DATE")
                {
                    cl.Header = "记账时间";
                }
                else if (table.Columns[i].ColumnName == "CHAJIA")
                {
                    cl.Header = "差价";
                }
                cl.Sortable = false;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;
                TextField fils = new TextField();
                fils.ReadOnly = true;

                fils.ID = i.ToString();
                fils.SelectOnFocus = false;
                //fils.DecimalPrecision = 2;
                cl.Editor.Add(fils);
                

                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }

            this.Store1.DataSource = table;
            this.Store1.DataBind();


        }
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), GetEndDate());
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            Report report_dal = new Report();
            DataTable dt = report_dal.GetMtrlDetail(GetBeginDate(), GetEndDate(),"1");
            DataRow dr = dt.NewRow();
            dr["MTRL_NAME"] = "合计";
            dr["CHAJIA"] = dt.Compute("Sum(CHAJIA)", "");
            dt.Rows.Add(dr);
            ExportData ex = new ExportData();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "MTRL_NAME")
                {
                    dt.Columns[i].ColumnName = "物资名称";
                }
                else if (dt.Columns[i].ColumnName == "MTRL_CODE")
                {
                    dt.Columns[i].ColumnName = "物资代码";
                }
                else if (dt.Columns[i].ColumnName == "MTRL_SPEC")
                {
                    dt.Columns[i].ColumnName = "物资规格";
                }
                else if (dt.Columns[i].ColumnName == "UNITS")
                {
                    dt.Columns[i].ColumnName = "单位";
                }
                else if (dt.Columns[i].ColumnName == "QUANTITY")
                {
                    dt.Columns[i].ColumnName = "数量";
                }
                else if (dt.Columns[i].ColumnName == "PURCHASE_PRICE")
                {
                    dt.Columns[i].ColumnName = "进货价";
                }
                else if (dt.Columns[i].ColumnName == "RETAIL_PRICE")
                {
                    dt.Columns[i].ColumnName = "招标价";
                }
                else if (dt.Columns[i].ColumnName == "ACCOUNT_DATE")
                {
                    dt.Columns[i].ColumnName = "记账时间";
                }
                else if (dt.Columns[i].ColumnName == "CHAJIA")
                {
                    dt.Columns[i].ColumnName = "差价";
                }
            }
            this.outexcel(dt, "器械明细表");

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
       
      
    }
}
