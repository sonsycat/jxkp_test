using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP
{
    /// <summary>
    /// 核算科目医疗收入
    /// </summary>
    public partial class Account_Subject_Income : PageBase
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

                string deptcode = this.DeptFilter("");
                if (deptcode != "")
                {
                    GoldNet.Model.User user = (GoldNet.Model.User)Session["CURRENTSTAFF"];
                    cbbdept.SelectedItem.Value = user.AccountDeptCode;
                    cbbdept.SelectedItem.Text = user.AccountDeptName;
                    cbbdept.Disabled = true;
                }

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    this.GridPanel_Show.ColumnModel.Columns[5].Hidden = true;
                    this.GridPanel_Show.ColumnModel.Columns[6].Hidden = true;
                    this.GridPanel_Show.ColumnModel.Columns[7].Hidden = true;
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
                    dept = " and a.dept_code in (select DEPT_CODE from comm.sys_dept_dict b where b.ACCOUNT_DEPT_CODE='" + cbbdept.SelectedItem.Value + "') ";
                }
            }
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = report_dal.GetAccountSubjectIncome(GetBeginDate(), GetEndDate(), balance, dept, hospro);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("Account_Subject_Income");
                Session["Account_Subject_Income"] = dt;
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
            if (Session["Account_Subject_Income"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Account_Subject_Income"];

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    dt.Columns.Remove("ARMACCOUNTINCOMES");
                    dt.Columns.Remove("CVALATIONINCOMES");
                    dt.Columns.Remove("HVALATIONINCOMES");
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "CLASS_NAME")
                    {
                        dt.Columns[i].ColumnName = "核算科目";
                    }
                    else if (dt.Columns[i].ColumnName == "ACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "总收入";
                    }
                    else if (dt.Columns[i].ColumnName == "FACACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入小计";
                    }
                    else if (dt.Columns[i].ColumnName == "CREALINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入门诊";
                    }
                    else if (dt.Columns[i].ColumnName == "HREALINCOMES")
                    {
                        dt.Columns[i].ColumnName = "实际收入住院";
                    }
                    else if (dt.Columns[i].ColumnName == "ARMACCOUNTINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入小计";
                    }
                    else if (dt.Columns[i].ColumnName == "CVALATIONINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入门诊";
                    }
                    else if (dt.Columns[i].ColumnName == "HVALATIONINCOMES")
                    {
                        dt.Columns[i].ColumnName = "计价收入住院";
                    }
                    else if (dt.Columns[i].ColumnName == "CLASS_CODE")
                    {
                        dt.Columns[i].ColumnName = "项目代码";
                    }
                    else if (dt.Columns[i].ColumnName == "ID")
                    {
                        dt.Columns[i].ColumnName = "顺序";
                    }
                }
                //string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月-" + this.ccbYearTo.SelectedItem.Value + "年" + this.ccbMonthTo.SelectedItem.Value + "月";
                //ex.ExportToLocal(dt, this.Page, "xls", "核算科目医疗收入统计报表(" + dates + ")");

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
                ISheet sheet = workbook.CreateSheet("核算科目医疗收入统计报表");

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
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "核算科目医疗收入统计报表.xls");
                    }
                    else
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "至" + e_date + "核算科目医疗收入统计报表.xls");
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
