using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;


namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class Hospital_Decompose_Detail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                data();
            }
        }
        protected void Store_RefreshData(object sender, EventArgs e)
        {
            data();
        }
        private void data()
        {
            string stardate = Request["stardate"].ToString();
            string enddate = Request["enddate"].ToString();
            string gettype = Request["gettype"].ToString(); 
            //
            Goldnet.Dal.Report report_dal = new Goldnet.Dal.Report();
            DataTable table = report_dal.DeptHospitalCostDetail(stardate, enddate,gettype);
            DataRow dr = table.NewRow();
            dr["DEPT_NAME"] = "合计";
            dr["COSTS"] = table.Compute("Sum(COSTS)", "");
            dr["COSTS_ARMYFREE"] = table.Compute("Sum(COSTS_ARMYFREE)", "");
            table.Rows.Add(dr);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store2.AddField(record);
                Column cl = new Column();
                if (table.Columns[i].ColumnName == "DEPT_NAME")
                    cl.Header = "科室名称";
                else if (table.Columns[i].ColumnName == "ITEM_NAME")
                    cl.Header = "项目名称";
                else if (table.Columns[i].ColumnName == "COSTS")
                    cl.Header = "对外成本";
                else if (table.Columns[i].ColumnName == "COSTS_ARMYFREE")
                    cl.Header = "减免成本";
                else if (table.Columns[i].ColumnName == "ACCOUNTING_DATE")
                    cl.Header = "时间";
                else if (table.Columns[i].ColumnName == "GET_TYPE")
                    cl.Header = "方式";
                else if (table.Columns[i].ColumnName == "COST_FLAG")
                    cl.Header = "类别";
                else if (table.Columns[i].ColumnName == "MEMO")
                    cl.Header = "备注";
                cl.Sortable = false;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;
                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }
            this.Store2.DataSource = table;
            this.Store2.DataBind();
        }
        protected void btnBrush_Click(object sender, EventArgs e)
        {
            data();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Hospital_Detail.hide();");
        }
     
    }
}
