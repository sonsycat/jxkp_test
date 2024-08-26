using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class DeptCostAccount_detail : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string itemcode = Request["itemcode"].ToString();
                string stardate = Request["stardate"].ToString();
                string enddate = Request["enddate"].ToString();
                string deptcode = Request["deptcode"].ToString();
                string balances = Request["balances"].ToString();
                string gettype = "";
                if (Request["gettype"] != null)
                {
                    gettype = Request["gettype"].ToString();
                }

                data(itemcode, stardate, enddate, deptcode, balances, gettype);
            }
        }

        /// <summary>
        /// 获取数据并绑定
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        /// <param name="deptcode"></param>
        /// <param name="balances"></param>
        /// <param name="gettype"></param>
        private void data(string itemcode, string stardate, string enddate, string deptcode, string balances, string gettype)
        {
            Report report_dal = new Report();
            DataTable table = report_dal.DeptCost_CostDetail(itemcode, stardate, enddate, balances, deptcode, gettype);
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
                {
                    cl.Header = "对外成本";
                    cl.Align = Alignment.Right;
                    cl.Renderer.Fn = "rmbMoney";
                }
                else if (table.Columns[i].ColumnName == "COSTS_ARMYFREE")
                {
                    cl.Header = "减免成本";
                    cl.Align = Alignment.Right;
                    cl.Renderer.Fn = "rmbMoney";
                }
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
                //NumberField fils = new NumberField();
                //fils.ID = i.ToString();
                //fils.SelectOnFocus = true;
                //fils.DecimalPrecision = 2;
                //cl.Editor.Add(fils);


                this.GridPanel2.ColumnModel.Columns.Add(cl);
            }
            this.Store2.DataSource = table;
            this.Store2.DataBind();

            Session.Remove("HospitalCostBenefit2");
            Session["HospitalCostBenefit2"] = table;
        }

        /// <summary>
        /// 返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Cost_Detail.hide();");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDecompose_Click(object sender, AjaxEventArgs e)
        {

            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow == null)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {

                string deptcode = selectRow[0]["DEPT_CODE"];
                string itemcode = Request["itemcode"].ToString();
                string stardate = Request["stardate"].ToString();
                string enddate = Request["enddate"].ToString();

                LoadConfig loadcfg = getLoadConfig("Cost_Detail.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", itemcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", stardate));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", enddate));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));

                showCenterSet(this.CostDetail, loadcfg);

            }
        }

        /// <summary>
        /// 双击设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {

            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                if (!selectRow[0]["GET_TYPE"].Equals("提取"))
                {
                    this.ShowMessage("提示", "提取的数据才有详细！");
                }
                else
                {
                    string deptcode = selectRow[0]["DEPT_CODE"];
                    string itemcode = Request["itemcode"].ToString();
                    string stardate = Request["stardate"].ToString();
                    string enddate = Request["enddate"].ToString();

                    LoadConfig loadcfg = getLoadConfig("Cost_Detail.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", itemcode));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", stardate));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", enddate));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));

                    showCenterSet(this.CostDetail, loadcfg);
                }
            }
        }

        /// <summary>
        /// 反序列化得到客户端提交的gridpanel数据行  
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
    
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["HospitalCostBenefit2"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["HospitalCostBenefit2"];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "FACACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入";
                    }
                    else if (dt.Columns[i].ColumnName == "COSTS")
                    {
                        dt.Columns[i].ColumnName = "实际成本";
                    }
                    else if (dt.Columns[i].ColumnName == "PROFIT")
                    {
                        dt.Columns[i].ColumnName = "实际收益";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMYCOSTS")
                    {
                        dt.Columns[i].ColumnName = "计价成本";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_CODE")
                    {
                        dt.Columns[i].ColumnName = "科室代码";
                    }
                    else if (dt.Columns[i].ColumnName == "MEDINCOMES")
                    {
                        dt.Columns[i].ColumnName = "药品收益";
                    }
                }

                ex.ExportToLocal(dt, this.Page, "xls", "医疗收支,药品效益汇总表");
            }
        }
    }
}
