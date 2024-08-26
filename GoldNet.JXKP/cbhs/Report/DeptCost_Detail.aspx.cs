using System;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP
{
    public partial class DeptCost_Detail : PageBase
    {
        private Report report_dal = new Report();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                SetStoreProxy();
                cbbType.SelectedItem.Value = "0";

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                //if (hospro == "1")
                //{
                //    this.gpCost.ColumnModel.Columns[3].Hidden = true;
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetStoreProxy()
        {
            //查找科室信息
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            //pro.Url = "../WebService/BonusDepts.ashx";
            pro.Url = "../WebService/BonusDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
            this.SDept.Proxy.Add(pro);
            JsonReader jr = new JsonReader();
            jr.ReaderID = "DEPT_CODE";
            jr.Root = "Bonusdepts";
            jr.TotalProperty = "totalCount";
            RecordField rf = new RecordField();
            rf.Name = "DEPT_CODE";
            jr.Fields.Add(rf);
            RecordField rfn = new RecordField();
            rfn.Name = "DEPT_NAME";
            jr.Fields.Add(rfn);
            this.SDept.Reader.Add(jr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            //this.gpCost.Reconfigure();
            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string dept = "";
            if (deptcode != "" && cbbdept.SelectedItem.Value == "")
            {
                dept = " and a.dept_code in (" + deptcode + ") ";
            }
            else
            {
                if (cbbdept.SelectedItem.Value != "")
                {
                    dept = " and a.dept_code in (select DEPT_CODE from comm.sys_dept_dict b where b.ACCOUNT_DEPT_CODE='" + cbbdept.SelectedItem.Value + "')  ";
                }
            }
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            string type = this.ComboBox1.SelectedItem.Value;

            DataTable dt1 = report_dal.DeptCostAccountCostByType(GetBeginDate(), GetEndDate(), balance, dept, hospro,type);

            DataTable dt2 = report_dal.DeptCostAccountCost(GetBeginDate(), GetEndDate(), balance, dept);

            //DataRow dr = dt1.NewRow();
            //dr["ITEM_NAME"] = "合计";
            //dr["COSTS"] = dt1.Compute("Sum(COSTS)", "");
            //dr["COSTS_ARMYFREE"] = dt1.Compute("Sum(COSTS_ARMYFREE)", "");
            //dr["TOTALCOST"] = dt1.Compute("Sum(TOTALCOST)", "");
            //dt1.Rows.Add(dr);
            //if (dt1.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt1.Columns.Count; i++)
            //    {
            //        Column cl = new Column();
            //        string headers = "";
            //        if (dt1.Columns[i].ColumnName.ToUpper().Equals("COST_TYPE_NAME")) headers = "类别";
            //        else if (dt1.Columns[i].ColumnName.ToUpper().Equals("ITEM_NAME")) headers = "项目名称";
            //        //else if (dt1.Columns[i].ColumnName.ToUpper().Equals("ITEM_CODE")) headers = "项目代码";
            //        else if (dt1.Columns[i].ColumnName.ToUpper().Equals("COSTS")) 
            //        { 
            //            headers = "对外成本"; 
            //            cl.Align = Alignment.Right;
            //            cl.Renderer.Fn = "rmbMoney";
            //        }
            //        else if (dt1.Columns[i].ColumnName.ToUpper().Equals("COSTS_ARMYFREE")) 
            //        {   
            //            headers = "减免成本"; 
            //            cl.Align = Alignment.Right;
            //            cl.Renderer.Fn = "rmbMoney";
            //        }
            //        else if (dt1.Columns[i].ColumnName.ToUpper().Equals("TOTALCOST")) 
            //        { 
            //            headers = "支出合计"; 
            //            cl.Align = Alignment.Right;
            //            cl.Renderer.Fn = "rmbMoney";
            //        }
            //        //else if (dt1.Columns[i].ColumnName.ToUpper().Equals("RATE")) headers = "占支出比";
            //        else continue;
            //        cl.Header = headers;
            //        cl.Sortable = false;
            //        cl.MenuDisabled = true;
            //        cl.ColumnID = dt1.Columns[i].ColumnName.ToUpper();
            //        cl.DataIndex = dt1.Columns[i].ColumnName.ToUpper();
            //        this.gpCost.AddColumn(cl);
            //    }
            //}
            //else
            //{
            //    this.gpCost.Height = Unit.Empty;
            //}
            if (dt1 != null)
            {

                SReportCost.DataSource = dt1;
                SReportCost.DataBind();
                Session.Remove("DeptCostAccountCost");
                Session["DeptCostAccountCost"] = dt1;

                //if (dt2.Rows.Count > 0 && dt2.Rows[0]["NET_INCOME"].ToString() != "")
                //{
                //    ScriptManager1.AddScript("totalMoney(" + dt2.Rows[0]["NET_INCOME"].ToString() + "," + dt2.Rows[0]["ARMY_INCOME"].ToString() + "," + dt2.Rows[0]["GROSS_INCOME"].ToString() + ");");
                //}
                //else
                //{
                //    ScriptManager1.AddScript("totalMoney(0,0,0);");
                //}
                //ScriptManager1.DataBind();
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "未找到数据",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["DeptCostAccountCost"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["DeptCostAccountCost"];


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (dt.Columns[i].ColumnName == "ITEM_CODE")
                    //{
                    //    dt.Columns[i].ColumnName = "成本科目代码";


                    //}
                    if (dt.Columns[i].ColumnName == "COST_TYPE_NAME")
                    {
                        dt.Columns[i].ColumnName = "成本类别";

                    }
                    else if (dt.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        dt.Columns[i].ColumnName = "成本科目名称";

                    }
                    else if (dt.Columns[i].ColumnName == "COSTS")
                    {
                        dt.Columns[i].ColumnName = "对外成本";

                    }
                    else if (dt.Columns[i].ColumnName == "COSTS_ARMYFREE")
                    {
                        dt.Columns[i].ColumnName = "减免成本";

                    }
                    else if (dt.Columns[i].ColumnName == "TOTALCOSTS")
                    {
                        dt.Columns[i].ColumnName = "成本合计";

                    }

                }

                //ex.ExportToLocal(dt, this.Page, "xls", "单位成本核算信息");

                string year = cbbYear.SelectedItem.Value.ToString();
                string month = cbbmonth.SelectedItem.Value.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                string s_date = year + "-" + month;

                string yearTo = ccbYearTo.SelectedItem.Value.ToString();
                string monthTo = ccbMonthTo.SelectedItem.Value.ToString();
                if (monthTo.Length == 1)
                {
                    monthTo = "0" + monthTo;
                }
                string e_date = yearTo + "-" + monthTo;

                // 创建工作簿
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("单位成本核算信息");

                // 创建表头
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

                // 创建样式
                ICellStyle numberStyle = workbook.CreateCellStyle();
                numberStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");

                ICellStyle textStyle = workbook.CreateCellStyle();
                textStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");

                ICellStyle currencyStyle = workbook.CreateCellStyle();
                currencyStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00"); // 金钱格式

                // 填充数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        string cellValue = dt.Rows[i][j].ToString();
                        double number;

                        if (double.TryParse(cellValue, out number))
                        {
                            cell.SetCellValue(number);

                            // 如果是金钱列，设置为金钱格式；否则设置为数值格式
                            //if (j == /* 你的金钱列索引，例如第5列 */)
                            //{
                            //    cell.CellStyle = numberStyle;
                            //}
                            //else
                            //{
                            //    cell.CellStyle = currencyStyle;
                            //}

                            cell.CellStyle = currencyStyle; // 数值格式
                        }
                        else
                        {
                            cell.SetCellValue(cellValue);
                            cell.CellStyle = textStyle; // 文本格式
                        }
                    }
                }

                // 自动调整列宽
                int maxColumnWidth = 30 * 256; // 最大宽度设置为30字符宽
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                    int currentWidth = sheet.GetColumnWidth(i);
                    if (currentWidth > maxColumnWidth)
                    {
                        sheet.SetColumnWidth(i, maxColumnWidth);
                    }
                }

                // 导出为Excel文件
                using (MemoryStream exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    Response.Clear();
                    if (s_date.Equals(e_date))
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "单位成本核算信息.xls");
                    }
                    else
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "至" + e_date + "单位成本核算信息.xls");
                    }
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
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
            string benginDate = year + month + "01";
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
            string endDate = year + month + "01";
            return endDate;
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
                if (selectRow[0]["ITEM_CODE"] != null)
                {
                    string itemcode = selectRow[0]["ITEM_CODE"];
                    string deptcode = this.cbbdept.SelectedItem.Value.ToString() != "" ? "'" + this.cbbdept.SelectedItem.Value.ToString() + "'" : this.DeptFilter("");

                    LoadConfig loadcfg = getLoadConfig("DeptCostAccount_detail.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", itemcode));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", GetBeginDate()));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", GetEndDate()));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("balances", this.cbbType.SelectedItem.Value));
                    showCenterSet(this.Cost_Detail, loadcfg);
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

    }
}
