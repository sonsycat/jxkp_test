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
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


namespace GoldNet.JXKP
{
    public partial class AccountDeptCost :PageBase
    {
        Report dal_report = new Report();
        /// <summary>
        /// 核算单位成本报表
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

                cbbType.Value = 0;
            }
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
                     
            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = dal_report.GetAccountDeptCost(GetBeginDate(),GetEndDate(),balance, deptcode, hospro);
            if (dt != null)
            {
                SReport.RemoveFields();
                GridPanel_Show.Reconfigure();
                GridPanel_Show.ColumnModel.Columns.Clear();
                GoldNet.JXKP.cbhs.Report.BuildControl bc = new GoldNet.JXKP.cbhs.Report.BuildControl();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    
                    if (dt.Columns[i].ColumnName != "DEPT_CODE" )
                    {
                        bc.AddRecord(dt.Columns[i].ColumnName, SReport);
                        ExtColumn Column = new ExtColumn();
                        Column.ColumnID = dt.Columns[i].ColumnName;
                        Column.Header = "<div style='text-align:center;'>" + dt.Columns[i].ColumnName + "</div>";
                        Column.Align = Alignment.Left;
                        if (dt.Columns[i].ColumnName != "科室" && dt.Columns[i].ColumnName != "核算类别")
                        {
                            Column.Renderer.Fn = "rmbMoney";
                            Column.Align = Alignment.Right;
                        }
                        Column.DataIndex = dt.Columns[i].ColumnName;
                        Column.MenuDisabled = true;
                        Column.Width = 120;
                        GridPanel_Show.ColumnModel.Columns.Add(Column);
                        GridPanel_Show.AddColumn(Column);
                    }
                }

                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("AccountDeptCost");
                Session["AccountDeptCost"] = dt;
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
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["AccountDeptCost"] != null)
            {
                //ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["AccountDeptCost"];               
                dt.Columns.Remove("DEPT_CODE");
                //ex.ExportToLocal(dt, this.Page, "xls", "核算单位成本报表");
                //this.outexcel(dt, "核算单位成本报表");

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
                ISheet sheet = workbook.CreateSheet("核算单位成本报表");

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
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "核算单位成本报表.xls");
                    }
                    else
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "至" + e_date + "核算单位成本报表.xls");
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
    }
}
