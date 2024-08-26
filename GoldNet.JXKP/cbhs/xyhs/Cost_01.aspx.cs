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
    public partial class Cost_01 :PageBase
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
                SetDict();
                if(!Page.IsPostBack)
                data(DateTime.Now.ToString("yyyy-MM") + "-01", DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01");
            }
        }
        /// <summary>
        /// 字典设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = dal.GetXYHSDeptType().Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.deptType.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["XYHS_DEPT_TYPE"].ToString(), table.Rows[i]["id"].ToString()));
                }
            }

        }
        private void data(string stdate, string enddate)
        {
            string deptcode = this.DeptFilter("");
            XyhsDetail report_dal = new XyhsDetail();
            //string wheres = " and cbhs.xyhs_costs_dict.item_code not in ('J','K','L','M')";

            DataTable table = report_dal.GetCost01(this.deptType.SelectedItem.Value.ToString(), stdate, enddate);
            table.Columns.Remove("DEPT_CODE");            
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(record);
                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName;
                if (cl.Header.Contains("费"))
                { 
                    cl.Renderer.Fn = "rmbMoney";
                }
                if (cl.Header.Contains("金"))
                {
                    cl.Renderer.Fn = "rmbMoney";
                }
                if (cl.Header.Contains("支出"))
                {
                    cl.Renderer.Fn = "rmbMoney";
                }
                if (cl.Header.Contains("合计"))
                {
                    cl.Renderer.Fn = "rmbMoney";
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
                if (cl.Header.Equals("DEPT_CODE"))
                {
                    cl.Hidden = true;
                }
                if (cl.Header.Equals("DEPT_CODE") || cl.Header.Equals("科室名称"))
                {
                    cl.Align = Alignment.Left;
                    cl.Width = Unit.Point(200);
                }
                else
                {
                    cl.Align = Alignment.Right;
                }
                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }
            Session.Remove("Cost_01");
            Session["Cost_01"] = table;
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), GetEndDate());
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["Cost_01"] == null)
                return;
            XyhsDetail report_dal = new XyhsDetail();
            //string wheres = " and cbhs.xyhs_costs_dict.item_code not in ('J','K','L','M')";
            DataTable costtype = report_dal.GetXyhsCostdictSFYL();//表头
            int length = costtype.Rows.Count;  
            TableCell[] header = new TableCell[length+3];
          
            header[0] = new TableHeaderCell();            
            header[0].ColumnSpan = length + 2;
            header[0].RowSpan = 1;
            header[0].VerticalAlign = VerticalAlign.Middle;
            header[0].Text = cbbYear.SelectedItem.Value.ToString() + "年" + cbbmonth.SelectedItem.Value.ToString() + "月份至" + ccbYearTo.SelectedItem.Value.ToString() + "年" + ccbMonthTo.SelectedItem.Value.ToString() + "月份" + "医院各科室直接成本表（医疗成本）</th></tr><tr>";

            header[1] = new TableHeaderCell();
            header[1].Text = "科室名称\\成本项目";
            header[1].ColumnSpan = 1;            

            header[2] = new TableHeaderCell();
            header[2].Text = "合计";
            header[2].ColumnSpan = 1;

            for (int i = 0; i < length; i++)
            {
                header[i + 3] = new TableHeaderCell(); 
                header[i + 3].Text = costtype.Rows[i]["ITEM_NAME"].ToString();//表头字段
            }
             
            MHeaderTabletoExcel((DataTable)Session["Cost_01"], header, "成本医01表"+DateTime.Now, null, 0);   
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
