using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;

namespace GoldNet.JXKP
{
    public partial class Cost_Detail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string itemcode = Request["itemcode"].ToString();
                string stardate = Request["stardate"].ToString();
                string enddate = Request["enddate"].ToString();
                string deptcode = Request["deptcode"].ToString();
                
               
                data(itemcode, stardate, enddate, deptcode);
            }
        }
        private void data(string itemcode, string stardate, string enddate, string deptcode)
        {
            Report report_dal = new Report();
            DataTable table = report_dal.CostDetail(itemcode, stardate, enddate, deptcode);
            DataRow dr = table.NewRow();
            dr["DEPT_NAME"] = "合计";
            dr["COSTS"] = table.Compute("Sum(COSTS)", "");
           
            table.Rows.Add(dr);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store2.AddField(record);
                Column cl = new Column();
                if (table.Columns[i].ColumnName == "DEPT_NAME")
                    cl.Header = "科室名称";
                else if (table.Columns[i].ColumnName == "DEPT_CODE")
                {
                    cl.Header = "科室编码";
                    cl.Hidden = true;
                }
                else if (table.Columns[i].ColumnName == "ITEM_CODE")
                {
                    cl.Header = "项目编码";
                    cl.Hidden = true;
                }
                else if (table.Columns[i].ColumnName == "COSTS")
                {
                    cl.Header = "成本";
                    cl.Align = Alignment.Right;
                }

                else if (table.Columns[i].ColumnName == "ST_DATE")
                    cl.Header = "时间";
                else if (table.Columns[i].ColumnName == "MTRL_CODE")
                    cl.Header = "物资编码";
                else if (table.Columns[i].ColumnName == "MTRL_NAME")
                    cl.Header = "物资名称";
                else if (table.Columns[i].ColumnName == "MTRL_SPEC")
                    cl.Header = "型号";
                else if (table.Columns[i].ColumnName == "UNITS")
                    cl.Header = "单位";
                else if (table.Columns[i].ColumnName == "DEPTER")
                    cl.Header = "领取人";
                cl.Sortable = false;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;
      


                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }
            this.Store2.DataSource = table;
            this.Store2.DataBind();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.CostDetail.hide();");
        }
    
    }
}
