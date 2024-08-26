using System;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP
{
    public partial class DeptCostAccount : PageBase
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
                //cbbType.SelectedItem.Text = "收付实现";

                //string deptcode = this.DeptFilter("");
                //if (deptcode != "")
                //{
                //    User user = (User)Session["CURRENTSTAFF"];
                //    cbbdept.SelectedItem.Value = user.HisDeptCode;
                //    cbbdept.SelectedItem.Text = user.HisDeptName;
                //    cbbdept.Disabled = true;
                //}
                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    this.gpIncome.ColumnModel.Columns[3].Hidden = true;
                    this.gpCost.ColumnModel.Columns[3].Hidden = true;
                }
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
            this.gpIncome.Reconfigure();
            this.gpCost.Reconfigure();
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
            DataTable dt = report_dal.DeptCostAccountIncomen(GetBeginDate(), GetEndDate(), balance, dept, hospro);
            DataTable dt1 = report_dal.DeptCostAccountCost(GetBeginDate(), GetEndDate(), balance, dept, hospro);

            //DataTable dt2 = report_dal.DeptCostAccountCost(GetBeginDate(), GetEndDate(), balance, dept);

            DataRow dr = dt.NewRow();
            dr["INCOM_TYPE_NAME"] = "合计";
            dr["INCOMES_CHARGES"] = dt.Compute("Sum(INCOMES_CHARGES)", "");
            dr["CHARGES"] = dt.Compute("Sum(CHARGES)", "");
            dr["INCOMES"] = dt.Compute("Sum(INCOMES)", "");

            Session.Remove("INCOMES_CHARGES");
            Session.Remove("CHARGES");
            Session.Remove("INCOMES");

            if (dt.Rows.Count > 0)
            {
                Session["INCOMES_CHARGES"] = Convert.ToDouble(dt.Compute("Sum(INCOMES_CHARGES)", ""));
                Session["CHARGES"] = Convert.ToDouble(dt.Compute("Sum(CHARGES)", ""));
                Session["INCOMES"] = Convert.ToDouble(dt.Compute("Sum(INCOMES)", ""));
            }
            dt.Rows.Add(dr);

            //DataRow dr1 = dt1.NewRow();
            //dr1["COST_TYPE_NAME"] = "合计";
            //dr1["COSTS"] = dt1.Compute("Sum(COSTS)", "");
            //dr1["COSTS_ARMYFREE"] = dt1.Compute("Sum(COSTS_ARMYFREE)", "");
            //dr1["TOTALCOST"] = dt1.Compute("Sum(TOTALCOST)", "");
            //dt1.Rows.Add(dr1);

            if (dt.Rows.Count > 0)
            {
                this.gpIncome.Height = 200;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Column cl = new Column();
                    string headers = "";
                    if (dt.Columns[i].ColumnName.ToUpper().Equals("INCOM_TYPE_NAME")) headers = "类别";
                    else if (dt.Columns[i].ColumnName.ToUpper().Equals("ITEM_NAME")) headers = "项目名称";
                    else if (dt.Columns[i].ColumnName.ToUpper().Equals("INCOMES_CHARGES")) 
                    { 
                        headers = "实际收入"; 
                        cl.Align = Alignment.Right;
                        cl.Renderer.Fn = "rmbMoney";
                    }
                    else if (dt.Columns[i].ColumnName.ToUpper().Equals("CHARGES")) 
                    { 
                        headers = "计价收入"; 
                        cl.Align = Alignment.Right;
                        cl.Renderer.Fn = "rmbMoney";
                    }
                    else if (dt.Columns[i].ColumnName.ToUpper().Equals("INCOMES")) 
                    { 
                        headers = "收入合计"; 
                        cl.Align = Alignment.Right;
                        cl.Renderer.Fn = "rmbMoney";
                    }
                    else if (dt.Columns[i].ColumnName.ToUpper().Equals("RATE")) 
                    { 
                        headers = "占收入比"; 
                        cl.Align = Alignment.Right;
                        cl.Renderer.Fn = "rmbMoney";
                    }
                    //else if (dt.Columns[i].ColumnName.ToUpper().Equals("ARMCOSTS")) { headers = "折算后军免收入"; cl.Align = Alignment.Right; }
                    else continue;
                    cl.Header = headers;
                    cl.Sortable = false;
                    cl.MenuDisabled = true;
                    cl.ColumnID = dt.Columns[i].ColumnName.ToUpper();
                    cl.DataIndex = dt.Columns[i].ColumnName.ToUpper();
                    this.gpIncome.AddColumn(cl);
                }
            }
            else
            {
                this.gpIncome.Height = Unit.Empty;
            }
            if (dt1.Rows.Count > 0)
            {
                this.gpCost.Height = 200;
                for (int i = 0; i < dt1.Columns.Count; i++)
                {
                    Column cl = new Column();
                    string headers = "";
                    if (dt1.Columns[i].ColumnName.ToUpper().Equals("COST_TYPE_NAME")) headers = "类别";
                    else if (dt1.Columns[i].ColumnName.ToUpper().Equals("ITEM_NAME")) headers = "项目名称";
                    //else if (dt1.Columns[i].ColumnName.ToUpper().Equals("ITEM_CODE")) headers = "项目代码";
                    else if (dt1.Columns[i].ColumnName.ToUpper().Equals("COSTS")) 
                    { 
                        headers = "对外成本"; 
                        cl.Align = Alignment.Right;
                        cl.Renderer.Fn = "rmbMoney";
                    }
                    else if (dt1.Columns[i].ColumnName.ToUpper().Equals("COSTS_ARMYFREE")) 
                    { 
                        headers = "减免成本"; 
                        cl.Align = Alignment.Right;
                        cl.Renderer.Fn = "rmbMoney";
                    }
                    else if (dt1.Columns[i].ColumnName.ToUpper().Equals("TOTALCOST")) 
                    { 
                        headers = "支出合计"; 
                        cl.Align = Alignment.Right;
                        cl.Renderer.Fn = "rmbMoney";
                    }
                    else if (dt1.Columns[i].ColumnName.ToUpper().Equals("RATE")) 
                    { 
                        headers = "占支出比"; 
                        cl.Align = Alignment.Right;
                        cl.Renderer.Fn = "rmbMoney";
                    }
                    else continue;
                    cl.Header = headers;
                    cl.Sortable = false;
                    cl.MenuDisabled = true;
                    cl.ColumnID = dt1.Columns[i].ColumnName.ToUpper();
                    cl.DataIndex = dt1.Columns[i].ColumnName.ToUpper();
                    this.gpCost.AddColumn(cl);
                }
            }
            else
            {
                this.gpCost.Height = Unit.Empty;
            }
            if (dt != null && dt1 != null)
            {
                {
                    SReportIncome.DataSource = dt;
                    SReportIncome.DataBind();
                    Session.Remove("DeptCostAccountIncome");
                    Session["DeptCostAccountIncome"] = dt;
                }

                // if (dt1.Rows.Count > 0)
                {
                    SReportCost.DataSource = dt1;
                    SReportCost.DataBind();
                    Session.Remove("DeptCostAccountCost");
                    Session["DeptCostAccountCost"] = dt1;
                }

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
            this.gpIncome.Reconfigure();
            this.gpCost.Reconfigure();
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
            DataTable dt = report_dal.DeptCostAccountIncome(GetBeginDate(), GetEndDate(), balance, dept, hospro);
            if (Session["DeptCostAccountIncome"] != null)
            {
                string flags = "0";
                ExportData ex = new ExportData();
                //DataTable dt = (DataTable)Session["DeptCostAccountIncome"];
                if (dt.Rows.Count > 0)
                {
                    flags = "1";
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "RECK_ITEM")
                    {
                        dt.Columns[i].ColumnName = "核算科目代码";
                    }
                    else if (dt.Columns[i].ColumnName == "INCOM_TYPE_NAME")
                    {
                        dt.Columns[i].ColumnName = "科目类别";
                    }
                    else if (dt.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        dt.Columns[i].ColumnName = "核算科目名称";
                    }
                    else if (dt.Columns[i].ColumnName == "INCOMES_CHARGES")
                    {
                        dt.Columns[i].ColumnName = "实际收入";
                    }
                    else if (dt.Columns[i].ColumnName == "CHARGES")
                    {
                        dt.Columns[i].ColumnName = "减免收入";
                    }
                    else if (dt.Columns[i].ColumnName == "INCOMES")
                    {
                        dt.Columns[i].ColumnName = "收入合计";
                    }
                    else if (dt.Columns[i].ColumnName == "RATE")
                    {
                        dt.Columns[i].ColumnName = "占收入比";
                    }
                    //else if (dt.Columns[i].ColumnName == "ARMCOSTS")
                    //{
                    //    dt.Columns[i].ColumnName = "折算后军免收入";
                    //}
                }
                DataRow dr0 = dt.NewRow();
                dr0["科目类别"] = "合计";
                dr0["实际收入"] = Session["INCOMES_CHARGES"] == null ? "0" : Session["INCOMES_CHARGES"].ToString();
                dr0["减免收入"] = Session["CHARGES"] == null ? "0" : Session["CHARGES"].ToString();
                dr0["收入合计"] = Session["INCOMES"] == null ? "0" : Session["INCOMES"].ToString();
                dt.Rows.Add(dr0);
                DataRow dr = dt.NewRow();
                DataTable dtcost = (DataTable)Session["DeptCostAccountCost"];
                for (int i = 0; i < dtcost.Columns.Count; i++)
                {
                    if (dtcost.Columns[i].ColumnName == "ITEM_CODE")
                    {
                        dtcost.Columns[i].ColumnName = "成本科目代码";
                        dr[i] = "成本科目代码";

                    }
                    else if (dtcost.Columns[i].ColumnName == "COST_TYPE_NAME")
                    {
                        dtcost.Columns[i].ColumnName = "成本类别";
                        dr[i] = "成本类别";
                    }
                    else if (dtcost.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        dtcost.Columns[i].ColumnName = "成本科目名称";
                        dr[i] = "成本科目名称";
                    }
                    else if (dtcost.Columns[i].ColumnName == "COSTS")
                    {
                        dtcost.Columns[i].ColumnName = "对外成本";
                        dr[i] = "对外成本";
                    }
                    else if (dtcost.Columns[i].ColumnName == "COSTS_ARMYFREE")
                    {
                        dtcost.Columns[i].ColumnName = "减免成本";
                        dr[i] = "减免成本";
                    }
                    else if (dtcost.Columns[i].ColumnName == "TOTALCOSTS")
                    {
                        dtcost.Columns[i].ColumnName = "成本合计";
                        dr[i] = "成本合计";
                    }
                    else if (dtcost.Columns[i].ColumnName == "RATE")
                    {
                        dtcost.Columns[i].ColumnName = "占成本比";
                        dr[i] = "占成本比";
                    }
                }
                dt.Rows.Add(dr);
                for (int i = 0; i < dtcost.Rows.Count; i++)
                {
                    DataRow costdr = dt.NewRow();
                    for (int j = 0; j < dtcost.Columns.Count; j++)
                    {
                        costdr[j] = dtcost.Rows[i][j];
                    }
                    dt.Rows.Add(costdr);
                }

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

                if (flags == "1")
                {
                    //ex.ExportToLocal(dt, this.Page, "xls", "单位成本核算信息");

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
                else
                {
                    //ex.ExportToLocal(dtcost, this.Page, "xls", "单位成本核算信息");

                    // 创建工作簿
                    IWorkbook workbook = new HSSFWorkbook();
                    ISheet sheet = workbook.CreateSheet("单位成本核算信息");

                    // 创建表头
                    IRow headerRow = sheet.CreateRow(0);
                    for (int i = 0; i < dtcost.Columns.Count; i++)
                    {
                        ICell cell = headerRow.CreateCell(i);
                        cell.SetCellValue(dtcost.Columns[i].ColumnName);
                    }

                    // 创建样式
                    ICellStyle numberStyle = workbook.CreateCellStyle();
                    numberStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");// 数值格式

                    ICellStyle textStyle = workbook.CreateCellStyle();
                    textStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");

                    ICellStyle currencyStyle = workbook.CreateCellStyle();
                    currencyStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00"); // 金钱格式

                    // 填充数据
                    for (int i = 0; i < dtcost.Rows.Count; i++)
                    {
                        IRow row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < dtcost.Columns.Count; j++)
                        {
                            ICell cell = row.CreateCell(j);
                            string cellValue = dtcost.Rows[i][j].ToString();
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

                                cell.CellStyle = currencyStyle; // 金钱格式
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
                    for (int i = 0; i < dtcost.Columns.Count; i++)
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

        //双击设置
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {

            RowSelectionModel sm = this.gpCost.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string itemcode = sm.SelectedRow.RecordID;
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
}
