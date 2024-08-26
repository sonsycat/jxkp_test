using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class DeptCostDetail : PageBase
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
                setprog(itemcode);
                data();
            }
        }

        /// <summary>
        /// 初始化分摊方案
        /// </summary>
        /// <param name="itemcode"></param>
        protected void setprog(string itemcode)
        {
            Cost_detail dal = new Cost_detail();
            DataTable progdt = dal.GetProgByCostItem(itemcode).Tables[0];
            for (int i = 0; i < progdt.Rows.Count; i++)
            {
                this.PROG_ITEM.Items.Add(new Goldnet.Ext.Web.ListItem(progdt.Rows[i]["PROG_NAME"].ToString(), progdt.Rows[i]["PROG_CODE"].ToString()));
            }
            this.PROG_ITEM.SelectedIndex = 0;
        }

        /// <summary>
        /// 列表刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, EventArgs e)
        {
            data();
        }

        /// <summary>
        /// 获取成本明细
        /// </summary>
        private void data()
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
            //
            Goldnet.Dal.Report report_dal = new Goldnet.Dal.Report();
            DataTable table = report_dal.DeptCostAccountCostDetail(itemcode, stardate, enddate, balances, deptcode, gettype);
            DataRow dr = table.NewRow();
            dr["DEPT_NAME"] = "合计";
            dr["COSTS"] = table.Compute("Sum(COSTS)", "");
            dr["COSTS_ARMYFREE"] = table.Compute("Sum(COSTS_ARMYFREE)", "");
            dr["TOTALCOST"] = table.Compute("Sum(TOTALCOST)", "");
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
                else if (table.Columns[i].ColumnName == "TOTALCOST")
                    cl.Header = "合计";

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBrush_Click(object sender, EventArgs e)
        {
            data();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Cost_Detail.hide();");
        }

        /// <summary>
        /// 自动分解处理
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
                Cost_detail dal = new Cost_detail();

                string deptcode = selectRow[0]["DEPT_CODE"];
                double costvalue = double.Parse(selectRow[0]["TOTALCOST"].ToString());
                string itemcode = Request["itemcode"].ToString();
                string stardate = Request["stardate"].ToString();
                string enddate = Request["enddate"].ToString();
                try
                {
                    dal.Exec_Sp_Extract_Cost_Input(this.PROG_ITEM.SelectedItem.Value, stardate.Substring(0, 6), itemcode, costvalue);

                    LoadConfig loadcfg = getLoadConfig("Decompose_Detail.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", itemcode));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", stardate));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", enddate));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));

                    showCenterSet(this.Decompose, loadcfg);
                }
                catch
                {
                    this.ShowMessage("提示", "分解出错！");
                }
            }
        }

        /// <summary>
        /// 手工分解
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDecomposesd_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow == null)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                Cost_detail dal = new Cost_detail();

                string deptcode = selectRow[0]["DEPT_CODE"];
                string costvalue = selectRow[0]["TOTALCOST"].ToString();
                string itemcode = Request["itemcode"].ToString();
                string stardate = Request["stardate"].ToString();
                string enddate = Request["enddate"].ToString();
                stardate = stardate.Substring(0, 4) + "-" + stardate.Substring(4, 2) + "-01";
                enddate = enddate.Substring(0, 4) + "-" + enddate.Substring(4, 2) + "-01";

                LoadConfig loadcfg = getLoadConfig("Manual_Decompose_deptset.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", itemcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", stardate));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", enddate));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("costvalue", costvalue));

                showCenterSet(this.deptset, loadcfg);
            }
        }

        /// <summary>
        /// 双击设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {

            //Dictionary<string, string>[] selectRow = GetSelectRow(e);
            //if (selectRow != null)
            //{
            //    if (selectRow[0]["GET_TYPE"].Equals("录入"))
            //    {
            //        this.ShowMessage("提示", "录入的数据没详细！");
            //    }
            //    else
            //    {
            //        string deptcode = selectRow[0]["DEPT_CODE"];
            //        string itemcode = Request["itemcode"].ToString();
            //        string stardate = Request["stardate"].ToString();
            //        string enddate = Request["enddate"].ToString();

            //        LoadConfig loadcfg = getLoadConfig("../Report/Cost_Detail.aspx");

            //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", itemcode));
            //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", stardate));
            //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", enddate));
            //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));

            //        showCenterSet(this.CostDetail, loadcfg);
            //    }
            //}
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
    }
}
